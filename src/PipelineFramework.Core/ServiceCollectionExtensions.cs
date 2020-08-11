using Microsoft.Extensions.DependencyInjection;
using PipelineFramework.Abstractions;
using PipelineFramework.PipelineComponentResolvers;
using System;
using System.Linq;
using System.Reflection;

namespace PipelineFramework
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an instance of <see cref="IAsyncPipelineComponentExecutionStatusReceiver"/> to the services collection.
        /// </summary>
        /// <typeparam name="T">Type of status receiver to add.</typeparam>
        /// <param name="services">Services collection dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAsyncPipelineComponentExecutionStatusReceiver<T>(this IServiceCollection services)
            where T : class, IAsyncPipelineComponentExecutionStatusReceiver =>
                services.AddSingleton<IAsyncPipelineComponentExecutionStatusReceiver, T>();

        /// <summary>
        /// Adds an instance of <see cref="IAsyncPipelineComponentExecutionStatusReceiver"/> to the services collection.
        /// </summary>
        /// <typeparam name="T">Type of status receiver to add.</typeparam>
        /// <param name="services">Services collection dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPipelineComponentExecutionStatusReceiver<T>(this IServiceCollection services)
            where T : class, IPipelineComponentExecutionStatusReceiver =>
            services.AddSingleton<IPipelineComponentExecutionStatusReceiver, T>();


        /// <summary>
        /// Adds an instance of <see cref="IPipelineComponentResolver"/> to services collection.
        /// </summary>
        /// <param name="services">Services collection dependency injection container.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPipelineComponentResolver(this IServiceCollection services)
            => services.AddSingleton<IPipelineComponentResolver>(provider => new ServiceProviderPipelineComponentResolver(provider));

        /// <summary>
        /// Adds the specified <see cref="TComponent"/> as an <see cref="IAsyncPipelineComponent{TPayload}"/> to the services collection.
        /// /// </summary>
        /// <typeparam name="TComponent">The component type to be added.</typeparam>
        /// <typeparam name="TPayload">The payload type the pipeline should use.</typeparam>
        /// <param name="services">Services collection used to add <see cref="IAsyncPipelineComponent{TPayload}"/> component instance.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAsyncPipelineComponent<TComponent, TPayload>(this IServiceCollection services)
            where TComponent : class, IAsyncPipelineComponent<TPayload>
            => services.AddSingleton<IAsyncPipelineComponent<TPayload>, TComponent>();

        /// <summary>
        /// Adds the specified <see cref="TComponent"/> as an <see cref="IPipelineComponent{TPayload}"/> to the services collection.
        /// /// </summary>
        /// <typeparam name="TComponent">The component type to be added.</typeparam>
        /// <typeparam name="TPayload">The payload type the pipeline should use.</typeparam>
        /// <param name="services">Services collection used to add <see cref="IPipelineComponent{TPayload}"/> component instance.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPipelineComponent<TComponent, TPayload>(this IServiceCollection services)
            where TComponent : class, IPipelineComponent<TPayload>
            => services.AddSingleton<IPipelineComponent<TPayload>, TComponent>();

        /// <summary>
        /// Scans the specified <see cref="Assembly"/> for any types that implement <see cref="IAsyncPipelineComponent{T}"/> and automatically adds those matching types to the services collection.
        /// </summary>
        /// <param name="services">Services collection for adding any types located implementing <see cref="IAsyncPipelineComponent{T}"/></param>
        /// <param name="assembly">The specified <see cref="Assembly"/> to scan.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAsyncPipelineComponentsFromAssembly(this IServiceCollection services, Assembly assembly)
            => AddComponentsFromAssembly(services, assembly, true);

        /// <summary>
        /// Scans the specified <see cref="Assembly"/> for any types that implement <see cref="IPipelineComponent{T}"/> and automatically adds those matching types to the services collection.
        /// </summary>
        /// <param name="services">Services collection for adding any types located implementing <see cref="IPipelineComponent{T}"/></param>
        /// <param name="assembly">The specified <see cref="Assembly"/> to scan.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPipelineComponentsFromAssembly(this IServiceCollection services, Assembly assembly)
            => AddComponentsFromAssembly(services, assembly);

        private static IServiceCollection AddComponentsFromAssembly(this IServiceCollection services, Assembly assembly, bool useAsyncComponents = false)
        {
            Func<Type, bool> isComponent;
            if (useAsyncComponents) isComponent = IsAsyncPipelineComponent;
            else isComponent = IsPipelineComponent;

            var components = from t in assembly.GetTypes()
                let interfaces = t.GetInterfaces()
                where !t.IsAbstract &&
                      !t.IsInterface &&
                      interfaces.Any(isComponent)
                select new
                {
                    ImplementingType = t,
                    PipelineComponentInterfaces = interfaces.Where(isComponent)
                };

            foreach (var component in components)
            {
                foreach (var interfaceType in component.PipelineComponentInterfaces)
                {
                    services.AddSingleton(interfaceType, component.ImplementingType);
                }
            }

            return services;

            //Local functions
            static bool IsAsyncPipelineComponent(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncPipelineComponent<>);
            static bool IsPipelineComponent(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineComponent<>);
        }
    }
}
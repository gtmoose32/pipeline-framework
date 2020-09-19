using Microsoft.Extensions.DependencyInjection;
using PipelineFramework.Abstractions;
using PipelineFramework.Abstractions.Builder;
using PipelineFramework.Builder;
using PipelineFramework.Extensions.Microsoft.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace PipelineFramework
{
    /// <summary>
    /// A set of extension methods for using Microsoft <see cref="IServiceCollection"/> to assist in registering required PipelineFramework objects.
    /// </summary>
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
        /// Adds the specified pipeline component as an <see cref="IAsyncPipelineComponent{TPayload}"/> to the services collection.
        /// /// </summary>
        /// <typeparam name="TComponent">The component type to be added.</typeparam>
        /// <typeparam name="TPayload">The payload type the pipeline should use.</typeparam>
        /// <param name="services">Services collection used to add <see cref="IAsyncPipelineComponent{TPayload}"/> component instance.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAsyncPipelineComponent<TComponent, TPayload>(this IServiceCollection services)
            where TComponent : class, IAsyncPipelineComponent<TPayload>
            => services.AddSingleton<IAsyncPipelineComponent<TPayload>, TComponent>();

        /// <summary>
        /// Adds the specified pipeline component as an <see cref="IPipelineComponent{TPayload}"/> to the services collection.
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

        /// <summary>
        /// Add Pipeline Framework support to project
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPipelineFramework(this IServiceCollection services)
        {
            services.AddPipelineComponentResolver();
            return services;
        }

        /// <summary>
        /// Adds an <see cref="IAsyncPipeline{T}"/> with a specified <see cref="IAsyncPipelineComponentExecutionStatusReceiver"/>
        /// </summary>
        /// <typeparam name="TPayload">The payload type that the pipeline supports</typeparam>
        /// <typeparam name="TStatusReceiver">The type of <see cref="IAsyncPipelineComponentExecutionStatusReceiver"/></typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="configAction"></param>
        /// <param name="settings"></param>
        /// <param name="lifetime">The desired lifetime of the pipeline and associated component</param>
        /// <returns></returns>
        public static IServiceCollection AddAsyncPipeline<TPayload, TStatusReceiver>(this IServiceCollection services,
            Action<AsyncPipelineComponentConfiguration<TPayload>> configAction,
            IDictionary<string, IDictionary<string, string>> settings = null,
            ServiceLifetime lifetime = ServiceLifetime.Singleton) where TStatusReceiver : class, IAsyncPipelineComponentExecutionStatusReceiver
        {
            return services.AddAsyncPipelineInternal(configAction, settings, typeof(TStatusReceiver), lifetime);
        }

        /// <summary>
        /// Adds an <see cref="IAsyncPipeline{T}"/>
        /// </summary>
        /// <typeparam name="TPayload">The payload type that the pipeline supports</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="configAction">The component configuration action</param>
        /// <param name="settings">Optional pipeline settings to apply</param>
        /// <param name="lifetime">The desired lifetime of the pipeline and associated component</param>
        /// <returns></returns>
        public static IServiceCollection AddAsyncPipeline<TPayload>(this IServiceCollection services,
            Action<AsyncPipelineComponentConfiguration<TPayload>> configAction,
            IDictionary<string, IDictionary<string, string>> settings = null,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            return services.AddAsyncPipelineInternal(configAction, settings, null, lifetime);
        }

        /// <summary>
        /// Adds an <see cref="IPipeline{T}"/> with a specified <see cref="IPipelineComponentExecutionStatusReceiver"/>
        /// </summary>
        /// <typeparam name="TPayload">The payload type that the pipeline supports</typeparam>
        /// <typeparam name="TStatusReceiver">The type of <see cref="IPipelineComponentExecutionStatusReceiver"/></typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="configAction">The component configuration action</param>
        /// <param name="settings">Optional pipeline settings to apply</param>
        /// <param name="lifetime">The desired lifetime of the pipeline and associated component</param>
        /// <returns></returns>
        public static IServiceCollection AddPipeline<TPayload, TStatusReceiver>(this IServiceCollection services,
            Action<PipelineComponentConfiguration<TPayload>> configAction,
            IDictionary<string, IDictionary<string, string>> settings = null,
            ServiceLifetime lifetime = ServiceLifetime.Singleton) where TStatusReceiver : class, IPipelineComponentExecutionStatusReceiver
        {
            return services.AddPipelineInternal(configAction, settings, typeof(TStatusReceiver), lifetime);
        }

        /// <summary>
        /// Adds an <see cref="IPipeline{T}"/>
        /// </summary>
        /// <typeparam name="TPayload">The payload type that the pipeline supports</typeparam>
        /// <param name="services">The services collection</param>
        /// <param name="configAction">The component configuration action</param>
        /// <param name="settings">Optional pipeline settings to apply</param>
        /// <param name="lifetime">The desired lifetime of the pipeline and associated component</param>
        /// <returns></returns>
        public static IServiceCollection AddPipeline<TPayload>(this IServiceCollection services,
            Action<PipelineComponentConfiguration<TPayload>> configAction,
            IDictionary<string, IDictionary<string, string>> settings = null,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            return services.AddPipelineInternal(configAction, settings, null, lifetime);
        }

        private static IServiceCollection AddAsyncPipelineInternal<TPayload>(this IServiceCollection services, 
            Action<AsyncPipelineComponentConfiguration<TPayload>> configAction,
            IDictionary<string, IDictionary<string, string>> settings,
            Type statusReceiverType,
            ServiceLifetime lifetime)
        {
            var config = new AsyncPipelineComponentConfiguration<TPayload>();
            configAction(config);
            config.Validate();

            if (statusReceiverType != null)
            {
                services.Add(new ServiceDescriptor(typeof(IAsyncPipelineComponentExecutionStatusReceiver), statusReceiverType, lifetime));
            }

            var serviceType = typeof(IAsyncPipelineComponent<TPayload>);
            foreach (var component in config.Components)
            {
                services.Add(config.CustomRegistrations.ContainsKey(component)
                    ? new ServiceDescriptor(serviceType, config.CustomRegistrations[component], lifetime)
                    : new ServiceDescriptor(serviceType, component, lifetime));
            }

            services.Add(new ServiceDescriptor(
                typeof(IAsyncPipeline<TPayload>), 
                provider => BuildAsyncPipeline(provider, settings), 
                lifetime));

            return services;

            IAsyncPipeline<TPayload> BuildAsyncPipeline(IServiceProvider provider,
                IDictionary<string, IDictionary<string, string>> pipelineSettings = null)
            {
                var initialBuilder =
                    (IAdditionalPipelineComponentHolder<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload>)
                    PipelineBuilder<TPayload>.InitializeAsyncPipeline(
                        provider.GetService<IAsyncPipelineComponentExecutionStatusReceiver>());

                initialBuilder =
                    config.Components.Aggregate(initialBuilder, (current, component) => current.WithComponent(component));

                var settingsHolder = initialBuilder.WithComponentResolver(provider.GetService<IPipelineComponentResolver>());
                var builder = pipelineSettings != null
                    ? settingsHolder.WithSettings(pipelineSettings)
                    : settingsHolder.WithoutSettings();
                return builder.Build();
            }
        }

        private static IServiceCollection AddPipelineInternal<TPayload>(this IServiceCollection services,
            Action<PipelineComponentConfiguration<TPayload>> configAction,
            IDictionary<string, IDictionary<string, string>> settings,
            Type statusReceiverType,
            ServiceLifetime lifetime)
        {
            var config = new PipelineComponentConfiguration<TPayload>();
            configAction(config);
            config.Validate();

            if (statusReceiverType != null)
            {
                services.Add(new ServiceDescriptor(typeof(IPipelineComponentExecutionStatusReceiver), statusReceiverType, lifetime));
            }

            foreach (var component in config.Components)
            {
                services.Add(new ServiceDescriptor(typeof(IPipelineComponent<TPayload>), component, lifetime));
            }

            services.Add(new ServiceDescriptor(
                typeof(IPipeline<TPayload>), 
                provider => BuildPipeline(provider, settings), 
                lifetime));

            return services;

            IPipeline<TPayload> BuildPipeline(IServiceProvider provider, 
                IDictionary<string, IDictionary<string, string>> pipelineSettings = null)
            {
                var initialBuilder =
                    (IAdditionalPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>,
                        TPayload>)
                    PipelineBuilder<TPayload>.InitializePipeline(
                        provider.GetService<IPipelineComponentExecutionStatusReceiver>());

                initialBuilder = config.Components.Aggregate(initialBuilder,
                    (current, component) => current.WithComponent(component));

                var settingsHolder =
                    initialBuilder.WithComponentResolver(provider.GetService<IPipelineComponentResolver>());
                var builder = pipelineSettings != null
                    ? settingsHolder.WithSettings(pipelineSettings)
                    : settingsHolder.WithoutSettings();
                return builder.Build();
            }
        }
    }
}
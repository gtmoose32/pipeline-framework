using Microsoft.Extensions.DependencyInjection;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// A pipeline component resolver that uses Microsoft Dependency Injection container <see cref="IServiceProvider"/> internally to store and resolve any pipeline component instance requests.
    /// </summary>
    public class ServiceProviderPipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serviceProvider">Microsoft Dependency Injection container</param>
        public ServiceProviderPipelineComponentResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public T GetInstance<T>(string name) where T : class, IPipelineComponent
        {
            var components = _serviceProvider.GetService<IEnumerable<T>>();
            
            var component = components.FirstOrDefault(c => c.Name == name);
            if (component == null) 
                throw new PipelineComponentNotFoundException($"PipelineComponent named '{name}' could not be located.");

            return component;
        }
    }
}
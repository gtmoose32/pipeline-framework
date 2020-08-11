using Microsoft.Extensions.DependencyInjection;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PipelineFramework.PipelineComponentResolvers
{
    public class ServiceProviderPipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderPipelineComponentResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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
using LightInject;
using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.LightInject
{
    public class PipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IServiceFactory _serviceFactory;

        public PipelineComponentResolver(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
        }

        public T GetInstance<T>(string name) where T : IPipelineComponent
        {
            return _serviceFactory.GetInstance<T>(name);
        }
    }
}

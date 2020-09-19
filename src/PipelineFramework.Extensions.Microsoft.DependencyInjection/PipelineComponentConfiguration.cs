using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    public class PipelineComponentConfiguration<TPayload> : PipelineComponentConfigurationBase
    {
        internal PipelineComponentConfiguration() { }

        public PipelineComponentConfiguration<TPayload> WithComponent<TComponent>() where TComponent : class, IPipelineComponent<TPayload>
        {
            AddComponent<TComponent>();
            return this;
        }

        public PipelineComponentConfiguration<TPayload> WithComponent<TComponent>(Func<IServiceProvider, TComponent> createInstance) 
            where TComponent : class, IAsyncPipelineComponent<TPayload>
        {
            AddComponent(createInstance);
            return this;
        }
    }
}
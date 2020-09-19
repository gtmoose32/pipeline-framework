using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    public class AsyncPipelineComponentConfiguration<TPayload> : PipelineComponentConfigurationBase
    {
        internal AsyncPipelineComponentConfiguration() { }

        public AsyncPipelineComponentConfiguration<TPayload> WithComponent<TComponent>() where TComponent : class, IAsyncPipelineComponent<TPayload>
        {
            AddComponent<TComponent>();
            return this;
        }

        public AsyncPipelineComponentConfiguration<TPayload> WithComponent<TComponent>(Func<IServiceProvider, TComponent> createInstance) where TComponent : class, IAsyncPipelineComponent<TPayload>
        {
            AddComponent(createInstance);
            return this;
        }
    }
}
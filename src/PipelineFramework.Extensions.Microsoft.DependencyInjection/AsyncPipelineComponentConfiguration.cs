using PipelineFramework.Abstractions;

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
    }
}
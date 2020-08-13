using PipelineFramework.Abstractions;

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
    }
}
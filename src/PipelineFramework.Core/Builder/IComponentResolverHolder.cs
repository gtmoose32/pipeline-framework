using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder
{
    public interface IComponentResolverHolder<out TPipeline, TPayload> where TPipeline : IPipeline
    {
        ISettingsHolder<TPipeline, TPayload> WithComponentResolver(IPipelineComponentResolver componentResolver);
    }
}
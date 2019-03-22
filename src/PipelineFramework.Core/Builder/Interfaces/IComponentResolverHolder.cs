using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IComponentResolverHolder<out TPipeline, TPayload> where TPipeline : IPipeline
    {
        ISettingsHolder<TPipeline, TPayload> WithComponentResolver(IPipelineComponentResolver componentResolver);
    }
}
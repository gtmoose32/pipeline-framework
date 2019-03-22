using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IComponentResolverHolder<out TPipeline> where TPipeline : IPipeline
    {
        ISettingsHolder<TPipeline> WithComponentResolver(IPipelineComponentResolver componentResolver);
    }
}
namespace PipelineFramework.Abstractions.Builder
{
    public interface IComponentResolverHolder<out TPipeline> where TPipeline : IPipeline
    {
        ISettingsHolder<TPipeline> WithComponentResolver(IPipelineComponentResolver componentResolver);
    }
}
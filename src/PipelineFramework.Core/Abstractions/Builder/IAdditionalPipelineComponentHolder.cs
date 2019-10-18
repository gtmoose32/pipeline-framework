namespace PipelineFramework.Abstractions.Builder
{
    public interface IAdditionalPipelineComponentHolder<out TPipeline, TComponentBase, TPayload> : 
        IInitialPipelineComponentHolder<TPipeline, TComponentBase, TPayload>,
        IComponentResolverHolder<TPipeline>
        where TPipeline : IPipeline
        where TComponentBase : IPipelineComponent
    {

    }
}
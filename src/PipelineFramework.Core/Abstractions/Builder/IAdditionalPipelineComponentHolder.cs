namespace PipelineFramework.Abstractions.Builder
{
    /// <summary>
    /// Defines an additional pipeline component holder for a pipeline builder.
    /// </summary>
    /// <typeparam name="TPipeline">Type of pipeline to build.</typeparam>
    /// <typeparam name="TComponentBase">Base type of pipeline components used to build the pipeline.</typeparam>
    /// <typeparam name="TPayload">Type of the payload the pipeline will use during execution.</typeparam>
    public interface IAdditionalPipelineComponentHolder<out TPipeline, TComponentBase, TPayload> : 
        IInitialPipelineComponentHolder<TPipeline, TComponentBase, TPayload>,
        IComponentResolverHolder<TPipeline>
        where TPipeline : IPipeline
        where TComponentBase : IPipelineComponent
    {
    }
}
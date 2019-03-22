using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IAdditionalPipelineComponentHolder<out TPipeline, TComponentBase, TPayload> : 
        IInitialPipelineComponentHolder<TPipeline, TComponentBase, TPayload>,
        IComponentResolverHolder<TPipeline>
        where TPipeline : IPipeline
        where TComponentBase : IPipelineComponent
    {

    }
}
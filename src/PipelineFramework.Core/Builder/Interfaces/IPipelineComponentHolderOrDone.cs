using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IPipelineComponentHolderOrDone<out TPipeline, TComponentBase, TPayload> : 
        IPipelineComponentHolder<TPipeline, TComponentBase, TPayload>,
        IComponentResolverHolder<TPipeline, TPayload>
        where TPipeline : IPipeline
        where TComponentBase : IPipelineComponent
    {

    }
}
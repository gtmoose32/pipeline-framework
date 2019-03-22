using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IPipelineComponentNameHolderOrDone<out TPipeline, TPayload> :
        IPipelineComponentNameHolder<TPipeline, TPayload>,
        IComponentResolverHolder<TPipeline, TPayload>
        where TPipeline : IPipeline
    {
    }
}
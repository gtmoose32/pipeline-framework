using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder
{
    public interface IPipelineComponentNameHolderOrDone<out TPipeline, TPayload> :
        IPipelineComponentNameHolder<TPipeline, TPayload>,
        IComponentResolverHolder<TPipeline, TPayload>
        where TPipeline : IPipeline
    {
    }
}
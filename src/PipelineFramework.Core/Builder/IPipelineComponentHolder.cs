using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder
{
    public interface IPipelineComponentHolder<out TPipeline, TComponentBase, TPayload> where TComponentBase : IPipelineComponent where TPipeline : IPipeline
    {
        IPipelineComponentHolderOrDone<TPipeline, TComponentBase, TPayload> WithComponent<TComponent>() where TComponent : IAsyncPipelineComponent<TPayload>;
    }
}
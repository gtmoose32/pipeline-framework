using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IPipelineComponentHolder<out TPipeline, TComponentBase, TPayload> where TComponentBase : IPipelineComponent where TPipeline : IPipeline
    {
        IPipelineComponentHolderOrDone<TPipeline, TComponentBase, TPayload> WithComponent<TComponent>() where TComponent : TComponentBase;
    }
}
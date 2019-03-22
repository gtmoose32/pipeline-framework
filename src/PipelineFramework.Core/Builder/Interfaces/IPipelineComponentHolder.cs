using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IPipelineComponentHolder<out TPipeline, TComponentBase, TPayload>
        where TPipeline : IPipeline
        where TComponentBase : IPipelineComponent 
    {
        IPipelineComponentHolderOrDone<TPipeline, TComponentBase, TPayload> WithComponent<TComponent>() where TComponent : TComponentBase;
        IPipelineComponentHolderOrDone<TPipeline, TComponentBase, TPayload> WithComponent(string componentName);
    }
}
namespace PipelineFramework.Abstractions.Builder
{
    public interface IInitialPipelineComponentHolder<out TPipeline, TComponentBase, TPayload>
        where TPipeline : IPipeline
        where TComponentBase : IPipelineComponent 
    {
        IAdditionalPipelineComponentHolder<TPipeline, TComponentBase, TPayload> WithComponent<TComponent>() where TComponent : TComponentBase;
        IAdditionalPipelineComponentHolder<TPipeline, TComponentBase, TPayload> WithComponent(string componentName);
    }
}
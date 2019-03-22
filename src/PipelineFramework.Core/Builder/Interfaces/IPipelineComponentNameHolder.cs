using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IPipelineComponentNameHolder<out TPipeline, TPayload> where TPipeline : IPipeline
    {
        IPipelineComponentNameHolderOrDone<TPipeline, TPayload> WithComponentName(string name);
    }
}
using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder
{
    public interface IPipelineComponentNameHolder<out TPipeline, TPayload> where TPipeline : IPipeline
    {
        IPipelineComponentNameHolderOrDone<TPipeline, TPayload> WithComponentName(string name);
    }
}
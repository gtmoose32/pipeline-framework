using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder
{
    public interface IPipelineBuilder<out TPipeline, TPayload> where TPipeline : IPipeline
    {
        TPipeline Build();
    }
}
using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IPipelineBuilder<out TPipeline, TPayload> where TPipeline : IPipeline
    {
        TPipeline Build();
    }
}
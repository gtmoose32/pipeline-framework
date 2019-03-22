using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface IPipelineBuilder<out TPipeline> where TPipeline : IPipeline
    {
        TPipeline Build();
    }
}
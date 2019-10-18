namespace PipelineFramework.Abstractions.Builder
{
    public interface IPipelineBuilder<out TPipeline> where TPipeline : IPipeline
    {
        TPipeline Build();
    }
}
namespace PipelineFramework.Abstractions.Builder
{
    /// <summary>
    /// Defines a pipeline builder.
    /// </summary>
    /// <typeparam name="TPipeline">Type of pipeline to build.</typeparam>
    public interface IPipelineBuilder<out TPipeline> where TPipeline : IPipeline
    {
        /// <summary>
        /// Creates a new pipeline.
        /// </summary>
        /// <returns>Pipeline</returns>
        TPipeline Build(string pipelineName = null);
    }
}
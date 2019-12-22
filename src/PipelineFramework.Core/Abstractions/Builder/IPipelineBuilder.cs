namespace PipelineFramework.Abstractions.Builder
{
    /// <summary>
    /// Defines a pipeline builder.
    /// </summary>
    /// <typeparam name="TPipeline">Type of pipeline to build.</typeparam>
    public interface IPipelineBuilder<out TPipeline> where TPipeline : IPipeline
    {
        /// <summary>
        /// Builds a instance of <see cref="TPipeline"/>.
        /// </summary>
        /// <returns>A new pipeline.</returns>
        TPipeline Build();
    }
}
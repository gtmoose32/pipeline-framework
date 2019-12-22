namespace PipelineFramework.Abstractions.Builder
{
    /// <summary>
    /// Defines a pipeline component resolver holder for a pipeline builder.  
    /// </summary>
    /// <typeparam name="TPipeline">Type of pipeline to build.</typeparam>
    public interface IComponentResolverHolder<out TPipeline> where TPipeline : IPipeline
    {
        /// <summary>
        /// Sets the pipeline component resolver that should be used to resolve all pipeline components defined for the pipeline.
        /// </summary>
        /// <param name="componentResolver">The <see cref="IPipelineComponentResolver"/> to be used to resolve <see cref="IPipelineComponent"/> requests.</param>
        /// <returns>A pipeline settings holder.</returns>
        ISettingsHolder<TPipeline> WithComponentResolver(IPipelineComponentResolver componentResolver);
    }
}
namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Defines a pipeline component resolver.
    /// </summary>
    public interface IPipelineComponentResolver
    {
        /// <summary>
        /// Attempts to use the name specified to resolve an instance of a pipeline component.
        /// </summary>
        /// <param name="name">Name of the pipeline component instance that is being requested.</param>
        /// <typeparam name="T">Pipeline component type being requested.</typeparam>
        /// <returns>An instance of <see cref="T"/> which must implement <see cref="IPipelineComponent"/>.</returns>
        T GetInstance<T>(string name) where T : class, IPipelineComponent;
    }
}

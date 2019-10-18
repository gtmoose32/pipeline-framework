namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPipelineComponentResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetInstance<T>(string name) where T : class, IPipelineComponent;
    }
}

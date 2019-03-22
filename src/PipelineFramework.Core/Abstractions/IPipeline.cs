using System.Threading;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Defines a pipeline that executes a linear work flow. 
    /// </summary>
    /// <typeparam name="T">Type of payload passed through the pipeline during execution.</typeparam>
    public interface IPipeline<T> : IPipeline
    {
        /// <summary>
        /// Executes linear work flow.
        /// </summary>
        /// <param name="payload">Type of payload passed through the pipeline during execution.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel pipeline execution prematurely.</param>
        /// <returns>Type of payload returned after execution.</returns>
        T Execute(T payload, CancellationToken cancellationToken = default);
    }

    public interface IPipeline { }
}

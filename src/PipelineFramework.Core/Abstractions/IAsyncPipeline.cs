using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Defines an pipeline that executes a linear work flow asynchronously. 
    /// </summary>
    /// <typeparam name="T">Type of payload passed through the pipeline during execution.</typeparam>
    public interface IAsyncPipeline<T> : IPipeline
    {
        /// <summary>
        /// Executes linear work flow asynchronously.
        /// </summary>
        /// <param name="payload">Type of payload passed through the pipeline during execution.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> used to cancel pipeline execution prematurely.</param>
        /// <returns><see cref="Task{T}"/></returns>
        Task<T> ExecuteAsync(T payload, CancellationToken cancellationToken = default);
    }
}

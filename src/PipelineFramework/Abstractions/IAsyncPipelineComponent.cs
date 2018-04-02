using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Defines the behavior of an asynchronous pipeline component.
    /// </summary>
    /// <typeparam name="T">The object that participates in the execution of the pipeline component.</typeparam>
    public interface IAsyncPipelineComponent<T> : IPipelineComponent
    {
        /// <summary>
        /// Executes this pipeline component asynchronously.
        /// </summary>
        /// <param name="payload">Payload object this instance uses for execution.</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Task{T}"/></returns>
        Task<T> ExecuteAsync(T payload, CancellationToken cancellationToken);
    }
}

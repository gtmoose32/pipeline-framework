using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Abstract base class for asynchronous pipeline components.
    /// </summary>
    /// <typeparam name="T">Type of payload to be used by component.</typeparam>
    public abstract class AsyncPipelineComponentBase<T> : PipelineComponentBase, IAsyncPipelineComponent<T>
    {
        /// <inheritdoc />
        public abstract Task<T> ExecuteAsync(T payload, CancellationToken cancellationToken);
    }
}

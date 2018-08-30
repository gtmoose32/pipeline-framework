using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Abstractions
{
    public interface IAsyncPipelineFactoryExecutor
    {
        Task<TPayload> CreateAsyncPipelineAndExecuteAsync<TPayload>(TPayload payload, CancellationToken cancellationToken = default);
    }
}
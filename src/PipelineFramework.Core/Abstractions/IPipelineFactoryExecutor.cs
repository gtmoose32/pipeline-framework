using System.Threading;

namespace PipelineFramework.Abstractions
{
    public interface IPipelineFactoryExecutor
    {
        TPayload CreatePipelineAndExecute<TPayload>(TPayload payload, CancellationToken cancellationToken = default);
    }
}

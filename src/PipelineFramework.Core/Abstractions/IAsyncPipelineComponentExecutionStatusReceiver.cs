using System.Threading.Tasks;

namespace PipelineFramework.Abstractions
{
    public interface IAsyncPipelineComponentExecutionStatusReceiver
    {
        Task ReceiveExecutionStartingAsync(PipelineComponentExecutionStartedInfo executionInfo);

        Task ReceiveExecutionCompletedAsync(PipelineComponentExecutionCompletedInfo executionInfo);
    }
}
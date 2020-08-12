using System.Threading.Tasks;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Specifies the contract for an async pipeline component execution status receiver.
    /// </summary>
    public interface IAsyncPipelineComponentExecutionStatusReceiver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionInfo"></param>
        /// <returns></returns>
        Task ReceiveExecutionStartingAsync(PipelineComponentExecutionStartingInfo executionInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionInfo"></param>
        /// <returns></returns>
        Task ReceiveExecutionCompletedAsync(PipelineComponentExecutionCompletedInfo executionInfo);
    }
}
using System.Threading.Tasks;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Specifies the contract for an pipeline component execution status receiver.
    /// </summary>
    public interface IPipelineComponentExecutionStatusReceiver
    {
        /// <summary>
        /// Receives pipeline component execution starting notification.
        /// </summary>
        /// <param name="executionInfo">Object containing info about the executing component.</param>
        /// <returns><see cref="Task"/></returns>
        void ReceiveExecutionStarting(PipelineComponentExecutionStartingInfo executionInfo);

        /// <summary>
        /// Receives pipeline component execution completed notification.
        /// </summary>
        /// <param name="executionInfo">Object containing info about the executing component.</param>
        /// <returns><see cref="Task"/></returns>
        void ReceiveExecutionCompleted(PipelineComponentExecutionCompletedInfo executionInfo);
    }
}
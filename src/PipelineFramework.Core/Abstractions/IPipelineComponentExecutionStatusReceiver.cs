namespace PipelineFramework.Abstractions
{
    public interface IPipelineComponentExecutionStatusReceiver
    {
        void ReceiveExecutionStarting(PipelineComponentExecutionStartingInfo executionInfo);

        void ReceiveExecutionCompleted(PipelineComponentExecutionCompletedInfo executionInfo);
    }
}
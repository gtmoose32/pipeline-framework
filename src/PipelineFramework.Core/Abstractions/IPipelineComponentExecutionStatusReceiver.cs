namespace PipelineFramework.Abstractions
{
    public interface IPipelineComponentExecutionStatusReceiver
    {
        void ReceiveExecutionStarting(PipelineComponentExecutionStartedInfo executionInfo);

        void ReceiveExecutionCompleted(PipelineComponentExecutionCompletedInfo executionInfo);
    }
}
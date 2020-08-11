using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework
{
    [ExcludeFromCodeCoverage]
    public class PipelineComponentExecutionCompletedInfo : PipelineComponentExecutionStartedInfo
    {
        public PipelineComponentExecutionCompletedInfo(string pipelineComponentName, TimeSpan executionTime, Exception exception = null) 
            : base(pipelineComponentName)
        {
            ExecutionTime = executionTime;
            Exception = exception;
        }

        public TimeSpan ExecutionTime { get; }
        public Exception Exception { get; }
    }
}
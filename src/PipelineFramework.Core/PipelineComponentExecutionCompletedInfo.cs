using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework
{
    /// <summary>
    /// Class for providing information about a pipeline component execution completion.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PipelineComponentExecutionCompletedInfo : PipelineComponentExecutionStartingInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipelineComponentName"></param>
        /// <param name="payload"></param>
        /// <param name="executionTime"></param>
        /// <param name="startTimeStamp"></param>
        /// <param name="exception"></param>
        public PipelineComponentExecutionCompletedInfo(string pipelineComponentName, object payload, TimeSpan executionTime, DateTime startTimeStamp, Exception exception = null) 
            : base(pipelineComponentName, payload, startTimeStamp)
        {
            ExecutionTime = executionTime;
            Exception = exception;
        }

        /// <summary>
        /// Instantiate a new <see cref="PipelineComponentExecutionCompletedInfo"/> instance
        /// </summary>
        /// <param name="startingInfo"></param>
        /// <param name="executionTime"></param>
        /// <param name="exception"></param>
        public PipelineComponentExecutionCompletedInfo(
            PipelineComponentExecutionStartingInfo startingInfo, 
            TimeSpan executionTime, 
            Exception exception = null)
            : base(startingInfo.PipelineComponentName, startingInfo.Payload, startingInfo.TimeStamp)
        {
            ExecutionTime = executionTime;
            Exception = exception;
        }

        /// <summary>
        /// Total amount of execution time
        /// </summary>
        public TimeSpan ExecutionTime { get; }


        /// <summary>
        /// Exception that might have occurred execution.
        /// </summary>
        public Exception Exception { get; }
    }
}
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
        /// Creates a new instance.
        /// </summary>
        /// <param name="pipelineComponentName">Name of the pipeline component that has executed.</param>
        /// <param name="executionTime">Total amount of execution time for the pipeline component.</param>
        /// <param name="exception">Exception that might have occurred during pipeline component execution.</param>
        [Obsolete("Use ctor with 4 parameters instead.")]
        public PipelineComponentExecutionCompletedInfo(string pipelineComponentName, TimeSpan executionTime, Exception exception = null) 
            : this(pipelineComponentName, null, executionTime, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipelineComponentName"></param>
        /// <param name="payload"></param>
        /// <param name="executionTime"></param>
        /// <param name="exception"></param>
        public PipelineComponentExecutionCompletedInfo(string pipelineComponentName, object payload, TimeSpan executionTime, Exception exception = null) 
            : base(pipelineComponentName, payload)
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
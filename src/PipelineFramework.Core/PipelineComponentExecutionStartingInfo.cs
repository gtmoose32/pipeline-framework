using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework
{
    /// <summary>
    /// Class for providing information about a pipeline component executing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PipelineComponentExecutionStartingInfo
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="pipelineComponentName">Name of the pipeline component that has executed.</param>
        public PipelineComponentExecutionStartingInfo(string pipelineComponentName)
        {
            PipelineComponentName = pipelineComponentName ?? throw new ArgumentNullException(nameof(pipelineComponentName));
            TimeStamp = DateTime.UtcNow;
        }        

        /// <summary>
        /// Name of the pipeline component that has executed.
        /// </summary>
        public string PipelineComponentName { get; }

        /// <summary>
        /// Provides pipeline component execution notification timestamp.
        /// </summary>
        public DateTime TimeStamp { get; }
    }
}
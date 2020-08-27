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
        [Obsolete("Use ctor with 2 parameters instead.")]
        public PipelineComponentExecutionStartingInfo(string pipelineComponentName)
            : this(pipelineComponentName, null)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="pipelineComponentName">Name of the pipeline component that has executed.</param>
        /// <param name="payload">Payload used by the executing pipeline.</param>
        public PipelineComponentExecutionStartingInfo(string pipelineComponentName, object payload)
        {
            PipelineComponentName = pipelineComponentName ?? throw new ArgumentNullException(nameof(pipelineComponentName));
            Payload = payload;
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

        /// <summary>
        /// Payload used by the executing pipeline.
        /// </summary>
        public object Payload { get; }
    }
}
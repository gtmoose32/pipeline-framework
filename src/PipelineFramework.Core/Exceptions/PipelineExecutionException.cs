using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PipelineFramework.Exceptions
{
    /// <summary>
    /// This exception is automatically thrown by both <see cref="Pipeline{T}"/> and <see cref="AsyncPipeline{T}"/> when a <see cref="IPipelineComponent"/> throws an unhandled exception during pipeline execution.
    /// </summary>
    [Serializable]
    public class PipelineExecutionException : PipelineComponentExceptionBase
    {
        private const string ErrorMessage = "Pipeline execution halted!  Pipeline component named '{0}' has thrown an exception.  See inner exception for details.";

        #region ctor
        public PipelineExecutionException(
            IPipelineComponent component, Exception componentException)
            : base(component, componentException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineExecutionException"/> with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data of the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains the contextual information about the source or the destination.</param>
        [ExcludeFromCodeCoverage]
        protected PipelineExecutionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { }  
        #endregion

        /// <inheritdoc />
        protected override string GetErrorMessage()
        {
            return string.Format(ErrorMessage, ThrowingComponent.Name);
        }
    }
}

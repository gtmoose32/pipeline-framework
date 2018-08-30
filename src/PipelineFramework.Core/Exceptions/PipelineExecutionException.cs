using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PipelineFramework.Exceptions
{
    /// <summary>
    /// This exception is thrown when any underlying <see cref="IPipelineComponent"/> raises an unhandled exception during 
    /// pipeline execution.
    /// </summary>
    [Serializable]
    public class PipelineExecutionException : PipelineComponentExceptionBase
    {
        private const string ErrorMessage = "Pipeline execution halted!  Pipeline component named '{0}' has thrown an exception.  See inner exception for details.";

        #region ctor
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="component">Type of <see cref="IPipelineComponent"/> that threw the exception.</param>
        /// <param name="componentException">The exception that was unhandled by <see cref="IPipelineComponent"/>.</param>
        // ReSharper disable once UnusedParameter.Local
        public PipelineExecutionException(
            IPipelineComponent component, Exception componentException)
            : base(component, componentException)
        { }

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

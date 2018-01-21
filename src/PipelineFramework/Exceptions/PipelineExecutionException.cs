using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Exceptions
{
    /// <summary>
    /// This exception is thrown when any underlying <see cref="IPipelineComponent"/> raises an unhandled exception during 
    /// pipeline execution.
    /// </summary>
    [Serializable]
    public class PipelineExecutionException : PipelineComponentExceptionBase
    {
        private const string ErrorMessage = "An exception has occurred within a pipeline component named '{0}', of type '{1}'.  See inner exception for details.";

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

        /// <inheritdoc />
        protected override string GetErrorMessage()
        {
            return string.Format(
                ErrorMessage, 
                ThrowingComponent.Name, 
                ThrowingComponent.GetType().Name);
        }
    }
}

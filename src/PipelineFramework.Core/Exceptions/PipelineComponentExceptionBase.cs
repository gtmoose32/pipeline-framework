using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PipelineFramework.Exceptions
{
    /// <summary>
    /// Abstract base class for pipeline framework exceptions.
    /// </summary>
    public abstract class PipelineComponentExceptionBase : Exception
    {
        #region ctor
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="component">Type of <see cref="IPipelineComponent"/> that threw the exception.</param>
        /// <param name="innerException">The exception that was unhandled by <see cref="IPipelineComponent"/>.</param>
        protected PipelineComponentExceptionBase(
            IPipelineComponent component,
            Exception innerException = null)
            : base(null, innerException)
        {
            ThrowingComponent = component;
        }

        
        /// <summary>
        /// Initializes a new instance with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data of the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains the contextual information about the source or the destination.</param>
        [ExcludeFromCodeCoverage]
        protected PipelineComponentExceptionBase(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { } 
        #endregion

        /// <summary>
        /// The pipeline component which threw the base exception from within the pipeline. 
        /// </summary>
        public IPipelineComponent ThrowingComponent { get; }

        /// <inheritdoc />
        public override string Message => GetErrorMessage();

        /// <summary>
        /// Provides exception specific message.
        /// </summary>
        /// <returns>Exception message</returns>
        protected abstract string GetErrorMessage();
    }
}

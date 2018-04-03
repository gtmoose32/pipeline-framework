﻿using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PipelineFramework.Exceptions
{
    /// <summary>
    /// Abstract base class for PEF exceptions.
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

        [ExcludeFromCodeCoverage]
        protected PipelineComponentExceptionBase(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { } 
        #endregion

        /// <summary>
        /// Exception source Type derived from <see cref="IPipelineComponent{T}" /> 
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

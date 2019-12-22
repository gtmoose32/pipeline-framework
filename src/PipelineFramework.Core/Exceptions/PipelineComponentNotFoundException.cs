using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PipelineFramework.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PipelineComponentNotFoundException : Exception
    {
        internal PipelineComponentNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineComponentNotFoundException"/> with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data of the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains the contextual information about the source or the destination.</param>
        [ExcludeFromCodeCoverage]
        protected PipelineComponentNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { }
    }
}

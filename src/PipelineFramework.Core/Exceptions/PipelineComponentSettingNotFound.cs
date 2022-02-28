using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace PipelineFramework.Exceptions
{
    /// <summary>
    /// Exception that is thrown by <see cref="Settings"/> when a specified setting cannot be found by a <see cref="IPipelineComponent"/>.
    /// </summary>
    [Serializable]
    public class PipelineComponentSettingNotFoundException : PipelineComponentExceptionBase
    {
        private const string ErrorMessage =
            "Pipeline component named '{0}' is referencing a setting named '{1}' that cannot be found.";

        #region ctor
        public PipelineComponentSettingNotFoundException(IPipelineComponent component, string settingName)
            : base(component)
        {
            SettingName = settingName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineComponentSettingNotFoundException"/> with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data of the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains the contextual information about the source or the destination.</param>
        [ExcludeFromCodeCoverage]
        protected PipelineComponentSettingNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        { } 
        #endregion

        /// <summary>
        /// Name of the setting that could not be found.
        /// </summary>
        public string SettingName { get; }

        /// <inheritdoc />
        protected override string GetErrorMessage()
        {
            return string.Format(ErrorMessage, ThrowingComponent.Name, SettingName);
        }
    }
}

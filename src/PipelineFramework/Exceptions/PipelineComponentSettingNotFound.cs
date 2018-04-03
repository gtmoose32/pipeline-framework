using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

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
        /// <summary>
        /// Creates a new exception instance.
        /// </summary>
        /// <param name="component">Type of <see cref="IPipelineComponent"/> that threw the exception.</param>
        /// <param name="settingName">The name of the setting that cannot be found.</param>
        public PipelineComponentSettingNotFoundException(IPipelineComponent component, string settingName)
            : base(component)
        {
            SettingName = settingName;
        }

        [ExcludeFromCodeCoverage]
        protected PipelineComponentSettingNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
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

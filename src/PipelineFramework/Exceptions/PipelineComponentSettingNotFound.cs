using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Exceptions
{
    /// <summary>
    /// Exception that is thrown by <see cref="Settings"/> when a specified setting cannot be found by a <see cref="IPipelineComponent"/>.
    /// </summary>
    [Serializable]
    public class PipelineComponentSettingNotFoundException : PipelineComponentExceptionBase
    {
        private const string ErrorMessage =
            "Pipeline component named '{0}' of type '{1}' is referencing a setting named '{2}' that cannot be found.";

        /// <summary>
        /// Creates a new exception instance.
        /// </summary>
        /// <param name="component">Type of <see cref="IPipelineComponent"/> that threw the exception.</param>
        /// <param name="settingName">The name of the setting that cannot be found.</param>
        // ReSharper disable once UnusedParameter.Local
        public PipelineComponentSettingNotFoundException(IPipelineComponent component, string settingName)
            : base(component)
        {
            SettingName = settingName;
        }

        /// <summary>
        /// Name of the setting that could not be found.
        /// </summary>
        public string SettingName { get; }

        /// <inheritdoc />
        protected override string GetErrorMessage()
        {
            return string.Format(
                ErrorMessage, 
                ThrowingComponent.Name, 
                ThrowingComponent.GetType().Name, 
                SettingName);
        }
    }
}

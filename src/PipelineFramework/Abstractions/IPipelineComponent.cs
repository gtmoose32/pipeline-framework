using System.Collections.Generic;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Defines a pipeline component.
    /// </summary>
    public interface IPipelineComponent
    {
        /// <summary>
        /// Name of the component.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initializes the component with name and configuration settings.
        /// </summary>
        /// <param name="name">Name of the component.</param>
        /// <param name="settings">Configuration settings.</param>
        void Initialize(string name, IDictionary<string, string> settings);
    }
}

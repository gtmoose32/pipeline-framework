using System.Collections.Generic;

namespace PipelineFramework.Abstractions.Builder
{
    /// <summary>
    /// Defines a configuration settings holder for a pipeline builder.
    /// </summary>
    /// <typeparam name="TPipeline"></typeparam>
    public interface ISettingsHolder<out TPipeline> where TPipeline : IPipeline
    {
        /// <summary>
        /// Sets the configuration settings the pipeline builder should use for building the pipeline.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>A pipeline builder.</returns>
        IPipelineBuilder<TPipeline> WithSettings(IDictionary<string, IDictionary<string, string>> settings);

        /// <summary>
        /// Instructs the pipeline builder to not use any configuration settings.
        /// </summary>
        /// <returns>A pipeline builder.</returns>
        IPipelineBuilder<TPipeline> WithNoSettings();
    }
}
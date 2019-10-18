using System.Collections.Generic;

namespace PipelineFramework.Abstractions.Builder
{
    public interface ISettingsHolder<out TPipeline> where TPipeline : IPipeline
    {
        IPipelineBuilder<TPipeline> WithSettings(IDictionary<string, IDictionary<string, string>> settings);

        IPipelineBuilder<TPipeline> WithNoSettings();
    }
}
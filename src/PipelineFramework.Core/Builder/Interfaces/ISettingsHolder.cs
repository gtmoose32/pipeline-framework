using System.Collections.Generic;
using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface ISettingsHolder<out TPipeline> where TPipeline : IPipeline
    {
        IPipelineBuilder<TPipeline> WithSettings(IDictionary<string, IDictionary<string, string>> settings);

        IPipelineBuilder<TPipeline> WithNoSettings();
    }
}
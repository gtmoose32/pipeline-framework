using System.Collections.Generic;
using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder.Interfaces
{
    public interface ISettingsHolder<out TPipeline, TPayload> where TPipeline : IPipeline
    {
        IPipelineBuilder<TPipeline, TPayload> WithSettings(IDictionary<string, IDictionary<string, string>> settings);

        IPipelineBuilder<TPipeline, TPayload> WithNoSettings();
    }
}
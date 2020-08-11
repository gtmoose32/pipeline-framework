using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework
{
    [ExcludeFromCodeCoverage]
    public class PipelineComponentExecutionStartedInfo
    {
        public PipelineComponentExecutionStartedInfo(string pipelineComponentName)
        {
            PipelineComponentName = pipelineComponentName ?? throw new ArgumentNullException(nameof(pipelineComponentName));
            TimeStamp = DateTime.UtcNow;
        }        

        public string PipelineComponentName { get; }
        public DateTime TimeStamp { get; }
    }
}
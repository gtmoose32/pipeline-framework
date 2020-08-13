using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Examples
{
    [ExcludeFromCodeCoverage]
    public class ExecutionStatusReceiver : IAsyncPipelineComponentExecutionStatusReceiver
    {
        public Task ReceiveExecutionStartingAsync(PipelineComponentExecutionStartingInfo executionInfo)
        {
            Console.WriteLine(
                $"Component '{executionInfo.PipelineComponentName}' execution starting at {executionInfo.TimeStamp.ToShortTimeString()}");

            return Task.CompletedTask;
        }
        
        public Task ReceiveExecutionCompletedAsync(PipelineComponentExecutionCompletedInfo executionInfo)
        {
            Console.WriteLine(
                $"Component '{executionInfo.PipelineComponentName}' execution completed at {executionInfo.TimeStamp.ToShortTimeString()}.  Duration: {executionInfo.ExecutionTime.TotalMilliseconds}ms");

            return Task.CompletedTask;
        }
    }
}
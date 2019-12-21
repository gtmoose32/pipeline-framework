using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Examples.Components
{
    [ExcludeFromCodeCoverage]
    public class BarComponent : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.Result = new Random().Next(100);
            
            payload.Messages.Add($"Component {Name} called external api and returned result {payload.Result}");

            return Task.FromResult(payload);
        }
    }

    [ExcludeFromCodeCoverage]
    public class BarComponentNonAsync : PipelineComponentBase<ExamplePipelinePayload>
    {
        public override ExamplePipelinePayload Execute(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.Result = new Random().Next(100);

            payload.Messages.Add($"Component {Name} called external api and returned result {payload.Result}");

            return payload;
        }
    }
}

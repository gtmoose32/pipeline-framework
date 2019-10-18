using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Examples.Components
{
    [ExcludeFromCodeCoverage]
    public class FooComponent : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.FooKey = Guid.NewGuid();
            payload.Messages.Add($"Component {Name} retrieved FooKey {payload.FooKey}");

            return Task.FromResult(payload);
        }
    }

    [ExcludeFromCodeCoverage]
    public class FooComponentNonAsync : PipelineComponentBase<ExamplePipelinePayload>
    {
        public override ExamplePipelinePayload Execute(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.FooKey = Guid.NewGuid();
            payload.Messages.Add($"Component {Name} retrieved FooKey {payload.FooKey}");
            return payload;
        }
    }
}
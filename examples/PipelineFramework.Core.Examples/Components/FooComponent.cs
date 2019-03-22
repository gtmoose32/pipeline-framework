using PipelineFramework.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Examples.Components
{
    public class FooComponent : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.FooKey = Guid.NewGuid();
            payload.Messages.Add($"Component {Name} retrieved FooKey {payload.FooKey}");

            return Task.FromResult(payload);
        }
    }

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
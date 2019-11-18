using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Examples.Components
{
    [ExcludeFromCodeCoverage]
    public class FooComponent : AsyncPipelineComponentBase<ExamplePipelinePayload>, IDisposable
    {
        public override Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.FooKey = Guid.NewGuid();
            payload.Messages.Add($"Component {Name} retrieved FooKey {payload.FooKey}");

            return Task.FromResult(payload);
        }

        /// <summary>
        /// This is only here to demonstrate the use of <see cref="IDisposable"/> with pipeline components.
        /// </summary>
        public void Dispose()
        {
            //Dispose of any resources
        }
    }

    [ExcludeFromCodeCoverage]
    public class FooComponentNonAsync : PipelineComponentBase<ExamplePipelinePayload>, IDisposable
    {
        public override ExamplePipelinePayload Execute(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            payload.FooKey = Guid.NewGuid();
            payload.Messages.Add($"Component {Name} retrieved FooKey {payload.FooKey}");
            return payload;
        }

        /// <summary>
        /// This is only here to demonstrate the use of <see cref="IDisposable"/> with pipeline components.
        /// </summary>
        public void Dispose()
        {
            //Dispose of any resources
        }
    }
}
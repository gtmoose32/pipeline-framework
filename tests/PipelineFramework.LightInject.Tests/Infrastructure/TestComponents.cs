using PipelineFramework.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Tests
{
    [ExcludeFromCodeCoverage]
    public class FooComponent : PipelineComponentBase<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = $"{GetType().Name} executed!";
            payload.Count++;
            return payload;
        }
    }

    [ExcludeFromCodeCoverage]
    public class BarComponent : PipelineComponentBase<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.BarStatus = $"{GetType().Name} executed!";
            payload.Count++;
            return payload;
        }
    }
}

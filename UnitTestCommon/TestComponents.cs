using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestCommon
{
    [ExcludeFromCodeCoverage]
    public class AsyncFooComponent : AsyncPipelineComponentBase<TestPayload>
    {
        public override Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = $"{GetType().Name} executed!";
            payload.Count++;

            return Task.FromResult(payload);
        }
    }

    [ExcludeFromCodeCoverage]
    public class ConfigurableComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            if (bool.Parse(Settings["UseFoo"]))
                payload.FooStatus = Settings["TestValue"];
            else
                payload.BarStatus = Settings["TestValue"];

            return payload;
        }

        public Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class FooComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = $"{GetType().Name} executed!";
            payload.Count++;
            return payload;
        }

        public Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class FooSettingNotFoundComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = Settings["setting that doesn't exist"];
            payload.Count++;
            return payload;
        }

        public Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class BarComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.BarStatus = $"{GetType().Name} executed!";
            payload.Count++;
            return payload;
        }

        public Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class BarExceptionComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken) 
            => throw new NotImplementedException();
    }

    [ExcludeFromCodeCoverage]
    public class DelayComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            Task.Delay(TimeSpan.FromSeconds(5), cancellationToken).Wait(cancellationToken);
            return payload;
        }
        
        public Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class PipelineExecutionTerminatingComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.Count++;
            return null;
        }
        public Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }


}

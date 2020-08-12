using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.TestInfrastructure
{
    [ExcludeFromCodeCoverage]
    public class AsyncTestComponent : AsyncPipelineComponentBase<TestPayload>
    {
        public override Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = $"{GetType().Name} executed!";
            payload.Count++;

            return Task.FromResult(payload);
        }

        public IDictionary<string, string> TestSettings => Settings;
    }

    [ExcludeFromCodeCoverage]
    public class ConfigurableComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        private bool _useFoo;

        public override void Initialize(IDictionary<string, string> settings)
        {
            base.Initialize(settings);

            _useFoo = Settings.GetSettingValue<bool>("UseFoo");
        }

        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            if (_useFoo)
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
            payload.FooWasCalled = true;
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
            payload.BarWasCalled = true;
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

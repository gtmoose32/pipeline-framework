using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class Payload
    {
        public int Count { get; set; }
        public string FooStatus { get; set; }
        public string BarStatus { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AsyncFooComponent : AsyncPipelineComponentBase<Payload>
    {
        public override Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = $"{GetType().Name} executed!";
            payload.Count++;

            return Task.FromResult(payload);
        }
    }

    [ExcludeFromCodeCoverage]
    public class ConfigurableComponent : PipelineComponentBase<Payload>, IAsyncPipelineComponent<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            if (bool.Parse(Settings["UseFoo"]))
                payload.FooStatus = Settings["TestValue"];
            else
                payload.BarStatus = Settings["TestValue"];

            return payload;
        }

        public Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class FooComponent : PipelineComponentBase<Payload>, IAsyncPipelineComponent<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = $"{GetType().Name} executed!";
            payload.Count++;
            return payload;
        }

        public Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class FooSettingNotFoundComponent : PipelineComponentBase<Payload>, IAsyncPipelineComponent<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = Settings["setting that doesn't exist"];
            payload.Count++;
            return payload;
        }

        public Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class BarComponent : PipelineComponentBase<Payload>, IAsyncPipelineComponent<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            payload.BarStatus = $"{GetType().Name} executed!";
            payload.Count++;
            return payload;
        }

        public Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class BarExceptionComponent : PipelineComponentBase<Payload>, IAsyncPipelineComponent<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken) 
            => throw new NotImplementedException();
    }

    [ExcludeFromCodeCoverage]
    public class DelayComponent : PipelineComponentBase<Payload>, IAsyncPipelineComponent<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            Task.Delay(TimeSpan.FromSeconds(5), cancellationToken).Wait(cancellationToken);
            return payload;
        }
        
        public Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

    [ExcludeFromCodeCoverage]
    public class PipelineExecutionTerminatingComponent : PipelineComponentBase<Payload>, IAsyncPipelineComponent<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            payload.Count++;
            return null;
        }
        public Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }

}

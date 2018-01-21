using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class Payload
    {
        public int Count { get; set; }
        public string FooStatus { get; set; }
        public string BarStatus { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ConfigurableComponent : PipelineComponentBase<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            if (bool.Parse(Settings["UseFoo"]))
                payload.FooStatus = Settings["TestValue"];
            else
                payload.BarStatus = Settings["TestValue"];

            return payload;
        }
    }

    [ExcludeFromCodeCoverage]
    public class AsyncConfigurableComponent : AsyncPipelineComponentBase<Payload>
    {
        public override Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            if (bool.Parse(Settings["UseFoo"]))
                payload.FooStatus = Settings["TestValue"];
            else
                payload.BarStatus = Settings["TestValue"];

            return Task.FromResult(payload);
        }
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
    public class AsyncFooSettingNotFoundComponent : AsyncPipelineComponentBase<Payload>
    {
        public override Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = Settings["setting that doesn't exist"];
            payload.Count++;

            return Task.FromResult(payload);
        }
    }

    [ExcludeFromCodeCoverage]
    public class AsyncBarComponent : AsyncPipelineComponentBase<Payload>
    {
        public override Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            payload.Count++;
            payload.BarStatus = $"{GetType().Name} executed!";

            return Task.FromResult(payload);
        }
    }

    [ExcludeFromCodeCoverage]
    public class AsyncBarExceptionComponent : AsyncPipelineComponentBase<Payload>
    {
        public override Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken) 
            => throw new NotImplementedException();
    }

    [ExcludeFromCodeCoverage]
    public class FooComponent : PipelineComponentBase<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = $"{GetType().Name} executed!";
            payload.Count++;
            return payload;
        }
    }

    [ExcludeFromCodeCoverage]
    public class FooSettingNotFoundComponent : PipelineComponentBase<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = Settings["setting that doesn't exist"];
            payload.Count++;
            return payload;
        }
    }

    [ExcludeFromCodeCoverage]
    public class BarComponent : PipelineComponentBase<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            payload.BarStatus = $"{GetType().Name} executed!";
            payload.Count++;
            return payload;
        }
    }

    [ExcludeFromCodeCoverage]
    public class BarExceptionComponent : PipelineComponentBase<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [ExcludeFromCodeCoverage]
    public class AsyncDelayComponent : AsyncPipelineComponentBase<Payload>
    {
        public override async Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

            return payload;
        }
    }

    [ExcludeFromCodeCoverage]
    public class DelayComponent : PipelineComponentBase<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            Task.Delay(TimeSpan.FromSeconds(5), cancellationToken).Wait(cancellationToken);
            return payload;
        }
    }

    [ExcludeFromCodeCoverage]
    public class AsyncFilteringComponent : AsyncPipelineComponentBase<Payload>
    {
        public override Task<Payload> ExecuteAsync(Payload payload, CancellationToken cancellationToken)
        {
            payload.Count++;
            return Task.FromResult(null as Payload);
        }
    }

    [ExcludeFromCodeCoverage]
    public class FilteringComponent : PipelineComponentBase<Payload>
    {
        public override Payload Execute(Payload payload, CancellationToken cancellationToken)
        {
            payload.Count++;
            return null;
        }
    }

}

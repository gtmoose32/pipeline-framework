using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Examples.Components
{
    [ExcludeFromCodeCoverage]
    public class DelayComponent : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        private TimeSpan _delay;

        public override void Initialize(IDictionary<string, string> settings)
        {
            base.Initialize(settings);

            _delay = Settings.GetSettingValue("DelayTimeSpan", TimeSpan.FromSeconds(1));
        }

        public override async Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            await Task.Delay(_delay, cancellationToken);

            payload.Messages.Add($"Component {Name} delayed for {_delay}");

            return payload;
        }
    }

    [ExcludeFromCodeCoverage]
    public class DelayComponentNonAsync : PipelineComponentBase<ExamplePipelinePayload>
    {
        private TimeSpan _delay;

        public override void Initialize(IDictionary<string, string> settings)
        {
            base.Initialize(settings);

            _delay = Settings.GetSettingValue("DelayTimeSpan", TimeSpan.FromSeconds(1));
        }

        public override ExamplePipelinePayload Execute(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            Thread.Sleep((int) _delay.TotalMilliseconds);
            payload.Messages.Add($"Component {Name} delayed for {_delay}");
            return payload;
        }
    }
}
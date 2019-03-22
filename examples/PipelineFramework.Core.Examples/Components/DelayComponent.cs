using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Examples.Components
{
    public class DelayComponent : AsyncPipelineComponentBase<ExamplePipelinePayload>
    {
        private TimeSpan _delay;

        public override void Initialize(string name, IDictionary<string, string> settings)
        {
            base.Initialize(name, settings);

            _delay = Settings.GetSettingValue("DelayTimeSpan", TimeSpan.FromSeconds(5), false);
        }

        public override async Task<ExamplePipelinePayload> ExecuteAsync(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            await Task.Delay(_delay, cancellationToken);

            payload.Messages.Add($"Component {Name} delayed for {_delay}");

            return payload;
        }
    }

    public class DelayComponentNonAsync : PipelineComponentBase<ExamplePipelinePayload>
    {
        private TimeSpan _delay;

        public override void Initialize(string name, IDictionary<string, string> settings)
        {
            base.Initialize(name, settings);

            _delay = Settings.GetSettingValue("DelayTimeSpan", TimeSpan.FromSeconds(5), false);
        }

        public override ExamplePipelinePayload Execute(ExamplePipelinePayload payload, CancellationToken cancellationToken)
        {
            Thread.Sleep((int) _delay.TotalMilliseconds);
            payload.Messages.Add($"Component {Name} delayed for {_delay}");
            return payload;
        }
    }
}
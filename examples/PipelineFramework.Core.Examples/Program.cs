using PipelineFramework.Abstractions;
using PipelineFramework.Core.Examples.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PipelineFramework.Builder;

namespace PipelineFramework.Core.Examples
{
    class Program
    {
        static async Task Main()
        {
            var components = new Dictionary<string, IPipelineComponent>
            {
                {typeof(FooComponent).Name, new FooComponent()},
                {typeof(DelayComponent).Name, new DelayComponent()},
                {typeof(BarComponent).Name, new BarComponent()},
            };

            var resolver = new DictionaryPipelineComponentResolver(components);

            var settings = new Dictionary<string, IDictionary<string, string>>
            {
                {typeof(DelayComponent).Name, new Dictionary<string, string>
                {
                    {"DelayTimeSpan", "00:00:10"}
                }}
            };

            var pipeline = PipelineBuilder<ExamplePipelinePayload>
                .Async()
                .WithComponent<FooComponent>()
                .WithComponent<DelayComponent>()
                .WithComponent<BarComponent>()
                .WithComponentResolver(resolver)
                .WithSettings(settings)
                .Build();
                    
            var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

            result.Messages.ForEach(Console.WriteLine);

            Console.Read();
        }
    }
}

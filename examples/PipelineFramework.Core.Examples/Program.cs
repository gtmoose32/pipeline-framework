using PipelineFramework.Abstractions;
using PipelineFramework.Core.Examples.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

            var order = new List<Type> {typeof(FooComponent), typeof(DelayComponent), typeof(BarComponent)};

            var pipeline = new AsyncPipeline<ExamplePipelinePayload>(resolver, order, settings);

            var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

            result.Messages.ForEach(Console.WriteLine);

            Console.Read();
        }
    }
}

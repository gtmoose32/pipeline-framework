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
                {typeof(FooComponentNonAsync).Name, new FooComponentNonAsync()},
                {typeof(DelayComponentNonAsync).Name, new DelayComponentNonAsync()},
                {typeof(BarComponentNonAsync).Name, new BarComponentNonAsync()}
            };

            var resolver = new DictionaryPipelineComponentResolver(components);

            var settings = new Dictionary<string, IDictionary<string, string>>
            {
                {typeof(DelayComponent).Name, new Dictionary<string, string>
                {
                    {"DelayTimeSpan", "00:00:10"}
                }}
            };

            await InvokePipelineAsync(resolver, settings);
            InvokePipeline(resolver, settings);

            Console.Read();
        }

        private static async Task InvokePipelineAsync(DictionaryPipelineComponentResolver resolver, Dictionary<string, IDictionary<string, string>> settings)
        {
            
            var pipeline = PipelineBuilder<ExamplePipelinePayload>
                .Async()
                .WithComponent<FooComponent>()
                .WithComponent<DelayComponent>()
                .WithComponent<BarComponent>()
                .WithComponentResolver(resolver)
                .WithSettings(settings)
                .Build();

            Console.WriteLine("Executing pipeline asynchronously.");
            var result = await pipeline.ExecuteAsync(new ExamplePipelinePayload(), CancellationToken.None);

            result.Messages.ForEach(Console.WriteLine);
        }

        private static void InvokePipeline(DictionaryPipelineComponentResolver resolver, Dictionary<string, IDictionary<string, string>> settings)
        {
            var pipeline = PipelineBuilder<ExamplePipelinePayload>
                .NonAsync()
                .WithComponent<FooComponentNonAsync>()
                .WithComponent<DelayComponentNonAsync>()
                .WithComponent<BarComponentNonAsync>()
                .WithComponentResolver(resolver)
                .WithSettings(settings)
                .Build();

            Console.WriteLine("Executing pipeline synchronously.");
            var result = pipeline.Execute(new ExamplePipelinePayload());

            result.Messages.ForEach(Console.WriteLine);
        }
    }
}

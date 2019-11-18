using PipelineFramework.Abstractions;
using PipelineFramework.Builder;
using PipelineFramework.Core.Examples.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Examples
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static async Task Main()
        {
            var components = new Dictionary<string, IPipelineComponent>
            {
                {nameof(FooComponent), new FooComponent()},
                {nameof(DelayComponent), new DelayComponent()},
                {nameof(BarComponent), new BarComponent()},
                {nameof(FooComponentNonAsync), new FooComponentNonAsync()},
                {nameof(DelayComponentNonAsync), new DelayComponentNonAsync()},
                {nameof(BarComponentNonAsync), new BarComponentNonAsync()}
            };

            var resolver = new DictionaryPipelineComponentResolver(components);

            var settings = new Dictionary<string, IDictionary<string, string>>
            {
                {typeof(DelayComponent).Name, new Dictionary<string, string>
                {
                    {"DelayTimeSpan", "00:00:05"}
                }}
            };

            Console.WriteLine();

            await InvokePipelineAsync(resolver, settings);
            InvokePipeline(resolver, settings);

            Console.Write("\nPress any key to exit...");
            Console.Read();
        }

        private static async Task InvokePipelineAsync(IPipelineComponentResolver resolver, IDictionary<string, IDictionary<string, string>> settings)
        {
            Console.WriteLine("Executing pipeline asynchronously.");

            var payload = new ExamplePipelinePayload();

            using (var pipeline = PipelineBuilder<ExamplePipelinePayload>
                .Async()
                .WithComponent<FooComponent>()
                .WithComponent<DelayComponent>()
                .WithComponent<BarComponent>()
                .WithComponentResolver(resolver)
                .WithSettings(settings)
                .Build())
            {
                payload = await pipeline.ExecuteAsync(payload);
            }

            payload.Messages.ForEach(Console.WriteLine);
        }

        private static void InvokePipeline(IPipelineComponentResolver resolver, IDictionary<string, IDictionary<string, string>> settings)
        {
            Console.WriteLine("\nExecuting pipeline synchronously.");

            var payload = new ExamplePipelinePayload();

            using (var pipeline = PipelineBuilder<ExamplePipelinePayload>
                .NonAsync()
                .WithComponent<FooComponentNonAsync>()
                .WithComponent<DelayComponentNonAsync>()
                .WithComponent<BarComponentNonAsync>()
                .WithComponentResolver(resolver)
                .WithSettings(settings)
                .Build())
            {
                payload = pipeline.Execute(payload);
            }

            payload.Messages.ForEach(Console.WriteLine);
        }
    }
}

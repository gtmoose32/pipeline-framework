using Microsoft.Extensions.DependencyInjection;
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
    static class Program
    {
        static async Task Main()
        {
            var resolver = new DictionaryPipelineComponentResolver();
            resolver.AddAsync(new FooComponent(), new DelayComponent(), new BarComponent());
            resolver.Add(new FooComponentNonAsync(), new DelayComponentNonAsync(), new BarComponentNonAsync());
            
            var settings = new Dictionary<string, IDictionary<string, string>>
            {
                {nameof(DelayComponent), new Dictionary<string, string>
                {
                    {"DelayTimeSpan", "00:00:05"}
                }}
            };

            Console.WriteLine();

            // ReSharper disable once MethodHasAsyncOverload
            InvokePipeline(resolver, settings);

            await InvokePipelineAsync(resolver, settings);

            await InvokePipelineWithDependencyInjectionAndStatusReceiverAsync(settings);

            await InvokeNamedPipelinesWithDependencyInjectionAndStatusReceiverAsync();

            Console.Write("\nPress any key to exit...");
            Console.Read();
        }

        private static void AddAsyncPipelineToContainer(
            IServiceCollection services,
            IDictionary<string, IDictionary<string, string>> settings = null, 
            string pipelineName = null)
        {
            services
                .AddPipelineFramework()
                .AddAsyncPipeline<ExamplePipelinePayload, ExecutionStatusReceiver>(
                    cfg => cfg
                        .WithComponent<FooComponent>()
                        .WithComponent<DelayComponent>()
                        .WithComponent<BarComponent>(),
                    settings, 
                    pipelineName);
        }

        private static async Task InvokePipelineWithDependencyInjectionAndStatusReceiverAsync(IDictionary<string, IDictionary<string, string>> settings)
        {
            Console.WriteLine("Executing pipeline asynchronously with dependency injection and execution status receiver.\n");

            var payload = new ExamplePipelinePayload();
            var services = new ServiceCollection();
            AddAsyncPipelineToContainer(services, settings);
            var serviceProvider = services.BuildServiceProvider();

            using (var pipeline = serviceProvider.GetService<IAsyncPipeline<ExamplePipelinePayload>>())
            {
                if (pipeline == null) return;
                payload = await pipeline.ExecuteAsync(payload);
            }

            Console.WriteLine("\n");

            payload.Messages.ForEach(Console.WriteLine);
            
            Console.WriteLine("\n");
        }

        private static async Task InvokeNamedPipelinesWithDependencyInjectionAndStatusReceiverAsync()
        {
            Console.WriteLine("Executing named pipelines asynchronously with dependency injection and execution status receiver.\n");

            var services = new ServiceCollection();
            AddAsyncPipelineToContainer(services, pipelineName: "pipeline-1");
            AddAsyncPipelineToContainer(services, pipelineName: "pipeline-2");
            var serviceProvider = services.BuildServiceProvider();

            foreach (var pipeline in serviceProvider.GetServices<IAsyncPipeline<ExamplePipelinePayload>>())
            {
                var payload = new ExamplePipelinePayload();

                Console.WriteLine("\n");

                using (pipeline)
                {
                    if (pipeline == null) return;
                    payload = await pipeline.ExecuteAsync(payload);
                    payload.Messages.ForEach(msg => Console.WriteLine($"{pipeline.Name}:  {msg}"));
                }
                
                Console.WriteLine("\n");
            }
        }

        private static async Task InvokePipelineAsync(IPipelineComponentResolver resolver, IDictionary<string, IDictionary<string, string>> settings)
        {
            Console.WriteLine("Executing pipeline asynchronously.\n");

            var payload = new ExamplePipelinePayload();

            using (var pipeline = PipelineBuilder<ExamplePipelinePayload>
                .InitializeAsyncPipeline()
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

            Console.WriteLine("\n");
        }

        private static void InvokePipeline(IPipelineComponentResolver resolver, IDictionary<string, IDictionary<string, string>> settings)
        {
            Console.WriteLine("\nExecuting pipeline synchronously.\n");

            var payload = new ExamplePipelinePayload();

            using (var pipeline = PipelineBuilder<ExamplePipelinePayload>
                .InitializePipeline()
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

            Console.WriteLine("\n");
        }
    }
}

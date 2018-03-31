using System.Threading;
using LightInject;
using PipelineFramework.Abstractions;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject
{
    public class LightInjectPipelineFactoryExecutor : IAsyncPipelineFactoryExecutor, IPipelineFactoryExecutor
    {
        private readonly IServiceFactory _serviceFactory;

        public LightInjectPipelineFactoryExecutor(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task<TPayload> CreateAsyncPipelineAndExecuteAsync<TPayload>(TPayload payload, CancellationToken cancellationToken = default)
        {
            var pipeline = _serviceFactory.GetInstance<IAsyncPipeline<TPayload>>();
            return pipeline.ExecuteAsync(payload, cancellationToken);
        }

        public TPayload CreatePipelineAndExecute<TPayload>(TPayload payload, CancellationToken cancellationToken = default)
        {
            var pipeline = _serviceFactory.GetInstance<IPipeline<TPayload>>();
            return pipeline.Execute(payload, cancellationToken);
        }
    }
}

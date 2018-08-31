using Autofac;
using PipelineFramework.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Autofac
{
    public class PipelineFactoryExecutor : IPipelineFactoryExecutor, IAsyncPipelineFactoryExecutor
    {
        private readonly IComponentContext _componentContext;

        public PipelineFactoryExecutor(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public TPayload CreatePipelineAndExecute<TPayload>(TPayload payload, CancellationToken cancellationToken = default)
        {
            var pipeline = _componentContext.Resolve<IPipeline<TPayload>>();
            return pipeline.Execute(payload, cancellationToken);
        }

        public Task<TPayload> CreateAsyncPipelineAndExecuteAsync<TPayload>(TPayload payload, CancellationToken cancellationToken = default)
        {
            var pipeline = _componentContext.Resolve<IAsyncPipeline<TPayload>>();
            return pipeline.ExecuteAsync(payload, cancellationToken);
        }
    }
}

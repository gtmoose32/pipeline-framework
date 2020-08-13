using PipelineFramework.Abstractions;
using PipelineFramework.Abstractions.Builder;

namespace PipelineFramework.Builder
{
    internal class AsyncPipelineBuilder<TPayload> : PipelineBuilderBase<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload>
    {
        private AsyncPipelineBuilder(IAsyncPipelineComponentExecutionStatusReceiver executionStatusReceiver = null)
            : base(executionStatusReceiver)
        {
        }

        public static IInitialPipelineComponentHolder<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> Initialize(
            IAsyncPipelineComponentExecutionStatusReceiver executionStatusReceiver) => 
                new AsyncPipelineBuilder<TPayload>(executionStatusReceiver);

        public override IAsyncPipeline<TPayload> Build() 
            => new AsyncPipeline<TPayload>(
                State.ComponentResolver, 
                State.ComponentNames, 
                State.Settings, 
                State.AsyncPipelineComponentExecutionStatusReceiver);
    }
}
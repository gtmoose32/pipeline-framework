using PipelineFramework.Abstractions;
using PipelineFramework.Abstractions.Builder;

namespace PipelineFramework.Builder
{
    internal class AsyncPipelineBuilder<TPayload> : PipelineBuilderBase<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload>
    {
        private AsyncPipelineBuilder()
        {
        }

        public static IInitialPipelineComponentHolder<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> Initialize()
            => new AsyncPipelineBuilder<TPayload>();

        public override IAsyncPipeline<TPayload> Build()
            => State.UseNoSettings
                ? new AsyncPipeline<TPayload>(State.ComponentResolver, State.ComponentNames)
                : new AsyncPipeline<TPayload>(State.ComponentResolver, State.ComponentNames, State.Settings);
    }
}
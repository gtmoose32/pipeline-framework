using PipelineFramework.Abstractions;
using PipelineFramework.Builder.Interfaces;

namespace PipelineFramework.Builder
{
    /// <summary>
    /// Builder to assist in correctly creating <see cref="IAsyncPipeline{T}"/> instances.
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    internal class AsyncPipelineBuilder<TPayload> : PipelineBuilderBase<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload>
    {
        private AsyncPipelineBuilder()
        {
        }

        public static IInitialPipelineComponentHolder<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> Initialize()
        {
            return new AsyncPipelineBuilder<TPayload>();
        }

        public override IAsyncPipeline<TPayload> Build()
        {
            return new AsyncPipeline<TPayload>(
                State.ComponentResolver,
                State.ComponentNames,
                State.Settings);
        }
    }
}
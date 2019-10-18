using PipelineFramework.Abstractions;
using PipelineFramework.Abstractions.Builder;

namespace PipelineFramework.Builder
{
    public static class PipelineBuilder<TPayload>
    {
        public static IInitialPipelineComponentHolder<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> Async()
        {
            return AsyncPipelineBuilder<TPayload>.Initialize();
        }

        public static IInitialPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> NonAsync()
        {
            return NonAsyncPipelineBuilder<TPayload>.Initialize();
        }
    }
}
using PipelineFramework.Abstractions;
using PipelineFramework.Builder;
using PipelineFramework.Builder.Interfaces;

namespace PipelineFramework
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
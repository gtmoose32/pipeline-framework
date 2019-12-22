using PipelineFramework.Abstractions;
using PipelineFramework.Abstractions.Builder;

namespace PipelineFramework.Builder
{
    /// <summary>
    /// Provides fluent builder syntax for initializing async/sync pipelines.
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public static class PipelineBuilder<TPayload>
    {
        /// <summary>
        /// Initiates an instance Async pipeline builder.
        /// </summary>
        /// <returns>Initial pipeline builder component holder.</returns>
        public static IInitialPipelineComponentHolder<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> Async()
        {
            return AsyncPipelineBuilder<TPayload>.Initialize();
        }

        /// <summary>
        /// Initiates an instance NonAsync pipeline builder.
        /// </summary>
        /// <returns>Initial pipeline builder component  holder.</returns>
        public static IInitialPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> NonAsync()
        {
            return NonAsyncPipelineBuilder<TPayload>.Initialize();
        }
    }
}
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
        /// <param name="executionStatusReceiver"></param>
        /// <returns>Initial pipeline builder component holder.</returns>
        public static IInitialPipelineComponentHolder<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> InitializeAsyncPipeline(
            IAsyncPipelineComponentExecutionStatusReceiver executionStatusReceiver = null)
        {
            return AsyncPipelineBuilder<TPayload>.Initialize(executionStatusReceiver);
        }

        /// <summary>
        /// Initiates an instance NonAsync pipeline builder.
        /// </summary>
        /// <param name="executionStatusReceiver"></param>
        /// <returns>Initial pipeline builder component  holder.</returns>
        public static IInitialPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> InitializePipeline(
            IPipelineComponentExecutionStatusReceiver executionStatusReceiver = null)
        {
            return NonAsyncPipelineBuilder<TPayload>.Initialize(executionStatusReceiver);
        }
    }
}
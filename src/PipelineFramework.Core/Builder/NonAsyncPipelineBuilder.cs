using PipelineFramework.Abstractions;
using PipelineFramework.Abstractions.Builder;

namespace PipelineFramework.Builder
{
    /// <summary>
    /// Builder implementation to assist in creating <see cref="IPipeline{T}"/> instances
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    internal class NonAsyncPipelineBuilder<TPayload> : PipelineBuilderBase<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload>
    {
        private NonAsyncPipelineBuilder()
        {
        }

        internal static IInitialPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> Initialize()
        {
            return new NonAsyncPipelineBuilder<TPayload>();
        }

        public override IPipeline<TPayload> Build()
        {
            return new Pipeline<TPayload>(
                    State.ComponentResolver,
                    State.ComponentNames,
                    State.Settings);
        }
    }
}
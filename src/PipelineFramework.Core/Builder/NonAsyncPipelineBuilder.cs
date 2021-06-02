using PipelineFramework.Abstractions;
using PipelineFramework.Abstractions.Builder;
using System;

namespace PipelineFramework.Builder
{
    internal class NonAsyncPipelineBuilder<TPayload> : PipelineBuilderBase<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload>
    {
        private NonAsyncPipelineBuilder(IPipelineComponentExecutionStatusReceiver executionStatusReceiver)
            : base(executionStatusReceiver)
        {
        }

        public static IInitialPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> Initialize(
            IPipelineComponentExecutionStatusReceiver executionStatusReceiver) 
                => new NonAsyncPipelineBuilder<TPayload>(executionStatusReceiver);
        
        public override IPipeline<TPayload> Build(string pipelineName = null)
            => new Pipeline<TPayload>(
                State.ComponentResolver, 
                State.ComponentNames, 
                State.Settings, 
                State.PipelineComponentExecutionStatusReceiver)
            {
                Name = pipelineName ?? $"Pipeline{Guid.NewGuid():N}"
            };
    }
}
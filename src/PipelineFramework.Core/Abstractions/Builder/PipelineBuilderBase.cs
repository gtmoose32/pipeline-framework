using PipelineFramework.Builder;
using System.Collections.Generic;

namespace PipelineFramework.Abstractions.Builder
{
    internal abstract class PipelineBuilderBase<TPipeline, TComponentBase, TPayload> : 
        IPipelineBuilder<TPipeline>,
        ISettingsHolder<TPipeline>, 
        IAdditionalPipelineComponentHolder<TPipeline, TComponentBase, TPayload> where TPipeline : IPipeline
        where TComponentBase : IPipelineComponent
    {
        protected readonly PipelineBuilderState State;

        #region ctor
        protected PipelineBuilderBase(IPipelineComponentExecutionStatusReceiver executionStatusReceiver) 
            : this()
        {
            State.PipelineComponentExecutionStatusReceiver = executionStatusReceiver;
        }

        protected PipelineBuilderBase(IAsyncPipelineComponentExecutionStatusReceiver executionStatusReceiver) 
            : this()
        {
            State.AsyncPipelineComponentExecutionStatusReceiver = executionStatusReceiver;
        }

        private PipelineBuilderBase()
        {
            State = new PipelineBuilderState();
        } 
        #endregion

        public abstract TPipeline Build();

        public IPipelineBuilder<TPipeline> WithSettings(IDictionary<string, IDictionary<string, string>> settings)
        {
            State.Settings = settings;
            return this;
        }

        public IPipelineBuilder<TPipeline> WithoutSettings() => this;

        public ISettingsHolder<TPipeline> WithComponentResolver(IPipelineComponentResolver componentResolver)
        {
            State.ComponentResolver = componentResolver;
            return this;
        }

        public IAdditionalPipelineComponentHolder<TPipeline, TComponentBase, TPayload> WithComponent<TComponent>() where TComponent : TComponentBase
        {
            State.AddComponent(typeof(TComponent));
            return this;
        }
    }
}
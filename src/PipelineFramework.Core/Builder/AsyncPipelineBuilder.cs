using PipelineFramework.Abstractions;
using PipelineFramework.Builder.Interfaces;
using System.Collections.Generic;

namespace PipelineFramework.Builder
{
    /// <summary>
    /// Builder to assist in correctly creating <see cref="IAsyncPipeline{T}"/> instances.
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public class AsyncPipelineBuilder<TPayload> :
        IPipelineComponentHolderOrDone<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload>,
        ISettingsHolder<IAsyncPipeline<TPayload>>,
        IPipelineBuilder<IAsyncPipeline<TPayload>>
    {
        private readonly PipelineBuilderState _state;

        private AsyncPipelineBuilder()
        {
            _state = new PipelineBuilderState();
        }

        public static IPipelineComponentHolder<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> Initialize()
        {
            return new AsyncPipelineBuilder<TPayload>();
        }

        public IPipelineComponentHolderOrDone<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> WithComponent<TComponent>()
            where TComponent : IAsyncPipelineComponent<TPayload>
        {
            _state.AddComponent(typeof(TComponent));
            return this;
        }

        public IPipelineComponentHolderOrDone<IAsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> WithComponent(string componentName)
        {
            _state.AddComponent(componentName);
            return this;
        }

        public ISettingsHolder<IAsyncPipeline<TPayload>> WithComponentResolver(IPipelineComponentResolver componentResolver)
        {
            _state.ComponentResolver = componentResolver;
            return this;
        }

        public IPipelineBuilder<IAsyncPipeline<TPayload>> WithSettings(IDictionary<string, IDictionary<string, string>> settings)
        {
            _state.Settings = settings;
            return this;
        }

        public IPipelineBuilder<IAsyncPipeline<TPayload>> WithNoSettings()
        {
            _state.UseDefaultSettings();
            return this;
        }

        public IAsyncPipeline<TPayload> Build()
        {
            return new AsyncPipeline<TPayload>(
                _state.ComponentResolver,
                _state.ComponentNames,
                _state.Settings);
        }
    }
}
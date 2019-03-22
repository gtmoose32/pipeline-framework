using PipelineFramework.Abstractions;
using PipelineFramework.Builder.Interfaces;
using System.Collections.Generic;

namespace PipelineFramework.Builder
{
    /// <summary>
    /// Builder implementation to assist in creating <see cref="IPipeline{T}"/> instances
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    internal class NonAsyncPipelineBuilder<TPayload> :
        IAdditionalPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload>,
        ISettingsHolder<IPipeline<TPayload>>,
        IPipelineBuilder<IPipeline<TPayload>>
    {
        private readonly PipelineBuilderState _state;

        private NonAsyncPipelineBuilder()
        {
            _state = new PipelineBuilderState();
        }

        public static IInitialPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> Initialize()
        {
            return new NonAsyncPipelineBuilder<TPayload>();
        }

        public IAdditionalPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> WithComponent<TComponent>()
            where TComponent : IPipelineComponent<TPayload>
        {
            _state.AddComponent(typeof(TComponent));
            return this;
        }

        public IAdditionalPipelineComponentHolder<IPipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> WithComponent(string componentName)
        {
            _state.AddComponent(componentName);
            return this;
        }

        public ISettingsHolder<IPipeline<TPayload>> WithComponentResolver(IPipelineComponentResolver componentResolver)
        {
            _state.ComponentResolver = componentResolver;
            return this;
        }

        public IPipelineBuilder<IPipeline<TPayload>> WithSettings(IDictionary<string, IDictionary<string, string>> settings)
        {
            _state.Settings = settings;
            return this;
        }

        public IPipelineBuilder<IPipeline<TPayload>> WithNoSettings()
        {
            _state.UseDefaultSettings();
            return this;
        }

        public IPipeline<TPayload> Build()
        {
            return new Pipeline<TPayload>(
                    _state.ComponentResolver,
                    _state.ComponentNames,
                    _state.Settings);
        }
    }
}
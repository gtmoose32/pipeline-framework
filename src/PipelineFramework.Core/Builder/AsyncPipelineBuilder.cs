using PipelineFramework.Abstractions;
using System.Collections.Generic;
using PipelineFramework.Builder.Interfaces;

namespace PipelineFramework.Builder
{
    public class AsyncPipelineBuilder<TPayload> :
        IPipelineComponentHolderOrDone<AsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload>,
        IPipelineComponentNameHolderOrDone<AsyncPipeline<TPayload>, TPayload>, 
        ISettingsHolder<AsyncPipeline<TPayload>, TPayload>,
        IPipelineBuilder<AsyncPipeline<TPayload>, TPayload>
    {
        private readonly PipelineBuilderState _state;

        private AsyncPipelineBuilder(bool useComponentTypes)
        {
            _state = new PipelineBuilderState(useComponentTypes);
        }

        public static IPipelineComponentHolder<AsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> UsingComponentTypes()
        {
            return new AsyncPipelineBuilder<TPayload>(true);
        }

        public static IPipelineComponentNameHolder<AsyncPipeline<TPayload>, TPayload> UsingComponentNames()
        {
            return new AsyncPipelineBuilder<TPayload>(false);
        }

        public IPipelineComponentHolderOrDone<AsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload> WithComponent<TComponent>()
            where TComponent : IAsyncPipelineComponent<TPayload>
        {
            _state.AddComponent(typeof(TComponent));
            return this;
        }

        public IPipelineComponentNameHolderOrDone<AsyncPipeline<TPayload>, TPayload> WithComponentName(string name)
        {
            _state.AddComponent(name);
            return this;
        }

        public ISettingsHolder<AsyncPipeline<TPayload>, TPayload> WithComponentResolver(IPipelineComponentResolver componentResolver)
        {
            _state.ComponentResolver = componentResolver;
            return this;
        }

        public IPipelineBuilder<AsyncPipeline<TPayload>, TPayload> WithSettings(IDictionary<string, IDictionary<string, string>> settings)
        {
            _state.Settings = settings;
            return this;
        }

        public IPipelineBuilder<AsyncPipeline<TPayload>, TPayload> WithNoSettings()
        {
            _state.UseDefaultSettings();
            return this;
        }

        public AsyncPipeline<TPayload> Build()
        {
            return _state.UseComponentTypes
                ? new AsyncPipeline<TPayload>(
                    _state.ComponentResolver,
                    _state.ComponentTypes,
                    _state.Settings)
                : new AsyncPipeline<TPayload>(
                    _state.ComponentResolver,
                    _state.ComponentNames,
                    _state.Settings);
        }
    }
}
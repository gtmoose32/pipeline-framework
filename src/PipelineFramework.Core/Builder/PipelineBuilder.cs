using System.Collections.Generic;
using PipelineFramework.Abstractions;

namespace PipelineFramework.Builder
{
    public class PipelineBuilder<TPayload> :
        IPipelineComponentHolderOrDone<Pipeline<TPayload>, IPipelineComponent<TPayload>, TPayload>,
        IPipelineComponentNameHolderOrDone<Pipeline<TPayload>, TPayload>,
        ISettingsHolder<Pipeline<TPayload>, TPayload>,
        IPipelineBuilder<Pipeline<TPayload>, TPayload>
    {
        private readonly PipelineBuilderState _state;

        private PipelineBuilder(bool useComponentTypes)
        {
            _state = new PipelineBuilderState(useComponentTypes);
        }

        public static IPipelineComponentHolder<Pipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> UsingComponentTypes()
        {
            return new PipelineBuilder<TPayload>(true);
        }

        public static IPipelineComponentNameHolder<Pipeline<TPayload>, TPayload> UsingComponentNames()
        {
            return new PipelineBuilder<TPayload>(false);
        }

        public IPipelineComponentHolderOrDone<Pipeline<TPayload>, IPipelineComponent<TPayload>, TPayload> WithComponent<TComponent>()
            where TComponent : IPipelineComponent<TPayload>
        {
            _state.AddComponent(typeof(TComponent));
            return this;
        }

        public IPipelineComponentNameHolderOrDone<Pipeline<TPayload>, TPayload> WithComponentName(string name)
        {
            _state.AddComponent(name);
            return this;
        }

        public ISettingsHolder<Pipeline<TPayload>, TPayload> WithComponentResolver(IPipelineComponentResolver componentResolver)
        {
            _state.ComponentResolver = componentResolver;
            return this;
        }

        public IPipelineBuilder<Pipeline<TPayload>, TPayload> WithSettings(IDictionary<string, IDictionary<string, string>> settings)
        {
            _state.Settings = settings;
            return this;
        }

        public IPipelineBuilder<Pipeline<TPayload>, TPayload> WithNoSettings()
        {
            _state.UseDefaultSettings();
            return this;
        }

        public Pipeline<TPayload> Build()
        {
            return _state.UseComponentTypes
                ? new Pipeline<TPayload>(
                    _state.ComponentResolver,
                    _state.ComponentTypes,
                    _state.Settings)
                : new Pipeline<TPayload>(
                    _state.ComponentResolver,
                    _state.ComponentNames,
                    _state.Settings);
        }
    }
}
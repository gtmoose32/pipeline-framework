using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;

namespace PipelineFramework.Builder
{
    public class AsyncPipelineBuilder<TPayload> :
        IPipelineComponentHolderOrDone<AsyncPipeline<TPayload>, IAsyncPipelineComponent<TPayload>, TPayload>,
        IPipelineComponentNameHolderOrDone<AsyncPipeline<TPayload>, TPayload>, 
        ISettingsHolder<AsyncPipeline<TPayload>, TPayload>,
        IPipelineBuilder<AsyncPipeline<TPayload>, TPayload>
    {
        private readonly List<Type> _componentTypes = new List<Type>();
        private readonly List<string> _componentNames = new List<string>();
        private IPipelineComponentResolver _componentResolver;
        private IDictionary<string, IDictionary<string, string>> _settings;
        private readonly bool _useComponentTypes;

        private AsyncPipelineBuilder(bool useComponentTypes)
        {
            _useComponentTypes = useComponentTypes;
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
            _componentTypes.Add(typeof(TComponent));
            return this;
        }

        public IPipelineComponentNameHolderOrDone<AsyncPipeline<TPayload>, TPayload> WithComponentName(string name)
        {
            _componentNames.Add(name);
            return this;
        }

        public ISettingsHolder<AsyncPipeline<TPayload>, TPayload> WithComponentResolver(IPipelineComponentResolver componentResolver)
        {
            _componentResolver = componentResolver;
            return this;
        }

        public IPipelineBuilder<AsyncPipeline<TPayload>, TPayload> WithSettings(IDictionary<string, IDictionary<string, string>> settings)
        {
            _settings = settings;
            return this;
        }

        public IPipelineBuilder<AsyncPipeline<TPayload>, TPayload> WithNoSettings()
        {
            _settings = new Dictionary<string, IDictionary<string, string>>();
            return this;
        }

        public AsyncPipeline<TPayload> Build()
        {
            return _useComponentTypes
                ? new AsyncPipeline<TPayload>(
                    _componentResolver,
                    _componentTypes,
                    _settings)
                : new AsyncPipeline<TPayload>(
                    _componentResolver,
                    _componentNames,
                    _settings);
        }
    }
}
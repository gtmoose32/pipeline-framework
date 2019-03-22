using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;

namespace PipelineFramework.Builder
{
    internal class PipelineBuilderState
    {
        private readonly List<Type> _componentTypes = new List<Type>();
        public IEnumerable<Type> ComponentTypes => _componentTypes;

        private readonly List<string> _componentNames = new List<string>();
        public IEnumerable<string> ComponentNames => _componentNames;
        public IPipelineComponentResolver ComponentResolver { get; set; }
        public IDictionary<string, IDictionary<string, string>> Settings { get; set; }
        public bool UseComponentTypes { get; }

        public PipelineBuilderState(bool useComponentTypes)
        {
            UseComponentTypes = useComponentTypes;
        }

        public void AddComponent(Type componentType)
        {
            _componentTypes.Add(componentType);
        }

        public void AddComponent(string componentName)
        {
            _componentNames.Add(componentName);
        }

        public void UseDefaultSettings()
        {
            Settings = new Dictionary<string, IDictionary<string, string>>();
        }
    }
}
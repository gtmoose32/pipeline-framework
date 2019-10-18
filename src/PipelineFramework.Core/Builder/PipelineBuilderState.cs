using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;

namespace PipelineFramework.Builder
{
    internal class PipelineBuilderState
    {
        private readonly List<string> _componentNames = new List<string>();
        public IEnumerable<string> ComponentNames => _componentNames;
        public IPipelineComponentResolver ComponentResolver { get; set; }
        public IDictionary<string, IDictionary<string, string>> Settings { get; set; }

        public void AddComponent(Type componentType)
        {
            AddComponent(componentType.Name);
        }

        public void AddComponent(string componentName)
        {
            if (string.IsNullOrWhiteSpace(componentName)) throw new ArgumentNullException(nameof(componentName));

            if (_componentNames.Contains(componentName))
            {
                throw new ArgumentException($"A component with name {componentName} has already been registered.", nameof(componentName));
            }

            _componentNames.Add(componentName);
        }

        public bool UseNoSettings { get; set; } = false;
    }
}
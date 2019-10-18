using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System.Collections.Generic;

namespace PipelineFramework
{
    public class DictionaryPipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IDictionary<string, IPipelineComponent> _components;

        public DictionaryPipelineComponentResolver(IDictionary<string, IPipelineComponent> components)
        {
            _components = components;
        }

        public T GetInstance<T>(string name) where T : class, IPipelineComponent
        {
            if (!_components.ContainsKey(name))
                throw new PipelineComponentNotFoundException($"PipelineComponent named '{name}' could not be located.");

            return (T)_components[name];
        }
    }
}

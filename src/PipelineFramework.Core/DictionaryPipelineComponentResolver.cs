using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System.Collections.Generic;

namespace PipelineFramework
{
    /// <summary>
    /// A pipeline component resolver that uses an <see cref="IDictionary{TKey,TValue}"/> internally to store and resolve any pipeline component instance requests.
    /// </summary>
    public class DictionaryPipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IDictionary<string, IPipelineComponent> _components;

        /// <summary>
        /// Creates a new instance of <see cref="DictionaryPipelineComponentResolver"/>.
        /// </summary>
        /// <param name="components">The internal dictionary of components used to resolve any pipeline component instance requests.</param>
        public DictionaryPipelineComponentResolver(IDictionary<string, IPipelineComponent> components)
        {
            _components = components;
        }

        /// <inheritdoc />
        public T GetInstance<T>(string name) where T : class, IPipelineComponent
        {
            if (!_components.ContainsKey(name))
                throw new PipelineComponentNotFoundException($"PipelineComponent named '{name}' could not be located.");

            return (T)_components[name];
        }
    }
}

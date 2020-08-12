using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PipelineFramework.PipelineComponentResolvers
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
        public DictionaryPipelineComponentResolver()
        {
            _components = new ConcurrentDictionary<string, IPipelineComponent>();
        }

        /// <inheritdoc />
        public T GetInstance<T>(string name) where T : class, IPipelineComponent
        {
            if (!_components.ContainsKey(name))
                throw new PipelineComponentNotFoundException($"PipelineComponent named '{name}' could not be located.");

            return (T)_components[name];
        }

        /// <summary>
        /// Adds the specified params array of <see cref="IAsyncPipelineComponent{TPayload}"/> to the pipeline component resolver.
        /// </summary>
        /// <param name="components">Array of pipeline components to add to the resolver.</param>
        /// <typeparam name="TPayload">Type defining the payload of the pipeline components being added.</typeparam>
        public void AddAsync<TPayload>(params IAsyncPipelineComponent<TPayload>[] components)
            => AddRangeAsync(components);

        /// <summary>
        /// Adds the specified range of <see cref="IAsyncPipelineComponent{TPayload}"/> to the pipeline component resolver.
        /// </summary>
        /// <param name="components">List of pipeline components to add to the resolver.</param>
        /// <typeparam name="TPayload">Type defining the payload of the pipeline components being added.</typeparam>
        public void AddRangeAsync<TPayload>(IEnumerable<IAsyncPipelineComponent<TPayload>> components)
        {
            foreach (var component in components)
            {
                AddAsync(component);
            }
        }

        /// <summary>
        /// Adds the specified <see cref="IAsyncPipelineComponent{TPayload}"/> to the pipeline component resolver.
        /// /// </summary>
        /// <typeparam name="TPayload">Type defining the payload of the pipeline components being added.</typeparam>
        /// <param name="component">Pipeline component to be added to the resolver.</param>
        public void AddAsync<TPayload>(IAsyncPipelineComponent<TPayload> component)
            => AddInternal(component);

        /// <summary>
        /// Adds the specified params array of <see cref="IAsyncPipelineComponent{TPayload}"/> to the pipeline component resolver.
        /// </summary>
        /// <param name="components">Array of pipeline components to add to the resolver.</param>
        /// <typeparam name="TPayload">Type defining the payload of the pipeline components being added.</typeparam>
        public void Add<TPayload>(params IPipelineComponent<TPayload>[] components)
            => AddRange(components.ToArray());

        /// <summary>
        /// Adds the specified range of <see cref="IPipelineComponent{TPayload}"/> to the pipeline component resolver.
        /// </summary>
        /// <param name="components">List of pipeline components to add to the resolver.</param>
        /// <typeparam name="TPayload">Type defining the payload of the pipeline components being added.</typeparam>
        public void AddRange<TPayload>(IEnumerable<IPipelineComponent<TPayload>> components)
        {
            foreach (var component in components)
            {
                Add(component);
            }
        }

        /// <summary>
        /// Adds the specified <see cref="IPipelineComponent{TPayload}"/> to the pipeline component resolver.
        /// /// </summary>
        /// <typeparam name="TPayload">Type defining the payload of the pipeline components being added.</typeparam>
        /// <param name="component">Pipeline component to be added to the resolver.</param>
        public void Add<TPayload>(IPipelineComponent<TPayload> component)
            => AddInternal(component);

        private void AddInternal(IPipelineComponent component)
        {
            var key = component.GetType().Name;
            if (_components.ContainsKey(key))
                throw new InvalidOperationException($"PipelineComponentResolver already contains an instance of type '{key}'");

            _components.Add(key, component);
        }
    }
}

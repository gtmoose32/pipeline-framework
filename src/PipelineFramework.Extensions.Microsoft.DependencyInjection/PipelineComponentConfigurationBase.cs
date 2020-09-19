using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// Provides pipeline configuration support for constructing pipelines using Microsoft dependency injection.
    /// </summary>
    public abstract class PipelineComponentConfigurationBase
    {
        private readonly List<Type> _components = new List<Type>();

        /// <summary>
        /// Dictionary of custom component factories by type used to create new instances of <see cref="IPipelineComponent"/>.
        /// </summary>
        public IDictionary<Type, Func<IServiceProvider, IPipelineComponent>> CustomComponentFactories { get; }
        
        /// <summary>
        /// Ordered list of <see cref="Type"/> used to construct pipelines.
        /// </summary>
        public IEnumerable<Type> Components => _components;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected PipelineComponentConfigurationBase()
        {
            CustomComponentFactories = new Dictionary<Type, Func<IServiceProvider, IPipelineComponent>>();
        }

        /// <summary>
        /// Adds an <see cref="IPipelineComponent{T}"/> to the <see cref="IPipeline{T}"/> configuration.
        /// </summary>
        /// <typeparam name="TComponent">Type that implements <see cref="IPipelineComponent{T}"/></typeparam>
        protected void AddComponent<TComponent>() where TComponent : class
        {
            _components.Add(typeof(TComponent));
        }

        /// <summary>
        /// Adds an <see cref="IPipelineComponent{T}"/> to the <see cref="IPipeline{T}"/> configuration.
        /// </summary>
        /// <param name="componentFactory">Factory used to provide new instances of the specified type param.</param>
        /// <typeparam name="TComponent">Type that implements <see cref="IPipelineComponent{T}"/></typeparam>
        protected void AddComponent<TComponent>(Func<IServiceProvider, TComponent> componentFactory) 
            where TComponent : class, IPipelineComponent
        {
            var type = typeof(TComponent);
            _components.Add(type);
            CustomComponentFactories[type] = componentFactory;
        }
        
        /// <summary>
        /// Validates pipeline configuration.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Validate()
        {
            if (_components.Count == 0)
            {
                throw new InvalidOperationException("PipelineConfigurationException! At least one component must be initialized");
            }
        }
    }
}
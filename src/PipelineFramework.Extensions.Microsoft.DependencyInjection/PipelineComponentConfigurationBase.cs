using System;
using System.Collections.Generic;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    public abstract class PipelineComponentConfigurationBase
    {
        private readonly List<Type> _components = new List<Type>();

        public IDictionary<Type, Func<IServiceProvider, object>> CustomRegistrations { get; }
        public IEnumerable<Type> Components => _components;

        protected PipelineComponentConfigurationBase()
        {
            CustomRegistrations = new Dictionary<Type, Func<IServiceProvider, object>>();
        }

        protected void AddComponent<TComponent>() where TComponent : class
        {
            _components.Add(typeof(TComponent));
        }

        protected void AddComponent<TComponent>(Func<IServiceProvider, TComponent> createInstance) where TComponent : class
        {
            var type = typeof(TComponent);
            _components.Add(type);
            CustomRegistrations.Add(type, createInstance);
        }
        
        public void Validate()
        {
            if (_components.Count == 0)
            {
                throw new InvalidOperationException("PipelineConfigurationException! At least one component must be initialized");
            }
        }
    }
}
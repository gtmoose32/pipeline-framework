using System;
using System.Collections.Generic;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    public abstract class PipelineComponentConfigurationBase
    {
        private readonly List<Type> _components = new List<Type>();
        public IEnumerable<Type> Components => _components;

        protected void AddComponent<TComponent>() where TComponent : class
        {
            _components.Add(typeof(TComponent));
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
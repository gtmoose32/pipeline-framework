using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PipelineFramework.Abstractions;

namespace PipelineFramework.Core.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class Resolver : IPipelineComponentResolver
    {
        private readonly IDictionary<string, IPipelineComponent> _components;
        public Resolver(IDictionary<string, IPipelineComponent> components)
        {
            _components = components;
        }

        public T GetInstance<T>(string name) where T : IPipelineComponent
        {
            return (T)_components[name];
        }
    }
}
using Autofac;
using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Autofac
{
    public class PipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IComponentContext _componentContext;

        public PipelineComponentResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
        }

        public T GetInstance<T>(string name) where T : IPipelineComponent
        {
            return _componentContext.ResolveNamed<T>(name);
        }
    }
}

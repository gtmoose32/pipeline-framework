using Autofac;
using PipelineFramework.Abstractions;

namespace PipelineFramework.Autofac
{
    public class PipelineComponentResolver : IPipelineComponentResolver
    {
        private readonly IComponentContext _componentContext;

        public PipelineComponentResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public T GetInstance<T>(string name) where T : IPipelineComponent
        {
            return _componentContext.ResolveNamed<T>(name);
        }
    }
}

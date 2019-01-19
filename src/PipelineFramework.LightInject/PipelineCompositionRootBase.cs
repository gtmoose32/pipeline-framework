using LightInject;
using PipelineFramework.Abstractions;

namespace PipelineFramework.LightInject
{
    public abstract class PipelineCompositionRootBase : ICompositionRoot
    {
        public virtual void Compose(IServiceRegistry registry)
        {
            var container = registry as IServiceFactory;

            registry.Register<IPipelineComponentResolver>(factory =>
                new PipelineComponentResolver(container), new PerContainerLifetime());
        }
    }
}

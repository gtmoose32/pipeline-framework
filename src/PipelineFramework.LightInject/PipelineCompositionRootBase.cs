using LightInject;
using PipelineFramework.Abstractions;

namespace PipelineFramework.LightInject
{
    public abstract class PipelineCompositionRootBase : ICompositionRoot
    {
        public virtual void Compose(IServiceRegistry registry)
        {
            registry.Register<IPipelineComponentResolver>(factory =>
                new PipelineComponentResolver(registry as IServiceContainer), new PerContainerLifetime());
        }
    }
}

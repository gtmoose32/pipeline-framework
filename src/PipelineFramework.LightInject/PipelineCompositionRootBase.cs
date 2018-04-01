using LightInject;
using PipelineFramework.Abstractions;

namespace PipelineFramework.LightInject
{
    public abstract class PipelineCompositionRootBase : ICompositionRoot
    {
        public virtual void Compose(IServiceRegistry registry)
        {
            var container = registry as IServiceFactory;

            registry
                .RegisterInstance(new LightInjectPipelineFactoryExecutor(container))
                .Register<IPipelineFactoryExecutor>(factory => container.GetInstance<LightInjectPipelineFactoryExecutor>())
                .Register<IAsyncPipelineFactoryExecutor>(factory => container.GetInstance<LightInjectPipelineFactoryExecutor>())
                .Register<IPipelineComponentResolver>(factory =>
                    new PipelineComponentResolver(container), new PerContainerLifetime());
        }
    }
}

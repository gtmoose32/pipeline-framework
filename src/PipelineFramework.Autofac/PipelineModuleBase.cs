using Autofac;
using PipelineFramework.Abstractions;

namespace PipelineFramework.Autofac
{
    public abstract class PipelineModuleBase : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(context => new PipelineFactoryExecutor(context)).InstancePerLifetimeScope();
            builder.Register(context => context.Resolve<PipelineFactoryExecutor>()).As<IPipelineFactoryExecutor>();
            builder.Register(context => context.Resolve<PipelineFactoryExecutor>()).As<IAsyncPipelineFactoryExecutor>();
            builder.RegisterType<PipelineComponentResolver>().As<IPipelineComponentResolver>().InstancePerLifetimeScope();
        }
    }
}

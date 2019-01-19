using Autofac;
using PipelineFramework.Abstractions;

namespace PipelineFramework.Autofac
{
    public abstract class PipelineModuleBase : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PipelineComponentResolver>().As<IPipelineComponentResolver>().InstancePerLifetimeScope();
        }
    }
}

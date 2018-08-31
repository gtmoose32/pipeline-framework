using Autofac;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Autofac.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class TestPipelineModule : PipelineModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}

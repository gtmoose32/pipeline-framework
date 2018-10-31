using Autofac;
using PipelineFramework.Abstractions;
using PipelineFramework.Tests.SharedInfrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Autofac.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class TestPipelineModule : PipelineModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<FooComponent>().Named<IPipelineComponent<TestPayload>>(typeof(FooComponent).Name);
            builder.RegisterType<BarComponent>().Named<IPipelineComponent<TestPayload>>(typeof(BarComponent).Name);

            builder.RegisterType<FooComponent>().Named<IAsyncPipelineComponent<TestPayload>>(typeof(FooComponent).Name);
            builder.RegisterType<BarComponent>().Named<IAsyncPipelineComponent<TestPayload>>(typeof(BarComponent).Name);

            var list = new List<Type> {typeof(FooComponent), typeof(BarComponent)};
            builder.RegisterInstance(list).As<IEnumerable<Type>>();

            builder.RegisterInstance(new Dictionary<string, IDictionary<string, string>>())
                .As<IDictionary<string, IDictionary<string, string>>>();

            builder.Register(context => new Pipeline<TestPayload>(
                    context.Resolve<IPipelineComponentResolver>(), 
                    context.Resolve<IEnumerable<Type>>(), 
                    context.Resolve<IDictionary<string, IDictionary<string, string>>>()))
                .As<IPipeline<TestPayload>>();

            builder.Register(context => new AsyncPipeline<TestPayload>(
                    context.Resolve<IPipelineComponentResolver>(), 
                    context.Resolve<IEnumerable<Type>>(), 
                    context.Resolve<IDictionary<string, IDictionary<string, string>>>()))
                .As<IAsyncPipeline<TestPayload>>();
        }
    }
}

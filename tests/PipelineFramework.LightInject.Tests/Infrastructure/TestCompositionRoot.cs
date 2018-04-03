using LightInject;
using PipelineFramework.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.LightInject.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestCompositionRoot : PipelineCompositionRootBase
    {
        public override void Compose(IServiceRegistry registry)
        {
            base.Compose(registry);

            registry.Register<IPipelineComponent<TestPayload>, FooComponent>(typeof(FooComponent).Name);
            registry.Register<IPipelineComponent<TestPayload>, BarComponent>(typeof(BarComponent).Name);

            registry.Register<IEnumerable<Type>>(factory =>
                new List<Type> { typeof(FooComponent), typeof(BarComponent) });

            registry.Register<IDictionary<string, IDictionary<string, string>>>(
                factory => new Dictionary<string, IDictionary<string, string>>());

            registry.Register<IPipeline<TestPayload>>(
                factory => new Pipeline<TestPayload>(
                    factory.GetInstance<IPipelineComponentResolver>(),
                    factory.GetInstance<IEnumerable<Type>>(),
                    factory.GetInstance<IDictionary<string, IDictionary<string, string>>>()));
        }
    }
}

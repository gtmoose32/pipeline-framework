using PipelineFramework.Abstractions;
using PipelineFramework.Core.Tests.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class PipelineTestsBase
    {
        protected PipelineTestsBase()
        {
            var components = new Dictionary<string, IPipelineComponent>
            {
                {typeof(FooComponent).Name, new FooComponent()},
                {typeof(BarComponent).Name, new BarComponent()},
                {typeof(FooSettingNotFoundComponent).Name, new FooSettingNotFoundComponent() },
                {typeof(BarExceptionComponent).Name, new BarExceptionComponent() },
                {typeof(DelayComponent).Name, new DelayComponent() },
                {"Component1", new ConfigurableComponent()},
                {"Component2", new ConfigurableComponent()},
                {typeof(PipelineExecutionTerminatingComponent).Name, new PipelineExecutionTerminatingComponent()}
            };

            PipelineComponentResolver = new DictionaryPipelineComponentResolver(components);

        }

        protected IPipelineComponentResolver PipelineComponentResolver { get; }
    }
}

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
                {nameof(FooComponent), new FooComponent()},
                {nameof(BarComponent), new BarComponent()},
                {nameof(FooSettingNotFoundComponent), new FooSettingNotFoundComponent() },
                {nameof(BarExceptionComponent), new BarExceptionComponent() },
                {nameof(DelayComponent), new DelayComponent() },
                {"Component1", new ConfigurableComponent()},
                {"Component2", new ConfigurableComponent()},
                {nameof(PipelineExecutionTerminatingComponent), new PipelineExecutionTerminatingComponent()}
            };

            PipelineComponentResolver = new DictionaryPipelineComponentResolver(components);

        }

        protected IPipelineComponentResolver PipelineComponentResolver { get; }
    }
}

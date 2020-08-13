using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class PipelineTestsBase
    {
        protected PipelineTestsBase()
        {
            //var components = new Dictionary<string, IPipelineComponent>
            //{
            //    {nameof(FooComponent), new FooComponent()},
            //    {nameof(BarComponent), new BarComponent()},
            //    {nameof(FooSettingNotFoundComponent), new FooSettingNotFoundComponent() },
            //    {nameof(BarExceptionComponent), new BarExceptionComponent() },
            //    {nameof(DelayComponent), new DelayComponent() },
            //    {nameof(ConfigurableComponent), new ConfigurableComponent()},
            //    {nameof(PipelineExecutionTerminatingComponent), new PipelineExecutionTerminatingComponent()}
            //};

            PipelineComponentResolver = new DictionaryPipelineComponentResolver();

        }

        protected DictionaryPipelineComponentResolver PipelineComponentResolver { get; }
    }
}

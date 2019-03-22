using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Builder.Interfaces;
using PipelineFramework.Core.Tests.Infrastructure;

namespace PipelineFramework.Core.Tests
{
    [TestClass]
    public class PipelineBuilderTests
    {
        [TestMethod]
        public void TestAsync()
        {
            // Act/Assert
            PipelineBuilder<TestPayload>.Async()
                .Should()
                .BeAssignableTo<IInitialPipelineComponentHolder<IAsyncPipeline<TestPayload>, IAsyncPipelineComponent<TestPayload>, TestPayload>>();
        }

        [TestMethod]
        public void TestNonAsync()
        {
            // Act/Assert
            PipelineBuilder<TestPayload>.NonAsync()
                .Should()
                .BeAssignableTo<IInitialPipelineComponentHolder<IPipeline<TestPayload>, IPipelineComponent<TestPayload>, TestPayload>>();
        }
    }
}
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Builder;
using PipelineFramework.Core.Tests.Infrastructure;
using System.Collections.Generic;

namespace PipelineFramework.Core.Tests.Builder
{
    [TestClass]
    public class NonAsyncPipelineBuilderTests : PipelineTestsBase
    {
        [TestMethod]
        public void TestBuilderByComponentType()
        {
            // Arrange
            var pipeline = NonAsyncPipelineBuilder<TestPayload>
                .Initialize()
                .WithComponent<FooComponent>()
                .WithComponent<BarComponent>()
                .WithComponentResolver(PipelineComponentResolver)
                .WithNoSettings()
                .Build();

            var payload = new TestPayload();

            // Act
            payload.FooWasCalled.Should().BeFalse();
            payload.BarWasCalled.Should().BeFalse();
            var result = pipeline.Execute(payload);

            // Assert
            result.Count.Should().Be(2);
            result.Count.Should().Be(2);
            result.FooWasCalled.Should().BeTrue();
            result.BarWasCalled.Should().BeTrue();
        }

        [TestMethod]
        public void TestBuilderByComponentName()
        {
            // Arrange
            var pipeline = NonAsyncPipelineBuilder<TestPayload>
                .Initialize()
                .WithComponent("FooComponent")
                .WithComponent("BarComponent")
                .WithComponentResolver(PipelineComponentResolver)
                .WithNoSettings()
                .Build();

            var payload = new TestPayload();

            // Act
            payload.FooWasCalled.Should().BeFalse();
            payload.BarWasCalled.Should().BeFalse();
            var result = pipeline.Execute(payload);

            // Assert
            result.Count.Should().Be(2);
            result.FooWasCalled.Should().BeTrue();
            result.BarWasCalled.Should().BeTrue();
        }

        [TestMethod]
        public void TestBuilderMixAndMatchComponents()
        {
            // Arrange
            var pipeline = NonAsyncPipelineBuilder<TestPayload>
                .Initialize()
                .WithComponent<FooComponent>()
                .WithComponent("BarComponent")
                .WithComponentResolver(PipelineComponentResolver)
                .WithNoSettings()
                .Build();

            var payload = new TestPayload();

            // Act
            payload.FooWasCalled.Should().BeFalse();
            payload.BarWasCalled.Should().BeFalse();
            var result = pipeline.Execute(payload);

            // Assert
            result.Count.Should().Be(2);
            result.FooWasCalled.Should().BeTrue();
            result.BarWasCalled.Should().BeTrue();
        }

        [TestMethod]
        public void TestBuilderWithSettings()
        {
            // Arrange
            var pipeline = NonAsyncPipelineBuilder<TestPayload>
                .Initialize()
                .WithComponent("Component1")
                .WithComponent("Component2")
                .WithComponentResolver(PipelineComponentResolver)
                .WithSettings(new Dictionary<string, IDictionary<string, string>>
                {
                    {
                        "Component1", new Dictionary<string, string>
                        {
                            {"UseFoo", "true"},
                            {"TestValue", "MyFooTestValue"}
                        }
                    },
                    {
                        "Component2", new Dictionary<string, string>
                        {
                            {"UseFoo", "false"},
                            {"TestValue", "MyBarTestValue"}
                        }
                    }
                })
                .Build();

            var payload = new TestPayload();

            // Act
            payload.FooStatus.Should().BeNull();
            payload.BarStatus.Should().BeNull();
            var result = pipeline.Execute(payload);

            // Assert
            result.FooStatus.Should().Be("MyFooTestValue");
            result.BarStatus.Should().Be("MyBarTestValue");
        }
    }
}
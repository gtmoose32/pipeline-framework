using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Builder;
using PipelineFramework.Core.Tests.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Tests.Builder
{
    [TestClass]
    public class AsyncPipelineBuilderTests : PipelineTestsBase
    {
        [TestMethod]
        public async Task TestBuilderByComponentType()
        {
            // Arrange
            var pipeline = AsyncPipelineBuilder<TestPayload>
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
            var result = await pipeline.ExecuteAsync(payload);

            // Assert
            result.Count.Should().Be(2);
            result.Count.Should().Be(2);
            result.FooWasCalled.Should().BeTrue();
            result.BarWasCalled.Should().BeTrue();
        }

        [TestMethod]
        public async Task TestBuilderByComponentName()
        {
            // Arrange
            var pipeline = AsyncPipelineBuilder<TestPayload>
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
            var result = await pipeline.ExecuteAsync(payload);

            // Assert
            result.Count.Should().Be(2);
            result.FooWasCalled.Should().BeTrue();
            result.BarWasCalled.Should().BeTrue();
        }

        [TestMethod]
        public async Task TestBuilderMixAndMatchComponents()
        {
            // Arrange
            var pipeline = AsyncPipelineBuilder<TestPayload>
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
            var result = await pipeline.ExecuteAsync(payload);

            // Assert
            result.Count.Should().Be(2);
            result.FooWasCalled.Should().BeTrue();
            result.BarWasCalled.Should().BeTrue();
        }

        [TestMethod]
        public async Task TestBuilderWithSettings()
        {
            // Arrange
            var pipeline = AsyncPipelineBuilder<TestPayload>
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
            var result = await pipeline.ExecuteAsync(payload);

            // Assert
            result.FooStatus.Should().Be("MyFooTestValue");
            result.BarStatus.Should().Be("MyBarTestValue");
        }
    }
}
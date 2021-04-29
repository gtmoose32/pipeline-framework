using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Builder;
using PipelineFramework.TestInfrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Tests.Builder
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AsyncPipelineBuilderTests : PipelineTestsBase
    {
        [TestMethod]
        public async Task TestBuilderByComponentType()
        {
            // Arrange
            const string name = "test-name";
            PipelineComponentResolver.AddAsync(new FooComponent());
            PipelineComponentResolver.AddAsync(new BarComponent());

            var pipeline = AsyncPipelineBuilder<TestPayload>
                .Initialize(null)
                .WithComponent<FooComponent>()
                .WithComponent<BarComponent>()
                .WithComponentResolver(PipelineComponentResolver)
                .WithoutSettings()
                .Build(name);

            var payload = new TestPayload();

            // Act
            payload.FooWasCalled.Should().BeFalse();
            payload.BarWasCalled.Should().BeFalse();
            var result = await pipeline.ExecuteAsync(payload);

            // Assert
            pipeline.Name.Should().Be(name);
            result.Count.Should().Be(2);
            result.Count.Should().Be(2);
            result.FooWasCalled.Should().BeTrue();
            result.BarWasCalled.Should().BeTrue();
        }

        [TestMethod]
        public async Task TestBuilderWithSettings()
        {
            // Arrange
            PipelineComponentResolver.AddAsync(new ConfigurableComponent());

            var pipeline = AsyncPipelineBuilder<TestPayload>
                .Initialize(null)
                .WithComponent<ConfigurableComponent>()
                .WithComponentResolver(PipelineComponentResolver)
                .WithSettings(new Dictionary<string, IDictionary<string, string>>
                {
                    {
                        "ConfigurableComponent", new Dictionary<string, string>
                        {
                            {"UseFoo", "true"},
                            {"TestValue", "MyFooTestValue"}
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
            pipeline.Name.StartsWith("AsyncPipeline").Should().BeTrue();
            result.FooStatus.Should().Be("MyFooTestValue");
            payload.BarStatus.Should().BeNull();
        }
    }
}
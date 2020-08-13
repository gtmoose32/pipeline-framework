using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Builder;
using PipelineFramework.TestInfrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests.Builder
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class NonAsyncPipelineBuilderTests : PipelineTestsBase
    {
        [TestMethod]
        public void TestBuilderByComponentType()
        {
            // Arrange
            PipelineComponentResolver.Add(new FooComponent());
            PipelineComponentResolver.Add(new BarComponent());

            var pipeline = NonAsyncPipelineBuilder<TestPayload>
                .Initialize(null)
                .WithComponent<FooComponent>()
                .WithComponent<BarComponent>()
                .WithComponentResolver(PipelineComponentResolver)
                .WithoutSettings()
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
        public void TestBuilderWithSettings()
        {
            // Arrange
            PipelineComponentResolver.Add(new ConfigurableComponent());

            var pipeline = NonAsyncPipelineBuilder<TestPayload>
                .Initialize(null)
                .WithComponent<ConfigurableComponent>()
                .WithComponentResolver(PipelineComponentResolver)
                .WithSettings(new Dictionary<string, IDictionary<string, string>>
                {
                    {
                        "ConfigurableComponent", new Dictionary<string, string>
                        {
                            {"UseFoo", "false"},
                            {"TestValue", "MyBarTestValue"}
                        }
                    }
                })
                .Build();

            var payload = new TestPayload();

            // Act
            var result = pipeline.Execute(payload);

            // Assert
            payload.FooStatus.Should().BeNull();
            result.BarStatus.Should().Be("MyBarTestValue");
        }
    }
}
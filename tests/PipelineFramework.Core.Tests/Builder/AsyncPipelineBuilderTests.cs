﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                .UsingComponentTypes()
                .WithComponent<FooComponent>()
                .WithComponent<BarComponent>()
                .WithComponentResolver(PipelineComponentResolver)
                .WithNoSettings()
                .Build();

            var payload = new TestPayload();

            // Act
            Assert.IsFalse(payload.FooWasCalled);
            Assert.IsFalse(payload.BarWasCalled);
            var result = await pipeline.ExecuteAsync(payload);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.FooWasCalled);
            Assert.IsTrue(result.BarWasCalled);
        }

        [TestMethod]
        public async Task TestBuilderByComponentName()
        {
            // Arrange
            var pipeline = AsyncPipelineBuilder<TestPayload>
                .UsingComponentNames()
                .WithComponentName("FooComponent")
                .WithComponentName("BarComponent")
                .WithComponentResolver(PipelineComponentResolver)
                .WithNoSettings()
                .Build();

            var payload = new TestPayload();

            // Act
            Assert.IsFalse(payload.FooWasCalled);
            Assert.IsFalse(payload.BarWasCalled);
            var result = await pipeline.ExecuteAsync(payload);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.FooWasCalled);
            Assert.IsTrue(result.BarWasCalled);
        }

        [TestMethod]
        public async Task TestBuilderWithSettings()
        {
            // Arrange
            var pipeline = AsyncPipelineBuilder<TestPayload>
                .UsingComponentNames()
                .WithComponentName("Component1")
                .WithComponentName("Component2")
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
            Assert.IsNull(payload.FooStatus);
            Assert.IsNull(payload.BarStatus);
            var result = await pipeline.ExecuteAsync(payload);

            // Assert
            Assert.AreEqual("MyFooTestValue", result.FooStatus);
            Assert.AreEqual("MyBarTestValue", result.BarStatus);
        }
    }
}
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Builder;
using PipelineFramework.Core.Tests.Infrastructure;

namespace PipelineFramework.Core.Tests.Builder
{
    [TestClass]
    public class PipelineBuilderTests : PipelineTestsBase
    {
        [TestMethod]
        public void TestBuilderByComponentType()
        {
            // Arrange
            var pipeline = PipelineBuilder<TestPayload>
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
            var result = pipeline.Execute(payload);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.FooWasCalled);
            Assert.IsTrue(result.BarWasCalled);
        }

        [TestMethod]
        public void TestBuilderByComponentName()
        {
            // Arrange
            var pipeline = PipelineBuilder<TestPayload>
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
            var result = pipeline.Execute(payload);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.FooWasCalled);
            Assert.IsTrue(result.BarWasCalled);
        }

        [TestMethod]
        public void TestBuilderWithSettings()
        {
            // Arrange
            var pipeline = PipelineBuilder<TestPayload>
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
            var result = pipeline.Execute(payload);

            // Assert
            Assert.AreEqual("MyFooTestValue", result.FooStatus);
            Assert.AreEqual("MyBarTestValue", result.BarStatus);
        }
    }
}
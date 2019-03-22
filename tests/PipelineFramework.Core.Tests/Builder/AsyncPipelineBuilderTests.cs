using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Builder;
using PipelineFramework.Core.Tests.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipelineFramework.Core.Tests.Builder
{
    [TestClass]
    public class AsyncPipelineBuilderTests
    {
        private static Dictionary<string, IPipelineComponent> _components;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _components = new Dictionary<string, IPipelineComponent>
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
        }


        [TestMethod]
        public async Task TestBuilderByComponentType()
        {
            // Arrange
            var pipeline = AsyncPipelineBuilder<TestPayload>
                .UsingComponentTypes()
                .WithComponent<FooComponent>()
                .WithComponent<BarComponent>()
                .WithComponentResolver(new DictionaryPipelineComponentResolver(_components))
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
                .WithComponentResolver(new DictionaryPipelineComponentResolver(_components))
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
                .WithComponentResolver(new DictionaryPipelineComponentResolver(_components))
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
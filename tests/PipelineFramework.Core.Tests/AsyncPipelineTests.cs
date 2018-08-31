using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnitTestCommon;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AsyncPipelineTests : PipelineTestsBase
    {
        [TestMethod]
        public void AsyncPipelineComponentBase_Test()
        {
            const string name = "myname";
            IPipelineComponent target = new AsyncFooComponent();
            target.Initialize(name, new Dictionary<string, string> {{"test", "value"}});

            target.Name.Should().Be(name);
        }

        [TestMethod]
        public async Task AsyncPipeline_DuplicateComponentsConfiguredDifferently_Test()
        {
            var settings = new Dictionary<string, IDictionary<string, string>>
            {
                {"Component1", new Dictionary<string, string> {{"TestValue", "Component1Value"}, {"UseFoo", "true"}}},
                {"Component2", new Dictionary<string, string> {{"TestValue", "Component2Value"}, {"UseFoo", "false"}}}
            };

            var payload = new TestPayload();
            var target = new AsyncPipeline<TestPayload>(
                PipelineComponentResolver, new List<string> { "Component1", "Component2" }, settings);

            var result = await target.ExecuteAsync(payload);

            result.Should().NotBeNull();
            result.Should().Be(payload);
            result.FooStatus.Should().Be("Component1Value");
            payload.BarStatus.Should().Be("Component2Value");
        }

        [TestMethod]
        public void AsyncPipeline_Excecution_Cancellation_Test()
        {
            var types = new List<Type> { typeof(DelayComponent), typeof(BarComponent) };
            var config = new Dictionary<string, IDictionary<string, string>>();

            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config);
            Func<Task<TestPayload>> act = () => target.ExecuteAsync(new TestPayload(), cts.Token);

            act.Should().Throw<OperationCanceledException>();
        }

        [TestMethod]
        public async Task AsyncPipeline_Execution_Test()
        {
            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config);
            var result = await target.ExecuteAsync(new TestPayload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(FooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(BarComponent).Name} executed!");
        }

        [TestMethod]
        public async Task AsyncPipeline_Execution_NullSettings_Test()
        {
            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, null);
            var result = await target.ExecuteAsync(new TestPayload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(FooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(BarComponent).Name} executed!");
        }

        [TestMethod]
        public void AsyncPipelineComponent_Exception_Test()
        {
            var types = new List<Type> { typeof(FooComponent), typeof(BarExceptionComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });


            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config);
            Func<Task<TestPayload>> act = () => target.ExecuteAsync(new TestPayload());

            act.Should().ThrowExactly<PipelineExecutionException>()
                .And.InnerException.Should().BeOfType<NotImplementedException>();
        }

        [TestMethod]
        public void AsyncPipelineComponent_SettingNotFoundException_Test()
        {
            var types = new List<Type> { typeof(FooSettingNotFoundComponent) };
            var config = new Dictionary<string, IDictionary<string, string>>();

            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config);
            Func<Task<TestPayload>> act = () => target.ExecuteAsync(new TestPayload());

            act.Should().ThrowExactly<PipelineExecutionException>()
                .And.InnerException.Should().BeOfType<PipelineComponentSettingNotFoundException>();
        }

        [TestMethod]
        public async Task Pipeline_AsyncFilterExecution_Test()
        {
            var types = new List<Type>
            {
                typeof(FooComponent),
                typeof(PipelineExecutionTerminatingComponent),
                typeof(BarComponent)
            };

            var config = new Dictionary<string, IDictionary<string, string>>();
            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config);
            var result = await target.ExecuteAsync(new TestPayload());

            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{typeof(FooComponent).Name} executed!");
            result.BarStatus.Should().BeNull();
        }
    }
}

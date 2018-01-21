using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using PipelineFramework.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AsyncPipelineTests
    {
        private IPipelineComponentResolver _resolver;

        [TestInitialize]
        public void Init()
        {
            var components = new Dictionary<string, IPipelineComponent>
            {
                {typeof(AsyncFooComponent).Name, new AsyncFooComponent()},
                {typeof(AsyncBarComponent).Name, new AsyncBarComponent()},
                {typeof(AsyncFooSettingNotFoundComponent).Name, new AsyncFooSettingNotFoundComponent()},
                {typeof(AsyncBarExceptionComponent).Name, new AsyncBarExceptionComponent()},
                {typeof(AsyncDelayComponent).Name, new AsyncDelayComponent()},
                {typeof(AsyncFilteringComponent).Name, new AsyncFilteringComponent()},
                {"Component1", new AsyncConfigurableComponent() },
                {"Component2", new AsyncConfigurableComponent() }
            };

            _resolver = new Resolver(components);
        }

        [TestMethod]
        public void AsyncComponentNameProperty_Test()
        {
            const string name = "myname";
            IPipelineComponent target = new AsyncFooComponent();
            target.Initialize(name,new Dictionary<string, string>());

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

            var payload = new Payload();
            var target = new AsyncPipeline<Payload>(
                _resolver, new List<string> { "Component1", "Component2" }, settings);

            var result = await target.ExecuteAsync(payload);

            result.Should().NotBeNull();
            result.Should().Be(payload);
            result.FooStatus.Should().Be("Component1Value");
            payload.BarStatus.Should().Be("Component2Value");
        }

        [TestMethod]
        public void AsyncPipeline_Excecution_Cancellation_Test()
        {
            var types = new List<Type> { typeof(AsyncDelayComponent), typeof(AsyncBarComponent) };
            var config = new Dictionary<string, IDictionary<string, string>>();

            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

            var target = new AsyncPipeline<Payload>(_resolver, types, config);
            Func<Task<Payload>> act = () => target.ExecuteAsync(new Payload(), cts.Token);

            act.ShouldThrow<OperationCanceledException>();
        }

        [TestMethod]
        public async Task AsyncPipeline_Execution_Test()
        {
            var types = new List<Type> { typeof(AsyncFooComponent), typeof(AsyncBarComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new AsyncPipeline<Payload>(_resolver, types, config);
            var result = await target.ExecuteAsync(new Payload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(AsyncFooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(AsyncBarComponent).Name} executed!");
        }

        [TestMethod]
        public async Task AsyncPipeline_Execution_NullSettings_Test()
        {
            var types = new List<Type> { typeof(AsyncFooComponent), typeof(AsyncBarComponent) };

            var target = new AsyncPipeline<Payload>(_resolver, types, null);
            var result = await target.ExecuteAsync(new Payload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(AsyncFooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(AsyncBarComponent).Name} executed!");
        }

        [TestMethod]
        public void AsyncPipelineComponent_Exception_Test()
        {
            var types = new List<Type> { typeof(AsyncFooComponent), typeof(AsyncBarExceptionComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });


            var target = new AsyncPipeline<Payload>(_resolver, types, config);
            Func<Task<Payload>> act = () => target.ExecuteAsync(new Payload());

            act.ShouldThrow<PipelineExecutionException>().And.InnerException.Should()
                .BeOfType<NotImplementedException>();
        }

        [TestMethod]
        public void AsyncPipelineComponent_SettingNotFoundException_Test()
        {
            var types = new List<Type> { typeof(AsyncFooSettingNotFoundComponent) };
            var config = new Dictionary<string, IDictionary<string, string>>();

            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var target = new AsyncPipeline<Payload>(_resolver, types, config);
            Func<Task<Payload>> act = () => target.ExecuteAsync(new Payload());

            act.ShouldThrowExactly<PipelineExecutionException>()
                .And.InnerException.Should().BeOfType<PipelineComponentSettingNotFoundException>();
        }

        [TestMethod]
        public async Task Pipeline_AsyncFilterExecution_Test()
        {
            var types = new List<Type>
            {
                typeof(AsyncFooComponent),
                typeof(AsyncFilteringComponent),
                typeof(AsyncBarComponent)
            };

            var config = new Dictionary<string, IDictionary<string, string>>();
            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var target = new AsyncPipeline<Payload>(_resolver, types, config);
            var result = await target.ExecuteAsync(new Payload());

            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{typeof(AsyncFooComponent).Name} executed!");
            result.BarStatus.Should().BeNull();
        }
    }
}

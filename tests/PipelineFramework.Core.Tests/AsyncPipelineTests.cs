using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PipelineFramework.Abstractions;
using PipelineFramework.Builder;
using PipelineFramework.Core.Tests.Infrastructure;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable ObjectCreationAsStatement

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AsyncPipelineTests : PipelineTestsBase
    {
        [TestMethod]
        public void AsyncPipeline_NullResolver_Test()
        {
            Action act = () => new AsyncPipeline<TestPayload>(null, new List<Type>());

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void AsyncPipeline_NullTypeList_Test()
        {
            Action act = () => new AsyncPipeline<TestPayload>(
                new DictionaryPipelineComponentResolver(new Dictionary<string, IPipelineComponent>()),
                null as IEnumerable<Type>);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void AsyncPipelineComponent_Initialize_Test()
        {
            const string name = "myname";
            var target = new AsyncTestComponent();
            target.Initialize(name, new Dictionary<string, string> { { "test", "value" } });

            target.Name.Should().Be(name);
            target.TestSettings.Count.Should().Be(1);
        }

        [TestMethod]
        public void AsyncPipelineComponent_Initialize_NullSettings_Test()
        {
            const string name = "myname";
            var target = new AsyncTestComponent();
            target.Initialize(name, null);

            target.Name.Should().Be(name);
            target.TestSettings.Count.Should().Be(0);
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
        public void AsyncPipeline_Execution_Cancellation_Test()
        {
            var types = new List<Type> { typeof(DelayComponent), typeof(BarComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

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
            Assert.IsTrue(result.FooStatus == $"{nameof(FooComponent)} executed!");
            Assert.IsTrue(result.BarStatus == $"{nameof(BarComponent)} executed!");
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
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config);
            Func<Task<TestPayload>> act = () => target.ExecuteAsync(new TestPayload());

            act.Should().ThrowExactly<PipelineExecutionException>()
                .And.InnerException.Should().BeOfType<PipelineComponentSettingNotFoundException>();
        }

        [TestMethod]
        public async Task AsyncPipeline_TerminateExecution_Test()
        {
            var types = new List<Type>
            {
                typeof(FooComponent),
                typeof(PipelineExecutionTerminatingComponent),
                typeof(BarComponent)
            };

            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config);
            var result = await target.ExecuteAsync(new TestPayload());

            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{nameof(FooComponent)} executed!");
            result.BarStatus.Should().BeNull();
        }

        [TestMethod]
        public async Task AsyncPipeline_Dispose_Test()
        {
            //Arrange
            var component1 = Substitute.For<IDisposableAsyncPipelineComponent>();
            var component2 = Substitute.For<IAsyncPipelineComponent<TestPayload>>();
            var component3 = Substitute.For<IDisposableAsyncPipelineComponent>();

            var resolver = new DictionaryPipelineComponentResolver(
                new Dictionary<string, IPipelineComponent>
                {
                    {nameof(component1), component1},
                    {nameof(component2), component2},
                    {nameof(component3), component3}
                });

            var payload = new TestPayload();
            component1.ExecuteAsync(Arg.Any<TestPayload>(), Arg.Any<CancellationToken>()).Returns(payload);
            component2.ExecuteAsync(Arg.Any<TestPayload>(), Arg.Any<CancellationToken>()).Returns(payload);
            component3.ExecuteAsync(Arg.Any<TestPayload>(), Arg.Any<CancellationToken>()).Returns(payload);

            TestPayload result;

            //Act
            using (var sut = PipelineBuilder<TestPayload>.Async()
                .WithComponent(nameof(component1))
                .WithComponent(nameof(component2))
                .WithComponent(nameof(component3))
                .WithComponentResolver(resolver)
                .WithNoSettings()
                .Build())
            {
                result = await sut.ExecuteAsync(payload, CancellationToken.None).ConfigureAwait(false);
            }
            
            //Assert
            result.Should().NotBeNull();
            component1.Received().Dispose();
            component3.Received().Dispose();
        }
    }
}

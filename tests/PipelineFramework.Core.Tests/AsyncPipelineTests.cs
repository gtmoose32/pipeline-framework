using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PipelineFramework.Abstractions;
using PipelineFramework.Builder;
using PipelineFramework.Exceptions;
using PipelineFramework.PipelineComponentResolvers;
using PipelineFramework.TestInfrastructure;
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
            Action act = () => new AsyncPipeline<TestPayload>(null, new List<Type>(), null, null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void AsyncPipeline_NullTypeList_Test()
        {
            Action act = () => new AsyncPipeline<TestPayload>(
                new DictionaryPipelineComponentResolver(),
                null as IEnumerable<Type>, null, null);

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void AsyncPipelineComponent_Initialize_Test()
        {
            var target = new AsyncTestComponent();
            target.Initialize(new Dictionary<string, string> { { "test", "value" } });

            target.TestSettings.Count.Should().Be(1);
        }

        [TestMethod]
        public void AsyncPipelineComponent_Initialize_NullSettings_Test()
        {
            var target = new AsyncTestComponent();
            target.Initialize(null);

            target.TestSettings.Count.Should().Be(0);
        }

        [TestMethod]
        public void AsyncPipeline_Execution_Cancellation_Test()
        {
            //Arrange
            PipelineComponentResolver.AddAsync(new DelayComponent(), new BarComponent());

            var types = new List<Type> { typeof(DelayComponent), typeof(BarComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config, null);

            //Act
            Func<Task<TestPayload>> act = () => target.ExecuteAsync(new TestPayload(), cts.Token);

            //Assert
            act.Should().Throw<OperationCanceledException>();
        }

        [TestMethod]
        public async Task AsyncPipeline_Execution_Test()
        {
            //Arrange
            PipelineComponentResolver.AddAsync(new FooComponent(), new BarComponent());

            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config, null);

            //Act
            var result = await target.ExecuteAsync(new TestPayload());

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{nameof(FooComponent)} executed!");
            result.BarStatus.Should().Be($"{nameof(BarComponent)} executed!");
        }

        [TestMethod]
        public void AsyncPipeline_ExecutionStatusNotification_Test()
        {
            //Arrange
            PipelineComponentResolver.AddAsync(new FooComponent(), new BarExceptionComponent());
            var receiver = Substitute.For<IAsyncPipelineComponentExecutionStatusReceiver>();

            var sut = PipelineBuilder<TestPayload>.InitializeAsyncPipeline(receiver)
                .WithComponent<FooComponent>()
                .WithComponent<BarExceptionComponent>()
                .WithComponentResolver(PipelineComponentResolver)
                .WithoutSettings()
                .Build();

            //Act
            Func<Task> act = () => sut.ExecuteAsync(new TestPayload());

            //Assert
            act.Should()
                .ThrowExactly<PipelineExecutionException>()
                .WithInnerExceptionExactly<NotImplementedException>();

            receiver.Received(2)
                .ReceiveExecutionStartingAsync(Arg.Is<PipelineComponentExecutionStartingInfo>(info => 
                    info.PipelineComponentName == nameof(FooComponent) || 
                    info.PipelineComponentName == nameof(BarExceptionComponent)));

            receiver.Received()
                .ReceiveExecutionCompletedAsync(
                    Arg.Is<PipelineComponentExecutionCompletedInfo>(info => 
                        info.PipelineComponentName == nameof(FooComponent) && 
                        info.ExecutionTime != TimeSpan.Zero &&
                        info.Exception == null));

            receiver.Received()
                .ReceiveExecutionCompletedAsync(
                    Arg.Is<PipelineComponentExecutionCompletedInfo>(info => 
                        info.PipelineComponentName == nameof(BarExceptionComponent) && 
                        info.ExecutionTime != TimeSpan.Zero &&
                        info.Exception is NotImplementedException));
        }

        [TestMethod]
        public void AsyncPipelineComponent_Exception_Test()
        {
            //Arrange
            PipelineComponentResolver.AddAsync(new FooComponent(), new BarExceptionComponent());

            var types = new List<Type> { typeof(FooComponent), typeof(BarExceptionComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });


            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config, null);

            //Act
            Func<Task<TestPayload>> act = () => target.ExecuteAsync(new TestPayload());

            //Assert
            act.Should().ThrowExactly<PipelineExecutionException>()
                .And.InnerException.Should().BeOfType<NotImplementedException>();
        }

        [TestMethod]
        public void AsyncPipelineComponent_SettingNotFoundException_Test()
        {
            //Arrange
            PipelineComponentResolver.AddAsync(new FooSettingNotFoundComponent());
            
            var types = new List<Type> { typeof(FooSettingNotFoundComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config, null);

            //Act
            Func<Task<TestPayload>> act = () => target.ExecuteAsync(new TestPayload());

            //Assert
            act.Should().ThrowExactly<PipelineExecutionException>()
                .And.InnerException.Should().BeOfType<PipelineComponentSettingNotFoundException>();
        }

        [TestMethod]
        public async Task AsyncPipeline_TerminateExecution_Test()
        {
            //Arrange
            PipelineComponentResolver.AddAsync(new FooComponent(), new PipelineExecutionTerminatingComponent(), new BarComponent());

            var types = new List<Type>
            {
                typeof(FooComponent),
                typeof(PipelineExecutionTerminatingComponent),
                typeof(BarComponent)
            };

            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new AsyncPipeline<TestPayload>(PipelineComponentResolver, types, config, null);

            //Act
            var result = await target.ExecuteAsync(new TestPayload());

            //Assert
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

            var components = new[] { component1, component2};

            var resolver = new DictionaryPipelineComponentResolver();
            resolver.AddAsync(components);

            var payload = new TestPayload();
            component1.ExecuteAsync(Arg.Any<TestPayload>(), Arg.Any<CancellationToken>()).Returns(payload);
            component2.ExecuteAsync(Arg.Any<TestPayload>(), Arg.Any<CancellationToken>()).Returns(payload);

            TestPayload result;

            //Act
            using (var sut = new AsyncPipeline<TestPayload>(resolver, components.Select(c => c.GetType()), null, null))
            {
                result = await sut.ExecuteAsync(payload, CancellationToken.None).ConfigureAwait(false);
            }
            
            //Assert
            result.Should().NotBeNull();
            component1.Received().Dispose();
        }
    }
}

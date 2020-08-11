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

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineTests : PipelineTestsBase
    {
        [TestMethod]
        public void Pipeline_Execution_Cancellation_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new DelayComponent(), new BarComponent());
            
            var types = new List<Type> { typeof(DelayComponent), typeof(BarComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> {{"test", "value"}});

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config, null);
            
            //Act
            Action act = () => target.Execute(new TestPayload(), cts.Token);

            //Assert
            act.Should().Throw<OperationCanceledException>();
        }

        [TestMethod]
        public void Pipeline_Execution_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new FooComponent(), new BarComponent());

            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> {{"test", "value"}});

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config, null);

            //Act
            var result = target.Execute(new TestPayload());

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{nameof(FooComponent)} executed!");
            result.BarStatus.Should().Be($"{nameof(BarComponent)} executed!");
        }

        [TestMethod]
        public void Pipeline_Execution_ComponentNames_NullSettings_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new FooComponent(), new BarComponent());
            var types = new List<string> { nameof(FooComponent), nameof(BarComponent) };

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, null, null);

            //Act
            var result = target.Execute(new TestPayload());

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{nameof(FooComponent)} executed!");
            result.BarStatus.Should().Be($"{nameof(BarComponent)} executed!");
        }

        [TestMethod]
        public void Pipeline_Execution_ComponentNames_EmptySettings_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new FooComponent(), new BarComponent());
            var types = new List<string> { nameof(FooComponent), nameof(BarComponent) };
            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, new Dictionary<string, IDictionary<string, string>>(), null);
            
            //Act
            var result = target.Execute(new TestPayload());

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{nameof(FooComponent)} executed!");
            result.BarStatus.Should().Be($"{nameof(BarComponent)} executed!");
        }

        [TestMethod]
        public void Pipeline_Execution_EmptySettings_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new FooComponent(), new BarComponent());
            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };
            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, new Dictionary<string, IDictionary<string, string>>(), null);

            //Act
            var result = target.Execute(new TestPayload());

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{nameof(FooComponent)} executed!");
            result.BarStatus.Should().Be($"{nameof(BarComponent)} executed!");
        }

        [TestMethod]
        public void PipelineComponent_Exception_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new FooComponent(), new BarExceptionComponent());
            var types = new List<Type> { typeof(FooComponent), typeof(BarExceptionComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config, null);
            
            //Act
            Action act = () => target.Execute(new TestPayload());

            //Assert
            act.Should()
                .ThrowExactly<PipelineExecutionException>()
                .WithInnerExceptionExactly<NotImplementedException>();
        }

        [TestMethod]
        public void PipelineComponent_SettingNotFoundException_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new FooSettingNotFoundComponent());
            var types = new List<Type> { typeof(FooSettingNotFoundComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> {{"test", "value"}});

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config, null);
            
            //Act
            Action act = () => target.Execute(new TestPayload());
            
            //Assert
            act.Should().ThrowExactly<PipelineExecutionException>()
                .WithInnerExceptionExactly<PipelineComponentSettingNotFoundException>();
        }

        [TestMethod]
        public void Pipeline_TerminateExecution_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new FooComponent(), new PipelineExecutionTerminatingComponent(), new BarComponent());
            var types = new List<Type> { typeof(FooComponent), typeof(PipelineExecutionTerminatingComponent), typeof(BarComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> {{"test", "value"}});

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config, null);
            
            //Act
            var result = target.Execute(new TestPayload());

            //Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be($"{nameof(FooComponent)} executed!");
            result.BarStatus.Should().BeNull();
        }

        [TestMethod]
        public void Pipeline_ExecutionStatusNotification_Test()
        {
            //Arrange
            PipelineComponentResolver.Add(new FooComponent(), new BarExceptionComponent());

            var receiver = Substitute.For<IPipelineComponentExecutionStatusReceiver>();

            var sut = PipelineBuilder<TestPayload>.InitializePipeline(receiver)
                .WithComponent<FooComponent>()
                .WithComponent<BarExceptionComponent>()
                .WithComponentResolver(PipelineComponentResolver)
                .WithoutSettings()
                .Build();

            //Act
            Action act = () => sut.Execute(new TestPayload());

            //Assert
            act.Should()
                .ThrowExactly<PipelineExecutionException>()
                .WithInnerExceptionExactly<NotImplementedException>();

            receiver.Received(2)
                .ReceiveExecutionStarting(Arg.Is<PipelineComponentExecutionStartedInfo>(info => 
                    info.PipelineComponentName == nameof(FooComponent) || 
                    info.PipelineComponentName == nameof(BarExceptionComponent)));

            receiver.Received()
                .ReceiveExecutionCompleted(
                    Arg.Is<PipelineComponentExecutionCompletedInfo>(info => 
                        info.PipelineComponentName == nameof(FooComponent) && 
                        info.ExecutionTime != TimeSpan.Zero &&
                        info.Exception == null));

            receiver.Received()
                .ReceiveExecutionCompleted(
                    Arg.Is<PipelineComponentExecutionCompletedInfo>(info => 
                        info.PipelineComponentName == nameof(BarExceptionComponent) && 
                        info.ExecutionTime != TimeSpan.Zero &&
                        info.Exception is NotImplementedException));

        }
    }
}

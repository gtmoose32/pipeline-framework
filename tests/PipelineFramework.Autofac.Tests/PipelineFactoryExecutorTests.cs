using System;
using Autofac;
using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PipelineFramework.Abstractions;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using PipelineFramework.Autofac.Tests.Infrastructure;

namespace PipelineFramework.Autofac.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineFactoryExecutorTests
    {
        private readonly PipelineFactoryExecutor _target;

        public PipelineFactoryExecutorTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestPipelineModule>();

            _target = new PipelineFactoryExecutor(builder.Build());
        }

        [TestMethod]
        public void CreatePipelineAndExecute()
        {
            //Act
            var result = _target.CreatePipelineAndExecute(new TestPayload(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.BarStatus.Should().Be(GetStatusCompareString(typeof(BarComponent).Name));
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be(GetStatusCompareString(typeof(FooComponent).Name));
        }

        [TestMethod]
        public async Task CreateAsyncPipelineAndExecuteAsync()
        {
            //Act
            var result = await _target.CreateAsyncPipelineAndExecuteAsync(new TestPayload(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.BarStatus.Should().Be(GetStatusCompareString(typeof(BarComponent).Name));
            result.Count.Should().Be(2);
            result.FooStatus.Should().Be(GetStatusCompareString(typeof(FooComponent).Name));
        }

        private static string GetStatusCompareString(string name)
        {
            return $"{name} executed!";
        }
    }
}

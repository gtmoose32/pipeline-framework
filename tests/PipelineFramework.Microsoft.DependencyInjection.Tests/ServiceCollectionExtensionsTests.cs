using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Extensions.Microsoft.DependencyInjection;
using PipelineFramework.TestInfrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PipelineFramework.Microsoft.DependencyInjection.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        private IServiceCollection _services;

        [TestInitialize]
        public void Init()
        {
            _services = new ServiceCollection();
        }

        [TestMethod]
        public void AddAsyncPipelineComponentExecutionStatusReceiver_Test()
        {
            //Arrange
            _services.AddAsyncPipelineComponentExecutionStatusReceiver<TestExecutionStatusReceiver>();
            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetService<IAsyncPipelineComponentExecutionStatusReceiver>();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TestExecutionStatusReceiver>();
        }

        [TestMethod]
        public void AddPipelineComponentExecutionStatusReceiver_Test()
        {
            //Arrange
            _services.AddPipelineComponentExecutionStatusReceiver<TestExecutionStatusReceiver>();
            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetService<IPipelineComponentExecutionStatusReceiver>();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TestExecutionStatusReceiver>();
        }

        [TestMethod]
        public void AddPipelineComponentResolver_Test()
        {
            //Arrange
            _services.AddPipelineComponentResolver();
            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetService<IPipelineComponentResolver>();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ServiceProviderPipelineComponentResolver>();
        }

        [TestMethod]
        public void AddAsyncPipelineComponent_Test()
        {
            //Arrange
            _services.AddAsyncPipelineComponent<FooComponent, TestPayload>();
            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetService<IAsyncPipelineComponent<TestPayload>>();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<FooComponent>();
        }

        [TestMethod]
        public void AddPipelineComponent_Test()
        {
            //Arrange
            _services.AddPipelineComponent<FooComponent, TestPayload>();
            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetService<IPipelineComponent<TestPayload>>();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<FooComponent>();
        }

        [TestMethod]
        public void AddAsyncPipelineComponentsFromAssembly_Test()
        {
            //Arrange
            _services.AddAsyncPipelineComponentsFromAssembly(typeof(TestPayload).Assembly);
            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetService<IEnumerable<IAsyncPipelineComponent<TestPayload>>>().ToArray();

            //Assert
            result.Should().NotBeNull();
            result.Length.Should().Be(8);
        }

        [TestMethod]
        public void AddPipelineComponentsFromAssembly_Test()
        {
            //Arrange
            _services.AddPipelineComponentsFromAssembly(typeof(TestPayload).Assembly);
            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetService<IEnumerable<IPipelineComponent<TestPayload>>>().ToArray();

            //Assert
            result.Should().NotBeNull();
            result.Length.Should().Be(7); 
        }
    }
}
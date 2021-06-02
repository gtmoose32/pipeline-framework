using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Extensions.Microsoft.DependencyInjection;
using PipelineFramework.TestInfrastructure;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Microsoft.DependencyInjection.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ServiceProviderExtensionsTests
    {
        private IServiceCollection _services;

        [TestInitialize]
        public void Init()
        {
            _services = new ServiceCollection();
        }

        [TestMethod]
        public void GetAsyncPipeline_Test()
        {
            //Arrange 
            const string name = "test-name";
            AddAsyncPipelineToContainer(_services, name);

            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetAsyncPipeline<TestPayload>(name);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(name);
        }

        [TestMethod]
        public void GetAsyncPipeline_NameParamIsNull_Test()
        {
            //Arrange 
            var sut = _services.BuildServiceProvider();

            //Act
            Action act = () => sut.GetAsyncPipeline<TestPayload>(null);

            //Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void GetAsyncPipeline_ServicesParamIsNull_Test()
        {
            //Arrange 
            IServiceProvider sut = null;

            //Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var result = sut.GetAsyncPipeline<TestPayload>(null);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetPipeline_Test()
        {
            //Arrange 
            const string name = "test-name";
            AddPipelineToContainer(_services, name);

            var sut = _services.BuildServiceProvider();

            //Act
            var result = sut.GetPipeline<TestPayload>(name);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(name);
        }

        [TestMethod]
        public void GetPipeline_NameParamIsNull_Test()
        {
            //Arrange 
            var sut = _services.BuildServiceProvider();

            //Act
            Action act = () => sut.GetPipeline<TestPayload>(null);

            //Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void GetPipeline_ServicesParamIsNull_Test()
        {
            //Arrange 
            IServiceProvider sut = null;

            //Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var result = sut.GetPipeline<TestPayload>(null);

            //Assert
            result.Should().BeNull();
        }

        private static void AddAsyncPipelineToContainer(
            IServiceCollection services,
            string pipelineName = null)
        {
            services
                .AddPipelineFramework()
                .AddAsyncPipeline<TestPayload, TestExecutionStatusReceiver>(
                    cfg => cfg
                        .WithComponent<FooComponent>()
                        .WithComponent<DelayComponent>()
                        .WithComponent<BarComponent>(),
                    pipelineName: pipelineName);
        }

        private static void AddPipelineToContainer(
            IServiceCollection services,
            string pipelineName = null)
        {
            services
                .AddPipelineFramework()
                .AddPipeline<TestPayload, TestExecutionStatusReceiver>(
                    cfg => cfg
                        .WithComponent<FooComponent>()
                        .WithComponent<DelayComponent>()
                        .WithComponent<BarComponent>(),
                    pipelineName: pipelineName);
        }
    }
}
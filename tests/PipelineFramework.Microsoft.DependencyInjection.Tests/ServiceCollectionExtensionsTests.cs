using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Extensions.Microsoft.DependencyInjection;
using PipelineFramework.TestInfrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        [TestMethod]
        public void AddAsyncPipeline()
        {
            // Arrange
            _services.AddPipelineFramework();
            _services.AddAsyncPipeline<TestPayload, TestExecutionStatusReceiver>(
                cfg => cfg
                    .WithComponent<FooComponent>()
                    .WithComponent<BarComponent>());

            var sut = _services.BuildServiceProvider();

            // Act and assert
            sut.GetService<IPipelineComponentResolver>()
                .Should().NotBeNull()
                .And.BeOfType<ServiceProviderPipelineComponentResolver>();

            sut.GetService<IAsyncPipeline<TestPayload>>()
                .Should().NotBeNull();

            sut.GetServices<IAsyncPipelineComponent<TestPayload>>()
                .Should().HaveCount(2);

            sut.GetService<IAsyncPipelineComponentExecutionStatusReceiver>()
                .Should().NotBeNull()
                .And.BeOfType<TestExecutionStatusReceiver>();
        }

        [TestMethod]
        public void AddPipeline()
        {
            // Arrange
            _services
                .AddPipelineFramework()
                .AddPipeline<TestPayload, TestExecutionStatusReceiver>(
                    cfg => cfg
                        .WithComponent<FooComponent>()
                        .WithComponent<BarComponent>());

            var sut = _services.BuildServiceProvider();

            // Act and assert
            sut.GetService<IPipelineComponentResolver>()
                .Should().NotBeNull()
                .And.BeOfType<ServiceProviderPipelineComponentResolver>();

            sut.GetService<IPipeline<TestPayload>>()
                .Should().NotBeNull();

            sut.GetServices<IPipelineComponent<TestPayload>>()
                .Should().HaveCount(2);

            sut.GetService<IPipelineComponentExecutionStatusReceiver>()
                .Should().NotBeNull()
                .And.BeOfType<TestExecutionStatusReceiver>();
        }

        [TestMethod]
        public void AsyncPipeline_SingletonTest_MultipleScopes_ReturnsSameInstance()
        {
            // Arrange
            RegisterDefaultPipeline(ServiceLifetime.Singleton);
            var sut = _services.BuildServiceProvider();

            // Act
            IAsyncPipeline<TestPayload> pipeline1;
            IAsyncPipeline<TestPayload> pipeline2;

            using (var scope = sut.CreateScope())
            {
                pipeline1 = scope.ServiceProvider.GetRequiredService<IAsyncPipeline<TestPayload>>();
            }

            using (var scope = sut.CreateScope())
            {
                pipeline2 = scope.ServiceProvider.GetRequiredService<IAsyncPipeline<TestPayload>>();
            }

            // Assert
            pipeline1.Should().BeSameAs(pipeline2);
        }

        [TestMethod]
        public void AsyncPipeline_ScopedTest_MultipleScopes_ReturnsDifferentInstance()
        {
            // Arrange
            RegisterDefaultPipeline(ServiceLifetime.Scoped);
            var sut = _services.BuildServiceProvider();

            // Act
            IAsyncPipeline<TestPayload> pipeline1;
            IAsyncPipeline<TestPayload> pipeline2;

            using (var scope = sut.CreateScope())
            {
                pipeline1 = scope.ServiceProvider.GetRequiredService<IAsyncPipeline<TestPayload>>();
            }

            using (var scope = sut.CreateScope())
            {
                pipeline2 = scope.ServiceProvider.GetRequiredService<IAsyncPipeline<TestPayload>>();
            }

            // Assert
            pipeline1.Should().NotBeSameAs(pipeline2);
        }

        [TestMethod]
        public void AsyncPipeline_ScopedTest_SameScope_ReturnsSameInstance()
        {
            // Arrange
            RegisterDefaultPipeline(ServiceLifetime.Scoped);
            var sut = _services.BuildServiceProvider();

            // Act
            IAsyncPipeline<TestPayload> pipeline1;
            IAsyncPipeline<TestPayload> pipeline2;

            using (var scope = sut.CreateScope())
            {
                pipeline1 = scope.ServiceProvider.GetRequiredService<IAsyncPipeline<TestPayload>>();
                pipeline2 = scope.ServiceProvider.GetRequiredService<IAsyncPipeline<TestPayload>>();
            }

            // Assert
            pipeline1.Should().BeSameAs(pipeline2);
        }

        [TestMethod]
        public void AsyncPipeline_Transient_SameScope_ReturnsDifferentInstances()
        {
            // Arrange
            RegisterDefaultPipeline(ServiceLifetime.Transient);
            var sut = _services.BuildServiceProvider();

            // Act
            IAsyncPipeline<TestPayload> pipeline1;
            IAsyncPipeline<TestPayload> pipeline2;

            using (var scope = sut.CreateScope())
            {
                pipeline1 = scope.ServiceProvider.GetRequiredService<IAsyncPipeline<TestPayload>>();
                pipeline2 = scope.ServiceProvider.GetRequiredService<IAsyncPipeline<TestPayload>>();
            }

            // Assert
            pipeline1.Should().NotBeSameAs(pipeline2);
        }

        [TestMethod]
        public void AsyncPipeline_WithComponentFactory_Test()
        {
            // Arrange
            _services
                .AddPipelineFramework()
                .AddAsyncPipeline<TestPayload>(
                    cfg => cfg
                        .WithComponent<FooComponent>()
                        .WithComponent<BarComponent>()
                        .WithComponent(provider => new ComponentFactoryTestComponent("I am foo!", "I am bar!")));

            var sut = _services.BuildServiceProvider();

            // Act
            var pipeline = sut.GetService<IAsyncPipeline<TestPayload>>();

            // Assert
            pipeline.Should().NotBeNull();
        }

        [TestMethod]
        public void Pipeline_WithComponentFactory_Test()
        {
            // Arrange
            _services
                .AddPipelineFramework()
                .AddPipeline<TestPayload>(
                    cfg => cfg
                        .WithComponent<FooComponent>()
                        .WithComponent<BarComponent>()
                        .WithComponent(provider => new ComponentFactoryTestComponent("I am foo!", "I am bar!")));

            var sut = _services.BuildServiceProvider();

            // Act
            var pipeline = sut.GetService<IPipeline<TestPayload>>();

            // Assert
            pipeline.Should().NotBeNull();
        }

        private void RegisterDefaultPipeline(ServiceLifetime lifetime)
        {
            _services
                .AddPipelineFramework()
                .AddAsyncPipeline<TestPayload>(
                    cfg => cfg
                        .WithComponent<FooComponent>()
                        .WithComponent<BarComponent>(), lifetime: lifetime);
        }
    }
    
    [ExcludeFromCodeCoverage]
    public class ComponentFactoryTestComponent : PipelineComponentBase<TestPayload>, IAsyncPipelineComponent<TestPayload>
    {
        private readonly string _fooStatus;
        private readonly string _barStatus;

        public ComponentFactoryTestComponent(string fooStatus, string barStatus)
        {
            _fooStatus = fooStatus;
            _barStatus = barStatus;
        }

        public override TestPayload Execute(TestPayload payload, CancellationToken cancellationToken)
        {
            payload.FooStatus = _fooStatus;
            payload.BarStatus = _barStatus;
            return payload;
        }

        public Task<TestPayload> ExecuteAsync(TestPayload payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(payload, cancellationToken));
        }
    }
}
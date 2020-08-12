using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using PipelineFramework.Extensions.Microsoft.DependencyInjection;
using PipelineFramework.TestInfrastructure;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Microsoft.DependencyInjection.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ServiceProviderPipelineComponentResolverTests
    {
        [TestMethod]
        public void GetInstance_Test()
        {
            //Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IAsyncPipelineComponent<TestPayload>, FooComponent>();
            services.AddSingleton<IAsyncPipelineComponent<TestPayload>, BarComponent>();

            var sut = new ServiceProviderPipelineComponentResolver(services.BuildServiceProvider());

            //Act
            var foo = sut.GetInstance<IAsyncPipelineComponent<TestPayload>>(nameof(FooComponent));
            var bar = sut.GetInstance<IAsyncPipelineComponent<TestPayload>>(nameof(BarComponent));

            //Assert
            foo.Should().NotBeNull();
            foo.Should().BeOfType<FooComponent>();

            bar.Should().NotBeNull();
            bar.Should().BeOfType<BarComponent>();
        }

        [TestMethod]
        public void GetInstance_ComponentNotFound_Test()
        {
            //Arrange
            var sut = new ServiceProviderPipelineComponentResolver(new ServiceCollection().BuildServiceProvider());
            
            //Act
            Action act = () => sut.GetInstance<IAsyncPipelineComponent<TestPayload>>(nameof(FooComponent));

            //Assert
            act.Should().ThrowExactly<PipelineComponentNotFoundException>();
        }
    }
}
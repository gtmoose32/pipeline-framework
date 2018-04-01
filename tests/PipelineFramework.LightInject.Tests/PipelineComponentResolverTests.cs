using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.LightInject.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineComponentResolverTests
    {
        private readonly IServiceContainer _container;

        public PipelineComponentResolverTests()
        {
            _container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            _container.Register<IPipelineComponent<TestPayload>, FooComponent>(typeof(FooComponent).Name);
        }

        [TestMethod]
        public void PipelineComponentResolver_CtorNullFactoryTest()
        {
            Action act  = () =>
            {
                // ReSharper disable once UnusedVariable
                var resolver = new PipelineComponentResolver(null);
            };

            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void PipelineComponentResolver_GetInstanceTest()
        {
            var target = new PipelineComponentResolver(_container);
            var result = target.GetInstance<IPipelineComponent<TestPayload>>(typeof(FooComponent).Name);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<FooComponent>();
        }
    }
}

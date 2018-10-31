using Autofac;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Tests.SharedInfrastructure;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Autofac.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineComponentResolverTests
    {
        private readonly IContainer _container;

        public PipelineComponentResolverTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FooComponent>().Named<IPipelineComponent<TestPayload>>(typeof(FooComponent).Name);
            _container = builder.Build();
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

using Autofac;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Autofac.Tests.Infrastructure;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Autofac.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineModuleTests
    {
        private readonly IContainer _container;

        public PipelineModuleTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestPipelineModule>();
            _container = builder.Build();
        }

        [TestMethod]
        public void PipelineModule_ResolvePipeline()
        {
            var result = _container.Resolve<IPipeline<TestPayload>>();

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Pipeline<TestPayload>>();
        }
    }
}
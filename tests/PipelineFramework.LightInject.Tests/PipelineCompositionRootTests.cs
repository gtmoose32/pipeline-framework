using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.LightInject.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineCompositionRootTests
    {
        private readonly IServiceContainer _container;

        public PipelineCompositionRootTests()
        {
            _container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
            //_container.Register<IPipelineComponent<TestPayload>, FooComponent>(typeof(FooComponent).Name);
            _container.RegisterAssembly(GetType().Assembly);
        }

        [TestMethod]
        public void PipelineCompositionRoot_GetPipeline()
        {
            var result = _container.GetInstance<IPipeline<TestPayload>>();

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Pipeline<TestPayload>>();
        }

        [TestMethod]
        public void PipelineCompositionRoot_GetPipelineFactoryExecutor()
        {
            var result = _container.GetInstance<IPipelineFactoryExecutor>();

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void PipelineCompositionRoot_GetAsyncPipelineFactoryExecutor()
        {
            var result = _container.GetInstance<IAsyncPipelineFactoryExecutor>();

            result.Should().NotBeNull();
        }
    }
}

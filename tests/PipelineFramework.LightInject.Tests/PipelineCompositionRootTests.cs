using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;

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
            _container.Register<IPipelineComponent<TestPayload>, FooComponent>(typeof(FooComponent).Name);
        }

        [TestMethod]
        public void PipelineCompositionRoot_GetPipeline()
        {
            _container.RegisterAssembly(GetType().Assembly);

            var result = _container.GetInstance<IPipeline<TestPayload>>();

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Pipeline<TestPayload>>();
        }

        [TestMethod]
        public void PipelineCompositionRoot_GetPipelineFactoryExecutor()
        {
            _container.RegisterAssembly(GetType().Assembly);

            var result = _container.GetInstance<IPipelineFactoryExecutor>();

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void PipelineCompositionRoot_GetAsyncPipelineFactoryExecutor()
        {
            _container.RegisterAssembly(GetType().Assembly);

            var result = _container.GetInstance<IAsyncPipelineFactoryExecutor>();

            result.Should().NotBeNull();
        }
    }
}

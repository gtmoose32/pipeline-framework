using AutoFixture;
using FluentAssertions;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PipelineFramework.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.LightInject.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class LightInjectPipelineFactoryExecutorTests
    {
        private readonly IFixture _fixture = new Fixture();
        private IServiceFactory _serviceFactory;
        private LightInjectPipelineFactoryExecutor _target;

        [TestInitialize]
        public void Init()
        {
            _serviceFactory = Substitute.For<IServiceFactory>();
            _target = new LightInjectPipelineFactoryExecutor(_serviceFactory);
        }

        [TestMethod]
        public void CreatePipelineAndExecute()
        {
            //Arrange
            var payloadResult = _fixture.Create<TestPayload>();
            var pipeline = Substitute.For<IPipeline<TestPayload>>();
            pipeline.Execute(Arg.Any<TestPayload>(), Arg.Any<CancellationToken>())
                .Returns(payloadResult);
            _serviceFactory.GetInstance<IPipeline<TestPayload>>().Returns(pipeline);

            //Act
            var result = _target.CreatePipelineAndExecute(new TestPayload(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(payloadResult);

            _serviceFactory.Received().GetInstance<IPipeline<TestPayload>>();
            pipeline.Received().Execute(Arg.Any<TestPayload>(), Arg.Is(CancellationToken.None));
        }

        [TestMethod]
        public async Task CreateAsyncPipelineAndExecuteAsync()
        {
            //Arrange
            var payloadResult = _fixture.Create<TestPayload>();
            var pipeline = Substitute.For<IAsyncPipeline<TestPayload>>();
            pipeline.ExecuteAsync(Arg.Any<TestPayload>(), Arg.Any<CancellationToken>())
                .Returns(payloadResult);
            _serviceFactory.GetInstance<IAsyncPipeline<TestPayload>>().Returns(pipeline);

            //Act
            var result = await _target.CreateAsyncPipelineAndExecuteAsync(new TestPayload(), CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Should().Be(payloadResult);

            _serviceFactory.Received().GetInstance<IAsyncPipeline<TestPayload>>();
            await pipeline.Received().ExecuteAsync(Arg.Any<TestPayload>(), Arg.Is(CancellationToken.None));
        }
    }
}

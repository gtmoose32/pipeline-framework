using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using PipelineFramework.TestInfrastructure;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests.PipelineComponentResolvers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DictionaryPipelineComponentResolverTests
    {
        private DictionaryPipelineComponentResolver _sut;

        [TestInitialize]
        public void Init()
        {
            _sut = new DictionaryPipelineComponentResolver();
        }

        [TestMethod]
        public void GetInstance()
        {
            //Arrange
            const string name = nameof(FooComponent);
            _sut.AddAsync(new FooComponent());

            //Act
            var result = _sut.GetInstance<IAsyncPipelineComponent<TestPayload>>(name);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<FooComponent>();
        }

        [TestMethod]
        public void GetInstance_ComponentNotFound()
        {
            //Arrange
            var name = nameof(FooComponent);

            //Act
            Action act = () => _sut.GetInstance<IAsyncPipelineComponent<TestPayload>>(name);

            //Assert
            act.Should().ThrowExactly<PipelineComponentNotFoundException>();
        }

        [TestMethod]
        public void Add_DuplicateComponent_Test()
        {
            //Arrange

            //Act
            Action act = () => _sut.Add(new FooComponent(), new FooComponent());

            //Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnitTestCommon;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DictionaryPipelineComponentResolverTests
    {
        private IPipelineComponentResolver _target;
        private IDictionary<string, IPipelineComponent> _components;

        [TestInitialize]
        public void Init()
        {
            _components = new Dictionary<string, IPipelineComponent>();
            _target = new DictionaryPipelineComponentResolver(_components);
        }

        [TestMethod]
        public void GetInstance()
        {
            //Arrange
            var name = typeof(FooComponent).Name;
            _components.Add(name, new FooComponent());

            //Act
            var result = _target.GetInstance<IAsyncPipelineComponent<TestPayload>>(name);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<FooComponent>();
        }

        [TestMethod]
        public void GetInstance_ComponentNotFound()
        {
            //Arrange
            var name = typeof(FooComponent).Name;

            //Act
            Action act = () => _target.GetInstance<IAsyncPipelineComponent<TestPayload>>(name);

            //Assert
            act.Should().ThrowExactly<PipelineComponentNotFoundException>();
        }
    }
}

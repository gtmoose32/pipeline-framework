using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Builder;
using PipelineFramework.Core.Tests.Infrastructure;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests.Builder
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineBuilderStateTests
    {
        private const string ComponentName = "FooComponent";

        [TestMethod]
        public void TestAddComponentTypeByType()
        {
            // Arrange
            var state = new PipelineBuilderState();

            // Act
            state.AddComponent(typeof(FooComponent));

            // Assert
            state.ComponentNames.Should().Contain(ComponentName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddDuplicateNameThrowsArgumentException()
        {
            // Arrange
            var state = new PipelineBuilderState();

            // Act
            state.AddComponent(typeof(FooComponent));
            state.AddComponent(typeof(FooComponent));
        }
    }
}
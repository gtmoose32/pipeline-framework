using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Builder;
using PipelineFramework.Core.Tests.Infrastructure;
using System;

namespace PipelineFramework.Core.Tests.Builder
{
    [TestClass]
    public class PipelineBuilderStateTests
    {
        private const string ComponentName = "FooComponent";

        [TestMethod]
        public void TestAddComponentTypeByName()
        {
            // Arrange
            var state = new PipelineBuilderState();

            // Act
            state.AddComponent(ComponentName);

            // Assert
            state.ComponentNames.Should().Contain(ComponentName);
        }

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
            state.AddComponent(ComponentName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddEmptyNameThrowsArgumentNullException()
        {
            // Arrange
            var state = new PipelineBuilderState();

            // Act
            state.AddComponent("   ");
        }
    }
}
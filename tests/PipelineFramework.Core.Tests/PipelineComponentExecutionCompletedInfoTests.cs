using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.TestInfrastructure;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineComponentExecutionCompletedInfoTests
    {
        [TestMethod]
        public void Ctor_SetsPropertiesFromStartingInfo()
        {
            // Arrange
            var startingInfo = new PipelineComponentExecutionStartingInfo("FooComponent", new TestPayload());
            var executionTime = TimeSpan.FromMilliseconds(100);
            var exception = new Exception();

            // Act
            var result = new PipelineComponentExecutionCompletedInfo(startingInfo, executionTime, exception);

            // Assert
            result.Should().NotBeNull();
            result.PipelineComponentName.Should().Be(startingInfo.PipelineComponentName);
            result.Payload.Should().Be(startingInfo.Payload);
            result.TimeStamp.Should().Be(startingInfo.TimeStamp);
            result.ExecutionTime.Should().Be(executionTime);
            result.Exception.Should().Be(exception);
        }
    }
}
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Exceptions;
using PipelineFramework.TestInfrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests.Exceptions
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void PipelineExecutionException_Properties_Test()
        {
            var component =  new FooComponent();
            component.Initialize(new Dictionary<string, string>());

            var exception = new ArgumentException("TestException");
            var target = new PipelineExecutionException(component, exception);

            target.Should().NotBeNull();
            target.ThrowingComponent.Should().BeAssignableTo<FooComponent>();
            target.InnerException.Should().BeAssignableTo<ArgumentException>();
            target.InnerException?.Message.Should().Be("TestException");
            target.Message.Should().StartWith("Pipeline execution halted!");
        }

        [TestMethod]
        public void PipelineComponentSettingNotFoundException_Test()
        {
            const string setting = "TestSetting";
            var component = new FooComponent();
            component.Initialize(new Dictionary<string, string>());

            var target = new PipelineComponentSettingNotFoundException(component, setting);

            target.Should().NotBeNull();
            target.ThrowingComponent.Should().BeAssignableTo<FooComponent>();
            target.SettingName.Should().Be(setting);
            target.Message.Should().StartWith("Pipeline component named");
        }
    }
}

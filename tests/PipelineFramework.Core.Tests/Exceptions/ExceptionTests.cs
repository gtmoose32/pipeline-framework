using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Core.Tests.Infrastructure;
using PipelineFramework.Exceptions;
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
            component.Initialize(component.GetType().Name, new Dictionary<string, string>());

            var message =
                $"An exception has occurred within a pipeline component named '{component.GetType().Name}', of type '{component.GetType().Name}'.  See inner exception for details.";

            var exception = new ArgumentException("TestException");
            var target = new PipelineExecutionException(component, exception);

            target.Should().NotBeNull();
            target.ThrowingComponent.Should().BeAssignableTo<FooComponent>();
            target.Message.Should().Be(message);
            target.InnerException.Should().BeAssignableTo<ArgumentException>();
            target.InnerException?.Message.Should().Be("TestException");
        }

        [TestMethod]
        public void PipelineComponentSettingNotFoundException_Test()
        {
            const string setting = "TestSetting";
            var component = new FooComponent();
            component.Initialize(component.GetType().Name, new Dictionary<string, string>());

            var message =
                $"Pipeline component named '{component.GetType().Name}' of type '{component.GetType().Name}' is referencing a setting named '{setting}' that cannot be found.";

            var target = new PipelineComponentSettingNotFoundException(component, setting);

            target.Should().NotBeNull();
            target.ThrowingComponent.Should().BeAssignableTo<FooComponent>();
            target.SettingName.Should().Be(setting);
            target.Message.Should().Be(message);
        }
    }
}

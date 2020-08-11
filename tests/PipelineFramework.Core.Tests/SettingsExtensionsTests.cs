using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Core.Tests.Infrastructure;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SettingsExtensionsTests
    {
        private Settings _target;

        [TestInitialize]
        public void Init()
        {
            _target = new Settings(new BarComponent());    
        }

        [TestMethod]
        public void AddRange_Test()
        {
            var config = new Dictionary<string, string>
            {
                {"1", "1"},
                {"2", "2"},
                {"3", "3"}
            };

            _target.AddRange(config.Select(kvp => kvp));

            _target.Count.Should().Be(3);
            _target.First().Key.Should().Be("1");
            _target.Last().Key.Should().Be("3");
        }

        [TestMethod]
        public void GetSettingValue_Test()
        {
            _target.Add("setting", "1");

            var result = _target.GetSettingValue("setting");

            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("1");
        }

        [TestMethod]
        public void GetSettingValue_SettingNotFound_ThrowNotFoundException_Test()
        {
            Action act = () => _target.GetSettingValue("setting");
            act.Should().ThrowExactly<PipelineComponentSettingNotFoundException>()
                .And.SettingName.Should().Be("setting");
        }

        [TestMethod]
        public void GetSettingValueGeneric_Test()
        {
            _target.Add("setting", "1");
            var result = _target.GetSettingValue<int>("setting");

            result.Should().Be(1);
        }

        [TestMethod]
        public void GettingSettingValueGeneric_InvalidSettingTest()
        {
            _target.Add("setting", "xxx");
            Action act = () => _target.GetSettingValue<int>("setting");

            act.Should().ThrowExactly<FormatException>();
        }

        [TestMethod]
        public void GettingSettingGeneric_SettingNotFound_UseDefaultValue_Test()
        {
            var result = _target.GetSettingValue("setting", 10);

            result.Should().Be(10);
        }

        [TestMethod]
        public void GettingSettingGeneric_CannotConvertSettingValue_UseDefaultValue_Test()
        {
            _target.Add("setting", "xxx");
            var result = _target.GetSettingValue("setting", 10);

            result.Should().Be(10);
        }

        [TestMethod]
        public void GettingSettingGeneric_ConvertToType_Test()
        {
            _target.Add("setting", "32");
            var result = _target.GetSettingValue("setting", 10);

            result.Should().Be(32);
        }
    }
}

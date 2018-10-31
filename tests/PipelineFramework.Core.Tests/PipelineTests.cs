using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Exceptions;
using PipelineFramework.Tests.SharedInfrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineTests : PipelineTestsBase
    {
        [TestMethod]
        public void Pipeline_DuplicateComponentsConfiguredDifferently_Test()
        {
            var settings = new Dictionary<string, IDictionary<string, string>>
            {
                {"Component1", new Dictionary<string, string> {{"TestValue", "Component1Value"}, {"UseFoo", "true"}}},
                {"Component2", new Dictionary<string, string> {{"TestValue", "Component2Value"}, {"UseFoo", "false"}}}
            };

            var payload = new TestPayload();
            var target = new Pipeline<TestPayload>(PipelineComponentResolver, new List<string> { "Component1", "Component2" }, settings);
            var actual = target.Execute(payload);

            Assert.IsNotNull(actual);
            Assert.AreSame(actual, payload);
            Assert.AreEqual(actual.FooStatus, "Component1Value");
            Assert.AreEqual(payload.BarStatus, "Component2Value");
        }

        [ExpectedException(typeof(OperationCanceledException), AllowDerivedTypes = true)]
        [TestMethod]
        public void Pipeline_Excecution_Cancellation_Test()
        {
            var types = new List<Type> { typeof(DelayComponent), typeof(BarComponent) };
            var config = new Dictionary<string, IDictionary<string, string>>();

            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config);
            target.Execute(new TestPayload(), cts.Token);
        }

        [TestMethod]
        public void Pipeline_Execution_Test()
        {
            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };
            var config = new Dictionary<string, IDictionary<string, string>>();

            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config);
            var result = target.Execute(new TestPayload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(FooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(BarComponent).Name} executed!");
        }

        [TestMethod]
        public void Pipeline_Execution_ComponentNames_NullSettings_Test()
        {
            //var types = new List<string> { "Foo", "Bar" };
            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, null);
            var result = target.Execute(new TestPayload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(FooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(BarComponent).Name} executed!");
        }

        [TestMethod]
        public void Pipeline_Execution_ComponentNames_EmptySettings_Test()
        {
            //var types = new List<string> { "Foo", "Bar" };
            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, new Dictionary<string, IDictionary<string, string>>());
            var result = target.Execute(new TestPayload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(FooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(BarComponent).Name} executed!");
        }

        [TestMethod]
        public void Pipeline_Execution_NullSettings_Test()
        {
            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, null);
            var result = target.Execute(new TestPayload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(FooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(BarComponent).Name} executed!");
        }

        [TestMethod]
        public void Pipeline_Execution_EmptySettings_Test()
        {
            var types = new List<Type> { typeof(FooComponent), typeof(BarComponent) };

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, new Dictionary<string, IDictionary<string, string>>());
            var result = target.Execute(new TestPayload());

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.FooStatus == $"{typeof(FooComponent).Name} executed!");
            Assert.IsTrue(result.BarStatus == $"{typeof(BarComponent).Name} executed!");
        }

        [ExpectedException(typeof(PipelineExecutionException))]
        [TestMethod]
        public void PipelineComponent_Exception_Test()
        {
            var types = new List<Type> { typeof(FooComponent), typeof(BarExceptionComponent) };
            var config = types.ToDictionary<Type, string, IDictionary<string, string>>(
                t => t.Name,
                t => new Dictionary<string, string> { { "test", "value" } });

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config);
            target.Execute(new TestPayload());
        }

        [TestMethod]
        public void PipelineComponent_SettingNotFoundException_Test()
        {
            var types = new List<Type> { typeof(FooSettingNotFoundComponent) };
            var config = new Dictionary<string, IDictionary<string, string>>();

            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config);
            try
            {
                target.Execute(new TestPayload());
            }
            catch (PipelineExecutionException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(PipelineComponentSettingNotFoundException));
            }
        }

        [TestMethod]
        public void Pipeline_FilterExecution_Test()
        {
            var types = new List<Type> { typeof(FooComponent), typeof(PipelineExecutionTerminatingComponent), typeof(BarComponent) };
            var config = new Dictionary<string, IDictionary<string, string>>();

            foreach (var t in types)
            {
                config.Add(t.Name, new Dictionary<string, string> { { "test", "value" } });
            }

            var target = new Pipeline<TestPayload>(PipelineComponentResolver, types, config);
            var result = target.Execute(new TestPayload());

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.FooStatus == $"{typeof(FooComponent).Name} executed!");
            Assert.IsNull(result.BarStatus);
        }
    }
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Core.Tests.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SettingsTests
    {
        private Settings _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new Settings(new FooComponent()); 
        }

        [TestMethod]
        public void Add_Test()
        {
            const string key = "key";
            const string value = "value";

            _target.Add(key, value);

            Assert.IsTrue(_target.ContainsKey(key));
            Assert.AreEqual(_target[key], value);
            Assert.IsTrue(_target.TryGetValue(key, out var result));
            Assert.AreEqual(result, value);
            Assert.AreEqual(_target.Count, 1);
            Assert.AreEqual(_target.Keys.Count, 1);
            Assert.AreEqual(_target.Values.Count, 1);
            Assert.AreEqual(_target.Count, 1);
            Assert.IsFalse(_target.IsReadOnly);
            Assert.IsTrue(_target.Contains(new KeyValuePair<string, string>(key, value)));
        }

        [TestMethod]
        public void SetByIndexer_Test()
        {
            const string key = "key";
            _target.Add(key, "value");

            _target[key] = "new_value";

            Assert.AreEqual(_target[key], "new_value");
        }

        [TestMethod]
        public void CopyTo_Test()
        {
            for (var i = 0; i < 10; i++)
            {
                _target.Add(i.ToString(), $"Number: {i}");
            }

            var results = new KeyValuePair<string, string>[_target.Count];
            _target.CopyTo(results, 0);

            _target.Count.Should().Be(10);
        }

        [TestMethod]
        public void Clear_Test()
        {
            for (var i = 0; i < 10; i++)
            {
                _target.Add(i.ToString(), $"Number: {i}");
            }

            _target.Clear();

            Assert.AreEqual(_target.Count, 0);
        }

        [TestMethod]
        public void Remove_Test()
        {
            const string key = "key";
            _target.Add(key, "value");

            _target.Remove(key);

            Assert.IsFalse(_target.ContainsKey(key));
        }

        [TestMethod]
        public void RemoveItem_Test()
        {
            const string key = "key";
            var kvp = new KeyValuePair<string, string>(key, "value");
            _target.Add(kvp);

            _target.Remove(kvp);

            Assert.IsFalse(_target.ContainsKey(key));
        }

        [TestMethod]
        public void GetEnumerator_Test()
        {
            for (var i = 0; i < 10; i++)
            {
                _target.Add(i.ToString(), $"Number: {i}");
            }

            var count = 0;
            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once UnusedVariable
            foreach (var setting in _target)
            {
                count++;
            }

            Assert.AreEqual(_target.Count, count);

            count = 0;
            // ReSharper disable once GenericEnumeratorNotDisposed
            var enumerator = _target.GetEnumerator();
            while (enumerator.MoveNext())
            {
                count++;
            }
            
            Assert.AreEqual(_target.Count, count);
        }
    }
}

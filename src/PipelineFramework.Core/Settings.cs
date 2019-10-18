using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework
{
    /// <summary>
    /// This class is merely a wrapper around <see cref="IDictionary{String, String}"/> that provides a detailed exception when 
    /// a specified setting is not found.
    /// </summary>
    internal class Settings : IDictionary<string, string>
    {
        private readonly IDictionary<string, string> _settings;
        private readonly IPipelineComponent _component;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        public Settings(IPipelineComponent component)
        {
            _component = component;
            _settings = new Dictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _settings.GetEnumerator();
        }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, string> item)
        {
            _settings.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _settings.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return _settings.Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _settings.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return _settings.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => _settings.Count;

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly => _settings.IsReadOnly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _settings.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            _settings.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _settings.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out string value)
        {
            return _settings.TryGetValue(key, out value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (!_settings.ContainsKey(key))
                    throw new PipelineComponentSettingNotFoundException(_component, key);

                return _settings[key];
            }
            set => _settings[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<string> Keys => _settings.Keys;

        /// <summary>
        /// 
        /// </summary>
        public ICollection<string> Values => _settings.Values;
    }
}

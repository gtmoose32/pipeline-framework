using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PipelineFramework
{
    /// <summary>
    /// Extension methods for working with pipeline component settings, <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds a list of key value pairs of strings to the dictionary./>
        /// </summary>
        /// <param name="settings">Dictionary to add to.</param>
        /// <param name="config">List of key value pair strings to be added.</param>
        public static void AddRange(
            this IDictionary<string, string> settings, 
            IEnumerable<KeyValuePair<string, string>> config)
        {
            foreach (var setting in config)
            {
                settings.Add(setting);
            }
        }

        /// <summary>
        /// Gets a setting from the dictionary and converts it to the specified type.  
        /// An exception will be thrown if the format for the conversion isn't correct.
        /// </summary>
        /// <param name="settings">Settings dictionary</param>
        /// <param name="name">Name of the setting</param>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <returns>Converted setting</returns>
        public static T GetSettingValue<T>(this IDictionary<string, string> settings, string name)
            => Convert<T>(settings.GetSettingValue(name));

        /// <summary>
        /// Gets a setting from the dictionary and converts it to the specified type.  
        /// This method won't throw an exception.
        /// </summary>
        /// <param name="settings">Settings dictionary</param>
        /// <param name="name">Name of the setting</param>
        /// <param name="defaultValue">Value to be returned if the setting cannot be found or conversion to type <see cref="T"/> fails.</param>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <returns>Converted setting</returns>
        public static T GetSettingValue<T>(this IDictionary<string, string> settings, string name, T defaultValue)
            => settings.TryGetValue(name, out var value)
                ? Convert<T>(value)
                : defaultValue;
        
        /// <summary>
        /// Gets a setting from the dictionary returns it.
        /// If the setting does not exist null will be returned.  
        /// </summary>
        /// <param name="settings">Settings dictionary</param>
        /// <param name="name">Name of the setting</param>
        /// <param name="defaultValue">Value to be returned if the setting cannot be found or found setting is null.</param>
        /// <returns></returns>
        public static string GetSettingValue(this IDictionary<string, string> settings, string name, string defaultValue = null)
            =>  defaultValue == null 
                ? settings[name]
                : settings.TryGetValue(name, out var value) ? value : defaultValue;
        
        private static T Convert<T>(string s)
        {
            try
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(s);
            }
            catch (Exception exception)
            {
                throw exception.GetBaseException();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PipelineFramework
{
    /// <summary>
    /// Pipeline Framework useful extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds a list of key value pairs of strings to the dictionary./>
        /// </summary>
        /// <param name="settings">Dictionary to add to.</param>
        /// <param name="config">List of key value pair strings to be added.</param>
        public static void AddRange(
            this Settings settings, 
            IEnumerable<KeyValuePair<string, string>> config)
        {
            foreach (var kvp in config)
            {
                settings.Add(kvp);
            }
        }

        /// <summary>
        /// Gets a setting from the dictionary and converts it to the specified type.  
        /// An exception will be thrown if the format for the conversion isn't correct.
        /// </summary>
        /// <param name="settings">Settings dictionary</param>
        /// <param name="name">Name of the setting</param>
        /// <param name="throwOnSettingNotFound"></param>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <returns>Converted setting</returns>
        public static T GetSettingValue<T>(
            this Settings settings, 
            string name, 
            bool throwOnSettingNotFound = true)
        {
            return settings.GetSettingValue(name).Convert<T>();
        }

        /// <summary>
        /// Gets a setting from the dictionary and converts it to the specified type.  
        /// This method won't throw an exception.
        /// </summary>
        /// <param name="settings">Settings dictionary</param>
        /// <param name="name">Name of the setting</param>
        /// <param name="defaultValue">Value to be returned if the conversion failed.</param>
        /// <param name="throwOnSettingNotFound"></param>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <returns>Converted setting</returns>
        public static T GetSettingValue<T>(
            this Settings settings, 
            string name, 
            T defaultValue, 
            bool throwOnSettingNotFound = true)
        {
            return settings.GetSettingValue(name, throwOnSettingNotFound).Convert(defaultValue);
        }

        /// <summary>
        /// Gets a setting from the dictionary returns it.
        /// If the setting does not exist null will be returned.  
        /// </summary>
        /// <param name="settings">Settings dictionary</param>
        /// <param name="name">Name of the setting</param>
        /// <param name="throwOnSettingNotFound"></param>
        /// <returns></returns>
        public static string GetSettingValue(
            this Settings settings, 
            string name, 
            bool throwOnSettingNotFound = true)
        {
            if (throwOnSettingNotFound) return settings[name];

            return settings.ContainsKey(name) ? settings[name] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Convert<T>(this string s, T defaultValue)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                return (T)converter.ConvertFromString(s);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Convert<T>(this string s)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(s);
        }
    }
}

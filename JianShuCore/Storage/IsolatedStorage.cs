using JianShuCore.Interface;
using System;
using System.Collections.Generic;
using Windows.Storage;
using static JianShuCore.Common.Untils;

namespace JianShuCore.Storage
{
    public class IsolatedStorage : IStorage
    {
        #region Property
        private ApplicationDataContainer container
        {
            get
            {
                return ApplicationData.Current.RoamingSettings;
            }
        }
        #endregion

        #region Public Function
        /// <summary>
        /// Add item to RomaingSettings
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public void AddItem(string key, object value)
        {
            container.Values[key] = Serialize(value);
        }

        /// <summary>
        /// Update item in RomaingSettings
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public void UpdateItem(string key, object value)
        {
            AddItem(key, value);
        }

        /// <summary>
        /// Judge whether RomaingSettings has this item through the key given
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>true for yes and false for no</returns>
        public bool ContainItem(string key)
        {
            object value = container.Values[key];
            if (value == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Clear data in RomaingSettings
        /// </summary>
        public void FlushAll()
        {
            foreach (KeyValuePair<string, object> current in container.Values)
            {
                RemoveItem(current.Key);
            }
        }

        /// <summary>
        /// Get item from RomaingSettings
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Value that you saved</returns>
        public T GetItem<T>(string key) where T : class
        {
            string value = container.Values[key]?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            else
            {
                return Deserialize<T>(value);
            }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetItems<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Remove item in RomaingSettings
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>true for success and false for failed</returns>
        public bool RemoveItem(string key)
        {
            return container.Values.Remove(key);
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

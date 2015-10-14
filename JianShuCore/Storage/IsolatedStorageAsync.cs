using JianShuCore.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using static JianShuCore.Common.Untils;

namespace JianShuCore.Storage
{
    public class IsolatedStorageAsync : IStorageAsync
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
        public async Task AddItemAsync(string key, object value)
        {
            await Task.Run(() => container.Values[key] = Serialize(value));
        }

        /// <summary>
        /// Update item in RomaingSettings
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public async Task UpdateItemAsync(string key, object value)
        {
            await AddItemAsync(key, value);
        }

        /// <summary>
        /// Judge whether RomaingSettings has this item through the key given
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>true for yes and false for no</returns>
        public async Task<bool> ContainItemAsync(string key)
        {
            return await Task.Run<bool>(() =>
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
            });
        }

        /// <summary>
        /// Clear data in RomaingSettings
        /// </summary>
        public async Task FlushAllAsync()
        {
            foreach(KeyValuePair<string ,object> current in container.Values)
            {
                await RemoveItemAsync(current.Key);
            }
        }

        /// <summary>
        /// Get item from RomaingSettings
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Value that you saved</returns>
        public async Task<T> GetItemAsync<T>(string key) where T : class
        {
            return await Task.Run<T>(() =>
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
            });
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> GetItemsAsync<T>(string key) where T : class
        {
            await Task.Run(() => { });
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove item in RomaingSettings
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>true for success and false for failed</returns>
        public async Task<bool> RemoveItemAsync(string key)
        {
            return await Task.Run<bool>(() => { return container.Values.Remove(key); });
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

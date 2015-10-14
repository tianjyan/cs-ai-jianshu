using JianShuCore.Common;
using JianShuCore.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static JianShuCore.Common.Untils;

namespace JianShuCore.Provider
{
    public class CacheProviderAsync : IProvider
    {
        #region Private Field
        //Factory Pattern
        IStorageAsync storage;
        StorageTypeAsync type;
        #endregion

        #region Constructor
        public CacheProviderAsync(StorageTypeAsync type)
        {
            this.type = type;
            Initialize();
        }
        #endregion

        #region Public Function
        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            if (type == StorageTypeAsync.None)
            {
                throw new NotSupportedException("Not support the type of none.");
            }

            storage = Activator.CreateInstance(Type.GetType(GetDescription(type))) as IStorageAsync;

            if (storage == null)
            {
                throw new DllNotFoundException("Can not find the type" + type.ToString());
            }
        }

        /// <summary>
        /// Add item to Storage
        /// </summary>
        /// <param name="key">
        /// If the type of storage is sqlite,
        /// should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task AddItemAsync(string key, object value)
        {
            await storage.AddItemAsync(key, value);
        }

        /// <summary>
        /// Update item in Storage
        /// </summary>
        /// <param name="key">
        /// If the type of storage is sqlite,
        /// should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UpdateItemAsync(string key, object value)
        {
            await storage.UpdateItemAsync(key, value);
        }

        /// <summary>
        /// Judge whether Storage has this item through the key given
        /// </summary>
        /// <param name="key">
        /// If the type of storage is sqlite,
        /// should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <returns>true for yes and false for no</returns>
        public async Task<bool> ContainItemAsync(string key)
        {
            return await storage.ContainItemAsync(key);
        }

        /// <summary>
        /// Clear data in Storage
        /// </summary>
        public async Task FlushAllAsync(StorageType type)
        {
            await storage.FlushAllAsync();
        }

        /// <summary>
        /// Get item from Storage
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">
        /// If the type of storage is sqlite,
        /// should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <returns>Value that you saved</returns>
        public async Task<T> GetItemAsync<T>(string key) where T : class
        {
            return await storage.GetItemAsync<T>(key);
        }

        /// <summary>
        /// Get items from Storage
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">
        /// If the type of storage is sqlite,
        /// should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <returns></returns>
        public async Task<List<T>> GetItemsAsync<T>(string key) where T : class
        {
            return await storage.GetItemsAsync<T>(key);
        }

        /// <summary>
        /// Remove item in Storage
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">
        /// If the type of storage is sqlite,
        /// should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <returns>Value that you saved</returns>
        public async Task<bool> RemoveItemAsync(string key)
        {
            return await storage.RemoveItemAsync(key);
        }
        #endregion
    }
}

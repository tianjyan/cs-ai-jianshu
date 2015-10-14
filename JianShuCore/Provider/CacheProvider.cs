using JianShuCore.Common;
using JianShuCore.Interface;
using System;
using System.Collections.Generic;
using static JianShuCore.Common.Untils;

namespace JianShuCore.Provider
{
    public class CacheProvider : IProvider
    {
        #region Private Field
        IStorage storage;
        StorageType type;
        #endregion

        #region Constructor
        public CacheProvider(StorageType type)
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
            if (type == StorageType.None)
            {
                throw new NotSupportedException("Not support the type of none.");
            }

            storage =Activator.CreateInstance(Type.GetType(GetDescription(type))) as IStorage;

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
        public void AddItem(string key, object value)
        {
            storage.AddItem(key, value);
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
        public void UpdateItem(string key, object value)
        {
            storage.UpdateItem(key, value);
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
        public bool ContainItem(string key)
        {
            return storage.ContainItem(key);
        }

        /// <summary>
        /// Clear data in Storage
        /// </summary>
        public void Flush(StorageType type)
        {
            storage.FlushAll();
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
        public T GetItem<T>(string key) where T :class
        {
            return storage.GetItem<T>(key);
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
        public List<T> GetItems<T>(string key) where T :class
        {
            return storage.GetItems<T>(key);
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
        public bool RemoveItem(string key)
        {
            return storage.RemoveItem(key);
        }

        /// <summary>
        /// Get the size of local file
        /// </summary>
        /// <returns></returns>
        public long GetSize()
        {
            return storage.GetSize();
        }
        #endregion
    }
}

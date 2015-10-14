using System.Collections.Generic;

namespace JianShuCore.Interface
{
    internal interface IStorage
    {
        void AddItem(string key, object value);
        void UpdateItem(string key, object value);
        bool ContainItem(string key);
        void FlushAll();
        T GetItem<T>(string key) where T : class;
        List<T> GetItems<T>(string key) where T : class;
        bool RemoveItem(string key);
        long GetSize();
    }
}

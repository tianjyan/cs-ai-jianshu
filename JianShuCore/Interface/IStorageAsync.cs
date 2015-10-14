using System.Collections.Generic;
using System.Threading.Tasks;

namespace JianShuCore.Interface
{
    interface IStorageAsync
    {
        Task AddItemAsync(string key, object value);
        Task UpdateItemAsync(string key, object value);
        Task<bool> ContainItemAsync(string key);
        Task FlushAllAsync();
        Task<T> GetItemAsync<T>(string key) where T : class;
        Task<List<T>> GetItemsAsync<T>(string key) where T : class;
        Task<bool> RemoveItemAsync(string key);
        long GetSize();
    }
}

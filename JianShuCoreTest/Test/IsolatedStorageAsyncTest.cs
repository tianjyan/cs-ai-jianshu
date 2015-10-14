using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using JianShuCore.Storage;
using JianShuCoreTest.ModelTest;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class IsolatedStorageAsyncTest
    {
        [TestMethod]
        public async Task AddClassAsyncTest()
        {
            IsolatedStorageAsync storage = new IsolatedStorageAsync();
            var value = new StorageModel(){ Name = "name", Value = "default" };
            string key = "Class";
            await storage.AddItemAsync(key, value);
            StorageModel result =  await storage.GetItemAsync<StorageModel>(key);
            
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public async Task AddStringAsyncTest()
        {
            IsolatedStorageAsync storage = new IsolatedStorageAsync();
            string value = "test";
            string key = "String";
            await storage.AddItemAsync(key, value);
            string result = await storage.GetItemAsync<string>(key);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public async Task AddObjectAsyncTest()
        {
            IsolatedStorageAsync storage = new IsolatedStorageAsync();
            object value = "test";
            string key = "Object";
            await storage.AddItemAsync(key, value);
            object result = await storage.GetItemAsync<object>(key);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public async Task RemoveAsyncTest()
        {
            IsolatedStorageAsync storage = new IsolatedStorageAsync();
            string value = "test";
            string key = "String";

            await storage.AddItemAsync(key, value);
            Assert.IsTrue(await storage.ContainItemAsync(key));
            await storage.RemoveItemAsync(key);
            Assert.IsFalse(await storage.ContainItemAsync(key));
        }

        [TestMethod]
        public async Task FlushAsyncTest()
        {
            IsolatedStorageAsync storage = new IsolatedStorageAsync();

            await storage.AddItemAsync("String1", "test");
            await storage.AddItemAsync("String2", "test");
            await storage.AddItemAsync("String3", "test");
            await storage.AddItemAsync("String4", "test");

            await storage.FlushAllAsync();

            Assert.IsFalse(await storage.ContainItemAsync("String1"));
            Assert.IsFalse(await storage.ContainItemAsync("String2"));
            Assert.IsFalse(await storage.ContainItemAsync("String3"));
            Assert.IsFalse(await storage.ContainItemAsync("String4"));
        }

        [TestMethod]
        public async Task UpdateAsyncTest()
        {
            IsolatedStorageAsync storage = new IsolatedStorageAsync();
            await storage.AddItemAsync("String", "test");
            await storage.UpdateItemAsync("String", "test1");

            Assert.AreEqual(await storage.GetItemAsync<string>("String"), "test1");

        }
    }
}

using JianShuCore.Storage;
using JianShuCoreTest.ModelTest;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class SqliteStorageAsyncTest
    {
        [TestMethod]
        public async Task SQLiteAsyncAddItemTest()
        {
            SqliteStorageAsync storage = new SqliteStorageAsync();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key = "Person/1";
            await storage.AddItemAsync(key, model);

            StorageModel result =await storage.GetItemAsync<StorageModel>(key);

            Assert.AreEqual(model, result);
        }

        [TestMethod]
        public async Task SQLiteAsyncUpdateItemTest()
        {
            SqliteStorageAsync storage = new SqliteStorageAsync();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key = "Person/1";

            await storage.AddItemAsync(key, model);

            model.Value = "value2";
            await storage.UpdateItemAsync(key, model);

            StorageModel reust = await storage.GetItemAsync<StorageModel>(key);

            Assert.AreEqual(reust.Value, "value2");
        }

        [TestMethod]
        public async Task SQLiteAsyncDelteItemTest()
        {
            SqliteStorageAsync storage = new SqliteStorageAsync();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key = "Person/1";

            await storage.AddItemAsync(key, model);
            Assert.IsTrue(await storage.ContainItemAsync(key));

            await storage.RemoveItemAsync(key);
            Assert.IsFalse(await storage.ContainItemAsync(key));
        }

        [TestMethod]
        public async Task SQLiteAsyncGetItemTest()
        {
            SqliteStorageAsync storage = new SqliteStorageAsync();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key = "Person/1";
            await storage.AddItemAsync(key, model);

            StorageModel result =await storage.GetItemAsync<StorageModel>(key);

            Assert.AreEqual(model, result);
        }

        [TestMethod]
        public async Task SQLiteAsyncGetItemsTest()
        {
            SqliteStorageAsync storage = new SqliteStorageAsync();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key1 = "Person1/1";
            string key2 = "Person1/2";
            string key3 = "Person1/3";

            await storage.AddItemAsync(key1, model);
            await storage.AddItemAsync(key2, model);
            await storage.AddItemAsync(key3, model);

            List<StorageModel> models = await storage.GetItemsAsync<StorageModel>("Person1/0");

            Assert.AreEqual(models.Count, 3);

            await storage.FlushAllAsync();
        }
    }
}

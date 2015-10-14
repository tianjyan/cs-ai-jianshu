using JianShuCore.Storage;
using JianShuCoreTest.ModelTest;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class SqliteStorageTest
    {
        [TestMethod]
        public void SQLiteAddItemTest()
        {
            SqliteStorage storage = new SqliteStorage();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key = "Person/1";
            storage.AddItem(key, model);

            StorageModel result = storage.GetItem<StorageModel>(key);

            Assert.AreEqual(model, result);
        }

        [TestMethod]
        public void SQLiteUpdateItemTest()
        {
            SqliteStorage storage = new SqliteStorage();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key = "Person/1";

            storage.AddItem(key, model);

            model.Value = "value2";
            storage.UpdateItem(key, model);

            StorageModel reust = storage.GetItem<StorageModel>(key);

            Assert.AreEqual(reust.Value, "value2");
        }

        [TestMethod]
        public void SQLiteDelteItemTest()
        {
            SqliteStorage storage = new SqliteStorage();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key = "Person/1";

            storage.AddItem(key, model);
            Assert.IsTrue(storage.ContainItem(key));

            storage.RemoveItem(key);
            Assert.IsFalse(storage.ContainItem(key));
        }

        [TestMethod]
        public void SQLiteGetItemTest()
        {
            SqliteStorage storage = new SqliteStorage();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key = "Person/1";
            storage.AddItem(key, model);

            StorageModel result = storage.GetItem<StorageModel>(key);

            Assert.AreEqual(model, result);
        }

        [TestMethod]
        public void SQLiteGetItemsTest()
        {
            SqliteStorage storage = new SqliteStorage();
            StorageModel model = new StorageModel() { Name = "name1", Value = "value1" };
            string key1 = "Person1/1";
            string key2 = "Person1/2";
            string key3 = "Person1/3";

            storage.AddItem(key1, model);
            storage.AddItem(key2, model);
            storage.AddItem(key3, model);

            List<StorageModel> models = storage.GetItems<StorageModel>("Person1/0");

            Assert.AreEqual(models.Count, 3);

            storage.FlushAll();
        }
    }
}

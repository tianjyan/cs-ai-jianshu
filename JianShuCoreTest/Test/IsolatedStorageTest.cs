using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using JianShuCore.Storage;
using JianShuCoreTest.ModelTest;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class IsolatedStorageTest
    {
        [TestMethod]
        public void AddClassTest()
        {
            IsolatedStorage storage = new IsolatedStorage();
            var value = new StorageModel() { Name = "name", Value = "default" };
            string key = "Class";
            storage.AddItem(key, value);
            StorageModel result = storage.GetItem<StorageModel>(key);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void AddStringTest()
        {
            IsolatedStorage storage = new IsolatedStorage();
            string value = "test";
            string key = "String";
            storage.AddItem(key, value);
            string result = storage.GetItem<string>(key);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void AddObjectTest()
        {
            IsolatedStorage storage = new IsolatedStorage();
            object value = "test";
            string key = "Object";
            storage.AddItem(key, value);
            object result = storage.GetItem<object>(key);

            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void RemoveTest()
        {
            IsolatedStorage storage = new IsolatedStorage();
            string value = "test";
            string key = "String";

            storage.AddItem(key, value);
            Assert.IsTrue(storage.ContainItem(key));
            storage.RemoveItem(key);
            Assert.IsFalse(storage.ContainItem(key));
        }

        [TestMethod]
        public void FlushTest()
        {
            IsolatedStorage storage = new IsolatedStorage();

            storage.AddItem("String1", "test");
            storage.AddItem("String2", "test");
            storage.AddItem("String3", "test");
            storage.AddItem("String4", "test");

            storage.FlushAll();

            Assert.IsFalse(storage.ContainItem("String1"));
            Assert.IsFalse(storage.ContainItem("String2"));
            Assert.IsFalse(storage.ContainItem("String3"));
            Assert.IsFalse(storage.ContainItem("String4"));
        }

        [TestMethod]
        public void UpdateTest()
        {
            IsolatedStorage storage = new IsolatedStorage();
            storage.AddItem("String", "test");
            storage.UpdateItem("String", "test1");

            Assert.AreEqual(storage.GetItem<string>("String"), "test1");
        }
    }
}

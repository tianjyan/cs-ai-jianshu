using JianShuCore.Common;
using JianShuCore.Provider;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class CacheProviderTest
    {
        [TestMethod]
        public void CahceInitTest()
        {
            CacheProvider iso = new CacheProvider(StorageType.IsolatedStorage);
            CacheProvider sql = new CacheProvider(StorageType.SqliteStorage);

            Assert.IsNotNull(iso);
            Assert.IsNotNull(sql);
        }
    }
}

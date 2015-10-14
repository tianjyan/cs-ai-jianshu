using JianShuCore.Common;
using JianShuCore.Provider;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class CacheProviderAsyncTest
    {
        [TestMethod]
        public void CahceAsyncInitTest()
        {
            CacheProviderAsync iso = new CacheProviderAsync(StorageTypeAsync.IsolatedStorageAsync);
            CacheProviderAsync sql = new CacheProviderAsync(StorageTypeAsync.SqliteStorageAsync);

            Assert.IsNotNull(iso);
            Assert.IsNotNull(sql);
        }
    }
}

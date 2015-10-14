using JianShuCore;
using JianShuCore.Model;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class CommonProviderTest
    {
        [TestMethod]
        public async Task QueryUserInfo()
        {
            CommonProvider common = new CommonProvider();
            UserBaseInfo user = await common.QueryUserInfo("2e128e04325c");
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task QueryTopicLastTest()
        {
            CommonProvider common = new CommonProvider();
            List<TopicLastResult> user = await common.QueryTopicLast("26293", 0);
            Assert.IsNotNull(user);
        }
    }

}

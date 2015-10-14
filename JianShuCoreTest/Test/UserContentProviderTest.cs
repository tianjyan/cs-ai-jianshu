using JianShuCore;
using JianShuCore.Model;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class UserContentProviderTest
    {
        [TestMethod]
        public async Task QueryBaseInfoTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            UserBaseInfo mr = await mine.QueryBaseInfo(result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(mr);
        }

        [TestMethod]
        public async Task QueryActivitiesTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            //7998636为上一个动态的ID-1的值
            List<ActivityResult> fr = await mine.QueryActivities(7998636, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(fr);
        }

        [TestMethod]
        public async Task QueryNoteTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<NoteResult> nr = await mine.QueryNote(0, 1, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryNotebookTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<NotebookResult> nr = await mine.QueryNotebook(0, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryBookmarkTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<BookmarkResult> nr = await mine.QueryBookmark(0, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryLikeNotesTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<Note> nr = await mine.QueryLikeNotes(0, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task LikeNoteTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            LikeResult nr = await mine.LikeNote(1664660, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task UnlikeNoteTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            LikeResult nr = await mine.UnlikeNote(1664660, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryFriendActivitiesTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<ActivityResult> nr = await mine.QueryFriendActivities(0, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QuerySubscriptionNotesTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<SubscriptionNotesResult> nr = await mine.QuerySubscriptionNotes(0, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryGetFeedsMsgTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<FeedsMsgResult> nr = await mine.QueryFeedMsg(result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryMsgCountTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            MsgCountResult nr = await mine.QueryMsgCount(result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryNotifyResultListTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<NotifyResult> nr = await mine.QueryNotify(0,null, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryFollowerTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<FollowUserInfo> nr = await mine.QueryFollowers(0, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QuerySubscriptionTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<Subscription> sb = await mine.QuerySubscription(1, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(sb);
        }

        [TestMethod]
        public async Task QueryOtherFollowerTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<FollowUserInfo> nr = await mine.QueryFollowers(1, "580197", result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }

        [TestMethod]
        public async Task QueryOtherNotebookTest()
        {
            UserContentProvider mine = new UserContentProvider();
            LoginResult result = await mine.Login(Config.username, Config.password);
            List<NotebookResult> nr = await mine.QueryOhterNotebook("580197", 0, result.Id.ToString(), result.MobileToken);
            Assert.IsNotNull(nr);
        }
    }
}

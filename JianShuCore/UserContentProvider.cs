using JianShuCore.Model;
using JianShuCore.Provider;
using JianShuCore.WebAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JianShuCore
{
    public class UserContentProvider
    {
        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public async Task<LoginResult> Login(string username, string password)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string content = string.Format("email={0}&password={1}", Uri.EscapeDataString(username), Uri.EscapeDataString(password));
            return await web.HttpPostRequest<LoginResult>(Config.Login, content, web.GetHeaders(null, null));
        }

        /// <summary>
        /// Register account
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="nickname">Nickname</param>
        /// <param name="password">Password</param>
        /// <returns>LoginResult</returns>
        public async Task<LoginResult> Register(string username, string nickname, string password)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string content = string.Format("email={0}&nickname={1}&password={2}", Uri.EscapeDataString(username), Uri.EscapeDataString(nickname),Uri.EscapeDataString(password));
            return await web.HttpPostRequest<LoginResult>(Config.Register, content, web.GetHeaders(null, null));
        }
         
        /// <summary>
        /// Query base user info
        /// </summary>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<UserBaseInfo> QueryBaseInfo(string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<UserBaseInfo>(Config.BaseInfo, web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query user's activities
        /// </summary>
        /// <param name="startid">The activity id that start</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<ActivityResult>> QueryActivities(int startid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = string.Format(Config.QueryActivities, userid);
            if (startid != 0)
            {
                url += "&max_id=" + startid;
            }
            return await web.HttpGetRequest<List<ActivityResult>>(url, web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query people that user followered
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<FollowUserInfo>> QueryFollowingUsers(int page, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<FollowUserInfo>>(string.Format(Config.Following, userid, page), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query people that other's followered
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="otherid">Ohter's User Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<FollowUserInfo>> QueryFollowingUsers(int page, string otherid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<FollowUserInfo>>(string.Format(Config.Following, otherid, page), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query fans
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<FollowUserInfo>> QueryFollowers(int page, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<FollowUserInfo>>(string.Format(Config.Followers, userid, page), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query other's fans
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="otherid">Ohter's User Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<FollowUserInfo>> QueryFollowers(int page, string otherid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<FollowUserInfo>>(string.Format(Config.Followers, otherid, page), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query user's notes
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="shared">1:Public article;0:Private article</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<NoteResult>> QueryNote(int page, int shared, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<NoteResult>>(string.Format(Config.Notes, page, shared), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query note detail
        /// </summary>
        /// <param name="noteId">Note Id</param>
        /// <returns></returns>
        public async Task<NoteDetailResult> QueryNoteDetail(string noteId, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<NoteDetailResult>(string.Format(Config.NoteDetail, noteId), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query user's notebook
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<NotebookResult>> QueryNotebook(int page, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<NotebookResult>>(string.Format(Config.Notebooks, page), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query other's notebook
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<NotebookResult>> QueryOhterNotebook(string otherid,int page, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<NotebookResult>>(string.Format(Config.OhtersNotebooks, otherid, page), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query user's marked notes
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<BookmarkResult>> QueryBookmark(int page, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<BookmarkResult>>(string.Format(Config.Bookmark, page), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query notes that user likes
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<Note>> QueryLikeNotes(int page, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<Note>>(string.Format(Config.LikedNotes, userid, page), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Set user like the note
        /// </summary>
        /// <param name="noteid">Note Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<LikeResult> LikeNote(int noteid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpPostRequest<LikeResult>(string.Format(Config.LikeNote, noteid), "?",web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Set user unlike the note
        /// </summary>
        /// <param name="noteid">Note Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<LikeResult> UnlikeNote(int noteid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpPostRequest<LikeResult>(string.Format(Config.UnlikeNote, noteid), "?", web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Follow user
        /// </summary>
        /// <param name="followuserid">Follow User Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<FollowResult> FollowUser(string followuserid, string userid, string mobiletoken)
        {

            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpPostRequest<FollowResult>(string.Format(Config.Follow, followuserid), "?" ,web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Unfollow user
        /// </summary>
        /// <param name="followuserid">Follow User Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<FollowResult> UnFollowUser(string followuserid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpPostRequest<FollowResult>(string.Format(Config.UnFollow, followuserid),"?", web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="noteid">Note Id</param>
        /// <param name="content">Content</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<AddCommentResult> AddComment(string noteid, string content, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpPostRequest<AddCommentResult>(string.Format(Config.AddComment, noteid), "&content=" + content, web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query friends' activities
        /// </summary>
        /// <param name="startid">Start Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<ActivityResult>> QueryFriendActivities(int startid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = string.Format(Config.QueryFriendActivities, userid);
            if (startid != 0)
            {
                url += "&max_id=" + startid;
            }
            return await web.HttpGetRequest<List<ActivityResult>>(url, web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query subscription notes
        /// </summary>
        /// <param name="startid">Start Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<SubscriptionNotesResult>> QuerySubscriptionNotes(int startid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = Config.SubscriptionNotes;
            if (startid != 0)
            {
                url +=  "&max_received_at=" + startid;
            }
            return await web.HttpGetRequest<List<SubscriptionNotesResult>>(url, web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Subscribe collection
        /// </summary>
        /// <param name="dicscoverid"></param>
        /// <param name="userid"></param>
        /// <param name="mobiletoken"></param>
        /// <returns></returns>
        public async Task<DiscoverSubscribeResult> Subscribe(int discoverid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpPostRequest<DiscoverSubscribeResult>(string.Format( Config.Subscribe, discoverid), "?", web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Unsubscribe collection
        /// </summary>
        /// <param name="dicscoverid"></param>
        /// <param name="userid"></param>
        /// <param name="mobiletoken"></param>
        /// <returns></returns>
        public async Task<DiscoverSubscribeResult> Unsubscribe(int discoverid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpPostRequest<DiscoverSubscribeResult>(string.Format(Config.Unsubscribe, discoverid), "?", web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query message count
        /// </summary>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<MsgCountResult> QueryMsgCount(string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<MsgCountResult>(Config.UnReadCounts, web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query notifies your want
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="types">Tyeps you want</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<NotifyResult>> QueryNotify(int page, List<string> types, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = string.Format(Config.Notifications, page);
            if (types != null && types.Count > 0)
            {
                foreach (string str in types)
                {
                    url += "&types[]=" + str;
                }
            }
            return await web.HttpGetRequest<List<NotifyResult>>(url, web.GetHeaders(userid, mobiletoken));
        }
        
        /// <summary>
        /// Query feed message
        /// </summary>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<FeedsMsgResult>> QueryFeedMsg(string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<FeedsMsgResult>>(Config.FeedsMsg, web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query Subscription
        /// </summary>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <param name="index">Index</param>
        /// <returns></returns>
        public async Task<List<Subscription>> QuerySubscription(int index,string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<Subscription>>(string.Format(Config.Subscriptions, userid, index), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query Subscription
        /// </summary>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <param name="index">Index</param>
        /// <returns></returns>
        public async Task<List<Subscription>> QuerySubscription(int index, string otherid, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<Subscription>>(string.Format(Config.Subscriptions, otherid, index), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query Ohter's Subscription
        /// </summary>
        /// <param name="otherid">Other Id</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <param name="index">Index</param>
        /// <returns></returns>
        public async Task<List<Subscription>> QueryOhterSubscription(string otherid , string userid, string mobiletoken, int index)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<List<Subscription>>(string.Format(Config.Subscriptions, otherid, index), web.GetHeaders(userid, mobiletoken));
        }

        /// <summary>
        /// Query Topic
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="index">Index</param>
        /// <param name="userid">User Id</param>
        /// <param name="mobiletoken">Mobile Token</param>
        /// <returns></returns>
        public async Task<List<DiscoverResult>> QueryTopic(TopicType type, int index, string userid, string mobiletoken)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = string.Format(Config.Discover, index);
            if (type != TopicType.None)
            {
                url += "&category_id=" + (int)type;
            }
            return await web.HttpGetRequest<List<DiscoverResult>>(url, web.GetHeaders(userid, mobiletoken));
        }
    }
}

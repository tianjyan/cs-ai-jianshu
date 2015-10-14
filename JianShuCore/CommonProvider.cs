using JianShuCore.Model;
using JianShuCore.Provider;
using JianShuCore.WebAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore
{
    public class CommonProvider
    {
        /// <summary>
        /// Query hot topic notes
        /// </summary>
        /// <param name="frequency">Frequency</param>
        /// <param name="index">Index</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        public async Task<List<TrendingResult>> QueryDiscover(HotTopicType frequency, int index, int count, List<TrendingResult> seen = null)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string text = string.Format(Config.Trending, frequency.ToString(), index, count);
            if(seen?.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach(TrendingResult i in seen)
                {
                    sb.Append("&seen_note_ids[]=" + i.Id);
                }
                text += sb.ToString();
            }
            return await web.HttpGetRequest<List<TrendingResult>>(text, web.GetHeaders(null, null));
        }

        /// <summary>
        /// Query hot topic notes
        /// </summary>
        /// <param name="type">Frequency</param>
        /// <param name="count">Index</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        public async Task<List<TrendingResult>> QueryDiscover(TopicType type,int startid, int count, int limit)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = string.Format(Config.Recommendations, count, limit);
            if (type != TopicType.None)
            {
                url += "&collection_category_id=" + (int)type;
            }
            if (startid != 0)
            {
                url += "&max_recommended_at=" + startid;
            }
            return await web.HttpGetRequest<List<TrendingResult>>(url, web.GetHeaders(null, null));
        }

        /// <summary>
        /// Query topic notes
        /// </summary>
        /// <param name="type">Topic Type</param>
        /// <param name="startid">Start Id</param>
        /// <returns></returns>
        public async Task<List<RecommendationResult>> QueryDiscover(TopicType type, int startid)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = Config.Recommendations;
            if (type != TopicType.None)
            {
                url += "&collection_category_id=" + (int)type;
            }
            if (startid != 0)
            {
                url += "&max_recommended_at=" + startid;
            }
            return await web.HttpGetRequest<List<RecommendationResult>>(url, web.GetHeaders(null, null));
        }

        /// <summary>
        /// Query note detail
        /// </summary>
        /// <param name="noteId">Note Id</param>
        /// <returns></returns>
        public async Task<NoteDetailResult> QueryNoteDetail(string noteId)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<NoteDetailResult>(string.Format(Config.NoteDetail, noteId), web.GetHeaders(null,null));
        }

        /// <summary>
        /// Query comment
        /// </summary>
        /// <param name="noteId">Note Id</param>
        /// <param name="startid">Start Id</param>
        /// <returns></returns>
        public async Task<List<CommentsResult>> QueryComment(string noteId, int startid)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = string.Format(Config.GetComments, noteId, 20, 20);
            if (startid != 0)
            {
                url  += "&max_id=" + startid;
            }
            return await web.HttpGetRequest<List<CommentsResult>>(url, web.GetHeaders(null, null));
        }
        
        /// <summary>
        /// Query ohter's user info
        /// </summary>
        /// <param name="userid">User Id</param>
        /// <returns></returns>
        public async Task<UserBaseInfo> QueryUserInfo(string userid)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            return await web.HttpGetRequest<UserBaseInfo>(string.Format(Config.UserInfo, userid), web.GetHeaders(null, null));
        }

        /// <summary>
        /// Query topic's last notes
        /// </summary>
        /// <param name="topicId">Topic ID</param>
        /// <param name="startid">Start ID</param>
        /// <returns></returns>
        public async Task<List<TopicLastResult>> QueryTopicLast(string topicId, int startid)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string url = string.Format(Config.TopicNewestNotes, topicId);
            if (startid != 0)
            {
                url += "&max_id=" + startid;
            }
            return await web.HttpGetRequest<List<TopicLastResult>>(url, web.GetHeaders(null, null));
        }
    }
}

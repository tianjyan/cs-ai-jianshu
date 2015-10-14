using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class TrendingResult : BaseResult
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            set;
        }

        [JsonProperty("title")]
        public string Title
        {
            get;
            set;
        }

        [JsonProperty("slug")]
        public string Slug
        {
            get;
            set;
        }

        [JsonProperty("desc")]
        public string Desc
        {
            get;
            set;
        }

        [JsonProperty("first_shared_at")]
        public string FirstSharedAt
        {
            get;
            set;
        }

        [JsonProperty("last_compiled_at")]
        public string LastCompiledAt
        {
            get;
            set;
        }

        [JsonProperty("list_image")]
        public string ListImage
        {
            get;
            set;
        }

        [JsonProperty("notebook")]
        public Notebook Notebook
        {
            get;
            set;
        }

        [JsonProperty("commentable")]
        public bool Commentable
        {
            get;
            set;
        }

        [JsonProperty("comments_count")]
        public int CommentsCount
        {
            get;
            set;
        }

        [JsonProperty("likes_count")]
        public int LikesCount
        {
            get;
            set;
        }

        [JsonProperty("views_count")]
        public int ViewsCount
        {
            get;
            set;
        }

        [JsonProperty("total_rewards_count")]
        public int RewardsCount
        {
            get;
            set;
        }

        [JsonProperty("recommended_at")]
        public int RecommendedAt
        {
            get;
            set;
        }

        [JsonProperty("is_liked")]
        public bool IsLiked
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }
    }
}

using JianShuCore.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace JianShuCore.Model
{
    public class NoteDetailResult : BaseResult
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

        [JsonProperty("commentable")]
        public bool Commentable
        {
            get;
            set;
        }

        [JsonProperty("image_url")]
        public string ImageUrl
        {
            get;
            set;
        }

        [JsonProperty("share_image")]
        public string ShareImage
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

        [JsonProperty("mobile_content")]
        public string MobileContent
        {
            get;
            set;
        }

        [JsonProperty("is_bookmarked")]
        public bool IsBookmarked
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

        [JsonProperty("last_compiled_at")]
        public int LastCompiledAt
        {
            get;
            set;
        }

        [JsonProperty("visit_id")]
        public int VisitId
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

        [JsonProperty("wordage")]
        public int Wordage
        {
            get;
            set;
        }

        [JsonProperty("first_shared_at")]
        public int FirstSharedAt
        {
            get;
            set;
        }

        [JsonProperty("collections")]
        public List<Collection> Collections
        {
            get;
            set;
        }
    }
}

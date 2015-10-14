using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class UserBaseInfo
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            set;
        }

        [JsonProperty("email")]
        public object Email
        {
            get;
            set;
        }

        [JsonProperty("nickname")]
        public string Nickname
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

        [JsonProperty("avatar")]
        public string Avatar
        {
            get;
            set;
        }

        [JsonProperty("intro_compiled")]
        public string IntroCompiled
        {
            get;
            set;
        }

        [JsonProperty("is_following_user")]
        public bool IsFollowingUser
        {
            get;
            set;
        }

        [JsonProperty("is_followed_by_user")]
        public bool IsFollowedByUser
        {
            get;
            set;
        }

        [JsonProperty("total_likes_received")]
        public int TotalLikesReceived
        {
            get;
            set;
        }

        [JsonProperty("total_wordage")]
        public int TotalWordage
        {
            get;
            set;
        }

        [JsonProperty("homepage")]
        public object Homepage
        {
            get;
            set;
        }

        [JsonProperty("subscriptions_count")]
        public int SubscriptionsCount
        {
            get;
            set;
        }

        [JsonProperty("accesses")]
        public List<Access> Accesses
        {
            get;
            set;
        }

        [JsonProperty("notebooks_count")]
        public int NotebooksCount
        {
            get;
            set;
        }

        [JsonProperty("notes_count")]
        public int NotesCount
        {
            get;
            set;
        }

        [JsonProperty("owned_collections_count")]
        public int OwnedCollectionsCount
        {
            get;
            set;
        }

        [JsonProperty("editable_collections_count")]
        public int EditableCollectionsCount
        {
            get;
            set;
        }

        [JsonProperty("following_count")]
        public int FollowingCount
        {
            get;
            set;
        }

        [JsonProperty("followers_count")]
        public int FollowersCount
        {
            get;
            set;
        }

        [JsonProperty("liked_notes_count")]
        public int LikedNotesCount
        {
            get;
            set;
        }

        [JsonProperty("subscribing_notebooks_count")]
        public int SubscribingNotebooksCount
        {
            get;
            set;
        }

        [JsonProperty("subscribing_collections_count")]
        public int SubscribingCollectionsCount
        {
            get;
            set;
        }

        [JsonProperty("activities_count")]
        public int ActivitiesCount
        {
            get;
            set;
        }

        [JsonProperty("bookmarks_count")]
        public int BookmarksCount
        {
            get;
            set;
        }
    }
}

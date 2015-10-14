using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class LoginResult : BaseResult
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
        public object IntroCompiled
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

        [JsonProperty("mobile_token")]
        public string MobileToken
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
    }
}

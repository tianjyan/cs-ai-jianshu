using Newtonsoft.Json;

namespace SocialShare.Weibo.Model
{
    [JsonObject]
    public class Visible
    {
        [JsonProperty("type")]
        public int Type
        {
            get;
            set;
        }

        [JsonProperty("list_id")]
        public int ListId
        {
            get;
            set;
        }
    }
}

using Newtonsoft.Json;

namespace SocialShare.Weibo.Model
{
    [JsonObject]
    public class ShareResultBase
    {
        [JsonProperty("error")]
        public string Error
        {
            get;
            set;
        }

        [JsonProperty("error_code")]
        public int ErrorCode
        {
            get;
            set;
        }

        [JsonProperty("request")]
        public string Request
        {
            get;
            set;
        }

        public bool IsSuccess
        {
            get
            {
                return ErrorCode <= 0;
            }
        }
    }
}

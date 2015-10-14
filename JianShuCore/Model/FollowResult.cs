using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class FollowResult : BaseResult
    {
        [JsonProperty("following_count")]
        private int FollowingCount
        {
            get;
            set;
        }

        [JsonProperty("followers_count")]
        private int FollowersCount
        {
            get;
            set;
        }
    }
}

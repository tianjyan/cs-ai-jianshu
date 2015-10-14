using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class LikeResult
    {
        [JsonProperty("is_liked")]
        public bool isLiked
        {
            get;
            set;
        }

        [JsonProperty("count")]
        public int Count
        {
            get;
            set;
        }
    }
}

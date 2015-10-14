using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class DiscoverSubscribeResult : BaseResult
    {
        [JsonProperty("is_subscribed")]
        public bool IsSubscribed
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

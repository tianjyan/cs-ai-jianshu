using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class SnsNicknames
    {
        [JsonProperty("name")]
        public string weibo { get; set; }
    }
}

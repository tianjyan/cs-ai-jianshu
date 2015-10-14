using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class Access
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            set;
        }

        [JsonProperty("provider")]
        public string Provider
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

        [JsonProperty("uid")]
        public string Uid
        {
            get;
            set;
        }

        [JsonProperty("username")]
        public string Username
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public object Name
        {
            get;
            set;
        }

        [JsonProperty("snses")]
        public List<Sns> Snses
        {
            get;
            set;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class SourceObject
    {
        [JsonProperty("id")]
        public int Id
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

        [JsonProperty("compiled_content")]
        public string CompiledContent
        {
            get;
            set;
        }

        [JsonProperty("user")]
        public User User
        {
            get;
            set;
        }

        [JsonProperty("note")]
        public Note Note
        {
            get;
            set;
        }

        [JsonProperty("created_at")]
        public long? CreatedAt
        {
            get;
            set;
        }
    }
}

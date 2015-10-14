using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class CommentsResult : BaseResult
    {
        [JsonProperty("id")]
        public int Id
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

        [JsonProperty("note")]
        public Note Note
        {
            get;
            set;
        }

        [JsonProperty("created_at")]
        public int CreatedAt
        {
            get;
            set;
        }

        [JsonProperty("user")]
        public Userex User
        {
            get;
            set;
        }
    }
}


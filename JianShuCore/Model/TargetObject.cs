using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class TargetObject
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public string Name
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
        public int CreatedAt
        {
            get;
            set;
        }

        [JsonProperty("title")]
        public string Title
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

        [JsonProperty("desc")]
        public string Desc
        {
            get;
            set;
        }

        [JsonProperty("published_at")]
        public int? PublishedAt
        {
            get;
            set;
        }

        [JsonProperty("notebook")]
        public Notebook Notebook
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

        [JsonProperty("avatar")]
        public string Avatar
        {
            get;
            set;
        }
    }
}

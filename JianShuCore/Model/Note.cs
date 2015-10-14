using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class Note
    {
        [JsonProperty("id")]
        public int Id
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
        public int PublishedAt
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
    }
}

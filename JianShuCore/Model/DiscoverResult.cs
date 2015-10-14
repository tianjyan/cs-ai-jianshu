using JianShuCore.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class DiscoverResult
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

        [JsonProperty("newly_added_at")]
        public int NewlyAddedAt
        {
            get;
            set;
        }

        [JsonProperty("image")]
        public string Image
        {
            get;
            set;
        }

        [JsonProperty("description")]
        public string Description
        {
            get;
            set;
        }

        [JsonProperty("subscribers_count")]
        public int SubscribersCount
        {
            get;
            set;
        }

        [JsonProperty("is_subscribed")]
        public bool IsSubscribed
        {
            get;
            set;
        }

        [JsonProperty("notes_count")]
        public int NotesCount
        {
            get;
            set;
        }

        [JsonProperty("owner")]
        public Owner Owner
        {
            get;
            set;
        }

        [JsonProperty("coeditors")]
        public List<Coeditor> Coeditors
        {
            get;
            set;
        }
    }
}

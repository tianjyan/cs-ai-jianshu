using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class NotebookResult
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

        [JsonProperty("notes_count")]
        public int NotesCount
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

        [JsonProperty("is_subscribing")]
        public bool IsSubscribing
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
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class Subscription
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("newly_added_at")]
        public string NewlyAddedAt { get; set; }

        [JsonProperty("description")]
        public string Desc { get; set; }

        [JsonProperty("wordage")]
        public string Wordage { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("is_subscribed")]
        public bool IsSubscribed { get; set; }

        [JsonProperty("notes_count")]
        public int NotesCount { get; set; }

        [JsonProperty("submission_count")]
        public int SubmissionCount { get; set; }

        [JsonProperty("subscribers_count")]
        public int SubscribersCount { get; set; }

        [JsonProperty("tags")]
        public object Tag { get; set; }

        [JsonProperty("can_contribute")]
        public bool CanContribute { get; set; }

        [JsonProperty("audit_contribute")]
        public bool AuditContribute { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }
    }
}

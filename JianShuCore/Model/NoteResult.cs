using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class NoteResult
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

        [JsonProperty("first_shared_at")]
        public int? FirstSharedAt
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

        [JsonProperty("seq_in_nb")]
        public int SeqInNb
        {
            get;
            set;
        }

        [JsonProperty("note_type")]
        public string NoteType
        {
            get;
            set;
        }

        [JsonProperty("shared")]
        public bool Shared
        {
            get;
            set;
        }

        [JsonProperty("notebook_id")]
        public int NotebookId
        {
            get;
            set;
        }

        [JsonProperty("last_compiled_at")]
        public int LastCompiledAt
        {
            get;
            set;
        }

        [JsonProperty("content_updated_at")]
        public int ContentUpdatedAt
        {
            get;
            set;
        }
    }
}

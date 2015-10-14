using Newtonsoft.Json;

namespace JianShuCore.Model
{
    public class TopicLastResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("collection")]
        public Collection Collection { get; set; }

        [JsonProperty("note")]
        public Note Note { get; set; }
    }
}

using JianShuCore.Model;

namespace AiJianShu.Model
{
    public class DiscoverItem
    {
        public string Content { get; set; }
        public TopicType Type { get; set; }
        public HotTopicType HotType { get; set; }
        public object Tag { get; set; }
    }
}

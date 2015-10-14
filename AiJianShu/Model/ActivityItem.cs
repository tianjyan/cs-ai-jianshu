using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiJianShu.Model
{
    public class ActivityItem
    {
        public Tuple<string,string> UserLink { get; set; }
        public Tuple<string, string> TargetLink { get; set; }
        public int ActivityId { get; set; }
        public string NickName { get; set; }
        public Uri Avatar { get; set; }
        public ActivityType Type { get; set; }
        public string Target { get; set; } = "";
        public string Content { get; set; }
        public int CreatedAt { get; set; }
    }
}

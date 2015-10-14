using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiJianShu.Model
{
    public class ChangeView
    {
        public ViewType FromView { get; set; }
        public ViewType ToView { get; set; }
        public object Context { get; set; }
        public EventType Event { get; set; }
    }
}

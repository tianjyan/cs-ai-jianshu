using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class NotifyResult
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            set;
        }

        [JsonProperty("notifiable")]
        public Notifiable Notifiable
        {
            get;
            set;
        }

        [JsonProperty("is_read")]
        public bool IsRead
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
    }
}
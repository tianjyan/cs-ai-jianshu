using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCoreTest.ModelTest
{
    public class StorageModel
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "Name";
        [JsonProperty("value")]
        public string Value { get; set; } = "Value";

        
        public override bool Equals(object obj)
        {
            StorageModel value = obj as StorageModel;
            if(value != null && value.Name == this.Name && value.Value == this.Value)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

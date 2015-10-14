using SQLite.Net.Attributes;

namespace JianShuCore.Common
{
    /// <summary>
    /// Model for cache
    /// </summary>
    public class CacheModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Type { get; set; }
        public string TypeID { get; set; }
        public string Content { get; set; }
    }
}

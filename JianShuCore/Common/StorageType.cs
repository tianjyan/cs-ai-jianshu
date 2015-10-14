namespace JianShuCore.Common
{
    public enum StorageType
    {
        [Description("None")]
        None,
        [Description("JianShuCore.Storage.IsolatedStorage")]
        IsolatedStorage,
        [Description("JianShuCore.Storage.SqliteStorage")]
        SqliteStorage
    }
}

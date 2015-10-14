namespace JianShuCore.Common
{
    public enum StorageTypeAsync
    {
        [Description("None")]
        None,
        [Description("JianShuCore.Storage.IsolatedStorageAsync")]
        IsolatedStorageAsync,
        [Description("JianShuCore.Storage.SqliteStorageAsync")]
        SqliteStorageAsync
    }
}

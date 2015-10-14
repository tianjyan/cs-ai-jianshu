namespace JianShuCoreTest
{
    internal static class Config
    {
        internal const string username = "jianshuapptest@sina.com";
        internal const string password = "jianshutest";
        internal const string JianShuLogin = "http://api.jianshu.io/v1/users/signin";
        internal const string Trending = "http://api.jianshu.io/v1/trending/{0}?&page={1}&count={2}";
        internal const string NoteDetail = "http://api.jianshu.io/v1/notes/{0}?&font_size=normal&read_mode=day";
    }
}

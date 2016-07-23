namespace JianShuCore.WebAPI
{
    internal static class Config
    {
        private const string Server = "http://api.jianshu.io/v1";

        internal const string APP_VERSION = "1.9.1";

        internal const string APP_NAME = "haruki";

        internal const string Login = "http://api.jianshu.io/v1/users/signin";

        internal const string Register = "http://api.jianshu.io/v1/users/signup";

        internal const string BaseInfo = "http://api.jianshu.io/v1/write/users/mine?";

        internal const string QueryActivities = @"http://api.jianshu.io/v1/users/{0}/activities?
                                            count=20&limit=20&exclude_types[]=collection-add_note-collectionnote&
                                            exclude_types[]=collection-approve_note-collectionsubmission&
                                            exclude_types[]=notebook-share_note-note&limit=20";

        internal const string Following = "http://api.jianshu.io/v1/users/{0}/following?page={1}";

        internal const string Followers = "http://api.jianshu.io/v1/users/{0}/followers?page={1}";

        internal const string Notes = "http://api.jianshu.io/v1/write/notes?page={0}&shared={1}";

        internal const string Notebooks = "http://api.jianshu.io/v1/write/notebooks?page={0}";

        internal const string OhtersNotebooks = "http://api.jianshu.io/v1/users/{0}/notebooks?page={1}";

        internal const string Bookmark = "http://api.jianshu.io/v1/bookmarks?page={0}";

        internal const string LikedNotes = "http://api.jianshu.io/v1/users/{0}/liked_notes?page={1}";

        internal const string LikeNote = "http://api.jianshu.io/v1/notes/{0}/like?";

        internal const string UnlikeNote = "http://api.jianshu.io/v1/notes/{0}/unlike?";

        internal const string UnFollow = "http://api.jianshu.io/v1/users/{0}/unfollow";

        internal const string Follow = "http://api.jianshu.io/v1/users/{0}/follow";

        internal const string AddComment = "http://api.jianshu.io/v1/notes/{0}/comments";

        internal const string QueryFriendActivities = @"http://api.jianshu.io/v1/feeds?
                                                    exclude_types[]=collection-add_note-collectionnote&exclude_types[]=collection-approve_note-collectionsubmission
                                                    &exclude_types[]=notebook-share_note-note&count=20&limit=20";

        internal const string SubscriptionNotes = "http://api.jianshu.io/v1/subscription_notes?count=20&limit=20";

        internal const string Subscribe = "http://api.jianshu.io/v1/collections/{0}/subscribe";

        internal const string Unsubscribe = "http://api.jianshu.io/v1/collections/{0}/unsubscribe";

        internal const string Notifications = "http://api.jianshu.io/v1/notifications?page={0}&all=1";

        internal const string UnReadCounts = @"http://api.jianshu.io/v1/notifications/unread_counts?
                                            types[]=user-comment_on_note-comment&types[]=comment-mention_somebody-user&
                                            types[]=user-like_something-note&types[]=user-like_something-user&
                                            types[]=user-like_something-collection&types[]=user-like_something-notebook&
                                            types[]=note-recommend_by_editor&types[]=collection-approve_note-collectionsubmission&
                                            types[]=collection-decline_note-collectionsubmission&types[]=collection-add_editor-user&
                                            types[]=collection-remove_editor-user&types[]=note-locked_by_editor&
                                            types[]=collection-add_note-collectionnote&types[]=collection-contribute_note-collectionsubmission";

        internal const string FeedsMsg = "http://api.jianshu.io/v1/feeds/updates?";

        internal const string Trending = "http://api.jianshu.io/v2/trending/{0}?&page={1}&count={2}";

        internal const string NoteDetail = "http://api.jianshu.io/v1/notes/{0}?&font_size=normal&read_mode=day";

        internal const string GetComments = "http://api.jianshu.io/v1/notes/{0}/comments?count={1}&limit={2}&order=desc";

        internal const string Recommendations = "http://api.jianshu.io/v2/recommendations/notes?count={0}&limit={1}";

        internal const string Discover = "http://api.jianshu.io/v1/collections?page={0}";

        internal const string Subscriptions = "http://api.jianshu.io/v1/users/{0}/subscriptions?types=collection&count=10&page={1}";

        internal const string TopicNewestNotes = "http://api.jianshu.io/v1/collections/{0}/notes/latest?&limit=10";


        internal const string Weibo_GetUserInfo = "https://api.weibo.com/2/users/show.json?uid={0}&access_token={1}";

        internal const string ThirdPartLogin = "http://api.jianshu.io/v1/users/signin_with_third_party";

        internal const string MobileToken = "http://api.jianshu.io/v1/users/{0}/mobile_token";        

        internal const string CollectionCategories = @"http://api.jianshu.io/v1/collection_categories.json?
                                                    app[name]=haruki&app[version]1.6.2&auth1={0}&auth2={1}
                                                    &device[guid]={2}&timestamp={3}&type=for_recommendations&user_id={4}";

        internal const string ZoneLastestNotes = "http://api.jianshu.io/v1/notes/latest?page={0}";


        internal const string UserInfo = "http://api.jianshu.io/v1/users/{0}?";
    }
}

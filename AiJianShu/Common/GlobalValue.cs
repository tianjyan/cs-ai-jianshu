using AiJianShu.Model;
using JianShuCore.Model;
using SocialShare.Weibo.Model;
using System;

namespace AiJianShu.Common
{
    public static class GlobalValue
    {
        #region 全局静态变量
        public readonly static Uri DefaultAvatar = new Uri("ms-appx:///Assets/default_avatar.png", UriKind.Absolute);

        public static UserContext CurrentUserContext;
        public static UserInfo CurrentWeiboUserInfo;
        public static UserBaseInfo CurrentUserBaseInfo;
        #endregion
    }
}

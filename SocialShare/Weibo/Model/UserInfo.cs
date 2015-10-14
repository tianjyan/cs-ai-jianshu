using System;

namespace SocialShare.Weibo.Model
{
    public class UserInfo
    {
        public string Token { get; set; }
        public string Uid { get; set; }
        public long ExpiresAt { get; set; }

        public bool CheckUseable()
        {
            return !string.IsNullOrEmpty(Token) &&
                !string.IsNullOrEmpty(Uid) &&
                Untils.FromTimestamp(ExpiresAt) > DateTime.Now;
        }
    }
}

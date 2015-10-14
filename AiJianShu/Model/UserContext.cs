namespace AiJianShu.Model
{
    public class UserContext
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public string MobileToken { get; set; }
        public string Avatar { get; set; }
        public bool IsLogin { get; set; }
        public object Tag { get; set; }
        public string Slug { get; set; }
        //构造函数，初始化Avatar
        public UserContext()
        {
            Avatar = "ms-appx:///Assets/default_avatar.png";
        }
    }
}

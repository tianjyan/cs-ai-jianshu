using AiJianShu.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JianShuCore;
using JianShuCore.Model;
using System.Windows.Input;

namespace AiJianShu.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region 字段
        private string username;
        #endregion

        #region 属性
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                RaisePropertyChanged();
            }
        }

        public ICommand LoginCommand { get; set; }
        #endregion

        #region 构造函数
        public LoginViewModel()
        {
            InitCMD();
        }
        #endregion

        #region 私有方法
        #region 初始化
        private void InitCMD()
        {
            LoginCommand = new RelayCommand<string>(ExecuteLoginCommand);
        }
        #endregion

        #region 命令
        private async void ExecuteLoginCommand(string password)
        {
            UserContext user = new UserContext();
            user.Username = Username;
            user.Password = password;

            UserContentProvider provider = new UserContentProvider();
            LoginResult result =await provider.Login(user.Username, user.Password);
            if(result.Error?.Count >= 0)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ShowMessage() { MessageContent = result.Error[0].Message });
            }
            else
            {
                user.UserId = result.Id.ToString();
                user.MobileToken = result.MobileToken;
                user.IsLogin = true;
                user.Avatar = result.Avatar;
                user.Slug = result.Slug;

                this.MessengerInstance.Send(new ChangeView() { FromView = ViewType.Login, ToView= ViewType.Home, Event = EventType.LoginSuccess,Context = user });
            }
        }
        #endregion
        #endregion
    }
}

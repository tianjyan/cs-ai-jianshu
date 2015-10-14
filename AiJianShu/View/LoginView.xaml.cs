using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AiJianShu.View
{
    public sealed partial class LoginView : UserControl
    {
        public LoginView()
        {
            this.InitializeComponent();
            this.Loaded += LoginViewLoaded;
        }

        private void LoginViewLoaded(object sender, RoutedEventArgs e)
        {
            if (Common.GlobalValue.CurrentUserContext != null)
            {
                UserName.Text = Common.GlobalValue.CurrentUserContext.Username;
                pwd.Password = Common.GlobalValue.CurrentUserContext.Password;
            }
        }
    }
}

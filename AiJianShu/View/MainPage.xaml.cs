using AiJianShu.Model;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace AiJianShu
{
    public sealed partial class MainPage : Page
    {
        private SolidColorBrush OpenedBrush;
        private SolidColorBrush ClosedBrush;
        private Storyboard ShowStoryboard;
        private Storyboard HiddenStoryboard;

        public MainPage()
        {
            this.InitializeComponent();
            OpenedBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x2A, 0x2A, 0x2A));
            ClosedBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0x9A, 0x7A));
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = Colors.Black;
                statusBar.BackgroundOpacity = 0.8;
                statusBar.ForegroundColor = Colors.White;
                statusBar.ShowAsync();
            }

            ShowStoryboard = this.Resources["ShowMessage"] as Storyboard;
            HiddenStoryboard = this.Resources["HiddenMessage"] as Storyboard;
            Messenger.Default.Register<ShowMessage>(this, ShowMessageAsync);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
            PanelButton.Background = splitView.IsPaneOpen ? OpenedBrush : ClosedBrush;
        }

        private async void ShowMessageAsync(ShowMessage message)
        {
            MessageText.Text = message.MessageContent;
            ShowStoryboard.Begin();
            await Task.Delay(2000);
            HiddenStoryboard.Begin();
        }

        private void SplitViewPaneClosed(SplitView sender, object args)
        {
            PanelButton.Background = ClosedBrush;
        }
    }
}

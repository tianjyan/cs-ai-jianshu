using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AiJianShu.View
{
    public sealed partial class FriendsView : UserControl
    {
        static double VerticalOffset;
        public FriendsView()
        {
            this.InitializeComponent();
            this.Unloaded += FriendsViewUnloaded;
            this.Loaded += FriendsViewLoaded;
        }

        private void FriendsViewLoaded(object sender, RoutedEventArgs e)
        {
            Left.SetVerticalOffset(VerticalOffset);
        }

        private void FriendsViewUnloaded(object sender, RoutedEventArgs e)
        {
            VerticalOffset = Left.GetVerticalOffset();
        }
    }
}

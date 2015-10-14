using AiJianShu.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AiJianShu.View
{
    public sealed partial class About : UserControl
    {
        private FrameworkElement Right;
        public About()
        {
            this.InitializeComponent();
            this.Loaded += AboutLoaded;
        }

        private void AboutLoaded(object sender, RoutedEventArgs e)
        {
            switch (Untils.DeviceFamily)
            {
                case "Desktop":
                    FindName(nameof(Right1));
                    Right = Right1;
                    break;
                case "Mobile":
                    FindName(nameof(Right2));
                    Right = Right2;
                    break;
                default:
                    FindName(nameof(Right1));
                    Right = Right1;
                    break;
            }
        }
    }
}

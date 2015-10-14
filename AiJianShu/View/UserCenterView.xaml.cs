using AiJianShu.Common;
using AiJianShu.ViewModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AiJianShu.View
{
    public sealed partial class UserCenterView : UserControl
    {
        private bool RightEverShowed;
        private const double FullWidth = 720;
        private const double MinListWidth = 320;
        private FrameworkElement Right;

        public UserCenterView()
        {
            this.InitializeComponent();
            this.Loaded += UserCenterViewLoaded;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                this.Unloaded += UserCenterViewUnloaded;
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsBackPressed;
            }
        }

        private void UserCenterViewUnloaded(object sender, RoutedEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonsBackPressed;
        }

        private void UserCenterViewLoaded(object sender, RoutedEventArgs e)
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

            this.Layoutroot.SizeChanged += LayoutrootSizeChanged;
            ResotreLayout();
        }

        #region 处理布局
        private void HardwareButtonsBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (btn_Back.Visibility == Visibility.Visible)
            {
                e.Handled = true;
                BackClick(null, null);
            }
            else
            {
                e.Handled = MainViewModel.userCenterViewModel.BackPreView();
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            ContentColumnDefinition1.Width = new GridLength(1, GridUnitType.Star);
            ContentColumnDefinition2.Width = new GridLength(0, GridUnitType.Pixel);
            btn_Back.Visibility = Visibility.Collapsed;
            Right.Visibility = Visibility.Collapsed;
            RightEverShowed = false;
        }

        private void LayoutrootSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Layoutroot.ActualWidth > FullWidth)
            {
                btn_Back.Visibility = Visibility.Collapsed;

                ContentColumnDefinition1.Width = new GridLength(320, GridUnitType.Pixel);
                ContentColumnDefinition2.Width = new GridLength(1, GridUnitType.Star);

                Right.Visibility = Visibility.Visible;
            }
            else if (!RightEverShowed)
            {
                ContentColumnDefinition1.Width = new GridLength(1, GridUnitType.Star);
                ContentColumnDefinition2.Width = new GridLength(0, GridUnitType.Pixel);
                btn_Back.Visibility = Visibility.Collapsed;
                Right.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_Back.Visibility = Visibility.Visible;
                ContentColumnDefinition1.Width = new GridLength(0, GridUnitType.Pixel);
                ContentColumnDefinition2.Width = new GridLength(1, GridUnitType.Star);
                Right.Visibility = Visibility.Visible;
            }
        }

        private void ResotreLayout()
        {
            btn_Back.Visibility = Visibility.Collapsed;
            Left.Visibility = Visibility.Visible;
            RightEverShowed = false;

            if (Layoutroot.ActualWidth > FullWidth)
            {
                ContentColumnDefinition1.Width = new GridLength(320, GridUnitType.Pixel);
                ContentColumnDefinition2.Width = new GridLength(1, GridUnitType.Star);
                Right.Visibility = Visibility.Visible;
            }
            else
            {
                ContentColumnDefinition1.Width = new GridLength(1, GridUnitType.Star);
                ContentColumnDefinition2.Width = new GridLength(0, GridUnitType.Pixel);
                Right.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        private void RightClick(object sender, RoutedEventArgs e)
        {
            RightEverShowed = true;

            if (Right.Visibility == Visibility.Collapsed)
            {
                Right.Visibility = Visibility.Visible;

                ContentColumnDefinition1.Width = new GridLength(0, GridUnitType.Pixel);
                ContentColumnDefinition2.Width = new GridLength(1, GridUnitType.Star);

                btn_Back.Visibility = Visibility.Visible;
            }
            else
            {
                btn_Back.Visibility = Visibility.Collapsed;
            }

            int index = Convert.ToInt32(((sender as Button).Tag));

            if(Untils.DeviceFamily == "Mobile")
            {
                pivot.SelectedIndex = index - 1;
            }
            else
            {
                switch(index)
                {
                    case 1:
                        RB1.IsChecked = true;
                        break;
                    case 2:
                        RB2.IsChecked = true;
                        break;
                    case 3:
                        RB3.IsChecked = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void BackViewClick(object sender, RoutedEventArgs e)
        {
            MainViewModel.userCenterViewModel.BackPreView();
        }
    }
}

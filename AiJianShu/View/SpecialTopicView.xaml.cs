using AiJianShu.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace AiJianShu.View
{
    public sealed partial class SpecialTopicView : UserControl
    {
        //标志位:标志文章内容部分是否打开过
        private bool RightEverShowed;
        private GridLength GridZeroPixel = new GridLength(0, GridUnitType.Pixel);
        private GridLength GridOneStar = new GridLength(1, GridUnitType.Star);
        private GridLength Grid320Pixel = new GridLength(320, GridUnitType.Pixel);
        private const double FullWidth = 720;
        private const double MinListWidth = 320;
        private const double Offfset = 35;
        private object PreSelecteItem;
        internal static double leftOffset;
        internal static double rightOffset;
        internal static bool rightCanShow;
        public SpecialTopicView()
        {
            this.InitializeComponent();
            this.Loaded += SpecialTopicViewLoaded;
            this.Unloaded += SpecialTopicViewUnloaded;

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsBackPressed;
            }
        }

        private void SpecialTopicViewUnloaded(object sender, RoutedEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonsBackPressed;
            }
            leftOffset = Left.GetVerticalOffset();
            rightOffset = rightLv.GetVerticalOffset();
        }

        #region 处理布局
        private void HardwareButtonsBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (btn_Back.Visibility == Visibility.Visible)
            {
                e.Handled = true;
                BackButtonClick(null, null);
                btn_Back.Visibility = Visibility.Collapsed;
            }
        }

        private void SpecialTopicViewLoaded(object sender, RoutedEventArgs e)
        {
            this.Layoutroot.SizeChanged += LayoutrootSizeChanged;
            ResotreLayout();
            Left.SetVerticalOffset(leftOffset);
            if (rightCanShow)
            {
                Right.Opacity = 1;
                rightLv.SetVerticalOffset(rightOffset);
                RightEverShowed = true;
                LayoutrootSizeChanged(null, null);
            }
        }

        private void LayoutrootSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //如果调整宽度大最大的预期宽度时.
            //将列表宽度设置为MinListWidth,并扩展文章内容填满空间
            if (Layoutroot.ActualWidth > FullWidth)
            {
                ContentColumnDefinition1.Width = Grid320Pixel;
                ContentColumnDefinition2.Width = GridOneStar;
                btn_Back.Visibility = Visibility.Collapsed;
            }
            //调整宽度时,如果从未显示过文章内容,则只改变列表的宽度
            else if (!RightEverShowed)
            {
                ContentColumnDefinition1.Width = GridOneStar;
                ContentColumnDefinition2.Width = GridZeroPixel;
                btn_Back.Visibility = Visibility.Collapsed;
            }
            //如果文章内容曾经显示过,则隐藏列表
            else
            {
                ContentColumnDefinition1.Width = GridZeroPixel;
                ContentColumnDefinition2.Width = GridOneStar;
                btn_Back.Visibility = Visibility.Visible;
            }
        }

        private void LeftSelectionChanged(object sender, TappedRoutedEventArgs e)
        {
            var selectedItem = (sender as ListView).SelectedItem;

            PreSelecteItem = selectedItem;


            rightCanShow = true;
            RightEverShowed = true;

            //如果布局不够,文章内容处于隐藏状态时,隐藏列表并显示后退按钮
            if (ContentColumnDefinition2.Width == GridZeroPixel)
            {
                ContentColumnDefinition1.Width = GridZeroPixel;
                ContentColumnDefinition2.Width = GridOneStar;
                btn_Back.Visibility = Visibility.Visible;
            }
            else
            {
                btn_Back.Visibility = Visibility.Collapsed;
            }

            Right.Opacity = 1;

        }

        //按后退键,隐藏文章内容按钮和后退按钮,显示列表并将其扩展到整个空间
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            ContentColumnDefinition1.Width = GridOneStar;
            ContentColumnDefinition2.Width = GridZeroPixel;
            RightEverShowed = false;
        }

        //初始化布局,默认开始显示列表
        private void ResotreLayout()
        {
            //置位列表显示和后退键隐藏
            btn_Back.Visibility = Visibility.Collapsed;
            //标记文章内容从未显示过
            RightEverShowed = false;

            //当布局的最大时,显示全部内容
            if (Layoutroot.ActualWidth > FullWidth)
            {
                ContentColumnDefinition1.Width = Grid320Pixel;
                ContentColumnDefinition2.Width = GridOneStar;
            }
            //布局不够时,只显示列表.
            else
            {
                ContentColumnDefinition1.Width = GridOneStar;
                ContentColumnDefinition2.Width = GridZeroPixel;
            }
        }
        #endregion

        private void PullToRefreshListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var temp = sender as ListView;
            if (temp.SelectedItem == null) return;
            string id = ((sender as ListView).SelectedItem as TopicLastItem).NoteId.ToString();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ChangeView() { Context = id, Event = EventType.Article, FromView = ViewType.SpecialTopic, ToView = ViewType.Article });
        }
    }
}

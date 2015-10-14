using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace AiJianShu.View
{
    public sealed partial class HomeView : UserControl
    {
        #region 字段
        //标志位:标志文章内容部分是否打开过
        private bool RightEverShowed;
        private GridLength GridZeroPixel = new GridLength(0, GridUnitType.Pixel);
        private GridLength GridOneStar = new GridLength(1, GridUnitType.Star);
        private GridLength Grid320Pixel = new GridLength(320, GridUnitType.Pixel);
        private const double FullWidth= 720;
        private const double MinListWidth = 320;
        private const double Offfset = 35;
        private object PreSelecteItem;
        #endregion

        #region 构造函数
        public HomeView()
        {
            this.InitializeComponent();
            Loaded += HomeViewLoaded;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                this.Unloaded += HomeViewUnloaded;
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonBackPressed;
            }
        }

        private void HomeViewUnloaded(object sender, RoutedEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonBackPressed;
        }
        #endregion

        #region 处理布局
        private void HardwareButtonBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if(ArticleContorl.BackBtnVisibility == Visibility.Visible)
            {
                e.Handled = true;
                if (ArticleContorl.CommentOpened)
                {
                    ArticleContorl.CloseCommentPanel();
                }
                else
                {
                    BackButtonClick(null, null);
                    ArticleContorl.BackBtnVisibility = Visibility.Collapsed;
                }
            }
        }

        private void HomeViewLoaded(object sender, RoutedEventArgs e)
        {
            this.Layoutroot.SizeChanged += LayoutrootSizeChanged;
            ResotreLayout();
        }

        private void LayoutrootSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //如果调整宽度大最大的预期宽度时.
            //将列表宽度设置为MinListWidth,并扩展文章内容填满空间
            if (Layoutroot.ActualWidth > FullWidth)
            {
                ContentColumnDefinition1.Width = Grid320Pixel;
                ContentColumnDefinition2.Width = GridOneStar;
                ArticleContorl.BackBtnVisibility = Visibility.Collapsed;
            }
            //调整宽度时,如果从未显示过文章内容,则只改变列表的宽度
            else if(!RightEverShowed)
            {
                ContentColumnDefinition1.Width = GridOneStar;
                ContentColumnDefinition2.Width = GridZeroPixel;
                ArticleContorl.BackBtnVisibility = Visibility.Collapsed;
            }
            //如果文章内容曾经显示过,则隐藏列表
            else
            {
                ContentColumnDefinition1.Width  = GridZeroPixel;
                ContentColumnDefinition2.Width = GridOneStar;
                ArticleContorl.BackBtnVisibility = Visibility.Visible;
            }
        }

        private void LeftSelectionChanged(object sender, TappedRoutedEventArgs e)
        {
            var selectedItem = (sender as ListView).SelectedItem;

            PreSelecteItem = selectedItem;

            RightEverShowed = true;

            //如果布局不够,文章内容处于隐藏状态时,隐藏列表并显示后退按钮
            if (ContentColumnDefinition2.Width == GridZeroPixel)
            {
                ContentColumnDefinition1.Width = GridZeroPixel;
                ContentColumnDefinition2.Width = GridOneStar;
                ArticleContorl.BackBtnVisibility = Visibility.Visible;
            }
            else
            {
                ArticleContorl.BackBtnVisibility = Visibility.Collapsed;
            }
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
            ArticleContorl.BackBtnVisibility = Visibility.Collapsed;
            //标记文章内容从未显示过
            RightEverShowed = false;

            //当布局的最大时,显示全部内容
            if(Layoutroot.ActualWidth > FullWidth)
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
    }
}

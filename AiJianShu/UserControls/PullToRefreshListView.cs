using AiJianShu.Command;
using System;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AiJianShu.UserControls
{
    public sealed class PullToRefreshListView : ListView
    {

        #region 公有方法
        public double GetVerticalOffset()
        {
            return RootScrollViewer.VerticalOffset;
        }

        public void SetVerticalOffset(double offset)
        {
            if (RootScrollViewer != null)
            {
                RootScrollViewer.ChangeView(default(double?), offset, default(float?));
            }
        }
        #endregion

        #region 构造函数
        public PullToRefreshListView()
        {
            this.DefaultStyleKey = typeof(PullToRefreshListView);
            Loaded += PullToRefreshScrollViewerLoaded;
        }
        #endregion

        #region 事件
        public event EventHandler RefreshContent;
        public event EventHandler MoreContent;
        #endregion

        #region 依赖属性
        public static readonly DependencyProperty PullPartTemplateProperty =
            DependencyProperty.Register("PullPartTemplate", typeof(string), typeof(PullToRefreshListView), new PropertyMetadata(App.Current.Resources["Pull"]));
        public static readonly DependencyProperty ReleasePartTemplateProperty =
            DependencyProperty.Register("ReleasePartTemplate", typeof(string), typeof(PullToRefreshListView), new PropertyMetadata(App.Current.Resources["Release"]));
        public static readonly DependencyProperty RefreshHeaderHeightProperty =
            DependencyProperty.Register("RefreshHeaderHeight", typeof(double), typeof(PullToRefreshListView), new PropertyMetadata(30D));
        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(PullToRefreshListView), new PropertyMetadata(null));
        public static readonly DependencyProperty MoreCommandProperty =
            DependencyProperty.Register("MoreCommand", typeof(ICommand), typeof(PullToRefreshListView), new PropertyMetadata(null));
        public static readonly DependencyProperty ArrowColorProperty =
            DependencyProperty.Register("ArrowColor", typeof(Brush), typeof(PullToRefreshListView), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
        #endregion

        #region 属性
        public string PullPartTemplate
        {
            get
            {
                return (string)base.GetValue(PullToRefreshListView.PullPartTemplateProperty);
            }
            set
            {
                base.SetValue(PullToRefreshListView.PullPartTemplateProperty, value);
            }
        }

        public string ReleasePartTemplate
        {
            get
            {
                return (string)base.GetValue(PullToRefreshListView.ReleasePartTemplateProperty);
            }
            set
            {
                base.SetValue(PullToRefreshListView.ReleasePartTemplateProperty, value);
            }
        }

        public double RefreshHeaderHeight
        {
            get
            {
                return (double)base.GetValue(PullToRefreshListView.RefreshHeaderHeightProperty);
            }
            set
            {
                base.SetValue(PullToRefreshListView.RefreshHeaderHeightProperty, value);
            }
        }

        public AsyncCommand RefreshCommand
        {
            get
            {
                return (AsyncCommand)base.GetValue(PullToRefreshListView.RefreshCommandProperty);
            }
            set
            {
                base.SetValue(PullToRefreshListView.RefreshCommandProperty, value);
            }
        }

        public AsyncCommand MoreCommand
        {
            get
            {
                return (AsyncCommand)base.GetValue(PullToRefreshListView.MoreCommandProperty);
            }
            set
            {
                base.SetValue(PullToRefreshListView.MoreCommandProperty, value);
            }
        }

        public Brush ArrowColor
        {
            get
            {
                return (Brush)base.GetValue(PullToRefreshListView.ArrowColorProperty);
            }
            set
            {
                base.SetValue(PullToRefreshListView.ArrowColorProperty, value);
            }
        }
        #endregion

        #region 字段
        private double OffsetTreshhold = 40;
        private DispatcherTimer CompressionTimer;
        private ScrollViewer RootScrollViewer;
        private DispatcherTimer Timer;
        private Grid ContainerGrid;
        private Border PullToRefreshIndicator;
        private bool IsCompressionTimerRunning;
        private bool IsReadyToRefresh;
        private bool IsCompressedEnough;
        private bool isBusy;
        #endregion

        #region 重写
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RootScrollViewer = base.GetTemplateChild("ScrollViewer") as ScrollViewer;
            RootScrollViewer.ViewChanging += ScrollViewerViewChanging;
            RootScrollViewer.ViewChanged += ScrollViewerViewChanged;
            RootScrollViewer.Margin = new Thickness(0, 0, 0, -RefreshHeaderHeight);
            RootScrollViewer.RenderTransform = new CompositeTransform() { TranslateY = -RefreshHeaderHeight };

            ContainerGrid = base.GetTemplateChild("ContainerGrid") as Grid;

            PullToRefreshIndicator = GetTemplateChild("PullToRefreshIndicator") as Border;
            SizeChanged += OnSizeChanged;
        }
        #endregion

        #region 路由事件
        private void PullToRefreshScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(100);
            Timer.Tick += TimerTick;

            CompressionTimer = new DispatcherTimer();
            CompressionTimer.Interval = TimeSpan.FromSeconds(1);
            CompressionTimer.Tick += CompressionTimerTick;

            Timer.Start();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
            };
        }

        private void ScrollViewerViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (e.NextView.VerticalOffset == 0)
            {
                Timer.Start();
            }
            else
            {
                if (Timer != null)
                {
                    Timer.Stop();
                }

                if (CompressionTimer != null)
                {
                    CompressionTimer.Stop();
                }

                IsCompressionTimerRunning = false;
                IsCompressedEnough = false;
                IsReadyToRefresh = false;

                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        private void TimerTick(object sender, object e)
        {
            if (ContainerGrid != null)
            {
                Rect elementBounds = PullToRefreshIndicator.TransformToVisual(ContainerGrid).TransformBounds(new Rect(0.0, 0.0, PullToRefreshIndicator.Height, RefreshHeaderHeight));
                var compressionOffset = elementBounds.Bottom;

                if (compressionOffset > OffsetTreshhold)
                {
                    if (!IsCompressionTimerRunning)
                    {
                        IsCompressionTimerRunning = true;
                        CompressionTimer.Start();
                    }

                    IsCompressedEnough = true;
                }
                else if (compressionOffset == 0 && IsReadyToRefresh)
                {
                    InvokeRefresh();
                }
                else
                {
                    IsCompressedEnough = false;
                    IsCompressionTimerRunning = false;
                }
            }
        }

        private void CompressionTimerTick(object sender, object e)
        {
            if (IsCompressedEnough)
            {
                VisualStateManager.GoToState(this, "ReadyToRefresh", true);
                IsReadyToRefresh = true;
            }
            else
            {
                IsCompressedEnough = false;
                CompressionTimer.Stop();
            }
        }

        private void ScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (RootScrollViewer.VerticalOffset == RootScrollViewer.ScrollableHeight)
            {
                InvokeMore();
            }
        }
        #endregion

        #region 其他
        private async void InvokeRefresh()
        {
            IsReadyToRefresh = false;
            VisualStateManager.GoToState(this, "Normal", true);
            if (!isBusy)
            {
                isBusy = true;
                if (RefreshContent != null)
                {
                    RefreshContent(this, EventArgs.Empty);
                }

                if (RefreshCommand != null && RefreshCommand.CanExecute(default(object)))
                {
                    await RefreshCommand.Execute(default(object));
                }
                isBusy = false;
            }
        }

        private async void InvokeMore()
        {
            if (!isBusy)
            {
                isBusy = true;
                if (MoreContent != null)
                {
                    MoreContent(this, EventArgs.Empty);
                }

                if (MoreCommand != null && MoreCommand.CanExecute(default(object)))
                {
                    await MoreCommand.Execute(default(object));
                }
                isBusy = false;
            }
        }
        #endregion
    }

}

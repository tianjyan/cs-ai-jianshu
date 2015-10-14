using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace AiJianShu.UserControls
{
    /// <summary>
    /// 拥有下拉更新和加载更多数据的ListView
    /// 界面设计在Style/Template中
    /// 可以自定义下拉更新的模板,加载更多的模板,更新中的模板
    /// </summary>
    /// <example>
    /// 用法举例:
    /// <code>
    /// <my:ExtendedListView
    ///         IsPullToRefreshEnabled="True" 
    ///         IsMoreDataRequestedEnabled="True"
    ///         PullToRefreshRequested="LeftPullToRefreshRequested"
    ///         MoreDataRequested="LeftMoreDataRequested">      
    ///         <my:ExtendedListView.PullToRefreshPartTemplate>
    ///            <DataTemplate>
    ///                  <TextBlock Text="继续下拉更新" HorizontalAlignment="Center"/>
    ///             </DataTemplate>
    ///          </my:ExtendedListView.PullToRefreshPartTemplate>
    ///        <my:ExtendedListView.LoadingPartTemplate>
    ///             <DataTemplate>
    ///                 <TextBlock Text="正在更新..." HorizontalAlignment="Center"/>
    ///              </DataTemplate>
    ///         </my:ExtendedListView.LoadingPartTemplate>
    ///         <my:ExtendedListView.MoreDataPartTemplate>
    ///              <DataTemplate>
    ///                  <TextBlock Text="加载更多..."/>
    ///              </DataTemplate>
    ///         </my:ExtendedListView.MoreDataPartTemplate>
    ///      </my:ExtendedListView>
    /// </code>
    /// </example>
    public class ExtendedListView : ListView
    {
        #region 构造函数
        public ExtendedListView()
        {
            base.DefaultStyleKey = typeof(ExtendedListView);
        }
        #endregion

        #region 私有字段
        private ScrollViewer RootScrollViewer;
        private ListView BaseListView;
        private ScrollViewer BaseScollViewer;
        private FrameworkElement RefreshGrid;
        private FrameworkElement LoadingPart;
        private FrameworkElement PullToRefreshPart;
        private FrameworkElement ContainerStackPanel;
        private FrameworkElement MoreDataPart;

        private bool isPullRefresh;
        private bool isBusy = true;
        private bool isTouch = true;
        private bool isOrientation;
        #endregion

        #region 属性
        #region 依赖属性
        /// <summary>
        /// 依赖属性:是否允许下拉更新
        /// </summary>
        public static readonly DependencyProperty IsPullToRefreshEnabledProperty = 
            DependencyProperty.RegisterAttached("IsPullToRefreshEnabled", typeof(bool), typeof(ExtendedListView), 
                new PropertyMetadata(false, new PropertyChangedCallback(ExtendedListView.OnPullToRefreshEnabledChanged)));

        /// <summary>
        /// 依赖属性:是否允许加载更多
        /// </summary>
        public static readonly DependencyProperty IsMoreDataRequestedEnabledProperty = 
            DependencyProperty.RegisterAttached("IsMoreDataRequestedEnabled", typeof(bool), typeof(ExtendedListView), new PropertyMetadata(false));

        /// <summary>
        /// 依赖属性:下拉更新加载数的模板
        /// </summary>
        public static readonly DependencyProperty LoadingPartTemplateProperty = 
            DependencyProperty.RegisterAttached("LoadingPartTemplate", typeof(DataTemplate), typeof(ExtendedListView), new PropertyMetadata(null));

        /// <summary>
        /// 依赖属性:下拉更新界面的模板
        /// </summary>
        public static readonly DependencyProperty PullToRefreshPartTemplateProperty = 
            DependencyProperty.RegisterAttached("PullToRefreshPartTemplate", typeof(DataTemplate), typeof(ExtendedListView), new PropertyMetadata(null));

        /// <summary>
        /// 依赖属性:加载更多数据的模板
        /// </summary>
        public static readonly DependencyProperty MoreDataPartTemplateProperty = 
            DependencyProperty.RegisterAttached("MoreDataPartTemplate", typeof(DataTemplate), typeof(ExtendedListView), new PropertyMetadata(null));
        #endregion

        #region 属性
        public bool IsPullToRefreshEnabled
        {
            get
            {
                return (bool)base.GetValue(ExtendedListView.IsPullToRefreshEnabledProperty);
            }
            set
            {
                base.SetValue(ExtendedListView.IsPullToRefreshEnabledProperty, value);
            }
        }

        public bool IsMoreDataRequestedEnabled
        {
            get
            {
                return (bool)base.GetValue(ExtendedListView.IsMoreDataRequestedEnabledProperty);
            }
            set
            {
                base.SetValue(ExtendedListView.IsMoreDataRequestedEnabledProperty, value);
            }
        }

        public DataTemplate LoadingPartTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(ExtendedListView.LoadingPartTemplateProperty);
            }
            set
            {
                base.SetValue(ExtendedListView.LoadingPartTemplateProperty, value);
            }
        }

        public DataTemplate PullToRefreshPartTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(ExtendedListView.PullToRefreshPartTemplateProperty);
            }
            set
            {
                base.SetValue(ExtendedListView.PullToRefreshPartTemplateProperty, value);
            }
        }

        public DataTemplate MoreDataPartTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(ExtendedListView.MoreDataPartTemplateProperty);
            }
            set
            {
                base.SetValue(ExtendedListView.MoreDataPartTemplateProperty, value);
            }
        }
        #endregion
        #endregion

        #region 路由事件
        private async void ScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!this.IsPullToRefreshEnabled || !this.isTouch || this.isBusy || this.isOrientation)
            {
                this.isOrientation = false;
                if (this.RootScrollViewer.VerticalOffset < this.RefreshGrid.RenderSize.Height)
                {
                    this.RootScrollViewer.ChangeView(default(double?), new double?(this.RefreshGrid.RenderSize.Height), default(float?), true);
                }
            }
            else
            {
                this.RefreshGrid.Opacity = 1.0 - this.RootScrollViewer.VerticalOffset / 100.0;
                if (this.RootScrollViewer.VerticalOffset == 0.0)
                {
                    this.RefreshGrid.Opacity = 1;
                }
                else
                {
                    this.RefreshGrid.Opacity = 0.5;
                }
                if (this.RootScrollViewer.VerticalOffset != 0.0)
                {
                    this.isPullRefresh = true;
                }
                if (!e.IsIntermediate)
                {
                    if (this.RootScrollViewer.VerticalOffset <= 5.0 && this.isPullRefresh && this.PullToRefreshRequested != null)
                    {
                        this.LoadingPart.Opacity = 1;
                        this.PullToRefreshPart.Opacity = 0;
                        try
                        {
                            this.isBusy = true;
                            await this.PullToRefreshRequested(this, EventArgs.Empty);
                            this.isBusy = false;
                        }
                        catch (Exception ex)
                        {
                            this.isPullRefresh = false;
                            this.RootScrollViewer.ChangeView(default(double?), new double?(this.RefreshGrid.RenderSize.Height), default(float?));
                            this.LoadingPart.Opacity = 0;
                            this.PullToRefreshPart.Opacity = 1;
                            throw ex;
                        }
                    }
                    this.isPullRefresh = false;
                    this.RootScrollViewer.ChangeView(default(double?), new double?(this.RefreshGrid.RenderSize.Height), default(float?));
                    await Task.Delay(10);
                    this.LoadingPart.Opacity = 0;
                    this.PullToRefreshPart.Opacity = 1;
                }
            }
        }

        private void ScrollViewerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (base.Parent is StackPanel)
            {
                return;
            }
            this.BaseListView.Height = e.NewSize.Height;
            this.BaseListView.Width = e.NewSize.Width;
            ThreadPoolTimer.CreateTimer(async delegate (ThreadPoolTimer source)
            {
                await base.Dispatcher.RunAsync(0, delegate
                {
                    this.RootScrollViewer.ChangeView(default(double?), new double?(this.RefreshGrid.RenderSize.Height), default(float?), true);
                });
            }, TimeSpan.FromMilliseconds(10.0));
        }

        private async void ScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            this.RootScrollViewer.ChangeView(default(double?), new double?(this.RefreshGrid.RenderSize.Height), default(float?), true);
            this.BaseListView.Width = this.BaseListView.Width - 1.0;
            this.BaseListView.Height = this.BaseListView.Height - 1.0;
            await Task.Delay(10);
            this.BaseListView.Width = this.BaseListView.Width + 1.0;
            this.BaseListView.Height = this.BaseListView.Height + 1.0;
            ThreadPoolTimer.CreateTimer(async delegate (ThreadPoolTimer source)
            {
                await base.Dispatcher.RunAsync(0, delegate
                {
                    this.RootScrollViewer.ChangeView(default(double?), new double?(this.RefreshGrid.RenderSize.Height), default(float?), true);
                });
            }, TimeSpan.FromMilliseconds(10.0));
        }

        private void ListViewRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (this.RightTapped != null)
            {
                this.RightTapped.Invoke(this, e);
            }
        }

        private void ListViewHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (this.Holding != null)
            {
                this.Holding.Invoke(this, e);
            }
        }

        private void ListViewContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (this.ContainerContentChanging != null)
            {
                this.ContainerContentChanging.Invoke(this, args);
            }
        }

        private void ListViewItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ItemClick != null)
            {
                this.ItemClick.Invoke(this, e);
            }
        }

        private void ListViewDragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            if (this.DragItemsStarting != null)
            {
                this.DragItemsStarting.Invoke(this, e);
            }
        }

        private void ListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged.Invoke(this, e);
            }
        }

        private void ListViewLoaded(object s, RoutedEventArgs e)
        {
            isBusy = false;
            if (this.BaseScollViewer == null)
            {
                this.BaseScollViewer = (this.FindChildControl<ScrollViewer>(this.BaseListView, "") as ScrollViewer);
                this.BaseScollViewer.ViewChanged += ListViewScollViewerViewChanged;
            }
        }

        private async void ListViewScollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs arg)
        {
            if (!this.isBusy)
            {
                if (this.IsMoreDataRequestedEnabled && !arg.IsIntermediate && 
                    this.BaseScollViewer.VerticalOffset == this.BaseScollViewer.ScrollableHeight && 
                    this.MoreDataRequested != null)
                {
                    this.MoreDataPart.Visibility = Visibility.Collapsed;
                    this.isBusy = true;
                    try
                    {
                        await this.MoreDataRequested(this, EventArgs.Empty);
                    }
                    catch (Exception ex)
                    {
                        this.isBusy = false;
                        this.MoreDataPart.Visibility = Visibility.Visible;
                        throw ex;
                    }
                    this.isBusy = false;
                    this.MoreDataPart.Visibility = Visibility.Visible;
                }
            }
        }
        #endregion

        #region 公有方法
        public DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement frameworkElement = child as FrameworkElement;
                if (frameworkElement == null)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(ctrlName) && child is T)
                {
                    return child;
                }
                if (child is T && frameworkElement.Name == ctrlName)
                {
                    return child;
                }
                DependencyObject dependencyObject = this.FindChildControl<T>(child, ctrlName);
                if (dependencyObject != null)
                {
                    return dependencyObject;
                }
            }
            return null;
        }
        #endregion

        #region 私有方法
        private static void OnPullToRefreshEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExtendedListView extendedListView = d as ExtendedListView;
            FrameworkElement frameworkElement = extendedListView.GetTemplateChild("RefreshGrid") as FrameworkElement;
            ScrollViewer scrollViewer = extendedListView.GetTemplateChild("RootScrollViewer") as ScrollViewer;
            if (frameworkElement == null || scrollViewer == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                frameworkElement.Opacity = 1;
                scrollViewer.ChangeView(default(double?), new double?(frameworkElement.RenderSize.Height), default(float?), true);
                scrollViewer.UpdateLayout();
                return;
            }
            frameworkElement.Opacity = 0;
        }

        private void OnWheel(object sender, PointerRoutedEventArgs e)
        {
            this.isTouch = false;
            this.RootScrollViewer.VerticalScrollMode = ScrollMode.Disabled;
        }

        private void OnPrssed(object sender, PointerRoutedEventArgs e)
        {
            this.isTouch = (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch);
            this.RootScrollViewer.VerticalScrollMode = isTouch ? ScrollMode.Enabled : ScrollMode.Disabled;
        }
        #endregion

        #region 其他
        public delegate Task AsyncEventHandler(object sender, EventArgs e);

        public new event SelectionChangedEventHandler SelectionChanged;

        public new event TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContainerContentChanging;

        public new event DragItemsStartingEventHandler DragItemsStarting;

        public new event ItemClickEventHandler ItemClick;

        public new event HoldingEventHandler Holding;

        public new event RightTappedEventHandler RightTapped;

        public event AsyncEventHandler PullToRefreshRequested;

        public event AsyncEventHandler MoreDataRequested;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            Grid grid = this.FindChildControl<Grid>(this, "") as Grid;
            ContentPresenter contentPresenter = base.GetTemplateChild("LoadingPart") as ContentPresenter;
            if (contentPresenter.ContentTemplate == null)
            {
                contentPresenter.ContentTemplate = grid.Resources["RefreshPartTemplate"] as DataTemplate;
            }
            contentPresenter = (base.GetTemplateChild("PullToRefreshPart") as ContentPresenter);
            if (contentPresenter.ContentTemplate == null)
            {
                contentPresenter.ContentTemplate = grid.Resources["PullToRefreshPartTemplate"] as DataTemplate;
            }
            contentPresenter = (base.GetTemplateChild("MoreDataPart") as ContentPresenter);
            if (contentPresenter.ContentTemplate == null)
            {
                contentPresenter.ContentTemplate = grid.Resources["MoreDataPartTemplate"] as DataTemplate;
            }

            this.MoreDataPart = (base.GetTemplateChild("MoreDataPart") as FrameworkElement);
            this.MoreDataPart.Visibility = Visibility.Visible;
            this.RefreshGrid = (base.GetTemplateChild("RefreshGrid") as FrameworkElement);
            this.LoadingPart = (base.GetTemplateChild("LoadingPart") as FrameworkElement);
            this.PullToRefreshPart = (base.GetTemplateChild("PullToRefreshPart") as FrameworkElement);
            this.RootScrollViewer = (base.GetTemplateChild("RootScrollViewer") as ScrollViewer);
            this.ContainerStackPanel = (base.GetTemplateChild("ContainerStackPanel") as FrameworkElement);
            this.BaseListView = (base.GetTemplateChild("BaseListView") as ListView);

            if (this.IsPullToRefreshEnabled)
            {
                this.RefreshGrid.Opacity = 1;
                this.RootScrollViewer.ChangeView(default(double?), new double?(this.RefreshGrid.RenderSize.Height), default(float?), true);
                this.RootScrollViewer.UpdateLayout();
            }
            else
            {
                this.RefreshGrid.Opacity = 0;
            }

            this.BaseListView.Loaded += ListViewLoaded;
            this.BaseListView.SelectionChanged += ListViewSelectionChanged;
            this.BaseListView.DragItemsStarting += ListViewDragItemsStarting;
            this.BaseListView.ItemClick += ListViewItemClick;
            this.BaseListView.ContainerContentChanging += ListViewContainerContentChanging;
            this.BaseListView.Holding += ListViewHolding;
            this.BaseListView.RightTapped += ListViewRightTapped;

            this.RootScrollViewer.Loaded += ScrollViewerLoaded;
            this.RootScrollViewer.SizeChanged += ScrollViewerSizeChanged;
            this.RootScrollViewer.ViewChanged += ScrollViewerViewChanged;

            DisplayInformation.GetForCurrentView().OrientationChanged += (s, e) => this.isOrientation = true;

            base.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(this.OnPrssed), true);
            base.AddHandler(UIElement.PointerWheelChangedEvent, new PointerEventHandler(this.OnWheel), true);

            if (base.Parent != null && base.Parent is StackPanel)
            {
                this.PullToRefreshPart.Visibility = Visibility.Visible;
            }
        }

        public new void ScrollIntoView(object view)
        {
            if (this.BaseListView != null)
            {
                this.BaseListView.ScrollIntoView(view);
            }
        }

        public new void SelectAll()
        {
            this.BaseListView.SelectAll();
        }

        public new void ScrollIntoView(object view, ScrollIntoViewAlignment alignment)
        {
            if (this.BaseListView != null)
            {
                this.BaseListView.ScrollIntoView(view, alignment);
            }
        }

        #endregion
    }
}

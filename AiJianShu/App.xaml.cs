using AiJianShu.Common;
using AiJianShu.ExceptionHandler;
using AiJianShu.Model;
using JianShuCore.Common;
using JianShuCore.Provider;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AiJianShu
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            this.UnhandledException += AppUnhandledException;

            CacheProvider cache = new CacheProvider(StorageType.IsolatedStorage);
            GlobalValue.CurrentUserContext = cache.GetItem<UserContext>(CacheKey.UserContext);
        }

        private void RegisterExceptionHandlingSynchronizationContext()
        {
            ExceptionHandlingSynchronizationContext
                .Register()
                .UnhandledException += SynchronizationContextUnhandledException;
        }

        private async void AppUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            await new MessageDialog("应用程序出错:\r\n" + e.Exception.Message)
                .ShowAsync();
        }

        private async void SynchronizationContextUnhandledException(object sender, AysncUnhandledExceptionEventArgs e)
        {
            e.Handled = true;


            if (e.Exception.Message.Contains("404"))
            {
                await GalaSoft.MvvmLight.Threading.DispatcherHelper.RunAsync(async () => {
                    await new MessageDialog(App.Current.Resources["ErrorCode_404"].ToString()).ShowAsync();
                });
            }
            else {
                await GalaSoft.MvvmLight.Threading.DispatcherHelper.RunAsync(async () => {
                    await new MessageDialog("异步线程出错:\r\n" + e.Exception.Message).ShowAsync();
                });
            }
     
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "zh-CN";
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("ms-appx:///Themes/zh_cn.xaml") });

            RegisterExceptionHandlingSynchronizationContext();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            RegisterExceptionHandlingSynchronizationContext();
            base.OnActivated(args);
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}

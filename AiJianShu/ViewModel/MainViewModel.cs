using AiJianShu.Common;
using AiJianShu.Model;
using AiJianShu.UserControls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JianShuCore;
using JianShuCore.Common;
using JianShuCore.Interface;
using JianShuCore.Provider;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace AiJianShu.ViewModel
{
    public class MainViewModel : ViewModelBase, IStatusProvider
    {
        #region 只读字段
        public readonly static HomeViewModel homeViewModel = new HomeViewModel();
        public readonly static FollowViewModel followViewModel = new FollowViewModel();
        public readonly static FriendsViewModel friendsViewModel = new FriendsViewModel();
        public readonly static SpecialTopicViewModel specialTopicViewModel = new SpecialTopicViewModel();
        readonly static LoginViewModel loginViewModel = new LoginViewModel();
        public readonly static LikeViewModel likeViewModel = new LikeViewModel();
        public static readonly UserCenterViewModel userCenterViewModel = new UserCenterViewModel();
        public readonly static ArticleViewModel articleViewModel = new ArticleViewModel();
        readonly static AboutViewModel aboutViewModel = new AboutViewModel(); 
        #endregion

        #region 字段
        private ViewModelBase currentViewModel;
        private Visibility followViewVisbility;
        private Uri avatar = GlobalValue.DefaultAvatar;
        private bool isLogin;
        private Visibility progressVisible = Visibility.Collapsed;
        private int requestCount;
        #endregion

        #region 属性
        public string DeviceFamily
        {
            get
            {
                return Untils.DeviceFamily;
            }
        }

        public Uri Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                if (value == null || value.AbsolutePath.EndsWith("default_avatar.png"))
                {
                    avatar = GlobalValue.DefaultAvatar;
                }
                else
                {
                    avatar = value;
                }
                RaisePropertyChanged();
            }
        }

        public bool IsLogin
        {
            get
            {
                return isLogin;
            }
            set
            {
                isLogin = value;
                RaisePropertyChanged();
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }
            set
            {
                if (currentViewModel == value)
                {
                    return;

                }
                currentViewModel = value;
                RaisePropertyChanged(() => CurrentViewModel);
                RaisePropertyChanged(() => CurrentTemplate);
            }
        }

        public Visibility FollowViewVisbility
        {
            get
            {
                return followViewVisbility;
            }
            set
            {
                followViewVisbility = value;
                RaisePropertyChanged();
            }

        }

        public DataTemplate CurrentTemplate
        {
            get
            {
                if (CurrentViewModel == null)
                {
                    return null;
                }

                return Untils.DataTemplateSelector.GetTemplate(CurrentViewModel);
            }
        }

        public Visibility ProgressVisible
        {
            get
            {
                return progressVisible;
            }
            set
            {
                progressVisible = value;
                RaisePropertyChanged();
            }
        }

        public ICommand HomeViewCommand { get; set; }
        public ICommand FollowViewCommand { get; set; }
        public ICommand SpecialTopicViewCommand { get; set; }
        public ICommand FriendsViewCommand { get; set; }
        public ICommand UserCenterViewCommand { get; set; }
        public ICommand LikeViewCommand { get; set; }
        public ICommand AboutViewCommand { get; set; }
        public RelayCommand<object> NavigationClickCommand { get; set; }
        #endregion

        #region 构造函数
        public MainViewModel()
        {
            Init.InitStatusProvider(this);
            InitMvvmLight();
            InitCMD();
            InitListener();
            CheckLogin();
        }
        #endregion

        #region 私有方法
        #region 命令
        private void ExecuteHomeViewCommand()
        {
            SwitchViewModel(ViewType.Home);
        }

        private void ExecuteFollowViewCommand()
        {
            SwitchViewModel(ViewType.Follow);
        }

        private void ExecuteSpecialTopicViewCommand()
        {
            SwitchViewModel(ViewType.SpecialTopic);
        }

        private void ExecuteFriendsViewCommand()
        {
            SwitchViewModel(ViewType.Friends);
        }

        private void ExecuteUserCenterViewCommand()
        {
            SwitchViewModel(ViewType.UserCenter);
        }

        private void ExecuteLikeViewCommand()
        {
            SwitchViewModel(ViewType.Like);
        }

        private void ExecuteAboutViewCommnd()
        {
            SwitchViewModel(ViewType.About);
        }

        private void ExecuteNavigationClickCommand(object value)
        {
            if (value is HyperLinkEventArgs)
            {
                Tuple<string, string> result = (value as HyperLinkEventArgs).Tag as Tuple<string, string>;
                switch (result.Item1)
                {
                    case "note":
                    case "comment_on_note":
                    case "share_note":
                        ListenerViewChanged(new ChangeView()
                        {
                            FromView = ViewType.Friends,
                            ToView = ViewType.Article,
                            Context = result.Item2,
                            Event = EventType.Article
                        });
                        break;
                    case "user":
                        ListenerViewChanged(new ChangeView()
                        {
                            FromView = ViewType.Friends,
                            ToView = ViewType.OtherUser,
                            Context = result.Item2,
                            Event = EventType.User
                        });
                        break;
                    default:
                        break;
                }    
            }
        }
        #endregion

        #region 初始化
        private void InitMvvmLight()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
        }

        private void InitCMD()
        {
            HomeViewCommand = new RelayCommand(ExecuteHomeViewCommand);
            FollowViewCommand = new RelayCommand(ExecuteFollowViewCommand);
            SpecialTopicViewCommand = new RelayCommand(ExecuteSpecialTopicViewCommand);
            FriendsViewCommand = new RelayCommand(ExecuteFriendsViewCommand);
            UserCenterViewCommand = new RelayCommand(ExecuteUserCenterViewCommand);
            LikeViewCommand = new RelayCommand(ExecuteLikeViewCommand);
            AboutViewCommand = new RelayCommand(ExecuteAboutViewCommnd);

            NavigationClickCommand = new RelayCommand<object>(ExecuteNavigationClickCommand);
        }

        private void InitListener()
        {
            this.MessengerInstance.Register<ChangeView>(this, ListenerViewChanged);
        }

        private async void CheckLogin()
        {
            if (GlobalValue.CurrentUserContext?.IsLogin == true)
            {
                try
                {
                    //查询基本的用户信息,顺便验证Token信息是否过期
                    GlobalValue.CurrentUserBaseInfo =
                        await new UserContentProvider().QueryBaseInfo(GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
                    IsLogin = true;
                    Avatar = new Uri(GlobalValue.CurrentUserContext.Avatar, UriKind.Absolute);
                    SwitchViewModel(ViewType.Home);
                    return;
                }
                catch
                {
                    MessengerInstance.Send(new ShowMessage() { MessageContent = App.Current.Resources["LoginExpire"].ToString() });
                }   
            }
            SwitchViewModel(ViewType.Login);
        }
        #endregion

        #region 监听
        private void ListenerViewChanged(ChangeView info)
        {
            switch(info.Event)
            {
                case EventType.LoginSuccess:
                    CacheProvider tempCache1 = new CacheProvider(StorageType.IsolatedStorage);
                    UserContext user = info.Context as UserContext;

                    if (tempCache1.ContainItem(CacheKey.UserContext)) tempCache1.UpdateItem(CacheKey.UserContext, user);
                    else tempCache1.AddItem(CacheKey.UserContext, user);

                    GlobalValue.CurrentUserContext = user;

                    Avatar = new Uri(GlobalValue.CurrentUserContext.Avatar, UriKind.Absolute);
                    IsLogin = true;
                    break;
                case EventType.Logout:
                    CacheProvider tempCache2 = new CacheProvider(StorageType.IsolatedStorage);

                    GlobalValue.CurrentUserContext.IsLogin = false;
                    tempCache2.UpdateItem(CacheKey.UserContext, GlobalValue.CurrentUserContext);

                    Avatar = GlobalValue.DefaultAvatar;
                    IsLogin = false;

                    WebContentProvider.CancelPendingRequests();

                    homeViewModel.Cleanup();
                    followViewModel.Cleanup();
                    friendsViewModel.Cleanup();
                    specialTopicViewModel.Cleanup();
                    likeViewModel.Cleanup();
                    userCenterViewModel.Cleanup();

                    View.FriendsView.VerticalOffset = 0;
                    View.SpecialTopicView.leftOffset = 0;
                    View.SpecialTopicView.rightOffset = 0;
                    View.SpecialTopicView.rightCanShow = false;
                    break;
                default:
                    break;
            }

            SwitchViewModel(info.ToView ,info);
        }
        #endregion

        #region 切换界面
        private async void SwitchViewModel(ViewType type, object args = null)
        {
            switch (type)
            {
                case ViewType.Home:
                    CurrentViewModel = homeViewModel;
                    homeViewModel.CheckInitialized();
                    break;
                case ViewType.Follow:
                    CurrentViewModel = followViewModel;
                    followViewModel.CheckInitialized();
                    break;
                case ViewType.SpecialTopic:
                    CurrentViewModel = specialTopicViewModel;
                    specialTopicViewModel.CheckInitialized();
                    break;
                case ViewType.Friends:
                    CurrentViewModel = friendsViewModel;
                    await friendsViewModel.CheckInitialized();
                    break;
                case ViewType.UserCenter:
                    CurrentViewModel = userCenterViewModel;
                    MessengerInstance.Send(new UserItem() { UserId = GlobalValue.CurrentUserContext.Slug, BackView = ViewType.None });
                    break;
                case ViewType.Like:
                    CurrentViewModel = likeViewModel;
                    likeViewModel.CheckInitialized();
                    break;
                case ViewType.Login:
                    CurrentViewModel = loginViewModel;
                    break;
                case ViewType.About:
                    CurrentViewModel = aboutViewModel;
                    aboutViewModel.RefreshSetting();
                    break;
                case ViewType.Article:
                    ChangeView info = args as ChangeView;
                    CurrentViewModel = articleViewModel;
                    MessengerInstance.Send(new ArticleItem() { NoteId = info.Context.ToString(), BackView = info.FromView });
                    break;
                case ViewType.OtherUser:
                    ChangeView userinfo = args as ChangeView;
                    CurrentViewModel = userCenterViewModel;
                    MessengerInstance.Send(new UserItem() { UserId = userinfo.Context.ToString(), BackView = userinfo.FromView });
                    break;
                default:
                    break;
            }
        }

        public void ChangeStatus(StatusType statusType)
        {
            if(statusType == StatusType.Busy)
            {
                requestCount++;
            }
            else if(statusType == StatusType.Idle)
            {
                requestCount--;
            }

            ProgressVisible = requestCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
        #endregion
    }
}

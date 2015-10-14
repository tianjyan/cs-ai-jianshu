using AiJianShu.Common;
using AiJianShu.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JianShuCore;
using System.Linq;
using JianShuCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AiJianShu.Command;

namespace AiJianShu.ViewModel
{
    public class UserCenterViewModel : ViewModelBase
    {
        #region 字段
        private Uri avatar;
        private UserBaseInfo baseInfo;
        private int followerCount;
        private int followingCount;
        private int likedNotesCount;
        private int bookmarksCount;
        private int subscribingCount;
        private int totalLikesReceived;
        private int totalWordage;
        private int notebooksCount;
        private ObservableCollection<FollowUser> followerUsers;
        private ObservableCollection<FollowUser> followingUsers;
        private ObservableCollection<SubscriptionItem> subscriptionItems;
        private ObservableCollection<NotebookResult> ownNotebooks;
        private int followerPage = 1;
        private int followingPage = 1;
        private int subscriptionPage = 1;
        private int notebookPage = 1;
        private string currentUserId;
        private bool isSelf;
        private bool canBack;
        private ViewType backView;
        #endregion

        #region 属性
        public ICommand LogoutCommand { get; set; }
        public RelayCommand<string> FollowUserCommand { get; set; }
        public RelayCommand<int> SubscriptionCommand { get; set; }

        public AsyncCommand RefreshFollowCommmand { get; set; }
        public AsyncCommand MoreFollowCommand { get; set; }
        public AsyncCommand RefreshFollowingCommand { get; set; }
        public AsyncCommand MoreFollowingCommnd { get; set; }
        public AsyncCommand RefreshSubscriptionsCommand { get; set; }
        public AsyncCommand MoreRefreshSubscriptionsCommand { get; set; }
        public AsyncCommand RefreshCollectionCommand { get; set; }
        public AsyncCommand MoreRefreshCollectionCommand { get; set; }

        public Uri Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                avatar = value;
                RaisePropertyChanged();
            }
        }
        public int FollowerCount
        {
            get
            {
                return followerCount;
            }
            set
            {
                followerCount = value;
                RaisePropertyChanged();
            }
        }
        public int FollowingCount
        {
            get
            {
                return followingCount;
            }
            set
            {
                followingCount = value;
                RaisePropertyChanged();
            }
        }
        public int LikedNotesCount
        {
            get
            {
                return likedNotesCount;
            }
            set
            {
                likedNotesCount = value;
                RaisePropertyChanged();
            }
        }
        public int BookmarksCount
        {
            get
            {
                return bookmarksCount;
            }
            set
            {
                bookmarksCount = value;
                RaisePropertyChanged();
            }
        }
        public int SubscribingCount
        {
            get
            {
                return subscribingCount;
            }
            set
            {
                subscribingCount = value;
                RaisePropertyChanged();
            }
        }
        public int TotalLikesReceived
        {
            get
            {
                return totalLikesReceived;
            }
            set
            {
                totalLikesReceived = value;
                RaisePropertyChanged();
            }
        }
        public int TotalWordage
        {
            get
            {
                return totalWordage;
            }
            set
            {
                totalWordage = value;
                RaisePropertyChanged();
            }
        }
        public int NotebooksCount
        {
            get
            {
                return notebooksCount;
            }
            set
            {
                notebooksCount = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<FollowUser> FollowerUsers
        {
            get
            {
                return followerUsers;
            }
            set
            {
                followerUsers = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<FollowUser> FollowingUsers
        {
            get
            {
                return followingUsers;
            }
            set
            {
                followingUsers = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<SubscriptionItem> SubscriptionItems
        {
            get
            {
                return subscriptionItems;
            }
            set
            {
                subscriptionItems = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<NotebookResult> OwnNotebooks
        {
            get
            {
                return ownNotebooks;
            }
            set
            {
                ownNotebooks = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSelf
        {
            get
            {
                return isSelf;
            }
            set
            {
                isSelf = value;
                RaisePropertyChanged();
            }
        }
        public bool CanBack
        {
            get
            {
                return canBack;
            }
            set
            {
                canBack = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region 构造函数
        public UserCenterViewModel()
        {
            InitCMD();
            InitListener();
        }
        #endregion

        #region 公有方法
        public bool BackPreView()
        {
            if (CanBack)
            {
                MessengerInstance.Send<ChangeView>(new ChangeView() { FromView = ViewType.OtherUser, ToView = backView });
            }
            return CanBack;
        }

        public async Task RefreshFollowers()
        {
            followerPage = 1;
            await QueryFollowers();
        }

        public async Task MoreFollowers()
        {
            await QueryFollowers();
        }

        public async Task RefreshFollowing()
        {
            followingPage = 1;
            await QueryFollowings();
        }

        public async Task MoreFollowing()
        {
            await QueryFollowings();
        }

        public async Task RefreshSubscriptions()
        {
            subscriptionPage = 1;
            await QuerySubscriptions();
        }

        public async Task MoreSubscriptions()
        {
            await QuerySubscriptions();
        }

        public async Task RefreshCollection()
        {
            notebookPage = 1;
            await QueryOwnerCollection();
        }

        public async Task MoreCollection()
        {
            await QueryOwnerCollection();
        }
        #endregion

        #region 私有方法
        #region 初始化
        private void InitCMD()
        {
            LogoutCommand = new RelayCommand(ExecuteLogoutCommand);
            FollowUserCommand = new RelayCommand<string>(ExecuteFollowUserCommand);
            SubscriptionCommand = new RelayCommand<int>(ExecuteSubscriptionCommand);

            RefreshFollowCommmand = new AsyncCommand(RefreshFollowers);
            MoreFollowCommand = new AsyncCommand(MoreFollowers);
            RefreshFollowingCommand = new AsyncCommand(RefreshFollowing);
            MoreFollowingCommnd = new AsyncCommand(MoreFollowing);
            RefreshSubscriptionsCommand = new AsyncCommand(RefreshSubscriptions);
            MoreRefreshSubscriptionsCommand = new AsyncCommand(MoreSubscriptions);
            RefreshCollectionCommand = new AsyncCommand(RefreshCollection);
            MoreRefreshCollectionCommand = new AsyncCommand(MoreCollection);
        }

        private void InitListener()
        {
            this.MessengerInstance.Register<UserItem>(this, ShowUser);
        }
        #endregion

        #region 命令
        private void ExecuteLogoutCommand()
        {
            baseInfo = null;

            if (GlobalValue.CurrentUserContext != null)
            {
                GlobalValue.CurrentUserContext.IsLogin = false;
                GlobalValue.CurrentUserContext.Avatar = "ms-appx:///Assets/default_avatar.png";
            }
            
            this.MessengerInstance.Send<ChangeView>(new ChangeView()
            {
                FromView = ViewType.UserCenter,
                ToView = ViewType.Login,
                Event = EventType.Logout,
                Context = GlobalValue.CurrentUserContext
            });
        }

        private async void ExecuteFollowUserCommand(string userid)
        {
            FollowUser follower = FollowerUsers.FirstOrDefault(x => x.Id == userid);
            FollowUser following = FollowingUsers.FirstOrDefault(x => x.Id == userid);
            if(follower?.IsFollowing == true || following?.IsFollowing == true)
            {
                UserContentProvider user = new UserContentProvider();
                FollowResult result = await user.UnFollowUser(userid, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
                if (result.Error == null || result.Error.Count == 0)
                {
                    if(follower != null) follower.IsFollowing = false;
                    if (following != null) following.IsFollowing = false;
                }
            }
            else
            {
                UserContentProvider user = new UserContentProvider();
                FollowResult result = await user.FollowUser(userid, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
                if (result.Error == null || result.Error.Count == 0)
                {
                    if (follower != null) follower.IsFollowing = true;
                    if (following != null) following.IsFollowing = true;
                }
            }
        }

        private async void ExecuteSubscriptionCommand(int id)
        {
           SubscriptionItem value =  SubscriptionItems.First(x => x.Id == id);
            if(value.IsSubscribed)
            {
                UserContentProvider user = new UserContentProvider();
                DiscoverSubscribeResult reuslt = await user.Unsubscribe(id, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
                if(reuslt.Error == null || reuslt.Error.Count == 0)
                {
                    value.IsSubscribed = false;
                }
            }
            else
            {
                UserContentProvider user = new UserContentProvider();
                DiscoverSubscribeResult reuslt = await user.Subscribe(id, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
                if (reuslt.Error == null || reuslt.Error.Count == 0)
                {
                    value.IsSubscribed = true;
                }
            }
        }
        #endregion

        private async void ShowUser(UserItem user)
        {
            followerPage = 1;
            followingPage = 1;
            subscriptionPage = 1;
            notebookPage = 1;

            if (user.UserId == GlobalValue.CurrentUserContext.Slug)
            {
                IsSelf = true;
            }
            else
            {
                IsSelf = false;
            }

            if (user.BackView == ViewType.None)
            {
                CanBack = false;
            }
            else
            {
                CanBack = true;
                backView = user.BackView;
            }

            currentUserId = user.UserId;

            await QueryBaseInfo();
            await QueryFollowers();
            await QueryFollowings();
            await QuerySubscriptions();
            await QueryOwnerCollection();
        }

        private async Task QueryBaseInfo()
        {
            if (IsSelf)
            {
                UserContentProvider user = new UserContentProvider();
                baseInfo = await user.QueryBaseInfo(GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
            }
            else
            {
                CommonProvider common = new CommonProvider();
                baseInfo = await common.QueryUserInfo(currentUserId);
            }
            Avatar = new Uri(baseInfo.Avatar);
            FollowerCount = baseInfo.FollowersCount;
            FollowingCount = baseInfo.FollowingCount;
            LikedNotesCount = baseInfo.LikedNotesCount;
            BookmarksCount = baseInfo.BookmarksCount;
            SubscribingCount = baseInfo.SubscribingCollectionsCount + baseInfo.SubscribingNotebooksCount;
            TotalWordage = baseInfo.TotalWordage;
            TotalLikesReceived = baseInfo.TotalLikesReceived;
            NotebooksCount = baseInfo.NotebooksCount;
            currentUserId = baseInfo.Id.ToString();
        }

        private async Task QueryFollowers()
        {
            if(followerPage == 1)
            {
                if(FollowerUsers == null)
                {
                    FollowerUsers = new ObservableCollection<FollowUser>();
                }
                else
                {
                    FollowerUsers.Clear();
                }
            }

            UserContentProvider user = new UserContentProvider();
            List<FollowUserInfo> result = await user.QueryFollowers(followerPage, currentUserId , GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);

            result.ForEach(x => FollowerUsers.Add(ConvertToFollowUser(x)));

            followerPage++;

            if(FollowerUsers.Count <= 10 && followerPage == 2)
            {
                await QueryFollowers();
            }
        }

        private async Task QueryFollowings()
        {
            if (followingPage == 1)
            {
                if (FollowingUsers == null)
                {
                    FollowingUsers = new ObservableCollection<FollowUser>();
                }
                else
                {
                    FollowingUsers.Clear();
                }
            }
            UserContentProvider user = new UserContentProvider();
            List<FollowUserInfo> result = await user.QueryFollowingUsers(followingPage, currentUserId, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);

            result.ForEach(x => FollowingUsers.Add(ConvertToFollowUser(x)));

            followingPage++;

            if(FollowingUsers.Count <= 10 && followingPage == 2)
            {
                await QueryFollowings();
            }
        }

        private async Task QuerySubscriptions()
        {
            if(subscriptionPage == 1)
            {
                if(SubscriptionItems == null)
                {
                    SubscriptionItems = new ObservableCollection<SubscriptionItem>();
                }
                else
                {
                    SubscriptionItems.Clear();
                }
            }

            UserContentProvider user = new UserContentProvider();
            List<Subscription> reuslt = await user.QuerySubscription(subscriptionPage, currentUserId ,GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);

            reuslt.ForEach(x => SubscriptionItems.Add(ConvertToSubscriptionItem(x)));

            subscriptionPage++;

            if (SubscriptionItems.Count <= 10 && subscriptionPage == 2)
            {
                await QuerySubscriptions();
            }
        }

        private async Task QueryOwnerCollection()
        {
            if(notebookPage == 1)
            {
                if(OwnNotebooks == null)
                {
                    OwnNotebooks = new ObservableCollection<NotebookResult>();
                }
                else
                {
                    OwnNotebooks.Clear();
                }
            }

            UserContentProvider user = new UserContentProvider();
            List<NotebookResult> result;

            if(currentUserId == GlobalValue.CurrentUserContext.UserId)
            {
                result = await user.QueryNotebook(notebookPage, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
            }
            else
            {
                result = await user.QueryOhterNotebook(currentUserId, notebookPage, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
            }

            result.ForEach(x => OwnNotebooks.Add(x));

            notebookPage++;

            if (OwnNotebooks.Count <= 10 && notebookPage == 2)
            {
                await QueryOwnerCollection();
            }
        }

        private FollowUser ConvertToFollowUser(FollowUserInfo source)
        {
            return new FollowUser()
            {
                Avatar = source.Avatar,
                Id = source.Id.ToString(),
                IsFollower = source.IsFollowedByUser,
                IsFollowing = source.IsFollowingUser,
                NickName = source.Nickname,
                TotalLikesReceived = source.TotalLikesReceived,
                TotalWordage = source.TotalWordage
            };
        }

        private SubscriptionItem ConvertToSubscriptionItem(Subscription source)
        {
            return new SubscriptionItem()
            {
                Id = source.Id,
                IsSubscribed = source.IsSubscribed,
                Title = source.Title,
                NewlyAddedAt = source.NewlyAddedAt,
                Desc = source.Desc,
                Image = source.Image,
                NotesCount = source.NotesCount,
                SubscribersCount = source.SubscribersCount
            };
        }
        #endregion
    }
}

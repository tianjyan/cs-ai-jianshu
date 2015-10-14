using AiJianShu.Command;
using AiJianShu.Common;
using AiJianShu.Model;
using GalaSoft.MvvmLight;
using JianShuCore;
using JianShuCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;

namespace AiJianShu.ViewModel
{
    public class FriendsViewModel : ViewModelBase
    {
        #region 字段
        private ObservableCollection<ActivityItem> activityResult;
        private double verticalOffset;
        #endregion

        #region 属性
        public ObservableCollection<ActivityItem> ActivityResult
        {
            get
            {
                return activityResult;
            }
            set
            {
                activityResult = value;
                RaisePropertyChanged();
            }     
        }

        public string DeviceFamily
        {
            get
            {
                if (string.IsNullOrEmpty(Untils.DeviceFamily))
                {
                    ResourceContext resContext = ResourceContext.GetForCurrentView();
                    Untils.DeviceFamily = resContext.QualifierValues["DeviceFamily"];
                }
                return Untils.DeviceFamily;
            }
        }

        public double VerticalOffset
        {
            get
            {
                return verticalOffset;
            }
            set
            {
                verticalOffset = value;
                RaisePropertyChanged();
            }
        }

        public AsyncCommand RefreshCommand { get; set; }
        public AsyncCommand MoreCommand { get; set; }
        #endregion

        #region 构造函数
        public FriendsViewModel()
        {
            InitCMD();
        }
        #endregion

        #region 公有方法
        public async Task CheckInitialized()
        {
            if(activityResult == null)
            {
                await QueryActivity();
            }
        }
        #endregion

        #region 私有方法
        #region 初始化
        private void InitCMD()
        {
            RefreshCommand = new AsyncCommand(RefreshActivity);
            MoreCommand = new AsyncCommand(MoreActivity);
        }
        #endregion

        private async Task QueryActivity()
        {
            UserContentProvider user = new UserContentProvider();
            List<ActivityResult> result = await user.QueryFriendActivities(0, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);


            ActivityResult = new ObservableCollection<ActivityItem>();

            foreach (var item in result)
            {
                ActivityResult.Add(ConvertToActivityItem(item));
            }

            //循环添加数据到20条
            while(ActivityResult.Count <20)
            {
                List<ActivityResult> loop = await user.QueryFriendActivities(ActivityResult[ActivityResult.Count - 1].ActivityId - 1, 
                    GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);

                if (loop.Count == 0) break;

                foreach (var item in loop)
                {
                    ActivityResult.Add(ConvertToActivityItem(item));
                }
            }
        }

        private ActivityItem ConvertToActivityItem(ActivityResult source)
        {
            ActivityItem result = new ActivityItem();
            if(source.Source.Object.Avatar.EndsWith("default_avatar.png"))
            {
                result.Avatar = new Uri("ms-appx:///Assets/default_avatar.png", UriKind.Absolute);
            }
            else
            {
                result.Avatar = new Uri(source.Source.Object.Avatar.ToString(), UriKind.Absolute);
            }
            result.NickName = source.Source.Object.Nickname;
            result.UserLink = new Tuple<string, string>(item1: "user", item2: source.Source.Object.Slug);
            result.ActivityId = source.Id;
            result.CreatedAt = source.CreatedAt;
            if(source.Event == "like_something")
            {
                if(source.Target.Type == "User")
                {
                    result.Type = ActivityType.FollowUser;
                    result.Target = source.Target.Object.Nickname;
                    result.TargetLink = new Tuple<string, string>(item1: "user", item2: source.Target.Object.Slug);
                }
                else if(source.Target.Type == "Note")
                {
                    result.Type = ActivityType.LikeNote;
                    result.Target = source.Target.Object.Title;
                    result.TargetLink = new Tuple<string, string>(item1: "note", item2: source.Target.Object.Slug);
                }
                else if(source.Target.Type == "Notebook")
                {
                    result.Type = ActivityType.LikeNoteBook;
                    result.Target = source.Target.Object.Title ?? source.Target.Object.Name;
                    result.TargetLink = new Tuple<string, string>(item1: "notebook", item2:"Nothing"/*item2: source.Target.Object.Notebook.Slug*/);
                }
                else if(source.Target.Type == "Collection")
                {
                    result.Type = ActivityType.LikeCollection;
                    result.Target = source.Target.Object.Title ?? source.Target.Object.Name;
                    result.TargetLink = new Tuple<string, string>(item1: "collection", item2: "Nothing"/*item2: source.Target.Object.Slug*/);
                }
            }
            else if(source.Event == "comment_on_note")
            {
                result.Type = ActivityType.Comment;
                result.Target = source.Target.Object.Note.Title;
                result.Content = source.Target.Object.CompiledContent;
                result.TargetLink = new Tuple<string, string>(item1: "comment_on_note", item2: source.Target.Object.Note.Slug);
            }
            else if(source.Event == "user_created")
            {
                result.Type = ActivityType.Created;
            }
            else if(source.Event == "share_note")
            {
                result.Type = ActivityType.Note;
                result.Target = source.Target.Object.Title;
                result.TargetLink = new Tuple<string, string>(item1: "share_note", item2: source.Target.Object.Slug);
            }
            return result;
        }

        #region 加载更多和下拉刷新
        private async Task RefreshActivity()
        {
            await QueryActivity();
        }

        private async Task MoreActivity()
        {
            UserContentProvider user = new UserContentProvider();
            List<ActivityResult> result = await user.QueryFriendActivities(
                ActivityResult[ActivityResult.Count - 1].ActivityId - 1, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);

            foreach (var item in result)
            {
                ActivityResult.Add(ConvertToActivityItem(item));
            }
        }
        #endregion
        #endregion
    }
}

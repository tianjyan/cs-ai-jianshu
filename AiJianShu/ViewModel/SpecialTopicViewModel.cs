using AiJianShu.Command;
using AiJianShu.Common;
using AiJianShu.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JianShuCore;
using JianShuCore.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AiJianShu.ViewModel
{
    public class SpecialTopicViewModel : ViewModelBase
    {
        #region 字段
        private TopicItem selectedItem;
        private DiscoverItem typeSelectedItem;
        private ObservableCollection<TopicItem> topicItems;
        private ObservableCollection<TopicLastItem> lastNotes;
        private int page;
        private int lastPage;
        #endregion

        #region 属性
        public AsyncCommand RefreshCommand { get; set; }
        public AsyncCommand MoreCommand { get; set; }
        public AsyncCommand RefreshNoteCommand { get; set; }
        public AsyncCommand MoreNoteCommand { get; set; }
        public RelayCommand<int> SubscriptionCommand { get; set; }

        public string DeviceFamily
        {
            get
            {
                return Untils.DeviceFamily;
            }
        }

        public ObservableCollection<TopicItem> TopicItems
        {
            get
            {
                return topicItems;
            }
            set
            {
                topicItems = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TopicLastItem> LastNotes
        {
            get
            {
                return lastNotes;
            }
            set
            {
                lastNotes = value;
                RaisePropertyChanged();
            }
        }

        public TopicItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                RefershNote();
                RaisePropertyChanged();
            }
        }

        public List<DiscoverItem> DisconverItems { get; set; }

        public DiscoverItem TypeSelectedItem
        {
            get
            {
                return typeSelectedItem;
            }
            set
            {
                typeSelectedItem = value;
                Refresh();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region 构造函数
        public SpecialTopicViewModel()
        {
            Initialized();
            InitCMD();
        }
        #endregion

        #region 公有方法
        public void CheckInitialized()
        {
            if (TypeSelectedItem == null)
            {
                TypeSelectedItem = DisconverItems[0];
            }
        }
        #endregion

        #region 私有方法
        #region 初始化
        private void Initialized()
        {
            DisconverItems = new List<DiscoverItem>();
            DisconverItems.Add(new DiscoverItem() { Content = "热门", Type = TopicType.None });
            DisconverItems.Add(new DiscoverItem() { Content = "生活家", Type = TopicType.Lifehome });
            DisconverItems.Add(new DiscoverItem() { Content = "世间事", Type = TopicType.World });
            DisconverItems.Add(new DiscoverItem() { Content = "@IT", Type = TopicType.IT });
            DisconverItems.Add(new DiscoverItem() { Content = "电影", Type = TopicType.Movie });
            DisconverItems.Add(new DiscoverItem() { Content = "经典", Type = TopicType.Classic });
            DisconverItems.Add(new DiscoverItem() { Content = "连载", Type = TopicType.Publish });
            DisconverItems.Add(new DiscoverItem() { Content = "读图", Type = TopicType.Blueprint });
            DisconverItems.Add(new DiscoverItem() { Content = "程序员", Type = TopicType.Programer });
            DisconverItems.Add(new DiscoverItem() { Content = "市集", Type = TopicType.Fair });
            DisconverItems.Add(new DiscoverItem() { Content = "有意思", Type = TopicType.Interesting });
            DisconverItems.Add(new DiscoverItem() { Content = "书", Type = TopicType.Book });
            DisconverItems.Add(new DiscoverItem() { Content = "热议", Type = TopicType.Comment });
            DisconverItems.Add(new DiscoverItem() { Content = "音乐", Type = TopicType.Music });
            DisconverItems.Add(new DiscoverItem() { Content = "电视", Type = TopicType.TV });
            DisconverItems.Add(new DiscoverItem() { Content = "二次元", Type = TopicType.Quadratic });
            DisconverItems.Add(new DiscoverItem() { Content = "旅行", Type = TopicType.Travel });
            DisconverItems.Add(new DiscoverItem() { Content = "美食", Type = TopicType.Food });
            DisconverItems.Add(new DiscoverItem() { Content = "时尚", Type = TopicType.Fashion });
            DisconverItems.Add(new DiscoverItem() { Content = "体育", Type = TopicType.Sport });
            DisconverItems.Add(new DiscoverItem() { Content = "猫猫狗狗", Type = TopicType.Pet });
            DisconverItems.Add(new DiscoverItem() { Content = "植物", Type = TopicType.Plant });
            DisconverItems.Add(new DiscoverItem() { Content = "诗", Type = TopicType.Poetry });
            DisconverItems.Add(new DiscoverItem() { Content = "城市", Type = TopicType.City });
            DisconverItems.Add(new DiscoverItem() { Content = "历史", Type = TopicType.Histo });
            DisconverItems.Add(new DiscoverItem() { Content = "理财", Type = TopicType.Finance });
            DisconverItems.Add(new DiscoverItem() { Content = "职场", Type = TopicType.Job });
            DisconverItems.Add(new DiscoverItem() { Content = "语言", Type = TopicType.Language });
            DisconverItems.Add(new DiscoverItem() { Content = "学术", Type = TopicType.Knowledge });
            DisconverItems.Add(new DiscoverItem() { Content = "个人收藏和其他", Type = TopicType.Misc });
        }

        private void InitCMD()
        {
            RefreshCommand = new AsyncCommand(Refresh);
            MoreCommand = new AsyncCommand(More);
            RefreshNoteCommand = new AsyncCommand(RefershNote);
            MoreNoteCommand = new AsyncCommand(MoreNote);
            SubscriptionCommand = new RelayCommand<int>(ExecuteSubscriptionCommand);
        }
        #endregion

        #region 下拉刷新和加载更多
        private async Task Refresh()
        {
            page = 1;
            await QueryTopics();
        }

        private async Task More()
        {
            await QueryTopics();
        }

        private async Task RefershNote()
        {
            lastPage = 0;
            await QueryLast();
        }

        private async Task MoreNote()
        {
            await QueryLast();
        }

        private async Task QueryTopics()
        {            
            if(page == 1)
            {
                if(TopicItems == null)
                {
                    TopicItems = new ObservableCollection<TopicItem>();
                }
                else
                {
                    TopicItems.Clear();
                }
            }

            UserContentProvider user = new UserContentProvider();
            List<DiscoverResult> reuslt = await user.QueryTopic(TypeSelectedItem.Type, page, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);

            reuslt.ForEach(x => TopicItems.Add(ConvertToTopicItem(x)));

            page++;
        }

        private async void ExecuteSubscriptionCommand(int id)
        {
            TopicItem value = TopicItems.First(x => x.Id == id);
            if (value.IsSubscribed)
            {
                UserContentProvider user = new UserContentProvider();
                DiscoverSubscribeResult reuslt = await user.Unsubscribe(id, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
                if (reuslt.Error == null || reuslt.Error.Count == 0)
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

        private async Task QueryLast()
        {
            if (SelectedItem == null) return;

            if (lastPage == 0)
            {
                if (LastNotes == null)
                {
                    LastNotes = new ObservableCollection<TopicLastItem>();
                }
                else
                {
                    LastNotes.Clear();
                }
            }

            CommonProvider common = new CommonProvider();
            List<TopicLastResult> result = await common.QueryTopicLast(SelectedItem.Id.ToString(), lastPage);

            result.ForEach(x => LastNotes.Add(ConvertToTopicLastItem(x)));

            if (LastNotes?.Any() == true)
            {
                lastPage = LastNotes.Last().Id - 1;
            }

        }

        #endregion

        #region 其他
        private TopicItem ConvertToTopicItem(DiscoverResult source)
        {
            return new TopicItem()
            {
                Id = source.Id,
                IsSubscribed = source.IsSubscribed,
                Title = source.Title,
                Slug = source.Slug,
                NewlyAddedAt = source.NewlyAddedAt.ToString(),
                Description = source.Description,
                Image = source.Image,
                NotesCount = source.NotesCount,
                SubscribersCount = source.SubscribersCount,
                Coeditors = source.Coeditors.Select(c => c.Nickname).ToList(),
                Owner = source.Owner.Nickname
            };
        }

        private TopicLastItem ConvertToTopicLastItem(TopicLastResult source)
        {
            return new TopicLastItem()
            {
                Id = source.Id,
                NoteId = source.Note.Id,
                Title = source.Note.Title,
                Desc = source.Note.Desc,
                Avatar = source.Note.Notebook.User.Avatar
            };
        }
        #endregion
        #endregion
    }
}

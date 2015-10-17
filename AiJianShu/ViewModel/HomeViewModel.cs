using AiJianShu.Command;
using AiJianShu.Common;
using AiJianShu.Model;
using GalaSoft.MvvmLight;
using JianShuCore;
using JianShuCore.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AiJianShu.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        #region 字段
        private TrendingResult selectedItem;
        private DiscoverItem typeSelectedItem;
        private NoteDetailResult noteDetail;
        private ObservableCollection<TrendingResult> trendingList;
        private int pageIndex;
        #endregion

        #region 属性
        public AsyncCommand RefreshCommand { get; set; }
        public AsyncCommand MoreCommand { get; set; }
        public List<DiscoverItem> DisconverItems { get; set; }

        public string DeviceFamily
        {
            get
            {
                return Untils.DeviceFamily;
            }
        }

        public ObservableCollection<TrendingResult> TrendingList
        {
            get
            {
                return trendingList;
            }
            set
            {
                trendingList = value;
                RaisePropertyChanged();
            }
        }

        public TrendingResult SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                //当选中文章改变时,获取文章
                if (selectedItem != null)
                {
                    QueryDetail(selectedItem.Id.ToString());
                }
                RaisePropertyChanged();
            }
        }

        public DiscoverItem TypeSelectedItem
        {
            get
            {
                return typeSelectedItem;
            }
            set
            {
                typeSelectedItem = value;
                pageIndex = 0;
                if (typeSelectedItem != null)
                {
                    QueryTrending(value);
                }
                RaisePropertyChanged();
            }
        } 

        public NoteDetailResult NoteDetail
        {
            get
            {
                return noteDetail;
            }
            set
            {
                noteDetail = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region 构造函数
        public HomeViewModel()
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

        public override void Cleanup()
        {
            base.Cleanup();
            TypeSelectedItem = null;
            pageIndex = 0;
        }
        #endregion

        #region 私有方法
        #region 初始化
        private void Initialized()
        {
            DisconverItems = new List<DiscoverItem>();
            DisconverItems.Add(new DiscoverItem() { Content = "热门", HotType = HotTopicType.now });
            DisconverItems.Add(new DiscoverItem() { Content = "七日热门", HotType = HotTopicType.weekly });
            DisconverItems.Add(new DiscoverItem() { Content = "三十日热门", HotType = HotTopicType.monthly });
            DisconverItems.Add(new DiscoverItem() { Content = "最新", Type = TopicType.Newset });
            DisconverItems.Add(new DiscoverItem() { Content = "生活家", Type = TopicType.Lifehome });
            DisconverItems.Add(new DiscoverItem() { Content = "世间事", Type = TopicType.World });
            DisconverItems.Add(new DiscoverItem() { Content = "@IT", Type = TopicType.IT });
            DisconverItems.Add(new DiscoverItem() { Content = "电影", Type = TopicType.Movie });
            DisconverItems.Add(new DiscoverItem() { Content = "经典", Type = TopicType.Classic });
            DisconverItems.Add(new DiscoverItem() { Content = "连载", Type = TopicType.Publish });
            DisconverItems.Add(new DiscoverItem() { Content = "读图", Type = TopicType.Blueprint });
            DisconverItems.Add(new DiscoverItem() { Content = "市集", Type = TopicType.Fair });
        }

        private void InitCMD()
        {
            RefreshCommand = new AsyncCommand(RefreshTrending);
            MoreCommand = new AsyncCommand(MoreTrending);
        }
        #endregion

        #region 加载更多和下拉刷新
        private async Task RefreshTrending()
        {
            pageIndex = 0;
            await QueryTrending(TypeSelectedItem);
        }

        private async Task MoreTrending()
        {
            await QueryTrending(TypeSelectedItem);
        }

        private async Task QueryTrending(DiscoverItem item)
        {
            CommonProvider common = new CommonProvider();
            List<TrendingResult> result = new List<TrendingResult>();

            if (pageIndex == 0)
            {
                if (TrendingList == null)
                {
                    TrendingList = new ObservableCollection<TrendingResult>();
                }
                else
                {
                    TrendingList.Clear();
                }
            }

            if (TypeSelectedItem.HotType != HotTopicType.None)
            {
                result = await common.QueryDiscover(TypeSelectedItem.HotType, pageIndex + 1, 20, new List<TrendingResult>(TrendingList));
            }

            if (TypeSelectedItem.Type != TopicType.None)
            {
                int index = 0;
                if (TrendingList.Count != 0)
                {
                    index = TrendingList[TrendingList.Count - 1].RecommendedAt - 1;
                }
                result = await common.QueryDiscover(TypeSelectedItem.Type, index, 20, 20);
            }

            result.ForEach(x => TrendingList.Add(x));

            pageIndex++;
        }

        private async void QueryDetail(string id)
        {
            var temp = Untils.ReadNoteDetial(id);
            if (temp == null)
            {
                UserContentProvider user = new UserContentProvider();
                NoteDetail = await user.QueryNoteDetail(id, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
                Untils.WriteNoteDetial(NoteDetail);
            }
            else
            {
                NoteDetail = temp;
            }
        }
        #endregion
        #endregion
    }
}

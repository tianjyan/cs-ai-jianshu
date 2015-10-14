using AiJianShu.Command;
using AiJianShu.Common;
using GalaSoft.MvvmLight;
using JianShuCore;
using JianShuCore.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AiJianShu.ViewModel
{
    public class FollowViewModel : ViewModelBase
    {
        #region 字段
        private SubscriptionNotesResult selectedItem;
        private ObservableCollection<SubscriptionNotesResult> subscriptionList;
        private NoteDetailResult noteDetail;
        private int pageIndex;
        #endregion

        #region 属性
        public AsyncCommand RefreshCommand { get; set; }
        public AsyncCommand MoreCommand { get; set; }
        public string DeviceFamily
        {
            get
            {
                return Untils.DeviceFamily;
            }
        }

        public ObservableCollection<SubscriptionNotesResult> SubscriptionList
        {
            get
            {
                return subscriptionList;
            }
            set
            {
                subscriptionList = value;
                RaisePropertyChanged();
            }
        }

        public SubscriptionNotesResult SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                if (selectedItem != null)
                {
                    QueryDetail(selectedItem.Id.ToString());
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
        public FollowViewModel()
        {
            InitCMD();
        }
        #endregion

        #region 公有方法
        public async void CheckInitialized()
        {
            if(subscriptionList == null)
            {
                await QuerySubscription();
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
            subscriptionList = null;
            pageIndex = 0;
        }
        #endregion

        #region 私有方法
        #region 初始化
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
            await QuerySubscription();
        }

        private async Task MoreTrending()
        {
            await QuerySubscription();
        }
        #endregion

        #region 其他
        private async Task QuerySubscription()
        {
            UserContentProvider user = new UserContentProvider();

            if(pageIndex == 0)
            {
                if(SubscriptionList == null)
                {
                    SubscriptionList = new ObservableCollection<SubscriptionNotesResult>();
                }
                else
                {
                    SubscriptionList.Clear();
                }
            }

            int index = 0;
            if(SubscriptionList.Count !=0)
            {
                index = SubscriptionList[SubscriptionList.Count - 1].ReceivedAt - 1;
            }
            List<SubscriptionNotesResult> result = await user.QuerySubscriptionNotes(index, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
            result.ForEach(x => SubscriptionList.Add(x));
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

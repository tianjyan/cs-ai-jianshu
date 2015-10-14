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
    public class LikeViewModel : ViewModelBase
    {
        #region 字段
        private List<NoteItem> likeList;
        private List<NoteItem> markList;
        private string typeSelectedItem;
        private NoteItem selectedItem;
        private ObservableCollection<NoteItem> displayCollection;
        private NoteDetailResult noteDetail;
        private int likePage;
        private int markPage;
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

        public ObservableCollection<NoteItem> DisplayCollection
        {
            get
            {
                return displayCollection;
            }
            set
            {
                displayCollection = value;
                RaisePropertyChanged();
            }
        }

        public string TypeSelectedItem
        {
            get
            {
                return typeSelectedItem;
            }
            set
            {
                typeSelectedItem = value;
                TypeSelectedItemChanged(value);
                RaisePropertyChanged();
            }
        }

        public NoteItem SelectedItem
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

        public List<string> Items { get; set; }

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

        #region 公有方法
        public void CheckInitialized()
        {
            if (TypeSelectedItem == null)
            {
                TypeSelectedItem = Items[0];
            }
        }
        #endregion

        #region 构造函数
        public LikeViewModel()
        {
            Initialized();
            InitCMD();
        }
        #endregion

        #region 私有方法
        #region 初始化
        private void Initialized()
        {
            Items = new List<string>();
            Items.Add("喜欢");
            Items.Add("收藏");
        }

        private void InitCMD()
        {
            RefreshCommand = new AsyncCommand(RefreshNote);
            MoreCommand = new AsyncCommand(MoreNote);
        }

        #region 下拉刷新和加载更多
        private async Task RefreshNote()
        {
            if (typeSelectedItem == "喜欢")
            {
                likePage = 0;
            }
            else
            {
                markPage = 0;
            }
            await QueryNote(typeSelectedItem);
        }

        private async Task MoreNote()
        {
            await QueryNote(typeSelectedItem);
        }
        #endregion
        #endregion

        #region 其他
        private async void TypeSelectedItemChanged(string type)
        {
            if (type == "喜欢" && likePage != 0)
            {
                DisplayCollection.Clear();
                likeList.ForEach(x => DisplayCollection.Add(x));
            }
            else if(type == "收藏" && markPage != 0)
            {
                DisplayCollection.Clear();
                markList.ForEach(x => DisplayCollection.Add(x));
            }
            else
            {
                await QueryNote(type);
            }

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

        private async Task QueryNote(string type)
        {
            UserContentProvider user = new UserContentProvider();
            
            if(type == "喜欢")
            {
                if(likePage == 0)
                {
                    DisplayCollection = new ObservableCollection<NoteItem>();
                    likeList = new List<NoteItem>();
                }

                List<Note> temp = await user.QueryLikeNotes(likePage, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);

                temp.ForEach(x =>
                {
                    var v = ConvertToNoteItem(x);
                    likeList.Add(v);
                    DisplayCollection.Add(v);
                });
                likePage++;
            }
            else
            {
                if(markPage == 0)
                {
                    DisplayCollection = new ObservableCollection<NoteItem>();
                    markList = new List<NoteItem>();
                }

                List<BookmarkResult> temp = await user.QueryBookmark(markPage, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
                temp.ForEach(x =>
                {
                    var v = ConvertToNoteItem(x.Note);
                    markList.Add(v);
                    DisplayCollection.Add(v);
                });
            }
        }

        private NoteItem ConvertToNoteItem(Note source)
        {
            return new NoteItem()
            {
                Id = source.Id,
                Desc = source.Desc,
                PublishedAt = source.PublishedAt,
                Title = source.Title,
                NickName = source.Notebook.User.Nickname,
                Avatar = source.Notebook.User.Avatar,
                IsLike = true
            };
        }
        #endregion
        #endregion

    }
}

using AiJianShu.Common;
using AiJianShu.Model;
using GalaSoft.MvvmLight;
using JianShuCore;
using JianShuCore.Model;
using System.Threading.Tasks;

namespace AiJianShu.ViewModel
{
    public class ArticleViewModel : ViewModelBase
    {
        #region 字段
        private ViewType backView;
        private NoteDetailResult noteDetail;
        #endregion

        #region 属性
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
        public ArticleViewModel()
        {
            InitListener();
        }
        #endregion

        #region 私有方法
        #region 初始化
        private void InitListener()
        {
            this.MessengerInstance.Register<ArticleItem>(this, ShowArticle);
        }
        #endregion

        private async void ShowArticle(ArticleItem article)
        {
            backView = article.BackView;
            await QueryDetail(article.NoteId);
        }

        private async Task QueryDetail(string id)
        {
            UserContentProvider user = new UserContentProvider();
            NoteDetail = await user.QueryNoteDetail(id, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
            Untils.WriteNoteDetial(NoteDetail);
        }
        #endregion

        #region 公有方法
        public void BackPreView()
        {
            MessengerInstance.Send<ChangeView>(new ChangeView() { FromView = ViewType.Article, ToView = backView });
        }
        #endregion
    }
}

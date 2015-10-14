using AiJianShu.Common;
using AiJianShu.Model;
using JianShuCore;
using JianShuCore.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace AiJianShu.UserControls
{
    public sealed partial class ArticleControl : UserControl
    {
        #region 字段
        private Storyboard OpenStoryboard;
        private Storyboard CloseStoryboard;
        private bool CommentIsOpened;
        private int CommentIndex;
        private Tuple<string, string> ReplyTarget;//Item1--NickName, Item2--UserId
        private ObservableCollection<CommentsResult> CommentList;
        #endregion

        #region 依赖属性
        public string Note
        {
            get { return (string)GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(NoteDetailResult), typeof(ArticleControl),
                new PropertyMetadata(default(NoteDetailResult), new PropertyChangedCallback(ArticleControl.OnNoteChanged)));

        public static void OnNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ArticleControl control = d as ArticleControl;
                control.NoteDetail = e.NewValue as NoteDetailResult;
                
                if(control.NoteDetail.IsLiked)
                {
                    control.LikeBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    control.LikeBtn.Visibility = Visibility.Collapsed;
                }

                control.progressRing.IsActive = true;
                control.webView.NavigateToString(control.NoteDetail.MobileContent);
                control.commentBtn.Opacity = 1;
            }

        }
        #endregion

        #region 属性
        public NoteDetailResult NoteDetail { get; set; }
        public Visibility BackBtnVisibility
        {
            get
            {
                return BackBtn.Visibility;
            }
            set
            {
                BackBtn.Visibility = value;
            }
        }
        public bool CommentOpened
        {
            get
            {
                return CommentIsOpened;
            }
        }
        #endregion

        #region 事件
        public event EventHandler<RoutedEventArgs> BackClick;
        #endregion

        #region 构造函数
        public ArticleControl()
        {
            this.InitializeComponent();

            Loaded += ArticleControlLoaded;

            OpenStoryboard = this.Resources["OpenComment"] as Storyboard;
            CloseStoryboard = this.Resources["CloseComment"] as Storyboard;
        }
        #endregion

        #region 私有方法
        private void ArticleControlLoaded(object sender, RoutedEventArgs e)
        {
            switch (Untils.DeviceFamily)
            {
                case "Desktop":
                    break;
                case "Mobile":
                    RowDefinition3.Height = (GridLength)App.Current.Resources["PanelTopHeight"];
                    ArticleStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(ArticleGrid, 2);
                    BackBtn.Opacity = 0;
                    break;
                default:
                    break;
            }
        }

        private void NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            progressRing.IsActive = false;
        }

        #region 后退按钮
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (BackClick != null)
            {
                BackClick(sender, e);
            }
            CloseCommentGrid();
        }
        #endregion

        #region 喜欢
        private async void LikeClick(object sender, RoutedEventArgs e)
        {
            UserContentProvider user = new UserContentProvider();
            LikeResult reuslt = null;
            if (LikeBtn.Visibility == Visibility.Collapsed)
            {
                reuslt = await user.LikeNote(NoteDetail.Id, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
            }
            else
            {
                reuslt = await user.UnlikeNote(NoteDetail.Id, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
            }

            Untils.UpdateNoteDetail(NoteDetail);

            LikeBtn.Visibility = reuslt.isLiked ? Visibility.Visible : Visibility.Collapsed;
            if (reuslt.isLiked)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ShowMessage() { MessageContent = App.Current.Resources["LikeMessage"].ToString() });
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ShowMessage() { MessageContent = App.Current.Resources["UnlikeMessage"].ToString() });
            }
        }
        #endregion

        #region 分享
        private void ShareWeiboClick(object sender, RoutedEventArgs e)
        {
            WeiboShare(weiboShareTb.Text);
            ShareFlyout.Hide();
        }

        private void ShareDismissClick(object sender, RoutedEventArgs e)
        {
            ShareFlyout.Hide();
        }

        private void ShareClick(object sender, RoutedEventArgs e)
        {
            weiboShareTb.ClearValue(TextBox.TextProperty);
        }

        private async void WeiboShare(string content)
        {
            string text = string.Format(App.Current.Resources["ShareArticle"].ToString(), content, this.NoteDetail.Title);

            if (await Untils.WeiboShareImage(NoteDetail.ImageUrl, text))
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ShowMessage() { MessageContent = App.Current.Resources["ShareSuccess"].ToString() });
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ShowMessage() { MessageContent = App.Current.Resources["ShareFail"].ToString() });
            }
        }
        #endregion

        #region 阅读评论
        #region 加载评论和刷新评论
        private async Task QueryComment(int noteid)
        {
            if (CommentIndex == 0)
            {
                if (CommentList == null)
                {
                    CommentList = new ObservableCollection<CommentsResult>();
                    commentListView.ItemsSource = CommentList;
                }
                else
                {
                    CommentList.Clear();
                }
            }

            int index = 0;
            if (CommentList.Count != 0)
            {
                index = CommentList[CommentList.Count - 1].Id - 1;
            }

            List<CommentsResult> result = new List<CommentsResult>();

            CommonProvider common = new CommonProvider();
            result = await common.QueryComment(noteid.ToString(), index);

            result.ForEach(x => CommentList.Add(x));

            CommentIndex++;
        }

        private async void RefreshComment(object sender, EventArgs e)
        {
            CommentIndex = 0;
            await QueryComment(NoteDetail.Id);
        }

        private async void MoreComment(object sender, EventArgs e)
        {
            await QueryComment(NoteDetail.Id);
        }
        #endregion

        #region 评论打开关闭和侧滑
        private void CloseCommentGrid()
        {
            CloseStoryboard.Begin();
            CommentIsOpened = false;
            DismissLayout.Visibility = Visibility.Collapsed;
        }

        private void OpenCommentGrid()
        {
            OpenStoryboard.Begin();
            RefreshComment(this, EventArgs.Empty);
            CommentIsOpened = true;
            DismissLayout.Visibility = Visibility.Visible;
        }

        private void CommentGridManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var x = e.Velocities.Linear.X;
            if (x <= -0.1)
            {
                OpenCommentGrid();
            }
            else if (x > -0.1 && x < 0.1)
            {
                if (Math.Abs((commentGrid.RenderTransform as CompositeTransform).TranslateX) > 150)
                {
                    OpenCommentGrid();
                }
                else
                {
                    CloseCommentGrid();
                }

            }
            else
            {
                CloseCommentGrid();
            }
        }

        private void CommentGridManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {

            var x = (commentGrid.RenderTransform as CompositeTransform).TranslateX + e.Delta.Translation.X;
            if (x < -300)
            {
                x = -300;
            }
            (commentGrid.RenderTransform as CompositeTransform).TranslateX = x;
            (ManipulationLayout.RenderTransform as CompositeTransform).TranslateX = x;
        }

        private void CommentBtnClick(object sender, RoutedEventArgs e)
        {
            if (CommentIsOpened)
            {
                CloseCommentGrid();
            }
            else
            {
                OpenCommentGrid();
            }
        }

        private void DismissLayoutTapped(object sender, TappedRoutedEventArgs e)
        {
            CloseCommentGrid();
        }
        #endregion
        #endregion

        #region 添加评论
        private void DismissCommentClick(object sender, RoutedEventArgs e)
        {
            CommentFlyout.Hide();
        }

        private void LinkReplyClick(object sender, UserControls.HyperLinkEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Run run = link.Inlines[0] as Run;

            Tuple<string, string, string> tuple = e.Tag as Tuple<string, string, string>;

            ReplyTarget = new Tuple<string, string>(run.Text, tuple.Item2);

            FillCommentTextBox(ReplyTarget);
        }

        private void ReplyClick(object sender, RoutedEventArgs e)
        {
            CommentsResult result = (sender as Button).Tag as CommentsResult;
            ReplyTarget = new Tuple<string, string>(result.User.Nickname, result.User.Slug);

            FillCommentTextBox(ReplyTarget);
        }

        private void FillCommentTextBox(Tuple<string, string> tuple)
        {
            CommentFlyout.ShowAt(commentFlyoutBtn);
            commentTb.ClearValue(TextBox.TextProperty);
            commentTb.Text = "@" + tuple.Item1.Trim('@') + " ";
            commentTb.SelectionStart = int.MaxValue;
        }

        private void FlyoutClick(object sender, RoutedEventArgs e)
        {
            ReplyTarget = null;
            commentTb.ClearValue(TextBox.TextProperty);
        }

        private async void SendeCommentClick(object sender, RoutedEventArgs e)
        {
            await Untils.SendComment(commentTb.Text, ReplyTarget, NoteDetail.Id.ToString());
            CommentFlyout.Hide();
        }
        #endregion
        #endregion

        #region 公有方法
        public void CloseCommentPanel()
        {
            CloseCommentGrid();
        }
        #endregion
    }
}
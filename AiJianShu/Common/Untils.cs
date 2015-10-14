using AiJianShu.Model;
using GalaSoft.MvvmLight;
using JianShuCore;
using JianShuCore.Common;
using JianShuCore.Model;
using JianShuCore.Provider;
using SocialShare.Weibo;
using SocialShare.Weibo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AiJianShu.Common
{
    /// <summary>
    /// 辅助功能类
    /// </summary>
    public class Untils
    {
        static Untils()
        {
            ResourceContext resContext = ResourceContext.GetForCurrentView();
            DeviceFamily = resContext.QualifierValues["DeviceFamily"];
            cache = new CacheProvider(StorageType.SqliteStorage);
        }

        public static string DeviceFamily { get; set; }

        private static CacheProvider cache;

        public const string HtmlFile = "WebView.html";

        public static Dictionary<string, DataTemplate> views = new Dictionary<string, DataTemplate>();

        //Reference:http://www.codeproject.com/Articles/113152/Applying-Data-Templates-Dynamically-by-Type-in-WP
        //解决DataTemplate没有DataType的属性的问题.
        public static class DataTemplateSelector
        {
            public static DataTemplate GetTemplate(ViewModelBase param)
            {
                Type t = param.GetType();
                if(!views.ContainsKey(t.Name))
                {
                    views.Add(t.Name, App.Current.Resources[t.Name] as DataTemplate);
                }

                return views[t.Name];
            }
        }

        /// <summary>
        /// 保存Html文本到本地
        /// </summary>
        /// <param name="content"></param>
        public static async void SaveHtml(string content)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.CreateFileAsync(HtmlFile, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);
        }

        #region WebView附加属性,解决WebView不能绑定Html文本信息的问题
        public static readonly DependencyProperty SourceStringProperty =
        DependencyProperty.RegisterAttached("SourceString", typeof(string), typeof(Untils), new PropertyMetadata("", OnSourceStringChanged));

        public static string GetSourceString(DependencyObject obj) { return (string)obj.GetValue(SourceStringProperty); }
        public static void SetSourceString(DependencyObject obj, string value) { obj.SetValue(SourceStringProperty, value); }

        private static void OnSourceStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView wv = d as WebView;
            if (wv != null)
            {
                wv.NavigateToString((string)e.NewValue);
            }
        }
        #endregion

        #region 新浪微博分享文本和图片
        public static async Task<bool> WeiboShareImage(string url, string text)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("Image or Text should be empty");
            }
            return await WeiboShare(url, text);
        }

        public static async Task<bool> WeiboShareText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("Image or Text should be empty");
            }
            return await WeiboShare(string.Empty, text);
        }

        private static async Task<bool> WeiboShare(string url, string text)
        {
            CacheProvider cache = new CacheProvider(StorageType.IsolatedStorage);

            if (GlobalValue.CurrentWeiboUserInfo == null)
            {
                GlobalValue.CurrentWeiboUserInfo = cache.GetItem<UserInfo>(CacheKey.WeiboUser) ?? new UserInfo();
            }
            byte[] image = await new HttpClient().GetByteArrayAsync(url);

            bool shareResult = false;

            try
            {
                WeiboClient client = new WeiboClient(GlobalValue.CurrentWeiboUserInfo);
                await client.LoginAsync();
                WeiboResult result;
                if (!string.IsNullOrEmpty(url))
                {
                    result = await client.ShareImageAsync(image, text);
                }
                else
                {
                    result = await client.ShareTextAsync(text);
                }
                GlobalValue.CurrentWeiboUserInfo = client.UserInfo;

                cache.UpdateItem(CacheKey.WeiboUser, GlobalValue.CurrentWeiboUserInfo);
                shareResult = true;
            }
            catch
            {
                shareResult = false;
            }
            return shareResult;
        }
        #endregion

        #region 发送评论
        public static async Task<bool> SendComment(string commentContent, Tuple<string, string> replyTarget, string noteId)
        {
            UserContentProvider content = new UserContentProvider();
            var result = await content.AddComment(noteId, commentContent, GlobalValue.CurrentUserContext.UserId, GlobalValue.CurrentUserContext.MobileToken);
            if(result.Error == null || result.Error.Count == 0)
            {
               GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ShowMessage() { MessageContent = "评论成功!" });
                return true;
            }
            else
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new ShowMessage() { MessageContent = "评论失败!" });
                return false;
            }
        }
        #endregion

        #region 缓存文章
        public static void WriteNoteDetial(NoteDetailResult noteDetail)
        {
            cache.AddItem(CacheKey.NoteDetail + noteDetail.Id, noteDetail);
        }

        public static NoteDetailResult ReadNoteDetial(string id)
        {
            return cache.GetItem<NoteDetailResult>(CacheKey.NoteDetail + id);
        }

        public static bool UpdateNoteDetail(NoteDetailResult noteDetail)
        {
            if(cache.ContainItem(CacheKey.NoteDetail + noteDetail.Id))
            {
                cache.UpdateItem(CacheKey.NoteDetail + noteDetail.Id, noteDetail);
                return true;
            }
            return false;
        }

        public static long GetCacheSize()
        {
            return cache.GetSize();
        }

        public static void CleanUpCache()
        {
            string filePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "JianShuCahce.db");
            new FileInfo(filePath).Delete();
            cache = new CacheProvider(StorageType.SqliteStorage);
        }
        #endregion
    }
}

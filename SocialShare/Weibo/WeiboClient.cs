using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocialShare.Weibo.Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Web.Http;

namespace SocialShare.Weibo
{
    public class WeiboClient
    {
        public UserInfo UserInfo { get; set; }
        private AppInfo appInfo;
        public WeiboClient(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException();
            }

            this.UserInfo = userInfo;
            this.appInfo = new AppInfo() { Key = Config.Key, Secret = Config.Secret, RedirectUri = Config.Uri };
        }

        public async Task LoginAsync()
        {
            if(!UserInfo.CheckUseable())
            {
                await Authorize(await GetAuthorizeCodeAsync());
            }
        }

        private async Task<string> GetAuthorizeCodeAsync()
        {
            Uri uri = new Uri("https://api.weibo.com/oauth2/authorize");

            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            pairs.Add(new KeyValuePair<string, string>("client_id", appInfo.Key));
            pairs.Add(new KeyValuePair<string, string>("redirect_uri", appInfo.RedirectUri));
            pairs.Add(new KeyValuePair<string, string>("display", Untils.GetDisplay()));

            uri = Untils.AddHeader(uri, pairs);

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, uri, new Uri(appInfo.RedirectUri));
            string code = "";
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    code = new Uri(result.ResponseData).GetQueryParameter("code");
                    break;
                case WebAuthenticationStatus.UserCancel:
                    throw new Exception("user cancel authorize");
                case WebAuthenticationStatus.ErrorHttp:
                    throw new Exception("http connection error");
                default:
                    throw new Exception("unknow error");
            }
            return code;
        }

        private async Task Authorize(string code)
        {
            Uri uri = new Uri("https://api.weibo.com/oauth2/access_token");

            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();

            pairs.Add(new KeyValuePair<string, string>("client_id", appInfo.Key));
            pairs.Add(new KeyValuePair<string, string>("client_secret", appInfo.Secret));
            pairs.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            pairs.Add(new KeyValuePair<string, string>("code", code));
            pairs.Add(new KeyValuePair<string, string>("redirect_uri", appInfo.RedirectUri));

            HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(pairs);

            using (HttpClient client = new HttpClient())
            {
                DateTime time = DateTime.Now;

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync(uri, content);
                }
                catch (Exception ex)
                {
                    throw new Exception("network error", ex);
                }
                string json = await response.Content.ReadAsStringAsync();

                JObject accessToken = JsonConvert.DeserializeObject<JObject>(json);
                UserInfo.Token = accessToken["access_token"].ToString();
                UserInfo.ExpiresAt = Untils.ToTimestamp(time) + (long)accessToken["expires_in"];
                UserInfo.Uid = accessToken["uid"].ToString();
            }
        }

        public async Task<WeiboResult> ShareTextAsync(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Text could not be empty", nameof(text));
            }

            if (!UserInfo.CheckUseable())
            {
                string authorizeCode = await this.GetAuthorizeCodeAsync();
                await this.Authorize(authorizeCode);
            }

            Uri uri = new Uri("https://api.weibo.com/2/statuses/update.json");

            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("access_token", UserInfo.Token);
            pairs.Add("status", text);

            HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(pairs);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync(uri, content);
                }
                catch (Exception ex)
                {
                    throw new Exception("Network error", ex);
                }
                return await response.Content.ReadAsJsonAsync<WeiboResult>();
            }
        }

        public async Task<WeiboResult> ShareImageAsync(byte[] image, string text)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Text could not be empty", nameof(text));
            }

            if (!UserInfo.CheckUseable())
            {
                string authorizeCode = await this.GetAuthorizeCodeAsync();
                await this.Authorize(authorizeCode);
            }

            Uri uri = new Uri("https://upload.api.weibo.com/2/statuses/upload.json");

            HttpBufferContent bufferContent = new HttpBufferContent(image.AsBuffer());

            HttpMultipartFormDataContent content = new HttpMultipartFormDataContent();

            content.Add(new HttpStringContent(UserInfo.Token), "access_token");
            content.Add(new HttpStringContent(text), "status");
            content.Add(bufferContent, "pic", "pic.jpg");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync(uri, content);
                }
                catch (Exception ex)
                {
                    throw new Exception("Network error", ex);
                }
                return await response.Content.ReadAsJsonAsync<WeiboResult>();
            }
        }
    }
}

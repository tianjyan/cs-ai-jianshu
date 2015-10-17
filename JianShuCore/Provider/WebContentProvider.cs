using JianShuCore.Common;
using JianShuCore.Interface;
using JianShuCore.WebAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace JianShuCore.Provider
{
    public class WebContentProvider : IProvider
    {
        #region Private Field
        private static readonly WebContentProvider instance = new WebContentProvider();
        private static HttpClient httpClient;
        private string IMEI;
        #endregion

        #region Constructor
        private WebContentProvider()
        {
            Initialize();
        }
        #endregion

        #region Public Function

        #region Singleton Pattern
        public static WebContentProvider GetInstance()
        {
            if (IsConnectedToInternet())
            {
                return instance;
            }
            else
            {
                throw new HttpRequestException("Seems unable to connect to the network.");
            }
        }

        public static void CancelPendingRequests()
        {
            httpClient.CancelPendingRequests();
        }
        #endregion

        /// <summary>
        /// Initialize
        /// Initial the IMEI and HttpClient
        /// </summary>
        public void Initialize()
        {
            if (string.IsNullOrEmpty(IMEI))
            {
                Random random = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 15; i++)
                {
                    IMEI += random.Next(10);
                }

                httpClient = new HttpClient();
            }
        }

        /// <summary>
        /// Get Http Request
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>String</returns>
        public async Task<string> HttpGetRequest(string url)
        {
            string result = string.Empty;
            try
            {
                Untils.ChangeStatus(StatusType.Busy);
                result = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            finally
            {
                Untils.ChangeStatus(StatusType.Idle);
            }
            return result;
        }

        /// <summary>
        /// Post Http Request
        /// </summary>
        /// <param name="url">Request Url</param>
        /// <param name="contentStr">Post Content</param>
        /// <returns></returns>
        public async Task<string> HttpPostRequest(string url, string contentStr)
        {
            string result = string.Empty;
            try
            {
                Untils.ChangeStatus(StatusType.Busy);
                HttpContent httpContent = new StringContent(contentStr);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, httpContent);
                if (httpResponseMessage != null && httpResponseMessage.Content != null)
                {
                    result = await httpResponseMessage.Content.ReadAsStringAsync();
                }
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException("500 Internal Server Error");
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            finally
            {
                Untils.ChangeStatus(StatusType.Idle);
            }
            return result;
        }

        /// <summary>
        /// Get Http Request and Converter to T
        /// </summary>
        /// <typeparam name="T">Type you want to converter</typeparam>
        /// <param name="url">Request url</param>
        /// <param name="headers">Request headers</param>
        /// <returns>Type you want</returns>
        public async Task<T> HttpGetRequest<T>(string url, Dictionary<string, string> headers = null) where T : class
        {
            AddHeader(headers);
            string request =  url + string.Format("&device[guid]={0}&app[name]={1}&app[version]={2}", IMEI, Config.APP_NAME, Config.APP_VERSION);
            string response = await HttpGetRequest(request);
            T result;
            if (!string.IsNullOrEmpty(response))
            {
                T t = await Task.Factory.StartNew<T>(() => JsonConvert.DeserializeObject<T>(response));
                result = t;
            }
            else
            {
                result = default(T);
            }
            return result;
        }

        /// <summary>
        /// Post Htpp Request
        /// </summary>
        /// <typeparam name="T">Type you want to converter</typeparam>
        /// <param name="url">Post Url</param>
        /// <param name="content">Post Content</param>
        /// <param name="headers">Post Headers</param>
        /// <returns>Type you want</returns>
        public async Task<T> HttpPostRequest<T>(string url, string content, Dictionary<string, string> headers = null) where T : class
        {
            AddHeader(headers);
            string contentStr = content;
            if (!string.IsNullOrEmpty(contentStr))
            {
                contentStr += string.Format("&device[guid]={0}&app[name]={1}&app[version]={2}", IMEI, Config.APP_NAME, Config.APP_VERSION);
            }
            string response = await HttpPostRequest(url, contentStr);
            T result;
            if (!string.IsNullOrEmpty(response))
            {
                result = await Task.Factory.StartNew<T>(() => JsonConvert.DeserializeObject<T>(response));
            }
            else
            {
                result = default(T);
            }
            return result;
        }

        /// <summary>
        /// Generate Headers
        /// </summary>
        /// <param name="userId">User ID. Can be null</param>
        /// <param name="mobileToken">Mobile Token. Can be null</param>
        /// <returns></returns>
        public Dictionary<string, string> GetHeaders(string userId, string mobileToken)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            MD5CryptoProvider md5Crypto = new MD5CryptoProvider();
            string timeStamp = string.Concat(GetSystemCurrentTimeSeconds());
            byte[] data = md5Crypto.ComputeHash(Encoding.UTF8.GetBytes("be7db80162cce75a11eb280bd75b961d" + timeStamp));
            string text = md5Crypto.HashToString(data);

            dictionary.Add("X-Timestamp", timeStamp);
            dictionary.Add("X-Auth-1", text);
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(mobileToken))
            {
                dictionary.Add("X-User-Id", userId);
                dictionary.Add("X-AUTH-2", md5Crypto.ComputerHashAsString(mobileToken + timeStamp));
            }
            return dictionary;
        }
        #endregion

        #region Private Function
        private void AddHeader(Dictionary<string, string> headers)
        {
            if (httpClient != null && httpClient.DefaultRequestHeaders != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
            }
            if (headers != null)
            {
                using (Dictionary<string, string>.Enumerator enumerator = headers.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, string> current = enumerator.Current;
                        httpClient.DefaultRequestHeaders.Add(current.Key, current.Value);
                    }
                }
            }
        }

        private long GetSystemCurrentTimeSeconds()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000L) / 10000L / 1000L;
        }

        public static bool IsConnectedToInternet()
        {
            return NetworkInformation.GetInternetConnectionProfile()?.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }

        #endregion
    }
}

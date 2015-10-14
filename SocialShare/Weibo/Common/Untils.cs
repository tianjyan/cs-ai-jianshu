using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Web.Http;

namespace SocialShare.Weibo
{
    internal static class Untils
    {
        internal static long ToTimestamp(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        internal static DateTime FromTimestamp(long unixTimestamp)
        {
            DateTime utcDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return utcDateTime.AddSeconds(unixTimestamp).ToLocalTime();
        }

        internal static Uri AddHeader(Uri uri, string key, string value)
        {
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();

            string query = uri.Query;
            if (query == "?")
            {
                query = string.Empty;
            }
            if (query.Length > 0)
            {
                WwwFormUrlDecoder decoder = new WwwFormUrlDecoder(query);
                foreach (var nameValue in decoder)
                {
                    pairs.Add(new KeyValuePair<string, string>(nameValue.Name, nameValue.Value));
                }
            }

            key = WebUtility.UrlEncode(key);
            value = WebUtility.UrlEncode(value);

            bool exist = false;
            for (int i = 0; i < pairs.Count; i++)
            {
                if (pairs[i].Key == key)
                {
                    pairs[i] = new KeyValuePair<string, string>(key, value);
                    exist = true;
                    break;
                }
            }
            if (exist == false)
            {
                pairs.Add(new KeyValuePair<string, string>(key, value));
            }

            query = string.Join("&", pairs.Select(temp => temp.Key + "=" + temp.Value));

            return new UriBuilder(uri) { Query = query }.Uri;
        }

        internal static Uri AddHeader(Uri uri, IEnumerable<KeyValuePair<string, string>> parameter)
        {
            foreach (var nameValue in parameter)
            {
                uri = AddHeader(uri, nameValue.Key, nameValue.Value);
            }
            return uri;
        }

        internal static string GetQueryParameter(this Uri uri, string key)
        {
            string query = uri.Query;
            if (query.Length <= 1)
            {
                return null;
            }
            WwwFormUrlDecoder decoder = new WwwFormUrlDecoder(query);
            return decoder.GetFirstValueByName(key);
        }

        internal static string GetDisplay()
        {
            ResourceContext resContext = ResourceContext.GetForCurrentView();
            string deviceFamily = resContext.QualifierValues["DeviceFamily"];
            string display = "client";
            switch(deviceFamily)
            {
                case "Desktop":
                    display = "client";
                    break;
                case "Mobile":
                    display = "mobile";
                    break;
                default:
                    break;
            }
            return display;
        }

        internal static async Task<T> ReadAsJsonAsync<T>(this IHttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

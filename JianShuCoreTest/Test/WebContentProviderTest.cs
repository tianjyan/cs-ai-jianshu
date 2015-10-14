using JianShuCore.Model;
using JianShuCore.Provider;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JianShuCoreTest.Test
{
    [TestClass]
    public class WebContentProviderTest
    {
        [TestMethod]
        public async Task LoginTest()
        {   
            LoginResult result = await LoginResult(Config.username, Config.password);
            LoginResult wrong = await LoginResult(Config.username, string.Empty);

            Assert.AreEqual(result.Error, null);
            Assert.AreEqual(wrong.Error.Count, 1);
        }

        [TestMethod]
        public async Task TrendingTest()
        {
            List<TrendingResult> daily = await TrendingResult(nameof(daily), 20, 20);
            List<TrendingResult> weekly = await TrendingResult(nameof(weekly), 2, 10);
            List<TrendingResult> monthly = await TrendingResult(nameof(monthly), 3, 30);
            List<TrendingResult> it = await TrendingResult("now", 0, 20);


            Assert.AreEqual(daily.Count, 20);
            Assert.AreEqual(weekly.Count, 10);
            Assert.AreEqual(monthly.Count, 30);
        }

        [TestMethod]
        public async Task NoteDetailTest()
        {
            NoteDetailResult result = await NoteDetailResult("7a490fc8a288", null, null);
            Assert.IsNotNull(result);
        }

        public async Task<LoginResult> LoginResult(string username, string password)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string content = string.Format("email={0}&password={1}", Uri.EscapeDataString(username), Uri.EscapeDataString(password));
            return await web.HttpPostRequest<LoginResult>(Config.JianShuLogin, content, web.GetHeaders(null, null));
        }

        public async Task<List<TrendingResult>> TrendingResult(string frequency, int index, int count)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string text = string.Format(Config.Trending, frequency, index, count);
            return await web.HttpGetRequest<List<TrendingResult>>(text, web.GetHeaders(null, null));
        }

        public async Task<NoteDetailResult> NoteDetailResult(string noteId, string user_id, string mobile_token)
        {
            WebContentProvider web = WebContentProvider.GetInstance();
            string text = string.Format(Config.NoteDetail, noteId);
            return await web.HttpGetRequest<NoteDetailResult>(text, web.GetHeaders(null, null));
        }
    }
}

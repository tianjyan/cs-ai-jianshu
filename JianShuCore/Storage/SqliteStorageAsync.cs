using JianShuCore.Common;
using JianShuCore.Interface;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using static JianShuCore.Common.Untils;

namespace JianShuCore.Storage
{
    public class SqliteStorageAsync : IStorageAsync
    {
        #region Private Field
        private const string DBName = "JianShuCahce.db";
        private const string TableSQL = @"
                CREATE TABLE IF NOT EXISTS Cache(
                    ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    Type TEXT NOT NULL,
                    TypeID TEXT NOT NULL,
                    TimeStamp TEXT,
                    Content TEXT
                    );";
        private const string InsertSQL = @"
                INSERT INTO Cache (Type, TypeID, TimeStamp, Content) 
                    VALUES ('{0}', '{1}', '{2}', '{3}');";
        private const string SelectSQL = @"
                SELECT * FROM Cache
                    WHERE Type = '{0}' AND TypeID = '{1}';";
        private const string DeleteSQL = @"
                DELETE FROM Cache WHERE Type = '{0}' AND TypeID = '{1}';";
        private const string UpdateSQL = @"
                UPDATE Cache  SET TimeStamp = '{2}', Content = '{3}' 
                WHERE Type = '{0}' AND TypeID = '{1}';";
        private const string  CountSQL = @"
                SELECT COUNT(*) FROM Cache
                    WHERE Type = '{0}' AND TypeID = '{1}';";
        private const string SelectCollectionSQL = @"
                SELECT * FROM Cache WHERE Type = '{0}';";
        private const string TableDelete = @"DROP TABLE IF EXISTS Cache;";

        private readonly SQLiteConnectionString connectionString;
        #endregion

        #region Constructor
        public SqliteStorageAsync()
        {
            connectionString = new SQLiteConnectionString(
                Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName), false);
            InitDatabaseAsync();

        }
        #endregion

        #region Public Function
        /// <summary>
        /// Add item to SQLite
        /// </summary>
        /// <param name="key">
        /// Should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <param name="value">Value you want to save</param>
        /// <returns></returns>
        public async Task AddItemAsync(string key, object value)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnectionWithLock conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString))
            {
                SQLiteAsyncConnection c = new SQLiteAsyncConnection(() => conn);
                await c.ExecuteAsync(string.Format(InsertSQL,
                values.Item1, values.Item2, values.Item3, Serialize(value)));
            }
        }

        /// <summary>
        /// Update item in SQLite
        /// </summary>
        /// <param name="key">
        /// Should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <param name="value">Value you want to save</param>
        /// <returns></returns>
        public async Task UpdateItemAsync(string key, object value)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnectionWithLock conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString))
            {
                SQLiteAsyncConnection c = new SQLiteAsyncConnection(() => conn);
                await c.ExecuteAsync(string.Format(UpdateSQL,
                values.Item1, values.Item2, values.Item3, Serialize(value)));
            }
        }

        /// <summary>
        /// Judge whether SQLite has this item through the key given
        /// </summary>
        /// <param name="key">
        /// Should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <returns>true for yes and false for no</returns>
        public async Task<bool> ContainItemAsync(string key)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnectionWithLock conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString))
            {
                SQLiteAsyncConnection c = new SQLiteAsyncConnection(() => conn);
                return await c.ExecuteScalarAsync<int>(
                    string.Format(CountSQL, values.Item1, values.Item2)) > 0 ? true : false;
            }
        }

        /// <summary>
        /// Clear data in SQLite
        /// </summary>
        public async Task FlushAllAsync()
        {
            using (SQLiteConnectionWithLock conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString))
            {
                SQLiteAsyncConnection c = new SQLiteAsyncConnection(() => conn);
                await c.ExecuteAsync(TableDelete);
            }
        }

        /// <summary>
        /// Get item from SQLite
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">
        /// Should be the format of "Type/TypeID"
        /// Eg:If you have a type of "Person" and the id is 123
        /// so the format will like "Person/123"
        /// </param>
        /// <returns>Value that you saved</returns>
        public async Task<T> GetItemAsync<T>(string key) where T : class
        {
            List<CacheModel> models = await GetCacheItems(key, false);
            if (models.Count() > 0)
            {
                return Deserialize<T>(models[0].Content);
            }
            return null;
        }

        /// <summary>
        /// Get items from SQLite
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">
        /// Should be the format of "Type/0"
        /// Eg:If you have a type of "Person"
        /// so the format will like "Person/0"
        /// </param>
        /// <returns></returns>
        public async Task<List<T>> GetItemsAsync<T>(string key) where T :class
        {
            List<CacheModel> models = await GetCacheItems(key, true);
            return new List<T>(from i in models select Deserialize<T>(i.Content));
        }

        /// <summary>
        /// Remove item in SQLite
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Value that you saved</returns>
        public async Task<bool> RemoveItemAsync(string key)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnectionWithLock conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString))
            {
                SQLiteAsyncConnection c = new SQLiteAsyncConnection(() => conn);
                await c.ExecuteAsync(string.Format(DeleteSQL, values.Item1, values.Item2));
            }
            return true;
        }

        /// <summary>
        /// Get the size of local file
        /// </summary>
        /// <returns></returns>
        public long GetSize()
        {
            FileInfo info = new FileInfo(Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName));
            return info.Length;
        }
        #endregion

        #region Private Function
        //Init the Database and create the table
        private async void InitDatabaseAsync()
        {
            //The SQLiteAsyncConnection class now takes a Func in the constructor instead of a path.
            //This is done because the async classes are now just meant 
            //to be wrappers around the normal sqlite connection.
            //To use SQLiteAsyncConnection just create
            //an instance of a SQLiteConnectionWithLock and pass in that through a func, 
            //e.g.: new SQLiteAsyncConnection(() => _sqliteConnectionWithLock);
            //Please be aware that the Task.
            //Run pattern used in SQLiteAsyncConnection can be considered an anti-pattern 
            //(libraries should not provide async methods unless they are truly async). 
            //This class is maintained for backwards compatability 
            //and for use-cases where async-isolation is handy.
            using (SQLiteConnectionWithLock conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString))
            {
                SQLiteAsyncConnection c = new SQLiteAsyncConnection(() => conn);
                await c.ExecuteAsync(TableSQL);
            }
        }

        private Tuple<string,long,string> UnpackKey(string key)
        {
            string[] values = key.Trim('/').Split('/');
            if (values.Count() == 2)
            {
                string Type = values[0];
                string TypeId = values[1];
                long ParseId = 0;
                if (!string.IsNullOrEmpty(Type) && long.TryParse(TypeId, out ParseId))
                {
                    Tuple<string, long, string> result = new Tuple<string, long, string>(Type, ParseId, DateTime.Now.ToString("yyyy-MM-dd"));
                    return result;
                }
            }
            throw new ArgumentException("Invalid object: " + key);
        }

        private async Task<List<CacheModel>> GetCacheItems(string key , bool selectCollection)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnectionWithLock conn = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString))
            {
                SQLiteAsyncConnection c = new SQLiteAsyncConnection(() => conn);
                return await c.QueryAsync<CacheModel>(string.Format(selectCollection ? SelectCollectionSQL : SelectSQL, values.Item1, values.Item2)) ?? new List<CacheModel>();
            }
        }
        #endregion
    }
}
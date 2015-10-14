using JianShuCore.Common;
using JianShuCore.Interface;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using static JianShuCore.Common.Untils;

namespace JianShuCore.Storage
{
    public class SqliteStorage : IStorage
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
        private const string CountSQL = @"
                SELECT COUNT(*) FROM Cache
                    WHERE Type = '{0}' AND TypeID = '{1}';";

        private const string SelectCollectionSQL = @"
                SELECT * FROM Cache WHERE Type = '{0}';";
        private const string TableDelete = @"DROP TABLE IF EXISTS Cache;";

        private readonly string filePath;
        #endregion

        #region Constructor
        public SqliteStorage()
        {
            filePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBName);
            InitDatabase();
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
        public void AddItem(string key, object value)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), filePath))
            {
                conn.Execute(string.Format(InsertSQL,
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
        public bool ContainItem(string key)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), filePath))
            {
                return conn.ExecuteScalar<int>(
                    string.Format(CountSQL, values.Item1, values.Item2)) > 0 ? true : false;
            }
        }

        /// <summary>
        /// Clear data in SQLite
        /// </summary>
        public void FlushAll()
        {
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), filePath))
            {
                conn.Execute(TableDelete);
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
        public T GetItem<T>(string key) where T : class
        {
            List<CacheModel> models = GetCacheItems(key, false);
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
        public List<T> GetItems<T>(string key) where T : class
        {
            List<CacheModel> models = GetCacheItems(key, true);
            return new List<T>(from i in models select Deserialize<T>(i.Content));
        }

        /// <summary>
        /// Remove item in SQLite
        /// </summary>
        /// <typeparam name="T">Type that you saved</typeparam>
        /// <param name="key">Key</param>
        /// <returns>Value that you saved</returns>
        public bool RemoveItem(string key)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), filePath))
            {
                conn.Execute(string.Format(DeleteSQL, values.Item1, values.Item2));
            }
            return true;
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
        public void UpdateItem(string key, object value)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), filePath))
            {
                conn.Execute(string.Format(UpdateSQL,
                values.Item1, values.Item2, values.Item3, Serialize(value)));
            }
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
        private void InitDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), filePath))
            {
                conn.Execute(TableSQL);
            }
        }

        private Tuple<string, long, string> UnpackKey(string key)
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

        private List<CacheModel> GetCacheItems(string key, bool isList)
        {
            Tuple<string, long, string> values = UnpackKey(key);
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), filePath))
            {
                return conn.Query<CacheModel>(string.Format(isList? SelectCollectionSQL : SelectSQL, values.Item1, values.Item2)) ?? new List<CacheModel>();
            }
        }
        #endregion
    }
}

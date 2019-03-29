using System;
using System.Data.SQLite;
using System.IO;

namespace SatisfactoryLinePlanner.Database
{
    /// <summary>
    /// データベースの初期処理をするやつ（予定）
    /// </summary>
    class DBOperation : Database
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBOperation()
        {
            // データベースファイルがなかったらつくる
            if (!File.Exists(RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                try
                {
                    SQLiteConnection.CreateFile(RoamingPath + AppDirectory + DBDirectory + DBFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown when creating Database file.");
                    Console.WriteLine("Exception Message:" + e.Message);
                }
            }
        }
    }
}

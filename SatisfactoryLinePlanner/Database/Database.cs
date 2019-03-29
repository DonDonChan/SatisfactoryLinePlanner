using System;
using System.IO;

namespace SatisfactoryLinePlanner.Database
{
    /// <summary>
    /// データベースのあれこれを保持するクラス（予定）
    /// 
    /// ToDO
    ///     ・テーブル構成情報をクラス化
    /// 
    /// </summary>
    class Database : ApplicationInfo
    {
        private string dbDirectory = @"/Database";
        private string dbFileName = @"/SatisfactoryData.db";

        public string DBDirectory { get { return dbDirectory; } }
        public string DBFileName { get { return dbFileName; } }

        public Database()
        {
            // データベースフォルダのチェック
            if (!Directory.Exists(RoamingPath + AppDirectory + DBDirectory))
            {
                try
                {
                    Directory.CreateDirectory(RoamingPath + AppDirectory + DBDirectory);
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown when creating Database directory.");
                    Console.WriteLine("Exception Message:" + e.Message);
                }
            }
        }
    }
}

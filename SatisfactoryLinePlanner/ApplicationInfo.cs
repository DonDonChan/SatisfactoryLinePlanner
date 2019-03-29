using System;
using System.IO;

namespace SatisfactoryLinePlanner
{
    /// <summary>
    /// アプリケーションのあれこれを保持するクラス（予定）
    /// 
    /// ToDo
    ///     ・コンフィグファイルとか
    ///     ・構築したラインを保存したりとか
    /// 
    /// </summary>
    class ApplicationInfo
    {
        private string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);  // AppData/Roaming の場所
        private string appDirectory = @"/SatisfactoryLinePlannner";                                         // アプリケーション情報を保存する場所

        /// <summary>
        /// AppData/Roaming の場所
        /// </summary>
        public string RoamingPath { get { return roamingPath; } }

        /// <summary>
        /// アプリケーション情報を保存する場所
        /// </summary>
        public string AppDirectory { get { return appDirectory; } }

        // コンストラクタ
        public ApplicationInfo()
        {
            // アプリケーションフォルダのチェック
            if (!Directory.Exists(RoamingPath + AppDirectory))
            {
                try
                {
                    Directory.CreateDirectory(RoamingPath + AppDirectory);
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception was thrown when creating application directory.");
                    Console.WriteLine("Exception Message:" + e.Message);
                }
            }
        }
    }
}

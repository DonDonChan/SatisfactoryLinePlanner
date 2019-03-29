using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryLinePlanner.Database
{
    class DBDrop : DBOperation
    {
        /// <summary>
        /// テーブル削除（あるやつだけ）
        /// </summary>
        /// <param name="table">削除するテーブル名</param>
        public void DropTable(string table)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = "DROP TABLE IF EXISTS " + table;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception was thrown when table dropping. Table name is " + table);
                        Console.WriteLine("Exception Message:" + e.Message);
                    }
                }
            }
        }
    }
}

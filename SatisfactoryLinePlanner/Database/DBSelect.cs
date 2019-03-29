using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SatisfactoryLinePlanner.Database
{
    class DBSelect : DBOperation
    {
        public void TestSelect(string table)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = "SELECT * FROM " + table;

                        command.Prepare();

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine( reader[3].ToString());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception was thrown when selecting data.");
                        Console.WriteLine("Exception Message:" + e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// データ検索するやつ
        /// </summary>
        /// <param name="table">取り出す表 絶対いれて</param>
        /// <param name="join">表結合 なくてもいい</param>
        /// <param name="columns">取り出す列名 絶対いれて</param>
        /// <param name="where">条件式 なくてもいい</param>
        /// <param name="whereParams">条件の値 上を入れたら絶対いれて</param>
        /// <param name="order">順序指定 なくてもいい</param>
        /// <returns>取り出したやつ</returns>
        public List<String[]> SelectData(string table, string join, string[] columns, string where, List<string[]> whereParams, string order)
        {
            List<string[]> resultList = new List<string[]>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    try
                    {
                        // SQL文の用意
                        command.CommandText = "SELECT * FROM " + table + " " + join;

                        // Whereがあれば追加
                        if (where != null)
                        {
                            command.CommandText += " WHERE " + where;

                            foreach (string[] whereParam in whereParams)
                            {
                                command.Parameters.Add(new SQLiteParameter(whereParam[0], whereParam[1]));
                            }
                        }

                        // Orderがあれば追加、なければ1列目を昇順で
                        if (order != null)
                        {
                            command.CommandText += " ORDER BY " + order;
                        }
                        else
                        {
                            command.CommandText += " ORDER BY 1 ASC ";
                        }
                        
                        // よくわからない
                        command.Prepare();

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // 結果を1行ずつリストに保存
                            while (reader.Read())
                            {
                                string[] columnData = new string[columns.Length];
                                int i = 0;
                                foreach (string column in columns)
                                {
                                    columnData[i] = reader[column].ToString();
                                    i++;
                                }
                                resultList.Add(columnData);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception was thrown when selecting data.");
                        Console.WriteLine("SQL:" + command.CommandText);
                        Console.WriteLine("Exception Message:" + e.Message);
                    }
                }
            }

            return resultList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace SatisfactoryLinePlanner
{
    class DatabaseOperation
    {
        private string dbFilePath = "SatisfactoryData.db";

        public DatabaseOperation()
        {
            //InitializeDB();
        }

        private void InitializeDB()
        {
            DropTable();
            CreateTable();
            InsertDataFromFile();
        }

        /// <summary>
        /// 全テーブル作成（ないやつだけ）
        /// </summary>
        private void CreateTable()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                using (SQLiteCommand command = con.CreateCommand())
                {
                    try
                    {
                        command.CommandText = "CREATE TABLE IF NOT EXISTS FacilityType ( FacilityTypeId VARCHAR(50) NOT NULL, FacilityType VARCHAR(50) NOT NULL, PRIMARY KEY (FacilityTypeId) )";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE IF NOT EXISTS Facility ( FacilityId VARCHAR(50) NOT NULL, FacilityName VARCHAR(50) NOT NULL, RequiredPower INT NOT NULL, Efficiency INT NOT NULL, FacilityTypeId VARCHAR(50) NOT NULL, PRIMARY KEY (FacilityId), FOREIGN KEY(FacilityTypeId) REFERENCES FacilityType(FacilityTypeId) )";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE IF NOT EXISTS Generator ( GeneratorId VARCHAR(50) NOT NULL, GeneratorName VARCHAR(50) NOT NULL, GenaratingCapacity INT NOT NULL, PRIMARY KEY (GeneratorId) )";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE IF NOT EXISTS Material ( MaterialId VARCHAR(50) NOT NULL, MaterialName VARCHAR(50) NOT NULL, IsOre INT DEFAULT(0) NOT NULL, PRIMARY KEY (MaterialId) )";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE IF NOT EXISTS MaterialRecipe ( RecipeId VARCHAR(50) NOT NULL, RecipeName VARCHAR(50) NOT NULL, MaterialId VARCHAR(50) NOT NULL, RequiredFacilityType VARCHAR(50) NOT NULL, UnitProductionTime INT NOT NULL, UnitProductionValue INT NOT NULL, PRIMARY KEY (RecipeId), FOREIGN KEY(MaterialId) REFERENCES Material(MaterialId), FOREIGN KEY(RequiredFacilityType) REFERENCES FacilityType(FacilityTypeId) )";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE IF NOT EXISTS RequiredMaterialOfUnitProduction ( RecipeRequiredMaterialId INTEGER NOT NULL, RecipeId VARCHAR(50) NOT NULL, RequiredMaterialId VARCHAR(50) NOT NULL, RequiredMaterialValue VARCHAR(50) NOT NULL, PRIMARY KEY (RecipeRequiredMaterialId), FOREIGN KEY(RecipeId) REFERENCES MaterialRecipe(RecipeId), FOREIGN KEY(RequiredMaterialId) REFERENCES Material(MaterialId) )";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
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
        /// <param name="order">順序指定 絶対いれて</param>
        /// <returns>取り出したやつ</returns>
        public List<String[]> SelectData(string table, string join, string[] columns, string where, List<string[]> whereParams, string order)
        {
            List<string[]> list = new List<string[]>();

            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                using (SQLiteCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM " + table + " " + join;

                    if (where != null)
                    {
                        command.CommandText += " WHERE " + where;

                        foreach (string[] whereParam in whereParams)
                        {
                            command.Parameters.Add(new SQLiteParameter(whereParam[0], whereParam[1]));
                        }
                    }

                    command.CommandText += " ORDER BY " + order;

                    command.Prepare();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string[] columnData = new string[columns.Length];
                            int i = 0;
                            foreach (string column in columns)
                            {
                                columnData[i] = reader[column].ToString();
                                i++;
                            }
                            list.Add(columnData);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 検査用全件検索
        /// </summary>
        /// <param name="table"></param>
        private void AllShow(string table)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                using (SQLiteCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM " + table;

                    command.Prepare();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("ColumnData1:" + reader[0]);
                            Console.WriteLine("ColumnData2:" + reader[1]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// FacilityTypeテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">データ</param>
        public void InsertToFacilityTypeTable(List<string[]> data)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                foreach (string[] datam in data)
                {
                    using (var transaction = con.BeginTransaction())
                    {
                        using (SQLiteCommand command = con.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "INSERT INTO FacilityType (FacilityTypeId, FacilityType) VALUES (@FACILITYTYPEID, @FACILITYTYPENAME)";
                                command.Parameters.Add(new SQLiteParameter("@FACILITYTYPEID", datam[0]));
                                command.Parameters.Add(new SQLiteParameter("@FACILITYTYPENAME", datam[1]));
                                command.ExecuteNonQuery();

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Facilityテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">データ</param>
        public void InsertToFacilityTable(List<string[]> data)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                foreach (string[] datam in data)
                {
                    using (var transaction = con.BeginTransaction())
                    {
                        using (SQLiteCommand command = con.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "INSERT INTO Facility (FacilityId, FacilityName, RequiredPower, Efficiency, FacilityTypeId) VALUES (@FACILITYID, @FACILITYNAME, @REQUIREDPOWER, @EFFICIENCY, @FACILITYTYPEID)";
                                command.Parameters.Add(new SQLiteParameter("@FACILITYID", datam[0]));
                                command.Parameters.Add(new SQLiteParameter("@FACILITYNAME", datam[1]));
                                command.Parameters.Add(new SQLiteParameter("@REQUIREDPOWER", datam[2]));
                                command.Parameters.Add(new SQLiteParameter("@EFFICIENCY", datam[3]));
                                command.Parameters.Add(new SQLiteParameter("@FACILITYTYPEID", datam[4]));
                                command.ExecuteNonQuery();

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generatorテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">データ</param>
        public void InsertToGeneratorTable(List<string[]> data)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                foreach (string[] datam in data)
                {
                    using (var transaction = con.BeginTransaction())
                    {
                        using (SQLiteCommand command = con.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "INSERT INTO Generator (GeneratorId, GeneratorName, GenaratingCapacity) VALUES (@GENERATORID, @GENERATORNAME, @GENARATINGCAPACITY)";
                                command.Parameters.Add(new SQLiteParameter("@GENERATORID", datam[0]));
                                command.Parameters.Add(new SQLiteParameter("@GENERATORNAME", datam[1]));
                                command.Parameters.Add(new SQLiteParameter("@GENARATINGCAPACITY", datam[2]));
                                command.ExecuteNonQuery();

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Materialテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">データ</param>
        public void InsertToMaterialTable(List<string[]> data)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                foreach (string[] datam in data)
                {
                    using (var transaction = con.BeginTransaction())
                    {
                        using (SQLiteCommand command = con.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "INSERT INTO Material (MaterialId, MaterialName, IsOre) VALUES (@MATERIALID, @MATERIALNAME, @ISORE)";
                                command.Parameters.Add(new SQLiteParameter("@MATERIALID", datam[0]));
                                command.Parameters.Add(new SQLiteParameter("@MATERIALNAME", datam[1]));
                                command.Parameters.Add(new SQLiteParameter("@ISORE", datam[2]));
                                command.ExecuteNonQuery();

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// MaterialRecipeテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">データ</param>
        public void InsertToMaterialRecipeTable(List<string[]> data)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                foreach (string[] datam in data)
                {
                    using (var transaction = con.BeginTransaction())
                    {
                        using (SQLiteCommand command = con.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "INSERT INTO MaterialRecipe (RecipeId, RecipeName, MaterialId, RequiredFacilityType, UnitProductionTime, UnitProductionValue) VALUES (@RECIPEID, @RECIPENAME, @MATERIALID, @REQUIREDFACILITYTYPE, @UNITPRODUCTIONTIME, @UNITPRODUCTIONVALUE)";
                                command.Parameters.Add(new SQLiteParameter("@RECIPEID", datam[0]));
                                command.Parameters.Add(new SQLiteParameter("@RECIPENAME", datam[1]));
                                command.Parameters.Add(new SQLiteParameter("@MATERIALID", datam[2]));
                                command.Parameters.Add(new SQLiteParameter("@REQUIREDFACILITYTYPE", datam[3]));
                                command.Parameters.Add(new SQLiteParameter("@UNITPRODUCTIONTIME", datam[4]));
                                command.Parameters.Add(new SQLiteParameter("@UNITPRODUCTIONVALUE", datam[5]));
                                command.ExecuteNonQuery();

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// RequiredMaterialOfUnitProductionテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">データ</param>
        public void InsertToRequiredMaterialOfUnitProductionTable(List<string[]> data)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                foreach (string[] datam in data)
                {
                    using (var transaction = con.BeginTransaction())
                    {
                        using (SQLiteCommand command = con.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "INSERT INTO RequiredMaterialOfUnitProduction (RecipeId, RequiredMaterialId, RequiredMaterialValue) VALUES (@RECIPEID, @REQUIREDMATERIALID, @REQUIREDMATERIALVALUE)";
                                command.Parameters.Add(new SQLiteParameter("@RECIPEID", datam[0]));
                                command.Parameters.Add(new SQLiteParameter("@REQUIREDMATERIALID", datam[1]));
                                command.Parameters.Add(new SQLiteParameter("@REQUIREDMATERIALVALUE", datam[2]));
                                command.ExecuteNonQuery();

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ファイルからデータを挿入
        /// </summary>
        public void InsertDataFromFile()
        {
            this.InsertToFacilityTypeTable(this.GetDataFromFile("FacilityType"));
            this.InsertToFacilityTable(this.GetDataFromFile("Facility"));
            this.InsertToGeneratorTable(this.GetDataFromFile("Generator"));
            this.InsertToMaterialTable(this.GetDataFromFile("MaterialData"));
            this.InsertToMaterialRecipeTable(this.GetDataFromFile("MaterialRecipe"));
            this.InsertToRequiredMaterialOfUnitProductionTable(this.GetDataFromFile("RequiredMaterialOfUnitProduction"));
        }

        /// <summary>
        /// 全テーブル削除（あるやつだけ）
        /// </summary>
        public void DropTable()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbFilePath))
            {
                con.Open();
                using (SQLiteCommand command = con.CreateCommand())
                {
                    try
                    {
                        command.CommandText = "DROP TABLE IF EXISTS RequiredMaterialOfUnitProduction";
                        command.ExecuteNonQuery();

                        command.CommandText = "DROP TABLE IF EXISTS MaterialRecipe";
                        command.ExecuteNonQuery();

                        command.CommandText = "DROP TABLE IF EXISTS Material";
                        command.ExecuteNonQuery();

                        command.CommandText = "DROP TABLE IF EXISTS Generator";
                        command.ExecuteNonQuery();

                        command.CommandText = "DROP TABLE IF EXISTS Facility";
                        command.ExecuteNonQuery();

                        command.CommandText = "DROP TABLE IF EXISTS FacilityType";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// テキストファイルからデータを取り出す
        /// </summary>
        /// <param name="fileName">ファイルの名前（拡張子抜き）</param>
        /// <returns>データのリスト</returns>
        private List<String[]> GetDataFromFile(string fileName)
        {
            List<String[]> list = new List<string[]>();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SatisfactoryLinePlanner.Resources." + fileName + ".txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    try
                    {
                        using (var reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            while (reader.Peek() >= 0)
                            {
                                string dataLine = reader.ReadLine();
                                string[] splitted = dataLine.Split('\t');
                                list.Add(splitted);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            return list;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryLinePlanner.Database
{
    /// <summary>
    /// データベースを初期化するやつ
    /// </summary>
    class DBInitializer : DBOperation
    {
        /// <summary>
        /// データベースを初期化する
        /// </summary>
        public void Initialize()
        {
            DropAllTable();
            CreateTable();
        }

        /// <summary>
        /// データベースを初期化してデータをいれる
        /// </summary>
        public void InitializeAndInsert()
        {
            Initialize();

            InsertDataFromFile();
        }

        /// <summary>
        /// 全テーブル削除
        /// </summary>
        private void DropAllTable()
        {
            DBDrop dBDrop = new DBDrop();

            string[] tables = { "RequiredMaterials", "MaterialRecipes", "Materials", "Generators", "ProductionBuildings", "Buildings" };
            foreach (string table in tables)
            {
                dBDrop.DropTable(table);
            }
        }

        /// <summary>
        /// 全テーブル作成（ないやつだけ）
        /// </summary>
        private void CreateTable()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                con.Open();
                using (SQLiteCommand command = con.CreateCommand())
                {
                    try
                    {
                        string createBuildingsSQL = @"
                            CREATE TABLE IF NOT EXISTS Buildings (
                                TypeId VARCHAR(50) NOT NULL,
                                Type VARCHAR(50) NOT NULL,
                            PRIMARY KEY (TypeId)
                        )";

                        string createProductionBuildingsSQL = @"
                            CREATE TABLE IF NOT EXISTS ProductionBuildings (
                                BuildingId VARCHAR(50) NOT NULL,
                                Name VARCHAR(50) NOT NULL,
                                RequiredPower INT NOT NULL,
                                Efficiency INT NOT NULL,
                                TypeId VARCHAR(50) NOT NULL,
                            PRIMARY KEY (BuildingId),
                            FOREIGN KEY(TypeId) REFERENCES Buildings(TypeId) 
                        )";

                        string createGeneratorsSQL = @"
                            CREATE TABLE IF NOT EXISTS Generators(
                                GeneratorId VARCHAR(50) NOT NULL,
                                Name VARCHAR(50) NOT NULL,
                                Capacity INT NOT NULL,
                                TypeId VARCHAR(50) NOT NULL,
                            PRIMARY KEY(GeneratorId),
                            FOREIGN KEY(TypeId) REFERENCES Buildings(TypeId)
                        )";

                        string createMaterialsSQL = @"
                            CREATE TABLE IF NOT EXISTS Materials (
                                MaterialId VARCHAR(50) NOT NULL,
                                Name VARCHAR(50) NOT NULL,
                                IsOre INT DEFAULT(0) NOT NULL,
                            PRIMARY KEY (MaterialId)
                        )";

                        string createMaterialsRecipeSQL = @"
                            CREATE TABLE IF NOT EXISTS MaterialRecipes (
                                RecipeId VARCHAR(50) NOT NULL,
                                Name VARCHAR(50) NOT NULL,
                                MaterialId VARCHAR(50) NOT NULL,
                                RequiredBuildingTypeId VARCHAR(50) NOT NULL,
                                UnitProductionTime INT NOT NULL,
                                UnitProductionPieces INT NOT NULL,
                            PRIMARY KEY (RecipeId),
                            FOREIGN KEY(MaterialId) REFERENCES Materials(MaterialId),
                            FOREIGN KEY(RequiredBuildingTypeId) REFERENCES Buildings(BuildingTypeId)
                        )";

                        string createRequiredMaterials = @"
                            CREATE TABLE IF NOT EXISTS RequiredMaterials (
                                Id INTEGER NOT NULL,
                                RecipeId VARCHAR(50) NOT NULL,
                                RequiredMaterialId VARCHAR(50) NOT NULL,
                                NumberOfRequiredMaterial VARCHAR(50) NOT NULL,
                            PRIMARY KEY (Id),
                            FOREIGN KEY(RecipeId) REFERENCES MaterialRecipe(RecipeId),
                            FOREIGN KEY(RequiredMaterialId) REFERENCES Materials(MaterialId)
                        )";

                        command.CommandText = createBuildingsSQL;
                        command.ExecuteNonQuery();

                        command.CommandText = createProductionBuildingsSQL;
                        command.ExecuteNonQuery();

                        command.CommandText = createGeneratorsSQL;
                        command.ExecuteNonQuery();

                        command.CommandText = createMaterialsSQL;
                        command.ExecuteNonQuery();

                        command.CommandText = createMaterialsRecipeSQL;
                        command.ExecuteNonQuery();

                        command.CommandText = createRequiredMaterials;
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
        /// ファイルからデータを挿入
        /// </summary>
        public void InsertDataFromFile()
        {
            DBInsert dBInsert = new DBInsert();

            dBInsert.InsertToBuildingsTable(GetDataFromFile("Buildings"));
            dBInsert.InsertToProductionBuildingsTable(GetDataFromFile("ProductionBuildings"));
            dBInsert.InsertToGeneratorsTable(GetDataFromFile("Generators"));
            dBInsert.InsertToMaterialsTable(GetDataFromFile("Materials"));
            dBInsert.InsertToMaterialRecipesTable(GetDataFromFile("MaterialRecipes"));
            dBInsert.InsertToRequiredMaterialsTable(GetDataFromFile("RequiredMaterials"));
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
            var resourceName = "SatisfactoryLinePlanner.Resources.DataTextFile." + fileName + ".txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    try
                    {
                        using (var reader = new StreamReader(stream, Encoding.GetEncoding("shift_jis")))
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

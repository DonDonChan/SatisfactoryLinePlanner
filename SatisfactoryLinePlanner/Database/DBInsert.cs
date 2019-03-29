using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SatisfactoryLinePlanner.Database
{
    class DBInsert : DBOperation
    {
        /// <summary>
        /// Buildingsテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">string TypeId, string Type</param>
        public void InsertToBuildingsTable(List<string[]> data)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                foreach (string[] datam in data)
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "INSERT INTO Buildings (TypeId, Type) VALUES (@TYPEID, @TYPE)";
                            command.Parameters.Add(new SQLiteParameter("@TYPEID", datam[0]));
                            command.Parameters.Add(new SQLiteParameter("@TYPE", datam[1]));
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An exception was thrown when inserting to Buildings Table.");
                            Console.WriteLine("Exception Message:" + e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ProductionBuildingsテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">BuildingId, Name, RequiredPower, Efficiency, TypeId</param>
        public void InsertToProductionBuildingsTable(List<string[]> data)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                foreach (string[] datam in data)
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "INSERT INTO ProductionBuildings (BuildingId, Name, RequiredPower, Efficiency, TypeId) VALUES (@BUILDINGID, @NAME, @REQUIREDPOWER, @EFFICIENCY, @TYPEID)";
                            command.Parameters.Add(new SQLiteParameter("@BUILDINGID", datam[0]));
                            command.Parameters.Add(new SQLiteParameter("@NAME", datam[1]));
                            command.Parameters.Add(new SQLiteParameter("@REQUIREDPOWER", datam[2]));
                            command.Parameters.Add(new SQLiteParameter("@EFFICIENCY", datam[3]));
                            command.Parameters.Add(new SQLiteParameter("@TYPEID", datam[4]));
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An exception was thrown when inserting to Production Buildings Table.");
                            Console.WriteLine("Exception Message:" + e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generatorsテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">GeneratorId, Name, Capacity, TypeId</param>
        public void InsertToGeneratorsTable(List<string[]> data)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                foreach (string[] datam in data)
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "INSERT INTO Generators (GeneratorId, Name, Capacity, TypeId) VALUES (@GENERATORID, @NAME, @CAPACITY, @TYPEID)";
                            command.Parameters.Add(new SQLiteParameter("@GENERATORID", datam[0]));
                            command.Parameters.Add(new SQLiteParameter("@NAME", datam[1]));
                            command.Parameters.Add(new SQLiteParameter("@CAPACITY", datam[2]));
                            command.Parameters.Add(new SQLiteParameter("@TYPEID", datam[3]));
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An exception was thrown when inserting to Generators Table.");
                            Console.WriteLine("Exception Message:" + e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Materialsテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">MaterialId, Name, IsOre</param>
        public void InsertToMaterialsTable(List<string[]> data)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                foreach (string[] datam in data)
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "INSERT INTO Materials (MaterialId, Name, IsOre) VALUES (@MATERIALID, @NAME, @ISORE)";
                            command.Parameters.Add(new SQLiteParameter("@MATERIALID", datam[0]));
                            command.Parameters.Add(new SQLiteParameter("@NAME", datam[1]));
                            command.Parameters.Add(new SQLiteParameter("@ISORE", datam[2]));
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An exception was thrown when inserting to Materials Table.");
                            Console.WriteLine("Exception Message:" + e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// MaterialRecipesテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">RecipeId, Name, MaterialId, RequiredBuildingTypeId, UnitProductionTime, UnitProductionPieces</param>
        public void InsertToMaterialRecipesTable(List<string[]> data)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                foreach (string[] datam in data)
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "INSERT INTO MaterialRecipes (RecipeId, Name, MaterialId, RequiredBuildingTypeId, UnitProductionTime, UnitProductionPieces) VALUES (@RECIPEID, @NAME, @MATERIALID, @REQUIREDFACILITYTYPEID, @UNITPRODUCTIONTIME, @UNITPRODUCTIONPIECES)";
                            command.Parameters.Add(new SQLiteParameter("@RECIPEID", datam[0]));
                            command.Parameters.Add(new SQLiteParameter("@NAME", datam[1]));
                            command.Parameters.Add(new SQLiteParameter("@MATERIALID", datam[2]));
                            command.Parameters.Add(new SQLiteParameter("@REQUIREDFACILITYTYPEID", datam[3]));
                            command.Parameters.Add(new SQLiteParameter("@UNITPRODUCTIONTIME", datam[4]));
                            command.Parameters.Add(new SQLiteParameter("@UNITPRODUCTIONPIECES", datam[5]));
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An exception was thrown when inserting to Material Recipes Table.");
                            Console.WriteLine("Exception Message:" + e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// RequiredMaterialsテーブルに挿入するやつ
        /// </summary>
        /// <param name="data">RecipeId, RequiredMaterialId, NumberOfRequiredMaterial</param>
        public void InsertToRequiredMaterialsTable(List<string[]> data)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + RoamingPath + AppDirectory + DBDirectory + DBFileName))
            {
                connection.Open();
                foreach (string[] datam in data)
                {
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "INSERT INTO RequiredMaterials (RecipeId, RequiredMaterialId, NumberOfRequiredMaterial) VALUES (@RECIPEID, @REQUIREDMATERIALID, @NUMBEROFREQUIREDMATERIAL)";
                            command.Parameters.Add(new SQLiteParameter("@RECIPEID", datam[0]));
                            command.Parameters.Add(new SQLiteParameter("@REQUIREDMATERIALID", datam[1]));
                            command.Parameters.Add(new SQLiteParameter("@NUMBEROFREQUIREDMATERIAL", datam[2]));
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An exception was thrown when inserting to Required Materials Table.");
                            Console.WriteLine("Exception Message:" + e.Message);
                        }
                    }
                }
            }
        }
    }
}

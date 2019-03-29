using System;

namespace SatisfactoryLinePlanner.Beans
{
    class ProductionRecipe : Recipe
    {
        private int positon;                                // レシピの位置
        private double numberOfMaterialProduction;          // 設定された分間生産数
        private ProductionBuildingWithState buildingToUse;  // レシピで使用する建物の情報
        private int purity;                                 // レシピが鉱石だった場合の鉱脈の純度

        /// <summary>
        /// レシピの位置
        /// </summary>
        public int Position { get { return positon; }set { positon = value; } } 

        /// <summary>
        /// 設定された分間生産数
        /// </summary>
        public double NumberOfMaterialProduction { get { return numberOfMaterialProduction; } set { numberOfMaterialProduction = value; } }

        /// <summary>
        /// レシピで使用する建物の情報
        /// </summary>
        public ProductionBuildingWithState BuildingToUse { get { return buildingToUse; } set { buildingToUse = value; } }

        /// <summary>
        /// レシピが鉱石だった場合の鉱脈の純度
        /// </summary>
        public int Purity { get { return purity; } set { purity = value; } }

        /// <summary>
        /// 総消費電力（MW）
        /// </summary>
        public double TotalPowerUsage
        {
            get { return buildingToUse.PowerUsage * buildingToUse.NumberOfBuildings; }
        }

        /// <summary>
        /// 実単位生産時間
        /// </summary>
        public double ActualUnitProductionTime
        {
            get
            {
                if(IsOre == 1)
                {
                    if(purity == 0)
                    {
                        return Math.Round((UnitProductionTime * 2.0) / (buildingToUse.Efficiency / 100.0) / (buildingToUse.OverclockingNumber / 100.0), 2);
                    }
                    else if(purity == 2)
                    {
                        return Math.Round((UnitProductionTime / 2.0) / (buildingToUse.Efficiency / 100.0) / (buildingToUse.OverclockingNumber / 100.0), 2);
                    }
                    else
                    {
                        return Math.Round(UnitProductionTime / (buildingToUse.Efficiency / 100.0) / (buildingToUse.OverclockingNumber / 100.0), 2);
                    }
                }
                else
                {
                    return Math.Round(UnitProductionTime / (buildingToUse.Efficiency / 100.0) / (buildingToUse.OverclockingNumber / 100.0), 2);
                }
            }
        }

        /// <summary>
        /// 実分間生産回数
        /// </summary>
        public double ActualProductionsNumberOfTimes
        {
            get { return Math.Round(60.0 / ActualUnitProductionTime, 2); }
        }

        /// <summary>
        /// 実分間生産数
        /// </summary>
        public double ActualNumberOfProductionPerMin
        {
            get { return ActualProductionsNumberOfTimes * UnitProductionPieces; }
        }

        /// <summary>
        /// 実分間素材要求数
        /// </summary>
        public double[] ActualRequiredNumberOfMaterialsPerMin
        {
            get
            {
                double[] required = new double[RequiredMaterials.Count];
                int i = 0;
                foreach (RequiredMaterial requiredMaterial in RequiredMaterials)
                {
                    required[i] = Math.Round(requiredMaterial.NumberOfRequired * ActualProductionsNumberOfTimes * NumberOfMaterialProduction / ActualProductionsNumberOfTimes, 2);
                    i++;
                }
                return required;
            }
        }

        public ProductionRecipe()
        {
            buildingToUse = new ProductionBuildingWithState();
        }
    }
}

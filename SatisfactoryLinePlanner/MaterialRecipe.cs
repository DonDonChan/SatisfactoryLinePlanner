using System.Collections.Generic;

namespace SatisfactoryLinePlanner
{
    public class MaterialRecipe
    {
        public string RecipeId { get; set; } // レシピID
        public string RecipeName { get; set; } // レシピ名

        public string RequiredFacilityId { get; set; } // 要求施設タイプ
        public string UseFacilityId { get; set; } // 使用施設ID
        public string UseFacility { get; set; } // 使用施設
        public int FacilityEfficiency { get; set; } // 施設効率
        public double FacilityValue { get; set; } // 施設数
        public int FacirilyPowerUsage { get; set; } // 施設電力使用量

        public int OverClockValue { get; set; } // オーバークロック数（%）

        public string ProductMaterialId { get; set; } // 生産物ID
        public string ProductMaterialName { get; set; } // 生産物名
        public double ProductMaterialValue { get; set; } // 生産個数

        public int UnitProductionTime { get; set; } // 単位生産時間
        public int UnitProductionValue { get; set; } // 単位生産個数

        public List<string> RequestMaterialId { get; set; } // 要求素材IDリスト
        public List<string> RequestMaterialNames { get; set; } // 要求素材名
        public List<int> RequestMaterialValues { get; set; } // 要求素材数
    }
}

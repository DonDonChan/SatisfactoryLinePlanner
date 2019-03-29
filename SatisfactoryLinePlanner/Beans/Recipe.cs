using System.Collections.Generic;

namespace SatisfactoryLinePlanner.Beans
{
    /// <summary>
    /// レシピを保存していろいろするクラス
    /// </summary>
    class Recipe
    {
        private string recipeId;                            // レシピID
        private string recipeName;                          // レシピ名
        private Material productionMaterial;                // 生産品の情報
        private Building requiredBuilding;                  // 必要な建物のタイプ情報
        private int unitProductionTime;                     // 単位生産時間（秒）
        private int unitProductionPieces;                   // 単位生産数（個）
        private List<RequiredMaterial> requiredMaterials;   // 要求素材の情報リスト
        private int isOre;                                  // このレシピが鉱石か

        /// <summary>
        /// レシピID
        /// </summary>
        public string RecipeId { get { return recipeId; } set { recipeId = value; } }

        /// <summary>
        /// レシピ名
        /// </summary>
        public string RecipeName { get { return recipeName; } set { recipeName = value; } }

        /// <summary>
        /// 生産品の情報
        /// </summary>
        public Material ProductionMaterial { get { return productionMaterial; } set { productionMaterial = value; } }

        /// <summary>
        /// 必要な建物のタイプ情報
        /// </summary>
        public Building RequiredBuilding { get { return requiredBuilding; } set { requiredBuilding = value; } }

        /// <summary>
        /// 単位生産時間（秒）
        /// </summary>
        public int UnitProductionTime { get { return unitProductionTime; } set { unitProductionTime = value; } }

        /// <summary>
        /// 単位生産数（個）
        /// </summary>
        public int UnitProductionPieces { get { return unitProductionPieces; } set { unitProductionPieces = value; } }

        /// <summary>
        /// 要求素材の情報リスト
        /// </summary>
        public List<RequiredMaterial> RequiredMaterials { get { return requiredMaterials; } set { requiredMaterials = value; } }

        /// <summary>
        /// このレシピが鉱石か
        /// </summary>
        public int IsOre { get { return isOre; } set { isOre = value; } }

        public Recipe()
        {
            productionMaterial = new Material();
            requiredBuilding = new Building();
            requiredMaterials = new List<RequiredMaterial>();
        }
    }
}

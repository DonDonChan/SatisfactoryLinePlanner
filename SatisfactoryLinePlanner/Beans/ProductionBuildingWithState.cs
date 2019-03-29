namespace SatisfactoryLinePlanner.Beans
{
    /// <summary>
    /// 建物の状態（数、OC値）も保持するクラス
    /// </summary>
    class ProductionBuildingWithState : ProductionBuilding
    {
        private double numberOfBuildings;   // 建物の数
        private int overclockNumber;        // オーバークロック数（%）

        /// <summary>
        /// 建物の数
        /// </summary>
        public double NumberOfBuildings { get { return numberOfBuildings; } set { numberOfBuildings = value; } }

        /// <summary>
        /// オーバークロック数（%）
        /// </summary>
        public int OverclockingNumber { get { return overclockNumber; } set { overclockNumber = value; } }
    }
}

namespace SatisfactoryLinePlanner.Beans
{
    /// <summary>
    /// 建造の概要を保存するクラス
    /// </summary>
    class ProductionBuilding : Building
    {
        private string buildingId;  // 建物ID
        private string name;        //建物名
        private int efficiency;     // 建物の効率（%）
        private int powerUsage;     // 建物の消費電力（MW）

        /// <summary>
        /// 建物ID
        /// </summary>
        public string BuildingId { get { return buildingId; } set { buildingId = value; } }

        /// <summary>
        /// 建物名
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// 建物の効率（%）
        /// </summary>
        public int Efficiency { get { return efficiency; } set { efficiency = value; } }

        /// <summary>
        /// 建物の消費電力（MW）
        /// </summary>
        public int PowerUsage { get { return powerUsage; } set { powerUsage = value; } }
    }
}

namespace SatisfactoryLinePlanner.Beans
{
    /// <summary>
    /// 素材、生産物の情報を保持するクラス
    /// </summary>
    class Material
    {
        private string materialId;  // 素材ID
        private string name;        // 素材名

        /// <summary>
        /// 素材ID
        /// </summary>
        public string MaterialId { get { return materialId; } set { materialId = value; } }

        /// <summary>
        /// 素材名
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
    }
}

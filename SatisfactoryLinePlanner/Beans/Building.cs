namespace SatisfactoryLinePlanner.Beans
{
    /// <summary>
    /// 建物のタイプを保持するクラス
    /// </summary>
    class Building
    {
        private string typeId;  // 建物タイプID
        private string type;    // 建物タイプ名

        /// <summary>
        /// 建物タイプID
        /// </summary>
        public string TypeId { get { return typeId; } set { typeId = value; } }

        /// <summary>
        /// 建物タイプ名
        /// </summary>
        public string Type { get { return type; } set { type = value; } }
    }
}

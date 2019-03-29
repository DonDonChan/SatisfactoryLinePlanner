namespace SatisfactoryLinePlanner.Beans
{
    /// <summary>
    /// 要求素材の情報を保持するクラス
    /// </summary>
    class RequiredMaterial : Material
    {
        private int numberOfRequired;   // 単位素材要求数

        /// <summary>
        /// 単位素材要求数
        /// </summary>
        public int NumberOfRequired { get { return numberOfRequired; } set { numberOfRequired = value; } }
    }
}

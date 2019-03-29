using SatisfactoryLinePlanner.ListItems;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SatisfactoryLinePlanner.ProductionBlockTab
{
    /// <summary>
    /// ProductionBlockTab.xaml の相互作用ロジック
    /// </summary>
    public partial class ProductionBlockTab : UserControl
    {
        public ProductionBlockTab()
        {
            InitializeComponent();
        }

        /// <summary>
        /// レシピ追加ボタンにイベントを登録する
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOnButton_AddRecipes(RoutedEventHandler eventHandler)
        {
            Button_AddRcipe.Click += eventHandler;
        }

        /// <summary>
        /// レシピ消去ボタンにイベントを登録する
        /// </summary>
        /// <param name="addRecipeButtonClicked">登録するイベント</param>
        public void RegisterEventOnButton_ClearRecipes(RoutedEventHandler eventHandler)
        {
            Button_ClearRecipes.Click += eventHandler;
        }

        /// <summary>
        /// 生産物リストにアイテムを追加
        /// </summary>
        /// <param name="materialWithNumber">追加するやつのリスト</param>
        public void AddMaterialsToProductionListPanel(IReadOnlyList<MaterialWithNum> materialWithNumber)
        {
            foreach(MaterialWithNum item in materialWithNumber)
            {
                Panel_ProductionMaterialsList.Children.Add(item);
            }
        }

        /// <summary>
        /// 要求物リストにアイテムを追加
        /// </summary>
        /// <param name="materialWithNumOnButton">追加するやつのリスト</param>
        public void AddMaterialsToRequiredListPanel(IReadOnlyList<MaterialWithNumOnButton> materialWithNumber)
        {
            foreach(MaterialWithNumOnButton item in materialWithNumber)
            {
                Panel_RequiredMaterialsList.Children.Add(item);
            }
        }

        /// <summary>
        /// レシピのリストをパネルに表示する
        /// </summary>
        /// <param name="productionRecipes">レシピのリスト</param>
        public void AddRecipesToRecipeListPanel(IReadOnlyList<ProductionRecipeListItem> productionRecipes)
        {
            foreach(ProductionRecipeListItem item in productionRecipes)
            {
                Panel_ProductionRecipeList.Children.Add(item);
            }
        }

        /// <summary>
        /// リストの表示クリア
        /// </summary>
        public void ClearListPanels()
        {
            // 生産物リストと要求物リストの表示をクリア
            Panel_ProductionMaterialsList.Children.Clear();
            Panel_RequiredMaterialsList.Children.Clear();

            // レシピリストの表示をクリア
            Panel_ProductionRecipeList.Children.Clear();
        }
    }
}

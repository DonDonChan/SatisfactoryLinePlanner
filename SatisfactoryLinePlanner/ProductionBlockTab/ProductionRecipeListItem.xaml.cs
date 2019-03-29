using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SatisfactoryLinePlanner.ProductionBlockTab
{
    /// <summary>
    /// ProductionRecipeListItem.xaml の相互作用ロジック
    /// </summary>
    public partial class ProductionRecipeListItem : UserControl
    {
        public ProductionRecipeListItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 位置変更ボタン（↑）にイベントを登録する
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOnButton_UpItem(RoutedEventHandler eventHandler)
        {
            Button_UpItem.Click += eventHandler;
        }

        /// <summary>
        /// 位置変更ボタン（↓）にイベントを登録する
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOnButton_DownItem(RoutedEventHandler eventHandler)
        {
            Button_DownItem.Click += eventHandler;
        }

        /// <summary>
        /// レシピ削除ボタンにイベントを登録する
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOnButton_DeleteItem(RoutedEventHandler eventHandler)
        {
            Button_DeleteItem.Click += eventHandler;
        }
    }
}

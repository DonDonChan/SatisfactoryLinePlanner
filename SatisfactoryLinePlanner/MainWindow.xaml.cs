using SatisfactoryLinePlanner.Database;
using SatisfactoryLinePlanner.ProductionBlockTab;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SatisfactoryLinePlanner
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // ウィンドウのどこでも移動可能にする
            MouseLeftButtonDown += (sender, e) => this.DragMove();
        }

        private void DBInitButtonClicked(object sender, EventArgs e)
        {
            DBInitializer dbInitializer = new DBInitializer();

            dbInitializer.InitializeAndInsert();
        }

        /// <summary>
        /// メインコンテンツパネルにコントロールを追加
        /// </summary>
        /// <param name="control">追加するコントロール</param>
        public void AddPanelToMainContent(UserControl control)
        {
            Panel_MainContents.Children.Add(control);
        }

        /// <summary>
        /// メインコンテンツパネルをクリア
        /// </summary>
        public void ClearMainContent()
        {
            Panel_MainContents.Children.Clear();
        }

        /// <summary>
        /// 最小化ボタンが押された
        /// </summary>
        /// <param name="eventHandler">追加するイベント</param>
        public void RegisterEventToMinimumWindowButton(RoutedEventHandler eventHandler)
        {
            Button_MinimumWindow.Click += eventHandler;
        }

        /// <summary>
        /// ウィンドウサイズ変更ボタンが押された
        /// </summary>
        /// <param name="eventHandler">追加するイベント</param>
        public void RegisterEventToChangeWindowSizeButton(RoutedEventHandler eventHandler)
        {
            Button_ChangeWindowSize.Click += eventHandler;
        }

        /// <summary>
        /// ウィンドウサイズが最大化された
        /// </summary>
        /// <param name="eventHandler"></param>
        public void RegisterEventToResizedToMaximum(SizeChangedEventHandler eventHandler)
        {
            this.SizeChanged += eventHandler;
        }

        /// <summary>
        /// ウィンドウを閉じるボタンが押された
        /// </summary>
        /// <param name="eventHandler">追加するイベント</param>
        public void RegisterEventToCloseWindowButton(RoutedEventHandler eventHandler)
        {
            Button_CloseWindow.Click += eventHandler;
        }

        /// <summary>
        /// 全体生産情報タブメニューをクリックした
        /// </summary>
        /// <param name="eventHandler">追加するイベント</param>
        public void RegisterEventToOverallMenuTabButton(RoutedEventHandler eventHandler)
        {
            Button_OverallTab.Click += eventHandler;
        }

        /// <summary>
        /// 生産ブロックタブメニューをクリックした
        /// </summary>
        /// <param name="eventHandler">追加するイベント</param>
        public void RegisterEventToProductionBlockMenuTabButton(RoutedEventHandler eventHandler)
        {
            Button_ProductionBlock.Click += eventHandler;
        }
    }
}

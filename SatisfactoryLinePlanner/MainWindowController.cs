using System;
using System.Windows;
using SatisfactoryLinePlanner.OverallTab;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SatisfactoryLinePlanner.ProductionBlockTab;

namespace SatisfactoryLinePlanner
{
    class MainWindowController
    {
        private MainWindow mainWindow;

        public MainWindowController()
        {
            // メインウィンドウの初期化と表示位置をスクリーンの真ん中へ
            mainWindow = new MainWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            // イベントの追加
            mainWindow.RegisterEventToMinimumWindowButton(MinimumWindowButtonClicked);
            mainWindow.RegisterEventToChangeWindowSizeButton(WindowSizeChangeButtonClicked);
            mainWindow.RegisterEventToResizedToMaximum(ResizedMaximumSize);
            mainWindow.RegisterEventToCloseWindowButton(WindowCloseButtonClicked);
            mainWindow.RegisterEventToOverallMenuTabButton(OverallMenuTabButtonClicked);
            mainWindow.RegisterEventToProductionBlockMenuTabButton(ProductionBlockMenuTabButtonClicked);

            mainWindow.Show();
        }

        /// <summary>
        /// ウィンドウ最小化ボタンが押されたイベント
        /// </summary>
        public void MinimumWindowButtonClicked(object sender, EventArgs e)
        {
            mainWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// ウィンドウサイズボタンが押されたイベント
        /// </summary>
        public void WindowSizeChangeButtonClicked(object sender, EventArgs e)
        {
            if (mainWindow.WindowState.Equals(WindowState.Maximized))
            {
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.Button_ChangeWindowSize.Content = "1";
                mainWindow.WindowBorder.Margin = new Thickness(20);
            }
            else
            {
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.Button_ChangeWindowSize.Content = "2";
                mainWindow.WindowBorder.Margin = new Thickness(0);
            }
        }

        /// <summary>
        /// ウィンドウが変更された
        /// </summary>
        public void ResizedMaximumSize(object sender, EventArgs e)
        {
            if (mainWindow.WindowState.Equals(WindowState.Maximized))
            {
                mainWindow.Button_ChangeWindowSize.Content = "2";
                mainWindow.WindowBorder.Margin = new Thickness(0);
            }
            else if (mainWindow.WindowState.Equals(WindowState.Normal))
            {
                mainWindow.Button_ChangeWindowSize.Content = "1";
                mainWindow.WindowBorder.Margin = new Thickness(20);
            }
        }

        /// <summary>
        /// ウィンドウを閉じるボタンが押されたイベント
        /// </summary>
        public void WindowCloseButtonClicked(object sender, EventArgs e)
        {
            mainWindow.Close();

            Environment.Exit(0);
        }

        /// <summary>
        /// メニュータブで全体情報を押した
        /// </summary>
        public void OverallMenuTabButtonClicked(object sender, EventArgs e)
        {
            mainWindow.ClearMainContent();
            OverallTabController controller = new OverallTabController();
            mainWindow.AddPanelToMainContent(controller._OverallTab);
        }

        /// <summary>
        /// メニュータブで生産ブロックを押した
        /// </summary>
        public void ProductionBlockMenuTabButtonClicked(object sender, EventArgs e)
        {
            mainWindow.ClearMainContent();
            ProductionBlockTabController controller = new ProductionBlockTabController(mainWindow);
            mainWindow.AddPanelToMainContent(controller.BlockTab);
        }
    }
}

using SatisfactoryLinePlanner.Database;
using SatisfactoryLinePlanner.ProductionBlockTab;
using System;
using System.Windows;

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

            ProductionBlockTabController productBlockTabController = new ProductionBlockTabController(this);

            ProductionTab.Content = productBlockTabController.BlockTab;

            DBInitButton.Click += new RoutedEventHandler(DBInitButtonClicked);
        }

        private void DBInitButtonClicked(object sender, EventArgs e)
        {
            DBInitializer dbInitializer = new DBInitializer();

            dbInitializer.InitializeAndInsert();
        }
    }
}

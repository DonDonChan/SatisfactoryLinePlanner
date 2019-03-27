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

            string dbFilePath = "./SatisfactoryData.db";

            if (!System.IO.File.Exists(dbFilePath))
            {
                MessageBox.Show("データベースを作成します。");
                DatabaseOperation databaseOperation = new DatabaseOperation();
                databaseOperation.InitializeDB();
            }

            DBInitButton.Click += new RoutedEventHandler(DBInitButtonClicked);
        }

        private void DBInitButtonClicked(object sender, EventArgs e)
        {
            DatabaseOperation databaseOperation = new DatabaseOperation();

            databaseOperation.InitializeDB();
        }
    }
}

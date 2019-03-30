using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SatisfactoryLinePlanner.RecipeSelect
{
    /// <summary>
    /// RecipeSelectWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class RecipeSelectWindow : Window
    {
        public RecipeSelectWindow()
        {
            InitializeComponent();

            // ウィンドウのどこでも移動可能にする
            MouseLeftButtonDown += (sender, e) => this.DragMove();
        }

        /// <summary>
        /// レシピ名選択コンボボックスにイベントを登録
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOnRecipeNameListComboBox(SelectionChangedEventHandler eventHandler)
        {
            Combobox_Recipe.SelectionChanged += eventHandler;
        }

        /// <summary>
        /// 生産数設定テキストボックスにイベント登録
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOnNumberOfProductionTextBox(TextChangedEventHandler eventHandler)
        {
            TextBox_RequiredNumberOfProduction.TextChanged += eventHandler;
            //TextBox_RequiredNumberOfProduction.LostFocus += eventHandler;
        }

        /// <summary>
        /// 建物選択コンボボックスにイベントを登録
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventBuildingListComboBox(SelectionChangedEventHandler eventHandler)
        {
            Combobox_Building.SelectionChanged += eventHandler;
        }

        /// <summary>
        /// オーバークロックスライダーにイベント登録
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOverclockNumberSlider(RoutedPropertyChangedEventHandler<double> eventHandler)
        {
            Slider_Overclock.ValueChanged += eventHandler;
        }

        /// <summary>
        /// オーバークロックテキストボックスにイベント登録
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOverclockNumberTextBox(TextChangedEventHandler eventHandler)
        {
            TextBox_OverclockNumber.TextChanged += eventHandler;
        }

        /// <summary>
        /// ラジオボタンにイベントを登録
        /// </summary>
        public void RegisterEventPurityRadioButton(RoutedEventHandler eventHandler)
        {
            Radio_Impure.Checked += eventHandler;
            Radio_Normal.Checked += eventHandler;
            Radio_Pure.Checked += eventHandler;
        }

        /// <summary>
        /// 決定ボタンにイベントを登録
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOnConfirmButtton(RoutedEventHandler eventHandler)
        {
            Button_Confirm.Click += eventHandler;
        }

        /// <summary>
        /// キャンセルボタンにイベントを登録
        /// </summary>
        /// <param name="eventHandler">登録するイベント</param>
        public void RegisterEventOnCancelButtton(RoutedEventHandler eventHandler)
        {
            Button_Cancel.Click += eventHandler;
        }

        /// <summary>
        /// レシピ名のリストをコンボボックスに入れる
        /// </summary>
        /// <param name="names">レシピ名のリスト データベースからとったまま</param>
        public void SetRecipeNameListToComboBox(IReadOnlyList<string[]> names)
        {
            Combobox_Recipe.Items.Clear();

            foreach (string[] name in names)
            {
                Combobox_Recipe.Items.Add(name[0]);
            }

            //Combobox_Recipe.SelectedIndex = 0;  // リストの一番上を選択しておく
        }

        /// <summary>
        /// 要求素材リストに表示
        /// </summary>
        /// <param name="materials">表示する要求素材のリスト</param>
        public void SetRequiredMaterialsToPanel(IReadOnlyList<string[]> materials)
        {
            Grid[] requiredGrids = {
                Panel_Required1,
                Panel_Required2,
                Panel_Required3,
                Panel_Required4
            };

            TextBlock[] requiredTextNames = {
                TextBlock_RequiredName1,
                TextBlock_RequiredName2,
                TextBlock_RequiredName3,
                TextBlock_RequiredName4,
            };

            TextBlock[] requiredTextUnitNums = {
                TextBlock_UnitRequiredNum1,
                TextBlock_UnitRequiredNum2,
                TextBlock_UnitRequiredNum3,
                TextBlock_UnitRequiredNum4,
            };

            TextBlock[] requiredTextActualNums = {
                TextBlock_ActualRequiredNum1,
                TextBlock_ActualRequiredNum2,
                TextBlock_ActualRequiredNum3,
                TextBlock_ActualRequiredNum4,
            };

            // 一旦全非表示
            foreach (Grid grid in requiredGrids)
            {
                grid.Visibility = Visibility.Hidden;
            }

            // 要求素材のリストから表示させる
            int i = 0;
            foreach (string[] material in materials)
            {
                requiredGrids[i].Visibility = Visibility.Visible;
                requiredTextNames[i].Text = material[0];
                requiredTextUnitNums[i].Text = material[1];
                requiredTextActualNums[i].Text = material[2];
                i++;
            }
        }

        /// <summary>
        /// 建物のリストをコンボボックスに入れる
        /// </summary>
        /// <param name="buildings">建物のリスト データベースからとったまま</param>
        public void SetBuildingListToComboBox(IReadOnlyList<string[]> buildings)
        {
            Combobox_Building.Items.Clear();

            foreach (string[] building in buildings)
            {
                Combobox_Building.Items.Add(building[0]);
            }

            Combobox_Building.SelectedIndex = 0;    // リストの一番上を選択しておく
        }

        /// <summary>
        /// 純度ラジオボタンの表示切り替え
        /// </summary>
        /// <param name="flag">どっちか</param>
        public void ChangeVisibiilityRadios(int flag)
        {
            if (flag == 1)
            {
                PurityGroup.Visibility = Visibility.Visible;
            }
            else
            {
                PurityGroup.Visibility = Visibility.Hidden;
            }
        }

        public void ShowMessageInvalidNumberOfProduction()
        {
            MessageBox.Show(this, "レシピの設定に不足があるか無効です。\n・レシピは選択しましたか？\n・生産数が0以下になっていませんか？\n・生産数が少なすぎませんか？", "レシピを追加できません", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        public void BorderAttentionOnRecipeSelect(bool flag)
        {
            if(flag)
            {
                Combobox_Recipe.BorderBrush = Brushes.Red;
            }
            else
            {
                Combobox_Recipe.BorderBrush = Brushes.White;
            }
        }

        public void BorderAttentionOnNumberOfProduction(bool flag)
        {
            if (flag)
            {
                TextBox_RequiredNumberOfProduction.BorderBrush = Brushes.Red;
            }
            else
            {
                TextBox_RequiredNumberOfProduction.BorderBrush = Brushes.White;
            }
        }
    }
}

using SatisfactoryLinePlanner.Beans;
using SatisfactoryLinePlanner.Database;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SatisfactoryLinePlanner.RecipeSelect
{
    /// <summary>
    /// レシピを選択するウィンドウを操作する
    /// 
    /// ToDo
    ///     ・コンボックスに入れる処理でリストから取り出して渡す（かも）
    /// 
    /// </summary>
    class RecipeSelectWindowController
    {
        private ProductionRecipe productionRecipe;  // 生産レシピクラス

        private RecipeSelectWindow recipeSelectWindow;   // レシピ選択ウィンドウ

        private bool recipeDone = false;    // レシピが設定されているかどうか

        public ProductionRecipe ProductionRecipe { get { return recipeDone ? productionRecipe : null; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RecipeSelectWindowController()
        {
            // レシピ選択ウィンドウの初期化
            recipeSelectWindow = new RecipeSelectWindow();

            productionRecipe = new ProductionRecipe();

            SetRecipeListToCombox();    // レシピをコンボボックスへ

            // イベント登録
            recipeSelectWindow.RegisterEventOnRecipeNameListComboBox(RecipiListSelected);
            recipeSelectWindow.RegisterEventOnNumberOfProductionTextBox(RequestNumberOfProductinChanged);
            recipeSelectWindow.RegisterEventBuildingListComboBox(BuildingSelected);
            recipeSelectWindow.RegisterEventPurityRadioButton(ChangePurityRadio);
            recipeSelectWindow.RegisterEventOverclockNumberSlider(SliderValueChanged);
            recipeSelectWindow.RegisterEventOverclockNumberTextBox(OverclockTextChanged);
            recipeSelectWindow.RegisterEventOnConfirmButtton(ConfirmButtonClicked);
            recipeSelectWindow.RegisterEventOnCancelButtton(CancelButtonClicked);

            // 鉱石純度ラジオを非表示
            recipeSelectWindow.ChangeVisibiilityRadios(0);

            // オーバークロックだけ初期値を入れる
            productionRecipe.BuildingToUse.OverclockingNumber = 100;
        }

        /// <summary>
        /// 全レシピ名を取得してコンボボックスに入れる
        /// </summary>
        private void SetRecipeListToCombox()
        {
            DBSelect dbSelect = new DBSelect();
            List<string[]> recipeNames = dbSelect.SelectData("MaterialRecipes", null, new string[] { "Name" }, null, null, null);

            recipeSelectWindow.SetRecipeNameListToComboBox(recipeNames);
        }

        /// <summary>
        /// 生産する素材を指定してレシピ名を取得してコンボボックスに入れる
        /// </summary>
        private void SetRecipeListToCombox(string materialId)
        {
            DBSelect dbSelect = new DBSelect();
            List<string[]> recipeNames = dbSelect.SelectData("MaterialRecipes", null, new string[] { "Name" }, "MaterialId = @MATERIALID", new List<string[]>() { new string[] { "@MATERIALID", materialId } }, null);

            recipeSelectWindow.SetRecipeNameListToComboBox(recipeNames);

            recipeSelectWindow.Combobox_Recipe.SelectedIndex = 0;
        }

        /// <summary>
        /// 建物のリストを取得してコンボボックスに入れる
        /// </summary>
        /// <param name="typeId">指定する建物のタイプID</param>
        private void SetBuildingListToCombox(string typeId)
        {
            DBSelect dbSelect = new DBSelect();
            List<string[]> Buildings = dbSelect.SelectData("ProductionBuildings", null, new string[] { "Name" }, "TypeId = @TYPEID", new List<string[]>() { new string[] { "@TypeId", typeId } }, null);

            recipeSelectWindow.SetBuildingListToComboBox(Buildings);
        }

        /// <summary>
        /// レシピが選択された
        /// </summary>
        private void RecipiListSelected(object sender, EventArgs e)
        {
            productionRecipe.RecipeName = recipeSelectWindow.Combobox_Recipe.SelectedItem.ToString(); // 選択されたレシピ名を取得

            DBSelect dbSelect = new DBSelect();
            List<string[]> recipeData = dbSelect.SelectData("MaterialRecipes", null, new string[] { "RecipeId", "MaterialId", "RequiredBuildingTypeId", "UnitProductionTime", "UnitProductionPieces" }, "Name = @NAME", new List<string[]>() { new string[] { "@NAME", productionRecipe.RecipeName } }, null);

            string[] recipe = recipeData[0];    // レシピデータを取り出す

            // 取り出したデータを格納
            productionRecipe.RecipeId = recipe[0];                          // レシピID
            productionRecipe.ProductionMaterial.MaterialId = recipe[1];     // 生産物ID
            productionRecipe.BuildingToUse.TypeId = recipe[2];              // 要求建物タイプID
            productionRecipe.UnitProductionTime = int.Parse(recipe[3]);     // 単位生産時間
            productionRecipe.UnitProductionPieces = int.Parse(recipe[4]);   // 単位生産数

            // 生産品名を取得
            List<string[]> materialName = dbSelect.SelectData("Materials", null, new string[] { "Name", "IsOre" }, "MaterialId = @MATERIALID", new List<string[]>() { new string[] { "@MATERIALID", productionRecipe.ProductionMaterial.MaterialId } }, null);
            productionRecipe.ProductionMaterial.Name = materialName[0][0];
            productionRecipe.IsOre = int.Parse(materialName[0][1]);

            // 鉱石だったら純度ラジオを表示する
            recipeSelectWindow.ChangeVisibiilityRadios(productionRecipe.IsOre);

            // 要求素材を取得
            productionRecipe.RequiredMaterials.Clear();

            List<string[]> requiredMaterials = dbSelect.SelectData("RequiredMaterials RM", "LEFT JOIN Materials M ON RM.RequiredMaterialId = M.MaterialId", new string[] { "RequiredMaterialId", "Name", "NumberOfRequiredMaterial" }, "RecipeId = @RECIPEID", new List<string[]>() { new string[] { "@RECIPEID", productionRecipe.RecipeId } }, null);
            foreach (string[] required in requiredMaterials)
            {
                RequiredMaterial requiredMaterial = new RequiredMaterial();
                requiredMaterial.MaterialId = required[0];
                requiredMaterial.Name = required[1];
                requiredMaterial.NumberOfRequired = int.Parse(required[2]);

                productionRecipe.RequiredMaterials.Add(requiredMaterial);
            }

            recipeSelectWindow.BorderAttentionOnRecipeSelect(false);

            // 要求素材を表示させる
            RequiedMaterialsChange();

            // 施設リストを取得して入れる
            SetBuildingListToCombox(productionRecipe.BuildingToUse.TypeId);
        }

        /// <summary>
        /// 生産数が変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestNumberOfProductinChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            double productValue;

            // 小数点入力は第1位まで0を許してあげる
            if (Regex.IsMatch(textBox.Text, @"^\d\d{0,6}\.$|\.[0]$", RegexOptions.ECMAScript))
            {
                productValue = 0;
            }
            else
            {
                if (double.TryParse(textBox.Text, out productValue))
                {
                    if (productValue < 0)
                    {
                        productValue = 0;
                    }
                    else if (productValue > 1000000)
                    {
                        productValue = 1000000;
                    }
                }
                else
                {
                    productValue = 0;
                }

                textBox.Text = Math.Round(productValue, 2).ToString();
                productionRecipe.NumberOfMaterialProduction = Math.Round(productValue, 2);  // 少数2位まで残す

                recipeSelectWindow.BorderAttentionOnNumberOfProduction(false);

                ProductionStatesChanged();
            }
        }

        /// <summary>
        /// 使用施設が変更された
        /// </summary>
        private void BuildingSelected(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.Items.Count > 0)
            {
                string selected = comboBox.SelectedItem.ToString();

                // 施設の検索
                DBSelect dbSelect = new DBSelect();
                List<string[]> buildings = dbSelect.SelectData("ProductionBuildings", null, new string[] { "BuildingId", "Name", "RequiredPower", "Efficiency" }, "Name = @NAME", new List<string[]>() { new string[] { "@NAME", selected } }, null);

                string[] building = buildings[0];
                productionRecipe.BuildingToUse.BuildingId = building[0];
                productionRecipe.BuildingToUse.Name = building[1];
                productionRecipe.BuildingToUse.PowerUsage = int.Parse(building[2]);
                productionRecipe.BuildingToUse.Efficiency = int.Parse(building[3]);

                ProductionStatesChanged();
            }
        }

        /// <summary>
        /// OCテキストボックスの値が変わったらスライドの位置も変える
        /// </summary>
        private void OverclockTextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (int.TryParse(textBox.Text, out int overClockValue))
            {
                if (overClockValue < 0)
                {
                    overClockValue = 1;
                    textBox.Text = overClockValue.ToString();
                    recipeSelectWindow.Slider_Overclock.Value = overClockValue;
                }
                else if (overClockValue > 250)
                {
                    overClockValue = 250;
                    textBox.Text = overClockValue.ToString();
                    recipeSelectWindow.Slider_Overclock.Value = overClockValue;
                }
                else
                {
                    textBox.Text = overClockValue.ToString();
                    recipeSelectWindow.Slider_Overclock.Value = overClockValue;
                }
            }
            else
            {
                overClockValue = 1;
                textBox.Text = overClockValue.ToString();
                recipeSelectWindow.Slider_Overclock.Value = overClockValue;
            }

            productionRecipe.BuildingToUse.OverclockingNumber = overClockValue;
            ProductionStatesChanged();
        }

        /// <summary>
        /// スライドの位置が変わったらOCテキストボックスの値も変える
        /// </summary>
        private void SliderValueChanged(object sender, EventArgs e)
        {
            if (sender is Slider slider)
            {
                recipeSelectWindow.TextBox_OverclockNumber.Text = Math.Round(slider.Value).ToString();

                int overClockValue = (int)(Math.Round(slider.Value));
                productionRecipe.BuildingToUse.OverclockingNumber = overClockValue;
                ProductionStatesChanged();
            }
        }

        /// <summary>
        /// 純度ラジオボタンが変更された
        /// </summary>
        public void ChangePurityRadio(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Name == "Radio_Impure")
            {
                productionRecipe.Purity = 0;
            }
            else if (radioButton.Name == "Radio_Normal")
            {
                productionRecipe.Purity = 1;
            }
            else
            {
                productionRecipe.Purity = 2;
            }

            ProductionStatesChanged();
        }

        /// <summary>
        /// 要求素材の表示
        /// </summary>
        public void RequiedMaterialsChange()
        {
            List<string[]> materials = new List<string[]>();

            int i = 0;
            foreach (RequiredMaterial material in productionRecipe.RequiredMaterials)
            {
                string[] required = {
                    material.Name,
                    material.NumberOfRequired.ToString(),
                    productionRecipe.ActualRequiredNumberOfMaterialsPerMin[i].ToString()
                };
                i++;

                materials.Add(required);
            }

            recipeSelectWindow.SetRequiredMaterialsToPanel(materials);
        }

        /// <summary>
        /// 確定ボタンが押された
        /// </summary>
        public void ConfirmButtonClicked(object sender, EventArgs e)
        {
            // レシピ選択のチェック
            if (productionRecipe.RecipeName != null && !productionRecipe.RecipeName.Equals(""))
            {
                recipeDone = true;
            }
            else
            {
                recipeDone = false;
                recipeSelectWindow.BorderAttentionOnRecipeSelect(true);
            }

            // 生産数設定のチェック
            if (productionRecipe.NumberOfMaterialProduction > 0)
            {

                recipeDone = true;
            }
            else
            {
                recipeDone = false;
                recipeSelectWindow.BorderAttentionOnNumberOfProduction(true);
            }

            if (recipeDone)
            {
                recipeSelectWindow.Close();
            }
        }

        /// <summary>
        /// キャンセルボタンが押された
        /// </summary>
        public void CancelButtonClicked(object sender, EventArgs e)
        {
            recipeDone = false;

            recipeSelectWindow.Close();
        }

        /// <summary>
        /// 生産に関する数値が変わった(生産数とかOCとか)
        /// </summary>
        private void ProductionStatesChanged()
        {
            recipeSelectWindow.TextBlock_UnitProductionTime.Text = Math.Round(productionRecipe.ActualUnitProductionTime, 2).ToString(); // 実単位生産時間
            recipeSelectWindow.TextBlock_UnitProductionPieces.Text = productionRecipe.UnitProductionPieces.ToString(); // 実単位生産数
            recipeSelectWindow.TextBlock_NumberOfProductionPerMin.Text = Math.Round(productionRecipe.ActualNumberOfProductionPerMin, 2).ToString();  // 実分間生産数

            double requestFacilityValue = Math.Round(productionRecipe.NumberOfMaterialProduction / productionRecipe.ActualNumberOfProductionPerMin, 2); // 必要な施設数
            productionRecipe.BuildingToUse.NumberOfBuildings = requestFacilityValue;
            recipeSelectWindow.TextBlock_RequiredNumberOfBuilding.Text = requestFacilityValue.ToString();

            // 要求素材を表示させる
            RequiedMaterialsChange();
        }

        /// <summary>
        /// レシピ選択ウィンドウを開く（初期値無し）
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        public void ShowRecipeSelectWindow(Window owner)
        {
            // 親ウィンドウの中央に表示させたい
            recipeSelectWindow.Owner = owner;
            recipeSelectWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            recipeSelectWindow.ShowDialog();
        }

        /// <summary>
        /// レシピ選択ウィンドウを開く（生産品、生産数指定）
        /// </summary>
        /// <param name="materialName"></param>
        /// <param name="numberOfProduction"></param>
        public void ShowRecipeSelectWindow(string materialName, double numberOfProduction, Window owner)
        {
            // 親ウィンドウの中央に表示させたい
            recipeSelectWindow.Owner = owner;
            recipeSelectWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // 素材IDを検索
            DBSelect dbSelect = new DBSelect();
            List<string[]> materialId = dbSelect.SelectData("Materials", null, new string[] { "MaterialId" }, "Name = @NAME", new List<string[]> { new string[] { "@NAME", materialName } }, null);

            SetRecipeListToCombox(materialId[0][0]);

            // 生産数を設定
            recipeSelectWindow.TextBox_RequiredNumberOfProduction.Text = numberOfProduction.ToString();
            Console.WriteLine("Num:" + numberOfProduction);

            recipeSelectWindow.ShowDialog();
        }
    }
}

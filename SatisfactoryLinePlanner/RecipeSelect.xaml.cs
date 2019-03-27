using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SatisfactoryLinePlanner
{
    /// <summary>
    /// RecipeSelect.xaml の相互作用ロジック
    /// </summary>
    public partial class RecipeSelect : Window
    {
        private DatabaseOperation databaseOperation;

        static private MaterialRecipe materialRecipe;

        static bool dataOk = false;

        public RecipeSelect()
        {
            InitializeComponent();

            // 色々初期化
            databaseOperation = new DatabaseOperation();
            materialRecipe = new MaterialRecipe();

            // イベントの追加
            ProductionRecipe.SelectionChanged += new SelectionChangedEventHandler(RecipiListSelected);
            RequestProductValue.TextChanged += new TextChangedEventHandler(ProductValueChanged);

            UseFacility.SelectionChanged += new SelectionChangedEventHandler(FacilitySelected);

            OverClockValue.TextChanged += new TextChangedEventHandler(OverClockTextChanged);
            OverClockSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(SliderValueChanged);

            ConfirmButton.Click += new RoutedEventHandler(ConfirmButtonClicked);
            CancelButton.Click += new RoutedEventHandler(CancelButtonClicked);

            // レシピのリスト表示
            GetRecipeList();

            // オーバークロックだけ初期値を入れる
            materialRecipe.OverClockValue = 100;
        }

        public RecipeSelect(string materialName, double productValue)
        {
            InitializeComponent();

            // 色々初期化
            databaseOperation = new DatabaseOperation();
            materialRecipe = new MaterialRecipe();

            // イベントの追加
            ProductionRecipe.SelectionChanged += new SelectionChangedEventHandler(RecipiListSelected);
            RequestProductValue.TextChanged += new TextChangedEventHandler(ProductValueChanged);

            UseFacility.SelectionChanged += new SelectionChangedEventHandler(FacilitySelected);

            OverClockValue.TextChanged += new TextChangedEventHandler(OverClockTextChanged);
            OverClockSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(SliderValueChanged);

            ConfirmButton.Click += new RoutedEventHandler(ConfirmButtonClicked);
            CancelButton.Click += new RoutedEventHandler(CancelButtonClicked);

            // オーバークロックだけ初期値を入れる
            materialRecipe.OverClockValue = 100;

            // レシピのリスト表示
            GetRecipeList(materialName);

            // 生産数入力
            RequestProductValue.Text = productValue.ToString();
        }

        /// <summary>
        /// OCテキストボックスの値が変わったらスライドの位置も変える
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OverClockTextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (int.TryParse(textBox.Text, out int overClockValue))
            {
                if (overClockValue < 0)
                {
                    overClockValue = 1;
                    textBox.Text = overClockValue.ToString();
                    OverClockSlider.Value = overClockValue;
                }
                else if (overClockValue > 250)
                {
                    overClockValue = 250;
                    textBox.Text = overClockValue.ToString();
                    OverClockSlider.Value = overClockValue;
                }
                else
                {
                    textBox.Text = overClockValue.ToString();
                    OverClockSlider.Value = overClockValue;
                }
            }
            else
            {
                overClockValue = 1;
                textBox.Text = overClockValue.ToString();
                OverClockSlider.Value = overClockValue;
            }

            materialRecipe.OverClockValue = overClockValue;
            EfficiencyValueChanged();
        }

        /// <summary>
        /// スライドの位置が変わったらOCテキストボックスの値も変える
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderValueChanged(object sender, EventArgs e)
        {
            if (sender is Slider slider)
            {
                OverClockValue.Text = Math.Round(slider.Value).ToString();

                int overClockValue = (int)(Math.Round(slider.Value));
                materialRecipe.OverClockValue = overClockValue;
                EfficiencyValueChanged();
            }
        }

        /// <summary>
        /// 効率が変わった(OCとか)
        /// </summary>
        private void EfficiencyValueChanged()
        {
            double upt = Math.Round((materialRecipe.UnitProductionTime / (materialRecipe.FacilityEfficiency / 100.0)) / (materialRecipe.OverClockValue / 100.0), 2);
            UnitProductionTime.Text = upt.ToString();
            UnitProductionValue.Text = materialRecipe.UnitProductionValue.ToString();
            ProductionValuePerMinute.Text = Math.Round((60.0 / upt) * materialRecipe.UnitProductionValue, 2).ToString();

            double requestFacilityValue = Math.Round(materialRecipe.ProductMaterialValue / (60.0 / upt) * materialRecipe.UnitProductionValue, 2);
            materialRecipe.FacilityValue = requestFacilityValue;
            RequestFacilityValue.Text = requestFacilityValue.ToString();
        }

        /// <summary>
        /// 全レシピ名を取得してコンボックスに入れる
        /// </summary>
        private void GetRecipeList()
        {
            List<string[]> list = databaseOperation.SelectData("MaterialRecipe", null, new string[] { "RecipeName" }, null, null, "RecipeName ASC");

            foreach (string[] val in list)
            {
                ProductionRecipe.Items.Add(val[0]);
            }
        }

        /// <summary>
        /// 指定の生産物レシピを取得してコンボックスに入れる
        /// </summary>
        private void GetRecipeList(string materialName)
        {
            List<string[]> materialNames = databaseOperation.SelectData("Material", null, new string[] { "MaterialId" }, "MaterialName = @MATERIALNAME", new List<string[]>() { new string[] { "@MATERIALNAME", materialName } }, "MaterialId ASC");

            List<string[]> list = databaseOperation.SelectData("MaterialRecipe", null, new string[] { "RecipeName" }, "MaterialId = @MATERIALID", new List<string[]>() { new string[] { "@MATERIALID", materialNames[0][0] } }, "RecipeName ASC");

            foreach (string[] val in list)
            {
                ProductionRecipe.Items.Add(val[0]);
            }

            ProductionRecipe.SelectedIndex = 0;
        }

        /// <summary>
        /// レシピが選択された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecipiListSelected(object sender, EventArgs e)
        {
            materialRecipe.RecipeName = ProductionRecipe.SelectedItem.ToString();

            List<string[]> recipeData = databaseOperation.SelectData("MaterialRecipe", null, new string[] { "RecipeId", "MaterialId", "RequiredFacilityType", "UnitProductionTime", "UnitProductionValue" }, "RecipeName = @RECIPENAME", new List<string[]>() { new string[] { "@RECIPENAME", materialRecipe.RecipeName } }, "RecipeName ASC");

            string[] recipe = recipeData[0];

            // 取り出したデータを格納
            materialRecipe.RecipeId = recipe[0];
            materialRecipe.ProductMaterialId = recipe[1];
            materialRecipe.RequiredFacilityId = recipe[2];
            materialRecipe.UnitProductionTime = int.Parse(recipe[3]);
            materialRecipe.UnitProductionValue = int.Parse(recipe[4]);

            List<string[]> productName = databaseOperation.SelectData("Material", null, new string[] { "MaterialName" }, "MaterialId = @MATERIALID", new List<string[]>() { new string[] { "@MATERIALID", materialRecipe.ProductMaterialId } }, "MaterialId ASC");
            materialRecipe.ProductMaterialName = productName[0][0];

            // 施設リストを取得して入れる
            UseFacility.Items.Clear();
            List<string[]> facilityList = databaseOperation.SelectData("Facility", null, new string[] { "FacilityName" }, "FacilityTypeId = @FacilityTypeId", new List<string[]>() { new string[] { "@FacilityTypeId", materialRecipe.RequiredFacilityId } }, "FacilityName ASC");
            foreach (string[] val in facilityList)
            {
                UseFacility.Items.Add(val[0]);
            }

            UseFacility.SelectedIndex = 0;

            // 施設の検索
            List<string[]> list = databaseOperation.SelectData("Facility", null, new string[] { "FacilityId", "FacilityName", "RequiredPower", "Efficiency" }, "FacilityName = @FACILITYNAME", new List<string[]>() { new string[] { "@FACILITYNAME", UseFacility.SelectedValue.ToString() } }, "FacilityName ASC");

            string[] facilityData = list[0];
            materialRecipe.UseFacilityId = facilityData[0];
            materialRecipe.UseFacility = facilityData[1];
            materialRecipe.FacirilyPowerUsage = int.Parse(facilityData[2]);
            materialRecipe.FacilityEfficiency = int.Parse(facilityData[3]);

            // レシピデータ書き換え
            UnitProductionTime.Text = materialRecipe.UnitProductionTime.ToString();
            UnitProductionValue.Text = materialRecipe.UnitProductionValue.ToString();
            ProductionValuePerMinute.Text = (60.0 / materialRecipe.UnitProductionTime * materialRecipe.UnitProductionValue).ToString();
        }

        /// <summary>
        /// 生産数が変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductValueChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (double.TryParse(textBox.Text, out double productValue))
            {
                if (productValue < 0)
                {
                    productValue = 0;
                    materialRecipe.ProductMaterialValue = productValue;
                }
                else if (productValue > 1000000)
                {
                    productValue = 1000000;
                    textBox.Text = productValue.ToString();
                    materialRecipe.ProductMaterialValue = productValue;
                }
                else
                {
                    textBox.Text = productValue.ToString();
                    materialRecipe.ProductMaterialValue = Math.Round(productValue, 2);
                }
            }
            else
            {
                productValue = 0;
                textBox.Text = productValue.ToString();
                materialRecipe.ProductMaterialValue = productValue;
            }

            materialRecipe.ProductMaterialValue = productValue;

            double requestFacilityValue = Math.Round(productValue / (60.0 / materialRecipe.UnitProductionTime * materialRecipe.UnitProductionValue), 2);
            materialRecipe.FacilityValue = requestFacilityValue;
            RequestFacilityValue.Text = requestFacilityValue.ToString();
        }

        /// <summary>
        /// 使用施設が変更された
        /// </summary>
        private void FacilitySelected(object sender, EventArgs e)
        {
            if(UseFacility.Items.Count > 0)
            {
                string selected = UseFacility.SelectedItem.ToString();

                // 施設の検索
                List<string[]> list = databaseOperation.SelectData("Facility", null, new string[] { "FacilityId", "FacilityName", "RequiredPower", "Efficiency" }, "FacilityName = @FACILITYNAME", new List<string[]>() { new string[] { "@FACILITYNAME", selected } }, "FacilityName ASC");

                string[] facilityData = list[0];
                materialRecipe.UseFacilityId = facilityData[0];
                materialRecipe.UseFacility = facilityData[1];
                materialRecipe.FacirilyPowerUsage = int.Parse(facilityData[2]);
                materialRecipe.FacilityEfficiency = int.Parse(facilityData[3]);

                EfficiencyValueChanged();
            }
        }

        /// <summary>
        /// 確定ボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButtonClicked(object sender, EventArgs e)
        {
            if(materialRecipe.RecipeId == null)
            {
                return;
            }

            if(RequestProductValue.Text == "")
            {
                return;
            }

            // 必要な素材と数を取得
            List<string[]> list = databaseOperation.SelectData("RequiredMaterialOfUnitProduction RMoUP", "LEFT JOIN Material M ON RMoUP.RequiredMaterialId = M.MaterialId", new string[] { "MaterialName", "RequiredMaterialValue" }, "RMoUP.RecipeId = @RECIPEID", new List<string[]>() { new string[] { "@RECIPEID", materialRecipe.RecipeId } }, "RMoUP.RecipeRequiredMaterialId ASC");

            List<string> materialName = new List<string>();
            List<int> materialValue = new List<int>();

            foreach(string[] materials in list)
            {
                materialName.Add(materials[0]);
                materialValue.Add(int.Parse(materials[1]));
            }

            materialRecipe.RequestMaterialNames = materialName;
            materialRecipe.RequestMaterialValues = materialValue;

            dataOk = true;

            this.Close();
        }


        /// <summary>
        /// キャンセルボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();

            Console.WriteLine("DEBUG:" + dataOk);
        }

        /// <summary>
        /// ウィンドウを開く（レシピ指定無し）
        /// </summary>
        /// <returns>レシピデータ</returns>
        static public MaterialRecipe ShowWindow()
        {
            dataOk = false;

            RecipeSelect recipeSelect = new RecipeSelect();
            recipeSelect.ShowDialog();


            if (dataOk)
            {
                return materialRecipe;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ウィンドウを開く（レシピと個数指定あり）
        /// </summary>
        /// <param name="materialName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static public MaterialRecipe ShowWindow(string materialName, double value)
        {
            dataOk = false;

            RecipeSelect recipeSelect = new RecipeSelect(materialName, value);
            recipeSelect.ShowDialog();

            if (dataOk)
            {
                Console.WriteLine("DEBUG:" + dataOk);
                return materialRecipe;
            }
            else
            {
                Console.WriteLine("DEBUG:" + dataOk);
                return null;
            }
        }
    }
}

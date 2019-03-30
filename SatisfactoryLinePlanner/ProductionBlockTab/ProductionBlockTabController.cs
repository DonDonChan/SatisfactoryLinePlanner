using SatisfactoryLinePlanner.Beans;
using SatisfactoryLinePlanner.ListItems;
using SatisfactoryLinePlanner.RecipeSelect;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SatisfactoryLinePlanner.ProductionBlockTab
{
    class ProductionBlockTabController
    {
        private List<ProductionRecipe> productionRecipes;   // 生産レシピのリスト

        private ProductionBlockTab productionBlockTab; // 生産ブロックタブ（ユーザコントロール）

        private Window parentWindow;    // 親ウィンドウ

        /// <summary>
        /// 生産ブロックタブ（ユーザコントロール）
        /// </summary>
        public ProductionBlockTab BlockTab { get { return productionBlockTab; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProductionBlockTabController(Window parent)
        {
            // 親ウィンドウの設定
            parentWindow = parent;

            // 生産ブロックタブの生成
            productionBlockTab = new ProductionBlockTab();

            // レシピリストの初期化
            productionRecipes = new List<ProductionRecipe>();

            // イベントの設定
            productionBlockTab.RegisterEventOnButton_AddRecipes(new RoutedEventHandler(AddRecipeButtonClicked));
            productionBlockTab.RegisterEventOnButton_ClearRecipes(new RoutedEventHandler(ClearRecipesButtonClicked));
        }

        /// <summary>
        /// レシピ追加ボタン
        /// </summary>
        private void AddRecipeButtonClicked(object sender, EventArgs e)
        {
            RecipeSelectWindowController recipeSelectController = new RecipeSelectWindowController();
            recipeSelectController.ShowRecipeSelectWindow(parentWindow);

            ProductionRecipe productionRecipe = recipeSelectController.ProductionRecipe;

            if (productionRecipe != null)
            {
                AddRecipe(productionRecipe);
            }

            recipeSelectController = null;
        }

        /// <summary>
        /// レシピ全消去ボタン
        /// </summary>
        private void ClearRecipesButtonClicked(object sender, EventArgs e)
        {
            ClearRecipeList();
        }

        /// <summary>
        /// レシピの追加
        /// </summary>
        /// <param name="newRecipe">追加するレシピ</param>
        public void AddRecipe(ProductionRecipe newRecipe)
        {
            // リストになければ追加（これ多分いらない）
            if (!productionRecipes.Contains(newRecipe))
            {
                productionRecipes.Add(newRecipe);
            }

            RefreshListPanels(); // リストの再表示
        }

        /// <summary>
        /// レシピの順序入れ替え
        /// </summary>
        /// <param name="position">入れ替え対象の位置</param>
        /// <param name="direction">入れ替える方向（-1:上, 1:下）</param>
        private void ChangeRecipeListOrder(int position, int direction)
        {
            // 入れ替える前にレシピ内に保持してある順番を入れ替えておく
            productionRecipes[position].Position = position - direction;
            productionRecipes[position + direction].Position = position;

            // 入れ替える（0番目と最後はボタンを無効化しているのでぬるぽはしないはず…）
            ProductionRecipe swap = productionRecipes[position + direction];
            productionRecipes[position + direction] = productionRecipes[position];
            productionRecipes[position] = swap;

            RefreshListPanels(); // リストの再表示
        }

        /// <summary>
        /// レシピの削除（指定位置）
        /// </summary>
        /// <param name="position">削除位置</param>
        private void RemoveRecipe(int position)
        {
            productionRecipes.RemoveAt(position);

            RefreshListPanels(); // リストの再表示
        }

        /// <summary>
        /// レシピの全削除
        /// </summary>
        public void ClearRecipeList()
        {
            productionRecipes.Clear();

            RefreshListPanels(); // リストの再表示
        }

        /// <summary>
        /// リストの再表示
        /// </summary>
        private void RefreshListPanels()
        {
            productionBlockTab.ClearListPanels(); // リストの表示クリア

            //productMaterials.Clear();
            //requestMaterials.Clear();

            productionBlockTab.AddRecipesToRecipeListPanel(CreateRecipeListItems());

            CreateProductionAndRequiredList();
        }

        /// <summary>
        /// 生産レシピ表示用コントロールのリストをつくる
        /// </summary>
        /// <returns></returns>
        private List<ProductionRecipeListItem> CreateRecipeListItems()
        {
            List<ProductionRecipeListItem> items = new List<ProductionRecipeListItem>();

            foreach (ProductionRecipe recipe in productionRecipes)
            {
                recipe.Position = items.Count; // このレシピの位置を保持

                ProductionRecipeListItem item = new ProductionRecipeListItem();

                // コントロールへの設定
                item.TextBlock_Recipe.Text = recipe.RecipeName;
                item.TextBlock_PowerUsage.Text = recipe.TotalPowerUsage.ToString();
                item.TextBlock_Building.Text = recipe.BuildingToUse.Name;
                item.TextBlock_NumOfBuilding.Text = recipe.BuildingToUse.NumberOfBuildings.ToString();
                item.TextBlock_Overclock.Text = recipe.BuildingToUse.OverclockingNumber.ToString();
                item.TextBlock_ProductMaterial.Text = recipe.ProductionMaterial.Name;
                item.TextBlock_NumOfProductMaterial.Text = recipe.NumberOfMaterialProduction.ToString();

                TextBlock[] materialNames = {
                    item.TextBlock_RequestMaterial1,
                    item.TextBlock_RequestMaterial2,
                    item.TextBlock_RequestMaterial3,
                    item.TextBlock_RequestMaterial4
                };

                TextBlock[] requiredNumbers = {
                    item.TextBlock_NumOfRequestMaterial1,
                    item.TextBlock_NumOfRequestMaterial2,
                    item.TextBlock_NumOfRequestMaterial3,
                    item.TextBlock_NumOfRequestMaterial4
                };

                int i = 0;
                foreach (RequiredMaterial requiredMaterial in recipe.RequiredMaterials)
                {
                    materialNames[i].Text = requiredMaterial.Name;
                    requiredNumbers[i].Text = recipe.ActualRequiredNumberOfMaterialsPerMin[i].ToString();
                    i++;
                }

                // ボタンの一部無効化
                if (items.Count == 0)
                {
                    item.Button_UpItem.IsEnabled = false;
                }
                if (items.Count == productionRecipes.Count - 1)
                {
                    item.Button_DownItem.IsEnabled = false;
                }

                // ボタンイベントの登録
                void UpItemButtonClicked(object sender, EventArgs e)
                {
                    ChangeRecipeListOrder(recipe.Position, -1);
                }
                item.RegisterEventOnButton_UpItem(UpItemButtonClicked);

                void DownItemButtonClicked(object sender, EventArgs e)
                {
                    ChangeRecipeListOrder(recipe.Position, 1);
                }
                item.RegisterEventOnButton_DownItem(DownItemButtonClicked);

                void DeleteItemButtonClicked(object sender, EventArgs e)
                {
                    RemoveRecipe(recipe.Position);
                }
                item.RegisterEventOnButton_DeleteItem(DeleteItemButtonClicked);

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 生産物と要求物のリストを生成して表示まで
        /// </summary>
        private void CreateProductionAndRequiredList()
        {
            List<string[]> materialList = new List<string[]>();

            // レシピのリストを逆転したものを用意
            List<ProductionRecipe> reversed = new List<ProductionRecipe>(productionRecipes);
            reversed.Reverse();

            foreach(ProductionRecipe recipe in reversed)
            {
                int i = 0;
                // 要求素材から検索して同名があれば数値だけ追加、なければ名前と追加
                foreach (RequiredMaterial requiredMaterial in recipe.RequiredMaterials)
                {
                    bool flag = false;
                    foreach (string[] material in materialList)
                    {
                        if (material[0].Equals(requiredMaterial.Name) && !material[1].Equals("0"))
                        {
                            flag = true;
                            material[1] = Math.Round(double.Parse(material[1]) - recipe.ActualRequiredNumberOfMaterialsPerMin[i], 2).ToString();
                        }
                    }

                    if (!flag)
                    {
                        materialList.Add(new string[] { requiredMaterial.Name, (-recipe.ActualRequiredNumberOfMaterialsPerMin[i]).ToString() });
                    }
                    i++;
                }

                i = 0;
                bool exist = false;
                foreach(string[] production in materialList)
                {
                    if (production[0].Equals(recipe.ProductionMaterial.Name) && double.TryParse(production[1], out double pro) && pro > 0)
                    {
                        exist = true;
                        break;
                    }
                    i++;
                }

                if (exist)
                {
                    Console.WriteLine("Name:" + materialList[i][0]);
                    Console.WriteLine("Number:" + materialList[i][1]);
                    Console.WriteLine("Production:" + recipe.NumberOfMaterialProduction);
                    Console.WriteLine("Result:" + double.Parse(materialList[i][1]) + recipe.NumberOfMaterialProduction);
                    materialList[i][1] = (double.Parse(materialList[i][1]) + recipe.NumberOfMaterialProduction).ToString();
                }
                else
                {
                    materialList.Add(new string[] { recipe.ProductionMaterial.Name, recipe.NumberOfMaterialProduction.ToString() });
                }
            }

            // 表示用のリストとか作成
            List<MaterialWithNumOnButton> productionList = new List<MaterialWithNumOnButton>();
            List<MaterialWithNumOnButton> requiredList = new List<MaterialWithNumOnButton>();

            foreach(string[] value in materialList)
            {
                if(double.Parse(value[1]) < 0)
                {
                    MaterialWithNumOnButton material = new MaterialWithNumOnButton();

                    // 名前と数値の設定
                    material.TextBlock_Name.Text = value[0];
                    material.TextBlock_Number.Text = (-(double.Parse(value[1]))).ToString();

                    // ボタンイベントの追加
                    void RequiredListClicked (object sender, EventArgs e)
                    {
                        RecipeSelectWindowController recipeSelectController = new RecipeSelectWindowController();
                        recipeSelectController.ShowRecipeSelectWindow(value[0], -(double.Parse(value[1])), parentWindow);

                        ProductionRecipe productionRecipe = recipeSelectController.ProductionRecipe;

                        if (productionRecipe != null)
                        {
                            AddRecipe(productionRecipe);
                        }

                        recipeSelectController = null;
                    }

                    material.Button_RequestRecipeSelect.Click += RequiredListClicked;

                    requiredList.Add(material);
                }
                else if (double.Parse(value[1]) > 0)
                {
                    MaterialWithNumOnButton material = new MaterialWithNumOnButton();

                    // 名前と数値の設定
                    material.TextBlock_Name.Text = value[0];
                    material.TextBlock_Number.Text = (double.Parse(value[1])).ToString();
                    productionList.Add(material);
                }
            }

            // 表示させる
            productionBlockTab.AddMaterialsToProductionListPanel(productionList);
            productionBlockTab.AddMaterialsToRequiredListPanel(requiredList);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SatisfactoryLinePlanner
{
    /// <summary>
    /// ProductionTab.xaml の相互作用ロジック
    /// </summary>
    public partial class ProductionTab : UserControl
    {
        private int counter = 0;
        private List<MaterialRecipe> materialRecipeList = new List<MaterialRecipe>();

        private Dictionary<string, double> productMaterials = new Dictionary<string, double>();
        private Dictionary<string, double> requestMaterials = new Dictionary<string, double>();

        public ProductionTab()
        {
            InitializeComponent();

            Button addRecipeButton = AddRcipe;
            addRecipeButton.Click += new RoutedEventHandler(AddButtonClicked);

            Button clearRecipeButton = ClearAll;
            clearRecipeButton.Click += new RoutedEventHandler(ClearAllButtonClick);
        }

        /// <summary>
        /// レシピ追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButtonClicked(object sender, EventArgs e)
        {
            MaterialRecipe materialRecipe = RecipeSelect.ShowWindow();

            if (materialRecipe != null)
            {
                AddRecipe(materialRecipe);
            }
        }

        /// <summary>
        /// レシピ全消去ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAllButtonClick(object sender, EventArgs e)
        {
            ClearList();
        }

        /// <summary>
        /// レシピリストに変更があった
        /// </summary>
        public void RecipeListChanged()
        {
            // 生産物の方
            foreach (MaterialRecipe recipe in materialRecipeList)
            {
                string materialName = recipe.ProductMaterialName;
                if (productMaterials.ContainsKey(materialName))
                {
                    productMaterials[materialName] += recipe.ProductMaterialValue;
                }
                else
                {
                    productMaterials.Add(materialName, recipe.ProductMaterialValue);
                }
            }

            // 要求物の方
            foreach (MaterialRecipe recipe in materialRecipeList)
            {
                List<string> materialNames = recipe.RequestMaterialNames;
                List<int> materialValues = recipe.RequestMaterialValues;

                double upt = Math.Round((recipe.UnitProductionTime / (recipe.FacilityEfficiency / 100.0)) / (recipe.OverClockValue / 100.0), 2);
                double minutePer = Math.Round((60 / upt), 2);

                for (int i = 0; i < materialNames.Count; i++)
                {
                    if (requestMaterials.ContainsKey(materialNames[i]))
                    {
                        requestMaterials[materialNames[i]] += Math.Round((minutePer * materialValues[i]) * Math.Round(recipe.ProductMaterialValue / minutePer, 2), 1);
                    }
                    else
                    {
                        requestMaterials.Add(materialNames[i], Math.Round((minutePer * materialValues[i]) * Math.Round(recipe.ProductMaterialValue / minutePer, 2), 1));
                    }
                }
            }

            Dictionary<string, double> productSwapList = new Dictionary<string, double>();
            Dictionary<string, double> requestSwapList = new Dictionary<string, double>();

            // 生産と要求の比較
            foreach (KeyValuePair<string, double> comp in productMaterials)
            {
                if (requestMaterials.ContainsKey(comp.Key))
                {
                    if(requestMaterials[comp.Key] - comp.Value > 0)
                    {
                        requestSwapList.Add(comp.Key, Math.Round(requestMaterials[comp.Key] - comp.Value, 1));
                    }
                    else if(comp.Value - requestMaterials[comp.Key] != 0)
                    {
                        productSwapList.Add(comp.Key, Math.Round(comp.Value - requestMaterials[comp.Key], 1));
                    }
                }
                else
                {
                    productSwapList.Add(comp.Key, comp.Value);
                }
            }

            foreach (KeyValuePair<string, double> comp in requestMaterials)
            {
                if (!productMaterials.ContainsKey(comp.Key))
                {
                    requestSwapList.Add(comp.Key, comp.Value);
                }
            }

            productMaterials = productSwapList;
            requestMaterials = requestSwapList;

            // 表示
            foreach (KeyValuePair<string, double> data in productMaterials)
            {
                ProductionMaterialListItem product = new ProductionMaterialListItem();
                product.TextBlockMaterialName.Text = data.Key;
                product.TextBlockMaterialValue.Text = data.Value.ToString();

                ProductionOutListPanel.Children.Add(product);
            }

            foreach (KeyValuePair<string, double> data in requestMaterials)
            {
                RequestMaterialListItem request = new RequestMaterialListItem();
                request.TextBlockMaterialName.Text = data.Key;
                request.TextBlockMaterialValue.Text = data.Value.ToString();

                void routedEventHandler(object sender, RoutedEventArgs e)
                {
                    MaterialRecipe materialRecipe = RecipeSelect.ShowWindow(data.Key, data.Value);

                    if (materialRecipe != null)
                    {
                        AddRecipe(materialRecipe);
                    }
                }

                request.RequestRecipeSelect.Click += routedEventHandler;

                ProductionInListPanel.Children.Add(request);
            }
        }

        /// <summary>
        /// レシピの追加
        /// </summary>
        /// <param name="materialRecipe">追加するレシピ</param>
        public void AddRecipe(MaterialRecipe materialRecipe)
        {
            if (!materialRecipeList.Contains(materialRecipe))
            {
                materialRecipeList.Add(materialRecipe);
            }

            RefreshList();
        }

        /// <summary>
        /// レシピの削除（指定位置）
        /// </summary>
        /// <param name="position">削除位置</param>
        private void RemoveRecipe(int position)
        {
            materialRecipeList.RemoveAt(position);

            RefreshList();
        }

        /// <summary>
        /// リストの再表示
        /// </summary>
        private void RefreshList()
        {
            ClearView();

            foreach (MaterialRecipe materialRecipe in materialRecipeList)
            {
                AddRecipeView(materialRecipe);
            }

            RecipeListChanged();
        }

        /// <summary>
        /// リストの表示を消すだけ
        /// </summary>
        private void ClearView()
        {
            counter = 0;

            productMaterials.Clear();
            requestMaterials.Clear();

            ProductionOutListPanel.Children.Clear();
            ProductionInListPanel.Children.Clear();
            ProductionRecipeListPanel.Children.Clear();
        }

        /// <summary>
        /// リストの全削除
        /// </summary>
        public void ClearList()
        {
            ClearView();
            materialRecipeList.Clear();
        }

        /// <summary>
        /// レシピをレシピリストに表示させるやつ
        /// </summary>
        /// <param name="materialRecipe">レシピクラスのやつ</param>
        private void AddRecipeView(MaterialRecipe materialRecipe)
        {
            ProductionRecipeListItem productionRecipeListItem = new ProductionRecipeListItem();

            productionRecipeListItem.Position.Text = counter.ToString();

            productionRecipeListItem.RecipeName.Text = materialRecipe.RecipeName;
            productionRecipeListItem.FacilityName.Text = materialRecipe.UseFacility;
            productionRecipeListItem.FacilityValue.Text = materialRecipe.FacilityValue.ToString();
            productionRecipeListItem.OverCloack.Text = materialRecipe.OverClockValue.ToString();
            productionRecipeListItem.Power.Text = (materialRecipe.FacirilyPowerUsage * materialRecipe.FacilityValue * (materialRecipe.OverClockValue / 100)).ToString();
            productionRecipeListItem.ProductMaterialName.Text = materialRecipe.ProductMaterialName;
            productionRecipeListItem.ProductMaterialValue.Text = materialRecipe.ProductMaterialValue.ToString();
            //productionRecipeListItem.ProductMaterialValue.Text = ((60 / materialRecipe.UnitProductionTime) * materialRecipe.UnitProductionValue * materialRecipe.FacilityValue).ToString();

            TextBlock[] materialNames =
            {
                    productionRecipeListItem.RequestMaterialName1,
                    productionRecipeListItem.RequestMaterialName2,
                    productionRecipeListItem.RequestMaterialName3,
                    productionRecipeListItem.RequestMaterialName4
                };

            int i = 0;
            foreach (String material in materialRecipe.RequestMaterialNames)
            {
                materialNames[i].Text = material;
                i++;
            }

            TextBlock[] materialValues =
            {
                    productionRecipeListItem.RequestMaterialValue1,
                    productionRecipeListItem.RequestMaterialValue2,
                    productionRecipeListItem.RequestMaterialValue3,
                    productionRecipeListItem.RequestMaterialValue4
                };

            i = 0;
            double upt = Math.Round((materialRecipe.UnitProductionTime / (materialRecipe.FacilityEfficiency / 100.0)) / (materialRecipe.OverClockValue / 100.0), 2);
            foreach (int value in materialRecipe.RequestMaterialValues)
            {
                double minutePer = Math.Round((60 / upt), 2);
                materialValues[i].Text = Math.Round((minutePer * value) * Math.Round(materialRecipe.ProductMaterialValue / minutePer, 2), 1).ToString();
                i++;
            }

            if (counter == 0)
            {
                productionRecipeListItem.RecipeListItemUp.IsEnabled = false;
            }

            if (counter + 1 == materialRecipeList.Count)
            {
                productionRecipeListItem.RecipeListItemDown.IsEnabled = false;
            }

            productionRecipeListItem.RecipeListItemUp.Click += new RoutedEventHandler(UpButtonClick);
            productionRecipeListItem.RecipeListItemDown.Click += new RoutedEventHandler(DownButtonClick);
            productionRecipeListItem.RecipeListItemDelete.Click += new RoutedEventHandler(DeleteButtonClick);

            this.ProductionRecipeListPanel.Children.Add(productionRecipeListItem);

            counter++;
        }

        /// <summary>
        /// 移動ボタン（↑）
        /// </summary>
        /// <param name="sender">押したボタン</param>
        /// <param name="e">イベント</param>
        private void UpButtonClick(object sender, EventArgs e)
        {
            int positon = this.GetPosition(sender);

            // リストの入れ替え
            MaterialRecipe swap = materialRecipeList[positon - 1];

            materialRecipeList[positon - 1] = materialRecipeList[positon];
            materialRecipeList[positon] = swap;

            RefreshList();
        }

        /// <summary>
        /// 移動ボタン（↓）
        /// </summary>
        /// <param name="sender">押したボタン</param>
        /// <param name="e">イベント</param>
        private void DownButtonClick(object sender, EventArgs e)
        {
            int positon = this.GetPosition(sender);

            // リストの入れ替え
            MaterialRecipe swap = materialRecipeList[positon + 1];
            materialRecipeList[positon + 1] = materialRecipeList[positon];
            materialRecipeList[positon] = swap;

            RefreshList();
        }

        /// <summary>
        /// 消去ボタン
        /// </summary>
        /// <param name="sender">押したボタン</param>
        /// <param name="e">イベント</param>
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            int positon = this.GetPosition(sender);
            if (positon > -1)
            {
                RemoveRecipe(positon);
            }
        }

        /// <summary>
        /// レシピの表示位置を取得するやつ
        /// </summary>
        /// <param name="sender">押したボタンそのままこっちに</param>
        /// <returns></returns>
        private int GetPosition(object sender)
        {
            Button button = (Button)sender;
            Grid parentGrid1 = (Grid)button.Parent;
            Grid parentGrid2 = (Grid)parentGrid1.Parent;
            foreach (UIElement element in parentGrid2.Children)
            {
                if (element is TextBlock textBlock)
                {
                    return int.Parse(textBlock.Text);
                }
            }

            return -1;
        }
    }
}

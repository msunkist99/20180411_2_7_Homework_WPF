using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using _20180411_2_7_Homework_WPF.Menus;

namespace _20180411_2_7_Homework_WPF
{
    public partial class MainWindow 
    {
        private Menus.Menus breakfastMenu;
        private Menus.Menus lunchMenu;
        private Menus.Menus dinnerMenu;
        private Menus.Menus selectedMenus;

        private bool twentyOneOrOver;
        private bool twentyOneOrOverChecked;

        private int menuSpecialIndex;

        private string mealSelection;

        private double tipPercentage = Convert.ToDouble(ConfigurationManager.AppSettings["TipPercentage"]);
        private double dailySpecialDiscount = Convert.ToDouble(ConfigurationManager.AppSettings["DailySpecialDiscount"]);

        public MainWindow()
        {
            InitializeComponent();

            breakfastMenu = new Menus.Menus(Menus.MealChoice.Breakfast);
            lunchMenu = new Menus.Menus(Menus.MealChoice.Lunch);
            dinnerMenu = new Menus.Menus(Menus.MealChoice.Dinner);

            int legalDrinkingAge = Convert.ToInt32(ConfigurationManager.AppSettings["LegalDrinkingAge"]);
            
            todaysDateTextBlock.Text = DateTime.Now.ToShortDateString();
            bornByDateTextBlock.Text = DateTime.Now.AddYears(legalDrinkingAge * -1).ToShortDateString();
        }

        private void seatSelection_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;

            switch (button.Name)
            {
                case "boothRadioButton":
                    break;

                case "tableRadioButton":
                    break;

                default:
                    break;
            }
        }

        private void diningSelection_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;

            menuListBox.Items.Clear();
            drinkListBox.Items.Clear();

            Random random = new Random();

            switch (button.Name)
            {
                case "breakfastRadioButton":
                    foreach (var menuItem in breakfastMenu.MealMenu)
                    {
                        menuListBox.Items.Add(menuItem.itemName + "\r\n" + menuItem.itemCost.ToString("C2"));
                    }

                    menuSpecialIndex = random.Next(0, breakfastMenu.MealMenu.Count);
                    dailySpecialTextblock.Text = String.Format("Today's special is -\r\n  {0} \r\n {1}", breakfastMenu.MealMenu.ElementAt(menuSpecialIndex).itemName, (breakfastMenu.MealMenu.ElementAt(menuSpecialIndex).itemCost * (1 - dailySpecialDiscount)).ToString("C2"));

                    foreach (var menuItem in breakfastMenu.DrinkMenu)
                    {
                        drinkListBox.Items.Add(menuItem.itemName + menuItem.itemRestriction + "\r\n" + menuItem.itemCost.ToString("C2"));
                    }

                    selectedMenus = breakfastMenu;
                    break;

                case "lunchRadioButton":
                    foreach (var menuItem in lunchMenu.MealMenu)
                    {
                        menuListBox.Items.Add(menuItem.itemName + "\r\n" + menuItem.itemCost.ToString("C2"));
                    }

                    menuSpecialIndex = random.Next(0, lunchMenu.MealMenu.Count());
                    dailySpecialTextblock.Text = String.Format("Today's special is -\r\n  {0} \r\n {1}", lunchMenu.MealMenu.ElementAt(menuSpecialIndex).itemName, (lunchMenu.MealMenu.ElementAt(menuSpecialIndex).itemCost * .9).ToString("C2"));

                    foreach (var menuItem in lunchMenu.DrinkMenu)
                    {
                        drinkListBox.Items.Add(menuItem.itemName + menuItem.itemRestriction + "\r\n" + menuItem.itemCost.ToString("C2"));
                    }

                    selectedMenus = lunchMenu;
                    break;

                case "dinnerRadioButton":
                    foreach (var menuItem in dinnerMenu.MealMenu)
                    {
                        menuListBox.Items.Add(menuItem.itemName + "\r\n" + menuItem.itemCost.ToString("C2"));
                    }

                    menuSpecialIndex = random.Next(0, dinnerMenu.MealMenu.Count());
                    dailySpecialTextblock.Text = String.Format("Today's special is -\r\n  {0} \r\n {1}", dinnerMenu.MealMenu.ElementAt(menuSpecialIndex).itemName, (dinnerMenu.MealMenu.ElementAt(menuSpecialIndex).itemCost * .9).ToString("C2"));

                    foreach (var menuItem in dinnerMenu.DrinkMenu)
                    {
                        drinkListBox.Items.Add(menuItem.itemName + menuItem.itemRestriction + "\r\n" + menuItem.itemCost.ToString("C2"));
                    }

                    selectedMenus = dinnerMenu;
                    break;
            }

            menuHeadingTextBlock.Text = String.Format("Your {0} selections - ", button.Content);
            mealSelection = button.Content.ToString();

            ageRetrictionTextBlock.Text = "   * - contains alcohol - age retrictions apply";

        }

        private void drinkListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (drinkListBox.SelectedItems.Count > 0)
            {
                foreach (object obj in drinkListBox.SelectedItems)
                {
                    if (selectedMenus.DrinkMenu.ElementAt(drinkListBox.Items.IndexOf(obj)).itemRestriction == "*")
                    {
                        birthdateStackPanel.Visibility = System.Windows.Visibility.Visible;
                        if (twentyOneOrOverChecked == false)
                        {
                            inputBirthdateTextBox.Focus();
                            enterBirthdateTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                        }
                        else
                        {
                            enterBirthdateTextBlock.Foreground = System.Windows.Media.Brushes.Black;
                        }
                    }
                }

                double cost = 0;
                foreach (object obj in drinkListBox.SelectedItems)
                {
                    if (selectedMenus.DrinkMenu.ElementAt(drinkListBox.Items.IndexOf(obj)).itemRestriction == "*")
                    {
                        if (twentyOneOrOver == true)
                        {
                            cost += selectedMenus.DrinkMenu.ElementAt(drinkListBox.Items.IndexOf(obj)).itemCost;
                        }
                    }
                    else
                    {
                        cost += selectedMenus.DrinkMenu.ElementAt(drinkListBox.Items.IndexOf(obj)).itemCost;
                    }
                }

                double tip = cost * tipPercentage;
                double total = cost + tip;

                checkoutDrinkTextblock.Text = String.Format("Your {0} Beverage Check - ", mealSelection);
                checkoutDrinkCostTextBlock.Text = String.Format("Beverage Cost - {0}", cost.ToString("C2"));
                checkoutDrinkTipTextBlock.Text = String.Format("Suggest Tip - {0}", tip.ToString("C2"));
                checkoutDrinkTotalCostTextBlock.Text = String.Format("Beverage Total - {0}", total.ToString("C2"));
            }
            else
            {
                checkoutDrinkCostTextBlock.Text = "";
                checkoutDrinkTipTextBlock.Text = "";
                checkoutDrinkTotalCostTextBlock.Text = "";
            }
        }

        private void menuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (menuListBox.SelectedItems.Count > 0)
            {
                double cost = 0;
                foreach (object obj in menuListBox.SelectedItems)
                {
                    if (menuListBox.Items.IndexOf(obj) == menuSpecialIndex)
                    {
                        cost += selectedMenus.MealMenu.ElementAt(menuListBox.Items.IndexOf(obj)).itemCost * (1 - dailySpecialDiscount);
                    }
                    else
                    {
                        cost += selectedMenus.MealMenu.ElementAt(menuListBox.Items.IndexOf(obj)).itemCost;
                    }
                }

                double tip = cost * .2;
                double total = cost + tip;

                checkoutMealTextblock.Text = String.Format("Your {0} Meal Check - ", mealSelection);
                checkoutMealCostTextBlock.Text = String.Format("Meal Cost - {0}", cost.ToString("C2"));
                checkoutMealTipTextBlock.Text = String.Format("Suggest Tip - {0}", tip.ToString("C2"));
                checkoutMealTotalCostTextBlock.Text = String.Format("Meal Total - {0}", total.ToString("C2"));
            }
            else
            {
                checkoutMealCostTextBlock.Text = "";
                checkoutMealTipTextBlock.Text = "";
                checkoutMealTotalCostTextBlock.Text = "";

            }
        }

        private void inputBirthdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (twentyOneOrOverChecked == false)
            {
                try
                {
                    if (DateTime.Parse(inputBirthdateTextBox.Text) > DateTime.Parse(bornByDateTextBlock.Text))
                    {
                        twentyOneOrOver = false;
                        validationResponseTextBlock.Text = "You are NOT 21 or over";
                        twentyOneOrOverChecked = true;
                    }
                    else
                    {
                        twentyOneOrOver = true;
                        validationResponseTextBlock.Text = "You are 21 or over";
                        twentyOneOrOverChecked = true;
                    }
                }
                catch
                {
                    validationResponseTextBlock.Text = "Invalid format for birthdate";
                    inputBirthdateTextBox.Text = "";
                }
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            twentyOneOrOver = false;
            twentyOneOrOverChecked = false;
            validationResponseTextBlock.Text = "";
            inputBirthdateTextBox.Text = "";
            birthdateStackPanel.Visibility = System.Windows.Visibility.Hidden;

            boothRadioButton.IsChecked = false;
            tableRadioButton.IsChecked = false;
            toGoRadioButton.IsChecked = false;

            breakfastRadioButton.IsChecked = false;
            lunchRadioButton.IsChecked = false;
            dinnerRadioButton.IsChecked = false;

            dailySpecialTextblock.Text = "";

            ageRetrictionTextBlock.Text = "";

            menuListBox.Items.Clear();
            drinkListBox.Items.Clear();

            checkoutMealCostTextBlock.Text = "";
            checkoutMealTipTextBlock.Text = "";
            checkoutMealTotalCostTextBlock.Text = "";
            checkoutDrinkCostTextBlock.Text = "";
            checkoutDrinkTipTextBlock.Text = "";
            checkoutDrinkTotalCostTextBlock.Text = "";
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using calcaot.Models;
using calcaot.ViewModels;

namespace calcaot
{
    public partial class MealDetailWindow : Window
    {
        private readonly MealGroupViewModel _meal;
        private readonly MainViewModel _viewModel;

        public MealDetailWindow(MealGroupViewModel meal, MainViewModel viewModel)
        {
            InitializeComponent();
            _meal = meal;
            _viewModel = viewModel;
            TitleText.Text = meal.Name;
            UpdateSummary();
            FoodsList.ItemsSource = meal.Foods;
        }

        private void UpdateSummary()
        {
            SummaryText.Text = _meal.Summary;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is FoodItem food)
            {
                var result = MessageBox.Show(
                    $"Удалить \"{food.Name}\"?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _meal.RemoveFood(food);
                    _viewModel.RefreshTotals();
                    UpdateSummary();
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is FoodItem food)
            {
                var dialog = new AddFoodWindow(_meal.Name, food) { Owner = this };
                if (dialog.ShowDialog() == true)
                {
                    var newFood = new FoodItem
                    {
                        Id = food.Id,
                        Name = dialog.FoodName,
                        Weight = dialog.Weight,
                        Calories = (dialog.CalPer100 * dialog.Weight) / 100,
                        MealType = _meal.Name
                    };

                    _meal.ReplaceFood(food, newFood);
                    _viewModel.RefreshTotals();
                    UpdateSummary();
                }
            }
        }
    }
}

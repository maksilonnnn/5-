using System.Windows;
using System.Windows.Controls;

namespace CalorieApp
{
    public partial class MealDetailWindow : Window
    {
        private readonly MealGroup _meal;
        private readonly MainViewModel _viewModel;

        public MealDetailWindow(MealGroup meal, MainViewModel viewModel)
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not FoodItem item) return;

            var result = MessageBox.Show(
                $"Удалить «{item.Name}»?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            _meal.RemoveFood(item);
            _viewModel.RefreshTotals();
            UpdateSummary();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is not FoodItem item) return;

            var dialog = new AddFoodWindow(_meal.Name, item) { Owner = this };
            if (dialog.ShowDialog() != true) return;

            item.Name = dialog.FoodName;
            item.Weight = dialog.Weight;
            item.Calories = (dialog.CalPer100 * dialog.Weight) / 100;
            item.MealType = _meal.Name;

            _meal.Refresh();
            _viewModel.RefreshTotals();
            UpdateSummary();
            FoodsList.Items.Refresh();
        }
    }
}
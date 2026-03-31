using System.Windows;
using calcaot.ViewModels;

namespace calcaot
{
    public partial class MealDetailWindow : Window
    {
        private readonly MealGroupViewModel _meal;

        public MealDetailWindow(MealGroupViewModel meal, MainViewModel viewModel)
        {
            InitializeComponent();
            _meal = meal;
            TitleText.Text = meal.Name;
            UpdateSummary();
            FoodsList.ItemsSource = meal.Foods;
        }

        private void UpdateSummary()
        {
            SummaryText.Text = _meal.Summary;
        }
    }
}

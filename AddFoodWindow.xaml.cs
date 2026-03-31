using System.Windows;
using calcaot.Models;

namespace calcaot
{
    public partial class AddFoodWindow : Window
    {
        public string MealName  { get; private set; }
        public string FoodName  { get; private set; } = string.Empty;
        public double Weight    { get; private set; }
        public double CalPer100 { get; private set; }

        public AddFoodWindow(string mealName, FoodItem? existing = null)
        {
            InitializeComponent();
            MealName = mealName;
            TitleText.Text = existing != null ? $"Изменить — {mealName}" : mealName;

            if (existing != null)
            {
                NameInput.Text   = existing.Name;
                WeightInput.Text = existing.Weight.ToString();
                CalInput.Text    = (existing.Calories / existing.Weight * 100).ToString("F0");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameInput.Text))
            {
                MessageBox.Show("Введите название блюда.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!double.TryParse(WeightInput.Text, out double weight) || weight <= 0)
            {
                MessageBox.Show("Введите корректный вес.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!double.TryParse(CalInput.Text, out double cal) || cal < 0)
            {
                MessageBox.Show("Введите корректные калории.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            FoodName  = NameInput.Text.Trim();
            Weight    = weight;
            CalPer100 = cal;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

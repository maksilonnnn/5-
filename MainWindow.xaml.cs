using System;
using System.Windows;
using System.Windows.Controls;

namespace CalorieApp
{
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // Валидация: все поля должны быть заполнены
            if (string.IsNullOrWhiteSpace(NameInput.Text))
            {
                MessageBox.Show("Введите название блюда.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(WeightInput.Text, out double weight) || weight <= 0)
            {
                MessageBox.Show("Введите корректный вес (число больше 0).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(CalInput.Text, out double cal100) || cal100 < 0)
            {
                MessageBox.Show("Введите корректные калории на 100г.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получаем выбранный приём пищи из ComboBox
            string meal = (MealCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Перекус";

            // Добавляем блюдо через ViewModel
            viewModel.AddFood(NameInput.Text.Trim(), weight, cal100, meal);

            // Очищаем поля после добавления
            NameInput.Text = string.Empty;
            WeightInput.Text = string.Empty;
            CalInput.Text = string.Empty;
            MealCombo.SelectedIndex = 0;
        }
    }
}
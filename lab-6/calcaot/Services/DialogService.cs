using System.Windows;
using calcaot.Models;
using calcaot.Interfaces;
using calcaot.ViewModels;

namespace calcaot.Services
{
    /// <summary>
    /// Реализация диалогового сервиса — паттерн Facade (Фасад).
    ///
    /// DialogService является конкретной реализацией фасада.
    /// Он знает о всех деталях создания диалоговых окон:
    ///   • создаёт нужный ViewModel для каждого диалога;
    ///   • создаёт окно и назначает ему владельца (Owner);
    ///   • вызывает ShowDialog() и возвращает результат.
    ///
    /// Вся эта сложность скрыта от вызывающего кода.
    /// MainViewModel и MealGroupViewModel используют только интерфейс
    /// IDialogService, не зная о существовании конкретных классов окон.
    /// </summary>
    public class DialogService : IDialogService
    {
        public FoodItem? ShowAddFoodDialog(string mealName, FoodItem? existingFood)
        {
            var owner = Application.Current?.MainWindow;
            var viewModel = new AddFoodViewModel(mealName, existingFood);

            var dialog = new AddFoodWindow(viewModel) { Owner = owner };
            var result = dialog.ShowDialog();
            return result == true ? viewModel.ResultFood : null;
        }

        public bool ShowDeleteConfirmation(string foodName)
        {
            var owner = Application.Current?.MainWindow;

            var dialog = new DeleteConfirmationWindow(foodName) { Owner = owner };
            var result = dialog.ShowDialog();
            return result == true;
        }

        public void ShowMealDetailDialog(MealGroupViewModel meal)
        {
            var owner = Application.Current?.MainWindow;

            var dialog = new MealDetailWindow(meal) { Owner = owner };
            dialog.ShowDialog();
        }
    }
}

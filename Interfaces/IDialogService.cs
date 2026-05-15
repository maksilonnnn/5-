using calcaot.Models;
using calcaot.ViewModels;

namespace calcaot.Interfaces
{
    /// <summary>
    /// Паттерн Facade (Фасад): интерфейс фасада над диалоговыми окнами приложения.
    ///
    /// В WPF открытие диалога обычно требует:
    ///   • создать ViewModel;
    ///   • создать окно;
    ///   • назначить Owner;
    ///   • вызвать ShowDialog;
    ///   • обработать результат.
    ///
    /// ViewModel-классы не должны знать об этих деталях и ссылаться на View-окна.
    /// Вместо этого они используют IDialogService — единый контракт.
    /// </summary>
    public interface IDialogService
    {
        FoodItem? ShowAddFoodDialog(string mealName, FoodItem? existingFood);
        bool ShowDeleteConfirmation(string foodName);
        void ShowMealDetailDialog(MealGroupViewModel meal);
    }
}

using calcaot.Models;
using calcaot.ViewModels;

namespace calcaot.Services
{
    /// <summary>
    /// Интерфейс диалогового сервиса — паттерн Facade (Фасад).
    ///
    /// Паттерн Facade предоставляет простой интерфейс к сложной подсистеме.
    /// Открытие диалогового окна в WPF требует нескольких шагов:
    /// создать ViewModel, создать окно, назначить владельца, вызвать ShowDialog,
    /// получить результат. ViewModel не должен знать об этих деталях.
    ///
    /// IDialogService скрывает всю эту сложность за тремя простыми методами.
    /// ViewModel вызывает, например, ShowAddFoodDialog() и получает результат —
    /// не зная ни о каких окнах, не держа ссылок на View-классы.
    ///
    /// Это также упрощает тестирование: в тестах можно подставить
    /// фиктивную реализацию IDialogService вместо реальных окон.
    /// </summary>
    public interface IDialogService
    {
        FoodItem? ShowAddFoodDialog(string mealName, FoodItem? existingFood);
        bool ShowDeleteConfirmation(string foodName);
        void ShowMealDetailDialog(MealGroupViewModel meal);
    }
}

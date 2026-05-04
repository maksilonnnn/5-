using System;
using calcaot.Models;

namespace calcaot.ViewModels.Commands
{
    /// <summary>
    /// Команда «Изменить еду» — паттерн Command (Команда), ConcreteCommand.
    ///
    /// Инкапсулирует операцию редактирования продукта.
    /// Хранит как старую версию продукта (_oldItem), так и новую (_newItem).
    ///
    /// Execute() — заменяет старый продукт новым.
    /// Undo()    — заменяет новый продукт обратно старым.
    /// </summary>
    public class EditFoodCommand : IUndoableCommand
    {
        private readonly MealGroupViewModel _mealGroup;
        private readonly FoodItem _oldItem;
        private readonly FoodItem _newItem;

        public EditFoodCommand(MealGroupViewModel mealGroup, FoodItem oldItem, FoodItem newItem)
        {
            _mealGroup = mealGroup ?? throw new ArgumentNullException(nameof(mealGroup));
            _oldItem = oldItem ?? throw new ArgumentNullException(nameof(oldItem));
            _newItem = newItem ?? throw new ArgumentNullException(nameof(newItem));
        }

        public void Execute() => _mealGroup.ReplaceFood(_oldItem, _newItem);

        public void Undo() => _mealGroup.ReplaceFood(_newItem, _oldItem);
    }
}

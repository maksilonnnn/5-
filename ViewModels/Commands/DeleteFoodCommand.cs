using System;
using calcaot.Models;
using calcaot.Interfaces;

namespace calcaot.ViewModels.Commands
{
    /// <summary>
    /// Команда «Удалить еду» — паттерн Command (Команда), ConcreteCommand.
    ///
    /// Инкапсулирует операцию удаления продукта из приёма пищи.
    /// Перед удалением сохраняет индекс элемента в списке (_savedIndex),
    /// чтобы при отмене восстановить продукт на прежнюю позицию.
    ///
    /// Execute() — запоминает позицию и удаляет продукт.
    /// Undo()    — вставляет продукт обратно на сохранённую позицию.
    /// </summary>
    public class DeleteFoodCommand : IUndoableCommand
    {
        private readonly MealGroupViewModel _mealGroup;
        private readonly FoodItem _item;
        private int _savedIndex;

        public DeleteFoodCommand(MealGroupViewModel mealGroup, FoodItem item)
        {
            _mealGroup = mealGroup ?? throw new ArgumentNullException(nameof(mealGroup));
            _item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public void Execute()
        {
            _savedIndex = _mealGroup.Foods.IndexOf(_item);
            _mealGroup.RemoveFood(_item);
        }

        public void Undo()
        {
            _mealGroup.InsertFood(_savedIndex, _item);
        }
    }
}

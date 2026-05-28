using System;
using calcaot.Models;
using calcaot.Interfaces;

namespace calcaot.ViewModels.Commands
{
    /// <summary>
    /// Команда «Добавить еду» — паттерн Command (Команда), ConcreteCommand.
    ///
    /// Инкапсулирует операцию добавления продукта в приём пищи.
    /// Хранит ссылку на группу приёма пищи (Receiver) и добавляемый продукт.
    ///
    /// Execute() — добавляет продукт в список.
    /// Undo()    — удаляет тот же продукт, возвращая состояние к исходному.
    ///
    /// Команда передаётся в CommandHistory.Execute(), который
    /// сохраняет её в стеке для возможной последующей отмены.
    /// </summary>
    public class AddFoodCommand : IUndoableCommand
    {
        private readonly MealGroupViewModel _mealGroup;
        private readonly FoodItem _item;

        public AddFoodCommand(MealGroupViewModel mealGroup, FoodItem item)
        {
            _mealGroup = mealGroup ?? throw new ArgumentNullException(nameof(mealGroup));
            _item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public void Execute() => _mealGroup.AddFood(_item);

        public void Undo() => _mealGroup.RemoveFood(_item);
    }
}

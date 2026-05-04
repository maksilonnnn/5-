using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using calcaot.Models;
using calcaot.Services;
using calcaot.ViewModels.Commands;

namespace calcaot.ViewModels
{
    /// <summary>
    /// ...
    /// Коллекция Foods объявлена как ObservableCollection — это тоже
    /// реализация паттерна Observer: при добавлении или удалении элемента
    /// поднимается событие CollectionChanged, и ListView в UI обновляется
    /// автоматически, без ручного обновления интерфейса.
    /// </summary>
    public class MealGroupViewModel : ViewModelBase
    {
        private readonly MealGroup _model;
        private readonly IDialogService _dialogService;
        private readonly CommandHistory _history;

        public ObservableCollection<FoodItem> Foods { get; }

        public ICommand DeleteFoodCommand { get; }
        public ICommand EditFoodCommand { get; }

        public MealGroupViewModel(MealGroup model, IDialogService dialogService, CommandHistory history)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _history = history ?? throw new ArgumentNullException(nameof(history));

            Foods = new ObservableCollection<FoodItem>(_model.Foods);
            Foods.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(TotalCalories));
                OnPropertyChanged(nameof(Summary));
            };

            DeleteFoodCommand = new RelayCommand<FoodItem?>(ExecuteDeleteFood, CanExecuteFood);
            EditFoodCommand = new RelayCommand<FoodItem?>(ExecuteEditFood, CanExecuteFood);
        }

        private static bool CanExecuteFood(FoodItem? food) => food != null;

        private void ExecuteDeleteFood(FoodItem? food)
        {
            if (food == null)
                return;

            var isConfirmed = _dialogService.ShowDeleteConfirmation(food.Name);
            if (!isConfirmed)
                return;

            _history.Execute(new DeleteFoodCommand(this, food));
        }

        private void ExecuteEditFood(FoodItem? food)
        {
            if (food == null)
                return;

            var result = _dialogService.ShowAddFoodDialog(
                mealName: Name,
                existingFood: food);

            if (result == null)
                return;

            _history.Execute(new EditFoodCommand(this, food, result));
        }

        public string Name => _model.Name;
        public string Icon => _model.Icon;
        public double CalorieGoal => _model.CalorieGoal;

        public double TotalCalories => Foods.Sum(f => f.Calories);

        public string Summary => Foods.Count == 0
            ? $"0 / {(int)CalorieGoal} Cal"
            : $"{(int)TotalCalories} / {(int)CalorieGoal} Cal";

        public void AddFood(FoodItem item)
        {
            Foods.Add(item);
            _model.Foods.Add(item);
        }

        public void InsertFood(int index, FoodItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (index < 0) index = 0;
            if (index > Foods.Count) index = Foods.Count;

            Foods.Insert(index, item);
            _model.Foods.Insert(index, item);
        }

        public void RemoveFood(FoodItem item)
        {
            if (Foods.Contains(item))
            {
                Foods.Remove(item);
                _model.Foods.Remove(item);
            }
        }

        public void ReplaceFood(FoodItem oldItem, FoodItem newItem)
        {
            int index = Foods.IndexOf(oldItem);
            if (index >= 0)
            {
                Foods[index] = newItem;
                _model.Foods[index] = newItem;
            }
        }
    }
}

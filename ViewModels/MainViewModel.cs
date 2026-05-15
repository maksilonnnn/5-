using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using calcaot.Models;
using calcaot.Interfaces;
using calcaot.ViewModels.Commands;

namespace calcaot.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const double DailyNorm = 2500;

        private readonly IDialogService _dialogService;
        private readonly IThemeService _themeService;
        private bool _isDark;

        public CommandHistory History { get; } = new CommandHistory();

        public ObservableCollection<MealGroupViewModel> MealGroups { get; }

        public bool IsDarkTheme
        {
            get => _isDark;
            set
            {
                if (_isDark != value)
                {
                    _isDark = value;
                    OnPropertyChanged();
                    _themeService.ApplyTheme(_isDark);
                }
            }
        }

        public ICommand ToggleThemeCommand { get; }
        public ICommand OpenAddFoodCommand { get; }
        public ICommand OpenMealDetailCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public double ConsumedCalories => MealGroups.Sum(m => m.TotalCalories);
        public double RemainingCalories => DailyNorm - ConsumedCalories;
        public double Progress => ConsumedCalories / DailyNorm;

        public MainViewModel(IDialogService dialogService, IThemeService themeService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));

            ToggleThemeCommand = new RelayCommand(() => IsDarkTheme = !IsDarkTheme);

            OpenAddFoodCommand = new RelayCommand<string?>(ExecuteOpenAddFood);
            OpenMealDetailCommand = new RelayCommand<MealGroupViewModel?>(ExecuteOpenMealDetail);

            UndoCommand = new RelayCommand(() => History.Undo(), () => History.CanUndo);
            RedoCommand = new RelayCommand(() => History.Redo(), () => History.CanRedo);

            MealGroups = new ObservableCollection<MealGroupViewModel>
            {
                new MealGroupViewModel(new MealGroup
                {
                    Name = "Завтрак",
                    Icon = "pack://application:,,,/Interface/ingestion-png/Завтрак.png",
                    CalorieGoal = 600
                }, _dialogService, History),

                new MealGroupViewModel(new MealGroup
                {
                    Name = "Обед",
                    Icon = "pack://application:,,,/Interface/ingestion-png/Обед.png",
                    CalorieGoal = 900
                }, _dialogService, History),

                new MealGroupViewModel(new MealGroup
                {
                    Name = "Ужин",
                    Icon = "pack://application:,,,/Interface/ingestion-png/Ужин.png",
                    CalorieGoal = 700
                }, _dialogService, History),

                new MealGroupViewModel(new MealGroup
                {
                    Name = "Перекус",
                    Icon = "pack://application:,,,/Interface/ingestion-png/Перекус.png",
                    CalorieGoal = 300
                }, _dialogService, History),
            };

            foreach (var meal in MealGroups)
            {
                meal.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(MealGroupViewModel.TotalCalories))
                    {
                        OnPropertyChanged(nameof(ConsumedCalories));
                        OnPropertyChanged(nameof(RemainingCalories));
                        OnPropertyChanged(nameof(Progress));
                    }
                };
            }
        }

        private void ExecuteOpenAddFood(string? mealName)
        {
            if (string.IsNullOrWhiteSpace(mealName))
                return;

            var result = _dialogService.ShowAddFoodDialog(mealName, existingFood: null);
            if (result == null)
                return;

            if (Math.Abs(result.Weight) < double.Epsilon)
                return;

            var calPer100 = (result.Calories / result.Weight) * 100;
            AddFood(mealName, result.Name, result.Weight, calPer100);
        }

        private void ExecuteOpenMealDetail(MealGroupViewModel? meal)
        {
            if (meal == null)
                return;

            _dialogService.ShowMealDetailDialog(meal);
        }

        private void AddFood(string mealName, string name, double weight, double cal100)
        {
            var meal = MealGroups.FirstOrDefault(m => m.Name == mealName);
            if (meal == null) return;

            var item = new FoodItem
            {
                Name = name,
                Weight = weight,
                Calories = (cal100 * weight) / 100,
                MealType = mealName
            };

            History.Execute(new AddFoodCommand(meal, item));
        }
    }
}

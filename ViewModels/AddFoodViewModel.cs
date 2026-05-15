using System;
using System.Windows.Input;
using calcaot.Models;

namespace calcaot.ViewModels
{
    public class AddFoodViewModel : ViewModelBase
    {
        private string _foodName = string.Empty;
        private string _weightText = string.Empty;
        private string _caloriesText = string.Empty;
        private string _errorMessage = string.Empty;
        private readonly FoodItem? _existingFood;
        private bool _isEditMode;

        private FoodItem? _resultFood;

        public string MealName { get; }
        public string Title { get; }

        public FoodItem? ResultFood
        {
            get => _resultFood;
            private set
            {
                _resultFood = value;
                OnPropertyChanged();
            }
        }

        public event Action<bool>? RequestClose;

        public string FoodName
        {
            get => _foodName;
            set
            {
                if (_foodName != value)
                {
                    _foodName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string WeightText
        {
            get => _weightText;
            set
            {
                if (_weightText != value)
                {
                    _weightText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CaloriesText
        {
            get => _caloriesText;
            set
            {
                if (_caloriesText != value)
                {
                    _caloriesText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public AddFoodViewModel(string mealName, FoodItem? existingFood = null)
        {
            MealName = mealName ?? throw new ArgumentNullException(nameof(mealName));
            _existingFood = existingFood;
            IsEditMode = existingFood != null;

            Title = existingFood != null ? $"Изменить — {mealName}" : mealName;

            if (existingFood != null)
            {
                FoodName = existingFood.Name;
                WeightText = existingFood.Weight.ToString();
                CaloriesText = (existingFood.Calories / existingFood.Weight * 100).ToString("F0");
            }

            ConfirmCommand = new RelayCommand(Confirm, CanConfirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanConfirm()
        {
            return !string.IsNullOrWhiteSpace(FoodName) &&
                   double.TryParse(WeightText, out var w) && w >= 0 &&
                   double.TryParse(CaloriesText, out var c) && c >= 0;
        }

        private void Confirm()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(FoodName))
            {
                ErrorMessage = "Введите название блюда.";
                return;
            }

            if (!double.TryParse(WeightText, out var weight) || weight < 0)
            {
                ErrorMessage = "Введите корректный вес.";
                return;
            }

            if (!double.TryParse(CaloriesText, out var cal) || cal < 0)
            {
                ErrorMessage = "Введите корректные калории.";
                return;
            }

            var food = new FoodItem
            {
                Id = _existingFood?.Id ?? 0,
                Name = FoodName.Trim(),
                Weight = weight,
                Calories = (cal * weight) / 100,
                MealType = _existingFood?.MealType ?? string.Empty
            };

            ResultFood = food;
            RequestClose?.Invoke(true);
        }

        private void Cancel()
        {
            RequestClose?.Invoke(false);
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using calcaot.Models;

namespace calcaot.ViewModels
{
    public class MealGroupViewModel : INotifyPropertyChanged
    {
        private readonly MealGroup _model;

        public ObservableCollection<FoodItem> Foods { get; }

        public MealGroupViewModel(MealGroup model)
        {
            _model = model;
            Foods = new ObservableCollection<FoodItem>(_model.Foods);
            Foods.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TotalCalories));
                OnPropertyChanged(nameof(Summary));
            };
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

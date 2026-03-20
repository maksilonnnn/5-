using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

public class MealGroup : INotifyPropertyChanged
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public double CalorieGoal { get; set; }
    public ObservableCollection<FoodItem> Foods { get; set; } = new ObservableCollection<FoodItem>();

    public double TotalCalories => Foods.Sum(f => f.Calories);
    public string Summary => Foods.Count == 0
        ? $"0 / {(int)CalorieGoal} Cal"
        : $"{(int)TotalCalories} / {(int)CalorieGoal} Cal";

    public void Refresh()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCalories)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary)));
    }

    public void RemoveFood(FoodItem item)
    {
        Foods.Remove(item);
        Refresh();
    }

    public void ReplaceFood(FoodItem oldItem, FoodItem newItem)
    {
        int index = Foods.IndexOf(oldItem);
        if (index >= 0)
        {
            Foods[index] = newItem;
            Refresh();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
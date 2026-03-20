using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

public class MainViewModel : INotifyPropertyChanged
{
    private const double DailyNorm = 2500;

    public ObservableCollection<MealGroup> MealGroups { get; set; }

    public double ConsumedCalories => MealGroups.Sum(m => m.TotalCalories);
    public double RemainingCalories => DailyNorm - ConsumedCalories;
    public double Progress => ConsumedCalories / DailyNorm;

    public MainViewModel()
    {
        MealGroups = new ObservableCollection<MealGroup>
        {
            new MealGroup { Name = "Завтрак", Icon = "🌅", CalorieGoal = 600 },
            new MealGroup { Name = "Обед",    Icon = "☀️", CalorieGoal = 900 },
            new MealGroup { Name = "Ужин",    Icon = "🌙", CalorieGoal = 700 },
            new MealGroup { Name = "Перекус", Icon = "🍎", CalorieGoal = 300 },
        };
    }

    public void AddFood(string mealName, string name, double weight, double cal100)
    {
        var meal = MealGroups.FirstOrDefault(m => m.Name == mealName);
        if (meal == null) return;

        meal.Foods.Add(new FoodItem
        {
            Name = name,
            Weight = weight,
            Calories = (cal100 * weight) / 100,
            MealType = mealName
        });

        meal.Refresh();
        OnPropertyChanged(nameof(ConsumedCalories));
        OnPropertyChanged(nameof(RemainingCalories));
        OnPropertyChanged(nameof(Progress));
    }
    public void RefreshTotals()
{
    OnPropertyChanged(nameof(ConsumedCalories));
    OnPropertyChanged(nameof(RemainingCalories));
    OnPropertyChanged(nameof(Progress));
}

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
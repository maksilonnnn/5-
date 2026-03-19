using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

public class MainViewModel : INotifyPropertyChanged
{
    private double _dailyNorm = 2500;
    

    public ObservableCollection<FoodItem> ConsumedFoods { get; set; } = new ObservableCollection<FoodItem>();

    public double RemainingCalories => _dailyNorm - ConsumedFoods.Sum(f => f.Calories);

    public void AddFood(string name, double weight, double cal100, string meal)
    {
        double finalCal = (cal100 * weight) / 100;
        ConsumedFoods.Add(new FoodItem { 
            Name = name, 
            Weight = weight, 
            Calories = finalCal, 
            MealType = meal 
        });
        
        OnPropertyChanged(nameof(RemainingCalories));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

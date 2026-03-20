using System.ComponentModel;
using System.Runtime.CompilerServices;

public class FoodItem : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private double _weight;
    private double _calories;
    private string _mealType = string.Empty;

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }
    public double Weight
    {
        get => _weight;
        set { _weight = value; OnPropertyChanged(); }
    }
    public double Calories
    {
        get => _calories;
        set { _calories = value; OnPropertyChanged(); }
    }
    public string MealType
    {
        get => _mealType;
        set { _mealType = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
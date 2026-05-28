namespace calcaot.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Weight { get; set; }
        public double Calories { get; set; }
        public string MealType { get; set; } = string.Empty;
    }
}
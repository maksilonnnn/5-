using System.Collections.Generic;

namespace calcaot.Models
{
    public class MealGroup
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public double CalorieGoal { get; set; }
        public List<FoodItem> Foods { get; set; } = new List<FoodItem>();
    }
}

using calcaot.Models;

namespace calcaot.Tests;

public class FoodItemTests
{
    [Fact]
    public void Properties_CanBeSetAndRead()
    {
        var food = new FoodItem
        {
            Id = 7,
            Name = "Овсянка",
            Weight = 150,
            Calories = 255,
            MealType = "Завтрак"
        };

        Assert.Equal(7, food.Id);
        Assert.Equal("Овсянка", food.Name);
        Assert.Equal(150, food.Weight);
        Assert.Equal(255, food.Calories);
        Assert.Equal("Завтрак", food.MealType);
    }

    [Fact]
    public void DefaultValues_AreEmpty()
    {
        var food = new FoodItem();

        Assert.Equal(0, food.Id);
        Assert.Equal(string.Empty, food.Name);
        Assert.Equal(0, food.Weight);
        Assert.Equal(0, food.Calories);
        Assert.Equal(string.Empty, food.MealType);
    }
}

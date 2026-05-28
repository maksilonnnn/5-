using calcaot.Interfaces;
using calcaot.Models;
using calcaot.ViewModels;
using calcaot.ViewModels.Commands;

namespace calcaot.Tests;

internal class FakeDialogService : IDialogService
{
    public FoodItem? ShowAddFoodDialog(string mealName, FoodItem? existingFood) => null;
    public bool ShowDeleteConfirmation(string foodName) => true;
    public void ShowMealDetailDialog(MealGroupViewModel meal) { }
}

public class MealGroupViewModelTests
{
    private static MealGroupViewModel CreateVm(CommandHistory? history = null)
    {
        var model = new MealGroup { Name = "Завтрак", Icon = string.Empty, CalorieGoal = 600 };
        return new MealGroupViewModel(model, new FakeDialogService(), history ?? new CommandHistory());
    }

    [Fact]
    public void Foods_StartsEmpty()
    {
        Assert.Empty(CreateVm().Foods);
    }

    [Fact]
    public void AddFood_IncreasesCount()
    {
        var vm = CreateVm();
        vm.AddFood(new FoodItem { Name = "Яйцо", Calories = 70 });

        Assert.Single(vm.Foods);
    }

    [Fact]
    public void RemoveFood_DecreasesCount()
    {
        var vm = CreateVm();
        var food = new FoodItem { Name = "Яйцо", Calories = 70 };
        vm.AddFood(food);
        vm.RemoveFood(food);

        Assert.Empty(vm.Foods);
    }

    [Fact]
    public void TotalCalories_SumsCorrectly()
    {
        var vm = CreateVm();
        vm.AddFood(new FoodItem { Calories = 100 });
        vm.AddFood(new FoodItem { Calories = 200 });

        Assert.Equal(300, vm.TotalCalories);
    }

    [Fact]
    public void TotalCalories_WhenEmpty_IsZero()
    {
        Assert.Equal(0, CreateVm().TotalCalories);
    }

    [Fact]
    public void InsertFood_AtIndex_InsertsCorrectly()
    {
        var vm = CreateVm();
        var first = new FoodItem { Name = "Первый" };
        var second = new FoodItem { Name = "Второй" };
        var inserted = new FoodItem { Name = "Вставленный" };

        vm.AddFood(first);
        vm.AddFood(second);
        vm.InsertFood(1, inserted);

        Assert.Equal("Вставленный", vm.Foods[1].Name);
    }

    [Fact]
    public void ReplaceFood_ReplacesAtSameIndex()
    {
        var vm = CreateVm();
        var old = new FoodItem { Name = "Старый", Calories = 100 };
        var newer = new FoodItem { Name = "Новый", Calories = 200 };

        vm.AddFood(old);
        vm.ReplaceFood(old, newer);

        Assert.Equal("Новый", vm.Foods[0].Name);
        Assert.Single(vm.Foods);
    }

    [Fact]
    public void Name_ReturnsModelName()
    {
        Assert.Equal("Завтрак", CreateVm().Name);
    }

    [Fact]
    public void CalorieGoal_ReturnsModelValue()
    {
        Assert.Equal(600, CreateVm().CalorieGoal);
    }
}

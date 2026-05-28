using System.Collections.Generic;
using calcaot.Interfaces;
using calcaot.Models;
using calcaot.ViewModels;
using calcaot.ViewModels.Commands;

namespace calcaot.Tests;

public class FoodCommandTests
{
    private static MealGroupViewModel CreateVm() =>
        new(
            new MealGroup { Name = "Обед", Icon = string.Empty, CalorieGoal = 900 },
            new FakeDialogService(),
            new CommandHistory()
        );

    [Fact]
    public void AddFoodCommand_Execute_AddsFoodToGroup()
    {
        var vm = CreateVm();
        var food = new FoodItem { Name = "Суп", Calories = 200 };
        var cmd = new AddFoodCommand(vm, food);

        cmd.Execute();

        Assert.Contains(food, vm.Foods);
    }

    [Fact]
    public void AddFoodCommand_Undo_RemovesFoodFromGroup()
    {
        var vm = CreateVm();
        var food = new FoodItem { Name = "Суп", Calories = 200 };
        var cmd = new AddFoodCommand(vm, food);

        cmd.Execute();
        cmd.Undo();

        Assert.DoesNotContain(food, vm.Foods);
    }

    [Fact]
    public void DeleteFoodCommand_Execute_RemovesFood()
    {
        var vm = CreateVm();
        var food = new FoodItem { Name = "Хлеб", Calories = 80 };
        vm.AddFood(food);
        var cmd = new DeleteFoodCommand(vm, food);

        cmd.Execute();

        Assert.DoesNotContain(food, vm.Foods);
    }

    [Fact]
    public void DeleteFoodCommand_Undo_RestoresFood()
    {
        var vm = CreateVm();
        var food = new FoodItem { Name = "Хлеб", Calories = 80 };
        vm.AddFood(food);
        var cmd = new DeleteFoodCommand(vm, food);

        cmd.Execute();
        cmd.Undo();

        Assert.Contains(food, vm.Foods);
    }

    [Fact]
    public void DeleteFoodCommand_Undo_RestoresFoodAtOriginalIndex()
    {
        var vm = CreateVm();
        var first = new FoodItem { Name = "Первый" };
        var target = new FoodItem { Name = "Цель" };
        var last = new FoodItem { Name = "Последний" };
        vm.AddFood(first);
        vm.AddFood(target);
        vm.AddFood(last);

        var cmd = new DeleteFoodCommand(vm, target);
        cmd.Execute();
        cmd.Undo();

        Assert.Equal("Цель", vm.Foods[1].Name);
    }

    [Fact]
    public void EditFoodCommand_Execute_ReplacesOldWithNew()
    {
        var vm = CreateVm();
        var oldItem = new FoodItem { Name = "Старый", Calories = 100 };
        var newItem = new FoodItem { Name = "Новый", Calories = 250 };
        vm.AddFood(oldItem);
        var cmd = new EditFoodCommand(vm, oldItem, newItem);

        cmd.Execute();

        Assert.DoesNotContain(oldItem, vm.Foods);
        Assert.Contains(newItem, vm.Foods);
    }

    [Fact]
    public void EditFoodCommand_Undo_RestoresOldItem()
    {
        var vm = CreateVm();
        var oldItem = new FoodItem { Name = "Старый", Calories = 100 };
        var newItem = new FoodItem { Name = "Новый", Calories = 250 };
        vm.AddFood(oldItem);
        var cmd = new EditFoodCommand(vm, oldItem, newItem);

        cmd.Execute();
        cmd.Undo();

        Assert.Contains(oldItem, vm.Foods);
        Assert.DoesNotContain(newItem, vm.Foods);
    }

    [Fact]
    public void EditFoodCommand_Execute_DoesNotChangeCount()
    {
        var vm = CreateVm();
        var oldItem = new FoodItem { Name = "Старый", Calories = 100 };
        var newItem = new FoodItem { Name = "Новый", Calories = 250 };
        vm.AddFood(oldItem);
        var cmd = new EditFoodCommand(vm, oldItem, newItem);

        cmd.Execute();

        Assert.Single(vm.Foods);
    }
}

using calcaot.Services;

namespace calcaot.Tests;

public class DiaryMementoTests
{
    [Fact]
    public void Constructor_StoresMealGroups()
    {
        var foods = new List<FoodSnapshot> { new(1, "Яблоко", 100, 52, "Завтрак") };
        var groups = new List<MealGroupSnapshot> { new("Завтрак", foods) };

        var memento = new DiaryMemento(groups);

        Assert.Single(memento.MealGroups);
        Assert.Equal("Завтрак", memento.MealGroups[0].Name);
    }

    [Fact]
    public void Constructor_EmptyMealGroups_IsAllowed()
    {
        var memento = new DiaryMemento(new List<MealGroupSnapshot>());

        Assert.Empty(memento.MealGroups);
    }

    [Fact]
    public void RecordEquality_SameReference_AreEqual()
    {
        var foods = new List<FoodSnapshot> { new(1, "Яблоко", 100, 52, "Завтрак") };
        var groups = new List<MealGroupSnapshot> { new("Завтрак", foods) };
        var a = new DiaryMemento(groups);

        Assert.Equal(a, a);
    }

    [Fact]
    public void RecordEquality_DifferentListInstances_NotEqual()
    {
        var foods1 = new List<FoodSnapshot> { new(1, "Яблоко", 100, 52, "Завтрак") };
        var foods2 = new List<FoodSnapshot> { new(1, "Яблоко", 100, 52, "Завтрак") };
        var a = new DiaryMemento(new List<MealGroupSnapshot> { new("Завтрак", foods1) });
        var b = new DiaryMemento(new List<MealGroupSnapshot> { new("Завтрак", foods2) });

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void FoodSnapshot_StoresAllProperties()
    {
        var snap = new FoodSnapshot(42, "Банан", 150, 130, "Обед");

        Assert.Equal(42, snap.Id);
        Assert.Equal("Банан", snap.Name);
        Assert.Equal(150, snap.Weight);
        Assert.Equal(130, snap.Calories);
        Assert.Equal("Обед", snap.MealType);
    }

    [Fact]
    public void MealGroupSnapshot_StoresNameAndFoods()
    {
        var snap = new MealGroupSnapshot("Ужин", new List<FoodSnapshot>());

        Assert.Equal("Ужин", snap.Name);
        Assert.Empty(snap.Foods);
    }
}

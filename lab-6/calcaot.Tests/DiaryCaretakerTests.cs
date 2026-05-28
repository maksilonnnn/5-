using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using calcaot.Services;

namespace calcaot.Tests;

public class DiaryCaretakerTests : IDisposable
{
    private readonly string _testFilePath =
        Path.Combine(Path.GetTempPath(), $"diary_test_{Guid.NewGuid()}.json");

    private DiaryCaretaker CreateCaretaker() => new(_testFilePath);

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }

    [Fact]
    public void SaveFilePath_EndsWithDiaryJson()
    {
        Assert.EndsWith("diary.json", DiaryCaretaker.SaveFilePath);
    }

    [Fact]
    public void GetInstance_ReturnsNonNull()
    {
        Assert.NotNull(DiaryCaretaker.GetInstance());
    }

    [Fact]
    public void GetInstance_ReturnsSameInstance()
    {
        Assert.Same(DiaryCaretaker.GetInstance(), DiaryCaretaker.GetInstance());
    }

    [Fact]
    public async Task SaveAsync_WritesCorrectJsonToFile()
    {
        var caretaker = CreateCaretaker();
        var foods = new List<FoodSnapshot> { new(1, "Гречка", 200, 340, "Обед") };
        var memento = new DiaryMemento(new List<MealGroupSnapshot> { new("Обед", foods) });

        await caretaker.SaveAsync(memento);

        Assert.True(File.Exists(_testFilePath));
        var json = await File.ReadAllTextAsync(_testFilePath);
        var loaded = JsonSerializer.Deserialize<DiaryMemento>(json);

        Assert.NotNull(loaded);
        Assert.Single(loaded!.MealGroups);
        Assert.Equal("Обед", loaded.MealGroups[0].Name);
        Assert.Equal("Гречка", loaded.MealGroups[0].Foods[0].Name);
    }

    [Fact]
    public async Task LoadAsync_RestoresFromJsonFile()
    {
        var caretaker = CreateCaretaker();
        var foods = new List<FoodSnapshot> { new(2, "Творог", 150, 165, "Завтрак") };
        var original = new DiaryMemento(new List<MealGroupSnapshot> { new("Завтрак", foods) });

        await caretaker.SaveAsync(original);
        var loaded = await caretaker.LoadAsync();

        Assert.NotNull(loaded);
        Assert.Equal("Завтрак", loaded!.MealGroups[0].Name);
        Assert.Equal("Творог", loaded.MealGroups[0].Foods[0].Name);
    }

    [Fact]
    public async Task LoadAsync_WhenNoFileExists_ReturnsNull()
    {
        var caretaker = CreateCaretaker();

        var result = await caretaker.LoadAsync();

        Assert.Null(result);
    }

    [Fact]
    public async Task SaveAndLoad_RoundTrip_PreservesData()
    {
        var caretaker = CreateCaretaker();
        var foods = new List<FoodSnapshot>
        {
            new(3, "Рис", 250, 325, "Обед"),
            new(4, "Курица", 180, 297, "Обед"),
        };
        var memento = new DiaryMemento(new List<MealGroupSnapshot> { new("Обед", foods) });

        await caretaker.SaveAsync(memento);
        var loaded = await caretaker.LoadAsync();

        Assert.NotNull(loaded);
        Assert.Equal(2, loaded!.MealGroups[0].Foods.Count);
        Assert.Equal("Рис", loaded.MealGroups[0].Foods[0].Name);
        Assert.Equal("Курица", loaded.MealGroups[0].Foods[1].Name);
    }

    [Fact]
    public async Task SaveAsync_IsSavingFalseAfterCompletion()
    {
        var caretaker = CreateCaretaker();
        var memento = new DiaryMemento(new List<MealGroupSnapshot>());

        await caretaker.SaveAsync(memento);

        Assert.False(caretaker.IsSaving);
    }
}

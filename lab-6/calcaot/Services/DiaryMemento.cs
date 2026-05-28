using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace calcaot.Services
{
    /// <summary>
    /// Паттерн Memento (Хранитель) — неизменяемый снимок дневника питания
    /// в определённый момент времени.
    ///
    /// Зачем это нужно:
    /// Позволяет сохранить текущее состояние дневника (все приёмы пищи
    /// со всеми продуктами) и восстановить его при следующем запуске.
    /// Объект-снимок неизменяем — сохранённые данные нельзя случайно повредить.
    ///
    /// Как это работает:
    /// 1) Originator (MainViewModel) создаёт DiaryMemento через CreateMemento().
    /// 2) Caretaker (DiaryCaretaker) сериализует его в JSON и записывает в файл.
    /// 3) При загрузке Caretaker десериализует DiaryMemento и передаёт Originator
    ///    через RestoreMemento() — дневник восстанавливается.
    /// </summary>
    public record DiaryMemento(
        [property: JsonPropertyName("mealGroups")] List<MealGroupSnapshot> MealGroups
    );

    /// <summary>Снимок одной группы приёма пищи (Завтрак, Обед и т.д.).</summary>
    public record MealGroupSnapshot(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("foods")] List<FoodSnapshot> Foods
    );

    /// <summary>Снимок одного продукта питания.</summary>
    public record FoodSnapshot(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("weight")] double Weight,
        [property: JsonPropertyName("calories")] double Calories,
        [property: JsonPropertyName("mealType")] string MealType
    );
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace calcaot.Services
{
    /// <summary>
    /// Паттерн Caretaker (Опекун) из Memento — отвечает за сохранение
    /// и восстановление снимков дневника питания (DiaryMemento).
    ///
    /// Зачем это нужно:
    /// Автоматически сохраняет дневник питания при любом изменении
    /// и восстанавливает его при запуске приложения.
    ///
    /// Как это работает:
    /// 1) ScheduleSave() вызывается после каждой команды (Add/Delete/Edit/Undo/Redo).
    /// 2) Debounce 300 мс — если изменения идут подряд, ждём окончания серии.
    /// 3) SaveAsync() сериализует DiaryMemento в JSON и записывает в diary.json.
    /// 4) LoadAsync() вызывается при старте — читает файл и возвращает снимок.
    /// </summary>
    public class DiaryCaretaker : INotifyPropertyChanged
    {
        // Singleton — единственный экземпляр на всё приложение
        private static DiaryCaretaker? _instance;
        public static DiaryCaretaker GetInstance() =>
            _instance ??= new DiaryCaretaker();

        private readonly string _filePath;
        private CancellationTokenSource? _debounceCts;
        private bool _isSaving;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Идёт ли сейчас сохранение. UI привязывается к этому свойству
        /// для показа/скрытия индикатора загрузки.
        /// </summary>
        public bool IsSaving
        {
            get => _isSaving;
            private set
            {
                if (_isSaving != value)
                {
                    _isSaving = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true   // красивый читаемый JSON
        };

        /// <summary>Путь к файлу сохранения — рядом с .exe.</summary>
        public static string SaveFilePath =>
            Path.Combine(AppContext.BaseDirectory, "diary.json");

        private DiaryCaretaker()
        {
            _filePath = SaveFilePath;
        }

        internal DiaryCaretaker(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public async void ScheduleSave(DiaryMemento memento)
        {
            _debounceCts?.Cancel();
            _debounceCts = new CancellationTokenSource();
            var token = _debounceCts.Token;

            try
            {
                await Task.Delay(300, token);
                await SaveAsync(memento, token);
            }
            catch (OperationCanceledException)
            {
                // Пришло новое изменение раньше — это нормально, игнорируем
            }
        }

        public async Task SaveAsync(DiaryMemento memento, CancellationToken ct = default)
        {
            IsSaving = true;
            try
            {
                var json = JsonSerializer.Serialize(memento, JsonOptions);
                await File.WriteAllTextAsync(_filePath, json, ct);
            }
            finally
            {
                IsSaving = false;
            }
        }

        /// <summary>Загрузить снимок из файла (при старте приложения).</summary>
        public async Task<DiaryMemento?> LoadAsync()
        {
            if (!File.Exists(_filePath))
                return null;

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<DiaryMemento>(json);
        }
    }
}

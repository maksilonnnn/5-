using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace calcaot.ViewModels
{
    /// <summary>
    /// Базовый класс для всех ViewModel — паттерн Observer (Наблюдатель).
    ///
    /// Реализует интерфейс INotifyPropertyChanged, который является
    /// стандартной реализацией паттерна Observer в .NET.
    ///
    /// Как это работает:
    ///   Когда свойство ViewModel изменяется, вызывается OnPropertyChanged().
    ///   Это поднимает событие PropertyChanged, на которое WPF-биндинг
    ///   автоматически подписан. Получив уведомление, UI обновляет
    ///   привязанный элемент без какого-либо кода в code-behind.
    ///
    /// Например, при изменении ConsumedCalories в MainViewModel
    /// прогресс-дуга и счётчик калорий обновляются автоматически —
    /// ViewModel не знает ничего о конкретных элементах UI.
    ///
    /// Атрибут [CallerMemberName] позволяет не передавать имя свойства
    /// вручную: OnPropertyChanged() определяет его автоматически.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

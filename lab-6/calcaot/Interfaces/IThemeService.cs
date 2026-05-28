using System.Windows;

namespace calcaot.Interfaces
{
    /// <summary>
    /// Паттерн Facade (Фасад): интерфейс фасада над системой тем приложения.
    ///
    /// ThemeService прячет детали переключения ResourceDictionary WPF
    /// за единым методом ApplyTheme(bool isDark).
    /// </summary>
    public interface IThemeService
    {
        void ApplyTheme(bool isDark);
    }
}

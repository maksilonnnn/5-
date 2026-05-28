using System.Windows;
using calcaot.Interfaces;

namespace calcaot.Services
{
/// <summary>
/// Реализация фасада над системой тем — паттерн Facade (Фасад).
/// Скрывает детали работы с ResourceDictionary WPF за методом ApplyTheme().
/// </summary>
    public class ThemeService : IThemeService
    {
        public void ApplyTheme(bool isDark)
        {
            var themeName = isDark ? "DarkTheme.xaml" : "LightTheme.xaml";
            var themeUri = new Uri($"pack://application:,,,/Interface/Themes/{themeName}");
            var newTheme = new ResourceDictionary { Source = themeUri };

            if (Application.Current.Resources.MergedDictionaries.Count > 0)
            {
                Application.Current.Resources.MergedDictionaries[0] = newTheme;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(newTheme);
            }
        }
    }
}

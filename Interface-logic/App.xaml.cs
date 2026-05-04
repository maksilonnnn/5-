using System;
using System.Windows;
using calcaot.Services;
using calcaot.ViewModels;

namespace calcaot;
public partial class App : Application
{
    private MainViewModel? _mainViewModel;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var dialogService = new DialogService();
        var themeService = new ThemeService();
        _mainViewModel = new MainViewModel(dialogService);

        // Применяем начальную тему
        themeService.ApplyTheme(_mainViewModel.IsDarkTheme);

        var mainWindow = new MainWindow(_mainViewModel, themeService);
        mainWindow.Show();
    }
}

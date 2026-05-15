using System;
using System.Windows;
using calcaot.Interfaces;
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
        _mainViewModel = new MainViewModel(dialogService, themeService);

        var mainWindow = new MainWindow(_mainViewModel);
        mainWindow.Show();
    }
}

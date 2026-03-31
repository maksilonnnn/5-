using System.Windows;
using System.Windows.Media;

namespace calcaot;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        InitLightTheme();
    }

    private void InitLightTheme()
    {
        var res = Resources;
        res["AppBackground"]    = new SolidColorBrush(Color.FromRgb(245, 245, 247));
        res["CardBackground"]   = new SolidColorBrush(Colors.White);
        res["PrimaryText"]      = new SolidColorBrush(Color.FromRgb(28, 28, 30));
        res["SecondaryText"]    = new SolidColorBrush(Color.FromRgb(142, 142, 147));
        res["IconBackground"]   = new SolidColorBrush(Color.FromRgb(239, 244, 255));
        res["DeleteBackground"] = new SolidColorBrush(Color.FromRgb(255, 240, 240));
        res["EditBackground"]   = new SolidColorBrush(Color.FromRgb(239, 244, 255));
        res["InputBorder"]      = new SolidColorBrush(Color.FromRgb(224, 224, 224));
    }
}



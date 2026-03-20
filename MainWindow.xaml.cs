using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CalorieApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel = new MainViewModel();
        private bool _isDark = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.PropertyChanged += (s, e) => UpdateProgressArc();
            UpdateProgressArc();
        }

        private void UpdateProgressArc()
        {
            double progress = Math.Clamp(viewModel.Progress, 0.001, 0.999);
            const double cx = 65, cy = 65, r = 60;
            double startRad = -Math.PI / 2;
            double endRad = startRad + progress * 2 * Math.PI;
            double startX = cx + r * Math.Cos(startRad);
            double startY = cy + r * Math.Sin(startRad);
            double endX = cx + r * Math.Cos(endRad);
            double endY = cy + r * Math.Sin(endRad);
            var figure = new PathFigure { StartPoint = new Point(startX, startY) };
            figure.Segments.Add(new ArcSegment(
                new Point(endX, endY),
                new Size(r, r),
                0,
                progress > 0.5,
                SweepDirection.Clockwise,
                true));
            ProgressArc.Data = new PathGeometry(new[] { figure });
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            _isDark = !_isDark;
            var res = Application.Current.Resources;
            if (_isDark)
            {
                res["AppBackground"]    = new SolidColorBrush(Color.FromRgb(28,  28,  30));
                res["CardBackground"]   = new SolidColorBrush(Color.FromRgb(44,  44,  46));
                res["PrimaryText"]      = new SolidColorBrush(Colors.White);
                res["SecondaryText"]    = new SolidColorBrush(Color.FromRgb(174, 174, 178));
                res["IconBackground"]   = new SolidColorBrush(Color.FromRgb(40,  55,  80));
                res["DeleteBackground"] = new SolidColorBrush(Color.FromRgb(80,  30,  30));
                res["EditBackground"]   = new SolidColorBrush(Color.FromRgb(40,  55,  80));
                res["InputBorder"]      = new SolidColorBrush(Color.FromRgb(72,  72,  74));
            }
            else
            {
                res["AppBackground"]    = new SolidColorBrush(Color.FromRgb(245, 245, 247));
                res["CardBackground"]   = new SolidColorBrush(Colors.White);
                res["PrimaryText"]      = new SolidColorBrush(Color.FromRgb(28,  28,  30));
                res["SecondaryText"]    = new SolidColorBrush(Color.FromRgb(142, 142, 147));
                res["IconBackground"]   = new SolidColorBrush(Color.FromRgb(239, 244, 255));
                res["DeleteBackground"] = new SolidColorBrush(Color.FromRgb(255, 240, 240));
                res["EditBackground"]   = new SolidColorBrush(Color.FromRgb(239, 244, 255));
                res["InputBorder"]      = new SolidColorBrush(Color.FromRgb(224, 224, 224));
            }
        }

        private void AddFood_Click(object sender, RoutedEventArgs e)
        {
            string mealName = (sender as Button)?.Tag?.ToString() ?? "";
            var dialog = new AddFoodWindow(mealName) { Owner = this };
            if (dialog.ShowDialog() == true)
                viewModel.AddFood(mealName, dialog.FoodName, dialog.Weight, dialog.CalPer100);
        }

        private void MealRow_Click(object sender, RoutedEventArgs e)
        {
            string mealName = (sender as Button)?.Tag?.ToString() ?? "";
            var meal = viewModel.MealGroups.FirstOrDefault(m => m.Name == mealName);
            if (meal == null) return;
            new MealDetailWindow(meal, viewModel) { Owner = this }.ShowDialog();
        }
    }
}
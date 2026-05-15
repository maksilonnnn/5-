using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using calcaot.ViewModels;

namespace calcaot
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MainViewModel.IsDarkTheme))
                return;

            if (sender is MainViewModel viewModel)
            {
                Dispatcher.Invoke(() => AnimateThemeTransition(viewModel.IsDarkTheme));
            }
        }

        private void AnimateThemeTransition(bool isDark)
        {
            var backgroundBrush = TryFindResource("AppBackground") as SolidColorBrush;
            var overlayBrush = backgroundBrush != null
                ? new SolidColorBrush(backgroundBrush.Color)
                : new SolidColorBrush(Colors.Black);

            ThemeFadeOverlay.Fill = overlayBrush;
            ThemeFadeOverlay.Visibility = Visibility.Visible;

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(180))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            fadeIn.Completed += (_, _) =>
            {
                var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(180))
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                fadeOut.Completed += (_, _) =>
                {
                    ThemeFadeOverlay.Visibility = Visibility.Collapsed;
                };

                ThemeFadeOverlay.BeginAnimation(OpacityProperty, fadeOut);
            };

            ThemeFadeOverlay.BeginAnimation(OpacityProperty, fadeIn);
        }
    }
}

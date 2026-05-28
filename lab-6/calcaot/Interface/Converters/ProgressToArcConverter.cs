using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace calcaot.Interface.Converters
{
    public sealed class ProgressToArcConverter : IValueConverter
    {
        private const double Cx = 65;
        private const double Cy = 65;
        private const double R = 60;

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            double progress = value switch
            {
                double d => d,
                float f => f,
                int i => i,
                long l => l,
                _ => 0d
            };

            progress = Math.Clamp(progress, 0.001, 0.999);

            double startRad = -Math.PI / 2;
            double endRad = startRad + progress * 2 * Math.PI;

            var startPoint = new Point(Cx + R * Math.Cos(startRad), Cy + R * Math.Sin(startRad));
            var endPoint = new Point(Cx + R * Math.Cos(endRad), Cy + R * Math.Sin(endRad));

            bool isLargeArc = progress > 0.5;

            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure { StartPoint = startPoint };
            pathFigure.Segments.Add(new ArcSegment(endPoint, new Size(R, R), 0, isLargeArc, SweepDirection.Clockwise, true));
            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
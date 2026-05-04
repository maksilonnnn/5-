using System.Windows;
using calcaot.ViewModels;

namespace calcaot
{
    public partial class MealDetailWindow : Window
    {
        public MealDetailWindow(MealGroupViewModel meal)
        {
            InitializeComponent();
            DataContext = meal;
        }
    }
}

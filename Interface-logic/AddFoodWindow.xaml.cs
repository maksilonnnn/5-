using System.Windows;
using calcaot.ViewModels;

namespace calcaot
{
    public partial class AddFoodWindow : Window
    {
        private readonly AddFoodViewModel _viewModel;

        public AddFoodWindow(AddFoodViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            DataContext = _viewModel;

            _viewModel.RequestClose += (isConfirmed) =>
            {
                DialogResult = isConfirmed;
                Close();
            };
        }
    }
}

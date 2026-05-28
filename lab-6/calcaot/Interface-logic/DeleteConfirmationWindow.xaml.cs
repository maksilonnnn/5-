using System.Windows;
using calcaot.ViewModels;

namespace calcaot
{
    public partial class DeleteConfirmationWindow : Window
    {
        private readonly DeleteConfirmationViewModel _viewModel;

        public DeleteConfirmationWindow(string foodName)
        {
            InitializeComponent();
            _viewModel = new DeleteConfirmationViewModel(foodName);
            DataContext = _viewModel;

            _viewModel.RequestClose += (isConfirmed) =>
            {
                DialogResult = isConfirmed;
                Close();
            };
        }
    }
}
using System;
using System.Windows.Input;

namespace calcaot.ViewModels
{
    public class DeleteConfirmationViewModel : ViewModelBase
    {
        public string Message { get; }

        public event Action<bool>? RequestClose;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public DeleteConfirmationViewModel(string foodName)
        {
            Message = $"Точно ли вы хотите удалить \"{foodName}\"?";

            ConfirmCommand = new RelayCommand(() => RequestClose?.Invoke(true));
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));
        }
    }
}
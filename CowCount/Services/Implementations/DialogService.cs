using CowCount.Services.Interfaces;
using CowCount.ViewModels;
using System.Windows;

namespace CowCount.Services.Implementations
{
    public class DialogService : IDialogService
    {
        public object? ShowDialog(object viewModel)
        {
            var window = new CowCount.Views.ModalWindow
            {
                Owner = Application.Current.MainWindow
            };

            var dialogViewModel = new DialogViewModel(viewModel, () => { window.Close(); });

            window.DataContext = dialogViewModel;

            window.ShowDialog();
            window.Close();

            return dialogViewModel.DialogResult;
        }

        public object? Show(object viewModel)
        {
            var window = new CowCount.Views.ModalWindow
            {
                Owner = Application.Current.MainWindow
            };

            var dialogViewModel = new DialogViewModel(viewModel, () => { window.Close(); });

            window.DataContext = dialogViewModel;

            window.Show();

            return dialogViewModel.DialogResult;
        }
    }
}

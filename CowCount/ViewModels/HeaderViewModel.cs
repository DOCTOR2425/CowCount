using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class HeaderViewModel : ViewModelBase, IDisposable
    {
        private readonly CancellationTokenSource _mainCancellationTokenSource = new();
        private readonly ISavedDataService _savedDataService;
        private readonly IDialogService _dialogService;

        private Data _data;

        public HeaderViewModel(ISavedDataService savedDataService,
                               IDialogService dialogService)
        {
            _savedDataService = savedDataService;
            _dialogService = dialogService;

            AddGroupCommand = new RelayCommand(AddGroupCommandExecute);
        }

        public ICommand AddGroupCommand { get; }

        public async Task InitializeAsync()
        {
            _data = await _savedDataService.GetDataAsync(_mainCancellationTokenSource.Token);
        }

        private void AddGroupCommandExecute(object? obj)
        {
            var dialogViewModel = new AddGroupDialogWindowViewModel(obj is string defaultValue ? defaultValue : null);
            var dialogResult = _dialogService.ShowDialog(dialogViewModel);

            if (dialogResult is null or false ||
                string.IsNullOrEmpty(dialogViewModel.GroupName))
            {
                return;
            }

            if (_data.Groups is null ||
                _data.Groups.Count == 0)
            {
                _data.Groups = new();
            }
            else if (_data.Groups.Any(g => g.Equals(dialogViewModel.GroupName, StringComparison.CurrentCultureIgnoreCase)))
            {
                MessageBox.Show($"Группа с название \"{dialogViewModel.GroupName}\" уже существует",
                                "Ошибка длбавления",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                AddGroupCommandExecute(dialogViewModel.GroupName);
                return;
            }

            _data.Groups.Add(dialogViewModel.GroupName);
            Task.Factory.StartNew(() =>
                _savedDataService.UpdateDataAsync(_data, _mainCancellationTokenSource.Token));
        }

        public void Dispose()
        {
            _mainCancellationTokenSource.Dispose();
        }
    }
}

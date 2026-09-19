using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class GroupsViewModel : ViewModelBase
    {
        private readonly ISavedDataService _savedDataService;
        private readonly IDialogService _dialogService;

        private CancellationTokenSource _cancellationTokenSource;
        private Data _data;
        private ObservableCollectionEx<string>? _groups;

        public GroupsViewModel(ISavedDataService savedDataService,
                               IDialogService dialogService)
        {
            _savedDataService = savedDataService;
            _dialogService = dialogService;

            AddGroupCommand = new RelayCommand(AddGroupCommandExecute);
            EditGroupCommand = new RelayCommand(EditGroupCommandExecute);
            DeleteGroupCommand = new RelayCommand(DeleteGroupCommandExecute);
        }

        public ICommand AddGroupCommand { get; }
        public ICommand EditGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }

        public ObservableCollectionEx<string>? Groups
        {
            get => _groups;
            set
            {
                if (value == _groups)
                {
                    return;
                }

                _groups = value;
                OnPropertyChanged();
            }
        }

        public async Task InitializeAsync(CancellationToken parentToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(parentToken);

            try
            {
                _data = await _savedDataService.GetDataAsync(_cancellationTokenSource.Token)
                                               .ConfigureAwait(false);
                if (_data.Groups is not null)
                {
                    Groups = new(_data.Groups);
                }
            }
            catch (Exception exception)
            {
            }
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
            Groups = new(_data.Groups);
            Task.Factory.StartNew(() =>
                _savedDataService.UpdateDataAsync(_data, _cancellationTokenSource.Token));
        }

        private void EditGroupCommandExecute(object? obj)
        {

        }

        private void DeleteGroupCommandExecute(object? obj)
        {
            if (obj is not string targetGroup ||
                _data.Groups is null)
            {
                return;
            }

            var result = MessageBox.Show(
                $"Вы хотите удалить группу \"{targetGroup}\"?\n" +
                $"Все коровы и секции группы \"{targetGroup}\" СОХРАНЯТСЯ, но останутся без группы.",
                "Удаление группы.",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _data.Groups.Remove(targetGroup);
                if (_data.Sections is not null)
                {
                    _data.Sections.Where(s => s.Group == targetGroup).ToList().ForEach(s => s.Group = null);
                }
                if (_data.Cows is not null)
                {
                    _data.Cows.Where(c => c.Group == targetGroup).ToList().ForEach(c => c.Group = null);
                }

                Groups = new(_data.Groups);

                Task.Factory.StartNew(() =>
                    _savedDataService.UpdateDataAsync(_data, _cancellationTokenSource.Token));
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}

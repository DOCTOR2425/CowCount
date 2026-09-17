using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly CancellationTokenSource _mainCancellationTokenSource = new();
        private readonly ISavedDataService _savedDataService;
        private readonly IDialogService _dialogService;

        private Data _data;

        public MainViewModel(BarnsListViewModel barnsListViewModel,
                             SectionsListViewModel sectionsListViewModel,
                             CowsListViewModel cowsListViewModel,
                             ISavedDataService savedDataService,
                             IDialogService dialogService)
        {
            BarnsListViewModel = barnsListViewModel;
            SectionsListViewModel = sectionsListViewModel;
            CowsListViewModel = cowsListViewModel;
            _savedDataService = savedDataService;
            _dialogService = dialogService;

            BarnSelectionChangedCommand = new RelayCommand(BarnSelectionChangedCommandExecute);
            SectionSelectionChangedCommand = new RelayCommand(SectionSelectionChangedCommandExecute);
            CowSelectionChangedCommand = new RelayCommand(CowSelectionChangedCommandExecute);
            AddBarnCommand = new RelayCommand(AddBarnCommandExecute);
            AddSectionCommand = new RelayCommand(AddSectionCommandExecute);
        }

        public BarnsListViewModel BarnsListViewModel { get; }
        public SectionsListViewModel SectionsListViewModel { get; }
        public CowsListViewModel CowsListViewModel { get; }
        public ICommand BarnSelectionChangedCommand { get; set; }
        public ICommand SectionSelectionChangedCommand { get; set; }
        public ICommand CowSelectionChangedCommand { get; set; }
        public ICommand AddBarnCommand { get; set; }
        public ICommand AddSectionCommand { get; set; }

        public async Task InitializeAsync()
        {
            try
            {
                _data = await _savedDataService.GetDataAsync(_mainCancellationTokenSource.Token);
                BarnsListViewModel.SetBarns(_data.Barns);
            }
            catch (Exception exception)
            {
            }
        }

        private void BarnSelectionChangedCommandExecute(object? obj)
        {
            if (obj is int selectedBarn &&
                _data is not null)
            {
                CowsListViewModel.SetCows(null);
                if (_data.Sections is null)
                {
                    SectionsListViewModel.SetSections(new List<Section>());
                    return;
                }

                var sections = _data.Sections.Where(s => s.BarnNumber == selectedBarn).ToList();
                SectionsListViewModel.SetSections(sections);
            }
        }

        private void SectionSelectionChangedCommandExecute(object? obj)
        {
            if (obj is Section selectedSection &&
                _data is not null)
            {
                if (_data.Cows is null)
                {
                    CowsListViewModel.SetCows(new List<Cow>());
                    return;
                }

                var cows = _data.Cows.Where(c => c.SectionNumber == selectedSection.Number &&
                                                 c.BarnNumber == selectedSection.BarnNumber).ToList();
                CowsListViewModel.SetCows(cows);
            }
        }

        private void CowSelectionChangedCommandExecute(object? obj)
        {

        }

        private void AddBarnCommandExecute(object? obj)
        {
            var dialogViewModel = new AddBarnDialogWindowViewModel(obj is int defaultValue ? defaultValue : null);
            var dialogResult = _dialogService.ShowDialog(dialogViewModel);

            if (dialogResult is null or false ||
                dialogViewModel.BarnNumber is not int)
            {
                return;
            }
            if (_data.Barns is null)
            {
                _data.Barns = new List<int>();
            }

            if (_data.Barns.Any(b => b == dialogViewModel.BarnNumber))
            {
                MessageBox.Show($"Коровник с номером {dialogViewModel.BarnNumber} уже существует.",
                                "Ошибка добавления.",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                AddBarnCommandExecute(dialogViewModel.BarnNumber);
                return;
            }

            _data.Barns.Add((int)dialogViewModel.BarnNumber);
            Task.Factory.StartNew(() =>
                _savedDataService.UpdateDataAsync(_data, _mainCancellationTokenSource.Token));

            BarnsListViewModel.SetBarns(_data.Barns);
        }

        private void AddSectionCommandExecute(object? obj)
        {
            var dialogViewModel = new AddSectionDialogWindowViewModel(
                _data.Barns,
                _data.Groups,
                obj is Section defaultValue ? defaultValue : null);
            var dialogResult = _dialogService.ShowDialog(dialogViewModel);

            if (dialogResult is null or false ||
                dialogViewModel.Number is null ||
                dialogViewModel.BarnNumber is null)
            {
                return;
            }

            var section = new Section()
            {
                Number = (int)dialogViewModel.Number,
                Group = dialogViewModel.Group,
                BarnNumber = (int)dialogViewModel.BarnNumber,
            };

            if (_data.Sections is null)
            {
                _data.Sections = new List<Section>();
            }
            else if (_data.Sections.Any(s => s.BarnNumber == section.BarnNumber &&
                                             s.Number == section.Number))
            {
                MessageBox.Show($"Секция с номером {section.Number} уже существует в коровнике {section.BarnNumber}.",
                                "Ошибка добавления.",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                AddSectionCommandExecute(section);
                return;
            }

            _data.Sections.Add(section);
            Task.Factory.StartNew(() =>
                _savedDataService.UpdateDataAsync(_data, _mainCancellationTokenSource.Token));

            var sections = _data.Sections.Where(s => s.BarnNumber == BarnsListViewModel.SelectedBarn).ToList();
            SectionsListViewModel.SetSections(sections);
        }

        public void Dispose()
        {
            _mainCancellationTokenSource.Dispose();
        }
    }
}

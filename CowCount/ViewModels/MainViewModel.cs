using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly CancellationTokenSource _mainCancellationTokenSource = new();
        private readonly ISavedDataService _savedDataService;

        private Data _data;

        public MainViewModel(BarnsListViewModel barnsListViewModel,
                             SectionsListViewModel sectionsListViewModel,
                             CowsListViewModel cowsListViewModel,
                             ISavedDataService savedDataService)
        {
            BarnsListViewModel = barnsListViewModel;
            SectionsListViewModel = sectionsListViewModel;
            CowsListViewModel = cowsListViewModel;
            _savedDataService = savedDataService;

            BarnSelectionChangedCommand = new RelayCommand(BarnSelectionChangedCommandExecute);
            SectionSelectionChangedCommand = new RelayCommand(SectionSelectionChangedCommandExecute);
            CowSelectionChangedCommand = new RelayCommand(CowSelectionChangedCommandExecute);
        }

        public BarnsListViewModel BarnsListViewModel { get; }
        public SectionsListViewModel SectionsListViewModel { get; }
        public CowsListViewModel CowsListViewModel { get; }
        public ICommand BarnSelectionChangedCommand { get; set; }
        public ICommand SectionSelectionChangedCommand { get; set; }
        public ICommand CowSelectionChangedCommand { get; set; }

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
                if (_data.Sections is null)
                {
                    SectionsListViewModel.SetSections(null);
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
                    CowsListViewModel.SetCows(null);
                    return;
                }

                var cows = _data.Cows.Where(s => s.SectionNumber == selectedSection.Number).ToList();
                CowsListViewModel.SetCows(cows);
            }
        }

        private void CowSelectionChangedCommandExecute(object? obj)
        {

        }

        public void Dispose()
        {
            _mainCancellationTokenSource.Dispose();
        }
    }
}

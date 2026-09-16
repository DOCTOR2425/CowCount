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

        private List<int>? _barns;
        private List<Section>? _sections;
        private List<Cow>? _cows;
        private List<string>? _groups;

        private Data _data;

        public MainViewModel(BarnsListViewModel barnsListViewModel,
                             SectionListViewModel sectionsListViewModel,
                             ISavedDataService savedDataService)
        {
            BarnsListViewModel = barnsListViewModel;
            SectionListViewModel = sectionsListViewModel;
            _savedDataService = savedDataService;

            BarnSelectionChangedCommand = new RelayCommand(BarnSelectionChangedCommandExecute);
            SectionSelectionChangedCommand = new RelayCommand(SectionSelectionChangedCommandExecute);
        }

        public BarnsListViewModel BarnsListViewModel { get; }
        public SectionListViewModel SectionListViewModel { get; }
        public ICommand BarnSelectionChangedCommand { get; set; }
        public ICommand SectionSelectionChangedCommand { get; set; }

        public List<int>? Barns
        {
            get => _barns;
            set
            {
                if (value == _barns)
                {
                    return;
                }

                _barns = value;
                OnPropertyChanged();
            }
        }

        public List<Section>? Sections
        {
            get => _sections;
            set
            {
                if (value == _sections)
                {
                    return;
                }

                _sections = value;
                OnPropertyChanged();
            }
        }

        public List<Cow>? Cows
        {
            get => _cows;
            set
            {
                if (value == _cows)
                {
                    return;
                }

                _cows = value;
                OnPropertyChanged();
            }
        }

        public List<string>? Groups
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
                    SectionListViewModel.SetSections(null);
                    return;
                }

                var sections = _data.Sections.Where(s => s.BarnNumber == selectedBarn).ToList();
                SectionListViewModel.SetSections(sections);
            }
        }

        private void SectionSelectionChangedCommandExecute(object? obj)
        {
            if (obj is Section selectedSection &&
                _data is not null)
            {
                if (_data.Cows is null)
                {

                    return;
                }

                //var cows = _data.Sections.Where(s => s.BarnNumber == selectedBarn).ToList();
                //SectionListViewModel.SetSectionsAsync(cows);
            }
        }

        public void Dispose()
        {
            _mainCancellationTokenSource.Dispose();
        }
    }
}

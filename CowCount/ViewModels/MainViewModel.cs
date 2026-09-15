using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Windows.Controls;
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

        public Section[]? Sections
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

        public Cow[]? Cows
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

        public string[]? Groups
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
                var cancellationToken = _mainCancellationTokenSource.Token;
                
                await BarnsListViewModel.InitializeAsync(cancellationToken);
            }
            catch (Exception exception)
            {
            }
        }

        private void BarnSelectionChangedCommandExecute(object? obj)
        {

        }

        private void SectionSelectionChangedCommandExecute(object? obj)
        {

        }

        public void Dispose()
        {
            _mainCancellationTokenSource.Dispose();
        }
    }
}

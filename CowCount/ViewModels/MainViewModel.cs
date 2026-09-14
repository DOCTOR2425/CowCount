using CowCount.Models;
using CowCount.Services.Interfaces;

namespace CowCount.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly CancellationTokenSource _mainCancellationTokenSource = new();
        private readonly ISavedDataService _savedDataService;

        private int[]? _barns;
        private Section[]? _sections;
        private Cow[]? _cows;
        private string[]? _groups;

        private Data _data;

        public MainViewModel(BarnsListViewModel barnsListViewModel,
                             SectionListViewModel sectionsListViewModel,
                             ISavedDataService savedDataService)
        {
            BarnsListViewModel = barnsListViewModel;
            SectionListViewModel = sectionsListViewModel;
            _savedDataService = savedDataService;
        }

        public BarnsListViewModel BarnsListViewModel { get; }
        public SectionListViewModel SectionListViewModel { get; }

        public int[]? Barns
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

        public void Dispose()
        {
            _mainCancellationTokenSource.Dispose();
        }
    }
}

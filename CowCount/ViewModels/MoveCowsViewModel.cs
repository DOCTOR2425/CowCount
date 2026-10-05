using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class MoveCowsViewModel : ViewModelBase
    {
        private readonly ISavedDataService _savedDataService;

        private CancellationTokenSource _cancellationTokenSource;
        private Data _data;

        public MoveCowsViewModel(ISavedDataService savedDataService)
        {
            _savedDataService = savedDataService;

            SourceBarnSelectionChangedCommand = new RelayCommand(SourceBarnSelectionChangedCommandExecute);
            SourceSectionSelectionChangedCommand = new RelayCommand(SourceSectionSelectionChangedCommandExecute);
        }

        public ICommand SourceBarnSelectionChangedCommand { get; }
        public ICommand SourceSectionSelectionChangedCommand { get; }

        public ObservableCollectionEx<int> SourceBarns { get; } = new();
        public ObservableCollectionEx<Section> SourceSections { get; } = new();
        public ObservableCollectionEx<Cow> SourceCows { get; } = new();
        public ObservableCollectionEx<int> TargetBarns { get; } = new();
        public ObservableCollectionEx<Section> TargetSections { get; } = new();
        public ObservableCollectionEx<Cow> TargetCows { get; } = new();

        public async Task InitializeAsync(CancellationToken parentToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(parentToken);

            try
            {
                _data = await _savedDataService.GetDataAsync(_cancellationTokenSource.Token)
                                               .ConfigureAwait(false);
                if (_data.Barns is not null)
                {
                    SourceBarns.ReplaceRange(_data.Barns);
                }
            }
            catch (Exception exception)
            {
            }
        }

        private void SourceBarnSelectionChangedCommandExecute(object? obj)
        {
            if (_data.Sections is not null &&
                obj is int banrNumber)
            {
                SourceSections
                    .ReplaceRange(_data.Sections
                        .Where(s => s.BarnNumber == banrNumber)
                        .ToList());
            }
        }

        private void SourceSectionSelectionChangedCommandExecute(object? obj)
        {
            if (_data.Cows is not null &&
                obj is Section section)
            {
                SourceCows
                    .ReplaceRange(_data.Cows
                        .Where(c => c.BarnNumber == section.BarnNumber &&
                               c.SectionNumber == section.Number)
                        .ToList());
            }
        }

    }
}

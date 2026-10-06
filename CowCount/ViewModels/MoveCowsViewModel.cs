using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Collections.Frozen;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class MoveCowsViewModel : ViewModelBase
    {
        private readonly ISavedDataService _savedDataService;

        private CancellationTokenSource _cancellationTokenSource;
        private Data _data;
        private Cow? _selectedLeftCow;
        private Cow? _selectedRightCow;
        private int? _selectedBarn;
        private Section? _selectedSection;

        public MoveCowsViewModel(ISavedDataService savedDataService)
        {
            _savedDataService = savedDataService;

            SourceBarnSelectionChangedCommand = new RelayCommand(SourceBarnSelectionChangedCommandExecute);
            SourceSectionSelectionChangedCommand = new RelayCommand(SourceSectionSelectionChangedCommandExecute);
            TargetBarnSelectionChangedCommand = new RelayCommand(TargetBarnSelectionChangedCommandExecute);
            MoveCowRightCommand = new RelayCommand(MoveCowRightCommandExecute);
            MoveCowLeftCommand = new RelayCommand(MoveCowLeftCommandExecute);
            MoveAllCowsRightCommand = new RelayCommand(MoveAllCowsRightCommandExecute);
            MoveAllCowsLeftCommand = new RelayCommand(MoveAllCowsLeftCommandExecute);
            MoveSelectedCowsCommand = new RelayCommand(MoveSelectedCowsCommandExecute);
        }

        public ICommand SourceBarnSelectionChangedCommand { get; }
        public ICommand SourceSectionSelectionChangedCommand { get; }
        public ICommand TargetBarnSelectionChangedCommand { get; }
        public ICommand MoveCowLeftCommand { get; }
        public ICommand MoveCowRightCommand { get; }
        public ICommand MoveAllCowsLeftCommand { get; }
        public ICommand MoveAllCowsRightCommand { get; }
        public ICommand MoveSelectedCowsCommand { get; }

        public ObservableCollectionEx<int> SourceBarns { get; } = new();
        public ObservableCollectionEx<Section> SourceSections { get; } = new();
        public ObservableCollectionEx<Cow> SourceCows { get; } = new();
        public ObservableCollectionEx<int> TargetBarns { get; } = new();
        public ObservableCollectionEx<Section> TargetSections { get; } = new();
        public ObservableCollectionEx<Cow> TargetCows { get; } = new();

        public Cow? SelectedLeftCow
        {
            get => _selectedLeftCow;
            set
            {
                if (value == _selectedLeftCow)
                {
                    return;
                }

                _selectedLeftCow = value;
                OnPropertyChanged();
            }
        }

        public Cow? SelectedRightCow
        {
            get => _selectedRightCow;
            set
            {
                if (value == _selectedRightCow)
                {
                    return;
                }

                _selectedRightCow = value;
                OnPropertyChanged();
            }
        }

        public int? SelectedTargetBarn
        {
            get => _selectedBarn;
            set
            {
                if (value == _selectedBarn)
                {
                    return;
                }

                _selectedBarn = value;
                OnPropertyChanged();
            }
        }

        public Section? SelectedTargetSection
        {
            get => _selectedSection;
            set
            {
                if (value == _selectedSection)
                {
                    return;
                }

                _selectedSection = value;
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
                if (_data.Barns is not null)
                {
                    SourceBarns.ReplaceRange(_data.Barns);
                    TargetBarns.ReplaceRange(_data.Barns);
                }
            }
            catch (Exception)
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

        private void TargetBarnSelectionChangedCommandExecute(object? obj)
        {
            if (_data.Sections is not null &&
                obj is int banrNumber)
            {
                TargetSections
                    .ReplaceRange(_data.Sections
                        .Where(s => s.BarnNumber == banrNumber)
                        .ToList());
            }
        }

        private void MoveCowLeftCommandExecute(object? obj)
        {
            if (SelectedRightCow is null)
            {
                return;
            }

            TargetCows.Remove(SelectedRightCow);
        }

        private void MoveCowRightCommandExecute(object? obj)
        {
            if (SelectedLeftCow is null ||
                TargetCows.Contains(SelectedLeftCow))
            {
                return;
            }

            TargetCows.Add(SelectedLeftCow);
        }

        private void MoveAllCowsLeftCommandExecute(object? obj)
        {
            TargetCows.Clear();
        }

        private void MoveAllCowsRightCommandExecute(object? obj)
        {
            if (SourceCows is null)
            {
                return;
            }

            foreach (var cow in SourceCows)
            {
                if (TargetCows.Contains(cow) == false)
                {
                    TargetCows.Add(cow);
                }
            }
        }

        private void MoveSelectedCowsCommandExecute(object? obj)
        {
            if (TargetCows.Any() == false ||
                _data.Cows is null ||
                _data.Cows.Any() == false ||
                SelectedTargetBarn == null ||
                SelectedTargetSection == null)
            {
                return;
            }

            foreach (var cow in TargetCows)
            {
                var target = _data.Cows.FirstOrDefault(c => c.Number == cow.Number);
                if (target is not null)
                {
                    target.BarnNumber = (int)SelectedTargetBarn;
                    target.SectionNumber = (int)SelectedTargetSection.Number;
                }
            }

            _ = _savedDataService.UpdateDataAsync(_data, _cancellationTokenSource.Token);
        }
    }
}

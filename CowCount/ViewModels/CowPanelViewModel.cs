using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class CowPanelViewModel : ViewModelBase
    {
        private ISavedDataService _savedDataService;
        private IDialogService _dialogService;
        private CancellationTokenSource _cancellationTokenSource;

        private Data _data;
        private Cow? _cow;
        private Cow? _changedCow;
        private bool _isCowInChangeState;
        private List<string>? _groups;

        public CowPanelViewModel(ISavedDataService savedDataService,
                                 IDialogService dialogService)
        {
            _savedDataService = savedDataService;
            _dialogService = dialogService;

            ChangeCowCommand = new RelayCommand(ChangeCowCommandExecute);
            SaveChangesCowCommand = new RelayCommand(SaveChangesCowCommandExecute);
            CancelChangesCowCommand = new RelayCommand(CancelChangesCowCommandExecute);
        }

        public ICommand ChangeCowCommand { get; set; }
        public ICommand CancelChangesCowCommand { get; set; }
        public ICommand SaveChangesCowCommand { get; set; }

        public Cow? Cow
        {
            get => _cow;
            set
            {
                if (value == _cow)
                {
                    return;
                }

                _cow = value;
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

        public Cow? ChangedCow
        {
            get => _changedCow;
            set
            {
                if (value == _changedCow)
                {
                    return;
                }

                _changedCow = value;
                OnPropertyChanged();
            }
        }

        public bool IsCowInChangeState
        {
            get => _isCowInChangeState;
            set
            {
                if (value == _isCowInChangeState)
                {
                    return;
                }

                _isCowInChangeState = value;
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
                    Groups = [.. _data.Groups, string.Empty];
                }
            }
            catch (Exception)
            {
            }
        }

        public void SetCow(Cow? cow)
        {
            Cow = DeepCowCopy(cow);
            ChangedCow = DeepCowCopy(cow);
        }

        public Cow? DeepCowCopy(Cow? item)
        {
            if (item is null)
            {
                return null;
            }

            string json = JsonSerializer.Serialize(item);
            return JsonSerializer.Deserialize<Cow>(json);
        }

        private void ChangeCowCommandExecute(object? obj)
        {
            IsCowInChangeState = true;
        }

        private void SaveChangesCowCommandExecute(object? obj)
        {
            if (Cow is null ||
                ChangedCow is null ||
                _data.Cows is null)
            {
                return;
            }
            var cowToUpdate = _data.Cows.FirstOrDefault(c => c.Number == Cow.Number);


            if (cowToUpdate is null)
            {
                return;
            }
            cowToUpdate.BarnNumber = ChangedCow.BarnNumber;
            cowToUpdate.SectionNumber = ChangedCow.SectionNumber;
            cowToUpdate.Group = ChangedCow.Group;
            cowToUpdate.Gender = ChangedCow.Gender;
            cowToUpdate.Breed = ChangedCow.Breed;
            cowToUpdate.Imported = ChangedCow.Imported;
            cowToUpdate.Note = ChangedCow.Note;
            cowToUpdate.Birthday = ChangedCow.Birthday;
            cowToUpdate.DeathDate = ChangedCow.DeathDate;
            cowToUpdate.LastInseminationDate = ChangedCow.LastInseminationDate;
            cowToUpdate.LastCalvingDate = ChangedCow.LastCalvingDate;
            cowToUpdate.SpermDonorNickname = ChangedCow.SpermDonorNickname;

            if (ChangedCow.ActionLog is not null)
            {
                //cowToUpdate.ActionLog = ChangedCow.ActionLog
                //    .Select(action => action.Clone())
                //    .ToList();

                cowToUpdate.ActionLog = [.. ChangedCow.ActionLog];
            }
            else
            {
                cowToUpdate.ActionLog = null;
            }

            IsCowInChangeState = false;

            _ = _savedDataService.UpdateDataAsync(_data, _cancellationTokenSource.Token);
        }

        private void CancelChangesCowCommandExecute(object? obj)
        {
            SetCow(Cow);
            IsCowInChangeState = false;
        }
    }
}

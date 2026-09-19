using CowCount.Commands;
using CowCount.Models;
using CowCount.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class GroupsViewModel : ViewModelBase
    {
        private readonly ISavedDataService _savedDataService;
        private readonly IDialogService _dialogService;
        private Data _data;

        public ObservableCollection<string> Groups { get; set; }

        public ICommand AddGroupCommand { get; }
        public ICommand EditGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }

        public GroupsViewModel(ISavedDataService savedDataService,
                               IDialogService dialogService)
        {
            _savedDataService = savedDataService;
            _dialogService = dialogService;

            //AddGroupCommand = new RelayCommand(AddGroupCommandExecute);
            //EditGroupCommand = new RelayCommand(EditGroupCommandExecute);
            //DeleteGroupCommand = new RelayCommand(DeleteGroupCommandExecute);
        }

        public void SetData(Data data)
        {
            _data = data;
            Groups = new ObservableCollection<string>(data.Groups ?? new List<string>());
        }
    }
}

using CowCount.ModalWindow;
using CowCount.Models;

namespace CowCount.ViewModels
{
    public class AddSectionDialogWindowViewModel : ViewModelBase, IModalWindowSettings
    {
        private int? _number;
        private string? _group;
        private int? _barnNumber;
        private List<int>? _existingBarns;
        private List<string>? _existingGroups;

        public AddSectionDialogWindowViewModel(
            List<int>? existingBarns,
            List<string>? existingGroups,
            Section? defaultValue = null)
        {
            ModalWindowSettings = new ModalWindowSettings
            {
                Title = "Добавить секцию",
                CanResize = false,
                MinWidth = 200,
                MinHeight = 200,
                Height = 200,
                Width = 200
            };

            _existingBarns = existingBarns;
            _existingGroups = existingGroups;

            if(defaultValue is not null)
            {
                Number = defaultValue.Number;
                Group = defaultValue.Group;
                BarnNumber = defaultValue.BarnNumber;
            }
        }

        public ModalWindowSettings ModalWindowSettings { get; }

        public int? Number
        {
            get => _number;
            set
            {
                if (value == _number)
                {
                    return;
                }

                _number = value;
                OnPropertyChanged();
            }
        }

        public string? Group
        {
            get => _group;
            set
            {
                if (value == _group)
                {
                    return;
                }

                _group = value;
                OnPropertyChanged();
            }
        }

        public int? BarnNumber
        {
            get => _barnNumber;
            set
            {
                if (value == _barnNumber)
                {
                    return;
                }

                _barnNumber = value;
                OnPropertyChanged();
            }
        }

        public List<int>? ExistingBarns
        {
            get => _existingBarns;
            set
            {
                if (value == _existingBarns)
                {
                    return;
                }

                _existingBarns = value;
                OnPropertyChanged();
            }
        }

        public List<string>? ExistingGroups
        {
            get => _existingGroups;
            set
            {
                if (value == _existingGroups)
                {
                    return;
                }

                _existingGroups = value;
                OnPropertyChanged();
            }
        }
    }
}

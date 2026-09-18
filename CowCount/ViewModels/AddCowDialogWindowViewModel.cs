using CowCount.Commands;
using CowCount.ModalWindow;
using CowCount.Models;
using System.Windows.Input;

namespace CowCount.ViewModels
{
    public class AddCowDialogWindowViewModel : ViewModelBase, IModalWindowSettings
    {
        private int? _number;
        private string? _name;
        private Section? _section;
        private int? _barnNumber;
        private string? _group;
        private string? _note;

        private List<int>? _existingBarns;
        private List<Section>? _existingSections;
        private List<Section>? _sectionsInBarn;
        private List<string>? _existingGroups;

        public AddCowDialogWindowViewModel(
            List<int>? existingBarns,
            List<Section>? existingSections,
            List<string>? existingGroups,
            Cow? defaultValue = null)
        {
            ModalWindowSettings = new ModalWindowSettings
            {
                Title = "Добавить корову",
                CanResize = false,
                MinWidth = 200,
                MinHeight = 300,
                Height = 300,
                Width = 200
            };

            ExistingBarns = new(existingBarns);
            ExistingSections = new(existingSections);

            if (existingGroups is not null)
            {
                ExistingGroups = new(existingGroups);
                ExistingGroups.Add(string.Empty);
            }

            if (defaultValue is not null)
            {
                Number = defaultValue.Number;
                BarnNumber = defaultValue.BarnNumber;
                Name = defaultValue.Name;
                Group = defaultValue.Group;
                Note = defaultValue.Note;

                if (ExistingSections is not null)
                {
                    Section = ExistingSections.FirstOrDefault(s =>
                        s.BarnNumber == defaultValue.BarnNumber &&
                        s.Number == defaultValue.SectionNumber);
                }
            }

            BarnSelectionChangedCommand = new RelayCommand(BarnSelectionChangedCommandExecute);
        }

        public ModalWindowSettings ModalWindowSettings { get; }

        public ICommand BarnSelectionChangedCommand { get; }

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

        public string? Name
        {
            get => _name;
            set
            {
                if (value == _name)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged();
            }
        }

        public Section? Section
        {
            get => _section;
            set
            {
                if (value == _section)
                {
                    return;
                }

                _section = value;
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

        public string? Note
        {
            get => _note;
            set
            {
                if (value == _note)
                {
                    return;
                }

                _note = value;
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

        public List<Section>? SectionsInBarn
        {
            get => _sectionsInBarn;
            set
            {
                if (value == _sectionsInBarn)
                {
                    return;
                }

                _sectionsInBarn = value;
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

        public List<Section>? ExistingSections
        {
            get => _existingSections;
            set
            {
                if (value == _existingSections)
                {
                    return;
                }

                _existingSections = value;
                OnPropertyChanged();
            }
        }

        private void BarnSelectionChangedCommandExecute(object? obj)
        {
            if (ExistingSections is not null &&
                obj is int banrNumber)
            {
                SectionsInBarn = ExistingSections
                    .Where(s => s.BarnNumber == banrNumber)
                    .ToList();
            }
        }
    }
}

using CowCount.ModalWindow;
using CowCount.Models;

namespace CowCount.ViewModels
{
    public class MoveCowsDialogWindowViewModel : ViewModelBase, IModalWindowSettings
    {
        private List<Cow>? _cows;
        private List<int>? _barns;
        private List<Section>? _sections;

        private int? _selectedBarn;
        private Section? _selectedSection;

        public MoveCowsDialogWindowViewModel(List<Cow>? cows, 
                                             List<int>? barns, 
                                             List<Section>? sections)
        {
            ModalWindowSettings = new ModalWindowSettings
            {
                Title = "Переместить коров",
                CanResize = false,
                MinWidth = 600,
                MinHeight = 400,
                Height = 400,
                Width = 600
            };

            Cows = cows;
            Barns = barns;
            Sections = sections;
        }

        public ModalWindowSettings ModalWindowSettings { get; }

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

        public int? SelectedBarn
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

        public Section? SelectedSection
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
    }
}

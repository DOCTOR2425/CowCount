using CowCount.Models;

namespace CowCount.ViewModels
{
    public class SectionsListViewModel : ViewModelBase
    {
        private ObservableCollectionEx<Section>? _sections;
        private Section? _selectedSection;

        public ObservableCollectionEx<Section>? Sections
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

        public void SetSections(List<Section>? sections)
        {
            if (sections is null)
            {
                Sections = null;
                return;
            }
            Sections = new ObservableCollectionEx<Section>(sections);
        }
    }
}

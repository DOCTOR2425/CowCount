using CowCount.Models;

namespace CowCount.ViewModels
{
    public class SectionsListViewModel : ViewModelBase
    {
        private Section? _selectedSection;

        public ObservableCollectionEx<Section> Sections { get; } = new();

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
                Sections.Clear();
                return;
            }
            Sections.ReplaceRange(sections);
        }
    }
}

using CowCount.Models;
using CowCount.Services.Interfaces;

namespace CowCount.ViewModels
{
    public class SectionListViewModel : ViewModelBase
    {
        private List<Section>? _sections;
        private Section? _selectedSection;

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
            Sections = sections;
        }
    }
}

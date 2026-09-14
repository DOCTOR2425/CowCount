using CowCount.Models;
using CowCount.Services.Interfaces;

namespace CowCount.ViewModels
{
    public class SectionListViewModel : ViewModelBase
    {
        private readonly ISavedDataService _savedDataService;
        private Section[]? _sections;
        private Section? _selectedSection;
        private Data _data;

        public SectionListViewModel(ISavedDataService savedDataService)
        {
            _savedDataService = savedDataService;
        }

        public Section[]? Sections
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
    }
}

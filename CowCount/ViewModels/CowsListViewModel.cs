using CowCount.Models;

namespace CowCount.ViewModels
{
    public class CowsListViewModel : ViewModelBase
    {
        private List<Cow>? _cows;
        private Cow? _selectedCow;

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

        public Cow? SelectedCow
        {
            get => _selectedCow;
            set
            {
                if (value == _selectedCow)
                {
                    return;
                }

                _selectedCow = value;
                OnPropertyChanged();
            }
        }

        public void SetSections(List<Cow>? cows)
        {
            Cows = cows;
        }
    }
}

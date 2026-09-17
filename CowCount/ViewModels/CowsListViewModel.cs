using CowCount.Models;

namespace CowCount.ViewModels
{
    public class CowsListViewModel : ViewModelBase
    {
        private ObservableCollectionEx<Cow>? _cows;
        private Cow? _selectedCow;

        public ObservableCollectionEx<Cow>? Cows
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

        public void SetCows(List<Cow>? cows)
        {
            if (cows is null)
            {
                Cows = null;
                return;
            }
            Cows = new ObservableCollectionEx<Cow>(cows);
        }
    }
}

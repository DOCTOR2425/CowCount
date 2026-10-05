using CowCount.Models;

namespace CowCount.ViewModels
{
    public class CowsListViewModel : ViewModelBase
    {
        private Cow? _selectedCow;

        public ObservableCollectionEx<Cow> Cows { get; } = new();

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
                Cows.Clear();
                return;
            }
            Cows.ReplaceRange(cows);
        }
    }
}

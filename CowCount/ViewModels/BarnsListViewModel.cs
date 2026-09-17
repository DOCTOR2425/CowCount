namespace CowCount.ViewModels
{
    public class BarnsListViewModel : ViewModelBase
    {
        private ObservableCollectionEx<int>? _barns;
        private int? _selectedBarn;

        public ObservableCollectionEx<int>? Barns
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

        public void SetBarns(List<int>? barns)
        {
            if (barns is null)
            {
                Barns = null;
                return;
            }
            Barns = new ObservableCollectionEx<int>(barns);
        }
    }
}

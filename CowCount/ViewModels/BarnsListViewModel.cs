namespace CowCount.ViewModels
{
    public class BarnsListViewModel : ViewModelBase
    {
        private int? _selectedBarn;

        public ObservableCollectionEx<int> Barns { get; } = new();

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
                Barns.Clear();
                return;
            }
            Barns.ReplaceRange(barns);
        }
    }
}

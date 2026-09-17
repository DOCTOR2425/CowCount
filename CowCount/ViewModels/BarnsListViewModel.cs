namespace CowCount.ViewModels
{
    public class BarnsListViewModel : ViewModelBase
    {
        private List<int>? _barns;
        private int? _selectedBarn;

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
            Barns = barns;
        }
    }
}

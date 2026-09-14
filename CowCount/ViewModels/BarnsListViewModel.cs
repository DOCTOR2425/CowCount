using CowCount.Models;
using CowCount.Services.Interfaces;

namespace CowCount.ViewModels
{
    public class BarnsListViewModel : ViewModelBase
    {
        private readonly ISavedDataService _savedDataService;
        private int[]? _barns;
        private int? _selectedBarn;
        private Data _data;

        public BarnsListViewModel(ISavedDataService savedDataService)
        {
            _savedDataService = savedDataService;
        }

        public int[]? Barns
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

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            try
            {
                _data = await _savedDataService.GetDataAsync(cancellationToken);

                if (_data.Barns is not null)
                {
                    Barns = _data.Barns;
                }
            }
            catch (Exception exception)
            {
            }
        }
    }
}

using CowCount.Models;
using CowCount.Services.Interfaces;

namespace CowCount.ViewModels
{
    public class CowPanelViewModel : ViewModelBase
    {
        private Cow? _cow { get; set; }
        private ISavedDataService _savedDataService;

        private CancellationTokenSource _cancellationTokenSource;
        private Data _data;

        public CowPanelViewModel(ISavedDataService savedDataService)
        {
            _savedDataService = savedDataService;
        }

        public Cow? Cow
        {
            get => _cow;
            set
            {
                if (value == _cow)
                {
                    return;
                }

                _cow = value;
                OnPropertyChanged();
            }
        }

        public async Task InitializeAsync(CancellationToken parentToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(parentToken);

            try
            {
                _data = await _savedDataService.GetDataAsync(_cancellationTokenSource.Token)
                                               .ConfigureAwait(false);
            }
            catch (Exception)
            {
            }
        }

        public void SetCow(Cow? cow)
        {
            Cow = cow;
        }
    }
}

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
        private CancellationTokenSource? _loadingCancellationTokenSource;

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

        public async Task SetSectionAsync(Section[]? sections, CancellationToken cancellationToken)
        {
            if (sections is null)
            {
                return;
            }

            try
            {
                if (_loadingCancellationTokenSource is not null)
                {
                    await _loadingCancellationTokenSource.CancelAsync().ConfigureAwait(false);
                    _loadingCancellationTokenSource.Dispose();
                }
                _loadingCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                Sections = sections;
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (Exception ex)
            {
            }
        }

        public void Dispose()
        {
            _loadingCancellationTokenSource?.Cancel();
            _loadingCancellationTokenSource?.Dispose();
            _loadingCancellationTokenSource = null;
        }
    }
}

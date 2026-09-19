namespace CowCount.ViewModels
{
    public class TabsViewModel : ViewModelBase, IDisposable
    {
        private readonly CancellationTokenSource _mainCancellationTokenSource = new();

        public TabsViewModel(MainViewModel mainViewModel,
                             GroupsViewModel groupsViewModel)
        {
            MainViewModel = mainViewModel;
            GroupsViewModel = groupsViewModel;
        }

        public MainViewModel MainViewModel { get; }
        public GroupsViewModel GroupsViewModel { get; }

        public int SelectedTabIndex { get; set; }

        public async Task InitializeAsync()
        {
            await MainViewModel.InitializeAsync(_mainCancellationTokenSource.Token);
            await GroupsViewModel.InitializeAsync(_mainCancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _mainCancellationTokenSource.Cancel();
            _mainCancellationTokenSource.Dispose();
        }
    }
}

namespace CowCount.ViewModels
{
    public class TabsViewModel : ViewModelBase, IDisposable
    {
        public MainViewModel MainViewModel { get; }
        public GroupsViewModel GroupsViewModel { get; }

        public int SelectedTabIndex { get; set; }

        public TabsViewModel(MainViewModel mainViewModel,
                             GroupsViewModel groupsViewModel)
        {
            MainViewModel = mainViewModel;
            GroupsViewModel = groupsViewModel;
        }

        public void Dispose()
        {
            //_mainCancellationTokenSource.Dispose();
        }
    }
}

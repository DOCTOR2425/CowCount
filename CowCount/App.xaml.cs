using Autofac;
using CowCount.Services.Implementations;
using CowCount.Services.Interfaces;
using CowCount.ViewModels;
using System.Windows;

namespace CowCount
{
    public partial class App : Application
    {
        private ILifetimeScope? _applicationScope;

        public static IContainer? Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();

            builder.RegisterType<SavedDataService>()
                   .As<ISavedDataService>()
                   .SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<BarnsListViewModel>().AsSelf();
            builder.RegisterType<SectionsListViewModel>().AsSelf();
            builder.RegisterType<CowsListViewModel>().AsSelf();

            Container = builder.Build();
            _applicationScope = Container.BeginLifetimeScope();

            var mainViewModel = _applicationScope.Resolve<MainViewModel>();
            _ = mainViewModel.InitializeAsync();

            var window = _applicationScope.Resolve<MainWindow>();
            window.DataContext = mainViewModel;
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _applicationScope?.Dispose();
            Container?.Dispose();
            base.OnExit(e);
        }
    }
}

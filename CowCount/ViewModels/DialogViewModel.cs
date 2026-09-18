using CowCount.Commands;
using CowCount.ModalWindow;
using System.Windows;
using System.Windows.Input;

namespace CowCount.ViewModels
{// TODO Сделать невозможным нажатие кнопки OK при невыполнении условия _canExecute для OkCommand = new RelayCommand(OkCommandExecute);
    public class DialogViewModel : ViewModelBase
    {
        private const double DefaultWidth = 800;
        private const double DefaultMinWidth = 600;
        private const double DefaultMaxWidth = 800;
        private const double DefaultHeight = 600;
        private const double DefaultMinHeight = 400;
        private const double DefaultMaxHeight = 600;
        private readonly Action _dialogResultAction;
        private object? _contentViewModel;
        private ResizeMode _resizeMode;
        private string? _title;

        public DialogViewModel(object contentViewModel, Action closeDialogAction)
        {
            if (contentViewModel is IModalWindowSettings contentWithSettings)
            {
                Title = contentWithSettings.ModalWindowSettings.Title;

                if (contentWithSettings.ModalWindowSettings.CanResize)
                {
                    ResizeMode = ResizeMode.CanResizeWithGrip;
                }

                Width = contentWithSettings.ModalWindowSettings.Width ?? DefaultWidth;
                MinWidth = contentWithSettings.ModalWindowSettings.MinWidth ?? DefaultMinWidth;
                MaxWidth = contentWithSettings.ModalWindowSettings.MaxWidth ?? DefaultMaxWidth;

                Height = contentWithSettings.ModalWindowSettings.Height ?? DefaultHeight;
                MinHeight = contentWithSettings.ModalWindowSettings.MinHeight ?? DefaultMinHeight;
                MaxHeight = contentWithSettings.ModalWindowSettings.MaxHeight ?? DefaultMaxHeight;
            }

            ContentViewModel = contentViewModel;
            _dialogResultAction = closeDialogAction;
            CancelCommand = new RelayCommand(CancelCommandExecute);
            OkCommand = new RelayCommand(OkCommandExecute);
        }

        public string? Title
        {
            get => _title;
            set
            {
                if (value == _title)
                {
                    return;
                }

                _title = value;
                OnPropertyChanged();
            }
        }

        public ResizeMode ResizeMode
        {
            get => _resizeMode;
            private set
            {
                if (value == _resizeMode)
                {
                    return;
                }

                _resizeMode = value;
                OnPropertyChanged();
            }
        }

        public double Width { get; }

        public double MinWidth { get; }

        public double MaxWidth { get; }

        public double Height { get; }

        public double MinHeight { get; }

        public double MaxHeight { get; }

        public object? ContentViewModel
        {
            get => _contentViewModel;
            private set
            {
                if (Equals(value, _contentViewModel))
                {
                    return;
                }

                _contentViewModel = value;
                OnPropertyChanged();
            }
        }

        public bool? DialogResult { get; private set; }

        public ICommand CancelCommand { get; }

        public ICommand OkCommand { get; }

        private void CancelCommandExecute(object? obj)
        {
            DialogResult = false;
            _dialogResultAction.Invoke();
        }

        private void OkCommandExecute(object? obj)
        {
            DialogResult = true;
            _dialogResultAction.Invoke();
        }
    }
}

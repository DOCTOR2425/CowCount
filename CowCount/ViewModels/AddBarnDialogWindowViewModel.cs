using CowCount.ModalWindow;

namespace CowCount.ViewModels
{
    public class AddBarnDialogWindowViewModel : ViewModelBase, IModalWindowSettings
    {
        private int? _barnNumber;

        public AddBarnDialogWindowViewModel(int? defaultValue = null)
        {
            ModalWindowSettings = new ModalWindowSettings
            {
                Title = "Добавить коровник",
                CanResize = false,
                MinWidth = 200,
                MinHeight = 100,
                Height = 100,
                Width = 200
            };

            _barnNumber = defaultValue;
        }

        public ModalWindowSettings ModalWindowSettings { get; }

        public int? BarnNumber
        {
            get => _barnNumber;
            set
            {
                if (value == _barnNumber)
                {
                    return;
                }

                _barnNumber = value;
                OnPropertyChanged();
            }
        }
    }
}

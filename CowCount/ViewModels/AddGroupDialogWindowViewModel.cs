using CowCount.ModalWindow;

namespace CowCount.ViewModels
{
    public class AddGroupDialogWindowViewModel : ViewModelBase, IModalWindowSettings
    {
        private string? _groupName;
        public AddGroupDialogWindowViewModel(string? defaultValue = null)
        {
            ModalWindowSettings = new ModalWindowSettings
            {
                Title = "Добавить группу",
                CanResize = false,
                MinWidth = 200,
                MinHeight = 100,
                Height = 100,
                Width = 200
            };

            if (defaultValue is not null)
            {
                GroupName = defaultValue;
            }
        }

        public ModalWindowSettings ModalWindowSettings { get; }

        public string? GroupName
        {
            get => _groupName;
            set
            {
                if (value == _groupName)
                {
                    return;
                }

                _groupName = value;
                OnPropertyChanged();
            }
        }
    }
}

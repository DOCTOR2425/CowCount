using System.Windows.Input;

namespace CowCount.Commands
{
    /// <summary>
    /// Реализация команды.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Func<object?, bool> _canExecute;
        private readonly Action<object?> _execute;

        public RelayCommand(Action<object?> execute, Func<object?, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object?> execute)
        {
            _execute = execute;
            _canExecute = _ => true;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}

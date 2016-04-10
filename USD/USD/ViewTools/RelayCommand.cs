using System;
using System.Windows.Input;

namespace USD.ViewTools
{
    internal class RelayCommand : ICommand
    {
        public RelayCommand(Action<object> action, Predicate<object> canExecutePredicate = null)
        {
            ExecuteDelegate = action;
            CanExecuteDelegate = canExecutePredicate;
        }

        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(parameter);
            }
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(parameter);
            }
        }
    }
}
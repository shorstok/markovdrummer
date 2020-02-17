using System;
using System.Windows.Input;

namespace markov_drummer.Vm
{
    public class DelegateCommand : ICommand
    {
        readonly Func<object, bool> _canExecute;
        readonly Action<object> _execute;

        
        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        
        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        
        
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }
    }
}

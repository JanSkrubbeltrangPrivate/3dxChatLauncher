using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VM.Models
{
    internal class DelegateCommandResolverAsync: ICommand
    {
        private readonly Func<Task>? execute;
        private readonly Func<bool>? canExecute;

        public DelegateCommandResolverAsync(Func<Task>? execute) : this(execute, null)
        {
        }

        public DelegateCommandResolverAsync(Func<Task>? execute, Func<bool>? canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object? parameter)
        {
            execute?.Invoke();
        }
    }
}

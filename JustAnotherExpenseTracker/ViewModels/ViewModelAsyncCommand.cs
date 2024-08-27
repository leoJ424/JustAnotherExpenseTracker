using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class ViewModelAsyncCommand : ICommand
    {
        private readonly Func<object?, Task> _executeAsync;
        private readonly Predicate<object?>? _canExecute;
        private bool _isExecuting;

        public ViewModelAsyncCommand(Func<object?, Task> executeAsync, Predicate<object?>? canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async void Execute(object? parameter)
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await _executeAsync(parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

    }
}

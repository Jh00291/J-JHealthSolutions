using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A command that can be bound to UI elements, allowing them to trigger actions with or without specific conditions.
    /// Implements the ICommand interface.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;            // The action to execute.
        private readonly Predicate<object> _canExecute;      // The condition to determine if the command can execute.

        /// <summary>
        /// Initializes a new instance of the RelayCommand class with the specified execute action.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class with the specified execute action and canExecute condition.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked</param>
        /// <param name="canExecute">The condition that determines if the command can execute</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command, can be null</param>
        /// <returns>True if the command can execute, otherwise false</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Occurs when changes in the execution conditions of the command take place.
        /// WPF subscribes to this event to know when to re-query CanExecute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command, can be null</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

}

using System;
using System.Windows.Input;

namespace BitrixComponentsAnalizer.Commands
{
    internal sealed class AnalizeCommand: ICommand
    {
        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}

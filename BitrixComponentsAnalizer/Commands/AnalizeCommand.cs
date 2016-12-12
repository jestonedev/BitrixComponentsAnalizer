using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using BitrixComponentsAnalizer.BitrixInfrastructure.ValueObjects;

namespace BitrixComponentsAnalizer.Commands
{
    public sealed class AnalizeCommand: ICommand
    {
        private readonly Action<IEnumerable<BitrixComponent>> _onComplete;
        private readonly Action<Exception> _onError;
        public delegate Task<IEnumerable<BitrixComponent>> AnalizeAsyncAction();

        private readonly AnalizeAsyncAction _analizeAsyncAction;

        private bool _inProcessing;

        public AnalizeCommand(
            AnalizeAsyncAction analizeAsyncAction, 
            Action<IEnumerable<BitrixComponent>> onComplete, Action<Exception> onError)
        {
            if (analizeAsyncAction == null)
            {
                throw new ArgumentNullException("onComplete");
            }
            if (onComplete == null)
            {
                throw new ArgumentNullException("onComplete");
            }
            if (onError == null)
            {
                throw new ArgumentNullException("onError");
            }
            _onComplete = onComplete;
            _analizeAsyncAction = analizeAsyncAction;
            _onError = onError;
        }

        public void Execute(object parameter)
        {
            _inProcessing = true;
            RaiseCanExecuteChanged();
            var task = _analizeAsyncAction();
            task.GetAwaiter().OnCompleted(() =>
            {
                if (task.Exception != null)
                {
                    _onError(task.Exception);
                }
                else
                {
                    _onComplete(task.Result);
                }
                _inProcessing = false;
                RaiseCanExecuteChanged();
            });
        }

        public bool CanExecute(object parameter)
        {
            return !_inProcessing;
        }

        private void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, new System.EventArgs());
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}

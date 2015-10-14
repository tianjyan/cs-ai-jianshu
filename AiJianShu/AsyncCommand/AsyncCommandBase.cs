using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AiJianShu.Command
{
    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);
        public abstract Task ExecuteAsync(object parameter);
        public async Task Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}

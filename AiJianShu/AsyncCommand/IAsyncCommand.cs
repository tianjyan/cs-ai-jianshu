using System.Threading.Tasks;
using System.Windows.Input;

namespace AiJianShu.Command
{
    public interface IAsyncCommand : ICommand
    {
         Task ExecuteAsync(object parameter);
    }
}

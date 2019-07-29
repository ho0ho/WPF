using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeditSmile2D.ViewModel.Command
{
    public class MainViewCommand : ICommand
    {
        public Action _executeMethod;
        public event EventHandler CanExecuteChanged;

        public MainViewCommand(Action executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeMethod.Invoke();
        }
    }
}

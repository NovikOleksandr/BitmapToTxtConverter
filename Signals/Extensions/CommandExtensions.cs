using System.Windows.Input;

namespace Signals.Extensions
{
    public static class CommandExtensions
    {
        public static void CheckAndExecute(this ICommand command, object commandParameter)
        {
            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
    }
}

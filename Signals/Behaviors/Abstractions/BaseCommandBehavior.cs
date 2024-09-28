using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using System.Windows.Input;

namespace Signals.Behaviors.Abstractions
{
    public abstract class BaseCommandBehavior<TDependencyObject> : Behavior<TDependencyObject>
        where TDependencyObject : DependencyObject
    {
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
    }
}

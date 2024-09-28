using Microsoft.UI.Xaml;
using Signals.Behaviors.Abstractions;
using Signals.Extensions;

namespace Signals.Behaviors.Common
{
    public sealed class LoadingCommandBehavior : BaseCommandBehavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loading += OnLoading;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loading -= OnLoading;
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            Command.CheckAndExecute(CommandParameter);
        }

    }
}

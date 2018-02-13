using navappwpf.Common;
using NmeaParser.Helper;

namespace navappwpf.ViewModels
{
    public class WindViewModel : ViewModelBase
    {
        public WindViewModel(NavigationDisplay navigateDisplay) : base(new DispatcherWrapper())
        {
            Navigatedisplay = navigateDisplay;
        }

        public override void ExecutePreviousCommand()
        {
            base.ExecutePreviousCommand();
        }

        public override void ExecuteNextCommand()
        {
            base.ExecuteNextCommand();
        }
    }
}

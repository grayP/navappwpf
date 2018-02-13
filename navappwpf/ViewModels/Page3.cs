
using navappwpf.Common;
using NmeaParser.Helper;

namespace navappwpf.ViewModels
{
    public class Page3 : ViewModelBase
    {
        public Page3(NavigationDisplay navigateDisplay) : base(new DispatcherWrapper())
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

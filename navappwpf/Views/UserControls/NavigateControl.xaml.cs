using NmeaParser.Helper;
using System.Windows;
using System.Windows.Controls;

namespace navappwpf.Views.UserControls
{
    /// <summary>
    /// Interaction logic for NavigateControl.xaml
    /// </summary>
    public partial class NavigateControl : UserControl
    {
        public NavigateControl()
        {
            InitializeComponent();
        }

        public NavigationDisplay Message
        {
            get { return (NavigationDisplay)GetValue(NavigateProperty); }
            set { SetValue(NavigateProperty, value); }
        }

        public static readonly DependencyProperty NavigateProperty = DependencyProperty.Register("Message", typeof(NavigationDisplay), typeof(NavigateControl), new PropertyMetadata(null));

    }
}

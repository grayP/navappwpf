using System.Windows;
using System.Windows.Controls;

namespace navappwpf.Views.UserControls
{
    /// <summary>
    /// Interaction logic for KeyValuePairControl.xaml
    /// </summary>
    public partial class KeyValuePairControl : UserControl
    {
        public KeyValuePairControl()
        {
            InitializeComponent();
        }
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(KeyValuePairControl), new PropertyMetadata(null));
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(KeyValuePairControl), new PropertyMetadata(null));
    }
}

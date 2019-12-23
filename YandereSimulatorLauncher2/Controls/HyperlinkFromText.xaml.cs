using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YandereSimulatorLauncher2.Controls
{
    /// <summary>
    /// Interaction logic for HyperlinkFromText.xaml
    /// </summary>
    public partial class HyperlinkFromText : UserControl
    {
        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof(string), typeof(HyperlinkFromText), new PropertyMetadata(string.Empty, DisplayTextChanged));
        
        public string DisplayText
        {
            get
            {
                return (string)GetValue(DisplayTextProperty);
            }

            set
            {
                SetValue(DisplayTextProperty, value);
            }
        }

        private static void DisplayTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is HyperlinkFromText)
            {
                HyperlinkFromText castControl = obj as HyperlinkFromText;
                castControl.MyDisplayText.Text = castControl.DisplayText;
            }
        }

        public HyperlinkFromText()
        {
            InitializeComponent();
        }


    }
}

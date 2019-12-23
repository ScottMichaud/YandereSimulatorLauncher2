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
    /// Interaction logic for HyperlinkFromImage.xaml
    /// </summary>
    public partial class HyperlinkFromImage : UserControl
    {
        public static readonly DependencyProperty DisplayImageProperty = DependencyProperty.Register("DisplayImage", typeof(string), typeof(HyperlinkFromImage), new PropertyMetadata(string.Empty, DisplayTextChanged));

        public string DisplayImage
        {
            get
            {
                return (string)GetValue(DisplayImageProperty);
            }

            set
            {
                SetValue(DisplayImageProperty, value);
            }
        }

        private static void DisplayTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is HyperlinkFromImage)
            {
                HyperlinkFromImage castControl = obj as HyperlinkFromImage;
                castControl.MyDisplayImage.Source = new BitmapImage(new Uri(castControl.DisplayImage, UriKind.Relative));
            }
        }

        public HyperlinkFromImage()
        {
            InitializeComponent();
        }
    }
}

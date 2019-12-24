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
        #region XAML Properties
        public static readonly DependencyProperty DisplayImageProperty = DependencyProperty.Register("DisplayImage", typeof(string), typeof(HyperlinkFromImage), new PropertyMetadata(string.Empty, DisplayTextChanged));
        public static readonly DependencyProperty LinkedUrlProperty = DependencyProperty.Register("LinkedUrl", typeof(string), typeof(HyperlinkFromImage), new PropertyMetadata(string.Empty, LinkedUrlChanged));

        public string DisplayImage
        {
            get { return (string)GetValue(DisplayImageProperty); }
            set { SetValue(DisplayImageProperty, value); }
        }

        public string LinkedUrl
        {
            get { return (string)GetValue(LinkedUrlProperty); }
            set { SetValue(LinkedUrlProperty, value); }
        }

        private static void DisplayTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is HyperlinkFromImage)
            {
                HyperlinkFromImage castControl = obj as HyperlinkFromImage;
                castControl.MyDisplayImage.Source = new BitmapImage(new Uri(castControl.DisplayImage, UriKind.Relative));
            }
        }

        private static void LinkedUrlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // No effects
        }
        #endregion

        #region C# Properties

        private bool IsButtonPrimed { get; set; }

        #endregion

        public HyperlinkFromImage()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsButtonPrimed = true;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsButtonPrimed == true)
            {
                System.Diagnostics.Process.Start(LinkedUrl);
            }

            IsButtonPrimed = false;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            IsButtonPrimed = false;
        }
    }
}

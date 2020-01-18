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
using YandereSimulatorLauncher2;

namespace YandereSimulatorLauncher2.Controls
{
    /// <summary>
    /// Interaction logic for OtherHyperlinkFromText.xaml
    /// </summary>
    public partial class OtherHyperlinkFromText : UserControl
    {
        #region XAML Properties
        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof(string), typeof(OtherHyperlinkFromText), new PropertyMetadata(string.Empty, DisplayTextChanged));
        public static readonly DependencyProperty LinkedUrlProperty = DependencyProperty.Register("LinkedUrl", typeof(string), typeof(OtherHyperlinkFromText), new PropertyMetadata(string.Empty, LinkedUrlChanged));

        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public string LinkedUrl
        {
            get { return (string)GetValue(LinkedUrlProperty); }
            set { SetValue(LinkedUrlProperty, value); }
        }

        private static void DisplayTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is OtherHyperlinkFromText)
            {
                OtherHyperlinkFromText castControl = obj as OtherHyperlinkFromText;
                castControl.MyDisplayText.Text = castControl.DisplayText;
            }
        }

        private static void LinkedUrlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // No effects
        }
        #endregion

        private bool IsButtonPrimed { get; set; } = false;

        public OtherHyperlinkFromText()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsButtonPrimed = true;
        }

        private async void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsButtonPrimed == true)
            {
                System.Diagnostics.Process.Start(LinkedUrl);
                await Logic.UpdatePlayHelpers.AsynchronousWait(500);
                Application.Current.MainWindow.Close();
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

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
        public static readonly DependencyProperty DisplayImageProperty = DependencyProperty.Register("DisplayImage", typeof(string), typeof(HyperlinkFromImage), new PropertyMetadata(string.Empty, DisplayImageChanged));
        public static readonly DependencyProperty LinkedUrlProperty = DependencyProperty.Register("LinkedUrl", typeof(string), typeof(HyperlinkFromImage), new PropertyMetadata(string.Empty, LinkedUrlChanged));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(HyperlinkFromImage), new PropertyMetadata(0.0, ImageHeightChanged));

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

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        private static void ImageHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is HyperlinkFromImage)
            {
                HyperlinkFromImage castControl = obj as HyperlinkFromImage;
                castControl.MyDisplayImage.Height = castControl.ImageHeight;
            }
        }

        private bool mIsDere = true;
        public bool IsDere
        {
            get { return mIsDere; }
            set
            {
                if (mIsDere != value)
                {
                    mIsDere = value;

                    if (mIsDere)
                    {
                        SetDere();
                    }
                    else
                    {
                        SetYan();
                    }
                }
            }
        }

        private static void DisplayImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is HyperlinkFromImage)
            {
                HyperlinkFromImage castControl = obj as HyperlinkFromImage;
                ///YandereSimulatorLauncher2;component/EmbeddedAssets/Images/twitter-white.png
                //castControl.MyDisplayImage.Source = new BitmapImage(new Uri(castControl.DisplayImage, UriKind.Relative));
                castControl.MyDisplayImage.Source = new BitmapImage(castControl.ConvertImageTokenToUri(castControl.DisplayImage));
            }
        }

        private static void LinkedUrlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // No effects
        }
        #endregion

        #region C# Properties

        private bool IsButtonPrimed { get; set; } = false;

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

        private Uri ConvertImageTokenToUri(string inToken)
        {
            return new Uri("/YandereSimulatorLauncher2;component/EmbeddedAssets/Images/" + inToken + (IsDere ? "-white.png" : "-black.png"), UriKind.Relative);
        }

        private void SetDere()
        {
            MyDisplayImage.Source = new BitmapImage(ConvertImageTokenToUri(DisplayImage));
            MyDisplayImageUnderline.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void SetYan()
        {
            MyDisplayImage.Source = new BitmapImage(ConvertImageTokenToUri(DisplayImage));
            MyDisplayImageUnderline.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }
    }
}

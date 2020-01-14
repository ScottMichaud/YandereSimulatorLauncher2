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
using System.Windows.Media.Animation;

namespace YandereSimulatorLauncher2.Controls
{
    public enum DownloadBarMode
    {
        DownloadingGame,
        DownloadingLauncher,
        Extracting,
        Waiting
    }

    /// <summary>
    /// Interaction logic for DownloadBar.xaml
    /// </summary>
    public partial class DownloadBar : UserControl
    {
        public static readonly DependencyProperty IsDereProperty = DependencyProperty.Register("IsDere", typeof(bool), typeof(DownloadBar), new PropertyMetadata(true, IsDereChanged));
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(DownloadBar), new PropertyMetadata(false, IsOpenChanged));

        // I'd normally use a list but it cannot be any other value. Fixed sizes.
        private readonly Rectangle[] barSegments = new Rectangle[93];

        public bool IsDere
        {
            get { return (bool)GetValue(IsDereProperty); }
            set { SetValue(IsDereProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        DownloadBarMode mPreviousBarMode = DownloadBarMode.Waiting;
        public void ChangeProgress(DownloadBarMode inMode, double inPercentComplete = -1, double inSpeed = -1)
        {
            ChangeBarModeIfApplicable(inMode);

            string taskToken = ConvertDownloadBarModeToDisplayString(inMode);
            string speedToken = ConvertBytesPerSecondToSpeedString(inSpeed);
            string percentToken = ConvertPercentToDisplayString(inPercentComplete);

            BarLabel.Text = taskToken + percentToken + speedToken;

            FillLoadingBars(inPercentComplete);
        }

        private void ChangeBarModeIfApplicable(DownloadBarMode inMode)
        {
            if (inMode != mPreviousBarMode)
            {
                // If leaving Extracting
                if (mPreviousBarMode == DownloadBarMode.Extracting)
                {
                    CompositionTarget.Rendering -= DoThrobbingRender;
                }

                mPreviousBarMode = inMode;

                // If entering Extracting
                if (mPreviousBarMode == DownloadBarMode.Extracting)
                {
                    CompositionTarget.Rendering += DoThrobbingRender;
                }
            }
        }

        private void FillLoadingBars(double inPercentComplete)
        {
            if (inPercentComplete < 0) { return; }

            for (int i = 0; i < 93; i += 1)
            {
                double portion = i / 0.93;
                if (portion < inPercentComplete)
                {
                    barSegments[i].Visibility = Visibility.Visible;
                }
                else
                {
                    barSegments[i].Visibility = Visibility.Hidden;
                }
            }
        }

        private static void IsDereChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is DownloadBar)
            {
                DownloadBar castControl = obj as DownloadBar;

                if (castControl.IsDere)
                {
                    castControl.SetDere();
                }
                else
                {
                    castControl.SetYan();
                }
            }
        }

        private static void IsOpenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is DownloadBar)
            {
                DownloadBar castControl = obj as DownloadBar;

                if (castControl.IsOpen)
                {
                    castControl.SetOpen();
                }
                else
                {
                    castControl.SetClosed();
                }
            }
        }

        public DownloadBar()
        {
            InitializeComponent();
            CreateBarSegments();
        }

        const double mThrobberSpeed = 3.0;
        const int mNumberActive = 30;
        private void DoThrobbingRender(object inSender, EventArgs args)
        {
            // We'll see if this is enough precision...
            RenderingEventArgs castArgs = args as RenderingEventArgs;
            double throbberProgress = Math.Sin(castArgs.RenderingTime.TotalSeconds * mThrobberSpeed);
            throbberProgress += 1;
            throbberProgress *= 0.5;
            int numberDisabled = 93 - mNumberActive;
            int startIndex = (int)Math.Floor(throbberProgress * numberDisabled);

            for (int i = 0; i < 93; i += 1)
            {
                if (i > startIndex && i < (startIndex + mNumberActive))
                {
                    barSegments[i].Visibility = Visibility.Visible;
                }
                else
                {
                    barSegments[i].Visibility = Visibility.Hidden;
                }
            }
        }

        private void SetDere()
        {
            SlidingContainer.Background = App.HexToBrush("#ff80d3");
            BarContainer.Background = App.HexToBrush("#ee63bb");
            SlidingContainer.BorderBrush = App.HexToBrush("#95286d");
            BarContainer.BorderBrush = App.HexToBrush("#95286d");
            BarLabel.Foreground = App.HexToBrush("#FFFFFF");

            Brush dereBarBrush = App.HexToBrush("#FFFFFF");

            for (int i = 0; i < 93; i += 1)
            {
                barSegments[i].Fill = dereBarBrush;
            }
        }

        private void SetYan()
        {
            SlidingContainer.Background = App.HexToBrush("#ff0000");
            BarContainer.Background = App.HexToBrush("#bb0000");
            SlidingContainer.BorderBrush = App.HexToBrush("#330000");
            BarContainer.BorderBrush = App.HexToBrush("#330000");
            BarLabel.Foreground = App.HexToBrush("#000000");

            Brush yanBarBrush = App.HexToBrush("#000000");

            for (int i = 0; i < 93; i += 1)
            {
                barSegments[i].Fill = yanBarBrush;
            }
        }

        private void SetOpen()
        {
            DoubleAnimation openAnimation = new DoubleAnimation(50.0, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            openAnimation.EasingFunction = new BackEase();
            SlidingContainer.BeginAnimation(Canvas.RightProperty, openAnimation);
        }

        private void SetClosed()
        {
            DoubleAnimation closeAnimation = new DoubleAnimation(-520.0, new Duration(new TimeSpan(0, 0, 0, 0, 250)));
            SlidingContainer.BeginAnimation(Canvas.RightProperty, closeAnimation);
        }

        private void CreateBarSegments()
        {
            for (int i = 0; i < 93; i += 1)
            {
                Rectangle newRectangle = new Rectangle();
                newRectangle.Width = 2;
                newRectangle.Height = 22;
                newRectangle.Fill = App.HexToBrush("#FFF");
                newRectangle.Visibility = Visibility.Hidden;
                BarSegmentContainer.Children.Add(newRectangle);
                barSegments[i] = newRectangle;
                Canvas.SetLeft(newRectangle, 5 * i);
                Canvas.SetTop(newRectangle, 4);
            }
        }

        private static string ConvertPercentToDisplayString(double inPercent)
        {
            if (inPercent < 0) { return ""; }

            return " " + ((int)Math.Round(inPercent)).ToString() + "%";
        }

        private static string ConvertBytesPerSecondToSpeedString(double inBytesPerSecond)
        {
            // Pattern: If less than {number of 1024s multiplied together} then divide
            //          by {one less than the number of 1024s multiplied together}
            //
            //          - If less than 1.0 kB/s: measure in B/s.
            //          - If less than 1.0 MB/s: measure in kB/s.
            //          (etc.)
            if (inBytesPerSecond < 0)
            {
                return "";
            }

            if (inBytesPerSecond < 1024.0)
            {
                return " (" + inBytesPerSecond.ToString("0.##") + " B/s)";
            }

            if (inBytesPerSecond < (1024.0 * 1024.0))
            {
                return " (" + (inBytesPerSecond / 1024.0).ToString("0.##") + " kB/s)";
            }

            if (inBytesPerSecond < (1024.0 * 1024.0 * 1024.0))
            {
                return " (" + (inBytesPerSecond / (1024.0 * 1024.0)).ToString("0.##") + " MB/s)";
            }

            if (inBytesPerSecond < (1024.0 * 1024.0 * 1024.0 * 1024.0))
            {
                return " (" + (inBytesPerSecond / (1024.0 * 1024.0 * 1024.0)).ToString("0.##") + " GB/s)";
            }

            return " (" + (inBytesPerSecond / (1024.0 * 1024.0 * 1024.0 * 1024.0)).ToString("0.##") + " TB/s)";
        }

        private static string ConvertDownloadBarModeToDisplayString(DownloadBarMode inMode)
        {
            switch (inMode)
            {
                case DownloadBarMode.Waiting:
                    return "";
                case DownloadBarMode.DownloadingGame:
                    return "Downloading Yandere Simulator:";
                case DownloadBarMode.DownloadingLauncher:
                    return "Downloading new launcher";
                case DownloadBarMode.Extracting:
                    return "Extracting files";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

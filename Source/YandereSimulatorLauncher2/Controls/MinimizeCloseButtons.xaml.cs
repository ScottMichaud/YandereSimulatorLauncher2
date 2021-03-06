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
    /// Interaction logic for MinimizeCloseButtons.xaml
    /// </summary>
    public partial class MinimizeCloseButtons : UserControl
    {

        #region C# Properties

        // The "primed" pattern (seen here and other controls) allows the user
        // to cancel button presses by dragging the cursor off the button before
        // releasing it (to confirm). It's the difference between a "click" and
        // an action directly on MouseDown.
        private bool IsMinimizePrimed { get; set; } = false;
        private bool IsClosePrimed { get; set; } = false;

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

        #endregion

        public MinimizeCloseButtons()
        {
            InitializeComponent();
        }

        #region Minimize Events
        private void OnMinimizeMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsMinimizePrimed = true;
        }

        private void OnMinimizeMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMinimizePrimed == true)
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }

            IsMinimizePrimed = false;
        }

        private void OnMinimizeMouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void OnMinimizeMouseLeave(object sender, MouseEventArgs e)
        {
            IsMinimizePrimed = false;
        }
        #endregion

        #region Close Events
        private void OnCloseMouseDown(object sender, MouseButtonEventArgs e)
        {
            IsClosePrimed = true;
        }

        private void OnCloseMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsClosePrimed == true)
            {
                Application.Current.MainWindow.Close();
            }

            IsClosePrimed = false;
        }

        private void OnCloseMouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void OnCloseMouseLeave(object sender, MouseEventArgs e)
        {
            IsClosePrimed = false;
        }
        #endregion

        private Uri ConvertImageTokenToUri(string inToken)
        {
            return new Uri("/YandereSimulatorLauncher2;component/EmbeddedAssets/Images/" + inToken + (IsDere ? "-white.png" : "-black.png"), UriKind.Relative);
        }

        private void SetDere()
        {
            MyCloseButton.Source = new BitmapImage(ConvertImageTokenToUri("close"));
            MyMinimizeButton.Source = new BitmapImage(ConvertImageTokenToUri("minimize"));
        }

        private void SetYan()
        {
            MyCloseButton.Source = new BitmapImage(ConvertImageTokenToUri("close"));
            MyMinimizeButton.Source = new BitmapImage(ConvertImageTokenToUri("minimize"));
        }
    }
}

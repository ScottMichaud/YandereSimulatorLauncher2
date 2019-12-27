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

        private bool IsMinimizePrimed { get; set; } = false;
        private bool IsClosePrimed { get; set; } = false;

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
    }
}

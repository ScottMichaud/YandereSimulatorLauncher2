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
    /// Interaction logic for DownloadBar.xaml
    /// </summary>
    public partial class DownloadBar : UserControl
    {
        public static readonly DependencyProperty IsDereProperty = DependencyProperty.Register("IsDere", typeof(bool), typeof(DownloadBar), new PropertyMetadata(true, IsDereChanged));

        public bool IsDere
        {
            get { return (bool)GetValue(IsDereProperty); }
            set { SetValue(IsDereProperty, value); }
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

        public DownloadBar()
        {
            InitializeComponent();
        }

        private void DoRender()
        {
            
        }

        private void SetDere()
        {
            SlidingContainer.Background = App.HexToBrush("#ff80d3");
            BarContainer.Background = App.HexToBrush("#ee63bb");
            SlidingContainer.BorderBrush = App.HexToBrush("#95286d");
            BarContainer.BorderBrush = App.HexToBrush("#95286d");
            BarLabel.Foreground = App.HexToBrush("#FFFFFF");
        }

        private void SetYan()
        {
            SlidingContainer.Background = App.HexToBrush("#ff0000");
            BarContainer.Background = App.HexToBrush("#bb0000");
            SlidingContainer.BorderBrush = App.HexToBrush("#330000");
            BarContainer.BorderBrush = App.HexToBrush("#330000");
            BarLabel.Foreground = App.HexToBrush("#000000");
        }
    }
}

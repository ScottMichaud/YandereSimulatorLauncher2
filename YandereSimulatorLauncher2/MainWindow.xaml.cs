using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace YandereSimulatorLauncher2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer flipFlopTimerYanDere;
        private bool isDere = true;
        private DateTime nextYanDereFlip = DateTime.MinValue;
        private const double secondsToDisplayDere = 15.0;
        private const double secondsToDisplayYan = 1.0;

        public MainWindow()
        {
            UnpackVideoResources();
            InitializeComponent();
            HandleVisualStyles();
            StartYanDereFlipFlop();
        }

        protected override sealed void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void HandleVisualStyles()
        {
            if (NativeMethods.DwmCompositionIsEnabled)
            //if (false)
            {
                EnableHighQuality();
            }
            else
            {
                DisableHighQuality();
            }
        }

        private void EnableHighQuality()
        {
            // Allow shadows
            Width = 906;
            Height = 701;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            ShadowBorder.Margin = new Thickness(13);

            //
        }

        private void DisableHighQuality()
        {
            // Disable shadows
            Width = 882;
            Height = 677;
            AllowsTransparency = false;
            Background = Brushes.Black;
            ShadowBorder.Margin = new Thickness(1);

            //
        }

        private void UnpackVideoResources()
        {
            // On my PC, these two operations add a total of ~19ms to load time.
            // Probably don't need to do anything fancy.
            Directory.CreateDirectory(App.LauncherTempFileDirectory);
            UnpackVideoFile(YandereSimulatorLauncher2.Properties.Resources.mainpanel_dere, App.MainPanelDereFileLocation);
            UnpackVideoFile(YandereSimulatorLauncher2.Properties.Resources.mainpanel_yan, App.MainPanelYanFileLocation);
        }

        private static void UnpackVideoFile(byte[] inResource, string inFilename)
        {
            try
            {
                File.WriteAllBytes(inFilename, inResource);
            }
            catch (Exception)
            {
                
            }
        }

        private void StartYanDereFlipFlop()
        {
            // Create a 100ms timer that calls FlipFlopTimerYanDere_OnTick() on the UI thread.
            flipFlopTimerYanDere = new DispatcherTimer(TimeSpan.FromMilliseconds(100.0), DispatcherPriority.Normal, FlipFlopTimerYanDere_OnTick, Dispatcher.CurrentDispatcher);
            nextYanDereFlip = DateTime.Now + TimeSpan.FromSeconds(secondsToDisplayDere);
            flipFlopTimerYanDere.Start();
        }

        private void FlipFlopTimerYanDere_OnTick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            if (currentTime < nextYanDereFlip) { return; }

            // Perform the flip.
            // NOTE: Basing the next flip time off of now, rather than when nextYanDereFlip expired.
            isDere = !isDere;
            nextYanDereFlip = currentTime + TimeSpan.FromSeconds(isDere ? secondsToDisplayDere : secondsToDisplayYan);
            
            if (isDere)
            {
                SetDere();
            }
            else
            {
                SetYan();
            }
        }

        private void SetDere()
        {
            ElementYanDereVideoPlayer.IsDere = true;
        }

        private void SetYan()
        {
            ElementYanDereVideoPlayer.IsDere = false;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs evt)
        {
            if (evt.ChangedButton == MouseButton.Left && evt.Handled == false)
            {
                DragMove();
            }
        }
    }
}

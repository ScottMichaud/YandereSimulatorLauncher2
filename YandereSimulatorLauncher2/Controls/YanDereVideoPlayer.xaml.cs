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
using YandereSimulatorLauncher2.Popups;

namespace YandereSimulatorLauncher2.Controls
{
    /// <summary>
    /// Interaction logic for YanDereVideoPlayer.xaml
    /// </summary>
    public partial class YanDereVideoPlayer : UserControl
    {
        public static readonly DependencyProperty IsDereProperty = DependencyProperty.Register("IsDere", typeof(bool), typeof(YanDereVideoPlayer), new PropertyMetadata(true, IsDereChanged));

        public event EventHandler YanDereCheckboxClicked;

        private bool isYanVideoLoaded = false;
        private bool isDereVideoLoaded = false;

        public bool IsDere
        {
            get { return (bool)GetValue(IsDereProperty); }
            set { SetValue(IsDereProperty, value); }
        }

        public bool IsYanDereFlipFlopEnabled
        {
            get
            {
                return YanDereEnabledCheckbox.IsChecked ?? false;
            }
        }

        private bool IsVideoEnabledChecked
        {
            get
            {
                return VideoEnabledCheckbox.IsChecked ?? false;
            }
        }

        public YanDereVideoPlayer()
        {
            InitializeComponent();
            ConfigureComponent();
        }

        private static void IsDereChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is YanDereVideoPlayer)
            {
                YanDereVideoPlayer castControl = obj as YanDereVideoPlayer;

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

        private void ConfigureComponent()
        {
            SetInitialYanDereEnabledCheckbox();
            SetInitialVideoEnabledCheckbox();

            if (IsVideoEnabledChecked)
            //if (false)
            {
                LoadVideos();
            }
            else
            {
                UnloadVideos();
            }

            ReportBugButton.PlayButtonClicked += ReportBug_OnClicked;

            //ReportBugButton.Background = App.HexToBrush("#ff80d3");
            //ReportBugButton.Foreground = App.HexToBrush("#FFFFFF");
            
        }

        private void SetInitialVideoEnabledCheckbox()
        {
            if (NativeMethods.DwmCompositionIsEnabled)
            {
                VideoEnabledCheckbox.IsChecked = true;
                VideoEnabledCheckbox.IsEnabled = true;
                VideoEnabledCheckbox.Checked += VideoEnabledCheckbox_OnChecked;
                VideoEnabledCheckbox.Unchecked += VideoEnabledCheckbox_OnUnChecked;
            }
            else
            {
                VideoEnabledCheckbox.IsChecked = false;
                VideoEnabledCheckbox.IsEnabled = false;
                VideoEnabledCheckboxText.Text = "Video Requires Enabling Visual Styles";
            }
        }

        private void SetInitialYanDereEnabledCheckbox()
        {
            YanDereEnabledCheckbox.IsChecked = true;
            YanDereEnabledCheckbox.IsEnabled = true;
            YanDereEnabledCheckbox.Checked += YanDereEnabledCheckbox_OnChecked;
            YanDereEnabledCheckbox.Unchecked += YanDereEnabledCheckbox_OnChecked;
        }

        private void LoadVideos()
        {
            VideoBackgroundDere.Source = new Uri(App.MainPanelDereFileLocation);
            VideoBackgroundYan.Source = new Uri(App.MainPanelYanFileLocation);
        }

        private void UnloadVideos()
        {
            VideoBackgroundDere.Source = null;
            VideoBackgroundYan.Source = null;
        }

        private void SetDere()
        {
            ImageBackgroundYan.Visibility = Visibility.Hidden;
            VideoBackgroundYan.Visibility = Visibility.Hidden;
            if (isYanVideoLoaded) { VideoBackgroundYan.Stop(); }
            if (isDereVideoLoaded) { VideoBackgroundDere.Play(); }
            ReportBugButton.IsDere = true;
            //ReportBugButton.Background = App.HexToBrush("#ff80d3");
            //ReportBugButton.Foreground = App.HexToBrush("#FFFFFF");
        }

        private void SetYan()
        {
            ImageBackgroundYan.Visibility = Visibility.Visible;
            if (IsVideoEnabledChecked) { VideoBackgroundYan.Visibility = Visibility.Visible; }
            if (isYanVideoLoaded) { VideoBackgroundYan.Play(); }
            if (isDereVideoLoaded) { VideoBackgroundDere.Stop(); }
            ReportBugButton.IsDere = false;
            //ReportBugButton.Background = App.HexToBrush("#ff0000");
            //ReportBugButton.Foreground = App.HexToBrush("#000000");
        }

        private void VideoDere_OnLoaded(object sender, RoutedEventArgs e)
        {
            isDereVideoLoaded = true;
            VideoBackgroundDere.Play();
        }

        private void VideoYan_OnLoaded(object sender, RoutedEventArgs e)
        {
            isYanVideoLoaded = true;
            VideoBackgroundYan.Play();
        }

        private void VideoDere_OnEnded(object sender, RoutedEventArgs e)
        {
            VideoBackgroundDere.Position = new TimeSpan(0, 0, 0, 0, 1);
            VideoBackgroundDere.Play();
        }

        private void VideoYan_OnEnded(object sender, RoutedEventArgs e)
        {
            VideoBackgroundYan.Position = new TimeSpan(0, 0, 0, 0, 1);
            VideoBackgroundYan.Play();
        }

        private void VideoEnabledCheckbox_OnChecked(object sender, EventArgs e)
        {
            VideoBackgroundDere.Visibility = Visibility.Visible;

            if (IsDere == false)
            {
                VideoBackgroundYan.Visibility = Visibility.Visible;
            }
        }

        private void VideoEnabledCheckbox_OnUnChecked(object sender, EventArgs e)
        {
            VideoBackgroundYan.Visibility = Visibility.Hidden;
            VideoBackgroundDere.Visibility = Visibility.Hidden;
        }

        private void YanDereEnabledCheckbox_OnChecked(object sender, EventArgs e)
        {
            YanDereCheckboxClicked.Invoke(this, new EventArgs());
        }

        private void ReportBug_OnClicked(object sender, EventArgs e)
        {
            ReportLauncherBug reportLauncherPopup = new ReportLauncherBug();
            reportLauncherPopup.ShowDialog();
        }
    }
}

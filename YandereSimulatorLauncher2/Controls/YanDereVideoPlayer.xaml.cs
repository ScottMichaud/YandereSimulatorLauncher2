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
    /// Interaction logic for YanDereVideoPlayer.xaml
    /// </summary>
    public partial class YanDereVideoPlayer : UserControl
    {
        public static readonly DependencyProperty IsDereProperty = DependencyProperty.Register("IsDere", typeof(bool), typeof(YanDereVideoPlayer), new PropertyMetadata(true, IsDereChanged));

        private bool isYanVideoLoaded = false;
        private bool isDereVideoLoaded = false;

        public bool IsDere
        {
            get { return (bool)GetValue(IsDereProperty); }
            set { SetValue(IsDereProperty, value); }
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
        }

        private void SetYan()
        {
            ImageBackgroundYan.Visibility = Visibility.Visible;
            if (IsVideoEnabledChecked) { VideoBackgroundYan.Visibility = Visibility.Visible; }
            if (isYanVideoLoaded) { VideoBackgroundYan.Play(); }
            if (isDereVideoLoaded) { VideoBackgroundDere.Stop(); }
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
    }
}

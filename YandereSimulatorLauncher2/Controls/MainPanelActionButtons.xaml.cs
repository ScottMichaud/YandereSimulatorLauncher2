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
    public enum YsInstallMode
    {
        Unset,
        RetryInstall,
        PromptToInstall,
        CheckingForUpdates,
        ConfirmingUpdate,
        PromptToCheck,
        PromptToUpdate,
        Downloading,
        Unpacking,
        Launching,
        UpdatingLauncher,
        YouAreUpToDate
    }

    /// <summary>
    /// Interaction logic for MainPanelActionButtons.xaml
    /// </summary>
    public partial class MainPanelActionButtons : UserControl
    {
        public event EventHandler InstallButtonClicked;
        public event EventHandler PlayButtonClicked;

        private YsInstallMode mCurrentMode;
        public YsInstallMode CurrentMode
        {
            get { return mCurrentMode; }
            set
            {
                if (mCurrentMode != value)
                {
                    mCurrentMode = value;
                    DoRender();
                }
            }
        }

        private bool mIsDere = true;
        public bool IsDere
        {
            get
            {
                return mIsDere;
            }
            set
            {
                if (mIsDere != value)
                {
                    mIsDere = value;
                    DoRender();
                }
            }
        }

        private bool mIsUserHoveringInstall = false;
        private bool mIsInstallPrimed = false;
        private bool mIsUserHoveringPlay = false;
        private bool mIsPlayPrimed = false;

        public MainPanelActionButtons()
        {
            InitializeComponent();
            DoRender();
        }

        private void DoRender()
        {
            RenderCurrentMode();
            ColorButton(mutBorder: InstallUpdateButton, inIsPrimed: mIsInstallPrimed, inIsUserHovering: mIsUserHoveringInstall, inIsDere: IsDere);
            ColorButton(mutBorder: PlayButton, inIsPrimed: mIsPlayPrimed, inIsUserHovering: mIsUserHoveringPlay, inIsDere: IsDere);
            InstallUpdateButtonText.Foreground = IsDere ? App.HexToBrush("#ffffff") : App.HexToBrush("#000000");
            PlayButtonText.Foreground = IsDere ? App.HexToBrush("#ffffff") : App.HexToBrush("#000000");

            InstallUpdateButton.Opacity = InstallUpdateButton.IsEnabled ? 1 : 0.5;
            PlayButton.Opacity = PlayButton.IsEnabled ? 1 : 0.5;
        }

        private void RenderCurrentMode()
        {
            switch (CurrentMode)
            {
                case YsInstallMode.Unset:
                    InstallUpdateButtonText.Text = "";
                    PlayButtonText.Text = "";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    break;
                case YsInstallMode.RetryInstall:
                    InstallUpdateButtonText.Text = "Retry install";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = true;
                    PlayButton.IsEnabled = false;
                    break;
                case YsInstallMode.PromptToInstall:
                    InstallUpdateButtonText.Text = "Install";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = true;
                    PlayButton.IsEnabled = false;
                    break;
                case YsInstallMode.CheckingForUpdates:
                    InstallUpdateButtonText.Text = "Checking for updates...";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = true;
                    break;
                case YsInstallMode.ConfirmingUpdate:
                    InstallUpdateButtonText.Text = "Confirming...";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    break;
                case YsInstallMode.PromptToCheck:
                    InstallUpdateButtonText.Text = "Check for updates";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = true;
                    PlayButton.IsEnabled = true;
                    break;
                case YsInstallMode.PromptToUpdate:
                    InstallUpdateButtonText.Text = "Update available";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = true;
                    PlayButton.IsEnabled = true;
                    break;
                case YsInstallMode.YouAreUpToDate:
                    InstallUpdateButtonText.Text = "No update available";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = true;
                    break;
                case YsInstallMode.Downloading:
                    InstallUpdateButtonText.Text = "Downloading...";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    break;
                case YsInstallMode.Unpacking:
                    InstallUpdateButtonText.Text = "Unpacking...";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    break;
                case YsInstallMode.Launching:
                    InstallUpdateButtonText.Text = "Launching...";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    break;
                case YsInstallMode.UpdatingLauncher:
                    InstallUpdateButtonText.Text = "Downloading...";
                    PlayButtonText.Text = "Play";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    break;
                default:
                    InstallUpdateButtonText.Text = "";
                    PlayButtonText.Text = "";
                    InstallUpdateButton.IsEnabled = false;
                    PlayButton.IsEnabled = false;
                    break;
            }
        }

        private static void ColorButton(Border mutBorder, bool inIsPrimed, bool inIsUserHovering, bool inIsDere)
        {
            mutBorder.BorderBrush = inIsDere ? App.HexToBrush("#95286d") : App.HexToBrush("#330000");

            if (inIsPrimed)
            {
                mutBorder.Background = inIsDere ? App.HexToBrush("#ff91da") : App.HexToBrush("#ff5555");
            }
            else if (inIsUserHovering)
            {
                mutBorder.Background = inIsDere ? App.HexToBrush("#d877b6") : App.HexToBrush("#b70000");
            }
            else
            {
                mutBorder.Background = inIsDere ? App.HexToBrush("#ff80d3") : App.HexToBrush("#ff0000");
            }
        }

        private void Install_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            mIsInstallPrimed = true;
            DoRender();
        }

        private void Install_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mIsInstallPrimed == true)
            {
                InstallButtonClicked?.Invoke(this, new EventArgs());
            }

            mIsInstallPrimed = false;
            DoRender();
        }

        private void Install_OnMouseEnter(object sender, MouseEventArgs e)
        {
            mIsUserHoveringInstall = true;
            DoRender();
        }

        private void Install_OnMouseLeave(object sender, MouseEventArgs e)
        {
            mIsUserHoveringInstall = false;
            mIsInstallPrimed = false;
            DoRender();
        }

        private void Play_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            mIsPlayPrimed = true;
            DoRender();
        }

        private void Play_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mIsPlayPrimed == true)
            {
                PlayButtonClicked?.Invoke(this, new EventArgs());
            }

            mIsPlayPrimed = false;
            DoRender();
        }

        private void Play_OnMouseEnter(object sender, MouseEventArgs e)
        {
            mIsUserHoveringPlay = true;
            DoRender();
        }

        private void Play_OnMouseLeave(object sender, MouseEventArgs e)
        {
            mIsUserHoveringPlay = false;
            mIsPlayPrimed = false;
            DoRender();
        }
    }
}

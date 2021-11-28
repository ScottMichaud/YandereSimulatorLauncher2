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
using System.Windows.Shell;

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
            AddEventHandlers();
            StartYanDereFlipFlop();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Support all encryption schemes. 
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 |
                                                              System.Net.SecurityProtocolType.Tls11 |
                                                              System.Net.SecurityProtocolType.Tls |
                                                              System.Net.SecurityProtocolType.Ssl3;
            
            if (Environment.Is64BitOperatingSystem == false)
            {
                MessageBox.Show("Your computer reports that it is running a 32-bit operating system. Yandere Simulator requires 64-bit Windows since April 10th, 2020.\n\nThe launcher will not attempt to block you from downloading or running the game, but Windows is telling the launcher that Yandere Simulator will not run.", "Yandere Simulator is not supported on this device");
            }

            await DoCheckForUpdates();
            await DoCheckForLauncherUpdate();
        }

        protected override sealed void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void HandleVisualStyles()
        {
            if (NativeMethods.DwmCompositionIsEnabled)
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
        }

        private void DisableHighQuality()
        {
            // Disable shadows
            Width = 882;
            Height = 677;
            AllowsTransparency = false;
            Background = Brushes.Black;
            ShadowBorder.Margin = new Thickness(1);
        }

        private void AddEventHandlers()
        {
            ElementMainPanelActionButtons.InstallButtonClicked += InstallButton_OnClick;
            ElementMainPanelActionButtons.PlayButtonClicked += PlayButton_OnClick;
            ElementYanDereVideoPlayer.YanDereCheckboxClicked += VideoPlayer_OnDereReset;
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
                if (File.Exists(inFilename) == false)
                {
                    File.WriteAllBytes(inFilename, inResource);
                }
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
            // NOTE: Basing the next flip time off of now, rather than when next YanDereFlip expired.
            isDere = !isDere;
            nextYanDereFlip = currentTime + TimeSpan.FromSeconds(isDere ? secondsToDisplayDere : secondsToDisplayYan);
            
            if (ElementYanDereVideoPlayer.IsYanDereFlipFlopEnabled == true)
            {
                if (isDere)
                {
                    SetDere();
                }
                else
                {
                    SetYan();
                }
            }
        }

        private void SetDere()
        {
            ElementYanDereVideoPlayer.IsDere = true;
            ElementMainPanelActionButtons.IsDere = true;
            ElementMinimizeCloseButtons.IsDere = true;
            ElementDownloadBar.IsDere = true;
            YandereSimulatorLogo.Source = new BitmapImage(new Uri("/YandereSimulatorLauncher2;component/EmbeddedAssets/Images/yandere-simulator-logo.png", UriKind.Relative));

            GridTopbar.Background = App.HexToBrush("#ff80d3");
            GridLinkbar.Background = App.HexToBrush("#ee63bb");

            UpdateFlyin.IsDere = true;

            LinkAbout.IsDere = true;
            LinkBlog.IsDere = true;
            LinkContact.IsDere = true;
            LinkDonate.IsDere = true;
            LinkVolunteer.IsDere = true;
            LinkYoutube.IsDere = true;
            LinkTwitch.IsDere = true;
            LinkTwitter.IsDere = true;
        }

        private void SetYan()
        {
            ElementYanDereVideoPlayer.IsDere = false;
            ElementMainPanelActionButtons.IsDere = false;
            ElementMinimizeCloseButtons.IsDere = false;
            ElementDownloadBar.IsDere = false;
            YandereSimulatorLogo.Source = new BitmapImage(new Uri("/YandereSimulatorLauncher2;component/EmbeddedAssets/Images/yandere-simulator-logo-black.png", UriKind.Relative));

            GridTopbar.Background = App.HexToBrush("#ff0000");
            GridLinkbar.Background = App.HexToBrush("#bb0000");

            UpdateFlyin.IsDere = false;

            LinkAbout.IsDere = false;
            LinkBlog.IsDere = false;
            LinkContact.IsDere = false;
            LinkDonate.IsDere = false;
            LinkVolunteer.IsDere = false;
            LinkYoutube.IsDere = false;
            LinkTwitch.IsDere = false;
            LinkTwitter.IsDere = false;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs evt)
        {
            if (evt.ChangedButton == MouseButton.Left && evt.Handled == false)
            {
                DragMove();
            }
        }

        private async void InstallButton_OnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                await DoRelevantInstallTask();
            }
            catch (Exception)
            {

            }
        }

        private async void PlayButton_OnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                await DoPlay();
            }
            catch (Exception)
            {
                MessageBoxResult result = MessageBox.Show("Yandere Simulator has failed to start.\nWould you like to fresh install the latest version?", "Failed to launch Yandere Simulator", MessageBoxButton.YesNo, MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    await DoInstall();
                }
                else
                {
                    Close();
                }
            }
        }

        private async Task DoRelevantInstallTask()
        {
            switch (ElementMainPanelActionButtons.CurrentMode)
            {
                case Controls.YsInstallMode.Unset:
                    throw new NotImplementedException("The install button should be locked.");
                case Controls.YsInstallMode.RetryInstall:
                    await DoInstall();
                    break;
                case Controls.YsInstallMode.PromptToInstall:
                    await DoInstall();
                    break;
                case Controls.YsInstallMode.CheckingForUpdates:
                    throw new NotImplementedException("The install button should be locked.");
                case Controls.YsInstallMode.ConfirmingUpdate:
                    throw new NotImplementedException("The install button should be locked.");
                case Controls.YsInstallMode.PromptToCheck:
                    await DoCheckForUpdates();
                    break;
                case Controls.YsInstallMode.PromptToUpdate:
                    await DoUpdate();
                    break;
                case Controls.YsInstallMode.Downloading:
                    throw new NotImplementedException("The install button should be locked.");
                case Controls.YsInstallMode.Unpacking:
                    throw new NotImplementedException("The install button should be locked.");
                case Controls.YsInstallMode.Launching:
                    throw new NotImplementedException("The install button should be locked.");
                case Controls.YsInstallMode.UpdatingLauncher:
                    throw new NotImplementedException("The install button should be locked.");
                case Controls.YsInstallMode.YouAreUpToDate:
                    break;
                default:
                    throw new NotImplementedException("The install button should be locked.");
            }
        }

        private double mLastDownloadBytes = 0.0;
        private DateTime mLastReportTime = DateTime.Now;
        private async Task DoInstall()
        {
            ElementMainPanelActionButtons.CurrentMode = Controls.YsInstallMode.Downloading;
            ElementDownloadBar.ChangeProgress(Controls.DownloadBarMode.Waiting);
            ElementDownloadBar.IsOpen = true;

            mLastDownloadBytes = 0.0;
            mLastReportTime = DateTime.Now;

            try
            {
                this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                await Logic.UpdatePlayHelpers.DownloadAndInstall(
                    (double currentBytes, double totalFileBytes) =>
                        {
                            this.Dispatcher.Invoke
                            (
                                (Action<double, double>)((inCurrentBytes, inTotalFileBytes) =>
                                {
                                    DateTime now = DateTime.Now;
                                    if ((now - mLastReportTime).TotalSeconds < 0.25) { return; }

                                    double currentPercent = (inCurrentBytes / inTotalFileBytes) * 100.0;
                                    currentPercent = Math.Max(Math.Min(currentPercent, 100), 0); // Clamp to 0->100%
                                    this.TaskbarItemInfo.ProgressValue = currentPercent / 100d;
                                    double timeSinceLastReport = (now - mLastReportTime).TotalSeconds;
                                    double currentSpeed = (inCurrentBytes - mLastDownloadBytes) / timeSinceLastReport;

                                    ElementDownloadBar.ChangeProgress(Controls.DownloadBarMode.DownloadingGame, currentPercent, currentSpeed);

                                    mLastReportTime = now;
                                    mLastDownloadBytes = inCurrentBytes;
                                }), new object[] { currentBytes, totalFileBytes }
                            );
                        },
                    () =>
                        {
                            this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
                            ElementMainPanelActionButtons.CurrentMode = Controls.YsInstallMode.Unpacking;
                            ElementDownloadBar.ChangeProgress(Controls.DownloadBarMode.Extracting);
                        }
                    );
            }
            catch(Logic.ServiceNotFoundException)
            {
                MessageBox.Show("The website did not provide a download link that the launcher could recognize." +
                    "\n\n" +
                    "If the launcher is reporting that a new launcher version is available, then download it. Otherwise, report a launcher issue from the launcher.",
                    "Cannot download game",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch(Logic.CannotConnectToMegaException ex)
            {
                string innerMessage = "";

                // Find the inner exception.
                if (ex.InnerException != null)
                {
                    innerMessage = "\n\nError Received: " + ex.InnerException.Message;
                }

                MessageBox.Show(
                    "Unable to download the game at this point in time." +
                    "\n\n" +
                    "The launcher downloads the game from Mega.nz, which might be temporarily limiting downloads to your IP address." +
                    "\n\n" +
                    "Please try again later, or download the game from one of the alternate links provided at:" +
                    "\n\n" +
                    "yanderedev.wordpress.com/downloads" + innerMessage,
                    "Cannot download game",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Logic.CannotLoginToMegaException)
            {
                MessageBox.Show(
                    "Unable to download the game." +
                    "\n\n" +
                    "The launcher failed to login to Mega.nz's download service." +
                    "\n\n" +
                    "Try to download the game (without the launcher) from one of the alternate links at:" +
                    "\n\n" +
                    "yanderedev.wordpress.com/downloads",
                    "Cannot download game",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "The launcher encountered an unknown error." +
                    "\n\n" +
                    "Please try again. If the problem persists, try going into the launcher folder and deleting YandereSimulator.zip and the YandereSimulator folder (if they exist)." +
                    "\n\n" +
                    "You can also download the game directly from one of the alternate links provided at:" +
                    "\n\n" +
                    "yanderedev.wordpress.com/downloads",
                    "Cannot download game",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            this.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            ElementDownloadBar.ChangeProgress(Controls.DownloadBarMode.Waiting);
            ElementDownloadBar.IsOpen = false;

            await DoCheckForUpdates();
        }

        private async Task DoCheckForUpdates()
        {
            ElementMainPanelActionButtons.CurrentMode = Controls.YsInstallMode.CheckingForUpdates;

            if (Logic.UpdatePlayHelpers.DoesGameExist())
            {
                if (await Logic.UpdatePlayHelpers.DoesUpdateExist())
                {
                    ElementMainPanelActionButtons.CurrentMode = Controls.YsInstallMode.PromptToUpdate;
                }
                else
                {
                    ElementMainPanelActionButtons.CurrentMode = Controls.YsInstallMode.YouAreUpToDate;
                    await Logic.UpdatePlayHelpers.AsynchronousWait(1000);
                    ElementMainPanelActionButtons.CurrentMode = Controls.YsInstallMode.PromptToCheck;
                }
            }
            else
            {
                ElementMainPanelActionButtons.CurrentMode = Controls.YsInstallMode.PromptToInstall;
            }
        }

        private async Task DoCheckForLauncherUpdate()
        {
            if (await Logic.UpdatePlayHelpers.DoesLauncherUpdateExist(App.LauncherVersion))
            {
                UpdateFlyin.IsOpen = true;
            }
        }

        private async Task DoUpdate()
        {
            if (Logic.UpdatePlayHelpers.IsGameRunning())
            {
                MessageBox.Show("Yandere Simulator is running. Please shut down the game before performing the update.", "Cannot update Yandere Simulator", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                await DoInstall();
            }
        }

        private async Task DoPlay()
        {
            ElementMainPanelActionButtons.CurrentMode = Controls.YsInstallMode.Launching;
            Logic.UpdatePlayHelpers.StartGame();
            await Logic.UpdatePlayHelpers.AsynchronousWait(500);
            Close();
        }

        private void VideoPlayer_OnDereReset(object sender, EventArgs e)
        {
            SetDere();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (ElementMainPanelActionButtons.CurrentMode == Controls.YsInstallMode.Unpacking)
            {
                if (MessageBox.Show("Closing the launcher while unpacking files will corrupt them.\n\nDo you wish to force-close the launcher anyway?", "Busy Extracting Files", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

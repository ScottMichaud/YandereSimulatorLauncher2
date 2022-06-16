using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private static readonly BitmapSource dereLogoSource = new BitmapImage(new Uri("/YandereSimulatorLauncher2;component/EmbeddedAssets/Images/yandere-simulator-logo.png", UriKind.Relative));
        private static readonly BitmapSource yanLogoSource = new BitmapImage(new Uri("/YandereSimulatorLauncher2;component/EmbeddedAssets/Images/yandere-simulator-logo-black.png", UriKind.Relative));

        public MainWindow()
        {
            UnpackVideoResources();
            InitializeComponent();
            HandleVisualStyles();
            AddEventHandlers(GetElementYanDereVideoPlayer());
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
                MessageBox.Show(Properties.Lang.Lang.x64Error, Properties.Lang.Lang.notSupported, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (IsRunningFromTempFileFolder)
            {
                // Slightly different title bar message so I can tell which is which from screenshots.
                MessageBox.Show(Properties.Lang.Lang.extract_files, Properties.Lang.Lang.extract_please_title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (IsRunningFromSystemFolder)
            {
                // Slightly different title bar message so I can tell which is which from screenshots.
                MessageBox.Show(Properties.Lang.Lang.extract_files, Properties.Lang.Lang.extract_please_title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            StartYanDereFlipFlop();

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

        private bool IsRunningFromTempFileFolder
        {
            get
            {
                string currentFullPath = System.IO.Path.GetFullPath("./").ToLowerInvariant();
                string tempFullPath = System.IO.Path.GetTempPath().ToLowerInvariant();

                // It would be better to base off of executing assembly location by reflection, but it has been like this
                // for years, and I don't have much time to support a follow-up release if it introduces obscure bugs.
                //string reflectionFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

                //MessageBox.Show(currentFullPath + "\n\n" + tempFullPath + "\n\n" + systemFullPath + "\n\n" + reflectionFullPath);

                return currentFullPath.StartsWith(tempFullPath);
            }
        }

        private bool IsRunningFromSystemFolder
        {
            get
            {
                string currentFullPath = System.IO.Path.GetFullPath("./").ToLowerInvariant();
                string systemFullPath = Environment.SystemDirectory.ToLowerInvariant();
                return currentFullPath.StartsWith(systemFullPath);
            }
        }

        private Controls.YanDereVideoPlayer GetElementYanDereVideoPlayer()
        {
            return ElementYanDereVideoPlayer;
        }

        private void AddEventHandlers(Controls.YanDereVideoPlayer elementYanDereVideoPlayer)
        {
            ElementMainPanelActionButtons.InstallButtonClicked += InstallButton_OnClick;
            ElementMainPanelActionButtons.PlayButtonClicked += PlayButton_OnClick;
            elementYanDereVideoPlayer.YanDereCheckboxClicked += VideoPlayer_OnDereReset;
        }

        private void UnpackVideoResources()
        {
            // On my PC, these two operations add a total of ~19ms to load time.
            // Probably don't need to do anything fancy.
            if (Directory.Exists(App.LauncherTempFileDirectory) == false)
            {
                Directory.CreateDirectory(App.LauncherTempFileDirectory);
            }

            CleanOldVideoFiles();
            UnpackVideoFile(YandereSimulatorLauncher2.Properties.Resources.mainpanel_dere, App.MainPanelDereFileLocation);
            UnpackVideoFile(YandereSimulatorLauncher2.Properties.Resources.mainpanel_yan, App.MainPanelYanFileLocation);
            SetVideoFileVersion();
        }

        private static bool AreVideoFilesOld
        {
            get
            {
                if (File.Exists(App.MainPanelVideosVersionFileLocation) == false) { return true; }
                string versionFileContents = File.ReadAllText(App.MainPanelVideosVersionFileLocation);
                
                if (int.TryParse(versionFileContents, out var number))
                {
                    return number < 2;
                }

                return true;
            }
        }

        private static void CleanOldVideoFiles()
        {
            if (AreVideoFilesOld)
            {
                if (File.Exists(App.MainPanelDereFileLocation))
                {
                    File.Delete(App.MainPanelDereFileLocation);
                }

                if (File.Exists(App.MainPanelYanFileLocation))
                {
                    File.Delete(App.MainPanelYanFileLocation);
                }
            }
        }

        private static void SetVideoFileVersion()
        {
            if (AreVideoFilesOld)
            {
                File.WriteAllText(App.MainPanelVideosVersionFileLocation, "2");
            }
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
            YandereSimulatorLogo.Source = dereLogoSource;

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
            YandereSimulatorLogo.Source = yanLogoSource;

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
                MessageBoxResult result = MessageBox.Show(Properties.Lang.Lang.start_failed, Properties.Lang.Lang.start_failed_title, MessageBoxButton.YesNo, MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    await DoInstall();
                }
            }
        }

        private async Task DoRelevantInstallTask()
        {
            switch (ElementMainPanelActionButtons.CurrentMode)
            {
                case Controls.YsInstallMode.Unset:
                    throw new NotImplementedException(Properties.Lang.Lang.revelant_install_task);
                case Controls.YsInstallMode.RetryInstall:
                    await DoInstall();
                    break;
                case Controls.YsInstallMode.PromptToInstall:
                    await DoInstall();
                    break;
                case Controls.YsInstallMode.CheckingForUpdates:
                    throw new NotImplementedException(Properties.Lang.Lang.revelant_install_task);
                case Controls.YsInstallMode.ConfirmingUpdate:
                    throw new NotImplementedException(Properties.Lang.Lang.revelant_install_task);
                case Controls.YsInstallMode.PromptToCheck:
                    await DoCheckForUpdates();
                    break;
                case Controls.YsInstallMode.PromptToUpdate:
                    await DoUpdate();
                    break;
                case Controls.YsInstallMode.Downloading:
                    throw new NotImplementedException(Properties.Lang.Lang.revelant_install_task);
                case Controls.YsInstallMode.Unpacking:
                    throw new NotImplementedException(Properties.Lang.Lang.revelant_install_task);
                case Controls.YsInstallMode.Launching:
                    throw new NotImplementedException(Properties.Lang.Lang.revelant_install_task);
                case Controls.YsInstallMode.UpdatingLauncher:
                    throw new NotImplementedException(Properties.Lang.Lang.revelant_install_task);
                case Controls.YsInstallMode.YouAreUpToDate:
                    break;
                default:
                    throw new NotImplementedException(Properties.Lang.Lang.revelant_install_task);
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
                MessageBox.Show(Properties.Lang.Lang.website_not_provide,
                    Properties.Lang.Lang.cannot_download_game,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Logic.CannotConnectToMegaException)
            {
                MessageBox.Show(Properties.Lang.Lang.download_eror,
                    Properties.Lang.Lang.cannot_download_game,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Logic.CannotLoginToMegaException)
            {
                MessageBox.Show(Properties.Lang.Lang.unable_to_download,
                    Properties.Lang.Lang.cannot_download_game,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(Properties.Lang.Lang.unknown_error,
                    Properties.Lang.Lang.cannot_download_game,
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
                MessageBox.Show(Properties.Lang.Lang.is_running, Properties.Lang.Lang.cannot_update, MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        private void VideoPlayer_OnDereReset(object sender, EventArgs e) => SetDere();

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (ElementMainPanelActionButtons.CurrentMode == Controls.YsInstallMode.Unpacking)
            {
                if (MessageBox.Show(Properties.Lang.Lang.unpack_error, Properties.Lang.Lang.busy_extracting, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

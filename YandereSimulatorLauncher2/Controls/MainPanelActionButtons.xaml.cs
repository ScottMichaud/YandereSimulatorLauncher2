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
    /// Interaction logic for MainPanelActionButtons.xaml
    /// </summary>
    public partial class MainPanelActionButtons : UserControl
    {
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
        }

        private void DoRender()
        {
            RenderButton(mutBorder: InstallUpdateButton, inIsPrimed: mIsInstallPrimed, inIsUserHovering: mIsUserHoveringInstall, inIsDere: IsDere);
            RenderButton(mutBorder: PlayButton, inIsPrimed: mIsPlayPrimed, inIsUserHovering: mIsUserHoveringPlay, inIsDere: IsDere);
            InstallUpdateButtonText.Foreground = IsDere ? App.HexToBrush("#ffffff") : App.HexToBrush("#000000");
            PlayButtonText.Foreground = IsDere ? App.HexToBrush("#ffffff") : App.HexToBrush("#000000");
        }

        private static void RenderButton(Border mutBorder, bool inIsPrimed, bool inIsUserHovering, bool inIsDere)
        {
            if (inIsPrimed)
            {
                mutBorder.Background = inIsDere ? App.HexToBrush("#ffade3") : App.HexToBrush("#ff5555");
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
                //Do action
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
                //Do action
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

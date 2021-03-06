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
    /// Interaction logic for ReportBugButton.xaml
    /// </summary>
    public partial class ReportBugButton : UserControl
    {
        public event EventHandler PlayButtonClicked;

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

        private bool mIsPrimed = false;
        private bool mIsHovering = false;

        public ReportBugButton()
        {
            InitializeComponent();
        }

        private void DoRender()
        {
            ColorButton();
        }

        private void ColorButton()
        {
            RootButton.BorderBrush = mIsDere ? App.HexToBrush("#95286d") : App.HexToBrush("#330000");

            if (mIsPrimed)
            {
                RootButton.Background = mIsDere ? App.HexToBrush("#ff91da") : App.HexToBrush("#ff5555");
            }
            else if (mIsHovering)
            {
                RootButton.Background = mIsDere ? App.HexToBrush("#d877b6") : App.HexToBrush("#b70000");
            }
            else
            {
                RootButton.Background = mIsDere ? App.HexToBrush("#ff80d3") : App.HexToBrush("#ff0000");
            }

            ButtonText.Foreground = mIsDere ? App.HexToBrush("#FFFFFF") : App.HexToBrush("#000000");
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            mIsPrimed = true;
            DoRender();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mIsPrimed == true)
            {
                PlayButtonClicked?.Invoke(this, new EventArgs());
            }

            mIsPrimed = false;
            DoRender();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            mIsHovering = true;
            DoRender();
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            mIsHovering = false;
            mIsPrimed = false;
            DoRender();
        }
    }
}

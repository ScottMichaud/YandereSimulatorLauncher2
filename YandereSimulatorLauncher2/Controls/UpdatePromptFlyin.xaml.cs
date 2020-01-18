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
using System.Windows.Media.Animation;

namespace YandereSimulatorLauncher2.Controls
{
    /// <summary>
    /// Interaction logic for UpdatePromptFlyin.xaml
    /// </summary>
    public partial class UpdatePromptFlyin : UserControl
    {
        public static readonly DependencyProperty IsDereProperty = DependencyProperty.Register("IsDere", typeof(bool), typeof(UpdatePromptFlyin), new PropertyMetadata(true, IsDereChanged));
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(UpdatePromptFlyin), new PropertyMetadata(false, IsOpenChanged));

        public bool IsDere
        {
            get { return (bool)GetValue(IsDereProperty); }
            set { SetValue(IsDereProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        private static void IsDereChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is UpdatePromptFlyin)
            {
                UpdatePromptFlyin castControl = obj as UpdatePromptFlyin;

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

        private static void IsOpenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is UpdatePromptFlyin)
            {
                UpdatePromptFlyin castControl = obj as UpdatePromptFlyin;

                if (castControl.IsOpen)
                {
                    castControl.SetOpen();
                }
                else
                {
                    castControl.SetClosed();
                }
            }
        }

        public UpdatePromptFlyin()
        {
            InitializeComponent();
        }

        private void SetDere()
        {
            SlidingContainer.Background = App.HexToBrush("#ff80d3");
            SlidingContainer.BorderBrush = App.HexToBrush("#95286d");
            UpdateLabel.Foreground = App.HexToBrush("#FFFFFF");
        }

        private void SetYan()
        {
            SlidingContainer.Background = App.HexToBrush("#ff0000");
            SlidingContainer.BorderBrush = App.HexToBrush("#330000");
            UpdateLabel.Foreground = App.HexToBrush("#000000");
        }

        private void SetOpen()
        {
            DoubleAnimation openAnimation = new DoubleAnimation(5, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            openAnimation.EasingFunction = new BackEase();
            SlidingContainer.BeginAnimation(Canvas.BottomProperty, openAnimation);
        }

        private void SetClosed()
        {
            DoubleAnimation closeAnimation = new DoubleAnimation(-50, new Duration(new TimeSpan(0, 0, 0, 0, 250)));
            SlidingContainer.BeginAnimation(Canvas.BottomProperty, closeAnimation);
        }
    }
}

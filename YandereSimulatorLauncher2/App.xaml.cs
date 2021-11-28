using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace YandereSimulatorLauncher2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(System.Windows.ExitEventArgs e)
        {
            base.OnExit(e);

            //DeleteVideoResources();
        }

        public static int LauncherVersion
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor; }
        }

        public static string LauncherTempFileDirectory 
        { 
            get
            {
                return System.IO.Path.Combine(System.IO.Path.GetTempPath(), "{" + YandereSimulatorLauncher2.Properties.Resources.VideoLocationGuid.ToUpper() + "}");
            }
        }

        public static string MainPanelVideosVersionFileLocation
        {
            get
            {
                return System.IO.Path.Combine(LauncherTempFileDirectory, "video-version.txt");
            }
        }

        public static string MainPanelDereFileLocation
        {
            get
            {
                return System.IO.Path.Combine(LauncherTempFileDirectory, "mainpanel-dere.wmv");
            }
        }

        public static string MainPanelYanFileLocation
        {
            get
            {
                return System.IO.Path.Combine(LauncherTempFileDirectory, "mainpanel-yan.wmv");
            }
        }

        public static System.Windows.Media.SolidColorBrush HexToBrush(string inHex)
        {
            return new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(inHex));
        }
    }
}

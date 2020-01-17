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

        public static double ExpectedDownloadSize
        {
            get
            {
                return 1.15 * (1024.0 * 1024.0 * 1024.0); //Assume ~1.15 GB.
            }
        }

        public static System.Windows.Media.SolidColorBrush HexToBrush(string inHex)
        {
            return new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(inHex));
        }

        //
        // Currently unused.
        // Don't waste time deleting the file on shutdown. 
        // It'll get overwritten next launch anyway.
        // Videos were moved since this was written anyway.
        //

        //private void DeleteVideoResources()
        //{
        //    try
        //    {
        //        if (File.Exists("mainpanel-dere.wmv"))
        //        {
        //            File.Delete("mainpanel-dere.wmv");
        //        }

        //        if (File.Exists("mainpanel-yan.wmv"))
        //        {
        //            File.Delete("mainpanel-yan.wmv");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        // If we fail to delete -- let it remain.
        //    }
        //}
    }
}

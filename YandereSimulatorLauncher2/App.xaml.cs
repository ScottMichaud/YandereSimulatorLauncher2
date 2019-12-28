using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

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

            DeleteVideoResources();
        }

        private void DeleteVideoResources()
        {
            try
            {
                if (File.Exists("mainpanel-dere.wmv"))
                {
                    File.Delete("mainpanel-dere.wmv");
                }

                if (File.Exists("mainpanel-yan.wmv"))
                {
                    File.Delete("mainpanel-yan.wmv");
                }
            }
            catch (Exception)
            {
                // If we fail to delete -- let it remain.
            }
        }
    }
}

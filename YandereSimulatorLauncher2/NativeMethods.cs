using System.Runtime.InteropServices;

namespace YandereSimulatorLauncher2
{
    class NativeMethods
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(out bool isEnabled);

        internal static bool DwmCompositionIsEnabled
        {
            get
            {
                if (DwmIsCompositionEnabled(out bool isEnabled) == 0)
                {
                    return isEnabled;
                }

                return false;
            }
        }
    }
}

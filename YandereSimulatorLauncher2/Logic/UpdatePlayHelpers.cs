using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace YandereSimulatorLauncher2.Logic
{
    class UpdatePlayHelpers
    {
        public const string GamePath = "YandereSimulator\\YandereSimulator.exe";
        public const string GameVersionHttp = "https://www.yanderesimulator.com/version.txt";
        public const string GameVersionFilePath = "YandereSimulator\\GameVersion.txt";

        public static void StartGame()
        {
            using (Process game = new Process())
            {
                game.StartInfo.UseShellExecute = false;
                game.StartInfo.FileName = GamePath;
                game.Start();
            }
        }

        public static bool DoesGameExist()
        {
            return System.IO.File.Exists(GamePath);
        }

        public async static Task<bool> DoesUpdateExist()
        {
            // Queue
            Task<string> versionOnSite = FetchHttpText(GameVersionHttp);
            Task<string> versionOnDisk = FetchTextFileContents(GameVersionFilePath);
            Task minimumCheckTime = AsynchronousWait(250);

            // Consume
            string siteVersion = await versionOnSite;
            string diskVersion = await versionOnDisk;
            await minimumCheckTime;

            // Process
            return IsUpdateRequired(inSiteVersion: siteVersion, inDiskVersion: diskVersion);
        }

        public async static Task AsynchronousWait(int inMilliseconds)
        {
            await Task.Delay(inMilliseconds);
        }

        private async static Task<string> FetchTextFileContents(string inPath)
        {
            if (System.IO.File.Exists(inPath) == false) { return ""; }

            try
            {
                using (System.IO.StreamReader streamReader = System.IO.File.OpenText(inPath))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private async static Task<string> FetchHttpText(string inUrl)
        {
            try
            {
                using (System.Net.Http.HttpClient getClient = new System.Net.Http.HttpClient())
                {
                    System.Net.Http.HttpResponseMessage response = await getClient.GetAsync(inUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }

                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static bool IsUpdateRequired(string inSiteVersion, string inDiskVersion)
        {
            if (string.IsNullOrWhiteSpace(inDiskVersion)) { return true; }
            if (string.IsNullOrWhiteSpace(inSiteVersion)) { return false; }

            double diskAsDouble;
            double siteAsDouble;
            if (double.TryParse(inDiskVersion, out diskAsDouble) == false) { return true; }
            if (double.TryParse(inSiteVersion, out siteAsDouble) == false) { return false; }

            return siteAsDouble > diskAsDouble;
        }
    }
}

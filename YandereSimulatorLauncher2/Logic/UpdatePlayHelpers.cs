using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using CG.Web.MegaApiClient;

namespace YandereSimulatorLauncher2.Logic
{
    public delegate void DownloadProgressCallback(double bytes, double totalSizeInBytes);
    public delegate void UnzipStartCallback();

    class UpdatePlayHelpers
    {
        private static readonly HttpClient staticHttpClient = new HttpClient();

        public static string GameExePath { get { return "YandereSimulator\\YandereSimulator.exe"; } }
        public static string GameDirectoryPath { get { return "YandereSimulator"; } }
        public static string GameVersionHttp { get { return "https://www.yanderesimulator.com/version.txt" + AntiCacheToken; } }
        public static string GameFileHttpMinusCacheBuster = "https://dl.yanderesimulator.com/latest.zip"; //Add ?{{versionOnSite}} to it.
        public static string GameVersionFilePath = "YandereSimulator\\GameVersion.txt";
        public static string GameZipSaveLocation = "YandereSimulator.zip";

        public static string LauncherVersionHttp { get { return "https://www.yanderesimulator.com/launcherversion.txt" + AntiCacheToken; } }

        public static string SiteUrlsTxtUrl { get { return "http://yanderesimulator.com/urls.txt" + AntiCacheToken; } }

        private static string AntiCacheToken
        {
            get
            {
                DateTime currentTime = DateTime.UtcNow;
                return "?" + currentTime.Ticks.ToString();
            }
        }

        public static void StartGame()
        {
            using (Process game = new Process())
            {
                game.StartInfo.UseShellExecute = false;
                game.StartInfo.FileName = GameExePath;
                game.Start();
            }
        }

        public static bool DoesGameExist()
        {
            return System.IO.File.Exists(GameExePath);
        }

        public async static Task DownloadAndInstall(DownloadProgressCallback delDownloadProgress, UnzipStartCallback delUnzipStart)
        {
            // Fetch the current version.
            // NOTE: YandereDev says that he always updates version.txt *after* the new build upload is complete.
            //       As such, we should be able to rely upon version.txt as a cache buster.
            //       If he goofs, he can just re-increment version.txt.

            string versionOnSite = await FetchHttpText(GameVersionHttp);
            await FetchMegaFile(inSaveLocation: GameZipSaveLocation, delProgress: delDownloadProgress);

            if (System.IO.Directory.Exists(GameDirectoryPath))
            {
                // C# struggles to delete folders if, for example, there's an active Windows Explorer looking into them.
                bool successful = DeleteAsMuchAsPossible(GameDirectoryPath);
            }

            delUnzipStart();
            await UnpackZipFile(inFile: GameZipSaveLocation, inUnpackLocation: GameDirectoryPath);

            using (System.IO.StreamWriter gameVersionFile = System.IO.File.CreateText(GameVersionFilePath))
            {
                gameVersionFile.WriteLine(versionOnSite);
            }

            if (System.IO.File.Exists(GameZipSaveLocation))
            {
                System.IO.File.Delete(GameZipSaveLocation);
            }
        }

        public async static Task<bool> DoesUpdateExist()
        {
            // Queue
            Task<string> versionOnSite = FetchHttpText(GameVersionHttp);
            Task<string> versionOnDisk = FetchTextFileContents(GameVersionFilePath);
            // Add a little lag so it doesn't instantly complete (which flickers text on the update/install button).
            Task minimumCheckTime = AsynchronousWait(500);

            // Consume
            string siteVersion = await versionOnSite;
            string diskVersion = await versionOnDisk;
            await minimumCheckTime;

            // Process
            return IsUpdateRequired(inSiteVersion: siteVersion, inDiskVersion: diskVersion);
        }

        public async static Task<bool> DoesLauncherUpdateExist(int inLauncherVersion)
        {
            Task<string> versionOnSite = FetchHttpText(LauncherVersionHttp);
            Task minimumCheckTime = AsynchronousWait(500);

            string siteVersion = await versionOnSite;
            await minimumCheckTime;

            return IsLauncherUpdateRequired(inSiteVersion: siteVersion, inAssemblyVersion: inLauncherVersion);
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
                using (HttpResponseMessage response = await staticHttpClient.GetAsync(inUrl))
                {
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

            if (double.TryParse(inDiskVersion, out double diskAsDouble) == false) { return true; }
            if (double.TryParse(inSiteVersion, out double siteAsDouble) == false) { return false; }

            return siteAsDouble > diskAsDouble;
        }

        private static bool IsLauncherUpdateRequired(string inSiteVersion, int inAssemblyVersion)
        {
            if (string.IsNullOrWhiteSpace(inSiteVersion)) { return false; }

            if (double.TryParse(inSiteVersion, out double siteAsDouble) == false) { return false; }

            return siteAsDouble > inAssemblyVersion;
        }

        private async static Task FetchMegaFile(string inSaveLocation, DownloadProgressCallback delProgress)
        {
            string megaUrl = await FetchMegaUrl();

            if (File.Exists(inSaveLocation))
            {
                File.Delete(inSaveLocation);
            }

            // Because the MegaApiClient just Task.Run()s (rather than actually parking until the native events are over)
            // I might want to use the single-threaded API all wrapped in a single Task.Run(). Probably negligible gains
            // though (only save ~number of commands - 1 threadpool requests) and it could be annoying if I want to handle
            // user input for ex: "Do you want to retry?"
            MegaApiClient client = new MegaApiClient();
            try
            {
                await client.LoginAnonymousAsync();
                INodeInfo node = await client.GetNodeFromLinkAsync(new Uri(megaUrl));
                await client.DownloadFileAsync(new Uri(megaUrl), inSaveLocation, new ProgressReporter(delProgress, node.Size));
            }
            catch (Exception ex)
            {
                // Check to see if we can split up the errors any further.
                throw new CannotConnectToMegaException("", ex); ;
            }
            finally
            {
                await client.LogoutAsync();
            }
        }

        private async static Task<string> FetchMegaUrl()
        {
            List<string> urls = SplitToLines(await FetchHttpText(SiteUrlsTxtUrl));
            string megaLine = PickRelevantLineFromList(urls, "mega.nz");
            if (megaLine is null) { throw new ServiceNotFoundException("Mega.nz"); }
            string megaUrl = PickUrlFromLine(megaLine);
            if (megaUrl is null) { throw new ServiceNotFoundException("Mega.nz"); }
            return megaUrl;
        }

        private static List<string> SplitToLines(string inString)
        {
            List<string> output = new List<string>();
            
            using (StringReader reader = new StringReader(inString))
            {
                string line = reader.ReadLine();

                while ((line is null) == false)
                {
                    output.Add(line);
                    line = reader.ReadLine();
                }
            }

            return output;
        }

        private static string PickRelevantLineFromList(List<string> inList, string inToken)
        {
            foreach (string currentString in inList)
            {
                if (currentString.Contains(inToken)) { return currentString; }
            }

            return null;
        }

        private static string PickUrlFromLine(string inLine)
        {
            if (inLine is null) { return null; }
            string[] lineTokens = inLine.Trim().Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            if (lineTokens.Length == 2) { return lineTokens[1].Trim(); }
            return null;
        }

        private async static Task UnpackZipFile(string inFile, string inUnpackLocation)
        {
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(sourceArchiveFileName: inFile, destinationDirectoryName: inUnpackLocation);
            });
        }

        private static bool DeleteAsMuchAsPossible(string inPath)
        {
            bool totalSuccess = true;
            string[] childFiles = System.IO.Directory.GetFiles(inPath);
            string[] childDirectories = System.IO.Directory.GetDirectories(inPath);

            foreach (string currentFile in childFiles)
            {
                try
                {
                    if (System.IO.File.GetAttributes(currentFile).HasFlag(System.IO.FileAttributes.ReparsePoint)) { continue; }
                    System.IO.File.SetAttributes(currentFile, System.IO.FileAttributes.Normal);
                    System.IO.File.Delete(currentFile);
                }
                catch (Exception)
                {
                    totalSuccess = false;
                }
            }

            foreach (string currentDir in childDirectories)
            {
                if (DeleteAsMuchAsPossible(currentDir) == false)
                {
                    totalSuccess = false;
                }
            }

            try
            {
                System.IO.Directory.Delete(inPath, false);
            }
            catch (Exception)
            {
                totalSuccess = false;
            }

            return totalSuccess;
        }
    }

    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string inMessage)
            : base(inMessage)
        {

        }
    }

    public class CannotConnectToMegaException : Exception
    {
        public CannotConnectToMegaException(string inMessage, Exception inInnerException)
            : base(inMessage, inInnerException)
        {

        }
    }

    public class ProgressReporter : IProgress<double>
    {
        readonly DownloadProgressCallback callback;
        readonly double totalFileBytes = 1;

        public ProgressReporter(DownloadProgressCallback inCallback, double inTotalFileBytes)
        {
            callback = inCallback;
            totalFileBytes = inTotalFileBytes;
        }

        public void Report(double value)
        {
            callback(value * totalFileBytes / 100.0, totalFileBytes);
        }
    }
}

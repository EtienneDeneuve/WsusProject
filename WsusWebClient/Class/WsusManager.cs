using System.Diagnostics;
using System.Runtime.InteropServices;
using WUApiLib;

namespace GetWsusGroup.Class
{
    class WsusManager
    {
        internal static void Wsus(string option)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.WindowStyle = ProcessWindowStyle.Maximized;
            psi.Arguments = $"/c wuauclt {option}";
            Process.Start(psi);
        }

        internal static void WsusResetAuth()
        {
            Wsus("/a");
        }
        internal static void WsusReportNow()
        {
            Wsus("/r");
        }
        internal static void WsusDectectNow()
        {
            Wsus("/DetectNow");
        }
        internal static void WsusCheckWsus()
        {

            UpdateSearcher usearcher = new UpdateSearcher();
          
            var search = usearcher.Search("IsInstalled=0 and IsHidden=0");
            if (search.Updates.Count > 0)
            {
                Program.LogInfo($"There are updates available for installation");
                WsusDownloadFromWsus(search);
            }
            else
            {
                Program.LogInfo($"There are no updates available for installation");
            }

        }
        internal static void WsusDownloadFromWsus(ISearchResult search)
        {

            UpdateDownloader udownloader = new UpdateDownloader();
            udownloader.Updates = search.Updates;
            udownloader.Download();
            UpdateCollection updatesToInstall = new UpdateCollection();
            foreach (IUpdate update in search.Updates)
            {
                if (update.IsDownloaded)
                    updatesToInstall.Add(update);
            }
            WsusInstaller(updatesToInstall);
        }

        internal static void WsusInstaller(UpdateCollection updatesToInstall)
        {
            IUpdateInstaller installer = new UpdateInstaller();
            installer.Updates = updatesToInstall;
            IInstallationResult installationRes = installer.Install();
            for (int i = 0; i < updatesToInstall.Count; i++)
            {
                if (installationRes.GetUpdateResult(i).HResult == 0)
                {
                    Program.LogInfo($"Installed :  {updatesToInstall[i].Title}");
                }
                else
                {
                    Program.LogInfo($"Failed :  {updatesToInstall[i].Title}");
                }
            }
        }

        internal static void CheckUpdate()
        {
            WsusResetAuth();
            WsusReportNow();
            WsusDectectNow();
        }
    }
}

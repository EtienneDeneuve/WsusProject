using GetWsusGroup.Class;
using Models;
using System;
using System.Configuration;
using System.Diagnostics;

namespace GetWsusGroup
{

    class Program
    {
        static readonly string SOURCE = ConfigurationManager.AppSettings["LogSource"].ToString();
        static readonly string LOG = ConfigurationManager.AppSettings["EventLog"].ToString();
        static readonly string SERVICE_NAME = ConfigurationManager.AppSettings["WsusService"].ToString();

        static void Main(string[] args)
        {
            if (!EventLog.SourceExists(SOURCE))
            {
                EventLog.CreateEventSource(SOURCE, LOG);
            }

            var wbMgr = new WebManager();
            try
            {
                wbMgr.GetAuthCookie().Wait();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(ExceptionMessage(e));
#if DEBUG
                Console.ReadLine();
#endif
                return;

            }
            try
            {
                var computer = wbMgr.GetGroupNameAsync().Result;
                wbMgr.SetCurrentVersion(
                    new VersionModel
                    {
                        ApplicationName = VersionManager.GetApplicationName(),
                        ApplicationVersion = VersionManager.GetApplicationVersion(),
                        ComputerName = computer.Name,
                        ComputerGroup = computer.Group
                    }).Wait();
                ServicesManager.StopService(SERVICE_NAME);
                CertificateManager.InstallCerticate(wbMgr.GetCertificate().Result);
                RegistryManager.RegisterKey(computer);
                ServicesManager.StartService(SERVICE_NAME);
                WsusManager.CheckUpdate();
                WsusManager.WsusCheckWsus();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(ExceptionMessage(e));
#if DEBUG
                Console.ReadLine();
#endif
                return;

            }
#if DEBUG
            Console.ReadLine();
#endif

        }

        internal static void LogError(string message, Exception e)
        {
            EventLog.WriteEntry(SOURCE, $"Error : {message}{Environment.NewLine}{ExceptionMessage(e)}", EventLogEntryType.Error, 234);
        }

        internal static void LogInfo(string message)
        {
            EventLog.WriteEntry(SOURCE, $"Success : {message}", EventLogEntryType.Information, 235);
        }

        internal static string ExceptionMessage(Exception e, int depth = 0)
        {
            if (e == null)
                return string.Empty;

            return $"{"".PadLeft(depth, ' ')}{e.Message}{Environment.NewLine}{ExceptionMessage(e.InnerException, ++depth)}";
        }
    }
}

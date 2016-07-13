using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetWsusGroup.Class
{
    class VersionManager
    {
        static readonly string SoftwareEditor = ConfigurationManager.AppSettings["SoftwareEditor"].ToString();

        static readonly string ApplicationPathx86 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), SoftwareEditor);
        static readonly string ApplicationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), SoftwareEditor).Replace(" (x86)","");

        static string ExeFile = null;



        static VersionManager()
        {
            var NameArray = ConfigurationManager.AppSettings["SoftwareNameVersion"].ToString().Split(',');
            ExeFile = GetExeFile(ApplicationPath, NameArray);
            if (ExeFile == null)
            {
                ExeFile = GetExeFile(ApplicationPathx86, NameArray);
            }
           
        }

        internal static string GetExeFile(string path, string[] appNames)
        {
            Console.WriteLine(path);
            if (!Directory.Exists(path))
            {
                return null;
            }
            foreach (var exeFile in Directory.GetFiles(path, "*.exe", SearchOption.AllDirectories))
            {
                if (appNames != null && appNames.Any(name => exeFile.Contains(name)))
                {
                    return exeFile;
                }
            }
            return null;
        }


        internal static string GetApplicationName()
        {
            if (ExeFile == null)
                throw new FileNotFoundException("No Application Found !");

            return ExeFile;
        }
        internal static string GetApplicationVersion()
        {
            if (ExeFile == null)
                throw new FileNotFoundException("No Application Found !");

            return FileVersionInfo.GetVersionInfo(ExeFile).ProductVersion;
        }
    }
}

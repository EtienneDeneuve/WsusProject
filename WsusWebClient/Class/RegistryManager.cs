using Microsoft.Win32;
using Models;
using System;

namespace GetWsusGroup.Class
{
    internal static class RegistryManager
    {
        private const string WU_KEY = "SOFTWARE\\Policies\\Microsoft\\Windows\\WindowsUpdate";
        private const string TARGET_GROUP = "TargetGroup";

        internal static void RegisterKey(ComputerModel computer)
        {
            try
            {
                var groupName = computer.Group.Replace("\"", "");
#if DEBUG
                Console.WriteLine("{0}\t{1}", computer.Name, groupName);
#endif
                RegistryKey myKey = Registry.LocalMachine.OpenSubKey(WU_KEY, true);
                if (myKey != null)
                {
                    var oldValue = myKey.GetValue(TARGET_GROUP);
                    myKey.SetValue(TARGET_GROUP, groupName);
                    Program.LogInfo($"{TARGET_GROUP}: Old : {oldValue} => New : {groupName}");
                }
            }
            catch (Exception e)
            {
                var msg = $"RegisterKey => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }

        }
    }

}
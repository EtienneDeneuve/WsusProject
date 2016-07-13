using System;
using System.ServiceProcess;


namespace GetWsusGroup.Class
{
    class ServicesManager
    {
        public static void StopService(string serviceName)
        {
            ServiceController serviceController = new ServiceController(serviceName);
            try
            {
                if ((serviceController.Status.Equals(ServiceControllerStatus.Running)) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    serviceController.Stop();
                }
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
            }
            catch (Exception e)
            {
                var msg = $"StopService {serviceName} => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }
        }
        public static void StartService(string serviceName)
        {
            ServiceController serviceController = new ServiceController(serviceName);
            try
            {
                if (!serviceController.Status.Equals(ServiceControllerStatus.Running))
                {
                    serviceController.Start();
                }
                serviceController.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch (Exception e)
            {
                var msg = $"StartService {serviceName} => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }
        }

    }
}

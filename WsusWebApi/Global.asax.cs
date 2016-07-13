using Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;

namespace WsusGroupWS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //string filename = WebConfigurationManager.AppSettings["csvFile"];
            //char delim = WebConfigurationManager.AppSettings["Delimiter"][0];
            //if (File.Exists(filename))
            //{
            //    Application["Computers"] = new List<ComputerModel>(File.ReadAllLines(filename).Select(line => new ComputerModel { Name = line.Split(delim)[0], Group = line.Split(delim)[1] }));
            //}
        }
    }
}

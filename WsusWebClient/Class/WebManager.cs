using Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GetWsusGroup.Class
{
    public class WebManager
    {
        HttpClient client;

        public WebManager()
        {
            client = new HttpClient(new HttpClientHandler { CookieContainer = new CookieContainer() });
        }

        public async Task GetAuthCookie()
        {
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServiceUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var adsiName = $"WinNT://{Environment.MachineName},Computer";

            DirectoryEntry dirEntry = null;
            try
            {
                dirEntry = new DirectoryEntry(adsiName);
            }
            catch (Exception e)
            {
                var msg = $"Get DirectoryEntry => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }

            byte[] objSid = null;
            try
            {
                objSid = (byte[])new DirectoryEntry(adsiName).Children
                            .Cast<DirectoryEntry>().First().InvokeGet("objectSID");
            }
            catch (Exception e)
            {
                var msg = $"Get Object SID => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }

            SecurityIdentifier ComputerSID = null;
            try
            {
                ComputerSID = new SecurityIdentifier(objSid, 0).AccountDomainSid;
            }
            catch (Exception e)
            {
                var msg = $"Get Account Domain SID => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }

            var dataContent = new AuthModel()
            {
                Sid = ComputerSID.ToString(),
                Key = ConfigurationManager.AppSettings["KeyStore"]
            };
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(dataContent), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/Auth/Authorize", content);
                if (response.IsSuccessStatusCode)
                {
                    Program.LogInfo("Authorized");
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (HttpRequestException hre)
            {
                var msg = $"Get Authorize => {hre.Message}";
                Program.LogError(msg, hre);
                throw new Exception(msg, hre);
            }
            catch (Exception e)
            {
                var msg = $"Get Authorize => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }
        }

        public async Task<ComputerModel> GetGroupNameAsync()
        {
            var computername = System.Environment.MachineName;
#if DEBUG
            Console.WriteLine("{0}", computername);
#endif
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/Computer/GetComputerGroup?Name=" + computername);
                if (response.IsSuccessStatusCode)
                {
                    return new ComputerModel
                    {
                        Name = computername.ToString(),
                        Group = response.Content.ReadAsStringAsync().Result
                    };
                }
                else
                {
                    Program.LogError(response.ReasonPhrase, new Exception("GetComputerGroup Status Error"));
                    return null;
                }
            }
            catch (Exception e)
            {
                var msg = $"Get ComputerGroup => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }
        }

        public async Task SetCurrentVersion(VersionModel dataContent)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(JsonConvert.SerializeObject(dataContent), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("api/Data/SetVersion", content);
                if (response.IsSuccessStatusCode)
                {
                    Program.LogInfo("Software Version Sent !");
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (HttpRequestException hre)
            {
                var msg = $"Set Current Version => {hre.Message}";
                Program.LogError(msg, hre);
                throw new Exception(msg, hre);
            }
            catch (Exception e)
            {
                var msg = $"Set CurrentVersion => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }
        }

        public async Task<string> GetCertificate()
        {
            try
            {
                //var toto = new WebClient();
                //toto.DownloadFile();
                //toto.DownloadData();
                //TODO: Add WebClient.DownloadFile();

                var wc = new WebClient();
                var WebClientURI = new Uri($"{ConfigurationManager.AppSettings["ServiceUrl"].ToString()}/api/Certificate/GetCertificate");
                wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                var certFile = Path.Combine(Path.GetTempPath(), "wsus.cer");
                await Task.Run(() => wc.DownloadFileAsync(WebClientURI, certFile));

                return certFile;
            }
            catch (HttpRequestException hre)
            {
                var msg = $"Set Current Version => {hre.Message}";
                Program.LogError(msg, hre);
                throw new Exception(msg, hre);
            }
            catch (Exception e)
            {
                var msg = $"Set CurrentVersion => {e.Message}";
                Program.LogError(msg, e);
                throw new Exception(msg, e);
            }
        }
    }
}

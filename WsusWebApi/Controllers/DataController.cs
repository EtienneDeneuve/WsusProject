using Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;

namespace WsusGroupWS.Controllers
{
    public class DataController : ApiController
    {
        [Authorize]
        [HttpPost]
        public void SetVersion(VersionModel version)
        {
            try
            {
                var client = new HttpClient(new HttpClientHandler { CookieContainer = new CookieContainer() });
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["DataServiceUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(JsonConvert.SerializeObject(version), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("api/Version/Save", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    InternalServerError(new Exception(response.ReasonPhrase));
                }
            }
            catch (HttpRequestException hre)
            {
                var msg = $"Set OpenPharma Version => {hre.Message}";
                InternalServerError(new Exception(msg, hre));
            }
            catch (Exception e)
            {
                var msg = $"Set OpenPharma Version => {e.Message}";
                InternalServerError(new Exception(msg, e));
            }
        }
    }
}
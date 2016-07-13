using System.Collections.Generic;
using System.Web.Http;
using System.Web;
using Models;
using System.Net.Http;
using System;
using System.Web.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;

namespace WsusGroupWS.Controllers
{
    public class ComputerController : ApiController
    {
        //[Authorize]
        public IEnumerable<ComputerModel> GetAllComputers()
        {
            return (List<ComputerModel>)HttpContext.Current.Application["Computers"];
        }

        [Authorize]
        public IHttpActionResult GetComputerGroup(string Name)
        {
            try
            {
                var client = new HttpClient(new HttpClientHandler { CookieContainer = new CookieContainer() });
                client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["DataServiceUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/Computer/GetComputerByName?ComputerName=" + Name).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return InternalServerError(new Exception(response.ReasonPhrase));
                }
                else
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(VersionModel));
                    var model = jsonSerializer.ReadObject(response.Content.ReadAsStreamAsync().Result) as VersionModel;
                    if (model == null)
                        return Ok("Not found !");
                    return Ok(model.ComputerGroup.ToString());
                }
            }
            catch (HttpRequestException hre)
            {
                var msg = $"Get Computer Group => {hre.Message}";
                return InternalServerError(new Exception(msg, hre));
            }
            catch (Exception e)
            {
                var msg = $"Get Computer Groupn => {e.Message}";
                return InternalServerError(new Exception(msg, e));
            }
        }
    }
}

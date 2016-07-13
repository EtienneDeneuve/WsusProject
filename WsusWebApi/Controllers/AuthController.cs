using Models;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Security;

namespace WsusGroupWS.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost]
        public void Authorize(AuthModel Model)
        {
            if (WebConfigurationManager.AppSettings["KeyStore"] == Model.Key)
            {
                FormsAuthentication.SetAuthCookie(Model.Sid, true);
            }
        }
    }
}
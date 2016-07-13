using Models;
using System.Linq;
using System.Web.Http;

namespace WsusDataWepApi.Controllers
{
    public class ComputerController : ApiController
    {
        public VersionModel GetComputerByName(string ComputerName)
        {
            try
            {
                using (var ctx = new VersionContext())
                {
                    return ctx.VersionModels.First(vm => vm.ComputerName == ComputerName);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
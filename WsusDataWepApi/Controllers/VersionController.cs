using Models;
using System;
using System.Data.SqlClient;
using System.Web.Http;

namespace WsusDataWepApi.Controllers
{
    public class VersionController : ApiController
    {
        [HttpPost]
        public void Save(VersionModel version)
        {
            try
            {
                using (var ctx = new VersionContext())
                {
                    var query = ctx.VersionModels.SqlQuery(@"
                            SELECT * 
                            FROM dbo.VersionModels 
                            WHERE ComputerGroup=@ComputerGroup
                            AND ComputerName=@ComputerName
                            AND ApplicationName=@ApplicationName",
                            new SqlParameter("@ComputerGroup", version.ComputerGroup),
                            new SqlParameter("@ComputerName", version.ComputerName),
                            new SqlParameter("@ApplicationName", version.ApplicationName));

                    var model = query.FirstAsync().Result;
                    if (model != null)
                    {
                        model.ApplicationVersion = version.ApplicationVersion;
                        model.LastCheckTimestamp = DateTime.Now;
                    }
                    else
                    {
                        version.LastCheckTimestamp = DateTime.Now;
                        ctx.VersionModels.Add(version);
                    }

                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                InternalServerError(e);
            }
        }
    }
}
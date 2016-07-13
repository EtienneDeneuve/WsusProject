using Models;
using System.Data.Entity;

namespace WsusDataWepApi
{
    public class VersionContext : DbContext
    {
        public VersionContext() : base("name=VersionDBConnectionString")
        {
            Database.SetInitializer<VersionContext>(new CreateDatabaseIfNotExists<VersionContext>());
        }
        public DbSet<VersionModel> VersionModels { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Clean_Planet
{
    internal class AppDBContext : DbContext
    {
        public AppDBContext() : base("name=CleanPlanetEntities") { }

        public DbSet<Partners_import> partners{ get; set; }
        public DbSet<Partner_services_import> partner_Services{ get; set; }
        public DbSet<Services_import> services{ get; set; }
        public DbSet<Service_type_import> service_Types{ get; set; }
        public DbSet<Material_type_import> material_Types{ get; set; }
    }
}

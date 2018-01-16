using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using anchor1.utilities;
using anchor1v.Models;

namespace anchor1.Models
{
    public class loginContext : DbContext
    {

        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<anchor1.Models.anchor1Context>());

        public loginContext() 
            : base(connstring())
        {
        }

        public DbSet<tab_database> tab_database{ get; set; }
        public DbSet<tab_photo_coy> tab_photo_coy { get; set; }
        public DbSet<tab_soft> tab_soft { get; set; }

        private static string connstring()
        {
            Clogin cqx = new Clogin();
            return cqx.baseconnect().ToString();

        }
    }
}

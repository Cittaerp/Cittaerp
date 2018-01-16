using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using anchor1.Models;
using anchor1v.Models;
using anchor1.utilities;

namespace anchor1.Models
{
    public class anchor1Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<anchor1.Models.anchor1Context>());

        public anchor1Context() 
            : base(connstring())
        {
            
            // Get the ObjectContext related to this DbContext
            //var objectContext = (this as IObjectContextAdapter).ObjectContext;
            string timeouts = ConfigurationManager.AppSettings["ctime"];

            // Sets the command timeout for all the commands
            this.Database.CommandTimeout = Convert.ToInt16(timeouts);
        }

        public DbSet<tab_bank> tab_bank { get; set; }
        public DbSet<tab_allow> tab_allow { get; set; }
        public DbSet<tab_analy> tab_analy { get; set; }
        public DbSet<tab_calc> tab_calc { get; set; }
        public DbSet<tab_loan> tab_loan { get; set; }
        public DbSet<tab_soft> tab_soft { get; set; }
        public DbSet<tab_analy_code> tab_analy_code { get; set; }
        public DbSet<tab_grade> tab_grade { get; set; }
        public DbSet<tab_over> tab_over { get; set; }
        public DbSet<tab_tax> tab_tax { get; set; }
        public DbSet<tab_category_advice> tab_category_advice { get; set; }
        public DbSet<tab_array> tab_array { get; set; }
        public DbSet<tab_calend> tab_calend { get; set; }
        public DbSet<tab_train> tab_train { get; set; }
        public DbSet<tab_daily> tab_daily { get; set; }
        public DbSet<tab_temp_rep> tab_temp_rep { get; set; }
        public DbSet<tab_default> tab_default { get; set; }
        public DbSet<tab_staff> tab_staff { get; set; }
        public DbSet<tab_pen_pfa> tab_pen_pfa { get; set; }
        public DbSet<tab_job_desc> tab_job_desc { get; set; }
        public DbSet<tab_trans_input> tab_trans_input { get; set; }
        public DbSet<tab_trans_over> tab_trans_over { get; set; }
        public DbSet<tab_trans_loan> tab_trans_loan { get; set; }
        public DbSet<tab_mast_loan> tab_mast_loan { get; set; }
        public DbSet<tab_mast_status> tab_mast_status { get; set; }
        public DbSet<tab_trans_allow> tab_trans_allow { get; set; }
        public DbSet<tab_trans_daily> tab_trans_daily { get; set; }
        public DbSet<tab_trans_prom> tab_trans_prom { get; set; }
        public DbSet<tab_trans_stat> tab_trans_stat { get; set; }
        public DbSet<tab_document> tab_document { get; set; }
        public DbSet<tab_advice> tab_advice { get; set; }
        public DbSet<tab_staff_person> tab_staff_person { get; set; }
        public DbSet<tab_photo_coy> tab_photo_coy { get; set; }
        public DbSet<tab_attach> tab_attach { get; set; }
        public DbSet<tab_mast_allow> tab_mast_allow { get; set; }
        public DbSet<tab_self_approval> tab_self_approval { get; set; }
        public DbSet<tab_type> tab_type { get; set; }
        public DbSet<tab_type2> tab_type2 { get; set; }
        public DbSet<tab_defined> tab_defined { get; set; }
        public DbSet<tab_layout> tab_layout { get; set; }
        public DbSet<tab_account> tab_account { get; set; }
        public DbSet<tab_train_default> tab_train_default { get; set; }
        public DbSet<tab_photo> tab_photo { get; set; }
        public DbSet<tab_holiday> tab_holiday { get; set; }
        public DbSet<tab_delegate> tab_delegate { get; set; }
        public DbSet<tab_transaction> tab_transaction { get; set; }
        public DbSet<tab_depend> tab_depend { get; set; }
        public DbSet<tab_train_group> tab_train_group { get; set; }
        public DbSet<tab_train_plan> tab_train_plan { get; set; }
        public DbSet<tab_limit> tab_limit { get; set; }
        public DbSet<tab_manning> tab_manning { get; set; }
        public DbSet<tab_train_attach> tab_train_attach { get; set; }
        //public DbSet<tab_map> tab_map { get; set; }
        public DbSet<tab_pen_allow> tab_pen_allow { get; set; }
        public DbSet<tab_pen_daily> tab_pen_daily { get; set; }
        public DbSet<tab_escalation> tab_escalation { get; set; }
        public DbSet<vw_user> vw_user { get; set; }
        public DbSet<tab_app_category> tab_app_category { get; set; }
        public DbSet<tab_app_definition> tab_app_definition { get; set; }
        public DbSet<tab_app_goals> tab_app_goals { get; set; }
        public DbSet<tab_appraisal> tab_appraisal { get; set; }
        public DbSet<tab_pay_default> tab_pay_default { get; set; }
        public DbSet<tab_transexpense> tab_transexpense { get; set; }
        public DbSet<tab_transexpense_details> tab_transexpense_details { get; set; }
        public DbSet<tab_doctrans> tab_doctrans { get; set; }
        public DbSet<tab_docpost> tab_docpost { get; set; }
        public DbSet<vw_staff_mast> vw_staff_mast { get; set; }
        public DbSet<tab_staff_skill> tab_staff_skill { get; set; }
        public DbSet<tab_app_actbud> tab_app_actbud { get; set; }
        public DbSet<tab_app_actdet> tab_app_actdet { get; set; }
        public DbSet<tab_appraisal_default> tab_appraisal_default { get; set; }
        public DbSet<tab_staff_bal> tab_staff_bal { get; set; }
        public DbSet<tab_hreason> tab_hreason { get; set; }
        public DbSet<tab_docpara> tab_docpara { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tab_trans_stat>().Property(x => x.percentage).HasPrecision(8, 4);
            modelBuilder.Entity<tab_trans_allow>().Property(x => x.allow_percentage).HasPrecision(8, 4);
            modelBuilder.Entity<tab_trans_allow>().Property(x => x.tax_percentage).HasPrecision(8, 4);
            modelBuilder.Entity<tab_trans_loan>().Property(x => x.interest_rate_trans).HasPrecision(6, 3);
            modelBuilder.Entity<tab_daily>().Property(x => x.rate_amount ).HasPrecision(8, 4);
            modelBuilder.Entity<tab_daily>().Property(x => x.max_percentage).HasPrecision(8, 4);
            modelBuilder.Entity<tab_tax>().Property(x => x.default_percentage).HasPrecision(8, 4);
            modelBuilder.Entity<tab_tax>().Property(x => x.min_percentage).HasPrecision(8, 4);
            modelBuilder.Entity<tab_over>().Property(x => x.weekday_rate).HasPrecision(8, 4);
            modelBuilder.Entity<tab_over>().Property(x => x.weekend_rate).HasPrecision(8, 4);
            modelBuilder.Entity<tab_over>().Property(x => x.max_percentage).HasPrecision(8, 4);
            modelBuilder.Entity<tab_over>().Property(x => x.weekday_rate).HasPrecision(8, 4);
            modelBuilder.Entity<tab_over>().Property(x => x.weekday_rate).HasPrecision(8, 4);
            modelBuilder.Entity<tab_defined>().Property(x => x.amount1).HasPrecision(18, 8);
            
        }

        private static string connstring()
        {
            Clogin utils = new Clogin();
            //return utils.company_database("99999").ToString();
            return utils.mydataconnect().ToString();

        }
    }
}

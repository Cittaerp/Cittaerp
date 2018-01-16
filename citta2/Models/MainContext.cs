using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using anchor1.utilities;

namespace CittaErp.Models
{
    public class MainContext : DbContext
    {

        public MainContext()
       : base(connstring())
        {

            // Get the ObjectContext related to this DbContext
            //var objectContext = (this as IObjectContextAdapter).ObjectContext;
            string timeouts = ConfigurationManager.AppSettings["ctime"];

            // Sets the command timeout for all the commands
            this.Database.CommandTimeout = Convert.ToInt16(timeouts);
        }

        public DbSet<GB_001_PCT> GB_001_PCT { get; set; }
        public DbSet<GB_001_PST> GB_001_PST { get; set; }
        public DbSet<AP_001_PTERM> AP_001_PTERM { get; set; }
        public DbSet<AR_001_CTERM> AR_001_CTERM { get; set; }
        public DbSet<AR_002_QUOTE> AR_002_QUOTE { get; set; }
        public DbSet<AR_002_SALES> AR_002_SALES { get; set; }
        public DbSet<DC_001_DISTS> DC_001_DISTS { get; set; }
        public DbSet<MC_001_EXCRT> MC_001_EXCRT { get; set; }
        public DbSet<WF_001_WKFL> WF_001_WKFL { get; set; }
        public DbSet<IV_001_ITEM> IV_001_ITEM { get; set; }
        public DbSet<AP_001_PUROT> AP_001_PUROT { get; set; }
        public DbSet<WF_001_APRDG> WF_001_APRDG { get; set; }
        public DbSet<GL_001_CATEG> GL_001_CATEG { get; set; }
        public DbSet<GL_001_CHART> GL_001_CHART { get; set; }
        public DbSet<AR_001_DADRS> AR_001_DADRS { get; set; }
        public DbSet<GL_001_ATYPE> GL_001_ATYPE { get; set; }
        public DbSet<MC_001_CUREN> MC_001_CUREN { get; set; }
        public DbSet<GB_001_HANAL> GB_001_HANAL { get; set; }
        public DbSet<GB_001_DANAL> GB_001_DANAL { get; set; }
        public DbSet<AR_001_CUSTM> AR_001_CUSTM { get; set; }
        public DbSet<AP_001_VENDR> AP_001_VENDR { get; set; }
        public DbSet<GB_001_INTP> GB_001_INTP { get; set; }
        public DbSet<GB_001_RSONC> GB_001_RSONC { get; set; }
        public DbSet<GL_001_JONT> GL_001_JONT { get; set; }
        public DbSet<GB_001_TAX> GB_001_TAX { get; set; }
        public DbSet<IC_001_INCOY> IC_001_INCOY { get; set; }
        public DbSet<FA_001_ASSET> FA_001_ASSET { get; set; }
        public DbSet<BK_001_BANK> BK_001_BANK { get; set; }
        public DbSet<AR_001_MTRIX> AR_001_MTRIX { get; set; }
        public DbSet<GB_001_EMP> GB_001_EMP { get; set; }
        public DbSet<FA_001_ASSETM> FA_001_ASSETM { get; set; }
        public DbSet<AR_001_STRAN> AR_001_STRAN { get; set; }
        public DbSet<AP_001_PTRAN> AP_001_PTRAN { get; set; }
        public DbSet<GB_001_DOC> GB_001_DOC { get; set; }
        public DbSet<DC_001_DISC> DC_001_DISC { get; set; }
        public DbSet<IV_001_WAREH> IV_001_WAREH { get; set; }
        public DbSet<GB_001_HEADER> GB_001_HEADER { get; set; }
        public DbSet<GB_999_MSG> GB_999_MSG { get; set; }
        public DbSet<GB_001_PCODE> GB_001_PCODE { get; set; }
        public DbSet<GB_001_COY> GB_001_COY { get; set; }
        public DbSet<AP_002_VTRAN> AP_002_VTRAN { get; set; }
        public DbSet<AP_002_VTRAD> AP_002_VTRAD { get; set; }
        public DbSet<GL_002_JONAL> GL_002_JONAL { get; set; }
        public DbSet<GL_002_JONAD> GL_002_JONAD { get; set; }
        public DbSet<AP_002_ADOEN> AP_002_ADOEN { get; set; }
        public DbSet<BK_002_BANKR> BK_002_BANKR { get; set; }
        public DbSet<BU_002_BUGTH> BU_002_BUGTH { get; set; }
        public DbSet<BU_002_BUGTD> BU_002_BUGTD { get; set; }
        public DbSet<IV_002_INVTY> IV_002_INVTY { get; set; }
        public DbSet<IV_002_IVTYD> IV_002_IVTYD { get; set; }
        public DbSet<IV_002_COUNT> IV_002_COUNT { get; set; }
        public DbSet<IV_002_CONTD> IV_002_CONTD { get; set; }
        public DbSet<MC_002_REVAL> MC_002_REVAL { get; set; }
        public DbSet<IV_001_ITMST> IV_001_ITMST { get; set; }
        public DbSet<AR_002_SODT> AR_002_SODT { get; set; }
        public DbSet<GL_001_GLDS> GL_001_GLDS { get; set; }
        public DbSet<AR_002_INVCE> AR_002_INVCE { get; set; }
        public DbSet<GB_001_LOGIN> GB_001_LOGIN { get; set; }
        public DbSet<BU_002_BUGTR> BU_002_BUGTR { get; set; }
        public DbSet<GL_002_JONAR> GL_002_JONAR { get; set; }
        public DbSet<GB_001_ITMG> GB_001_ITMG { get; set; }
        public DbSet<GB_001_ITMUS> GB_001_ITMUS { get; set; }
        public DbSet<FA_001_INSUR> FA_001_INSUR { get; set; }
        public DbSet<IV_002_PCNT> IV_002_PCNT { get; set; }
        public DbSet<IV_001_PC> IV_001_PC { get; set; }
        public DbSet<AR_001_PMTRX> AR_001_PMTRX { get; set; }
        public DbSet<IV_002_PAY> IV_002_PAY { get; set; }
        public DbSet<AP_002_PTRN> AP_002_PTRN { get; set; }
        public DbSet<AP_002_PTRND> AP_002_PTRND { get; set; }
        public DbSet<IV_002_OST> IV_002_OST { get; set; }
        public DbSet<WO_002_WKO> WO_002_WKO { get; set; }
        public DbSet<JC_002_JCD> JC_002_JCD { get; set; }
        public DbSet<JC_002_JDCDET> JC_002_JDCDET { get; set; }
        public DbSet<WO_002_WKODT> WO_002_WKODT { get; set; }
        public DbSet<AP_002_PURHEAD> AP_002_PURHEAD { get; set; }
        public DbSet<AG_002_ASSET> AG_002_ASSET { get; set; }
        public DbSet<WT_001_WKT> WT_001_WKT { get; set; }
        public DbSet<WC_001_WKC> WC_001_WKC { get; set; }
        public DbSet<TC_001_TCL> TC_001_TCL { get; set; }
        public DbSet<AG_001_AMG> AG_001_AMG { get; set; }
        public DbSet<SC_001_SCM> SC_001_SCM { get; set; }
        public DbSet<TK_001_TAC> TK_001_TAC { get; set; }
        public DbSet<AG_001_ASG> AG_001_ASG { get; set; }
        public DbSet<AA_002_AAM>AA_002_AAM { get; set; }
        public DbSet<JB_001_JOB>JB_001_JOB { get; set; }
        public DbSet<MN_002_MNT> MN_002_MNT { get; set; }
        //public DbSet<GB_001_ORG> GB_001_ORG { get; set; }
        //public DbSet<tab_calc> tab_calc { get; set; }
        //public DbSet<tab_array> tab_array { get; set; }
        //public DbSet<tab_soft> tab_soft { get; set; }
        //public DbSet<tab_document> tab_document { get; set; }
        //public DbSet<vw_user> vw_user { get; set; }
        //public DbSet<tab_staff> tab_staff { get; set; }
        //public DbSet<tab_bank> tab_bank { get; set; }

        //public DbSet<tab_type> tab_type { get; set; }
        //public DbSet<tab_self_approval> tab_self_approval { get; set; }
        //public DbSet<tab_docpara> tab_docpara { get; set; }

        public DbSet<GB_001_PTYS> GB_001_PTYS { get; set; }

        public DbSet<GB_001_PHEAD> GB_001_PHEAD { get; set; }
        public DbSet<GB_001_MAP> GB_001_MAP { get; set; }
        //public DbSet<tab_temp_rep> tab_temp_rep { get; set; }
        public DbSet<AP_002_POD> AP_002_POD { get; set; }
        public DbSet<AP_002_GRN> AP_002_GRN { get; set; }
        public DbSet<AP_002_IBKNG> AP_002_IBKNG { get; set; }
        public DbSet<AP_002_RQUSN> AP_002_RQUSN { get; set; }
        public DbSet<AP_002_QUOTE> AP_002_QUOTE { get; set; }
        public DbSet<AP_002_RSQNH> AP_002_RSQNH { get; set; }

        ////anchor1 module
        //public DbSet<tab_bank> tab_bank { get; set; }
        //public DbSet<tab_allow> tab_allow { get; set; }
        //public DbSet<tab_analy> tab_analy { get; set; }
        //public DbSet<tab_calc> tab_calc { get; set; }
        //public DbSet<tab_loan> tab_loan { get; set; }
        //public DbSet<tab_soft> tab_soft { get; set; }
        //public DbSet<tab_analy_code> tab_analy_code { get; set; }
        //public DbSet<tab_grade> tab_grade { get; set; }
        //public DbSet<tab_over> tab_over { get; set; }
        //public DbSet<tab_tax> tab_tax { get; set; }
        //public DbSet<tab_category_advice> tab_category_advice { get; set; }
        //public DbSet<tab_array> tab_array { get; set; }
        //public DbSet<tab_calend> tab_calend { get; set; }
        //public DbSet<tab_train> tab_train { get; set; }
        //public DbSet<tab_daily> tab_daily { get; set; }
        //public DbSet<tab_temp_rep> tab_temp_rep { get; set; }
        //public DbSet<tab_default> tab_default { get; set; }
        //public DbSet<tab_staff> tab_staff { get; set; }
        //public DbSet<tab_pen_pfa> tab_pen_pfa { get; set; }
        //public DbSet<tab_job_desc> tab_job_desc { get; set; }
        //public DbSet<tab_trans_input> tab_trans_input { get; set; }
        //public DbSet<tab_trans_over> tab_trans_over { get; set; }
        //public DbSet<tab_trans_loan> tab_trans_loan { get; set; }
        //public DbSet<tab_mast_loan> tab_mast_loan { get; set; }
        //public DbSet<tab_mast_status> tab_mast_status { get; set; }
        //public DbSet<tab_trans_allow> tab_trans_allow { get; set; }
        //public DbSet<tab_trans_daily> tab_trans_daily { get; set; }
        //public DbSet<tab_trans_prom> tab_trans_prom { get; set; }
        //public DbSet<tab_trans_stat> tab_trans_stat { get; set; }
        //public DbSet<tab_document> tab_document { get; set; }
        //public DbSet<tab_advice> tab_advice { get; set; }
        //public DbSet<tab_staff_person> tab_staff_person { get; set; }
        //public DbSet<tab_photo_coy> tab_photo_coy { get; set; }
        //public DbSet<tab_attach> tab_attach { get; set; }
        //public DbSet<tab_mast_allow> tab_mast_allow { get; set; }
        //public DbSet<tab_self_approval> tab_self_approval { get; set; }
        //public DbSet<tab_type> tab_type { get; set; }
        //public DbSet<tab_type2> tab_type2 { get; set; }
        //public DbSet<tab_defined> tab_defined { get; set; }
        //public DbSet<tab_layout> tab_layout { get; set; }
        //public DbSet<tab_account> tab_account { get; set; }
        //public DbSet<tab_train_default> tab_train_default { get; set; }
        //public DbSet<tab_photo> tab_photo { get; set; }
        //public DbSet<tab_holiday> tab_holiday { get; set; }
        //public DbSet<tab_delegate> tab_delegate { get; set; }
        //public DbSet<tab_transaction> tab_transaction { get; set; }
        //public DbSet<tab_depend> tab_depend { get; set; }
        //public DbSet<tab_train_group> tab_train_group { get; set; }
        //public DbSet<tab_train_plan> tab_train_plan { get; set; }
        //public DbSet<tab_limit> tab_limit { get; set; }
        //public DbSet<tab_manning> tab_manning { get; set; }
        //public DbSet<tab_train_attach> tab_train_attach { get; set; }
        ////public DbSet<tab_map> tab_map { get; set; }
        //public DbSet<tab_pen_allow> tab_pen_allow { get; set; }
        //public DbSet<tab_pen_daily> tab_pen_daily { get; set; }
        //public DbSet<tab_escalation> tab_escalation { get; set; }
        //public DbSet<vw_user> vw_user { get; set; }
        //public DbSet<tab_app_category> tab_app_category { get; set; }
        //public DbSet<tab_app_definition> tab_app_definition { get; set; }
        //public DbSet<tab_app_goals> tab_app_goals { get; set; }
        //public DbSet<tab_appraisal> tab_appraisal { get; set; }
        //public DbSet<tab_pay_default> tab_pay_default { get; set; }
        //public DbSet<tab_transexpense> tab_transexpense { get; set; }
        //public DbSet<tab_transexpense_details> tab_transexpense_details { get; set; }
        //public DbSet<tab_doctrans> tab_doctrans { get; set; }
        //public DbSet<tab_docpost> tab_docpost { get; set; }
        //public DbSet<vw_staff_mast> vw_staff_mast { get; set; }
        //public DbSet<tab_staff_skill> tab_staff_skill { get; set; }
        //public DbSet<tab_app_actbud> tab_app_actbud { get; set; }
        //public DbSet<tab_app_actdet> tab_app_actdet { get; set; }
        //public DbSet<tab_appraisal_default> tab_appraisal_default { get; set; }
        //public DbSet<tab_staff_bal> tab_staff_bal { get; set; }
        //public DbSet<tab_hreason> tab_hreason { get; set; }
        //public DbSet<tab_docpara> tab_docpara { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MC_001_EXCRT>().Property(x => x.exchange_rate).HasPrecision(18, 7);
            modelBuilder.Entity<AR_002_SALES>().Property(x => x.exchange_rate).HasPrecision(18, 7);


            //modelBuilder.Entity<tab_trans_stat>().Property(x => x.percentage).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_trans_allow>().Property(x => x.allow_percentage).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_trans_allow>().Property(x => x.tax_percentage).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_trans_loan>().Property(x => x.interest_rate_trans).HasPrecision(6, 3);
            //modelBuilder.Entity<tab_daily>().Property(x => x.rate_amount).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_daily>().Property(x => x.max_percentage).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_tax>().Property(x => x.default_percentage).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_tax>().Property(x => x.min_percentage).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_over>().Property(x => x.weekday_rate).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_over>().Property(x => x.weekend_rate).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_over>().Property(x => x.max_percentage).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_over>().Property(x => x.weekday_rate).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_over>().Property(x => x.weekday_rate).HasPrecision(8, 4);
            //modelBuilder.Entity<tab_defined>().Property(x => x.amount1).HasPrecision(18, 8);

        }

        private static string connstring()
        {
            Clogin cqx = new Clogin();
            return cqx.mydataconnect().ToString();

        }

    }


}
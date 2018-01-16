using CittaErp.Models;
using anchor1.Filters;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CittaErp.Controllers
{
    public class Due_maintenanceController : Controller
    {
        FA_001_ASSET AM_001_AMS = new FA_001_ASSET();
        WO_002_WKO WO_002_WKO = new WO_002_WKO();
        WO_002_WKODT WO_002_WKODT = new WO_002_WKODT();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        bool err_flag = true;
        string[] container = new string[] { };
        List<object> list = new List<object>();
        string action_flag = "";
        // GET: Due_maintenance

        [EncryptionActionAttribute]
        public ActionResult Index()
        {

            util.init_values();
            pubsess = (pubsess)Session["pubsess"];

            string query = "select  c1 vwstring0, c6 vwint1, c2 vwstring1, name1_msg vwstring6,";
            query += " case when c4 = 'D' then SUBSTRING(c3, 1, 4) +'/'+ SUBSTRING(c3, 5, 2)+'/'+ ";
            query += " SUBSTRING(c3, 7, 2) else c3 end vwstring3,";
            query += " c5 vwint2 from vw_maindue left outer join GB_999_MSG gb on gb.code_msg = c4 where gb.type_msg = 'calf'";
            var bglist = db.Database.SqlQuery<vw_genlay>(query).ToList();
            return View(bglist);
}

        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "";
            var bgme = (from bg in db.FA_001_ASSET
                        where bg.fixed_asset_code == id
                        select bg.fixed_asset_code).FirstOrDefault();
            if (bgme != null)
            {
                sqlstr = "UPDATE [dbo].[FA_001_ASSET] SET asset_requires_maintenace = 'N' WHERE fixed_asset_code =" + util.sqlquote(id);
            }
            else
            {
                sqlstr = "UPDATE [dbo].[AG_001_ASG] SET asset_requires_maintenace = 'N' WHERE asset_grouping_id =" + util.sqlquote(id);
            }
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        [EncryptionActionAttribute]
        public ActionResult Convert(string key1, string key2)
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            var bgrunlist = (from bg in db.FA_001_ASSET
                             join bh in db.AG_001_AMG
                             on new { a1 = bg.group_type_id } equals new { a1 = bh.maintenance_group_type_id }
                             into bh1
                             from bh2 in bh1.DefaultIfEmpty()
                             join bk in db.GB_999_MSG
                             on new { a1 = bh2.nature, a2 = "nat" } equals new { a1 = bk.code_msg, a2 = bk.type_msg }
                             into bk1
                             from bk2 in bk1.DefaultIfEmpty()
                             where bg.fixed_asset_code == key1
                             select new { bg, bh2, bk2 }).FirstOrDefault();
            if (bgrunlist.bh2 != null)
            {
                WO_002_WKO.estimated_total_cost = bgrunlist.bh2.estimated_total;
                WO_002_WKO.maintenance_id = bgrunlist.bg.group_type_id;
                WO_002_WKO.gl_account = bgrunlist.bh2.gl_account;
                WO_002_WKO.asset_or_group = bgrunlist.bg.fixed_asset_code;
                WO_002_WKO.total_materials_cost = bgrunlist.bh2.material_total;
                WO_002_WKO.total_hr_cost = bgrunlist.bh2.hr_total;
                WO_002_WKO.total_contract_amount = bgrunlist.bh2.subcontract_total;

            }
            WO_002_WKO = new WO_002_WKO();
            WO_002_WKO.created_by = pubsess.userid;
            WO_002_WKO.created_date = DateTime.UtcNow;
            var duplicate =
                 from bg in db.WO_002_WKO
                 select bg;
            var count = duplicate.Count();
            WO_002_WKO.work_order_id = "wk" + count + 1;
            WO_002_WKO.work_order_description = "Due Maintenance-"+key2;
            WO_002_WKO.status = "A";
            WO_002_WKO.job_card_id = "";
            WO_002_WKO.estimated_start_date_time = "";
            WO_002_WKO.estimated_end_date_time = "";
            WO_002_WKO.maintenance_id = "";
            WO_002_WKO.activation_id = "";
            WO_002_WKO.estimated_total_cost = 0;
            WO_002_WKO.maintenance_id = "";
            WO_002_WKO.gl_account = "";
            WO_002_WKO.asset_or_group = key1;
            WO_002_WKO.total_materials_cost = 0;
            WO_002_WKO.total_hr_cost = 0;
            WO_002_WKO.total_contract_amount = 0;
            WO_002_WKO.modified_date = DateTime.UtcNow;
            WO_002_WKO.work_center_id = "";
            WO_002_WKO.work_order_date = "";
            WO_002_WKO.team_lead = "";
            WO_002_WKO.modified_by = pubsess.userid;
            WO_002_WKO.approval_level = 0;
            WO_002_WKO.approval_date = DateTime.UtcNow;
            WO_002_WKO.approval_by = "";
            WO_002_WKO.cvt_to_wrk_ord = "";
            WO_002_WKO.flag = "D";
            WO_002_WKO.note = "";
            db.Entry(WO_002_WKO).State = EntityState.Added;
            try
            {
                db.SaveChanges();
            }

            catch (Exception err)
            {
                if (err.InnerException == null)
                    ModelState.AddModelError(String.Empty, err.Message);
                else
                    ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);

                err_flag = false;
            }
            return RedirectToAction("Index", "WorkOrder");
        }

    }
}
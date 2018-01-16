using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CittaErp.Models;
using anchor1.Filters;
using System.Data.Entity;
using CittaErp.utilities;

namespace CittaErp.Controllers
{
    public class Asset_activity_monitorController : Controller
    {
        // GET: Asset_activity_monitor

        AA_002_AAM AA_002_AAM = new AA_002_AAM();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;
        bool err_flag = true;
        string action_flag = "";

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            var bglist = from bh in db.AA_002_AAM
                         join bg in db.FA_001_ASSET
                         on new { a1 = bh.fixed_assest_id } equals new { a1 = bg.fixed_asset_code }
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join bi in db.GB_999_MSG
                         on new {a1 = bg2.unit_of_reading, a2 = "calf"} equals new {a1= bi.code_msg, a2 = bi.type_msg}
                         into bi1 
                         from bi2 in bi1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.fixed_assest_id,
                             vwint0 = bh.cumulative_activity_level_figure,
                             vwstring1 = bg2.description,
                             vwstring2 =bi2.name1_msg, 
                             vwstring3 = bh.Tran_date
                         };

            return View(bglist.ToList());
        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            select_query();
            return View(glay);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            update_file();
            if (err_flag)
                return RedirectToAction("Create");
            select_query();
            return View(glay);
        }
        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            AA_002_AAM = db.AA_002_AAM.Find(key1);
            if (AA_002_AAM != null)
                read_record();
            select_query();
            return View(glay);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt == "D")
            {
                delete_record();
                return RedirectToAction("Index");
            }

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AA_002_AAM] where fixed_assest_id ='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            AA_002_AAM = db.AA_002_AAM.Find(glay.vwstring0);
            if (AA_002_AAM != null)
            {
                db.AA_002_AAM.Remove(AA_002_AAM);
                db.SaveChanges();
            }
        }
        private void update_file()
        {
            err_flag = true;
            validation_routine();

            if (err_flag)
                update_record();

        }

        private void validation_routine()
        {
            string error_msg = "";


            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please Select a Fixed Asset");
                err_flag = false;
            }
           if(action_flag == "Create")
            {
                AA_002_AAM bnk = db.AA_002_AAM.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicate Fixed Asset");
                    err_flag = false;
                }
            }
        }

        [HttpPost]
        public ActionResult asset(string id)
        {
            string measure = "" ;
            int mesval = 0;
            var bgassign3 = (from bg in db.FA_001_ASSET
                             join bh in db.GB_999_MSG
                             on new {a1 = bg.unit_of_reading, a2="calf"} equals new {a1 = bh.code_msg, a2 = bh.type_msg}
                             into bh1
                             from bh2 in bh1.DefaultIfEmpty()
                             where bg.fixed_asset_code == id
                             select new { bg, bh2 }).FirstOrDefault();
            if (bgassign3 != null) {
                measure = bgassign3.bg.unit_of_reading;
                mesval = Convert.ToInt32(bgassign3.bg.cumulative_amount);

            }

            List<SelectListItem> ary2 = new List<SelectListItem>();
            ary2.Add(new SelectListItem { Value = "mesdet", Text = measure });
            ary2.Add(new SelectListItem { Value = "cumm", Text = mesval.ToString() });
            var calf = from bg in db.GB_999_MSG
                       where bg.type_msg == "calf"
                       select bg;

            ViewBag.calf1 = new SelectList(calf.ToList(), "code_msg", "name1_msg", glay.vwstring2);
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary2.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");
        }
        public void select_query()
        {

            var fxd = from bg in db.FA_001_ASSET
                      select bg;
            ViewBag.fxd1 = new SelectList(fxd.ToList(), "fixed_asset_code", "description", glay.vwstring0);

            var mnt = from bd in db.AG_001_AMG
                      select bd;
            ViewBag.mnt1 = new SelectList(mnt.ToList(), "maintenance_group_type_id", "description", glay.vwstring1);

            var calf = from bg in db.GB_999_MSG
                       where bg.type_msg == "calf"
                       select bg;

            ViewBag.calf1 = new SelectList(calf.ToList(), "code_msg", "name1_msg", glay.vwstring2);
        }

        private void read_record()
        {

            glay.vwbool0 = false;
            glay.vwstring0 = AA_002_AAM.fixed_assest_id;
            glay.vwstring1 = AA_002_AAM.maintenance_type;
            glay.vwstring2 = util.date_slash(AA_002_AAM.Tran_date);
            glay.vwint0 = AA_002_AAM.cumulative_activity_level_figure;
            glay.vwstring3 = AA_002_AAM.cumulative_activity_level;
            glay.vwstring4 = AA_002_AAM.note;

            //if (AA_002_AAM.inactive_status == "Y")
            //    glay.vwbool0 = true;
        }
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwdtarray1 = new DateTime[10];
            glay.vwdclarray2 = new decimal[10];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
        }
        [HttpPost]

        private void update_record()
        {
           if(action_flag == "Create")
            {
                AA_002_AAM = new AA_002_AAM();
                AA_002_AAM.created_by = pubsess.userid;
                AA_002_AAM.created_date = DateTime.UtcNow;

            }
            else
            {
                AA_002_AAM = db.AA_002_AAM.Find(glay.vwstring0);

            }

            //string query = "UPDATE dbo.AG_001_ASG";
            //query += " SET cumulative_amount =" + glay.vwint0;
            //query += " WHERE fixed_assets_id =" + util.sqlquote(glay.vwstring0);
            //var sqlite = db.Database.ExecuteSqlCommand(query);

            string query1 = "UPDATE dbo.FA_001_ASSET";
            query1 += " SET uor ="+util.sqlquote(glay.vwstring3)+ ", cummulative_amt = " + glay.vwint0;
            query1 += " ,group_type_id = " + util.sqlquote(glay.vwstring1);
            query1 += " WHERE fixed_asset_code =" + util.sqlquote(glay.vwstring0);
            var sqlite1 = db.Database.ExecuteSqlCommand(query1);

            AA_002_AAM.fixed_assest_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            AA_002_AAM.maintenance_type = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            AA_002_AAM.Tran_date = util.date_ddmmyyyyy(glay.vwstring2);
            AA_002_AAM.cumulative_activity_level = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            AA_002_AAM.cumulative_activity_level_figure = glay.vwint0;
            AA_002_AAM.attach_document = "";
            AA_002_AAM.modified_date = DateTime.UtcNow;
            AA_002_AAM.modified_by = pubsess.userid;

            AA_002_AAM.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            // AA_002_AAM.inactive_status = glay.vwbool0 ? "Y" : "N";

           if(action_flag == "Create")
                db.Entry(AA_002_AAM).State = EntityState.Added;
            else
                db.Entry(AA_002_AAM).State = EntityState.Modified;
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
        }


    }
}
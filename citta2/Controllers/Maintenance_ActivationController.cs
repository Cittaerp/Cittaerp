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

    public class Maintenance_ActivationController : Controller
    {
        // GET: Maintenance_Activation

        MN_002_MNT MN_002_MNT = new MN_002_MNT();
        WO_002_WKO WO_002_WKO = new WO_002_WKO();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;
        bool err_flag = true;
        int flag;
        string key1;
        string action_flag = "";

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)HttpContext.Session["pubsess"];
           
            var bglist = from bh in db.MN_002_MNT
                         join bg in db.GB_001_EMP
                         on new { a1 = bh.reported_by } equals new { a1 = bg.employee_code }
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join bi in db.GB_999_MSG
                         on new { a1 = bh.activation_type } equals new { a1 = bi.code_msg }
                         into bi1
                         from bi2 in bi1.DefaultIfEmpty()
                         where bi2.type_msg == "act"
                         select new vw_genlay

                         {
                             vwint0 = bh.activation_id,
                             vwstring1 = bi2.name1_msg,
                             vwstring2 = bg2.name,
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
            {
                if (glay.vwstring6 == "N")
                {
                    ViewBag.successMessage = "Success";
                    return RedirectToAction("Create");
                }
                else
                {
                    glay.vwstring9 = Session["activation"].ToString();

                    return RedirectToAction("Index", "WorkOrder", new { key1 = glay.vwstring9, flag = 2, subcheck = "C" });
                }
            }
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(int key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            MN_002_MNT = db.MN_002_MNT.Find(key1);
            if (MN_002_MNT != null)
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
            photo1 = photofile;
            update_file();
            if (err_flag)
            {
                if (glay.vwstring6 == "N")
                {
                    ViewBag.successMessage = "Success";
                    return RedirectToAction("Create");
                }
                else
                {
                    glay.vwstring9 = Session["activation"].ToString();
                    //return RedirectToAction("Create", "WorkOrder", new { key1 = glay.vwstring9, flag = 2, subcheck = "C" });
                    return RedirectToAction("Index", "WorkOrder");
                }
            }
            select_query();
            return View(glay);
        }

        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[MN_002_MNT] where activation_id ='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        private void delete_record()
        {
            MN_002_MNT = db.MN_002_MNT.Find(glay.vwint0);
            if (MN_002_MNT != null)
            {
                db.MN_002_MNT.Remove(MN_002_MNT);
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


            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //{
            //    ModelState.AddModelError(String.Empty, "ID must not be spaces");
            //    err_flag = false;
            //}

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please insert description");
                err_flag = false;
            }



            if (glay.vwdate0.Equals("") || glay.vwdate0.Equals(null))
            {
                ModelState.AddModelError(String.Empty, "Please Select a Transaction Date");
                err_flag = false;
            }

        }
        public void select_query()
        {

            var emp = from bg in db.GB_001_EMP
                      select bg;
            ViewBag.emp1 = new SelectList(emp.ToList(), "employee_code", "name");

            var wkt = from bh in db.WC_001_WKC
                      select bh;
            ViewBag.wkt1 = new SelectList(wkt.ToList(), "work_center_id", "description", glay.vwstring5);

            var fxd = from bh in db.FA_001_ASSET
                      where bh.active_status == "N"
                      select bh;
            ViewBag.fxd = new SelectList(fxd.ToList(), "fixed_asset_code", "description", glay.vwstring3);

            var atype = from bh in db.GB_999_MSG
                        where bh.type_msg == "act"
                        select bh;
            ViewBag.atype1 = new SelectList(atype.ToList(), "code_msg", "name1_msg", glay.vwstring1);
        }
        private void read_record()
        {

            // glay.vwstring2 = "N";
            glay.vwbool0 = false;
            glay.vwint0 = MN_002_MNT.activation_id;
            glay.vwstring1 = MN_002_MNT.activation_type;
            glay.vwstring5 = MN_002_MNT.trans_date;
            //glay.vwstring3 =util.date_slash(MN_002_MNT.exp_maint_date);


            glay.vwstring4 = MN_002_MNT.reported_by;
            // glay.vwstring5 = MN_002_MNT.main_work_center;

            glay.vwstring6 = MN_002_MNT.convert_to_work_order;
            glay.vwstring7 = MN_002_MNT.explain_work;

            //if (MN_002_MNT.inactive_status == "Y")
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
        public ActionResult asset(string id)
        {

            var bgassign = (from bg in db.FA_001_ASSET
                            where bg.fixed_asset_code == id
                            select new { bg }).FirstOrDefault();
            string emp_id = bgassign.bg.asset_location;
            string emp_name = bgassign.bg.asset_user;

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "fixed", Text = emp_id });
            ary.Add(new SelectListItem { Value = "assett", Text = emp_name });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");

        }
        private void update_record()
        {
           if(action_flag == "Create")
            {
                MN_002_MNT = new MN_002_MNT();
                MN_002_MNT.created_by = pubsess.userid;
                MN_002_MNT.created_date = DateTime.UtcNow;

            }
            else
            {
                MN_002_MNT = db.MN_002_MNT.Find(glay.vwint0);
            }
            //MN_002_MNT.activation_id = 0;
            MN_002_MNT.activation_type = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            MN_002_MNT.trans_date = glay.vwstring5;
            MN_002_MNT.fixed_asset_code = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;

            MN_002_MNT.reported_by = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            // MN_002_MNT.main_work_center = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;


            MN_002_MNT.convert_to_work_order = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            MN_002_MNT.explain_work = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            MN_002_MNT.attach_document = "";

            //  MN_002_MNT.inactive_status = glay.vwbool0 ? "Y" : "N";
            MN_002_MNT.modified_date = DateTime.UtcNow;
            MN_002_MNT.modified_by = pubsess.userid;
            MN_002_MNT.approval_level = 0;
            MN_002_MNT.approval_by = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            MN_002_MNT.approval_date = DateTime.UtcNow;
            Session["activation"] = glay.vwint0;
           if(action_flag == "Create")
                db.Entry(MN_002_MNT).State = EntityState.Added;
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
            if(glay.vwstring6 == "Y")
            {
                WO_002_WKO = new WO_002_WKO();
                WO_002_WKO.created_by = pubsess.userid;
                WO_002_WKO.created_date = DateTime.UtcNow;
                var duplicate =
                from bg in db.WO_002_WKO
                select bg;
                var count = duplicate.Count();
                WO_002_WKO.work_order_id = "wk" + count + 1;
                var due = (from bg in db.FA_001_ASSET
                           where bg.fixed_asset_code == glay.vwstring3
                           select bg.description).FirstOrDefault();
                WO_002_WKO.work_order_description = "Repairs-"+due;
                WO_002_WKO.status = "A";
                WO_002_WKO.job_card_id = "";
                WO_002_WKO.estimated_start_date_time = "";
                WO_002_WKO.estimated_end_date_time = "";
                WO_002_WKO.maintenance_id = "";
                WO_002_WKO.activation_id = "";
                WO_002_WKO.estimated_total_cost = 0;
                WO_002_WKO.maintenance_id = "";
                WO_002_WKO.gl_account = "";
                WO_002_WKO.total_materials_cost = 0;
                WO_002_WKO.flag = "R";
                WO_002_WKO.total_hr_cost = 0;
                WO_002_WKO.total_contract_amount = 0;
                WO_002_WKO.modified_date = DateTime.UtcNow;
                WO_002_WKO.work_center_id = "";
                WO_002_WKO.work_order_date = "";
                WO_002_WKO.team_lead = "";
                WO_002_WKO.asset_or_group = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
                WO_002_WKO.modified_by = pubsess.userid;
                WO_002_WKO.approval_level = 0;
                WO_002_WKO.approval_date = DateTime.UtcNow;
                WO_002_WKO.approval_by = "";
                WO_002_WKO.cvt_to_wrk_ord = "";
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
            }
           
            if (err_flag)
            {
                string[] num = new string[1];

                util.write_document("MNT", MN_002_MNT.activation_id.ToString(), photo1, num);

            }
        }




    }
}
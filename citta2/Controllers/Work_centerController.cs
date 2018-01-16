using CittaErp.Models;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class Work_centerController : Controller
    {
        WC_001_WKC WC_001_WKC = new WC_001_WKC();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        bool err_flag = true;
        string action_flag = "";
        // GET: Work_center
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            var bglist = from bh in db.WC_001_WKC
                         join bg in db.GB_001_EMP
                         on new { a1 = bh.employee_code } equals new { a1 = bg.employee_code }
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.work_center_id,
                             vwstring1 = bh.description,
                             vwstring2 = bh.location,
                             vwstring3 = bk2.name,
                             vwstring4 = bk2.job_role.ToString(),
                             vwstring5 = bh.inactive_status == "N" ? "Active" : "Inactive"
                         };

            return View(bglist.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            select_query();
            return View(glay);
        }
        [HttpPost]
        public ActionResult Create(vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            initial_rtn();
            update_file();
            if (err_flag)
                return RedirectToAction("Create");
            select_query();
            return View(glay);
        }
        public ActionResult Edit(string key1)
        {
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
            psess = (psess)Session["psess"];
            
            pubsess = (pubsess)Session["pubsess"];
            WC_001_WKC = db.WC_001_WKC.Find(key1);
            if (WC_001_WKC != null)
                read_record();
            select_query();
            return View(glay);
        }
        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
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
            string sqlstr = "delete from [dbo].[WC_001_WKC] where work_center_id ='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            WC_001_WKC = db.WC_001_WKC.Find(glay.vwstring0);
            if (WC_001_WKC != null)
            {
                db.WC_001_WKC.Remove(WC_001_WKC);
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
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "ID must not be spaces");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring3))
            {
                ModelState.AddModelError(String.Empty, "Please select a Manager");
                err_flag = false;
            }



            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please Insert Description");
                err_flag = false;
            }




           if(action_flag == "Create")
            {
                FA_001_ASSET bnk = db.FA_001_ASSET.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }
        }
        private void select_query()
        {

            var bglist = from bg in db.GB_001_EMP
                         where bg.active_status == "N"
                         orderby bg.employee_code
                         select bg;

            ViewBag.employee = new SelectList(bglist.ToList(), "employee_code", "name", glay.vwstrarray0[3]);
        }
        private void read_record()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwdclarray0 = new decimal[10];
            glay.vwstrarray6 = new string[20];
            // glay.vwstring2 = "N";
            glay.vwbool0 = false;
            glay.vwstring0 = WC_001_WKC.work_center_id;
            glay.vwstring1 = WC_001_WKC.description;
            glay.vwstring2 = WC_001_WKC.location;
            glay.vwstring3 = WC_001_WKC.employee_code;
            glay.vwstring4 = WC_001_WKC.created_by;
            glay.vwstring5 = WC_001_WKC.created_date.ToString("dd/mm/yyyy");
            glay.vwstring6 = WC_001_WKC.modified_by;
            glay.vwstring7 = WC_001_WKC.note;
            glay.vwstring9 = WC_001_WKC.modified_date.ToString("dd/mm/yyyy");


            if (WC_001_WKC.inactive_status == "Y")
                glay.vwbool0 = true;


        }


        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwdtarray1 = new DateTime[10];
            glay.vwdclarray2 = new decimal[10];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            //glay.vwbool0 = true;
        }

        [HttpPost]
        public ActionResult assign(string id)
        {

            psess = (psess)Session["psess"];
            var bgassign = (from bg in db.GB_001_EMP
                            where bg.employee_code == id
                            select new { bg }).FirstOrDefault();
            string emp_id = bgassign.bg.employee_code;
            string emp_name = bgassign.bg.name;

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "employeeid", Text = emp_id });
            ary.Add(new SelectListItem { Value = "employee", Text = emp_name });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }
        private void update_record()
        {
           if(action_flag == "Create")
            {
                WC_001_WKC = new WC_001_WKC();
                WC_001_WKC.created_by = pubsess.userid;
                WC_001_WKC.created_date = DateTime.UtcNow;

            }
            else
            {
                WC_001_WKC = db.WC_001_WKC.Find(glay.vwstring0);
            }
            WC_001_WKC.work_center_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            WC_001_WKC.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            WC_001_WKC.location = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            WC_001_WKC.employee_code = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            WC_001_WKC.inactive_status = glay.vwbool0 ? "Y" : "N";
            WC_001_WKC.modified_date = DateTime.UtcNow;
            WC_001_WKC.modified_by = pubsess.userid;
            WC_001_WKC.note = "";
           if(action_flag == "Create")
                db.Entry(WC_001_WKC).State = EntityState.Added;
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
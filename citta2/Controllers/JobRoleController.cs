
using CittaErp.Models;
using CittaErp.utilities;
using anchor1.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CittaErp.Controllers
{
    public class JobRoleController : Controller
    {
        JB_001_JOB JB_001_JOB = new JB_001_JOB();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        bool err_flag = true;
        string location, asset_user = "";
        string[] container = new string[] { };
        string action_flag = "";
        // GET: JobRole
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            var bglist = from bh in db.JB_001_JOB
                         join bg in db.GB_999_MSG
                         on new { a1 = bh.costing_basis, a2 = "cost" } equals new { a1 = bg.code_msg, a2 = bg.type_msg}
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwint0 = bh.job_id,
                             vwstring1 = bh.job_title,
                             vwstring2 = bk2.name1_msg,
                             vwdecimal0 = bh.cost,
                             vwstring4 = bh.inactive_status == "N" ? "Active" : "Inactive"
                         };

            return View(bglist.ToList());
        }

        [EncryptionActionAttribute]
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            update_file();
            if (err_flag)
                return RedirectToAction("Create");
            initial_rtn();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(int key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            JB_001_JOB = db.JB_001_JOB.Find(key1);
            if (JB_001_JOB != null)
                read_record();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
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

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[JB_001_JOB] where job_id='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            JB_001_JOB = db.JB_001_JOB.Find(glay.vwstring0);
            if (JB_001_JOB != null)
            {
                db.JB_001_JOB.Remove(JB_001_JOB);
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
        private void update_record()
        {
           if(action_flag == "Create")
            {
                JB_001_JOB = new JB_001_JOB();
                JB_001_JOB.created_by = pubsess.userid;
                JB_001_JOB.created_date = DateTime.UtcNow;

            }
            else
            {
                JB_001_JOB = db.JB_001_JOB.Find(glay.vwint0);
            }
            //JB_001_JOB.work_center_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            JB_001_JOB.job_title = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            JB_001_JOB.costing_basis = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            JB_001_JOB.cost = glay.vwdecimal0;
            JB_001_JOB.inactive_status = glay.vwbool0 ? "Y" : "N";
            JB_001_JOB.modified_date = DateTime.UtcNow;
            JB_001_JOB.modified_by = pubsess.userid;
            JB_001_JOB.note = "";
            JB_001_JOB.attach_document = "";
           if(action_flag == "Create")
                db.Entry(JB_001_JOB).State = EntityState.Added;
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
        private void select_query()
        {

            var bglist = from bg in db.GB_999_MSG
                         where bg.type_msg == "cost"
                         select bg;

            ViewBag.timing = new SelectList(bglist.ToList(), "code_msg", "name1_msg", glay.vwstring2);
        }

        private void validation_routine()
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please Insert a Job Title");
                err_flag = false;
            }

           if(action_flag == "Create")
            {

                var bgassign = (from bg in db.JB_001_JOB
                                select bg.job_title).ToList();
               
                foreach (var item in bgassign)
                {
                    if (glay.vwstring1 == item)
                    {
                        ModelState.AddModelError(String.Empty, "Can not accept dupicate Job Title");
                        err_flag = false;
                        break;
                    }
                }
            }
        }
        private void read_record()
        {
            glay.vwbool0 = false;
            glay.vwint0 = JB_001_JOB.job_id;
            glay.vwstring1 = JB_001_JOB.job_title;
            glay.vwstring2 = JB_001_JOB.costing_basis;
            glay.vwdecimal0 = JB_001_JOB.cost;
            glay.vwstring3 = JB_001_JOB.created_by;
            glay.vwstring4 = JB_001_JOB.created_date.ToString("dd/mm/yyyy");
            glay.vwstring5 = JB_001_JOB.modified_by;
            glay.vwstring6 = JB_001_JOB.note;
            if (JB_001_JOB.inactive_status == "Y")
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
            //glay.vwstring2 = "N";
            //glay.vwbool0 = true;
        }

    }
}
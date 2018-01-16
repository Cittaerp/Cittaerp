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
    
    public class Technical_comController : Controller
    {
        TC_001_TCL TC_001_TCL = new TC_001_TCL();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        bool err_flag = true;
        string action_flag = "";
        // GET: Technical_com
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)HttpContext.Session["pubsess"];
            var bglist = from bh in db.TC_001_TCL
                         select new vw_genlay
                         {
                             vwstring0 = bh.technical_competency_level_id,
                             vwstring1 = bh.description,
                             vwstring2 = bh.required_competence,
                             vwstring3 = bh.inactive_status == "N" ? "Active" : "Inactive"
                         };

            return View(bglist.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            select_query();
            return View(glay);
        }

        private void select_query()
        {
        }

        private void initial_rtn()
        {
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
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
                TC_001_TCL = new TC_001_TCL();
                TC_001_TCL.created_by = pubsess.userid;
                TC_001_TCL.created_date = DateTime.UtcNow;

            }
            else
            {
                TC_001_TCL = db.TC_001_TCL.Find(glay.vwstring0);
            }
            //JB_001_JOB.work_center_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            TC_001_TCL.technical_competency_level_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            TC_001_TCL.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            TC_001_TCL.required_competence = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            TC_001_TCL.inactive_status = glay.vwbool0 ? "Y" : "N";
            TC_001_TCL.modified_date = DateTime.UtcNow;
            TC_001_TCL.modified_by = pubsess.userid;
            TC_001_TCL.note = "";
            TC_001_TCL.comments = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3; 
           if(action_flag == "Create")
                db.Entry(TC_001_TCL).State = EntityState.Added;
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

        private void validation_routine()
        {
            string error_msg = "";

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please Insert a Description");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                TC_001_TCL asst = db.TC_001_TCL.Find(glay.vwstring0);
                if (asst != null)
                {
                    ModelState.AddModelError(String.Empty, "Technical competency ID Exists");
                    err_flag = false;
                }

                var bgassign = (from bg in db.TC_001_TCL
                                select bg.description).ToList();

                foreach (var item in bgassign)
                {
                    if (glay.vwstring1 == item)
                    {
                        ModelState.AddModelError(String.Empty, "Can not accept dupicate Competency level ");
                        err_flag = false;
                        break;
                    }
                }
            }
        }

        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            TC_001_TCL = db.TC_001_TCL.Find(key1);
            if (TC_001_TCL != null)
                read_record();
            select_query();
            return View(glay);
        }

        private void read_record()
        {
            glay.vwbool0 = false;
            glay.vwstring0 = TC_001_TCL.technical_competency_level_id;
            glay.vwstring1 = TC_001_TCL.description;
            glay.vwstring2 = TC_001_TCL.required_competence;
            glay.vwstring3 = TC_001_TCL.comments;
            glay.vwstring4 = TC_001_TCL.created_by;
            glay.vwstring5 = TC_001_TCL.created_date.ToString("dd/mm/yyyy");
            glay.vwstring6 = TC_001_TCL.modified_by;
            glay.vwstring7 = TC_001_TCL.note;
            if (TC_001_TCL.inactive_status == "Y")
                glay.vwbool0 = true;
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
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

        private void delete_record()
        {

            TC_001_TCL = db.TC_001_TCL.Find(glay.vwstring0);
            if (TC_001_TCL != null)
            {
                db.TC_001_TCL.Remove(TC_001_TCL);
                db.SaveChanges();
            }
        }
    }
}
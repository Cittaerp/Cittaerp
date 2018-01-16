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
    public class TagcyController : Controller
    {
        AR_001_CTERM AR_001_CTERM = new AR_001_CTERM();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();

        bool err_flag = true;
        string delmsg = "";
        string action_flag = "";
        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.AR_001_CTERM
                         select new vw_genlay
                         {
                             vwstring0 = bh.credit_term_code,
                             vwstring1 = bh.description,
                             vwint0 = bh.num_of_days,
                             vwstring3 = bh.active_status == "N" ? "Active" : "Inactive"
                         };


            return View(bglist.ToList());


        }
   
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            return View(glay);
        }

        [HttpPost]
        public ActionResult Create (vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            return View(glay);
        }

        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            AR_001_CTERM = db.AR_001_CTERM.Find(key1);
            if (AR_001_CTERM != null)
                read_record();

            return View(glay);
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt=="D")
            { //delete record
                delete_record();
                return RedirectToAction("Index");
            }

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            return View(glay);
        }
        private void delete_record()
        {
            if (util.delete_check("CTERM", glay.vwstring0))
            {
                AR_001_CTERM = db.AR_001_CTERM.Find(glay.vwstring0);
                db.AR_001_CTERM.Remove(AR_001_CTERM);
                db.SaveChanges();
            }
            else
            {
                delmsg = "Credit Term in Use";
                ModelState.AddModelError(String.Empty, delmsg);
                err_flag = false;

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
            if (action_flag=="Create")
            {
                AR_001_CTERM = new AR_001_CTERM();
                AR_001_CTERM.created_by = pubsess.userid;
                AR_001_CTERM.created_date = DateTime.UtcNow;
                AR_001_CTERM.delete_flag = "N";
            }
            else
            {
                AR_001_CTERM = db.AR_001_CTERM.Find(glay.vwstring0);
            }

            AR_001_CTERM.credit_term_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            AR_001_CTERM.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            AR_001_CTERM.num_of_days = glay.vwint0;
            AR_001_CTERM.modified_date = DateTime.UtcNow;
            AR_001_CTERM.modified_by = pubsess.userid;
            AR_001_CTERM.note = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            AR_001_CTERM.active_status = glay.vwbool0 ? "Y" : "N";


           if(action_flag == "Create")
                db.Entry(AR_001_CTERM).State = EntityState.Added;
            else
                db.Entry(AR_001_CTERM).State = EntityState.Modified;
           
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
           // string error_msg = "";

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Description  must not be spaces");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                string sqlstr = "select '1' query0 from AR_001_CTERM where description=" + util.sqlquote(glay.vwstring1);
                var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
                if (bglist1 != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicate Description");
                    err_flag = false;
                }
            }
            if (glay.vwint0 < 0)
            {
                ModelState.AddModelError(String.Empty, "Number of days is not valid, please enter a valid number");
                err_flag = false;

            }
            //if (error_msg !="")
            //{
            //    ModelState.AddModelError(String.Empty, error_msg);
            //    err_flag = false;
            //}
        }

        private void read_record()
        {
            glay.vwstring0 = AR_001_CTERM.credit_term_code;
            glay.vwstring1=AR_001_CTERM.description;
            glay.vwint0 = AR_001_CTERM.num_of_days;
            glay.vwstring2 = AR_001_CTERM.note;
            glay.vwbool0 = false;
            if (AR_001_CTERM.active_status == "Y")
                glay.vwbool0 = true;
        
        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            err_flag = true;
            glay.vwstring0 = id;
            delete_record();

            if (!err_flag)
            {
                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "1", Text = delmsg });
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary.ToArray(),
                                    "Value",
                                    "Text")
                                   , JsonRequestBehavior.AllowGet);


            }
            return RedirectToAction("Index"); 

        }
	}
}
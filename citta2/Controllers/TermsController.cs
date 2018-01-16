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
    public class TermsController : Controller
    {
        AP_001_PTERM AP_001_PTERM = new AP_001_PTERM();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();

        bool err_flag = true;
        string action_flag = "";
        
        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.AP_001_PTERM
                         select new vw_genlay
                         {
                             vwstring0 = bh.payment_term_code,
                             vwstring1 = bh.description,
                             vwint0 = bh.num_of_days,
                             vwstring2=bh.note,
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
            AP_001_PTERM = db.AP_001_PTERM.Find(key1);
            if (AP_001_PTERM != null)
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
            AP_001_PTERM = db.AP_001_PTERM.Find(glay.vwstring0);
            if (AP_001_PTERM!=null)
            {
                db.AP_001_PTERM.Remove(AP_001_PTERM);
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
            if (action_flag=="Create")
            {
                AP_001_PTERM = new AP_001_PTERM();
                AP_001_PTERM.created_by = pubsess.userid;
                AP_001_PTERM.created_date = DateTime.UtcNow;
            }
            else
            {
                AP_001_PTERM = db.AP_001_PTERM.Find(glay.vwstring0);
            }

            AP_001_PTERM.payment_term_code = glay.vwstring0;
            AP_001_PTERM.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            AP_001_PTERM.num_of_days = glay.vwint0;
            AP_001_PTERM.modified_date = DateTime.UtcNow;
            AP_001_PTERM.modified_by = pubsess.userid;
            AP_001_PTERM.note = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            AP_001_PTERM.active_status = glay.vwbool0 ? "Y" : "N";


           if(action_flag == "Create")
                db.Entry(AP_001_PTERM).State = EntityState.Added;
            else
                db.Entry(AP_001_PTERM).State = EntityState.Modified;
           
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
           // string error_msg="";
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //{
            //    ModelState.AddModelError(String.Empty, "Please enter Id");
            //    err_flag = false;
            //}

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please enter Description");
                err_flag = false;
            }
            if (glay.vwint0 <= 0)
            {
                ModelState.AddModelError(String.Empty, "Please insert valid Number of Days");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                string sqlstr = "select '1' query0 from AP_001_PTERM where description=" + util.sqlquote(glay.vwstring1);
                var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
                if (bglist1 != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicate Description");
                    err_flag = false;
                }
            }

        }

        private void read_record()
        {
            glay.vwstring0=AP_001_PTERM.payment_term_code;
            glay.vwstring1=AP_001_PTERM.description;
            glay.vwint0 = AP_001_PTERM.num_of_days;
            glay.vwstring2 = AP_001_PTERM.note;
            if (AP_001_PTERM.active_status == "Y")
                glay.vwbool0 = true;
        
        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AP_001_PTERM] where payment_term_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }
	}
}
using CittaErp.Models;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using anchor1.Filters;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class ItmuseController : Controller
    {
        GB_001_ITMUS GB_001_ITMUS = new GB_001_ITMUS();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();

        bool err_flag = true;
        string delmsg = "";
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.GB_001_ITMUS
                         select new vw_genlay
                         {
                             vwstring0 = bh.item_usage_id,
                             vwstring1 = bh.item_usage_name,
                             };


            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            glay.vwblarray0 = new bool[20];
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            glay.vwblarray0 = new bool[20];
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            glay.vwblarray0 = new bool[20];
            GB_001_ITMUS = db.GB_001_ITMUS.Find(key1);
            if (GB_001_ITMUS != null)
                read_record();

            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            glay.vwblarray0 = new bool[20];
            return View(glay);
        }


        private void delete_record()
        {
            
                GB_001_ITMUS = db.GB_001_ITMUS.Find(glay.vwstring0);
                db.GB_001_ITMUS.Remove(GB_001_ITMUS);
                db.SaveChanges();
           
         

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
                GB_001_ITMUS = new GB_001_ITMUS();
                GB_001_ITMUS.created_by = pubsess.userid;
                GB_001_ITMUS.created_date = DateTime.UtcNow;
                }
            else
            {
                GB_001_ITMUS = db.GB_001_ITMUS.Find(glay.vwstring0);
            }

            GB_001_ITMUS.item_usage_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            GB_001_ITMUS.item_usage_name = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            GB_001_ITMUS.modified_date = DateTime.UtcNow;
            GB_001_ITMUS.modified_by = pubsess.userid;
            GB_001_ITMUS.spare_part = glay.vwblarray0[0] ? "Y" : "N";
            GB_001_ITMUS.sales = glay.vwblarray0[1] ? "Y" : "N";
            GB_001_ITMUS.purchases = glay.vwblarray0[2] ? "Y" : "N";
            GB_001_ITMUS.production = glay.vwblarray0[3] ? "Y" : "N";
            GB_001_ITMUS.consumables = glay.vwblarray0[4] ? "Y" : "N";
           

           if(action_flag == "Create")
                db.Entry(GB_001_ITMUS).State = EntityState.Added;
            else
                db.Entry(GB_001_ITMUS).State = EntityState.Modified;
           
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
                string sqlstr = "select '1' query0 from GB_001_ITMUS where item_usage_name=" + util.sqlquote(glay.vwstring1);
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
            glay.vwstring0 = GB_001_ITMUS.item_usage_id;
            glay.vwstring1=GB_001_ITMUS.item_usage_name;
            glay.vwblarray0[0] = false;
            glay.vwblarray0[1] = false;
            glay.vwblarray0[2] = false;
            glay.vwblarray0[3] = false;
            glay.vwblarray0[4] = false;
            if (GB_001_ITMUS.spare_part == "Y")
                glay.vwblarray0[0] = true;
            if (GB_001_ITMUS.sales == "Y")
                glay.vwblarray0[1] = true;
            if (GB_001_ITMUS.purchases == "Y")
                glay.vwblarray0[2] = true;
            if (GB_001_ITMUS.production == "Y")
                glay.vwblarray0[3] = true;
            if (GB_001_ITMUS.consumables == "Y")
                glay.vwblarray0[4] = true;
        
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


            return RedirectToAction("Index");

        }
	}
}
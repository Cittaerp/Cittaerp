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
    public class Disc_app_selController : Controller
    {
        DC_001_DISTS DC_001_DISTS = new DC_001_DISTS();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();

        bool err_flag = true;
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.DC_001_DISTS
                         select new vw_genlay
                         {
                             //vwstring0 = bh.credit_term_code,
                             //vwstring1 = bh.description,
                             //vwint0 = bh.num_of_days,
                             //vwstring2=bh.note,
                             vwstring3 = bh.active_status == "N" ? "Open" : "Closed"
                         };
            
            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
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

            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            DC_001_DISTS = db.DC_001_DISTS.Find(key1);
            if (DC_001_DISTS != null)
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
            return View(glay);
        }


        private void delete_record()
        {
            DC_001_DISTS = db.DC_001_DISTS.Find(glay.vwstring0);
            if (DC_001_DISTS!=null)
            {
                db.DC_001_DISTS.Remove(DC_001_DISTS);
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
                DC_001_DISTS = new DC_001_DISTS();
                DC_001_DISTS.created_by = pubsess.userid;
                DC_001_DISTS.created_date = DateTime.UtcNow;
            }
            else
            {
                DC_001_DISTS = db.DC_001_DISTS.Find(glay.vwstring0, glay.vwstring2, glay.vwstring3);
            }
            DC_001_DISTS.discount_selection_basis = glay.vwstring0;
            DC_001_DISTS.selection_code = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            DC_001_DISTS.discount_code = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;

            //DC_001_DISTS.discount_flag = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            if (glay.vwstring2 == "all")
            {
                DC_001_DISTS.discount_flag = "Y";
            }
            else
            {
                DC_001_DISTS.discount_flag = "N";
            }


            DC_001_DISTS.modified_date = DateTime.UtcNow;
            DC_001_DISTS.modified_by = pubsess.userid;
            DC_001_DISTS.active_status = glay.vwbool0 ? "Y" : "N";

           if(action_flag == "Create")
                db.Entry(DC_001_DISTS).State = EntityState.Added;
            else
                db.Entry(DC_001_DISTS).State = EntityState.Modified;
           
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
            string error_msg="";
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
                error_msg = "Please enter Id";

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
                error_msg = "Please enter Name";

           if(action_flag == "Create")
            {
                DC_001_DISTS bnk = db.DC_001_DISTS.Find(glay.vwstring0);
                if (bnk != null)
                    error_msg = "Can not accept duplicates";
            }

            if (error_msg !="")
            {
                ModelState.AddModelError(String.Empty, error_msg);
                err_flag = false;
            }
        }

        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstring0 = DC_001_DISTS.discount_selection_basis;
            glay.vwstring2 = DC_001_DISTS.selection_code;
            glay.vwstring3 = DC_001_DISTS.discount_code;
            //glay.vwstring3 = DC_001_DISTS.discount_flag;

            if (DC_001_DISTS.active_status == "Y")
            {
                glay.vwbool0 = true;
            }
            
        }

        private void error_message()
        {

        }


        [HttpPost]
        public ActionResult pricehead_list(string id)
        {

            if (id == "disc1")
            {
                var hdet = from bg in db.IV_001_ITEM
                           orderby bg.item_name
                           select new
                           {
                               c1 = bg.item_code,
                               c2 = bg.item_name
                           };
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    hdet.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
            }
            else if (id == "disc2")
            {
                var hdet = (from bg in db.GB_001_COY
                            where bg.id_code == "COYPRICE"
                            select bg).FirstOrDefault();

                string pcl1 = hdet.field6;
                string pcl2 = hdet.field7;
                string pcl3 = hdet.field8;
                string pcl4 = hdet.field9;
                string pcl5 = hdet.field10;
                string pcl6 = hdet.field11;

                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "1", Text = pcl1 });
                ary.Add(new SelectListItem { Value = "2", Text = pcl2 });
                ary.Add(new SelectListItem { Value = "3", Text = pcl3 });
                ary.Add(new SelectListItem { Value = "4", Text = pcl4 });
                ary.Add(new SelectListItem { Value = "5", Text = pcl5 });
                ary.Add(new SelectListItem { Value = "6", Text = pcl6 });

                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary,
                                    "Value",
                                    "Text")
                               , JsonRequestBehavior.AllowGet);
            }
            else if (id == "disc4")
            {
                var hdet = from bg in db.GB_001_HANAL
                           orderby bg.header_description
                           select new
                           {
                               c1 = bg.header_sequence,
                               c2 = bg.header_description
                           };

                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    hdet.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);

            }
            else
            {
                var hdet = from bg in db.AR_001_CUSTM
                           orderby bg.cust_biz_name
                           select new
                           {
                               c1 = bg.customer_code,
                               c2 = bg.cust_biz_name
                           };


                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    hdet.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
   

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[DC_001_DISTS] where credit_term_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using anchor1.Filters;
using System.Data.Entity;
using CittaErp.utilities;

namespace CittaErp.Controllers
{
    public class Analysis_code_headerController : Controller
    {
        //
        // GET: /Analysis_code_header/
        GB_001_HANAL GB_001_HANAL = new GB_001_HANAL();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        psess psess =new psess();
        cittautil util = new cittautil();

        string ptype = "";
        bool err_flag = true;
        string delmsg = "";
        string action_flag = "";

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            var bglist = from bh in db.GB_001_HANAL

                         select new vw_genlay
                         {
                             vwstring0 = bh.header_sequence,
                             vwstring1 = bh.header_description,
                             vwstring2 = bh.items_to_capture,


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
            //select_query();
            return View(glay);
        }
        
        [HttpPost]
        public ActionResult delete_list(string id)
        {
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

            //select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            GB_001_HANAL = db.GB_001_HANAL.Find(key1);
            if (GB_001_HANAL != null)
                read_record();

            //            select_query();
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
                if (err_flag == false)
                {
                    select_query();
                    return View(glay);
                }
                return RedirectToAction("Index");
            }

            update_file();
            if (err_flag)
                return RedirectToAction("Index");

            //select_query();
            return View(glay);
        }
        private void delete_record()
        {
            if (util.delete_check("HANAL", glay.vwstring0))
            {
                GB_001_HANAL = db.GB_001_HANAL.Find(glay.vwstring0);
                db.GB_001_HANAL.Remove(GB_001_HANAL);
                db.SaveChanges();
            }
            else
            {
                delmsg = "Analysis Header in Use";
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
           if(action_flag == "Create")
            {
                GB_001_HANAL = new GB_001_HANAL();
                GB_001_HANAL.created_by = pubsess.userid;
                GB_001_HANAL.created_date = DateTime.UtcNow;
                GB_001_HANAL.delete_flag = "N";
            
            }
            else
            {
                GB_001_HANAL = db.GB_001_HANAL.Find(glay.vwstring0);
            }

            GB_001_HANAL.header_sequence = glay.vwstring0;
            GB_001_HANAL.header_description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            GB_001_HANAL.items_to_capture = glay.vwbool0 ? "Y" : "N";
            GB_001_HANAL.modified_date = DateTime.UtcNow;
            GB_001_HANAL.modified_by = pubsess.userid;
            GB_001_HANAL.active_status = glay.vwbool1 ? "Y" : "N";
            GB_001_HANAL.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
           


           if(action_flag == "Create")
                db.Entry(GB_001_HANAL).State = EntityState.Added;
            else
                db.Entry(GB_001_HANAL).State = EntityState.Modified;

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
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //    error_msg = "Please enter Id";

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Please enter Name;

           if(action_flag == "Create")
            {
                GB_001_HANAL bnk = db.GB_001_HANAL.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }
        private void select_query()
        {
            //var bglist = from bh in db.GL_001_CHART
            //           select bh;

            //    ViewBag.glaccount = new SelectList(bglist.ToList(), "account_code", "account_name", glay.vwstring2);

        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];

        }
        private void read_record()
        {
            //glay.vwstrarray0 = new string[50];
            glay.vwstring0 = GB_001_HANAL.header_sequence;
            glay.vwstring1 = GB_001_HANAL.header_description;
            
           if ( GB_001_HANAL.items_to_capture=="Y"){
               glay.vwbool0 = true;
           }

            if (GB_001_HANAL.active_status == "Y")
            {
                glay.vwbool1 = true;
            }
            glay.vwstring3 = GB_001_HANAL.note;
        }
        private void error_message()
        {

        }





	}
}
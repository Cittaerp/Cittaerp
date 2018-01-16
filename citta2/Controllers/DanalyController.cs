using CittaErp.Models;
using anchor1.Filters;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class DanalyController : Controller
    {
        GB_001_DANAL GB_001_DANAL = new GB_001_DANAL();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        bool err_flag = true;
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.GB_001_DANAL
                         join bk1 in db.GB_001_HANAL
                         on new { a1 = bh.header_sequence } equals new { a1 =bk1.header_sequence}
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.header_sequence,
                             vwstring1 = bh.analysis_code,
                             vwstring2 = bh.analysis_description,
                             vwstring4 = bh.note,
                             vwstring3 = bh.active_status == "N" ? "Active" : "Inactive",
                             vwstring5 = bk3.header_description
                         };
            
            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            glay.vwstring3 = "N";
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
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
        public ActionResult Edit(string key1, string key2)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            GB_001_DANAL = db.GB_001_DANAL.Find(key1, key2);
            if (GB_001_DANAL != null)
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

            if (id_xhrt=="D")
            { //delete record
                delete_record();
                return RedirectToAction("Index");
            }

            photo1 = photofile;
            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }


        private void delete_record()
        {
            GB_001_DANAL = db.GB_001_DANAL.Find(glay.vwstring0, glay.vwstring1);
            if (GB_001_DANAL!=null)
            {
                db.GB_001_DANAL.Remove(GB_001_DANAL);
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
                GB_001_DANAL = new GB_001_DANAL();
                GB_001_DANAL.created_by = pubsess.userid;
                GB_001_DANAL.created_date = DateTime.UtcNow;
            }
            else
            {
                GB_001_DANAL = db.GB_001_DANAL.Find(glay.vwstring0, glay.vwstring1);
            }

            GB_001_DANAL.attach_document = "";
            GB_001_DANAL.header_sequence = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            GB_001_DANAL.analysis_code = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            GB_001_DANAL.analysis_description = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GB_001_DANAL.date_range_limit = string.IsNullOrWhiteSpace(glay.vwstring3) ? "N" : glay.vwstring3;
            if  (GB_001_DANAL.date_range_limit == "N")
            {
                GB_001_DANAL.date_from = DateTime.UtcNow.ToString("yyyyMMdd");
                GB_001_DANAL.date_to = DateTime.UtcNow.ToString("yyyyMMdd");
                }  
            else 
            {
                GB_001_DANAL.date_from = util.date_yyyymmdd(glay.vwstring5);
                GB_001_DANAL.date_to = util.date_yyyymmdd(glay.vwstring6);
                }
            GB_001_DANAL.modified_date = DateTime.UtcNow;
            GB_001_DANAL.modified_by = pubsess.userid;
            GB_001_DANAL.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            GB_001_DANAL.active_status = glay.vwbool0 ? "Y" : "N";
            


           if(action_flag == "Create")
                db.Entry(GB_001_DANAL).State = EntityState.Added;
            else
                db.Entry(GB_001_DANAL).State = EntityState.Modified;
           
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
            if (err_flag)
            {
                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("DANALYSIS", GB_001_DANAL.analysis_code, photo1, glay.vwstrarray9);
                }

            }

        }

        private void validation_routine()
        {
           // string error_msg="";
            if (string.IsNullOrWhiteSpace(glay.vwstring1))
                 {
                ModelState.AddModelError(String.Empty, "Please enter Id");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring2))
                 {
                ModelState.AddModelError(String.Empty, "Description must not be spaces");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                GB_001_DANAL bnk = db.GB_001_DANAL.Find(glay.vwstring0, glay.vwstring1);
                if (bnk != null)
                     {
                ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                err_flag = false;
            }
            }
            if (glay.vwstring3 == "Y")
            {
                if (!util.date_validate(glay.vwstring6))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid end date");
                    err_flag = false;
                }

                if (!util.date_validate(glay.vwstring5))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid start date");
                    err_flag = false;
                }
            }
            else
            {
                glay.vwstring6 = "";
                glay.vwstring5 = "";
            }

           
        }
        private void select_query()
        {
            ViewBag.header_sequence = util.para_selectquery("50", glay.vwstring0);
            //var bglist = from bh in db.GB_001_HANAL
            //             where bh.active_status == "N"
            //             orderby bh.header_description
            //             select bh;

            //ViewBag.header_sequence = new SelectList(bglist.ToList(), "header_sequence", "header_description", glay.vwstring0);

        }

        private void read_record()
        {
            glay.vwstring0 = GB_001_DANAL.header_sequence;
            glay.vwstring1 = GB_001_DANAL.analysis_code;
            glay.vwstring2 = GB_001_DANAL.analysis_description;
            glay.vwstring3 = GB_001_DANAL.date_range_limit;
            glay.vwstring5 = util.date_slash(GB_001_DANAL.date_from);
            glay.vwstring6 = util.date_slash(GB_001_DANAL.date_to);
            glay.vwstring4 = GB_001_DANAL.note;
            glay.vwbool0 = false;
            if (GB_001_DANAL.active_status == "Y")
                glay.vwbool0 = true;
                       
            var bglist = from bg in db.GB_001_DOC
                        where bg.screen_code == " DANALYSIS" && bg.document_code == GB_001_DANAL.analysis_code
                    orderby bg.document_sequence
                    select bg;

        ViewBag.anapict = bglist.ToList();
        
        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_DANAL] where header_sequence+'[]'+analysis_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }

        public ActionResult show_doc(int id)
        {
            var bglist = (from bg in db.GB_001_DOC
                          where bg.document_sequence == id
                          select bg).FirstOrDefault();


            byte[] imagedata = bglist.document_image;
            return File(imagedata, "png");
        }
 
	}
}
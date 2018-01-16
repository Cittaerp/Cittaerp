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
    public class WkflwController : Controller
    {
        WF_001_WKFL WF_001_WKFL = new WF_001_WKFL();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        bool err_flag = true;
        string action_flag = "";
        
        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.WF_001_WKFL
                         select new vw_genlay
                         {
                             vwstring0 = bh.approval_group_code,
                             vwstring1 = bh.description,
                             vwstring2 = bh.group_member,
                             vwdecimal0 = bh.transaction_minimum_amount,
                             vwstring3=bh.note,
                             vwstring4 = bh.active_status == "N" ? "Active" : "Inactive"
                         };
            
            return View(bglist.ToList());


        }
   
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            select_query();
            return View(glay);
        }

        [HttpPost]
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

        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            WF_001_WKFL = db.WF_001_WKFL.Find(key1);
            if (WF_001_WKFL != null)
                read_record();

            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt=="D")
            { 
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
            WF_001_WKFL = db.WF_001_WKFL.Find(glay.vwstring0);
            if (WF_001_WKFL!=null)
            {
                db.WF_001_WKFL.Remove(WF_001_WKFL);
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
                WF_001_WKFL = new WF_001_WKFL();
                WF_001_WKFL.created_by = pubsess.userid;
                WF_001_WKFL.created_date = DateTime.UtcNow;
            }
            else
            {
                WF_001_WKFL = db.WF_001_WKFL.Find(glay.vwstring0);
            }
            WF_001_WKFL.attach_document = "";
            WF_001_WKFL.approval_group_code= string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            WF_001_WKFL.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            WF_001_WKFL.group_member = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            WF_001_WKFL.transaction_minimum_amount = glay.vwdecimal0;
            WF_001_WKFL.modified_date = DateTime.UtcNow;
            WF_001_WKFL.modified_by = pubsess.userid;
            WF_001_WKFL.note = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            WF_001_WKFL.active_status = glay.vwbool0 ? "Y" : "N";


           if(action_flag == "Create")
                db.Entry(WF_001_WKFL).State = EntityState.Added;
            else
                db.Entry(WF_001_WKFL).State = EntityState.Modified;
           
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
                    util.write_document("WORKFLOW", WF_001_WKFL.approval_group_code, photo1, glay.vwstrarray9);
                }

            }

        }

        private void validation_routine()
        {
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "ID must not be spaces");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Description must not be spaces");
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(glay.vwstring2))
            {
                ModelState.AddModelError(String.Empty, "Group Member must not be spaces");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                WF_001_WKFL bnk = db.WF_001_WKFL.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }

        private void read_record()
        {
            glay.vwstring0=WF_001_WKFL.approval_group_code;
            glay.vwstring1=WF_001_WKFL.description;
            glay.vwstring2 = WF_001_WKFL.group_member;
            glay.vwdecimal0 = WF_001_WKFL.transaction_minimum_amount;
            glay.vwstring3 = WF_001_WKFL.note;
            glay.vwbool0 = false;
            if (WF_001_WKFL.active_status=="Y")
                glay.vwbool0=true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "WORKFLOW" && bg.document_code == WF_001_WKFL.approval_group_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();
        
        }
        private void select_query()
        {
            ViewBag.group = util.para_selectquery("67", glay.vwstring2,"N");
            //ViewBag.group = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring2);

            //var bgitem = from bg in db.GB_001_EMP
            //             where bg.active_status == "N"
            //             orderby bg.name
            //             select bg;

            //ViewBag.group = new SelectList(bgitem.ToList(), "employee_code", "name", glay.vwstring2);

        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[WF_001_WKFL] where approval_group_code=" + util.sqlquote(id);
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
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
    public class WareHController : Controller
    {
        IV_001_WAREH IV_001_WAREH = new IV_001_WAREH();
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


            var bglist = from bh in db.IV_001_WAREH
                         join bk in db.GB_001_PCODE
                         on new { a1 = "01", a2 = bh.site_code } equals new {a1 = bk.parameter_type, a2 = bk.parameter_code }
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.warehouse_code,
                             vwstring1 = bh.warehouse_name,
                             vwstring6 = bh.contact_name,
                             vwstring3 = bk2.parameter_name,
                             vwstring4 = bh.branch_address_code,
                             vwstring2 = bh.active_status == "N" ? "Active" : "Inactive"
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
            IV_001_WAREH = db.IV_001_WAREH.Find(key1);
            if (IV_001_WAREH != null)
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

        [HttpPost]
        public ActionResult Index1(string id_xhrt)
        {
            if (id_xhrt == "D")
            {
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
            IV_001_WAREH = db.IV_001_WAREH.Find(glay.vwstring0);
            if (IV_001_WAREH!=null)
            {
                db.IV_001_WAREH.Remove(IV_001_WAREH);
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
                IV_001_WAREH = new IV_001_WAREH();
                IV_001_WAREH.created_by = pubsess.userid;
                IV_001_WAREH.created_date = DateTime.UtcNow;
            }
            else
            {
                IV_001_WAREH = db.IV_001_WAREH.Find(glay.vwstring0);
            }

                IV_001_WAREH.warehouse_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                IV_001_WAREH.warehouse_name = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                IV_001_WAREH.site_code =string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
                IV_001_WAREH.branch_address_code = string.IsNullOrWhiteSpace(glay.vwstring10) ? "" : glay.vwstring10;
                IV_001_WAREH.contact_name = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
                IV_001_WAREH.contact_email = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
                IV_001_WAREH.contact_phone = string.IsNullOrWhiteSpace(glay.vwstring8) ? "" : glay.vwstring8;
                IV_001_WAREH.modified_date = DateTime.UtcNow;
                IV_001_WAREH.modified_by = pubsess.userid;
                IV_001_WAREH.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
                IV_001_WAREH.active_status = glay.vwbool0 ? "Y" : "N";
                IV_001_WAREH.default_warehouse = glay.vwbool1 ? "Y" : "N";
                IV_001_WAREH.attach_document = string.IsNullOrWhiteSpace(glay.vwstring9) ? "Y" : glay.vwstring9;


           if(action_flag == "Create")  
                db.Entry(IV_001_WAREH).State = EntityState.Added;
            else 
                db.Entry(IV_001_WAREH).State = EntityState.Modified;

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
                if (glay.vwbool1)
                {
                    string str = " update IV_001_WAREH set default_warehouse = 'N' where warehouse_code !="+ util.sqlquote(glay.vwstring0);
                    db.Database.ExecuteSqlCommand(str);
                }
                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("WAREHOUSE", IV_001_WAREH.warehouse_code, photo1, glay.vwstrarray9);
                }

            }

        }

        private void validation_routine()
        {
           // string error_msg="";
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, " Please enter warehouse id");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, " Please enter Name");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                IV_001_WAREH bnk = db.IV_001_WAREH.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }

        private void read_record()

        {
            glay.vwstring0 = IV_001_WAREH.warehouse_code;
            glay.vwstring1 = IV_001_WAREH.warehouse_name;
            glay.vwstring2 = IV_001_WAREH.site_code;
            glay.vwstring10 = IV_001_WAREH.branch_address_code;
            glay.vwstring4 = IV_001_WAREH.note;
            if (IV_001_WAREH.active_status == "Y")
                glay.vwbool0 = true;
            if (IV_001_WAREH.default_warehouse == "Y")
                glay.vwbool1 = true;
            
            glay.vwstring6 = IV_001_WAREH.contact_name;
            glay.vwstring7 = IV_001_WAREH.contact_email;
            glay.vwstring8 = IV_001_WAREH.contact_phone;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "WAREHOUSE" && bg.document_code == IV_001_WAREH.warehouse_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();

        
        }
        private void select_query()
        {
            ViewBag.site_code = util.para_selectquery("01", glay.vwstring2,"N");
            //var bglist = from bg in db.GB_001_PCODE
            //             where bg.parameter_type=="01" && bg.active_status == "N"
            //             orderby bg.parameter_name
            //             select bg;
            //ViewBag.site_code = new SelectList(bglist.ToList(), "parameter_code", "parameter_name", glay.vwstring2);

            var bgitem = from bg in db.AR_001_DADRS
                         where bg.address_type == "CY" && bg.active_status == "N"
                         orderby bg.location_alias
                         select bg;

            ViewBag.branch = new SelectList(bgitem.ToList(), "address_code", "location_alias", glay.vwint2);

            ViewBag.job = util.para_selectquery("04", glay.vwstring9,"N");
            //var bgite = from bg in db.GB_001_PCODE
            //             where bg.parameter_type == "04" && bg.active_status == "N"
            //            orderby bg.parameter_name
            //             select bg;
            //ViewBag.job = new SelectList(bgite.ToList(), "parameter_code", "parameter_name", glay.vwstring9);


        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[IV_001_WAREH] where warehouse_code=" + util.sqlquote(id);
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
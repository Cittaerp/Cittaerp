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
    public class CurrenController : Controller
    {
        MC_001_CUREN MC_001_CUREN = new MC_001_CUREN();
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


            var bglist = from bh in db.MC_001_CUREN
                         join bg in db.GB_001_PCODE
                         on new { a1 = bh.currency_code } equals new { a1= bg.gl_account_code}
                         where bg.parameter_type =="13"
                         select new vw_genlay
                         {
                             vwstring0 = bh.currency_code,
                             vwstring1 = bh.currency_sym,
                             vwstring2 = bh.symbol_display,
                             vwstring3 = bh.gl_acc_for_real,
                             vwstring4 = bh.gl_acc_for_unreal,
                             vwstring5 = bh.note,
                             vwstring6 = bh.currency_description,
                             vwstring7 = bh.active_status == "N" ? "Active" : "Inactive",
                             vwstring8 = bg.parameter_name
                         };
            
            return View(bglist.ToList());


        }
   
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
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
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            MC_001_CUREN = db.MC_001_CUREN.Find(key1);
            if (MC_001_CUREN != null)
                read_record();

            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
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
            MC_001_CUREN = db.MC_001_CUREN.Find(glay.vwstring0);
            if (MC_001_CUREN!=null)
            {
                db.MC_001_CUREN.Remove(MC_001_CUREN);
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
                MC_001_CUREN = new MC_001_CUREN();
                MC_001_CUREN.created_by = pubsess.userid;
                MC_001_CUREN.created_date = DateTime.UtcNow;
            }
            else
            {
                MC_001_CUREN = db.MC_001_CUREN.Find(glay.vwstring0);
            }
            MC_001_CUREN.attach_document = "";
            MC_001_CUREN.currency_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            MC_001_CUREN.currency_sym = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            MC_001_CUREN.symbol_display = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            MC_001_CUREN.gl_acc_for_real = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            MC_001_CUREN.gl_acc_for_unreal = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            MC_001_CUREN.modified_date = DateTime.UtcNow;
            MC_001_CUREN.modified_by = pubsess.userid;
            MC_001_CUREN.note = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            MC_001_CUREN.currency_description = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            MC_001_CUREN.active_status = glay.vwbool0 ? "Y" : "N";


           if(action_flag == "Create")
                db.Entry(MC_001_CUREN).State = EntityState.Added;
            else
                db.Entry(MC_001_CUREN).State = EntityState.Modified;
           
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
                util.parameter_deleteflag("007", glay.vwstring0);
                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, MC_001_CUREN b where a.account_code = b.gl_acc_for_real";
                //str += " and currency_code =" + util.sqlquote(glay.vwstring0);
                ////db.Database.ExecuteSqlCommand(str);

                // str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, MC_001_CUREN b where a.account_code = b.gl_acc_for_unreal";
                //str += " and currency_code =" + util.sqlquote(glay.vwstring0);
                ////db.Database.ExecuteSqlCommand(str);
               

                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("CURRENCY", MC_001_CUREN.currency_code, photo1, glay.vwstrarray9);
                }

            }

        }

        private void validation_routine()
        {
            //string error_msg="";
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
                {
                ModelState.AddModelError(String.Empty, "Please enter ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
                {
                ModelState.AddModelError(String.Empty, "Please enter name");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                MC_001_CUREN bnk = db.MC_001_CUREN.Find(glay.vwstring0);
                if (bnk != null)
                    {
                ModelState.AddModelError(String.Empty,  "Can not accept duplicates");
                err_flag = false;
            }
            }

        }
        private void select_query()
        {
            //var bglist = from bh in db.MC_001_CUREN
            //             select bh;

            //ViewBag.currency = new SelectList(bglist.ToList(), "currency_code", "currency_description", glay.vwstring1);

            var bglistd = from bh in db.GB_999_MSG
                          where bh.type_msg == "CURREN"
                          orderby bh.name1_msg
                         select bh;

            ViewBag.symbol_display = new SelectList(bglistd.ToList(), "code_msg", "name1_msg", glay.vwstring2);


            ViewBag.unrealised = util.read_ledger("009", glay.vwstring4);
            ViewBag.realised = util.read_ledger("008", glay.vwstring3);
            //var bg2 = util.read_ledger("009");
            //ViewBag.unrealised = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring4);

            //bg2 = util.read_ledger("008");
            //ViewBag.realised = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring3);
        }

        private void read_record()
        {
            glay.vwstring0 = MC_001_CUREN.currency_code;
            glay.vwstring1 = MC_001_CUREN.currency_sym;
            glay.vwstring2 = MC_001_CUREN.symbol_display;
            glay.vwstring3 = MC_001_CUREN.gl_acc_for_real;
            glay.vwstring4 = MC_001_CUREN.gl_acc_for_unreal;
            glay.vwstring5 = MC_001_CUREN.note;
            glay.vwstring6 = MC_001_CUREN.currency_description;
            glay.vwbool0 = false;
            if (MC_001_CUREN.active_status == "Y")
                glay.vwbool0 = true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "CURRENCY" && bg.document_code == MC_001_CUREN.currency_code
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
            string sqlstr = "delete from [dbo].[MC_001_CUREN] where currency_code=" + util.sqlquote(id);
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
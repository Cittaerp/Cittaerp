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
    public class BuploadController : Controller
    {
        BK_002_BANKR BK_002_BANKR = new BK_002_BANKR();
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


            var bglist = from bh in db.BK_002_BANKR
                         select new vw_genlay
                         {
                             vwstring0 = bh.bank_code,
                             vwstring1 = util.date_slash(bh.transaction_date),
                             vwdecimal0 = bh.credit_amount,
                             vwdecimal1=bh.debit_amount,
                         };
            
            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
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
        public ActionResult Create (vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
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
            BK_002_BANKR = db.BK_002_BANKR.Find(key1);
            if (BK_002_BANKR != null)
                read_record();
            select_query();
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
            select_query();
            return View(glay);
        }


        private void delete_record()
        {
            BK_002_BANKR = db.BK_002_BANKR.Find(glay.vwstring0);
            if (BK_002_BANKR!=null)
            {
                db.BK_002_BANKR.Remove(BK_002_BANKR);
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
                BK_002_BANKR = new BK_002_BANKR();
                BK_002_BANKR.created_by = pubsess.userid;
                BK_002_BANKR.created_date = DateTime.UtcNow;
            }
            else
            {
                BK_002_BANKR = db.BK_002_BANKR.Find(glay.vwstring0);
            }
            BK_002_BANKR.attach_document = "";
            BK_002_BANKR.bank_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            BK_002_BANKR.transaction_date = util.date_yyyymmdd(glay.vwstring1);
            BK_002_BANKR.reference_number = glay.vwint0;
            BK_002_BANKR.description = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            BK_002_BANKR.debit_amount = glay.vwdecimal0;
            BK_002_BANKR.credit_amount = glay.vwdecimal1;
            BK_002_BANKR.value_date = util.date_yyyymmdd(glay.vwstring3);
            BK_002_BANKR.modified_date = DateTime.UtcNow;
            BK_002_BANKR.modified_by = pubsess.userid;
            BK_002_BANKR.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            BK_002_BANKR.post_checkbox = glay.vwbool0 ? "Y" : "N";


           if(action_flag == "Create")
                db.Entry(BK_002_BANKR).State = EntityState.Added;
            else
                db.Entry(BK_002_BANKR).State = EntityState.Modified;
           
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
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please enter Id");
                err_flag = false;
            }

            if (!util.date_validate(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
                err_flag = false;
            }

            if (!util.date_validate(glay.vwstring3))
            {
                ModelState.AddModelError(String.Empty, "Please insert a valid value date");
                err_flag = false;
            }

        }

        private void read_record()
        {
            glay.vwstring0=BK_002_BANKR.bank_code;
            glay.vwstring1 = util.date_slash(BK_002_BANKR.transaction_date);
            glay.vwstring2=BK_002_BANKR.description;
            glay.vwint0 = BK_002_BANKR.reference_number;
            glay.vwdecimal0 = BK_002_BANKR.debit_amount;
            glay.vwdecimal1 = BK_002_BANKR.credit_amount;
            glay.vwstring3 = util.date_slash(BK_002_BANKR.value_date);
            glay.vwstring2 = BK_002_BANKR.note;
            if (BK_002_BANKR.post_checkbox == "Y")
                glay.vwbool0 = true;
        
        }

        private void select_query()
        {
            ViewBag.bank = util.para_selectquery("004", glay.vwstring0);

            //var bglist = from bg in db.BK_001_BANK
            //             where bg.active_status == "N"
            //             orderby bg.bank_name
            //             select bg;

            //ViewBag.bank = new SelectList(bglist.ToList(), "bank_code", "bank_name", glay.vwstring0);


        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[BK_002_BANKR] where bank_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }
	}
}
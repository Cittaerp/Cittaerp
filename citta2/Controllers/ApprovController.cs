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
    public class ApprovController : Controller
    {
        WF_001_APRDG WF_001_APRDG = new WF_001_APRDG();
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


            var bglist = from bh in db.WF_001_APRDG
                         join bk1 in db.GB_999_MSG
                         on new { a1 = bh.delegation_transaction } equals new { a1 = bk1.code_msg }
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()
                         join bk4 in db.GB_001_EMP
                         on new { b1 = bh.delegator_employee } equals new { b1 = bk4.employee_code }
                         into bk5
                         from bk6 in bk5.DefaultIfEmpty()
                         join bk7 in db.GB_001_EMP
                         on new { b1 = bh.delegated_employee } equals new { b1 = bk7.employee_code }
                         into bk8
                         from bk9 in bk8.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwint0 = bh.approval_delegation_sequence,
                             vwstring1 = bh.delegation_transaction,
                             vwstring2 = bh.delegator_employee,
                             vwstring3 = bh.delegated_employee,
                             vwstring7 = bh.active_status == "N" ? "Active" :"Inactive",
                             vwstring4 = bk3.name1_msg,
                             vwstring5 = bk6.name,
                             vwstring6 = bk9.name
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
        public ActionResult Create(vw_genlay glay_in)
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(int key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            WF_001_APRDG = db.WF_001_APRDG.Find(key1);
            if (WF_001_APRDG != null)
                read_record();

            select_query();
            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            glay = glay_in;

            if (id_xhrt == "D")
            {
                delete_record();
                return RedirectToAction("Index");
            }

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }

        //[HttpPost]
        //public ActionResult Index1(string id_xhrt)
        //{
        //    if (id_xhrt == "D")
        //    {
        //        delete_record();
        //        return RedirectToAction("Index");
        //    }

        //    update_file();
        //    if (err_flag)
        //        return RedirectToAction("Index");
        //    return View(glay);

        //}
        private void delete_record()
        {
            WF_001_APRDG = db.WF_001_APRDG.Find(glay.vwint0);
            if (WF_001_APRDG != null)
            {
                db.WF_001_APRDG.Remove(WF_001_APRDG);
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
                WF_001_APRDG = new WF_001_APRDG();
                WF_001_APRDG.created_by = pubsess.userid;
                WF_001_APRDG.created_date = DateTime.UtcNow;
              
                
            }
            else
            {
                WF_001_APRDG = db.WF_001_APRDG.Find(glay.vwint0);
            }

            
            WF_001_APRDG.approval_delegation_sequence = glay.vwint0;
            WF_001_APRDG.delegation_transaction = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            WF_001_APRDG.delegator_employee = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            WF_001_APRDG.delegated_employee = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            WF_001_APRDG.start_date = util.date_yyyymmdd(glay.vwstring5);
            WF_001_APRDG.end_date = util.date_yyyymmdd(glay.vwstring6);
            WF_001_APRDG.modified_date = DateTime.UtcNow;
            WF_001_APRDG.modified_by = pubsess.userid;
            WF_001_APRDG.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            WF_001_APRDG.active_status = glay.vwbool0 ? "Y" : "N";


           if(action_flag == "Create")
                db.Entry(WF_001_APRDG).State = EntityState.Added;
            else
                db.Entry(WF_001_APRDG).State = EntityState.Modified;

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
            //string error_msg = "";
            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Transaction must not be spaces");
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(glay.vwstring2))
            {
                ModelState.AddModelError(String.Empty, "Delegator must not be spaces");
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(glay.vwstring3))
            {
                ModelState.AddModelError(String.Empty, "Delegatee must not be spaces");
                err_flag = false;
            }

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

           if(action_flag == "Create")
            {
                WF_001_APRDG bnk = db.WF_001_APRDG.Find(glay.vwint0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }

        private void read_record()
        {
            
            glay.vwint0 = WF_001_APRDG.approval_delegation_sequence;
            glay.vwstring1 = WF_001_APRDG.delegation_transaction;
            glay.vwstring2 = WF_001_APRDG.delegator_employee;
            glay.vwstring3 = WF_001_APRDG.delegated_employee;
            glay.vwstring4 = WF_001_APRDG.note;
            if (WF_001_APRDG.active_status == "Y")
                glay.vwbool0 = true;
            glay.vwstring5 = util.date_slash(WF_001_APRDG.start_date);
            glay.vwstring6 = util.date_slash(WF_001_APRDG.end_date);


        }
        private void select_query()
        {
            var bglist = from bg in db.GB_999_MSG
                         where bg.type_msg == "APPROV"
                         orderby bg.name1_msg
                         select bg;

            ViewBag.delegation = new SelectList(bglist.ToList(), "code_msg", "name1_msg", glay.vwstring1);


            ViewBag.delagator = util.para_selectquery("62", glay.vwstring2);
           // ViewBag.delagator = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring2);
           
            //var bgitem = from bg in db.GB_001_EMP
            //             where bg.active_status == "N"
            //             orderby bg.name
            //             select bg;

            //ViewBag.delagator = new SelectList(bgitem.ToList(), "employee_code", "name", glay.vwstring2);

            ViewBag.delegatee = util.para_selectquery("62", glay.vwstring3);
           // ViewBag.delegatee = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring3);

        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[WF_001_APRDG] where approval_delegation_sequence=" + util.sqlquote(id);

             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }

    }
}
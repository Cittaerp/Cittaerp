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
    public class RevalController : Controller
    {
        MC_002_REVAL MC_002_REVAL = new MC_002_REVAL();
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


            var bglist = from bh in db.MC_002_REVAL
                         join bk1 in db.GB_999_MSG
                         on new{a1 = "AC", a2 = bh.account_code } equals new { a1 = bk1.type_msg, a2 = bk1.code_msg }
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.reference_number,
                             vwstring1 = bh.posting_date,
                             vwstring2 = bk3.name1_msg,
                             vwdecimal0 = bh.current_balance
                         };

            return View(bglist.ToList());


        }

        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
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

            initial_rtn();
            select_query();
            return View(glay);
        }

        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            initial_rtn();
            
            pubsess = (pubsess)Session["pubsess"];
            

            MC_002_REVAL = db.MC_002_REVAL.Find(key1);
            if (MC_002_REVAL != null)
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

            if (id_xhrt == "D")
            {
                delete_record();
                return RedirectToAction("Index");
            }
            photo1 = photofile;
            update_file();
            if (err_flag)
                return RedirectToAction("Index");

            initial_rtn();
            select_query();
            return View(glay);
        }

        [HttpPost]
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
            MC_002_REVAL = db.MC_002_REVAL.Find(glay.vwstring0);
            if (MC_002_REVAL != null)
            {
                db.MC_002_REVAL.Remove(MC_002_REVAL);
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
                MC_002_REVAL = new MC_002_REVAL();
                MC_002_REVAL.created_by = pubsess.userid;
                MC_002_REVAL.created_date = DateTime.UtcNow;
            }
            else
            {
                MC_002_REVAL = db.MC_002_REVAL.Find(glay.vwstring0);
            }
            MC_002_REVAL.attach_document = "";
            MC_002_REVAL.reference_number = glay.vwint0.ToString();
            MC_002_REVAL.currency_code = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            MC_002_REVAL.posting_date = util.date_yyyymmdd(glay.vwstring2);
            MC_002_REVAL.account_code = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            MC_002_REVAL.current_balance = glay.vwdecimal0;
            MC_002_REVAL.base_balance = glay.vwdecimal1;
            MC_002_REVAL.closing_exchange_rate = glay.vwdecimal2;
            MC_002_REVAL.base_revalued_balance = glay.vwdecimal3;
            MC_002_REVAL.unrealised_gain_loss = glay.vwdecimal4;
            MC_002_REVAL.auto_reversal_date = util.date_yyyymmdd(glay.vwstring4);
            MC_002_REVAL.approval_level = glay.vwint1;
            MC_002_REVAL.approval_by = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            MC_002_REVAL.modified_date = DateTime.UtcNow;
            MC_002_REVAL.modified_by = pubsess.userid;
            MC_002_REVAL.note = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;



           if(action_flag == "Create")
                db.Entry(MC_002_REVAL).State = EntityState.Added;
            else
                db.Entry(MC_002_REVAL).State = EntityState.Modified;

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
                    util.write_document("REVAL", MC_002_REVAL.reference_number.ToString(), photo1, glay.vwstrarray9);
                }

            }

        }

        private void validation_routine()
        {
            //string error_msg = "";
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //    error_msg = "Please enter Id";

            //if (Session["action_flag"].ToString() == "Create")
            //{
            //    MC_002_REVAL bnk = db.MC_002_REVAL.Find(glay.vwstring0);
            //    if (bnk != null)
            //        error_msg = "Can not accept duplicates";
            //}
            
            if (!util.date_validate(glay.vwstring2))
            {
                ModelState.AddModelError(String.Empty, "Please insert a posting  date");
                err_flag = false;
            }
            
            if (!util.date_validate(glay.vwstring4))
            {
                ModelState.AddModelError(String.Empty, "Please insert a reversal date");
                err_flag = false;
            }
           
            //if (error_msg != "")
            //{
            //    ModelState.AddModelError(String.Empty, error_msg);
            //    err_flag = false;
            //}
        }

        private void read_record()
        {

            glay.vwstring0 = MC_002_REVAL.reference_number;
            glay.vwstring1 = MC_002_REVAL.currency_code;
            glay.vwstring2 = util.date_slash(MC_002_REVAL.posting_date);
            glay.vwstring3 = MC_002_REVAL.account_code;
            glay.vwdecimal0 = MC_002_REVAL.current_balance;
            glay.vwdecimal1 = MC_002_REVAL.base_balance;
            glay.vwdecimal2 = MC_002_REVAL.closing_exchange_rate;
            glay.vwdecimal3 = MC_002_REVAL.base_revalued_balance;
            glay.vwdecimal4 = MC_002_REVAL.unrealised_gain_loss;
            glay.vwstring4 = util.date_slash(MC_002_REVAL.auto_reversal_date);
            glay.vwint1 = MC_002_REVAL.approval_level;
            glay.vwstring6 = MC_002_REVAL.approval_by;
            glay.vwstring7 = MC_002_REVAL.note;


            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "REVAL" && bg.document_code == MC_002_REVAL.reference_number.ToString()
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();

        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
        }
        private void select_query()
        {
            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);

            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring0);

            //var bgitemi = from bg in db.MC_001_CUREN
            //              where bg.active_status == "N"
            //              orderby bg.currency_description
            //              select bg;

            //ViewBag.currency = new SelectList(bgitemi.ToList(), "currency_code", "currency_description", glay.vwstring0);

            var bgiteme = from bg in db.GB_999_MSG
                          where bg.type_msg == "AC"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.account = new SelectList(bgiteme.ToList(), "code_msg", "name1_msg", glay.vwstring2);


        }

        private void error_message()
        {

        }

        //[HttpPost]
        //public ActionResult pricehead_list(string id)
        //{
        //    // write your query statement
        //    var hdet = from bg in db.GB_001_PCODE
        //               where bg.parameter_type == "14" && bg.gl_account_code == id
        //               orderby bg.parameter_name
        //               select new
        //               {
        //                   c1 = bg.parameter_code,
        //                   c2 = bg.parameter_name
        //               };


        //    if (HttpContext.Request.IsAjaxRequest())
        //        return Json(new SelectList(
        //                        hdet.ToArray(),
        //                        "c1",
        //                        "c2")
        //                   , JsonRequestBehavior.AllowGet);
        //    //}
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[MC_002_REVAL] where reference_number=" + util.sqlquote(id);

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
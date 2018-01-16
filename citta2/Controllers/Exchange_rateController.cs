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
    public class Exchange_rateController : Controller
    {
        //
        // GET: /Exchange_rate/
        MC_001_EXCRT MC_001_EXCRT = new MC_001_EXCRT();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();

        string ptype = "";
        bool err_flag = true;
        string action_flag = "";

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.MC_001_EXCRT
                         join bk1 in db.MC_001_CUREN
                         on new { a1 = bh.currency_code } equals new { a1 = bk1.currency_code}
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()

                         select new vw_genlay
                         {
                             vwstring0 = bh.currency_code,
                             vwstring2 = bh.date_from,
                             vwstring3 = bh.date_to,
                             vwstring1 = bk3.currency_description,
                             vwdecimal0 = bh.exchange_rate
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
        public ActionResult delete_list(string id)
        {
          
            // write your query statement
            string sqlstr = "delete from [dbo].[MC_001_EXCRT] where currency_code+'[]'+ date_from=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in)
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
        public ActionResult Edit(string key1, string key2)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            MC_001_EXCRT = db.MC_001_EXCRT.Find(key1, key2);
            if (MC_001_EXCRT != null)
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
        private void delete_record()
        {
            MC_001_EXCRT = db.MC_001_EXCRT.Find(glay.vwstring0, util.date_yyyymmdd(glay.vwstring2));
            if (MC_001_EXCRT != null)
            {
                db.MC_001_EXCRT.Remove(MC_001_EXCRT);
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
                MC_001_EXCRT = new MC_001_EXCRT() ;
                MC_001_EXCRT.created_by = pubsess.userid;
                MC_001_EXCRT.created_date = DateTime.UtcNow;
            }
            else
            {
                MC_001_EXCRT = db.MC_001_EXCRT.Find(glay.vwstring0, util.date_yyyymmdd(glay.vwstring2));
            }

            MC_001_EXCRT.currency_code = glay.vwstring0;
            MC_001_EXCRT.date_from = util.date_yyyymmdd(glay.vwstring2);
            MC_001_EXCRT.date_to = util.date_yyyymmdd(glay.vwstring3);
            MC_001_EXCRT.exchange_rate = glay.vwdecimal0;
            MC_001_EXCRT.modified_date = DateTime.UtcNow;
            MC_001_EXCRT.modified_by = pubsess.userid;
            MC_001_EXCRT.active_status = glay.vwbool0 ? "Y" : "N";
            MC_001_EXCRT.note = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;


           if(action_flag == "Create")
                db.Entry(MC_001_EXCRT).State = EntityState.Added;
            else
                db.Entry(MC_001_EXCRT).State = EntityState.Modified;

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
            string datestr = "";
            string d1 = util.date_yyyymmdd(glay.vwstring2);
            string d2 = util.date_yyyymmdd(glay.vwstring3);
            
            if (glay.vwdecimal0 == 0)
            {
                ModelState.AddModelError(String.Empty, "Exchange rate can not be zero");
                err_flag = false;
            }
           if(action_flag == "Create")
            {
                MC_001_EXCRT bnk = db.MC_001_EXCRT.Find(glay.vwstring0, glay.vwstring2);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }

                datestr = "select 1 intquery0 from MC_001_EXCRT where " + util.sqlquote(d1) + " between date_from and date_to  and currency_code=" + util.sqlquote(glay.vwstring0);
                int datevar = db.Database.SqlQuery<querylay>(datestr).ToList().Count;
                if (datevar > 0)
                {
                    ModelState.AddModelError(String.Empty, "Date From is overlappinng previous rates");
                    err_flag = false;

                }

                datestr = "select 1 intquery0 from MC_001_EXCRT where " + util.sqlquote(d2) + " between date_from and date_to  and currency_code=" + util.sqlquote(glay.vwstring0);
                datevar = db.Database.SqlQuery<querylay>(datestr).ToList().Count;
                if (datevar > 0)
                {
                    ModelState.AddModelError(String.Empty, "Date To is overlappinng previous rates");
                    err_flag = false;

                }

            }
            if (!util.date_validate(glay.vwstring2))
            {
                ModelState.AddModelError(String.Empty, "Please insert a valid start date");
                err_flag = false;
            }
            if (!util.date_validate(glay.vwstring3))
            {
                ModelState.AddModelError(String.Empty, "Please insert a valid end date");
                err_flag = false;
            }

            if (Convert.ToInt32(d1) > Convert.ToInt32(d2))
            {
                ModelState.AddModelError(String.Empty, "End date must be greater than start date");
                err_flag = false;
            }


            if (Session["action_flag"].ToString() == "Edit")
            {
                datestr = "select date_from query0 from MC_001_EXCRT where " + util.sqlquote(d2) + " between date_from and date_to  and currency_code=" + util.sqlquote(glay.vwstring0);
                var datevart = db.Database.SqlQuery<querylay>(datestr).ToList();
                foreach(var item in datevart)
                {
                    if (d1!=item.query0){
                    ModelState.AddModelError(String.Empty, "Date To is overlapping previous rates");
                    err_flag = false;
                    }
                }
            }
        }

        private void select_query() 
        {
        //    var current = from pc in db.MC_001_CUREN
        //                  where pc.active_status == "N"
        //                  orderby pc.currency_description
        //             select pc;
        //    ViewBag.currency = new SelectList(current.ToList(), "currency_code", "currency_description", glay.vwstring0);
            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring0);

            read_label();
        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
        }
        private void read_record() 
        {
            glay.vwstrarray0 = new string[50];
            
            glay.vwstring0 = MC_001_EXCRT.currency_code;
            glay.vwstring2 = util.date_slash(MC_001_EXCRT.date_from);
            glay.vwstring3 = util.date_slash(MC_001_EXCRT.date_to);
           // glay.vwdate1 = MC_001_EXCRT.date_from;
            glay.vwdecimal0 = MC_001_EXCRT.exchange_rate;
            glay.vwstring1 = MC_001_EXCRT.note;
            if (MC_001_EXCRT.active_status == "Y")
            {
                glay.vwbool0 = true;
            }
           
        }
        private void error_message()
        {

        }

        private void read_label()
        {
            if (pubsess.exchange_rate_mode=="B")
            {
                glay.vwstring5 = "1 " + pubsess.base_currency_description + " = ";
                glay.vwstring6 = "";
            }
            else
            {
                glay.vwstring5 = "1 currency code = ";
                glay.vwstring6 = pubsess.base_currency_description;
            }
        }
    }
}
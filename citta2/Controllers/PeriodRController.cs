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
    public class PeriodRController : Controller
    {
        GB_001_PCT GB_001_PCT = new GB_001_PCT();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();

        bool err_flag = true;
        int num_prd = 0;
        string action_flag = "";
        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            

            var bglist = from bh in db.GB_001_PCT
                         select new vw_genlay
                         {
                             vwstring0 = bh.period_number,
                             vwstring2 = bh.date_from,
                             vwstring3 = bh.date_to,
                             vwstring1=bh.note,
                         };
            
            return View(bglist.ToList());


        }
   
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            return View(glay);
        }

        public ActionResult periodmenu()
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            psess.temp1 = (from bg in db.GB_001_COY where bg.id_code == "COYPRD" select bg.field3).FirstOrDefault();
            Session["psess"] = psess;
               
                return RedirectToAction("Prd");

        }
        public ActionResult Prd()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            return View(glay);
        }
        [HttpPost]
        public ActionResult Create (vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            return View(glay);
        }

        [HttpPost]
        public ActionResult Prd(vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;
            psess.sarrayt2 = glay.vwstrarray1;
            psess.sarrayt3 = glay.vwstrarray2;
            psess.sarrayt1 = glay.vwstrarray3;
            Session["psess"] = psess;
            
            update_file();

            if (err_flag)
                return RedirectToAction("Home", "Log_in");

            return View(glay);
        }
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            GB_001_PCT = db.GB_001_PCT.Find(key1);
            if (GB_001_PCT != null)
                read_record();

            return View(glay);
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
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

        [HttpPost]
        public ActionResult period_calp(string num_prd)
        {
            psess = (psess)Session["psess"];
            string num_prd1 = num_prd;
            psess.temp1 = num_prd;
            Session["psess"] = psess;
            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = num_prd1 });
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                               , JsonRequestBehavior.AllowGet);

            //}
            return RedirectToAction("Prd");
        }
        private void delete_record()
        {
            GB_001_PCT = db.GB_001_PCT.Find(glay.vwstring0);
            if (GB_001_PCT!=null)
            {
                db.GB_001_PCT.Remove(GB_001_PCT);
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

            //List<string> cal = new List<string>();
            

            num_prd = Convert.ToInt16(psess.temp1 );

            for (int ctr = 0; ctr < num_prd; ctr ++)
            {
                GB_001_PCT GB_001_PCT = new Models.GB_001_PCT();
                    GB_001_PCT.period_number = (ctr+1).ToString();
                    GB_001_PCT.prd_description = glay.vwstrarray3[ctr];
                    GB_001_PCT.date_from = glay.vwstrarray1[ctr];
                    GB_001_PCT.date_to = glay.vwstrarray2[ctr];
                    GB_001_PCT.created_date = DateTime.UtcNow;
                    GB_001_PCT.created_by = pubsess.userid;
                    GB_001_PCT.modified_date = DateTime.UtcNow;
                    GB_001_PCT.modified_by = pubsess.userid;
                    GB_001_PCT.note = "";
                    //var a = String.Concat(GB_001_PCT.period_number, GB_001_PCT.date_from, GB_001_PCT.date_to, GB_001_PCT.created_date, GB_001_PCT.created_by, GB_001_PCT.modified_date, GB_001_PCT.modified_by, GB_001_PCT.note);

                   
                    db.Entry(GB_001_PCT).State = EntityState.Added;

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
           

        }

        private void validation_routine()
        {
            int fromlen = glay.vwstrarray1.Length;
           // string error_msg = ""; int ldatey =0;
            DateTime cmgdate = DateTime.ParseExact(glay.vwstrarray2[fromlen-1], "dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            DateTime nextdate = cmgdate.AddDays(1);
            DateTime fdate = DateTime.ParseExact(glay.vwstrarray1[0], "dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            int daten = new DateTime(fdate.Year, 12, 31).DayOfYear;
            int firstdate = DateTime.ParseExact(glay.vwstrarray1[0], "dd/MM", System.Globalization.CultureInfo.InvariantCulture).DayOfYear;
            int lastdate = DateTime.ParseExact(glay.vwstrarray2[fromlen - 1], "dd/MM", System.Globalization.CultureInfo.InvariantCulture).DayOfYear;

            if (nextdate < fdate)
            {
                ModelState.AddModelError(String.Empty, "period dates not in sync ");
                err_flag = false;
            }
            else if (nextdate > fdate)
            {
                ModelState.AddModelError(String.Empty, "period dates not in sync");
                err_flag = false;
            }
            else
                err_flag = true;
            //if (lastdate < firstdate)
            //    ldatey = lastdate + daten;
            //else
            //    ldatey = lastdate;

            //int datedif = ldatey - firstdate;
            //if (daten > datedif + 1)
            //{
            //    ModelState.AddModelError(String.Empty, "period calender is less than a year");
            //    err_flag = false;
            //}
            //if (daten < datedif + 1)
            //{
            //    ModelState.AddModelError(String.Empty, "period calender is greater than a year");
            //    err_flag = false;
            //}
            //for (int ctr = 0; ctr < fromlen-1; ctr++)
            //{
            //    DateTime a = DateTime.ParseExact(glay.vwstrarray2[ctr], "dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            //    DateTime b = DateTime.ParseExact(glay.vwstrarray1[ctr+1], "dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            //    if (a > b)
            //    {
            //        ModelState.AddModelError(String.Empty, "Date To must not be higher than previous Date From");
            //        err_flag = false;
            //    }
            //}
            ////    glay.vwstring0 = glay.vwint0.ToString(); 

            //if (Session["action_flag"].ToString() == "Create")
            //{
            //    GB_001_PCT bnk = db.GB_001_PCT.Find(glay.vwstring0);
            //    if (bnk != null)
            //        error_msg = "Can not accept duplicates";
            //}

            //if (error_msg !="")
            //{
            //    ModelState.AddModelError(String.Empty, error_msg);
            //    err_flag = false;
            //}
        }

        private void read_record()
        {
            glay.vwstring0=GB_001_PCT.period_number;
            glay.vwstring2 = GB_001_PCT.date_from;
            glay.vwstring3 = GB_001_PCT.date_to;
            glay.vwstring1 = GB_001_PCT.note;
            glay.vwint0 = Convert.ToInt16(GB_001_PCT.period_number);
        
        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_PCT] where period_number=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult prd_calculate(string mthend)
        {
            //string cur_code = "";
            //string shw_rate = "";
            //string rate_flag = "N";
            //string curen_name = "";
            // write your query statement
            int num = 1;
            DateTime cmgdate = DateTime.ParseExact(mthend, "dd/MM", System.Globalization.CultureInfo.InvariantCulture);
            DateTime nextdate = cmgdate.AddDays(num);

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = nextdate.ToString("dd/MM") });
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            //}
            return RedirectToAction("Index");
        }

	}
}
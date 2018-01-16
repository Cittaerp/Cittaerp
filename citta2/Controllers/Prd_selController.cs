using CittaErp.Models;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CittaErp.Controllers
{
    public class Prd_selController : Controller
    {
        GB_001_COY GB_001_COY = new GB_001_COY();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();

        bool err_flag = true;
        bool up1_flag;
        string prdbasis = "";
        //
        // GET: /Prd_sel/
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            

            //var bglist = from bh in db.GB_001_COY
            //             select new vw_genlay
            //             {
            //                 vwstring0 = bh.period_sel_code,
            //                 vwstring1 = bh.period_closing_basis == "C" ? "Calender" : "Period",
            //                 vwstring3 = bh.posting_waiver == "Y"? "Yes" : "No"
            //             };


            //return View(bglist.ToList());
            return View();

        }

        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            select_query();
            return View(glay);
        }

   
        [HttpPost]
        public ActionResult Create(vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            select_query();
            return View(glay);
        }
        public ActionResult Edit()
        {
           // util.init_values();
            ViewBag.action_flag = "Edit";
            psess = (psess)Session["psess"];
            initial_rtn();

            pubsess = (pubsess)Session["pubsess"];
            GB_001_COY = (from bk in db.GB_001_COY
                          where bk.id_code == "COYPRD"
                          select bk).FirstOrDefault();

            if (GB_001_COY != null)
            {
                psess.temp0 = GB_001_COY.field1;
                read_record();
                
            }
            else
                psess.temp0 = "";
            Session["psess"] = psess;
            select_query();
            return View(glay);

        }
    
        [HttpPost]

         public ActionResult Edit(vw_genlay glay_in, string id_xhrt, string coybtn)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;

            if (coybtn=="0")
                return RedirectToAction("Home", "Log_in");

            if (id_xhrt=="D")
            { 
                delete_record();
                return RedirectToAction("Index");
            }

            update_file();
          
             if (err_flag)
                    return RedirectToAction("Home", "Log_in");
            
            initial_rtn();
            select_query();
            return View(glay);
        }

        //public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        //{

        //    pubsess = (pubsess)Session["pubsess"];
        //    glay = glay_in;

        //    if (id_xhrt == "D")
        //    { //delete record
        //        delete_record();
        //        return RedirectToAction("Index");
        //    }

        //    update_file();
        //    if (err_flag)
        //        return RedirectToAction("Index");
        //    initial_rtn();
        //    select_query();
        //    return View(glay);
        //}

        private void delete_record()
        {
            if (util.delete_check("PST", glay.vwstring0))
            {
                GB_001_COY = db.GB_001_COY.Find(glay.vwstring0);
                db.GB_001_COY.Remove(GB_001_COY);
                db.SaveChanges();
            }
            else
            {
                //delmsg = "Period in Use";
                //ModelState.AddModelError(String.Empty, delmsg);
                //err_flag = false;

            }

        }
 
        private void select_query()
        {
            var bglistc = from bg in db.GB_999_MSG
                          where bg.type_msg == "FYM"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.end = new SelectList(bglistc.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[18]);

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
            //company period selection
            GB_001_COY = db.GB_001_COY.Find("COYPRD");
            up1_flag = false;
            if (GB_001_COY == null)
            {
                GB_001_COY = new GB_001_COY();
                init_coyrecord();
                GB_001_COY.created_by = pubsess.userid;
                GB_001_COY.created_date = DateTime.UtcNow;
                GB_001_COY.id_code = "COYPRD";
                up1_flag = true;
            }

           // GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[17]) ? "" : glay.vwstrarray0[17];
          //  GB_001_COY.field3 = glay.vwdecimal0.ToString();
            if (prdbasis == "C")
            {
                //GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray1[0]) ? "" : glay.vwstrarray1[0];
               // GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                GB_001_COY.field5 = util.date_yyyymmdd(glay.vwstring3);
                GB_001_COY.field6 = util.date_yyyymmdd(glay.vwstring4);
                GB_001_COY.field7 = util.date_yyyymmdd(glay.vwstring5) ;
                GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            }
            else
            {
                //GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[18]) ? "" : glay.vwstrarray0[18];
                GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
                GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstring8) ? "" : glay.vwstring8;
                GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            }
            GB_001_COY.modified_by = pubsess.userid;
            GB_001_COY.modified_date = DateTime.UtcNow;

            if (up1_flag)
                db.Entry(GB_001_COY).State = EntityState.Added;
            else
                db.Entry(GB_001_COY).State = EntityState.Modified;

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
            prdbasis = psess.temp0.ToString();
            if (glay.vwstring1 == null)
            {
                glay.vwstring1 = "";
            }
            if (glay.vwstring1 == "Y")
            {
                if (!util.date_validate(glay.vwstring3))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Open Date From");
                    err_flag = false;
                } if (!util.date_validate(glay.vwstring4))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Open Date To");
                    err_flag = false;
                } if (!util.date_validate(glay.vwstring5))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Expiry Date");
                    err_flag = false;
                }
            }
            if (glay.vwstring2 == null)
            {
                glay.vwstring2 = "";
            }
            if (glay.vwstring2 == "Y")
            {
                if (!util.date_validate(glay.vwstring6))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Open Period From");
                    err_flag = false;
                }
                if (!util.date_validate(glay.vwstring7))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Open Period To");
                    err_flag = false;
                } if (!util.date_validate(glay.vwstring8))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Open Period From");
                    err_flag = false;
                }
            }
           //if (prdbasis == "P" && glay.vwstring0 != "")
           // {
           //     string current_period = glay.vwstring0;
           //     string ddate = current_period.Substring(0, 2);

           //     decimal current = Convert.ToDecimal(ddate);


           //     if (current > glay.vwdecimal0)
           //     {
           //         ModelState.AddModelError(String.Empty, "Current Period has exceeded Number of Period");
           //         err_flag = false;

           //     }
           // }
           // else
           // {
           // }
          

            //if (error_msg !="")
            //{
            //    ModelState.AddModelError(String.Empty, error_msg);
            //    err_flag = false;
            //}
        }

        private void read_record()
        {
            GB_001_COY rcoyprd = db.GB_001_COY.Find("COYPRD");
            if (rcoyprd != null)
            {
              //  glay.vwdecimal0 = Convert.ToDecimal(rcoyprd.field3);
                if (psess.temp0.ToString() == "C")
                {
                    //glay.vwstrarray1[1] = rcoyprd.field4;
                    glay.vwstring3 = util.date_slash(rcoyprd.field5);
                    glay.vwstring4 = util.date_slash(rcoyprd.field6);
                    glay.vwstring5 = util.date_slash(rcoyprd.field7);
                    glay.vwstring1 = rcoyprd.field8;
                }
                else
                {
                    //glay.vwstrarray0[18] = rcoyprd.field2;
                    glay.vwstring0 = rcoyprd.field4;
                    glay.vwstring6 = rcoyprd.field5;
                    glay.vwstring7 = rcoyprd.field6;
                    glay.vwstring8 = rcoyprd.field7;
                    glay.vwstring2 = rcoyprd.field8;
                }
            }
        }

        private void init_coyrecord()
        {
            GB_001_COY.field1 = "";
            GB_001_COY.field2 = "";
            GB_001_COY.field3 = "";
            GB_001_COY.field4 = "";
            GB_001_COY.field5 = "";
            GB_001_COY.field6 = "";
            GB_001_COY.field7 = "";
            GB_001_COY.field8 = "";
            GB_001_COY.field9 = "";
            GB_001_COY.field10 = "";
            GB_001_COY.field11 = "";
            GB_001_COY.field12 = "";
            GB_001_COY.field13 = "";
            GB_001_COY.field14 = "";
            GB_001_COY.field15 = "";

        }
        private void error_message()
        {

        }

        //[HttpPost]
        //public ActionResult delete_list(string id)
        //{
        //    // write your query statement
        //    err_flag = true;
        //    glay.vwstring0 = id;
        //    delete_record();

        //    if (!err_flag)
        //    {
        //        List<SelectListItem> ary = new List<SelectListItem>();
        //        ary.Add(new SelectListItem { Value = "1", Text = delmsg });
        //        if (HttpContext.Request.IsAjaxRequest())
        //            return Json(new SelectList(
        //                            ary.ToArray(),
        //                            "Value",
        //                            "Text")
        //                           , JsonRequestBehavior.AllowGet);


        //    }
        //    return RedirectToAction("Index");

        //}
	
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];

        }

     
	}
}
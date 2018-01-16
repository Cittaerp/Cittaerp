using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
//using CittaErp.Filters;
using CittaErp.utilities;
using System.Data.Entity;

namespace CittaErp.Controllers
{
    public class zglrepwController : Controller
    {
        //
        // GET: /StdDgn/

        private MainContext db = new MainContext();
        private MainContext db2 = new MainContext();
        Boolean err_flag = true;
        string laction = "";
        vw_genlay mcollect = new vw_genlay();
        cittautil util = new cittautil();
        tab_calc tab_calc = new tab_calc();
        psess psess;
        pubsess pblock;
        string type_code = "";
        string err_message;


        //[EncryptionActionAttribute]
        public ActionResult Index(string pc = null)
        {
            //if (utils.check_option() == 1||pc==null)
            //    return RedirectToAction("Welcome", "Home");
            util.init_values();
            psess.temp0 = "33";
            type_code = (string)psess.temp0;

            formula_title(pc);
            psess.temp5 = type_code;
            string rep1t = psess.temp8.ToString();
            var blist = from s in db.tab_calc
                        where s.para_code==type_code
                        orderby s.name1
                        select new vw_genlay { vwstring0 = s.calc_code, vwstring1 = s.name1};

            return View(blist.ToList());
        }

        //
        // GET: /Bank/Create

        //[EncryptionActionAttribute]
        public ActionResult Create(string xy = null)
        {
            mcollect.datmode = "C";
            //if (utils.check_option() == 1 || xy != "1")
            //    return RedirectToAction("Welcome", "Home");

            pblock = (pubsess)Session["pubsess"];
            type_code = (string)psess.temp0;
//            psess.temp4 = "1";
            init_class();
  
            bank_group_select();

            return View("Edit", mcollect);
        }

        //
        // POST: /Bank/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            mcollect = mcollect1;
            type_code = psess.temp0;
            laction = "Create";

            write_data();
            if (err_flag)
                return RedirectToAction("Create");

            bank_group_select();
            mcollect.datmode = "C";

            return View("Edit", mcollect);

        }

        //

        //[EncryptionActionAttribute]
        public ActionResult Edit(string jy )
        {
            mcollect.datmode = "E";
            //if (utils.check_option() == 1 || jy == null)
            //    return RedirectToAction("Welcome", "Home");

            pblock = (pubsess)Session["pubsess"];
            type_code = (string)psess.temp0;
            
            tab_calc = db.tab_calc.Find(type_code, jy);
            if (tab_calc == null)
            {
                return RedirectToAction("Index");
            }

  //          psess.temp4 = "1";
            read_daily_record();
            bank_group_select();
            return View(mcollect);
        }

        //
        // POST: /Bank/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(vw_genlay mcollect1, string xhrt)
        {
            pblock = (pubsess)Session["pubsess"];
            type_code = (string)psess.temp0;
            mcollect = mcollect1;

            if (xhrt == "D")
            {
                DeleteConfirmed();
                return RedirectToAction("Index");
            }

            write_data();
            if (err_flag)
                return RedirectToAction("Index");

            bank_group_select();
            mcollect.datmode = "E";

            return View(mcollect);

        }

        //
        // POST: /Bank/Delete/5

        //[HttpPost, ActionName("Delete")]
        private void DeleteConfirmed()
        {
            tab_calc = db.tab_calc.Find(type_code, mcollect.vwcode );
            write_log("Delete");
            db.tab_calc.Remove(tab_calc);
            db.SaveChanges();

            //return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void bank_group_select()
        {
            mcollect.vwstring10 = type_code;

            string str1 = "select distinct array_code c1, array_code c2 from tab_array where para_code='31' order by 2";
            var bgaddc = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.lrow = new SelectList(bgaddc.ToList(), "c1", "c2");

            str1 = "select distinct calc_code c1, name1 c2 from tab_calc where para_code='30' order by 2";
            bgaddc = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.lcolumn = new SelectList(bgaddc.ToList(), "c1", "c2");


        }

        private void write_data()
        {
            pblock = (pubsess)Session["pubsess"];
            err_flag = true;
            validate_fields();

            if (err_flag)
                write_rec();
        }

        private void write_rec()
        {
            DateTime odate = util.logdatetime();
            if (laction == "Create")
            {
                tab_calc = new tab_calc();
                tab_calc.internal_use = "N";
                tab_calc.created_by = pblock.userid;
                tab_calc.date_created = odate;
            }
            else
            {
                tab_calc = db.tab_calc.Find(type_code, mcollect.vwcode);
            }

            tab_calc.para_code = type_code;
            tab_calc.calc_code = mcollect.vwcode;
            tab_calc.name1= mcollect.vwstring0;
            tab_calc.report_name = mcollect.vwstring1;
            tab_calc.transfer_code = mcollect.vwstring2;
            tab_calc.suppress_zero = "";
            tab_calc.wide_column = "";
            tab_calc.report_type = "";
            tab_calc.menu_option = "";
            tab_calc.comment_code = "";
            tab_calc.addnotes = "";
            tab_calc.amended_by = pblock.userid;
            tab_calc.date_amended = odate;

            if (laction == "Create")
                db.tab_calc.Add(tab_calc);
            else
                db.Entry(tab_calc).State = EntityState.Modified;

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


            if(err_flag)
            {
//                write_temp("", "");
                write_log(laction);
            }

        }

        private void read_daily_record()
        {
            pblock = (pubsess)Session["pubsess"];
            init_class();
            mcollect.vwcode = tab_calc.calc_code;
            mcollect.vwstring0 = tab_calc.name1;
            mcollect.vwstring1 = tab_calc.report_name;
            mcollect.vwstring2 = tab_calc.transfer_code;

        }

        private void validate_fields()
        {
            err_message = "";
            if (string.IsNullOrWhiteSpace(mcollect.vwcode))
            {
                err_message = "Code can not be Spaces";
                ModelState.AddModelError(String.Empty, err_message);
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(mcollect.vwstring0))
            {
                err_message = "Name can not be Spaces";
                ModelState.AddModelError(String.Empty, err_message);
                err_flag = false;
            }

            if (laction == "Create")
            {
                tab_calc cbank = db.tab_calc.Find(type_code, mcollect.vwcode);
                if (cbank != null)
                {
                    err_message = "Record Already Created";
                    ModelState.AddModelError(String.Empty, err_message);
                    err_flag = false;
                }
            }


        }

        [HttpPost]
        public ActionResult DailyList(string idx)
        {
            string Code = idx;
           
            string str1 = "select c1, c2 from ( ";
            str1 += "select calc_code c1, name1 c2, 'EXCEP'c3 from tab_calc where para_code='20' union all ";
            str1 += "select report_code , report_name1, 'TRANS' from tab_soft where para_code='DREP1' union all ";
            str1 += "select report_code , report_name1, 'WKORD' from tab_soft where para_code='WKREP' union all ";
            str1 += "select report_code , report_name1, 'ANNV' from tab_soft where para_code='HREP2' union all ";
            str1 += "select train_code , course_name, 'BALAN' from tab_train where para_code='H24' union all ";
            str1 += "select doc_code , name1, 'PDOC' from tab_document where doc_type='P' union all ";
            str1 += "select doc_code , name1, 'HDOC' from tab_document where doc_type='H' union all ";
            str1 += "select report_code, report_name1, 'AUDIT' from tab_soft where para_code='HAUD' union all ";
            str1 += "select trans_type, name1, 'TRANSBASE' from tab_type union all ";
            str1 += "select report_code , report_name1, 'TRANSBASE' from tab_soft where para_code='CADV' ) b ";
            str1 += " where b.c3=" + util.pads(Code);
            str1 += " order by 2 ";
            var bgcol7 = db.Database.SqlQuery<vw_query>(str1);
            
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                bgcol7.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");
        }


        private void init_class()
        {
            mcollect.vwcode = "";
            mcollect.vwdclarray0 = new decimal[50];
            mcollect.vwdclarray1 = new decimal[50];
            mcollect.vwdclarray2 = new decimal[50];
            mcollect.vwdclarray3 = new decimal[50];
            mcollect.vwstrarray0 = new string[50];
            mcollect.vwstrarray1 = new string[50];
            mcollect.vwstrarray2 = new string[50];
            mcollect.vwstrarray3 = new string[50];
            mcollect.vwstrarray4 = new string[50];
            mcollect.vwblarray0 = new bool[50];
            mcollect.vwblarray1 = new bool[50];
            mcollect.vwblarray2 = new bool[50];
            mcollect.vwstrarray6 = new string[50];
            //mcollect.vwstring3 = (string)(psess.temp9);
            //if ((string)(psess.temp0) == "H20" || (string)(psess.temp0) == "HA20")
            //    mcollect.ws_string8 = "A";
            //else
            //    mcollect.ws_string8 = "";
        }

 


        private void formula_title(string pc)
        {
                psess.temp1 = "Financial Report Writer Definition ";
                psess.temp2 = "S";
                psess.temp7 = " and report_name5=='Y' ";
                psess.temp8 = "DREP";
                psess.temp9 = "PYREP";
            //}
            //else if (pc == "H16")
            //{
            //    psess.temp1 = "Systems Report Definition  ";
            //    psess.temp2 = "T";
            //    psess.temp7 = " and report_name5=='Y' ";
            //    psess.temp8 = "HREP";
            //    psess.temp9 = "HRREP";
            //}
            //else if (pc == "HA08")
            //{
            //    psess.temp1 = "Systems Report Definition ";
            //    psess.temp2 = "T";
            //    psess.temp7 = " and report_name3=='Y' ";
            //    psess.temp8 = "HAREP";
            //    psess.temp9 = "PFREP";
            //}
            //else if (pc == "F14")
            //{
            //    psess.temp1 = "Systems Report Definition ";
            //    psess.temp2 = "S";
            //    psess.temp7 = " and report_name5=='Y' ";
            //    psess.temp8 = "FREP";
            //    psess.temp9 = "FLREP";
            //}

        }


        private void write_log(string option)
        {
            string opt;
            if (option == "Create")
                opt = "N";
            else if (option == "Delete")
                opt = "D";
            else
                opt = "A";

            string select1 = " where para_code=" + util.pads(tab_calc.para_code) + " and calc_code=" + util.pads(tab_calc.calc_code);
            util.write_plog(tab_calc.para_code, select1, opt, "A", pblock.userid, tab_calc.para_code, tab_calc.calc_code);

        }

    }
}

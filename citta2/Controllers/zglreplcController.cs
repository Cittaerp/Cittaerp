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
    public class zglreplcController : Controller
    {
        //
        // GET: /StdDgn/

        private MainContext db = new MainContext();
        private MainContext db2 = new MainContext();
        Boolean err_flag = true;
        string laction = "";
        vw_genlay mcollect = new vw_genlay();
        tab_array tab_array = new tab_array();
        pubsess pblock;
        psess psess;
        cittautil util = new cittautil();
        string type_code = "";
        string err_message;


        //[EncryptionActionAttribute]
        public ActionResult Index(string pc = null)
        {
            // if (util.check_option() == 1||pc==null)
            //    return RedirectToAction("Welcome", "Home");
            util.init_values();
            psess.temp0 = "31";
            type_code = (string)psess.temp0;


            formula_title(pc);
            psess.temp5 = type_code;
            string rep1t = psess.temp8.ToString();
            var blist = from s in db.tab_array
                        where s.para_code==type_code
                        orderby s.array_code, s.count_array
                        select new vw_genlay { vwstring0 = s.array_code, vwstring1 = s.operand, vwdecimal0 = s.count_array };

            return View(blist.ToList());
        }

        //
        // GET: /Bank/Create

        //[EncryptionActionAttribute]
        public ActionResult Create(string xy = null)
        {
            mcollect.datmode = "C";
            //if (util.check_option() == 1 || xy != "1")
            //    return RedirectToAction("Welcome", "Home");

            pblock = (pubsess)Session["pubsess"];
            type_code = (string)psess.temp0;
//            psess.temp4 = "1";
            init_class();
  
            mcollect.vwstring3 = "D";
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
            type_code = (string)psess.temp0;
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
        public ActionResult Edit(string jy ,string id2)
        {
            mcollect.datmode = "E";
            //if (util.check_option() == 1 || jy == null)
            //    return RedirectToAction("Welcome", "Home");

            pblock = (pubsess)Session["pubsess"];
            type_code = (string)psess.temp0;
            int id3 = Convert.ToInt16(id2);
            tab_array = db.tab_array.Find(type_code, jy,id3);
            if (tab_array == null)
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
            psess = (psess)Session["psess"];
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
            tab_array = db.tab_array.Find(type_code, mcollect.vwcode ,mcollect.vwint0);
            write_log("Delete");
            db.tab_array.Remove(tab_array);
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

            string str1 = "select report_code c0, report_name1 c1 from tab_soft where para_code='LTYPE' order by cast(report_name2 as int)";
            var bgaddc = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.ltype = new SelectList(bgaddc.ToList(), "c0", "c1");

            str1 = "select report_code c0, report_name1 c1 from tab_soft where para_code='LCONTENT' order by cast(report_name2 as int)";
            bgaddc = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.lcontent = new SelectList(bgaddc.ToList(), "c0", "c1");

            var bgmenu = from bg2 in db.tab_soft
                         where bg2.para_code == "RMENU"
                         orderby bg2.report_name4, bg2.report_name3
                         select bg2;

            var bgsort2 = from bg2 in db.GL_001_CATEG
                          where bg2.active_status == "N"
                          select bg2;

            ViewBag.select_cat = new SelectList(bgsort2.ToList(), "acct_cat_sequence", "acct_cat_desc");

            var bgsort3 = from bg2 in db.GL_001_ATYPE
                          where bg2.active_status == "N"
                          select bg2;

            ViewBag.select_type = new SelectList(bgsort3.ToList(), "acct_type_code", "acct_type_desc");

            str1 = "select calc_code c0, name1 c1 from tab_calc where para_code='32' order by 2";
            bgaddc = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.lcalc = new SelectList(bgaddc.ToList(), "c0", "c1",mcollect.vwstring8);



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
                tab_array = new tab_array();
                tab_array.internal_use = "N";
            }
            else
            {
                tab_array = db.tab_array.Find(type_code, mcollect.vwcode,mcollect.vwint0);
            }

            tab_array.para_code = type_code;
            tab_array.array_code = mcollect.vwcode;
            tab_array.count_array = mcollect.vwint0;
            tab_array.operand = mcollect.vwstring0;
            tab_array.source1 = mcollect.vwstring1;
            tab_array.period = mcollect.vwstring2;
            tab_array.operator1 = mcollect.vwstring4;
            tab_array.select1 = mcollect.vwstring5;
            tab_array.sort1 = mcollect.vwstring6;
            tab_array.true_desc = mcollect.vwstring7;
            tab_array.false_desc = mcollect.vwstring3;
            tab_array.amended_by = pblock.userid;
            tab_array.date_amended = odate;
            tab_array.fldr_code = string.IsNullOrWhiteSpace(mcollect.vwstring8) ? "" : mcollect.vwstring8;

            if (laction == "Create")
                db.tab_array.Add(tab_array);
            else
                db.Entry(tab_array).State = EntityState.Modified;

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
            mcollect.vwcode = tab_array.array_code;
            mcollect.vwint0 = Convert.ToInt16(tab_array.count_array);
            mcollect.vwstring0 = tab_array.operand;
            mcollect.vwstring1 = tab_array.source1;
            mcollect.vwstring2 = tab_array.period;
            mcollect.vwstring4 = tab_array.operator1;
            mcollect.vwstring5 = tab_array.select1;
            mcollect.vwstring6 = tab_array.sort1;
            mcollect.vwstring7 = tab_array.true_desc;
            mcollect.vwstring3 = tab_array.false_desc;
            mcollect.vwstring8 = tab_array.fldr_code;

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

            if (string.IsNullOrWhiteSpace(mcollect.vwstring4))
                mcollect.vwstring4 = "";
            if (string.IsNullOrWhiteSpace(mcollect.vwstring6))
                mcollect.vwstring6 = "";
            if (string.IsNullOrWhiteSpace(mcollect.vwstring5))
                mcollect.vwstring5 = mcollect.vwstring4;
            if (string.IsNullOrWhiteSpace(mcollect.vwstring7))
                mcollect.vwstring7 = mcollect.vwstring6;

            if (laction == "Create")
            {
                tab_array cbank = db.tab_array.Find(type_code, mcollect.vwcode,mcollect.vwint0);
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
           
            string str1 = "select c0, c1 from ( ";
            str1 += "select calc_code c0, name1 c1, 'EXCEP'query2 from tab_array where para_code='20' union all ";
            str1 += "select report_code , report_name1, 'TRANS' from tab_soft where para_code='DREP1' union all ";
            str1 += "select report_code , report_name1, 'WKORD' from tab_soft where para_code='WKREP' union all ";
            str1 += "select report_code , report_name1, 'ANNV' from tab_soft where para_code='HREP2' union all ";
            str1 += "select train_code , course_name, 'BALAN' from tab_train where para_code='H24' union all ";
            str1 += "select doc_code , name1, 'PDOC' from tab_document where doc_type='P' union all ";
            str1 += "select doc_code , name1, 'HDOC' from tab_document where doc_type='H' union all ";
            str1 += "select report_code, report_name1, 'AUDIT' from tab_soft where para_code='HAUD' union all ";
            str1 += "select trans_type, name1, 'TRANSBASE' from tab_type union all ";
            str1 += "select report_code , report_name1, 'TRANSBASE' from tab_soft where para_code='CADV' ) b ";
            str1 += " where b.query2=" + util.pads(Code);
            str1 += " order by 2 ";
            var bgcol7 = db.Database.SqlQuery<vw_query>(str1);
            
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                bgcol7.ToArray(),
                                "c0",
                                "c1")
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

        private void write_temp(string action, string daily_code)
        {
            int ctr1;

            if (action == "R")
            {

                string sortkey = type_code + "S";
                var bgsort = from bgl in db.tab_array
                             where bgl.para_code == sortkey && bgl.array_code == daily_code
                             select bgl;

                foreach (var bgl in bgsort)
                {
                    ctr1 = Convert.ToInt16(bgl.count_array)-1;
                    mcollect.vwstrarray0[ctr1] = bgl.sort1;
                    mcollect.vwstrarray2[ctr1] = bgl.select1;
                    mcollect.vwblarray0[ctr1] = bgl.operand == "Y" ? true : false;
                    mcollect.vwblarray1[ctr1] = bgl.source1 == "Y" ? true : false;
                    mcollect.vwblarray2[ctr1] = bgl.period == "Y" ? true : false;
                }

                sortkey = type_code + "C";
                bgsort = from bgl in db.tab_array
                             where bgl.para_code == sortkey && bgl.array_code == daily_code
                             select bgl;

                foreach (var bgl in bgsort)
                {
                    ctr1 = Convert.ToInt16(bgl.count_array)-1;
                    mcollect.vwstrarray6[ctr1] = bgl.operand;
                }

            }

        }



        private void formula_title(string pc)
        {
                psess.temp1 = "Financial Report Line Content Definition ";
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

            string select1 = " where para_code=" + util.pads(tab_array.para_code) + " and calc_code=" + util.pads(tab_array.array_code);
            //util.write_plog(tab_array.para_code, select1, opt, "A", pblock.userid, tab_array.para_code, tab_array.array_code);

        }

    }
}

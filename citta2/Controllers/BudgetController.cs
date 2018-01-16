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

    public class BudgetController : Controller
    {
        //
        // GET: /Employee/

        BU_002_BUGTD BU_002_BUGTD = new BU_002_BUGTD();
        BU_002_BUGTH gdoc = new BU_002_BUGTH();
        MainContext db = new MainContext();
        MainContext db1 = new MainContext();
        vw_genlay glay = new vw_genlay();
        queryhead qheader = new queryhead();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string preseq = "";
        string ptype = "";
        decimal f = 0;
        decimal y = 0;
        bool err_flag = true;
        decimal qty = 0; decimal pricep = 0; decimal tax_amt = 0; decimal net_amt = 0; decimal quote_amt = 0; decimal ext_price = 0;
        decimal net_amt1 = 0; decimal quote_amt1 = 0; decimal qtx = 0;
        decimal taxm = 0; decimal disc_per1 = 0;
        string action_flag = "";

        [EncryptionActionAttribute]
        public ActionResult Index()
        {

            util.init_values();

            //Session["hbudg"] = "";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            //Session["curren_name"] = pubsess.base_currency_description;
            var bglist = from bh in db.BU_002_BUGTH
                         join bj in db.GB_999_MSG
                            on new { a1 = bh.period, a2 = "FYM" } equals new { a1 = bj.code_msg, a2 = bj.type_msg }
                            into bj1
                         from bj2 in bj1.DefaultIfEmpty()
                         where bh.approval_level == 0
                         select new vw_genlay
                         {
                             vwint0 = bh.budget_journal_number,
                             vwstring0 = bh.budget_reference_number,
                             vwdecimal0 = bh.total_debit,
                             vwdecimal1 = bh.total_credit,
                             vwstring1 = bh.batch_description,
                             vwstring2 = bh.control_flag == "" ? "No Entry" : bh.control == 0 ? "Yes" : "No",
                             vwstring3 = "Open",
                             vwstring4 = bj2.name1_msg != null ? bj2.name1_msg + " " + bh.transaction_date : bh.period + " " + bh.transaction_date,
                             vwstring5 = bh.created_by
                         };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult CreateHeader()
        {
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            //psess.temp4 = "";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            
            select_query_head("H");
            glay.vwstring8 = "H";

            show_screen_info();

            return View(glay);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHeader(vw_genlay glay_in, HttpPostedFileBase[] photofile, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            glay = glay_in;
            photo1 = photofile;

            update_file(headtype);

            if (err_flag)
            {                    
                    return RedirectToAction("CreateDetails");
            }

            select_query_head("H");
            
            glay.vwstring8 = "H";
            glay.vwstrarray2 = new string[20];
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditHeader()
        {
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            pubsess = (pubsess)Session["pubsess"];
            qheader = (queryhead)Session["qheader"];
            psess = (psess)Session["psess"];
            initial_rtn();
            
            glay.vwstring8 = "H";
            //glay.vwstring9 = Session["hbudg"].ToString();
            gdoc = db.BU_002_BUGTH.Find(qheader.intquery0);
            if (gdoc != null)
                read_header();
            select_query_head("H");

            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHeader(vw_genlay glay_in,  HttpPostedFileBase[] photofile, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            qheader = (queryhead)Session["qheader"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            glay = glay_in;
            // glay.vwstring9 = Session["hbudg"].ToString();
             photo1 = photofile;
            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head("H");

            glay.vwstring8 = "H";
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult CreateDetails(string headtype="D")
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            pubsess = (pubsess)Session["pubsess"];
            qheader = (queryhead)Session["qheader"];
            psess = (psess)Session["psess"];
            initial_rtn();
                header_ana("D");
            select_query_head("D");
            glay.vwstring8 = headtype;
           // glay.vwstring9 = Session["hbudg"].ToString();
            detail_intial();
            glay.vwstrarray2[0] = pubsess.base_currency_description;
            glay.vwstring9 = pubsess.exchange_editable;
            // glay.vwstring6 = qheader.query0;
            //glay.vwstring5 = pubsess.base_currency_code;

            show_screen_info();

            return View(glay);
        }

        private void detail_intial()
        {
            glay.vwstrarray2 = new string[20];
            glay.vwstring6 = qheader.query0;
            glay.vwstring5 = pubsess.base_currency_code;
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
            glay.vwstring9 = pubsess.price_editable;
            //glay.vwstrarray2[0] = qheader.query6;
            //glay.vwstrarray2[2] = "N";
            //glay.vwstrarray0[0] = qheader.query1;
            // Session["curren_name"] = pubsess.base_currency_description;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetails(vw_genlay glay_in, string headtype = "D")
        {
            pubsess = (pubsess)Session["pubsess"];
            qheader = (queryhead)Session["qheader"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            glay = glay_in;
           // glay.vwstring9 = Session["hbudg"].ToString();
            if (headtype == "send_app")
            {
                err_flag=validation_routine_send(headtype);
                if (err_flag)
                {
                    util.update_entry("011", qheader.intquery0.ToString(), pubsess.userid);
                    string str = "update BU_002_BUGTH set approval_level=99 where budget_journal_number=" + qheader.intquery0;
                    db.Database.ExecuteSqlCommand(str);

                    return RedirectToAction("CreateHeader");
                }
            }

            if (err_flag)
            {
                update_file(headtype);

                if (err_flag)
                {
                    return RedirectToAction("CreateDetails");
                }
            }
            glay.vwlist0 = new List<querylay>[20];
            header_ana("D");
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray2 = new string[20];
            show_screen_info();
            select_query_head();
            cur_read();
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult Edit(int key1)
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            

            gdoc = db.BU_002_BUGTH.Find(key1);
            if (gdoc != null)
            {
               // Session["hbudg"] = gdoc.budget_journal_number;
               // Session["ref_def"] = gdoc.budget_reference_number;
               // psess.temp4 = "";
                set_qheader();
               return RedirectToAction("CreateDetails",  new {headtype="D"});

            }

            return View(glay);

        }

        private void set_qheader()
        {
            queryhead qheader = new queryhead();
            qheader.intquery0 = gdoc.budget_journal_number;
            qheader.query1 = gdoc.period;
            qheader.query0 = gdoc.budget_reference_number;
            qheader.query3 = gdoc.transaction_date;
            qheader.query4 = gdoc.batch_description;
            Session["qheader"] = qheader;


        }

        //[HttpPost]
        //public ActionResult Edit(vw_genlay glay_in, string id_xhrt, string headtype)
        //{
        //    pubsess = (pubsess)Session["pubsess"];
        //    glay = glay_in;

        //    if (id_xhrt == "D")
        //    {
        //        delete_record();
        //        return RedirectToAction("Index");
        //    }
        //    update_file(headtype);
        //    if (err_flag)
        //        return RedirectToAction("Index");

        //    select_query_head();
        //    return View(glay);
        //}
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails(int key1, int key2 )
        {
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            pubsess = (pubsess)Session["pubsess"];
            qheader = (queryhead)Session["qheader"];
            psess = (psess)Session["psess"];
            glay.vwstring9 = pubsess.exchange_editable;
        
            BU_002_BUGTD = db.BU_002_BUGTD.Find(qheader.intquery0, key2);
            if (BU_002_BUGTD == null)
                return View(glay);
            //Session["hbudg"] = BU_002_BUGTD.budget_journal_number;
            //Session["line_num"] = BU_002_BUGTD.sequence_number;

            initial_rtn();
            detail_intial();
            move_detail();
            header_ana("D");
            select_query_head();
            glay.vwstring8 = "D";

            show_screen_info();

            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails(vw_genlay glay_in, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            qheader = (queryhead)Session["qheader"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            glay = glay_in;
            //glay.vwstring9 = Session["hbudg"].ToString();
            glay.vwdecimal4 = Convert.ToDecimal(Session["line_num"]);
            if (headtype == "send_app")
            {
                err_flag = validation_routine_send(headtype);
                if (err_flag)
                {
                    util.update_entry("011", qheader.intquery0.ToString(), pubsess.userid);
                    string str = "update BU_002_BUGTH set approval_level=99 where budget_journal_number=" + qheader.intquery0;
                    db.Database.ExecuteSqlCommand(str);

                    return RedirectToAction("CreateHeader");
                }
                else
                {
                    select_query_head("D");
                    glay.vwlist0 = new List<querylay>[20];
            
                    glay.vwstring8 = "H";
                    detail_intial();
                    show_screen_info();
                    header_ana();
                    return View(glay);
                }
            }
          
            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }
            glay.vwlist0 = new List<querylay>[20];
            select_query_head();

            glay.vwstring8 = "H";
            detail_intial();
            show_screen_info();
            header_ana();
            return View(glay);

        }
        private void delete_record()
        {
            BU_002_BUGTD = db.BU_002_BUGTD.Find(glay.vwint0, glay.vwint1);
            if (BU_002_BUGTD != null)
            {
                db.BU_002_BUGTD.Remove(BU_002_BUGTD);
                db.SaveChanges();
            }
        }
        private void update_file( string headtype)
        {
            err_flag = true;
            validation_routine(headtype);

            if (err_flag)
                update_record();

        }

        private void update_record()
        {
            string cr_flag = action_flag;

            if (cr_flag.IndexOf("Header") > 0)
            {
                if (cr_flag == "CreateHeader")
                {
                    gdoc = new BU_002_BUGTH();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;
                    gdoc.approval_level = 0;
                    gdoc.approval_date = DateTime.UtcNow;
                    //gdoc.budget_journal_number = glay.vwint0;
                    
                }
                else
                {
                    gdoc = db.BU_002_BUGTH.Find(qheader.intquery0);
                }
                gdoc.budget_reference_number = glay.vwstring0;
                gdoc.period = glay.vwstrarray0[1] + glay.vwstrarray0[0];
                //gdoc.period = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : (glay.vwstrarray0[0]);
                gdoc.transaction_date = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : (glay.vwstrarray0[1]);
                gdoc.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : (glay.vwstrarray0[3]);
                gdoc.batch_description = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : (glay.vwstrarray0[2]);
                gdoc.control_flag = "";
                gdoc.modified_by = pubsess.userid;
                gdoc.modified_date = DateTime.UtcNow;

                if (cr_flag == "CreateHeader")
                    db.Entry(gdoc).State = EntityState.Added;

                else
                    db.Entry(gdoc).State = EntityState.Modified;
            }

            else
            {

                if (cr_flag == "CreateDetails")
                {

                    BU_002_BUGTD = new BU_002_BUGTD();
                    BU_002_BUGTD.created_by = pubsess.userid;
                    BU_002_BUGTD.created_date = DateTime.UtcNow;
                    glay.vwint4 = get_next_line_sequence();             
                }
                else
                {
                    BU_002_BUGTD = db.BU_002_BUGTD.Find(qheader.intquery0, glay.vwint4);
                }


                //the detailsub button has been clicked

                BU_002_BUGTD.budget_journal_number = qheader.intquery0;
                BU_002_BUGTD.sequence_number = glay.vwint4;
                BU_002_BUGTD.reference_number = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : (glay.vwstring6);
                BU_002_BUGTD.description = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : (glay.vwstring3);
                BU_002_BUGTD.amount = glay.vwdecimal0;

                BU_002_BUGTD.amount_type = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : (glay.vwstring4);
                BU_002_BUGTD.account_type_debit = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : (glay.vwstring1);
                BU_002_BUGTD.account_code_debit = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : (glay.vwstring2);

                BU_002_BUGTD.currency_code = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : (glay.vwstring5);
                BU_002_BUGTD.exchange_rate = glay.vwdecimal5;
                BU_002_BUGTD.base_amount = glay.vwdecimal5 * glay.vwdecimal0;
                BU_002_BUGTD.modified_date = DateTime.UtcNow;
                BU_002_BUGTD.modified_by = pubsess.userid;

                BU_002_BUGTD.analysis_code1 = "";
                BU_002_BUGTD.analysis_code2 = "";
                BU_002_BUGTD.analysis_code3 = "";
                BU_002_BUGTD.analysis_code4 = "";
                BU_002_BUGTD.analysis_code5 = "";
                BU_002_BUGTD.analysis_code6 = "";
                BU_002_BUGTD.analysis_code7 = "";
                BU_002_BUGTD.analysis_code8 = "";
                BU_002_BUGTD.analysis_code9 = "";
                BU_002_BUGTD.analysis_code10 = "";


                if (pubsess.base_currency_code== BU_002_BUGTD.currency_code)
                {
                    BU_002_BUGTD.exchange_rate = 1;
                    BU_002_BUGTD.base_amount = BU_002_BUGTD.amount;
                }
                else
                {
                    if (glay.vwstring9 != "Y")
                    {
                        string curent_date = DateTime.Now.ToString("yyyyMMdd");
                        string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(BU_002_BUGTD.currency_code) + " and '" + curent_date + "' between date_from and date_to";
                        var exch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                        if (exch != null)
                            BU_002_BUGTD.exchange_rate = exch.dquery0;
                    }
                }
                //Session["ref_def"] = BU_002_BUGTD.reference_number;
                if (glay.vwstrarray6 != null)
                {
                    int arrlen = glay.vwstrarray6.Length;
                    if (arrlen > 0)
                        BU_002_BUGTD.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                    if (arrlen > 1)
                        BU_002_BUGTD.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                    if (arrlen > 2)
                        BU_002_BUGTD.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                    if (arrlen > 3)
                        BU_002_BUGTD.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                    if (arrlen > 4)
                        BU_002_BUGTD.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                    if (arrlen > 5)
                        BU_002_BUGTD.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                    if (arrlen > 6)
                        BU_002_BUGTD.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                    if (arrlen > 7)
                        BU_002_BUGTD.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                    if (arrlen > 8)
                        BU_002_BUGTD.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                    if (arrlen > 9)
                        BU_002_BUGTD.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                    psess.intemp0 = arrlen;
                    Session["psess"] = psess;
                }

                if (cr_flag == "CreateDetails")

                    db.Entry(BU_002_BUGTD).State = EntityState.Added;
                else
                    db.Entry(BU_002_BUGTD).State = EntityState.Modified;


            }

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

            if (cr_flag == "CreateHeader")
            {
                //Session["hbudg"] = gdoc.budget_journal_number;
                //Session["ref_def"] = gdoc.budget_reference_number;
                }
            if (err_flag && cr_flag.IndexOf("Header") < 0)
            {
                qheader.query0 = BU_002_BUGTD.reference_number;
               // qheader.query3 = util.date_slash(BU_002_BUGTD.transaction_date);
                Session["qheader"] = qheader;
            
                amount_update();
                
                string sp = "update BU_002_BUGTH set control_flag = 'Y' where budget_journal_number =" + glay.vwint0;
                db.Database.ExecuteSqlCommand(sp);
                if (BU_002_BUGTD.account_type_debit == "001")
                {
                    string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, BU_002_BUGTD b where a.account_code = b.account_code_debit";
                    str += " and budget_journal_number =" + glay.vwint0;
                    //db.Database.ExecuteSqlCommand(str);
                    //string jhn = BU_002_BUGTD.account_code_debit;
                    //string str = "update GL_001_CHART set delete_flag ='Y' where account_code = " + util.sqlquote(jhn);
                    //db.Database.ExecuteSqlCommand(str);

                }
               string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, BU_002_BUGTD b where header_sequence in (analysis_code1";
                stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                stri += " and budget_journal_number =" + glay.vwint0;
                //db.Database.ExecuteSqlCommand(stri);
              
            }     

            if (err_flag && cr_flag.IndexOf("Header") > 0)
            {
                set_qheader();
                util.write_document("BUDGET", gdoc.budget_journal_number.ToString(), photo1, glay.vwstrarray9);
            }

        }
        private int get_next_line_sequence()
        {
            string sqlstr = "select isnull(max(sequence_number),0) vwint1 from BU_002_BUGTD where reference_number=" + util.sqlquote(glay.vwstring6);
            var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
            return sql1.vwint1 + 1;

        }
    
        private void validation_routine(string headtype)
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            
            string cr_flag = action_flag;
            if (cr_flag.IndexOf("Detail") > 0)
            {

                if (string.IsNullOrWhiteSpace(glay.vwstring6))
                {
                    ModelState.AddModelError(String.Empty, "Enter Reference Number");
                    err_flag = false;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstring3))
                {
                    ModelState.AddModelError(String.Empty, "Enter Description");
                    err_flag = false;
                } 
                if (string.IsNullOrWhiteSpace(glay.vwstring1))
                {
                    ModelState.AddModelError(String.Empty, "Enter Posting Class");
                    err_flag = false;
                }
                if (!string.IsNullOrWhiteSpace(glay.vwstring1) && string.IsNullOrWhiteSpace(glay.vwstring2))
                {
                    ModelState.AddModelError(String.Empty, "Enter Posting Code");
                    err_flag = false;
                }
                if (glay.vwdecimal1 == 0)
                {
                    ModelState.AddModelError(String.Empty, "Exchange Rate can not be Zero");
                    err_flag = false;
                }
                if (glay.vwdecimal0 == 0)
                {
                    ModelState.AddModelError(String.Empty, "Enter an Amount");
                    err_flag = false;
                }
                if (glay.vwstring4 == null)
                {
                    ModelState.AddModelError(String.Empty, "Specify Debit/Credit for Amount");
                    err_flag = false;
                }
              
                for (int count1 = 0; count1 < 10; count1++)
                {
                    if (aheader7[count1] == "Y" && string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
                    {
                        error_msg = aheader5[count1] + " is mandatory. ";
                        ModelState.AddModelError(String.Empty, error_msg);
                        err_flag = false;
                    }
                }
            }
            if (headtype == "send_app")
            {
                initial_rtn();
                decimal contrl_check = (from bg in db.BU_002_BUGTH
                                        where bg.budget_journal_number == qheader.intquery0
                                       select bg.control).FirstOrDefault();
                glay.vwstrarray0[5] = contrl_check.ToString();
                if (glay.vwstrarray0[5] != "0.00")
                {
                    ModelState.AddModelError(String.Empty, "Total Credit must be equal to Total Debit");
                    err_flag = false;
                }
                else
                    posted();
            }

        }

        private Boolean validation_routine_send(string headtype)
        {
            err_flag = true;
            //string error_msg = "";
                initial_rtn();
                decimal contrl_check = (from bg in db.BU_002_BUGTH
                                        where bg.budget_journal_number == qheader.intquery0
                                        select bg.control).FirstOrDefault();
                glay.vwstrarray0[5] = contrl_check.ToString();
                if (glay.vwstrarray0[5] != "0.00")
                {
                    ModelState.AddModelError(String.Empty, "Total Credit must be equal to Total Debit");
                    err_flag = false;
                }
                else
                    posted();

                return err_flag;
        }

        private void posted()
        {
            qheader = (queryhead)Session["qheader"];

            glay.vwstrarray6 = new string[20];
             var bglist = from bh in db.BU_002_BUGTD
                         where bh.budget_journal_number == qheader.intquery0
                         select new vw_genlay

                         {
                            
                            vwdecimal4 =  bh.sequence_number,
                            vwstring1 = bh.account_type_debit,
                            vwstring2 = bh.account_code_debit,
                            vwstring3 = bh.description,
                           vwstring4 = bh.amount_type,
                            vwstring5 = bh.currency_code,
                           vwstring6 = bh.reference_number,
                            vwdecimal0 = bh.amount,
                            vwdecimal3 = bh.base_amount,
                            vwdecimal5 = bh.exchange_rate,
                           // vwstrarray6[0] =  bh.analysis_code1,
                           // vwstrarray6[1] = bh.analysis_code2,
                           // vwstrarray6[2] = bh.analysis_code3,
                           // vwstrarray6[3] = bh.analysis_code4,
                           // vwstrarray6[4] = bh.analysis_code5,
                           //vwstrarray6[5] = bh.analysis_code6,
                           // vwstrarray6[6] = bh.analysis_code7,
                           //vwstrarray6[7] = bh.analysis_code8,
                           // vwstrarray6[8] = bh.analysis_code9,
                           //vwstrarray6[9] = bh.analysis_code10,
                     };
            var xodr = bglist.ToList();
            foreach (var item in xodr)
            {
                BU_002_BUGTR BU_002_BUGTR = new BU_002_BUGTR();
                BU_002_BUGTR.created_by = pubsess.userid;
                BU_002_BUGTR.created_date = DateTime.UtcNow;
                BU_002_BUGTR.budget_journal_number = qheader.intquery0;
                BU_002_BUGTR.sequence_number = item.vwint4;
                BU_002_BUGTR.reference_number = string.IsNullOrWhiteSpace(item.vwstring6) ? "" : (item.vwstring6);
                BU_002_BUGTR.description = string.IsNullOrWhiteSpace(item.vwstring3) ? "" : (item.vwstring3);
                BU_002_BUGTR.amount = item.vwdecimal0;
                BU_002_BUGTR.period = qheader.query1;
                BU_002_BUGTR.amount_type = string.IsNullOrWhiteSpace(item.vwstring4) ? "" : (item.vwstring4);
                BU_002_BUGTR.account_type_debit = string.IsNullOrWhiteSpace(item.vwstring1) ? "" : (item.vwstring1);
                BU_002_BUGTR.account_code_debit = string.IsNullOrWhiteSpace(item.vwstring2) ? "" : (item.vwstring2);
                BU_002_BUGTR.year = qheader.query3;
                BU_002_BUGTR.currency_code = string.IsNullOrWhiteSpace(item.vwstring5) ? "" : (item.vwstring5);
                BU_002_BUGTR.exchange_rate = item.vwdecimal5;
                BU_002_BUGTR.base_amount = item.vwdecimal5 * item.vwdecimal0;
                BU_002_BUGTR.modified_date = DateTime.UtcNow;
                BU_002_BUGTR.modified_by = pubsess.userid;
                BU_002_BUGTR.batch_description = qheader.query4;
                BU_002_BUGTR.analysis_code1 = "";
                BU_002_BUGTR.analysis_code2 = "";
                BU_002_BUGTR.analysis_code3 = "";
                BU_002_BUGTR.analysis_code4 = "";
                BU_002_BUGTR.analysis_code5 = "";
                BU_002_BUGTR.analysis_code6 = "";
                BU_002_BUGTR.analysis_code7 = "";
                BU_002_BUGTR.analysis_code8 = "";
                BU_002_BUGTR.analysis_code9 = "";
                BU_002_BUGTR.analysis_code10 = "";
                BU_002_BUGTR.approval_level = 0;
                BU_002_BUGTR.approval_by = "";
                db.Entry(BU_002_BUGTR).State = EntityState.Added;

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

                string idd = qheader.intquery0.ToString() + "[]" + glay.vwint4.ToString();
                
       

            }
            string id = qheader.intquery0.ToString();
            delete_list(id);

        }

        public ActionResult PostIndex()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            var bglist = from bh in db.BU_002_BUGTR
                         join bg in db.MC_001_CUREN
                         on new { a1 = bh.currency_code } equals new { a1 = bg.currency_code }
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join bk in db.GL_001_CHART
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bk.account_code, a2 = "003" }
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         join bl in db.AR_001_CUSTM
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bl.customer_code, a2 = "001" }
                         into bl1
                         from bl2 in bl1.DefaultIfEmpty()
                         join bm in db.AP_001_VENDR
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bm.vendor_code, a2 = "002" }
                         into bm1
                         from bm2 in bm1.DefaultIfEmpty()
                         join bn in db.BK_001_BANK
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bn.bank_code, a2 = "004" }
                         into bn1
                         from bn2 in bn1.DefaultIfEmpty()
                         join bo in db.GB_001_RSONC
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bo.consignment_code, a2 = "012" }
                         into bo1
                         from bo2 in bo1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwint0 = bh.budget_journal_number,
                             vwint1 = bh.sequence_number,
                             vwstring4 = bh.account_type_debit,
                             vwstring3 = bh.account_type_debit == "003" ? bk2.account_name : bh.account_type_debit == "001" ? bl2.cust_biz_name : bh.account_type_debit == "002" ? bm2.vend_biz_name : bh.account_type_debit == "004" ? bn2.bank_name : bo2.description,
                             vwstring2 = bh.description,
                             vwstring1 = bh.reference_number,
                             vwdecimal0 = bh.amount,
                             vwdecimal1 = bh.base_amount,
                             vwstring5 = bg2.currency_description,
                             vwstring6 = bh.amount_type,
                             vwstring7 = bh.period + " "+ bh.year,
                             vwstring8 = bh.batch_description,
                             vwstring0 = bh.account_type_debit == "003" ? bk2.account_code : bh.account_type_debit == "001" ? bl2.customer_code : bh.account_type_debit == "002" ? bm2.vendor_code : bh.account_type_debit == "004" ? bn2.bank_code : bo2.consignment_code
                         };

            return View(bglist.ToList());
            
        }
     
        private void select_query_head(string type1="D")
        {
            if (type1 == "H")
            {
                year_cal();
                if (pubsess.period_closing == "C")
                {
                    var empu = from pf in db.GB_999_MSG
                               where pf.type_msg == "FYM"
                               //orderby pf.name1_msg
                               select pf;
                    ViewBag.month = new SelectList(empu.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[0]);
                }
                else
                {
                    var empu = from pf in db.GB_001_PCT
                               orderby pf.period_number
                               select pf;
                    ViewBag.month = new SelectList(empu.ToList(), "period_number", "prd_description", glay.vwstrarray0[0]);
                }

                //if (pubsess.period_closing == "C")
                //{
                //    string disckf = pubsess.valid_datefrm.Substring(3, 2);
                //    string disckt = pubsess.valid_dateto.Substring(3, 2);

                //    string srt = " select code_msg query0, name1_msg query1 from  dbo.GB_999_MSG pf where pf.type_msg = 'FYM'and  code_msg =" + util.sqlquote(pubsess.curent_mth) + "union all";
                //    srt += " select code_msg query0, name1_msg query1  from dbo.GB_999_MSG pg where pg.type_msg = 'FYM' and code_msg between" + util.sqlquote(disckf) + " and" + util.sqlquote(disckt) + "order by pf.code_msg";
                //    var bf2 = db.Database.SqlQuery<querylay>(srt).ToList();
                //    ViewBag.month = new SelectList(bf2.ToList(), "query0", "query1", glay.vwstrarray0[0]);
                
                //}
                //else
                //{
                //    var empu = from pf in db.GB_001_PCT
                //               orderby pf.period_number
                //               select pf;
                //    ViewBag.month = new SelectList(empu.ToList(), "period_number", "prd_description", glay.vwstrarray0[0]);
                //}
            }
            if (type1 == "D")
            {
                string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
                var emp1 = db.Database.SqlQuery<querylay>(str1);

                ViewBag.cur = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring5);

                var empi = from pf in db.GB_999_MSG
                           where pf.type_msg == "HEAD" && pf.name6_msg == "P"
                           orderby pf.name1_msg
                           select pf;
                ViewBag.debit = new SelectList(empi.ToList(), "code_msg", "name1_msg", glay.vwstring1);

                 str1 = "select bank_code query0, c2 query1 from vw_allcust where qcode=" + util.sqlquote(glay.vwstring1);
                var hdet = db.Database.SqlQuery<querylay>(str1);
                ViewBag.code = new SelectList(hdet.ToList(), "query0", "query1", glay.vwstring2);

                //if (glay.vwstring1 == "003")
                //{
                //    string sqlstr = "select account_code query0, account_name query1, account_code+ '---'+ account_name as query2 from GL_001_CHART where account_type_code in (select acct_type1 from GL_001_GLDS where gl_default_id = '002'union all ";
                //    sqlstr += " select acct_type2 from GL_001_GLDS where gl_default_id = '002'union all select acct_type3 from GL_001_GLDS where gl_default_id = '002'union all ";
                //    sqlstr += " select acct_type4 from GL_001_GLDS where gl_default_id = '002'union all select acct_type5 from GL_001_GLDS where gl_default_id = '002') order by account_name";
                //    var bg2 = db.Database.SqlQuery<querylay>(sqlstr);
                //    ViewBag.code = new SelectList(bg2.ToList(), "query0", "query2", glay.vwstrarray0[17]);
                //}
                //else if (glay.vwstring1 == "001")
                //{
                //    var bat = from bg in db.AR_001_CUSTM
                //              where bg.active_status == "N"
                //              orderby bg.cust_biz_name
                //              select bg;
                //    ViewBag.code = new SelectList(bat.ToList(), "customer_code", "cust_biz_name", glay.vwstring2);
                //}
                //else
                //{
                //    ViewBag.code = new SelectList("", "", glay.vwstring2);
                //}

            }
        }

        private void year_cal()
        {
            int thisyear = DateTime.Now.Year;
            int startyear = thisyear - 10;
            int endyear = thisyear + 11;
            List<SelectListItem> ary = new List<SelectListItem>();
            for (int yctr = startyear; yctr < endyear; yctr++)
            {
                ary.Add(new SelectListItem { Value = yctr.ToString(), Text = yctr.ToString() });
            }

            ViewBag.year = ary;

        }

        private void initial_rtn()
        {
            // string hcount = Session["hbudg"].ToString();
             glay.vwdclarray0 = new decimal[50];
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray1 = new string[20];
            glay.vwstrarray2 = new string[20];
            glay.vwdecimal5 = 1;
            //string ref_def = (from bg in db.BU_002_BUGTH
            //                  where bg.budget_journal_number == hbug
            //               select bg.budget_reference_number).FirstOrDefault();
            //glay.vwstring6 = ref_def;
            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwstrarray0[wctr] = "";
            glay.vwlist0 = new List<querylay>[20];


        }

        private void cur_read()
        {
            string curcode = "";
            curcode = glay.vwstring5;
            string read_cur = (from bg in db.MC_001_CUREN
                               where bg.currency_code == curcode
                               select bg.currency_description).FirstOrDefault();
            glay.vwstrarray2[0] = read_cur;
          
            // Session["curren_name"] = read_cur;
        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            //int idd = Convert.ToInt16(id);
            //var flagupdate = (from bd in db.BU_002_BUGTD
            //                  where bd.budget_journal_number == idd
            //                  select bd);
            //foreach (var item in flagupdate)
            //{
                
            string sqlstr = "delete from [dbo].[BU_002_BUGTH] where budget_journal_number =" + id;
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "delete from [dbo].[BU_002_BUGTD] where budget_journal_number =" + id;
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult delete_detail(string id)
        {
            // write your query statement

            
          //  glay.vwstring9 = Session["hbudg"].ToString();
            string sqlstr = "delete from [dbo].[BU_002_BUGTD] where cast(budget_journal_number as varchar) +'[]'+ cast(sequence_number as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            amount_update();
            return RedirectToAction("CreateDetails");
            
        }

        private void read_details()
        {
           
            //string hcount = Session["hbudg"].ToString();
            //int hbug = 0;
            //int.TryParse(hcount, out hbug);
                var bglist = from bh in db.BU_002_BUGTD
                             join bg in db.MC_001_CUREN
                             on new { a1 = bh.currency_code } equals new { a1 = bg.currency_code }
                             into bg1
                             from bg2 in bg1.DefaultIfEmpty()
                             join bk in db.GL_001_CHART
                             on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bk.account_code, a2 = "003" }
                             into bk1
                             from bk2 in bk1.DefaultIfEmpty()
                             join bl in db.AR_001_CUSTM
                             on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bl.customer_code, a2 = "001" }
                             into bl1
                             from bl2 in bl1.DefaultIfEmpty()
                             join bm in db.AP_001_VENDR
                             on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bm.vendor_code, a2 = "002" }
                             into bm1
                             from bm2 in bm1.DefaultIfEmpty()
                             join bn in db.BK_001_BANK
                             on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bn.bank_code, a2 = "004" }
                             into bn1
                             from bn2 in bn1.DefaultIfEmpty()
                             join bo in db.GB_001_RSONC
                             on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bo.consignment_code, a2 = "012" }
                             into bo1
                             from bo2 in bo1.DefaultIfEmpty()
                             where bh.budget_journal_number == qheader.intquery0
                             select new vw_genlay
                             {
                                 vwint0 = bh.budget_journal_number,
                                 vwint1 = bh.sequence_number,
                                 vwstring4 = bh.account_type_debit,
                                 vwstring3 = bh.account_type_debit=="003"?bk2.account_name:bh.account_type_debit=="001"?bl2.cust_biz_name:bh.account_type_debit=="002"?bm2.vend_biz_name:bh.account_type_debit=="004"?bn2.bank_name:bo2.description,
                                 vwstring2 = bh.description,
                                 vwstring1 = bh.reference_number,
                                 vwdecimal0 = bh.amount,
                                 vwdecimal1 = bh.base_amount,
                                 vwstring5 = bg2.currency_description,
                                 vwstring6 = bh.amount_type,
                                 vwstring0 = bh.account_type_debit=="003"?bk2.account_code:bh.account_type_debit=="001"?bl2.customer_code:bh.account_type_debit=="002"?bm2.vendor_code:bh.account_type_debit=="004"?bn2.bank_code:bo2.consignment_code
                             };

                ViewBag.x1 = bglist.ToList();
            }
        
        private void re_cal(string item_amt, string tax_code, string tax_amt, string rel_tax)
        {
            string pricep = tax_code;

            var bgtax = (from bg in db.GB_001_TAX
                         where bg.tax_code == pricep
                         select bg).FirstOrDefault();
            decimal tax = 0;
            decimal.TryParse(item_amt, out qty);
            decimal.TryParse(tax_amt, out taxm);

            tax = bgtax.tax_rate / 100;
            taxm = tax * qty;
            if (bgtax.tax_impact == "S")
                taxm = 0 - taxm;

            
            

        }

         private decimal tax_calculation(string icode, decimal amount)
        {
            var bgtax = (from bg in db.GB_001_TAX
                         where bg.tax_code == icode
                         select bg).FirstOrDefault();
            if (bgtax == null)
                return 0;
            if (bgtax.module_basis == "P")
                return 0;
            if (bgtax.computation_basis == "T")
                return 0;
            decimal tax = 0;
            tax = amount * bgtax.tax_rate / 100;
            if (bgtax.tax_impact == "S")
                tax = 0 - tax;
            return tax;

            
        }
         private void show_screen_info()
         {
             display_header();
             read_details();

         }
        private void read_header()
        {
            //string hcount = Session["hbudg"].ToString();
            //int hbug = 0;
            //int.TryParse(hcount, out hbug);
            var bg2list = (from bh in db.BU_002_BUGTH
                           where bh.budget_journal_number == qheader.intquery0
                           select bh).FirstOrDefault();


            vw_genlay glayhead = new vw_genlay();
            if (bg2list != null)
            {
                glay.vwstrarray0 = new string[20];

                glay.vwint0 = bg2list.budget_journal_number;
                glay.vwstring0 = bg2list.budget_reference_number;
                glay.vwstrarray0[0] = bg2list.period.Substring(4, 2);
                glay.vwstrarray0[1] = bg2list.period.Substring(0, 4);
                //glay.vwstrarray0[0] = bg2list.period;
                //glay.vwstrarray0[1] = bg2list.transaction_date;
                glay.vwstrarray0[3] = bg2list.note;
                glay.vwstrarray0[2] = bg2list.batch_description;
                glay.vwstrarray0[4] = bg2list.total_debit.ToString("#,##0.00");
                glay.vwstrarray0[5] = bg2list.total_credit.ToString("#,##0.00");
                glay.vwstrarray0[6] = bg2list.control.ToString("#,##0.00");
                string docode = gdoc.budget_journal_number.ToString();
                var bglist = from bg in db.GB_001_DOC
                             where bg.screen_code == "BUDGET" && bg.document_code == docode
                             orderby bg.document_sequence
                             select bg;

                ViewBag.anapict = bglist.ToList();

                
            }
            ViewBag.x2 = glayhead;


        }

        private void display_header()
        {
            //string hcount = Session["hbudg"].ToString();
            //int hbug = 0;
            //int.TryParse(hcount, out hbug);
            var bg2list = (from bh in db.BU_002_BUGTH
                           where bh.budget_journal_number == qheader.intquery0
                           select bh).FirstOrDefault();


            vw_genlay glayhead = new vw_genlay();
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwstrarray1[0] = bg2list.budget_journal_number.ToString();
                glayhead.vwstrarray1[1] = bg2list.budget_reference_number;
                glayhead.vwstrarray1[2] = bg2list.transaction_date;
                glayhead.vwstrarray1[3] = bg2list.total_debit.ToString("#,##0.00");
                glayhead.vwstrarray1[4] = bg2list.total_credit.ToString("#,##0.00");
                glayhead.vwstrarray1[5] = bg2list.control.ToString("#,##0.00");
                glayhead.vwstrarray1[6] = bg2list.batch_description;



            }
            ViewBag.x2 = glayhead;


        }

        private void init_header()
        {
            gdoc.budget_journal_number = 0;
            gdoc.budget_reference_number = "";
            gdoc.period = "";
            gdoc.transaction_date = "";
            gdoc.total_debit = 0;
            gdoc.total_credit = 0;
            gdoc.project_code = "";
            gdoc.approval_level = 0;
            gdoc.created_by = "";
            gdoc.approval_by = "";
            gdoc.attach_document = "";
            gdoc.approval_by = "";
            gdoc.modified_by = "";
            gdoc.note = "";
           

        }

        [HttpPost]
        public ActionResult trancode(string typecode, string id, string ac_code1 = "", string curren = "", string tdate="")
        {
            if (typecode == "01")
            {
                List<SelectListItem> ary = new List<SelectListItem>();

                ary = ac_curen(id, ac_code1, curren, tdate);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary.ToArray(),
                                    "Value",
                                    "Text")
                               , JsonRequestBehavior.AllowGet);

            }
            else if (typecode == "02")
            {
                string str1 = "select bank_code query0, c2 query1 from vw_allcust where qcode=" + util.sqlquote(id);
                var hdet = db.Database.SqlQuery<querylay>(str1);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    hdet.ToArray(),
                                    "query0",
                                    "query1")
                               , JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");

        }


        private IQueryable<querylay> ac_code(string id)
        {

            var hdet = from bg in db.GB_999_MSG
                       where bg.type_msg == "XXF"
                       select new querylay { query0 = "", query1 = "" };

            if (id == "003")
            {
                hdet = from bg in db.GL_001_CHART
                       orderby bg.account_code
                       select new querylay
                       {
                           query0 = bg.account_code,
                           query1 = bg.account_code + " &nbsp;&nbsp;" + bg.account_name
                       };
            }
            else if (id == "001")
            {
                hdet = from bg in db.AR_001_CUSTM
                       orderby bg.customer_code
                       select new querylay
                       {
                           query0 = bg.customer_code,
                           query1 = bg.customer_code + "&nbsp;&nbsp;" + bg.cust_biz_name
                       };

            }
            else if (id == "002")
            {
                hdet = from bg in db.AP_001_VENDR
                       orderby bg.vendor_code
                       select new querylay
                       {
                           query0 = bg.vendor_code,
                           query1 = bg.vendor_code + "&nbsp;&nbsp;" + bg.vend_biz_name
                       };

            }
            else if (id == "004")
            {
                hdet = from bg in db.BK_001_BANK
                       orderby bg.bank_code
                       select new querylay
                       {
                           query0 = bg.bank_code,
                           query1 = bg.bank_code + "&nbsp;&nbsp;" + bg.bank_name
                       };

            }
            else if (id == "012")
            {
                hdet = from bg in db.GB_001_RSONC
                       orderby bg.consignment_code
                       select new querylay
                       {
                           query0 = bg.consignment_code,
                           query1 = bg.consignment_code + "&nbsp; &nbsp;" + bg.description
                       };


            }

            return hdet;
        }


        private List<SelectListItem> ac_curen(string id, string ac_code, string curren, string tdate)
        {
            string cur_code = ""; string curen_name = "";
            string shw_rate = "";
            string rate_flag = "N";
            tdate = util.date_yyyymmdd(tdate);
            
            string str1 = "select currency_code query0 from vw_allcust where bank_code=" + util.sqlquote(ac_code) + " and qcode=" + util.sqlquote(id);
            var bh1 = db.Database.SqlQuery<querylay>(str1).FirstOrDefault();
            if (bh1 != null)
                cur_code = bh1.query0;

            querypass pass1 = util.basecur_description(cur_code);
            shw_rate = pass1.query0;
            curen_name = pass1.query1;
            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = cur_code, Text = curen_name });

            decimal rat_code = 0;
            pubsess pubsess = (pubsess)Session["pubsess"];
            if (pubsess.base_currency_code == cur_code)
            {
                rat_code = 1;
                ary.Add(new SelectListItem { Value = cur_code, Text = "1" });
            }
            else
            {
                // write your query statement
                string curent_date = DateTime.Now.ToString("yyyyMMdd");

                string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(cur_code) + " and '" + tdate + "' between date_from and date_to";
                var dbexch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                if (dbexch != null)
                    rat_code = dbexch.dquery0;

                ary.Add(new SelectListItem { Value = cur_code, Text = rat_code.ToString() });

            }
            return ary;
        }

         [HttpPost]
        public ActionResult b_amt(string rel_amt, string mn_rate)
        {
            decimal bas_amt = 0; 
            // write your query statement
            decimal.TryParse(rel_amt, out qty);
            decimal.TryParse(mn_rate, out pricep);


            bas_amt = qty * pricep;

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = bas_amt.ToString("#,##0.00") });
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary.ToArray(),
                                    "Value",
                                    "Text")
                               , JsonRequestBehavior.AllowGet);
            
            //}
            return RedirectToAction("Index");
        }
       
        private void move_detail()
         {
             string amt_check = "";
            glay.vwstrarray6 = new string[20];
            ////string bug = Session["hbudg"].ToString();
            //int key2 = Convert.ToInt16(Session["line_num"]);
            ////int key1 = Convert.ToInt16(bug);
            //var bgdtl = (from bg in db.BU_002_BUGTD
            //             where bg.budget_journal_number== key1 && bg.sequence_number == key2
            //             select bg).FirstOrDefault();
            glay.vwint4 = BU_002_BUGTD.sequence_number;
            glay.vwstring1 = BU_002_BUGTD.account_type_debit;
            glay.vwstring2 = BU_002_BUGTD.account_code_debit;
            glay.vwstring3 = BU_002_BUGTD.description;
            glay.vwstring4 = BU_002_BUGTD.amount_type;
            glay.vwstring5 = BU_002_BUGTD.currency_code;
            glay.vwstring6 = BU_002_BUGTD.reference_number;
            glay.vwdecimal0 = BU_002_BUGTD.amount;
            glay.vwdecimal3 = BU_002_BUGTD.base_amount;
            glay.vwdecimal5 = BU_002_BUGTD.exchange_rate;
            glay.vwstrarray6[0] =  BU_002_BUGTD.analysis_code1;
            glay.vwstrarray6[1] = BU_002_BUGTD.analysis_code2;
            glay.vwstrarray6[2] = BU_002_BUGTD.analysis_code3;
            glay.vwstrarray6[3] = BU_002_BUGTD.analysis_code4;
            glay.vwstrarray6[4] = BU_002_BUGTD.analysis_code5;
            glay.vwstrarray6[5] = BU_002_BUGTD.analysis_code6;
            glay.vwstrarray6[6] = BU_002_BUGTD.analysis_code7;
            glay.vwstrarray6[7] = BU_002_BUGTD.analysis_code8;
            glay.vwstrarray6[8] = BU_002_BUGTD.analysis_code9;
            glay.vwstrarray6[9] = BU_002_BUGTD.analysis_code10;
            amt_check = "N";

            if (BU_002_BUGTD.currency_code == pubsess.base_currency_code)
                amt_check = "Y";
            cur_read();
            //string display_cur = (from bg in db.MC_001_CUREN
            //                      where BU_002_BUGTD.currency_code == bg.currency_code
            //                      select bg.currency_description
            //                          ).FirstOrDefault();
            psess.temp4 = amt_check;
           // Session["curren_name"] = display_cur;
            Session["psess"] = psess;
        }

        private void amount_update()
        {
            string sqlstr = " update BU_002_BUGTH set total_debit = a001, total_credit = a002, control = a003";
            sqlstr += " from BU_002_BUGTH a,(select sum(case amount_type when 'D' then (base_amount) else 0 end) a001,sum(case amount_type when 'C' then (base_amount) else 0 end) a002, sum(case amount_type when 'D' then (base_amount) else 0 end)-sum(case amount_type when 'C' then (base_amount) else 0 end) as a003  from BU_002_BUGTD where budget_journal_number=" + qheader.intquery0 + ") bx";
            sqlstr += " where budget_journal_number=" + qheader.intquery0;
            db.Database.ExecuteSqlCommand(sqlstr);

        }


        private void header_ana(string commandn = "")
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            SelectList[] head_det = new SelectList[20];

            //Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            //psess.sarrayt1 = aheader5;

            if (commandn == "D")
            {
                var bglist = from bg in db.GB_001_HEADER
                             where bg.header_type_code == "011" && bg.sequence_no != 99
                             select bg;

                foreach (var item in bglist.ToList())
                {
                    int count2 = item.sequence_no - 1;
                    aheader7[count2] = item.mandatory_flag;
                    glay.vwstrarray4[count2] = item.header_code;
                    var bglist2 = (from bg in db.GB_001_HANAL
                                   where bg.header_sequence == item.header_code
                                   select bg).FirstOrDefault();

                    if (bglist2 != null)
                    {
                        glay.vwstrarray5[count2] = bglist2.header_description;
                        
                        string str = " select analysis_code query0, analysis_description query1 from GB_001_DANAL where header_sequence = ";
                        str += util.sqlquote(item.header_code);
                        var str1 = db.Database.SqlQuery<querylay>(str);
                        glay.vwlist0[count2] = str1.ToList();

                    }

                }

                // // Session["head_det"] = head_det;
                // //Session["aheader7"] = aheader7;
                // psess.sarrayt1 = glay.vwstrarray5;
                psess.sarrayt0 = aheader7;
                psess.sarrayt1 = glay.vwstrarray5;
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;
using anchor1.Filters;
using System.Data.Entity.SqlServer;

namespace CittaErp.Controllers
{

    public class JournalController : Controller
    {
        //
        // GET: /Employee/

        GL_002_JONAD GL_002_JONAD = new GL_002_JONAD();
        GL_002_JONAL gdoc = new GL_002_JONAL();
        MainContext db = new MainContext();
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
        decimal taxm = 0; decimal disc_per1 = 0; string rel_def = ""; string re_ref = "";
        string jrnlno="";
        string action_flag = "";

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            psess = (psess)Session["psess"];
             
            util.init_values();

            
            var bglist = from bh in db.GL_002_JONAL
                         join bj in db.GB_999_MSG
                           on new { a1 = bh.period, a2 = "FYM" } equals new { a1 = bj.code_msg, a2 = bj.type_msg }
                           into bj1
                           from bj2 in bj1.DefaultIfEmpty()
                         where bh.approval_level == 0
                         select new vw_genlay
                         {
                             vwint0 = bh.journal_number,
                             vwstring0 = bh.manual_reference_number,
                             vwdecimal0 = bh.total_debit,
                             vwdecimal1 = bh.total_credit,
                             vwstring1 = bh.batch_description,
                             vwstring2 = bh.control_flag == "" ? "No Entry" : bh.control == 0 ? "Yes" : "No",//bh.total_credit > bh.total_debit ? "No": bh.total_credit < bh.total_debit? "No" : "Yes",
                             vwstring3 = "Open",
                             vwstring4 = bj2.name1_msg != null ? bj2.name1_msg + " " + bh.year : bh.period + " " + bh.year

                         };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult CreateHeader()
        {
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            //psess.temp4 = "";
            //Session["fxd_ast"] = "SJ";
            //jrnlno = "";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            period_basis();
            select_query_head("H");
            glay.vwstring8 = "H";

            return View(glay);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHeader(vw_genlay glay_in,  HttpPostedFileBase[] photofile, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            glay = glay_in;
            qheader.query2 = "SJ";
            photo1 = photofile;
            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head("H");
            glay.vwstrarray2 = new string[20];
            glay.vwstring8 = "H";
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditHeader()
        {
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            initial_rtn();

            glay.vwstring8 = "H";
            //glay.vwstring9 = qheader.intquery2.ToString();

            gdoc = db.GL_002_JONAL.Find(qheader.intquery2);
            if (gdoc != null)
               read_header();

            select_query_head("H");

            return View(glay);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHeader(vw_genlay glay_in, HttpPostedFileBase[] photofile, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            glay = glay_in;
           // glay.vwstring9 = qheader.intquery2.ToString();
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

        public ActionResult CreateDetails(string headtype="D")
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            
            initial_rtn();
                header_ana("D");
                select_query_head("D");
            glay.vwstring8 = headtype;
           // glay.vwstring9 = qheader.intquery2.ToString();
            detail_intial();
            glay.vwstrarray2[3] = pubsess.base_currency_description;
            glay.vwstring9 = pubsess.exchange_editable;


            show_screen_info();
           
            return View(glay);
        }

        private void detail_intial()
        {
            glay.vwstrarray0 = new string[20]; 
            glay.vwstrarray2 = new string[20];
            glay.vwstring1 = qheader.query0;
            glay.vwstring10 = pubsess.base_currency_code;
            glay.vwstrarray2[0] = qheader.query6;
            glay.vwstrarray2[2] = "N";
            glay.vwstrarray0[0] = qheader.query1;
            glay.vwstring9 = pubsess.price_editable;
            //glay.vwstrarray2[3] = pubsess.base_currency_description;

           // Session["curren_name"] = pubsess.base_currency_description;

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetails(vw_genlay glay_in, string headtype = "D")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            glay = glay_in;
            //jrnlno = Session["hjon"].ToString();

            //glay.vwstring9 = qheader.intquery2.ToString();

            if (headtype == "send_app")
            {
                err_flag = validation_routine_send(headtype);
                if (err_flag)
                {
                    util.update_entry("010", qheader.intquery2.ToString(), pubsess.userid);
                    string str = "update GL_002_JONAL set approval_level=99 where journal_number=" + qheader.intquery2;
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
            select_query_head("D");
            cur_read();

            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult Edit(int key1)
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            gdoc = db.GL_002_JONAL.Find(key1);
            if (gdoc != null)
            {
                //Session["tran_date"] = DateTime.Now.ToString("dd/MM/yyyy");
               // Session["fxd_ast"] = gdoc.journal_type;
                set_qheader();
                return RedirectToAction("CreateDetails", new { headtype = "D" });
            }

          
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditDetails(int key1, int key2)
        {
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails"; 
          pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            glay.vwstring9 = pubsess.exchange_editable;

            GL_002_JONAD = db.GL_002_JONAD.Find(qheader.intquery2, key2);
            if (GL_002_JONAD == null)
                return View(glay);

            //Session["line_num"] = GL_002_JONAD.sequence_number;
            initial_rtn();
            detail_intial();
            move_detail();
            header_ana("D");
            select_query_head("D");
            glay.vwstring8 = "D";
            
            show_screen_info();

            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails(vw_genlay glay_in, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails"; 
          
            glay = glay_in;
           // glay.vwstring9 = qheader.intquery2.ToString();
            glay.vwdecimal4 = Convert.ToDecimal(Session["line_num"]);

            if (headtype == "send_app")
            {
                err_flag = validation_routine_send(headtype);
                if (err_flag)
                {
                    util.update_entry("010", qheader.intquery2.ToString(), pubsess.userid);
                    string str = "update GL_002_JONAL set approval_level=99 where journal_number=" + qheader.intquery2;
                    db.Database.ExecuteSqlCommand(str);

                    return RedirectToAction("CreateHeader");
                }
                else
                {
                    select_query_head("D");

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
            select_query_head("D");

            glay.vwlist0 = new List<querylay>[20]; 
            glay.vwstring8 = "H";
            detail_intial();
            show_screen_info();
            header_ana();
            //cur_read();
            return View(glay);

        }

        private void delete_record()
        {
            GL_002_JONAD = db.GL_002_JONAD.Find(glay.vwint0, glay.vwint1);
            if (GL_002_JONAD != null)
            {
                db.GL_002_JONAD.Remove(GL_002_JONAD);
                db.SaveChanges();
            }
        }
        private void update_file(string headtype)
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
                    gdoc = new GL_002_JONAL();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;
                    gdoc.approval_level = 0;
                    gdoc.approval_date = DateTime.UtcNow;
                    
                }
                else
                {
                    gdoc = db.GL_002_JONAL.Find(qheader.intquery2);
                }

                gdoc.manual_reference_number = glay.vwstring0;
                gdoc.journal_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : (glay.vwstrarray0[0]);
                gdoc.number_of_cycle = glay.vwint3;
                gdoc.cycle_interval = glay.vwint2;
                gdoc.period = glay.vwstrarray0[2] + glay.vwstrarray0[1];
                //gdoc.period = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : (glay.vwstrarray0[1]);
                gdoc.year = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : (glay.vwstrarray0[2]);
                gdoc.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : (glay.vwstrarray0[4]);
                gdoc.batch_description = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : (glay.vwstrarray0[3]);
                if (glay.vwstrarray0[0] == "AJ" && glay.vwstrarray3[0] != "")
                {
                    gdoc.reversing_date = util.date_yyyymmdd(glay.vwstrarray3[0]);
                    gdoc.rev_approval_mode = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : (glay.vwstrarray0[6]);
                }
                if (glay.vwstrarray0[0] == "RJ" && glay.vwstrarray0[5] != "")
                    gdoc.approval_mode = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : (glay.vwstrarray0[5]);
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
                    GL_002_JONAD = new GL_002_JONAD();
                    GL_002_JONAD.created_by = pubsess.userid;
                    GL_002_JONAD.created_date = DateTime.UtcNow;
                    glay.vwint4 = get_next_line_sequence();             
                }
                else
                {
                    GL_002_JONAD = db.GL_002_JONAD.Find(qheader.intquery2, glay.vwint4);
                }

                 //the detailsub button has been clicked
                GL_002_JONAD.journal_number = qheader.intquery2;
                GL_002_JONAD.sequence_number = glay.vwint4;
                GL_002_JONAD.reference_number = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : (glay.vwstring1);
                GL_002_JONAD.description = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : (glay.vwstring2);
                GL_002_JONAD.amount = glay.vwdecimal0;
                GL_002_JONAD.amount_type = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : (glay.vwstring5);
                GL_002_JONAD.account_type_debit = string.IsNullOrWhiteSpace(glay.vwstring6)? "":(glay.vwstring6);
                GL_002_JONAD.account_code_debit = string.IsNullOrWhiteSpace(glay.vwstring7)? "": (glay.vwstring7);
                
                GL_002_JONAD.transaction_date = util.date_yyyymmdd(glay.vwstrarray2[0]);
                GL_002_JONAD.currency = string.IsNullOrWhiteSpace(glay.vwstring10) ? "" : (glay.vwstring10);
                GL_002_JONAD.exchange_rate = glay.vwdecimal1;
                GL_002_JONAD.base_amount = glay.vwdecimal1 * glay.vwdecimal0;
                string base_cur = pubsess.base_currency_code;
                
                if (base_cur==GL_002_JONAD.currency)
                {
                    GL_002_JONAD.exchange_rate = 1;
                    GL_002_JONAD.base_amount = GL_002_JONAD.amount;
                }
                else
                {
                    if (glay.vwstring9 != "Y")
                    {
                        string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(GL_002_JONAD.currency) + " and '" + GL_002_JONAD.transaction_date + "' between date_from and date_to";
                        var exch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                        if (exch != null)
                            GL_002_JONAD.exchange_rate = exch.dquery0;
                    }
                }

                GL_002_JONAD.asset_class = "";
                GL_002_JONAD.fixed_asset_code = "";
                GL_002_JONAD.fa_transaction_type = "";

                if (qheader.query1 == "FJ")
                {
                    GL_002_JONAD.asset_class = string.IsNullOrWhiteSpace(glay.vwstrarray2[1]) ? "" : glay.vwstrarray2[1];
                    GL_002_JONAD.fixed_asset_code = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : (glay.vwstring3);
                    GL_002_JONAD.fa_transaction_type = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : (glay.vwstring4);
                }
                
                
               // GL_002_JONAD.net_amount = net_amt;
                GL_002_JONAD.analysis_code1 = "";
                GL_002_JONAD.analysis_code2 = "";
                GL_002_JONAD.analysis_code3 = "";
                GL_002_JONAD.analysis_code4 = "";
                GL_002_JONAD.analysis_code5 = "";
                GL_002_JONAD.analysis_code6 = "";
                GL_002_JONAD.analysis_code7 = "";
                GL_002_JONAD.analysis_code8 = "";
                GL_002_JONAD.analysis_code9 = "";
                GL_002_JONAD.analysis_code10 = "";
                GL_002_JONAD.modified_date = DateTime.UtcNow;
                GL_002_JONAD.modified_by = pubsess.userid;

                //Session["ref_def"] = GL_002_JONAD.reference_number;
                //Session["tran_date"] = util.date_slash(GL_002_JONAD.transaction_date);

                if (glay.vwstrarray6 != null)
                {
                    int arrlen = glay.vwstrarray6.Length;
                    if (arrlen > 0)
                        GL_002_JONAD.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                    if (arrlen > 1)
                        GL_002_JONAD.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                    if (arrlen > 2)
                        GL_002_JONAD.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                    if (arrlen > 3)
                        GL_002_JONAD.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                    if (arrlen > 4)
                        GL_002_JONAD.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                    if (arrlen > 5)
                        GL_002_JONAD.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                    if (arrlen > 6)
                        GL_002_JONAD.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                    if (arrlen > 7)
                        GL_002_JONAD.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                    if (arrlen > 8)
                        GL_002_JONAD.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                    if (arrlen > 9)
                        GL_002_JONAD.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                    psess.intemp0 = arrlen;
                }

                if (cr_flag == "CreateDetails")
                    db.Entry(GL_002_JONAD).State = EntityState.Added;
                else
                    db.Entry(GL_002_JONAD).State = EntityState.Modified;
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
//Session["hjon"] = gdoc.journal_number;
                //Session["ref_def"] = gdoc.manual_reference_number;
               // Session["tran_date"] = DateTime.Now.ToString("dd/MM/yyyy");
                //Session["fxd_ast"] = gdoc.journal_type;
                //jrnlno = gdoc.journal_number.ToString();
                //psess.temp4 = gdoc.reversing_date;
                //Session["periodmth"] = gdoc.period;
                //Session["periodyr"] = gdoc.year;
                }
            if (err_flag && cr_flag.IndexOf("Header") < 0)
            {
               qheader.query0  = GL_002_JONAD.reference_number;
               qheader.query6 = util.date_slash(GL_002_JONAD.transaction_date);
               Session["qheader"] = qheader;
            
                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, GL_002_JONAD b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and journal_number =" + glay.vwint0;
                //db.Database.ExecuteSqlCommand(stri);
              

                string sp = "update GL_002_JONAL set control_flag = 'Y' where journal_number =" + glay.vwint0;
                db.Database.ExecuteSqlCommand(sp);
                //if (GL_002_JONAD.account_type_debit == "003")
                //{
                //    string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, GL_002_JONAD b where a.account_code = b.account_code_debit";
                //    str += " and journal_number =" + glay.vwint0;
                //    //db.Database.ExecuteSqlCommand(str);
               
               // }
            
                amount_update();
              //string  jh = GL_002_JONAD.asset_class;
              // string st = "update GB_001_PCODE set delete_flag ='Y' where parameter_type = '15' and parameter_code = " + util.sqlquote(jh);
                //db.Database.ExecuteSqlCommand(st);

            }
            if (err_flag && cr_flag.IndexOf("Header") > 0)
            {
                set_qheader();
               util.write_document("JOURNAL", gdoc.journal_number.ToString(), photo1, glay.vwstrarray9);
               
            }
         
        }

        private int get_next_line_sequence()
        {
            string sqlstr = "select isnull(max(sequence_number),0) vwint1 from GL_002_JONAD where reference_number=" + util.sqlquote(glay.vwstring1);
            var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
            return sql1.vwint1 + 1;
                
        }
        private void validation_routine(string headtype)
        {
            DateTime date_chk = DateTime.Now; string revdate = "";
          //  qheader = (queryhead)Session["qheader"];
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            string[] jtype1 = new string[20];
           
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            jtype1 = (string[])psess.sarrayt2;
            string prd_chk = pubsess.period_closing;
            DateTime mcurent = DateTime.Now;
            DateTime invaliddate = new DateTime(1000, 01, 01);

            string cr_flag = action_flag;
            if (cr_flag.IndexOf("Header") > 0)
            {
                if (glay.vwstrarray0[0] == "AJ")
                {
                    

                        if (!util.date_validate(glay.vwstrarray3[0]))
                        {
                            ModelState.AddModelError(String.Empty, "Please insert a valid date");
                            err_flag = false;
                        }
                        if (glay.vwstrarray0[5] == "")
                        {
                            ModelState.AddModelError(String.Empty, "Please select a Reversing Approval Mode");
                            err_flag = false;
                        }

                    
                }
                if (glay.vwstrarray0[0] == "RJ")
                {

                    if (glay.vwstrarray0[5]== "")
                    {
                        ModelState.AddModelError(String.Empty, "Please select a Recuring Approval Mode");
                        err_flag = false;
                    }

                }
                if (glay.vwstring0 == ""||glay.vwstring0 == null)
                {
                    ModelState.AddModelError(String.Empty, "Please insert manual number");
                    err_flag = false;
                }
                revdate = util.date_yyyymmdd(glay.vwstrarray3[0]);
                string tran_date = util.date_yyyymmdd(DateTime.Now.ToString("dd/MM/yyyy"));
                if (string.Compare(revdate, tran_date) < 0 && glay.vwstrarray0[0] == "AJ")
                    {
                        ModelState.AddModelError(String.Empty, "Reversing date must be a future date");
                        err_flag = false;
                    }
                

                
            }
            if (cr_flag.IndexOf("Details") > 0)
            {
                
                if (!util.date_validate(glay.vwstrarray2[0]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
                    err_flag = false;
                }
                    
                if (qheader.query2 == "AJ")
                {
                    revdate = util.date_yyyymmdd(glay.vwstrarray3[0]);
                }
                 string tran_date = util.date_yyyymmdd(glay.vwstrarray2[0]);
                 if (prd_chk == "C")
                 {
                     int year = Convert.ToInt32(qheader.query4); int month = Convert.ToInt32(qheader.query3.Substring(4));
                     int current_day = DateTime.Now.Day;
                     var firstDayOfMonth = new DateTime(year, month, 1);
                     var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                     string startday = qheader.query3 + "0"+firstDayOfMonth.Day.ToString();
                     string endday = qheader.query3+ lastDayOfMonth.Day.ToString();
                     string tra_date = tran_date;
                     if (Convert.ToInt32(tra_date) < Convert.ToInt32(startday) || Convert.ToInt32(tra_date) > Convert.ToInt32(endday))
                     {
                         ModelState.AddModelError(String.Empty, "Transaction date must be within current active period");
                         err_flag = false;
                     }
                     
                 }
                 else
                 {
                     int year = Convert.ToInt32(qheader.query4); int month = Convert.ToInt32(qheader.query3.Substring(4));
                     string prdnum = month.ToString();
                     int current_day = DateTime.Now.Day;
                     var prdate = (from bg in db.GB_001_PCT
                                   where bg.period_number == prdnum
                                   select bg).FirstOrDefault();

                     string startd = prdate.date_from + "/" + qheader.query4; string endd = prdate.date_to + "/" + qheader.query4;
                     DateTime startdt = util.date_convert(startd); DateTime enddt = util.date_convert(endd);

                     string startday = startdt.ToString("yyyyMMdd");
                     string endday = enddt.ToString("yyyyMMdd");
                     string tra_date = tran_date;
                     if (Convert.ToInt32(tra_date) < Convert.ToInt32(startday) || Convert.ToInt32(tra_date) > Convert.ToInt32(endday))
                     {
                         ModelState.AddModelError(String.Empty, "Transaction date must be within current active period");
                         err_flag = false;
                     }
                 }  
                if (string.Compare(revdate , tran_date)>0 && qheader.query1 == "AJ")
                {
                    ModelState.AddModelError(String.Empty, "Reversing date must be a future date");
                    err_flag = false;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstrarray2[1]) && qheader.query1 == "FJ")
                {
                    ModelState.AddModelError(String.Empty, "Enter Asset Class");
                    err_flag = false;
                }
                if (glay.vwstrarray2[1] != "" && glay.vwstring3 == null && qheader.query1 == "FJ")
                {
                    ModelState.AddModelError(String.Empty, "Enter Asset ID");
                    err_flag = false;
                }

                if (string.IsNullOrWhiteSpace(glay.vwstring1))
                {
                    ModelState.AddModelError(String.Empty, "Enter Reference Number");
                    err_flag = false;
                } 
                if (string.IsNullOrWhiteSpace(glay.vwstring2))
                {
                    ModelState.AddModelError(String.Empty, "Enter Description");
                    err_flag = false;
                } 
                if (string.IsNullOrWhiteSpace(glay.vwstring6))
                {
                    ModelState.AddModelError(String.Empty, "Enter Posting Class");
                    err_flag = false;
                }
                if (!string.IsNullOrWhiteSpace(glay.vwstring6) && string.IsNullOrWhiteSpace(glay.vwstring7))
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

                if (string.IsNullOrWhiteSpace(glay.vwstring5))
                {
                    ModelState.AddModelError(String.Empty, "Specify Debit/Credit for Amount");
                    err_flag = false;
                }
               
                for (int count1 = 0; count1 < 10; count1++)
                {
                    if (aheader7[count1] == "Y" | jtype1[count1] == "Y" && string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
                    {
                        error_msg = aheader5[count1] + " is mandatory. ";
                        ModelState.AddModelError(String.Empty, error_msg);
                        err_flag = false;
                    }

                    //if ( && string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
                    //{
                    //    error_msg = aheader5[count1] + " is mandatory. ";
                    //    ModelState.AddModelError(String.Empty, error_msg);
                    //    err_flag = false;
                    //}
                }

            }
            if (headtype == "send_app")
            {
               
            }

        
                //if (error_msg != "")
                //{
                //    ModelState.AddModelError(String.Empty, error_msg);
                //    err_flag = false;
                //}
        }
        private void set_qheader()
        {
            qheader.query1 = gdoc.journal_type;
            qheader.query0 = gdoc.manual_reference_number;
            qheader.query2 = util.date_slash(gdoc.reversing_date);
            qheader.intquery1 = gdoc.number_of_cycle;
            qheader.intquery0 = gdoc.cycle_interval;
            qheader.intquery2 = gdoc.journal_number;
            qheader.query3 = gdoc.period;
            qheader.query4 = gdoc.year;
            qheader.query5 = gdoc.batch_description;
            qheader.query6 = DateTime.Now.ToString("dd/MM/yyyy");
            Session["qheader"] = qheader;
           
        }
        private void current_val()
        {
            var chyk = (from bg in db.GB_001_COY
                        where bg.id_code =="COYSET"
                        select bg).FirstOrDefault();
            if (chyk != null)
            {
                string wavr = chyk.field15;
                string dfrm = chyk.field7;
                string dto = chyk.field8;
                string expd = chyk.field9;
            }
        }
        private Boolean validation_routine_send(string headtype)
        {
            err_flag = true;
           // string error_msg = "";
            initial_rtn();
            decimal contrl_check = (from bg in db.GL_002_JONAL
                                    where bg.journal_number == qheader.intquery2
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
           // string trans_date = "";
            var bglist = from bh in db.GL_002_JONAD
                         where bh.journal_number == qheader.intquery2
                         select new vw_genlay

                         {
                             
                             vwint4= bh.sequence_number,
                             vwstring1 = bh.reference_number,
                             vwstring10 = bh.currency,
                             vwstring7 = bh.account_code_debit,
                             vwstring2 = bh.description,
                             vwstring3 = bh.fixed_asset_code,
                             vwstring4 = bh.fa_transaction_type,
                             vwstring5 = bh.amount_type,
                             vwstring6 = bh.account_type_debit,
                             vwdecimal0 = bh.amount,
                             vwdecimal1 = bh.exchange_rate,
                             vwdecimal3 = bh.base_amount,
                             //vwstring8 = bh.transaction_date.ToString("dd/MM/yyyy"),
                             //vwstring8 = SqlFunctions.DateName("day",bh.transaction_date) + "/" + "0"+SqlFunctions.StringConvert((double)bh.transaction_date.Month).TrimStart() + "/" +  SqlFunctions.DateName("year",bh.transaction_date),
                             vwstring9 = bh.asset_class

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
               


                GL_002_JONAR GL_002_JONAR = new GL_002_JONAR();
                GL_002_JONAR.journal_number = qheader.intquery2;
                GL_002_JONAR.sequence_number = item.vwint4;
                GL_002_JONAR.reference_number = string.IsNullOrWhiteSpace(item.vwstring1) ? "" : (item.vwstring1);
                GL_002_JONAR.number_of_cycle = qheader.intquery1;
                GL_002_JONAR.cycle_interval = qheader.intquery0;
                GL_002_JONAR.description = string.IsNullOrWhiteSpace(item.vwstring2) ? "" : (item.vwstring2);
                GL_002_JONAR.amount = item.vwdecimal0;
                GL_002_JONAR.amount_type = string.IsNullOrWhiteSpace(item.vwstring5) ? "" : (item.vwstring5);
                GL_002_JONAR.account_type_debit = string.IsNullOrWhiteSpace(item.vwstring6) ? "" : (item.vwstring6);
                GL_002_JONAR.account_code_debit = string.IsNullOrWhiteSpace(item.vwstring7) ? "" : (item.vwstring7);
                GL_002_JONAR.reversing_date = util.date_yyyymmdd(qheader.query2);
                GL_002_JONAR.period = string.IsNullOrWhiteSpace(qheader.query3) ? "" : (qheader.query3);
                GL_002_JONAR.year = string.IsNullOrWhiteSpace(qheader.query4) ? "" : (qheader.query4);
                GL_002_JONAR.transaction_date = util.date_yyyymmdd(item.vwstring8);
                GL_002_JONAR.currency = string.IsNullOrWhiteSpace(item.vwstring10) ? "" : (item.vwstring10);
                GL_002_JONAR.exchange_rate = item.vwdecimal1;
                GL_002_JONAR.base_amount = item.vwdecimal1 * item.vwdecimal0;
                GL_002_JONAR.exchange_rate = 1;
                GL_002_JONAR.base_amount = GL_002_JONAR.amount;
                GL_002_JONAR.asset_class = string.IsNullOrWhiteSpace(item.vwstring9) ? "" : item.vwstring9;
                GL_002_JONAR.fixed_asset_code = string.IsNullOrWhiteSpace(item.vwstring3) ? "" : (item.vwstring3);
                GL_002_JONAR.fa_transaction_type = string.IsNullOrWhiteSpace(item.vwstring4) ? "" : (item.vwstring4);
                GL_002_JONAR.batch_description = string.IsNullOrWhiteSpace(qheader.query5) ? "" : (qheader.query5);
                GL_002_JONAR.approval_level = 0;
                GL_002_JONAR.approval_by = "";

                // GL_002_JONAR.net_amount = net_amt;
                GL_002_JONAR.analysis_code1 = "";
                GL_002_JONAR.analysis_code2 = "";
                GL_002_JONAR.analysis_code3 = "";
                GL_002_JONAR.analysis_code4 = "";
                GL_002_JONAR.analysis_code5 = "";
                GL_002_JONAR.analysis_code6 = "";
                GL_002_JONAR.analysis_code7 = "";
                GL_002_JONAR.analysis_code8 = "";
                GL_002_JONAR.analysis_code9 = "";
                GL_002_JONAR.analysis_code10 = "";
                GL_002_JONAR.created_date = DateTime.UtcNow;
                GL_002_JONAR.modified_date = DateTime.UtcNow;
                GL_002_JONAR.modified_by = pubsess.userid;
                GL_002_JONAR.created_by = pubsess.userid;
                db.Entry(GL_002_JONAR).State = EntityState.Added;
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
            string id = qheader.intquery2.ToString();
            delete_list(id);

        }

        private void select_query_head(string type1)
        {                
            if (type1 == "H")
            {
                year_cal();
                string disckf = pubsess.valid_datefrm.Substring(4, 2);
                string disckt = pubsess.valid_dateto.Substring(4, 2);
                string curent_mth = pubsess.curent_datefrm.Substring(4, 2);

                if (pubsess.period_closing == "C")
                {
                    string srt = "select distinct query0, query1 from (";
                    srt += " select code_msg query0, name1_msg query1 from  dbo.GB_999_MSG pf where pf.type_msg = 'FYM'and  code_msg =" + util.sqlquote(curent_mth) + "union all";
                    srt += " select code_msg query0, name1_msg query1  from dbo.GB_999_MSG pg where pg.type_msg = 'FYM' and code_msg between" + util.sqlquote(disckf) + " and" + util.sqlquote(disckt) + ") b order by 2";
                    var bf2 = db.Database.SqlQuery<querylay>(srt).ToList();
                    ViewBag.month = new SelectList(bf2.ToList(), "query0", "query1", glay.vwstrarray0[1]);
                }
                else
                {
                    string srt = "select distinct query0, query1 from (";
                    srt = " select period_number query0, prd_description query1 from  dbo.GB_001_PCT pf where period_number =" + util.sqlquote(curent_mth) + "union all";
                    srt += " select period_number query0, prd_description query1  from dbo.GB_001_PCT pg where period_number between" + util.sqlquote(disckf) + " and" + util.sqlquote(disckt) + ") b order by 2";
                    var bf2 = db.Database.SqlQuery<querylay>(srt).ToList();
                    ViewBag.month = new SelectList(bf2.ToList(), "query0", "query1", glay.vwstrarray0[1]);

                }

                var empz = from pf in db.GL_001_JONT
                           select new { c1 = pf.journal_code, c2 = pf.name };
                ViewBag.jtype = new SelectList(empz.ToList().Distinct(), "c1", "c2", glay.vwstrarray0[0]);
            }


            if (type1 == "D")
            {
                string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
                var emp1 = db.Database.SqlQuery<querylay>(str1);

                ViewBag.cur = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring10);

                //var emp = from pf in db.MC_001_CUREN
                //          where pf.active_status == "N"
                //          orderby pf.currency_description
                //          select pf;
                //ViewBag.cur = new SelectList(emp.ToList(), "currency_code", "currency_description", glay.vwstring10);
                //var bgl = from bg in db.GB_001_PCODE
                //          where bg.parameter_type == "15" && bg.active_status == "N"
                //          orderby bg.parameter_name
                //          select bg;

                //ViewBag.fixc = new SelectList(bgl.ToList(), "parameter_code", "parameter_name", glay.vwstrarray2[1]);
                ViewBag.fixc = util.para_selectquery("15", glay.vwstrarray2[1]);

                ViewBag.fix = util.para_selectquery("61", glay.vwstring3);
                //ViewBag.fix = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring3);
          
                //var fixd = from bg in db.FA_001_ASSET
                //           where bg.active_status == "N"
                //           orderby bg.description
                //           select bg;
                //ViewBag.fix = new SelectList(fixd.ToList(), "fixed_asset_code", "description", glay.vwstring3);

                var empi = from pf in db.GB_999_MSG
                           where pf.type_msg == "HEAD" && pf.name6_msg == "P"
                           orderby pf.name1_msg
                           select pf;
                ViewBag.debit = new SelectList(empi.ToList(), "code_msg", "name1_msg", glay.vwstring6);


                var empF = from pf in db.GB_999_MSG
                           where pf.type_msg == "FAT"
                           orderby pf.name1_msg
                           select pf;
                ViewBag.ftype = new SelectList(empF.ToList(), "code_msg", "name1_msg", glay.vwstring4);

                 str1 = "select bank_code query0, c2 query1 from vw_allcust where qcode=" + util.sqlquote(glay.vwstring6);
                var hdet = db.Database.SqlQuery<querylay>(str1);
                ViewBag.code = new SelectList(hdet.ToList(), "query0", "query1", glay.vwstring7);

            }
        }

        private void select_queryhide(string type1 = "D")
        {
            if (type1 == "D")
            {
                var emp = from pf in db.MC_001_CUREN
                          select pf;
                ViewBag.cur = new SelectList(emp.ToList(), "currency_code", "currency_description");

                var bgl = from bg in db.GB_001_PCODE
                          where bg.parameter_type == "15"
                          select bg;

                ViewBag.fixc = new SelectList(bgl.ToList(), "parameter_code", "parameter_name");


                var fixd = from bg in db.FA_001_ASSET
                           select bg;
                ViewBag.fix = new SelectList(fixd.ToList(), "fixed_asset_code", "description");

                var empi = from pf in db.GB_999_MSG
                           where pf.type_msg == "HEAD" && pf.name6_msg == "P"
                           select pf;
                ViewBag.debit = new SelectList(empi.ToList(), "code_msg", "name1_msg");


                var empF = from pf in db.GB_999_MSG
                           where pf.type_msg == "FAT"
                           select pf;
                ViewBag.ftype = new SelectList(empF.ToList(), "code_msg", "name1_msg");

                if (glay.vwstring6 == "003")
                {
                    ViewBag.code = util.cal_gl("001");

            
                }
                else if (glay.vwstring6 == "001")
                {
                    var bat = from bg in db.AR_001_CUSTM
                              select bg;
                    ViewBag.code = new SelectList(bat.ToList(), "customer_code", "cust_biz_name");
                }
                else if (glay.vwstring6 == "002")
                {
                    var bat = from bg in db.AP_001_VENDR
                              select bg;
                    ViewBag.code = new SelectList(bat.ToList(), "vendor_code", "vend_biz_name");
                }
                else if (glay.vwstring6 == "004")
                {
                    var bat = from bg in db.BK_001_BANK
                              select bg;
                    ViewBag.code = new SelectList(bat.ToList(), "bank_code", "bank_name");
                }
                else if (glay.vwstring6 == "012")
                {
                    var bat = from bg in db.GB_001_RSONC
                              select bg;
                    ViewBag.code = new SelectList(bat.ToList(), "consignment_code", "description");
                }
                else
                {
                    ViewBag.code = new SelectList("", "");
                }
            }

        }

        private void initial_rtn()
        {
            glay.vwdclarray0 = new decimal[50];
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
            glay.vwstrarray2 = new string[20];
            glay.vwstrarray3 = new string[20];
            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwstrarray0[wctr] = "";
            glay.vwstrarray0[2] = DateTime.Now.Year.ToString();
            glay.vwstrarray0[0] = "SJ";
            glay.vwstrarray0[5] = "A";
            glay.vwstrarray0[6] = "A";
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray7 = new string[20];
            glay.vwlist0 = new List<querylay>[20];
        

        }
    

        private void cur_read()
        {
            string curcode = "";
            curcode = glay.vwstring10;
            string read_cur = (from bg in db.MC_001_CUREN
                               where bg.currency_code == curcode
                               select bg.currency_description).FirstOrDefault();
            glay.vwstrarray2[3] = read_cur;
           // Session["curren_name"] = read_cur;
        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GL_002_JONAL] where cast (journal_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "delete from [dbo].[GL_002_JONAD] where cast(journal_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        public ActionResult delete_detail(string id)
        {
            // write your query statement
            //jrnlno = Session["hjon"].ToString();
            string sqlstr = "delete from [dbo].[GL_002_JONAD] where cast(journal_number as varchar) +'[]'+ cast(sequence_number as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            amount_update();
            select_queryhide("D");
            if (HttpContext.Request.IsAjaxRequest())
                return Json(
                            JsonRequestBehavior.AllowGet);
            return RedirectToAction("CreateDetails");
            
        }

        private void read_details()
        {
            var bglist = from bh in db.GL_002_JONAD
                         join bg in db.MC_001_CUREN
                         on new { a1 = bh.currency } equals new { a1 = bg.currency_code }
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
                         where bh.journal_number == qheader.intquery2
                         select new vw_genlay
                         {
                             vwint0 = bh.journal_number,
                             vwint1 = bh.sequence_number,
                             vwstring6 = bh.account_type_debit,
                             vwstring7 = bh.account_type_debit == "003" ? bk2.account_name : bh.account_type_debit == "001" ? bl2.cust_biz_name : bh.account_type_debit == "002" ? bm2.vend_biz_name : bh.account_type_debit == "004" ? bn2.bank_name : bo2.description,
                             vwstring2 = bh.description,
                             vwstring1 = bh.reference_number,
                             vwstring3 = bh.fixed_asset_code,
                             vwstring4 = bh.fa_transaction_type,
                             vwdecimal0 = bh.amount,
                             vwdecimal3 = bh.base_amount,
                             vwstring5 = bh.amount_type,
                             vwstring0 = bh.account_type_debit == "003" ? bk2.account_code : bh.account_type_debit == "001" ? bl2.customer_code : bh.account_type_debit == "002" ? bm2.vendor_code : bh.account_type_debit == "004" ? bn2.bank_code : bo2.consignment_code,
                             vwstring10 = bg2.currency_description
                         };
           ViewBag.x1 = bglist.ToList();
           // var check = bglist.ToList();   

        }

        public ActionResult PostIndex()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            var bglist = from bh in db.GL_002_JONAR
                         join bg in db.MC_001_CUREN
                         on new { a1 = bh.currency } equals new { a1 = bg.currency_code }
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
                             vwint0 = bh.journal_number,
                             vwint1 = bh.sequence_number,
                             vwstring6 = bh.account_type_debit,
                             vwstring7 = bh.account_type_debit == "003" ? bk2.account_name : bh.account_type_debit == "001" ? bl2.cust_biz_name : bh.account_type_debit == "002" ? bm2.vend_biz_name : bh.account_type_debit == "004" ? bn2.bank_name : bo2.description,
                             vwstring2 = bh.description,
                             vwstring1 = bh.reference_number,
                             vwstring3 = bh.fixed_asset_code,
                             vwstring4 = bh.fa_transaction_type,
                             vwdecimal0 = bh.amount,
                             vwdecimal3 = bh.base_amount,
                             vwstring5 = bh.amount_type,
                             vwint2 = bh.number_of_cycle,
                             vwint3 = bh.cycle_interval,
                             vwstring8 = bh.period + " " + bh.year,
                             vwstring0 = bh.account_type_debit == "003" ? bk2.account_code : bh.account_type_debit == "001" ? bl2.customer_code : bh.account_type_debit == "002" ? bm2.vendor_code : bh.account_type_debit == "004" ? bn2.bank_code : bo2.consignment_code,
                             vwstring10 = bg2.currency_description
                         };
            return View(bglist.ToList());


        }

        private void show_screen_info()
        {
            display_header();
            read_details();

        }

       
        [HttpPost]
        public ActionResult trancode(string typecode, string id, string ac_code1 = "", string curren = "", string tdate="")
        {
            if (typecode == "01")
            {
                List<SelectListItem> ary = new List<SelectListItem>();

                ary = ac_curen(id, ac_code1, curren,tdate);
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


        private IEnumerable<querylay> ac_code(string id)
        {

            //var hdet = from bg in db.GB_999_MSG
            //           where bg.type_msg == "XXF"
            //           select new querylay { query0 = "", query1 = "" };
            string sqlstr = " select code_msg query0, name1_msg query1 from GB_999_MSG where type_msg = 'XXF'";
            var hdet = db.Database.SqlQuery<querylay>(sqlstr);

            if (id == "003")
            {
                //hdet = from bg in db.GL_001_CHART
                //       orderby bg.account_code
                //       select new querylay
                //       {
                //           query0 = bg.account_code,
                //           query1 = bg.account_code + " &nbsp;&nbsp;" + bg.account_name
                //       };
                 sqlstr = "select account_code query0, account_code+ '---'+ account_name as query1 from GL_001_CHART where account_type_code in (select acct_type1 from GL_001_GLDS where gl_default_id = '002'union all ";
                sqlstr += " select acct_type2 from GL_001_GLDS where gl_default_id = '002'union all select acct_type3 from GL_001_GLDS where gl_default_id = '002'union all ";
                sqlstr += " select acct_type4 from GL_001_GLDS where gl_default_id = '002'union all select acct_type5 from GL_001_GLDS where gl_default_id = '002') order by account_code";
                 hdet = db.Database.SqlQuery<querylay>(sqlstr);
                    
            }
            else if (id == "001")
            {
                //hdet = from bg in db.AR_001_CUSTM
                //       orderby bg.customer_code
                //       select new querylay
                //       {
                //           query0 = bg.customer_code,
                //           query1 = bg.customer_code + "&nbsp;&nbsp;" + bg.cust_biz_name
                //       };
                sqlstr = "select customer_code query0, customer_code+'---'+ cust_biz_name query1 from AR_001_CUSTM order by customer_code";
                hdet = db.Database.SqlQuery<querylay>(sqlstr);

            }
            else if (id == "002")
            {
                //hdet = from bg in db.AP_001_VENDR
                //       orderby bg.vendor_code
                //       select new querylay
                //       {
                //           query0 = bg.vendor_code,
                //           query1 = bg.vendor_code + "&nbsp;&nbsp;" + bg.vend_biz_name
                //       };
                sqlstr = "select vendor_code query0, vendor_code+'---'+ vend_biz_name query1 from AP_001_VENDR order by vendor_code";
                hdet = db.Database.SqlQuery<querylay>(sqlstr);

            }
            else if (id == "004")
            {
                //hdet = from bg in db.BK_001_BANK
                //       orderby bg.bank_code
                //       select new querylay
                //       {
                //           query0 = bg.bank_code,
                //           query1 = bg.bank_code + "&nbsp;&nbsp;" + bg.bank_name
                //       };
                sqlstr = "select bank_code query0, bank_code+'---'+ bank_name query1 from BK_001_BANK order by bank_code";
                hdet = db.Database.SqlQuery<querylay>(sqlstr);

            }
            else if (id == "012")
            {
                //hdet = from bg in db.GB_001_RSONC
                //       orderby bg.consignment_code
                //       select new querylay
                //       {
                //           query0 = bg.consignment_code,
                //           query1 = bg.consignment_code + "&nbsp; &nbsp;" + bg.description
                //       };
                sqlstr = "select consignment_code query0, consignment_code+'---'+ description query1 from GB_001_RSONC order by consignment_code";
                hdet = db.Database.SqlQuery<querylay>(sqlstr);


            }
            else if (id == "005")
            {
                //hdet = from bg in db.WO_002_WKO
                //       orderby bg.work_order_id
                //       select new querylay
                //       {
                //           query0 = bg.work_order_id,
                //           query1 = bg.work_order_id + "&nbsp; &nbsp;" + bg.work_order_description
                //       };
                sqlstr = "select work_order_id query0, work_order_id+'---'+ work_order_description query1 from WO_002_WKO order by work_order_id";
                hdet = db.Database.SqlQuery<querylay>(sqlstr);


            }

            return hdet;
        }

        [HttpPost]
        public ActionResult class_fid(string id)
        {
            // write your query statement
            var hdet = from bg in db.FA_001_ASSET
                       where bg.asset_class == id
                       orderby bg.description
                       select new
                       {
                           c1 = bg.fixed_asset_code,
                           c2 = bg.fixed_asset_code+"---"+bg.description
                       };


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                hdet.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");
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
               
                //string stg1 = "select exchange_rate dquery0  from MC_001_EXCRT where currency_code=" + util.sqlquote(cur_code) + " and date_from <=" + util.sqlquote(curent_date);
                //stg1 += " and date_to>=" + util.sqlquote(curent_date);
                //var bghv = db.Database.SqlQuery<querylay>(stg1).FirstOrDefault();
               // rat_code = bghv == null ? 0 : bghv.dquery0;
               // rate_flag = "Y";
                string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(cur_code) + " and '" + tdate + "' between date_from and date_to";
                var dbexch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                if (dbexch != null)
                    rat_code = dbexch.dquery0;

                ary.Add(new SelectListItem { Value = cur_code, Text = rat_code.ToString() });
            
            }

            //List<SelectListItem> ary = new List<SelectListItem>();
            //ary.Add(new SelectListItem { Value = "1", Text = cur_code });
            //ary.Add(new SelectListItem { Value = "2", Text = rate_flag });
            //ary.Add(new SelectListItem { Value = "3", Text = rat_code.ToString("#,##0.00") });
            //ary.Add(new SelectListItem { Value = "4", Text = curen_name });
            return ary;
        }
      
        private void read_header()
        {
            ModelState.Remove(glay.vwstrarray0[5]);
           // glay.vwstrarray0 = new string[20];
            var bg2list = (from bh in db.GL_002_JONAL
                           join bi in db.GB_999_MSG
                           on new { a1 = bh.journal_type, a2 = "JT" } equals new { a1 = bi.code_msg, a2 = bi.type_msg}
                           into bi1
                           from bi2 in bi1.DefaultIfEmpty()
                           join bj in db.GB_999_MSG
                           on new { a1 = bh.period, a2 = "FYM" } equals new { a1 = bj.code_msg, a2 = bj.type_msg}
                           into bj1
                           from bj2 in bj1.DefaultIfEmpty()
                           where bh.journal_number == qheader.intquery2
                           select new {bh, bi2, bj2 }).FirstOrDefault();


            if (bg2list != null)
            {
                glay.vwint0 = bg2list.bh.journal_number;
                glay.vwstring0 = bg2list.bh.manual_reference_number;
                glay.vwstrarray0[0] = bg2list.bh.journal_type;
                glay.vwint3 = bg2list.bh.number_of_cycle;
                glay.vwint2 = bg2list.bh.cycle_interval;
                if (bg2list.bh.journal_type == "AJ")
                {
                    glay.vwstrarray3[0] = util.date_slash(bg2list.bh.reversing_date);
                    glay.vwstrarray0[6] = bg2list.bh.rev_approval_mode;
                }
                if (bg2list.bh.journal_type == "RJ")
                    glay.vwstrarray0[5] = bg2list.bh.approval_mode;
               // glay.vwstrarray0[1] = bg2list.bh.period;
                //glay.vwstrarray0[2] = bg2list.bh.year;
                glay.vwstrarray0[1] = bg2list.bh.period.Substring(4, 2);
                glay.vwstrarray0[2] = bg2list.bh.period.Substring(0, 4);
                glay.vwstrarray0[4] = bg2list.bh.note;
                glay.vwstrarray0[3] = bg2list.bh.batch_description;
               // glay.vwstrarray0[6] = bg2list.bh.rev_approval_mode;

                string docode = gdoc.journal_number.ToString();
                var bglist = from bg in db.GB_001_DOC
                             where bg.screen_code == "JOURNAL" && bg.document_code == docode
                             orderby bg.document_sequence
                             select bg;

                ViewBag.anapict = bglist.ToList();
                
            }
           // Session["hdisplay"] = glay.vwstrarray0[0];

        }

        private void display_header()
        {

             var bg2list = (from bh in db.GL_002_JONAL
                           join bi in db.GL_001_JONT
                           on new { a1 = bh.journal_type } equals new { a1 = bi.journal_code }
                           into bi1
                           from bi2 in bi1.DefaultIfEmpty()
                           join bj in db.GB_999_MSG
                           on new { a1 = bh.period, a2 = "FYM" } equals new { a1 = bj.code_msg, a2 = bj.type_msg }
                           into bj1
                           from bj2 in bj1.DefaultIfEmpty()
                           where bh.journal_number == qheader.intquery2
                           select new { bh, bi2, bj2 }).FirstOrDefault();
            //vw_genlay glayhead = new vw_genlay();
            //glayhead.vwstrarray1 = new string[20];
            //string bglist = " select  cast(bh.journal_number as varchar) c1, cast(bh.manual_reference_number as varchar) vwstrarray1[1], bi.name1_msg vwstrarray1[2],";
            //bglist += "cast(bh.total_debit as varchar) vwstrarray1[3], cast(bh.total_credit as varchar) as vwstrarray1[4], cast(bh.control as varchar) as vwstrarray1[5],";
            //bglist += " case when bj.name1_msg = 'NULL' then bj.name1_msg + ' ' + bh.year else bh.period + ' ' + bh.year  end vwstrarray1[6] ,";
            //bglist += "cast(bh.number_of_cycle as varchar) vwstrarray1[7], cast(bh.cycle_interval as varchar) vwstrarray1[8], cast(bh.reversing_date as varchar) as vwstrarray1[9],";
            //bglist += " bh.batch_description vwstrarray1[10] from  dbo.GL_002_JONAL bh left  outer join dbo.GB_999_MSG  bi";
            //      bglist +="  on  bh.journal_type =  bi.code_msg and bi.type_msg= 'JT' left outer join dbo.GB_999_MSG bj";
            //      bglist += " on bh.period =  bj.code_msg and bj.type_msg = 'FYM' where bh.journal_number ="+ hbug;
            //      var str11 = db.Database.SqlQuery<vw_genlay>(bglist).FirstOrDefault();




            vw_genlay glayhead = new vw_genlay();
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwstrarray1[0] = bg2list.bh.journal_number.ToString();
                glayhead.vwstrarray1[1] = bg2list.bh.manual_reference_number;
                if (bg2list.bi2 != null)
                    glayhead.vwstrarray1[2] = bg2list.bi2.name;
                else
                    glayhead.vwstrarray1[2] = "";
                glayhead.vwstrarray1[3] = bg2list.bh.total_debit.ToString("#,##0.00");
                glayhead.vwstrarray1[4] = bg2list.bh.total_credit.ToString("#,##0.00");
                glayhead.vwstrarray1[5] = bg2list.bh.control.ToString("#,##0.00");
                if (bg2list.bj2 !=null)
                glayhead.vwstrarray1[6] = bg2list.bj2.name1_msg + " " + bg2list.bh.year;
                else
                    glayhead.vwstrarray1[6] = bg2list.bh.period + " " + bg2list.bh.year;
                glayhead.vwstrarray1[7] = bg2list.bh.number_of_cycle.ToString();
                glayhead.vwstrarray1[8] = bg2list.bh.cycle_interval.ToString();
                glayhead.vwstrarray1[9] = util.date_slash(bg2list.bh.reversing_date);
                // glayhead.vwstrarray1[10] = bg2list.bh.year;
                glayhead.vwstrarray1[10] = bg2list.bh.batch_description;

            }
            ViewBag.x2 = glayhead;
        }


        private void init_header()
        {
            gdoc.manual_reference_number = "";
            gdoc.period = "";
            gdoc.year = "";
            gdoc.journal_type = "";
            gdoc.number_of_cycle = 0;
            gdoc.cycle_interval = 0;
            gdoc.reversing_date = "";
            gdoc.rev_approval_mode = "";
            gdoc.total_debit = 0;
            gdoc.total_credit = 0;
            gdoc.control = 0;
            gdoc.project_code = "";
            gdoc.approval_level = 0;
            gdoc.created_by = "";
            gdoc.approval_by = "";
            gdoc.attach_document = "";
            gdoc.exchange_rate = 0;
            gdoc.modified_by = "";
            gdoc.note = "";
            gdoc.approval_mode = "";
           

        }

 
        private void move_detail()
        {
            glay.vwstrarray6 = new string[20];
            
            //int key1 = Convert.ToInt16(jrnlno);
            //int key2 = Convert.ToInt16(Session["line_num"]);
            ////var bgdtl = (from bg in db.GL_002_JONAD
            //             where bg.journal_number == key1 && bg.sequence_number == key2
            //             select bg).FirstOrDefault();
            glay.vwint4 = GL_002_JONAD.sequence_number;
            glay.vwstring1 = GL_002_JONAD.reference_number;
            glay.vwstring10 = GL_002_JONAD.currency;
            glay.vwstring7 = GL_002_JONAD.account_code_debit;
            glay.vwstring2 = GL_002_JONAD.description;
            glay.vwstring3 = GL_002_JONAD.fixed_asset_code;
            glay.vwstring4 = GL_002_JONAD.fa_transaction_type;
            glay.vwstring5 = GL_002_JONAD.amount_type;
            glay.vwstring6 = GL_002_JONAD.account_type_debit;
            glay.vwdecimal0 = GL_002_JONAD.amount;
            glay.vwdecimal1 = GL_002_JONAD.exchange_rate;
            glay.vwdecimal3 = GL_002_JONAD.base_amount;
            glay.vwstrarray2[0] = util.date_slash(GL_002_JONAD.transaction_date);
            glay.vwstrarray2[1] = GL_002_JONAD.asset_class;
            glay.vwstrarray6[0] = GL_002_JONAD.analysis_code1;
            glay.vwstrarray6[1] = GL_002_JONAD.analysis_code2;
            glay.vwstrarray6[2] = GL_002_JONAD.analysis_code3;
            glay.vwstrarray6[3] = GL_002_JONAD.analysis_code4;
            glay.vwstrarray6[4] = GL_002_JONAD.analysis_code5;
            glay.vwstrarray6[5] = GL_002_JONAD.analysis_code6;
            glay.vwstrarray6[6] = GL_002_JONAD.analysis_code7;
            glay.vwstrarray6[7] = GL_002_JONAD.analysis_code8;
            glay.vwstrarray6[8] = GL_002_JONAD.analysis_code9;
            glay.vwstrarray6[9] = GL_002_JONAD.analysis_code10;

            cur_read();

           string amt_check = "N";

            if (GL_002_JONAD.currency == pubsess.base_currency_code)
                amt_check = "Y";

            psess.temp4 = amt_check;
        
        }

        private void header_ana(string commandn="")
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string [] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            string[] jtype1 = new string[20];
            string[] jtype2 = new string[20];
            SelectList[] head_det = new SelectList[20];

            //Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;
            psess.sarrayt2 = jtype1;
            psess.sarrayt3 = jtype2;

            if (commandn == "D")
            {
                var bglist = from bg in db.GB_001_HEADER
                             
                             where bg.header_type_code == "010" && bg.sequence_no != 99
                             select bg;

                foreach (var item in bglist.ToList())
                {
                    int count2 = item.sequence_no-1;
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

                    var journal_type = (from bf in db.GL_001_JONT
                                        where bf.journal_code == qheader.query1 && bf.sequence_no == item.sequence_no+1
                                        select bf).FirstOrDefault();
                    if (journal_type != null)
                    {
                        jtype1[count2] = journal_type.analysis_code;
                        jtype2[count2] = journal_type.visibility;
                    }

                }

                //get_journal_type();


                psess.sarrayt0 = aheader7;
                psess.sarrayt1 = glay.vwstrarray5;
                psess.sarrayt2 = jtype1;
                psess.sarrayt3 = jtype2;

            }

        }

        private void amount_update()
        {
            string sqlstr = " update GL_002_JONAL set total_debit = a001, total_credit = a002, control = a003" ;
            sqlstr += " from GL_002_JONAL a,  (select  IsNull(sum(case amount_type when 'D' then (base_amount) else 0 end),0) a001, IsNull(sum(case amount_type when 'C' then (base_amount) else 0 end),0) a002, IsNull(sum(case amount_type when 'D' then (base_amount) else 0 end),0) - IsNull(sum(case amount_type when 'C' then (base_amount) else 0 end),0) as a003  from GL_002_JONAD where journal_number=" + qheader.intquery2 +") bx";
            sqlstr += " where journal_number=" + qheader.intquery2;
            db.Database.ExecuteSqlCommand(sqlstr);

        }

        private void year_cal()
        {
            int thisyear=DateTime.Now.Year;
            int startyear=thisyear-10;
            int endyear=thisyear+11;
            List<SelectListItem> ary = new List<SelectListItem>();
            for (int yctr = startyear; yctr < endyear; yctr++)
            {
                ary.Add(new SelectListItem { Value = yctr.ToString(), Text = yctr.ToString() });
            }

            ViewBag.year = ary;

        }

        private void period_basis()
        {
            if (pubsess.period_closing== "C")
            {
                int current_month = DateTime.Now.Month;
                int current_year = DateTime.Now.Year;
                string cur_mth = 0 + current_month.ToString();
                glay.vwstrarray0[1] = cur_mth;
                glay.vwstrarray0[2] = current_year.ToString();
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

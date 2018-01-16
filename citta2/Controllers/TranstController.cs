using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;
using System.Web.Mvc;
using anchor1.Filters;
using anchor1.utilities;
using anchor1.Models;

namespace CittaErp.Controllers
{

    public class TranstController : Controller
    {
        //
        // GET: /Employee/

        AP_002_VTRAD AP_002_VTRAD = new AP_002_VTRAD();
        AP_002_VTRAN gdoc = new AP_002_VTRAN();

        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        queryhead qheader = new queryhead();
        HttpPostedFileBase[] photo1;

        string preseq = "";
        string ptype = "";
        decimal f = 0;
        decimal y = 0;
        bool err_flag = true;
        decimal qty = 0; decimal pricep = 0; decimal tax_amt = 0; decimal net_amt = 0; decimal quote_amt = 0; decimal ext_price = 0;
        decimal net_amt1 = 0; decimal quote_amt1 = 0; decimal qtx = 0;
        decimal taxm = 0; decimal disc_per1 = 0;
        string trnstno = ""; string trnstcd = "";
        string action_flag = "";
        Boolean app_flag;

        [EncryptionActionAttribute]
        public ActionResult Index(string pc)
        {

            if (pc == null)
                return RedirectToAction("Welcome", "log_in");

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            if (pc != "")
            {
                psess.temp0 = pc;
                ptype = pc;
                header_rtn();
            }

            ptype = psess.temp0.ToString();

            if (ptype == "")
                error_message();

            var bglist = from bh in db.AP_002_VTRAN
                         join bj in db.GB_999_MSG
                           on new { a1 = bh.transaction_date, a2 = "FYM" } equals new { a1 = bj.code_msg, a2 = bj.type_msg }
                           into bj1
                           from bj2 in bj1.DefaultIfEmpty()
                         where bh.module_account_type == ptype && bh.processed==0
                         select new vw_genlay
                         {
                             vwint0 = bh.document_number,
                             vwstring0 = bh.reference_number,
                             vwdecimal0 = bh.total_amount,
                             vwstring1 = bh.transaction_code + " "+ bh.transaction_type,
                             vwstring2 = bh.batch_information,
                              vwstring3 = "Open",
                            vwstring4 = bj2.name1_msg != null ? bj2.name1_msg + " " + bh.transaction_date : bh.period,
                            vwstring5 = bh.created_by
                         };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult CreateHeader(string pc)
        {
            if (pc == null)
                return RedirectToAction("Welcome", "log_in");


            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            pubsess = (pubsess)Session["pubsess"];

            initial_rtn();
            period_basis();

            select_query_head("H");
            glay.vwstring8 = "H";

            
            return View("EditDetails",glay);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHeader(vw_genlay glay_in, HttpPostedFileBase[] photofiles, string headtype)
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            glay = glay_in;
            photo1 = photofiles;
            glay.vwstring8 = "H";

            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails", null, new { anc = Ccheckg.convert_pass2("headtype=1") });
            }

            select_query_head("H");  

            return View("EditDetails", glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditHeader(string pc)
        {
            if (pc == null)
                return RedirectToAction("Welcome", "log_in");

            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            pubsess = (pubsess)Session["pubsess"];
            qheader = (queryhead)Session["qheader"];
            initial_rtn();
            glay.datmode = "E";

            glay.vwstring8 = "H";
            gdoc = db.AP_002_VTRAN.Find(qheader.intquery0);
            if (gdoc != null)
                read_header();

            select_query_head("H");

            return View("EditDetails", glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHeader(vw_genlay glay_in, HttpPostedFileBase[] photofile, string headtype)
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            glay = glay_in;
            photo1 = photofile;
            glay.datmode = "E";
            glay.vwstring8 = "H";

            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails", null, new { anc = Ccheckg.convert_pass2("headtype=1") });
            }

            select_query_head("H");

            return View("EditDetails", glay);

        }

        [EncryptionActionAttribute]
        public ActionResult CreateDetails(string headtype)
        {
            if (headtype == null)
                return RedirectToAction("Welcome", "log_in");

            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            qheader = (queryhead)Session["qheader"];

            initial_rtn();
            header_ana("D");
            select_query_head("D");
            glay.vwstring8 = "D";

            detail_intial();
            glay.vwstrarray2[0] = pubsess.base_currency_description;
            glay.vwstring9 = pubsess.exchange_editable;

            show_screen_info();
            
            return View("EditDetails", glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetails(vw_genlay glay_in, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            glay = glay_in;
            glay.vwstring8 = "D";

            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails", null, new { anc = Ccheckg.convert_pass2("headtype=1") });
            }

            glay.vwlist0 = new List<querylay>[20];
            header_ana("D");
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray2 = new string[20];
            show_screen_info();
            select_query_head("D");
            cur_read();

            return View("EditDetails", glay);

        }


        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {
            int key2 = 0;
            int.TryParse(key1, out key2);
            ViewBag.action_flag = "Create";
            
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            

            gdoc = db.AP_002_VTRAN.Find(key2);
            if (gdoc != null)
            {
                set_qheader(); 
                return RedirectToAction("CreateDetails", null, new { anc = Ccheckg.convert_pass2("headtype=1") });
            }

            return View("EditDetails", glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditDetails(string key1)
        {
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ptype = psess.temp0.ToString();
            glay.vwstring9 = pubsess.exchange_editable;

            int key2 = 0;
            int.TryParse(key1, out key2);
            AP_002_VTRAD = db.AP_002_VTRAD.Find(qheader.intquery0, key2);
            if (AP_002_VTRAD == null)
                 return View(glay);

           
            initial_rtn();
            detail_intial();
            move_detail();
            header_ana("D");
            glay.vwstring8 = "D";
            select_query_head("D");
            show_screen_info();

            return View("EditDetails", glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails(vw_genlay glay_in, string headtype, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            ptype = psess.temp0.ToString();

            glay = glay_in;
            glay.vwstring8 = "D";
            if (id_xhrt=="D")
            {
                delete_record();
                return RedirectToAction("Index", null, new { anc = Ccheckg.convert_pass2("pc= ") });
            }

            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails", null, new { anc = Ccheckg.convert_pass2("headtype=1") });
            }

            select_query_head("D");
            detail_intial();
            show_screen_info();
            header_ana();

            return View("EditDetails", glay);

        }

        private void delete_record()
        {
            AP_002_VTRAD = db.AP_002_VTRAD.Find(glay.vwint0, glay.vwint1);
            if (AP_002_VTRAD != null)
            {
                db.AP_002_VTRAD.Remove(AP_002_VTRAD);
                db.SaveChanges();
            }
        }
        private void detail_intial()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray2 = new string[20];
            glay.vwstring1 = qheader.query0;
            glay.vwstring7 = pubsess.base_currency_code;
            glay.vwstring3 = qheader.query6;
            glay.vwstring10 = "N";
            glay.vwstring9 = pubsess.price_editable;

        }

        private void update_file(string headtype)
        {
            err_flag = true;
            validation_routine(headtype);

            if (app_flag)
                approval_rtn();

            if (err_flag)
                update_record(headtype);

        }

        private void update_record(string headtype)
        {
            string cr_flag = action_flag;

            if (cr_flag.IndexOf("Header") > 0)
            {
                if (cr_flag == "CreateHeader")
                {
                    gdoc = new AP_002_VTRAN();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;
                    gdoc.processed = 0;
                    gdoc.approval_date = DateTime.UtcNow;
                }
                else
                {
                    gdoc = db.AP_002_VTRAN.Find(qheader.intquery0);
                }
                
                gdoc.transaction_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : (glay.vwstrarray0[1]);
                gdoc.reference_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : (glay.vwstrarray0[0]);
                gdoc.module_account_type = ptype;
                gdoc.period = glay.vwstrarray0[3] + glay.vwstrarray0[2];
                gdoc.transaction_date = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : (glay.vwstrarray0[3]);
                gdoc.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : (glay.vwstrarray0[4]);
                gdoc.batch_information = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : (glay.vwstrarray0[5]);
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
                    AP_002_VTRAD = new AP_002_VTRAD();
                    AP_002_VTRAD.created_by = pubsess.userid;
                    AP_002_VTRAD.created_date = DateTime.UtcNow;
                    glay.vwint4 = get_next_line_sequence();             
                    
                }
                else
                {
                    AP_002_VTRAD = db.AP_002_VTRAD.Find(qheader.intquery0, glay.vwint4);
                }

                  //the detailsub button has been clicked

                AP_002_VTRAD.document_number = qheader.intquery0;
                AP_002_VTRAD.sequence_number = glay.vwint4;
                AP_002_VTRAD.module_account_type = psess.temp0.ToString();
                AP_002_VTRAD.transaction_code = qheader.query1;
                AP_002_VTRAD.period = qheader.query3;
                AP_002_VTRAD.reference_number = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : (glay.vwstring1);
                AP_002_VTRAD.description = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : (glay.vwstring2);
                AP_002_VTRAD.amount = glay.vwdecimal0;
                AP_002_VTRAD.transaction_type = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : (glay.vwstring4);
                AP_002_VTRAD.account_type = string.IsNullOrWhiteSpace(glay.vwstring5)? "":(glay.vwstring5);
                AP_002_VTRAD.account_code = string.IsNullOrWhiteSpace(glay.vwstring6)? "": (glay.vwstring6);
                AP_002_VTRAD.transaction_date = util.date_yyyymmdd(glay.vwstring3);
                AP_002_VTRAD.exchange_rate = glay.vwdecimal1;
                AP_002_VTRAD.currency = string.IsNullOrWhiteSpace(glay.vwstring7)?"":(glay.vwstring7);
                AP_002_VTRAD.base_amount = glay.vwdecimal0 * glay.vwdecimal1;

                if (pubsess.base_currency_code == AP_002_VTRAD.currency)
                {
                    AP_002_VTRAD.exchange_rate = 1;
                    AP_002_VTRAD.base_amount = AP_002_VTRAD.amount;
                }
                else
                {
                    if (glay.vwstring9 != "Y")
                    {
                        string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(AP_002_VTRAD.currency) + " and '" + AP_002_VTRAD.transaction_date + "' between date_from and date_to";
                        var exch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                        if (exch != null)
                            AP_002_VTRAD.exchange_rate = exch.dquery0;
                    }
                }

                string amt_type = (from bf in db.GB_001_PCODE
                                   where bf.parameter_type == "16" && bf.con_state_link == ptype && bf.parameter_code == AP_002_VTRAD.transaction_type
                                   select bf.gl_account_code).FirstOrDefault();
               
                AP_002_VTRAD.amount_type = amt_type;
                
                AP_002_VTRAD.analysis_code1 = "";
                AP_002_VTRAD.analysis_code2 = "";
                AP_002_VTRAD.analysis_code3 = "";
                AP_002_VTRAD.analysis_code4 = "";
                AP_002_VTRAD.analysis_code5 = "";
                AP_002_VTRAD.analysis_code6 = "";
                AP_002_VTRAD.analysis_code7 = "";
                AP_002_VTRAD.analysis_code8 = "";
                AP_002_VTRAD.analysis_code9 = "";
                AP_002_VTRAD.analysis_code10 = "";
                AP_002_VTRAD.modified_date = DateTime.UtcNow;
                AP_002_VTRAD.modified_by = pubsess.userid;

                if (glay.vwstrarray6 != null)
                {
                    int arrlen = glay.vwstrarray6.Length;
                    if (arrlen > 0)
                        AP_002_VTRAD.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                    if (arrlen > 1)
                        AP_002_VTRAD.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                    if (arrlen > 2)
                        AP_002_VTRAD.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                    if (arrlen > 3)
                        AP_002_VTRAD.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                    if (arrlen > 4)
                        AP_002_VTRAD.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                    if (arrlen > 5)
                        AP_002_VTRAD.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                    if (arrlen > 6)
                        AP_002_VTRAD.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                    if (arrlen > 7)
                        AP_002_VTRAD.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                    if (arrlen > 8)
                        AP_002_VTRAD.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                    if (arrlen > 9)
                        AP_002_VTRAD.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                    psess.intemp0 = arrlen;
                    Session["psess"] = psess;
            
                }
                if (cr_flag == "CreateDetails")
                    db.Entry(AP_002_VTRAD).State = EntityState.Added;
                else
                    db.Entry(AP_002_VTRAD).State = EntityState.Modified;
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


            if (err_flag && cr_flag.IndexOf("Header") < 0)
            {
                qheader.query0 = AP_002_VTRAD.reference_number;
                qheader.query6 = util.date_slash(AP_002_VTRAD.transaction_date);
                Session["qheader"] = qheader;
                amount_update();

                //if (AP_002_VTRAD.account_type == "003")
                //{
                //    string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, AP_002_VTRAD b where a.account_code = b.account_code";
                //    str += " and document_number =" + glay.vwint0;
                //    //db.Database.ExecuteSqlCommand(str);
               
                //    amount_update();
                //}
                //else if (AP_002_VTRAD.account_type == "002")
                //{
                //    string str = "update AP_001_VENDR set delete_flag ='Y' from AP_001_VENDR a, AP_002_VTRAD b where a.account_code = b.account_code";
                //    str += " and document_number =" + glay.vwint0;
                //    //db.Database.ExecuteSqlCommand(str);
                //    amount_update();
                //}
                //else if (AP_002_VTRAD.account_type == "001")
                //{
                //    string str = "update AR_001_CUSTM set delete_flag ='Y' from AR_001_CUSTM a, AP_002_VTRAD b where a.account_code = b.account_code";
                //    str += " and document_number =" + glay.vwint0;
                //    //db.Database.ExecuteSqlCommand(str);
                //    amount_update();
                //}
                //else if (AP_002_VTRAD.account_type == "004")
                //{
                //    string str = "update BK_001_BANK set delete_flag ='Y' from BK_001_BANK a, AP_002_VTRAD b where a.account_code = b.account_code";
                //    str += " and document_number =" + glay.vwint0;
                //    //db.Database.ExecuteSqlCommand(str);
                //    amount_update();
                //}
                //else if (AP_002_VTRAD.account_type == "012")
                //{
                //    string str = "update GB_001_RSONC set delete_flag ='Y' from GB_001_RSONC a, AP_002_VTRAD b where a.account_code = b.account_code";
                //    str += " and document_number =" + glay.vwint0;
                //    //db.Database.ExecuteSqlCommand(str);
                //    amount_update();
                //}
                //else
                //{
                //    amount_update();
                //}

                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, AP_002_VTRAD b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and document_number =" + glay.vwint0;
                ////db.Database.ExecuteSqlCommand(stri);
            }

            if (err_flag && cr_flag.IndexOf("Header") > 0)
            {
                set_qheader();
                if (err_flag && photo1 != null)
                    util.write_document(psess.temp0.ToString(), gdoc.document_number.ToString(), photo1, glay.vwstrarray9);               
            }

            if (err_flag && headtype == "send_app" && glay.vwstring8=="D")
                approval_rtn();
            
        }

        private int get_next_line_sequence()
        {
            string sqlstr = "select isnull(max(sequence_number),0) vwint1 from AP_002_VTRAD where reference_number=" + util.sqlquote(glay.vwstring1);
            var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
            return sql1.vwint1 + 1;
                    
        }
        private void set_qheader()
        {
            qheader.query1 = gdoc.transaction_code;
            qheader.query0 = gdoc.reference_number;
            qheader.query2 = gdoc.transaction_type;
            qheader.intquery0 = gdoc.document_number;
            qheader.query3 = gdoc.period;
            qheader.query4 = gdoc.transaction_date;
            qheader.query5 = gdoc.batch_information;
            qheader.query6 = DateTime.Now.ToString("dd/MM/yyyy");
            Session["qheader"] = qheader;
        }
    
        private void validation_routine(string headtype)
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            string prd_chk = pubsess.period_closing;
            aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            app_flag = false;

            string cr_flag = action_flag;

            if (headtype == "send_app" && glay.vwstring8 == "D" && glay.vwdecimal0 == 0)
            {
                err_flag = true;
                app_flag = true;
                return;
            }

            if (cr_flag.IndexOf("Details") > 0)
            {
                string transdatechk = "Y";

                if (!util.date_validate(glay.vwstring3))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a Valid transaction  date");
                    err_flag = false;
                    transdatechk = "N";
                }

                if (prd_chk == "C")
                {
                    int year = Convert.ToInt32(qheader.query4); int month = Convert.ToInt32(qheader.query3.Substring(4));
                    int current_day = DateTime.Now.Day;
                    var firstDayOfMonth = new DateTime(year, month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    string startday = qheader.query3 + "0" + firstDayOfMonth.Day.ToString();
                    string endday = qheader.query3 + lastDayOfMonth.Day.ToString();
                    if (transdatechk == "Y")
                    {
                        string tra_date = util.date_yyyymmdd(glay.vwstring3);
                        if (Convert.ToInt32(tra_date) < Convert.ToInt32(startday) || Convert.ToInt32(tra_date) > Convert.ToInt32(endday))
                        {
                            ModelState.AddModelError(String.Empty, "Transaction date must be within current active period");
                            err_flag = false;
                        }
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
                    string tra_date = util.date_yyyymmdd(glay.vwstring3);
                    if (Convert.ToInt32(tra_date) < Convert.ToInt32(startday) || Convert.ToInt32(tra_date) > Convert.ToInt32(endday))
                    {
                        ModelState.AddModelError(String.Empty, "Transaction date must be within current active period");
                        err_flag = false;
                    }
                }


                if (glay.vwstring5 == null)
                {
                    ModelState.AddModelError(String.Empty, "Enter Posting Class");
                    err_flag = false;
                }
                if (glay.vwstring5 != null && glay.vwstring6 == null)
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

            //if (headtype == "send_app")
            //{
            //    initial_rtn();
            //    int hbug = 0;
            //    int.TryParse(trnstno, out hbug);
            //    decimal contrl_check = (from bg in db.GL_002_JONAL
            //                            where bg.journal_number == hbug
            //                            select bg.control).FirstOrDefault();
            //    glay.vwstrarray0[5] = contrl_check.ToString();
            //    if (glay.vwstrarray0[5] != "0")
            //    {
            //        ModelState.AddModelError(String.Empty, "Total Credit must be equal to Total Debit");
            //        err_flag = false;
            //    }

            //}

        }
        private void select_query_head(string type1)
        {
           
            if (type1 == "H")
            {

                year_cal();

                string str1 = "select bank_code query0, c2 query1 from vw_allcust where qcode=" + util.sqlquote(ptype);
                var emp1 = db.Database.SqlQuery<querylay>(str1);
                ViewBag.code = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[1]);

                string disckf = pubsess.valid_datefrm.Substring(4, 2);
                string disckt = pubsess.valid_dateto.Substring(4, 2);
                string curent_mth = pubsess.curent_datefrm.Substring(4, 2);

                if (pubsess.period_closing == "C")
                {
                    string srt = "select distinct query0, query1 from (";
                    srt += " select code_msg query0, name1_msg query1 from  dbo.GB_999_MSG pf where pf.type_msg = 'FYM'and  code_msg =" + util.sqlquote(curent_mth) + "union all";
                    srt += " select code_msg query0, name1_msg query1  from dbo.GB_999_MSG pg where pg.type_msg = 'FYM' and code_msg between" + util.sqlquote(disckf) + " and" + util.sqlquote(disckt) + ") b order by 2";
                    var bf2 = db.Database.SqlQuery<querylay>(srt).ToList();
                    ViewBag.month = new SelectList(bf2.ToList(), "query0", "query1", glay.vwstrarray0[2]);
                }
                else
                {   
                    string srt = "select distinct query0, query1 from (";
                    srt = " select period_number query0, prd_description query1 from  dbo.GB_001_PCT pf where period_number =" + util.sqlquote(curent_mth) + "union all";
                    srt += " select period_number query0, prd_description query1  from dbo.GB_001_PCT pg where period_number between" + util.sqlquote(disckf) + " and" + util.sqlquote(disckt) + ") b order by 2";
                    var bf2 = db.Database.SqlQuery<querylay>(srt).ToList();
                    ViewBag.month = new SelectList(bf2.ToList(), "query0", "query1", glay.vwstrarray0[2]);

                }

            }
            if (type1 == "D")
            {
                string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
                var emp1 = db.Database.SqlQuery<querylay>(str1);

                ViewBag.cur = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring7);

                var empi = from pf in db.GB_999_MSG
                           where pf.type_msg == "HEAD" && pf.name6_msg == "P"
                           orderby pf.name1_msg
                           select pf;
                ViewBag.debit = new SelectList(empi.ToList(), "code_msg", "name1_msg", glay.vwstring5);


                var empz = from bf in db.GB_001_PCODE
                              where bf.parameter_type == "16" && bf.con_state_link== ptype
                              select bf;
                ViewBag.type = new SelectList(empz.ToList(), "parameter_code", "parameter_name", glay.vwstring4);

                 str1 = "select bank_code query0,  bank_code + ' - '+bank_name query1 from vw_allcust where qcode=" + util.sqlquote(glay.vwstring5) +" order by bank_name";
                 emp1 = db.Database.SqlQuery<querylay>(str1);
                ViewBag.acode = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring6);
               
            }

        }


        private void initial_rtn()
        {
            glay.vwdclarray0 = new decimal[50];
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
            glay.vwstrarray2 = new string[20];
            glay.vwstrarray6 = new string[20];
            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwstrarray0[wctr] = "";

            glay.vwstrarray0[2] = "0" + DateTime.Now.Month.ToString();
            glay.vwlist0 = new List<querylay>[20];
        

        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AP_002_VTRAN] where cast (document_number as varchar)="+ util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "delete from [dbo].[AP_002_VTRAD] where cast(document_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index", null, new { anc = Ccheckg.convert_pass2("pc = ") });
        }

        public ActionResult delete_detail(string id)
        {
            // write your query statement
            //glay.vwstring3 = psess.temp0.ToString();
            qheader = (queryhead)Session["qheader"];
            string sqlstr = "delete from [dbo].[AP_002_VTRAD] where cast(document_number as varchar) +'[]'+ cast(sequence_number as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            amount_update();
            return RedirectToAction("CreateDetails", null, new { anc = Ccheckg.convert_pass2("headtype=1") });
            
        }

        private void read_details()
        {
            string str1 = "select ";
            str1 += "vwstring0 = bh.module_account_type,";
            str1 += " vwstring2 = bh.transaction_code,";
            str1 += " vwint0 = bh.document_number, ";
            str1 += " vwint1 = bh.sequence_number, ";
            str1 += " vwstring6 = bh.account_type, ";
            str1 += " vwstring7 = dbo.find_name('1',bh.account_type,bh.account_code), ";
            str1 += " vwstring3 = bh.description, ";
            str1 += " vwstring1 = bh.reference_number, ";
            str1 += " vwdecimal0 = bh.amount, ";
            str1 += " vwdecimal3 = bh.base_amount, ";
            str1 += " vwstring4 =  dbo.find_name('6','16',bh.transaction_type), ";
            str1 += " vwstring5 = dbo.find_name('2','',bh.currency), ";
            str1 += " vwstring8 = bh.amount_type, ";
            str1 += " vwstring10 = bh.account_code ";
            str1 += " from AP_002_VTRAD bh where bh.document_number = " + qheader.intquery0.ToString();
            ViewBag.x1 = db.Database.SqlQuery<vw_genlay>(str1).ToList();

        }

        private void show_screen_info()
        {
            display_header();
            read_details();

        }

        [HttpPost]
        public ActionResult budget_cal(string item_amt, string tax_code, string tax_amt)
        {
            
            decimal.TryParse(item_amt, out qty);
            string  pricep = tax_code;
            decimal.TryParse(tax_amt, out taxm);

            var bgassign = (from bg in db.GB_001_TAX
                            where bg.tax_code == tax_code
                            select bg).FirstOrDefault();

            if (bgassign != null)
            {
                decimal rel_tax = bgassign.tax_rate;

                re_cal(item_amt, tax_code, tax_amt, rel_tax.ToString());
            }
             net_amt = qty + taxm;

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = taxm.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "2", Text = net_amt.ToString("#,##0.00") });

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index", null, new { anc = Ccheckg.convert_pass2("pc = ") });

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

        private void read_header()
        {
            var bg2list = (from bh in db.AP_002_VTRAN
                           where bh.document_number == qheader.intquery0
                           select new { bh}).FirstOrDefault();

            if (bg2list != null)
            {
              
                glay.vwint0 = bg2list.bh.document_number;
                glay.vwstrarray0[1] = bg2list.bh.transaction_code;
                glay.vwstrarray0[0] = bg2list.bh.reference_number;
                glay.vwstrarray0[2] = bg2list.bh.period.Substring(4,2);
                glay.vwstrarray0[3] = bg2list.bh.period.Substring(0, 4);
                glay.vwstrarray0[4] = bg2list.bh.note;
                glay.vwstrarray0[5] = bg2list.bh.batch_information;

                //string docode = bg2list.bh.document_number.ToString();
                //string scrncode = psess.temp0.ToString();
                //var bglist = from bg in db.GB_001_DOC
                //             where bg.screen_code == scrncode  && bg.document_code == docode
                //             orderby bg.document_sequence
                //             select bg;

                //ViewBag.anapict = bglist.ToList();

                ViewBag.atfile = util.read_attachmentp(psess.temp0.ToString(), bg2list.bh.document_number.ToString());
                glay.imagecat = "P";
               // glay.imagedesc = (from bg in db.tab_bank where bg.para_code == "DOCMSG" && bg.bank_code == type_code select bg.name1).FirstOrDefault();
            }

        }

        private void display_header()
        {
            var bg2list = (from bh in db.AP_002_VTRAN
                           where bh.document_number == qheader.intquery0
                           select new { bh }).FirstOrDefault();
           
            vw_genlay glayhead = new vw_genlay();
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwstrarray1[0] = bg2list.bh.transaction_code;
                glayhead.vwstrarray1[1] = bg2list.bh.document_number.ToString();
                glayhead.vwstrarray1[2] = bg2list.bh.reference_number;
                glayhead.vwstrarray1[3] = bg2list.bh.total_amount.ToString("#,##0.00");
                string sqlstr = "select  IsNull(sum(case amount_type when 'D' then (base_amount) else 0 end),0) dbt, IsNull(sum(case amount_type when 'C' then (base_amount) else 0 end),0) crdt from AP_002_VTRAD where document_number=" + qheader.intquery0;
               var str = db.Database.SqlQuery<addr>(sqlstr).FirstOrDefault();
                decimal cntrl = str.dbt - str.crdt;
               glayhead.vwstrarray1[4] = str.dbt.ToString("#,##0.00");
               glayhead.vwstrarray1[5] = str.crdt.ToString("#,##0.00");
                glayhead.vwstrarray1[6] = cntrl.ToString();
                glayhead.vwstrarray1[7] = bg2list.bh.batch_information;
               
                string str1 = "select bank_code + ' - ' + bank_name query0 from vw_allcust where qcode=" + util.sqlquote(bg2list.bh.module_account_type);
                str1 += " and bank_code=" + util.sqlquote(bg2list.bh.transaction_code);
                var bh2 = db.Database.SqlQuery<querylay>(str1).FirstOrDefault();
                if(bh2 != null)
                glayhead.vwstrarray1[0] = bh2.query0;

            }
            ViewBag.x2 = glayhead;


        }

        private void init_header()
        {
            gdoc.module_account_type = "";
            gdoc.period = "";
            gdoc.transaction_code = "";
            gdoc.reference_number = "";
            gdoc.transaction_type = "";
            gdoc.transaction_date = "";
            gdoc.total_amount = 0;
            gdoc.processed = 0;
            gdoc.created_by = "";
            gdoc.approval_by = "";
            gdoc.modified_by = "";
            gdoc.note = "";
           

        }
        [HttpPost]
        public ActionResult trancode(string typecode, string id, string ac_code1 = "", string curren = "", string tdate="")
        {
            psess = (psess)Session["psess"];
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
                string str1 = "select bank_code query0,  bank_code + ' - '+bank_name query1 from vw_allcust where qcode=" + util.sqlquote(id);
                var hdet = db.Database.SqlQuery<querylay>(str1);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    hdet.ToArray(),
                                    "query0",
                                    "query1")
                               , JsonRequestBehavior.AllowGet);
            }
            else if (typecode == "03")
            {
                List<SelectListItem> ary = new List<SelectListItem>();

                ary = strancode(id);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary.ToArray(),
                                    "Value",
                                    "Text")
                               , JsonRequestBehavior.AllowGet);

            }

            return RedirectToAction("Index", null, new { anc = Ccheckg.convert_pass2("pc = ") });

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

        private List<SelectListItem> strancode(string stype)
        {
            List<SelectListItem> ary = new List<SelectListItem>();
            string scode = "TTY" + psess.temp0.ToString();
            string transtype = (from bf in db.GB_001_PCODE
                                where bf.parameter_type == "16" && bf.con_state_link == ptype && bf.parameter_code == stype
                                select bf.gl_account_code).FirstOrDefault();
            if (!(string.IsNullOrWhiteSpace(transtype)))
            {
                string str1 = "select count(0) intquery0 from GB_999_MSG where type_msg='HEAD' and code_msg in (" + (transtype) + ")";
                var bh = db.Database.SqlQuery<querylay>(str1).FirstOrDefault();
                int cot1 = bh.intquery0;

                if (cot1 > 0)
                {
                    str1 = "select code_msg query0, name1_msg query1 from GB_999_MSG where type_msg='HEAD' and code_msg in (" + (transtype) + ")";
                    var bh1 = db.Database.SqlQuery<querylay>(str1).ToList();
                    foreach (var item in bh1)
                    {
                        ary.Add(new SelectListItem { Value = item.query0, Text = item.query1 });
                    }
                    ary.Add(new SelectListItem { Value = "99X", Text = "postcode" });
                }

                if (cot1 == 1)
                {
                    str1 = "select bank_code query0, bank_name query1 from vw_allcust where qcode in (" + (transtype) + ")";
                    var bh2 = db.Database.SqlQuery<querylay>(str1).ToList();
                    foreach (var item in bh2)
                    {
                        ary.Add(new SelectListItem { Value = item.query0, Text = item.query1 });
                    }
                    ary.Add(new SelectListItem { Value = "99Z", Text = "actcode" });

                }
            }
            else
            {
                ary.Add(new SelectListItem { Value = "", Text = "" });
                string str1 = "select code_msg query0, name1_msg query1 from GB_999_MSG where type_msg='HEAD' ";
                var bh1 = db.Database.SqlQuery<querylay>(str1).ToList();
                foreach (var item in bh1)
                {
                    ary.Add(new SelectListItem { Value = item.query0, Text = item.query1 });
                }
                ary.Add(new SelectListItem { Value = "99X", Text = "postcode" });

                ary.Add(new SelectListItem { Value = "", Text = "" });
                ary.Add(new SelectListItem { Value = "99Z", Text = "actcode" });
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
            return RedirectToAction("Index", null, new { anc = Ccheckg.convert_pass2("pc = ") });
        }

        private void move_detail()
        {
            string amt_check = "";
            glay.vwstrarray6 = new string[20];
            glay.vwint4 = AP_002_VTRAD.sequence_number;
            glay.vwstring1 = AP_002_VTRAD.reference_number;
            glay.vwstring7 = AP_002_VTRAD.currency;
            glay.vwstring6 = AP_002_VTRAD.account_code;
            glay.vwstring2 = AP_002_VTRAD.description;
            glay.vwdecimal1 = AP_002_VTRAD.exchange_rate;
            glay.vwstring3 = util.date_slash(AP_002_VTRAD.transaction_date);
            glay.vwstring4 = AP_002_VTRAD.transaction_type;
            glay.vwstring5 = AP_002_VTRAD.account_type;
            glay.vwdecimal0 = AP_002_VTRAD.amount;
            glay.vwdecimal2 = AP_002_VTRAD.base_amount;

            glay.vwstrarray6[0] = AP_002_VTRAD.analysis_code1;
            glay.vwstrarray6[1] = AP_002_VTRAD.analysis_code2;
            glay.vwstrarray6[2] = AP_002_VTRAD.analysis_code3;
            glay.vwstrarray6[3] = AP_002_VTRAD.analysis_code4;
            glay.vwstrarray6[4] = AP_002_VTRAD.analysis_code5;
            glay.vwstrarray6[5] = AP_002_VTRAD.analysis_code6;
            glay.vwstrarray6[6] = AP_002_VTRAD.analysis_code7;
            glay.vwstrarray6[7] = AP_002_VTRAD.analysis_code8;
            glay.vwstrarray6[8] = AP_002_VTRAD.analysis_code9;
            glay.vwstrarray6[9] = AP_002_VTRAD.analysis_code10;

            cur_read();

            amt_check = "N";

            if (AP_002_VTRAD.currency == pubsess.base_currency_code)
                amt_check = "Y";

            psess.temp4 = amt_check;
            Session["psess"] = psess;
            
        }

        private void header_ana(string commandn = "")
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            SelectList[] head_det = new SelectList[20];

            psess.sarrayt1 = aheader5;

            //if (commandn == "D")
            {
                //var bglist = from bg in db.GB_001_HEADER
                //             where bg.header_type_code == "010" && bg.sequence_no != 99
                //             select bg;
                //string str = " select a.sequence_no intquery0, a.mandatory_flag query0, a.header_code query1, b.header_description query2 from ";
                //str += " GB_001_HEADER a, GB_001_HANAL b where a.header_code = b.header_sequence  and header_type_code = '010' order by sequence_no ";
                //var bglist = db.Database.SqlQuery<vw_query>(str);
                //foreach (var item in bglist.ToList())
                //{
                //    int count2 = item.sequence_no - 1;
                //    aheader7[count2] = item.mandatory_flag;
                //    glay.vwstrarray4[count2] = item.header_code;
                //    var bglist2 = (from bg in db.GB_001_HANAL
                //                   where bg.header_sequence == item.header_code
                //                   select bg).FirstOrDefault();

                //    if (bglist2 != null)
                //    {
                //        glay.vwstrarray5[count2] = bglist2.header_description;
                //        string str = " select analysis_code query0, analysis_description query1 from GB_001_DANAL where header_sequence = ";
                //        str += util.sqlquote(item.header_code);
                //        var str1 = db.Database.SqlQuery<querylay>(str);
                //        glay.vwlist0[count2] = str1.ToList();

                //    }

                string str2 = " select a.sequence_no intquery0, a.mandatory_flag query0, a.header_code query1, b.header_description query2 from ";
                str2 += " GB_001_HEADER a, GB_001_HANAL b where a.header_code = b.header_sequence  and header_type_code = '010' order by sequence_no ";
                var bglist4 = db.Database.SqlQuery<querylay>(str2);
                foreach (var item in bglist4.ToList())
                {
                    int count2 = item.intquery0 - 1;
                    aheader7[count2] = item.query0;
                    glay.vwstrarray4[count2] = item.query1;
                    glay.vwstrarray5[count2] = item.query2;
                    string str = " select analysis_code query0, analysis_description query1 from GB_001_DANAL where header_sequence = ";
                    str += util.sqlquote(item.query1);
                    var str1 = db.Database.SqlQuery<querylay>(str);
                    glay.vwlist0[count2] = str1.ToList();

                }
            }

            psess.sarrayt0 = aheader7;
            psess.sarrayt1 = glay.vwstrarray5;
        

    }

        private void amount_update()
        {
            string sqlstr = " update AP_002_VTRAN set total_amount = a001";
            sqlstr += " from AP_002_VTRAN a, (select isnull(sum( base_amount),0) a001 from AP_002_VTRAD where document_number=" + +qheader.intquery0 +") bx";
            sqlstr += " where document_number=" + +qheader.intquery0;
            db.Database.ExecuteSqlCommand(sqlstr);

        }

        private void cur_read()
        {
            string curcode = "";
            curcode = glay.vwstring7;
            string read_cur = (from bg in db.MC_001_CUREN
                               where bg.currency_code == curcode
                               select bg.currency_description).FirstOrDefault();

            glay.vwstrarray2[0] = read_cur;
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

        private void period_basis()
        {
            if (pubsess.period_closing == "C")
            {
                int current_month = DateTime.Now.Month;
                int current_year = DateTime.Now.Year;
                string cur_mth = 0 + current_month.ToString();
                glay.vwstrarray0[2] = cur_mth;
                glay.vwstrarray0[3] = current_year.ToString();
            }
        }

        private void header_rtn()
        {
            var hlabel = (from bh in db.GB_999_MSG
                          where bh.type_msg == "HEAD" && bh.code_msg == ptype && bh.name6_msg == "P"
                         select bh).FirstOrDefault();


                psess.temp1 = hlabel.name1_msg;
                psess.temp2 = hlabel.name2_msg;
                psess.ptitle = hlabel.name3_msg;
                Session["psess"] = psess;
            
        }

        public ActionResult show_doc(int id)
        {
            var bglist = (from bg in db.GB_001_DOC
                          where bg.document_sequence == id
                          select bg).FirstOrDefault();


            byte[] imagedata = bglist.document_image;
            return File(imagedata, "png");
        }

        private void approval_rtn()
        {
            string str = "update AP_002_VTRAN set approval_level=1 where document_number=" + qheader.intquery0;
            db.Database.ExecuteSqlCommand(str);

            RedirectToAction("CreateHeader", null, new { anc = Ccheckg.convert_pass2("pc=1") });
        }


    }
}

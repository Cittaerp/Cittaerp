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


namespace CittaErp.Controllers
{

    public class OrderiController : Controller
    {
        //
        // GET: /Employee/

        AR_002_SODT AR_002_SODT = new AR_002_SODT();
        AR_002_SALES gdoc = new AR_002_SALES();
        MainContext db = new MainContext();
        MainContext db1 = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        discountfsp  fsp = new discountfsp();
        HttpPostedFileBase[] photo1;

        string preseq = "";
        string ptype = "";
        decimal f = 0;
        decimal y = 0;
        bool err_flag = true;
        decimal qty = 0; decimal pricep = 0; decimal tax_amt = 0; decimal net_amt = 0; decimal quote_amt = 0; decimal ext_price = 0;
        decimal net_amt1 = 0; decimal quote_amt1 = 0; decimal qtx = 0; decimal disc; decimal taxinvoice = 0; decimal tax1 = 0; decimal tax2 = 0;
        decimal taxm = 0; decimal disc_per1 = 0; string rel_def = ""; string re_ref = ""; decimal tax3 = 0; decimal tax4 = 0; decimal tax5 = 0;
        string salseqno = ""; string set_price = "N"; string dis_error = ""; decimal item_dis; string itm_code = ""; DateTime expdate; decimal actual_dis = 0;
        string action_flag = "";
        [EncryptionActionAttribute]
        public ActionResult Index(string headtype)
        {
            
            util.init_values();

           // Session["hsseq"] = "";
            //Session["curren_name"] = "";
            //Session["sal_rep"] ="";
               
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            var bglist = from bh in db.AR_002_SALES
                         join bg1 in db.AR_001_CUSTM
                        on new { a2 = bh.customer_code } equals new { a2 = bg1.customer_code }
                         join bn in db.MC_001_CUREN
                         on new { b3 = bg1.currency_code } equals new { b3 = bn.currency_code }

                         where bh.status == 02 || bh.order_reference != ""
                         select new vw_genlay
                         {
                             vwint0 = bh.sale_sequence_number,
                             vwstring0 = bh.order_reference,
                             vwdecimal0 = bh.order_total_amount,
                             vwstring1 = bg1.cust_biz_name,
                             vwstring3 = bh.order_date,
                             vwstring4 =bh.order_expiration_date,
                             vwstring2 = bn.currency_description
                         };

            return View(bglist.ToList());

           
        }

        [EncryptionActionAttribute]
        public ActionResult QuoteIndex()
        {

            util.init_values();

            Session["hsseq"] = "";
            Session["curren_name"] = "";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            var bglist = from bh in db.AR_002_SALES
                         join bg1 in db.AR_001_CUSTM
                        on new { a2 = bh.customer_code } equals new { a2 = bg1.customer_code }
                         join bn in db.MC_001_CUREN
                         on new { b3 = bg1.currency_code } equals new { b3 = bn.currency_code }

                         where bh.status == 01
                         select new vw_genlay
                         {
                             vwint0 = bh.sale_sequence_number,
                             vwstring0 = bh.quote_reference,
                             vwdecimal0 = bh.quote_total_amount,
                             vwstring1 = bg1.cust_biz_name,
                             vwstring3 = bh.quote_date,
                             vwstring4 = bh.quote_expiration_date,
                             vwstring2 = bn.currency_description,
                             vwdate2 = DateTime.UtcNow
                             
                         };

            return View(bglist.ToList());


        }
        [EncryptionActionAttribute]
        public ActionResult QuoteConvert(int key1, string key2, string headtype)
        {
            ViewBag.action_flag = "QuoteConvert";
            action_flag = "QuoteConvert";
            psess = (psess)Session["psess"];
            salseqno = key1.ToString();
            headtype = "Q";
            
            if (err_flag)
            {
                order_update(key1, key2);
                return RedirectToAction("Index");
            }
            return View("QuoteIndex");
        }

        [EncryptionActionAttribute]
        public ActionResult CreateHeader()
        {
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            psess = (psess)Session["psess"];
            psess.temp4 = "";
            Session["shw_baseamt"] = "N";
            salseqno = "";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();

            select_query_head("H");
            glay.vwstring8 = "H";
            glay.vwstrarray0[2] = pubsess.base_currency_code;
           // glay.vwstrarray0[3] = Session["tran_date"].ToString();
            glay.vwstrarray0[6] = "N";
            Session["curren_name"] = pubsess.base_currency_description;
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
            photo1 = photofile;
            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head("H");

            glay.vwstring8 = "H";
            cur_read();
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditHeader()
        {
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            psess = (psess)Session["psess"];
            psess.temp4 = "";
            Session["shw_baseamt"] = "N";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();


            glay.vwstring8 = "H";
            glay.vwstring9 = Session["hsseq"].ToString();
            salseqno = Session["hsseq"].ToString();
            gdoc = db.AR_002_SALES.Find(Convert.ToInt16(glay.vwstring9));
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
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            glay = glay_in;
            glay.vwstring9 = Session["hsseq"].ToString();
            salseqno = Session["hsseq"].ToString();
            photo1 = photofile;
            update_file(headtype);

            if (err_flag)
            {


                return RedirectToAction("CreateDetails");
            }

            select_query_head("H");

            glay.vwstring8 = "H";
            cur_read();
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult CreateDetails(string headtype="D")
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            salseqno = Session["hsseq"].ToString();
            
            initial_rtn();
            tax_sel();
            waredefault();
                header_ana("D");
                select_query_head("D");
            glay.vwstring8 = headtype;
            glay.vwstring9 = Session["hsseq"].ToString();
            glay.vwstring7 = Session["ref_def"].ToString();
            preseq = Session["ref_def"].ToString();
            show_screen_info();
           
            return View(glay);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetails(vw_genlay glay_in, string hid_price, string headtype = "D")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            glay = glay_in;
            salseqno = Session["hsseq"].ToString();
            glay.vwstring9 = Session["hsseq"].ToString();
            glay.vwstring7 = Session["ref_def"].ToString();
            preseq = Session["ref_def"].ToString();
            
            if (headtype == "send_app")
            {
                validation_routine(headtype);
                 

            }
            


            if (err_flag)
            {
                update_file(headtype);

                if (err_flag)
                {
                   invoice_cal();//----
                    return RedirectToAction("CreateDetails");
                }
            }
            tax_sel();
            header_ana("D");
            show_screen_info();
            select_query_head("D");
           

            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult Edit(int key1)
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            

            gdoc = db.AR_002_SALES.Find(key1);
            if (gdoc != null)
            {
                Session["hsseq"] = gdoc.sale_sequence_number;
                Session["ref_def"] = gdoc.order_reference;
                Session["tran_date"] = gdoc.transaction_date;
                Session["shw_baseamt"] = "N";
                return RedirectToAction("CreateDetails", new { headtype = "D" });
            }

            salseqno = "";

            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditDetails(int key1, int key2)
        {
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            psess = (psess)Session["psess"];
            salseqno = Session["hsseq"].ToString();
            int jno = Convert.ToInt16(salseqno);
            pubsess = (pubsess)Session["pubsess"];
            key1 = Convert.ToInt16(salseqno);
            AR_002_SODT = db.AR_002_SODT.Find(key1, key2);
            if (AR_002_SODT == null)
                return View(glay);
            Session["line_num"] = AR_002_SODT.line_sequence;
           
            initial_rtn();
            tax_sel();
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
            salseqno = Session["hsseq"].ToString();
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            
            glay = glay_in;
            glay.vwstring9 = Session["hsseq"].ToString();
            glay.vwdecimal4 = Convert.ToDecimal(Session["line_num"]);

            update_file(headtype);

            if (err_flag)
            {


                return RedirectToAction("CreateDetails");
            }
            select_query_head("D");

            glay.vwstring8 = "H";
            show_screen_info();
            header_ana();
            tax_sel();
            
            return View(glay);

        }
        private void delete_record()
        {
            AR_002_SODT = db.AR_002_SODT.Find(glay.vwint0, glay.vwint1);
            if (AR_002_SODT != null)
            {
                db.AR_002_SODT.Remove(AR_002_SODT);
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
                    gdoc = new AR_002_SALES();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;

                    var headlist = (from bh in db.AP_001_PUROT
                                    where bh.parameter_code == "SLSEQ" && (bh.order_type == "02" || bh.order_type == "single")
                                    select bh).FirstOrDefault();
                    string pref = headlist.order_prefix;
                    int seq = headlist.order_sequence;
                    int num = headlist.numeric_size;
                    string sqlstr = " update AP_001_PUROT set order_sequence = order_sequence+1 where parameter_code = 'SLSEQ' and (order_type = '02' or order_type = 'single')";
                    db.Database.ExecuteSqlCommand(sqlstr);


                    if (num == 0)
                    {
                        preseq = pref + seq.ToString();
                    }
                    else
                    {
                        string seq1 = ("000000000" + seq.ToString()).Substring(9 - num + seq.ToString().Length, num);
                        preseq = pref + seq1;
                    }
                    glay.vwstring7 = preseq;
                    gdoc.order_reference = preseq;
                }
                else
                {
                    glay.vwint0 = Convert.ToInt16(salseqno);
                    gdoc = db.AR_002_SALES.Find(glay.vwint0);
                }

                string dh1 = util.date_yyyymmdd(glay.vwstrarray0[4]);
                DateTime q_date = new DateTime(Convert.ToInt16(dh1.Substring(0, 4)), Convert.ToInt16(dh1.Substring(4, 2)), Convert.ToInt16(dh1.Substring(6, 2)));
                expdate = q_date.AddDays(glay.vwint1);
               
                gdoc.status = 02;
                gdoc.sales_transaction_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : (glay.vwstrarray0[0]);
                gdoc.customer_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : (glay.vwstrarray0[1]);
                gdoc.currency_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : (glay.vwstrarray0[2]);
                gdoc.exchange_rate = glay.vwdecimal0;
                gdoc.number_of_day = glay.vwint1;
                gdoc.transaction_date = util.date_yyyymmdd(glay.vwstrarray0[3]);
                gdoc.order_date = util.date_yyyymmdd(glay.vwstrarray0[4]);
                gdoc.expected_delivery_date = util.date_yyyymmdd(glay.vwstrarray0[5]);
                gdoc.delivery_address_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : (glay.vwstrarray0[6]);
                gdoc.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : (glay.vwstrarray0[7]);

                gdoc.modified_by = pubsess.userid;
                gdoc.modified_date = DateTime.UtcNow;

                Session["ex_rate"] = gdoc.exchange_rate;
                Session["tran_date"] = util.date_slash(gdoc.transaction_date);

                if (cr_flag == "CreateHeader")
                    db.Entry(gdoc).State = EntityState.Added;
                else
                    db.Entry(gdoc).State = EntityState.Modified;
            }
           else
            {

                if (cr_flag == "CreateDetails")
                {
                    AR_002_SODT = new AR_002_SODT();
                    AR_002_SODT.created_by = pubsess.userid;
                    AR_002_SODT.created_date = DateTime.UtcNow;
                    string sqlstr = "select isnull(max(line_sequence),0) vwint1 from AR_002_SODT where sales_order_sequence=" + util.sqlquote(glay.vwstring7);
                    var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
                    glay.vwdecimal4 = sql1.vwint1 + 1;
                }
                else
                {
                    glay.vwint0 = Convert.ToInt16(salseqno);
                    glay.vwdecimal4 = Convert.ToInt16(Session["line_num"]);
                    AR_002_SODT = db.AR_002_SODT.Find(glay.vwint0, Convert.ToInt16(glay.vwdecimal4));
                }

                 //the detailsub button has been clicked
                ext_price = glay.vwdclarray0[0] * glay.vwdclarray0[1];
                disc = glay.vwdclarray0[2];
                disc_per1 = glay.vwdclarray0[3];

                re_cal(glay.vwstring0);
                if (disc_per1 == 0)
                    actual_dis = disc + item_dis;
                else
                    actual_dis = (disc_per1 * ext_price / 100) + item_dis;
                //invoice_cal();
                AR_002_SODT.include_tax = "";
                glay.vwint0 = Convert.ToInt16(salseqno);
                AR_002_SODT.sale_sequence_number = glay.vwint0;
                AR_002_SODT.line_sequence = Convert.ToInt16(glay.vwdecimal4);
                AR_002_SODT.sales_order_sequence = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : (glay.vwstring7);
                AR_002_SODT.item_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                AR_002_SODT.item_warehouse_code = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                AR_002_SODT.quote_qty = glay.vwdclarray0[0];
                //AR_002_SODT.order_date = util.date_yyyymmdd(glay.vwstring3);
                //AR_002_SODT.pick_quantity = glay.vwdclarray0[1];
                AR_002_SODT.price = glay.vwdclarray0[1];
                AR_002_SODT.tax_amount = tax_amt;
                AR_002_SODT.tax_amount1 = Convert.ToDecimal(Session["taxr1"]);
                AR_002_SODT.tax_amount2 = Convert.ToDecimal(Session["taxr2"]);
                AR_002_SODT.tax_amount3 = Convert.ToDecimal(Session["taxr3"]);
                AR_002_SODT.tax_amount4 = Convert.ToDecimal(Session["taxr4"]);
                AR_002_SODT.tax_amount5 = Convert.ToDecimal(Session["taxr5"]);
                AR_002_SODT.base_amount = 0;
                AR_002_SODT.ext_price = ext_price;
                AR_002_SODT.discount_percent = glay.vwdclarray0[3];
                AR_002_SODT.discount_amount = glay.vwdclarray0[2];
                AR_002_SODT.net_amount = net_amt;
                AR_002_SODT.quote_amount = quote_amt;
                AR_002_SODT.actual_discount = glay.vwdecimal5;
                glay.vwdecimal0 = Convert.ToDecimal(Session["ex_rate"]);
                AR_002_SODT.base_amount = glay.vwdecimal0 * quote_amt;

                //if (base_cur==AR_002_SODT.currency)
                //{
                //    AR_002_SODT.exchange_rate = 1;
                //    AR_002_SODT.base_amount = AR_002_SODT.amount;
                //}

                AR_002_SODT.analysis_code1 = "";
                AR_002_SODT.analysis_code2 = "";
                AR_002_SODT.analysis_code3 = "";
                AR_002_SODT.analysis_code4 = "";
                AR_002_SODT.analysis_code5 = "";
                AR_002_SODT.analysis_code6 = "";
                AR_002_SODT.analysis_code7 = "";
                AR_002_SODT.analysis_code8 = "";
                AR_002_SODT.analysis_code9 = "";
                AR_002_SODT.analysis_code10 = "";
                AR_002_SODT.modified_date = DateTime.UtcNow;
                AR_002_SODT.modified_by = pubsess.userid;

                Session["ref_def"] = AR_002_SODT.sales_order_sequence;
               

                if (glay.vwstrarray6 != null)
                {
                    int arrlen = glay.vwstrarray6.Length;
                    if (arrlen > 0)
                        AR_002_SODT.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                    if (arrlen > 1)
                        AR_002_SODT.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                    if (arrlen > 2)
                        AR_002_SODT.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                    if (arrlen > 3)
                        AR_002_SODT.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                    if (arrlen > 4)
                        AR_002_SODT.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                    if (arrlen > 5)
                        AR_002_SODT.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                    if (arrlen > 6)
                        AR_002_SODT.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                    if (arrlen > 7)
                        AR_002_SODT.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                    if (arrlen > 8)
                        AR_002_SODT.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                    if (arrlen > 9)
                        AR_002_SODT.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                    psess.intemp0 = arrlen;
                }

                if (cr_flag == "CreateDetails")
                    db.Entry(AR_002_SODT).State = EntityState.Added;
                else
                    db.Entry(AR_002_SODT).State = EntityState.Modified;
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
                Session["hsseq"] = gdoc.sale_sequence_number;
                salseqno = gdoc.sale_sequence_number.ToString();
                Session["ref_def"] = gdoc.order_reference;
                querylay qlay = new querylay();
                qlay.query1 = gdoc.sales_transaction_type;
                qlay.query0 = gdoc.customer_code;
                qlay.query2 = gdoc.currency_code;
                //qlay.intquery1 = gdoc.number_of_cycle;
                //qlay.intquery0 = gdoc.cycle_interval;
                qlay.query3 = util.date_slash(gdoc.transaction_date);
                qlay.query4 = util.date_slash(gdoc.order_date);
                qlay.query5 = util.date_slash(gdoc.expected_delivery_date);
                qlay.query6 = gdoc.delivery_address_code;
                Session["headerval"] = qlay;
            
            }
            if (err_flag && cr_flag.IndexOf("Header") < 0)
            {
                string item_price = glay.vwdclarray0[2].ToString();
                call_discount(glay.vwstring0, item_price);
                //fsp.itemdis_calculation(glay.vwstring0, item_price);
                quote_update();

                string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, AR_002_SODT b where header_sequence in (analysis_code1";
                stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                stri += " and sale_sequence_number =" + glay.vwint0;
                //db.Database.ExecuteSqlCommand(stri);

            }
            if (err_flag && cr_flag.IndexOf("Header") > 0)
            {
                {
                    util.write_document("SALESOR", gdoc.sale_sequence_number.ToString(), photo1, glay.vwstrarray9);
                }

            }
        
        }
        private void validation_routine(string headtype)
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
           // DateTime date_chk = Convert.ToDateTime(psess.temp4);
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            

            string cr_flag = action_flag;
           // DateTime date_chki = DateTime.Now;
            DateTime invaliddate = new DateTime(1000, 01, 01);
           
            if (cr_flag.IndexOf("Header") > 0)
            {

                if (!util.date_validate(glay.vwstrarray0[4]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid order date");
                    err_flag = false;
                }

                if (!util.date_validate(glay.vwstrarray0[3]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
                    err_flag = false;
                }

                if (!util.date_validate(glay.vwstrarray0[5]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid expected delivery date");
                    err_flag = false;
                }
            }
           
           
                if (cr_flag.IndexOf("Details") > 0)
                {
                    {
                        if (glay.vwdclarray0[2] != 0 && glay.vwdclarray0[3] != 0)
                        {
                            ModelState.AddModelError(String.Empty, "You can either enter Discount Amount or Discount Percent");
                            err_flag = false;
                        }
                    }


                    var headlist = (from bh in db.AP_001_PUROT
                                    where bh.parameter_code == "SLSEQ" && (bh.order_type == "02" || bh.order_type == "single")
                                    select bh).FirstOrDefault();
                    if (headlist == null)
                    {
                        ModelState.AddModelError(String.Empty, "Order sequencing number not created");
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
          
            if (headtype == "Q") //----
            {
                int hbug = 0;
                int.TryParse(salseqno, out hbug);
                string cur_date = DateTime.UtcNow.ToString("yyyyMMdd");
                string contrl_check = (from bg in db.AR_002_SALES
                                        where bg.sale_sequence_number == hbug
                                        select bg.quote_expiration_date).FirstOrDefault();
                if (string.Compare(contrl_check , cur_date) > 0)
                {
                    ModelState.AddModelError(string.Empty, "Quote has Expired");
                    err_flag = false;
                }

            }

        
                //if (error_msg != "")
                //{
                //    ModelState.AddModelError(String.Empty, error_msg);
                //    err_flag = false;
                //}
        }
        
        private void dis_validation(string disc_amt, string disc_per)
        {
            initial_rtn();
            decimal.TryParse(disc_amt, out disc);
            decimal.TryParse(disc_per, out disc_per1);
            

            if (disc != 0 && disc_per1 != 0)
            {
                ModelState.AddModelError(String.Empty, "You can either enter Discount Amount or Discount Percent");
                err_flag = false;
                 dis_error = "You can either enter Discount Amount or Discount Percent";
            }
                
         
        }
        private void select_query_head(string type1)
        {
            if (type1 == "H")
            {
                year_cal();
                ViewBag.cust = util.para_selectquery("001", glay.vwstrarray0[1]);
                //ViewBag.cust = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[1]);
          
                //var emp = from pf in db.AR_001_CUSTM
                //          where pf.active_status == "N"
                //          orderby pf.cust_biz_name
                //          select pf;
                //ViewBag.cust = new SelectList(emp.ToList(), "customer_code", "cust_biz_name", glay.vwstrarray0[1]);

                var empz = from pf in db.AR_001_STRAN
                           where pf.order == "Y" && pf.active_status == "N"
                           orderby pf.order_name
                           select pf;
                ViewBag.salestt = new SelectList(empz.Distinct().ToList(), "order_code", "order_name", glay.vwstrarray0[0]);
                var del = from jk in db.AR_001_DADRS
                          where jk.address_type == "CU" && jk.active_status == "N"
                          orderby jk.location_alias
                          select jk;
                ViewBag.deladd = new SelectList(del.ToList(), "address_code", "location_alias", glay.vwstrarray0[6]);
            }


            if (type1 == "D")
            {
                ViewBag.item  = util.para_selectquery("60", glay.vwstring0);
          
                //var empe = from pf in db.IV_001_ITEM
                //           where pf.active_status == "N"
                //           orderby pf.item_name
                //           select pf;
                //ViewBag.item = new SelectList(empe.ToList(), "item_code", "item_name", glay.vwstring0);
                string str1 = "select warehouse_code query0, c2 query1 from vw_warehouse_site ";
                var emp1 = db.Database.SqlQuery<querylay>(str1);
                ViewBag.itemw = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring1);

                //var itm = from pg in db.IV_001_WAREH
                //          where pg.active_status == "N"
                //          orderby pg.warehouse_name
                //          select pg;
                //ViewBag.itemw = new SelectList(itm.ToList(), "warehouse_code", "warehouse_name", glay.vwstring1);

            }
        }
        private void select_queryhide(string type1 = "D")
        {
            if (type1 == "D")
            {
                var empe = from pf in db.IV_001_ITEM
                           where pf.active_status == "N"
                           select pf;
                ViewBag.item = new SelectList(empe.ToList(), "item_code", "item_name", glay.vwstring0);

            }
       
        }
        private void initial_rtn()
        {
            glay.vwdclarray0 = new decimal[50];
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwstrarray0[wctr] = "";
            glay.vwstrarray0[3] = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwstrarray0[4] = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray7 = new string[20];


        }
        private void cur_read()
        {
            string curcode = "";
            curcode = glay.vwstrarray0[2];
            string read_cur = (from bg in db.MC_001_CUREN
                               where bg.currency_code == curcode
                               select bg.currency_description).FirstOrDefault();

            Session["curren_name"] = read_cur;
        }
        private void waredefault()
        {
            string wdefault = (from bg in db.IV_001_WAREH
                                   where bg.default_warehouse == "Y"
                                   select bg.warehouse_code).FirstOrDefault();
            glay.vwstring1 = wdefault;
        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AR_002_SALES] where cast (sale_sequence_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "delete from [dbo].[AR_002_SODT] where cast(sale_sequence_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        public ActionResult delete_detail(string id)
        {
            // write your query statement
            salseqno = Session["hsseq"].ToString();
            string sqlstr = "delete from [dbo].[AR_002_SODT] where cast(sale_sequence_number as varchar) +'[]'+ cast(line_sequence as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            string item_price = glay.vwdclarray0[0].ToString();
            call_discount(glay.vwstring0, item_price);
            quote_update();
            select_queryhide("D");
            if (HttpContext.Request.IsAjaxRequest())
                return Json(
                            JsonRequestBehavior.AllowGet);
            return RedirectToAction("CreateDetails");
            
        }

        private void read_details()
        {
            int hbug = 0;
            int.TryParse(salseqno, out hbug);
            var bglist = from bh in db.AR_002_SODT
                         join bg in db.IV_001_ITEM
                         on new { a1 = bh.item_code } equals new { a1 = bg.item_code }
                         join bf in db.GB_001_PCODE
                         on new { a1 = bg.sku_sequence, a2 = "01" } equals new { a1 = bf.parameter_code, a2 = bf.parameter_type }
                         where bh.sale_sequence_number == hbug
                         select new vw_genlay
                         {
                             vwint0 = bh.sale_sequence_number,
                             vwint1 = bh.line_sequence,
                             vwstring1 = bh.sales_order_sequence,
                             vwstring2 = bg.item_name,
                             vwstring3 = bf.parameter_name,
                             vwdecimal0 = bh.price,
                             vwdecimal1 = bh.quote_qty,
                             //vwdecimal6 = bh.pick_quantity,
                             vwdecimal2 = bh.ext_price,
                             vwdecimal3 = bh.tax_amount,
                             vwdecimal4 = bh.actual_discount,
                             vwdecimal5 = bh.quote_amount
                         };
            ViewBag.x1 = bglist.ToList();


        }
        private void show_screen_info()
        {
            display_header();
           read_details();

        }

        private void read_header()
        {
            string amt_check = "";
            glay.vwstrarray0 = new string[20];
            int hbug = 0;
            int.TryParse(salseqno, out hbug);
            var bg2list = (from bh in db.AR_002_SALES
                           join bh1 in db.AR_001_CUSTM
                          on new { a1 = bh.customer_code } equals new { a1 = bh1.customer_code }
                           where bh.sale_sequence_number == hbug
                           select new {bh, bh1}).FirstOrDefault();
                    if (bg2list != null)
                       {
                            glay.vwstrarray0[0] = bg2list.bh.sales_transaction_type;
                            glay.vwstrarray0[1] = bg2list.bh1.customer_code;
                            glay.vwstrarray0[2] = bg2list.bh.currency_code;
                            glay.vwdecimal0 = bg2list.bh.exchange_rate;
                            glay.vwstrarray0[3] = util.date_slash(bg2list.bh.transaction_date);
                            glay.vwstrarray0[4] = util.date_slash(bg2list.bh.order_date);
                            glay.vwint1 = bg2list.bh.number_of_day;
                           // glay.vwstrarray0[3] = bg2list.bh.quote_expiration_date.ToString("dd/MM/yyyy");
                            //glay.vwstrarray0[4] = bg2list.bg2.description;
                            glay.vwstrarray0[5] = bg2list.bh.note;
                            string docode = gdoc.sale_sequence_number.ToString();
                            var bglist = from bg in db.GB_001_DOC
                            where bg.screen_code == "SALESOR" && bg.document_code == docode
                            orderby bg.document_sequence
                            select bg;

                ViewBag.anapict = bglist.ToList();
                amt_check = "N";
                cur_read();
                if (bg2list.bh.currency_code == pubsess.base_currency_code)
                    amt_check = "Y";

               psess.temp4 = amt_check;
            
            }
        }
        private void display_header()
        {
            string bal_qty = "";
            bal_qty = Session["sal_rep"].ToString();
            DateTime expdate = DateTime.Now;
            int qdate = Convert.ToInt16(Session["hsseq"]);
            //var qdate = (from bg in db.AR_002_SALES
            //             select bg.quote_date).FirstOrDefault();
            var chkdate = (from bg in db.AR_002_SALES
                           where bg.sale_sequence_number == qdate
                           select new
                           {
                               c0 = bg.order_date,
                               c1 = bg.number_of_day
                           }).FirstOrDefault();
            int numday = chkdate.c1;

            string dh1 = chkdate.c0;
            DateTime quotedate = new DateTime(Convert.ToInt16(dh1.Substring(0, 4)), Convert.ToInt16(dh1.Substring(4, 2)), Convert.ToInt16(dh1.Substring(6, 2)));
            expdate = quotedate.AddDays(numday);

            int hbug = 0;
            int.TryParse(salseqno, out hbug);
            var bg2list = (from bh in db.AR_002_SALES
                           join bh1 in db.AR_001_CUSTM
                           on new { a1 = bh.customer_code } equals new { a1 = bh1.customer_code }
                           join bj in db.AR_001_STRAN
                           on new { a1 = bh.sales_transaction_type } equals new { a1 = bj.order_code,}
                           into bj1
                           from bj2 in bj1.DefaultIfEmpty()
                           where bh.sale_sequence_number == hbug
                           select new { bh, bh1, bj2 }).FirstOrDefault();


            vw_genlay glayhead = new vw_genlay();
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwstrarray1[0] = bg2list.bh1.cust_biz_name;
                glayhead.vwstrarray1[2] = bg2list.bh.order_total_ext_price.ToString("#,#00.00");
                glayhead.vwstrarray1[1] = expdate.ToString("dd/MM/yyyy");
                glayhead.vwstrarray1[8] = bal_qty;
                glayhead.vwstring2 = util.date_slash(bg2list.bh.transaction_date);
                glayhead.vwstrarray1[3] = bg2list.bh.order_total_discount.ToString("#,##0.00");
                glayhead.vwstrarray1[4] = bg2list.bh.order_total_tax.ToString("#,#00.00");
                glayhead.vwstrarray1[5] = bg2list.bh.order_total_amount.ToString("#,#00.00");
                glayhead.vwstrarray1[6] = bg2list.bh.order_total_inv_tax.ToString("#,#00.00");
                glayhead.vwstrarray1[7] = bg2list.bh.order_reference;

            }
            ViewBag.x2 = glayhead;
        }
        private void init_header()
        {
            gdoc.quote_total_amount = 0;
            gdoc.payment_term_code = "";
            gdoc.item_warehouse_code = "";
            gdoc.number_of_day = 0;
            gdoc.order_total_amount = 0;
            gdoc.transaction_date = "";
            gdoc.quote_date = "";
            gdoc.order_date = "";
            gdoc.quote_expiration_date = "";
            gdoc.order_expiration_date = "";
            gdoc.expected_delivery_date = "";
            gdoc.order_total_discount = 0;
            gdoc.order_total_ext_price = 0;
            gdoc.order_total_tax = 0;
            gdoc.quote_total_discount = 0;
            gdoc.quote_total_ext_price = 0;
            gdoc.quote_total_qty = 0;
            gdoc.quote_total_tax = 0;
            gdoc.invoice_total_amount = 0;
            gdoc.invoice_total_discount = 0;
            gdoc.invoice_total_ext_price = 0;
            gdoc.invoice_total_tax = 0;
            gdoc.project_code = "";
            gdoc.approval_level = 0;
            gdoc.created_by = "";
            gdoc.approval_by = "";
           // gdoc.attach_document = "";
            gdoc.exchange_rate = 0;
            gdoc.modified_by = "";
            gdoc.note = "";
            gdoc.analysis_code1 = "";
            gdoc.analysis_code10 = "";
            gdoc.analysis_code2 = "";
            gdoc.analysis_code3 = "";
            gdoc.analysis_code4 = "";
            gdoc.analysis_code5 = "";
            gdoc.analysis_code6 = "";
            gdoc.analysis_code7 = "";
            gdoc.analysis_code8 = "";
            gdoc.analysis_code9 = "";
            gdoc.created_by = "";
            gdoc.currency_code = "";
            gdoc.customer_code = "";
            gdoc.delivery_address_code = "";
            gdoc.exchange_rate = 0;
            gdoc.sales_transaction_type = "";
            gdoc.status = 0;
            gdoc.invoice_reference = "";
            gdoc.order_reference = "";
            gdoc.quote_reference = "";
          
        }
        private void order_update(int key1, string key2)
        {
            pubsess = (pubsess)Session["pubsess"];
            string str1 = "select quote_total_ext_price as vwdecimal0, quote_total_inv_tax as vwdecimal1, quote_total_discount as vwdecimal2, quote_total_tax as vwdecimal3, quote_total_amount as vwdecimal4, quote_total_qty as vwdecimal5";
            str1 += " from AR_002_SALES where quote_reference=" + util.sqlquote(key2);
            var str2 = db.Database.SqlQuery<vw_genlay>(str1).FirstOrDefault();

            string str = "update AR_002_SALES set order_reference=" + util.sqlquote(key2) + ", order_total_ext_price=" + str2.vwdecimal0 + ", order_total_inv_tax =";
            str += + str2.vwdecimal1 +", order_total_discount=" + str2.vwdecimal2 + ", order_total_tax=" + str2.vwdecimal3 + ", order_total_amount=";
            str += + str2.vwdecimal4 + ", order_total_qty=" + str2.vwdecimal5 + " where quote_reference=" + util.sqlquote(key2);
            db.Database.ExecuteSqlCommand(str);

            int hbug = 0;
            int.TryParse(salseqno, out hbug);
            var bglist = from bh in db.AR_002_QUOTE
                         where bh.sale_sequence_number == key1
                         select new vw_genlay
                         {
                             vwint0 = bh.sale_sequence_number,
                             vwint1 = bh.line_sequence,
                             vwstring1 = bh.quote_reference,
                             vwstring0 = bh.item_code,
                             vwdecimal0 = bh.price,
                             vwdecimal1 = bh.quote_qty,
                             vwdecimal2 = bh.ext_price,
                             vwdecimal3 = bh.tax_amount,
                             vwdecimal4 = bh.actual_discount,
                             vwdecimal5 = bh.quote_amount,
                             vwdecimal6 = bh.net_amount,
                             vwstring2 = bh.base_amount.ToString()
                         };
            var xodr = bglist.ToList();
            foreach (var item in xodr)
            {
                AR_002_SODT AR_002_SODT = new AR_002_SODT();
                AR_002_SODT.include_tax = "";
                AR_002_SODT.created_by = pubsess.userid;
                AR_002_SODT.created_date = DateTime.UtcNow;
                AR_002_SODT.sale_sequence_number = item.vwint0;
                AR_002_SODT.line_sequence = item.vwint1;
                AR_002_SODT.sales_order_sequence = item.vwstring1;
                AR_002_SODT.item_code = item.vwstring0;
                AR_002_SODT.item_warehouse_code = "";
                AR_002_SODT.quote_qty = item.vwdecimal1;
                //AR_002_SODT.order_date = DateTime.UtcNow;
                AR_002_SODT.pick_quantity = 0;
                AR_002_SODT.price = item.vwdecimal0;
                AR_002_SODT.tax_amount = item.vwdecimal3;
                AR_002_SODT.tax_amount1 = 0;
                AR_002_SODT.tax_amount2 = 0;
                AR_002_SODT.tax_amount3 = 0;
                AR_002_SODT.tax_amount4 = 0;
                AR_002_SODT.tax_amount5 = 0;
                AR_002_SODT.base_amount = 0;
                AR_002_SODT.ext_price = item.vwdecimal2;
                AR_002_SODT.discount_percent = 0;
                AR_002_SODT.discount_amount = 0;
                AR_002_SODT.net_amount = item.vwdecimal6;
                AR_002_SODT.quote_amount = item.vwdecimal5;
                AR_002_SODT.actual_discount = item.vwdecimal4;
                AR_002_SODT.base_amount = Convert.ToDecimal(item.vwstring2);
                AR_002_SODT.analysis_code1 = "";
                AR_002_SODT.analysis_code2 = "";
                AR_002_SODT.analysis_code3 = "";
                AR_002_SODT.analysis_code4 = "";
                AR_002_SODT.analysis_code5 = "";
                AR_002_SODT.analysis_code6 = "";
                AR_002_SODT.analysis_code7 = "";
                AR_002_SODT.analysis_code8 = "";
                AR_002_SODT.analysis_code9 = "";
                AR_002_SODT.analysis_code10 = "";
                AR_002_SODT.modified_date = DateTime.UtcNow;
                AR_002_SODT.modified_by = pubsess.userid;

                db.Entry(AR_002_SODT).State = EntityState.Added;

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

        [HttpPost]
        public ActionResult cust_curen(string cust_code)
        {
            psess = (psess)Session["psess"];
            string cur_code = "";
            string shw_rate = "";
            string rate_flag = "N";
            string curen_name = "";
            // write your query statement
           
           
                cur_code = (from bg in db.AR_001_CUSTM
                            where bg.customer_code == cust_code
                            select bg.currency_code).FirstOrDefault();


                querypass pass1 = util.basecur_description(cur_code);
                shw_rate = pass1.query0;
                curen_name = pass1.query1;

                decimal rat_code = 0;
                if (shw_rate == cur_code)
                    rat_code = 1;
                else
                {
                    // write your query statement
                    rat_code = (from bg in db.MC_001_EXCRT
                                where bg.currency_code == cur_code
                                select bg.exchange_rate).FirstOrDefault();
                    rate_flag = "Y";
                }

                var bgassign = (from bg in db.AR_001_CUSTM
                                join bn in db.GB_001_EMP
                                on new { a1 = bg.sales_rep } equals new { a1 = bn.employee_code }
                                into bn1
                                from bn2 in bn1.DefaultIfEmpty()
                                where bg.customer_code == cust_code
                                select new { bg, bn2 }).FirstOrDefault();

                string bal_qty = bgassign.bn2.name;
                Session["sal_rep"] = bal_qty;
            Session["shw_baseamt"] = rate_flag;
            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = cur_code });
            ary.Add(new SelectListItem { Value = "2", Text = rate_flag });
            ary.Add(new SelectListItem { Value = "3", Text = rat_code.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "4", Text = curen_name });
            ary.Add(new SelectListItem { Value = "qty", Text = bal_qty });
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            //}
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult assign(string item_code)
        {

            psess = (psess)Session["psess"];
            var bgassign = (from bg in db.IV_001_ITEM
                            join bn in db.GB_001_PCODE
                            on new { a1 = bg.sku_sequence } equals new { a1 = bn.parameter_code }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            join bm in db.DC_001_DISC
                            on new { a1 = bg.discount_code } equals new { a1 = bm.discount_code}
                            into bm1
                            from bm2 in bm1.DefaultIfEmpty()
                            where bg.item_code == item_code
                            select new { bg, bn2, bm2 }).FirstOrDefault();

            string sku_name = bgassign.bn2.parameter_name;
            string price_dis = bgassign.bg.item_type;
            decimal item_dis = 0;
            string item_dist = bgassign.bm2.stepped_discount_active;
            if (item_dist == "F")
            {
                item_dis = bgassign.bm2.discount_amount;
            }
            //var edit_chk = (from cy in db.GB_001_COY
            //                select cy).FirstOrDefault();

            //string price_edt = edit_chk.allow_price_edit;
            //if (price_edt == "N" && price_dis == "I")
            //{
            //    set_price = "Y";
            //}

            var bgfind = (from bh in db.AR_001_CUSTM
                          select bh).FirstOrDefault();

            string cust_class = bgfind.price_class;

            if (cust_class == "2")
                f = bgassign.bg.selling_price_class2;
            else if (cust_class == "3")
                f = bgassign.bg.selling_price_class3;

            else if (cust_class == "4")
                f = bgassign.bg.selling_price_class4;
            else if (cust_class == "5")
                f = bgassign.bg.selling_price_class5;
            else if (cust_class == "6")
                f = bgassign.bg.selling_price_class6;
            else
                f = bgassign.bg.selling_price_class1;


            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "sku", Text = sku_name });
            ary.Add(new SelectListItem { Value = "price", Text = f.ToString() });
            ary.Add(new SelectListItem { Value = "price_flag", Text = set_price });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }
        private void call_discount(string item_code, string item_price)
        {
            //decimal pricep = 0; decimal item_dis = 0; decimal ext_price = 0;
            int hbug = 0;

            hbug = Convert.ToInt16(Session["hsseq"]);
            string str1 = "select AR_002_SODT.item_code as vwstring0,sum(AR_002_SODT.ext_price) as vwdecimal0, sum(AR_002_SODT.order_quantity) as vwdecimal1,";
            str1 += " stepped_criteria as vwstring1,stepped_discount_active as vwstring2,IV_001_ITEM.discount_code as vwstring3";
            str1 += " from AR_002_SODT, DC_001_DISC,IV_001_ITEM where sale_sequence_number=" + hbug + " and IV_001_ITEM.item_code=AR_002_SODT.item_code and";
            str1 += " IV_001_ITEM.discount_code=DC_001_DISC.discount_code and stepped_discount_active in ('S','P')";
            str1 += " group by AR_002_SODT.item_code, stepped_criteria,stepped_discount_active,IV_001_ITEM.discount_code";
            Session["str1"] = str1;
            fsp.itemdis_calculation(item_code);

        }  

        [HttpPost]
        public ActionResult item_bal(string ware_code)
        {

            psess = (psess)Session["psess"];
            var bgassign = (from bg in db.IV_001_WAREH
                            join bn in db.IV_001_ITMST
                            on new { a1 = bg.warehouse_code } equals new { a1 = bn.warehouse }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            where bg.warehouse_code == ware_code
                            select new { bg, bn2 }).FirstOrDefault();

            int bal_qty = bgassign.bn2.bal_qty;


            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "qty", Text = bal_qty.ToString() });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }
       
        [HttpPost]
        public ActionResult price_cal(string item_price, string item_qty, string item_code, string disc_amt, string disc_per)
        {
            ModelState.Remove("vwdclarray0[0]");
            ModelState.Remove("vwdclarray0[2]");
            ModelState.Remove("vwdclarray0[3]");
            //qty = Convert.ToDecimal(item_qty);
            // price = Convert.ToDecimal(item_price);
            psess = (psess)Session["psess"];
            err_flag = true;
            decimal actual_dis = 0; string show_dis = "N";
            decimal item_dis = 0; decimal man_dis = 0;
            decimal.TryParse(item_qty, out qty);
            decimal.TryParse(item_price, out pricep);
            decimal.TryParse(disc_amt, out disc);
            decimal.TryParse(disc_per, out disc_per1);

            ext_price = qty * pricep;

            re_cal(item_code);
           // call_discount(item_code, item_price);
            //fsp.itemdis_calculation(item_code, item_price);
            
            tax_sel();
            dis_validation(disc_amt,disc_per);
            if (item_dis != 0)
            {
                psess.temp2 = "Y";
                show_dis = "Y";
            }
            if (err_flag)
            {
                if (disc != 0)
                {
                    man_dis = disc;
                    actual_dis = disc + item_dis;
                }
                else
                {
                    actual_dis = disc_per1 * ext_price / 100 + item_dis;
                    man_dis = disc_per1 * ext_price / 100;
                }
            }
            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = ext_price.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "2", Text = tax_amt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "3", Text = net_amt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "4", Text = quote_amt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "5", Text = actual_dis.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "6", Text = dis_error });
            ary.Add(new SelectListItem { Value = "7", Text = tax1.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "8", Text = tax2.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "9", Text = tax3.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "10", Text = tax4.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "11", Text = tax5.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "12", Text = item_dis.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "13", Text = man_dis.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "14", Text = show_dis });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }
        private void re_cal(string item_code)
        {
            net_amt = ext_price - disc - (disc_per1 * ext_price / 100);

            var bgassign = (from bg in db.IV_001_ITEM
                            where bg.item_code == item_code
                            select bg).FirstOrDefault();
            if (bgassign != null)
            {
                 tax1 = tax_calculation(bgassign.tax_code1, net_amt);
                 tax2 = tax_calculation(bgassign.tax_code2, net_amt);
                 tax3 = tax_calculation(bgassign.tax_code3, net_amt);
                 tax4 = tax_calculation(bgassign.tax_code4, net_amt);
                 tax5 = tax_calculation(bgassign.tax_code5, net_amt);

                tax_amt = tax1 + tax2 + tax3 + tax4 + tax5;
                Session["taxr1"] = tax1;
                Session["taxr2"] = tax2;
                Session["taxr3"] = tax3;
                Session["taxr4"] = tax4;
                Session["taxr5"] = tax5;

            }
            quote_amt = net_amt - tax_amt;

            var bgassigni = (from bg in db.IV_001_ITEM
                             join bm in db.DC_001_DISC
                             on new { a1 = bg.discount_code } equals new { a1 = bm.discount_code }
                             into bm1
                             from bm2 in bm1.DefaultIfEmpty()
                             where bg.item_code == item_code && bm2.discount_count == 0
                             select new { bg, bm2 }).FirstOrDefault();

            string price_dis = bgassigni.bg.item_type;
            string item_dist = bgassigni.bm2.stepped_discount_active;
            if (item_dist == "F")
            {
                decimal fdisamt = bgassigni.bm2.discount_amount;
                decimal fdisper = bgassigni.bm2.discount_percent;
                if (fdisper == 0)
                    item_dis = bgassigni.bm2.discount_amount;
                else
                {
                    decimal fdiscal = bgassigni.bm2.discount_percent;
                    item_dis = fdiscal / 100 * ext_price;

                }
                Session["item_dis"] = item_dis;
            }

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
        private decimal tax_invoice(string icode, decimal amount)
        {
            var bgtax = (from bg in db.GB_001_TAX
                         where bg.tax_code == icode
                         select bg).FirstOrDefault();
            if (bgtax == null)
                return 0;
            if (bgtax.module_basis == "P")
                return 0;
            if (bgtax.computation_basis == "L")

                return 0;
            decimal tax = 0;
            tax = amount * bgtax.tax_rate / 100;
            if (bgtax.tax_impact == "S")
                tax = 0 - tax;
            return tax;


        }
        private void invoice_cal()
        {
            int key1 = Convert.ToInt16(salseqno);
            string salesttype = (from fh in db.AR_002_SALES
                                 where fh.sale_sequence_number == key1
                                 select fh.sales_transaction_type).FirstOrDefault();
           // string str1 = "select (sum(ext_price) query0,sum(actual_discount)query1) from AR_002_SODT where quote_reference=" + util.sqlquote(glay.vwstring7);

            string str1 = "select sum(ext_price) - sum(actual_discount) as dquery0 from AR_002_SODT where sales_order_sequence=" + util.sqlquote(glay.vwstring7);
            var str2 = db.Database.SqlQuery<querylay>(str1).FirstOrDefault();
           // decimal invoice_tax = 0;
            if (str2 != null)
                net_amt = str2.dquery0;
            //foreach(var item in str2.ToList()){
            //    net_amt = item.dquery0;
            //}
            

            var bgassign = (from bg in db.AR_001_STRAN
                            where bg.order_code == salesttype
                            select bg).FirstOrDefault();
            if (bgassign != null)
            {
                tax1 = tax_invoice(bgassign.tax_invoice1, net_amt);
                tax2 = tax_invoice(bgassign.tax_invoice2, net_amt);

                tax_amt = tax1 + tax2;
                string str3 = "update AR_002_SODT set tax_invoiceamt1=" + tax1 + ", tax_invoiceamt2=" + tax2 + "where sales_order_sequence=" + util.sqlquote(glay.vwstring7);
                db.Database.ExecuteSqlCommand(str3);

            }
           // invoice_tax = net_amt - tax_amt;
            string str = "update AR_002_SALES set order_total_inv_tax=" + tax_amt + "where order_reference=" + util.sqlquote(glay.vwstring7);
            db.Database.ExecuteSqlCommand(str);


        }
        private void move_detail()
        {
            ModelState.Remove("vwdclarray1[0]");
            ModelState.Remove("vwdclarray1[1]");
            ModelState.Remove("vwdclarray1[2]");
            ModelState.Remove("vwdclarray1[3]");
            ModelState.Remove("vwdclarray1[4]");
            glay.vwstrarray6 = new string[20];
            glay.vwdclarray1 = new decimal[20];
            
            int key1 = Convert.ToInt16(salseqno);
            int key2 = Convert.ToInt16(Session["line_num"]);

            var bgdtl = (from bg in db.AR_002_SODT
                         where bg.sale_sequence_number == key1 && bg.line_sequence == key2
                         select bg).FirstOrDefault();
            glay.vwdecimal4 = bgdtl.line_sequence;
            glay.vwstring7 = bgdtl.sales_order_sequence;
            glay.vwstring0 = bgdtl.item_code;
            glay.vwstring1 = bgdtl.item_warehouse_code;
            glay.vwdclarray0[1] = bgdtl.price;
            glay.vwdclarray0[0] = bgdtl.quote_qty;
            //glay.vwdclarray0[1] = bgdtl.pick_quantity;
            glay.vwdecimal2 = bgdtl.ext_price;
            //glay.vwstring3 = bgdtl.order_date.ToString("dd/MM/yyyy");
            glay.vwdclarray0[2] = bgdtl.discount_amount;
            glay.vwdclarray0[3] = bgdtl.discount_percent;
            glay.vwdclarray0[4] = bgdtl.net_amount;
            glay.vwdecimal1 = bgdtl.tax_amount;
            glay.vwdecimal3 = bgdtl.base_amount;
            glay.vwdecimal5 = bgdtl.actual_discount;
            glay.vwdclarray0[5] = bgdtl.quote_amount;
            glay.vwstrarray6[0] = bgdtl.analysis_code1;
            glay.vwstrarray6[1] = bgdtl.analysis_code2;
            glay.vwstrarray6[2] = bgdtl.analysis_code3;
            glay.vwstrarray6[3] = bgdtl.analysis_code4;
            glay.vwstrarray6[4] = bgdtl.analysis_code5;
            glay.vwstrarray6[5] = bgdtl.analysis_code6;
            glay.vwstrarray6[6] = bgdtl.analysis_code7;
            glay.vwstrarray6[7] = bgdtl.analysis_code8;
            glay.vwstrarray6[8] = bgdtl.analysis_code9;
            glay.vwstrarray6[9] = bgdtl.analysis_code10;
            glay.vwdclarray1[0] = bgdtl.tax_amount1;
            glay.vwdclarray1[1] = bgdtl.tax_amount2;
            glay.vwdclarray1[2] = bgdtl.tax_amount3;
            glay.vwdclarray1[3] = bgdtl.tax_amount4;
            glay.vwdclarray1[4] = bgdtl.tax_amount5;
            var sku = (from bg in db.IV_001_ITEM
                            join bn in db.GB_001_PCODE
                            on new { a1 = bg.sku_sequence } equals new { a1 = bn.parameter_code }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            where bg.item_code == glay.vwstring0
                            select new { bg, bn2 }).FirstOrDefault();

            string sku_name = sku.bn2.parameter_name;
            psess.temp2 = sku_name;

        }
        private void header_ana(string commandn="")
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string [] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            SelectList[] head_det = new SelectList[20];

           // Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;

            if (commandn == "D")
            {
                var bglist = from bg in db.GB_001_HEADER
                             where bg.header_type_code == "Sales" && bg.sequence_no != 99
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
                        var bglist3 = from bg in db.GB_001_DANAL
                                      where bg.header_sequence == item.header_code
                                      select bg;
                        head_det[count2] = new SelectList(bglist3.ToList(), "analysis_code", "analysis_description", glay.vwstrarray6[count2]);

                    }

                }

               // Session["head_det"] = head_det;
                //Session["aheader7"] = aheader7;
                psess.sarrayt1 = glay.vwstrarray5;
            }

        }
        private void tax_sel()
        {
            ModelState.Clear();
            var itemtax = (from bg in db.IV_001_ITEM
                           join bh in db.GB_001_TAX
                           on new { a1 = bg.tax_code1 } equals new {a1 = bh.tax_code}
                           into bh1
                           from bh2 in bh1.DefaultIfEmpty()
                           join bh3 in db.GB_001_TAX
                           on new { a1 = bg.tax_code2 } equals new {a1 = bh3.tax_code}
                           into bh4
                           from bh5 in bh4.DefaultIfEmpty()
                           join bh6 in db.GB_001_TAX
                           on new { a1 = bg.tax_code3 } equals new {a1 = bh6.tax_code}
                           into bh7
                           from bh8 in bh7.DefaultIfEmpty()
                           join bh9 in db.GB_001_TAX
                           on new { a1 = bg.tax_code4 } equals new {a1 = bh9.tax_code}
                           into bh10
                           from bh11 in bh10.DefaultIfEmpty()
                           join bh12 in db.GB_001_TAX
                           on new { a1 = bg.tax_code5 } equals new {a1 = bh12.tax_code}
                           into bh13
                           from bh14 in bh13.DefaultIfEmpty()
                           
                         select new {bh2,bh5,bh8,bh11,bh14}).FirstOrDefault();
            if (itemtax != null)
            {
                glay.vwstrarray7 = new string[20];
                glay.vwdclarray1 = new decimal[20];
                if (itemtax.bh2 != null)
                {
                    glay.vwstrarray7[0] = itemtax.bh2.tax_name;
                    glay.vwdclarray1[0] = Convert.ToDecimal(Session["taxr1"]);
                }
                if (itemtax.bh5 != null)
                {
                    glay.vwstrarray7[1] = itemtax.bh5.tax_name;
                    glay.vwdclarray1[1] = Convert.ToDecimal(Session["taxr2"]);
                }
                if (itemtax.bh8 != null)
                {
                    glay.vwstrarray7[2] = itemtax.bh8.tax_name;
                    glay.vwdclarray1[2] = Convert.ToDecimal(Session["taxr3"]);
                }
                if (itemtax.bh11 != null)
                {
                    glay.vwstrarray7[3] = itemtax.bh11.tax_name;
                    glay.vwdclarray1[3] = Convert.ToDecimal(Session["taxr4"]);
                }
                if (itemtax.bh14 != null)
                {
                    glay.vwstrarray7[4] = itemtax.bh14.tax_name;
                    glay.vwdclarray1[4] = Convert.ToDecimal(Session["taxr5"]);
                }
            }
       
        }
        private void quote_update()
        {
            decimal item_promo = Convert.ToDecimal(Session["fpromoqty"]);
            decimal item_step = Convert.ToDecimal(Session["fstepqty"]);

            string sqlstr = " update AR_002_SALES set order_total_amount = a001-at3, order_total_tax = a002, order_total_discount = a003, order_total_ext_price = a004, order_total_qty = a005 from"; 
            sqlstr += " (select * from";
            sqlstr += " (select sum(order_amount) a001, sum(tax_amount) a002, sum (discount_amount) +" + item_promo + "+" + item_step + " as  a003, sum(ext_price) a004, sum(order_quantity)-" + item_promo + "+" + item_step + "as a005 from AR_002_SODT where  sales_order_sequence=" + util.sqlquote(glay.vwstring7) + ") ax,";
            sqlstr += " (select at1+at2 as at3 from (select distinct tax_invoiceamt1 at1, tax_invoiceamt2 at2 from AR_002_SODT where  sales_order_sequence=" + util.sqlquote(glay.vwstring7) + ") b)bx";
            sqlstr += " ) bxz where order_reference=" + util.sqlquote(glay.vwstring7);
            //sqlstr += " where quote_reference=" + util.sqlquote(preseq);
            db.Database.ExecuteSqlCommand(sqlstr);

            //string sqlstr = " update AR_002_SALES set quote_total_amount = a001, quote_total_tax = a002, quote_total_discount = a003, quote_total_ext_price = a004, quote_total_qty = a005" ;
            //sqlstr += " select souy - at3 as a001 from";
            //sqlstr += " (select sum(quote_amount) souy, sum(tax_amount) a002, sum (discount_amount) a003,";
            //sqlstr += " sum(ext_price) a004, sum(quote_qty)a005 from AR_002_SODT) ax,";
            //sqlstr += " (select at1+at2 as at3 from";
            //sqlstr += " (select distinct tax_invoiceamt1 at1, tax_invoiceamt2 at2 from AR_002_SODT where  quote_reference =" + util.sqlquote(glay.vwstring7) + ") b)bx";

            //sqlstr += " from AR_002_SALES a, (select sum(quote_amount) a001, sum(tax_amount) a002, sum (discount_amount) a003, sum(ext_price) a004, sum(quote_qty)a005 from AR_002_SODT where quote_reference=" + util.sqlquote(glay.vwstring7) + ") bx";
            
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

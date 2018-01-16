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

    public class ReceivedController : Controller
    {
        //
        // GET: /Employee/

        AP_002_GRN AP_002_GRN = new AP_002_GRN();
        AP_002_PURHEAD gdoc = new AP_002_PURHEAD();
        MainContext db = new MainContext();
        MainContext db1 = new MainContext();
        vw_genlay glay = new vw_genlay();
        queryhead qheader = new queryhead();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        discountfsp fsp = new discountfsp();
        HttpPostedFileBase[] photo1;

        string preseq = "";
        string ptype = "";
        decimal f = 0;
        decimal y = 0;
        bool err_flag = true;
        decimal qty = 0; decimal pricep = 0; decimal net_amt = 0; decimal quote_amt = 0; decimal ext_price = 0; string cust_code;
        decimal actual_dis = 0; decimal disc; decimal disc_per1 = 0; string dis_error = ""; decimal item_dis = 0; DateTime expdate;
        string[,] recarray; string sku_name = ""; decimal updated_price = 0;
        string action_flag = "";
        
        int line_cr = 0;

        [EncryptionActionAttribute]
        public ActionResult Index()
        {

            util.init_values();

            //  Session["curren_name"] = "";
          
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            var bglist = from bh in db.AP_002_PURHEAD
                         join bg1 in db.AP_001_VENDR
                        on new { a2 = bh.vendor_code } equals new { a2 = bg1.vendor_code }
                         join bn in db.MC_001_CUREN
                         on new { b3 = bg1.currency_code } equals new { b3 = bn.currency_code }

                         where bh.status == 03 && bh.approval_level == 0
                         select new vw_genlay
                         {
                             vwint0 = bh.purchase_sequence_number,
                             vwstring0 = bh.grn_reference,
                            // vwdecimal0 = bh.grn_total_amount,
                             vwstring1 = bg1.vend_biz_name,
                             vwstring3 = bh.transaction_date,
                            // vwstring4 = bh.order_expiration_date,
                             vwstring2 = bn.currency_description
                         };

            return View(bglist.ToList());


        }
        [EncryptionActionAttribute]
        public ActionResult QuoteConvert(int key1, string key2, string headtype)
        {
            ViewBag.action_flag = "QuoteConvert";
            action_flag = "QuoteConvert";
            psess = (psess)Session["psess"];
            //salseqno = key1.ToString();
            // headtype = "Q";
            validation_routine(headtype);
            gdoc = db.AP_002_PURHEAD.Find(key1);
            if (err_flag)
            {
                set_qheader();
                order_update(key1, key2, headtype);
                return RedirectToAction("CreateDetails");
            }
            return View("QuoteIndex");
        }
        [EncryptionActionAttribute]
        public ActionResult CreateHeader()
        {
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();

            select_query_head("H");
            glay.vwstring8 = "H";
            glay.vwstrarray0[2] = pubsess.base_currency_code;
            glay.vwbool1 = true;
            //Session["curren_name"] = pubsess.base_currency_description;
            glay.vwstrarray0[6] = pubsess.base_currency_description;
            glay.vwstring4 = pubsess.exchange_editable;

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
            glay.vwstring9 = qheader.intquery0.ToString();
            glay.vwstring4 = pubsess.exchange_editable;

            gdoc = db.AP_002_PURHEAD.Find(qheader.intquery0);
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
            glay.vwstring9 = qheader.intquery0.ToString();

            photo1 = photofile;
            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head("H");

            glay.vwstring8 = "H";
            //cur_read();
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult CreateDetails(string headtype = "D")
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];


            initial_rtn();
            waredefault();
            header_ana("D");
            select_query_head("D");
            glay.vwstring8 = headtype;
            glay.vwstring9 = qheader.intquery0.ToString();
            detail_intial();

            show_screen_info();

            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetails(vw_genlay glay_in, string hid_price, string headtype = "D")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
           
            glay = glay_in;
            glay.vwstring9 = qheader.intquery0.ToString();
            glay.vwstring7 = qheader.query3;
            preseq = qheader.query3;

            if (headtype == "send_app")
            {
                //validation_routine(headtype);
                if (err_flag)
                {
                    string str = "update AP_002_PURHEAD set approval_level=99 where purchase_sequence_number=" + qheader.intquery0;
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

            header_ana("D");
            detail_intial();
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
            
            gdoc = db.AP_002_PURHEAD.Find(key1);
            if (gdoc != null)
            {
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

            AP_002_GRN = db.AP_002_GRN.Find(qheader.intquery0, key2, 0);
            if (AP_002_GRN == null)
                return View(glay);

            initial_rtn();
            move_detail();
            header_ana("D");
            select_query_head("D");
            glay.vwstring8 = "D";
            detail_intial();

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
            glay.vwstring9 = qheader.intquery0.ToString();
            glay.vwdecimal4 = Convert.ToDecimal(Session["line_num"]);
            if (headtype == "send_app")
            {
                //validation_routine(headtype);
                if (err_flag)
                {
                    string str = "update AP_002_PURHEAD set approval_level=99 where purchase_sequence_number=" + qheader.intquery0;
                    db.Database.ExecuteSqlCommand(str);
                    return RedirectToAction("CreateHeader");
                }

            }
            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head("D");

            glay.vwstring8 = "H";
            detail_intial();
            show_screen_info();
            header_ana();

            return View(glay);

        }

        private void delete_record()
        {
            AP_002_GRN = db.AP_002_GRN.Find(glay.vwint0, glay.vwint1, 0);
            if (AP_002_GRN != null)
            {
                db.AP_002_GRN.Remove(AP_002_GRN);
                db.SaveChanges();
            }
        }

        private void order_update(int key1, string key2, string headtype)
        {

            var headlist = (from bh in db.AP_001_PUROT
                            where bh.parameter_code == "PURSEQ" && (bh.order_type == "03" || bh.order_type == "single")
                            select bh).FirstOrDefault();
            string pref = headlist.order_prefix;
            int seq = headlist.order_sequence;
            int num = headlist.numeric_size;
            string sqlstr = " update AP_001_PUROT set order_sequence = order_sequence+1 where parameter_code = 'PURSEQ' and (order_type = '03' or order_type = 'single')";
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

            pubsess = (pubsess)Session["pubsess"];
            string str1 = "select order_total_extended_price as vwdecimal0, order_total_tax as vwdecimal3, order_total_amount as vwdecimal4,";
            str1 += " order_date vwstring0, order_expiration_date vwstring1 from AP_002_PURHEAD where order_reference=" + util.sqlquote(key2);
            var str2 = db.Database.SqlQuery<vw_genlay>(str1).FirstOrDefault();

            string str = "update AP_002_PURHEAD set order_reference=" + util.sqlquote(key2) + ",  grn_reference=" + util.sqlquote(preseq) + ", order_total_extended_price=" + str2.vwdecimal0;
            str += ", order_total_tax=" + str2.vwdecimal3 + ", order_total_amount=";
            str += +str2.vwdecimal4 + ", order_date=" + str2.vwstring0 + ", order_expiration_date=" + str2.vwstring1 + ", approval_level=0, status=03 where order_reference=" + util.sqlquote(key2);
            db.Database.ExecuteSqlCommand(str);

            int hbug = 0;
            // int.TryParse(salseqno, out hbug);
            var bglist = from bh in db.AP_002_POD
                         where bh.purchase_sequence_number == key1
                         select new vw_genlay
                         {
                             vwint0 = bh.purchase_sequence_number,
                             vwint1 = bh.line_sequence,
                             vwint2 = bh.sub_line_sequence,
                             vwstring1 = bh.purchase_order_sequence,
                             vwstring0 = bh.item_code,
                             vwdecimal0 = bh.price,
                             vwdecimal1 = bh.order_quantity,
                             vwdecimal2 = bh.extended_price,
                             vwdecimal3 = bh.wht,
                             //vwdecimal4 = bh.actual_discount,
                             vwdecimal5 = bh.order_amount,
                             vwdecimal6 = bh.net_amount,
                             vwstring2 = bh.base_amount.ToString(),
                             //vwstring3 = bh.order,
                             vwstring4 = bh.vendor_code
                         };
            var xodr = bglist.ToList();
            foreach (var item in xodr)
            {
                AP_002_GRN AP_002_GRN = new AP_002_GRN();
               // AP_002_GRN.include_tax = "";
                AP_002_GRN.created_by = pubsess.userid;
                AP_002_GRN.created_date = DateTime.UtcNow;
                AP_002_GRN.purchase_sequence_number = item.vwint0;
                AP_002_GRN.line_sequence = item.vwint1;
                AP_002_GRN.sub_line_sequence = item.vwint2;
               AP_002_GRN.purchase_grn_sequence = item.vwstring1;
                AP_002_GRN.item_code = item.vwstring0;
                AP_002_GRN.item_warehouse_code = "";
                AP_002_GRN.vendor_code = item.vwstring4;
                AP_002_GRN.grn_quantity = 0;
                //AP_002_GRN.grn_date = DateTime.UtcNow;
                AP_002_GRN.po_quantity = item.vwdecimal1;
                AP_002_GRN.outstanding_quantity = item.vwdecimal1;
                AP_002_GRN.price = item.vwdecimal0;
                AP_002_GRN.wht = item.vwdecimal3;
                //AP_002_GRN.tax_amount1 = 0;
                //AP_002_GRN.tax_amount2 = 0;
                //AP_002_GRN.tax_amount3 = 0;
                //AP_002_GRN.tax_amount4 = 0;
                //AP_002_GRN.tax_amount5 = 0;
                AP_002_GRN.base_amount = 0;
                AP_002_GRN.extended_price = item.vwdecimal2;
                //AP_002_GRN.discount_percent = 0;
                // AP_002_GRN.discount_amount = 0;
                //AP_002_GRN.net_amount = item.vwdecimal6;
                //AP_002_GRN.order_amount = item.vwdecimal5;
                // AP_002_GRN.actual_discount = item.vwdecimal4;
                AP_002_GRN.base_amount = Convert.ToDecimal(item.vwstring2);
                // AP_002_GRN.dis_flag = item.vwstring3;
                AP_002_GRN.analysis_code1 = "";
                AP_002_GRN.analysis_code2 = "";
                AP_002_GRN.analysis_code3 = "";
                AP_002_GRN.analysis_code4 = "";
                AP_002_GRN.analysis_code5 = "";
                AP_002_GRN.analysis_code6 = "";
                AP_002_GRN.analysis_code7 = "";
                AP_002_GRN.analysis_code8 = "";
                AP_002_GRN.analysis_code9 = "";
                AP_002_GRN.analysis_code10 = "";
                AP_002_GRN.modified_date = DateTime.UtcNow;
                AP_002_GRN.modified_by = pubsess.userid;
                AP_002_GRN.additional_description = "";
                AP_002_GRN.transaction_type = "03";
                AP_002_GRN.bin_location_code = "";
                AP_002_GRN.grn_date = DateTime.Now.ToString("yyyyMMdd");
                
                db.Entry(AP_002_GRN).State = EntityState.Added;

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
            if (xodr.Count > 0)
            {
                if (headtype == "with")
                {
                    recarray = fsp.itemdis_calculation(qheader.intquery0.ToString(), 2);
                    createtrans();
                    recarray = fsp.totalinv_calculation(qheader.intquery0.ToString(), 2);
                    createtrans();
                    string strq = "update AP_002_GRN set tax_invoiceamt1 = tax_amount1, tax_invoiceamt2 = tax_amount2, tax_amount1 = 0, tax_amount2 = 0 where purchase_sequence_number =" + qheader.intquery0.ToString() + " and item_code = 'InvoiceTax'";
                    db.Database.ExecuteSqlCommand(strq);
                    quote_update();

                }
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
                    gdoc = new AP_002_PURHEAD();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;
                    gdoc.approval_level = 0;
                    gdoc.approval_date = DateTime.UtcNow;
                    var headlist = (from bh in db.AP_001_PUROT
                                    where bh.parameter_code == "PURSEQ" && (bh.order_type == "03" || bh.order_type == "single")
                                    select bh).FirstOrDefault();
                    string pref = headlist.order_prefix;
                    int seq = headlist.order_sequence;
                    int num = headlist.numeric_size;
                    string sqlstr = " update AP_001_PUROT set order_sequence = order_sequence+1 where parameter_code = 'PURSEQ' and (order_type = '02' or order_type = 'single')";
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
                    gdoc.grn_reference = preseq;
                }
                else
                {
                    gdoc = db.AP_002_PURHEAD.Find(qheader.intquery0);
                }
                string dh1 = util.date_yyyymmdd(glay.vwstrarray0[4]);
                DateTime q_date = new DateTime(Convert.ToInt16(dh1.Substring(0, 4)), Convert.ToInt16(dh1.Substring(4, 2)), Convert.ToInt16(dh1.Substring(6, 2)));
                expdate = q_date.AddDays(glay.vwint1);
                gdoc.status = 03;
                gdoc.purchase_transaction_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : (glay.vwstrarray0[0]);
                gdoc.vendor_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : (glay.vwstrarray0[1]);
                gdoc.currency_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : (glay.vwstrarray0[2]);
                //gdoc.vend_order_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : (glay.vwstrarray0[10]);
                gdoc.exchange_rate = glay.vwdecimal0;
                gdoc.number_of_day = glay.vwint1;
                gdoc.transaction_date = util.date_yyyymmdd(glay.vwstrarray0[4]);
               // gdoc.order_expiration_date = expdate.ToString("yyyyMMdd");
               // gdoc.order_date = util.date_yyyymmdd(glay.vwstrarray0[4]);
               // gdoc.expected_delivery_date = util.date_yyyymmdd(glay.vwstrarray0[5]);
               // gdoc.delivery_address_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : (glay.vwstrarray0[8]);
                gdoc.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : (glay.vwstrarray0[5]);
                gdoc.exchange_rate_mode = pubsess.exchange_rate_mode;

                if (gdoc.currency_code == pubsess.base_currency_code)
                    gdoc.exchange_rate = 1;
                else
                {
                    if (glay.vwstring4 != "Y")
                    {
                        string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(gdoc.currency_code) + " and '" + gdoc.transaction_date + "' between date_from and date_to";
                        var exch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                        if (exch != null)
                            gdoc.exchange_rate = exch.dquery0;
                    }
                }

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
                    AP_002_GRN = new AP_002_GRN();
                    AP_002_GRN.created_by = pubsess.userid;
                    AP_002_GRN.created_date = DateTime.UtcNow;
                    glay.vwint2 = get_next_line_sequence();
                }
                else
                {
                    AP_002_GRN = db.AP_002_GRN.Find(qheader.intquery0, glay.vwint2, 0);
                }

                //the detailsub button has been clicked
                get_price(glay.vwstring0);
               // glay.vwdclarray0[0] = updated_price;

                //decimal[] decarray = quote_line(glay.vwstring0, glay.vwdclarray0[0], glay.vwdclarray0[1]);

                //ext_price = glay.vwdclarray0[1] * pricep;
                //disc = glay.vwdclarray0[2];
                //disc_per1 = glay.vwdclarray0[3];


                //decimal tax = decarray[2] + decarray[3] + decarray[4] + decarray[5] + decarray[6];
                //net_amt = decarray[0];

                AP_002_GRN.additional_description = "";
                AP_002_GRN.purchase_sequence_number = qheader.intquery0;
                AP_002_GRN.line_sequence = glay.vwint2;
                AP_002_GRN.sub_line_sequence = 0;
                AP_002_GRN.purchase_grn_sequence = qheader.query3; //string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : (glay.vwstring7);
                AP_002_GRN.item_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                AP_002_GRN.vendor_code = qheader.query1;
                AP_002_GRN.item_warehouse_code = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                AP_002_GRN.transaction_type = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
                AP_002_GRN.bin_location_code = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
                AP_002_GRN.po_quantity = 0;
                AP_002_GRN.outstanding_quantity = 0;
                AP_002_GRN.grn_quantity = glay.vwdecimal1;
                AP_002_GRN.price = 0;
                AP_002_GRN.wht = 0;
                //AP_002_GRN.tax_amount2 = decarray[3];
                //AP_002_GRN.tax_amount3 = decarray[4];
                //AP_002_GRN.tax_amount4 = decarray[5];
                //AP_002_GRN.tax_amount5 = decarray[6];
                AP_002_GRN.base_amount = 0;
                AP_002_GRN.extended_price = net_amt;
                // AP_002_GRN.discount_percent = glay.vwdclarray0[3];
                // AP_002_GRN.discount_amount = glay.vwdclarray0[2];
                AP_002_GRN.bin_location_code = "";
                //AP_002_GRN.order_amount = decarray[7];
                AP_002_GRN.grn_date = util.date_yyyymmdd(glay.vwstring6);
                glay.vwdecimal0 = qheader.dquery0;
               // AP_002_GRN.base_amount = AP_002_GRN.order_amount;

                //AP_002_GRN.dis_flag = "N";
               // AP_002_GRN.include_tax = "";
                AP_002_GRN.analysis_code1 = "";
                AP_002_GRN.analysis_code2 = "";
                AP_002_GRN.analysis_code3 = "";
                AP_002_GRN.analysis_code4 = "";
                AP_002_GRN.analysis_code5 = "";
                AP_002_GRN.analysis_code6 = "";
                AP_002_GRN.analysis_code7 = "";
                AP_002_GRN.analysis_code8 = "";
                AP_002_GRN.analysis_code9 = "";
                AP_002_GRN.analysis_code10 = "";
                AP_002_GRN.modified_date = DateTime.UtcNow;
                AP_002_GRN.modified_by = pubsess.userid;


                //if (glay.vwstrarray6 != null)
                //{
                //    int arrlen = glay.vwstrarray6.Length;
                //    if (arrlen > 0)
                //        AP_002_GRN.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                //    if (arrlen > 1)
                //        AP_002_GRN.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                //    if (arrlen > 2)
                //        AP_002_GRN.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                //    if (arrlen > 3)
                //        AP_002_GRN.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                //    if (arrlen > 4)
                //        AP_002_GRN.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                //    if (arrlen > 5)
                //        AP_002_GRN.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                //    if (arrlen > 6)
                //        AP_002_GRN.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                //    if (arrlen > 7)
                //        AP_002_GRN.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                //    if (arrlen > 8)

                //        AP_002_GRN.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                //    if (arrlen > 9)
                //        AP_002_GRN.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                //    psess.intemp0 = arrlen;
                //    Session["psess"] = psess;
            
                //}

                if (cr_flag == "CreateDetails")
                    db.Entry(AP_002_GRN).State = EntityState.Added;
                else
                    db.Entry(AP_002_GRN).State = EntityState.Modified;
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
                string str = " update AP_002_PURHEAD set exchange_rate =" + qheader.dquery0 + " where purchase_sequence_number =" + qheader.intquery0;
                db.Database.ExecuteSqlCommand(str);
                //call_discount();
                qheader.bquery0 = false;
                Session["qheader"] = qheader;
            }

            if (err_flag && cr_flag.IndexOf("Header") > 0)
            {
                {
                    set_qheader();
                    util.write_document("PURGRN", gdoc.purchase_sequence_number.ToString(), photo1, glay.vwstrarray9);
                }

            }

        }

        [EncryptionActionAttribute]
        public ActionResult QuoteIndex()
        {

            util.init_values();

            Session["hsseq"] = "";
            Session["curren_name"] = "";
            pubsess = (pubsess)Session["pubsess"];
            string t = DateTime.Now.ToString("yyyyMMdd");
            int text = Convert.ToInt32(t);
            var bglist = from bh in db.AP_002_PURHEAD
                         join bg1 in db.AP_001_VENDR
                        on new { a2 = bh.vendor_code } equals new { a2 = bg1.vendor_code }
                         join bn in db.MC_001_CUREN
                         on new { b3 = bg1.currency_code } equals new { b3 = bn.currency_code }

                         where bh.status == 02 && bh.approval_level == 99
                         select new vw_genlay
                         {
                             vwint0 = bh.purchase_sequence_number,
                             vwstring0 = bh.order_reference,
                             vwdecimal0 = bh.order_total_amount,
                             vwstring1 = bg1.vend_biz_name,
                             vwstring3 = bh.order_date,
                             vwstring4 = bh.order_expiration_date,
                             vwstring2 = bn.currency_description,
                             vwdate2 = DateTime.UtcNow

                         };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Expired()
        {

            util.init_values();

            Session["hsseq"] = "";
            Session["curren_name"] = "";
            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.AP_002_PURHEAD
                         join bg1 in db.AP_001_VENDR
                        on new { a2 = bh.vendor_code } equals new { a2 = bg1.vendor_code }
                         join bn in db.MC_001_CUREN
                         on new { b3 = bg1.currency_code } equals new { b3 = bn.currency_code }

                         where bh.status == 01 && bh.approval_level == 99
                         select new vw_genlay
                         {
                             vwint0 = bh.purchase_sequence_number,
                             //vwstring0 = bh.quote_reference,
                             vwdecimal0 = bh.quote_total_amount,
                             vwstring1 = bg1.vend_biz_name,
                             vwstring3 = bh.order_date,
                             vwstring4 = bh.order_expiration_date,
                             vwstring2 = bn.currency_description,
                             vwdate2 = DateTime.UtcNow

                         };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult ConfirmOrder()
        {

            util.init_values();

            Session["hsseq"] = "";
            Session["curren_name"] = "";
            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.AP_002_PURHEAD
                         join bg1 in db.AP_001_VENDR
                       on new { a2 = bh.vendor_code } equals new { a2 = bg1.vendor_code }
                         join bn in db.MC_001_CUREN
                         on new { b3 = bg1.currency_code } equals new { b3 = bn.currency_code }

                         where bh.status == 02 || bh.order_reference != "" && bh.approval_level == 99
                         select new vw_genlay
                         {
                             vwint0 = bh.purchase_sequence_number,
                             vwstring0 = bh.order_reference,
                             vwdecimal0 = bh.order_total_amount,
                             vwstring1 = bg1.vend_biz_name,
                             vwstring3 = bh.order_date,
                             vwstring4 = bh.order_expiration_date,
                             vwstring2 = bn.currency_description,
                             vwdate2 = DateTime.UtcNow

                         };

            return View(bglist.ToList());


        }

        private void waredefault()
        {
            string wdefault = (from bg in db.IV_001_WAREH
                               where bg.default_warehouse == "Y"
                               select bg.warehouse_code).FirstOrDefault();
            glay.vwstring1 = wdefault;
        }
        private int get_next_line_sequence()
        {
            string sqlstr = "select isnull(max(line_sequence),0) vwint1 from AP_002_GRN where line_sequence <> 999 and purchase_grn_sequence=" + util.sqlquote(qheader.query3);
            var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
            return sql1.vwint1 + 1;

        }

        private void createtrans()
        {

            int hbug = qheader.intquery0;
            string quoteref = qheader.query3;
            decimal quote_amt = 0;

            for (int rctr = 0; rctr < recarray.GetLength(0); rctr++)
            {
                if (recarray[rctr, 0] == null)
                    break;

                string icode = recarray[rctr, 0];
                decimal tax = Convert.ToDecimal(recarray[rctr, 3]) + Convert.ToDecimal(recarray[rctr, 4]) + Convert.ToDecimal(recarray[rctr, 5]) + Convert.ToDecimal(recarray[rctr, 6]) + Convert.ToDecimal(recarray[rctr, 7]);
                //if (taxinc == "Y")
                //{
                //    quote_amt= Convert.ToDecimal(recarray[rctr, 12]) - tax;
                //    recarray[rctr, 12] = quote_amt.ToString();
                //}

                //quote_amt = Convert.ToDecimal(recarray[rctr, 12]) + Convert.ToDecimal(recarray[rctr, 2]) + tax;
                //if (recarray[rctr, 10] == "G")
                //{
                //    quote_amt = Convert.ToDecimal(recarray[rctr, 1]);
                //    recarray[rctr, 1] = "0";
                //}

                //decimal price = (Convert.ToDecimal(recarray[rctr, 1]) + Convert.ToDecimal(recarray[rctr, 2]))/Convert.ToDecimal(recarray[rctr,9]);
                string subseq = recarray[rctr, 10] == "M" ? "1" : recarray[rctr, 10] == "F" ? "2" : recarray[rctr, 10] == "Y" ? "3" : recarray[rctr, 10] == "S" ? "4" : recarray[rctr, 10] == "L" ? "5" : recarray[rctr, 10] == "I" ? "6" : "0";

                string str1 = "insert into AP_002_GRN(purchase_sequence_number,line_sequence,sub_line_sequence,sales_order_sequence,item_code,order_quantity,price,extended_price,discount_percent,discount_amount,";
                str1 += "actual_discount,net_amount,wht,order_amount,tax_amount1,tax_amount2,tax_amount3,tax_amount4,tax_amount5,tax_invoiceamt1,tax_invoiceamt2,";
                str1 += "tax_invoice,base_amount,analysis_code1,analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,";
                str1 += "analysis_code7,analysis_code8,analysis_code9,analysis_code10,dis_flag,modified_by,created_by) values(";
                str1 += hbug.ToString() + "," + recarray[rctr, 13] + "," + subseq + "," + util.sqlquote(quoteref) + ",";
                str1 += util.sqlquote(recarray[rctr, 0]) + ",";
                str1 += util.sqlquote(recarray[rctr, 9]) + ",";
                str1 += util.sqlquote(recarray[rctr, 11]) + ",";
                str1 += util.sqlquote(recarray[rctr, 12]) + ",";
                str1 += "0,0,";
                str1 += recarray[rctr, 2] + ",";

                str1 += "0,";
                str1 += tax.ToString() + ",";
                str1 += recarray[rctr, 14] + ",";
                //if (recarray[rctr, 10] == "I")
                //    str1 += tax.ToString() +",";
                //else
                //    str1 += quote_amt.ToString() + ",";
                str1 += recarray[rctr, 3] + ",";
                str1 += recarray[rctr, 4] + ",";
                str1 += recarray[rctr, 5] + ",";
                str1 += recarray[rctr, 6] + ",";
                str1 += recarray[rctr, 7] + ",0,0,0,0,'','','','','','','','','','',";
                str1 += util.sqlquote(recarray[rctr, 10]) + ",";
                str1 += util.sqlquote(pubsess.userid) + ",";
                str1 += util.sqlquote(pubsess.userid);
                str1 += ")";

                try
                {
                    db.Database.ExecuteSqlCommand(str1);
                }

                catch (Exception err)
                {
                    if (err.InnerException == null)
                        ModelState.AddModelError(String.Empty, err.Message);
                    else
                        ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);

                    err_flag = false;
                }

                line_cr++;

            }
        }
        private void validation_routine(string headtype)
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            DateTime date_chk = Convert.ToDateTime(psess.temp4);
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;


            string cr_flag = action_flag;
            DateTime date_chki = DateTime.Now;
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
                //if (!util.date_validate(glay.vwstrarray0[5]))
                //{
                //    ModelState.AddModelError(String.Empty, "Please insert a valid expected delivery date");
                //    err_flag = false;
                //}
                if (glay.vwdecimal0 == 0)
                {
                    ModelState.AddModelError(String.Empty, "Exchange Rate can not be Zero");
                    err_flag = false;
                }
            }


            if (cr_flag.IndexOf("Details") > 0)
            {
                //if (glay.vwdclarray0[0] == 0)
                //{
                //    ModelState.AddModelError(String.Empty, "Price can not be Zero");
                //    err_flag = false;
                //}
                if (qheader.dquery0 == 0)
                {
                    ModelState.AddModelError(String.Empty, "Exchange Rate can not be Zero");
                    err_flag = false;
                }
                //if (glay.vwstring5 == "Y")
                //{
                //    if (glay.vwdclarray0[2] != 0 && glay.vwdclarray0[3] != 0)
                //    {
                //        ModelState.AddModelError(String.Empty, "You can either enter Discount Amount or Discount Percent");
                //        err_flag = false;
                //    }
                //}
                if (glay.vwdclarray0[1] == 0)
                {
                    ModelState.AddModelError(String.Empty, "Enter a valid Quantity Value");
                    err_flag = false;
                }


                var headlist = (from bh in db.AP_001_PUROT
                                where bh.parameter_code == "PURSEQ" && (bh.order_type == "02" || bh.order_type == "single")
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

                //if (headtype == "without"||headtype=="with") //----
                //{
                //    string cur_date = DateTime.UtcNow.ToString("yyyyMMdd");
                //    string contrl_check = (from bg in db.AP_002_PURHEAD
                //                           where bg.purchase_sequence_number == qheader.intquery0
                //                           select bg.quote_expiration_date).FirstOrDefault();
                //    if (string.Compare(contrl_check, cur_date) > 0)
                //    {
                //        ModelState.AddModelError(string.Empty, "Quote has Expired");
                //        err_flag = false;
                //    }

                //}
            }

        }
        //private void dis_validation(string disc_amt, string disc_per)
        //{
        //    initial_rtn();
        //    decimal.TryParse(disc_amt, out disc);
        //    decimal.TryParse(disc_per, out disc_per1);


        //    if (disc != 0 && disc_per1 != 0)
        //    {
        //        ModelState.AddModelError(String.Empty, "You can either enter Discount Amount or Discount Percent");
        //        err_flag = false;
        //        dis_error = "You can either enter Discount Amount or Discount Percent";
        //    }


        //}

        private void select_query_head(string type1)
        {
            if (type1 == "H")
            {
                year_cal();
                ViewBag.cust = util.para_selectquery("002", glay.vwstrarray0[1]);

                var empz = from pf in db.AP_001_PTRAN
                           where pf.purchase_order == "Y" && pf.active_status == "N"
                           orderby pf.purchase_order_name
                           select pf;
                ViewBag.salestt = new SelectList(empz.Distinct().ToList(), "purchase_order_code", "purchase_order_name", glay.vwstrarray0[0]);

                //var del = from jk in db.AR_001_DADRS
                //          where jk.address_type == "CU" && jk.active_status == "N"
                //          orderby jk.location_alias
                //          select jk;
                //ViewBag.deladd = new SelectList(del.ToList(), "address_code", "location_alias", glay.vwstrarray0[6]);
            }


            if (type1 == "D")
            {
                var empt = from pf in db.GB_999_MSG
                           where pf.type_msg == "TRT" && pf.code_msg == "03"
                           orderby pf.name1_msg
                           select pf;
                ViewBag.type = new SelectList(empt.ToList(), "code_msg", "name1_msg", glay.vwstring5);

                // ViewBag.item = util.para_selectquery("008", glay.vwstring0);
                var empe = from pf in db.IV_001_ITEM
                           where pf.active_status == "N" && pf.sales == "Y"
                           orderby pf.item_code, pf.item_name
                           select new { c1 = pf.item_code, c2 = pf.item_code + "--- " + pf.item_name };
                ViewBag.item = new SelectList(empe.ToList(), "c1", "c2", glay.vwstring0);

                string str1 = "select warehouse_code query0, c2 query1 from vw_warehouse_site ";
                var emp1 = db.Database.SqlQuery<querylay>(str1);
                ViewBag.itemw = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring1);

                var emp = from pf in db.GB_001_PCODE
                           where pf.parameter_type == "02"
                           select pf;
                ViewBag.bin = new SelectList(emp.ToList(), "parameter_code", "parameter_name", glay.vwstring3);


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
            pubsess = (pubsess)Session["pubsess"];
            glay.vwdclarray0 = new decimal[50];
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwstrarray0[wctr] = "";
            glay.vwstrarray0[3] = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwstrarray0[4] = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray7 = new string[20];
            glay.vwstring5 = "N";
            glay.vwstring1 = (from bg in db.IV_001_WAREH
                              where bg.default_warehouse == "Y"
                              select bg.warehouse_code).FirstOrDefault();
            psess.temp1 = pubsess.manual_discount;
            glay.vwlist0 = new List<querylay>[20];
            Session["psess"] = psess;
            
        }


        private void cur_read()
        {
            string curcode = "";
            curcode = glay.vwstrarray0[2];
            string read_cur = (from bg in db.MC_001_CUREN
                               where bg.currency_code == curcode
                               select bg.currency_description).FirstOrDefault();

            //  Session["curren_name"] = read_cur;
            glay.vwstrarray0[6] = read_cur;
        }
        private void error_message()
        {

        }
        [HttpPost]
        public ActionResult due_date(string id)
        {
            int expday = 0;
            int.TryParse(id, out expday);
            DateTime todaydate = DateTime.Today;
            expdate = todaydate.AddDays(expday);

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = (expdate).ToString("dd/MM/yyyy") });

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
        public ActionResult item_bal(string ware_code)
        {

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
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AP_002_PURHEAD] where cast (purchase_sequence_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "delete from [dbo].[AP_002_GRN] where cast(purchase_sequence_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        public ActionResult delete_detail(string id)
        {
            // write your query statement
            qheader = (queryhead)Session["qheader"];
            string sqlstr = "delete from [dbo].[AP_002_GRN] where cast(purchase_sequence_number as varchar) +'[]'+ cast(line_sequence as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "select count(0) intquery0 from AP_002_GRN where purchase_sequence_number=" + qheader.intquery0.ToString();
            var delctr = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (delctr.intquery0 == 0)
            {
                sqlstr = "delete from AP_002_GRN where purchase_sequence_number=" + qheader.intquery0.ToString();
                db.Database.ExecuteSqlCommand(sqlstr);
                sqlstr = "update AP_002_PURHEAD set order_total_ext_price=0,order_total_discount=0, order_total_inv_tax=0,order_total_tax=0,order_total_amount=0 ";
                sqlstr += " where purchase_sequence_number=" + qheader.intquery0.ToString();
                db.Database.ExecuteSqlCommand(sqlstr);

            }
            else
               // call_discount();

            display_header();
            select_queryhide("D");
            if (HttpContext.Request.IsAjaxRequest())
                return Json(
                            JsonRequestBehavior.AllowGet);
            return RedirectToAction("CreateDetails");

        }

        [HttpPost]
        public ActionResult void_list(string id)
        {
            // write your query statement
            string str = "update AP_002_PURHEAD set approval_level=100 where purchase_sequence_number=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(str);
            return RedirectToAction("QuoteIndex");
        }
        [HttpPost]
        public ActionResult reactivate_list(string id)
        {
            // write your query statement
            int intid;
            int.TryParse(id, out intid);
            int numofday = (from bg in db.AP_002_PURHEAD
                            where bg.purchase_sequence_number == intid
                            select bg.number_of_day).FirstOrDefault();
            string newquotedate = DateTime.Now.ToString("yyyyMMdd");
            DateTime q_date = new DateTime(Convert.ToInt16(newquotedate.Substring(0, 4)), Convert.ToInt16(newquotedate.Substring(4, 2)), Convert.ToInt16(newquotedate.Substring(6, 2)));
            expdate = q_date.AddDays(numofday);
            string strexpdate = expdate.ToString("yyyyMMdd");

            string str = "update AP_002_PURHEAD set quote_date=" + newquotedate + ", quote_expiration_date=" + strexpdate + " where purchase_sequence_number=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(str);
            return RedirectToAction("Expired");
        }
        [HttpPost]
        public ActionResult confirm_list(string id)
        {
            // write your query statement
            string str = "update AP_002_PURHEAD set approval_level=101 where purchase_sequence_number=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(str);
            return RedirectToAction("ConfirmOrder");
        }
        private void read_details()
        {
            int hbug = qheader.intquery0;
            decimal exc_rt = qheader.dquery1;

            var bglist = from bh in db.AP_002_GRN
                         join bg in db.IV_001_ITEM
                         on new { a1 = bh.item_code } equals new { a1 = bg.item_code }
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join bf in db.GB_001_PCODE
                         on new { a1 = bg2.sku_sequence, a2 = "10" } equals new { a1 = bf.parameter_code, a2 = bf.parameter_type }
                         into bf1
                         from bf2 in bf1.DefaultIfEmpty()
                         join gh in db.GB_999_MSG
                         on new { a1 = "TRT", a2 = bh.transaction_type } equals new { a1=gh.type_msg, a2=gh.code_msg}
                         into gh1
                         from gh2 in gh1.DefaultIfEmpty()
                         where bh.purchase_sequence_number == hbug
                         orderby bh.line_sequence//,bh.sub_line_sequence
                         select new vw_genlay
                         {
                             vwint0 = bh.purchase_sequence_number,
                             vwint1 = bh.line_sequence,
                             vwint2 = bh.sub_line_sequence,
                            vwstring1 = bh.purchase_grn_sequence,
                             vwstring2 = bg2.item_name,
                             vwstring0 = bh.item_code,
                             vwstring3 = bf2.parameter_name,
                             vwdecimal0 = bh.price * exc_rt,
                             vwdecimal1 = bh.grn_quantity,
                             vwdecimal2 = bh.extended_price * exc_rt,
                             vwdecimal3 = bh.wht * exc_rt,
                             //vwdecimal4 = bh.actual_discount * exc_rt,
                             //vwdecimal5 = bh.order_amount * exc_rt,
                             vwstring4 = bh.transaction_type,
                             vwstring6 = gh2.name1_msg,
                             //vwdecimal6 = bh.delivery_quantity
                         };

            var bgl = bglist.ToList();
            ViewBag.x1 = bgl;
            if (bgl.Count > 0)
            {
                qheader.bquery0 = false;
                Session["qheader"] = qheader;
                glay.vwbool1 = false;
            }


        }

        private void show_screen_info()
        {
            display_header();
            read_details();

        }

        private void detail_intial()
        {
            glay.vwstring7 = qheader.query3;
            glay.vwstring2 = qheader.query5;
            glay.vwstring3 = qheader.dquery0.ToString();
            preseq = qheader.query3;
            if (pubsess.price_editable == "Y")
                glay.vwbool0 = true;
            glay.vwbool1 = qheader.bquery0;
            glay.vwstring4 = pubsess.price_editable;

        }
        private void read_header()
        {

            string amt_check = "";
            glay.vwstrarray0 = new string[20];
            int hbug = qheader.intquery0;

            var bg2list = (from bh in db.AP_002_PURHEAD
                           join bh1 in db.AP_001_VENDR
                          on new { a1 = bh.vendor_code } equals new { a1 = bh1.vendor_code }
                           where bh.purchase_sequence_number == hbug
                           select new { bh, bh1 }).FirstOrDefault();

            if (bg2list != null)
            {
                glay.vwstrarray0[0] = bg2list.bh.purchase_transaction_type;
                glay.vwstrarray0[1] = bg2list.bh1.vendor_code;
                glay.vwstrarray0[2] = bg2list.bh.currency_code;
                glay.vwdecimal0 = bg2list.bh.exchange_rate;
                glay.vwstrarray0[3] = util.date_slash(bg2list.bh.transaction_date);
                glay.vwstrarray0[4] = util.date_slash(bg2list.bh.order_date);
                glay.vwint1 = bg2list.bh.number_of_day;
                glay.vwstrarray0[6] = qheader.query6;
                glay.vwstrarray0[5] = bg2list.bh.note;
                glay.vwstrarray0[10] = bg2list.bh.vend_order_number;
                string docode = gdoc.purchase_sequence_number.ToString();
                var bglist = from bg in db.GB_001_DOC
                             where bg.screen_code == "SALES" && bg.document_code == docode
                             orderby bg.document_sequence
                             select bg;

                ViewBag.anapict = bglist.ToList();

                //amt_check = "N";
                ////cur_read();
                //if (bg2list.bh.currency_code == pubsess.base_currency_code)
                //    amt_check = "Y";

                //psess.temp4 = amt_check;
                //psess.temp3 = glay.vwstrarray0[1];

            }
        }

        private void exp_cal()
        {

        }
        private void display_header()
        {

            //ModelState.Clear();
            ModelState.Remove("vwstrarray1[3]");
            ModelState.Remove("vwstrarray1[4]");
            ModelState.Remove("vwstrarray1[5]");
            ModelState.Remove("vwstrarray1[6]");
            ModelState.Remove("vwstrarray1[7]");
            ModelState.Remove("exchrate");
            expdate = DateTime.Now;
            int qdate = qheader.intquery0;
            var chkdate = (from bg in db.AP_002_PURHEAD
                           where bg.purchase_sequence_number == qdate
                           select new
                           {
                               c0 = bg.order_date,
                               c1 = bg.number_of_day
                           }).FirstOrDefault();
            int numday = chkdate.c1;

            string dh1 = chkdate.c0;
            if (!string.IsNullOrWhiteSpace(dh1))
            {
                DateTime quotedate = new DateTime(Convert.ToInt16(dh1.Substring(0, 4)), Convert.ToInt16(dh1.Substring(4, 2)), Convert.ToInt16(dh1.Substring(6, 2)));
                expdate = quotedate.AddDays(numday);
            }

            decimal exc_rt = qheader.dquery1;

            int hbug = qheader.intquery0;
            var bg2list = (from bh in db.AP_002_PURHEAD
                           join bh1 in db.AP_001_VENDR
                           on new { a1 = bh.vendor_code } equals new { a1 = bh1.vendor_code }
                           join bj in db.AP_001_PTRAN
                           on new { a1 = bh.purchase_transaction_type } equals new { a1 = bj.purchase_order_code }
                           into bj1
                           from bj2 in bj1.DefaultIfEmpty()
                           where bh.purchase_sequence_number == hbug
                           select new { bh, bh1, bj2 }).FirstOrDefault();


            vw_genlay glayhead = new vw_genlay();
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwstrarray1[0] = bg2list.bh1.vend_biz_name;
                glayhead.vwstrarray1[2] = expdate.ToString("dd/MM/yyyy");
                glayhead.vwstrarray1[1] = qheader.query3 == "" ? bg2list.bh.order_reference : qheader.query3;
                glayhead.vwstrarray1[3] = (bg2list.bh.order_total_extended_price * exc_rt).ToString();
                glayhead.vwstrarray1[7] = util.date_slash(bg2list.bh.order_date);
                glayhead.vwstrarray1[8] = bg2list.bj2.purchase_order_name;
                glayhead.vwstrarray1[4] = (bg2list.bh.order_total_discount * exc_rt).ToString();
                glayhead.vwstrarray1[5] = (bg2list.bh.order_total_tax * exc_rt).ToString();
                glayhead.vwstrarray1[6] = (bg2list.bh.order_total_amount * exc_rt).ToString();
                glayhead.vwstrarray1[9] = util.date_slash(qheader.query2);
                glayhead.vwstrarray1[10] = qheader.dquery0.ToString();
                glayhead.vwstrarray1[11] = qheader.query6;
                psess.temp3 = bg2list.bh.vendor_code;
                Session["psess"] = psess;
            
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
            gdoc.purchase_quote_date = "";
            gdoc.purchase_quote_expiration_date = "";
            gdoc.purchase_order_date = "";
            gdoc.purchase_order_expiration_date = "";
            gdoc.attach_document = "";
            gdoc.grn_reference = "";
            gdoc.order_date = "";
            gdoc.requisition_date = "";
            gdoc.requisition_reference = "";
            gdoc.order_expiration_date = "";
            gdoc.expected_delivery_date = "";
            gdoc.order_total_discount = 0;
            gdoc.order_total_extended_price = 0;
            gdoc.order_total_tax = 0;
            gdoc.quote_total_discount = 0;
            gdoc.quote_total_extended_price = 0;
            //gdoc.quote_total_qty = 0;
            gdoc.quote_total_tax = 0;
            gdoc.vend_order_number = "";
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
            gdoc.vendor_code = "";
            gdoc.delivery_address_code = "";
            gdoc.exchange_rate = 0;
            gdoc.purchase_transaction_type = "";
            gdoc.status = 0;
            //gdoc.invoice_reference = "";
            gdoc.order_reference = "";
            gdoc.quote_reference = "";
        }

        [HttpPost]
        public ActionResult get_currency(string cust_code, string tdate)
        {
            // write your query statement

            tdate = util.date_yyyymmdd(tdate);
            decimal rat_code = 0;

            var curlist = (from bg in db.MC_001_CUREN
                           join bh in db.AP_001_VENDR
                           on new { c1 = bg.currency_code } equals new { c1 = bh.currency_code }
                           where bh.vendor_code == cust_code
                           select new
                           {
                               c1 = bg.currency_description,
                               c0 = bg.currency_code
                           }).FirstOrDefault();



            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = curlist.c0, Text = curlist.c1 });

            pubsess pubsess = (pubsess)Session["pubsess"];
            if (pubsess.base_currency_code == curlist.c0)
                ary.Add(new SelectListItem { Value = curlist.c0, Text = "1" });
            else
            {
                string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(curlist.c0) + " and '" + tdate + "' between date_from and date_to";
                var dbexch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                if (dbexch != null)
                    rat_code = dbexch.dquery0;

                ary.Add(new SelectListItem { Value = curlist.c0, Text = rat_code.ToString() });
            }


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
            get_price(item_code);

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "sku", Text = sku_name });
            ary.Add(new SelectListItem { Value = "price", Text = updated_price.ToString() });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }

        private void get_price(string item_code)
        {
            var bgassign = (from bg in db.IV_001_ITEM
                            join bn in db.GB_001_PCODE
                            on new { a1 = bg.sku_sequence, a2 = "10" } equals new { a1 = bn.parameter_code, a2 = bn.parameter_type }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            //join bm in db.AR_001_MTRIX
                            //on new { a1 = bg.item_code, a2 = bg.header_sequence } equals new { a1 = bm.item_code, a2 = bm.header_sequence }
                            //into bm1
                            //from bm2 in bm1.DefaultIfEmpty()
                            where bg.item_code == item_code
                            select new { bg, bn2 }).FirstOrDefault();
            sku_name = bgassign.bn2.parameter_name;

            string pmtrix = bgassign.bg.price_matrix;

            qheader = (queryhead)Session["qheader"];
            string cust_code = qheader.query1;
            //var bgfind = (from bh in db.AR_001_CUSTM
            //              where bh.customer_code == cust_code
            //              select bh).FirstOrDefault();

            //string cust_class = bgfind.price_class;
            //if (pmtrix == "Y")
            //{
            //    if (bgassign.bm2 == null)
            //        f = 0;
            //    else if (cust_class == "2")
            //        f = bgassign.bm2.selling_price_class2;
            //    else if (cust_class == "3")
            //        f = bgassign.bm2.selling_price_class3;
            //    else if (cust_class == "4")
            //        f = bgassign.bm2.selling_price_class4;
            //    else if (cust_class == "5")
            //        f = bgassign.bm2.selling_price_class5;
            //    else if (cust_class == "6")
            //        f = bgassign.bm2.selling_price_class6;
            //    else
            //        f = bgassign.bm2.selling_price_class1;
            //}

            //else
            //{
            //    if (cust_class == "2")
            //        f = bgassign.bg.selling_price_class2;
            //    else if (cust_class == "3")
            //        f = bgassign.bg.selling_price_class3;
            //    else if (cust_class == "4")
            //        f = bgassign.bg.selling_price_class4;
            //    else if (cust_class == "5")
            //        f = bgassign.bg.selling_price_class5;
            //    else if (cust_class == "6")
            //        f = bgassign.bg.selling_price_class6;
            //    else
            //        f = bgassign.bg.selling_price_class1;
            //}
            updated_price = 0;
        }

        private void call_discount()
        {
            //string item_price = glay.vwdclarray0[0].ToString();
            //string str = " delete AP_002_GRN where purchase_sequence_number =" + qheader.intquery0.ToString() + "and dis_flag = 'I'";
            //db.Database.ExecuteSqlCommand(str);

            //recarray = fsp.itemdis_calculation(qheader.intquery0.ToString(),2);
            //line_cr = get_next_line_sequence();
            // createtrans();
            recarray = fsp.totalinv_calculation(qheader.intquery0.ToString(), 2);
            createtrans();
            string strq = "update AP_002_GRN set tax_invoiceamt1 = tax_amount1, tax_invoiceamt2 = tax_amount2, tax_amount1 = 0, tax_amount2 = 0 where purchase_sequence_number =" + qheader.intquery0.ToString() + " and item_code = 'InvoiceTax'";
            db.Database.ExecuteSqlCommand(strq);
            quote_update();

            line_cr = 999;

            strq = "select order_total_discount vwdecimal0, order_total_tax vwdecimal1, order_total_qty vwdecimal2, order_total_ext_price vwdecimal3, order_total_amount vwdecimal4 from AP_002_PURHEAD where purchase_sequence_number=" + qheader.intquery0.ToString();
            var bh = db.Database.SqlQuery<vw_genlay>(strq).FirstOrDefault();
            recarray = new string[1, 20];
            recarray[0, 0] = "";
            recarray[0, 1] = bh.vwdecimal4.ToString();
            recarray[0, 2] = bh.vwdecimal0.ToString();
            recarray[0, 3] = bh.vwdecimal1.ToString();
            recarray[0, 4] = "0";
            recarray[0, 5] = "0";
            recarray[0, 6] = "0";
            recarray[0, 7] = "0";
            recarray[0, 8] = "0";
            recarray[0, 9] = bh.vwdecimal2.ToString();
            recarray[0, 10] = "G";
            recarray[0, 11] = "0";
            recarray[0, 12] = bh.vwdecimal3.ToString();
            recarray[0, 13] = "999";
            recarray[0, 14] = bh.vwdecimal4.ToString();
            if (bh.vwdecimal3 != 0)
                createtrans();

        }
        [HttpPost]
        public ActionResult price_cal(string item_price, string item_qty, string item_code, string disc_amt, string disc_per)
        {
            ModelState.Remove("vwdclarray0[0]");
            ModelState.Remove("vwdclarray0[2]");
            ModelState.Remove("vwdclarray0[3]");

            err_flag = true;
            decimal actual_dis = 0;
            decimal[] decarry = new decimal[20];

            decimal.TryParse(item_qty, out qty);
            decimal.TryParse(item_price, out pricep);
            decimal.TryParse(disc_amt, out disc);
            decimal.TryParse(disc_per, out disc_per1);



            decarry = quote_line(item_code, pricep, qty);

            //dis_validation(disc_amt, disc_per);


            qheader = (queryhead)Session["qheader"];

            decimal tax = decarry[2] + decarry[3] + decarry[4] + decarry[5] + decarry[6];
            net_amt = decarry[0];


            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = (net_amt * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "2", Text = (tax * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "3", Text = (decarry[0] * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "4", Text = (decarry[7] * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "5", Text = (actual_dis * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "6", Text = dis_error });
            ary.Add(new SelectListItem { Value = "7", Text = (qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "8", Text = (pricep * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "9", Text = (decarry[7]).ToString("#,##0.00") });

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }

        private decimal[] quote_line(string itemcode, decimal price, decimal qty)
        {
            decimal[] decarry = new decimal[20];
            decimal ext_price = qty * price;


            string taxinc = (from bg in db.IV_001_ITEM
                             where bg.item_code == itemcode
                             select bg.tax_inclusive).FirstOrDefault();

            if (taxinc == "Y")
                decarry = fsp.get_taxrate(itemcode, price, qty);
            else
            {
                decarry = fsp.quote_tax_rtn(itemcode, ext_price);
                decarry[8] = price;
            }
            return decarry;
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

            decimal exc_rt = qheader.dquery0;

            var bgdtl = (from bh in db.IV_001_ITEM
                         join bf in db.GB_001_PCODE
                         on new { a1 = bh.sku_sequence, a2 = "10" } equals new { a1 = bf.parameter_code, a2 = bf.parameter_type }
                         into bf1
                         from bf2 in bf1.DefaultIfEmpty()
                         where bh.item_code == AP_002_GRN.item_code
                         select new { bh, bf2 }).FirstOrDefault();


            glay.vwint2 = AP_002_GRN.line_sequence;
            glay.vwstring7 = AP_002_GRN.purchase_grn_sequence;
            glay.vwstring0 = AP_002_GRN.item_code;
            glay.vwstring5 = AP_002_GRN.transaction_type;
            glay.vwdclarray0[0] = AP_002_GRN.price;
            glay.vwdecimal4 = (AP_002_GRN.price * exc_rt);
            glay.vwdclarray0[1] = AP_002_GRN.po_quantity;
            glay.vwdclarray0[2] = AP_002_GRN.outstanding_quantity;
            glay.vwdclarray0[3] = AP_002_GRN.grn_quantity;
            glay.vwdecimal2 = (AP_002_GRN.extended_price * exc_rt);
            //glay.vwdclarray0[2] = (AP_002_GRN.discount_amount * exc_rt); ;
            // glay.vwdclarray0[3] = AP_002_GRN.discount_percent;
           // glay.vwdclarray0[4] = (AP_002_GRN.net_amount * exc_rt); ;
            glay.vwdecimal1 = (AP_002_GRN.wht * exc_rt); ;
            glay.vwdecimal3 = AP_002_GRN.base_amount;
            //glay.vwdecimal5 = (AP_002_GRN.actual_discount * exc_rt); ;
            //glay.vwdclarray0[5] = (AP_002_GRN.order_amount * exc_rt); ;
            glay.vwstrarray6[0] = AP_002_GRN.analysis_code1;
            glay.vwstrarray6[1] = AP_002_GRN.analysis_code2;
            glay.vwstrarray6[2] = AP_002_GRN.analysis_code3;
            glay.vwstrarray6[3] = AP_002_GRN.analysis_code4;
            glay.vwstrarray6[4] = AP_002_GRN.analysis_code5;
            glay.vwstrarray6[5] = AP_002_GRN.analysis_code6;
            glay.vwstrarray6[6] = AP_002_GRN.analysis_code7;
            glay.vwstrarray6[7] = AP_002_GRN.analysis_code8;
            glay.vwstrarray6[8] = AP_002_GRN.analysis_code9;
            glay.vwstrarray6[9] = AP_002_GRN.analysis_code10;
            //glay.vwdclarray1[0] = AP_002_GRN.tax_amount1;
            //glay.vwdclarray1[1] = AP_002_GRN.tax_amount2;
            //glay.vwdclarray1[2] = AP_002_GRN.tax_amount3;
            //glay.vwdclarray1[3] = AP_002_GRN.tax_amount4;
            //glay.vwdclarray1[4] = AP_002_GRN.tax_amount5;
            //glay.vwstring5 = AP_002_GRN.discount_amount != 0 ? "Y" : AP_002_GRN.discount_percent != 0 ? "Y" : "N";
            psess.temp2 = bgdtl.bf2.parameter_name;
            Session["psess"] = psess;
            
        }

        private void header_ana(string commandn = "")
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
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

        private void quote_update()
        {
            string sqlstr = " update AP_002_PURHEAD set order_total_amount = a001, order_total_tax = a002, order_total_discount = a003, order_total_extended_price = a004, order_total_qty = a005 from AP_002_PURHEAD ax, ";
            sqlstr += " (select sum(order_amount) a001, sum(tax_amount) a002, sum (actual_discount) a003, sum(extended_price) a004, sum(order_quantity) as a005 from AP_002_GRN where  purchase_grn_sequence=" + util.sqlquote(qheader.query3);
            sqlstr += " ) bxz where ax.order_reference=" + util.sqlquote(qheader.query3);
            db.Database.ExecuteSqlCommand(sqlstr);


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

        private void set_qheader()
        {
            qheader.query0 = gdoc.purchase_transaction_type;
            qheader.intquery0 = gdoc.purchase_sequence_number;
            qheader.query1 = gdoc.vendor_code;
            qheader.dquery0 = gdoc.exchange_rate;
            qheader.query2 = gdoc.transaction_date;
            qheader.query3 = gdoc.order_reference;
            qheader.bquery0 = true;
            var bgassign = (from bg in db.AP_001_PTRAN
                            where bg.purchase_order_code == gdoc.purchase_transaction_type && bg.sequence_no == 0
                            select bg).FirstOrDefault();

            qheader.query4 = "Y";
            if (string.IsNullOrWhiteSpace(bgassign.tax_invoice1) && string.IsNullOrWhiteSpace(bgassign.tax_invoice2))
                qheader.query4 = "N";

            qheader.query5 = "Y";
            if (pubsess.base_currency_code == gdoc.currency_code)
                qheader.query5 = "N";

            if (pubsess.exchange_editable != "Y")
            {
                string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(gdoc.currency_code) + " and '" + gdoc.transaction_date + "' between date_from and date_to";
                var exch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                if (exch != null)
                    qheader.dquery0 = exch.dquery0;
            }

            string read_cur = (from bg in db.MC_001_CUREN
                               where bg.currency_code == gdoc.currency_code
                               select bg.currency_description).FirstOrDefault();

            qheader.query6 = read_cur;
            qheader.dquery1 = qheader.dquery0;
            if (pubsess.exchange_rate_mode == "F")
            {
                qheader.dquery1 = 1 / qheader.dquery0;
            }
            Session["qheader"] = qheader;

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

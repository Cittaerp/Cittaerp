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

    public class PcontractController : Controller
    {
        //
        // GET: /Employee/

        IV_002_PCNT IV_002_PCNT = new IV_002_PCNT();
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
        string[,] recarray;
        string action_flag = "";
        
        int line_cr = 0;
        [EncryptionActionAttribute]
        public ActionResult Index()
        {

            util.init_values();

            psess = (psess)Session["psess"];
            //  Session["curren_name"] = "";
            psess.temp2 = "";
            Session["shwdis"] = "";
            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.IV_002_PCNT
                         join bg1 in db.AR_001_CUSTM
                        on new { a2 = bh.customer_id } equals new { a2 = bg1.customer_code }
                         join bn in db.GB_999_MSG
                         on new { b3 = bh.contract_type, b1 = "CTYPE" } equals new { b3 = bn.code_msg, b1 = bn.type_msg }
                         where bh.approval_level == 0
                         select new vw_genlay
                         {
                             vwstring6 = bh.contract_id,
                             vwstring0 = bh.property_id,
                             vwstring1 = bn.name1_msg,
                             vwdecimal0 = bh.dep_amt_paid,
                             vwstring2 = bh.transaction_date,
                             vwstring3 = bh.sales_rep,
                             vwstring4 = bg1.cust_biz_name,
                             vwstring5 = bh.product
                         };

            return View(bglist.ToList());


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
            header_ana();
            select_query_head();
            //display_header();

            return View(glay);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetails(vw_genlay glay_in, string hid_price, HttpPostedFileBase[] photofile, string headtype = "D")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            
            glay = glay_in;
            photo1 = photofile;

            if (headtype == "send_app")
            {
                //validation_routine(headtype);
                if (err_flag)
                {
                    update_file(headtype);
                    get_periodyear();
                    string str = "update IV_002_PCNT set approval_level=99 where contract_id=" + util.sqlquote(glay.vwstring6) + " and property_id=" + glay.vwstring0;
                    db.Database.ExecuteSqlCommand(str);
                    return RedirectToAction("CreateDetails");
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

            select_query_head();
            header_ana();

            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditDetails(string key1, string key2)
        {
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];

            IV_002_PCNT = db.IV_002_PCNT.Find(key1, key2);
            if (IV_002_PCNT == null)
                return View(glay);

            initial_rtn();
            move_detail();
            header_ana();
            select_query_head();

            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails(vw_genlay glay_in, HttpPostedFileBase[] photofile, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            
            glay = glay_in;

            if (headtype == "send_app")
            {
                //validation_routine(headtype);
                if (err_flag)
                {
                    get_periodyear();
                    string str = "update IV_002_PCNT set approval_level=99 where contract_id=" + util.sqlquote(glay.vwstring6) + " and property_id=" + glay.vwstring0;
                    db.Database.ExecuteSqlCommand(str);
                    return RedirectToAction("CreateDetails");

                }

            }

            photo1 = photofile;

            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head();

            header_ana();

            return View(glay);

        }
        private void delete_record()
        {
            IV_002_PCNT = db.IV_002_PCNT.Find(glay.vwstring0, glay.vwstring6);
            if (IV_002_PCNT != null)
            {
                db.IV_002_PCNT.Remove(IV_002_PCNT);
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


            if (cr_flag == "CreateDetails")
            {
                IV_002_PCNT = new IV_002_PCNT();
                IV_002_PCNT.created_by = pubsess.userid;
                IV_002_PCNT.created_date = DateTime.UtcNow;
                IV_002_PCNT.approval_level = 0;
                IV_002_PCNT.approval_date = DateTime.UtcNow;
                IV_002_PCNT.approval_by = pubsess.userid;
                //string sqlstr = "select isnull(max(sequence_num),0) vwint1 from IV_002_PCNT where contract_id=" + util.sqlquote(glay.vwstring0);
                //var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
                //glay.vwint4 = sql1.vwint1 + 1;
            }
            else
            {
                IV_002_PCNT = db.IV_002_PCNT.Find(glay.vwstring0, glay.vwstring6);
            }

            //the detailsub button has been clicked


            IV_002_PCNT.contract_id = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            IV_002_PCNT.property_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            IV_002_PCNT.contract_description = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            //IV_002_PCNT.sequence_num = glay.vwint4;
            IV_002_PCNT.contract_type = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            IV_002_PCNT.transaction_date = util.date_yyyymmdd(glay.vwstring2);
            IV_002_PCNT.sales_rep = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            IV_002_PCNT.customer_id = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            IV_002_PCNT.effect_period = util.date_yyyymmdd(glay.vwstring5);
            IV_002_PCNT.effect_year = ""; //string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            IV_002_PCNT.dep_amt_paid = glay.vwdecimal0;
            IV_002_PCNT.price = glay.vwdecimal6;
            IV_002_PCNT.quantity = glay.vwdclarray0[0];
            IV_002_PCNT.sales_com = glay.vwdecimal1;
            IV_002_PCNT.sales_val = glay.vwdecimal2;
            IV_002_PCNT.num_payment = glay.vwdecimal5;
            IV_002_PCNT.monthly_amt = glay.vwdecimal4;
            IV_002_PCNT.note = "";

            IV_002_PCNT.modified_date = DateTime.UtcNow;
            IV_002_PCNT.modified_by = pubsess.userid;
            glay.vwint2 = glay.vwint0;
            //string period = glay.vwint2 <10 ? "0" +glay.vwint2.ToString() : glay.vwint2.ToString(); 


            if (cr_flag == "CreateDetails")
                db.Entry(IV_002_PCNT).State = EntityState.Added;
            else
                db.Entry(IV_002_PCNT).State = EntityState.Modified;


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



            if (err_flag)
            {
                {
                    util.write_document("CONTRACT", IV_002_PCNT.contract_id, photo1, glay.vwstrarray9);
                }
                //string str = " update AR_002_SALES set exchange_rate =" + qheader.dquery0 +" where sale_sequence_number =" + qheader.intquery0;
                // db.Database.ExecuteSqlCommand(str);
                //call_discount();
                //qheader.bquery0 = false;
                //Session["qheader"] = qheader;
            }


        }


        //private int get_next_line_sequence()
        //{
        //    string sqlstr = "select isnull(max(line_sequence),0) vwint1 from IV_002_PCNT where line_sequence <> 999 and quote_reference=" + util.sqlquote(qheader.query3);
        //    var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
        //    return sql1.vwint1 + 1;

        //}
        private void get_periodyear()
        {
            int mprd = 0; int myr = 0; int i = 0;
            int conyear = Convert.ToInt32(glay.vwstring2.Substring(6)); int intdiv = 0; int x = 0;
            int prd = Convert.ToInt32(glay.vwstring5.Substring(3,2));
            decimal tot = prd + glay.vwdecimal5;
            for (int mth = prd; mth < tot; mth++)
            {
                x = mth;
                intdiv = x / 12;
                //do{
                x = x % 12;
                //}
                //while (x >= 12);

                if (x == 0)
                {
                    mprd = 12;
                }
                else
                {
                    mprd = x;
                    myr = intdiv + conyear;
                }

                create_transaction(mprd, myr, i);
                i++;

            }

        }

        private void create_transaction(int mprd, int myr, int i)
        {
            string ref_num = glay.vwstring0 + "/" + glay.vwstring2.Substring(6) + "/" + glay.vwstring5;
            string curen = (from bg in db.AR_001_CUSTM
                            where bg.customer_code == glay.vwstring4
                            select bg.currency_code).FirstOrDefault();
            string accode = (from bg in db.IV_001_ITEM
                             where bg.item_code == glay.vwstring0
                             select bg.gl_property).FirstOrDefault();
            //for (int i = 0; i < glay.vwdecimal5; i++)
            //{
            AP_002_VTRAN AP_002_VTRAN = new AP_002_VTRAN();
            AP_002_VTRAN.transaction_type = "";
            AP_002_VTRAN.total_amount = 0;
            AP_002_VTRAN.approval_by = "";
            AP_002_VTRAN.created_date = DateTime.UtcNow;
            AP_002_VTRAN.created_by = pubsess.userid;
            AP_002_VTRAN.processed = 0;
            AP_002_VTRAN.approval_date = DateTime.UtcNow;

            AP_002_VTRAN.transaction_code = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : (glay.vwstring4);
            AP_002_VTRAN.reference_number = ref_num;
            AP_002_VTRAN.module_account_type = "001";
            AP_002_VTRAN.period = mprd < 10 ? "0" + mprd.ToString() : mprd.ToString();
            AP_002_VTRAN.transaction_date = myr.ToString();
            AP_002_VTRAN.note = "";
            AP_002_VTRAN.batch_information = "Property";
            AP_002_VTRAN.modified_by = pubsess.userid;
            AP_002_VTRAN.modified_date = DateTime.UtcNow;
            glay.vwint2 = glay.vwint2 + 1;
            db1.Entry(AP_002_VTRAN).State = EntityState.Added;
            try
            {
                db1.SaveChanges();
            }

            catch (Exception err)
            {
                if (err.InnerException == null)
                    ModelState.AddModelError(String.Empty, err.Message);
                else
                    ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);

                err_flag = false;
            }
            if (err_flag)
            {
                int doc = AP_002_VTRAN.document_number;
                AP_002_VTRAD AP_002_VTRAD = new AP_002_VTRAD();
                AP_002_VTRAD.created_by = pubsess.userid;
                AP_002_VTRAD.created_date = DateTime.UtcNow;
                //string sqlstr = "select isnull(max(sequence_number),0) vwint1 from AP_002_VTRAD where document_number=" + doc;
                //var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
                //glay.vwint4 = sql1.vwint1 + 1;

                AP_002_VTRAD.document_number = doc;
                AP_002_VTRAD.sequence_number = 1;
                AP_002_VTRAD.module_account_type = "001";
                AP_002_VTRAD.transaction_code = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : (glay.vwstring4);
                AP_002_VTRAD.reference_number = ref_num;
                AP_002_VTRAD.description = "Property";
                AP_002_VTRAD.amount = i == 0 ? glay.vwdecimal0 : glay.vwdecimal4;
                AP_002_VTRAD.transaction_type = "D";
                AP_002_VTRAD.account_type = "003";
                AP_002_VTRAD.account_code = accode;
                AP_002_VTRAD.transaction_date = util.date_yyyymmdd(glay.vwstring2);
                AP_002_VTRAD.exchange_rate = 1;
                AP_002_VTRAD.currency = curen;
                AP_002_VTRAD.base_amount = glay.vwdecimal0 * 1;

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

                //string trans_type = "TTY" + ptype;
                //string amt_type = (from bf in db.GB_999_MSG
                //                   where bf.type_msg == trans_type && bf.code_msg == AP_002_VTRAD.transaction_type
                //                   select bf.name2_msg).FirstOrDefault();
                AP_002_VTRAD.amount_type = "C";

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

                db1.Entry(AP_002_VTRAD).State = EntityState.Added;
                try
                {
                    db1.SaveChanges();
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

                string str1 = "insert into IV_002_PCNT(sale_sequence_number,line_sequence,sub_line_sequence,quote_reference,item_code,quote_qty,price,ext_price,discount_percent,discount_amount,";
                str1 += "actual_discount,net_amount,tax_amount,quote_amount,tax_amount1,tax_amount2,tax_amount3,tax_amount4,tax_amount5,tax_invoiceamt1,tax_invoiceamt2,";
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
                    ModelState.AddModelError(String.Empty, "Please insert a valid quote date");
                    err_flag = false;
                }

                if (!util.date_validate(glay.vwstrarray0[3]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
                    err_flag = false;
                }

                if (glay.vwdecimal0 == 0)
                {
                    ModelState.AddModelError(String.Empty, "Exchange Rate can not be Zero");
                    err_flag = false;
                }
            }


            if (cr_flag.IndexOf("Details") > 0)
            {
                if (Session["action_flag"].ToString() == "CreateDetails")
                {
                    IV_002_PCNT bnk = db.IV_002_PCNT.Find(glay.vwstring0, glay.vwstring6);
                    if (bnk != null)
                    {
                        ModelState.AddModelError(String.Empty, "Contract ID already exist");
                        err_flag = false;
                    }
                }

                if (!util.date_validate(glay.vwstring2))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
                    err_flag = false;
                }


                if (glay.vwdclarray0[0] <= 0)
                {
                    ModelState.AddModelError(String.Empty, "Quantity must be greater than Zero");
                    err_flag = false;
                }
                //if (glay.vwdclarray0[0] == 0)
                //{
                //    ModelState.AddModelError(String.Empty, "Price can not be Zero");
                //    err_flag = false;
                //}
                //if (qheader.dquery0 == 0)
                //{
                //    ModelState.AddModelError(String.Empty, "Exchange Rate can not be Zero");
                //    err_flag = false;
                //}
                //if (glay.vwstring1 == "Y")
                //{
                //    if (glay.vwdclarray0[2] != 0 && glay.vwdclarray0[3] != 0)
                //    {
                //        ModelState.AddModelError(String.Empty, "You can either enter Discount Amount or Discount Percent");
                //        err_flag = false;
                //    }
                //}
                //if (glay.vwdclarray0[1] == 0)
                //{
                //    ModelState.AddModelError(String.Empty, "Enter a valid Quantity Value");
                //    err_flag = false;
                //}


                //var headlist = (from bh in db.AP_001_PUROT
                //                where bh.parameter_code == "SLSEQ" && (bh.order_type == "01" || bh.order_type == "single")
                //                select bh).FirstOrDefault();
                //if (headlist == null)
                //{
                //    ModelState.AddModelError(String.Empty, "Quote sequencing number not created");
                //    err_flag = false;
                //}



                //for (int count1 = 0; count1 < 10; count1++)
                //{
                //    if (aheader7[count1] == "Y" && string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
                //    {
                //        error_msg = aheader5[count1] + " is mandatory. ";
                //        ModelState.AddModelError(String.Empty, error_msg);
                //        err_flag = false;
                //    }
                //}


            }

        }

        private void select_query_head()
        {

            var empe = from pf in db.IV_001_ITEM
                       where pf.active_status == "N" && pf.sales == "Y" && pf.item_type == "P"
                       orderby pf.item_code, pf.item_name
                       select new { c1 = pf.item_code, c2 = pf.item_code + "--- " + pf.item_name };
            ViewBag.cid = new SelectList(empe.ToList(), "c1", "c2", glay.vwstring0);

            var type = from bg in db.GB_999_MSG
                       where bg.type_msg == "CTYPE"
                       orderby bg.name1_msg
                       select bg;
            ViewBag.ctype = new SelectList(type.ToList(), "code_msg", "name1_msg", glay.vwstring1);

            ViewBag.srep = util.para_selectquery("62", glay.vwstring3, "N");
            ViewBag.cust = util.para_selectquery("001", glay.vwstring4, "N");

            year_cal();
            // string disckf = pubsess.valid_datefrm.Substring(4, 2);
            // string disckt = pubsess.valid_dateto.Substring(4, 2);
            //string curent_mth = pubsess.curent_datefrm.Substring(4, 2);


            //string srt = "select distinct query0, query1 from (";
            //srt += " select code_msg query0, name1_msg query1 from  dbo.GB_999_MSG pf where pf.type_msg = 'FYM'and  code_msg =" + util.sqlquote(curent_mth) + "union all";
            //srt += " select code_msg query0, name1_msg query1  from dbo.GB_999_MSG pg where pg.type_msg = 'FYM' and code_msg between" + util.sqlquote(disckf) + " and" + util.sqlquote(disckt) + ") b order by 2";
            //var bf2 = db.Database.SqlQuery<querylay>(srt).ToList();
            //ViewBag.month = new SelectList(bf2.ToList(), "query0", "query1", glay.vwstring5);

            var empu = from pf in db.GB_999_MSG
                       where pf.type_msg == "FYM"
                       //orderby pf.name1_msg
                       select pf;
            ViewBag.month = new SelectList(empu.ToList(), "code_msg", "name1_msg", glay.vwstring5);

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
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray7 = new string[20];
            glay.vwstring2 = DateTime.Now.ToString("dd/MM/yyyy");
            //glay.vwdclarray0[0] = 1;
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
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[IV_002_PCNT] where property_id  +'[]'+ contract_id =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        public ActionResult delete_detail(string id)
        {
            // write your query statement
            qheader = (queryhead)Session["qheader"];
            string sqlstr = "delete from [dbo].[IV_002_PCNT] where cast(sale_sequence_number as varchar) +'[]'+ cast(line_sequence as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "select count(0) intquery0 from IV_002_PCNT where dis_flag='N' and sale_sequence_number=" + qheader.intquery0.ToString();
            var delctr = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (delctr.intquery0 == 0)
            {
                sqlstr = "delete from IV_002_PCNT where sale_sequence_number=" + qheader.intquery0.ToString();
                db.Database.ExecuteSqlCommand(sqlstr);
                sqlstr = "update AR_002_SALES set quote_total_qty=0,quote_total_ext_price=0,quote_total_discount=0, quote_total_inv_tax=0,quote_total_tax=0,quote_total_amount=0 ";
                sqlstr += " where sale_sequence_number=" + qheader.intquery0.ToString();
                db.Database.ExecuteSqlCommand(sqlstr);

            }
            else
                call_discount();

            display_header();
            select_queryhide("D");
            if (HttpContext.Request.IsAjaxRequest())
                return Json(
                            JsonRequestBehavior.AllowGet);
            return RedirectToAction("CreateDetails");

        }

        private void read_details()
        {
            int hbug = qheader.intquery0;
            decimal exc_rt = qheader.dquery1;

            var bglist = from bh in db.IV_002_PCNT
                         where bh.contract_id == ""
                         select new vw_genlay
                         {
                             //vwint0 = bh.sale_sequence_number,
                             //vwint1 = bh.line_sequence,
                             //vwint2 = bh.sub_line_sequence,
                             //vwstring1 = bh.quote_reference,
                             //vwstring2 = bg2.item_name,
                             //vwstring0 = bh.item_code,
                             //vwstring3 = bf2.parameter_name,
                             //vwdecimal0 = bh.price * exc_rt,
                             //vwdecimal1 = bh.quote_qty,
                             //vwdecimal2 = bh.ext_price * exc_rt,
                             //vwdecimal3 = bh.tax_amount * exc_rt,
                             //vwdecimal4 = bh.actual_discount * exc_rt,
                             //vwdecimal5 = bh.quote_amount*exc_rt,
                             //vwstring4 = bh.dis_flag
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

            var bg2list = (from bh in db.AR_002_SALES
                           join bh1 in db.AR_001_CUSTM
                          on new { a1 = bh.customer_code } equals new { a1 = bh1.customer_code }
                           where bh.sale_sequence_number == hbug
                           select new { bh, bh1 }).FirstOrDefault();

            if (bg2list != null)
            {
                glay.vwstrarray0[0] = bg2list.bh.sales_transaction_type;
                glay.vwstrarray0[1] = bg2list.bh1.customer_code;
                glay.vwstrarray0[2] = bg2list.bh.currency_code;
                glay.vwdecimal0 = bg2list.bh.exchange_rate;
                glay.vwstrarray0[3] = util.date_slash(bg2list.bh.transaction_date);
                glay.vwstrarray0[4] = util.date_slash(bg2list.bh.quote_date);
                glay.vwint1 = bg2list.bh.number_of_day;
                // glay.vwstrarray0[3] = bg2list.bh.quote_expiration_date.ToString("dd/MM/yyyy");
                glay.vwstrarray0[6] = qheader.query6;
                glay.vwstrarray0[5] = bg2list.bh.note;
                //string docode = gdoc.sale_sequence_number.ToString();
                //var bglist = from bg in db.GB_001_DOC
                //             where bg.screen_code == "SALES" && bg.document_code == docode
                //             orderby bg.document_sequence
                //             select bg;

                //ViewBag.anapict = bglist.ToList();

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
            DateTime expdate = DateTime.Now;
            int qdate = qheader.intquery0;
            var chkdate = (from bg in db.AR_002_SALES
                           where bg.sale_sequence_number == qdate
                           select new
                           {
                               c0 = bg.quote_date,
                               c1 = bg.number_of_day
                           }).FirstOrDefault();
            int numday = chkdate.c1;

            string dh1 = chkdate.c0;
            DateTime quotedate = new DateTime(Convert.ToInt16(dh1.Substring(0, 4)), Convert.ToInt16(dh1.Substring(4, 2)), Convert.ToInt16(dh1.Substring(6, 2)));
            expdate = quotedate.AddDays(numday);

            decimal exc_rt = qheader.dquery1;

            int hbug = qheader.intquery0;
            var bg2list = (from bh in db.AR_002_SALES
                           join bh1 in db.AR_001_CUSTM
                           on new { a1 = bh.customer_code } equals new { a1 = bh1.customer_code }
                           join bj in db.AR_001_STRAN
                           on new { a1 = bh.sales_transaction_type } equals new { a1 = bj.order_code, }
                           into bj1
                           from bj2 in bj1.DefaultIfEmpty()
                           where bh.sale_sequence_number == hbug
                           select new { bh, bh1, bj2 }).FirstOrDefault();


            vw_genlay glayhead = new vw_genlay();
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwstrarray1[0] = bg2list.bh1.cust_biz_name;
                glayhead.vwstrarray1[2] = expdate.ToString("dd/MM/yyyy");
                glayhead.vwstrarray1[1] = qheader.query3;
                glayhead.vwstrarray1[3] = (bg2list.bh.quote_total_ext_price * exc_rt).ToString();
                glayhead.vwstrarray1[7] = util.date_slash(bg2list.bh.quote_date);
                glayhead.vwstrarray1[8] = bg2list.bj2.order_name;
                glayhead.vwstrarray1[4] = (bg2list.bh.quote_total_discount * exc_rt).ToString();
                glayhead.vwstrarray1[5] = (bg2list.bh.quote_total_tax * exc_rt).ToString();
                glayhead.vwstrarray1[6] = (bg2list.bh.quote_total_amount * exc_rt).ToString();
                glayhead.vwstrarray1[9] = util.date_slash(qheader.query2);
                glayhead.vwstrarray1[10] = qheader.dquery0.ToString();
                glayhead.vwstrarray1[11] = qheader.query6;
                psess.temp3 = bg2list.bh.customer_code;


            }
            ViewBag.x2 = glayhead;
        }

        [HttpPost]
        public ActionResult get_commision_value(string item_code, string dep_amt_paid, string price, string qty, string tdate)
        {
            // write your query statement
            psess = (psess)Session["psess"];
            ModelState.Remove("glay.vwdecimal1");
            tdate = util.date_yyyymmdd(tdate);
            decimal s_val = 0; decimal n_pyt = 0; decimal dep_amt = Convert.ToDecimal(dep_amt_paid);
            decimal s_comm = 0; decimal d_amt = 0; decimal m_amt = 0; decimal dprice = 0; decimal dqty = 1;
            decimal.TryParse(price, out dprice);
            decimal.TryParse(qty, out dqty);
            //if (dprice == 0)
            //    dprice = 1;
            if (dqty == 0)
                dqty = 1;
            var curlist = (from bg in db.IV_001_ITEM
                           where bg.item_code == item_code
                           select new
                           {
                               c1 = bg.agency_comm_flat,
                               c0 = bg.agency_comm_per,
                               c2 = bg.selling_price_class1,
                               c3 = bg.deposit_flat,
                               c4 = bg.deposit_percent,
                               c5 = bg.num_installment
                           }).FirstOrDefault();
            if (curlist != null)
            {
                if (curlist.c1 != 0)
                    s_comm = curlist.c1 * dqty;
                else
                    s_comm = (curlist.c0 / 100 * curlist.c2) * dqty;
                s_val = curlist.c2;
                if (curlist.c3 != 0)
                    d_amt = curlist.c3;
                else
                    d_amt = curlist.c4 / 100 * curlist.c2;
                if (curlist.c5 != 0)
                    m_amt = (s_val - dep_amt) / curlist.c5;
                else
                    m_amt = 0;
                n_pyt = curlist.c5;
            }
            decimal n_svl = dprice * dqty;

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = s_comm.ToString() });
            ary.Add(new SelectListItem { Value = "2", Text = s_val.ToString() });
            ary.Add(new SelectListItem { Value = "3", Text = d_amt.ToString() });
            ary.Add(new SelectListItem { Value = "4", Text = m_amt.ToString() });
            ary.Add(new SelectListItem { Value = "5", Text = n_pyt.ToString() });
            ary.Add(new SelectListItem { Value = "6", Text = n_svl.ToString() });

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
                            on new { a1 = bg.sku_sequence, a2 = "10" } equals new { a1 = bn.parameter_code, a2 = bn.parameter_type }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            join bm in db.AR_001_MTRIX
                            on new { a1 = bg.item_code, a2 = bg.header_sequence } equals new { a1 = bm.item_code, a2 = bm.header_sequence }
                            into bm1
                            from bm2 in bm1.DefaultIfEmpty()
                            where bg.item_code == item_code
                            select new { bg, bn2, bm2 }).FirstOrDefault();

            string sku_name = bgassign.bn2.parameter_name;
            //string price_dis = bgassign.bg.item_type;
            string pmtrix = bgassign.bg.price_matrix;

            qheader = (queryhead)Session["qheader"];
            string cust_code = qheader.query1;
            var bgfind = (from bh in db.AR_001_CUSTM
                          where bh.customer_code == cust_code
                          select bh).FirstOrDefault();

            string cust_class = bgfind.price_class;
            if (pmtrix == "Y")
            {
                if (bgassign.bm2 == null)
                    f = 0;
                else if (cust_class == "2")
                    f = bgassign.bm2.selling_price_class2;
                else if (cust_class == "3")
                    f = bgassign.bm2.selling_price_class3;
                else if (cust_class == "4")
                    f = bgassign.bm2.selling_price_class4;
                else if (cust_class == "5")
                    f = bgassign.bm2.selling_price_class5;
                else if (cust_class == "6")
                    f = bgassign.bm2.selling_price_class6;
                else
                    f = bgassign.bm2.selling_price_class1;
            }

            else
            {
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
            }

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "sku", Text = sku_name });
            ary.Add(new SelectListItem { Value = "price", Text = f.ToString() });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }

        private void call_discount()
        {
            //string item_price = glay.vwdclarray0[0].ToString();
            //string str = " delete IV_002_PCNT where sale_sequence_number =" + qheader.intquery0.ToString() + "and dis_flag = 'I'";
            //db.Database.ExecuteSqlCommand(str);

            recarray = fsp.itemdis_calculation(qheader.intquery0.ToString());
            //line_cr = get_next_line_sequence();
            createtrans();
            recarray = fsp.totalinv_calculation(qheader.intquery0.ToString());
            createtrans();
            string strq = "update IV_002_PCNT set tax_invoiceamt1 = tax_amount1, tax_invoiceamt2 = tax_amount2, tax_amount1 = 0, tax_amount2 = 0 where sale_sequence_number =" + qheader.intquery0.ToString() + " and item_code = 'InvoiceTax'";
            db.Database.ExecuteSqlCommand(strq);
            quote_update();

            line_cr = 999;

            strq = "select quote_total_discount vwdecimal0, quote_total_tax vwdecimal1, quote_total_qty vwdecimal2, quote_total_ext_price vwdecimal3, quote_total_amount vwdecimal4 from AR_002_SALES where sale_sequence_number=" + qheader.intquery0.ToString();
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
            psess = (psess)Session["psess"];
            ModelState.Remove("vwdclarray0[0]");
            ModelState.Remove("vwdclarray0[2]");
            ModelState.Remove("vwdclarray0[3]");

            err_flag = true;
            decimal actual_dis = 0;
            decimal[] decarry = new decimal[20];

            //decimal.TryParse(item_qty, out qty);
            //decimal.TryParse(item_price, out pricep);
            //decimal.TryParse(disc_amt, out disc);
            //decimal.TryParse(disc_per, out disc_per1);



            //decarry = quote_line(item_code, pricep, qty);

            //dis_validation(disc_amt, disc_per);


            qheader = (queryhead)Session["qheader"];

            decimal tax = decarry[2] + decarry[3] + decarry[4] + decarry[5] + decarry[6];
            //net_amt = decarry[0];


            List<SelectListItem> ary = new List<SelectListItem>();
            //ary.Add(new SelectListItem { Value = "1", Text = (net_amt*qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "2", Text = (tax * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "3", Text = (decarry[0] * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "4", Text = (decarry[7] * qheader.dquery1).ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "5", Text = (actual_dis * qheader.dquery1).ToString("#,##0.00") });
            //ary.Add(new SelectListItem { Value = "6", Text = dis_error });
            ary.Add(new SelectListItem { Value = "7", Text = (qheader.dquery1).ToString("#,##0.00") });
            //ary.Add(new SelectListItem { Value = "8", Text = (pricep * qheader.dquery1).ToString("#,##0.00") });
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

            //decimal exc_rt = qheader.dquery0; 

            //var bgdtl = (from bh in db.IV_001_ITEM
            //             join bf in db.GB_001_PCODE
            //             on new { a1 = bh.sku_sequence,a2="10" } equals new { a1 = bf.parameter_code, a2=bf.parameter_type }
            //             into bf1
            //             from bf2 in bf1.DefaultIfEmpty()
            //             where bh.item_code == IV_002_PCNT.contract_id
            //             select new { bh, bf2 }).FirstOrDefault();


            glay.vwstring0 = IV_002_PCNT.property_id;
            glay.vwstring1 = IV_002_PCNT.contract_type;
            glay.vwstring2 = util.date_slash(IV_002_PCNT.transaction_date);
            glay.vwstring3 = IV_002_PCNT.sales_rep;
            glay.vwstring4 = IV_002_PCNT.customer_id;
            // glay.vwstring5 = IV_002_PCNT.product;
            glay.vwstring5 = util.date_slash(IV_002_PCNT.effect_period);
            //glay.vwstring6 = IV_002_PCNT.effect_year;
            glay.vwdecimal0 = IV_002_PCNT.dep_amt_paid;
            glay.vwdecimal6 = IV_002_PCNT.price;
            glay.vwdclarray0[0] = IV_002_PCNT.quantity;
            glay.vwdecimal1 = IV_002_PCNT.sales_com;
            glay.vwdecimal2 = IV_002_PCNT.sales_val;
            glay.vwdecimal5 = IV_002_PCNT.num_payment;
            glay.vwdecimal4 = IV_002_PCNT.monthly_amt;
            glay.vwstring6 = IV_002_PCNT.contract_id;
            glay.vwstring7 = IV_002_PCNT.contract_description;
            //glay.vwdecimal3 = IV_002_PCNT.base_amount;
            //glay.vwdecimal5 = (IV_002_PCNT.actual_discount * exc_rt); ;
            //psess.temp2 = bgdtl.bf2.parameter_name;

        }

        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            SelectList[] head_det = new SelectList[20];

           // Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;

            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "016" && bg.sequence_no != 99
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

        private void quote_update()
        {
            string sqlstr = " update AR_002_SALES set quote_total_amount = a001, quote_total_tax = a002, quote_total_discount = a003, quote_total_ext_price = a004, quote_total_qty = a005 from AR_002_SALES ax, ";
            sqlstr += " (select sum(quote_amount) a001, sum(tax_amount) a002, sum (actual_discount) a003, sum(ext_price) a004, sum(quote_qty) as a005 from IV_002_PCNT where  quote_reference=" + util.sqlquote(qheader.query3);
            sqlstr += " ) bxz where ax.quote_reference=" + util.sqlquote(qheader.query3);
            //sqlstr += " where quote_reference=" + util.sqlquote(preseq);
            db.Database.ExecuteSqlCommand(sqlstr);

            //string sqlstr = " update AR_002_SALES set quote_total_amount = a001, quote_total_tax = a002, quote_total_discount = a003, quote_total_ext_price = a004, quote_total_qty = a005" ;
            //sqlstr += " select souy - at3 as a001 from";
            //sqlstr += " (select sum(quote_amount) souy, sum(tax_amount) a002, sum (discount_amount) a003,";
            //sqlstr += " sum(ext_price) a004, sum(quote_qty)a005 from IV_002_PCNT) ax,";
            //sqlstr += " (select at1+at2 as at3 from";
            //sqlstr += " (select distinct tax_invoiceamt1 at1, tax_invoiceamt2 at2 from IV_002_PCNT where  quote_reference =" + util.sqlquote(glay.vwstring7) + ") b)bx";

            //sqlstr += " from AR_002_SALES a, (select sum(quote_amount) a001, sum(tax_amount) a002, sum (discount_amount) a003, sum(ext_price) a004, sum(quote_qty)a005 from IV_002_PCNT where quote_reference=" + util.sqlquote(glay.vwstring7) + ") bx";

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

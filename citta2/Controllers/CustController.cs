using CittaErp.Models;
using anchor1.Filters;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class CustController : Controller
    {
        AR_001_CUSTM AR_001_CUSTM = new AR_001_CUSTM();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string atype = "cus";
        bool err_flag = true;
        string delmsg = "";
        string action_flag = ""; 
        string move_auto = "N";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            psess.temp7 = "";
            //Session["multi_curenchk"] = pubsess.multi_currency;
            Session["psess"] = psess;
            
            var bglist = from bh in db.AR_001_CUSTM
                         join bk in db.GB_999_MSG
                         on new { a1 = bh.business_type, a2 = "BIZT" } equals new { a1 = bk.code_msg, a2 = bk.type_msg }
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.customer_code,
                             vwstring1 = bh.cust_biz_name,
                             vwstring2 = bk2.name1_msg,
                             vwstring3 = bh.biz_reg_number,
                             vwstring4 = bh.gl_account_code,
                             vwstring5 = bh.active_status == "N" ? "Active" : "Inactive"
                         };

            return View(bglist.ToList());


        }

        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            header_ana();
            select_query();
            priceclass();
            cal_auto();
            psess.temp5 = move_auto;
            Session["psess"] = psess;
            if (move_auto == "Y")
                glay.vwstring0 = "AUTO";
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            cal_auto();
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            // initial_rtn();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            psess.temp5 = move_auto;
            Session["psess"] = psess;
            initial_rtn();
             
            AR_001_CUSTM = db.AR_001_CUSTM.Find(key1);
            if (AR_001_CUSTM != null)
                read_record();
            header_ana();
            select_query();
            priceclass();
            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt == "D")
            {
                delete_record();
                if (err_flag == false)
                {
                    initial_rtn();
                    header_ana();
                    select_query();
                    return View(glay);
                }
                return RedirectToAction("Index");
            }

            photo1 = photofile;
            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        private void delete_record()
        {
            if (util.delete_check("CUSTM", glay.vwstring0))
            {
                AR_001_CUSTM = db.AR_001_CUSTM.Find(glay.vwstring0);
                db.AR_001_CUSTM.Remove(AR_001_CUSTM);
                db.SaveChanges();
            }
            else
            {
                delmsg = "Customer in Use";
                ModelState.AddModelError(String.Empty, delmsg);
                err_flag = false;

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
           if(action_flag == "Create")
            {
                
                AR_001_CUSTM = new AR_001_CUSTM();
                init_prty();
                AR_001_CUSTM.created_by = pubsess.userid;
                AR_001_CUSTM.created_date = DateTime.UtcNow;
                AR_001_CUSTM.delete_flag = "N";
                if (move_auto == "Y")
                    glay.vwstring0 = util.autogen_num("CUS");
            }
            else
            {
                AR_001_CUSTM = db.AR_001_CUSTM.Find(glay.vwstring0);
            }
            AR_001_CUSTM.attach_document = "";
            AR_001_CUSTM.customer_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            AR_001_CUSTM.cust_biz_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            AR_001_CUSTM.business_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            AR_001_CUSTM.tax_reg_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            AR_001_CUSTM.biz_reg_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            AR_001_CUSTM.cust_address1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            AR_001_CUSTM.cust_address2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
            AR_001_CUSTM.credit_limit_amt = glay.vwdecimal0;
            AR_001_CUSTM.del_same_loc = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            AR_001_CUSTM.special_discount = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            AR_001_CUSTM.cust_city = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
            AR_001_CUSTM.cust_postal_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            AR_001_CUSTM.cust_country = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
            AR_001_CUSTM.cust_state = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
            AR_001_CUSTM.statement_email = glay.vwbool0 ? "Y" : "N";
            AR_001_CUSTM.cust_website = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
            AR_001_CUSTM.cust_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
            AR_001_CUSTM.cust_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
            AR_001_CUSTM.bil_same_loc = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
            if (glay.vwstrarray0[13] == "Y")
            {
                AR_001_CUSTM.billing_address1 = glay.vwstrarray0[4];
                AR_001_CUSTM.billing_address2 = glay.vwstrarray0[5];
                AR_001_CUSTM.billing_city = glay.vwstrarray0[6];
                AR_001_CUSTM.billing_postal_code = glay.vwstrarray0[7];
                AR_001_CUSTM.billing_country = glay.vwstrarray0[8];
                AR_001_CUSTM.billing_state = glay.vwstrarray0[9];
                AR_001_CUSTM.billing_email = glay.vwstrarray0[11];
                AR_001_CUSTM.billing_phone = glay.vwstrarray0[12];
            }
            else
            {
                AR_001_CUSTM.billing_address1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
                AR_001_CUSTM.billing_address2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[15]) ? "" : glay.vwstrarray0[15];
                AR_001_CUSTM.billing_city = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
                AR_001_CUSTM.billing_postal_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[17]) ? "" : glay.vwstrarray0[17];
                AR_001_CUSTM.billing_country = string.IsNullOrWhiteSpace(glay.vwstrarray0[18]) ? "" : glay.vwstrarray0[18];
                AR_001_CUSTM.billing_state = string.IsNullOrWhiteSpace(glay.vwstrarray0[19]) ? "" : glay.vwstrarray0[19];
                AR_001_CUSTM.billing_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[20]) ? "" : glay.vwstrarray0[20];
                AR_001_CUSTM.billing_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[21]) ? "" : glay.vwstrarray0[21];
            }
            AR_001_CUSTM.contact_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[22]) ? "" : glay.vwstrarray0[22];
            AR_001_CUSTM.contact_dept = string.IsNullOrWhiteSpace(glay.vwstrarray0[23]) ? "" : glay.vwstrarray0[23];
            AR_001_CUSTM.contact_job_title = string.IsNullOrWhiteSpace(glay.vwstrarray0[24]) ? "" : glay.vwstrarray0[24];
            AR_001_CUSTM.contact_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[25]) ? "" : glay.vwstrarray0[25];
            AR_001_CUSTM.contact_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[26]) ? "" : glay.vwstrarray0[26];
            //AR_001_CUSTM.contact_country = string.IsNullOrWhiteSpace(glay.vwstrarray0[27]) ? "" : glay.vwstrarray0[27];
            AR_001_CUSTM.currency_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[27]) ? "" : glay.vwstrarray0[27];
            AR_001_CUSTM.payment_method = string.IsNullOrWhiteSpace(glay.vwstrarray0[28]) ? "" : glay.vwstrarray0[28];
            AR_001_CUSTM.card_details_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[29]) ? "" : glay.vwstrarray0[29];
            AR_001_CUSTM.name_on_card = string.IsNullOrWhiteSpace(glay.vwstrarray0[30]) ? "" : glay.vwstrarray0[30];
            AR_001_CUSTM.card_details_maker = string.IsNullOrWhiteSpace(glay.vwstrarray0[31]) ? "" : glay.vwstrarray0[31];
            AR_001_CUSTM.card_security_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[32]) ? "" : glay.vwstrarray0[32];
            AR_001_CUSTM.card_expiry_date = string.IsNullOrWhiteSpace(glay.vwstrarray0[33]) ? "" : glay.vwstrarray0[33];
            AR_001_CUSTM.credit_facilities = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            AR_001_CUSTM.payment_term_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[34]) ? "" : glay.vwstrarray0[34];
            AR_001_CUSTM.payment_matching = string.IsNullOrWhiteSpace(glay.vwstrarray0[35]) ? "" : glay.vwstrarray0[35];
            AR_001_CUSTM.gl_account_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[36]) ? "" : glay.vwstrarray0[36];
            AR_001_CUSTM.sales_rep = string.IsNullOrWhiteSpace(glay.vwstrarray0[37]) ? "" : glay.vwstrarray0[37];
            AR_001_CUSTM.price_class = string.IsNullOrWhiteSpace(glay.vwstrarray0[38]) ? "" : glay.vwstrarray0[38];
            AR_001_CUSTM.cust_discount_percent = glay.vwdecimal2;
            AR_001_CUSTM.modified_date = DateTime.UtcNow;
            AR_001_CUSTM.modified_by = pubsess.userid;
            AR_001_CUSTM.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[39]) ? "" : glay.vwstrarray0[39];
            AR_001_CUSTM.active_status = glay.vwbool1 ? "Y" : "N";
            AR_001_CUSTM.pinstalment_paid = 0;
            AR_001_CUSTM.poutstand_instalment_amt = 0;
            AR_001_CUSTM.num_pinstalment_outstand = 0;

            AR_001_CUSTM.analysis_code1 = "";
            AR_001_CUSTM.analysis_code2 = "";
            AR_001_CUSTM.analysis_code3 = "";
            AR_001_CUSTM.analysis_code4 = "";
            AR_001_CUSTM.analysis_code5 = "";
            AR_001_CUSTM.analysis_code6 = "";
            AR_001_CUSTM.analysis_code7 = "";
            AR_001_CUSTM.analysis_code8 = "";
            AR_001_CUSTM.analysis_code9 = "";
            AR_001_CUSTM.analysis_code10 = "";

            int arrlen = glay.vwstrarray6.Length;
            if (arrlen > 0)
                AR_001_CUSTM.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
            if (arrlen > 1)
                AR_001_CUSTM.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
            if (arrlen > 2)
                AR_001_CUSTM.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
            if (arrlen > 3)
                AR_001_CUSTM.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
            if (arrlen > 4)
                AR_001_CUSTM.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
            if (arrlen > 5)
                AR_001_CUSTM.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
            if (arrlen > 6)
                AR_001_CUSTM.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
            if (arrlen > 7)
                AR_001_CUSTM.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
            if (arrlen > 8)
                AR_001_CUSTM.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
            if (arrlen > 9)
                AR_001_CUSTM.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
            psess.intemp0 = arrlen;
            Session["psess"] = psess;
            
            if (AR_001_CUSTM.currency_code == "")
            {
                AR_001_CUSTM.currency_code = pubsess.base_currency_code;
            }

           if(action_flag == "Create")
                db.Entry(AR_001_CUSTM).State = EntityState.Added;
            else
                db.Entry(AR_001_CUSTM).State = EntityState.Modified;

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
                util.parameter_deleteflag("001", glay.vwstring0);
                string str = "update AR_001_CTERM set delete_flag ='Y' from AR_001_CTERM a, AR_001_CUSTM b where a.credit_term_code = b.payment_term_code";
                str += " and customer_code =" + util.sqlquote(glay.vwstring0);
                db.Database.ExecuteSqlCommand(str);

                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, AR_001_CUSTM b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and customer_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(stri);



                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("CUSTOMER", AR_001_CUSTM.customer_code, photo1, glay.vwstrarray9);
                }

            }
            if (err_flag)
            {
                if (glay.vwstring2 == "Y")
                {
                    AR_001_DADRS gadd = new AR_001_DADRS();
                    gadd.address_type = "CUSTOMER";
                    gadd.customer_code = glay.vwstring0;
                    gadd.address_code = 0;
                    gadd.address1 = glay.vwstrarray0[4];
                    gadd.address2 = glay.vwstrarray0[5];
                    gadd.city = glay.vwstrarray0[6];
                    gadd.postal_code = glay.vwstrarray0[7];
                    gadd.country = glay.vwstrarray0[8];
                    gadd.state = glay.vwstrarray0[9];
                    gadd.email = glay.vwstrarray0[11];
                    gadd.phone = glay.vwstrarray0[12];
                    gadd.contact_name = glay.vwstrarray0[22];
                    gadd.contact_dept = glay.vwstrarray0[23];
                    gadd.contact_email = glay.vwstrarray0[25];
                    gadd.contact_phone = glay.vwstrarray0[26];
                    gadd.contact_job_title = glay.vwstrarray0[24];
                    gadd.created_by = pubsess.userid;
                    gadd.modified_by = pubsess.userid;
                    gadd.created_date = DateTime.UtcNow;
                    gadd.modified_date = DateTime.UtcNow;
                    gadd.active_status = "Y";
                    gadd.note = "";


                }

            }

        }

        private void validation_routine()
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            if (string.IsNullOrWhiteSpace(glay.vwstring0) && move_auto != "Y")
            {
                ModelState.AddModelError(String.Empty, "Customer ID must not be spaces");
                err_flag = false;
            }
            if (glay.vwstring3 == "Y")
            {
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[34]))
                {
                    ModelState.AddModelError(String.Empty, "Credit Days Term must not be spaces");
                    err_flag = false;
                }
                if (glay.vwdecimal0 <= 0)
                {
                    ModelState.AddModelError(String.Empty, "Enter a valid Credit Limit Amount");
                    err_flag = false;
                }
            }
           if(action_flag == "Create")
            {
                AR_001_CUSTM bnk = db.AR_001_CUSTM.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Customer ID already exist");
                    err_flag = false;
                }

           

            string sqlstr = "select '1' query0 from AR_001_CUSTM where cust_biz_name=" + util.sqlquote(glay.vwstrarray0[0]) + " and customer_code <> " + util.sqlquote(glay.vwstring0);
            var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (bglist1 != null)
            {
                ModelState.AddModelError(String.Empty, "Can not accept duplicate Business Name");
                err_flag = false;
            }
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

        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray6 = new string[20];

            glay.vwstring0 = AR_001_CUSTM.customer_code;
            glay.vwstrarray0[0] = AR_001_CUSTM.cust_biz_name;
            glay.vwstrarray0[1] = AR_001_CUSTM.business_type;
            glay.vwstrarray0[2] = AR_001_CUSTM.tax_reg_number;
            glay.vwstrarray0[3] = AR_001_CUSTM.biz_reg_number;
            glay.vwstrarray0[4] = AR_001_CUSTM.cust_address1;
            glay.vwstrarray0[5] = AR_001_CUSTM.cust_address2;
            glay.vwstrarray0[6] = AR_001_CUSTM.cust_city;
            glay.vwdecimal0 = AR_001_CUSTM.credit_limit_amt;
            glay.vwstrarray0[7] = AR_001_CUSTM.cust_postal_code;
            glay.vwstrarray0[8] = AR_001_CUSTM.cust_country;
            glay.vwstrarray0[9] = AR_001_CUSTM.cust_state;
            glay.vwstrarray0[10] = AR_001_CUSTM.cust_website;
            glay.vwstrarray0[11] = AR_001_CUSTM.cust_email;
            glay.vwstrarray0[12] = AR_001_CUSTM.cust_phone;
            glay.vwstrarray0[13] = AR_001_CUSTM.bil_same_loc;
            glay.vwstring2 = AR_001_CUSTM.del_same_loc;
            glay.vwstring1 = AR_001_CUSTM.special_discount;
            glay.vwdecimal2 = AR_001_CUSTM.cust_discount_percent;
            glay.vwstrarray0[14] = AR_001_CUSTM.billing_address1;
            glay.vwstrarray0[15] = AR_001_CUSTM.billing_address2;
            glay.vwstrarray0[16] = AR_001_CUSTM.billing_city;
            glay.vwstrarray0[17] = AR_001_CUSTM.billing_postal_code;
            glay.vwstrarray0[18] = AR_001_CUSTM.billing_country;
            glay.vwstrarray0[19] = AR_001_CUSTM.billing_state;
            glay.vwstrarray0[20] = AR_001_CUSTM.billing_email;
            glay.vwstrarray0[21] = AR_001_CUSTM.billing_phone;
            glay.vwstrarray0[22] = AR_001_CUSTM.contact_name;
            glay.vwstrarray0[23] = AR_001_CUSTM.contact_dept;
            glay.vwstrarray0[24] = AR_001_CUSTM.contact_job_title;
            glay.vwstrarray0[25] = AR_001_CUSTM.contact_email;
            glay.vwstrarray0[26] = AR_001_CUSTM.contact_phone;
            //glay.vwstrarray0[27] = AR_001_CUSTM.contact_country;
            glay.vwstrarray0[27] = AR_001_CUSTM.currency_code;
            glay.vwstrarray0[28] = AR_001_CUSTM.payment_method;
            glay.vwstrarray0[29] = AR_001_CUSTM.card_details_number;
            glay.vwstrarray0[30] = AR_001_CUSTM.name_on_card;
            glay.vwstrarray0[31] = AR_001_CUSTM.card_details_maker;
            glay.vwstrarray0[32] = AR_001_CUSTM.card_security_code;
            glay.vwstrarray0[33] = AR_001_CUSTM.card_expiry_date;
            glay.vwstring3 = AR_001_CUSTM.credit_facilities;
            glay.vwstrarray0[34] = AR_001_CUSTM.payment_term_code;
            glay.vwstrarray0[35] = AR_001_CUSTM.payment_matching;
            glay.vwstrarray0[36] = AR_001_CUSTM.gl_account_code;
            glay.vwstrarray0[37] = AR_001_CUSTM.sales_rep;
            glay.vwstrarray0[38] = AR_001_CUSTM.price_class;
            glay.vwstrarray0[39] = AR_001_CUSTM.note;
            glay.vwdecimal4 = AR_001_CUSTM.pinstalment_paid;
            glay.vwdecimal3 = AR_001_CUSTM.poutstand_instalment_amt;
            glay.vwint0 = AR_001_CUSTM.num_pinstalment_outstand;
            glay.vwstrarray6[0] = AR_001_CUSTM.analysis_code1;
            glay.vwstrarray6[1] = AR_001_CUSTM.analysis_code2;
            glay.vwstrarray6[2] = AR_001_CUSTM.analysis_code3;
            glay.vwstrarray6[3] = AR_001_CUSTM.analysis_code4;
            glay.vwstrarray6[4] = AR_001_CUSTM.analysis_code5;
            glay.vwstrarray6[5] = AR_001_CUSTM.analysis_code6;
            glay.vwstrarray6[6] = AR_001_CUSTM.analysis_code7;
            glay.vwstrarray6[7] = AR_001_CUSTM.analysis_code8;
            glay.vwstrarray6[8] = AR_001_CUSTM.analysis_code9;
            glay.vwstrarray6[9] = AR_001_CUSTM.analysis_code10;

            if (AR_001_CUSTM.statement_email == "Y")
                glay.vwbool0 = true;
            if (AR_001_CUSTM.active_status == "Y")
                glay.vwbool1 = true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "CUSTOMER" && bg.document_code == AR_001_CUSTM.customer_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();


        }

        private void initial_rtn()
        {

            glay.vwstrarray0 = new string[50];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray0[34] = "N";
            glay.vwstrarray0[28] = "C";
            glay.vwstrarray0[13] = "Y";
            glay.vwstring1 = "N";
            glay.vwstring2 = "Y";
            glay.vwstring3 = "N";
            glay.vwbool1 = false;
            glay.vwstrarray0[27] = pubsess.base_currency_code;
            glay.vwstring11 = pubsess.base_currency_code;
            glay.vwlist0 = new List<querylay>[20];
        }
        private void select_query()
        {
            var bglist = from bg in db.AR_001_CTERM
                         where bg.active_status == "N"
                         orderby bg.description
                         select bg;

            ViewBag.credit = new SelectList(bglist.ToList(), "credit_term_code", "description", glay.vwstrarray0[34]);

            ViewBag.sales = util.para_selectquery("62", glay.vwstrarray0[37]);
            //  ViewBag.sales = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[37]);
            //var bglistu = from bg in db.GB_001_EMP
            //              where bg.active_status == "N"
            //              orderby bg.name
            //             select bg;

            //ViewBag.sales = new SelectList(bglistu.ToList(), "employee_code", "name", glay.vwstrarray0[37]);
            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[27]);

            //var bglistc = from bg in db.MC_001_CUREN
            //              where bg.active_status == "N"
            //              orderby bg.currency_description
            //             select bg;

            //ViewBag.currency = new SelectList(bglistc.ToList(), "currency_code", "currency_description", glay.vwstrarray0[27]);
            ViewBag.country = util.para_selectquery("13", glay.vwstrarray0[8], "N");
            //var bgitem = from bg in db.GB_001_PCODE
            //             where bg.parameter_type == "13" && bg.active_status == "N"
            //             orderby bg.parameter_name
            //             select bg;
            //ViewBag.country = new SelectList(bgitem.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[8]);
            ViewBag.country1 = util.para_selectquery("13", glay.vwstrarray0[18], "N");

            var hdet = (from bg in db.GB_001_COY
                        where bg.id_code == "COYPRICE"
                        select bg).FirstOrDefault();

            string pcl1 = hdet.field6;
            string pcl2 = hdet.field7;
            string pcl3 = hdet.field8;
            string pcl4 = hdet.field9;
            string pcl5 = hdet.field10;
            string pcl6 = hdet.field11;

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = pcl1 });
            ary.Add(new SelectListItem { Value = "2", Text = pcl2 });
            ary.Add(new SelectListItem { Value = "3", Text = pcl3 });
            ary.Add(new SelectListItem { Value = "4", Text = pcl4 });
            ary.Add(new SelectListItem { Value = "5", Text = pcl5 });
            ary.Add(new SelectListItem { Value = "6", Text = pcl6 });

            ViewBag.price = new SelectList(ary.ToArray(), "Value", "Text", glay.vwstrarray0[38]);
            //if (glay.vwstrarray0[8] != null)
            //{
            //    var bglisti = from bg in db.GB_001_PCODE
            //                  where bg.parameter_type == "14" && bg.active_status == "N"
            //                  orderby bg.parameter_name
            //                  select bg;
            //    ViewBag.state = new SelectList(bglisti.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[9]);
            //}
            //else
            //{
            //    ViewBag.state = new SelectList("", "", glay.vwstrarray0[9]);
            //}
            ViewBag.job = util.para_selectquery("04", glay.vwstrarray0[24]);
            //var bglista = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "04" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //              select bg;

            //ViewBag.job = new SelectList(bglista.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[24]);

            ViewBag.department = util.para_selectquery("05", glay.vwstrarray0[23]);
            //var bglistb = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "05"&& bg.active_status == "N"
            //              orderby bg.parameter_name
            //              select bg;
            //ViewBag.department = new SelectList(bglistb.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[23]);

            var bgitemy = from bg in db.GB_999_MSG
                          where bg.type_msg == "PM"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.payment = new SelectList(bgitemy.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[35]);
            var bgitemt = from bg in db.GB_999_MSG
                          where bg.type_msg == "BIZT"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.type = new SelectList(bgitemt.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[1]);

            var bgitemz = from bg in db.GB_999_MSG
                          where bg.type_msg == "CT"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.card = new SelectList(bgitemz.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[31]);

            ViewBag.gl_acc = util.read_ledger("010", glay.vwstrarray0[36]);
            // bg2 = util.read_ledger("010");
            //ViewBag.gl_acc = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[36]);


            //string sqlstg = "select account_code query0, account_name query1 from GL_001_CHART where account_type_code = (select acct_type_code  from GL_001_ATYPE where acct_type_code = 'prt')";
            //var bg5 = db.Database.SqlQuery<querylay>(sqlstg);

            //ViewBag.gl_disposal_code = new SelectList(bg5.ToList(), "query0", "query1", glay.vwstrarray0[15]);

        }
        private void priceclass()
        {
            //string ctply = "";
            var hdet = (from bg in db.GB_001_COY
                        where bg.id_code == "COYPRICE"
                        select bg).FirstOrDefault();

            string ctdis = hdet.field1;
            //glay.vwstring3 = hdet.field1;
            psess.temp7 = ctdis;
            Session["psess"] = psess;
            
        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            err_flag = true;
            glay.vwstring0 = id;
            delete_record();

            if (!err_flag)
            {
                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "1", Text = delmsg });
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary.ToArray(),
                                    "Value",
                                    "Text")
                                   , JsonRequestBehavior.AllowGet);


            }
            return RedirectToAction("Index");
        }

        private void init_prty()
        {
            AR_001_CUSTM.title = "";
            AR_001_CUSTM.other_name = "";
            AR_001_CUSTM.sex = "";
            AR_001_CUSTM.marital_status = "";
            AR_001_CUSTM.dob = "";
            AR_001_CUSTM.nationality = "";
            AR_001_CUSTM.emp_address = "";
            AR_001_CUSTM.kin_name = "";
            AR_001_CUSTM.kin_address = "";
            AR_001_CUSTM.kin_phone = "";
            AR_001_CUSTM.purpose = "";
            AR_001_CUSTM.employer = "";
        }
        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

            SelectList[] head_det = new SelectList[20];

            //Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            //psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "017" && bg.sequence_no != 99
                         select bg;

            foreach (var item in bglist.ToList())
            {
                int count2 = item.sequence_no;
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

        [HttpPost]
        public ActionResult pricehead_list(string id)
        {
            // write your query statement
            var hdet = from bg in db.GB_001_PCODE
                       where bg.parameter_type == "14" && bg.gl_account_code == id
                       orderby bg.parameter_name
                       select new
                       {
                           c1 = bg.parameter_code,
                           c2 = bg.parameter_name
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

        public ActionResult show_doc(int id)
        {
            var bglist = (from bg in db.GB_001_DOC
                          where bg.document_sequence == id
                          select bg).FirstOrDefault();


            byte[] imagedata = bglist.document_image;
            return File(imagedata, "png");
        }

        //private void parameter_deleteflag(string datatype, string givenval)
        //{
        //    string table_name = "";
        //    string acctname = "";
        //    string custcode = "";

        //    if (datatype == "001")
        //    {
        //        table_name = " AR_001_CUSTM ";
        //        acctname = " gl_account_code ";
        //        custcode = " customer_code ";
        //    }

        //    string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a," + table_name+" b where a.account_code in (b.";
        //    str += acctname + ") and " + custcode +"=" + util.sqlquote(givenval);
        //    db.Database.ExecuteSqlCommand(str);

        //    str = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, " + table_name + " b where header_sequence in (analysis_code1";
        //    str += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
        //    str += " and " + custcode + "=" + util.sqlquote(givenval);
        //    db.Database.ExecuteSqlCommand(str);

        //}


        private void cal_auto()
        {
            var autoset = (from bg in db.GB_001_COY
                           where bg.id_code == "COYAUTO"
                           select bg.field4).FirstOrDefault();

            if (autoset == "Y")
                move_auto = "Y";

        }
    }

}
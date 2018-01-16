using CittaErp.Models;
using CittaErp.utilities;
using anchor1.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class PpaymentController : Controller
    {
        IV_002_PAY IV_002_PAY = new IV_002_PAY();
        MainContext db = new MainContext();
        MainContext db1 = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string atype = "cus";
        bool err_flag = true;
        string delmsg = "";
        string move_auto = "N";
        bool submit_flag = false;
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            //Session["multi_curenchk"] = pubsess.multi_currency;

            var bglist = from bh in db.IV_002_PAY
                         join bg in db.IV_001_ITEM
                         on new { a2 = bh.item_code } equals new { a2 = bg.item_code }
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         where bh.approval_level !=0
                         select new vw_genlay
                         {
                            vwint0 =bh.ref_number,
                            vwstring2= bh.teller_num,
                             vwstring0 = bg2.item_name,
                             vwstring1 = bh.bank_account,
                             vwstring5 = bh.contract_id,
                             vwdecimal0 = bh.payment_amt,
                             vwstring3 = bh.transaction_date
                         };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            header_ana();
            select_query();
           // cal_auto();
           // psess.temp5 = move_auto;
            //if (move_auto == "Y")
            //    glay.vwstring0 = "AUTO";
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile, string headtype = "D")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;

            if (headtype == "send_app")
            {
                submit_flag = true;

            }
            //cal_auto();
            update_file();

            if (err_flag){

                return RedirectToAction("Create");
            }
            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            psess = (psess)Session["psess"];
            // initial_rtn();
            psess.temp5 = move_auto;
            pubsess = (pubsess)Session["pubsess"];
            IV_002_PAY = db.IV_002_PAY.Find(key1);
            if (IV_002_PAY != null)
                read_record();
            header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile, string headtype="D")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;
            if (headtype == "send_app")
            {
                submit_flag = true;

            }
            if (id_xhrt == "D")
            {
                delete_record();
                if (err_flag)
                    return RedirectToAction("Index");
            }

            photo1 = photofile;
            if (err_flag)
            {
                update_file();
                return RedirectToAction("Index");
            }
            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        private void delete_record()
        {
            IV_002_PAY = db.IV_002_PAY.Find(glay.vwstring5);
            if (IV_002_PAY != null)
            {
                db.IV_002_PAY.Remove(IV_002_PAY);
                db.SaveChanges();
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
                IV_002_PAY = new IV_002_PAY();
                IV_002_PAY.created_by = pubsess.userid;
                IV_002_PAY.created_date = DateTime.UtcNow;
                IV_002_PAY.approval_level = 0;
                IV_002_PAY.approval_date = DateTime.UtcNow;
                IV_002_PAY.approval_by = pubsess.userid;
               
            }
            else
            {
                //string id = glay.vwstring5;
                IV_002_PAY = db.IV_002_PAY.Find(glay.vwstring5);
            }

            int itm = glay.vwstring0.IndexOf("[]");
            string itm1 = glay.vwstring0.Substring(0, itm);
            IV_002_PAY.contract_id = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            IV_002_PAY.teller_num = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            IV_002_PAY.property_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            IV_002_PAY.item_code = itm1;
            IV_002_PAY.ref_number = 0;
            IV_002_PAY.bank_account = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            IV_002_PAY.transaction_date = util.date_yyyymmdd(glay.vwstring2);
            //IV_002_PAY.sales_rep = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            //IV_002_PAY.customer_id = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            IV_002_PAY.pay_method = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            IV_002_PAY.price = glay.vwdecimal6;
            IV_002_PAY.payment_amt = glay.vwdecimal0;
            IV_002_PAY.sales_com = glay.vwdecimal1;
            IV_002_PAY.sales_val = glay.vwdecimal2;
            IV_002_PAY.tenor = glay.vwdecimal5;
            IV_002_PAY.monthly_amt = glay.vwdecimal4;
            IV_002_PAY.exchange_rate = glay.vwdecimal7;
            IV_002_PAY.base_amount = 0;
            IV_002_PAY.currency = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            IV_002_PAY.modified_date = DateTime.UtcNow;
            IV_002_PAY.modified_by = pubsess.userid;
            IV_002_PAY.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
           // IV_002_PAY.active_status = glay.vwbool1 ? "Y" : "N";

            if (IV_002_PAY.currency == pubsess.base_currency_code)
                IV_002_PAY.exchange_rate = 1;
            else
            {

                string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(IV_002_PAY.currency) + " and '" + IV_002_PAY.transaction_date + "' between date_from and date_to";
                var exch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                if (exch != null)
                    IV_002_PAY.exchange_rate = exch.dquery0;

            }

            IV_002_PAY.analysis_code1 = "";
            IV_002_PAY.analysis_code2 = "";
            IV_002_PAY.analysis_code3 = "";
            IV_002_PAY.analysis_code4 = "";
            IV_002_PAY.analysis_code5 = "";
            IV_002_PAY.analysis_code6 = "";
            IV_002_PAY.analysis_code7 = "";
            IV_002_PAY.analysis_code8 = "";
            IV_002_PAY.analysis_code9 = "";
            IV_002_PAY.analysis_code10 = "";


            int arrlen = glay.vwstrarray6.Length;
            if (arrlen > 0)
                IV_002_PAY.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
            if (arrlen > 1)
                IV_002_PAY.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
            if (arrlen > 2)
                IV_002_PAY.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
            if (arrlen > 3)
                IV_002_PAY.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
            if (arrlen > 4)
                IV_002_PAY.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
            if (arrlen > 5)
                IV_002_PAY.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
            if (arrlen > 6)
                IV_002_PAY.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
            if (arrlen > 7)
                IV_002_PAY.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
            if (arrlen > 8)
                IV_002_PAY.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
            if (arrlen > 9)
                IV_002_PAY.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];

            psess.intemp0 = arrlen;

            if (submit_flag)
            {
                IV_002_PAY.approval_level = 0;
                string key1 = glay.vwstring5;

               
                IV_001_PC PC = db.IV_001_PC.Find(key1);

                if (PC != null && PC.leeds_name != "")
                {
                    string cust_code = util.autogen_num("CUS");
                    string str = "update IV_001_PC set customer_id =" + util.sqlquote(cust_code) + ", approval_level = 1 where contract_id = " + util.sqlquote(key1);
                    db1.Database.ExecuteSqlCommand(str);

                    string str1 = "insert into AR_001_CUSTM (customer_code,cust_biz_name,sales_rep,cust_address1,business_type,title,nationality,other_name,sex,marital_status,cust_email,dob,";
                    str1 += " cust_phone,emp_address,kin_name,kin_address, contact_job_title,kin_phone,purpose,employer,gl_account_code,currency_code,created_by) values (";
                    str1 += util.sqlquote(cust_code) + "," + util.sqlquote(PC.leeds_name) + "," + util.sqlquote(PC.sales_rep) + ",";
                    str1 += util.sqlquote(PC.address) + "," + util.sqlquote(PC.business_type) + "," + util.sqlquote(PC.title) + ",";
                    str1 += util.sqlquote(PC.nationality) + "," + util.sqlquote(PC.other_name) + "," + util.sqlquote(PC.sex) + ",";
                    str1 += util.sqlquote(PC.marital_status) + "," + util.sqlquote(PC.cust_email) + "," + util.sqlquote(PC.dob) + ",";
                    str1 += util.sqlquote(PC.cust_phone) + "," + util.sqlquote(PC.emp_address) + "," + util.sqlquote(PC.kin_name) + ",";
                    str1 += util.sqlquote(PC.kin_address) + "," + util.sqlquote(PC.contact_job_title) + "," + util.sqlquote(PC.kin_phone) + ",";
                    str1 += util.sqlquote(PC.purpose) + "," + util.sqlquote(PC.employer) + "," + "120003" + "," + util.sqlquote(PC.currency_code) + "," + util.sqlquote(pubsess.userid) + ")";
                    try
                    {
                        db1.Database.ExecuteSqlCommand(str1);
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
           if(action_flag == "Create")
                db.Entry(IV_002_PAY).State = EntityState.Added;
            else
                db.Entry(IV_002_PAY).State = EntityState.Modified;

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
                if (submit_flag)
                    util.update_entry("022", IV_002_PAY.contract_id.ToString(), pubsess.userid);
              
                
                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("PCONTRACT", IV_002_PAY.contract_id.ToString(), photo1, glay.vwstrarray9);
                }

            }
            if (err_flag)
            {
              
            }

        }

        private void validation_routine()
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            //if (Session["action_flag"].ToString() == "Create")
            //{
            //    IV_002_PCNT bnk = db.IV_002_PCNT.Find(glay.vwstring0, glay.vwstrarray0[0]);
            //    if (bnk != null)
            //    {
            //        ModelState.AddModelError(String.Empty, "Contract ID already exist");
            //        err_flag = false;
            //    }
            //}

            if (!util.date_validate(glay.vwstring2))
            {
                ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
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

            if (glay.vwdecimal0==0)
            {
                ModelState.AddModelError(String.Empty, "Payment Amount must be creater than zero");
                err_flag = false;
            }

        }

        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray6 = new string[20];

            var pc = (from bg in db.IV_001_PC
                      where bg.contract_id == IV_002_PAY.contract_id
                      select bg).FirstOrDefault();

            glay.vwstring5 = IV_002_PAY.contract_id;
            glay.vwint0 = IV_002_PAY.ref_number;
            glay.vwstring0 = IV_002_PAY.property_id;
            glay.vwstring7 = IV_002_PAY.teller_num;
            //glay.vwstrarray0[1] = IV_002_PAY.contract_type;
            glay.vwstring2 = util.date_slash(IV_002_PAY.transaction_date);
           // glay.vwstrarray0[3] = IV_002_PAY.sales_rep;
           // glay.vwstrarray0[4] = IV_002_PAY.customer_id;
            glay.vwstring1 = IV_002_PAY.bank_account;
            glay.vwdecimal6 = pc.price;
            glay.vwdecimal0 = IV_002_PAY.payment_amt;
            glay.vwdecimal1 = pc.sales_com;
            glay.vwdecimal2 = pc.sales_val;
            glay.vwdecimal5 = pc.tenor;
            glay.vwdecimal4 = pc.monthly_amt;
            glay.vwstring3 = IV_002_PAY.pay_method;
            glay.vwdecimal7 = pc.exchange_rate;
            glay.vwstring3 = IV_002_PAY.currency;
            glay.vwint0 = Convert.ToInt32(pc.quantity);
            glay.vwfloat1 = glay.vwdecimal2 - glay.vwdecimal1;
            glay.vwstring4 = IV_002_PAY.note;
            glay.vwstrarray6[0] = IV_002_PAY.analysis_code1;
            glay.vwstrarray6[1] = IV_002_PAY.analysis_code2;
            glay.vwstrarray6[2] = IV_002_PAY.analysis_code3;
            glay.vwstrarray6[3] = IV_002_PAY.analysis_code4;
            glay.vwstrarray6[4] = IV_002_PAY.analysis_code5;
            glay.vwstrarray6[5] = IV_002_PAY.analysis_code6;
            glay.vwstrarray6[6] = IV_002_PAY.analysis_code7;
            glay.vwstrarray6[7] = IV_002_PAY.analysis_code8;
            glay.vwstrarray6[8] = IV_002_PAY.analysis_code9;
            glay.vwstrarray6[9] = IV_002_PAY.analysis_code10;
            glay.vwfloat1 = glay.vwdecimal2 - glay.vwdecimal4;

           
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "CUSTOMER" && bg.document_code == IV_002_PAY.contract_id.ToString()
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();


        }

        [HttpPost]
        public ActionResult get_commision_value(string item_code, string qty)
        {
            // write your query statement
            ModelState.Remove("glay.vwdecimal1");
            //tdate = util.date_yyyymmdd(tdate);
            decimal s_val = 0; decimal n_pyt = 0; decimal n_svl=0; //int contact = Convert.ToInt32(item_code);
            decimal s_comm = 0; decimal d_amt = 0; decimal m_amt = 0; decimal dprice = 0; decimal dqty = 0;
            decimal bal = 0; string spec = ""; string curen = ""; decimal outbal = 0;
            decimal.TryParse(qty, out dqty);
            string id_prop = "";

            var curlist = (from bg in db.IV_001_PC
                           join bg1 in db.IV_001_ITEM
                           on new { a1 = bg.item_code } equals new { a1 = bg1.item_code }
                           into bg2
                           from bg3 in bg2.DefaultIfEmpty()
                           where bg.contract_id == item_code
                           select new
                           {
                               c1 = bg.sales_com,
                               c0 = bg.price,
                               c2 = bg.exp_deposit,
                               c3 = bg.monthly_amt,
                               c4 = bg3.deposit_percent,
                               c5 = bg.tenor,
                               c6 = bg.sales_val,
                               c7 = bg3.item_name,
                               c8 = bg.property_id,
                               c9 = bg.currency_code,
                               c10 = bg.item_code,
                               c11 = bg.quantity
                           }).FirstOrDefault();
            if (curlist != null)
            {
                s_comm = curlist.c1;
               
                s_val = curlist.c0;
                
                d_amt = curlist.c2;

                m_amt = curlist.c3;
                                    
                n_pyt = curlist.c5;

                n_svl= curlist.c6;
               
                bal = n_svl- s_comm;
                
                spec =curlist.c7;
                
                curen = curlist.c9;

                id_prop = "(" + curlist.c10 + "/" + curlist.c5 +  ") - " + curlist.c7;

                if (dqty != 0)
                    outbal = bal - dqty;
            }
            string read_cur = (from bg in db.MC_001_CUREN
                               where bg.currency_code == curen
                               select bg.currency_description).FirstOrDefault();

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = s_comm.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "2", Text = s_val.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "3", Text = d_amt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "4", Text = m_amt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "5", Text = n_pyt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "6", Text = n_svl.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "7", Text = bal.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "8", Text = id_prop });
            ary.Add(new SelectListItem { Value = "9", Text = read_cur });
            ary.Add(new SelectListItem { Value = "10", Text = curen });
            ary.Add(new SelectListItem { Value = "11", Text = curlist.c8 });
            ary.Add(new SelectListItem { Value = "12", Text = outbal.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "13", Text = curlist.c11.ToString() });
            
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
        public ActionResult get_currency(string curren_code)
        {
            // write your query statement

            string tdate = DateTime.Now.ToString("yyyyMMdd");

            decimal rat_code = 0;

            pubsess pubsess = (pubsess)Session["pubsess"];
            List<SelectListItem> ary = new List<SelectListItem>();

            if (pubsess.base_currency_code == curren_code)
                ary.Add(new SelectListItem { Value = curren_code, Text = "1" });
            else
            {
                string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(curren_code) + " and '" + tdate + "' between date_from and date_to";
                var dbexch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                if (dbexch != null)
                    rat_code = dbexch.dquery0;

                ary.Add(new SelectListItem { Value = curren_code, Text = rat_code.ToString() });
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
  
        private void initial_rtn()
        {

            glay.vwstrarray0 = new string[50];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwbool1 = false;
            glay.vwstrarray0[27] = pubsess.base_currency_code;
            glay.vwstring6 = pubsess.base_currency_code;
            glay.vwstring2 = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwlist0 = new List<querylay>[20];
        }
        private void select_query()
        {
            var empe = from pf in db.IV_001_ITEM
                       where pf.active_status == "N" && pf.sales == "Y" && pf.item_type == "P"
                       orderby pf.item_code, pf.item_name
                       select new { c1 = pf.item_code, c2 = pf.item_code + "--- " + pf.item_name };
            ViewBag.pid = new SelectList(empe.ToList(), "c1", "c2", glay.vwstring0);
           
            var empb = from pf in db.BK_001_BANK
                       where pf.active_status == "N" 
                       orderby pf.bank_code, pf.bank_name
                       select new { c1 = pf.bank_code, c2 = pf.bank_code + "--- " + pf.bank_name };
            ViewBag.bank = new SelectList(empb.ToList(), "c1", "c2", glay.vwstring1);

            get_contract();
           

            var type = from bg in db.GB_999_MSG
                       where bg.type_msg == "PAYM"
                       select bg;
            ViewBag.paym = new SelectList(type.ToList(), "code_msg", "name1_msg", glay.vwstring3);

            ViewBag.srep = util.para_selectquery("62", glay.vwstrarray0[3], "N");
            ViewBag.cust = util.para_selectquery("001", glay.vwstrarray0[4], "N");

      
            ViewBag.sales = util.para_selectquery("62", glay.vwstrarray0[37]);
           
            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[8]);

            ViewBag.country = util.para_selectquery("13", glay.vwstrarray0[14], "N");
           
            var bgitemt = from bg in db.GB_999_MSG
                          where bg.type_msg == "BIZT"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.type = new SelectList(bgitemt.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[5]);

            
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

        }

        private void get_contract()
        {
            //string str = " select a.contract_id c0, case when a.leeds_name = '' then b.cust_biz_name end c1 from IV_001_PC a, AR_001_CUSTM b";
            //str += " where a.customer_id = b.customer_code";

            string str = "select contract_id query0, leeds_name+ '-' + contract_id query1 from [dbo].[IV_001_PC] where leeds_name <> ''  and approval_level = 0 union all";
            str += " select contract_id query0, cust_biz_name+ '-' + contract_id query1 from [dbo].[IV_001_PC], [dbo].[AR_001_CUSTM] where customer_id = customer_code and approval_level = 0 ";
            var emp1 = db.Database.SqlQuery<querylay>(str);
            ViewBag.cid = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring5);

////            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[16]);
////var emp = from pf in db.IV_001_PC
////                      join 
////                      select pf;
////            ViewBag.cid = new SelectList(emp.ToList(), "contract_id", "contract_id", glay.vwint0);
        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            err_flag = true;
            glay.vwstring0 = id;
           // delete_record();

            string sqlstr = "delete from [dbo].[IV_002_PAY] where contract_id=" + util.sqlquote(id);

            db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");

            //if (!err_flag)
            //{
            //    List<SelectListItem> ary = new List<SelectListItem>();
            //    ary.Add(new SelectListItem { Value = "1", Text = delmsg });
            //    if (HttpContext.Request.IsAjaxRequest())
            //        return Json(new SelectList(
            //                        ary.ToArray(),
            //                        "Value",
            //                        "Text")
            //                       , JsonRequestBehavior.AllowGet);


            //}
            //return RedirectToAction("Index");
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
                         where bg.header_type_code == "004" && bg.sequence_no != 99
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
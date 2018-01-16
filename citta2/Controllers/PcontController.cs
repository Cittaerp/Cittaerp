using CittaErp.Models;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class PcontController : Controller
    {
        IV_001_PC IV_001_PC = new IV_001_PC();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;
        HttpPostedFileBase photo2;

        string atype = "cus";
        bool err_flag = true;
        string delmsg = "";
        string move_auto = "N";
        bool submit_flag = false;
        string action_flag = "";
        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();
            psess = (psess)Session["psess"];
            psess.temp7 = "";
            pubsess = (pubsess)Session["pubsess"];
           // Session["multi_curenchk"] = pubsess.multi_currency;
            Session["psess"] = psess;
            
            var bglist = from bh in db.IV_001_PC
                         join bk in db.AR_001_CUSTM
                         on new { a1 = bh.customer_id} equals new { a1 = bk.customer_code}
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         join bg in db.IV_001_ITEM
                         on new { a2 = bh.item_code } equals new { a2 = bg.item_code}
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         where bh.approval_level==-1
                         select new vw_genlay
                         {
                             vwstring0 = bh.customer_id,
                             vwstring5 = bh.contract_id,
                             vwstring2 = bh.leeds_name == "" ? bk2.cust_biz_name : bh.leeds_name,
                             vwstring3 = bh.leeds_name == "" ? "Existing Customer" : "New Lead",
                             vwstring4 = bg2.item_name,
                             //vwstring5 = bh.active_status == "N" ? "Active" : "Inactive"
                         };

            return View(bglist.ToList());


        }

        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            header_ana();
            select_query();
            cal_auto();
            psess.temp5 = move_auto;
            Session["psess"] = psess;
            
            if (move_auto == "Y")
                glay.vwstring3 = "AUTO";
            return View(glay);
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1, string headtype = "D", string subcheck = "")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            photo2 = picture1;
            if (headtype == "send_app")
            {
                submit_flag = true;

            }
            if (subcheck == "RR")
            {
                custpreload();
                header_ana();
                select_query();
                glay.vwstrarray2 = new string[50];
                return View(glay);
            }
            cal_auto();
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        private void custpreload()
        {
            string customer = glay.vwstrarray0[4];

            ModelState.Clear();
            initial_rtn();
            var itempre = (from bg in db.IV_001_PC
                           where bg.customer_id == customer
                           select bg).FirstOrDefault();
            if (itempre != null)
            {
                glay.vwstrarray0[4] = customer;
                glay.vwstrarray0[23] = itempre.title;
                glay.vwstrarray0[0] = itempre.leeds_name;
                glay.vwstrarray0[1] = itempre.other_name;
                glay.vwstrarray0[6] = itempre.purpose;
                glay.vwstrarray0[7] = itempre.sex;
                glay.vwstrarray0[21] = itempre.marital_status;
                glay.vwstrarray0[11] = itempre.dob;
                glay.vwstrarray0[14] = itempre.nationality;
                glay.vwstrarray0[9] = itempre.address;
                glay.vwstrarray0[19] = itempre.cust_phone;
                glay.vwstrarray0[18] = itempre.cust_email;
                glay.vwstrarray0[12] = itempre.contact_job_title;
                glay.vwstrarray0[13] = itempre.employer;
                glay.vwstrarray0[10] = itempre.emp_address;
                glay.vwstrarray0[5] = itempre.business_type;
                glay.vwstrarray0[15] = itempre.kin_name;
                glay.vwstrarray0[16] = itempre.kin_address;
                glay.vwstrarray0[17] = itempre.kin_phone;
            }

        }

        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            psess = (psess)Session["psess"];
            // initial_rtn();
            psess.temp5 = move_auto;
            Session["psess"] = psess;
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            IV_001_PC = db.IV_001_PC.Find(key1);
            if (IV_001_PC != null)
                read_record();
            header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1, string headtype = "D")
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
            photo2 = picture1;
            if (err_flag)
            {
                update_file();
                if (err_flag)
                    return RedirectToAction("Index");
            }

            initial_rtn();
            header_ana();
            select_query();

            return View(glay);
        }

        private void delete_record()
        {
            IV_001_PC = db.IV_001_PC.Find(glay.vwstring3);
            if (IV_001_PC != null)
            {
                db.IV_001_PC.Remove(IV_001_PC);
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
                IV_001_PC = new IV_001_PC();
                IV_001_PC.created_by = pubsess.userid;
                IV_001_PC.created_date = DateTime.UtcNow;
                IV_001_PC.approval_level = -1;
                IV_001_PC.approval_date = DateTime.UtcNow;
                IV_001_PC.approval_by = pubsess.userid;
                IV_001_PC.current_balance = 0;
                IV_001_PC.full_payment = "N";
                IV_001_PC.gift1 = "";
                IV_001_PC.gift2 = "";
                IV_001_PC.gift3 = "";
                IV_001_PC.gift4 = "";
                IV_001_PC.gift5 = "";


                if (move_auto == "Y")
                    glay.vwstring3 = util.autogen_num("PC");
            
               
            }
            else
            {
                IV_001_PC = db.IV_001_PC.Find(glay.vwstring3);
            }
           // IV_001_PC.contract_id = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            int itm = Convert.ToInt16(glay.vwstring0);
           
            var pmtrx = (from bg in db.AR_001_PMTRX
                         where bg.auto_id == itm
                         select bg).FirstOrDefault();
             //string itm1 = glay.vwstring0.Substring(0, itm);
            IV_001_PC.gift_flag = "N";
            IV_001_PC.property_id = pmtrx.item_code+"[]"+pmtrx.tenor;
            IV_001_PC.item_code = pmtrx.item_code;
            IV_001_PC.contract_id = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            IV_001_PC.other_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            IV_001_PC.transaction_date = util.date_yyyymmdd(glay.vwstrarray0[2]);
            IV_001_PC.sales_rep = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            IV_001_PC.customer_id = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            IV_001_PC.leeds_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            IV_001_PC.price = glay.vwdecimal6;
            IV_001_PC.exp_deposit = glay.vwdecimal3;
            IV_001_PC.quantity = glay.vwdecimal0;
            IV_001_PC.sales_com = glay.vwdecimal1;
            IV_001_PC.sales_val = glay.vwdecimal2;
            IV_001_PC.net_sales_val = glay.vwdecimal2-glay.vwdecimal1;
            IV_001_PC.tenor = pmtrx.tenor;
            IV_001_PC.monthly_amt = glay.vwdecimal4;
            IV_001_PC.commission_paid = glay.vwdecimal7;
            IV_001_PC.exchange_rate = 0;
            IV_001_PC.business_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
            IV_001_PC.purpose = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
            IV_001_PC.sex = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            IV_001_PC.address = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
            IV_001_PC.emp_address = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
            IV_001_PC.dob = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
            IV_001_PC.cust_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[19]) ? "" : glay.vwstrarray0[19];
            IV_001_PC.nationality = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
            IV_001_PC.employer = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
            IV_001_PC.kin_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[15]) ? "" : glay.vwstrarray0[15];
            IV_001_PC.kin_address = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
            IV_001_PC.kin_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[17]) ? "" : glay.vwstrarray0[17];
            IV_001_PC.cust_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[18]) ? "" : glay.vwstrarray0[18];
            IV_001_PC.contact_job_title = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
            IV_001_PC.marital_status = string.IsNullOrWhiteSpace(glay.vwstrarray0[21]) ? "" : glay.vwstrarray0[21];
            IV_001_PC.currency_code = pmtrx.currency;
            IV_001_PC.specification = string.IsNullOrWhiteSpace(glay.vwstrarray0[22]) ? "" : glay.vwstrarray0[22];
            IV_001_PC.title = string.IsNullOrWhiteSpace(glay.vwstrarray0[23]) ? "" : glay.vwstrarray0[23];
            IV_001_PC.selected_promo = pmtrx.selected_promo;
            IV_001_PC.termination_charge = string.IsNullOrWhiteSpace(glay.vwstrarray0[25]) ? "" : glay.vwstrarray0[25];
            IV_001_PC.modified_date = DateTime.UtcNow;
            IV_001_PC.modified_by = pubsess.userid;
            IV_001_PC.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[20]) ? "" : glay.vwstrarray0[20];
            IV_001_PC.active_status = glay.vwbool1 ? "Y" : "N";
            IV_001_PC.commission_balance = glay.vwdecimal2 - glay.vwdecimal1;

            IV_001_PC.analysis_code1 = "";
            IV_001_PC.analysis_code2 = "";
            IV_001_PC.analysis_code3 = "";
            IV_001_PC.analysis_code4 = "";
            IV_001_PC.analysis_code5 = "";
            IV_001_PC.analysis_code6 = "";
            IV_001_PC.analysis_code7 = "";
            IV_001_PC.analysis_code8 = "";
            IV_001_PC.analysis_code9 = "";
            IV_001_PC.analysis_code10 = "";

            int arrlen = glay.vwstrarray6.Length;
            if (arrlen > 0)
                IV_001_PC.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
            if (arrlen > 1)
                IV_001_PC.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
            if (arrlen > 2)
                IV_001_PC.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
            if (arrlen > 3)
                IV_001_PC.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
            if (arrlen > 4)
                IV_001_PC.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
            if (arrlen > 5)
                IV_001_PC.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
            if (arrlen > 6)
                IV_001_PC.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
            if (arrlen > 7)
                IV_001_PC.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
            if (arrlen > 8)
                IV_001_PC.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
            if (arrlen > 9)
                IV_001_PC.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
            psess.intemp0 = arrlen;
            Session["psess"] = psess;
            
            decimal get_disc = (from pf in db.DC_001_DISC
                            where pf.discount_code == IV_001_PC.selected_promo && pf.stepped_discount_active == "P" && pf.discount_count == 0
                            select pf.qualified_amount).FirstOrDefault();
                
            IV_001_PC.gift_min_amount = 0-get_disc;

            var gift_gl = from bg in db.DC_001_DISC
                          where bg.discount_code == IV_001_PC.selected_promo && bg.stepped_discount_active == "P"
                          select bg;
            foreach (var item in gift_gl.ToList())
            {
                if (item.discount_count == 1)
                {
                    IV_001_PC.gift1 = item.gift_code;
                    IV_001_PC.gift_qty1 = item.gift_qty;

                }
                if (item.discount_count == 2)
                {
                    IV_001_PC.gift2 = item.gift_code;
                    IV_001_PC.gift_qty2 = item.gift_qty;

                }
                if (item.discount_count == 3)
                {
                    IV_001_PC.gift3 = item.gift_code;
                    IV_001_PC.gift_qty3 = item.gift_qty;

                }
                if (item.discount_count == 4)
                {
                    IV_001_PC.gift4 = item.gift_code;
                    IV_001_PC.gift_qty4 = item.gift_qty;

                }
                if (item.discount_count == 5)
                {
                    IV_001_PC.gift5 = item.gift_code;
                    IV_001_PC.gift_qty5 = item.gift_qty;

                }
               }
            


            if (IV_001_PC.currency_code == "")
            {
                IV_001_PC.currency_code = pubsess.base_currency_code;
            }
            if (photo2 != null)
            {
                if ((photo2 != null && Session["action_flag"].ToString() != "Create") || (Session["action_flag"].ToString() == "Create"))
                {
                    byte[] uploaded = new byte[photo2.InputStream.Length];
                    photo2.InputStream.Read(uploaded, 0, uploaded.Length);
                    IV_001_PC.cus_photo = uploaded;
                }
            }

            if (submit_flag)
            {
                IV_001_PC.approval_level = 0;
                util.update_entry("021", IV_001_PC.contract_id.ToString(), pubsess.userid);
            }
           if(action_flag == "Create")
                db.Entry(IV_001_PC).State = EntityState.Added;
            else
                db.Entry(IV_001_PC).State = EntityState.Modified;

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
                //util.parameter_deleteflag("001", glay.vwstring0);
                //string str = "update AR_001_CTERM set delete_flag ='Y' from AR_001_CTERM a, IV_001_PC b where a.credit_term_code = b.payment_term_code";
                //str += " and customer_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(str);

                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, IV_001_PC b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and customer_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(stri);



                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("PCONTRACT", IV_001_PC.contract_id.ToString(), photo1, glay.vwstrarray9);
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
            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) && string.IsNullOrWhiteSpace(glay.vwstrarray0[4]))
            {
                ModelState.AddModelError(String.Empty, "Please insert Name or select Existing Customer");
                err_flag = false;
            }
            //if (!(string.IsNullOrWhiteSpace(glay.vwstrarray0[0])) && !(string.IsNullOrWhiteSpace(glay.vwstrarray0[4])))
            //{
            //    ModelState.AddModelError(String.Empty, "You can only insert Name or select Existing Customer");
            //    err_flag = false;
            //}
            if (!util.date_validate(glay.vwstrarray0[2]))
            {
                ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
                err_flag = false;
            }


            if (glay.vwdecimal0 <= 0)
            {
                ModelState.AddModelError(String.Empty, "Quantity must be greater than Zero");
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(glay.vwstring0) && move_auto != "Y")
            {
                ModelState.AddModelError(String.Empty, "Customer ID must not be spaces");
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

        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray6 = new string[20];

            glay.vwstring3 = IV_001_PC.contract_id;
            glay.vwstring0 = IV_001_PC.property_id;
            glay.vwstrarray0[1] = IV_001_PC.other_name;
            glay.vwstrarray0[2] = util.date_slash(IV_001_PC.transaction_date);
            glay.vwstrarray0[3] = IV_001_PC.sales_rep;
            glay.vwstrarray0[4] = IV_001_PC.customer_id;
            //glay.vwstring5 = IV_002_PCNT.product;
            glay.vwdecimal6 = IV_001_PC.price;
            glay.vwdecimal3 = IV_001_PC.exp_deposit;
            glay.vwdecimal0 = IV_001_PC.quantity;
            glay.vwdecimal1 = IV_001_PC.sales_com;
            glay.vwdecimal2 = IV_001_PC.sales_val;
            glay.vwint1 = IV_001_PC.tenor;
            glay.vwdecimal4 = IV_001_PC.monthly_amt;
            glay.vwdecimal7 = IV_001_PC.exchange_rate;
            glay.vwstrarray0[0] = IV_001_PC.leeds_name;
            
            glay.vwstrarray0[5] = IV_001_PC.business_type;
            glay.vwstrarray0[6] = IV_001_PC.purpose;
            glay.vwstrarray0[7] = IV_001_PC.sex;
            glay.vwstrarray0[9] = IV_001_PC.address;
            glay.vwstrarray0[10] = IV_001_PC.emp_address;
            glay.vwstrarray0[11] = IV_001_PC.dob;
            glay.vwstrarray0[12] = IV_001_PC.contact_job_title;
            glay.vwstrarray0[14] = IV_001_PC.nationality;
            glay.vwstrarray0[13] = IV_001_PC.employer;
            glay.vwstrarray0[15] = IV_001_PC.kin_name;
            glay.vwstrarray0[16] = IV_001_PC.kin_address;
            glay.vwstrarray0[17] = IV_001_PC.kin_phone;
            glay.vwstrarray0[18] = IV_001_PC.cust_email;
            glay.vwstrarray0[19] = IV_001_PC.cust_phone;
            //glay.vwstrarray0[20] = IV_001_PC.contact_email;
            glay.vwstrarray0[21] = IV_001_PC.marital_status;
            glay.vwstrarray0[8] = IV_001_PC.currency_code;
            glay.vwstrarray0[22] = IV_001_PC.specification;
            glay.vwstrarray0[23] = IV_001_PC.title;
            glay.vwstrarray0[24] = IV_001_PC.selected_promo;
            glay.vwstrarray0[25] = IV_001_PC.termination_charge;
            
            glay.vwstrarray0[20] = IV_001_PC.note;
            glay.vwstrarray6[0] = IV_001_PC.analysis_code1;
            glay.vwstrarray6[1] = IV_001_PC.analysis_code2;
            glay.vwstrarray6[2] = IV_001_PC.analysis_code3;
            glay.vwstrarray6[3] = IV_001_PC.analysis_code4;
            glay.vwstrarray6[4] = IV_001_PC.analysis_code5;
            glay.vwstrarray6[5] = IV_001_PC.analysis_code6;
            glay.vwstrarray6[6] = IV_001_PC.analysis_code7;
            glay.vwstrarray6[7] = IV_001_PC.analysis_code8;
            glay.vwstrarray6[8] = IV_001_PC.analysis_code9;
            glay.vwstrarray6[9] = IV_001_PC.analysis_code10;
            glay.vwdecimal5 = glay.vwdecimal2 - glay.vwdecimal1;

            string curen = glay.vwstrarray0[8];
            glay.vwstring2 = (from bg in db.MC_001_CUREN
                               where bg.currency_code == curen
                               select bg.currency_description).FirstOrDefault();
            if (IV_001_PC.active_status == "Y")
                glay.vwbool1 = true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "CUSTOMER" && bg.document_code == IV_001_PC.customer_id
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
            decimal s_val = 0; decimal n_pyt = 0; //decimal dep_amt = Convert.ToDecimal(dep_amt_paid);
            decimal s_comm = 0; decimal d_amt = 0; decimal m_amt = 0; decimal dprice = 0; decimal dqty = 0;
            decimal bal = 0; string spec = ""; string curen = ""; decimal selected_promo = 0;
            decimal.TryParse(qty, out dqty); decimal n_svl = 0; string promo_name = "No Promo";
            //if (dprice == 0)
            //    dprice = 1;
            if (dqty == 0)
                dqty = 1;
            //int itm = item_code.IndexOf("[]");
            int tenor1 = Convert.ToInt32(item_code);
            
            var curlist = (from bg in db.AR_001_PMTRX
                           join bg1 in db.IV_001_ITEM
                           on new { a1 = bg.item_code } equals new { a1 = bg1.item_code }
                           into bg2
                           from bg3 in bg2.DefaultIfEmpty()
                           join bf in db.DC_001_DISC
                           on new { a1 = bg.selected_promo, a2 = "P", a3 = "FG" } equals new { a1 = bf.discount_code, a2 = bf.stepped_discount_active, a3 = bf.stepped_criteria }
                           into bf1
                           from bf2 in bf1.DefaultIfEmpty()
                           where bg.auto_id == tenor1 
                           select new{bg,bg3,bf2}).FirstOrDefault();
                               //c1 = bg3.agency_comm_flat,
                               //c0 = bg3.agency_comm_per,
                               //c2 = bg.selling_price_class1,
                               //c3 = bg.deposit_flat,
                               //c4 = bg.deposit_percent,
                               //c5 = bg.num_installment,
                               //c6=bg3.specification,
                               //c7 = bg.currency
                           
            if (curlist != null)
            {

                s_val = curlist.bg.selling_price_class1;

                n_svl = s_val * dqty;

                if (curlist.bg.deposit_flat != 0)
                    d_amt = curlist.bg.deposit_flat;
                else
                    d_amt = curlist.bg.deposit_percent / 100 * curlist.bg.selling_price_class1;
               
                d_amt = d_amt * dqty;
               
                m_amt =  curlist.bg.month_price;
               
                    m_amt = m_amt * dqty;


                n_pyt = curlist.bg.tenor;

                if (curlist.bf2 != null)
                {
                    if (curlist.bf2.time_bound == "Y")
                    {
                        string datefrm = curlist.bf2.discount_date_from; string dateto = curlist.bf2.discount_date_to;
                        int datfm = Convert.ToInt32(datefrm); int datto = Convert.ToInt32(dateto);
                        string datenow = DateTime.Now.ToString("yyyyMMdd"); int datnw = Convert.ToInt32(datenow);


                        if (datnw < datto)
                        {
                            promo_name = curlist.bf2.discount_name;
                            if (curlist.bf2.discount_amount != 0)
                                s_comm = curlist.bf2.discount_amount * dqty;
                            else
                                s_comm = (curlist.bf2.discount_percent / 100 * curlist.bg.selling_price_class1) * dqty;
                        }
                        else
                            s_comm = 0;
                    }
                    else
                    {
                        promo_name = curlist.bf2.discount_name;
                        
                        if (curlist.bf2.discount_amount != 0)
                            s_comm = curlist.bf2.discount_amount * dqty;
                        else
                            s_comm = (curlist.bf2.discount_percent / 100 * curlist.bg.selling_price_class1) * dqty;
                    }
                }
              
             
                
                bal = n_svl- s_comm;

                spec = curlist.bg3.specification;

                curen = curlist.bg.currency;
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
            ary.Add(new SelectListItem { Value = "8", Text = spec });
            ary.Add(new SelectListItem { Value = "9", Text = read_cur });
            ary.Add(new SelectListItem { Value = "10", Text = curen });
            ary.Add(new SelectListItem { Value = "14", Text = promo_name });

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
            glay.vwstrarray0[2] = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwstrarray0[28] = "M";
            glay.vwstrarray0[27] = "0";
            glay.vwstrarray0[13] = "Y";
            glay.vwstring1 = "Open";
            glay.vwbool1 = true;
            glay.vwstrarray0[8] = pubsess.base_currency_code;
            glay.vwstring11 = pubsess.base_currency_code;
            glay.vwdecimal0 = 1;
            glay.vwlist0 = new List<querylay>[20];
        }
        private void select_query()
        {
            var empe = (from pf in db.AR_001_PMTRX
                       join bg in db.IV_001_ITEM
                       on new { a1 = pf.item_code } equals new { a1=bg.item_code}
                       into bg1
                       from bg2 in bg1.DefaultIfEmpty()
                       join dr in db.DC_001_DISC
                       on new { a1 = pf.selected_promo } equals new { a1=dr.discount_code}
                       into dr1
                       from dr2 in dr1.DefaultIfEmpty()
                       where pf.active_status == "N" 
                       orderby pf.item_code
                        select new { c1 = pf.auto_id, c2 = dr2.discount_name + "--" + bg2.item_name + "--" + pf.tenor });
            ViewBag.cid = new SelectList(empe.ToList(), "c1", "c2", glay.vwstring0);

            //var temp = from pf in db.AR_001_PMTRX
            //           where pf.active_status == "N"
            //           select new { c1 = pf.tenor, c2 = pf.item_code + "--- " + pf.tenor };
            //ViewBag.tenor = new SelectList(temp.ToList(), "c1", "c2", glay.vwstrarray0[23]);

            var type = from bg in db.GB_999_MSG
                       where bg.type_msg == "MS"
                       select bg;
            ViewBag.ms = new SelectList(type.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[21]);
            
            var typw = from bg in db.GB_999_MSG
                       where bg.type_msg == "TIT"
                       select bg;
            ViewBag.tit = new SelectList(typw.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[23]);
           
            var typr = from bg in db.GB_999_MSG
                       where bg.type_msg == "SEX"
                       select bg;
            ViewBag.sex = new SelectList(typr.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[7]);

            var typ = from bg in db.GB_001_PCODE
                       where bg.parameter_type == "18"
                       select bg;
            ViewBag.pur = new SelectList(typ.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[6]);

            ViewBag.srep = util.para_selectquery("62", glay.vwstrarray0[3], "N");
            ViewBag.cust = util.para_selectquery("001", glay.vwstrarray0[4], "N");

      
            ViewBag.sales = util.para_selectquery("62", glay.vwstrarray0[37]);
           
            
            //string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            //var emp1 = db.Database.SqlQuery<querylay>(str1);

            //ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[8]);

            ViewBag.country = util.para_selectquery("13", glay.vwstrarray0[14], "N");
           
            var bgitemt = from bg in db.GB_999_MSG
                          where bg.type_msg == "BIZT"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.type = new SelectList(bgitemt.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[5]);

            var dis = from bg in db.DC_001_DISC
                      select new { c1 = bg.discount_code, c2 = bg.discount_name };
            ViewBag.promo = new SelectList(dis.Distinct().ToList(), "c1", "c2", glay.vwstrarray0[24]);

            var bgitem = from bg in db.GB_001_PCODE
                          where bg.parameter_type == "19"
                          orderby bg.parameter_name
                          select bg;

            ViewBag.admin = new SelectList(bgitem.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[25]);

            
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
            //delete_record();

            string sqlstr = "delete from [dbo].[IV_001_PC] where contract_id=" + util.sqlquote(id);

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

           //// Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "014" && bg.sequence_no != 99
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
        //        table_name = " IV_001_PC ";
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
                           where bg.id_code == "COYAUTO1"
                           select bg.field12).FirstOrDefault();

            if (autoset == "Y")
                move_auto = "Y";

        }
    }

}
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
    public class T_ownerController : Controller
    {
        IV_002_OST IV_002_OST = new IV_002_OST();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string atype = "cus";
        bool err_flag = true;
        string delmsg = "";
        string move_auto = "N";
        string action_flag = "";
        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            //Session["multi_curenchk"] = pubsess.multi_currency;

            var bglist = from bh in db.IV_002_OST
                         select new vw_genlay
                         {
                             //vwstring0 = bh.property_id,
                            //vwint0 = bh.contract_id,
                             vwstring5 = bh.contract_id,
                             vwdecimal0 = bh.payment_amt
                            // vwstring5 = bh.active_status == "N" ? "Active" : "Inactive"
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
           // cal_auto();
            //psess.temp5 = move_auto;
            if (move_auto == "Y")
                glay.vwstring0 = "AUTO";
            return View(glay);
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
           pubsess = (pubsess)Session["pubsess"];
           psess = (psess)Session["psess"];
           ViewBag.action_flag = "Create";
           action_flag = "Create";
           glay = glay_in;
            photo1 = photofile;
            //cal_auto();
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        public ActionResult Edit(string key1)
        {
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
            // initial_rtn();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            IV_002_OST = db.IV_002_OST.Find(key1);
            if (IV_002_OST != null)
                read_record();
            header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
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
                IV_002_OST = db.IV_002_OST.Find(glay.vwstring0);
                db.IV_002_OST.Remove(IV_002_OST);
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
                IV_002_OST = new IV_002_OST();
                IV_002_OST.created_by = pubsess.userid;
                IV_002_OST.created_date = DateTime.UtcNow;
                IV_002_OST.approval_level = 0;
                IV_002_OST.approval_date = DateTime.UtcNow;
                IV_002_OST.approval_by = pubsess.userid;
                IV_002_OST.contract_id = glay.vwint0.ToString();
           
            }
            else
            {
                string id = glay.vwint0.ToString();
                IV_002_OST = db.IV_002_OST.Find(id);
            }
            string[] proerty = glay.vwstring0.Split('(',')');
            
            IV_002_OST.property = proerty[1];
            IV_002_OST.transaction_date = util.date_yyyymmdd(glay.vwstring2);
            IV_002_OST.payment_amt = glay.vwdecimal3;
            IV_002_OST.sales_com = glay.vwdecimal1;
            IV_002_OST.sales_val = glay.vwdecimal2;
            IV_002_OST.vat = glay.vwdecimal5;
            IV_002_OST.p_status = glay.vwdecimal4;
            IV_002_OST.gift = glay.vwdecimal0;
            IV_002_OST.modified_date = DateTime.UtcNow;
            IV_002_OST.modified_by = pubsess.userid;
            IV_002_OST.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
           
           // IV_002_OST.active_status = glay.vwbool1 ? "Y" : "N";
           
            //IV_002_OST.analysis_code1 = "";
            //IV_002_OST.analysis_code2 = "";
            //IV_002_OST.analysis_code3 = "";
            //IV_002_OST.analysis_code4 = "";
            //IV_002_OST.analysis_code5 = "";
            //IV_002_OST.analysis_code6 = "";
            //IV_002_OST.analysis_code7 = "";
            //IV_002_OST.analysis_code8 = "";
            //IV_002_OST.analysis_code9 = "";
            //IV_002_OST.analysis_code10 = "";

            //int arrlen = glay.vwstrarray6.Length;
            //if (arrlen > 0)
            //    IV_002_OST.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
            //if (arrlen > 1)
            //    IV_002_OST.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
            //if (arrlen > 2)
            //    IV_002_OST.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
            //if (arrlen > 3)
            //    IV_002_OST.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
            //if (arrlen > 4)
            //    IV_002_OST.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
            //if (arrlen > 5)
            //    IV_002_OST.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
            //if (arrlen > 6)
            //    IV_002_OST.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
            //if (arrlen > 7)
            //    IV_002_OST.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
            //if (arrlen > 8)
            //    IV_002_OST.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
            //if (arrlen > 9)
            //    IV_002_OST.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
            //psess.intemp0 = arrlen;
            
           if(action_flag == "Create")
                db.Entry(IV_002_OST).State = EntityState.Added;
            else
                db.Entry(IV_002_OST).State = EntityState.Modified;

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
                //string str = "update AR_001_CTERM set delete_flag ='Y' from AR_001_CTERM a, IV_002_OST b where a.credit_term_code = b.payment_term_code";
                //str += " and customer_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(str);

                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, IV_002_OST b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and customer_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(stri);



                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("T", IV_002_OST.contract_id.ToString(), photo1, glay.vwstrarray9);
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

        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray6 = new string[20];

            glay.vwstring5 = IV_002_OST.contract_id;
            glay.vwint0 = Convert.ToInt32(IV_002_OST.contract_id);
            glay.vwstring0 = IV_002_OST.property;
            //glay.vwstrarray0[1] = IV_002_OST.contract_type;
            glay.vwstring2 = util.date_slash(IV_002_OST.transaction_date);
           // glay.vwstrarray0[3] = IV_002_OST.sales_rep;
           // glay.vwstrarray0[4] = IV_002_OST.customer_id;
            glay.vwdecimal0 = IV_002_OST.gift;
            glay.vwdecimal1 = IV_002_OST.sales_com;
            glay.vwdecimal2 = IV_002_OST.sales_val;
            glay.vwdecimal3 = IV_002_OST.payment_amt;
            glay.vwdecimal5 = IV_002_OST.vat;
            glay.vwdecimal4 = IV_002_OST.p_status;
            
                //glay.vwstrarray0[23] = IV_002_OST.tenor;
            //glay.vwstrarray0[8] = IV_002_OST.currency_code;
            glay.vwstring4 = IV_002_OST.note;
            //glay.vwstrarray6[0] = IV_002_OST.analysis_code1;
            //glay.vwstrarray6[1] = IV_002_OST.analysis_code2;
            //glay.vwstrarray6[2] = IV_002_OST.analysis_code3;
            //glay.vwstrarray6[3] = IV_002_OST.analysis_code4;
            //glay.vwstrarray6[4] = IV_002_OST.analysis_code5;
            //glay.vwstrarray6[5] = IV_002_OST.analysis_code6;
            //glay.vwstrarray6[6] = IV_002_OST.analysis_code7;
            //glay.vwstrarray6[7] = IV_002_OST.analysis_code8;
            //glay.vwstrarray6[8] = IV_002_OST.analysis_code9;
            //glay.vwstrarray6[9] = IV_002_OST.analysis_code10;

           
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "CUSTOMER" && bg.document_code == IV_002_OST.contract_id.ToString()
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();


        }

        [HttpPost]
        public ActionResult get_commision_value(string item_code)
        {
            // write your query statement
            ModelState.Remove("glay.vwdecimal1");
            ModelState.Remove("glay.vwstring0");
            // tdate = util.date_yyyymmdd(tdate);
            decimal s_val = 0; decimal n_pyt = 0; string spec = ""; string id_prop=""; decimal bal=0;
            decimal s_comm = 0; decimal d_amt = 0; decimal m_amt = 0; decimal dprice = 0; decimal dqty = 1;
            string stus = "";// decimal.TryParse(price, out dprice);  
            decimal n_svl = 0;
            //decimal.TryParse(qty, out dqty);
            //if (dprice == 0)
            //    dprice = 1;
            //int itmc = Convert.ToInt32(item_code);
            
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
                               c11 = bg3.owner_transfer
                           }).FirstOrDefault();
            if (curlist != null)
            {
                s_comm = curlist.c1;

                s_val = curlist.c0;

                d_amt = curlist.c3*((curlist.c11/100)*curlist.c5);

                stus = curlist.c11 == 100 ? "Complete" : "Incomplete";

                n_pyt = curlist.c5;

                n_svl = curlist.c6;

                bal = n_svl - d_amt;

                spec = curlist.c7;

               // curen = curlist.c9;

                id_prop = "(" + curlist.c10 + "/" + curlist.c5 + ") - " + curlist.c7;
            }
            //string read_cur = (from bg in db.MC_001_CUREN
            //                   where bg.currency_code == curen
            //                   select bg.currency_description).FirstOrDefault();

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = s_comm.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "2", Text = s_val.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "3", Text = d_amt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "4", Text = stus });
            ary.Add(new SelectListItem { Value = "5", Text = n_pyt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "6", Text = n_svl.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "7", Text = bal.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "8", Text = id_prop });
            //ary.Add(new SelectListItem { Value = "9", Text = read_cur });
            //ary.Add(new SelectListItem { Value = "10", Text = curen });
            ary.Add(new SelectListItem { Value = "11", Text = curlist.c8 });

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
            glay.vwstring11 = pubsess.base_currency_code;
            glay.vwlist0 = new List<querylay>[20];
        }
        private void select_query()
        {
            //var empe = from pf in db.IV_001_ITEM
            //           where pf.active_status == "N" && pf.sales == "Y" && pf.item_type == "P"
            //           orderby pf.item_code, pf.item_name
            //           select new { c1 = pf.item_code, c2 = pf.item_code + "--- " + pf.item_name };
            //ViewBag.pid = new SelectList(empe.ToList(), "c1", "c2", glay.vwstring0);
           
            //var empb = from pf in db.BK_001_BANK
            //           where pf.active_status == "N" 
            //           orderby pf.bank_code, pf.bank_name
            //           select new { c1 = pf.bank_code, c2 = pf.bank_code + "--- " + pf.bank_name };
            //ViewBag.bank = new SelectList(empb.ToList(), "c1", "c2", glay.vwstring1);

            get_contract();
           
           
            //var bgitemt = from bg in db.GB_999_MSG
            //              where bg.type_msg == "BIZT"
            //              orderby bg.name1_msg
            //              select bg;

            //ViewBag.type = new SelectList(bgitemt.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[5]);

            
        }
        private void get_contract()
        {

            string str = "select contract_id query0, leeds_name+ '-' + contract_id query1 from [dbo].[IV_001_PC] where leeds_name <> '' union all";
            str += " select contract_id query0, cust_biz_name+ '-' + contract_id query1 from [dbo].[IV_001_PC], [dbo].[AR_001_CUSTM] where customer_id = customer_code";
            var emp1 = db.Database.SqlQuery<querylay>(str);
            ViewBag.cid = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring1);

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
                         where bg.header_type_code == "001" && bg.sequence_no != 99
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
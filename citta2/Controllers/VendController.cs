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
    public class VendController : Controller
    {
        AP_001_VENDR AP_001_VENDR = new AP_001_VENDR();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string atype = "ved";
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

            var bglist = from bh in db.AP_001_VENDR
                         join bk in db.GB_999_MSG
                         on new { a1 = bh.business_type, a2 = "BIZT" } equals new { a1 = bk.code_msg, a2 = bk.type_msg }
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.vendor_code,
                             vwstring1 = bh.vend_biz_name,   
                             vwstring2 = bk2.name1_msg,
                             vwstring3 = bh.biz_reg_number,
                             vwstring4 = bh.gl_account_code,
                             vwstring5 = bh.active_status == "N" ? "Active" : "Inactive"
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
            cal_auto();
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

            psess = (psess)Session["psess"];
            pubsess = (pubsess)Session["pubsess"];
            action_flag = "Edit";
            ViewBag.action_flag = action_flag;

            initial_rtn();
            psess.temp5 = move_auto;
            Session["psess"] = psess;
            
            AP_001_VENDR = db.AP_001_VENDR.Find(key1);
            if (AP_001_VENDR != null)
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
            ViewBag.action_flag = action_flag;

            glay = glay_in;

            if (id_xhrt=="D")
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
            if (util.delete_check("VENDR", glay.vwstring0))
            {
                AP_001_VENDR = db.AP_001_VENDR.Find(glay.vwstring0);
                db.AP_001_VENDR.Remove(AP_001_VENDR);
                db.SaveChanges();
            }
            else
            {
                delmsg = "Vendor in Use";
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
            if (action_flag=="Create")
            {
                AP_001_VENDR = new AP_001_VENDR();
                AP_001_VENDR.created_by = pubsess.userid;
                AP_001_VENDR.created_date = DateTime.UtcNow;
                AP_001_VENDR.delete_flag = "N";
                if (move_auto == "Y")
                    glay.vwstring0 = util.autogen_num("VEN");
            }
            else
            {
                AP_001_VENDR = db.AP_001_VENDR.Find(glay.vwstring0);
            }
            AP_001_VENDR.attach_document = "";
            if (glay.vwstrarray0[20] == "N")
            {
                glay.vwstrarray0[21] = "";
                glay.vwdecimal0 = 0;

            }
           
            AP_001_VENDR.vendor_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            AP_001_VENDR.vend_biz_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            AP_001_VENDR.vend_address1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            AP_001_VENDR.vend_address2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            AP_001_VENDR.business_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            AP_001_VENDR.tax_reg_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            AP_001_VENDR.biz_reg_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
            AP_001_VENDR.credit_limit_amt = glay.vwdecimal0;
            AP_001_VENDR.relationship_mgr = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
            AP_001_VENDR.vend_city = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            AP_001_VENDR.vend_postal_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
            AP_001_VENDR.vend_country = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
            AP_001_VENDR.vend_state = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
            AP_001_VENDR.statement = glay.vwbool0 ? "Y" : "N";
            AP_001_VENDR.vend_website = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
            AP_001_VENDR.vend_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
            AP_001_VENDR.vend_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
            AP_001_VENDR.contact_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
            AP_001_VENDR.contact_dept = string.IsNullOrWhiteSpace(glay.vwstrarray0[15]) ? "" : glay.vwstrarray0[15];
            AP_001_VENDR.contact_job_title = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
            AP_001_VENDR.contact_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[17]) ? "" : glay.vwstrarray0[17];
            AP_001_VENDR.contact_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[18]) ? "" : glay.vwstrarray0[18];
            //AP_001_VENDR.contact_country = string.IsNullOrWhiteSpace(glay.vwstrarray0[19]) ? "" : glay.vwstrarray0[19];
            AP_001_VENDR.currency_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[19]) ? "" : glay.vwstrarray0[19];
            AP_001_VENDR.credit_facilities = string.IsNullOrWhiteSpace(glay.vwstrarray0[20]) ? "" : glay.vwstrarray0[20];
            AP_001_VENDR.payment_term_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[21]) ? "" : glay.vwstrarray0[21];
            AP_001_VENDR.gl_account_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[22]) ? "" : glay.vwstrarray0[22];
            AP_001_VENDR.modified_date = DateTime.UtcNow;
            AP_001_VENDR.modified_by = pubsess.userid;
            AP_001_VENDR.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[23]) ? "" : glay.vwstrarray0[23];
            AP_001_VENDR.active_status = glay.vwbool1 ? "Y" : "N";
            AP_001_VENDR.branch_same_loc = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;

            AP_001_VENDR.analysis_code1 = "";
            AP_001_VENDR.analysis_code2 = "";
            AP_001_VENDR.analysis_code3 = "";
            AP_001_VENDR.analysis_code4 = "";
            AP_001_VENDR.analysis_code5 = "";
            AP_001_VENDR.analysis_code6 = "";
            AP_001_VENDR.analysis_code7 = "";
            AP_001_VENDR.analysis_code8 = "";
            AP_001_VENDR.analysis_code9 = "";
            AP_001_VENDR.analysis_code10 = "";

            int arrlen = glay.vwstrarray6.Length;
            if (arrlen > 0)
                AP_001_VENDR.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
            if (arrlen > 1)
                AP_001_VENDR.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
            if (arrlen > 2)
                AP_001_VENDR.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
            if (arrlen > 3)
                AP_001_VENDR.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
            if (arrlen > 4)
                AP_001_VENDR.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
            if (arrlen > 5)
                AP_001_VENDR.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
            if (arrlen > 6)
                AP_001_VENDR.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
            if (arrlen > 7)
                AP_001_VENDR.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
            if (arrlen > 8)
                AP_001_VENDR.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
            if (arrlen > 9)
                AP_001_VENDR.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
            psess.intemp0 = arrlen;
            Session["psess"] = psess;
        
            if (AP_001_VENDR.currency_code == "")
            {
                AP_001_VENDR.currency_code = pubsess.base_currency_code;
            }

            if (action_flag == "Create")  
                db.Entry(AP_001_VENDR).State = EntityState.Added;
            else 
                db.Entry(AP_001_VENDR).State = EntityState.Modified;

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
                util.parameter_deleteflag("002", glay.vwstring0);
                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, AP_001_VENDR b where a.account_code = b.gl_account_code";
                //str += " and vendor_code =" + util.sqlquote(glay.vwstring0);
                ////db.Database.ExecuteSqlCommand(str);
                ////db.Database.ExecuteSqlCommand(str);

                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, AP_001_VENDR b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and vendor_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(stri);
              
                {
                    util.write_document("VENDOR", AP_001_VENDR.vendor_code, photo1, glay.vwstrarray9);
                }

            }

            if (err_flag)
            {
                if (glay.vwstring2 == "Y")
                {
                    AR_001_DADRS gadd = new AR_001_DADRS();
                    gadd.address_type = "VENDOR";
                    gadd.customer_code = glay.vwstring0;
                    gadd.address_code = 0;
                    gadd.address1 = glay.vwstrarray0[1];
                    gadd.address2 = glay.vwstrarray0[2];
                    gadd.city = glay.vwstrarray0[7];
                    gadd.postal_code = glay.vwstrarray0[8];
                    gadd.country = glay.vwstrarray0[9];
                    gadd.state = glay.vwstrarray0[10];
                    gadd.email = glay.vwstrarray0[12];
                    gadd.phone = glay.vwstrarray0[13];
                    gadd.contact_name = glay.vwstrarray0[14];
                    gadd.contact_dept = glay.vwstrarray0[15];
                    gadd.contact_email = glay.vwstrarray0[17];
                    gadd.contact_phone = glay.vwstrarray0[18];
                    gadd.contact_job_title = glay.vwstrarray0[16];
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
            string error_msg="";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            if (string.IsNullOrWhiteSpace(glay.vwstring0) && move_auto != "Y")
            {
                ModelState.AddModelError(String.Empty, "Please enter Vendor ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[0]))
            {
                ModelState.AddModelError(String.Empty, "Please enter Business name");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[19]))
            {
                ModelState.AddModelError(String.Empty, "Please select currency ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[22]))
            {
                ModelState.AddModelError(String.Empty, "Please select GL account");
                err_flag = false;
            }

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Name must not be spaces";

            if (action_flag == "Create")
            {
                AP_001_VENDR bnk = db.AP_001_VENDR.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Vendor ID already exist");
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
         
            string sqlstr = "select '1' query0 from AP_001_VENDR where vend_biz_name=" + util.sqlquote(glay.vwstrarray0[0]) + " and vendor_code <> " + util.sqlquote(glay.vwstring0);
            var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (bglist1 != null)
            {
                ModelState.AddModelError(String.Empty, "Can not accept duplicate Business Name");
                err_flag = false;
            }
        }

        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray6 = new string[20];

            glay.vwstring0 = AP_001_VENDR.vendor_code;
            glay.vwstrarray0[0] = AP_001_VENDR.vend_biz_name;
            glay.vwstrarray0[1] = AP_001_VENDR.vend_address1;
            glay.vwstrarray0[2] = AP_001_VENDR.vend_address2;
            glay.vwstrarray0[3] = AP_001_VENDR.business_type;
            glay.vwstrarray0[4] = AP_001_VENDR.tax_reg_number;
            glay.vwstrarray0[5] = AP_001_VENDR.biz_reg_number;
            glay.vwstrarray0[6] = AP_001_VENDR.relationship_mgr;
            glay.vwstrarray0[7] = AP_001_VENDR.vend_city;
            glay.vwstring2 = "N";
            glay.vwdecimal0 = AP_001_VENDR.credit_limit_amt;
            glay.vwstrarray0[8] = AP_001_VENDR.vend_postal_code;
            glay.vwstrarray0[9] = AP_001_VENDR.vend_country;
            glay.vwstrarray0[10] = AP_001_VENDR.vend_state;
            glay.vwstrarray0[11] = AP_001_VENDR.vend_website;
            glay.vwstrarray0[12] = AP_001_VENDR.vend_email;
            glay.vwstrarray0[13] = AP_001_VENDR.vend_phone;
            glay.vwstrarray0[14] = AP_001_VENDR.contact_name;
            glay.vwstrarray0[15] = AP_001_VENDR.contact_dept;
            glay.vwstrarray0[16] = AP_001_VENDR.contact_job_title;
            glay.vwstrarray0[17] = AP_001_VENDR.contact_email;
            glay.vwstrarray0[18] = AP_001_VENDR.contact_phone;
            //glay.vwstrarray0[19] = AP_001_VENDR.contact_country;
            glay.vwstrarray0[19] = AP_001_VENDR.currency_code;
            glay.vwstrarray0[20] = AP_001_VENDR.credit_facilities;
            glay.vwstrarray0[21] = AP_001_VENDR.payment_term_code;
            glay.vwstrarray0[22] = AP_001_VENDR.gl_account_code;
            glay.vwstrarray0[23] = AP_001_VENDR.note;
            glay.vwstrarray6[0] = AP_001_VENDR.analysis_code1;
            glay.vwstrarray6[1] = AP_001_VENDR.analysis_code2;
            glay.vwstrarray6[2] = AP_001_VENDR.analysis_code3;
            glay.vwstrarray6[3] = AP_001_VENDR.analysis_code4;
            glay.vwstrarray6[4] = AP_001_VENDR.analysis_code5;
            glay.vwstrarray6[5] = AP_001_VENDR.analysis_code6;
            glay.vwstrarray6[6] = AP_001_VENDR.analysis_code7;
            glay.vwstrarray6[7] = AP_001_VENDR.analysis_code8;
            glay.vwstrarray6[8] = AP_001_VENDR.analysis_code9;
            glay.vwstrarray6[9] = AP_001_VENDR.analysis_code10;

            if (AP_001_VENDR.statement == "Y")
                glay.vwbool0 = true;
            if (AP_001_VENDR.active_status == "Y")
                glay.vwbool1 = true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "VENDOR" && bg.document_code ==AP_001_VENDR.vendor_code
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
            glay.vwstrarray0[20] = "N";
            glay.vwstring2 = "Y";
            glay.vwstring11 = pubsess.multi_currency;
            glay.vwstrarray0[19] = pubsess.base_currency_code;
            glay.vwlist0 = new List<querylay>[20];
        
        }
        private void select_query()
        {
            ViewBag.gl_acc = util.read_ledger("018", glay.vwstrarray0[22]);
            //var bg2 = util.read_ledger("018");
            //ViewBag.gl_acc = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[22]);

            var bglist = from bg in db.AP_001_PTERM
                         where bg.active_status == "N"
                         orderby bg.description
                         select bg;

            ViewBag.payment = new SelectList(bglist.ToList(), "payment_term_code", "description", glay.vwstrarray0[21]);

            ViewBag.manager = util.para_selectquery("62", glay.vwstrarray0[6]);
            //ViewBag.manager = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[6]);

            //var bglistu = from bg in db.GB_001_EMP
            //              where bg.active_status == "N"
            //              orderby bg.name
            //             select bg;

            //ViewBag.manager = new SelectList(bglistu.ToList(), "employee_code", "name", glay.vwstrarray0[6]);
            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[19]);

            //var bglistc = from bg in db.MC_001_CUREN
            //              where bg.active_status == "N"
            //              orderby bg.currency_description
            //             select bg;

            //ViewBag.currency = new SelectList(bglistc.ToList(), "currency_code", "currency_description", glay.vwstrarray0[19]);
            ViewBag.country = util.para_selectquery("13", glay.vwstrarray0[9],"N");
            //var bgitem = from bg in db.GB_001_PCODE
            //             where bg.parameter_type == "13" && bg.active_status == "N"
            //             orderby bg.parameter_name
            //             select bg;

            //ViewBag.country = new SelectList(bgitem.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[9]);

            //var bgite = from bg in db.GB_001_PCODE
            //             where bg.parameter_type == "13" && bg.active_status == "N"
            //            orderby bg.parameter_name
            //             select bg;

            //ViewBag.country1 = new SelectList(bgite.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[19]);
            ViewBag.state = util.para_selectquery("14", glay.vwstrarray0[10],"N");

            ViewBag.job = util.para_selectquery("04", glay.vwstrarray0[16]);

            ViewBag.department = util.para_selectquery("05", glay.vwstrarray0[15]);

            var bgitemi = from bg in db.GB_999_MSG
                          where bg.type_msg == "BIZT"
                          orderby bg.name1_msg
                         select bg;

            ViewBag.type = new SelectList(bgitemi.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[3]);
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

             psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "018" && bg.sequence_no != 99
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
                           select bg.field7).FirstOrDefault();

            if (autoset == "Y")
                move_auto = "Y";

        }
    }

}
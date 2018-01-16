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
    public class BankController : Controller
    {
        BK_001_BANK BK_001_BANK = new BK_001_BANK();
        MainContext db = new MainContext();
        MainContext db1 = new MainContext();
        vw_genlay glay = new vw_genlay();
        querylay qlay = new querylay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        bool err_flag = true;
        string action_flag = "";
        //
        // GET: /Citta/

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];

            psess.sarrayt0 = new string[20];
            psess.sarrayt1 = new string[20];
            Session["psess"] = psess;
           
            //ession["multi_curenchk"] = pubsess.multi_currency;

            var bglist = from bh in db.BK_001_BANK
                         join bk1 in db.GB_999_MSG
                         on new{a1 = "PO", a2 = bh.default_payment_option } equals new { a1 = bk1.type_msg, a2 = bk1.code_msg }
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.bank_code,
                             vwstring1 = bh.bank_name,
                             vwstring2 = bk3.name1_msg,
                             vwstring3 = bh.active_status == "N" ? "Active" : "Inactive"
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
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            
           
            BK_001_BANK = db.BK_001_BANK.Find(key1);
            if (BK_001_BANK != null)
                read_record();

            header_ana();
            select_query();
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

        //public ActionResult Index1(string id_xhrt)
        //{
        //    if (id_xhrt == "D")
        //    {
        //        delete_record();
        //        return RedirectToAction("Index");
        //    }

        //    update_file();
        //    if (err_flag)
        //        return RedirectToAction("Index");
        //    return View(glay);

        //}
        private void delete_record()
        {
            BK_001_BANK = db.BK_001_BANK.Find(glay.vwstring0);
            if (BK_001_BANK != null)
            {
                db.BK_001_BANK.Remove(BK_001_BANK);
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
                BK_001_BANK = new BK_001_BANK();
                BK_001_BANK.created_by = pubsess.userid;
                BK_001_BANK.created_date = DateTime.UtcNow;
                BK_001_BANK.delete_flag = "N";
            }
            else
            {
                BK_001_BANK = db.BK_001_BANK.Find(glay.vwstring0);
            }
            BK_001_BANK.attach_document = "";
            BK_001_BANK.bank_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            BK_001_BANK.bank_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            BK_001_BANK.branch = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            BK_001_BANK.bank_acc_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
            BK_001_BANK.address1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            BK_001_BANK.address2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            BK_001_BANK.city = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            BK_001_BANK.postal_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
            BK_001_BANK.country = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
            BK_001_BANK.state = string.IsNullOrWhiteSpace(glay.vwstrarray0[19]) ? "" : glay.vwstrarray0[19];
            BK_001_BANK.email = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            BK_001_BANK.website = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
            BK_001_BANK.phone_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
            BK_001_BANK.default_payment_option = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
            BK_001_BANK.relationship_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
            BK_001_BANK.relationship_job_title = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
            BK_001_BANK.relationship_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
            BK_001_BANK.relationship_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[15]) ? "" : glay.vwstrarray0[15];
            BK_001_BANK.currency_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
            BK_001_BANK.gl_account_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[17]) ? "" : glay.vwstrarray0[17];
            BK_001_BANK.modified_date = DateTime.UtcNow;
            BK_001_BANK.modified_by = pubsess.userid;
            BK_001_BANK.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[18]) ? "" : glay.vwstrarray0[18];
            BK_001_BANK.active_status = glay.vwbool0 ? "Y" : "N";
            BK_001_BANK.analysis_code1 = "";
            BK_001_BANK.analysis_code2 = "";
            BK_001_BANK.analysis_code3 = "";
            BK_001_BANK.analysis_code4 = "";
            BK_001_BANK.analysis_code5 = "";
            BK_001_BANK.analysis_code6 = "";
            BK_001_BANK.analysis_code7 = "";
            BK_001_BANK.analysis_code8 = "";
            BK_001_BANK.analysis_code9 = "";
            BK_001_BANK.analysis_code10 = "";

            int arrlen = glay.vwstrarray6.Length;
            if (arrlen > 0)
                BK_001_BANK.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
            if (arrlen > 1)
                BK_001_BANK.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
            if (arrlen > 2)
                BK_001_BANK.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
            if (arrlen > 3)
                BK_001_BANK.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
            if (arrlen > 4)
                BK_001_BANK.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
            if (arrlen > 5)
                BK_001_BANK.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
            if (arrlen > 6)
                BK_001_BANK.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
            if (arrlen > 7)
                BK_001_BANK.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
            if (arrlen > 8)
                BK_001_BANK.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
            if (arrlen > 9)
                BK_001_BANK.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
            if (BK_001_BANK.currency_code == "")
            {
                BK_001_BANK.currency_code = pubsess.base_currency_code;
            }

           if(action_flag == "Create")
                db.Entry(BK_001_BANK).State = EntityState.Added;
            else
                db.Entry(BK_001_BANK).State = EntityState.Modified;

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
                util.parameter_deleteflag("004", glay.vwstring0);

                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, BK_001_BANK b where a.account_code = b.gl_account_code";
                //    str += " and bank_code =" + util.sqlquote(glay.vwstring0);
                //    db.Database.ExecuteSqlCommand(str);
                //    //db.Database.ExecuteSqlCommand(str);

                // string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, BK_001_BANK b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and bank_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(stri);
              
                    }
                
               
               //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("BANK", BK_001_BANK.bank_code, photo1, glay.vwstrarray9);

                }

            }

        

        private void validation_routine()
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            
            if (string.IsNullOrWhiteSpace(glay.vwstring0)) 
            {
                ModelState.AddModelError(String.Empty, "Bank Id must not be spaces");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                BK_001_BANK bnk = db.BK_001_BANK.Find(glay.vwstring0);
                if (bnk != null) 
                {
                ModelState.AddModelError(String.Empty, "Can not accept duplicates");
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
            

            glay.vwstring0 = BK_001_BANK.bank_code;
            glay.vwstrarray0[0] = BK_001_BANK.bank_name;
            glay.vwstrarray0[1] = BK_001_BANK.branch;
            glay.vwstrarray0[2] = BK_001_BANK.address1;
            glay.vwstrarray0[3] = BK_001_BANK.address2;
            glay.vwstrarray0[4] = BK_001_BANK.city;
            glay.vwstrarray0[5] = BK_001_BANK.postal_code;
            glay.vwstrarray0[6] = BK_001_BANK.country;
            glay.vwstrarray0[7] = BK_001_BANK.email;
            glay.vwstrarray0[8] = BK_001_BANK.website;
            glay.vwstrarray0[9] = BK_001_BANK.phone_number;
            glay.vwstrarray0[10] = BK_001_BANK.bank_acc_number;
            glay.vwstrarray0[11] = BK_001_BANK.default_payment_option;
            glay.vwstrarray0[12] = BK_001_BANK.relationship_name;
            glay.vwstrarray0[13] = BK_001_BANK.relationship_job_title;
            glay.vwstrarray0[14] = BK_001_BANK.relationship_email;
            glay.vwstrarray0[15] = BK_001_BANK.relationship_phone;
            glay.vwstrarray0[16] = BK_001_BANK.currency_code;
            glay.vwstrarray0[19] = BK_001_BANK.state;
            glay.vwstrarray0[17] = BK_001_BANK.gl_account_code;
            glay.vwstrarray0[18] = BK_001_BANK.note;
            glay.vwstrarray6[0] = BK_001_BANK.analysis_code1;
            glay.vwstrarray6[1] = BK_001_BANK.analysis_code2;
            glay.vwstrarray6[2] = BK_001_BANK.analysis_code3;
            glay.vwstrarray6[3] = BK_001_BANK.analysis_code4;
            glay.vwstrarray6[4] = BK_001_BANK.analysis_code5;
            glay.vwstrarray6[5] = BK_001_BANK.analysis_code6;
            glay.vwstrarray6[6] = BK_001_BANK.analysis_code7;
            glay.vwstrarray6[7] = BK_001_BANK.analysis_code8;
            glay.vwstrarray6[8] = BK_001_BANK.analysis_code9;
            glay.vwstrarray6[9] = BK_001_BANK.analysis_code10;

            if (BK_001_BANK.active_status == "Y")
                glay.vwbool0 = true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "BANK" && bg.document_code == BK_001_BANK.bank_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();

        }


        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstring11 = pubsess.multi_currency;
            glay.vwstrarray0[16] = pubsess.base_currency_code;
            glay.vwlist0 = new List<querylay>[20];


        }
        private void select_query()
        {
            ViewBag.country = util.para_selectquery("13", glay.vwstrarray0[6], "N");

            //var bglist = from bg in db.GB_001_PCODE
            //             where bg.parameter_type == "13" && bg.active_status == "N"
            //             orderby bg.parameter_name
            //             select bg;

            //ViewBag.country = new SelectList(bglist.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[6]);
            ViewBag.job = util.para_selectquery("04", glay.vwstrarray0[13]);
            //var bglis = from bg in db.GB_001_PCODE
            //             where bg.parameter_type == "04" && bg.active_status == "N"
            //            orderby bg.parameter_name
            //             select bg;

            //ViewBag.job = new SelectList(bglis.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[13]);
            ViewBag.state = util.para_selectquery("14", glay.vwstrarray0[19]);
            //var bgitem = from bg in db.GB_001_PCODE
            //             where bg.parameter_type == "14" && bg.active_status == "N"
            //             orderby bg.parameter_name
            //             select bg;

            //ViewBag.state = new SelectList(bgitem.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[19]);

            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[16]);

            //var bgitemi = from bg in db.MC_001_CUREN
            //              where bg.active_status == "N"
            //              orderby bg.currency_description
            //              select bg;

            //ViewBag.currency = new SelectList(bgitemi.ToList(), "currency_code", "currency_description", glay.vwstrarray0[16]);

            var bgiteme = from bg in db.GB_999_MSG
                          where bg.type_msg == "PO"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.option = new SelectList(bgiteme.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[11]);


            ViewBag.ledger = util.read_ledger("006", glay.vwstrarray0[17]);
           //var bg2 = util.read_ledger("006");
           // ViewBag.ledger = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[17]);
            
           
        }

        private void error_message()
        {

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

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement

            string sqlstr = "delete from [dbo].[BK_001_BANK] where bank_code=" + util.sqlquote(id);

             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }


        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

            SelectList[] head_det = new SelectList[20];

           //// Session["head_det"] = head_det;
           // //Session["aheader7"] = aheader7;
           // psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "020" && bg.sequence_no != 99
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
                    //var bglist3 = from bg in db.GB_001_DANAL
                    //              where bg.header_sequence == item.header_code
                      //            select bg;
                    //glay.vwlist0[count2] = new SelectList(bglist3.ToList(), "analysis_code", "analysis_description", glay.vwstrarray6[count2]);
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
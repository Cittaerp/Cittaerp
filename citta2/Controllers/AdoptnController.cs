using anchor1.Filters;
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
    public class AdoptnController : Controller
    {
        AP_002_ADOEN AP_002_ADOEN = new AP_002_ADOEN();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase [] photo1;

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
            

            var bglist = from bh in db.AP_002_ADOEN
                         select new vw_genlay
                         {
                             vwint0 = bh.journal_number,
                             vwint1 = bh.sequence_number,
                             vwstring0 = bh.account_code,
                             vwstring1 = bh.reference_number,
                             vwdecimal0 = bh.amount,
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
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
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
        public ActionResult Edit(int key1, int key2)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            AP_002_ADOEN = db.AP_002_ADOEN.Find(key1, key2);
            if (AP_002_ADOEN != null)
                read_record();

            
            header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
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

        private void delete_record()
        {
            AP_002_ADOEN = db.AP_002_ADOEN.Find(glay.vwint0,glay.vwint1);
            if (AP_002_ADOEN != null)
            {
                db.AP_002_ADOEN.Remove(AP_002_ADOEN);
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
                AP_002_ADOEN = new AP_002_ADOEN();
                AP_002_ADOEN.created_by = pubsess.userid;
                AP_002_ADOEN.created_date = DateTime.UtcNow;
            }
            else
            {
                AP_002_ADOEN = db.AP_002_ADOEN.Find(glay.vwstring0);
            }
            AP_002_ADOEN.journal_number = glay.vwint0;
            AP_002_ADOEN.account_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            AP_002_ADOEN.intercoy_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            AP_002_ADOEN.description = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            AP_002_ADOEN.source_identifier = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            AP_002_ADOEN.transaction_date = util.date_yyyymmdd(glay.vwstrarray0[11]);
            AP_002_ADOEN.period = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            AP_002_ADOEN.year = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
            AP_002_ADOEN.exchange_rate = glay.vwdecimal0;
            AP_002_ADOEN.currency_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
            AP_002_ADOEN.reference_number_detail = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            AP_002_ADOEN.reference_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
            AP_002_ADOEN.amount = glay.vwdecimal1;
            AP_002_ADOEN.base_amount = glay.vwdecimal2;
            AP_002_ADOEN.debit_credit_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
            AP_002_ADOEN.modified_date = DateTime.UtcNow;
            AP_002_ADOEN.modified_by = pubsess.userid;
            AP_002_ADOEN.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
            AP_002_ADOEN.project_code = "";

            AP_002_ADOEN.analysis_code1 = "";
            AP_002_ADOEN.analysis_code2 = "";
            AP_002_ADOEN.analysis_code3 = "";
            AP_002_ADOEN.analysis_code4 = "";
            AP_002_ADOEN.analysis_code5 = "";
            AP_002_ADOEN.analysis_code6 = "";
            AP_002_ADOEN.analysis_code7 = "";
            AP_002_ADOEN.analysis_code8 = "";
            AP_002_ADOEN.analysis_code9 = "";
            AP_002_ADOEN.analysis_code10 = "";
            if (glay.vwstrarray6 != null)
            {
                int arrlen = glay.vwstrarray6.Length;
                if (arrlen > 0)
                    AP_002_ADOEN.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                if (arrlen > 1)
                    AP_002_ADOEN.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                if (arrlen > 2)
                    AP_002_ADOEN.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                if (arrlen > 3)
                    AP_002_ADOEN.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                if (arrlen > 4)
                    AP_002_ADOEN.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                if (arrlen > 5)
                    AP_002_ADOEN.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                if (arrlen > 6)
                    AP_002_ADOEN.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                if (arrlen > 7)
                    AP_002_ADOEN.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                if (arrlen > 8)
                    AP_002_ADOEN.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                if (arrlen > 9)
                    AP_002_ADOEN.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                psess.intemp0 = arrlen;
            }

           if(action_flag == "Create")
                db.Entry(AP_002_ADOEN).State = EntityState.Added;
            else
                db.Entry(AP_002_ADOEN).State = EntityState.Modified;

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
                string lstval = "select top 1  sequence_number intquery0 from AP_002_ADOEN ORDER BY sequence_number DESC ";
                var lstval1 = db.Database.SqlQuery<querylay>(lstval).FirstOrDefault();
                int lstval2 = lstval1.intquery0;
                string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, AP_002_ADOEN b where header_sequence in (analysis_code1";
                stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                stri += " and sequence_number =" + lstval2;
                //db.Database.ExecuteSqlCommand(stri);

               if(action_flag == "Create")
                {
                    util.write_document("ADOEN", AP_002_ADOEN.account_code, photo1, glay.vwstrarray9);
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
            
            DateTime date_chk = DateTime.Now;
            DateTime invaliddate = new DateTime(1000, 01, 01);
            if (!util.date_validate(glay.vwstrarray0[11]))
            {
                ModelState.AddModelError(String.Empty, "Please insert a valid date");
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
         
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //    error_msg = "Please enter ID";

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Name must not be spaces";

            //if (Session["action_flag"].ToString() == "Create")
            //{
            //    AP_002_ADOEN bnk = db.AP_002_ADOEN.Find(glay.vwstring0);
            //    if (bnk != null)
            //        error_msg = "Can not accept duplicates";
            //}
            //if (Session["action_flag"].ToString() == "Create")
            //{
            //    AP_002_ADOEN bnk = db.AP_002_ADOEN.Find(glay.vwstring1);
            //    if (bnk != null)
            //        error_msg = "Can not accept duplicates";
            //}
            //string sqlstr = "select '1' query0 from AP_002_ADOEN where account_name=" + util.sqlquote(glay.vwstring1) + " and account_code <> " + util.sqlquote(glay.vwstring0);
            //var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            //if (bglist1 != null)
            //    error_msg = "Can not accept duplicate Account Name";

            //if (error_msg != "")
            //{
            //    ModelState.AddModelError(String.Empty, error_msg);
            //    err_flag = false;
            //}
        }

        private void read_record()
        {
            //glay.vwstrarray6 = new string[20];
            glay.vwint0 = AP_002_ADOEN.journal_number;
            glay.vwint1 = AP_002_ADOEN.sequence_number;
            glay.vwstrarray0[0] = AP_002_ADOEN.account_code;
            glay.vwstrarray0[1] = AP_002_ADOEN.intercoy_code;
            glay.vwstrarray0[2] = AP_002_ADOEN.description;
            glay.vwstrarray0[3] = AP_002_ADOEN.source_identifier;
            glay.vwstrarray0[11] = util.date_slash(AP_002_ADOEN.transaction_date);
            glay.vwstrarray0[4] = AP_002_ADOEN.period;
            glay.vwstrarray0[5] = AP_002_ADOEN.year;
            glay.vwdecimal0 = AP_002_ADOEN.exchange_rate;
            glay.vwstrarray0[6] = AP_002_ADOEN.currency_code;
            glay.vwstrarray0[7] = AP_002_ADOEN.reference_number_detail;
            glay.vwstrarray0[8] = AP_002_ADOEN.reference_number;
            glay.vwdecimal1 = AP_002_ADOEN.amount;
            glay.vwdecimal2 = AP_002_ADOEN.base_amount;
            glay.vwstrarray0[9] = AP_002_ADOEN.debit_credit_code;
            glay.vwstrarray0[10] = AP_002_ADOEN.note;
            glay.vwstrarray6[0] = AP_002_ADOEN.analysis_code1;
            glay.vwstrarray6[1] = AP_002_ADOEN.analysis_code2;
            glay.vwstrarray6[2] = AP_002_ADOEN.analysis_code3;
            glay.vwstrarray6[3] = AP_002_ADOEN.analysis_code4;
            glay.vwstrarray6[4] = AP_002_ADOEN.analysis_code5;
            glay.vwstrarray6[5] = AP_002_ADOEN.analysis_code6;
            glay.vwstrarray6[6] = AP_002_ADOEN.analysis_code7;
            glay.vwstrarray6[7] = AP_002_ADOEN.analysis_code8;
            glay.vwstrarray6[8] = AP_002_ADOEN.analysis_code9;
            glay.vwstrarray6[9] = AP_002_ADOEN.analysis_code10;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "ADOEN" && bg.document_code == AP_002_ADOEN.account_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();


        }
        private void select_query()
        {
            var bglist = from bg in db.IC_001_INCOY
                         where bg.active_status == "N"
                         orderby bg.intercoy_name
                         select bg;

            ViewBag.icoy = new SelectList(bglist.ToList(), "intercoy_code", "intercoy_name", glay.vwstrarray0[1]);

            var bgitem = from bg in db.GB_999_MSG
                         where bg.type_msg == "HEAD" && bg.name6_msg =="P"
                         orderby bg.name1_msg
                         select bg;

            ViewBag.source = new SelectList(bgitem.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[3]);

            var bgiteme = from bg in db.GB_999_MSG
                         where bg.type_msg == "FYM" 
                         orderby bg.name1_msg
                         select bg;

            ViewBag.month = new SelectList(bgiteme.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[4]);


        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            int idd = Convert.ToInt16(id);
            var flagupdate = (from bd in db.AP_002_ADOEN
                              where bd.sequence_number == idd
                              select bd).FirstOrDefault();
                string jhn = flagupdate.analysis_code1;

            string sqlstr = "delete from [dbo].[AP_002_ADOEN] where journal_number+'[]'+sequence_number=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];           
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwbool0 = true;
            glay.vwlist0 = new List<querylay>[20];

        }

        [HttpPost]
        public ActionResult pricehead_list(string id)
        {
            // write your query statement
            var hdet = from bg in db.GL_001_CATEG
                       join bk in db.GL_001_ATYPE
                       on new { a1 = bg.acct_cat_sequence } equals new { a1=bk.acct_cat_sequence}
                       into bk2
                       from bk3 in bk2.DefaultIfEmpty()
                       where bk3.acct_type_code == id
                       orderby bg.acct_cat_desc
                       select new
                       {
                           c1 = bg.acct_cat_sequence,
                           c2 = bg.acct_cat_desc
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

        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

            SelectList[] head_det = new SelectList[20];

            //Session["head_det"] = head_det;
           // //Session["aheader7"] = aheader7;
           // psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "009" && bg.sequence_no != 99
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
                    //              select bg;
                    //head_det[count2] = new SelectList(bglist3.ToList(), "analysis_code", "analysis_description", glay.vwstrarray6[count2]);
                    
                    string str = " select analysis_code query0, analysis_description query1 from GB_001_DANAL where header_sequence = ";
                    str += util.sqlquote(item.header_code);
                    var str1 = db.Database.SqlQuery<querylay>(str);
                    glay.vwlist0[count2] = str1.ToList();
                }

            }

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
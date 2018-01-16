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
    public class PurchasesController : Controller
    {
        AP_001_PTRAN AP_001_PTRAN = new AP_001_PTRAN();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        bool err_flag = true;

        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            

            var bglist = from bh in db.AP_001_PTRAN
                         join bg in db.GB_999_MSG
                         on new { a1 = "PTT", a2 = bh.purchase_order_class } equals new { a1 = bg.type_msg, a2 = bg.code_msg}
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.purchase_order_code,
                             vwstring1 = bh.purchase_order_name,
                             vwstring2 = bg2.name1_msg,
                             vwstring3 = bh.purchase_requisition == "Y" ? "Yes" : "No",
                             vwint0 = bh.request_for_quote_sequence,
                             vwstring4 = bh.purchase_order == "Y"? "Yes" : "No",
                             vwstring5 = bh.quarantine_receipt == "Y" ? "Yes" : "No",
                             vwstring6 = bh.request_for_quote == "Y" ? "Yes" :"No",
                         };
            
            return View(bglist.Distinct().ToList());


        }
   
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
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

        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            AP_001_PTRAN = db.AP_001_PTRAN.Find(key1,0);
            if (AP_001_PTRAN != null)
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
            glay = glay_in;

            if (id_xhrt=="D")
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
            AP_001_PTRAN = db.AP_001_PTRAN.Find(glay.vwstring0,0);
            if (AP_001_PTRAN!=null)
            {
                db.AP_001_PTRAN.Remove(AP_001_PTRAN);
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
            string str1 = "delete from AP_001_PTRAN where purchase_order_code=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "insert into AP_001_PTRAN(purchase_order_code,sequence_no,purchase_order_class,purchase_order_name,purchase_requisition,request_for_quote,";
            str1 += "purchase_order,quarantine_receipt,inspection_certification,grn,fixed_asset_register,vendor_invoice_booking,tax_invoice1,tax_invoice2,";
            str1 += "header_code,mandatory_flag,modified_by,created_by,note,active_status) values(";
            str1 += util.sqlquote(glay.vwstring0) + ",0,";
            str1 += util.sqlquote(glay.vwstring2) + ",";
            str1 += util.sqlquote(glay.vwstring1) + ",";
            str1 += glay.vwblarray0[0] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[1] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[2] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[3] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[4] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[5] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[6] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[7] ? "'Y'," : "'N',";
            str1 += util.sqlquote(glay.vwstring4) + ",";
            str1 += util.sqlquote(glay.vwstring5) + ",";
            str1 += "'','',";
            str1 += util.sqlquote(pubsess.userid) + ",";
            str1 += util.sqlquote(pubsess.userid) + ",";
            str1 += util.sqlquote(glay.vwstring3) + ",";
            str1 += glay.vwblarray0[8] ? "'Y'" : "'N'";
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

            str1 = "insert into AP_001_PTRAN(purchase_order_code,sequence_no,header_code,mandatory_flag,created_by,modified_by) values (";
            string str2 = "";
            for (int count1 = 0; count1 < 10; count1++)
            {
                if (!string.IsNullOrWhiteSpace(glay.vwstrarray0[count1]))
                {
                    str2 = str1 + util.sqlquote(glay.vwstring0) + ",";
                    str2 += (count1 + 1).ToString() + ",";
                    str2 += util.sqlquote(glay.vwstrarray0[count1]) + ",";
                    str2 += glay.vwblarray1[count1] ? "'Y'" : "'N'";
                    str2 += "," + util.sqlquote(pubsess.userid);
                    str2 += "," + util.sqlquote(pubsess.userid) + ")";
                    try
                    {
                        db.Database.ExecuteSqlCommand(str2);
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

            if (err_flag)
            {
                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("PURCHASE", glay.vwstring0, photo1, glay.vwstrarray9);
                }
               
            }

        }

        private void validation_routine()
        {
            string error_msg = "";
            bool taxbk = false;
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Purchase Transaction Type ID must not be spaces");
                err_flag = false;
            }

            
                bool blk_flag = true;
               // bool dup_flag = false;
                for (int wctr = 0; wctr < 8; wctr++)
                {
                    if (glay.vwblarray0[wctr])
                    {
                        blk_flag = false;
                        taxbk = glay.vwblarray0[wctr];
                        //for (int ictr = 18; ictr < 23; ictr++)
                        //{
                        //    if (glay.vwblarray0[ictr] == taxbk && (wctr != ictr))
                        //        dup_flag = true;
                        //}
                    }
                }

                if (blk_flag)
                {
                    ModelState.AddModelError(String.Empty, "Select atleast one purchase transaction document");
                    err_flag = false;
                }
                //if (dup_flag)
                //{
                //    ModelState.AddModelError(String.Empty, "one or more TAX ID are equal");
                //    err_flag = false;
                //}
            

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
            glay.vwstrarray0 = new string[20];
            glay.vwblarray0 = new bool[20];
            glay.vwstrarray6 = new string[20];

            glay.vwstring0 = AP_001_PTRAN.purchase_order_code;
            glay.vwstring1 = AP_001_PTRAN.purchase_order_name;
            glay.vwstring2 = AP_001_PTRAN.purchase_order_class;
            if (AP_001_PTRAN.purchase_requisition == "Y")
                glay.vwblarray0[0] = true;
            if (AP_001_PTRAN.request_for_quote == "Y")
                glay.vwblarray0[1] = true;
            if (AP_001_PTRAN.purchase_order == "Y")
                glay.vwblarray0[2] = true;
            if (AP_001_PTRAN.quarantine_receipt == "Y")
                glay.vwblarray0[3] = true;
            if (AP_001_PTRAN.inspection_certification == "Y")
                glay.vwblarray0[4] = true;
            if (AP_001_PTRAN.grn == "Y")
                glay.vwblarray0[5] = true;
            if (AP_001_PTRAN.fixed_asset_register == "Y")
                glay.vwblarray0[6] = true;
            if (AP_001_PTRAN.vendor_invoice_booking == "Y")
                glay.vwblarray0[7] = true;
            glay.vwstring4 = AP_001_PTRAN.tax_invoice1;
            glay.vwstring5 = AP_001_PTRAN.tax_invoice2;
            if (string.IsNullOrWhiteSpace(glay.vwstring4) && string.IsNullOrWhiteSpace(glay.vwstring5))
            {
                glay.vwstring6 = "N";
            }
            else glay.vwstring6 = "Y";
            //glay.vwstrarray0[2] = AP_001_PTRAN.user_defined1;
            //glay.vwstrarray0[3] = AP_001_PTRAN.user_defined2;
            //glay.vwstrarray0[4] = AP_001_PTRAN.user_defined3;
            glay.vwstring3 = AP_001_PTRAN.note;
            var bhlist = from bg in db.AP_001_PTRAN
                         where bg.purchase_order_code == glay.vwstring0
                         select bg;

            foreach (var item in bhlist.ToList())
            {
                if (item.sequence_no != 0)
                {
                    glay.vwstring0 = item.purchase_order_code;
                    glay.vwstrarray0[item.sequence_no - 1] = item.header_code;
                    glay.vwblarray1[item.sequence_no - 1] = item.mandatory_flag == "Y" ? true : false;
                }
            }
            if (AP_001_PTRAN.active_status == "Y")
                glay.vwblarray0[8] = true;
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "PURCHASE" && bg.document_code == AP_001_PTRAN.purchase_order_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();

        
        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwblarray0 = new bool[20];
            glay.vwblarray1 = new bool[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwblarray0[2] = true;
            glay.vwblarray0[5] = true;
           glay.vwblarray0[7] = true;
           glay.vwlist0 = new List<querylay>[20];
        }
        private void select_query()
        {
            ViewBag.header1 = util.para_selectquery("50","");
            //var bgitem = from bh in db.GB_001_HANAL
            //             where bh.active_status == "N"
            //             orderby bh.header_description
            //             select bh;

            //ViewBag.header1 = bgitem.ToList();


            var bglist = from bh in db.GB_999_MSG
                         where bh.type_msg == "PTT"
                         orderby bh.name1_msg
                         select bh;
            ViewBag.pclass = new SelectList(bglist.ToList(), "code_msg", "name1_msg", glay.vwstring2);

            var bglistb = from bg in db.GB_001_TAX
                          where bg.computation_basis == "T" & bg.active_status == "N"
                          orderby bg.tax_name
                          select new { c1 = bg.tax_code, c2 = bg.tax_name };

            ViewBag.tax = new SelectList(bglistb.ToList(), "c1", "c2");

        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AP_001_PTRAN] where purchase_order_code=" + util.sqlquote(id);
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

           // Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "007" && bg.sequence_no != 99
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
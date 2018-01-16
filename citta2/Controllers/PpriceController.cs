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
    public class PpriceController : Controller
    {
        AR_001_PMTRX AR_001_PMTRX = new AR_001_PMTRX();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
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
            pmtrix();

            var bglist = from bh in db.AR_001_PMTRX
                         join bk5 in db.GB_999_MSG
                         on new { a1 = "TI", a2 = bh.tax_inclusive } equals new { a1 = bk5.type_msg, a2 = bk5.code_msg }
                         into bk6
                         from bk7 in bk6.DefaultIfEmpty()
                         join bf in db.IV_001_ITEM
                         on new { a1 = bh.item_code } equals new { a1 = bf.item_code }
                         into bf1
                         from bf2 in bf1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.item_code,
                             vwstring7 = bf2.item_name,
                             vwint0 = bh.tenor,
                             vwstring3 = bk7.name1_msg,
                             vwstring5 = bh.currency,
                             vwstring6 = bh.selected_promo,
                             vwdecimal0 = bh.selling_price_class1,
                             vwdecimal1 = bh.month_price,
                             vwstring4 = bh.note,
                             vwstring9 = bh.active_status == "N" ? "Active" : "Inactive",
                             vwstring8 = bh.tax_inclusive
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
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile, string subcheck)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            if (subcheck == "RR")
            {
                read_itemrecord();
                pmtrix();
                select_query();
                return View(glay);
            }
            update_file();

            if (err_flag)
                return RedirectToAction("Create");
            initial_rtn();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1, int key2, string key3,string key4)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            psess = (psess)Session["psess"];
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            //int intval = Convert.ToInt32(key2);

            AR_001_PMTRX = db.AR_001_PMTRX.Find(key1, key2,key3,key4);
            if (AR_001_PMTRX != null)
                read_record();

            //initial_rtn();
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
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        private void delete_record()
        {
            AR_001_PMTRX = db.AR_001_PMTRX.Find(glay.vwstring0, glay.vwdecimal7,glay.vwstring5,glay.vwstring6);
            if (AR_001_PMTRX != null)
            {
                db.AR_001_PMTRX.Remove(AR_001_PMTRX);
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
                AR_001_PMTRX = new AR_001_PMTRX();
                AR_001_PMTRX.created_by = pubsess.userid;
                AR_001_PMTRX.created_date = DateTime.UtcNow;
            }
            else
            {
                AR_001_PMTRX = db.AR_001_PMTRX.Find(glay.vwstring0, glay.vwint0,glay.vwstring5,glay.vwstring6);
            }
            AR_001_PMTRX.item_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            AR_001_PMTRX.tenor = glay.vwint0;
            AR_001_PMTRX.tax_inclusive = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            AR_001_PMTRX.selected_promo = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
           
            AR_001_PMTRX.selling_price_class1 = glay.vwdecimal0;
            AR_001_PMTRX.month_price = glay.vwdecimal1;
            AR_001_PMTRX.modified_date = DateTime.UtcNow;
            AR_001_PMTRX.modified_by = pubsess.userid;
            AR_001_PMTRX.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            AR_001_PMTRX.active_status = glay.vwbool0 ? "Y" : "N";
            AR_001_PMTRX.property_acquisition = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            AR_001_PMTRX.currency = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            AR_001_PMTRX.deposit_flat = glay.vwdecimal2;
            AR_001_PMTRX.deposit_percent = glay.vwdecimal3;
            AR_001_PMTRX.num_installment = 0;
            AR_001_PMTRX.installment_amt = 0;
            AR_001_PMTRX.interest_rate = 0;
            AR_001_PMTRX.penalty_amt = 0;
            AR_001_PMTRX.interest_rate_pen = 0;
            AR_001_PMTRX.installment_discrip = "";
            AR_001_PMTRX.property_acquisition = "";
            AR_001_PMTRX.deposit_flat = 0;
            AR_001_PMTRX.deposit_percent = 0;
            AR_001_PMTRX.installment_interval = 0;
            //if (glay.vwstring3 == "I")
            //{
            //    AR_001_PMTRX.installment_interval = glay.vwdclarray0[2];
            //    AR_001_PMTRX.installment_discrip = glay.vwstring5;
            //    AR_001_PMTRX.num_installment = glay.vwdclarray0[3];
            //    AR_001_PMTRX.installment_amt = glay.vwdclarray0[4];
            //}
            if (glay.vwstring3 == "M")
            {
                AR_001_PMTRX.interest_rate = glay.vwdecimal4;
               // AR_001_PMTRX.installment_amt = glay.vwdclarray0[6];
                //AR_001_PMTRX.num_installment = glay.vwdclarray0[7];
                AR_001_PMTRX.penalty_amt = glay.vwdecimal5;
                AR_001_PMTRX.interest_rate_pen = glay.vwdecimal6;
            }
                   

           if(action_flag == "Create")
                db.Entry(AR_001_PMTRX).State = EntityState.Added;
            else
                db.Entry(AR_001_PMTRX).State = EntityState.Modified;

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
                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("PMTRIX", AR_001_PMTRX.tenor.ToString(), photo1, glay.vwstrarray9);
                }

            }

        }

        private void validation_routine()
        {
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please select item");
                err_flag = false;
            }

            if (glay.vwint0==0)
            {
                ModelState.AddModelError(String.Empty, "Please enter Tenor ");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                AR_001_PMTRX bnk = db.AR_001_PMTRX.Find(glay.vwstring0, glay.vwint0,glay.vwstring5,glay.vwstring6);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }
           
        }
        private void select_query()
        {
            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);

            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring5);


           // ViewBag.item_code = util.para_selectquery("008", glay.vwstring0,"Y");
            var bglisti = from bh in db.IV_001_ITEM
                          where bh.active_status == "N" && bh.item_type == "P"
                          orderby bh.item_name
                          select new { c1 = bh.item_code, c2 = bh.item_code + "-" + bh.item_name };
            ViewBag.item_code = new SelectList(bglisti.ToList(), "c1", "c2", glay.vwstring0);

            var dis = from bg in db.DC_001_DISC
                      select new { c1 = bg.discount_code, c2 = bg.discount_name };
            ViewBag.promo = new SelectList(dis.Distinct().ToList(), "c1", "c2", glay.vwstring6);


          }

        private void read_record()
        {
            string[] head_dat = new string[20];
            glay.vwstring0 = AR_001_PMTRX.item_code;
            glay.vwint0 = AR_001_PMTRX.tenor;
            glay.vwstring2 = AR_001_PMTRX.tax_inclusive;
            glay.vwdecimal0 = AR_001_PMTRX.selling_price_class1;
            glay.vwdecimal1 = AR_001_PMTRX.month_price;
            glay.vwstring4 = AR_001_PMTRX.note;
            glay.vwbool0 = false;
            glay.vwstring3 = AR_001_PMTRX.property_acquisition;
            glay.vwstring5 = AR_001_PMTRX.currency;
            glay.vwdecimal2 = AR_001_PMTRX.deposit_flat;
            glay.vwdecimal3 = AR_001_PMTRX.deposit_percent;
            glay.vwstring6 = AR_001_PMTRX.selected_promo;
            //if (AR_001_PMTRX.property_acquisition == "I")
            //{
            //    glay.vwdclarray0[2] = AR_001_PMTRX.installment_interval;
            //    glay.vwstring5 = AR_001_PMTRX.installment_discrip;
            //    glay.vwdclarray0[3] = AR_001_PMTRX.num_installment;
            //    glay.vwdclarray0[4] = AR_001_PMTRX.installment_amt;
            //}
            if (AR_001_PMTRX.property_acquisition == "M")
            {
                glay.vwdecimal4 = AR_001_PMTRX.interest_rate;
                //glay.vwdclarray0[6] = AR_001_PMTRX.installment_amt;
                //glay.vwdclarray0[7] = AR_001_PMTRX.num_installment;
                glay.vwdecimal5 = AR_001_PMTRX.penalty_amt;
                glay.vwdecimal6 = AR_001_PMTRX.interest_rate_pen;
            }
           
            if (AR_001_PMTRX.active_status == "Y")
                glay.vwbool0 = true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "PMTRIX" && bg.document_code == AR_001_PMTRX.tenor.ToString()
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();
        }

        private void error_message()
        {

        }

        private void read_itemrecord()
        {
            ModelState.Clear();
            //ModelState.Remove("vwdclarray0[0]");
            //ModelState.Remove("vwdclarray0[1]");
            //ModelState.Remove("vwdclarray0[2]");
            //ModelState.Remove("vwdclarray0[3]");
            //ModelState.Remove("vwdclarray0[4]");
            //ModelState.Remove("vwdclarray0[5]");
            //ModelState.Remove("subcheck");
            //ModelState.Remove("vwstring1");

            // write your query statement
            var itm = (from bd in db.IV_001_ITEM
                       where bd.item_code == glay.vwstring0
                       select bd).FirstOrDefault();

            glay.vwstring3 = itm.tax_inclusive;
            glay.vwdclarray0 = new decimal[10];
            glay.vwdclarray0[0] = itm.selling_price_class1;
            glay.vwdclarray0[1] = itm.selling_price_class2;
            glay.vwdclarray0[2] = itm.selling_price_class3;
            glay.vwdclarray0[3] = itm.selling_price_class4;
            glay.vwdclarray0[4] = itm.selling_price_class5;
            glay.vwdclarray0[5] = itm.selling_price_class6;

            glay.vwstring1 = itm.header_sequence;
        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AR_001_PMTRX] where item_code+'[]'+cast (tenor as varchar)+'[]'+currency+'[]'+selected_promo=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }



        private void initial_rtn()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwdclarray0 = new decimal[20];
            glay.vwstring2 = "Y";
            glay.vwstring3 = "I";
            glay.vwstring6 = "Reg";

            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwdclarray0[wctr] = 0;

            glay.vwstring5 = pubsess.base_currency_code;
           

        }

        private void pmtrix()
        {
            var coyin = (from bg in db.GB_001_COY
                         where bg.id_code == "COYPRICE"
                         select bg).FirstOrDefault();

            glay.vwstrarray4 = new string[20];
            glay.vwstrarray4[0] = coyin.field6;
            glay.vwstrarray4[1] = coyin.field7;
            glay.vwstrarray4[2] = coyin.field8;
            glay.vwstrarray4[3] = coyin.field9;
            glay.vwstrarray4[4] = coyin.field10;
            glay.vwstrarray4[5] = coyin.field11;
            psess.temp0 = coyin.field6;
            psess.temp1 = coyin.field7;
            psess.temp2 = coyin.field8;
            Session["psess"] = psess;
            
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
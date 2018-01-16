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
    public class PriceController : Controller
    {
        AR_001_MTRIX AR_001_MTRIX = new AR_001_MTRIX();
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
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            pmtrix();

            var bglist = from bh in db.AR_001_MTRIX
                         join bk in db.GB_001_DANAL
                         on new { a1 = bh.header_sequence, a2 = bh.analysis_code } equals new { a1 = bk.header_sequence, a2 = bk.analysis_code }
                         into bk3
                         from bk4 in bk3.DefaultIfEmpty()
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
                             vwstring1 = bh.header_sequence,
                             vwstring2 = bh.analysis_code,
                             vwstring3 = bk7.name1_msg,
                             vwdecimal0 = bh.selling_price_class1,
                             vwdecimal1 = bh.selling_price_class2,
                             vwdecimal2 = bh.selling_price_class3,
                             vwdecimal3 = bh.selling_price_class4,
                             vwdecimal4 = bh.selling_price_class5,
                             vwdecimal5 = bh.selling_price_class6,
                             vwstring4 = bh.note,
                             vwstring6 = bh.active_status == "N" ? "Active" : "Inactive",
                             vwstring5 = bk4.analysis_description,
                             vwstring8 = bh.tax_inclusive
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
            pmtrix();
            select_query();
            return View(glay);
        }

        [HttpPost]
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

            pmtrix();
            select_query();
            return View(glay);
        }

        public ActionResult Edit(string key1, string key2, string key3)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            psess = (psess)Session["psess"];
            initial_rtn();
            pubsess = (pubsess)Session["pubsess"];

            AR_001_MTRIX = db.AR_001_MTRIX.Find(key1, key2, key3);
            if (AR_001_MTRIX != null)
                read_record();

            pmtrix();
            select_query();
            return View(glay);
        }

        [HttpPost]
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

            pmtrix();
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Index1(string id_xhrt)
        {
            if (id_xhrt == "D")
            {
                delete_record();
                return RedirectToAction("Index");
            }

            update_file();
            if (err_flag)
                return RedirectToAction("Index");

            select_query();
            return View(glay);

        }
        private void delete_record()
        {
            AR_001_MTRIX = db.AR_001_MTRIX.Find(glay.vwstring0, glay.vwstring1, glay.vwstring2);
            if (AR_001_MTRIX != null)
            {
                db.AR_001_MTRIX.Remove(AR_001_MTRIX);
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
                AR_001_MTRIX = new AR_001_MTRIX();
                AR_001_MTRIX.created_by = pubsess.userid;
                AR_001_MTRIX.created_date = DateTime.UtcNow;
            }
            else
            {
                AR_001_MTRIX = db.AR_001_MTRIX.Find(glay.vwstring0, glay.vwstring1, glay.vwstring2);
            }
            AR_001_MTRIX.attach_document = "";
            AR_001_MTRIX.item_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            AR_001_MTRIX.header_sequence = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            AR_001_MTRIX.analysis_code = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            AR_001_MTRIX.tax_inclusive = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;

            int arrayctr = glay.vwdclarray0.Length;
            if (arrayctr >= 1) AR_001_MTRIX.selling_price_class1 = glay.vwdclarray0[0];
            if (arrayctr >= 2) AR_001_MTRIX.selling_price_class2 = glay.vwdclarray0[1];
            if (arrayctr >= 3) AR_001_MTRIX.selling_price_class3 = glay.vwdclarray0[2];
            if (arrayctr >= 4) AR_001_MTRIX.selling_price_class4 = glay.vwdclarray0[3];
            if (arrayctr >= 5) AR_001_MTRIX.selling_price_class5 = glay.vwdclarray0[4];
            if (arrayctr >= 6) AR_001_MTRIX.selling_price_class6 = glay.vwdclarray0[5];
            AR_001_MTRIX.modified_date = DateTime.UtcNow;
            AR_001_MTRIX.modified_by = pubsess.userid;
            AR_001_MTRIX.note = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            AR_001_MTRIX.active_status = glay.vwbool0 ? "Y" : "N";


           if(action_flag == "Create")
                db.Entry(AR_001_MTRIX).State = EntityState.Added;
            else
                db.Entry(AR_001_MTRIX).State = EntityState.Modified;

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
                    util.write_document("MATRIX", AR_001_MTRIX.analysis_code, photo1, glay.vwstrarray9);
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

            if (string.IsNullOrWhiteSpace(glay.vwstring2))
            {
                ModelState.AddModelError(String.Empty, "Please enter analysis detail");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                AR_001_MTRIX bnk = db.AR_001_MTRIX.Find(glay.vwstring0, glay.vwstring1, glay.vwstring2);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }
            for (int pctr = 0; pctr < glay.vwdclarray0.Length; pctr++)
            {
                if ((glay.vwdclarray0[pctr] == 0))
                {
                    ModelState.AddModelError(String.Empty, "Enter a valid Value in all the price classes");
                    err_flag = false;
                }
            }


        }
        private void select_query()
        {
           // ViewBag.item_code = util.para_selectquery("008", glay.vwstring0,"Y");
            var bglisti = from bh in db.IV_001_ITEM
                          where bh.active_status == "N" && bh.price_matrix == "Y"
                          orderby bh.item_name
                          select bh;
            ViewBag.item_code = new SelectList(bglisti.ToList(), "item_code", "item_name", glay.vwstring0);

            glay.vwstring5= (from bh in db.GB_001_HANAL
                         where bh.header_sequence == glay.vwstring1
                         select bh.header_description).FirstOrDefault();

            ViewBag.analysis_code = util.para_selectquery("51", glay.vwstring2);
        }

        private void read_record()
        {
            string[] head_dat = new string[20];
            glay.vwdclarray0 = new decimal[20];
            glay.vwstring0 = AR_001_MTRIX.item_code;
            glay.vwstring1 = AR_001_MTRIX.header_sequence;
            glay.vwstring2 = AR_001_MTRIX.analysis_code;
            glay.vwstring3 = AR_001_MTRIX.tax_inclusive;
            glay.vwdclarray0[0] = AR_001_MTRIX.selling_price_class1;
            glay.vwdclarray0[1] = AR_001_MTRIX.selling_price_class2;
            glay.vwdclarray0[2] = AR_001_MTRIX.selling_price_class3;
            glay.vwdclarray0[3] = AR_001_MTRIX.selling_price_class4;
            glay.vwdclarray0[4] = AR_001_MTRIX.selling_price_class5;
            glay.vwdclarray0[5] = AR_001_MTRIX.selling_price_class6;
            glay.vwstring4 = AR_001_MTRIX.note;
            glay.vwbool0 = false;
            if (AR_001_MTRIX.active_status == "Y")
                glay.vwbool0 = true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "MATRIX" && bg.document_code == AR_001_MTRIX.analysis_code
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
            string sqlstr = "delete from [dbo].[AR_001_MTRIX] where item_code+'[]'+header_sequence+'[]'+analysis_code=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }



        private void initial_rtn()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwdclarray0 = new decimal[20];
            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwdclarray0[wctr] = 0;


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
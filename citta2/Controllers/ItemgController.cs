using CittaErp.Models;
using CittaErp.utilities;
using anchor1.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class ItemgController : Controller
    {
        GB_001_ITMG GB_001_ITMG = new GB_001_ITMG();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
       
        bool err_flag = true;
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.GB_001_ITMG
                         join bk in db.GB_999_MSG
                         on new { a1 = "ITT", a2 = bh.item_type } equals new { a1 = bk.type_msg, a2 = bk.code_msg}
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         join bt in db.GB_001_PCODE
                         on new { a1 = bh.sku_sequence, a2 = "10"} equals new {a1 = bt.parameter_code, a2 = bt.parameter_type}
                         into bt1
                         from bt2 in bt1.DefaultIfEmpty()
                         select 
                         new vw_genlay
                         {
                             vwstring0 = bh.item_group_id,
                             vwstring1 = bh.item_group,   
                             vwstring2 = bt2.parameter_name,
                             vwstring3 = bk2.name1_msg,
                         };
            
            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            
            initial_rtn();
          //  pmtrix();
           // header_ana();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;

            update_file();

            if (err_flag)
                return RedirectToAction("Create");
           
            initial_rtn();
           // pmtrix();
           // header_ana();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
            initial_rtn();
            pubsess = (pubsess)Session["pubsess"];
            GB_001_ITMG = db.GB_001_ITMG.Find(key1);
            if (GB_001_ITMG != null)
                read_record();
           
            //pmtrix();
            //header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt=="D")
            { 
                delete_record();
                return RedirectToAction("Index");
            }

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            initial_rtn();
            //pmtrix();
            //header_ana();
            select_query();
            return View(glay);
        }

        private void delete_record()
        {
            GB_001_ITMG = db.GB_001_ITMG.Find(glay.vwstring0);
            if (GB_001_ITMG!=null)
            {
                db.GB_001_ITMG.Remove(GB_001_ITMG);
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
            if (action_flag=="Create")
            {
                GB_001_ITMG = new GB_001_ITMG();
                GB_001_ITMG.created_by = pubsess.userid;
                GB_001_ITMG.created_date = DateTime.UtcNow;
               
                //GB_001_ITMG.last_purchase_cost = 0;
                //GB_001_ITMG.last_purchase_date = new DateTime(1990, 1, 1);
                //GB_001_ITMG.last_purchase_vendor = "";
            }
            else
            {
                GB_001_ITMG = db.GB_001_ITMG.Find(glay.vwstring0);
            }
                GB_001_ITMG.item_group_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                GB_001_ITMG.item_group = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
                GB_001_ITMG.item_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
                GB_001_ITMG.preferred_vendor = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
                GB_001_ITMG.item_costing_method = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
                GB_001_ITMG.tax = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
                GB_001_ITMG.tax_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
                GB_001_ITMG.tax_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
                GB_001_ITMG.tax_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
                GB_001_ITMG.tax_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
                GB_001_ITMG.tax_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
                GB_001_ITMG.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
                GB_001_ITMG.gl_price_var_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
                GB_001_ITMG.gl_inv_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
                GB_001_ITMG.gl_stockcount_variance_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
                GB_001_ITMG.gl_income_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
                GB_001_ITMG.gl_cos_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[15]) ? "" : glay.vwstrarray0[15];
               // GB_001_ITMG.item_group_maintenance = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
                GB_001_ITMG.sku_sequence = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
                GB_001_ITMG.modified_date = DateTime.UtcNow;
                GB_001_ITMG.modified_by = pubsess.userid;
                GB_001_ITMG.maintanance = glay.vwblarray0[0] ? "Y" : "N";
                GB_001_ITMG.sales = glay.vwblarray0[1] ? "Y" : "N";
                GB_001_ITMG.purchases = glay.vwblarray0[2] ? "Y" : "N";
                GB_001_ITMG.production = glay.vwblarray0[3] ? "Y" : "N";
                GB_001_ITMG.consumables = glay.vwblarray0[4] ? "Y" : "N";
                GB_001_ITMG.reusable = glay.vwblarray0[5] ? "Y" : "N";
                GB_001_ITMG.item_group_maintenance = "";
                //GB_001_ITMG.item_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
                // GB_001_ITMG.description = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2]; 
                //GB_001_ITMG.issurance_method = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                //GB_001_ITMG.serial_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
                //GB_001_ITMG.price_matrix = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
                //GB_001_ITMG.header_sequence = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
                //GB_001_ITMG.discount_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
                //GB_001_ITMG.reorder_level = glay.vwdclarray0[1]; 
                //GB_001_ITMG.weight_per_sku = glay.vwdclarray0[0];
                //GB_001_ITMG.minimum_stock = glay.vwdclarray0[2];
                //GB_001_ITMG.maximum_stock = glay.vwdclarray0[3];
                //GB_001_ITMG.standard_cost = glay.vwdclarray0[6];
                //GB_001_ITMG.reorder_quantity = glay.vwdclarray0[4];
                //GB_001_ITMG.standard_lead_time = glay.vwint1;
                //GB_001_ITMG.selling_price_class1 = glay.vwdclarray1[0];
                //GB_001_ITMG.selling_price_class2 = glay.vwdclarray1[1];
                //GB_001_ITMG.selling_price_class3 = glay.vwdclarray1[2];
                //GB_001_ITMG.selling_price_class4 = glay.vwdclarray1[3];
                //GB_001_ITMG.selling_price_class5 = glay.vwdclarray1[4];
                //GB_001_ITMG.selling_price_class6 = glay.vwdclarray1[5];
                //GB_001_ITMG.average_cost = glay.vwdclarray0[5];
               // GB_001_ITMG.active_status = glay.vwbool0 ? "Y" : "N";
                //GB_001_ITMG.analysis_code1 = "";
                //GB_001_ITMG.analysis_code2 = "";
                //GB_001_ITMG.analysis_code3 = "";
                //GB_001_ITMG.analysis_code4 = "";
                //GB_001_ITMG.analysis_code5 = "";
                //GB_001_ITMG.analysis_code6 = "";
                //GB_001_ITMG.analysis_code7 = "";
                //GB_001_ITMG.analysis_code8 = "";
                //GB_001_ITMG.analysis_code9 = "";
                //GB_001_ITMG.analysis_code10 = "";

           if(action_flag == "Create")  
                db.Entry(GB_001_ITMG).State = EntityState.Added;
            else 
                db.Entry(GB_001_ITMG).State = EntityState.Modified;

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
                //util.parameter_deleteflag("006", glay.vwstring0);
                                
            }

        }

        private void validation_routine()
        {
            string taxbk = "";
            
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please enter Item group ID");
                err_flag = false;
            }

            if (glay.vwstrarray0[4] == "Y")
            {
                bool blk_flag = true;
                bool dup_flag = false;
                for (int wctr = 5; wctr < 10; wctr++)
                {
                    if (!string.IsNullOrWhiteSpace(glay.vwstrarray0[wctr]))
                    {
                        blk_flag = false;
                        taxbk = glay.vwstrarray0[wctr];
                        for (int ictr = 5; ictr < 10; ictr++)
                        {
                            if (glay.vwstrarray0[ictr] == taxbk && (wctr != ictr))
                                dup_flag = true;
                        }
                    }
                }

                if (blk_flag)
                {
                    ModelState.AddModelError(String.Empty, "All TAX IDs must not be empty");
                    err_flag = false;
                }
                if (dup_flag)
                {
                    ModelState.AddModelError(String.Empty, "one or more TAX ID are equal");
                    err_flag = false;
                }
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[0]))
            {
                ModelState.AddModelError(String.Empty, "Item Group must not be spaces");
                err_flag = false;
            }
          
           if(action_flag == "Create")
            {
                GB_001_ITMG bnk = db.GB_001_ITMG.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Item group ID already exist");
                    err_flag = false;
                }
            }
            if (glay.vwstring2 == "")
            {
                ModelState.AddModelError(String.Empty, "Please enter unit of measure");
                err_flag = false;
            }

            //string sqlstr = "select '1' query0 from GB_001_ITMG where item_name=" + util.sqlquote(glay.vwstrarray0[0]) + " and item_code <> " + util.sqlquote(glay.vwstring0);
            //var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            //if (bglist1 != null)
            //{
            //    ModelState.AddModelError(String.Empty, "Can not accept duplicate Item Name");
            //    err_flag = false;
            //}
        }

        private void read_record()
        {
            glay.vwstring0 = GB_001_ITMG.item_group_id;
            glay.vwstring2 = GB_001_ITMG.sku_sequence;
            glay.vwstrarray0[0] = GB_001_ITMG.item_group;
            glay.vwstrarray0[1] = GB_001_ITMG.item_type;
            glay.vwstrarray0[2] = GB_001_ITMG.preferred_vendor;
            glay.vwstrarray0[3] = GB_001_ITMG.item_costing_method;
            glay.vwstrarray0[4] = GB_001_ITMG.tax;
            glay.vwstrarray0[5] = GB_001_ITMG.tax_code1;
            glay.vwstrarray0[6] = GB_001_ITMG.tax_code2;
            glay.vwstrarray0[7] = GB_001_ITMG.tax_code3;
            glay.vwstrarray0[8] = GB_001_ITMG.tax_code4;
            glay.vwstrarray0[9] = GB_001_ITMG.tax_code5;
            glay.vwstrarray0[10] = GB_001_ITMG.note;
            glay.vwstrarray0[11] = GB_001_ITMG.gl_price_var_code;
            glay.vwstrarray0[12] = GB_001_ITMG.gl_inv_code;
            glay.vwstrarray0[13] = GB_001_ITMG.gl_stockcount_variance_code;
            glay.vwstrarray0[14] = GB_001_ITMG.gl_income_code;
            glay.vwstrarray0[15] = GB_001_ITMG.gl_cos_code;
           // glay.vwstrarray0[16] = GB_001_ITMG.item_group_maintenance;
            glay.vwblarray0[0] = GB_001_ITMG.maintanance == "Y" ? true : false;
            glay.vwblarray0[1] = GB_001_ITMG.sales == "Y" ? true : false;
            glay.vwblarray0[2] = GB_001_ITMG.purchases == "Y" ? true : false;
            glay.vwblarray0[3] = GB_001_ITMG.production == "Y" ? true : false;
            glay.vwblarray0[4] = GB_001_ITMG.consumables == "Y" ? true : false;
            glay.vwblarray0[5] = GB_001_ITMG.reusable == "Y" ? true : false;
           
            //glay.vwstrarray0[0] = GB_001_ITMG.item_name;
            //glay.vwstrarray0[2] = GB_001_ITMG.description;
            //glay.vwstrarray0[13] = GB_001_ITMG.price_matrix;
            //glay.vwstrarray0[14] = GB_001_ITMG.header_sequence;
            //glay.vwdclarray0[1] = GB_001_ITMG.reorder_level;
            //glay.vwstrarray0[16] = GB_001_ITMG.discount_code;
            //glay.vwstrarray0[15] = "Y";
            //if (string.IsNullOrWhiteSpace(GB_001_ITMG.discount_code))
            //    glay.vwstrarray0[15] = "N";
            //glay.vwdclarray0[2] = GB_001_ITMG.minimum_stock;
            //glay.vwdclarray0[5] = GB_001_ITMG.average_cost;
           // glay.vwstring1 = GB_001_ITMG.issurance_method;
            //glay.vwdclarray0[0] = GB_001_ITMG.weight_per_sku;
            //glay.vwstrarray2[3] = GB_001_ITMG.last_purchase_date.ToShortDateString();
            //glay.vwdclarray0[4] = GB_001_ITMG.reorder_quantity;
            //glay.vwdclarray1[0] = GB_001_ITMG.selling_price_class1;
            //glay.vwdclarray1[1] = GB_001_ITMG.selling_price_class2;
            //glay.vwdclarray1[2] = GB_001_ITMG.selling_price_class3;
            //glay.vwdclarray1[3] = GB_001_ITMG.selling_price_class4;
            //glay.vwdclarray1[4] = GB_001_ITMG.selling_price_class5;
            //glay.vwdclarray1[5] = GB_001_ITMG.selling_price_class6;
            //glay.vwdclarray0[6] = GB_001_ITMG.standard_cost;
            //glay.vwstrarray2[0] = GB_001_ITMG.last_purchase_cost.ToString("#,###.00");
            //glay.vwdclarray0[3] = GB_001_ITMG.maximum_stock;
            //glay.vwint1 = GB_001_ITMG.standard_lead_time;
            //glay.vwstrarray2[1] = GB_001_ITMG.last_purchase_vendor;
            //glay.vwstrarray0[17] = "Y";
            //glay.vwstrarray6[0] = GB_001_ITMG.analysis_code1;
            //glay.vwstrarray6[1] = GB_001_ITMG.analysis_code2;
            //glay.vwstrarray6[2] = GB_001_ITMG.analysis_code3;
            //glay.vwstrarray6[3] = GB_001_ITMG.analysis_code4;
            //glay.vwstrarray6[4] = GB_001_ITMG.analysis_code5;
            //glay.vwstrarray6[5] = GB_001_ITMG.analysis_code6;
            //glay.vwstrarray6[6] = GB_001_ITMG.analysis_code7;
            //glay.vwstrarray6[7] = GB_001_ITMG.analysis_code8;
            //glay.vwstrarray6[8] = GB_001_ITMG.analysis_code9;
            //glay.vwstrarray6[9] = GB_001_ITMG.analysis_code10;

            //if (GB_001_ITMG.active_status == "Y")
            //    glay.vwbool0 = true;

            //var bglist = from bg in db.GB_001_DOC
            //             where bg.screen_code == "ITEM" && bg.document_code == GB_001_ITMG.item_code 
            //             orderby bg.document_sequence
            //            select bg;

            //ViewBag.anapict = bglist.ToList();
        
        }

       private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwdclarray0 = new decimal[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray7 = new string[20];
            glay.vwblarray0 = new bool[20];
            glay.vwstrarray2 = new string[50];
            glay.vwdclarray1 = new decimal[20];
            glay.vwstrarray9 = new string[100];
            glay.vwstrarray0[1] = "I";
            glay.vwstrarray0[5] = "N";
            glay.vwstrarray0[14] = "N";
           // glay.vwstrarray0[16] = "N";
            glay.vwstrarray0[18] = "N";
            glay.vwstrarray0[13] = "N";
            glay.vwstrarray0[6] = "A";
            glay.vwstrarray0[3] = "A";
            glay.vwstrarray0[4] = "N";
            glay.vwblarray0[1] = true;
            glay.vwblarray0[2] = true;
            glay.vwstring1 = "F";
           for ( int ctr = 0; ctr < 100; ctr++)
           {

                glay.vwstrarray9[ctr] = "";
           }

            string[] head_dat = new string[20];

        }
        private void select_query()
        {
           
            var bgitem = from bg in db.AP_001_VENDR
                         where bg.active_status == "N"
                         orderby bg.vend_biz_name
                         select bg;

            ViewBag.vendor = new SelectList(bgitem.ToList(), "vendor_code", "vend_biz_name", glay.vwstrarray0[2]);

            //var bglisti = from bg in db.GB_999_MSG
            //              where bg.type_msg == "ITU"
            //              orderby bg.name1_msg
            //              select bg;

            //ViewBag.itmuse = new SelectList(bglisti.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[16]);

            var bglistz = from bg in db.GB_001_PCODE
                          where bg.parameter_type == "10" && bg.active_status == "N"
                          orderby bg.parameter_name
                          select bg;

            ViewBag.unit = new SelectList(bglistz.ToList(), "parameter_code", "parameter_name", glay.vwstring2);

            //var bglista = from bg in db.DC_001_DISC
            //              where bg.active_status == "N"
            //              orderby bg.discount_name
            //             select bg;

            //ViewBag.discount = new SelectList(bglista.Distinct().ToList(), "discount_code", "discount_name", glay.vwstrarray0[16]);

            var bglistb = from bg in db.GB_001_TAX
                          where bg.computation_basis == "L" && bg.active_status == "N"
                          orderby bg.tax_name
                          select new { c1 = bg.tax_code, c2 = bg.tax_name};

            ViewBag.tax = new SelectList(bglistb.ToList(), "c1", "c2");


            ViewBag.price = util.read_ledger("003", glay.vwstrarray0[11]);
            //ViewBag.price = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[11]);

            ViewBag.income = util.read_ledger("001", glay.vwstrarray0[14]);
           // ViewBag.income = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[14]);

            ViewBag.invent = util.read_ledger("004", glay.vwstrarray0[12]);
            //ViewBag.invent = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[12]);

            ViewBag.cos = util.read_ledger("002", glay.vwstrarray0[15]);
            //ViewBag.cos = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[15]);

            ViewBag.stock = util.read_ledger("005", glay.vwstrarray0[13]);
            //ViewBag.stock = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[13]);

        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_ITMG] where item_group_id=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }


        //private void pmtrix()
        //{
        //    var coyin = (from bg in db.GB_001_COY
        //                 where bg.id_code == "COYPRICE"
        //                 select bg).FirstOrDefault();

        //    glay.vwstrarray7 = new string[20];
        //    glay.vwstrarray7[0] = coyin.field6;
        //    glay.vwstrarray7[1] = coyin.field7;
        //    glay.vwstrarray7[2] = coyin.field8;
        //    glay.vwstrarray7[3] = coyin.field9;
        //    glay.vwstrarray7[4] = coyin.field10;
        //    glay.vwstrarray7[5] = coyin.field11;

        //}

        //private void header_ana()
        //{
        //    glay.vwstrarray4 = new string[20];
        //    glay.vwstrarray5 = new string[20];
        //    SelectList[] head_det = new SelectList[20];

        //   // Session["head_det"] = head_det;

        //    var bglist = from bg in db.GB_001_HEADER
        //                 where bg.header_type_code == "008" && bg.sequence_no != 99
        //                 select bg;

        //    foreach (var item in bglist.ToList())
        //    {
        //        int count2 = item.sequence_no;
        //        glay.vwstrarray4[count2] = item.header_code;
        //        var bglist2 = (from bg in db.GB_001_HANAL
        //                       where bg.header_sequence == item.header_code
        //                       select bg).FirstOrDefault();

        //        if (bglist2 != null)
        //        {
        //            glay.vwstrarray5[count2] = bglist2.header_description;
        //            var bglist3 = from bg in db.GB_001_DANAL
        //                          where bg.header_sequence == item.header_code
        //                          select bg;
        //            head_det[count2] = new SelectList(bglist3.ToList(), "analysis_code", "analysis_description", glay.vwstrarray6[count2]);

        //        }

        //    }

        //   // Session["head_det"] = head_det;

        //}
                 
    }
}
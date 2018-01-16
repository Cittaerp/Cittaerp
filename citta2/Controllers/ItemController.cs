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
    public class ItemController : Controller
    {
        IV_001_ITEM IV_001_ITEM = new IV_001_ITEM();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;
        HttpPostedFileBase photo2;

        bool err_flag = true;
        string move_auto = "N";
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            

            var bglist = from bh in db.IV_001_ITEM
                         join bk in db.GB_999_MSG
                         on new { a1 = "ITT", a2 = bh.item_type } equals new { a1 = bk.type_msg, a2 = bk.code_msg}
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         join bt in db.GB_001_PCODE
                         on new { a1 = bh.sku_sequence, a2 = "10"} equals new {a1 = bt.parameter_code, a2 = bt.parameter_type}
                         into bt1
                         from bt2 in bt1.DefaultIfEmpty()
                         join bl in db.IV_001_ITMST
                         on new { a1 = bh.item_code } equals new { a1=bl.item_code}
                         into bl1
                         from bl2 in bl1.DefaultIfEmpty()
                         select 
                         new vw_genlay
                         {
                             vwstring0 = bh.item_code,
                             vwstring1 = bh.item_name,   
                             vwstring2 = bt2.parameter_name,
                             vwstring3 = bk2.name1_msg,
                             vwdecimal0 = bl2.bal_qty == null ? 0 : bl2.bal_qty,
                             vwstring4 = bh.active_status == "N" ? "Active" : "Inactive"
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
            pmtrix();
            header_ana();
            select_query();
            cal_auto();
            psess.temp5 = move_auto;
            psess = (psess)Session["psess"];
            
            if (move_auto == "Y")
                glay.vwstring0 = "AUTO";
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1, string subcheck)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            photo2 = picture1;
            cal_auto();
            if (subcheck == "RR")
            {
                itempreload();
                pmtrix();
                header_ana();
                select_query();
                glay.vwstrarray2 = new string[50];
                return View(glay);
            }
           
            update_file();

            if (err_flag)
                return RedirectToAction("Create");
           
            initial_rtn();
            pmtrix();
            header_ana();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {
            psess = (psess)Session["psess"];
            
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            psess.temp5 = move_auto;
           IV_001_ITEM = db.IV_001_ITEM.Find(key1);
            if (IV_001_ITEM != null)
                read_record();
           
            pmtrix();
            header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile,HttpPostedFileBase picture1)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt=="D")
            { 
                delete_record();
                return RedirectToAction("Index");
            }

            photo1 = photofile;
            photo2 = picture1;

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            initial_rtn();
            pmtrix();
            header_ana();
            select_query();
            return View(glay);
        }

        private void delete_record()
        {
            IV_001_ITEM = db.IV_001_ITEM.Find(glay.vwstring0);
            if (IV_001_ITEM!=null)
            {
                db.IV_001_ITEM.Remove(IV_001_ITEM);
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
                IV_001_ITEM = new IV_001_ITEM();
                IV_001_ITEM.created_by = pubsess.userid;
                IV_001_ITEM.created_date = DateTime.UtcNow;
                //IV_001_ITEM.revalued_start_date = new DateTime(1990, 1, 1);
               
                IV_001_ITEM.last_purchase_cost = 0;
                IV_001_ITEM.last_purchase_date = "";
                IV_001_ITEM.last_purchase_vendor = "";
                if (move_auto == "Y")
                    glay.vwstring0 = util.autogen_num("ITM");
            }
            else
            {
                IV_001_ITEM = db.IV_001_ITEM.Find(glay.vwstring0);
            }
                IV_001_ITEM.attach_document = "";
                IV_001_ITEM.serial_number = "";
                IV_001_ITEM.item_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                IV_001_ITEM.item_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
                IV_001_ITEM.item_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
                IV_001_ITEM.description = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2]; 
                IV_001_ITEM.item_group = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
                IV_001_ITEM.preferred_vendor = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
                IV_001_ITEM.tax_inclusive = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
                IV_001_ITEM.item_costing_method = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
                IV_001_ITEM.issurance_method = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                //IV_001_ITEM.serial_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
                IV_001_ITEM.gl_price_var_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
                IV_001_ITEM.gl_inv_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
                IV_001_ITEM.gl_income_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
                IV_001_ITEM.gl_cos_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
                IV_001_ITEM.gl_stockcount_variance_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
                IV_001_ITEM.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
                IV_001_ITEM.price_matrix = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
                IV_001_ITEM.header_sequence = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
                IV_001_ITEM.discount_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
                IV_001_ITEM.tax = string.IsNullOrWhiteSpace(glay.vwstrarray0[17]) ? "" : glay.vwstrarray0[17];
                IV_001_ITEM.tax_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[18]) ? "" : glay.vwstrarray0[18];
                IV_001_ITEM.tax_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[19]) ? "" : glay.vwstrarray0[19];
                IV_001_ITEM.tax_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray0[20]) ? "" : glay.vwstrarray0[20];
                IV_001_ITEM.tax_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray0[21]) ? "" : glay.vwstrarray0[21];
                IV_001_ITEM.tax_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray0[22]) ? "" : glay.vwstrarray0[22];
                IV_001_ITEM.specification = string.IsNullOrWhiteSpace(glay.vwstrarray0[23]) ? "" : glay.vwstrarray0[23];
                IV_001_ITEM.gl_property = string.IsNullOrWhiteSpace(glay.vwstrarray0[24]) ? "" : glay.vwstrarray0[24];
                IV_001_ITEM.gl_comm_exp = string.IsNullOrWhiteSpace(glay.vwstrarray0[25]) ? "" : glay.vwstrarray0[25];
                IV_001_ITEM.gl_def_com_exp = string.IsNullOrWhiteSpace(glay.vwstrarray0[26]) ? "" : glay.vwstrarray0[26];
                IV_001_ITEM.item_group_maintenance = "";
                IV_001_ITEM.reorder_level = glay.vwdclarray0[1]; 
                IV_001_ITEM.weight_per_sku = glay.vwdclarray0[0];
                IV_001_ITEM.minimum_stock = glay.vwdclarray0[2];
                IV_001_ITEM.maximum_stock = glay.vwdclarray0[3];
                IV_001_ITEM.sku_sequence = glay.vwstring2;
                IV_001_ITEM.standard_cost = glay.vwdclarray0[6];
                IV_001_ITEM.reorder_quantity = glay.vwdclarray0[4];
                IV_001_ITEM.standard_lead_time = glay.vwint1;
                IV_001_ITEM.selling_price_class1 = glay.vwdclarray1[0];
                IV_001_ITEM.selling_price_class2 = glay.vwdclarray1[1];
                IV_001_ITEM.selling_price_class3 = glay.vwdclarray1[2];
                IV_001_ITEM.selling_price_class4 = glay.vwdclarray1[3];
                IV_001_ITEM.selling_price_class5 = glay.vwdclarray1[4];
                IV_001_ITEM.selling_price_class6 = glay.vwdclarray1[5];
                IV_001_ITEM.modified_date = DateTime.UtcNow;
                IV_001_ITEM.modified_by = pubsess.userid;
                IV_001_ITEM.active_status = glay.vwbool0 ? "Y" : "N";
                IV_001_ITEM.maintanance = glay.vwblarray0[0] ? "Y" : "N";
                IV_001_ITEM.sales = glay.vwblarray0[1] ? "Y" : "N";
                IV_001_ITEM.purchases = glay.vwblarray0[2] ? "Y" : "N";
                IV_001_ITEM.production = glay.vwblarray0[3] ? "Y" : "N";
                IV_001_ITEM.consumables = glay.vwblarray0[4] ? "Y" : "N";
                IV_001_ITEM.reusable = glay.vwblarray0[5] ? "Y" : "N";
                IV_001_ITEM.marketing = glay.vwblarray0[6] ? "Y" : "N";
                IV_001_ITEM.exchange_rate = glay.vwdecimal1;
                IV_001_ITEM.currency = string.IsNullOrWhiteSpace(glay.vwstrarray0[27]) ? "" : glay.vwstrarray0[27];
                
                IV_001_ITEM.title_ref_num = string.IsNullOrWhiteSpace(glay.vwstrarray1[0]) ? "" : glay.vwstrarray1[0];
                IV_001_ITEM.location_address = string.IsNullOrWhiteSpace(glay.vwstrarray1[1]) ? "" : glay.vwstrarray1[1];
                IV_001_ITEM.property_type = string.IsNullOrWhiteSpace(glay.vwstrarray1[2]) ? "" : glay.vwstrarray1[2];
                IV_001_ITEM.product_manager = string.IsNullOrWhiteSpace(glay.vwstrarray1[3]) ? "" : glay.vwstrarray1[3];
                IV_001_ITEM.agency_comm_flat = glay.vwdclarray2[0];
                IV_001_ITEM.agency_comm_per = glay.vwdclarray2[1];
                //IV_001_ITEM.property_acquisition = string.IsNullOrWhiteSpace(glay.vwstrarray1[4]) ? "" : glay.vwstrarray1[4];
                IV_001_ITEM.title_ref_des = string.IsNullOrWhiteSpace(glay.vwstrarray1[5]) ? "" : glay.vwstrarray1[5];
                //IV_001_ITEM.deposit_flat = glay.vwdclarray2[5];
                //IV_001_ITEM.deposit_percent = glay.vwdclarray2[6];
                IV_001_ITEM.owner_transfer = glay.vwdclarray2[2];
                IV_001_ITEM.Contact_sales_value = 0;
                IV_001_ITEM.num_installment = 0;
                IV_001_ITEM.installment_amt = 0;
                IV_001_ITEM.interest_rate = 0;
                IV_001_ITEM.penalty_amt = 0;
                IV_001_ITEM.interest_rate_pen = 0;
                IV_001_ITEM.installment_discrip = "";
                IV_001_ITEM.property_acquisition = "";
                IV_001_ITEM.deposit_flat = 0;
                IV_001_ITEM.deposit_percent = 0;
                IV_001_ITEM.installment_interval = 0;
                //if (glay.vwstrarray1[4] == "I")
                //{
                //    IV_001_ITEM.installment_interval = glay.vwdclarray2[2];
                //    IV_001_ITEM.installment_discrip = glay.vwstrarray1[6];
                //    IV_001_ITEM.num_installment = glay.vwdclarray2[3];
                //    IV_001_ITEM.installment_amt = glay.vwdclarray2[4];
                //  }
                //if (glay.vwstrarray1[4] == "M")
                //{
                //    IV_001_ITEM.Contact_sales_value = glay.vwdclarray2[7];
                //    IV_001_ITEM.interest_rate = glay.vwdclarray2[8];
                //    IV_001_ITEM.installment_amt = glay.vwdclarray2[9];
                //    IV_001_ITEM.num_installment = glay.vwdclarray2[10];
                //    IV_001_ITEM.penalty_amt = glay.vwdclarray2[11];
                //    IV_001_ITEM.interest_rate_pen = glay.vwdclarray2[12];
                //   }
                IV_001_ITEM.analysis_code1 = "";
                IV_001_ITEM.analysis_code2 = "";
                IV_001_ITEM.analysis_code3 = "";
                IV_001_ITEM.analysis_code4 = "";
                IV_001_ITEM.analysis_code5 = "";
                IV_001_ITEM.analysis_code6 = "";
                IV_001_ITEM.analysis_code7 = "";
                IV_001_ITEM.analysis_code8 = "";
                IV_001_ITEM.analysis_code9 = "";
                IV_001_ITEM.analysis_code10 = "";

                int arrlen = glay.vwstrarray6.Length;
                if (arrlen>0)
                    IV_001_ITEM.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                if (arrlen > 1)
                    IV_001_ITEM.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                if (arrlen > 2)
                    IV_001_ITEM.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                if (arrlen > 3)
                    IV_001_ITEM.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                if (arrlen > 4)
                    IV_001_ITEM.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                if (arrlen > 5)
                    IV_001_ITEM.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                if (arrlen > 6)
                    IV_001_ITEM.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                if (arrlen > 7)
                    IV_001_ITEM.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                if (arrlen > 8)
                    IV_001_ITEM.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                if (arrlen > 9)
                    IV_001_ITEM.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                psess.intemp0 = arrlen;
                Session["psess"] = psess;
                if (photo2 != null)
                {
                    if ((photo2 != null && Session["action_flag"].ToString() != "Create") || (Session["action_flag"].ToString() == "Create"))
                    {
                        byte[] uploaded = new byte[photo2.InputStream.Length];
                        photo2.InputStream.Read(uploaded, 0, uploaded.Length);
                        IV_001_ITEM.item_picture = uploaded;
                    }
                }
           if(action_flag == "Create")  
                db.Entry(IV_001_ITEM).State = EntityState.Added;
            else 
                db.Entry(IV_001_ITEM).State = EntityState.Modified;

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
                util.parameter_deleteflag("006", glay.vwstring0);
                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, IV_001_ITEM b where account_code in (gl_cos_code";
                //str += ",gl_income_code,gl_inv_code,gl_price_var_code,gl_stockcount_variance_code)";
                //str += " and item_code =" + util.sqlquote(glay.vwstring0);
                ////db.Database.ExecuteSqlCommand(str);

                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, IV_001_ITEM b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and item_code =" + util.sqlquote(glay.vwstring0);
                ////db.Database.ExecuteSqlCommand(stri);
               

                {
                    util.write_document("ITEM", IV_001_ITEM.item_code, photo1, glay.vwstrarray9);
                }

            }

        }

        private void validation_routine()
        {
            string error_msg = "";
            string taxbk = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            if (glay.vwstrarray0[17] == "Y")
            {
                bool blk_flag = true;
                bool dup_flag = false;
                for (int wctr = 18; wctr < 23; wctr++)
                {
                    if (!string.IsNullOrWhiteSpace(glay.vwstrarray0[wctr]))
                    {
                        blk_flag = false;
                        taxbk = glay.vwstrarray0[wctr];
                        for (int ictr = 18; ictr < 23; ictr++)
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



            if (string.IsNullOrWhiteSpace(glay.vwstring0) && move_auto != "Y")
            {
                ModelState.AddModelError(String.Empty, "Please enter Item ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[0]))
            {
                ModelState.AddModelError(String.Empty, "Name must not be spaces");
                err_flag = false;
            }
            //if (string.IsNullOrWhiteSpace(glay.vwstrarray0[2]))
            //{
            //    ModelState.AddModelError(String.Empty, "Please enter Description");
            //    err_flag = false;
            //}
            if (glay.vwstring2 == "")
            {
                ModelState.AddModelError(String.Empty, "Please enter unit of measure");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                IV_001_ITEM bnk = db.IV_001_ITEM.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Item ID already exist");
                    err_flag = false;
                }
            }


            string sqlstr = "select '1' query0 from IV_001_ITEM where item_name=" + util.sqlquote(glay.vwstrarray0[0]) + " and item_code <> " + util.sqlquote(glay.vwstring0);
            var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (bglist1 != null)
            {
                ModelState.AddModelError(String.Empty, "Can not accept duplicate Item Name");
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
            glay.vwstring0 = IV_001_ITEM.item_code;
            glay.vwstrarray0[0] = IV_001_ITEM.item_name;
            glay.vwstrarray0[2] = IV_001_ITEM.description;
            glay.vwstrarray0[1] = IV_001_ITEM.item_type;
            glay.vwstrarray0[3] = IV_001_ITEM.item_group;
            glay.vwstrarray0[13] = IV_001_ITEM.price_matrix;
            glay.vwstrarray0[14] = IV_001_ITEM.header_sequence;
            glay.vwdclarray0[1] = IV_001_ITEM.reorder_level;
            glay.vwstrarray0[16] = IV_001_ITEM.discount_code;
            glay.vwstrarray0[15] = "Y";
            if (string.IsNullOrWhiteSpace(IV_001_ITEM.discount_code))
                glay.vwstrarray0[15] = "N";

            glay.vwdclarray0[2] = IV_001_ITEM.minimum_stock;
            glay.vwdclarray0[5] = IV_001_ITEM.average_cost;
           // glay.picture12 = IV_001_ITEM.item_picture;
            glay.vwstrarray0[6] = IV_001_ITEM.item_costing_method;
            glay.vwstring1 = IV_001_ITEM.issurance_method;
            glay.vwstrarray0[7] = IV_001_ITEM.gl_price_var_code;
            glay.vwstrarray0[8] = IV_001_ITEM.gl_inv_code;
            glay.vwstrarray0[9] = IV_001_ITEM.gl_income_code;
            glay.vwstring2 = IV_001_ITEM.sku_sequence;
            glay.vwdclarray0[0] = IV_001_ITEM.weight_per_sku;
            glay.vwstrarray2[3] = util.date_slash(IV_001_ITEM.last_purchase_date);
            glay.vwdclarray0[4] = IV_001_ITEM.reorder_quantity;
            glay.vwstrarray0[10] = IV_001_ITEM.gl_cos_code;
            glay.vwstrarray0[11] = IV_001_ITEM.gl_stockcount_variance_code;
            glay.vwstrarray0[4] = IV_001_ITEM.preferred_vendor;
            //glay.vwstrarray0[7] = IV_001_ITEM.serial_number;
            glay.vwdclarray1[0] = IV_001_ITEM.selling_price_class1;
            glay.vwdclarray1[1] = IV_001_ITEM.selling_price_class2;
            glay.vwdclarray1[2] = IV_001_ITEM.selling_price_class3;
            glay.vwdclarray1[3] = IV_001_ITEM.selling_price_class4;
            glay.vwdclarray1[4] = IV_001_ITEM.selling_price_class5;
            glay.vwdclarray1[5] = IV_001_ITEM.selling_price_class6;
            glay.vwdclarray0[6] = IV_001_ITEM.standard_cost;
            glay.vwstrarray2[0] = IV_001_ITEM.last_purchase_cost.ToString("#,###.00");
            glay.vwdclarray0[3] = IV_001_ITEM.maximum_stock;
            glay.vwint1 = IV_001_ITEM.standard_lead_time;
            glay.vwstrarray0[12] = IV_001_ITEM.note;
            glay.vwstrarray0[5] = IV_001_ITEM.tax_inclusive;
            glay.vwstrarray2[1] = IV_001_ITEM.last_purchase_vendor;
            glay.vwstrarray0[18] = IV_001_ITEM.tax_code1;
            glay.vwstrarray0[19] = IV_001_ITEM.tax_code2;
            glay.vwstrarray0[20] = IV_001_ITEM.tax_code3;
            glay.vwstrarray0[21] = IV_001_ITEM.tax_code4;
            glay.vwstrarray0[22] = IV_001_ITEM.tax_code5;
            glay.vwstrarray0[23] = IV_001_ITEM.specification;
            glay.vwstrarray0[24] = IV_001_ITEM.gl_property;
            glay.vwstrarray0[25] = IV_001_ITEM.gl_comm_exp;
            glay.vwstrarray0[26] = IV_001_ITEM.gl_def_com_exp;
            glay.vwstrarray0[17] = IV_001_ITEM.tax;
            glay.vwstrarray0[27] = IV_001_ITEM.currency;
            glay.vwdecimal1 = IV_001_ITEM.exchange_rate;
            glay.vwstrarray6[0] = IV_001_ITEM.analysis_code1;
            glay.vwstrarray6[1] = IV_001_ITEM.analysis_code2;
            glay.vwstrarray6[2] = IV_001_ITEM.analysis_code3;
            glay.vwstrarray6[3] = IV_001_ITEM.analysis_code4;
            glay.vwstrarray6[4] = IV_001_ITEM.analysis_code5;
            glay.vwstrarray6[5] = IV_001_ITEM.analysis_code6;
            glay.vwstrarray6[6] = IV_001_ITEM.analysis_code7;
            glay.vwstrarray6[7] = IV_001_ITEM.analysis_code8;
            glay.vwstrarray6[8] = IV_001_ITEM.analysis_code9;
            glay.vwstrarray6[9] = IV_001_ITEM.analysis_code10;

            if (IV_001_ITEM.active_status == "Y")
                glay.vwbool0 = true;
            glay.vwblarray0[0] = IV_001_ITEM.maintanance == "Y" ? true : false;
            glay.vwblarray0[1] = IV_001_ITEM.sales == "Y" ? true : false;
            glay.vwblarray0[2] = IV_001_ITEM.purchases == "Y" ? true : false;
            glay.vwblarray0[3] = IV_001_ITEM.production == "Y" ? true : false;
            glay.vwblarray0[4] = IV_001_ITEM.consumables == "Y" ? true : false;
            glay.vwblarray0[5] = IV_001_ITEM.reusable == "Y" ? true : false;
            glay.vwblarray0[6] = IV_001_ITEM.marketing == "Y" ? true : false;

            glay.vwstrarray1[0] = IV_001_ITEM.title_ref_num;
            glay.vwstrarray1[1] = IV_001_ITEM.location_address;
            glay.vwstrarray1[2] = IV_001_ITEM.property_type;
            glay.vwstrarray1[3] = IV_001_ITEM.product_manager;
            glay.vwdclarray2[0] = IV_001_ITEM.agency_comm_flat;
            glay.vwdclarray2[1] = IV_001_ITEM.agency_comm_per;
            glay.vwstrarray1[4] = IV_001_ITEM.property_acquisition;
            glay.vwstrarray1[5] = IV_001_ITEM.title_ref_des;
            glay.vwdclarray2[5] = IV_001_ITEM.deposit_flat;
            glay.vwdclarray2[6] = IV_001_ITEM.deposit_percent;
            if (IV_001_ITEM.property_acquisition == "I")
            {
                glay.vwdclarray2[2] = IV_001_ITEM.installment_interval;
                glay.vwstrarray1[6] = IV_001_ITEM.installment_discrip;
                glay.vwdclarray2[3] = IV_001_ITEM.num_installment;
                glay.vwdclarray2[4] = IV_001_ITEM.installment_amt;
               }
            if (IV_001_ITEM.property_acquisition == "M")
            {
                glay.vwdclarray2[7] = IV_001_ITEM.Contact_sales_value;
                glay.vwdclarray2[8] = IV_001_ITEM.interest_rate;
                glay.vwdclarray2[9] = IV_001_ITEM.installment_amt;
                glay.vwdclarray2[10] = IV_001_ITEM.num_installment;
                glay.vwdclarray2[11] = IV_001_ITEM.penalty_amt;
                glay.vwdclarray2[12] = IV_001_ITEM.interest_rate_pen;
               }
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "ITEM" && bg.document_code == IV_001_ITEM.item_code 
                         orderby bg.document_sequence
                        select bg;

            ViewBag.anapict = bglist.ToList();
        
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
            glay.vwstrarray1 = new string[50];
            glay.vwdclarray2 = new decimal[20];
            glay.vwstrarray9 = new string[100];
            glay.vwstrarray0[1] = "I";
            glay.vwstrarray0[5] = "N";
            glay.vwstrarray0[14] = "N";
            glay.vwstrarray0[16] = "N";
            glay.vwstrarray0[18] = "N";
            glay.vwstrarray0[13] = "N";

            glay.vwstrarray0[15] = "N";
            glay.vwstrarray0[17] = "N";
            glay.vwstrarray0[27] = pubsess.base_currency_code;

            glay.vwstrarray0[6] = "A";
            glay.vwblarray0[1] = true;
            glay.vwblarray0[2] = true;
           
            glay.vwstring1 = "F";
           for ( int ctr = 0; ctr < 100; ctr++)
           {

                glay.vwstrarray9[ctr] = "";
           }

           string[] head_dat = new string[20];
           glay.vwlist0 = new List<querylay>[20];

          
        }
        private void select_query()
        {
            //var bglist = from bg in db.AR_001_MTRIX
            //             join bk in db.GB_001_HANAL
            //             on new { a1 = bg.header_sequence } equals new { a1 = bk.header_sequence }
            //             where bg.active_status == "N"
            //             orderby bk.header_description
            //             select new { b1 = bk.header_sequence, b2 = bk.header_description };

            //ViewBag.header = new SelectList(bglist.Distinct().ToList(), "b1", "b2", glay.vwstrarray0[14]);
            ViewBag.header = util.para_selectquery("50", glay.vwstrarray0[14], "N");

            ViewBag.vendor = util.para_selectquery("002", glay.vwstrarray0[4]);
            //var bglisti = from bg in db.GB_999_MSG
            //              where bg.type_msg == "ITU"
            //              orderby bg.name1_msg
            //              select bg;

            //ViewBag.itmuse = new SelectList(bglisti.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[23]);

            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[27]);

            //ViewBag.vendor = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[4]);
            //var bgitem = from bg in db.AP_001_VENDR
            //             where bg.active_status == "N"
            //             orderby bg.vend_biz_name
            //             select bg;

            //ViewBag.vendor = new SelectList(bgitem.ToList(), "vendor_code", "vend_biz_name", glay.vwstrarray0[4]);
            ViewBag.group = util.para_selectquery("12", glay.vwstrarray0[3],"N");

          
            var pick = from bg in db.GB_999_MSG
                       where bg.type_msg == "PTYPE"
                       select bg;
            ViewBag.propt = new SelectList(pick.ToList(), "code_msg", "name1_msg", glay.vwstrarray1[2]);

            ViewBag.unit = util.para_selectquery("10", glay.vwstring2, "N");
            ViewBag.propm = util.para_selectquery("62", glay.vwstrarray1[3]);
            //var bglistz = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "10" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //              select bg;

            //ViewBag.unit = new SelectList(bglistz.ToList(), "parameter_code", "parameter_name", glay.vwint2);

            //ViewBag.discount = util.para_selectquery("006", glay.vwstrarray0[16]);
           // ViewBag.discount = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[16]);

            var bglista = from bg in db.DC_001_DISC
                          where bg.active_status == "N"
                          orderby bg.discount_name
                          select new { c1 = bg.discount_code, c2 = bg.discount_name };

            ViewBag.discount = new SelectList(bglista.Distinct().ToList(), "c1", "c2", glay.vwstrarray0[16]);

            var bglistb = from bg in db.GB_001_TAX
                          where bg.computation_basis == "L" && bg.active_status == "N"
                          orderby bg.tax_name
                          select new { c1 = bg.tax_code, c2 = bg.tax_name};

            ViewBag.tax = new SelectList(bglistb.ToList(), "c1", "c2");

            ViewBag.price = util.read_ledger("003", glay.vwstrarray0[7]);
            ViewBag.income = util.read_ledger("001", glay.vwstrarray0[9]);
            ViewBag.invent = util.read_ledger("004", glay.vwstrarray0[8]);
            ViewBag.cos = util.read_ledger("002", glay.vwstrarray0[10]);
            ViewBag.stock = util.read_ledger("005", glay.vwstrarray0[11]);
            ViewBag.ptty = util.read_ledger("020", glay.vwstrarray0[24]);
            ViewBag.comexp = util.read_ledger("020", glay.vwstrarray0[25]);
            ViewBag.dcomexp = util.read_ledger("020", glay.vwstrarray0[26]);
            // ViewBag.price = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[7]);

           // bg2 = util.read_ledger("001");
           // ViewBag.income = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[9]);

           // bg2 = util.read_ledger("004");
           // ViewBag.invent = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[8]);

           //bg2 = util.read_ledger("002");
           // ViewBag.cos = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[10]);

           // bg2 = util.read_ledger("005");
           // ViewBag.stock = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[11]);

        }

        private void error_message()
        {

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
  


        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[IV_001_ITEM] where item_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }


        private void pmtrix()
        {
            var coyin = (from bg in db.GB_001_COY
                         where bg.id_code == "COYPRICE"
                         select bg).FirstOrDefault();

            glay.vwstrarray7 = new string[20];
            glay.vwstrarray7[0] = coyin.field6;
            glay.vwstrarray7[1] = coyin.field7;
            glay.vwstrarray7[2] = coyin.field8;
            glay.vwstrarray7[3] = coyin.field9;
            glay.vwstrarray7[4] = coyin.field10;
            glay.vwstrarray7[5] = coyin.field11;

        }

        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            glay.vwlist0 = new List<querylay>[20];

            SelectList[] head_det = new SelectList[20];

            //Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "008" && bg.sequence_no != 99
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

    public ActionResult show(string id)
        {
        var dir = "";
        IV_001_ITEM = db.IV_001_ITEM.Find(id);
        if (IV_001_ITEM != null && IV_001_ITEM.item_picture != null)
        {
            byte[] imagedata = IV_001_ITEM.item_picture;

            return File(imagedata, "png");
        }
        else
        {
            dir = Server.MapPath("~/image");
            var path = Path.Combine(dir, "noLogo.png"); //validate the path for security or use other means to generate the path.
            return File(path, "png");
        }
    }

    public ActionResult show_doc(int id)
    {
        var bglist = (from bg in db.GB_001_DOC
                      where bg.document_sequence == id
                      select bg).FirstOrDefault();

        if (bglist != null)
        {
            byte[] imagedata = bglist.document_image;
            return File(imagedata, "png");
        }
        return View();
    }
    private void cal_auto()
    {
        var autoset = (from bg in db.GB_001_COY
                       where bg.id_code == "COYAUTO"
                       select bg.field13).FirstOrDefault();

        if (autoset == "Y")
            move_auto = "Y";

    }
    
    private void itempreload()
    {
        string itemgroup = glay.vwstrarray0[3];
        
        ModelState.Clear();
         var itempre = (from bg in db.GB_001_ITMG
                        where bg.item_group_id == itemgroup
                            select bg).FirstOrDefault();
         glay.vwstrarray0[3] = itemgroup;
         glay.vwstrarray0[1] = itempre.item_type;
         glay.vwstring2 = itempre.sku_sequence;
        // glay.vwstrarray0[23] = itempre.item_group_maintenance;
         glay.vwstrarray0[6] = itempre.item_costing_method;
         glay.vwstrarray0[4] = itempre.preferred_vendor;
         glay.vwstrarray0[17] = itempre.tax;
         glay.vwstrarray0[18] = itempre.tax_code1;
         glay.vwstrarray0[19] = itempre.tax_code2;
         glay.vwstrarray0[20] = itempre.tax_code3;
         glay.vwstrarray0[21] = itempre.tax_code4;
         glay.vwstrarray0[22] = itempre.tax_code5;
         glay.vwstrarray0[7] = itempre.gl_price_var_code;
         glay.vwstrarray0[8] = itempre.gl_inv_code;
         glay.vwstrarray0[11] = itempre.gl_stockcount_variance_code;
         glay.vwstrarray0[9] = itempre.gl_income_code;
         glay.vwstrarray0[10] = itempre.gl_cos_code;
         glay.vwblarray0[0] = itempre.maintanance == "Y" ? true : false;
         glay.vwblarray0[1] = itempre.sales == "Y" ? true : false;
         glay.vwblarray0[2] = itempre.purchases == "Y" ? true : false;
         glay.vwblarray0[3] = itempre.production == "Y" ? true : false;
         glay.vwblarray0[4] = itempre.consumables == "Y" ? true : false;
         glay.vwblarray0[5] = itempre.reusable == "Y" ? true : false;
           
         //List<SelectListItem> ary = new List<SelectListItem>();
         //ary.Add(new SelectListItem { Value = "1", Text = itempre.item_type});
         //ary.Add(new SelectListItem { Value = "2", Text = itempre.sku_sequence });
         //ary.Add(new SelectListItem { Value = "3", Text = itempre.item_group_maintenance });
         //ary.Add(new SelectListItem { Value = "4", Text = itempre.item_costing_method });
         //ary.Add(new SelectListItem { Value = "5", Text = itempre.preferred_vendor });
         //ary.Add(new SelectListItem { Value = "6", Text = itempre.tax });
         //ary.Add(new SelectListItem { Value = "7", Text = itempre.tax_code1 });
         //ary.Add(new SelectListItem { Value = "8", Text = itempre.tax_code2 });
         //ary.Add(new SelectListItem { Value = "9", Text = itempre.tax_code3 });
         //ary.Add(new SelectListItem { Value = "10", Text = itempre.tax_code4 });
         //ary.Add(new SelectListItem { Value = "11", Text = itempre.tax_code5 });
         //ary.Add(new SelectListItem { Value = "12", Text = itempre.gl_price_var_code });
         //ary.Add(new SelectListItem { Value = "13", Text = itempre.gl_inv_code });
         //ary.Add(new SelectListItem { Value = "14", Text = itempre.gl_stockcount_variance_code });
         //ary.Add(new SelectListItem { Value = "15", Text = itempre.gl_income_code });
         //ary.Add(new SelectListItem { Value = "16", Text = itempre.gl_cos_code });

         //if (HttpContext.Request.IsAjaxRequest())
         //    return Json(new SelectList(
         //                    ary.ToArray(),
         //                    "Value",
         //                    "Text")
         //               , JsonRequestBehavior.AllowGet);
         ////}
         //return RedirectToAction("Index");


    }
    }
}
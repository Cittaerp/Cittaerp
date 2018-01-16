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
    public class FixedController : Controller
    {
        FA_001_ASSET FA_001_ASSET = new FA_001_ASSET();
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
            

            var bglist = from bh in db.FA_001_ASSET
                         select new vw_genlay
                         {
                             vwstring0 = bh.fixed_asset_code,
                             vwstring1 = bh.reference_asset_code,   
                             vwstring2 = bh.description,
                             vwstring3 = bh.asset_specs,
                             vwstring5 = bh.purchase_date,
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
           
            photo1 = photofile;
            photo2 = picture1;
            cal_auto();
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        // [HttpPost]
        //public ActionResult load_depreciation(string id)
        //{
        //    if (id == "02")
        //    {
        //        var cl = "Amotization";
                    

        //        var b = cl.ToList();
        //    }
        //    var dl = from bg in db.GB_999_MSG
        //             where bg.code_msg== "02"
        //             select new 
        //             {
        //                 c1 = bg.code_msg,
        //                 c2 = bg.name1_msg
        //             };

        //    var c = dl.ToList();

        //    if (HttpContext.Request.IsAjaxRequest())
        //        return Json(new SelectList(
        //                         dl.ToArray(),
        //                         "c1",
        //                       "c2"),
        //                    JsonRequestBehavior.AllowGet);
        //    return View();
        //}
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            psess.temp5 = move_auto;
            Session["psess"] = psess;
            initial_rtn();
            
            FA_001_ASSET = db.FA_001_ASSET.Find(key1);
            if (FA_001_ASSET != null)
                read_record();

            header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1)
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
            header_ana();
            select_query();
            return View(glay);
        }

        //[HttpPost]
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
            FA_001_ASSET = db.FA_001_ASSET.Find(glay.vwstring0);
            if (FA_001_ASSET!=null)
            {
                db.FA_001_ASSET.Remove(FA_001_ASSET);
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
                FA_001_ASSET = new FA_001_ASSET();
                FA_001_ASSET.created_by = pubsess.userid;
                FA_001_ASSET.created_date = DateTime.UtcNow;
                FA_001_ASSET.revalued_start_date = "";
                FA_001_ASSET.asset_cost = 0;
                FA_001_ASSET.depreciation_to_date = 0;
                FA_001_ASSET.revalued_cost = 0;
                FA_001_ASSET.revalued_useful_life = 0;
                FA_001_ASSET.depreciation_cost = 0;
                FA_001_ASSET.net_book_value = 0;
                FA_001_ASSET.group_flag = "";
               
                if (move_auto == "Y")
                    glay.vwstring0 = util.autogen_num("FA");
            }
            else
            {
                FA_001_ASSET = db.FA_001_ASSET.Find(glay.vwstring0);
            }
                FA_001_ASSET.attach_document = "";
                FA_001_ASSET.fixed_asset_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                FA_001_ASSET.reference_asset_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
                FA_001_ASSET.description = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1]; 
                FA_001_ASSET.parent_asset_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
                FA_001_ASSET.manufacturer = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
                FA_001_ASSET.model = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
                FA_001_ASSET.asset_specs = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
                FA_001_ASSET.supplier = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
                FA_001_ASSET.purchase_date = util.date_yyyymmdd(glay.vwstring5);
                FA_001_ASSET.comm_date = util.date_yyyymmdd(glay.vwstring6); 
                FA_001_ASSET.insurance_policy = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
                FA_001_ASSET.asset_tag = string.IsNullOrWhiteSpace(glay.vwstrarray0[25]) ? "" : glay.vwstrarray0[25];
                FA_001_ASSET.asset_manufacturers_num = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : util.date_yyyymmdd(glay.vwstring7);  
                FA_001_ASSET.asset_location = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
                FA_001_ASSET.asset_user = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
                FA_001_ASSET.department = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
                FA_001_ASSET.depreciation_type = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
                FA_001_ASSET.intial_useful_life = glay.vwint0;
                FA_001_ASSET.initial_start_date = util.date_yyyymmdd(glay.vwstring4);
                FA_001_ASSET.internal_end_date = DateTime.UtcNow.ToString("yyyyMMdd");
                FA_001_ASSET.internal_start_date = DateTime.UtcNow.ToString("yyyyMMdd");
                FA_001_ASSET.residual_value = glay.vwdecimal3;
                FA_001_ASSET.asset_requires_maintenace = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;  
                FA_001_ASSET.gl_asset_acc_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
                FA_001_ASSET.gl_accum_depn_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
                FA_001_ASSET.gl_depn_expense_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
                FA_001_ASSET.gl_disposal_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[15]) ? "" : glay.vwstrarray0[15];
                FA_001_ASSET.modified_date = DateTime.UtcNow;
                FA_001_ASSET.modified_by = pubsess.userid;
                FA_001_ASSET.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
                FA_001_ASSET.asset_detail = string.IsNullOrWhiteSpace(glay.vwstrarray0[17]) ? "" : glay.vwstrarray0[17];
                FA_001_ASSET.asset_class = string.IsNullOrWhiteSpace(glay.vwstrarray0[18]) ? "" : glay.vwstrarray0[18];

                FA_001_ASSET.clearing_account = string.IsNullOrWhiteSpace(glay.vwstrarray0[19]) ? "" : glay.vwstrarray0[19];
                FA_001_ASSET.reserve_account = string.IsNullOrWhiteSpace(glay.vwstrarray0[20]) ? "" : glay.vwstrarray0[20];
                FA_001_ASSET.asset_nature = string.IsNullOrWhiteSpace(glay.vwstrarray0[21]) ? "" : glay.vwstrarray0[21];
                FA_001_ASSET.dispose_asset = string.IsNullOrWhiteSpace(glay.vwstrarray0[22]) ? "" : glay.vwstrarray0[22];
                FA_001_ASSET.disposal_date = util.date_yyyymmdd(glay.vwstrarray0[23]);
                FA_001_ASSET.disposed = string.IsNullOrWhiteSpace(glay.vwstrarray0[24]) ? "" : glay.vwstrarray0[24];
                FA_001_ASSET.group_type_id = string.IsNullOrWhiteSpace(glay.vwstrarray0[26]) ? "" : glay.vwstrarray0[26];
                FA_001_ASSET.cumulative_amount = Convert.ToInt32(glay.vwdclarray0[1]);
                FA_001_ASSET.last_maintenance_date = string.IsNullOrWhiteSpace(glay.vwstrarray0[27]) ? "" : glay.vwstrarray0[27];
                FA_001_ASSET.unit_of_reading = string.IsNullOrWhiteSpace(glay.vwstrarray0[28]) ? "" : glay.vwstrarray0[28];
                if (glay.vwstring3 == "Y")
                    FA_001_ASSET.required_maintenance_val = Convert.ToInt32(glay.vwstrarray0[29]);
                else
                    FA_001_ASSET.required_maintenance_val = 0;


                FA_001_ASSET.active_status = glay.vwbool0 ? "Y" : "N";

                FA_001_ASSET.additional_cost = string.IsNullOrWhiteSpace(glay.vwstring9) ? "" : glay.vwstring9;

                FA_001_ASSET.revalue = string.IsNullOrWhiteSpace(glay.vwstring10) ? "" : glay.vwstring10;
                

                FA_001_ASSET.analysis_code1 = "";
                FA_001_ASSET.analysis_code2 = "";
                FA_001_ASSET.analysis_code3 = "";
                FA_001_ASSET.analysis_code4 = "";
                FA_001_ASSET.analysis_code5 = "";
                FA_001_ASSET.analysis_code6 = "";
                FA_001_ASSET.analysis_code7 = "";
                FA_001_ASSET.analysis_code8 = "";
                FA_001_ASSET.analysis_code9 = "";
                FA_001_ASSET.analysis_code10 = "";

                int arrlen = glay.vwstrarray6.Length;
                if (arrlen>0)
                    FA_001_ASSET.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                if (arrlen > 1)
                    FA_001_ASSET.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                if (arrlen > 2)
                    FA_001_ASSET.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                if (arrlen > 3)
                    FA_001_ASSET.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                if (arrlen > 4)
                    FA_001_ASSET.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                if (arrlen > 5)
                    FA_001_ASSET.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                if (arrlen > 6)
                    FA_001_ASSET.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                if (arrlen > 7)
                    FA_001_ASSET.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                if (arrlen > 8)
                    FA_001_ASSET.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                if (arrlen > 9)
                    FA_001_ASSET.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                psess.intemp0 = arrlen;
                Session["psess"] = psess;
                if (photo2 != null)
                {
                    if ((photo2 != null && Session["action_flag"].ToString() != "Create") || (Session["action_flag"].ToString() == "Create"))
                    {
                        byte[] uploaded = new byte[photo2.InputStream.Length];
                        photo2.InputStream.Read(uploaded, 0, uploaded.Length);
                        FA_001_ASSET.asset_picture = uploaded;
                    }
                }

           if(action_flag == "Create")  
                db.Entry(FA_001_ASSET).State = EntityState.Added;
            else 
                db.Entry(FA_001_ASSET).State = EntityState.Modified;

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
                util.parameter_deleteflag("005", glay.vwstring0);
                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, FA_001_ASSET b where account_code in (gl_asset_acc_code";
                //str += ",gl_accum_depn_code,gl_depn_expense_code,asset_class,gl_disposal_code)";
                //str += " and fixed_asset_code =" + util.sqlquote(glay.vwstring0);
                ////db.Database.ExecuteSqlCommand(str);

                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, FA_001_ASSET b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and fixed_asset_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(stri);

                {
                    util.write_document("FIXEDAST", FA_001_ASSET.fixed_asset_code, photo1, glay.vwstrarray9);
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
                    ModelState.AddModelError(String.Empty, "ID must not be spaces");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[18]))
            {
                ModelState.AddModelError(String.Empty, "Please select fixed asset class");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[11]))
            {
                ModelState.AddModelError(String.Empty, "Please enter depreciation type");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[1]))
                {
                    ModelState.AddModelError(String.Empty, "Description must not be spaces");
                err_flag = false;
            }
            //DateTime date_chk = DateTime.Now;
            //DateTime date_chki = DateTime.Now;
            //DateTime invaliddate = new DateTime(1000, 01, 01);
            //date_chk = util.date_yyyymmdd(glay.vwstring6);
            if (!string.IsNullOrWhiteSpace(glay.vwstring6))
            {
                if (!util.date_validate(glay.vwstring6))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid commission  date");
                    err_flag = false;
                }
            }
            //date_chki = util.date_yyyymmdd(glay.vwstring5);
            //if (date_chki == invaliddate)

            if (!util.date_validate(glay.vwstring5))
            {
                ModelState.AddModelError(String.Empty, "Please insert a valid purchase date");
                err_flag = false;
            }

            if (glay.vwstrarray0[22] == "Y")
            {
                if (!util.date_validate(glay.vwstrarray0[23]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid disposal date");
                    err_flag = false;
                }
            }
            Int32 t1 = 0;
            Int32.TryParse(glay.vwstring6, out t1);
            if (t1 != 0)
            {
                Int32 t2 = 0;
                Int32.TryParse(glay.vwstring5, out t2);
                if (t1 > t2)
                {
                    ModelState.AddModelError(String.Empty, "Commisioning date should not be earlier than purchase date");
                    err_flag = false;
                }
            }

            
                if (!util.date_validate(glay.vwstring4))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid depreciation start date");
                    err_flag = false;
                }
           

            string d1 = util.date_yyyymmdd(glay.vwstring5);
            string d2 = util.date_yyyymmdd(glay.vwstring4);
            if (d1 != "" && d2 != "")
            {
                if (Convert.ToInt32(d2) < Convert.ToInt32(d1))
                {
                    ModelState.AddModelError(String.Empty, "Depreciation start date should not be earlier than purchase date");
                    err_flag = false;
                }
            }
            if (glay.vwstring3 == "Y")
            {
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[26]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Maintenance Type");
                    err_flag = false;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[28]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Unit of Readings");
                    err_flag = false;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[29]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid Required Maintenance Value ");
                    err_flag = false;
                }
            }
            //if (string.IsNullOrWhiteSpace(glay.vwstring5))
            //    {
            //        ModelState.AddModelError(String.Empty, "Date of Purchase must not be spaces");
            //    err_flag = false;
            //}


            //if (string.IsNullOrWhiteSpace(glay.vwstring4))
            //    {
            //        ModelState.AddModelError(String.Empty, "Depreciation Start Date must not be spaces");
            //    err_flag = false;
            //}

           if(action_flag == "Create")
            {
                FA_001_ASSET bnk = db.FA_001_ASSET.Find(glay.vwstring0);
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

            if (glay.vwstring0 == "Y")
            {
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[19]))
                {
                    ModelState.AddModelError(String.Empty, "Please select GL Revaluation Clearing Account");
                    err_flag = false;
                }

                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[20]))
                {
                    ModelState.AddModelError(String.Empty, "Please select GL Revaluation Reserve Account");
                    err_flag = false;
                }


            }
         
        }

        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwdclarray0 = new decimal[10];
            glay.vwstrarray6 = new string[20];
           // glay.vwstring2 = "N";
            glay.vwbool0 = false;
            glay.vwstring0 = FA_001_ASSET.fixed_asset_code;
            glay.vwstrarray0[0] = FA_001_ASSET.reference_asset_code;
            glay.vwstrarray0[1] = FA_001_ASSET.description;
            glay.vwstrarray0[2] = FA_001_ASSET.parent_asset_code;
            glay.vwstrarray0[3] = FA_001_ASSET.manufacturer;
            glay.vwstrarray0[4] = FA_001_ASSET.model;
            glay.picture12 = FA_001_ASSET.asset_picture;
            glay.vwstrarray0[5] = FA_001_ASSET.asset_specs;
            glay.vwstrarray0[6] = FA_001_ASSET.supplier;
            glay.vwstring5 = util.date_slash(FA_001_ASSET.purchase_date);
            glay.vwstring6 = util.date_slash(FA_001_ASSET.comm_date);
            glay.vwstring3 = FA_001_ASSET.asset_requires_maintenace;
            glay.vwstring9 = FA_001_ASSET.additional_cost;
            glay.vwstring10 = FA_001_ASSET.revalue; 
            glay.vwstrarray0[7] = FA_001_ASSET.insurance_policy;
            glay.vwstring7 = FA_001_ASSET.asset_manufacturers_num; 
            glay.vwdecimal1 = FA_001_ASSET.asset_cost;
            glay.vwdecimal2 = FA_001_ASSET.depreciation_to_date;
            glay.vwstrarray0[8] = FA_001_ASSET.asset_location;
            glay.vwstrarray0[9] = FA_001_ASSET.asset_user;
            glay.vwstrarray0[10] = FA_001_ASSET.department;
            glay.vwstrarray0[11] = FA_001_ASSET.depreciation_type;
            glay.vwint0 = FA_001_ASSET.intial_useful_life;
            glay.vwdecimal3 = FA_001_ASSET.residual_value;
            glay.vwstring4 = util.date_slash(FA_001_ASSET.initial_start_date); 
            glay.vwstrarray0[12] = FA_001_ASSET.gl_asset_acc_code;
            glay.vwstrarray0[13] = FA_001_ASSET.gl_accum_depn_code;
            glay.vwstrarray0[14] = FA_001_ASSET.gl_depn_expense_code;
            glay.vwstrarray0[15] = FA_001_ASSET.gl_disposal_code;
            glay.vwstring8 = util.date_slash(FA_001_ASSET.revalued_start_date); 
            glay.vwdecimal4 = FA_001_ASSET.revalued_cost;
            glay.vwdecimal5 = FA_001_ASSET.net_book_value;
            glay.vwdecimal6 = FA_001_ASSET.depreciation_rate;
            glay.vwdclarray0[0] = FA_001_ASSET.depreciation_cost;
            glay.vwint1 = FA_001_ASSET.revalued_useful_life;
            glay.vwstrarray0[16] = FA_001_ASSET.note;
            glay.vwstrarray0[17] = FA_001_ASSET.asset_detail;
            glay.vwstrarray0[18] = FA_001_ASSET.asset_class;

            glay.vwstrarray0[19] = FA_001_ASSET.clearing_account;
            glay.vwstrarray0[20] = FA_001_ASSET.reserve_account;
            glay.vwstrarray0[21] = FA_001_ASSET.asset_nature;
            glay.vwstrarray0[22] = FA_001_ASSET.dispose_asset;
            glay.vwstrarray0[23] = util.date_slash(FA_001_ASSET.disposal_date);
            glay.vwstrarray0[24] = FA_001_ASSET.disposed;
            glay.vwstrarray0[25] = FA_001_ASSET.asset_tag;
            glay.vwstrarray0[26] = FA_001_ASSET.group_type_id;
            glay.vwstrarray0[27] = FA_001_ASSET.last_maintenance_date ;
            glay.vwstrarray0[28] = FA_001_ASSET.unit_of_reading;
            glay.vwstrarray0[29] = FA_001_ASSET.required_maintenance_val.ToString();
            glay.vwdclarray0[1] = FA_001_ASSET.cumulative_amount;
            glay.vwstrarray0[30] = FA_001_ASSET.group_flag;
            glay.vwstrarray6[0] = FA_001_ASSET.analysis_code1;
            glay.vwstrarray6[1] = FA_001_ASSET.analysis_code2;
            glay.vwstrarray6[2] = FA_001_ASSET.analysis_code3;
            glay.vwstrarray6[3] = FA_001_ASSET.analysis_code4;
            glay.vwstrarray6[4] = FA_001_ASSET.analysis_code5;
            glay.vwstrarray6[5] = FA_001_ASSET.analysis_code6;
            glay.vwstrarray6[6] = FA_001_ASSET.analysis_code7;
            glay.vwstrarray6[7] = FA_001_ASSET.analysis_code8;
            glay.vwstrarray6[8] = FA_001_ASSET.analysis_code9;
            glay.vwstrarray6[9] = FA_001_ASSET.analysis_code10;

            if (FA_001_ASSET.active_status == "Y")
                glay.vwbool0 = true;
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "FIXEDAST" && bg.document_code == FA_001_ASSET.fixed_asset_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();

        
        }

       private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwdtarray0 = new DateTime[10];
            glay.vwdclarray0 = new decimal[10];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstring2 = "N";
            glay.vwstring10 = "N";
            glay.vwstrarray0[22] = "N";
            glay.vwstrarray0[11] = "st";
            glay.vwstring3 = "N";
            glay.vwlist0 = new List<querylay>[20];

        }
        private void select_query()
        {

            var dp = from bg in db.GB_999_MSG
                         where bg.type_msg=="DPSA"
                         select bg;
            ViewBag.depp = new SelectList(dp.ToList(), "code_msg", "name1_msg", glay.vwstring9);

            var dg = from bg in db.AG_001_AMG
                     where bg.nature == "nat1"
                     select bg;
            ViewBag.grptype = new SelectList(dg.ToList(), "maintenance_group_type_id", "description", glay.vwstrarray0[26]);

           var asn = from bg in db.GB_999_MSG
                     where bg.type_msg == "ASN"
                     select bg;
            ViewBag.asnt = new SelectList(asn.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[21]);

            var df = from bg in db.GB_999_MSG
                     where bg.type_msg == "UOR"
                     select bg;
            ViewBag.uor = new SelectList(df.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[28]);

            ViewBag.parent_asset_code = util.para_selectquery("61", glay.vwstrarray0[2]);
           // ViewBag.parent_asset_code = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[2]);
            ViewBag.supplier = util.para_selectquery("002", glay.vwstrarray0[6]);
           // ViewBag.supplier = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[6]);

            //var bgitem = from bg in db.AP_001_VENDR
            //             where bg.active_status == "N"
            //             orderby bg.vend_biz_name
            //             select bg;

            //ViewBag.supplier = new SelectList(bgitem.ToList(), "vendor_code", "vend_biz_name", glay.vwstrarray0[6]);
            ViewBag.asset_location = util.para_selectquery("11", glay.vwstrarray0[8]);
            ViewBag.insurance = util.para_selectquery("53", glay.vwstrarray0[7]);
            //var bglisti = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "11" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //             select bg;

            //ViewBag.asset_location = new SelectList(bglisti.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[8]);
            ViewBag.asset_class = util.para_selectquery("15", glay.vwstrarray0[18]);
            //var bgl = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "15" && bg.active_status == "N"
            //          orderby bg.parameter_name
            //              select bg;

            //ViewBag.asset_class = new SelectList(bgl.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[18]);

            ViewBag.asset_user = util.para_selectquery("62", glay.vwstrarray0[9]);

            //var bglista = from bg in db.GB_001_EMP
            //              where bg.active_status == "N"
            //              orderby bg.name
            //             select bg;

            //ViewBag.asset_user = new SelectList(bglista.ToList(), "employee_code", "name", glay.vwstrarray0[9]);
            ViewBag.department = util.para_selectquery("05", glay.vwstrarray0[10]);
            //var bglistb = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "05" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //              select bg;

            //ViewBag.department = new SelectList(bglistb.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[10]);

            var bgitemi = from bg in db.GB_999_MSG
                          where bg.type_msg == "DEP"
                          orderby bg.name1_msg
                         select bg;

            ViewBag.depreciation = new SelectList(bgitemi.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[11]);

            ViewBag.chart = util.read_ledger("012");
            //ViewBag.chart = new SelectList(bg2.ToList(), "query0", "query1");


        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[FA_001_ASSET] where fixed_asset_code=" + util.sqlquote(id);
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

            //Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            //psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "005" && bg.sequence_no != 99
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
            FA_001_ASSET = db.FA_001_ASSET.Find(id);
            if (FA_001_ASSET != null && FA_001_ASSET.asset_picture != null)
            {
                byte[] imagedata = FA_001_ASSET.asset_picture;

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
                           select bg.field10).FirstOrDefault();

            if (autoset == "Y")
                move_auto = "Y";

        }
    }
}
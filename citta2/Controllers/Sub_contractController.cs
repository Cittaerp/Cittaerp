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
    public class Sub_contractController : Controller
    {
        SC_001_SCM SC_001_SCM = new SC_001_SCM();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;
        bool err_flag = true;
        string action_flag = "";
        // GET: Sub_contract
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            var bglist = from bd in db.SC_001_SCM
                         join bg in db.AP_001_VENDR
                         on bd.vendor_id equals bg.vendor_code
                         where bg.active_status == "N"
                         select new vw_genlay
                         {
                             vwstring0 = bd.sub_contract_id,
                             vwstring1 = bd.description,
                             vwstring2 = bg.vend_biz_name,
                             vwstring3 = "",
                             vwstring4 = bd.total_cost.ToString(),
                             vwstring5 = bd.duration,
                             vwstring6 = bd.inactive_status == "N" ? "Active" : "Inactive"
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
            select_query();
            return View(glay);
        }
        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, string subcheck, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            if (subcheck == "RR")
            {
                assign2();
                return View(glay);
            }
            update_file();
            if (err_flag)
                return RedirectToAction("Create");
            initial_rtn();
            select_query();
            return View(glay);
        }
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            SC_001_SCM = db.SC_001_SCM.Find(key1);
            if (SC_001_SCM != null)
                read_record();
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
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            //write your query statement
            string sqlstr = "delete from SC_001_SCM where sub_contract_id='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            SC_001_SCM = db.SC_001_SCM.Find(glay.vwstring0);
            if (SC_001_SCM != null)
            {
                db.SC_001_SCM.Remove(SC_001_SCM);
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

        private void assign2()
        {
            string val1 = glay.vwstring0;
            string val2 = glay.vwstring4;
            string val3 = glay.vwstring5;
            string val4 = glay.vwstring6;
            string get_id = glay.vwstring2;
            string get_id2 = glay.vwstring3;
            ModelState.Clear();
            glay.vwstring2 = get_id;
            glay.vwstring3 = get_id2;
            glay.vwstring0 = val1;
            glay.vwstring4 = val2;
            glay.vwstring5 = val3;
            glay.vwstring6 = val4;
            var bglist1 = from bd in db.AP_001_VENDR
                          where bd.active_status == "N"
                          select bd;
            ViewBag.Vendor = new SelectList(bglist1.ToList(), "vendor_code", "vend_biz_name", glay.vwstring2);

            var bgassign = (from bg in db.AP_002_PURHEAD
                            join bh in db.AP_001_VENDR
                            on new { a1 = bg.vendor_code } equals new { a1 = bh.vendor_code }
                            into bh1
                            from bh2 in bh1.DefaultIfEmpty()
                            join bi in db.AP_001_PTRAN
                            on new { a2 = bg.purchase_transaction_type } equals new { a2 = bi.purchase_order_code }
                            into bi1
                            from bi2 in bi1.DefaultIfEmpty()
                            where bg.vendor_code == glay.vwstring2 && bi2.purchase_order_class == "LPO"
                            select new { c1 = bi2.purchase_order_code, c2 = bi2.purchase_order_name });
            ViewBag.serviceorder = new SelectList(bgassign.ToList(), "c1", "c2", glay.vwstring3);

            if (glay.vwstring3 != null)
            {
                var bgassign2 = (from bg in db.AP_002_PURHEAD
                                 where bg.purchase_transaction_type == glay.vwstring3
                                 select bg).FirstOrDefault();

                glay.vwstring4 = bgassign2.invoice_total_amount.ToString();
                glay.vwstring8 = bgassign.FirstOrDefault().c1;
            }




        }

        private void update_record()
        {
           if(action_flag == "Create")
            {
                SC_001_SCM = new SC_001_SCM();
                SC_001_SCM.created_by = pubsess.userid;
                SC_001_SCM.created_date = DateTime.UtcNow;

            }
            else
            {
                SC_001_SCM = db.SC_001_SCM.Find(glay.vwstring0);
            }
            SC_001_SCM.sub_contract_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            SC_001_SCM.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            SC_001_SCM.vendor_id = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            SC_001_SCM.service_order_id = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            SC_001_SCM.contract_date = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            SC_001_SCM.total_cost = Convert.ToDecimal(glay.vwstring4);
            SC_001_SCM.contract_ref_no = string.IsNullOrWhiteSpace(glay.vwstring8) ? "" : glay.vwstring8;
            SC_001_SCM.duration = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            SC_001_SCM.modified_date = DateTime.UtcNow;
            SC_001_SCM.modified_by = pubsess.userid;
            SC_001_SCM.note = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            SC_001_SCM.inactive_status = glay.vwbool0 ? "Y" : "N";
           if(action_flag == "Create")
                db.Entry(SC_001_SCM).State = EntityState.Added;
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
                {
                    util.write_document("SCM", SC_001_SCM.sub_contract_id, photo1, glay.vwstrarray9);
                }

            }
        }


        private void select_query()
        {
            var bglist1 = from bd in db.AP_001_VENDR
                          where bd.active_status == "N"
                          select bd;
            ViewBag.Vendor = new SelectList(bglist1.ToList(), "vendor_code", "vend_biz_name", glay.vwstring2);

            var bgassign = (from bg in db.AP_002_PURHEAD
                            join bh in db.AP_001_VENDR
                            on new { a1 = bg.vendor_code } equals new { a1 = bh.vendor_code }
                            into bh1
                            from bh2 in bh1.DefaultIfEmpty()
                            join bi in db.AP_001_PTRAN
                            on new { a2 = bg.purchase_transaction_type } equals new { a2 = bi.purchase_order_code }
                            into bi1
                            from bi2 in bi1.DefaultIfEmpty()
                            where bg.vendor_code == glay.vwstring2 && bi2.purchase_order_class == "LPO"
                            select new { c1 = bi2.purchase_order_code, c2 = bi2.purchase_order_name });
            ViewBag.serviceorder = new SelectList(bgassign.ToList(), "c1", "c2", glay.vwstring3);
        }
        private void read_record()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwdclarray0 = new decimal[10];
            glay.vwstrarray6 = new string[20];
            glay.vwstring2 = "N";
            glay.vwbool0 = false;
            glay.vwstring0 = SC_001_SCM.sub_contract_id;
            glay.vwstring1 = SC_001_SCM.description;
            glay.vwstring2 = SC_001_SCM.vendor_id;
            glay.vwstring3 = SC_001_SCM.service_order_id;
            glay.vwstring4 = SC_001_SCM.total_cost.ToString();
            glay.vwstring8 = SC_001_SCM.contract_ref_no;
            glay.vwstring7 = SC_001_SCM.contract_date;
            glay.vwstring5 = SC_001_SCM.duration;
            glay.vwstring6 = SC_001_SCM.note;

            if (SC_001_SCM.inactive_status == "Y")
                glay.vwbool0 = true;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "SCM" && bg.document_code == SC_001_SCM.sub_contract_id
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();

        }

        private void initial_rtn()
        {
            glay.vwstring8 = "";
            glay.vwstrarray0 = new string[20];
            glay.vwdtarray1 = new DateTime[10];
            glay.vwdclarray2 = new decimal[10];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
        }

        [HttpPost]
        //    public ActionResult assign(string id)
        //    {

        //        var bgassign = (from bg in db.FA_001_ASSET
        //                        where bg.fixed_asset_code == id
        //                        select new { bg }).FirstOrDefault();

        //        string loc = bgassign.bg.asset_location;
        //        string ast_user = bgassign.bg.asset_user;

        //        List<SelectListItem> ary = new List<SelectListItem>();
        //        ary.Add(new SelectListItem { Value = "location", Text = loc });
        //        ary.Add(new SelectListItem { Value = "asset_user", Text = ast_user });


        //        if (HttpContext.Request.IsAjaxRequest())
        //            return Json(new SelectList(
        //                            ary.ToArray(),
        //                            "Value",
        //                            "Text")
        //                       , JsonRequestBehavior.AllowGet);

        //        return RedirectToAction("Index");

        //}

        private void validation_routine()
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "ID must not be spaces");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Description must not be spaces");
                err_flag = false;


            }

            if (string.IsNullOrWhiteSpace(glay.vwstring2))
            {
                ModelState.AddModelError(String.Empty, "Please Select a Vendor");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                SC_001_SCM bnk = db.SC_001_SCM.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }

                var bgassign = (from bg in db.SC_001_SCM
                                select bg.description).ToList();
                foreach (var item in bgassign)
                {
                    if (glay.vwstring1 == item)
                    {
                        ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                        err_flag = false;
                        break;
                    }
                }

            }
        }
    }
}

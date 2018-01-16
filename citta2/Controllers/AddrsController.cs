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
    public class AddrsController : Controller
    {
        AR_001_DADRS AR_001_DADRS = new AR_001_DADRS();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        string atype2 = "";

        bool err_flag = true;
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index(string atype="")
        {
            util.init_values();
            
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            if (atype != "")
            {
                psess.temp0 = atype;
            }

            atype2 = psess.temp0.ToString();

            header_rtn();
            if (atype2 == "CU")
            {
                psess.temp1 = "Customer";
                Session["psess"] = psess;
        
                var bglist = from bh in db.AR_001_DADRS
                             join bk in db.AR_001_CUSTM
                             on new { b1 = bh.customer_code } equals new { b1 = bk.customer_code }
                             into bk1
                             from bk2 in bk1.DefaultIfEmpty()
                             where bh.address_type==atype2
                             select new vw_genlay
                             {
                                 vwstring0 = bh.customer_code,
                                 vwint1 = bh.address_code,
                                 vwstring2 = bh.location_alias,
                                 vwstring4 = bh.active_status == "N" ? "Active" :"Inactive",
                                 vwstring3 = bk2.cust_biz_name
                             };
                return View(bglist.ToList());
            }
            else
            if (atype2 == "CY")
            {
                psess.temp1 = "Company";
                Session["psess"] = psess;
        
                var bglist = from bh in db.AR_001_DADRS
                             join bk6 in db.GB_001_COY
                             on new { b3 = bh.customer_code } equals new { b3 = bk6.field1 } //correct me
                             into bk7
                             from bk8 in bk7.DefaultIfEmpty()
                             where bh.address_type == atype2
                             select new vw_genlay
                             {
                                 vwstring0 = bh.customer_code,
                                 vwint1 = bh.address_code,
                                 vwstring2 = bh.location_alias,
                                 vwstring4 = bh.active_status == "N" ? "Active" :"Inactive",
                                 vwstring3 = bk8.field1
                             };
                return View(bglist.ToList());
            }

            else
                
            {
                psess.temp1 = "Vendor";
                Session["psess"] = psess;
        
                var bglist = from bh in db.AR_001_DADRS
                             join bk3 in db.AP_001_VENDR
                             on new { b2 = bh.customer_code } equals new { b2 = bk3.vendor_code }
                             into bk4
                             from bk5 in bk4.DefaultIfEmpty()
                             where bh.address_type == atype2
                             select new vw_genlay
                             {
                                 vwstring0 = bh.customer_code,
                                 vwint1 = bh.address_code,
                                 vwstring2 = bh.location_alias,
                                 vwstring4 = bh.active_status == "N" ? "Active" :"Inactive",
                                 vwstring3 = bk5.vend_biz_name
                             };
                return View(bglist.ToList());
            }

            }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            atype2 = psess.temp0.ToString();

            initial_rtn();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (vw_genlay glay_in)
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            atype2 = psess.temp0.ToString();

            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");
            initial_rtn();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1, int key2)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            atype2 = psess.temp0.ToString();

            AR_001_DADRS = db.AR_001_DADRS.Find(key1, atype2, key2);
            if (AR_001_DADRS != null)
                read_record();

            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            atype2 = psess.temp0.ToString();

            glay = glay_in;

            if (id_xhrt=="D")
            { //delete record
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
            AR_001_DADRS = db.AR_001_DADRS.Find(glay.vwstring0, atype2, glay.vwint1);
            if (AR_001_DADRS!=null)
            {
                db.AR_001_DADRS.Remove(AR_001_DADRS); 
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
                AR_001_DADRS = new AR_001_DADRS();
                AR_001_DADRS.created_by = pubsess.userid;
                AR_001_DADRS.created_date = DateTime.UtcNow;
                string sqlstr = "select isnull(max(address_code),0) intquery0 from AR_001_DADRS where address_type=" + util.sqlquote(atype2);
                sqlstr += " and customer_code=" + util.sqlquote(glay.vwstring0);
                var sql1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
                int addctr = sql1.intquery0 + 1;
                AR_001_DADRS.address_type = atype2;
                AR_001_DADRS.address_code = addctr;
            }
            else
            {
                AR_001_DADRS = db.AR_001_DADRS.Find(glay.vwstring0, atype2, glay.vwint1);
            }

           
            AR_001_DADRS.customer_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            AR_001_DADRS.address1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            AR_001_DADRS.address2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            AR_001_DADRS.city = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            AR_001_DADRS.postal_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            AR_001_DADRS.country = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
            AR_001_DADRS.state = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
            AR_001_DADRS.email = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            AR_001_DADRS.phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
            AR_001_DADRS.contact_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
            AR_001_DADRS.contact_dept = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
            AR_001_DADRS.contact_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
            AR_001_DADRS.contact_phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
            AR_001_DADRS.contact_job_title = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
            AR_001_DADRS.location_alias = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            AR_001_DADRS.modified_date = DateTime.UtcNow;
            AR_001_DADRS.modified_by = pubsess.userid;
            AR_001_DADRS.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
            AR_001_DADRS.active_status = glay.vwbool0 ? "Y" : "N";


            if (action_flag == "Create")
                db.Entry(AR_001_DADRS).State = EntityState.Added;
            else
                db.Entry(AR_001_DADRS).State = EntityState.Modified;
           
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

        }

        private void validation_routine()
        {
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, psess.temp0+"must not be spaces");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[0]))
            {
                ModelState.AddModelError(String.Empty, "Location Alias must not be spaces");
                err_flag = false;
            }
           if(action_flag == "Create")
            {
                string sqlstr = "select '1' query0 from AR_001_DADRS where upper(location_alias) =" + util.sqlquote(glay.vwstrarray0[0].ToUpper()) + " and address_type =" + util.sqlquote(atype2);
                var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
                if (bglist1 != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicate Location Alias");
                    err_flag = false;
                }
            }
          
           if(action_flag == "Create")
            {
                AR_001_DADRS bnk = db.AR_001_DADRS.Find(glay.vwstring0, atype2, glay.vwint1);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray0[5] = pubsess.country_operation;
           // glay.vwbool0 = true;
        }

        private void select_query()
        {
            if (atype2 == "CU")
            {
                ViewBag.code = util.para_selectquery("001", glay.vwstring0);
           

                //var bglista = from bg in db.AR_001_CUSTM
                //              where bg.active_status == "N"
                //              orderby bg.cust_biz_name
                //              select bg;

                //ViewBag.code = new SelectList(bglista.ToList(), "customer_code", "cust_biz_name", glay.vwstring0);
            }
            else
            if (atype2 == "CY")
            {
                var bglista = from bg in db.GB_001_COY
                              select bg;

                ViewBag.code = new SelectList(bglista.ToList(), "company_code", "company_name", glay.vwstring0);
            }
            else
            {
                ViewBag.code = util.para_selectquery("002", glay.vwstring0);

                //var bglista = from bg in db.AP_001_VENDR
                //              where bg.active_status == "N"
                //              orderby bg.vend_biz_name
                //              select bg;

                //ViewBag.code = new SelectList(bglista.ToList(), "vendor_code", "vend_biz_name", glay.vwstring0);
            }
            var bglist = from bg in db.GB_001_PCODE
                         where bg.parameter_type == "13" && bg.active_status == "N"
                         orderby bg.parameter_name
                         select bg;

//            ViewBag.country = new SelectList(bglist.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[5]);
            ViewBag.country = util.para_selectquery("13", glay.vwstrarray0[5]);

            //var bgitem = from bg in db.GB_001_PCODE
            //             where bg.parameter_type == "14" && bg.active_status == "N"
            //             orderby bg.parameter_name
            //             select bg;

            //ViewBag.state = new SelectList(bgitem.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[6]);

            //var bgitemi = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "05" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //              select bg;

            //ViewBag.depart = new SelectList(bgitemi.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[11]);
            ViewBag.depart = util.para_selectquery("05", glay.vwstrarray0[11]);

            //var bgiteme = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "04" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //              select bg;

            //ViewBag.job = new SelectList(bgiteme.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[14]);
            ViewBag.job = util.para_selectquery("04", glay.vwstrarray0[14]);

        }
        private void read_record()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwstring0 = AR_001_DADRS.customer_code;
            glay.vwint1 = AR_001_DADRS.address_code;
            glay.vwstrarray0[1] = AR_001_DADRS.address1;
            glay.vwstrarray0[2] = AR_001_DADRS.address2;
            glay.vwstrarray0[3] = AR_001_DADRS.city;
            glay.vwstrarray0[4] = AR_001_DADRS.postal_code;
            glay.vwstrarray0[5] = AR_001_DADRS.country;
            glay.vwstrarray0[6] = AR_001_DADRS.state;
            glay.vwstrarray0[7] = AR_001_DADRS.email;
            glay.vwstrarray0[8] = AR_001_DADRS.phone;
            glay.vwstrarray0[9] = AR_001_DADRS.note;
            glay.vwstrarray0[10] = AR_001_DADRS.contact_name;
            glay.vwstrarray0[11] = AR_001_DADRS.contact_dept;
            glay.vwstrarray0[12] = AR_001_DADRS.contact_email;
            glay.vwstrarray0[13] = AR_001_DADRS.contact_phone;
            glay.vwstrarray0[14] = AR_001_DADRS.contact_job_title;
            glay.vwstrarray0[0] = AR_001_DADRS.location_alias;
            if (AR_001_DADRS.active_status == "Y")
                glay.vwbool0 = true;
        
        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            id = psess.temp0.ToString() + "[]" + id;
            string sqlstr = "delete from [dbo].[AR_001_DADRS] where address_type+'[]' + customer_code+'[]'+cast(address_code as varchar(20))=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
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
        private void header_rtn()
        {

            if (atype2 == "CU")
            {
                psess.temp2 = "Customer Delivery Adresss Maintenance";
               // psess.temp2 = "Site Id";
                psess.temp3 = "Customer Delivery Adresss Listing";
            }
            if (atype2 == "VR")
            {
                psess.temp2 = "Vendor Branch Adresss Maintenance";
                psess.temp3 = "Vendor Branch Adresss Listing";
            }
            if (atype2 == "CY")
            {
                psess.temp2 = "Company Branch Adresss Maintenance";
                psess.temp3 = "Company Branch Adresss Listing";
            }
            Session["psess"] = psess;
        }
        //private void addr()
        //{
        //    var coy = (from bg in db.GB_001_COY
        //                 select bg).FirstOrDefault();
        //    var ven = (from gh in db.AP_001_VENDR
        //               select gh).FirstOrDefault();
        //    var cus = (from fg in db.AR_001_CUSTM
        //               select fg).FirstOrDefault();

        //    string atype = "";

        //    glay.vwstrarray4 = new string[20];
        //    glay.vwstrarray4[0] = coy.company_code;
        //    glay.vwstrarray4[1] = coy.company_name;
        //    glay.vwstrarray4[2] = ven.vendor_code;
        //    glay.vwstrarray4[3] = ven.vend_biz_name;
        //    glay.vwstrarray4[4] = cus.customer_code;
        //    glay.vwstrarray4[5] = cus.cust_biz_name;

        //     if (atype == com) {

        //         glay.vwstrarray4[0] = coy.company_code;
        //         coy = (from bg in db.AR_001_DADRS
        //                    where bg.customer_code ==  coy.company_code
        //                    select coy).FirstOrDefault();
        //     }
        //     if (atype == ved)
        //     {
        //         glay.vwstrarray4[2] = ven.vendor_code;

        //     }



        //}

	}
}
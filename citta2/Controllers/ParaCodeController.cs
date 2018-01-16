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
    public class ParaCodeController : Controller
    {
        GB_001_PCODE GB_001_PCODE = new GB_001_PCODE();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string ptype = "";
        bool err_flag = true;
        string delmsg = "";
        string action_flag = "";
        //
        // GET: /Citta/

        [EncryptionActionAttribute]
        public ActionResult Index(string ptype1 = "")
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            if (ptype1 != "")
            {
                psess.temp0 = ptype1;
            }

            ptype = psess.temp0.ToString();
            Session["psess"] = psess;
            header_rtn();
            if (ptype == "")
                error_message();

            var bglist = from bh in db.GB_001_PCODE
                         join bk in db.GL_001_CHART
                         on new { a1 = bh.gl_account_code } equals new { a1 = bk.account_code }
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         join bk3 in db.MC_001_CUREN
                         on new { a1 = bh.gl_account_code } equals new { a1 = bk3.currency_code }
                         into bk4
                         from bk5 in bk4.DefaultIfEmpty()
                         join bk6 in db.GB_001_PCODE
                         on new { a1 = "13", a2 = bh.con_state_link } equals new { a1 = bk6.parameter_type, a2 = bk6.parameter_code }
                         into bk7
                         from bk8 in bk7.DefaultIfEmpty()
                         join dg in db.GB_999_MSG
                         on new{a1 = "HEAD", a2 = bh.con_state_link} equals new {a1 = dg.type_msg, a2 = dg.code_msg}
                         into dg1 
                         from dg2 in dg1.DefaultIfEmpty()
                         where bh.parameter_type == ptype
                         select new vw_genlay
                         {
                             vwstring0 = bh.parameter_code,
                             vwstring1 = bh.parameter_name,

                             vwstring2 = bh.parameter_type == "03" ? bk2.account_name : bh.parameter_type == "13" ? bk5.currency_description : bh.parameter_type == "14" ? bk8.parameter_name : "",
                             vwstring3 = bh.note,
                             vwstring4 = bh.active_status == "N" ? "Active" : "Inactive",
                             vwstring7 = dg2.name1_msg
                         };

            return View(bglist.ToList());


        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(vw_genlay glay_in)
        {
            delete_record();
            db.SaveChanges();
            return View("index");
        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            ViewBag.plabel = "Site";
            psess = (psess)Session["psess"];

            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            GB_001_PCODE = db.GB_001_PCODE.Find(ptype, key1);
            if (GB_001_PCODE != null)
                read_record();

            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt == "D")
            { //delete record
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


        private void delete_record()
        {
            ptype = psess.temp0.ToString();
            if (util.delete_check(ptype, glay.vwstring0))
            {
                GB_001_PCODE = db.GB_001_PCODE.Find(ptype, glay.vwstring0);
                db.GB_001_PCODE.Remove(GB_001_PCODE);
                db.SaveChanges();
            }
            else
            {
                string kname = psess.temp2.ToString();
                delmsg = kname +"in Use";
                ModelState.AddModelError(String.Empty, delmsg);
                err_flag = false;

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
                GB_001_PCODE = new GB_001_PCODE();
                GB_001_PCODE.created_by = pubsess.userid;
                GB_001_PCODE.created_date = DateTime.UtcNow;
                //string sqlstr = "select isnull(max(parameter_code),0) vwint1 from GB_001_PCODE where parameter_type=" + ptype;
                //var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
                //glay.vwdecimal4 = sql1.vwint1 + 1;
                GB_001_PCODE.delete_flag = "N";
                
            }
            else
            {
                GB_001_PCODE = db.GB_001_PCODE.Find(ptype, glay.vwstring0);
            }
            GB_001_PCODE.parameter_type = ptype;
            //GB_001_PCODE.parameter_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            if (ptype != "13")
            {
                GB_001_PCODE.parameter_code = glay.vwstring0;  // Convert.ToInt16(glay.vwdecimal4);
               
            }
            GB_001_PCODE.parameter_name = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
           if(string.IsNullOrWhiteSpace(glay.vwstring1))
               GB_001_PCODE.parameter_name = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            GB_001_PCODE.gl_account_code = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GB_001_PCODE.con_state_link = "";
            if (ptype == "14")
                GB_001_PCODE.con_state_link = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            if (ptype == "16")
            {
                GB_001_PCODE.gl_account_code = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
                GB_001_PCODE.con_state_link = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            }
            GB_001_PCODE.modified_date = DateTime.UtcNow;
            GB_001_PCODE.modified_by = pubsess.userid;
            GB_001_PCODE.note = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            GB_001_PCODE.active_status = glay.vwbool0 ? "Y" : "N";
            GB_001_PCODE.attach_document = "";
            
            if (ptype == "01")
            {
                GB_001_PCODE.gl_account_code = glay.vwbool1 ? "Y" : "N";
            }
           if(action_flag == "Create")
                db.Entry(GB_001_PCODE).State = EntityState.Added;
            else
                db.Entry(GB_001_PCODE).State = EntityState.Modified;

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
                if (ptype == "01")
                {
                    if (glay.vwbool1)
                    {
                        string str = " update GB_001_PCODE set gl_account_code = 'N' where parameter_type = '01' and parameter_code !=" + Convert.ToInt16(glay.vwdecimal4);
                        db.Database.ExecuteSqlCommand(str);
                    }
                }
                if (ptype == "03")
                {
                    string dflag = glay.vwint0.ToString();
                    util.parameter_deleteflag("010", dflag);
                    //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, GB_001_PCODE b where a.account_code = b.gl_account_code";
                    //str += " and parameter_code =" + util.sqlquote(glay.vwstring0);
                    //db.Database.ExecuteSqlCommand(str);

                }
                {
                    string pcod = GB_001_PCODE.parameter_code.ToString();
                    util.write_document(ptype, pcod, photo1, glay.vwstrarray9);

                    //GB_001_DOC gdoc = new GB_001_DOC();
                    //gdoc.screen_code = ptype;
                    //gdoc.document_code = pcod;
                    //gdoc.description = "";
                    //gdoc.created_by = pubsess.userid;
                    //gdoc.modified_by = pubsess.userid;
                    //gdoc.created_date = DateTime.UtcNow;
                    //gdoc.modified_date = DateTime.UtcNow;
                    //gdoc.active_status = "Y";
                    //gdoc.note = "";
                    //foreach (var bg in photo1)
                    //{
                    //    if (bg != null)
                    //    {
                    //        byte[] uploaded = new byte[bg.InputStream.Length];
                    //        bg.InputStream.Read(uploaded, 0, uploaded.Length);
                    //        gdoc.document_image = uploaded;
                    //        db.Entry(gdoc).State = EntityState.Added;
                    //        db.SaveChanges();
                    //    }
                    //}
                    //if (glay.vwstrarray9 != null)
                    //{
                    //    for (int pctr = 0; pctr < glay.vwstrarray9.Length; pctr++)
                    //    {
                    //        if (!(string.IsNullOrWhiteSpace(glay.vwstrarray9[pctr])))
                    //        {
                    //            int seqno = Convert.ToInt16(glay.vwstrarray9[pctr]);
                    //            GB_001_DOC gdoc1 = db.GB_001_DOC.Find(seqno);

                    //            db.GB_001_DOC.Remove(gdoc1);
                    //            db.SaveChanges();
                    //        }
                    //    }
                    //}

                }

            }

        }

        private void validation_routine()
        {
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //{
            //    ModelState.AddModelError(String.Empty, "Pls enter id");
            //    err_flag = false;
            //}

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please enter name/description");
                err_flag = false;
            }
            if (ptype == "03")
            {
                if (string.IsNullOrWhiteSpace(glay.vwstring2))
                {
                    ModelState.AddModelError(String.Empty, "Please select GL account");
                    err_flag = false;
                }
            }
            if (ptype == "14")
            {
                if (string.IsNullOrWhiteSpace(glay.vwstring2))
                {
                    ModelState.AddModelError(String.Empty, "Please select Country");
                    err_flag = false;
                }
            }
          
           if(action_flag == "Create")
            {
                string sqlstr = "select '1' query0 from GB_001_PCODE where parameter_name =" + util.sqlquote(glay.vwstring1) + " and parameter_type = " + ptype;
                var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
                if (bglist1 != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicate description");
                    err_flag = false;
                }

                GB_001_PCODE bnk = db.GB_001_PCODE.Find(ptype, glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }


        private void select_query()
        {
            if (ptype == "03")
            {
                ViewBag.glaccount = util.read_ledger("001", glay.vwstring2);
                //var bg2 = util.read_ledger("001");
                //ViewBag.glaccount = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring2);

            }
            if (ptype == "13")
            {
                string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
                var emp1 = db.Database.SqlQuery<querylay>(str1);

                ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring2);

                //var bglist = from bh in db.MC_001_CUREN
                //             where bh.active_status == "N"
                //             orderby bh.currency_description
                //             select bh;

                //ViewBag.currency = new SelectList(bglist.ToList(), "currency_code", "currency_description", glay.vwstring2);
            }
            if (ptype == "14")
            {
                ViewBag.country = util.para_selectquery("13", glay.vwstring2,"N");
                //var bglist = from bh in db.GB_001_PCODE
                //             where bh.parameter_type == "13" && bh.active_status == "N"
                //             orderby bh.parameter_name
                //             select bh;

                //ViewBag.country = new SelectList(bglist.ToList(), "parameter_code", "parameter_name", glay.vwstring2);
            }
            if (ptype == "16")
            {
               // ViewBag.country = util.para_selectquery("13", glay.vwstring2, "N");
                var bglist = from bh in db.GB_999_MSG
                             where bh.type_msg == "DC"
                             orderby bh.code_msg
                             select bh;

                ViewBag.d_c = new SelectList(bglist.ToList(), "code_msg", "name1_msg", glay.vwstring2);
                var bglis = from bh in db.GB_999_MSG
                            where bh.type_msg == "HEAD" && bh.name6_msg == "P"
                             orderby bh.code_msg
                             select bh;

                ViewBag.screen = new SelectList(bglis.ToList(), "code_msg", "name1_msg", glay.vwstring4);
            }
            //if (ptype == "17")
            //{
            //    ViewBag.cust = util.para_selectquery("001", glay.vwstring1, "N");
            //    get_contract();
            //}
        }

        private void read_record()
        {
            glay.vwbool0 = false;
            glay.vwstring0 = GB_001_PCODE.parameter_code;
            glay.vwstring1 = GB_001_PCODE.parameter_name;
            glay.vwstring5 = GB_001_PCODE.parameter_name;
            glay.vwstring2 = GB_001_PCODE.gl_account_code;
            glay.vwstring3 = GB_001_PCODE.note;
            glay.vwstring4 = GB_001_PCODE.con_state_link;
            if (ptype == "14")
                glay.vwstring2 = GB_001_PCODE.con_state_link;
            if (GB_001_PCODE.active_status == "Y")
                glay.vwbool0 = true;
            string pcod = GB_001_PCODE.parameter_code.ToString();
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == ptype && bg.document_code == pcod
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();

        }



        [HttpPost]
        public ActionResult delete_list(string id)
        {
            err_flag = true;
            psess = (psess)Session["psess"];
            glay.vwstring0 = id;
            delete_record();

            if (!err_flag)
            {
                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "1", Text = delmsg });
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary.ToArray(),
                                    "Value",
                                    "Text")
                                   , JsonRequestBehavior.AllowGet);


            }
            return RedirectToAction("Index");
        }

        private void header_rtn()
        {

            if (ptype == "01")
            {
                psess.temp1 = "Site Maintainance";
                psess.temp2 = "Site Id";
                psess.temp4 = "Name";
                psess.temp5 = "Designate as Default Site?";
                psess.temp3 = "Site Listing";

            }
            if (ptype == "02")
            {
                psess.temp1 = "Bin Location Maintainance";
                psess.temp2 = "Bin Id";
                psess.temp4 = "Bin Location";
                psess.temp5 = "";
                psess.temp3 = "Bin Location Listing";

            }
            if (ptype == "03")
            {
                psess.temp1 = "Reason Code Maintainance";
                psess.temp2 = "Reason Id";
                psess.temp4 = "Description";
                psess.temp5 = "GL Account Code";
                psess.temp3 = "Reason Code Listing";
            }
            if (ptype == "04")
            {
                psess.temp1 = "Job Role Maintainance";
                psess.temp2 = "Job Id";
                psess.temp4 = "Description";
                psess.temp5 = "";
                psess.temp3 = "Job Role Listing";
            }
            if (ptype == "05")
            {
                psess.temp1 = "Department Maintainance";
                psess.temp2 = "Department Id";
                psess.temp4 = "Name";
                psess.temp5 = "";
                psess.temp3 = "Department Listing";

            }
            if (ptype == "10")
            {
                psess.temp1 = "Unit of Mearsure Maintenance";
                psess.temp2 = "Unit of Mearsure Id";
                psess.temp4 = "Name";
                psess.temp5 = "Status";
                psess.temp3 = "Unit of Mearsure Listing";
            }
            if (ptype == "11")
            {
                psess.temp1 = "Asset Location Maintainance";
                psess.temp2 = "Asset Location Id";
                psess.temp4 = "Location";
                psess.temp5 = "";
                psess.temp3 = "Asset Location Listing";
            }
            if (ptype == "12")
            {
                psess.temp1 = "Item Group Maintainance";
                psess.temp2 = "Item Group Id";
                psess.temp4 = "Description";
                psess.temp5 = "";
                psess.temp3 = "Item Group Listing";
            }
            if (ptype == "13")
            {
                psess.temp1 = "Country Maintainance";
                psess.temp2 = "Country Id";
                psess.temp4 = "Country";
                psess.temp5 = "Currency Id";
                psess.temp3 = "Country Listing";
            }
            if (ptype == "14")
            {
                psess.temp1 = "State Maintainance";
                psess.temp2 = "State Id";
                psess.temp4 = "State";
                psess.temp5 = "Country of State";
                psess.temp3 = "State Listing";
            }
            if (ptype == "15")
            {
                psess.temp1 = "Asset Class Maintainance";
                psess.temp2 = "Asset Class Id";
                psess.temp4 = "Description";
                psess.temp5 = "Status";
                psess.temp3 = "Asset Class Listing";
            }
            if (ptype == "16")
            {
                psess.temp1 = "Transaction Type Maintainance";
                psess.temp2 = "Transaction Type Id";
                psess.temp4 = "Description";
                psess.temp5 = "Debit/Credit";
                psess.temp3 = "Transaction Type Listing";
            }
            if (ptype == "17")
            {
                psess.temp1 = "Payment Option";
                psess.temp2 = "ID ";
                psess.temp4 = "Description";
                psess.temp5 = "";
                psess.temp3 = "Payment Option Listing";
            }
            if (ptype == "18")
            {
                psess.temp1 = "Purpose Option";
                psess.temp2 = "ID ";
                psess.temp4 = "Description";
                psess.temp5 = "";
                psess.temp3 = "Purpose Option Listing";
            }
            if (ptype == "19")
            {
                psess.temp1 = "Admin Charge";
                psess.temp2 = "ID ";
                psess.temp4 = "Percentage Charge";
                psess.temp5 = "";
                psess.temp3 = "Percentage Charge Listing";
            }
            Session["psess"] = psess;
            
        }

        private void get_contract()
        {
         
            string str = "select contract_id query0, leeds_name+ '-' + contract_id query1 from [dbo].[IV_001_PC] where leeds_name <> '' union all";
            str += " select contract_id query0, cust_biz_name+ '-' + contract_id query1 from [dbo].[IV_001_PC], [dbo].[AR_001_CUSTM] where customer_id = customer_code";
            var emp1 = db.Database.SqlQuery<querylay>(str);
            ViewBag.cid = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring0);

          }
     
        private void error_message()
        {

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


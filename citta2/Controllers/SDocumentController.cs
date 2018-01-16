using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
//using CittaErp.Helpers;
using CittaErp.utilities;
//using CittaErp.Filters;

namespace CittaErp.Controllers
{
    public class SDocumentController : Controller
    {
        private MainContext db = new MainContext();
        Boolean err_flag = true;
        string laction = "";
///      static DataClasses1DataContext context = new DataClasses1DataContext();
        string type_code = "";
        comsess pblock;
        psess psess;
        tab_document tab_document = new tab_document();
        vw_collect mcollect = new vw_collect();
        cittautil utils = new cittautil();
        string err_message;

        //
        // ws_code doc code
        //ws_string0 bank name
        // tx_string0[0] doc_name

        //[EncryptionActionAttribute]
        public ActionResult Index(string pc = null)
        {
            //if (utils.check_option() == 1||pc==null)
            //    return RedirectToAction("Welcome", "Home");


            psess.temp1 = "Document Template";
            psess.temp5 = "DOC";
            Session["psess"] = psess;

            var blist = from d in db.tab_document
                        select d;

            return View(blist.ToList());
        }

        //
        // GET: /Nation/Create

        //[EncryptionActionAttribute]
        public ActionResult Create(string xy = null)
        {
            mcollect.datmode = "C";
           //if (utils.check_option() == 1 || xy != "1")
           //     return RedirectToAction("Welcome", "Home");
 
            pblock = (comsess)Session["comsess"];
            bank_query();
            psess.temp4 = "1";
            Session["psess"] = psess;

            return View("Edit", mcollect);
        }

        //
        // POST: /Nation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_collect mcollect1)
        {
            pblock = (comsess)Session["comsess"];
            mcollect = mcollect1;

            laction = "Create";

            write_data();
            if (err_flag)
                return RedirectToAction("Create");//, null, new { anc = Ccheckg.convert_pass2("xy=1") });

            bank_query();
            mcollect.datmode = "C";
            return View("Edit", mcollect);
        }

        //
        // GET: /Nation/Edit/5

        //[EncryptionActionAttribute]
        public ActionResult Edit(string jy = null)
        {
            mcollect.datmode = "E";
            //if (utils.check_option() == 1 || jy == null)
            //    return RedirectToAction("Welcome", "Home");

            pblock = (comsess)Session["comsess"];

            tab_document = db.tab_document.Find( jy);
            if (tab_document == null)
            {
                ModelState.AddModelError(String.Empty, "Document not found.");
                return RedirectToAction("Index");//, null, new { anc = Ccheckg.convert_pass2("pc=") });
            }

            read_daily();
            bank_query();
            psess.temp4 = "1";
            Session["psess"] = psess;

            return View(mcollect );
        }

        //
        // POST: /Nation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_collect mcollect1, string xhrt)
        {
            pblock = (comsess)Session["comsess"];
            mcollect = mcollect1;

            if (xhrt == "D")
            {
                DeleteConfirmed();
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            write_data();
            if (err_flag)
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});

            bank_query();
            mcollect.datmode = "E";
            return View(mcollect);

        }


        private void DeleteConfirmed()
        {
            tab_document = db.tab_document.Find( mcollect.ws_code);
            write_log("Delete");
            db.tab_document.Remove(tab_document);
            db.SaveChanges();
            //return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void write_data()
        {
            err_flag = true;
            validate_fields();

            if (err_flag)
                write_rec();
        }

        private void write_rec()
        {
            DateTime odate = utils.logdatetime();
            if (laction == "Create")
            {
                tab_document = new tab_document();
                tab_document.created_by = pblock.userid;
                tab_document.date_created = odate;
            }
            else
            {
                tab_document = db.tab_document.Find(mcollect.ws_code);
            }

            pblock = (comsess)Session["comsess"];
            err_flag = true;
            tab_document.doc_code = mcollect.ws_code;
            tab_document.name1 = mcollect.ws_string0;
            tab_document.doc_text = mcollect.tx_string0[0];
            tab_document.amended_by = pblock.userid;
            tab_document.date_amended = odate;
            tab_document.doc_type = mcollect.ws_string1;
            tab_document.numeric_size = 0;
            tab_document.right_margin = 0;
            
            if (laction == "Create")
                db.tab_document.Add(tab_document);
            else
                db.Entry(tab_document).State = EntityState.Modified;

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
                write_log(laction);

        }

        private void validate_fields()
        {
            err_message = "";
            if (string.IsNullOrWhiteSpace(mcollect.ws_code))
                err_message = "Code can not be Spaces";
            if (string.IsNullOrWhiteSpace(mcollect.ws_string0))
                err_message = "Name can not be Spaces";
            if (string.IsNullOrWhiteSpace(mcollect.ws_string1))
                err_message = "Select Document Based on Option";
            
            if (laction == "Create")
            {
                tab_document cbank = db.tab_document.Find(mcollect.ws_code);
                if (cbank != null)
                    err_message = "Record Already Created";
            }

            if (err_message != "")
            {
                ModelState.AddModelError(String.Empty, err_message);
                err_flag = false;
            }
            
        }

        private void read_daily()
        {
            mcollect.tx_string0 = new string[10];
            mcollect.ws_code= tab_document.doc_code;
            mcollect.ws_string0 = tab_document.name1;
            mcollect.tx_string0[0] = tab_document.doc_text;
            mcollect.ws_string1 = tab_document.doc_type;
        }

        private void bank_query()
        {
            var bglist2 = (from bg in db.tab_soft
                           where (bg.para_code == "QUERY" && bg.report_code == "SDOC")
                           select bg).First();
            var report_name2 = bglist2.report_name2;
            psess.temp3 = bglist2.report_name3;
            Session["psess"] = psess;
            report_name2 = report_name2.Replace("'", "");

            string[] opt_str;
            string[] sepa_str = new string[] { "," };

            opt_str = report_name2.Split(sepa_str, StringSplitOptions.None);

            var bglist3 = from bg2 in db.tab_soft
                          where (bg2.para_code == "QUERY" && opt_str.Contains(bg2.report_code))
                          orderby bg2.numeric_ind
                          select bg2;

            ViewBag.folders = new SelectList(bglist3.ToList(), "report_code", "report_name1");

            var bhlist1 = from bg in db.tab_soft
                          where bg.para_code == "DOCBASE"
                          orderby bg.report_name1
                          select bg;
            ViewBag.based = new SelectList(bhlist1.ToList(), "report_code", "report_name1", mcollect.ws_string1);

        }

        private void write_log(string option)
        {
            string opt;
            if (option == "Create")
                opt = "N";
            else if (option == "Delete")
                opt = "D";
            else
                opt = "A";

            string select1 = " where doc_code=" + utils.pads(tab_document.doc_code);
            utils.write_plog("DOC", select1, opt, "A", pblock.userid);

        }

        [HttpPost]
        public ActionResult DailyList(string idx)
        {
            List<SelectListItem> ary = new List<SelectListItem>();
            var str2 = utils.listdaily(idx);
            foreach (var item in str2.ToList())
            {
                ary.Add(new SelectListItem { Text = item.c2, Value = item.c1 });
            }
            ary.Add(new SelectListItem { Text = "operand", Value = "operand" });

            str2 = utils.listdaily(idx);
            foreach (var item in str2.ToList())
            {
                ary.Add(new SelectListItem { Text = item.c2, Value = item.c1 });
            }
            ary.Add(new SelectListItem { Text = "source", Value = "source" });

            str2 = utils.listdaily(idx);
            foreach (var item in str2.ToList())
            {
                ary.Add(new SelectListItem { Text = item.c2, Value = item.c1 });
            }
            ary.Add(new SelectListItem { Text = "period", Value = "period" });



            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");//, null, new { anc = Ccheckg.convert_pass2("pc=") });
        }

        [HttpPost]
        public ActionResult Daily2List(string idx)
        {
            // only for hr calculation and appraisal and when selection is transaction type and performance pages
            var str2 = utils.listdaily2(idx);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");//, null, new { anc = Ccheckg.convert_pass2("pc=") });
        }

        [HttpPost]
        public ActionResult Daily3List(string idx)
        {
            psess = (psess)Session["psess"];
            psess.temp6 = idx;
            Session["psess"] = psess;

            var str2 = utils.listdaily3(idx);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");//, null, new { anc = Ccheckg.convert_pass2("pc=") });

        }

        [HttpPost]
        public ActionResult hideList(string idx)
        {
            if (idx == "S")
                psess.temp0 = "13";
            else if (idx == "T")
                psess.temp0 = "14";
            else if (idx == "HS")
                psess.temp0 = "H13";
            else if (idx == "HT")
                psess.temp0 = "H15";
            else if (idx == "PF")
                psess.temp0 = "HA07";
            else if (idx == "P")
                psess.temp0 = "P04";
            else if (idx == "HE")
                psess.temp0 = "H46";
            else if (idx == "FT")
                psess.temp0 = "F13";
            else if (idx == "FS")
                psess.temp0 = "F17";
            else if (idx == "FM")
                psess.temp0 = "F18";
            else
                psess.temp0 = "13";

            string spc = psess.temp0.ToString();

            var bglist2 = (from bg in db.tab_soft
                           where (bg.para_code == "QUERY" && bg.report_code == spc)
                           select bg).First();
            var report_name2 = bglist2.report_name2;
            psess.temp3 = bglist2.report_name3;
            report_name2 = report_name2.Replace("'", "");

            string[] opt_str;
            string[] sepa_str = new string[] { "," };

            opt_str = report_name2.Split(sepa_str, StringSplitOptions.None);

            var bglist3 = from bg2 in db.tab_soft
                          where (bg2.para_code == "QUERY" && opt_str.Contains(bg2.report_code))
                          orderby bg2.numeric_ind
                          select bg2;

            //ViewBag.folders = new SelectList(bglist3.ToList(), "report_code", "report_name1");


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                bglist3.ToList(),
                                "report_code",
                                "report_name1")
                           , JsonRequestBehavior.AllowGet);

            Session["psess"] = psess;
            return RedirectToAction("Index");//, null, new { anc = Ccheckg.convert_pass2("pc=") });

        }

    }
}
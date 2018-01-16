using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;

namespace CittaErp.Controllers
{
    public class TaxController : Controller
    {
        //
        // GET: /Tax/
        GB_001_TAX GB_001_TAX = new GB_001_TAX();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string ptype = "";
        bool err_flag = true;
        string action_flag = "";
        
        public ActionResult Index(string ptype1 = "11")
        {
        
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            var bglist = from bh in db.GB_001_TAX
                         select new vw_genlay
                         {
                             vwstring0 = bh.tax_code,
                             vwstring1 = bh.tax_name,
                             vwdecimal0 = bh.tax_rate,
                         };
            return View(bglist.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];

            initial_rtn();
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_TAX] where tax_code=" + util.sqlquote(id);
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult load_GL(string id)
        {

            if (id == "val")
            {
                var c1 = util.read_ledger("017", "");

                if (HttpContext.Request.IsAjaxRequest())
                    return Json(c1,
                                JsonRequestBehavior.AllowGet);

            }

            string gl = (from bh in db.AP_001_VENDR
                         where bh.vendor_code == id
                         select bh.gl_account_code).FirstOrDefault();

            var cl = from bg in db.GL_001_CHART
                     where bg.account_code == gl
                     select new
                     {
                         c1 = bg.account_code,
                         c2 = bg.account_name
                     };

            var b = cl.ToList();

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                 cl.ToArray(),
                                 "c1",
                               "c2"),
                            JsonRequestBehavior.AllowGet);

            return View();
        }

        public ActionResult Edit(string key1)
        {
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            GB_001_TAX = db.GB_001_TAX.Find(key1);
            if (GB_001_TAX != null)
                read_record();
            select_query();
            return View(glay);
        }
        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
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
        private void delete_record()
        {
            GB_001_TAX = db.GB_001_TAX.Find(glay.vwstring0);
            if (GB_001_TAX != null)
            {
                db.GB_001_TAX.Remove(GB_001_TAX);
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
                GB_001_TAX = new GB_001_TAX();
                GB_001_TAX.created_by = pubsess.userid;
                GB_001_TAX.created_date = DateTime.UtcNow;
            }
            else
            {
                GB_001_TAX = db.GB_001_TAX.Find(glay.vwstring0);
            }
            GB_001_TAX.attach_document = "";
            GB_001_TAX.tax_code = glay.vwstring0;
            GB_001_TAX.tax_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            GB_001_TAX.tax_rate = (glay.vwdecimal0);
            GB_001_TAX.tax_reg_id = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            GB_001_TAX.tax_agency = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            GB_001_TAX.tax_impact = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            GB_001_TAX.module_basis = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            GB_001_TAX.computation_basis = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];

            GB_001_TAX.reclaimable = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            GB_001_TAX.salestax_payable_recognition = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GB_001_TAX.purchasetax_payable_recognition = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            GB_001_TAX.salestax_paytime = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;

            GB_001_TAX.gl_tax_acc_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];

            GB_001_TAX.modified_date = DateTime.UtcNow;
            GB_001_TAX.modified_by = pubsess.userid;
            GB_001_TAX.active_status = glay.vwbool0 ? "Y" : "N";
            GB_001_TAX.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            //GB_001_TAX.attach_document = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "Y" : glay.vwstrarray0[12];


           if(action_flag == "Create")
                db.Entry(GB_001_TAX).State = EntityState.Added;
            else
                db.Entry(GB_001_TAX).State = EntityState.Modified;

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
                util.parameter_deleteflag("011", glay.vwstring0);
                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, GB_001_TAX b where a.account_code = b.gl_tax_acc_code";
                //str += " and tax_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(str);

                {
                    util.write_document("TAX", GB_001_TAX.tax_code, photo1, glay.vwstrarray9);
                }

            }


        }
        private void validation_routine()
        {
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //    error_msg = "Please enter ID";


            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please select Tax ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[3]))
            {
                ModelState.AddModelError(String.Empty, "Please enter Tax impact on transaction");
                err_flag = false;
            }

            if (glay.vwdecimal0==0)
            {
                ModelState.AddModelError(String.Empty, "Rate can not be zero");
                err_flag = false;
            }

            if (action_flag == "Create")
            {
                GB_001_TAX bnk = db.GB_001_TAX.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }

        private void select_query()
        {
            string str1;
            if(glay.vwstrarray0[1]=="val")
                ViewBag.cart = util.read_ledger("017", glay.vwstrarray0[8]);
            else
            {
                 str1 = "select account_code query0, account_name query1 from GL_001_CHART where account_code=" + util.sqlquote(glay.vwstrarray0[8]);
                var bglist = db.Database.SqlQuery<querylay>(str1);
                ViewBag.cart = new SelectList(bglist.ToList(), "query0", "query1", glay.vwstrarray0[8]);
            }
            //var bg2 = util.read_ledger("017");
            //ViewBag.cart = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[6]);


            var taxi = from cd in db.GB_999_MSG
                       where cd.type_msg == "TAXI"
                       orderby cd.name1_msg
                       select cd;
            ViewBag.tax = new SelectList(taxi.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[3]);

            //var mob = from cf in db.GB_999_MSG
            //          where cf.type_msg == "MOD"
            //          select cf;
            //ViewBag.mo = new SelectList(mob.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[4]);

            var comp = from ce in db.GB_999_MSG
                       where ce.type_msg == "COMP"
                       orderby ce.name1_msg
                       select ce;
            ViewBag.com = new SelectList(comp.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[5]);

            var spr = from cg in db.GB_999_MSG
                      where cg.type_msg == "SPR"
                      select cg;
            ViewBag.sp = new SelectList(spr.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[8]);


            var sp = from ch in db.GB_999_MSG
                     where ch.type_msg == "SP"
                     select ch;
            ViewBag.ps = new SelectList(sp.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[9]);

            var ppr = from ci in db.GB_999_MSG
                      where ci.type_msg == "SP"
                      orderby ci.name1_msg
                      select ci;
            ViewBag.pp = new SelectList(ppr.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[7]);

            //ViewBag.vendor = util.para_selectquery("002", glay.vwstrarray0[1]);
             str1 = "select vendor_code query0, vend_biz_name query1, 0 from AP_001_VENDR union all ";
            str1 += " select 'val', 'Select GL Account',99 order by 3,2";
            var vend = db.Database.SqlQuery<querylay>(str1);
            ViewBag.vendor = new SelectList(vend.ToList(), "query0", "query1", glay.vwstrarray0[1]);

        }
   private void initial_rtn()
        {

            glay.vwstrarray0 = new string[50];
            glay.vwstrarray0[4] = "PS";
            glay.vwstrarray0[6] = "N";
            glay.vwstrarray0[12] = "Y";
            glay.vwstring1 = "N";
            glay.vwstrarray0[3] = "A";
        }
        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstring0 = GB_001_TAX.tax_code;
            glay.vwstrarray0[0] = GB_001_TAX.tax_name;
            glay.vwdecimal0 = GB_001_TAX.tax_rate;
            glay.vwstrarray0[1] = GB_001_TAX.tax_agency;
            glay.vwstrarray0[2] = GB_001_TAX.tax_reg_id;
            glay.vwstrarray0[3] = GB_001_TAX.tax_impact;
            glay.vwstrarray0[4] = GB_001_TAX.module_basis;
            glay.vwstrarray0[5] = GB_001_TAX.computation_basis;

            glay.vwstring1 = GB_001_TAX.reclaimable;
            glay.vwstring2 = GB_001_TAX.salestax_payable_recognition;
            glay.vwstring3 = GB_001_TAX.purchasetax_payable_recognition;
            glay.vwstring4 = GB_001_TAX.salestax_paytime;

            glay.vwstrarray0[8] = GB_001_TAX.gl_tax_acc_code;


            if (GB_001_TAX.active_status == "Y")
            {
                glay.vwbool0 = true;
            }

            glay.vwstrarray0[7] = GB_001_TAX.note;
            //glay.vwstrarray0[12] = GB_001_TAX.attach_document;
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "TAX" && bg.document_code == GB_001_TAX.tax_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();
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
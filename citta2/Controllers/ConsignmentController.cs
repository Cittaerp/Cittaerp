using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using anchor1.Filters;
using CittaErp.utilities;

namespace CittaErp.Controllers
{
    public class ConsignmentController : Controller
    {
        //
        // GET: /Consignment/

        GB_001_RSONC GB_001_RSONC = new GB_001_RSONC();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string ptype = "";
        bool err_flag = true;
        string action_flag = "";

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.GB_001_RSONC
                         join bk1 in db.MC_001_CUREN 
                         on new { a1 = bh.currency_code } equals new { a1 = bk1.currency_code }
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()
           
                         join bk0 in db.GL_001_CHART
                          on new { a1 = bh.gl_account } equals new { a1 = bk0.account_code }
                         into bk4
                         from bk5 in bk4.DefaultIfEmpty()

                         select new vw_genlay
                         {
                             vwstring0 = bh.consignment_code,
                             vwstring1 = bh.description,
                             vwstring2 = bk5.account_name,
                             vwstring3 = bk3.currency_sym
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
           select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            GB_001_RSONC = db.GB_001_RSONC.Find(key1);
            if (GB_001_RSONC != null)
                read_record();

           select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [GB_001_RSONC] where consignment_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        [HttpPost]
       [ValidateAntiForgeryToken]
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

            select_query();
            return View(glay);
        }
        private void delete_record()
        {
            GB_001_RSONC = db.GB_001_RSONC.Find(glay.vwstring0);
            if (GB_001_RSONC != null)
            {
                db.GB_001_RSONC.Remove(GB_001_RSONC);
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
                GB_001_RSONC = new GB_001_RSONC();
                GB_001_RSONC.created_by = pubsess.userid;
                GB_001_RSONC.created_date = DateTime.UtcNow;
            }
            else
            {
                GB_001_RSONC = db.GB_001_RSONC.Find(glay.vwstring0);
            }
            GB_001_RSONC.attach_document = "";
            GB_001_RSONC.consignment_code = glay.vwstring0;
            GB_001_RSONC.description = glay.vwstring1;
            GB_001_RSONC.gl_account = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GB_001_RSONC.currency_code = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            GB_001_RSONC.allocation_basis = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            GB_001_RSONC.os_purchase_order = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            GB_001_RSONC.modified_date = DateTime.UtcNow;
            GB_001_RSONC.modified_by = pubsess.userid;
            GB_001_RSONC.active_status = glay.vwbool0 ? "Y" : "N";
            GB_001_RSONC.note = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            //GB_001_RSONC.attach_document = glay.vwstring7;


           if(action_flag == "Create")
                db.Entry(GB_001_RSONC).State = EntityState.Added;
            else
                db.Entry(GB_001_RSONC).State = EntityState.Modified;

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
                util.parameter_deleteflag("012", glay.vwstring0);
                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, GB_001_RSONC b where a.account_code = b.gl_account";
                //str += " and consignment_code =" + util.sqlquote(glay.vwstring0);
                //db.Database.ExecuteSqlCommand(str);
               
                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("CONSIGN", GB_001_RSONC.consignment_code, photo1, glay.vwstrarray9);

                }

            }

        }
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstring3 = pubsess.base_currency_code;
        }
        private void validation_routine()
        {
            string error_msg = "";
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
                error_msg = "Please enter id";

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
                error_msg = "Please enter description";

           if(action_flag == "Create")
            {
                GB_001_RSONC bnk = db.GB_001_RSONC.Find(glay.vwstring0);
                if (bnk != null)
                {
                ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                err_flag = false;
                }
            }
            
        }
        private void select_query()
        {
            ViewBag.cart = util.read_ledger("007", glay.vwstring2);
            //var bg2 = util.read_ledger("007");
            //ViewBag.cart = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring2);

            var all = from al in db.GB_999_MSG
                       where al.type_msg == "AB"
                       orderby al.name1_msg
                       select al;
            ViewBag.alloc = new SelectList(all.ToList(), "code_msg", "name1_msg", glay.vwstring4);

            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring3);

            var cur = from bh in db.AP_001_PTRAN
                      where bh.active_status == "N" && bh.purchase_order_class=="o"
                      orderby bh.purchase_order_name
                      select bh;
            ViewBag.ospo = new SelectList(cur.ToList(), "purchase_order_code", "purchase_order_name", glay.vwstring5);

        }
        private void read_record()
        {
            glay.vwstring0 = GB_001_RSONC.consignment_code;
            glay.vwstring1 = GB_001_RSONC.description;
            glay.vwstring2 = GB_001_RSONC.gl_account;
            glay.vwstring3 = GB_001_RSONC.currency_code;
            glay.vwstring4 = GB_001_RSONC.allocation_basis;
            glay.vwstring5 = GB_001_RSONC.os_purchase_order;
            if (GB_001_RSONC.active_status == "Y")
            {
                glay.vwbool0 = true;
            }


            glay.vwstring6 = GB_001_RSONC.note;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "CONSIGN" && bg.document_code == GB_001_RSONC.consignment_code
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
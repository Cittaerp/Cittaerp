using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;
using anchor1.Filters;

namespace CittaErp.Controllers
{
    public class Inter_companyController : Controller
    {
        //
        // GET: /Employee/

        IC_001_INCOY IC_001_INCOY = new IC_001_INCOY();
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

            var bglist = from bh in db.IC_001_INCOY
                         
                         select new vw_genlay
                         {
                             vwstring0 = bh.intercoy_code,
                             vwstring1 = bh.intercoy_name,
                             vwstring2=bh.city,
                             vwstring3=bh.country,
                             vwstring4 = bh.active_status,

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

            select_query();
            return View(glay);
        }
       
        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[IC_001_INCOY] where intercoy_code='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            initial_rtn();
            pubsess = (pubsess)Session["pubsess"];
            IC_001_INCOY = db.IC_001_INCOY.Find(key1);
            if (IC_001_INCOY != null)
                read_record();
           
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;
           
            if (id_xhrt=="D")
            { 
                delete_record();
                return RedirectToAction("Index");
          }
            photo1 = photofile;
            update_file();
            if (err_flag)
                return RedirectToAction("Index");

            //select_query();
            return View(glay);
        }
        private void delete_record()
        {
            IC_001_INCOY = db.IC_001_INCOY.Find(glay.vwstring0);
            if (IC_001_INCOY != null)
            {
                db.IC_001_INCOY.Remove(IC_001_INCOY);
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
                IC_001_INCOY = new IC_001_INCOY();
                IC_001_INCOY.created_by = pubsess.userid;
                IC_001_INCOY.created_date = DateTime.UtcNow;
            }
            else
            {
                IC_001_INCOY = db.IC_001_INCOY.Find(glay.vwstring0);
            }
            IC_001_INCOY.attach_document = "";
            IC_001_INCOY.intercoy_code = glay.vwstring0;
            IC_001_INCOY.intercoy_name = glay.vwstrarray0[0];
            IC_001_INCOY.address1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            IC_001_INCOY.address2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            IC_001_INCOY.city = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            IC_001_INCOY.state = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
            IC_001_INCOY.postal_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            IC_001_INCOY.country = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
            IC_001_INCOY.email = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
            IC_001_INCOY.website = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            IC_001_INCOY.phone_number = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];

            IC_001_INCOY.relationship_name = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
            IC_001_INCOY.job_title = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
            IC_001_INCOY.relationship_email = string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
            IC_001_INCOY.phone = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];

            IC_001_INCOY.currency_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
            IC_001_INCOY.gl_account_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
            

            IC_001_INCOY.modified_date = DateTime.UtcNow;
            IC_001_INCOY.modified_by = pubsess.userid;

            IC_001_INCOY.active_status = glay.vwbool0 ? "Y" : "N";
            IC_001_INCOY.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[15]) ? "" : glay.vwstrarray0[15];
            //IC_001_INCOY.attach_document = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "Y" : glay.vwstrarray0[16];



           if(action_flag == "Create")
                db.Entry(IC_001_INCOY).State = EntityState.Added;
            else
                db.Entry(IC_001_INCOY).State = EntityState.Modified;
           
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
                util.parameter_deleteflag("009", glay.vwstring0);
                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, IC_001_INCOY b where a.account_code = b.gl_account_code";
                //str += " and bank_code =" + util.sqlquote(glay.vwstring0);
                ////db.Database.ExecuteSqlCommand(str);
              
                {
                    util.write_document("INT", IC_001_INCOY.intercoy_code, photo1, glay.vwstrarray9);
                }

            }
        }
        private void validation_routine()
        {
            //string error_msg = "";
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "InterCompany Id must not be spaces");
                err_flag = false;
            }

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Name must not be spaces";

           if(action_flag == "Create")
            {
                IC_001_INCOY bnk = db.IC_001_INCOY.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }
        private void select_query()
        {
            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currencycode = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[13]);

            //var currency = from bh in db.MC_001_CUREN
            //               where bh.active_status == "N"
            //               orderby bh.currency_description
            //          select bh;
            //ViewBag.currencycode = new SelectList(currency.ToList(), "currency_code", "currency_description", glay.vwstrarray0[13]);

            ViewBag.chart = util.read_ledger("016", glay.vwstrarray0[14]);
            //var bg2 = util.read_ledger("016");
            //ViewBag.chart = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[14]);

            ViewBag.country = util.para_selectquery("13", glay.vwstrarray0[5],"N");
            //var count = from bh in db.GB_001_PCODE
            //            where bh.parameter_type == "13" && bh.active_status == "N"
            //            orderby bh.parameter_name
            //               select bh;
            //ViewBag.country = new SelectList(count.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[5]);

            ViewBag.jobrole = util.para_selectquery("04", glay.vwstrarray0[10]);
            //var job = from bh in db.GB_001_PCODE
            //          where bh.parameter_type == "04" && bh.active_status == "N"
            //          orderby bh.parameter_name
            //            select bh;
            //ViewBag.jobrole = new SelectList(job.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[10]);
        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray0[13] = pubsess.base_currency_code;
            glay.vwstrarray0[5] = pubsess.country_operation;
        }
        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstring0 = IC_001_INCOY.intercoy_code;
            glay.vwstrarray0[0] = IC_001_INCOY.intercoy_name;
            glay.vwstrarray0[1] = IC_001_INCOY.address1;
            glay.vwstrarray0[2] = IC_001_INCOY.address2;
            glay.vwstrarray0[3] = IC_001_INCOY.city;
            glay.vwstrarray0[16] = IC_001_INCOY.state;
            glay.vwstrarray0[4] = IC_001_INCOY.postal_code;
            glay.vwstrarray0[5] = IC_001_INCOY.country;
            glay.vwstrarray0[6] = IC_001_INCOY.email;
            glay.vwstrarray0[7] = IC_001_INCOY.website;
            glay.vwstrarray0[8] = IC_001_INCOY.phone_number;
            glay.vwstrarray0[9] = IC_001_INCOY.relationship_name;
            glay.vwstrarray0[10] = IC_001_INCOY.job_title;
            glay.vwstrarray0[11] = IC_001_INCOY.relationship_email;
            glay.vwstrarray0[12] = IC_001_INCOY.phone;


            glay.vwstrarray0[13] = IC_001_INCOY.currency_code;
            glay.vwstrarray0[14] = IC_001_INCOY.gl_account_code;

            if (IC_001_INCOY.active_status == "Y")
            {
                glay.vwbool0 = true;
            }
            glay.vwstrarray0[15] = IC_001_INCOY.note;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "INT" && bg.document_code == IC_001_INCOY.intercoy_code
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

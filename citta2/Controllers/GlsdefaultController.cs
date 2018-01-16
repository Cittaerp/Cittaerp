using CittaErp.Models;
using CittaErp.utilities;
using anchor1.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace CittaErp.Controllers
{
    public class GlsdefaultController : Controller
    {
        //
        // GET: /Gldefault/
        GL_001_GLDS GL_001_GLDS = new GL_001_GLDS();
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


            var bglist = from bh in db.GL_001_GLDS
                         join bg in db.GL_001_ATYPE
                         on new { a1 = bh.acct_type1 } equals new { a1 = bg.acct_type_code}
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join bk in db.GL_001_ATYPE
                         on new { a1 = bh.acct_type2 } equals new { a1 = bk.acct_type_code}
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         join bf in db.GL_001_ATYPE
                         on new { a1 = bh.acct_type3 } equals new { a1 = bf.acct_type_code }
                         into bf1
                         from bf2 in bf1.DefaultIfEmpty()
                         join bi in db.GB_999_MSG
                         on new { a1 = bh.gl_default_id, a2 = "GLS"  } equals new {a1 = bi.code_msg, a2= bi.type_msg }
                         into bi1
                         from bi2 in bi1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.gl_default_id,
                             vwstring1 = bg2.acct_type_desc,
                             vwstring2 = bk2.acct_type_desc,
                             vwstring3 = bf2.acct_type_desc,
                             vwstring4 = bi2.name1_msg
                         };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
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
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            GL_001_GLDS = db.GL_001_GLDS.Find(key1);
            if (GL_001_GLDS != null)
                read_record();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
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

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }


        private void delete_record()
        {
            GL_001_GLDS = db.GL_001_GLDS.Find(glay.vwstring0);
            if (GL_001_GLDS != null)
            {
                db.GL_001_GLDS.Remove(GL_001_GLDS);
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
                GL_001_GLDS = new GL_001_GLDS();
                GL_001_GLDS.created_by = pubsess.userid;
                GL_001_GLDS.created_date = DateTime.UtcNow;
            }
            else
            {
                GL_001_GLDS = db.GL_001_GLDS.Find(glay.vwstring0);
            }

            GL_001_GLDS.gl_default_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            GL_001_GLDS.acct_type1 = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            GL_001_GLDS.modified_date = DateTime.UtcNow;
            GL_001_GLDS.modified_by = pubsess.userid;
            GL_001_GLDS.acct_type2 = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GL_001_GLDS.acct_type3 = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            GL_001_GLDS.acct_type4 = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            GL_001_GLDS.acct_type5 = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;


           if(action_flag == "Create")
                db.Entry(GL_001_GLDS).State = EntityState.Added;
            else
                db.Entry(GL_001_GLDS).State = EntityState.Modified;

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
                ModelState.AddModelError(String.Empty, "Id must not be spaces");
                err_flag = false;
            }

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Description must not be spaces";

           if(action_flag == "Create")
            {
                GL_001_GLDS bnk = db.GL_001_GLDS.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }

        private void read_record()
        {
            glay.vwstring0 = GL_001_GLDS.gl_default_id;
            glay.vwstring1 = GL_001_GLDS.acct_type1;
            glay.vwstring2 = GL_001_GLDS.acct_type2;
            glay.vwstring3 = GL_001_GLDS.acct_type3;
            glay.vwstring4 = GL_001_GLDS.acct_type4;
            glay.vwstring5 = GL_001_GLDS.acct_type5;
            
        }
        private void select_query()
        {
            ViewBag.acctype = util.para_selectquery("55","","N");
           //var bglistb = from bg in db.GL_001_ATYPE
           //               where bg.active_status == "N"
           //               orderby bg.acct_type_desc
           //               select new { c1 = bg.acct_type_code, c2 = bg.acct_type_desc };

           // ViewBag.acctype = new SelectList(bglistb.ToList(), "c1", "c2");
            var bgview = from nf in db.GB_999_MSG
                         where nf.type_msg == "GLS"
                         orderby nf.name1_msg
                         select nf;
            ViewBag.gl_selectn = new SelectList(bgview.ToList(), "code_msg", "name1_msg", glay.vwstring0);

        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GL_001_GLDS] where gl_default_id=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }
    }
}
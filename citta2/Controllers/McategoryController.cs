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
    public class McategoryController : Controller
    {
        GL_001_CATEG GL_001_CATEG = new GL_001_CATEG();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        string delmsg = "";
        bool err_flag = true;
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.GL_001_CATEG
                         join bk1 in db.GB_999_MSG
                         on new { a1 = "RG", a2 = bh.acct_cat_rpt_group } equals new { a1 = bk1.type_msg, a2 = bk1.code_msg }
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()
                         orderby bk3.name1_msg, bh.acct_cat_rpt_sequence
                         select new vw_genlay
                                    {
                                        vwstring4 = bh.acct_cat_sequence,
                                        vwstring0 = bh.acct_cat_desc,
                                        vwstring1 = bk3.name1_msg,
                                        vwint1 = bh.acct_cat_rpt_sequence,
                                        vwstring2 = bh.note,
                                        vwstring3 = bh.active_status == "N" ? "Active" : "Inactive"
                                    };
            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            GL_001_CATEG = db.GL_001_CATEG.Find(key1);
            if (GL_001_CATEG != null)
                read_record();

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

            if (id_xhrt=="D")
            { 
                delete_record();
                if (err_flag == false)
                {
                    return View(glay);
                }
                return RedirectToAction("Index");
            }

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        private void delete_record()
        {
            if (util.delete_check("MCAT", glay.vwstring3))
            {
                GL_001_CATEG = db.GL_001_CATEG.Find(glay.vwstring3);
                db.GL_001_CATEG.Remove(GL_001_CATEG);
                db.SaveChanges();
            }
            else
            {
                delmsg = "Account Category in Use";
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
            if (action_flag=="Create")
            {
                GL_001_CATEG = new GL_001_CATEG();
                GL_001_CATEG.created_by = pubsess.userid;
                GL_001_CATEG.created_date = DateTime.UtcNow;
                GL_001_CATEG.delete_flag = "N";
            }
            else
            {
                GL_001_CATEG = db.GL_001_CATEG.Find(glay.vwstring3);
            }

            GL_001_CATEG.acct_cat_sequence = glay.vwstring3;
            GL_001_CATEG.acct_cat_desc = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            GL_001_CATEG.acct_cat_rpt_group = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
             GL_001_CATEG.acct_cat_rpt_sequence = glay.vwint1;
            GL_001_CATEG.modified_date = DateTime.UtcNow;
            GL_001_CATEG.modified_by = pubsess.userid;
            GL_001_CATEG.note = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GL_001_CATEG.active_status = glay.vwbool0 ? "Y" : "N";
           

           if(action_flag == "Create")  
                db.Entry(GL_001_CATEG).State = EntityState.Added;
            else 
                db.Entry(GL_001_CATEG).State = EntityState.Modified;

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
            string err_free = "";
            if (string.IsNullOrWhiteSpace(glay.vwstring3))
            {
                ModelState.AddModelError(String.Empty, "Enter Category Id");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "select an Account Group");
                err_flag = false;
            }
            if (glay.vwint1 <= 0)
            {
                ModelState.AddModelError(String.Empty, "Report sequence must be greater than zero");
                err_flag = false;
            
            }
            string sqlstr = "select '1' query0 from GL_001_CATEG where upper(acct_cat_desc)=" + util.sqlquote(glay.vwstring0.ToUpper());
            var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (bglist1 != null)
            {
                if (Session["action_flag"].ToString() == "Edit")
                {
                    GL_001_CATEG = db.GL_001_CATEG.Find(glay.vwstring3);
                    string old_desc = GL_001_CATEG.acct_cat_desc;
                    if (old_desc != glay.vwstring0)
                    {
                        err_free = "N";
                        
                            ModelState.AddModelError(String.Empty, "Can not accept duplicate description");
                            err_flag = false;
                    }
                }
                if (Session["action_flag"].ToString() == "Create" && err_free == "")
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicate description");
                    err_flag = false;
                }
            }

            sqlstr = "select '1' query0 from GL_001_CATEG where acct_cat_rpt_sequence=" + glay.vwint1.ToString() + " and acct_cat_rpt_group= " + util.sqlquote(glay.vwstring1);
            bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (bglist1 != null)
            {
                if (Session["action_flag"].ToString() == "Edit")
                {
                    GL_001_CATEG = db.GL_001_CATEG.Find(glay.vwstring3);
                    int old_rpt = GL_001_CATEG.acct_cat_rpt_sequence;
                    if (old_rpt != glay.vwint1)
                    {
                        err_free = "N";

                        ModelState.AddModelError(String.Empty, "Can not accept duplicate report sequence");
                        err_flag = false;
                    }
                }
                if (Session["action_flag"].ToString() == "Create" && err_free == "")
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicate report sequence");
                    err_flag = false;
                }
            }

           if(action_flag == "Create")
            {
                GL_001_CATEG bnk = db.GL_001_CATEG.Find(glay.vwstring3);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            
            }

            
        }

        private void read_record()

        {
            glay.vwstring3 = GL_001_CATEG.acct_cat_sequence;
            glay.vwstring0=GL_001_CATEG.acct_cat_desc;
            glay.vwstring1 = GL_001_CATEG.acct_cat_rpt_group;
            glay.vwint1 = GL_001_CATEG.acct_cat_rpt_sequence;
            glay.vwstring2 = GL_001_CATEG.note;
            glay.vwbool0 = false;
            if (GL_001_CATEG.active_status == "Y")
                glay.vwbool0 = true;
        
        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {

            err_flag = true;
            glay.vwstring3 = id;
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
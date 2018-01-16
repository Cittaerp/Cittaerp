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
    public class Account_typeController : Controller
    {
        //
        // GET: /Account_type/

        GL_001_ATYPE GL_001_ATYPE = new GL_001_ATYPE();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        psess psess = new psess();
        cittautil util = new cittautil();

        //string ptype = "";
        bool err_flag = true;
        string delmsg = "";
        string action_flag = "";
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.GL_001_ATYPE
                         join bk1 in db.GL_001_CATEG
                         on new { a1 = bh.acct_cat_sequence } equals new { a1 = bk1.acct_cat_sequence }
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.acct_type_code,
                             vwstring1 = bh.acct_type_desc,
                             vwstring2 = bk3.acct_cat_desc,
                             vwint1 = bh.acct_type_rpt_sequence,
                             vwstring3= bh.active_status == "N" ? "Active" :"Inactive"


                         };

            return View(bglist.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            select_query();
            return View(glay);
        }
        [HttpPost]
        public ActionResult Create(vw_genlay glay_in)
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");
            select_query();
            return View(glay);
        }
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Create";
            action_flag = "Create";
           
            pubsess = (pubsess)Session["pubsess"];
            GL_001_ATYPE = db.GL_001_ATYPE.Find(key1);
            if (GL_001_ATYPE != null)
                read_record();
             select_query();
            return View(glay);
        }
        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            glay = glay_in;

            if (id_xhrt == "D")
            {
                delete_record();
                if (err_flag == false)
                {
                    select_query();
                    return View(glay);
                }
                    return RedirectToAction("Index");
            }

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement

            err_flag = true;
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
        private void delete_record()
        {

            if (util.delete_check("ATYPE", glay.vwstring0))
            {
                GL_001_ATYPE = db.GL_001_ATYPE.Find(glay.vwstring0);
                db.GL_001_ATYPE.Remove(GL_001_ATYPE);
                db.SaveChanges();
            }
            else
            {
                delmsg = "Account Type in Use";
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
            if (action_flag == "Create")
            {
                GL_001_ATYPE = new GL_001_ATYPE();
                GL_001_ATYPE.created_by = pubsess.userid;
                GL_001_ATYPE.created_date = DateTime.UtcNow;
                GL_001_ATYPE.delete_flag = "N";
            }
            else
            {
                GL_001_ATYPE = db.GL_001_ATYPE.Find(glay.vwstring0);
            }

            GL_001_ATYPE.acct_type_code = glay.vwstring0;
            GL_001_ATYPE.acct_type_desc = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            GL_001_ATYPE.acct_cat_sequence = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GL_001_ATYPE.acct_type_rpt_sequence = (glay.vwint2);
            GL_001_ATYPE.modified_date = DateTime.UtcNow;
            GL_001_ATYPE.modified_by = pubsess.userid;
            GL_001_ATYPE.active_status = glay.vwbool0 ? "Y" : "N";
            GL_001_ATYPE.note = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
           
            if (action_flag == "Create")
                db.Entry(GL_001_ATYPE).State = EntityState.Added;
            else
                db.Entry(GL_001_ATYPE).State = EntityState.Modified;

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
           // int jhn = GL_001_ATYPE.acct_cat_sequence;
           // string str = "update GL_001_CATEG set delete_flag ='Y' where acct_cat_sequence = " + jhn;
            //db.Database.ExecuteSqlCommand(str);
            string str = "update GL_001_CATEG set delete_flag ='Y' from GL_001_CATEG a, GL_001_ATYPE b where a.acct_cat_sequence = b.acct_cat_sequence";
            str += " and acct_type_code =" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str);
                    

        }
        private void validation_routine()
        {
            //string error_msg = "";
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please enter Account type Id");
                err_flag = false;
            }
            if (glay.vwint2 <= 0)
            {
                ModelState.AddModelError(String.Empty, "Report sequence must be greater than zero");
                err_flag = false;

            }
             //   error_msg = "Please enter ID";

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Name must not be spaces";

            if (action_flag == "Create")
            {
                GL_001_ATYPE bnk = db.GL_001_ATYPE.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }

                string sqlstr1 = "select '1' query0 from GL_001_ATYPE where acct_type_desc=" + util.sqlquote(glay.vwstrarray0[0]);
                    var bglist2 = db.Database.SqlQuery<querylay>(sqlstr1).FirstOrDefault();
                    if (bglist2 != null)
                    {
                        ModelState.AddModelError(String.Empty, "Can not accept duplicate Description");
                        err_flag = false;
                    }
            }
            string sqlstr = "select '1' query0 from GL_001_ATYPE where acct_cat_sequence="+ util.sqlquote(glay.vwint1.ToString());
            sqlstr += " and acct_type_rpt_sequence =" + util.sqlquote(glay.vwint2.ToString()) + " and acct_type_code<>" + glay.vwint0.ToString();
            var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (bglist1 != null)
            {
                ModelState.AddModelError(String.Empty, "Can not accept duplicate for report sequence within the same category");
                err_flag = false;
            }

        }
        private void select_query()
        {
            ViewBag.catg = util.para_selectquery("56", glay.vwstring2);
            //var cat = from bh in db.GL_001_CATEG
            //          where bh.active_status == "N"
            //          orderby bh.acct_cat_desc
            //          select bh;
            //ViewBag.catg = new SelectList(cat.ToList(), "acct_cat_sequence", "acct_cat_desc", glay.vwint1);
        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
        }
        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstring0 = GL_001_ATYPE.acct_type_code;
            glay.vwstrarray0[0] = GL_001_ATYPE.acct_type_desc;
            glay.vwstring2 = GL_001_ATYPE.acct_cat_sequence;
            glay.vwint2 = GL_001_ATYPE.acct_type_rpt_sequence;

            if (GL_001_ATYPE.active_status == "Y")
            {
                glay.vwbool0 = true;
            }
            glay.vwstrarray0[1] = GL_001_ATYPE.note;
            
        }
        private void error_message()
        {

        }







	}
}
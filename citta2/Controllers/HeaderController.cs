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
    public class HeaderController : Controller
    {
        GB_001_HEADER GB_001_HEADER = new GB_001_HEADER();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        string key_val = "";
        bool err_flag = true;
        string delmsg = "";
        string action_flag = "";
        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.GB_001_HEADER
                         join bk8 in db.GB_999_MSG
                         on new { a1 = bh.header_type_code, a2= "head" } equals new { a1 = bk8.code_msg, a2 = bk8.type_msg}
                         into bk9
                        from bk10 in bk9.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.header_type_code,
                             vwstring7 = bk10.name1_msg
                             
                         };
            
            return View(bglist.Distinct().ToList());


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
        public ActionResult Create (vw_genlay glay_in)
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

        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            key_val = key1;
                read_record();

            select_query();
            return View(glay);
        }

        [HttpPost]
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


        private void delete_record()
        {
            if (util.delete_check("HEAD", glay.vwstring0))
            {
                GB_001_HEADER = db.GB_001_HEADER.Find(glay.vwstring0);
                db.GB_001_HEADER.Remove(GB_001_HEADER);
                db.SaveChanges();
            }
            else
            {
                delmsg = "Analysis in Use";
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
            string str1 = "delete from GB_001_HEADER where header_type_code=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "insert into GB_001_HEADER(header_type_code,sequence_no,header_code,mandatory_flag,delete_flag,created_by) values (";
            string str2 = "";
            for (int count1 = 0; count1 < 10;count1++ )
            {
                if (!string.IsNullOrWhiteSpace(glay.vwstrarray0[count1]))
                {
                    str2 = str1 + util.sqlquote(glay.vwstring0) + ",";
                    str2 += (count1 + 1).ToString() + ",";
                    str2 += util.sqlquote(glay.vwstrarray0[count1]) + ",";
                    str2 += glay.vwblarray0[count1] ? "'Y'" : "'N'";
                    str2 += ",'N'," + util.sqlquote(pubsess.userid) + ")";
                    db.Database.ExecuteSqlCommand(str2);
                }
            }

            str1 = "insert into GB_001_HEADER(header_type_code,sequence_no,header_code,mandatory_flag, delete_flag,note,created_by) values (";
            str2 = str1 + util.sqlquote(glay.vwstring0) + ",99,'Notes','N','N',";
            str2 += util.sqlquote(glay.vwstring1) + "," + util.sqlquote(pubsess.userid) + ")";
            db.Database.ExecuteSqlCommand(str2);
            
            
        }

        private void validation_routine()
        {
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "select Header type");
                err_flag = false;
            }

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "select header";

           if(action_flag == "Create")
            {
                GB_001_HEADER bnk = db.GB_001_HEADER.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }
        private void select_query()
        {
            var bglist = from bh in db.GB_999_MSG
                         where bh.type_msg == "HEAD" && bh.name5_msg=="A"
                         orderby bh.name1_msg
                         select bh;

            ViewBag.parameter = new SelectList(bglist.ToList(), "code_msg", "name1_msg", glay.vwstring0);


            ViewBag.header1 = util.para_selectquery("50","");
            //var bgitem = from bh in db.GB_001_HANAL
            //             where bh.active_status == "N"
            //             orderby bh.header_description
            //             select bh;

            //ViewBag.header1 = bgitem.ToList();

            //ViewBag.header_1 = new SelectList(bgitem.ToList(), "header_sequence", "header_description", glay.vwstrarray0[0]);

            //var bgitm = from bh in db.GB_001_HANAL
            //            where bh.active_status == "N"
            //            orderby bh.header_description
            //             select bh;

            //ViewBag.header_2 = new SelectList(bgitm.ToList(), "header_sequence", "header_description", glay.vwstrarray0[1]);

            //var bglst = from bh in db.GB_001_HANAL
            //            where bh.active_status == "N"
            //            orderby bh.header_description
            //            select bh;

            //ViewBag.header_3 = new SelectList(bglst.ToList(), "header_sequence", "header_description", glay.vwstrarray0[2]);

        }

        private void read_record()
        {
            initial_rtn();
            var bhlist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == key_val
                         select bg;

            foreach (var item in bhlist.ToList())
            {
                if (item.sequence_no != 99)
                {
                    glay.vwstring0 = item.header_type_code;
                    glay.vwstrarray0[item.sequence_no-1] = item.header_code;
                    glay.vwblarray0[item.sequence_no-1] = item.mandatory_flag == "Y" ? true : false;
                }
                else
                    glay.vwstring1 = item.note;
            }
        
        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
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
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwblarray0 = new bool[20];
        }
        private void error_message()
        {

        }

	}
}
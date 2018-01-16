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
    public class PHeadController : Controller
    {
        GB_001_PHEAD GB_001_PHEAD = new GB_001_PHEAD();
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
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.GB_001_PHEAD
                         join bk8 in db.IV_001_ITEM
                         on new { a1 = bh.header_type_code } equals new { a1 = bk8.item_code}
                         into bk9
                        from bk10 in bk9.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.header_type_code,
                             vwstring7 = bk10.item_name
                             
                         };
            
            return View(bglist.Distinct().ToList());


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

        [EncryptionActionAttribute]
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
            string str1 = "delete from GB_001_PHEAD where header_type_code=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);
            //if (util.delete_check("PHEAD", glay.vwstring0))
            //{
            //    GB_001_PHEAD = db.GB_001_PHEAD.Find(glay.vwstring0);
            //    db.GB_001_PHEAD.Remove(GB_001_PHEAD);
            //    db.SaveChanges();
            //}
            //else
            //{
            //    delmsg = "Analysis in Use";
            //    ModelState.AddModelError(String.Empty, delmsg);
            //    err_flag = false;

            //}

        }
        private void update_file()
        {
            err_flag = true;
            //validation_routine();

            if (err_flag)
                update_record();

        }

        private void update_record()
        {
            string str1 = "delete from GB_001_PHEAD where header_type_code=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "insert into GB_001_PHEAD(header_type_code,sequence_no,fee_id,mandatory_flag,amount,delete_flag,created_by) values (";
            string str2 = "";
            for (int count1 = 0; count1 < glay.vwstrarray0.Length;count1++ )
            {
                if (!string.IsNullOrWhiteSpace(glay.vwstrarray0[count1]))
                {
                    str2 = str1 + util.sqlquote(glay.vwstring0) + ",";
                    str2 += (count1 + 1).ToString() + ",";
                    str2 += util.sqlquote(glay.vwstrarray0[count1]) + ",";
                    str2 += glay.vwblarray0[count1] ? "'Y'" : "'N'";
                    str2 += ","+glay.vwdclarray0[count1];
                    str2 += ",'N'," + util.sqlquote(pubsess.userid) + ")";
                    db.Database.ExecuteSqlCommand(str2);
                }
            }

            str1 = "insert into GB_001_PHEAD(header_type_code,sequence_no,fee_id,mandatory_flag, delete_flag,amount,note,created_by) values (";
            str2 = str1 + util.sqlquote(glay.vwstring0) + ",99,'Notes','N','N',0,";
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
                GB_001_PHEAD bnk = db.GB_001_PHEAD.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }

        }
        private void select_query()
        {
             
            var coyin = (from bg in db.GB_001_COY
                         where bg.id_code == "COYPPRTY"
                         select bg).FirstOrDefault();

            glay.vwstrarray0 = new string[20];
            glay.vwstrarray0[0] = coyin.field1;
            glay.vwstrarray0[1] = coyin.field2;
            glay.vwstrarray0[2] = coyin.field3;
            glay.vwstrarray0[3] = coyin.field4;
            glay.vwstrarray0[4] = coyin.field5;
            glay.vwstrarray0[5] = coyin.field6;
            glay.vwstrarray0[6] = coyin.field7;
            glay.vwstrarray0[7] = coyin.field8;
            glay.vwstrarray0[8] = coyin.field9;
            glay.vwstrarray0[9] = coyin.field10;
           // psess.temp0 = coyin.field6;
            //psess.temp1 = coyin.field7;
            //psess.temp2 = coyin.field8;

            var itr = from bg in db.IV_001_ITEM
                      where bg.item_type == "P" && bg.sales == "Y"
                      select bg;
            ViewBag.parameter = new SelectList(itr.ToList(), "item_code", "item_name", glay.vwstring0);
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
            var bhlist = from bg in db.GB_001_PHEAD
                         where bg.header_type_code == key_val
                         select bg;

            foreach (var item in bhlist.ToList())
            {
                if (item.sequence_no != 99)
                {
                    glay.vwstring0 = item.header_type_code;
                    glay.vwstrarray0[item.sequence_no-1] = item.fee_id;
                    glay.vwdclarray0[item.sequence_no - 1] = item.amount; 
                    glay.vwblarray0[item.sequence_no - 1] = item.mandatory_flag == "Y" ? true : false;
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
            glay.vwdclarray0 = new decimal[20];
        }
        private void error_message()
        {

        }

	}
}
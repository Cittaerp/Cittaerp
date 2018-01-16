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
    public class MapController : Controller
    {
        GB_001_MAP GB_001_MAP = new GB_001_MAP();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        string key_val = "";
        bool err_flag = true;
        string delmsg = "";
        string ptype = "";
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index(string pc=null)
        {
            psess = (psess)Session["psess"];
            
            util.init_values();

            if (!(string.IsNullOrWhiteSpace(pc)))
            {
                psess.temp0 = pc;
            }

            ptype = psess.temp0.ToString();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.GB_001_MAP
                         join bk8 in db.GB_999_MSG
                         on new { a1 = bh.trans_type, a2= "head" } equals new { a1 = bk8.code_msg, a2 = bk8.type_msg}
                         into bk9
                        from bk10 in bk9.DefaultIfEmpty()
                        where bh.main_head==ptype
                         select new vw_genlay
                         {
                             vwstring0 = bh.trans_type,
                             vwstring7 = bk10.name1_msg
                             
                         };
            
            return View(bglist.Distinct().ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            psess = (psess)Session["psess"];
            
            ptype = psess.temp0.ToString();
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (vw_genlay glay_in)
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
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

            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
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
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
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
            string str1 = "delete from GB_001_MAP where trans_type=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

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
            string str1 = "delete from GB_001_MAP where trans_type=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "insert into GB_001_MAP(trans_type,seq_no,main_head,jon_seq,tran_seq,created_by) values (";
            string str2 = "";
            for (int count1 = 0; count1 < 10;count1++ )
            {
                if (!string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
                {
                    str2 = str1 + util.sqlquote(glay.vwstring0) + ",";
                    str2 += (count1 + 1).ToString() + ",";
                    str2 += util.sqlquote(ptype) + ","; 
                    str2 += util.sqlquote(glay.vwstrarray4[count1]) + ",";
                    str2 += util.sqlquote(glay.vwstrarray6[count1]) + ",";
                    str2 += util.sqlquote(pubsess.userid) + ")";
                    db.Database.ExecuteSqlCommand(str2);
                }
            }

            //str1 = "insert into GB_001_MAP(header_type_code,sequence_no,header_code,mandatory_flag, delete_flag,note,created_by) values (";
            //str2 = str1 + util.sqlquote(glay.vwstring0) + ",99,'Notes','N','N',";
            //str2 += util.sqlquote(glay.vwstring1) + "," + util.sqlquote(pubsess.userid) + ")";
            //db.Database.ExecuteSqlCommand(str2);
            
            
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
                GB_001_MAP bnk = db.GB_001_MAP.Find(glay.vwstring0);
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
                         where bh.type_msg == "HEAD"
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
            var bhlist = from bg in db.GB_001_MAP
                         where bg.trans_type == key_val
                         select bg;

            foreach (var item in bhlist.ToList())
            {
                if (item.seq_no != 99)
                {
                    glay.vwstring0 = item.trans_type;
                    glay.vwstrarray4[item.seq_no - 1] = item.jon_seq;
                    glay.vwstrarray6[item.seq_no - 1] = item.tran_seq;
              }
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
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwlist0 = new List<querylay>[20];
        }
        private void error_message()
        {

        }

        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

            SelectList[] head_det = new SelectList[20];

            //Session["head_det"] = head_det;
           // //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;

            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == ptype && bg.sequence_no != 99
                         select bg;

            foreach (var item in bglist.ToList())
            {
               // glay.vwstring1 = item.header_type_code;
                int count2 = item.sequence_no;
                aheader7[count2] = item.mandatory_flag;
                glay.vwstrarray4[count2] = item.sequence_no.ToString();
                var bglist2 = (from bg in db.GB_001_HANAL
                               where bg.header_sequence == item.header_code
                               select bg).FirstOrDefault();

                if (bglist2 != null)
                {
                    glay.vwstrarray5[count2] = bglist2.header_description;

                    string str = " select analysis_code query0, analysis_description query1 from GB_001_DANAL where header_sequence = ";
                    str += util.sqlquote(item.header_code);
                    var str1 = db.Database.SqlQuery<querylay>(str);
                    glay.vwlist0[count2] = str1.ToList();

                }

            }

            // // Session["head_det"] = head_det;
            // //Session["aheader7"] = aheader7;
            // psess.sarrayt1 = glay.vwstrarray5;
            psess.sarrayt0 = aheader7;
            psess.sarrayt1 = glay.vwstrarray5;
        }

        [HttpPost]
        public ActionResult analy(string header_code)
        {
         
            var hdet = from bg in db.GB_001_HEADER
                              join bh in db.GB_001_HANAL
                              on new { a1 = bg.header_code } equals new { a1=bh.header_sequence}
                              into bg2
                              from bg3 in bg2.DefaultIfEmpty()
                              where bg.header_type_code == header_code && bg.sequence_no != 99
                                select new
                                   {
                                       c1 = bg.sequence_no,
                                       c2 = bg3.header_description
                                   };


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                hdet.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }

	}
}
using CittaErp.Models;
using anchor1.Filters;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class ChartController : Controller
    {
        GL_001_CHART GL_001_CHART = new GL_001_CHART();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase [] photo1;

        bool err_flag = true;
        string delmsg = "";
        string move_auto = "N";
        string action_flag = "";
        //
        // GET: /Citta/

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
           // util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            // Session["multi_curenchk"] = pubsess.multi_currency;

            var bglist = from bh in db.GL_001_CHART
                         join bk1 in db.GL_001_ATYPE
                         on new { a1 = bh.account_type_code } equals new { a1 = bk1.acct_type_code }
                         into bk2
                         from bk3 in bk2.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.account_code,
                             vwstring1 = bh.account_name,
                             vwstring2 = bh.account_type_code,
                            // vwstring2 = bh.b
                             vwstring3 = bh.archiving,
                             vwstring5 = bh.active_status == "N" ? "Active" : "Inactive",
                             vwstring4 = bk3.acct_type_desc
                         };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            header_ana();
            display_curren();
            select_query();
            cal_auto();
            psess.temp5 = move_auto;
            Session["psess"] = psess;
            if (move_auto == "Y")
                glay.vwstring0 = "AUTO";
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            cal_auto();
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            psess.temp5 = move_auto;
            initial_rtn();
            GL_001_CHART = db.GL_001_CHART.Find(key1);
            if (GL_001_CHART != null)
                read_record();
            Session["psess"] = psess;
            
            header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt == "D")
            {
                delete_record();
                if (err_flag == false)
                {
                    initial_rtn();
                    header_ana();
                    select_query();
                    return View(glay);
                }
                return RedirectToAction("Index");
            }

            photo1 = photofile;
            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

       
        private void delete_record()
        {
            if (util.delete_check("CHART", glay.vwstring0))
            {
                GL_001_CHART = db.GL_001_CHART.Find(glay.vwstring0);
                db.GL_001_CHART.Remove(GL_001_CHART);
                db.SaveChanges();
            }
            else
            {
                delmsg = "Account in Use";
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
           if(action_flag == "Create")
            {
                GL_001_CHART = new GL_001_CHART();
                GL_001_CHART.created_by = pubsess.userid;
                GL_001_CHART.created_date = DateTime.UtcNow;
                GL_001_CHART.delete_flag = "N";
                if (move_auto == "Y")
                    glay.vwstring0 = util.autogen_num("CHA");
            }
            else
            {
                GL_001_CHART = db.GL_001_CHART.Find(glay.vwstring0);
            }

            GL_001_CHART.attach_document = "";
            GL_001_CHART.account_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            GL_001_CHART.account_name = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            GL_001_CHART.account_type_code = string.IsNullOrWhiteSpace(glay.vwstring6) ?"":glay.vwstring6;
            GL_001_CHART.category_name = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GL_001_CHART.archiving = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            GL_001_CHART.currency_code = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            GL_001_CHART.modified_date = DateTime.UtcNow;
            GL_001_CHART.modified_by = pubsess.userid;
            GL_001_CHART.note = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            GL_001_CHART.active_status = glay.vwbool0 ? "Y" : "N";

            GL_001_CHART.analysis_code1 = "";
            GL_001_CHART.analysis_code2 = "";
            GL_001_CHART.analysis_code3 = "";
            GL_001_CHART.analysis_code4 = "";
            GL_001_CHART.analysis_code5 = "";
            GL_001_CHART.analysis_code6 = "";
            GL_001_CHART.analysis_code7 = "";
            GL_001_CHART.analysis_code8 = "";
            GL_001_CHART.analysis_code9 = "";
            GL_001_CHART.analysis_code10 = "";
            if (glay.vwstrarray6 != null)
            {
                int arrlen = glay.vwstrarray6.Length;
                if (arrlen > 0)
                    GL_001_CHART.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                if (arrlen > 1)
                    GL_001_CHART.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                if (arrlen > 2)
                    GL_001_CHART.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                if (arrlen > 3)
                    GL_001_CHART.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                if (arrlen > 4)
                    GL_001_CHART.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                if (arrlen > 5)
                    GL_001_CHART.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                if (arrlen > 6)
                    GL_001_CHART.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                if (arrlen > 7)
                    GL_001_CHART.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                if (arrlen > 8)
                    GL_001_CHART.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                if (arrlen > 9)
                    GL_001_CHART.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                psess.intemp0 = arrlen;
                Session["psess"] = psess;
            }
            if (GL_001_CHART.currency_code == "")
            {
                GL_001_CHART.currency_code = pubsess.base_currency_code;
            }

           if(action_flag == "Create")
                db.Entry(GL_001_CHART).State = EntityState.Added;
            else
                db.Entry(GL_001_CHART).State = EntityState.Modified;

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
                string str = "update GL_001_ATYPE set delete_flag ='Y' from GL_001_ATYPE a, GL_001_CHART b where a.acct_type_code = b.account_type_code";
                str += " and account_code =" + util.sqlquote(glay.vwstring0);
                db.Database.ExecuteSqlCommand(str);

                string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, GL_001_CHART b where header_sequence in (analysis_code1";
                stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                stri += " and account_code =" + util.sqlquote(glay.vwstring0);
                db.Database.ExecuteSqlCommand(stri);

                util.write_document("CHART", GL_001_CHART.account_code, photo1, glay.vwstrarray9);

            }
        }

        private void validation_routine()
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            
            if (string.IsNullOrWhiteSpace(glay.vwstring0) && move_auto != "Y")
                {
                ModelState.AddModelError(String.Empty, "Please enter ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
                {
                ModelState.AddModelError(String.Empty, "Name must not be spaces");
                err_flag = false;
            }

           if(action_flag == "Create")
            {
                GL_001_CHART bnk = db.GL_001_CHART.Find(glay.vwstring0);
                if (bnk != null)
                    {
                ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                err_flag = false;
            }
            }
            string sqlstr = "select '1' query0 from GL_001_CHART where account_name=" + util.sqlquote(glay.vwstring1) + " and account_code <> " + util.sqlquote(glay.vwstring0);
            var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (bglist1 != null)
            {
                ModelState.AddModelError(String.Empty, "Can not accept duplicate Account Name");
                err_flag = false;
            }

            for (int count1 = 0; count1 < 10; count1++)
            {
                if (aheader7[count1] == "Y" && string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
                {
                    error_msg = aheader5[count1] + " is mandatory. ";
                    ModelState.AddModelError(String.Empty, error_msg);
                    err_flag = false;
                }
            }
        }

        private void read_record()
        {
            //glay.vwstrarray6 = new string[20];

            glay.vwstring0 = GL_001_CHART.account_code;
            glay.vwstring1 = GL_001_CHART.account_name;
            glay.vwstring6 = GL_001_CHART.account_type_code;
            glay.vwstring3 = GL_001_CHART.archiving;
            glay.vwstring5 = GL_001_CHART.note;
            glay.vwstring2 = GL_001_CHART.category_name;
            glay.vwstrarray6[0] = GL_001_CHART.analysis_code1;
            glay.vwstrarray6[1] = GL_001_CHART.analysis_code2;
            glay.vwstrarray6[2] = GL_001_CHART.analysis_code3;
            glay.vwstrarray6[3] = GL_001_CHART.analysis_code4;
            glay.vwstrarray6[4] = GL_001_CHART.analysis_code5;
            glay.vwstrarray6[5] = GL_001_CHART.analysis_code6;
            glay.vwstrarray6[6] = GL_001_CHART.analysis_code7;
            glay.vwstrarray6[7] = GL_001_CHART.analysis_code8;
            glay.vwstrarray6[8] = GL_001_CHART.analysis_code9;
            glay.vwstrarray6[9] = GL_001_CHART.analysis_code10;
            if (GL_001_CHART.active_status == "Y")
                glay.vwbool0 = true;
            glay.vwstring4 = GL_001_CHART.currency_code;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "CHART" && bg.document_code == GL_001_CHART.account_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();


        }
        private void select_query()
        {
            ViewBag.account = util.para_selectquery("55", glay.vwstring6,"N");
            //var bglist = from bg in db.GL_001_ATYPE
            //             where bg.active_status == "N"
            //             orderby bg.acct_type_desc
            //             select bg;
            //ViewBag.account = new SelectList(bglist.ToList(), "acct_type_code", "acct_type_desc", glay.vwstring2);

            var bgitem = from bg in db.GB_999_MSG
                         where bg.type_msg == "ARCH"
                         orderby bg.name1_msg
                         select bg;

            ViewBag.archiving = new SelectList(bgitem.ToList(), "code_msg", "name1_msg", glay.vwstring3);

            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring4);


        }

        private void error_message()
        {

        }

        private void display_curren()
        {
            glay.vwstring4 = pubsess.base_currency_code;

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
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstring11 = pubsess.multi_currency;
            glay.vwlist0 = new List<querylay>[20];
        }

        [HttpPost]
        public ActionResult pricehead_list(string id)
        {
            // write your query statement
            var hdet = from bg in db.GL_001_CATEG
                       join bk in db.GL_001_ATYPE
                       on new { a1 = bg.acct_cat_sequence } equals new { a1=bk.acct_cat_sequence}
                       into bk2
                       from bk3 in bk2.DefaultIfEmpty()
                       where bk3.acct_type_code == id
                       orderby bg.acct_cat_desc
                       select new
                       {
                           c1 = bg.acct_cat_sequence,
                           c2 = bg.acct_cat_desc
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

        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

            SelectList[] head_det = new SelectList[20];

            //Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            //psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "019" && bg.sequence_no != 99
                         select bg;

            foreach (var item in bglist.ToList())
            {
                int count2 = item.sequence_no;
                aheader7[count2] = item.mandatory_flag;
                glay.vwstrarray4[count2] = item.header_code;
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

        public ActionResult show_doc(int id)
        {
            var bglist = (from bg in db.GB_001_DOC
                          where bg.document_sequence == id
                          select bg).FirstOrDefault();


            byte[] imagedata = bglist.document_image;
            return File(imagedata, "png");
        }


        private void cal_auto()
        {
            var autoset = (from bg in db.GB_001_COY
                           where bg.id_code == "COYAUTO"
                           select bg.field16).FirstOrDefault();

            if (autoset == "Y")
                move_auto = "Y";

        }
    }
}
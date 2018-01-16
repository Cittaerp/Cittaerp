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
    public class JontController : Controller
    {
        GL_001_JONT GL_001_JONT = new GL_001_JONT();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase [] photo1;

        bool err_flag = true;
        string delmsg = "";
        string move_auto = "N";
        string key_val = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
           // util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            // Session["multi_curenchk"] = pubsess.multi_currency;

            var bglist = from bh in db.GL_001_JONT
                         //join bk1 in db.GL_001_ATYPE
                         //on new { a1 = bh.account_type_code } equals new { a1 = bk1.acct_type_code }
                         //into bk2
                         //from bk3 in bk2.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.journal_code,
                             vwstring1 = bh.name,
                             vwstring2 = bh.journal_type,
                            // vwstring2 = bh.b
                           };

            return View(bglist.ToList());


        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
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
            psess = (psess)Session["psess"];
            
            ViewBag.action_flag = "Edit";
            psess.temp5 = move_auto;
            Session["psess"] = psess;
           
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            key_val = key1;
                read_record();
            
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
            if (util.delete_check("JONT", glay.vwstring0))
            {
                GL_001_JONT = db.GL_001_JONT.Find(glay.vwstring0);
                db.GL_001_JONT.Remove(GL_001_JONT);
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
            string str1 = "delete from GL_001_JONT where journal_code=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "insert into GL_001_JONT(journal_code,sequence_no,name,journal_type,analysis_code,visibility,created_by) values (";
            string str2 = "";
            for (int count1 = 0; count1 < glay.vwstrarray5.Length; count1++)
            {
                if (!string.IsNullOrWhiteSpace(glay.vwstrarray5[count1]))
                {
                    str2 = str1 + util.sqlquote(glay.vwstring0) + ",";
                    str2 += (count1 + 1).ToString() + ",";
                    str2 += util.sqlquote(glay.vwstring1) + ",";
                    str2 += util.sqlquote(glay.vwstring2) + ",";
                    str2 += glay.vwblarray1[count1] ? "'Y'" : "'N'";
                    str2 += ",";
                    str2 += glay.vwblarray2[count1] ? "'Y'" : "'N'";
                    str2 += ",";
                    str2 += util.sqlquote(pubsess.userid) + ")";
                    db.Database.ExecuteSqlCommand(str2);
                }
            }

          

            //if (Session["action_flag"].ToString() == "Create")
            //{
            //    GL_001_JONT = new GL_001_JONT();
            //    GL_001_JONT.created_by = pubsess.userid;
            //    GL_001_JONT.created_date = DateTime.UtcNow;
            //    }
            //else
            //{
            //    GL_001_JONT = db.GL_001_JONT.Find(glay.vwstring0);
            //}

            //GL_001_JONT.journal_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            //GL_001_JONT.name = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            //GL_001_JONT.journal_type = string.IsNullOrWhiteSpace(glay.vwstring2) ?"":glay.vwstring2;
            //GL_001_JONT.modified_date = DateTime.UtcNow;
            //GL_001_JONT.modified_by = pubsess.userid;
            ////GL_001_JONT.note = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            ////GL_001_JONT.active_status = glay.vwbool0 ? "Y" : "N";

            //GL_001_JONT.analysis_code1 = "";
            //GL_001_JONT.analysis_code2 = "";
            //GL_001_JONT.analysis_code3 = "";
            //GL_001_JONT.analysis_code4 = "";
            //GL_001_JONT.analysis_code5 = "";
            //GL_001_JONT.analysis_code6 = "";
            //GL_001_JONT.analysis_code7 = "";
            //GL_001_JONT.analysis_code8 = "";
            //GL_001_JONT.analysis_code9 = "";
            //GL_001_JONT.analysis_code10 = "";
            //if (glay.vwstrarray6 != null)
            //{
            //    int arrlen = glay.vwstrarray6.Length;
            //    if (arrlen > 0)
            //    {
            //        GL_001_JONT.analysis_code1 = glay.vwblarray1[0] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible1 = glay.vwblarray2[0] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 1)
            //    {
            //        GL_001_JONT.analysis_code2 = glay.vwblarray1[1] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible2 = glay.vwblarray2[1] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 2)
            //    {
            //        GL_001_JONT.analysis_code3 = glay.vwblarray1[2] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible3 = glay.vwblarray2[2] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 3)
            //    {
            //        GL_001_JONT.analysis_code4 = glay.vwblarray1[3] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible4 = glay.vwblarray2[3] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 4)
            //    {
            //        GL_001_JONT.analysis_code5 = glay.vwblarray1[4] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible5 = glay.vwblarray2[4] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 5)
            //    {
            //        GL_001_JONT.analysis_code6 = glay.vwblarray1[5] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible6 = glay.vwblarray2[5] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 6)
            //    {
            //        GL_001_JONT.analysis_code7 = glay.vwblarray1[6] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible7 = glay.vwblarray2[6] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 7)
            //    {
            //        GL_001_JONT.analysis_code8 = glay.vwblarray1[7] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible8 = glay.vwblarray2[7] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 8)
            //    {
            //        GL_001_JONT.analysis_code9 = glay.vwblarray1[8] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible9 = glay.vwblarray2[8] ? "'Y'" : "'N'";
            //    }
            //    if (arrlen > 9)
            //    {
            //        GL_001_JONT.analysis_code10 = glay.vwblarray1[9] ? "'Y'" : "'N'";
            //        GL_001_JONT.visible10 = glay.vwblarray2[9] ? "'Y'" : "'N'";
            //    }
            //    psess.intemp0 = arrlen;
            //}
           
            //if (Session["action_flag"].ToString() == "Create")
            //    db.Entry(GL_001_JONT).State = EntityState.Added;
            //else
            //    db.Entry(GL_001_JONT).State = EntityState.Modified;

            //try
            //{
            //    db.SaveChanges();
            //}

            //catch (Exception err)
            //{
            //    if (err.InnerException == null)
            //        ModelState.AddModelError(String.Empty, err.Message);
            //    else
            //        ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);

            //    err_flag = false;
            //}

            //if (err_flag)
            //{
            //    string str = "update GL_001_ATYPE set delete_flag ='Y' from GL_001_ATYPE a, GL_001_JONT b where a.acct_type_code = b.account_type_code";
            //    str += " and account_code =" + util.sqlquote(glay.vwstring0);
            //    db.Database.ExecuteSqlCommand(str);

            //    string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, GL_001_JONT b where header_sequence in (analysis_code1";
            //    stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
            //    stri += " and account_code =" + util.sqlquote(glay.vwstring0);
            //    db.Database.ExecuteSqlCommand(stri);

            //   // util.write_document("JONT", GL_001_JONT., photo1, glay.vwstrarray9);

            //}
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

            //if (Session["action_flag"].ToString() == "Create")
            //{
            //    GL_001_JONT bnk = db.GL_001_JONT.Find(glay.vwstring0);
            //    if (bnk != null)
            //        {
            //    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
            //    err_flag = false;
            //}
            }
            //string sqlstr = "select '1' query0 from GL_001_JONT where account_name=" + util.sqlquote(glay.vwstring1) + " and account_code <> " + util.sqlquote(glay.vwstring0);
            //var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            //if (bglist1 != null)
            //{
            //    ModelState.AddModelError(String.Empty, "Can not accept duplicate Account Name");
            //    err_flag = false;
            //}

            //for (int count1 = 0; count1 < 10; count1++)
            //{
            //    if (aheader7[count1] == "Y" && string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
            //    {
            //        error_msg = aheader5[count1] + " is mandatory. ";
            //        ModelState.AddModelError(String.Empty, error_msg);
            //        err_flag = false;
            //    }
            //}
        //}

        private void read_record()
        {
            //glay.vwstrarray6 = new string[20];


            initial_rtn();
            var bhlist = from bg in db.GL_001_JONT
                         where bg.journal_code == key_val
                         select bg;

            foreach (var item in bhlist.ToList())
            {
                if (item.sequence_no != 99)
                {
                    glay.vwstring0 = item.journal_code;
                    glay.vwstring1 = item.name;
                    glay.vwstring2 = item.journal_type;
                    glay.vwblarray1[item.sequence_no - 1] = item.analysis_code=="Y"? true:false;
                    glay.vwblarray2[item.sequence_no - 1] = item.visibility == "Y" ? true : false;
                }
                
            }

            //glay.vwstring0 = GL_001_JONT.journal_code;
            //glay.vwstring1 = GL_001_JONT.name;
            //glay.vwstring2 = GL_001_JONT.journal_type;
            //glay.vwblarray1[0] = GL_001_JONT.analysis_code1 == "Y" ? true : false;
            //glay.vwblarray1[1] = GL_001_JONT.analysis_code2 == "Y" ? true : false;
            //glay.vwblarray1[2] = GL_001_JONT.analysis_code3 == "Y" ? true : false;
            //glay.vwblarray1[3] = GL_001_JONT.analysis_code4 == "Y" ? true : false;
            //glay.vwblarray1[4] = GL_001_JONT.analysis_code5 == "Y" ? true : false;
            //glay.vwblarray1[5] = GL_001_JONT.analysis_code6 == "Y" ? true : false;
            //glay.vwblarray1[6] = GL_001_JONT.analysis_code7 == "Y" ? true : false;
            //glay.vwblarray1[7] = GL_001_JONT.analysis_code8 == "Y" ? true : false;
            //glay.vwblarray1[8] = GL_001_JONT.analysis_code9 == "Y" ? true : false;
            //glay.vwblarray1[9] = GL_001_JONT.analysis_code10 == "Y" ? true : false; 
            ////if (GL_001_JONT.active_status == "Y")
            //    glay.vwbool0 = true;
            //glay.vwstring4 = GL_001_JONT.currency_code;

            //var bglist = from bg in db.GB_001_DOC
            //             where bg.screen_code == "CHART" && bg.document_code == GL_001_JONT.account_code
            //             orderby bg.document_sequence
            //             select bg;

            //ViewBag.anapict = bglist.ToList();


        }
        private void select_query()
        {
            //ViewBag.account = util.para_selectquery("011", glay.vwstring6,"N");
            ////var bglist = from bg in db.GL_001_ATYPE
            ////             where bg.active_status == "N"
            ////             orderby bg.acct_type_desc
            ////             select bg;
            ////ViewBag.account = new SelectList(bglist.ToList(), "acct_type_code", "acct_type_desc", glay.vwstring2);

            //var bgitem = from bg in db.GB_999_MSG
            //             where bg.type_msg == "ARCH"
            //             orderby bg.name1_msg
            //             select bg;

            //ViewBag.archiving = new SelectList(bgitem.ToList(), "code_msg", "name1_msg", glay.vwstring3);

            //string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            //var emp1 = db.Database.SqlQuery<querylay>(str1);
            //ViewBag.currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring4);


        }

        private void error_message()
        {

        }

        private void display_curren()
        {
           // glay.vwstring4 = pubsess.base_currency_code;

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
            glay.vwblarray0 = new bool[20];
            glay.vwblarray1 = new bool[20];
            glay.vwblarray2 = new bool[20];
            //glay.vwstring11 = pubsess.multi_currency;
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
            psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "010" && bg.sequence_no != 99
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

            for (int count1 = 0; count1 < 10; count1++)
            {
                if (aheader7[count1] == "Y")
                {
                    glay.vwblarray0[count1] = true;
                    glay.vwblarray1[count1] = true;
                    glay.vwblarray2[count1] = true;
                }
            }
        
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
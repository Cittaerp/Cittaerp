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
    public class SalesController : Controller
    {
        AR_001_STRAN AR_001_STRAN = new AR_001_STRAN();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        bool err_flag = true;

        //
        // GET: /Citta/
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            

            var bglist = from bh in db.AR_001_STRAN
                         where bh.sequence_no == 0
                         select new vw_genlay
                         {
                             vwstring0 = bh.order_code,
                             vwstring1 = bh.order_name,
                             vwstring2 = bh.quote,
                             vwstring3 = bh.order,
                             vwstring4 = bh.invoice,
                             vwstring5 = bh.waybill,
                             vwstring6 = bh.active_status == "N" ? "Active" : "Inactive"
                         };

            return View(bglist.Distinct().ToList());


        }

        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;
            photo1 = photofile;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }

        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            AR_001_STRAN = db.AR_001_STRAN.Find(key1,0);
            if (AR_001_STRAN != null)
                read_record();
            header_ana();
            select_query();
            return View(glay);

        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
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
            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }


        private void delete_record()
        {
            AR_001_STRAN = db.AR_001_STRAN.Find(glay.vwstring0);
            if (AR_001_STRAN != null)
            {
                db.AR_001_STRAN.Remove(AR_001_STRAN);
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
            string str1 = "delete from AR_001_STRAN where order_code=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "insert into AR_001_STRAN(order_code,sequence_no,order_name,quote,[order],waybill,invoice,confirm_order,reserve_stock,tax_invoice1,tax_invoice2,";
            str1 += "header_code,mandatory_flag,modified_by,created_by,note,active_status) values(";
            str1 += util.sqlquote(glay.vwstring0) + ",0,";
            str1 += util.sqlquote(glay.vwstring1) + ",";
            str1 += glay.vwblarray0[0]   ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[1] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[2] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[3] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[5] ? "'Y'," : "'N',";
            str1 += glay.vwblarray0[6] ? "'Y'," : "'N',";
            if (glay.vwstring5 == "N")
            {
                str1 += "'','',";
            }
            else
            {
                str1 += util.sqlquote(glay.vwstring3) + ",";
                str1 += util.sqlquote(glay.vwstring4) + ",";
            }
            str1 += "'','',";
            str1 += util.sqlquote(pubsess.userid) + ",";
            str1 += util.sqlquote(pubsess.userid) + ",";
            str1 += util.sqlquote(glay.vwstring2) + ",";
            str1 += glay.vwblarray0[4] ? "'Y'" : "'N'";
            str1 += ")";

            try
            {
                db.Database.ExecuteSqlCommand(str1);
            }

            catch (Exception err)
            {
                if (err.InnerException == null)
                    ModelState.AddModelError(String.Empty, err.Message);
                else
                    ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);

                err_flag = false;
            }

            str1 = "insert into AR_001_STRAN(order_code,sequence_no,header_code,mandatory_flag,created_by,modified_by) values (";
            string str2 = "";
            for (int count1 = 0; count1 < 10; count1++)
            {
                if (!string.IsNullOrWhiteSpace(glay.vwstrarray0[count1]))
                {
                    str2 = str1 + util.sqlquote(glay.vwstring0) + ",";
                    str2 += (count1 + 1).ToString() + ",";
                    str2 += util.sqlquote(glay.vwstrarray0[count1]) + ",";
                    str2 += glay.vwblarray1[count1] ? "'Y'" : "'N'";
                    str2 += "," + util.sqlquote(pubsess.userid);
                    str2 += "," + util.sqlquote(pubsess.userid) + ")";
                    try
                    {
                        db.Database.ExecuteSqlCommand(str2);
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
            }


            if (err_flag)
            {
                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("SALES", glay.vwstring0, photo1, glay.vwstrarray9);
                }
               }

        }

        private void validation_routine()
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please enter Id");
                err_flag = false;
            }
            if (glay.vwstring5 == "Y")
            {
                if (string.IsNullOrWhiteSpace(glay.vwstring3) && string.IsNullOrWhiteSpace(glay.vwstring4))
                {
                    ModelState.AddModelError(String.Empty, "All TAX IDs must not be empty");
                    err_flag = false;
                }
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

            glay.vwblarray0[0] = false;
            glay.vwblarray0[1] = false;
            glay.vwblarray0[2] = false;
            glay.vwblarray0[3] = false;
            glay.vwblarray0[4] = false;

            glay.vwstring0 = AR_001_STRAN.order_code;
            glay.vwstring1 = AR_001_STRAN.order_name;
            glay.vwblarray0[0] = AR_001_STRAN.quote == "Y" ? true : false;
            glay.vwblarray0[1] = AR_001_STRAN.order == "Y" ? true : false;
            glay.vwblarray0[2] = AR_001_STRAN.waybill == "Y" ? true : false;
            glay.vwblarray0[3] = AR_001_STRAN.invoice == "Y" ? true : false;
            glay.vwblarray0[5] = AR_001_STRAN.confirm_order == "Y" ? true : false;
            glay.vwblarray0[6] = AR_001_STRAN.reserve_stock == "Y" ? true : false;
            glay.vwstring2 = AR_001_STRAN.note;
            glay.vwstring3 = AR_001_STRAN.tax_invoice1;
            glay.vwstring4 = AR_001_STRAN.tax_invoice2;
            if (string.IsNullOrWhiteSpace(glay.vwstring3) && string.IsNullOrWhiteSpace(glay.vwstring4))
            {
                glay.vwstring5 = "N";
            }
            else glay.vwstring5 = "Y";
            if (AR_001_STRAN.active_status == "Y")
                glay.vwblarray0[4] = true;
            var bhlist = from bg in db.AR_001_STRAN
                         where bg.order_code == glay.vwstring0
                         select bg;

            foreach (var item in bhlist.ToList())
            {
                if (item.sequence_no != 0)
                {
                    glay.vwstring0 = item.order_code;
                    glay.vwstrarray0[item.sequence_no - 1] = item.header_code;
                    glay.vwblarray1[item.sequence_no - 1] = item.mandatory_flag == "Y" ? true : false;
                }
            }
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "SALES" && bg.document_code == AR_001_STRAN.order_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();


        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwblarray0 = new bool[20];
            glay.vwblarray1 = new bool[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstring5 = "N";
            glay.vwblarray0[1] = true;
            glay.vwblarray0[3] = true;
            glay.vwlist0 = new List<querylay>[20];
        
        }
        private void select_query()
        {
            ViewBag.header1 = util.para_selectquery("50","");
            //var bgitem = from bh in db.GB_001_HANAL
            //             where bh.active_status == "N"
            //             orderby bh.header_description
            //             select bh;

            //ViewBag.header1 = bgitem.ToList();

            var bglistb = from bg in db.GB_001_TAX
                          where bg.computation_basis == "T" && bg.active_status == "N" && bg.module_basis.Contains("S")
                          orderby bg.tax_name
                          select new { c1 = bg.tax_code, c2 = bg.tax_name };

            ViewBag.tax = new SelectList(bglistb.ToList(), "c1", "c2");


        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AR_001_STRAN] where order_code=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }


        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

            SelectList[] head_det = new SelectList[20];

           // Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "006" && bg.sequence_no != 99
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
                    var bglist3 = from bg in db.GB_001_DANAL
                                  where bg.header_sequence == item.header_code
                                  select bg;
                    head_det[count2] = new SelectList(bglist3.ToList(), "analysis_code", "analysis_description", glay.vwstrarray6[count2]);

                }

            }

           // Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
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
 
    }
}
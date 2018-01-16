using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;
using System.IO;

namespace CittaErp.Controllers
{

    public class PsetController : Controller
    {
        //
        // GET: /Employee/

        GB_001_COY GB_001_COY = new GB_001_COY();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;
        HttpPostedFileBase photo2;

        string ptype = "";
        bool err_flag = true;
        string move_auto = "N";
        bool up1_flag;

        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            var bglist = from bh in db.GB_001_PTYS

                         select new vw_genlay
                         {
                             //vwstring0 = bh.fee_id,
                             //vwstring1 = bh.fee_description1,
                             //vwstring2 = bh.active_status == "N" ? "Active" : "Inactive"
                         };

            return View(bglist.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            header_ana();
            select_query();
            cal_auto();
            //psess.temp5 = move_auto;
            if (move_auto == "Y")
                glay.vwstring0 = "AUTO";
            return View(glay);
        }
        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;
            photo1 = photofile;
            photo2 = picture1;
            cal_auto();
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);

        }

        public ActionResult Edit()
        {
            ViewBag.action_flag = "Edit";

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            GB_001_COY = (from bk in db.GB_001_COY
                          select bk).FirstOrDefault();
            if (GB_001_COY != null)
                read_record();
            header_ana();
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1)
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
            photo2 = picture1;
            update_file();
            if (err_flag)
                return RedirectToAction("Home", "Log_in");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }
        private void delete_record()
        {
            GB_001_COY = db.GB_001_COY.Find(glay.vwstring0);
            if (GB_001_COY != null)
            {
                db.GB_001_COY.Remove(GB_001_COY);
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
            // company property
            GB_001_COY = db.GB_001_COY.Find("COYPPRTY");
            up1_flag = false;
            if (GB_001_COY == null)
            {
                GB_001_COY = new GB_001_COY();
                init_coyrecord();
                GB_001_COY.created_by = pubsess.userid;
                GB_001_COY.created_date = DateTime.UtcNow;
                GB_001_COY.id_code = "COYPPRTY";
                up1_flag = true;
            }

            GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
            GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
            GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
            GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
            GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
            GB_001_COY.field9 = string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
            GB_001_COY.field10 = string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
            GB_001_COY.field11 = glay.vwdecimal0.ToString();
            GB_001_COY.field12 = glay.vwdecimal1.ToString();
            GB_001_COY.field13 = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            GB_001_COY.field14 = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GB_001_COY.field15 = glay.vwdecimal2.ToString();
            GB_001_COY.field16 = glay.vwdecimal3.ToString();
            GB_001_COY.modified_by = pubsess.userid;
            GB_001_COY.modified_date = DateTime.UtcNow;

            if (up1_flag)
                db.Entry(GB_001_COY).State = EntityState.Added;
            else
                db.Entry(GB_001_COY).State = EntityState.Modified;

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

   // company property1
            GB_001_COY = db.GB_001_COY.Find("COYPPRTY1");
            up1_flag = false;
            if (GB_001_COY == null)
            {
                GB_001_COY = new GB_001_COY();
                init_coyrecord();
                GB_001_COY.created_by = pubsess.userid;
                GB_001_COY.created_date = DateTime.UtcNow;
                GB_001_COY.id_code = "COYPPRTY1";
                up1_flag = true;
            }

            GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray1[0]) ? "" : glay.vwstrarray1[0];
            GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray1[1]) ? "" : glay.vwstrarray1[1];
            GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstrarray1[2]) ? "" : glay.vwstrarray1[2];
            GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray1[3]) ? "" : glay.vwstrarray1[3];
            GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray1[4]) ? "" : glay.vwstrarray1[4];
            GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray1[5]) ? "" : glay.vwstrarray1[5];
            GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray1[6]) ? "" : glay.vwstrarray1[6];
            GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstrarray1[7]) ? "" : glay.vwstrarray1[7];
            GB_001_COY.field9 = string.IsNullOrWhiteSpace(glay.vwstrarray1[8]) ? "" : glay.vwstrarray1[8];
            GB_001_COY.field10 = string.IsNullOrWhiteSpace(glay.vwstrarray1[9]) ? "" : glay.vwstrarray1[9];
            GB_001_COY.field11 = glay.vwdecimal4.ToString();
            GB_001_COY.field12 = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            GB_001_COY.field13 = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            GB_001_COY.field14 = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            GB_001_COY.field15 = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            GB_001_COY.modified_by = pubsess.userid;
            GB_001_COY.modified_date = DateTime.UtcNow;

            if (up1_flag)
                db.Entry(GB_001_COY).State = EntityState.Added;
            else
                db.Entry(GB_001_COY).State = EntityState.Modified;

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

 // company property2
            GB_001_COY = db.GB_001_COY.Find("COYPPRTY2");
            up1_flag = false;
            if (GB_001_COY == null)
            {
                GB_001_COY = new GB_001_COY();
                init_coyrecord();
                GB_001_COY.created_by = pubsess.userid;
                GB_001_COY.created_date = DateTime.UtcNow;
                GB_001_COY.id_code = "COYPPRTY2";
                up1_flag = true;
            }

            GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstring8) ? "" : glay.vwstring8;
            GB_001_COY.modified_by = pubsess.userid;
            GB_001_COY.modified_date = DateTime.UtcNow;

            if (up1_flag)
                db.Entry(GB_001_COY).State = EntityState.Added;
            else
                db.Entry(GB_001_COY).State = EntityState.Modified;

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
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            
            
            //for (int count1 = 0; count1 < 10; count1++)
            //{
            //    if (aheader7[count1] == "Y" && string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
            //    {
            //        error_msg = aheader5[count1] + " is mandatory. ";
            //        ModelState.AddModelError(String.Empty, error_msg);
            //        err_flag = false;
            //    }
            //}
         

        }
        private void select_query()
        {
            ViewBag.glcom = util.read_ledger("020", glay.vwstring2);
            ViewBag.glcomt = util.read_ledger("020", glay.vwstring1);
            ViewBag.glfee = util.read_ledger("020");
            ViewBag.ptty = util.read_ledger("020", glay.vwstring0);
            ViewBag.comexp = util.read_ledger("020", glay.vwstring3);
            ViewBag.dcomexp = util.read_ledger("020", glay.vwstring4);
            ViewBag.wht = util.read_ledger("017", glay.vwstring5);
            

            var bglist = from bh in db.GB_001_HEADER
                         where bh.header_type_code == "010" && bh.sequence_no!=99
                        select new { c1 = bh.header_type_code + "[]" + bh.sequence_no, c2 = bh.header_code };
            ViewBag.staff = new SelectList(bglist.ToList(), "c1", "c2", glay.vwstring8);

            var pgf = from bg in db.GB_001_PCODE
                      where bg.parameter_type == "03"
                      select bg;
            ViewBag.rcode = new SelectList(pgf.ToList(), "parameter_code", "parameter_name", glay.vwstring6);

            var fih = from bg in db.IV_001_WAREH
                      select bg;
            ViewBag.ware = new SelectList(fih.ToList(), "warehouse_code", "warehouse_name", glay.vwstring7);
               
            
        }
        private void initial_rtn()
        {

            glay.vwstrarray0 = new string[50];
            glay.vwstrarray1 = new string[50];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwlist0 = new List<querylay>[20];
        }
        private void init_coyrecord()
        {
            GB_001_COY.field1 = "";
            GB_001_COY.field2 = "";
            GB_001_COY.field3 = "";
            GB_001_COY.field4 = "";
            GB_001_COY.field5 = "";
            GB_001_COY.field6 = "";
            GB_001_COY.field7 = "";
            GB_001_COY.field8 = "";
            GB_001_COY.field9 = "";
            GB_001_COY.field10 = "";
            GB_001_COY.field11 = "";
            GB_001_COY.field12 = "";
            GB_001_COY.field13 = "";
            GB_001_COY.field14 = "";
            GB_001_COY.field15 = "";
            GB_001_COY.field16 = "";

        }
	
        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray6 = new string[20];

            GB_001_COY rcoypprty = db.GB_001_COY.Find("COYPPRTY");
            GB_001_COY rcoypprty1 = db.GB_001_COY.Find("COYPPRTY1");
            GB_001_COY rcoypprty2 = db.GB_001_COY.Find("COYPPRTY2");
            if (rcoypprty != null)
            {
                glay.vwstrarray0[0] = rcoypprty.field1;
                glay.vwstrarray0[1] = rcoypprty.field2;
                glay.vwstrarray0[2] = rcoypprty.field3;
                glay.vwstrarray0[3] = rcoypprty.field4;
                glay.vwstrarray0[4] = rcoypprty.field5;
                glay.vwstrarray0[5] = rcoypprty.field6;
                glay.vwstrarray0[6] = rcoypprty.field7;
                glay.vwstrarray0[7] = rcoypprty.field8;
                glay.vwstrarray0[8] = rcoypprty.field9;
                glay.vwstrarray0[9] = rcoypprty.field10;
                decimal k2 = 0;
                decimal.TryParse(rcoypprty.field11, out k2);
                glay.vwdecimal0 = k2;
                k2 = 0;
                decimal.TryParse(rcoypprty.field12, out k2);
                glay.vwdecimal1 = k2;
                k2 = 0;
                decimal.TryParse(rcoypprty.field15, out k2);
                glay.vwdecimal2 = k2;
                k2 = 0;
                decimal.TryParse(rcoypprty.field16, out k2);
                glay.vwdecimal3 = k2;
                glay.vwstring1 = rcoypprty.field13;
                glay.vwstring2 = rcoypprty.field14;
            }
            if (rcoypprty1 != null)
            {
                glay.vwstrarray1[0] = rcoypprty1.field1;
                glay.vwstrarray1[1] = rcoypprty1.field2;
                glay.vwstrarray1[2] = rcoypprty1.field3;
                glay.vwstrarray1[3] = rcoypprty1.field4;
                glay.vwstrarray1[4] = rcoypprty1.field5;
                glay.vwstrarray1[5] = rcoypprty1.field6;
                glay.vwstrarray1[6] = rcoypprty1.field7;
                glay.vwstrarray1[7] = rcoypprty1.field8;
                glay.vwstrarray1[8] = rcoypprty1.field9;
                glay.vwstrarray1[9] = rcoypprty1.field10;
                decimal k2 = 0;
                decimal.TryParse(rcoypprty1.field11, out k2);
                glay.vwdecimal4 = k2;
                glay.vwstring3 = rcoypprty1.field12;
                glay.vwstring4 = rcoypprty1.field13;
                glay.vwstring5 = rcoypprty1.field14;
                glay.vwstring0 = rcoypprty1.field15;

            } 
            if (rcoypprty2 != null)
            {
                glay.vwstring6 = rcoypprty2.field1;
                glay.vwstring7 = rcoypprty2.field2;
                glay.vwstring8 = rcoypprty2.field3;

            }
          
        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_COY] where company_code=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
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

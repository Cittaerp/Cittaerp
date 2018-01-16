using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;
using anchor1.Filters;
using System.IO;

namespace CittaErp.Controllers
{

    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        GB_001_EMP GB_001_EMP = new GB_001_EMP();
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
        string action_flag = "";
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            var bglist = from bh in db.GB_001_EMP

                         select new vw_genlay
                         {
                             vwstring0 = bh.employee_code,
                             vwstring1 = bh.name,
                             vwstring2 = bh.department,
                             vwint0 = bh.job_role,
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
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
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

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            GB_001_EMP = db.GB_001_EMP.Find(key1);
            if (GB_001_EMP != null)
                read_record();
            header_ana();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile, HttpPostedFileBase picture1)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
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
                return RedirectToAction("Index");

            initial_rtn();
            header_ana();
            select_query();
            return View(glay);
        }
        private void delete_record()
        {
            GB_001_EMP = db.GB_001_EMP.Find(glay.vwstring0);
            if (GB_001_EMP != null)
            {
                db.GB_001_EMP.Remove(GB_001_EMP);
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
                GB_001_EMP = new GB_001_EMP();
                GB_001_EMP.created_by = pubsess.userid;
                GB_001_EMP.created_date = DateTime.UtcNow;
                if (move_auto == "Y")
                    glay.vwstring0 = util.autogen_num("EMP");
            }
            else
            {
                GB_001_EMP = db.GB_001_EMP.Find(glay.vwstring0);
            }
            GB_001_EMP.attach_document = "";
            GB_001_EMP.employee_code = glay.vwstring0;
            GB_001_EMP.name = glay.vwstring1;
            GB_001_EMP.department = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            GB_001_EMP.job_role = glay.vwint0;
            GB_001_EMP.email = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            GB_001_EMP.phone1 = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            GB_001_EMP.phone2 = string.IsNullOrWhiteSpace(glay.vwstring10) ? "" : glay.vwstring10;
            GB_001_EMP.city = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            GB_001_EMP.commission_policy = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            GB_001_EMP.country = string.IsNullOrWhiteSpace(glay.vwstring8) ? "" : glay.vwstring8;
            GB_001_EMP.modified_date = DateTime.UtcNow;
            GB_001_EMP.modified_by = pubsess.userid;
            GB_001_EMP.active_status = glay.vwbool0 ? "Y" : "N";
            GB_001_EMP.note = string.IsNullOrWhiteSpace(glay.vwstring9) ? "" : glay.vwstring9;
            GB_001_EMP.address_home = string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
            GB_001_EMP.gl_commission_pay = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            GB_001_EMP.gl_iou_adv = string.IsNullOrWhiteSpace(glay.vwstring11) ? "" : glay.vwstring11;
            GB_001_EMP.bank_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
            GB_001_EMP.bank_acc = string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
            GB_001_EMP.attach_document = "";
            GB_001_EMP.analysis_code1 = "";
            GB_001_EMP.analysis_code2 = "";
            GB_001_EMP.close_code = "";
            GB_001_EMP.analysis_code3 = "";
            GB_001_EMP.analysis_code4 = "";
            GB_001_EMP.analysis_code5 = "";
            GB_001_EMP.analysis_code6 = "";
            GB_001_EMP.analysis_code7 = "";
            GB_001_EMP.analysis_code8 = "";
            GB_001_EMP.analysis_code9 = "";
            GB_001_EMP.analysis_code10 = "";
            if (glay.vwstrarray6 != null)
            {
                int arrlen = glay.vwstrarray6.Length;
                if (arrlen > 0)
                    GB_001_EMP.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                if (arrlen > 1)
                    GB_001_EMP.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                if (arrlen > 2)
                    GB_001_EMP.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                if (arrlen > 3)
                    GB_001_EMP.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                if (arrlen > 4)
                    GB_001_EMP.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                if (arrlen > 5)
                    GB_001_EMP.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                if (arrlen > 6)
                    GB_001_EMP.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                if (arrlen > 7)
                    GB_001_EMP.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                if (arrlen > 8)
                    GB_001_EMP.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                if (arrlen > 9)
                    GB_001_EMP.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                psess.intemp0 = arrlen;
                Session["psess"] = psess;
                if (photo2 != null)
                {
                    if ((photo2 != null && Session["action_flag"].ToString() != "Create") || (Session["action_flag"].ToString() == "Create"))
                    {
                        byte[] uploaded = new byte[photo2.InputStream.Length];
                        photo2.InputStream.Read(uploaded, 0, uploaded.Length);
                        GB_001_EMP.emp_photo = uploaded;
                    }
                }
            


               if(action_flag == "Create")
                    db.Entry(GB_001_EMP).State = EntityState.Added;
                else
                    db.Entry(GB_001_EMP).State = EntityState.Modified;

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
                    string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, GB_001_EMP b where header_sequence in (analysis_code1";
                    stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                    stri += " and employee_code =" + util.sqlquote(glay.vwstring0);
                    //db.Database.ExecuteSqlCommand(stri);
              
                    {
                        util.write_document("EMP", GB_001_EMP.employee_code, photo1, glay.vwstrarray9);
                    }

                }

            }
        }
        private void validation_routine()
        {
            string error_msg = "";
            if (string.IsNullOrWhiteSpace(glay.vwstring0) && move_auto != "Y")
                error_msg = "Please enter ID";

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Name must not be spaces";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
            
           if(action_flag == "Create")
            {
                GB_001_EMP bnk = db.GB_001_EMP.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
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
        private void select_query()
        {
            ViewBag.glcom = util.read_ledger("020", glay.vwstring3);
            ViewBag.gliou = util.read_ledger("020", glay.vwstring11);

            ViewBag.jobrole = util.para_selectquery("04", glay.vwint0.ToString());
            ViewBag.bank = util.para_selectquery("004", glay.vwstrarray0[1]);
           
            ViewBag.employee = util.para_selectquery("05", glay.vwstring2);
            //var emp = from pf in db.GB_001_PCODE
            //          where pf.parameter_type == "05" && pf.active_status == "N"
            //          orderby pf.parameter_name
            //          select pf;
            //ViewBag.employee = new SelectList(emp.ToList(), "parameter_code", "parameter_name", glay.vwstring3);

            ViewBag.country = util.para_selectquery("13", glay.vwstring8,"N");
            //var bglisti = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "13" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //              select bg;
            //ViewBag.country = new SelectList(bglisti.ToList(), "parameter_code", "parameter_name", glay.vwstring8);

        }
        private void initial_rtn()
        {

            glay.vwstrarray0 = new string[50];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray0[10] = "Y";
            glay.vwlist0 = new List<querylay>[20];
        }

        private void read_record()
        {
             glay.vwstrarray0 = new string[50];
            glay.vwstrarray6 = new string[20];

            glay.vwstring0 = GB_001_EMP.employee_code;
            glay.vwstring1 = GB_001_EMP.name;
            glay.vwstring2 = GB_001_EMP.department;
            glay.vwint0 = GB_001_EMP.job_role;
            glay.vwstring4 = GB_001_EMP.email;
            glay.vwstring5 = GB_001_EMP.phone1;
            glay.vwstring10 = GB_001_EMP.phone2;
            glay.vwstring6 = GB_001_EMP.city;
            glay.vwstring7 = GB_001_EMP.commission_policy;
            glay.vwstring8 = GB_001_EMP.country;
            glay.vwstring3 = GB_001_EMP.gl_commission_pay;
            glay.vwstring11 = GB_001_EMP.gl_iou_adv;
            glay.vwstrarray0[0] = GB_001_EMP.address_home;
            glay.vwstrarray0[1] = GB_001_EMP.bank_code;
            glay.vwstrarray0[2] = GB_001_EMP.bank_acc;
            glay.vwstrarray6[0] = GB_001_EMP.analysis_code1;
            glay.vwstrarray6[1] = GB_001_EMP.analysis_code2;
            glay.vwstrarray6[2] = GB_001_EMP.analysis_code3;
            glay.vwstrarray6[3] = GB_001_EMP.analysis_code4;
            glay.vwstrarray6[4] = GB_001_EMP.analysis_code5;
            glay.vwstrarray6[5] = GB_001_EMP.analysis_code6;
            glay.vwstrarray6[6] = GB_001_EMP.analysis_code7;
            glay.vwstrarray6[7] = GB_001_EMP.analysis_code8;
            glay.vwstrarray6[8] = GB_001_EMP.analysis_code9;
            glay.vwstrarray6[9] = GB_001_EMP.analysis_code10;
           
            if (GB_001_EMP.active_status == "Y")
            {
                glay.vwbool0 = true;
            }
            glay.vwstring9 = GB_001_EMP.note;

            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "EMP" && bg.document_code == GB_001_EMP.employee_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();

        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_EMP] where employee_code=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        public ActionResult show(string id)
        {
            var dir = "";
            GB_001_EMP = db.GB_001_EMP.Find(id);
            if (GB_001_EMP != null && GB_001_EMP.emp_photo != null)
            {
                byte[] imagedata = GB_001_EMP.emp_photo;

                return File(imagedata, "png");
            }
            else
            {
                dir = Server.MapPath("~/image");
                var path = Path.Combine(dir, "noLogo.png"); //validate the path for security or use other means to generate the path.
                return File(path, "png");
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
                         where bg.header_type_code == "016" && bg.sequence_no != 99
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

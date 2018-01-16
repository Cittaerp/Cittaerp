using CittaErp.Models;
using anchor1.Filters;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class CoyController : Controller
    {
        GB_001_COY GB_001_COY = new GB_001_COY();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();

        string atype = "com"; string prd_go = "";
        bool err_flag = true;
        HttpPostedFileBase photo1;
        HttpPostedFileBase[] photo2;
        bool up1_flag;

        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
           
            var bglist = from bh in db.GB_001_COY
                         select bh;

            return View(bglist.ToList());

        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase photofile, HttpPostedFileBase[] picture1)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
           
            glay = glay_in;
            photo1 = photofile;
            photo2 = picture1;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            initial_rtn();
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit()
        {
            util.init_values();
            ViewBag.action_flag = "Edit";
            psess = (psess)Session["psess"];
            initial_rtn();

            pubsess = (pubsess)Session["pubsess"];
            GB_001_COY = (from bk in db.GB_001_COY
                          select bk).FirstOrDefault();

            if (GB_001_COY != null)
            {
                read_record();
               //read_pst();
            }
            select_query();
            return View(glay);

        }

        //private void read_pst()
        //{
        //    var prdseting = from bg in db.GB_001_PST
                            
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase photofile, HttpPostedFileBase[] picture1,string coybtn)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;

            if (coybtn=="0")
                return RedirectToAction("Home", "Log_in");

            if (id_xhrt=="D")
            { 
                delete_record();
                return RedirectToAction("Index");
            }

            photo1 = photofile;
            photo2 = picture1;
            update_file();
            if (prd_go == "Y" && err_flag != false ){
                psess.temp1 = (from bg in db.GB_001_COY where bg.id_code == "COYPRD" select bg.field3).FirstOrDefault();
                Session["psess"] = psess;
                return RedirectToAction("Prd", "PeriodR");
        }
            else
            {
                if (err_flag)
                    return RedirectToAction("Home", "Log_in");
            }
            initial_rtn();
            select_query();
            return View(glay);
        }

        //[HttpPost]
        //public ActionResult Index1(string id_xhrt)
        //{
        //    if (id_xhrt == "D")
        //    {
        //        delete_record();
        //        return RedirectToAction("Index");
        //    }

        //    update_file();
        //    if (err_flag)
        //        return RedirectToAction("Index");
        //    return View(glay);
            
        //}
        private void delete_record()
        {
            GB_001_COY = db.GB_001_COY.Find(glay.vwstring0);
            if (GB_001_COY!=null)
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

// company info
            GB_001_COY = db.GB_001_COY.Find("COYINFO");
            up1_flag = false;
            if (GB_001_COY==null)
            {
                GB_001_COY = new GB_001_COY();
                init_coyrecord();
                GB_001_COY.created_by = pubsess.userid;
                GB_001_COY.created_date = DateTime.UtcNow;
                GB_001_COY.id_code = "COYINFO";
                up1_flag = true;
            }

                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                GB_001_COY.field2= string.IsNullOrWhiteSpace(glay.vwstrarray0[0]) ? "" : glay.vwstrarray0[0];
                GB_001_COY.field3= string.IsNullOrWhiteSpace(glay.vwstrarray0[1]) ? "" : glay.vwstrarray0[1];
                GB_001_COY.field9 = glay.vwint0.ToString();
                GB_001_COY.field4= string.IsNullOrWhiteSpace(glay.vwstrarray0[2]) ? "" : glay.vwstrarray0[2];
                GB_001_COY.field5= string.IsNullOrWhiteSpace(glay.vwstrarray0[3]) ? "" : glay.vwstrarray0[3];
                GB_001_COY.field6= string.IsNullOrWhiteSpace(glay.vwstring9) ? "" : glay.vwstring9;
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray0[14]) ? "" : glay.vwstrarray0[14];
                GB_001_COY.field8 = "N";
                GB_001_COY.modified_by = pubsess.userid;
                GB_001_COY.modified_date = DateTime.UtcNow;

                if (photo1 != null )
                {

                    byte[] uploaded = new byte[photo1.InputStream.Length];
                    photo1.InputStream.Read(uploaded, 0, uploaded.Length);
                    GB_001_COY.company_logo = uploaded;
                }

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

//company address
                GB_001_COY = db.GB_001_COY.Find("COYADD");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY  = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYADD";
                    up1_flag = true;
                }

                GB_001_COY.field10 = string.IsNullOrWhiteSpace(glay.vwstrarray0[4]) ? "" : glay.vwstrarray0[4];
                GB_001_COY.field1= string.IsNullOrWhiteSpace(glay.vwstrarray0[5]) ? "" : glay.vwstrarray0[5];
                GB_001_COY.field2= string.IsNullOrWhiteSpace(glay.vwstrarray0[6]) ? "" : glay.vwstrarray0[6];
                GB_001_COY.field3= string.IsNullOrWhiteSpace(glay.vwstrarray0[7]) ? "" : glay.vwstrarray0[7];
                GB_001_COY.field4= string.IsNullOrWhiteSpace(glay.vwstrarray0[8]) ? "" : glay.vwstrarray0[8];
                GB_001_COY.field5= string.IsNullOrWhiteSpace(glay.vwstrarray0[9]) ? "" : glay.vwstrarray0[9];
                GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray0[10]) ? "" : glay.vwstrarray0[10];
                GB_001_COY.field7= string.IsNullOrWhiteSpace(glay.vwstrarray0[11]) ? "" : glay.vwstrarray0[11];
                GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstrarray0[12]) ? "" : glay.vwstrarray0[12];
                GB_001_COY.field9= string.IsNullOrWhiteSpace(glay.vwstrarray0[13]) ? "" : glay.vwstrarray0[13];
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


//company setting
                GB_001_COY = db.GB_001_COY.Find("COYSET");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY  = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYSET";
                    up1_flag = true;
                }
                
                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
               // GB_001_COY.field5 = glay.vwdclarray0[0].ToString();
                GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[16]) ? "" : glay.vwstrarray0[16];
            
                GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstrarray0[32]) ? "" : glay.vwstrarray0[32];
                GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray0[33]) ? "" : glay.vwstrarray0[33];
                GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray0[34]) ? "" : glay.vwstrarray0[34];
                GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray0[15]) ? "" : glay.vwstrarray0[15];
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray0[31]) ? "" : glay.vwstrarray0[31];
                GB_001_COY.field8 = glay.vwbool0 == true ? "Y" : "N";
                GB_001_COY.field9 = glay.vwbool1 == true ? "Y" : "N";
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

                //company period selection
                GB_001_COY = db.GB_001_COY.Find("COYPRD");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYPRD";
                    up1_flag = true;
                }

                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[17]) ? "" : glay.vwstrarray0[17];
                if (glay.vwstrarray0[17] == "P")
                {
                    GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray1[0]) ? "" : glay.vwstrarray1[0];
                    GB_001_COY.field3 = glay.vwdecimal0.ToString();
                    GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray1[1]) ? "" : glay.vwstrarray1[1];
                    GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray1[2]) ? "" : glay.vwstrarray1[2];
                    GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray1[3]) ? "" : glay.vwstrarray1[3];
                    GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray1[4]) ? "" : glay.vwstrarray1[4];
                    GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
                }
                else
                {
                     GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[18]) ? "" : glay.vwstrarray0[18];
                    GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray0[19]) ? "" : glay.vwstrarray0[19];
                    GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray0[20]) ? "" : glay.vwstrarray0[20];
                    GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray0[21]) ? "" : glay.vwstrarray0[21];
                    GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                    GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
                }
                GB_001_COY.field9 = string.IsNullOrWhiteSpace(glay.vwstrarray1[5]) ? "" : glay.vwstrarray1[5];
                GB_001_COY.field10 = string.IsNullOrWhiteSpace(glay.vwstrarray1[6]) ? "" : glay.vwstrarray1[6]; 
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



                //company price
                GB_001_COY = db.GB_001_COY.Find("COYPRICE");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY  = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYPRICE";
                    up1_flag = true;
                }
                
            
               // GB_001_COY.field5 = glay.vwblarray0[1] ? "Y" : "N";
                GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
                GB_001_COY.field12 = glay.vwblarray0[1] ? "Y" : "N";
                GB_001_COY.field13 = glay.vwblarray0[2] ? "Y" : "N";
                GB_001_COY.field4 = glay.vwblarray0[0] ? "Y" : "N";  
                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[22]) ? "" : glay.vwstrarray0[22];
                GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstrarray0[23]) ? "" : glay.vwstrarray0[23];
                GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray0[24]) ? "" : glay.vwstrarray0[24];
                GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray0[25]) ? "Standard" : glay.vwstrarray0[25];
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray0[26]) ? "" : glay.vwstrarray0[26];
                GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstrarray0[27]) ? "" : glay.vwstrarray0[27];
                GB_001_COY.field9 = string.IsNullOrWhiteSpace(glay.vwstrarray0[28]) ? "" : glay.vwstrarray0[28];
                GB_001_COY.field10 = string.IsNullOrWhiteSpace(glay.vwstrarray0[29]) ? "" : glay.vwstrarray0[29];
                GB_001_COY.field11 = string.IsNullOrWhiteSpace(glay.vwstrarray0[30]) ? "" : glay.vwstrarray0[30];
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


                //company account number/customer/vendor serial number setting
                GB_001_COY = db.GB_001_COY.Find("COYAUTO");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYAUTO";
                    up1_flag = true;
                }

                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray0[35]) ? "" : glay.vwstrarray0[35];
                GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray4[0]) ? "" : glay.vwstrarray4[0];
                GB_001_COY.field3 = glay.vwitarray0[0].ToString();
                GB_001_COY.field4 = glay.vwblarray1[0] ? "Y" : "N";
                GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray4[1]) ? "" : glay.vwstrarray4[1];
                GB_001_COY.field6 = glay.vwitarray0[1].ToString();
                GB_001_COY.field7 = glay.vwblarray1[1] ? "Y" : "N";
                GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstrarray4[2]) ? "" : glay.vwstrarray4[2];
                GB_001_COY.field9 = glay.vwitarray0[2].ToString();
                GB_001_COY.field10 = glay.vwblarray1[2] ? "Y" : "N";
                GB_001_COY.field11 = string.IsNullOrWhiteSpace(glay.vwstrarray4[3]) ? "" : glay.vwstrarray4[3];
                GB_001_COY.field12 = glay.vwitarray0[3].ToString();
                GB_001_COY.field13= glay.vwblarray1[3] ? "Y" : "N";
                GB_001_COY.field14 = string.IsNullOrWhiteSpace(glay.vwstrarray4[4]) ? "" : glay.vwstrarray4[4];
                GB_001_COY.field15= glay.vwitarray0[4].ToString();
                GB_001_COY.field16 = glay.vwblarray1[4] ? "Y" : "N";
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

                GB_001_COY = db.GB_001_COY.Find("COYAUTO1");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYAUTO1";
                    up1_flag = true;
                }

                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray4[5]) ? "" : glay.vwstrarray4[5];
                GB_001_COY.field2 = glay.vwitarray0[5].ToString();
                GB_001_COY.field3 = glay.vwblarray1[5] ? "Y" : "N";
                GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray4[6]) ? "" : glay.vwstrarray4[6];
                GB_001_COY.field5 = glay.vwitarray0[6].ToString();
                GB_001_COY.field6 = glay.vwblarray1[6] ? "Y" : "N";
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray4[7]) ? "" : glay.vwstrarray4[7];
                GB_001_COY.field8 = glay.vwitarray0[7].ToString();
                GB_001_COY.field9 = glay.vwblarray1[7] ? "Y" : "N";
                GB_001_COY.field10 = string.IsNullOrWhiteSpace(glay.vwstrarray4[8]) ? "" : glay.vwstrarray4[8];
                GB_001_COY.field11 = glay.vwitarray0[8].ToString();
                GB_001_COY.field12 = glay.vwblarray1[8] ? "Y" : "N";
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

 //company mail
                GB_001_COY = db.GB_001_COY.Find("COYMAIL");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYMAIL";
                    up1_flag = true;
                }

                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray2[0]) ? "" : glay.vwstrarray2[0];
                GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray2[1]) ? "" : glay.vwstrarray2[1];
                GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstrarray2[2]) ? "" : glay.vwstrarray2[2];
                GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray2[3]) ? "" : glay.vwstrarray2[3];
                GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray2[4]) ? "" : glay.vwstrarray2[4];
                GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray2[5]) ? "" : glay.vwstrarray2[5];
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray2[6]) ? "" : glay.vwstrarray2[6];
                GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstrarray2[7]) ? "" : glay.vwstrarray2[7];
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

 //company password
                GB_001_COY = db.GB_001_COY.Find("COYPASS");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYPASS";
                    up1_flag = true;
                }

                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray2[8]) ? "" : glay.vwstrarray2[8];
                GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray2[9]) ? "" : glay.vwstrarray2[9];
                GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstrarray2[10]) ? "" : glay.vwstrarray2[10];
                GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray2[11]) ? "" : glay.vwstrarray2[11];
                GB_001_COY.field5 = glay.vwbool2 ? "Y" : "N";
                GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray2[12]) ? "" : glay.vwstrarray2[12];
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray2[13]) ? "" : glay.vwstrarray2[13];
                GB_001_COY.field8 = glay.vwblarray0[3] ? "Y" : "N";
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

 //company report
                GB_001_COY = db.GB_001_COY.Find("COYREP");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYREP";
                    up1_flag = true;
                }

                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray2[14]) ? "" : glay.vwstrarray2[14];
                GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray2[15]) ? "" : glay.vwstrarray2[15];
                GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstrarray2[16]) ? "" : glay.vwstrarray2[16];
                GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray2[17]) ? "" : glay.vwstrarray2[17];
                GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray2[18]) ? "" : glay.vwstrarray2[18];
                GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray2[19]) ? "" : glay.vwstrarray2[19];
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray2[20]) ? "" : glay.vwstrarray2[20];
                GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstrarray2[21]) ? "" : glay.vwstrarray2[21];
                GB_001_COY.field9 = string.IsNullOrWhiteSpace(glay.vwstrarray2[22]) ? "" : glay.vwstrarray2[22];
                GB_001_COY.field10 = string.IsNullOrWhiteSpace(glay.vwstrarray2[23]) ? "" : glay.vwstrarray2[23];
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
            //company mail
                GB_001_COY = db.GB_001_COY.Find("COYMAIL");
                up1_flag = false;
                if (GB_001_COY == null)
                {
                    GB_001_COY = new GB_001_COY();
                    init_coyrecord();
                    GB_001_COY.created_by = pubsess.userid;
                    GB_001_COY.created_date = DateTime.UtcNow;
                    GB_001_COY.id_code = "COYMAIL";
                    up1_flag = true;
                }

                GB_001_COY.field1 = string.IsNullOrWhiteSpace(glay.vwstrarray2[0]) ? "" : glay.vwstrarray2[0];
                GB_001_COY.field2 = string.IsNullOrWhiteSpace(glay.vwstrarray2[1]) ? "" : glay.vwstrarray2[1];
                GB_001_COY.field3 = string.IsNullOrWhiteSpace(glay.vwstrarray2[2]) ? "" : glay.vwstrarray2[2];
                GB_001_COY.field4 = string.IsNullOrWhiteSpace(glay.vwstrarray2[3]) ? "" : glay.vwstrarray2[3];
                GB_001_COY.field5 = string.IsNullOrWhiteSpace(glay.vwstrarray2[4]) ? "" : glay.vwstrarray2[4];
                GB_001_COY.field6 = string.IsNullOrWhiteSpace(glay.vwstrarray2[5]) ? "" : glay.vwstrarray2[5];
                GB_001_COY.field7 = string.IsNullOrWhiteSpace(glay.vwstrarray2[6]) ? "" : glay.vwstrarray2[6];
                GB_001_COY.field8 = string.IsNullOrWhiteSpace(glay.vwstrarray2[7]) ? "" : glay.vwstrarray2[7];
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



            if (err_flag)
            {
                util.write_document("COMPANY", glay.vwstring0, photo2, glay.vwstrarray9);
                //if(action_flag == "Create")
                //{
                //    GB_001_DOC gdoc = new GB_001_DOC();
                //    gdoc.screen_code = "COMPANY";
                //    gdoc.document_code = glay.vwstring0;
                //    gdoc.description = "";
                //    gdoc.created_by = pubsess.userid;
                //    gdoc.modified_by = pubsess.userid;
                //    gdoc.created_date = DateTime.UtcNow;
                //    gdoc.modified_date = DateTime.UtcNow;
                //    gdoc.active_status = "Y";
                //    gdoc.note = "";
                //    foreach (var bg in photo2)
                //    {
                //        if (bg != null)
                //        {
                //            byte[] uploaded = new byte[bg.InputStream.Length];
                //            bg.InputStream.Read(uploaded, 0, uploaded.Length);
                //            gdoc.document_image = uploaded;
                //            db.Entry(gdoc).State = EntityState.Added;
                //            db.SaveChanges();
                //        }
                //    }

                //    if (glay.vwstrarray9 != null)
                //    {
                //        for (int pctr = 0; pctr < glay.vwstrarray9.Length; pctr++)
                //        {
                //            if (!(string.IsNullOrWhiteSpace(glay.vwstrarray9[pctr])))
                //            {
                //                int seqno = Convert.ToInt16(glay.vwstrarray9[pctr]);
                //                GB_001_DOC gdoc1 = db.GB_001_DOC.Find(seqno);

                //                db.GB_001_DOC.Remove(gdoc1);
                //                db.SaveChanges();
                //            }
                //        }
                //    }

                //}

            }

        }

        private void validation_routine()
        {
           // string error_msg="";
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //    error_msg = "Company ID must not be spaces";

            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //{
            //    ModelState.AddModelError(String.Empty, "Please enter exchange rate ratio");
            //    err_flag = false;
            //}

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[3]))
            {
                ModelState.AddModelError(String.Empty, "Please enter Tax impact");
                err_flag = false;
            }

            if (glay.vwstring3 == "B")
            {
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[15]))
                {
                    ModelState.AddModelError(String.Empty, "Please enter exchange rate ratio");
                    err_flag = false;
                }
            }

            if (glay.vwstrarray0[22] == "Y")
            {
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[23]))
                {
                    ModelState.AddModelError(String.Empty, "Please select System action at credit check failure");
                    err_flag = false;
                }

                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[24]))
                {
                    ModelState.AddModelError(String.Empty, "Please select Specify credit monitor basis");
                    err_flag = false;
                }

            }


            if (glay.vwstring1 == null)
            {
                glay.vwstring1 = "";
            }
            if (glay.vwstrarray0[17] == "P")
            {
                if (glay.vwdecimal0 <= 0)
                {
                    ModelState.AddModelError(String.Empty, "Enter a valid number of period");
                    err_flag = false;
                }
                if (glay.vwstrarray1[1] != "")
                {
                    string current_period = glay.vwstrarray1[1];
                    string ddate = current_period.Substring(0, 2);

                    decimal current = Convert.ToDecimal(ddate);


                    if (current > glay.vwdecimal0)
                    {
                        ModelState.AddModelError(String.Empty, "Current Period has exceeded Number of Period");
                        err_flag = false;

                    }
                }
            }
            glay.vwstrarray3 = new string[20];
            auto_type();
            for (int ctr = 0; ctr < 8; ctr++)
            {

                int prefix_len = glay.vwstrarray4[ctr].Length;
                int auto_length = prefix_len + Convert.ToInt32(glay.vwstrarray0[35]);
                if (auto_length > 10)
                {
                    ModelState.AddModelError(String.Empty, glay.vwstrarray3[ctr] + " and Sequence Number has exceeded the maximum length of 10");
                    err_flag = false;
                }

            }
            
           
            var bgi = (from bg in db.GB_001_PCT
                       select bg).FirstOrDefault();
            if (bgi == null && glay.vwstrarray0[17] == "P")
            {
                prd_go = "Y";
                
            }
            else prd_go = "N";

            int logosize = glay.vwint0;
            int maxsize = logosize * 1024;
            if (photo1 != null)
            {
                if (photo1.ContentLength > maxsize)
                {
                    ModelState.AddModelError(String.Empty, "cannot accept picture more than " + logosize + " kb");
                    err_flag = false;
                }
            }

            //string sqlstr = "select '1' query0 from GB_001_COY where company_name=" + util.sqlquote(glay.vwstrarray0[1]) + " and company_code <> " + util.sqlquote(glay.vwstring0);
            //var bglist1 = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            //if (bglist1 != null)
            //    error_msg = "Can not accept duplicate Company Name";

        //    if (error_msg !="")
        //    {
        //        ModelState.AddModelError(String.Empty, error_msg);
        //        err_flag = false;
        //    }
        }

        private void read_record()
        {
            GB_001_COY rcoyinfo = db.GB_001_COY.Find("COYINFO");
            GB_001_COY rcoyset = db.GB_001_COY.Find("COYSET");
            GB_001_COY rcoyadd = db.GB_001_COY.Find("COYADD");
            GB_001_COY rcoyprice = db.GB_001_COY.Find("COYPRICE");
            GB_001_COY rcoyauto = db.GB_001_COY.Find("COYAUTO");
            GB_001_COY rcoyauto1 = db.GB_001_COY.Find("COYAUTO1");
            GB_001_COY rcoyprd = db.GB_001_COY.Find("COYPRD");
            GB_001_COY rcoymail = db.GB_001_COY.Find("COYMAIL");
            GB_001_COY rcoypass = db.GB_001_COY.Find("COYPASS");
            GB_001_COY rcoyrep = db.GB_001_COY.Find("COYREP");
            if (rcoyinfo != null)
            {
                glay.vwstring0 = rcoyinfo.field1;
                glay.vwstrarray0[0] = rcoyinfo.field2;
                glay.vwstrarray0[1] = rcoyinfo.field3;
                int k1 = 0;
                int.TryParse(rcoyinfo.field9, out k1);
                glay.vwint0 = k1;
                glay.vwstrarray0[2] = rcoyinfo.field4;
                glay.vwstrarray0[3] = rcoyinfo.field5;
                glay.vwstring9 = rcoyinfo.field6;
                glay.vwstrarray0[14] = rcoyinfo.field7;
            }

            if (rcoyadd != null)
            {
                glay.vwstrarray0[4] = rcoyadd.field10;
                glay.vwstrarray0[5] = rcoyadd.field1;
                glay.vwstrarray0[6] = rcoyadd.field2;
                glay.vwstrarray0[7] = rcoyadd.field3;
                glay.vwstrarray0[8] = rcoyadd.field4;
                glay.vwstrarray0[9] = rcoyadd.field5;
                glay.vwstrarray0[10] = rcoyadd.field6;
                glay.vwstrarray0[11] = rcoyadd.field7;
                glay.vwstrarray0[12] = rcoyadd.field8;
                glay.vwstrarray0[13] = rcoyadd.field9;

            }

            if (rcoyset != null)
            {
              
                glay.vwstring3 = rcoyset.field1;
                glay.vwstrarray0[16] = rcoyset.field2;
                glay.vwstrarray0[32] = rcoyset.field3;
                glay.vwstrarray0[33] = rcoyset.field4;
                glay.vwstrarray0[34] = rcoyset.field5;
                glay.vwstrarray0[15] = rcoyset.field6;
                glay.vwstrarray0[31] = rcoyset.field7;
                glay.vwbool0 = rcoyprice.field8 == "Y" ? true : false;
                glay.vwbool1 = rcoyprice.field9 == "Y" ? true : false;
                
            }

            if (rcoyprd != null)
            {
                glay.vwstrarray0[17] = rcoyprd.field1;
                if (glay.vwstrarray0[17] == "P")
                {
                    glay.vwdecimal0 = Convert.ToDecimal(rcoyprd.field3);
                    glay.vwstrarray1[0] = rcoyprd.field2;
                    glay.vwstrarray1[1] = rcoyprd.field4;
                    glay.vwstrarray1[2] = rcoyprd.field5;
                    glay.vwstrarray1[3] = rcoyprd.field6;
                    glay.vwstrarray1[4] = rcoyprd.field7;
                    glay.vwstring4 = rcoyprd.field8;
                }
                else
                {
                    glay.vwstrarray0[18] = rcoyprd.field2;
                    glay.vwstrarray0[19] = rcoyprd.field4;
                    glay.vwstrarray0[20] = rcoyprd.field5;
                    glay.vwstrarray0[21] = rcoyprd.field6;
                    glay.vwstring1 = rcoyprd.field7;
                    glay.vwstring2 = rcoyprd.field8;
                }
                glay.vwstrarray1[5] = rcoyprd.field9;
                glay.vwstrarray1[6] = rcoyprd.field10;
            }

            if(rcoyprice!=null)
            {
               // glay.vwblarray0[1] = rcoyprice.field5 == "Y" ? true : false;
                glay.vwstring5 = rcoyprice.field5;
                glay.vwblarray0[1] = rcoyprice.field12 == "Y" ? true : false;
                glay.vwblarray0[2] = rcoyprice.field13 == "Y" ? true : false;
                glay.vwblarray0[0] = rcoyprice.field4 == "Y" ? true : false;
                glay.vwstrarray0[22] = rcoyprice.field1;
                glay.vwstrarray0[23] = rcoyprice.field3;
                glay.vwstrarray0[24] = rcoyprice.field2;
                glay.vwstrarray0[25] = rcoyprice.field6;
                glay.vwstrarray0[26] = rcoyprice.field7;
                glay.vwstrarray0[27] = rcoyprice.field8;
                glay.vwstrarray0[28] = rcoyprice.field9;
                glay.vwstrarray0[29] = rcoyprice.field10;
                glay.vwstrarray0[30] = rcoyprice.field11;

            }

            if (rcoyauto != null)
            {
                glay.vwstrarray0[35] = rcoyauto.field1;
                glay.vwstrarray4[0] = rcoyauto.field2;
                glay.vwitarray0[0] = Convert.ToInt16(rcoyauto.field3);
                glay.vwblarray1[0] = rcoyauto.field4 == "Y" ? true : false;
                glay.vwstrarray4[1] = rcoyauto.field5;
                glay.vwitarray0[1] = Convert.ToInt16(rcoyauto.field6);
                glay.vwblarray1[1] = rcoyauto.field7 == "Y" ? true : false;
                glay.vwstrarray4[2] = rcoyauto.field8;
                glay.vwitarray0[2] = Convert.ToInt16(rcoyauto.field9);
                glay.vwblarray1[2] = rcoyauto.field10 == "Y" ? true : false;
                glay.vwstrarray4[3] = rcoyauto.field11;
                glay.vwitarray0[3] = Convert.ToInt16(rcoyauto.field12);
                glay.vwblarray1[3] = rcoyauto.field13 == "Y" ? true : false;
                glay.vwstrarray4[4] = rcoyauto.field14;
                glay.vwitarray0[4] = Convert.ToInt16(rcoyauto.field15);
                glay.vwblarray1[4] = rcoyauto.field16 == "Y" ? true : false;

            } 
            if (rcoyauto1 != null)
            {
                glay.vwstrarray4[5] = rcoyauto1.field1;
                glay.vwitarray0[5] = Convert.ToInt16(rcoyauto1.field2);
                glay.vwblarray1[5] = rcoyauto1.field3 == "Y" ? true : false;
                glay.vwstrarray4[6] = rcoyauto1.field4;
                glay.vwitarray0[6] = Convert.ToInt16(rcoyauto1.field5);
                glay.vwblarray1[6] = rcoyauto1.field6 == "Y" ? true : false;
                glay.vwstrarray4[7] = rcoyauto1.field7;
                glay.vwitarray0[7] = Convert.ToInt16(rcoyauto1.field8);
                glay.vwblarray1[7] = rcoyauto1.field9 == "Y" ? true : false;
                glay.vwstrarray4[8] = rcoyauto1.field10;
                glay.vwitarray0[8] = Convert.ToInt16(rcoyauto1.field11);
                glay.vwblarray1[8] = rcoyauto1.field12 == "Y" ? true : false;
               
            }
            if (rcoymail != null)
            {
                glay.vwstrarray2[0] = rcoymail.field1;
                glay.vwstrarray2[1] = rcoymail.field2;
                glay.vwstrarray2[2] = rcoymail.field3;
                glay.vwstrarray2[3] = rcoymail.field4;
                glay.vwstrarray2[4] = rcoymail.field5;
                glay.vwstrarray2[5] = rcoymail.field6;
                glay.vwstrarray2[6] = rcoymail.field7;
                glay.vwstrarray2[7] = rcoymail.field8;

            } 
            if (rcoypass != null)
            {
                glay.vwstrarray2[8] = rcoypass.field1;
                glay.vwstrarray2[9] = rcoypass.field2;
                glay.vwstrarray2[10] = rcoypass.field3;
                glay.vwstrarray2[11] = rcoypass.field4;
                glay.vwbool2 = rcoypass.field5 == "Y" ? true : false; ;
                glay.vwstrarray2[12] = rcoypass.field6;
                glay.vwstrarray2[13] = rcoypass.field7;
                glay.vwblarray0[3] = rcoypass.field8 == "Y" ? true : false; ;

            } 
            if (rcoyrep != null)
            {
                glay.vwstrarray2[14] = rcoyrep.field1;
                glay.vwstrarray2[15] = rcoyrep.field2;
                glay.vwstrarray2[16] = rcoyrep.field3;
                glay.vwstrarray2[17] = rcoyrep.field4;
                glay.vwstrarray2[18] = rcoyrep.field5;
                glay.vwstrarray2[19] = rcoyrep.field6;
                glay.vwstrarray2[20] = rcoyrep.field7;
                glay.vwstrarray2[21] = rcoyrep.field8;
                glay.vwstrarray2[22] = rcoyrep.field9;
                glay.vwstrarray2[23] = rcoyrep.field10;

            }

            psess.temp6 = "";
            if (rcoyinfo != null)
            {
                var bglist = from bg in db.GB_001_DOC
                             where bg.screen_code == "COMPANY" && bg.document_code == rcoyinfo.field1
                             orderby bg.document_sequence
                             select bg;

                ViewBag.anapict = bglist.ToList();

                string display_cur = (from bg in db.MC_001_CUREN
                                      where rcoyinfo.field7 == bg.currency_code
                                      select bg.currency_description
                                        ).FirstOrDefault();
                psess.temp6 = display_cur== null? "" : display_cur;
                Session["psess"] = psess;
            }
        
        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwdclarray0 = new decimal[10];
            glay.vwblarray0 = new bool[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray0[18] = "C";
            glay.vwstring3 = "I";
            glay.vwstrarray0[33] = "N";
            glay.vwstrarray0[22] = "N";
            glay.vwstrarray1 = new string[50];
            glay.vwstrarray2 = new string[50];
            glay.vwstrarray3 = new string[50];
            glay.vwitarray0 = new int[20];
            glay.vwstrarray0[35] = "N";
            glay.vwstring4 = "N";
            glay.vwstrarray5 = new string[20];
            glay.vwblarray1 = new bool[10];

        }

        private void select_query()
        {
            var bglist = from bg in db.GB_001_COY
                         select bg;

            ViewBag.parent_company_code = new SelectList(bglist.ToList(), "company_code", "company_name", glay.vwstrarray0[0]);

            string str1 = "select currency_code query0, c2 query1 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.base_currency = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstrarray0[14]);

            //var bgvar = from bg in db.MC_001_CUREN
            //            where bg.active_status == "N"
            //            orderby bg.currency_description
            //            select bg;
            //ViewBag.base_currency = new SelectList(bgvar.ToList(), "currency_code", "currency_description", glay.vwstrarray0[14]);

            var bgitem = from bg in db.GB_999_MSG
                         where bg.type_msg == "BC"
                         orderby bg.name1_msg
                         select bg;

            ViewBag.category = new SelectList(bgitem.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[2]);

            var bgitemi = from bg in db.GB_999_MSG
                         where bg.type_msg == "WN"
                         orderby bg.name1_msg
                         select bg;

            ViewBag.wkflw = new SelectList(bgitemi.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[32]);
            
            var bgiteme = from bg in db.GB_999_MSG
                         where bg.type_msg == "WE"
                         orderby bg.name1_msg
                         select bg;

            ViewBag.wkflwe = new SelectList(bgiteme.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[34]);
            ViewBag.country = util.para_selectquery("13", glay.vwstrarray0[9],"N");
            //var bglisti = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "13" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //             select bg;
            //ViewBag.country = new SelectList(bglisti.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[9]);

            ViewBag.countryi = util.para_selectquery("13", glay.vwstring9,"N");

            ViewBag.state = util.para_selectquery("14", glay.vwstrarray0[10],"N");
            //var bglista = from bg in db.GB_001_PCODE
            //              where bg.parameter_type == "14" && bg.active_status == "N"
            //              orderby bg.parameter_name
            //             select bg;
            //ViewBag.state = new SelectList(bglista.ToList(), "parameter_code", "parameter_name", glay.vwstrarray0[10]);

            var bglistb = from bg in db.GB_999_MSG
                          where bg.type_msg == "ERR"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.exchange = new SelectList(bglistb.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[15]);

            var bgitemu = from bg in db.GB_999_MSG
                          where bg.type_msg == "WS"
                          orderby bg.name1_msg
                         select bg;

            ViewBag.sell = new SelectList(bgitemu.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[16]);

            var bglistc = from bg in db.GB_999_MSG
                          where bg.type_msg == "FYM"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.end = new SelectList(bglistc.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[18]);

            var bglistd = from bg in db.GB_999_MSG
                          where bg.type_msg == "CP"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.policy = new SelectList(bglistd.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[23]);

            var bgliste = from bg in db.GB_999_MSG
                          where bg.type_msg == "CCB"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.control = new SelectList(bgliste.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[24]);


            var bglis = from bg in db.GB_999_MSG
                          where bg.type_msg == "PASS"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.pass = new SelectList(bglis.ToList(), "code_msg", "name1_msg", glay.vwstrarray2[12]);
            //string sqlstg = "select account_code query0, account_name query1 from GL_001_CHART where account_type_code = (select acct_type_code  from GL_001_ATYPE where acct_type_code = 'prt')";
            //var bg5 = db.Database.SqlQuery<querylay>(sqlstg);

            //ViewBag.gl_disposal_code = new SelectList(bg5.ToList(), "query0", "query1", glay.vwstrarray0[15]);

            auto_type();

        }

        private void auto_type()
        {
            glay.vwstrarray3[0] = "Customer";
            glay.vwstrarray3[1] = "Vendor";
            glay.vwstrarray3[2] = "Fixed Asset";
            glay.vwstrarray3[3] = "Item";
            glay.vwstrarray3[4] = "Chart of Account";
            glay.vwstrarray3[5] = "Employee";
            glay.vwstrarray3[6] = " Import Consignment";
            glay.vwstrarray3[7] = "Maintenance Work Order";
            glay.vwstrarray3[8] = "Property Contract";
        }

    [HttpPost]
    public ActionResult pricehead_list2(string id)
        {
            // write your query statement
            ModelState.Remove("vwstrarray0[14]");
            //int seq = Convert.ToInt16(id);
            int di = Convert.ToInt16(id);
            var hdet = from bg in db.GB_001_PCODE
                       join bk in db.MC_001_CUREN
                       on new { a1 = bg.gl_account_code } equals new { a1 = bk.currency_code}
                       where bg.parameter_type == "13" && bg.parameter_code == id
                       
                       
                       select new
                       {
                           c1 = bk.currency_code,
                           c2 = bk.currency_description
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


        [HttpPost]
    public ActionResult pricehead_list(string id)
    {
        // write your query statement
        var hdet = from bg in db.GB_001_PCODE
                   where bg.parameter_type == "14" && bg.gl_account_code == id
                   orderby bg.parameter_name
                   select new
                   {
                       c1 = bg.parameter_code,
                       c2 = bg.parameter_name
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

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_COY] where company_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult period_cal(string mthend)
        {
            decimal nummth = 12;
            decimal current_period = 0;
            string cnow = DateTime.Now.Month.ToString();
            decimal final = Convert.ToDecimal(mthend);
            decimal current_mth = Convert.ToDecimal(cnow);
            decimal current_prd = current_mth - final;
            if (current_prd < 0)
                current_period = current_prd + nummth;
            else current_period = current_prd;

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = current_period.ToString() });
            ary.Add(new SelectListItem { Value = "2", Text = nummth.ToString() });
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                               , JsonRequestBehavior.AllowGet);

            //}
            return RedirectToAction("Index");
        }

        public ActionResult show(string id)
        {
             var dir = "";
            GB_001_COY = db.GB_001_COY.Find(id);
            if (GB_001_COY != null)
            {
                byte[] imagedata = GB_001_COY.company_logo;
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

            if (bglist != null)
            {
                byte[] imagedata = bglist.document_image;
                return File(imagedata, "png");
            }
            return View();
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
	}
}
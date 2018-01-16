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

    public class InsureController : Controller
    {
        //
        // GET: /Employee/

        FA_001_INSUR FA_001_INSUR = new FA_001_INSUR();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;
       
        string ptype = "";
        bool err_flag = true;
        string move_auto = "N";
        string action_flag = "";
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.FA_001_INSUR

                         select new vw_genlay
                         {
                             vwstring0 = bh.insurance_policy_id,
                             vwstring1 = bh.description,
                             vwstring2 = bh.effective_date,
                             vwstring3 = bh.expiry_date,
                         };

            return View(bglist.ToList());
        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            select_query();
            //initial_rtn();
            //header_ana();
            //cal_auto();
            //psess.temp5 = move_auto;
            //if (move_auto == "Y")
            //    glay.vwstring0 = "AUTO";
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            //cal_auto();
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            //initial_rtn();
            //header_ana();
            select_query();
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
           // initial_rtn();
            FA_001_INSUR = db.FA_001_INSUR.Find(key1);
            if (FA_001_INSUR != null)
                read_record();
           // header_ana();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
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

            //initial_rtn();
            //header_ana();
            select_query();
            return View(glay);
        }
        private void delete_record()
        {
            FA_001_INSUR = db.FA_001_INSUR.Find(glay.vwstring0);
            if (FA_001_INSUR != null)
            {
                db.FA_001_INSUR.Remove(FA_001_INSUR);
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
                FA_001_INSUR = new FA_001_INSUR();
                FA_001_INSUR.created_by = pubsess.userid;
                FA_001_INSUR.created_date = DateTime.UtcNow;
                //if (move_auto == "Y")
                //    glay.vwstring0 = util.autogen_num("EMP");
            }
            else
            {
                FA_001_INSUR = db.FA_001_INSUR.Find(glay.vwstring0);
            }
            FA_001_INSUR.insurance_policy_id = glay.vwstring0;
            FA_001_INSUR.description = glay.vwstring1;
            FA_001_INSUR.underwriter = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            FA_001_INSUR.insurance_broker = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            FA_001_INSUR.policy_cover_num = glay.vwint0;
            FA_001_INSUR.effective_date = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : util.date_yyyymmdd(glay.vwstring4);
            FA_001_INSUR.expiry_date = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : util.date_yyyymmdd(glay.vwstring5);
            FA_001_INSUR.sum_insured = glay.vwdecimal0;
            FA_001_INSUR.premuim = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            FA_001_INSUR.premuim_rate = glay.vwdecimal1;
            FA_001_INSUR.renewal_reminder_date = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            FA_001_INSUR.insured_asset_comment = string.IsNullOrWhiteSpace(glay.vwstring8) ? "" : glay.vwstring8;
            FA_001_INSUR.modified_date = DateTime.UtcNow;
            FA_001_INSUR.modified_by = pubsess.userid;
           // FA_001_INSUR.active_status = glay.vwbool0 ? "Y" : "N";
           

               if(action_flag == "Create")
                    db.Entry(FA_001_INSUR).State = EntityState.Added;
                else
                    db.Entry(FA_001_INSUR).State = EntityState.Modified;

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
                    //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, FA_001_INSUR b where header_sequence in (analysis_code1";
                    //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                    //stri += " and employee_code =" + util.sqlquote(glay.vwstring0);
                    //db.Database.ExecuteSqlCommand(stri);
              
                    {
                        util.write_document("EMP", FA_001_INSUR.insurance_policy_id, photo1, glay.vwstrarray9);
                    }

                }

            }
        
        private void validation_routine()
        {
            string error_msg = "";
            if (string.IsNullOrWhiteSpace(glay.vwstring0) )
                error_msg = "Please enter ID";

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Name must not be spaces";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          //aheader7 = psess.sarrayt0;
          //  aheader5 = psess.sarrayt1;
            
           if(action_flag == "Create")
            {
                FA_001_INSUR bnk = db.FA_001_INSUR.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }
            string d1 = util.date_yyyymmdd(glay.vwstring4);
            string d2 = util.date_yyyymmdd(glay.vwstring5);
           

            if (Convert.ToInt32(d1) > Convert.ToInt32(d2))
            {
                ModelState.AddModelError(String.Empty, "Expiry date must be greater than Effective date");
                err_flag = false;
            }
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
            ViewBag.underwriter = util.para_selectquery("002", glay.vwstring2);
            //var pe = from pc in db.GB_001_PCODE
            //         where pc.parameter_type == "04" && pc.active_status == "N"
            //         orderby pc.parameter_name
            //         select pc;
            //ViewBag.jobrole = new SelectList(pe.ToList(), "parameter_code", "parameter_name", glay.vwstring2);

            ViewBag.insuranceb = util.para_selectquery("002", glay.vwstring3);
          
        }
        //private void initial_rtn()
        //{

        //    glay.vwstrarray0 = new string[50];
        //    glay.vwstrarray4 = new string[20];
        //    glay.vwstrarray5 = new string[20];
        //    glay.vwstrarray6 = new string[20];
        //    glay.vwstrarray0[10] = "Y";
        //}

        private void read_record()
        {
             glay.vwstrarray0 = new string[50];
            glay.vwstrarray6 = new string[20];

            glay.vwstring0 = FA_001_INSUR.insurance_policy_id;
            glay.vwstring1 = FA_001_INSUR.description;
            glay.vwstring2 = FA_001_INSUR.underwriter;
            glay.vwstring3 = FA_001_INSUR.insurance_broker;
            glay.vwint0 = FA_001_INSUR.policy_cover_num;
            glay.vwstring4 = util.date_slash(FA_001_INSUR.effective_date);
            glay.vwstring5 = util.date_slash(FA_001_INSUR.expiry_date);
            glay.vwdecimal0 = FA_001_INSUR.sum_insured;
            glay.vwstring6 = FA_001_INSUR.premuim;
            glay.vwdecimal1 = FA_001_INSUR.premuim_rate;
            glay.vwstring7 = FA_001_INSUR.renewal_reminder_date;
            glay.vwstring8 = FA_001_INSUR.insured_asset_comment;
            
            //if (FA_001_INSUR.active_status == "Y")
            //{
            //    glay.vwbool0 = true;
            //}
            
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "INS" && bg.document_code == FA_001_INSUR.insurance_policy_id
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
            string sqlstr = "delete from [dbo].[FA_001_INSUR] where insurance_policy_id =" + util.sqlquote(id);
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

        //private void header_ana()
        //{
        //    glay.vwstrarray4 = new string[20];
        //    glay.vwstrarray5 = new string[20];
        //    string[] aheader7 = new string[20];
        //    string[] aheader5 = new string[20];

        //    SelectList[] head_det = new SelectList[20];

        //   // Session["head_det"] = head_det;
        //    //Session["aheader7"] = aheader7;
        //    psess.sarrayt1 = aheader5;


        //    var bglist = from bg in db.GB_001_HEADER
        //                 where bg.header_type_code == "014" && bg.sequence_no != 99
        //                 select bg;

        //    foreach (var item in bglist.ToList())
        //    {
        //        int count2 = item.sequence_no;
        //        aheader7[count2] = item.mandatory_flag;
        //        glay.vwstrarray4[count2] = item.header_code;
        //        var bglist2 = (from bg in db.GB_001_HANAL
        //                       where bg.header_sequence == item.header_code
        //                       select bg).FirstOrDefault();

        //        if (bglist2 != null)
        //        {
        //            glay.vwstrarray5[count2] = bglist2.header_description;
        //            var bglist3 = from bg in db.GB_001_DANAL
        //                          where bg.header_sequence == item.header_code
        //                          select bg;
        //            head_det[count2] = new SelectList(bglist3.ToList(), "analysis_code", "analysis_description", glay.vwstrarray6[count2]);

        //        }

        //    }

        //   // Session["head_det"] = head_det;
        //    //Session["aheader7"] = aheader7;
        //    psess.sarrayt1 = glay.vwstrarray5;
        //}

        //private void cal_auto()
        //{
        //    var autoset = (from bg in db.GB_001_COY
        //                   where bg.id_code == "COYAUTO"
        //                   select bg.field16).FirstOrDefault();

        //    if (autoset == "Y")
        //        move_auto = "Y";

        //}
    
    }
}

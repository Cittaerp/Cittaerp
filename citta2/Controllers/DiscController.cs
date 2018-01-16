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
    public class DiscController : Controller
    {
        DC_001_DISC DC_001_DISC = new DC_001_DISC();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string key_val = "";
        bool err_flag = true;
        string action_flag = "";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];


            var bglist = from bh in db.DC_001_DISC
                         join bk4 in db.GB_999_MSG
                         on new { a1 = "DT", a2 = bh.stepped_discount_active } equals new { a1 = bk4.type_msg, a2 = bk4.code_msg}
                         into bk5
                         from bk6 in bk5.DefaultIfEmpty()
                         where bh.discount_count == 0
                         select new vw_genlay
                         {
                             vwstring0 = bh.discount_code,
                             vwstring1 = bh.discount_name,
                             vwstring5 = bk6.name1_msg,
                             vwstring2 = bh.discount_date_from,
                             vwstring3 = bh.discount_date_to,
                             vwstring4 = bh.active_status == "N" ? "Active" : "Inactive"
                         };
            
            return View(bglist.ToList());


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
        public ActionResult Create(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            select_query();
            populateintarry();
        
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
            DC_001_DISC = db.DC_001_DISC.Find(key1,0);
            if (DC_001_DISC != null)
                read_record();

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

            if (id_xhrt=="D")
            { 
                delete_record();
                return RedirectToAction("Index");
            }

            photo1 = photofile;
            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            populateintarry();
            return View(glay);
        }

        private void delete_record()
        {
            DC_001_DISC = db.DC_001_DISC.Find(glay.vwstring0);
            if (DC_001_DISC!=null)
            {
                db.DC_001_DISC.Remove(DC_001_DISC);
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
            string str1 = "delete from DC_001_DISC where discount_code=" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "insert into DC_001_DISC(discount_code,discount_count,discount_name,stepped_discount_active,stepped_criteria,discount_amount,";
            str1 += "discount_percent,qualified_quantity,promo_criteria,discount_date_from,discount_date_to,discount_gl_acc,gift_code,gift_qty,gl_gift,modified_by,created_by,note,active_status,time_bound,qualified_amount) values(";
            str1 += util.sqlquote(glay.vwstring0) + ",0,";
            str1 += util.sqlquote(glay.vwstring1) + ",";
            str1 += util.sqlquote(glay.vwstring5) + ",";
            if(glay.vwstring5 == "P")
                str1 += util.sqlquote(glay.vwstring10) + ",";
            else
                str1 += util.sqlquote(glay.vwstring2) + ",";

            if (glay.vwstring5 == "F")
            {
                str1 += glay.vwdecimal1.ToString() + ",";
                str1 += glay.vwdecimal0.ToString() + ",0,'',";
            }
            else if (glay.vwstring5 == "P")
            {
                if (glay.vwstring10 == "Q")
                {
                    str1 += glay.vwdecimal3.ToString() + ",";
                    str1 += glay.vwdecimal4.ToString() + ",";
                 }

                else if (glay.vwstring10 == "FG")
                {
                    str1 += glay.vwdecimal6.ToString() + ",";
                    str1 += glay.vwdecimal5.ToString() + ",";
                }
                else
                    str1 += "0,0,";
                
                str1 += glay.vwdecimal2.ToString() + ",";
                str1 += util.sqlquote(glay.vwstring8) + ",";
            
            }
            else
                str1 += "0,0,0,'',";

            if (glay.vwstring9 == "Y")
            {
                str1 += "'" + util.date_yyyymmdd(glay.vwstring6) + "',";
                str1 += "'" + util.date_yyyymmdd(glay.vwstring7) + "',";
            }
            else
            {
                str1 += "'','',";
            }
            str1 += util.sqlquote(glay.vwstring3) + ",'',0,'',";
            str1 += util.sqlquote(pubsess.userid) + ",";
            str1 += util.sqlquote(pubsess.userid) + ",";
            str1 += util.sqlquote(glay.vwstring4) + ",";
            str1 += glay.vwbool1 ? "'Y'" : "'N'" + ",";
            str1 += util.sqlquote(glay.vwstring9) + ",";
            str1 += glay.vwdecimal7.ToString();
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

            str1 = "insert into DC_001_DISC(discount_code,discount_count,discount_name,stepped_discount_active,stepped_criteria,discount_amount,";
            str1 += "discount_percent,lower_limit,upper_limit,promo_criteria,modified_by,created_by,active_status) values(";
            string str11="";
            for (int count1 = 0; count1 < 10; count1++)
            {
                if (glay.vwitarray1[count1] != 0)
                {
                    str11 = str1 + util.sqlquote(glay.vwstring0) + ",";
                    str11 += (count1 + 1).ToString() + ",";
                    str11 += util.sqlquote(glay.vwstring1) + ",";
                    str11 += util.sqlquote(glay.vwstring5) + ",";
                    str11 += util.sqlquote(glay.vwstring2) + ",";
                    str11 += glay.vwdclarray3[count1].ToString() + ",";
                    str11 += glay.vwdclarray2[count1].ToString() + ",";
                    if (count1 == 0)
                    {
                        str11 += glay.vwitarray0[count1].ToString() + ",";
                    }
                    else
                    {
                        str11 += (glay.vwitarray1[count1-1]+1).ToString() + ",";
                    }
                    str11 += glay.vwitarray1[count1].ToString() + ",";
                    str11 += util.sqlquote(glay.vwstring8) + ",";
                    str11 += util.sqlquote(pubsess.userid) + ",";
                    str11 += util.sqlquote(pubsess.userid) + ",";
                    str11 += glay.vwbool1 ? "'Y'" : "'N'";
                    str11 += ")";
                    try
                    {
                        db.Database.ExecuteSqlCommand(str11);
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


            str1 = "insert into DC_001_DISC(discount_code,discount_count,discount_name,stepped_discount_active,stepped_criteria,discount_amount,";
            str1 += "discount_percent,gift_code,gift_qty,gl_gift,promo_criteria,modified_by,created_by,active_status) values(";
            str11 = "";
            for (int count1 = 0; count1 < 5; count1++)
            {
                if (glay.vwstrarray0[count1] != "")
                {
                    str11 = str1 + util.sqlquote(glay.vwstring0) + ",";
                    str11 += (count1 + 1).ToString() + ",";
                    str11 += util.sqlquote(glay.vwstring1) + ",";
                    str11 += util.sqlquote(glay.vwstring5) + ",";
                    str11 += util.sqlquote(glay.vwstring10) + ",";
                    str11 += glay.vwdecimal3.ToString() + ",";
                    str11 += glay.vwdecimal4.ToString() + ",";
                    str11 += util.sqlquote(glay.vwstrarray0[count1]) + ",";
                    str11 += glay.vwdclarray0[count1].ToString() + ",";
                    str11 +=  "'',"; 
                    str11 += util.sqlquote(glay.vwstring8) + ",";
                    str11 += util.sqlquote(pubsess.userid) + ",";
                    str11 += util.sqlquote(pubsess.userid) + ",";
                    str11 += glay.vwbool1 ? "'Y'" : "'N'";
                    str11 += ")";
                    try
                    {
                        db.Database.ExecuteSqlCommand(str11);
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
                util.parameter_deleteflag("008", glay.vwstring0);
                //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, DC_001_DISC b where a.account_code = b.discount_gl_acc";
                //str += " and discount_code =" + util.sqlquote(glay.vwstring0);
                ////db.Database.ExecuteSqlCommand(str);
                //db.Database.ExecuteSqlCommand(str);


                //if (Session["action_flag"].ToString() == "Create")
                {
                    util.write_document("DISCOUNT", glay.vwstring0, photo1, glay.vwstrarray9);
                }

            }

        }

        private void validation_routine()
        {

            //string error_msg = "";
            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Discount ID must not be spaces");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Discount Name must not be spaces");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring3))
            {
                ModelState.AddModelError(String.Empty, "Please enter Gl account, or create a Gl account");
                err_flag = false;
            }
            if (glay.vwstring5 == "S" )
            {
                if (glay.vwitarray0[0] == 0 && glay.vwitarray1[0] == 0 && (glay.vwdclarray2[0] == 0 && glay.vwdclarray3[0]==0))
                {
                    ModelState.AddModelError(String.Empty, "Please specify lower range, upper range, percentage or amount");
                    err_flag = false;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstring2))
                {
                    ModelState.AddModelError(String.Empty, "Please specify Discount Computation Criteria");
                    err_flag = false;
                }
            }
            for (int ftr = 0; ftr < glay.vwdclarray2.Length; ftr++)
            {
                if (glay.vwdclarray3[ftr] != 0 && glay.vwdclarray2[ftr] != 0)
                {
                    ModelState.AddModelError(String.Empty, "Please specify Percentage or Amount only on Line" + (ftr+1).ToString());
                    err_flag = false;
                }
            }


                if (glay.vwstring5 == "F" && ((glay.vwdecimal0 == 0 && glay.vwdecimal1 == 0) || (glay.vwdecimal0 != 0 && glay.vwdecimal1 != 0)))
                {
                    ModelState.AddModelError(String.Empty, "Only Flat Amount or Flat Percentage must be filled");
                    err_flag = false;
                }
            if (glay.vwstring5 == "P" && glay.vwstring10=="Q" &&(!(glay.vwdecimal2 != 0 && glay.vwdecimal3 != 0 && glay.vwdecimal4 != 0)))
            {
                ModelState.AddModelError(String.Empty, "Qualified Quantity, Promotion Quantity and Promotion Percentage must be filled");
                err_flag = false;
            }
            if (glay.vwstring9 == "Y")
            {

                if (!util.date_validate(glay.vwstring6))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid start date");
                    err_flag = false;
                }

                if (!util.date_validate(glay.vwstring7))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid end date");
                    err_flag = false;
                }
            }

        }

        private void read_record()
        {
            glay.vwdclarray0 = new decimal[50];
            glay.vwbool0 = true;
            glay.vwstring0 = DC_001_DISC.discount_code;
            glay.vwstring1 = DC_001_DISC.discount_name;
            glay.vwdecimal0 = DC_001_DISC.discount_percent;
            glay.vwdecimal1 = DC_001_DISC.discount_amount;
            glay.vwdecimal5 = DC_001_DISC.discount_percent;
            glay.vwdecimal6 = DC_001_DISC.discount_amount;
            glay.vwdecimal2 = DC_001_DISC.qualified_quantity;
            glay.vwdecimal3 = DC_001_DISC.discount_amount;
            glay.vwdecimal4 = DC_001_DISC.discount_percent;
            glay.vwstring2 = DC_001_DISC.stepped_criteria;
            glay.vwstring10 = DC_001_DISC.stepped_criteria;
            glay.vwstring6 = util.date_slash(DC_001_DISC.discount_date_from);
            glay.vwstring7 = util.date_slash(DC_001_DISC.discount_date_to);
            glay.vwstring3 = DC_001_DISC.discount_gl_acc;
            glay.vwstring4 = DC_001_DISC.note;
            glay.vwstring5 = DC_001_DISC.stepped_discount_active;
            glay.vwstring8 = DC_001_DISC.promo_criteria;
            glay.vwstring9 = DC_001_DISC.time_bound;



            if (DC_001_DISC.active_status == "Y")
                glay.vwbool1 = true;

            var bhlist = from bg in db.DC_001_DISC
                         where bg.discount_code == key_val && bg.discount_count != 0
                         select bg;

            foreach (var item in bhlist.ToList())
            {
                if (item.stepped_discount_active == "S")
                {
                    glay.vwitarray0[item.discount_count - 1] = item.lower_limit;
                    glay.vwitarray1[item.discount_count - 1] = item.upper_limit;
                    glay.vwdclarray2[item.discount_count - 1] = item.discount_percent;
                    glay.vwdclarray3[item.discount_count - 1] = item.discount_amount;
                }
                else
                {
                    glay.vwstrarray0[item.discount_count - 1] = item.gift_code;
                    glay.vwdclarray0[item.discount_count - 1] = item.gift_qty;

                   
                }
            }

            int countr = 0;
            
            for (int i = 0; i < glay.vwstrarray0.Length; i++)
            {
                if(!(string.IsNullOrWhiteSpace(glay.vwstrarray0[i]))){
                     countr++;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[i]))
                    break;
               
            }

            glay.vwint2 = countr;
            
            var bglist = from bg in db.GB_001_DOC
                         where bg.screen_code == "DISCOUNT" && bg.document_code == DC_001_DISC.discount_code
                         orderby bg.document_sequence
                         select bg;

            ViewBag.anapict = bglist.ToList();
           
        }

        private void initial_rtn()
        {
            glay.vwdclarray0 = new decimal[50];
            glay.vwdclarray1 = new decimal[50];
            glay.vwdclarray2 = new decimal[50];
            glay.vwdclarray3 = new decimal[50];
            glay.vwdclarray4 = new decimal[50];
            glay.vwitarray0 = new int[20];
            glay.vwitarray1 = new int[20];
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray1 = new string[50];
            glay.vwint2 = 1;
            glay.vwint3 = 1;
            glay.vwstring5 = "F";
            glay.vwstring9 = "N";
            glay.vwstring10 = "Q";
        }
        private void select_query()
        {
            var bgiteme = from bg in db.GB_999_MSG
                          where bg.type_msg == "SC"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.stepped = new SelectList(bgiteme.ToList(), "code_msg", "name1_msg", glay.vwstring2);

            var bgitem = from bg in db.GB_999_MSG
                          where bg.type_msg == "PMO"
                          orderby bg.name1_msg
                          select bg;

            ViewBag.promo = new SelectList(bgitem.ToList(), "code_msg", "name1_msg", glay.vwstring8);

            ViewBag.gl_code = util.read_ledger("011", glay.vwstring3);

            var bgitemt = from bg in db.IV_001_ITEM
                                where bg.active_status == "N" && bg.marketing == "Y"
                                orderby bg.item_code, bg.item_name
                                select new { c1 = bg.item_code, c2 = bg.item_code + "--- " + bg.item_name };
            ViewBag.promoitem = new SelectList(bgitemt.ToList(), "c1", "c2");

              var bg2 = util.read_ledger("011");
              ViewBag.glgift = util.read_ledger("020");
            //ViewBag.gl_code = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring3);

        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult range_assign(string rangeto1, string rangeto2, string rangeto3, string rangeto4, string rangeto5, string rangeto6, string rangeto7, string rangeto8, string rangeto9)
        { 
            decimal rangefrm1 = 0; decimal rangefrm2 = 0; decimal rangefrm3 = 0; decimal rangefrm4 = 0; decimal rangefrm5 = 0;
            decimal rangefrm6 = 0; decimal rangefrm7 = 0; decimal rangefrm8 = 0; decimal rangefrm9 = 0; decimal rng1 = 0; decimal rng2 = 0;
            decimal rng3 = 0; decimal rng4 = 0; decimal rng5 = 0; decimal rng6 = 0; decimal rng7 = 0; decimal rng8 = 0; decimal rng9 = 0;
            decimal.TryParse(rangeto1, out rng1);
            decimal.TryParse(rangeto2, out rng2);
            decimal.TryParse(rangeto3, out rng3);
            decimal.TryParse(rangeto4, out rng4);
            decimal.TryParse(rangeto5, out rng5);
            decimal.TryParse(rangeto6, out rng6);
            decimal.TryParse(rangeto7, out rng7);
            decimal.TryParse(rangeto8, out rng8);
            decimal.TryParse(rangeto9, out rng9);
            rangefrm1 = rng1 + 1;
            rangefrm2 = rng2 + 1;
            rangefrm3 = rng3 + 1;
            rangefrm4 = rng4 + 1;
            rangefrm5 = rng5 + 1;
            rangefrm6 = rng6 + 1;
            rangefrm7 = rng7 + 1;
            rangefrm8 = rng8 + 1;
            rangefrm9 = rng9 + 1;
            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = rangefrm1.ToString() });
            ary.Add(new SelectListItem { Value = "2", Text = rangefrm2.ToString() });
            ary.Add(new SelectListItem { Value = "3", Text = rangefrm3.ToString() });
            ary.Add(new SelectListItem { Value = "4", Text = rangefrm4.ToString() });
            ary.Add(new SelectListItem { Value = "5", Text = rangefrm5.ToString() });
            ary.Add(new SelectListItem { Value = "6", Text = rangefrm6.ToString() });
            ary.Add(new SelectListItem { Value = "7", Text = rangefrm7.ToString() });
            ary.Add(new SelectListItem { Value = "8", Text = rangefrm8.ToString() });
            ary.Add(new SelectListItem { Value = "9", Text = rangefrm9.ToString() });

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");


        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[DC_001_DISC] where discount_code=" + util.sqlquote(id);
              db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }

        private void populateintarry()
        {
            int value1 = glay.vwitarray0[0];
            glay.vwitarray0 = new int[20];
            glay.vwitarray0[0] = value1;
            for (int ftr = 0; ftr < glay.vwitarray1.Length; ftr++)
            {
                if (glay.vwitarray1[ftr] != 0)
                glay.vwitarray0[ftr+1] = glay.vwitarray1[ftr] + 1;

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
 
	}
}
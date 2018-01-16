using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using anchor1.Filters;
using System.Data.Entity;
using CittaErp.utilities;

namespace CittaErp.Controllers
{
    public class Discount_application_selectionController : Controller {
        //
        // GET: /discount_application_selection/

        DC_001_DISTS DC_001_DISTS = new DC_001_DISTS();
       
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();

        string dtype = "";
        bool err_flag = true;
        string action_flag = "";
        [EncryptionActionAttribute]
        public ActionResult Index () {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.DC_001_DISTS
                         join bk1 in db.DC_001_DISC
                         on new { a1 = bh.discount_code } equals new { a1 = bk1.discount_code }
                         join bk4 in db.IV_001_ITEM
                         on new { a1 = bh.selection_code } equals new { a1 = bk4.item_code }
                         join bk15 in db.GB_999_MSG
                         on new { a1 = "disc", a2 = bh.discount_selection_basis } equals new { a1 = bk15.type_msg, a2 = bk15.code_msg }

                         select new vw_genlay
                        {

                            vwstring0 = bk15.name1_msg,
                            vwstring1 = bk4.item_name,
                            vwstring2 = bk1.discount_name,
                            vwstring4 = bh.discount_selection_basis,
                            vwstring5 = bh.selection_code,
                            vwstring6 = bh.discount_code,

                        };

            var bglist1 = from bh in db.DC_001_DISTS

                          join bk1 in db.DC_001_DISC
                          on new { a1 = bh.discount_code } equals new { a1 = bk1.discount_code }

                          join bk7 in db.GB_001_COY
                          on new { a1 = "0" } equals new { a1 = "0" }

                          join bk15 in db.GB_999_MSG
                         on new { a1 = "disc", a2 = bh.discount_selection_basis } equals new { a1 = bk15.type_msg, a2 = bk15.code_msg }
                          where bh.selection_code == "1" || bh.selection_code == "2" || bh.selection_code == "3" || bh.selection_code == "4" || bh.selection_code == "5" || bh.selection_code == "6"

                          select new vw_genlay
                     {
                         vwstring0 = bk15.name1_msg,
                         vwstring1 = "1" ,//bh.selection_code == "1" ? bk7.price_class1 : bh.selection_code == "2" ? bk7.price_class2 : bh.selection_code == "3" ? bk7.price_class3 : bh.selection_code == "4" ? bk7.price_class4 : bh.selection_code == "5" ? bk7.price_class5 : bh.selection_code == "6" ? bk7.price_class6 : "",
                         vwstring2 = bk1.discount_name
                     };

            var bglist2 = from bh in db.DC_001_DISTS
                          join bk1 in db.DC_001_DISC
                          on new { a1 = bh.discount_code } equals new { a1 = bk1.discount_code }

                          join bk10 in db.AR_001_CUSTM
                           on new { a1 = bh.selection_code } equals new { a1 = bk10.cust_biz_name }


                          join bk15 in db.GB_999_MSG
                         on new { a1 = "disc", a2 = bh.discount_selection_basis } equals new { a1 = bk15.type_msg, a2 = bk15.code_msg }
                          select new vw_genlay
                          {
                              vwstring0 = bk15.name1_msg,
                              vwstring1 = bk10.cust_biz_name,
                              vwstring2 = bk1.discount_name,
                          };

            var bglist3 = from bh in db.DC_001_DISTS
                          join bk1 in db.DC_001_DISC
                          on new { a1 = bh.discount_code } equals new { a1 = bk1.discount_code }

                          join bk13 in db.GB_001_HANAL
                           on new { a1 = bh.discount_code } equals new { a1 = bk13.header_description }


                          join bk17 in db.GB_999_MSG
                         on new { a1 = "disc", a2 = bh.discount_selection_basis } equals new { a1 = bk17.type_msg, a2 = bk17.code_msg }

                          select new vw_genlay
                          {
                              vwstring0 = bk17.name1_msg,
                              vwstring1 = bk13.header_description,
                              vwstring2 = bk1.discount_name,
                          };


            var bhlist = bglist.ToList().Concat(bglist1.ToList().Concat(bglist2.ToList().Concat(bglist3.ToList())));
            return View(bhlist);
        
        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
           
            glay.vwstring0 = "disc1";
          
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in)
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
        public ActionResult Edit(string key1,string key2, string key3)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            DC_001_DISTS = db.DC_001_DISTS.Find(key1, key2, key3);
            if (DC_001_DISTS != null)
                read_record();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [EncryptionActionAttribute]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
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

            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }
        private void delete_record()
        {
            DC_001_DISTS = db.DC_001_DISTS.Find(glay.vwstring0, glay.vwstring2, glay.vwstring3);
            if (DC_001_DISTS != null)
            {
                db.DC_001_DISTS.Remove(DC_001_DISTS);
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
                DC_001_DISTS = new DC_001_DISTS();
                DC_001_DISTS.created_by = pubsess.userid;
                DC_001_DISTS.created_date = DateTime.UtcNow;
            }
            else
            {
                DC_001_DISTS = db.DC_001_DISTS.Find(glay.vwstring0, glay.vwstring2,glay.vwstring3);
            }
            DC_001_DISTS.discount_selection_basis = glay.vwstring0;
            DC_001_DISTS.selection_code = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            DC_001_DISTS.discount_code = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
           
            //DC_001_DISTS.discount_flag = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            if (glay.vwstring2 == "all")
            {
                DC_001_DISTS.discount_flag = "Y";
            }
            else
            {
                DC_001_DISTS.discount_flag = "N";
            }

            
            DC_001_DISTS.modified_date = DateTime.UtcNow;
            DC_001_DISTS.modified_by = pubsess.userid;
            DC_001_DISTS.active_status = glay.vwbool0 ? "Y" : "N";

           if(action_flag == "Create")
                db.Entry(DC_001_DISTS).State = EntityState.Added;
            else
                db.Entry(DC_001_DISTS).State = EntityState.Modified;

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
            //string error_msg = "";
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //    error_msg = "Please enter Id";

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Please enter Name;

           if(action_flag == "Create")
            {
                DC_001_DISTS bnk = db.DC_001_DISTS.Find(glay.vwstring0, glay.vwstring2, glay.vwstring3);

                if (bnk != null)
                {
                ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                err_flag = false;
                }
            }

        }
        
        private void select_query()
        {

            //string[] say = ViewBag.selection.ToArray();
            //for (int i = 0; i <= say.Length; i++)
            //{
            //    string TI = say[i];
            //    string CO = say[i];
            //    string CU = say[i];
            //    string HA = say[i];
            //}


                var tra = from tr in db.GB_999_MSG
                      where tr.type_msg == "DISC"
                      orderby tr.name1_msg
                      select tr;
            ViewBag.tran = new SelectList(tra.ToList(), "code_msg", "name1_msg", glay.vwstring0);

            var analysis = from hf in db.DC_001_DISC
                           where hf.active_status == "N"
                           orderby hf.discount_name
                           select hf;
            ViewBag.tanalysis = new SelectList(analysis.Distinct().ToList(), "discount_code", "discount_name", glay.vwstring3);

            if (glay.vwstring0 == "disc1")
            {
               List<SelectListItem> optionList = db.IV_001_ITEM
                                   .Select(x => new SelectListItem
                                   {
                                       Value = x.item_code,
                                       Text = x.item_name
                                   }).ToList();
               // Now add the item you want

               optionList.Add(new SelectListItem { Value = "01", Text = "All" });
               ViewBag.selection = new SelectList(optionList.ToList(), "Value", "Text", glay.vwstring2);
               //ViewBag.selection = new SelectList(load.ToList(), "item_code", "item_name", glay.vwstring2);
                //var load = from bh in db.IV_001_ITEM
                //           from bg in db.GB_999_MSG
                //           where bg.type_msg == "ALL"
                //           select new { bh, bg };
                //ViewBag.selection = load.ToList();
            
            }
            else if (glay.vwstring0 == "disc2")
            {
                var hdet = (from bg in db.GB_001_COY
                            where bg.id_code == "COYPRICE"
                            select bg).FirstOrDefault();

                string pcl1 = hdet.field6;
                string pcl2 = hdet.field7;
                string pcl3 = hdet.field8;
                string pcl4 = hdet.field9;
                string pcl5 = hdet.field10;
                string pcl6 = hdet.field11;

                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "1", Text = pcl1 });
                ary.Add(new SelectListItem { Value = "2", Text = pcl2 });
                ary.Add(new SelectListItem { Value = "3", Text = pcl3 });
                ary.Add(new SelectListItem { Value = "4", Text = pcl4 });
                ary.Add(new SelectListItem { Value = "5", Text = pcl5 });
                ary.Add(new SelectListItem { Value = "6", Text = pcl6 });
            ViewBag.selection = new SelectList(ary.ToArray(), "Value", "Text", glay.vwstring2);
                }
                else
                if (glay.vwstring0 == "disc3")
                    {
                        ViewBag.selection = util.para_selectquery("001", glay.vwstring2);
            //            var load = from bg in db.AR_001_CUSTM
            //                       where bg.active_status == "N"
            //                       orderby bg.cust_biz_name
            //                       select bg;
            //ViewBag.selection = new SelectList(load.ToList(), "customer_code", "cust_biz_name", glay.vwstring2);
                    } else

                    if (glay.vwstring0 == "disc4")
                    {
                        ViewBag.selection = util.para_selectquery("50", glay.vwstring2);
            //var load = from bg in db.GB_001_HANAL
            //           where bg.active_status == "N"
            //           orderby bg.header_description
            //           select bg;
            //ViewBag.selection = new SelectList(load.ToList(), "header_sequence", "header_description", glay.vwstring2);
                        }
        
       
        }
        
        private void pmtrix()
        {
            var coyin = (from bg in db.GB_001_COY
                         where bg.id_code == "COYPRICE"
                         select bg).FirstOrDefault();

            glay.vwstrarray2 = new string[20];
            glay.vwstrarray2[0] = coyin.field6;
            glay.vwstrarray2[1] = coyin.field7;
            glay.vwstrarray2[2] = coyin.field8;
            glay.vwstrarray2[3] = coyin.field9;
            glay.vwstrarray2[4] = coyin.field10;
            glay.vwstrarray2[5] = coyin.field11;
        }  
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
        }
        private void read_record()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstring0 = DC_001_DISTS.discount_selection_basis;
            glay.vwstring2 = DC_001_DISTS.selection_code;
            glay.vwstring3 = DC_001_DISTS.discount_code;
            //glay.vwstring3 = DC_001_DISTS.discount_flag;
           
            if (DC_001_DISTS.active_status == "Y")
            {
                glay.vwbool0 = true;
            }
            

        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[DC_001_DISTS] where discount_selection_basis +'[]'+ selection_code +'[]'+ discount_code ="+ util.sqlquote(id);
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult pricehead_list(string id)
        {
            
            if (id == "disc1")
            {
                var hdet = from bg in db.IV_001_ITEM
                           orderby bg.item_name
                           select new
                           {
                               c1 = bg.item_code,
                               c2 = bg.item_name
                           };
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    hdet.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
            }
            else if (id=="disc2")
            {
                var hdet = (from bg in db.GB_001_COY
                            where bg.id_code == "COYPRICE"
                            select bg).FirstOrDefault();

                string pcl1 = hdet.field6;
                string pcl2 = hdet.field7;
                string pcl3 = hdet.field8;
                string pcl4 = hdet.field9;
                string pcl5 = hdet.field10;
                string pcl6 = hdet.field11;


                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "1", Text = pcl1 });
                ary.Add(new SelectListItem { Value = "2", Text = pcl2 });
                ary.Add(new SelectListItem { Value = "3", Text = pcl3 });
                ary.Add(new SelectListItem { Value = "4", Text = pcl4 });
                ary.Add(new SelectListItem { Value = "5", Text = pcl5 });
                ary.Add(new SelectListItem { Value = "6", Text = pcl6 });

                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary,
                                    "Value",
                                    "Text")
                               , JsonRequestBehavior.AllowGet);
            }
            else if (id=="disc4")
            {
                var hdet = from bg in db.GB_001_HANAL
                           orderby bg.header_description
                           select new
                           {
                               c1 = bg.header_sequence,
                               c2 = bg.header_description
                           };

                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    hdet.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);

            }
            else 
            {
                var hdet = from bg in db.AR_001_CUSTM
                           orderby bg.cust_biz_name
                           select new
                           {
                               c1 = bg.customer_code,
                               c2 = bg.cust_biz_name
                           };
           
            
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                hdet.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
    }
}
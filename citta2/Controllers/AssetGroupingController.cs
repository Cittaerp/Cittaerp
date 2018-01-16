using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using anchor1.Filters;
using CittaErp.utilities;
using System.Data.Entity;

namespace CittaErp.Controllers
{
    public class AssetGroupingController : Controller
    {
        AG_001_ASG AG_001_ASG = new AG_001_ASG();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        bool err_flag = true;
        string location, asset_user = "";
        string[] container = new string[] { };
        string action_flag = "";

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            var bglist = from bh in db.AG_001_ASG
                         select new vw_genlay
                         {
                             vwstring0 = bh.asset_grouping_id,
                             vwstring1 = bh.description,
                             vwstring2 = bh.inactive_status == "N" ? "Active" : "Inactive"
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
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, string[] sname, string[] sname2)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            glay.vwstrarray1 = sname;
            glay.vwstrarray2 = sname2;
            update_file();
            if (err_flag)
                return RedirectToAction("Create");
            select_query();
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            psess = (psess)Session["psess"];
            initial_rtn();
            pubsess = (pubsess)Session["pubsess"];
            AG_001_ASG = db.AG_001_ASG.Find(key1);
            if (AG_001_ASG != null)
                read_record();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, string[] sname, string[] sname2)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            glay = glay_in;
            glay.vwstrarray1 = sname;
            glay.vwstrarray2 = sname2;
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

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string str = " update FA_001_ASSET set group_flag = '' where group_flag =" + util.sqlquote(id);
            var str2 = db.Database.ExecuteSqlCommand(str);
            string sqlstr = "delete from [dbo].[AG_001_ASG] where asset_grouping_id='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            AG_001_ASG = db.AG_001_ASG.Find(glay.vwstring0);
            if (AG_001_ASG != null)
            {
                string str = " update FA_001_ASSET set group_flag = '' where group_flag =" + util.sqlquote(glay.vwstring0);
                var str2 = db.Database.ExecuteSqlCommand(str);
                db.AG_001_ASG.Remove(AG_001_ASG);
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

        private void select_query()
        {
           
                var bglist = from bg in db.FA_001_ASSET
                             where bg.active_status == "N" && bg.group_flag == null || bg.group_flag == ""
                             orderby bg.reference_asset_code
                             select new { c1 = bg.fixed_asset_code, c2 = bg.description };

                ViewBag.description = new SelectList(bglist.ToList(), "c1", "c2");

            var bggroup = from bg in db.AG_001_AMG
                          where bg.inactive_status == "N" && bg.nature == "nat1"
                          select bg;
            ViewBag.assetgroup = new SelectList(bggroup.ToList(), "maintenance_group_type_id", "description", glay.vwstring2);
            var query = container.Select((r) => new { Text = r, Value = r });
            ViewBag.selected_staff = new SelectList(query.ToList(), "Value", "Text");
             if(Session["action_flag"].ToString() == "Create")
                ViewBag.deed = new SelectList("", "");
            else
            {
                var bgfix = (from bh in db.FA_001_ASSET
                             where bh.group_flag == glay.vwstring0
                             select bh);
                ViewBag.deed = new SelectList(bgfix.ToList(), "fixed_asset_code", "description");
            }
        }
        private void read_record()
        {
            Session["location"] = location;
            Session["asset_user"] = asset_user;
            List<string> me = new List<string>();
            
            //List<string> count = new List<string>();
            // for(int i = 0; i<container.Length; i++)
            // {
            //     string nmm = container[i];
            
              
            //}
           

            glay.vwbool0 = false;
            glay.vwstring0 = AG_001_ASG.asset_grouping_id;
            glay.vwstring1 = AG_001_ASG.description;
            glay.vwstring2 = AG_001_ASG.maintenance_type_id;
            var bgtype = (from bg in db.AG_001_AMG
                          join bh in db.GB_999_MSG
                          on new { a1 = bg.nature } equals new { a1 = bh.code_msg }
                          into bh1
                          from bh2 in bh1.DefaultIfEmpty()
                          where bh2.type_msg == "nat"
                          select new { bh2 }).FirstOrDefault();
            if(bgtype!=null)
            ViewData["natt"] = bgtype.bh2.name1_msg;
            glay.vwstring6 = AG_001_ASG.created_by;
          
            glay.vwstrarray0 = me.ToArray();
            glay.vwstring4 = AG_001_ASG.created_date.ToString("dd/mm/yyyy");
            glay.vwstring5 = AG_001_ASG.modified_by;
            glay.vwstring3 = AG_001_ASG.note;
            glay.vwint2 = AG_001_ASG.cumulative_amount;
            glay.vwstring6 = AG_001_ASG.asset_requires_maintenace;
            glay.vwstring7 = AG_001_ASG.inactive_status;
            glay.vwstring8 = AG_001_ASG.modified_date.ToString("dd/mm/yyyy");
            if (AG_001_ASG.inactive_status == "Y")
                glay.vwbool0 = true;


        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
            glay.vwstrarray2 = new string[20];
            glay.vwdtarray1 = new DateTime[10];
            glay.vwdclarray2 = new decimal[10];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            //glay.vwstring2 = "N";
            //glay.vwbool0 = true;
        }

        [HttpPost]
        public ActionResult asset(string id)
        {
            string loc = "";
            string ast_user = "";
            string descr = "";
            var bgassign = (from bg in db.FA_001_ASSET
                            where bg.fixed_asset_code == id
                            select new { bg }).FirstOrDefault();

            var bgdesc = (from bg in db.AG_001_AMG
                          join bh in db.GB_999_MSG
                          on new { a1 = bg.nature } equals new { a1 = bh.code_msg }
                          into bh1
                          from bh2 in bh1.DefaultIfEmpty()
                          where bh2.type_msg == "nat"
                          where bg.maintenance_group_type_id == id
                          select new { bg, bh2 }).FirstOrDefault();
            if (bgassign != null)
            {
                loc = bgassign.bg.asset_location;
                ast_user = bgassign.bg.asset_user;
            }
            if (bgdesc != null)
            {
                descr = bgdesc.bh2.name1_msg;
            }

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "location", Text = loc });
            ary.Add(new SelectListItem { Value = "asset_user", Text = ast_user });
            ary.Add(new SelectListItem { Value = "desc", Text = descr });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

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
                ModelState.AddModelError(String.Empty, "Please Specify Asset Group ID");
                err_flag = false;
            }



            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please Specify Asset Group Description");
                err_flag = false;
            }



           if(action_flag == "Create")
            {
                AG_001_ASG asst = db.AG_001_ASG.Find(glay.vwstring0);
                if (asst != null)
                {
                    ModelState.AddModelError(String.Empty, "Asset Group ID Exists");
                    err_flag = false;
                }
                var bgassign = (from bg in db.AG_001_ASG
                                select bg.description).ToList();
                        ModelState.AddModelError(String.Empty, "Asset Group Description Exists");
                foreach (var item in bgassign)
                {
                    if (glay.vwstring1 == item)
                    {
                        err_flag = false;
                        break;
                    }
                }


            }
            if (glay.vwstrarray1 == null)
            {
                ModelState.AddModelError(String.Empty, "Please Select an Asset");
                err_flag = false;
            }
            if (glay.vwstrarray1 != null)
            {
                if (glay.vwstrarray1.Length < 2)
                {
                    ModelState.AddModelError(String.Empty, "Please Select More Assets");
                    err_flag = false;
                }
            }


        }
        private void update_record()
        {
           if(action_flag == "Create")
            {
                AG_001_ASG = new AG_001_ASG();
                AG_001_ASG.created_by = pubsess.userid;
                AG_001_ASG.created_date = DateTime.UtcNow;

            }
            else
            {
                AG_001_ASG = db.AG_001_ASG.Find(glay.vwstring0);
            }
            var bgfix = (from bg in db.FA_001_ASSET
                         where bg.group_flag == glay.vwstring0
                         select bg.fixed_asset_code).ToList();
            foreach(var item in bgfix)
            {

                string str = " update FA_001_ASSET set group_flag = '' where fixed_asset_code =" + util.sqlquote(item);
                var str2 = db.Database.ExecuteSqlCommand(str);
            }
             
                for (int i = 0; i < glay.vwstrarray1.Length; i++)
                {
                    string str = " update FA_001_ASSET set group_flag = " + util.sqlquote(glay.vwstring0) + " where fixed_asset_code =" + util.sqlquote(glay.vwstrarray1[i]);
                    var str2 = db.Database.ExecuteSqlCommand(str);
                }
            AG_001_ASG.cumulative_amount = glay.vwint2;
            AG_001_ASG.last_maintenance_date = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            AG_001_ASG.maintenance_type_id = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            AG_001_ASG.asset_requires_maintenace = "Y";
            AG_001_ASG.asset_grouping_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            AG_001_ASG.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            AG_001_ASG.inactive_status = glay.vwbool0 ? "Y" : "N";
            AG_001_ASG.modified_date = DateTime.UtcNow;
            AG_001_ASG.modified_by = pubsess.userid;
            AG_001_ASG.note = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
           if(action_flag == "Create")
                db.Entry(AG_001_ASG).State = EntityState.Added;
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
    }
}


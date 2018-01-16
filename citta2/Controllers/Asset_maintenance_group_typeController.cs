using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CittaErp.Models;
using anchor1.Filters;
using System.Data.Entity;
using CittaErp.utilities;

namespace CittaErp.Controllers
{
    public class Asset_maintenance_group_typeController : Controller
    {
        // GET: Asset_maintenance_group_type
        AG_001_AMG AG_001_AMG = new AG_001_AMG();
        AG_002_ASSET AG_002_ASSET = new AG_002_ASSET();

        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        decimal deccalcc = 0;
        decimal deccalcc1 = 0;
        decimal deccalcc2 = 0;
        decimal deccalcc3 = 0;
        decimal deccalcc4 = 0;
        int counter = 0;
        int counter1 = 0;
        int counter2 = 0;
        int counter3 = 0;
        int counter4 = 0;
        int counter5 = 0;
        List<string> count = new List<string>();
        List<string> count1 = new List<string>();
        List<string> count2 = new List<string>();
        List<string> count3 = new List<string>();
        List<string> count4 = new List<string>();
        List<string> count5 = new List<string>();
        List<string> listpart = new List<string>();
        string me = "";

        string action_flag = ""; 
        bool err_flag;

        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.AG_001_AMG
                         join bg in db.GB_999_MSG
                         on new {a1 = bh.nature} equals new {a1 = bg.code_msg}
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         where bg2.type_msg == "nat"
                         select new vw_genlay
                         {
                             vwstring0 = bh.maintenance_group_type_id,
                             vwstring1 = bh.description,
                             vwstring2 = bg2.name1_msg,
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
        public ActionResult Create(vw_genlay glay_in, string subcheck, int tabcheck)
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            if (subcheck == "RO")
            {
                glay.vwint7 = tabcheck;
                assign2();
                select_query();
                ModelState.Remove("subcheck");
                return View(glay);
            }
            update_file();
            assign2();
            if (err_flag)
                return RedirectToAction("Create");
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult subcontract(string id)
        {

            var sub = (from bh in db.SC_001_SCM
                       where bh.sub_contract_id == id
                       select new { bh }).FirstOrDefault();


            string amt = sub.bh.total_cost.ToString();
            string des = sub.bh.description;

            List<SelectListItem> ary2 = new List<SelectListItem>();

            ary2.Add(new SelectListItem { Value = "1", Text = des });
            ary2.Add(new SelectListItem { Value = "2", Text = amt });

            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(ary2.ToArray(), "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult job_details(int id)
        {
            var job = (from bh in db.JB_001_JOB
                       where bh.job_id == id
                       select new { bh }).FirstOrDefault();

            string cost_basis = job.bh.costing_basis;
            decimal cost = job.bh.cost;

            List<SelectListItem> ary2 = new List<SelectListItem>();

            ary2.Add(new SelectListItem { Value = "1", Text = cost_basis });
            ary2.Add(new SelectListItem { Value = "2", Text = cost.ToString() });

            if (HttpContext.Request.IsAjaxRequest())
            {

                return Json(new SelectList(ary2.ToArray(), "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error");
            }


        }

        [EncryptionActionAttribute]
        public ActionResult Edit(string key1)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            AG_001_AMG = db.AG_001_AMG.Find(key1);
            if (AG_001_AMG != null)
               read_record();
            select_query();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, string subcheck , int tabcheck)
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
            if (subcheck == "RO")
            {
                glay.vwint7 = tabcheck;
                assign2();
                select_query();
                ModelState.Remove("subcheck");
                return View(glay);
            }
            update_file();
            assign2();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AG_002_ASSET] where maintenance_group_type_id='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);

            string sqlstr1 = "delete from [dbo].[AG_001_AMG] where maintenance_group_type_id='" + id + "'";
            int delctr1 = db.Database.ExecuteSqlCommand(sqlstr1);

            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            AG_001_AMG = db.AG_001_AMG.Find(glay.vwstring0);
            if (AG_001_AMG != null)
            {
                db.AG_001_AMG.Remove(AG_001_AMG);
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
                AG_001_AMG = new AG_001_AMG();
                AG_001_AMG.created_by = pubsess.userid;
                AG_001_AMG.created_date = DateTime.UtcNow;
            }
            else
            {
                AG_001_AMG = db.AG_001_AMG.Find(glay.vwstring0);
            }

            string str1 = "delete from AG_002_ASSET where maintenance_group_type_id =" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(str1);

            AG_001_AMG.maintenance_group_type_id = glay.vwstring0;
            AG_001_AMG.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;

            AG_001_AMG.nature = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;


            AG_001_AMG.required_maintenance_basis = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            AG_001_AMG.require_asset_running = Convert.ToInt32(glay.vwstring4);
            AG_001_AMG.task_descriptioin_id = "";
            AG_001_AMG.item_group_code = "";
            AG_001_AMG.qty = "";
            AG_001_AMG.gl_account = string.IsNullOrWhiteSpace(glay.vwstring6) ? "" : glay.vwstring6;
            AG_001_AMG.staff_id = "";
            AG_001_AMG.Estimated_hour = "";
            AG_001_AMG.sub_contracts_id = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            AG_001_AMG.estimated_total = glay.vwdecimal0;
            AG_001_AMG.material_total = glay.vwdecimal1;
            AG_001_AMG.hr_total = glay.vwdecimal2;
            AG_001_AMG.subcontract_total = glay.vwdecimal3;


            AG_001_AMG.Completed = glay.vwbool1 ? "Y" : "N";
            AG_001_AMG.modified_date = DateTime.UtcNow;
            AG_001_AMG.modified_by = pubsess.userid;
            AG_001_AMG.inactive_status = glay.vwbool0 ? "Y" : "N";
            AG_001_AMG.note = string.IsNullOrWhiteSpace(glay.vwstring8) ? "" : glay.vwstring8;

            int counter = 1;
            for (int i = 0; i < glay.vwstrarray0.Length; i++)
            {

                if (glay.vwstrarray0[i] != "")
                {
                    AG_002_ASSET AG_002_ASSET = new AG_002_ASSET();
                    AG_002_ASSET.maintenance_group_type_id = glay.vwstring0;
                    AG_002_ASSET.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[i]) ? "" : glay.vwstrarray0[i];
                    AG_002_ASSET.head_count = 0;
                    AG_002_ASSET.qtyreq = glay.vwitarray0[i];
                    AG_002_ASSET.time = "";
                    AG_002_ASSET.sequence_no = counter.ToString();
                    AG_002_ASSET.total = glay.vwdclarray3[i];
                    AG_002_ASSET.flagg = "I";
                    db.Entry(AG_002_ASSET).State = EntityState.Added;
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

                if (glay.vwstrarray1[i] != "")
                {

                    AG_002_ASSET AG_002_ASSET = new AG_002_ASSET();

                    AG_002_ASSET.maintenance_group_type_id = glay.vwstring0;
                    AG_002_ASSET.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray1[i]) ? "" : glay.vwstrarray1[i];
                    AG_002_ASSET.head_count = glay.vwitarray1[i];
                    AG_002_ASSET.qtyreq = glay.vwitarray2[i];
                    AG_002_ASSET.time = glay.vwstrarray3[i];
                    AG_002_ASSET.sequence_no = counter.ToString();
                    AG_002_ASSET.total = glay.vwdclarray0[i];
                    AG_002_ASSET.flagg = "J";
                    db.Entry(AG_002_ASSET).State = EntityState.Added;
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

                if (i <= 3)
                {
                    if (glay.vwstrarray2[i] != "")
                    {

                        AG_002_ASSET AG_002_ASSET = new AG_002_ASSET();

                        AG_002_ASSET.maintenance_group_type_id = glay.vwstring0;
                        AG_002_ASSET.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray2[i]) ? "" : glay.vwstrarray2[i];
                        AG_002_ASSET.head_count = 0;
                        AG_002_ASSET.qtyreq = 0;
                        AG_002_ASSET.sequence_no = counter.ToString();
                        AG_002_ASSET.time = "";
                        AG_002_ASSET.total = glay.vwdclarray2[i];
                        AG_002_ASSET.flagg = "L";
                        db.Entry(AG_002_ASSET).State = EntityState.Added;
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
                 if (!(string.IsNullOrWhiteSpace(glay.vwstrarray6[i])))
                {

                    AG_002_ASSET AG_002_ASSET = new AG_002_ASSET();

                    AG_002_ASSET.maintenance_group_type_id = glay.vwstring0;
                    AG_002_ASSET.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray7[i]) ? "" : glay.vwstrarray7[i];
                    AG_002_ASSET.head_count = 0;
                    AG_002_ASSET.qtyreq =glay.vwitarray3[i];
                    AG_002_ASSET.time = string.IsNullOrWhiteSpace(glay.vwstrarray6[i]) ? "" : glay.vwstrarray6[i];
                    AG_002_ASSET.sequence_no = counter.ToString();
                    AG_002_ASSET.total = 0;
                    AG_002_ASSET.flagg = "K";
                    db.Entry(AG_002_ASSET).State = EntityState.Added;
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

                counter++;
               
            }

            // int counting = 0;
            //for (int i = 0; i < 40; i += 4) {

            //    if (glay.vwstrarray1[i] != "")
            //    {

            //        AG_002_ASSET AG_002_ASSET = new AG_002_ASSET();

            //        AG_002_ASSET.maintenance_group_type_id = glay.vwstring0;
            //        AG_002_ASSET.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray1[i]) ? "" : glay.vwstrarray1[i];
            //        AG_002_ASSET.head_count = string.IsNullOrWhiteSpace(glay.vwstrarray1[i + 1]) ? "" : glay.vwstrarray1[i + 1];
            //        AG_002_ASSET.qtyreq = string.IsNullOrWhiteSpace(glay.vwstrarray1[i + 2]) ? "" : glay.vwstrarray1[i + 2];
            //        AG_002_ASSET.sequence_no = counting.ToString();
            //        AG_002_ASSET.total = string.IsNullOrWhiteSpace(glay.vwstrarray1[i + 3]) ? "" : glay.vwstrarray1[i + 3];
            //        AG_002_ASSET.flagg = "J";
            //        db.Entry(AG_002_ASSET).State = EntityState.Added;
            //        try
            //        {
            //            db.SaveChanges();
            //        }

            //        catch (Exception err)
            //        {
            //            if (err.InnerException == null)
            //                ModelState.AddModelError(String.Empty, err.Message);
            //            else
            //                ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);

            //            err_flag = false;
            //        }
            //        counting++;
            //    }
            //}



           if(action_flag == "Create")
            {
                db.Entry(AG_001_AMG).State = EntityState.Added;
            }
            else
            {
                db.Entry(AG_001_AMG).State = EntityState.Modified;
            }

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

            bool alert = false;

            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please insert Maintenance Type ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Pleease insert Descripton");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring2))
            {

                ModelState.AddModelError(String.Empty, "Please select Maintenance Nature");
                err_flag = false;
            }


            //if (glay.vwstring2 == "nat1")
            //{
            //    if (!string.IsNullOrWhiteSpace(glay.vwstring2))
            //    {
            //        if (string.IsNullOrWhiteSpace(glay.vwstring3))
            //        {
            //            ModelState.AddModelError(String.Empty, "Please Select Scheduling Basis");
            //            err_flag = false;
            //        }
            //    }
            //}

            //if (!string.IsNullOrWhiteSpace(glay.vwstring3))
            //{
            //    if (string.IsNullOrWhiteSpace(glay.vwstring4))
            //    {
            //        ModelState.AddModelError(String.Empty, "Please enter Required Maintenance Value");
            //        err_flag = false;
            //    }
            //}

            if (string.IsNullOrWhiteSpace(glay.vwstring6))
            {
                ModelState.AddModelError(String.Empty, "Please select GL Account");
                err_flag = false;
            }


            int find_itemcode = 0;
            for (int i = 0; i < glay.vwstrarray0.Length; i += 3)
            {
                string def = glay.vwstrarray0[i];

                for (int j = 0; j < glay.vwstrarray0.Length; j++)
                {
                    if (glay.vwstrarray0[j] != "")
                    {

                        if (def.Equals(glay.vwstrarray0[j]))
                        {
                            find_itemcode += 1;
                        }

                        if (find_itemcode == 2)
                        {
                            alert = true;
                            find_itemcode = 0;
                        }
                    }
                }

                if (alert == true)
                {
                    ModelState.AddModelError(String.Empty, "Item Code can not accept duplicates");
                    err_flag = false;
                    break;
                }

                find_itemcode = 0;
            }


           if(action_flag == "Create")
            {
                AG_001_AMG bnk = db.AG_001_AMG.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Maintenance Type ID can not accept duplicates");
                    err_flag = false;
                }

                var desc = (from bg in db.AG_001_AMG
                            select bg.description).ToList();

                foreach (string des in desc)
                {
                    if (des.Equals(glay.vwstring1))
                    {
                        ModelState.AddModelError(String.Empty, "Description can not accept duplicates");
                        err_flag = false;
                        break;
                    }

                }
            }

            //if (error_msg != "")
            //{
            //    ModelState.AddModelError(String.Empty, error_msg);
            //    err_flag = false;
            //}

        }
        private void select_query()
        {
            
            var bgsub = from bg in db.SC_001_SCM
                        select new
                        {
                            c1 = bg.sub_contract_id + "-" + bg.vendor_id,
                            c2 = bg.sub_contract_id
                        };

            ViewBag.subcon = new SelectList(bgsub.ToList(), "c2", "c1");

            //var bgmec = from bg in db.ME_001_MEC
            //            select bg;

            //ViewBag.mainexp = new SelectList(bgmec.ToList(), "maintenance_expense_id", "description", glay.vwstring8);



            var pay = from bg in db.GB_999_MSG
                      where bg.type_msg == "cost"
                      select bg;
            ViewBag.pay1 = new SelectList(pay.ToList(), "code_msg", "name1_msg");

            var nat = from bh in db.GB_999_MSG
                      where bh.type_msg == "nat"
                      select bh;
            ViewBag.nat1 = new SelectList(nat.ToList(), "code_msg", "name1_msg", glay.vwstring2);

            var sch = from bh in db.GB_999_MSG
                      where bh.type_msg == "sch"
                      select bh;
            ViewBag.sch1 = new SelectList(sch.ToList(), "code_msg", "name1_msg", glay.vwstring3);

            var itm = (from bh in db.IV_001_ITEM
                       where bh.active_status== "N"
                       select new { c1 = bh.item_code, c2 = bh.item_name });

            ViewBag.itm1 = new SelectList(itm.ToList(), "c1", "c2");


            var job = from bh in db.JB_001_JOB
                      select bh;
            ViewBag.job1 = new SelectList(job.ToList(), "job_id", "job_title");

            var uom = (from bh in db.GB_001_PCODE
                       join bk in db.IV_001_ITEM
                       on new { c1 = bh.parameter_code } equals new { c1 = bk.sku_sequence }
                       where bh.parameter_code == "05"
                       select new { x1 = bh.parameter_code, x2 = bh.parameter_name });

            ViewBag.uom1 = new SelectList(uom.ToList(), "x1", "x2");


            var chart = from bg in db.GL_001_CHART
                        select bg;
            ViewBag.chart1 = new SelectList(chart.ToList(), "account_code", "account_name", glay.vwstring6);

            var bgcompete = from bg in db.TC_001_TCL
                            where bg.inactive_status == "N"
                            select new { d1 = bg.technical_competency_level_id, d2 = bg.description };
            ViewBag.competency = new SelectList(bgcompete.ToList(), "d1", "d2");
        }

        [HttpPost]
        public ActionResult assign2(string id)
        {
            //var bgassign3 = (from bg in db.TK_001_TAC
            //                 where bg.task_id == id
            //                 select new { bg }).FirstOrDefault();
            //string taskcomm = bgassign3.bg.note;
            //string actstatus = bgassign3.bg.inactive_status;

            //List<SelectListItem> ary2 = new List<SelectListItem>();
            //ary2.Add(new SelectListItem { Value = "comm", Text = taskcomm });
            //ary2.Add(new SelectListItem { Value = "actstatus", Text = actstatus });

            //ViewData["actstatus"] = actstatus;
            //if (HttpContext.Request.IsAjaxRequest())
            //    return Json(new SelectList(
            //                    ary2.ToArray(),
            //                    "Value",
            //                    "Text")
            //               , JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult assign(string id)
        {
            var bgassign3 = (from bg in db.IV_001_ITEM
                             join bg1 in db.GB_001_PCODE
                             on new { a1 = bg.sku_sequence } equals new { a1 = bg1.parameter_code }
                             where bg.item_code == id
                             select new { bg, bg1 }).FirstOrDefault();
            string itemcost = bgassign3.bg.selling_price_class1.ToString();
            string measure = bgassign3.bg1.parameter_name.ToString();

            List<SelectListItem> ary2 = new List<SelectListItem>();
            ary2.Add(new SelectListItem { Value = "itemcostt", Text = itemcost });
            ary2.Add(new SelectListItem { Value = "measure", Text = measure });

            ViewData["itemcost"] = itemcost;
            ViewData["measure"] = measure;

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary2.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");
        }
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray1 = new string[50];
            glay.vwstrarray2 = new string[50];
            glay.vwstrarray3 = new string[50];
            glay.vwstrarray4 = new string[50];
            
            glay.vwstrarray9 = new string[50];

            glay.vwdclarray0 = new decimal[50];
            glay.vwdclarray1 = new decimal[50];
            glay.vwdclarray2 = new decimal[50];
            glay.vwdclarray3 = new decimal[50];

            glay.vwitarray0 = new int[50];
            glay.vwitarray1 = new int[50];
            glay.vwitarray2 = new int[50];
            glay.vwitarray3 = new int[50];
            glay.vwstrarray5 = new string[50];
            glay.vwstrarray6 = new string[50];
            glay.vwstrarray7 = new string[50];


            glay.vwint1 = 1;
            glay.vwint5 =1;
            glay.vwint4 = 1;
            glay.vwint3 = 1;
            glay.vwstrarray5[0] = "tab-pane active";
            glay.vwstrarray5[1] = "tab-pane";
            glay.vwstrarray5[2] = "tab-pane";
            glay.vwstrarray5[3] = "tab-pane";
            glay.vwstrarray5[4] = "tab-pane";
            for (int i = 0; i < 50; i++)
            {

                glay.vwstrarray0 [i] = "";
                glay.vwstrarray1 [i] = "";
                glay.vwstrarray2 [i] = "";
                glay.vwstrarray3 [i] = "";
                glay.vwstrarray4 [i] = "";

                glay.vwstrarray6 [i] = "";
                glay.vwstrarray7 [i] = "";
                glay.vwdclarray0[i] =0;
                glay.vwdclarray1[i] = 0;
                glay.vwdclarray2[i] = 0;
                glay.vwdclarray3[i] = 0;
                glay.vwitarray0[i] = 0;
                glay.vwitarray1[i] = 0;
                glay.vwitarray2[i] = 0;
                glay.vwitarray3[i] = 0;
                glay.vwstrarray9[i] = "";
                glay.vwstrarray3[i] = "";
            }
        }
        private void read_record()
        {
            string temp0 = glay.vwstring0;
            string temp1 = glay.vwstring1;
            string temp2 = glay.vwstring2;
            string temp3 = glay.vwstring3;
            string temp4 = glay.vwstring4;
            string temp5 = glay.vwstring5;
            string temp6 = glay.vwstring6;
            string temp7 = glay.vwstring7;
            string temp8 = glay.vwstring8;
            string temp9 = glay.vwstring9;
            decimal dcl1 = glay.vwdecimal0;
            int[] sst = new int[glay.vwitarray2.Length];
            glay.vwitarray2.CopyTo(sst, 0);
            int[] ssi = new int[glay.vwitarray1.Length];
            glay.vwitarray1.CopyTo(ssi, 0);
            string[] subv = new string[glay.vwstrarray2.Length];
            glay.vwstrarray2.CopyTo(subv, 0);
            decimal[] suba = new decimal[glay.vwdclarray2.Length];
            glay.vwdclarray2.CopyTo(suba, 0);
            decimal[] dc3 = new decimal[glay.vwdclarray3.Length];
            glay.vwdclarray3.CopyTo(dc3, 0);
            for (int i = 0; i < 10; i++)
            {
                ModelState.Remove("vwstring" + i);
                ModelState.Remove("vwdclarray9[" + i + "]");
                ModelState.Remove("vwdclarray1[" + i + "]");
                ModelState.Remove("vwitarray1[" + i + "]");
                ModelState.Remove("vwitarray2[" + i + "]");
                ModelState.Remove("vwstrarray3[" + i + "]");
                ModelState.Remove("vwstrarray2[" + i + "]");
                ModelState.Remove("vwdclarray3[" + i + "]");
                ModelState.Remove("vwstrarray4[" + i + "]");

            }
            ModelState.Remove("vwdecimal0");
            ModelState.Remove("vwdecimal3");
            ModelState.Remove("vwdecimal1");
            ModelState.Remove("vwdecimal2");
            ModelState.Remove("vwint1");
            ModelState.Remove("vwint5");
            ModelState.Remove("vwint4");
            ModelState.Remove("vwint0");
            glay.vwstring0 = temp0;
            glay.vwstring1 = temp1;
            glay.vwstring2 = temp2;
            glay.vwstring3 = temp3;
            glay.vwstring4 = temp4;
            glay.vwstring5 = temp5;
            glay.vwstring6 = temp6;
            glay.vwstring7 = temp7;
            glay.vwstring8 = temp8;
            glay.vwstring9 = temp9;
            glay.vwdecimal0 = dcl1;
            sst.CopyTo(glay.vwitarray2, 0);
            ssi.CopyTo(glay.vwitarray1, 0);
            subv.CopyTo(glay.vwstrarray2, 0);
            suba.CopyTo(glay.vwdclarray2, 0);
            dc3.CopyTo(glay.vwdclarray3, 0);
            glay.vwstring0 = AG_001_AMG.maintenance_group_type_id;
            glay.vwstring1 = AG_001_AMG.description;
                glay.vwstring0 = AG_001_AMG.maintenance_group_type_id;
            glay.vwdecimal0 = AG_001_AMG.estimated_total;
            glay.vwstring1 = AG_001_AMG.description;
            glay.vwstring2 = AG_001_AMG.nature;
            glay.vwstring3 = AG_001_AMG.required_maintenance_basis;
            glay.vwstring4 = AG_001_AMG.require_asset_running.ToString();
            glay.vwint0 = AG_001_AMG.require_asset_running;

            glay.vwstring5 = AG_001_AMG.task_descriptioin_id;
            glay.vwstring6 = AG_001_AMG.gl_account;
            glay.vwdecimal0 = AG_001_AMG.estimated_total;
            glay.vwdecimal1 = AG_001_AMG.material_total;
            glay.vwdecimal2 = AG_001_AMG.hr_total;
            glay.vwdecimal3 = AG_001_AMG.subcontract_total;

            //glay.vwstring6 = AG_001_AMG.team_lead;
            //glay.vwstring8 = AG_001_AMG.asset_or_group;
            //glay.vwdclarray3[0] = AG_001_AMG.total_materials_cost;
            //glay.vwdclarray3[1] = AG_001_AMG.total_hr_cost;
            //glay.vwdclarray3[2] = AG_001_AMG.total_duration;
            //glay.vwdclarray3[3] = AG_001_AMG.total_contract_amount;
            //glay.vwdclarray3[4] = AG_001_AMG.total_misc_amount;

            int counter = 0;
            string get_id = glay.vwstring0;
            var bgassign1 = (from bg in db.AG_002_ASSET
                             join bh in db.AG_001_AMG
                             on new { a1 = bg.maintenance_group_type_id } equals new { a1 = bh.maintenance_group_type_id }
                             into bh1
                             from bh2 in bh1.DefaultIfEmpty()
                             where bg.maintenance_group_type_id == get_id
                             orderby bg.flagg, bg.sequence_no
                             select new { bg, bh2 }).ToList();
            foreach (var item in bgassign1)
            {
                if (item.bg.flagg == "I")
                {
                    count.Add(item.bg.sequence_no);
                    me = item.bg.identity_code;
                    glay.vwstrarray0[counter] = me;
                    glay.vwdclarray3[counter] = item.bg.total;
                    glay.vwitarray0[counter] = item.bg.qtyreq;
                    var bgassignc = (from bg in db.IV_001_ITEM
                                     join bh in db.GB_001_PCODE
                                     on new { a1 = bg.sku_sequence } equals new { a1 = bh.parameter_code }
                                     into bh1
                                     from bh2 in bh1.DefaultIfEmpty()
                                     where bh2.parameter_type == "10" && bg.item_code == me
                                     select new { bg, bh2 }).FirstOrDefault();
                    if (bgassignc != null)
                    {
                        glay.vwdclarray1[counter] = bgassignc.bg.selling_price_class1;
                        ViewData["measure" + counter] = bgassignc.bh2.parameter_name;
                    }
                    counter++;
                }
                if (item.bg.flagg == "J")
                {
                    me = item.bg.identity_code;
                    count1.Add(item.bg.sequence_no);
                    int jab= Convert.ToInt32(item.bg.identity_code);
                    glay.vwstrarray1[counter1] = me;
                    glay.vwitarray1[counter1] = item.bg.head_count;
                    glay.vwitarray2[counter1] = item.bg.qtyreq;
                    var bgassignc = (from bg in db.JB_001_JOB
                                     join bh in db.GB_999_MSG
                                     on new { a1 = bg.costing_basis } equals new { a1 = bh.code_msg }
                                     into bh1
                                     from bh2 in bh1.DefaultIfEmpty()
                                     where bh2.type_msg == "cost" && bg.job_id == jab
                                     select new { bg, bh2 }).FirstOrDefault();
                    if (bgassignc != null)
                    {
                        glay.vwstrarray3[counter1] = bgassignc.bg.costing_basis;
                        ViewData["cost" + counter1] = bgassignc.bg.cost;
                        glay.vwdclarray0[counter1] = item.bg.total;
                    }
                    counter1++;

                }
                if (item.bg.flagg == "L")
                {

                    me = item.bg.identity_code;
                    count2.Add(item.bg.sequence_no);
                    glay.vwstrarray2[counter3] = item.bg.identity_code;
                    glay.vwdclarray2[counter3] = item.bg.total;
                    var bgassignme = (from bg in db.SC_001_SCM
                                      join bh in db.AP_001_VENDR
                                      on new { a1 = bg.vendor_id } equals new { a1 = bh.vendor_code }
                                      into bh1
                                      from bh2 in bh1.DefaultIfEmpty()
                                      where bg.sub_contract_id == me
                                      select new { bg, bh2 }).FirstOrDefault();
                    if (bgassignme != null)
                    {
                        glay.vwstrarray4[counter3] = bgassignme.bg.description;
                    }
                    counter3++;

                }
                if (item.bg.flagg == "K")
                {

                    count5.Add(item.bg.sequence_no);
                    glay.vwstrarray6[counter5] = item.bg.time;
                    glay.vwitarray3[counter5] = item.bg.qtyreq;
                        glay.vwstrarray7[counter5] = item.bg.identity_code;
                    counter5++;
                }
            }
            if(count.Count !=0)
            glay.vwint1 = count.Count;
            if (count1.Count != 0)
                glay.vwint5 = count1.Count;
            if (count2.Count != 0)
                glay.vwint4 = count2.Count;
            if (count5.Count != 0)
                glay.vwint3 = count5.Count;
        }

        public void assign2()
        {
            string temp0 = glay.vwstring0;
            string temp1 = glay.vwstring1;
            string temp2 = glay.vwstring2;
            string temp3 = glay.vwstring3;
            string temp4 = glay.vwstring4;
            string temp5 = glay.vwstring5;
            string temp6 = glay.vwstring6;
            string temp7 = glay.vwstring7;
            string temp8 = glay.vwstring8;
            string temp9 = glay.vwstring9;
            decimal dcl1 = glay.vwdecimal0;
            int[] sst = new int[glay.vwitarray2.Length];
            glay.vwitarray2.CopyTo(sst, 0);
            int[] ssi = new int[glay.vwitarray1.Length];
            glay.vwitarray1.CopyTo(ssi, 0);
            string[] subv = new string[glay.vwstrarray2.Length];
            glay.vwstrarray2.CopyTo(subv, 0);
            decimal[] suba = new decimal[glay.vwdclarray2.Length];
            glay.vwdclarray2.CopyTo(suba, 0);
            decimal[] ony = new decimal[glay.vwdclarray0.Length];
            glay.vwdclarray0.CopyTo(ony, 0);
            string[] subd = new string[glay.vwstrarray4.Length];
            glay.vwstrarray4.CopyTo(subd, 0);
            for (int i = 0; i < 10; i++)
            {
                ModelState.Remove("vwstring" + i);
                ModelState.Remove("vwdclarray9[" + i + "]");
                ModelState.Remove("vwdclarray1[" + i + "]");
                ModelState.Remove("vwdclarray0[" + i + "]");
                ModelState.Remove("vwitarray1[" + i + "]");
                ModelState.Remove("vwitarray2[" + i + "]");
                ModelState.Remove("vwstrarray3[" + i + "]");
                ModelState.Remove("vwstrarray2[" + i + "]");
                ModelState.Remove("vwdclarray2[" + i + "]");
                ModelState.Remove("vwdclarray3[" + i + "]");
                ModelState.Remove("vwstrarray4[" + i + "]");
                
            }
            ModelState.Remove("vwdecimal0");
            ModelState.Remove("vwdecimal3");
            ModelState.Remove("vwdecimal1");
            ModelState.Remove("vwdecimal2");
            ModelState.Remove("vwint1");
            ModelState.Remove("vwint5");
            ModelState.Remove("vwint4");
            glay.vwstring0 = temp0;
            glay.vwstring1 = temp1;
            glay.vwstring2 = temp2;
            glay.vwstring3 = temp3;
            glay.vwstring4 = temp4;
            glay.vwstring5 = temp5;
            glay.vwstring6 = temp6;
            glay.vwstring7 = temp7;
            glay.vwstring8 = temp8;
            glay.vwstring9 = temp9;
            glay.vwdecimal0 = dcl1;
            sst.CopyTo(glay.vwitarray2, 0);
            ssi.CopyTo(glay.vwitarray1, 0);
            subv.CopyTo(glay.vwstrarray2, 0);
            suba.CopyTo(glay.vwdclarray2, 0);
            subd.CopyTo(glay.vwstrarray4, 0);

            while (!(string.IsNullOrWhiteSpace(glay.vwstrarray0[counter])))
            {
                me = glay.vwstrarray0[counter];
                var bgassignc = (from bg in db.IV_001_ITEM
                                 join bh in db.GB_001_PCODE
                                 on new { a1 = bg.sku_sequence } equals new { a1 = bh.parameter_code }
                                 into bh1
                                 from bh2 in bh1.DefaultIfEmpty()
                                 where bh2.parameter_type == "10" && bg.item_code == me
                                 select new { bg, bh2 }).FirstOrDefault();
                if (bgassignc != null)
                {
                    glay.vwdclarray1[counter] = bgassignc.bg.selling_price_class1;
                   glay.vwdclarray3[counter]  = glay.vwitarray0[counter] * glay.vwdclarray1[counter];
                    ViewData["measure" + counter] = bgassignc.bh2.parameter_name;
                    count.Add(glay.vwstrarray0[counter]);
                }

                deccalcc += glay.vwitarray0[counter] * glay.vwdclarray1[counter];
                glay.vwdecimal1 = deccalcc;
                counter++;

            }

            while (!(string.IsNullOrWhiteSpace(glay.vwstrarray1[counter1])))
            {

                int jab = Convert.ToInt32(glay.vwstrarray1[counter1]);
                var bgassignc = (from bg in db.JB_001_JOB
                                 join bh in db.GB_999_MSG
                                 on new { a1 = bg.costing_basis } equals new { a1 = bh.code_msg }
                                 into bh1
                                 from bh2 in bh1.DefaultIfEmpty()
                                 where bh2.type_msg == "cost" && bg.job_id == jab
                                 select new { bg, bh2}).FirstOrDefault();
                if (bgassignc != null)
                {
                    ViewData["cost" + counter1] = bgassignc.bg.cost;
                    if (glay.vwstrarray3[counter1] == "D")
                    {
                        glay.vwdclarray0[counter1] = glay.vwitarray1[counter1] * bgassignc.bg.cost * glay.vwitarray2[counter1];
                        deccalcc1 += glay.vwitarray1[counter1] * bgassignc.bg.cost * glay.vwitarray2[counter1];
                        glay.vwdecimal2 = deccalcc1;
                    }
                    else if (glay.vwstrarray3[counter1] == "H")
                    {
                        double hour = 0.041667;
                        glay.vwdclarray0[counter1] = Math.Round((glay.vwitarray1[counter1] * bgassignc.bg.cost)* glay.vwitarray2[counter1] * Convert.ToDecimal(hour), 2);
                        deccalcc1 += glay.vwitarray1[counter1] * glay.vwitarray2[counter1] * bgassignc.bg.cost * Convert.ToDecimal(hour);
                        glay.vwdecimal2 = Math.Round(deccalcc1, 2);
                    }
                    else if (glay.vwstrarray3[counter1] == "M")
                    {
                        double hour = 0.016667;
                        glay.vwdclarray0[counter1] = Math.Round((glay.vwitarray1[counter1]* glay.vwitarray2[counter1] * bgassignc.bg.cost) * Convert.ToDecimal(hour), 2);
                        deccalcc1 += glay.vwitarray1[counter1]* glay.vwitarray2[counter1] * bgassignc.bg.cost * Convert.ToDecimal(hour);
                        glay.vwdecimal2 = Math.Round(deccalcc1, 2);
                    }
                    count1.Add(glay.vwstrarray1[counter1]);

                }
                counter1++;

            }

            while (!(string.IsNullOrWhiteSpace(glay.vwstrarray2[counter4])))
            {
                string subval = glay.vwstrarray2[counter4];
                count2.Add(glay.vwstrarray2[counter4]);
                var bgassignme = (from bg in db.SC_001_SCM
                                  join bh in db.AP_001_VENDR
                                  on new { a1 = bg.vendor_id } equals new { a1 = bh.vendor_code }
                                  into bh1
                                  from bh2 in bh1.DefaultIfEmpty()
                                  where bg.sub_contract_id == subval
                                  select new { bg, bh2 }).FirstOrDefault();
                if(glay.vwdclarray2[counter4]==0)
                glay.vwdclarray2[counter4] = bgassignme.bg.total_cost;
                glay.vwstrarray4[counter4] = bgassignme.bg.description;
                deccalcc4 += glay.vwdclarray2[counter4];
                glay.vwdecimal3 = deccalcc4;
                counter4++;
            }
            while (!(string.IsNullOrWhiteSpace(glay.vwstrarray6[counter5])))
            {
                count5.Add(glay.vwstrarray2[counter5]);
                deccalcc2 += glay.vwitarray3[counter5];
                glay.vwdecimal4 = deccalcc2;
                counter5++;
            }
            glay.vwdecimal0 = deccalcc + deccalcc1 + deccalcc2 + deccalcc3 + deccalcc4;
            if (count.Count != 0)
                glay.vwint1 = count.Count;
            if (count1.Count != 0)
                glay.vwint5 = count1.Count;
            if (count2.Count != 0)
                glay.vwint4 = count2.Count;
            if (count5.Count != 0)
                glay.vwint3 = count5.Count;

            if (glay.vwint7 == 1)
            {
                glay.vwstrarray5[1] = "tab-pane active";
                glay.vwstrarray5[0] = "tab-pane";
                glay.vwstrarray5[2] = "tab-pane";
                glay.vwstrarray5[3] = "tab-pane";
                glay.vwstrarray5[4] = "tab-pane";
            }
            else if (glay.vwint7 == 2)
            {
                glay.vwstrarray5[2] = "tab-pane active";
                glay.vwstrarray5[0] = "tab-pane";
                glay.vwstrarray5[1] = "tab-pane";
                glay.vwstrarray5[3] = "tab-pane";
                glay.vwstrarray5[4] = "tab-pane";
            }
            else if (glay.vwint7 == 4)
            {
                glay.vwstrarray5[3] = "tab-pane active";
                glay.vwstrarray5[0] = "tab-pane";
                glay.vwstrarray5[1] = "tab-pane";
                glay.vwstrarray5[2] = "tab-pane";
                glay.vwstrarray5[4] = "tab-pane";
            }
            else if (glay.vwint7 == 3)
            {
                glay.vwstrarray5[4] = "tab-pane active";
                glay.vwstrarray5[0] = "tab-pane";
                glay.vwstrarray5[1] = "tab-pane";
                glay.vwstrarray5[2] = "tab-pane";
                glay.vwstrarray5[3] = "tab-pane";
            }
        }
        private void error_message()
        {

        }

    }
}
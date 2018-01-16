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
    public class WorkOrderController : Controller
    {
        WO_002_WKO WO_002_WKO = new WO_002_WKO();
        WO_002_WKODT WO_002_WKODT = new WO_002_WKODT();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        bool err_flag = true;
        string[] container = new string[] { };
        List<SelectListItem> listpart = new List<SelectListItem>();
        int[] rt = new int[50];
        string[] st = new string[50];
        string me = "";
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
        List<string> count = new List<string>();
        List<string> count1 = new List<string>();
        List<string> count2 = new List<string>();
        List<string> count3 = new List<string>();
        List<string> count4 = new List<string>();
        DateTime dt;
        int itcal = 0;
        Double dbcal = 0;
        string action_flag = "";
        // GET: WorkOrder
        public ActionResult Index()
        {

            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            var bglist = from bh in db.WO_002_WKO
                         join bg in db.FA_001_ASSET
                         on new { a1 = bh.asset_or_group } equals new { a1 = bg.fixed_asset_code }
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join bi in db.GB_999_MSG
                         on new { a1 = bh.status, a2 = "stat" } equals new { a1 = bi.code_msg, a2 = bi.type_msg }
                         into bi1
                         from bi2 in bi1.DefaultIfEmpty()
                         join bj in db.GB_999_MSG
                         on new { a1 = bh.flag, a2 = "worknat" } equals new { a1 = bj.code_msg, a2 = bj.type_msg }
                         into bj1
                         from bj2 in bj1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.work_order_id,
                             vwstring3 = bg2.description,
                             vwstring4 = bh.asset_or_group,
                             vwstring6 = bj2.code_msg,
                             vwstring5 = bj2.name1_msg,
                             vwstring1 = bh.work_order_description,
                             vwstring2 = bi2.name1_msg
                         };
            return View(bglist.ToList());
        }

        public ActionResult Create(string key1, string subcheck, int flag)
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();

            if (subcheck == "C")
            {
                glay.vwstring9 = key1;
                glay.vwint6 = flag;
                string sqlstr = "";
                var bgme = (from bg in db.FA_001_ASSET
                            where bg.fixed_asset_code == key1
                            select bg.fixed_asset_code).FirstOrDefault();
                if (bgme != null)
                {
                    sqlstr = "UPDATE [dbo].[FA_001_ASSET] SET asset_requires_maintenace = 'N' WHERE fixed_asset_code =" + util.sqlquote(key1);
                }
                else
                {
                    sqlstr = "UPDATE [dbo].[AG_001_ASG] SET asset_requires_maintenace = 'N' WHERE asset_grouping_id =" + util.sqlquote(key1);
                }
                int delctr = db.Database.ExecuteSqlCommand(sqlstr);
                assign2();
                select_query();
                return View(glay);
            }
            glay.vwint0 = 1;
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, string subcheck, int tabcheck)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;

            if (subcheck == "RO")
            {
                glay.vwint7 = tabcheck;
                glay.vwint8 = 1;
                assign2();
                select_query();
                ModelState.Remove("subcheck");
                return View(glay);
            }
            update_file();
            if (err_flag)
                if (glay.vwstrarray2[10] == "N")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "WorkTransDoc");
                }
            glay.vwint8 = 1;
            assign2();
            select_query();
            return View(glay);
        }
        public ActionResult Edit(string key1, string key2, string key3)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            ModelState.Remove("vwstring2");
            initial_rtn();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            if (key1 == null)
                key1 = "";
            if (key2 == null)
                key2 = "";
            WO_002_WKO = db.WO_002_WKO.Find(key1, key2);
            if (WO_002_WKO != null)
                //glay.vwstring9 = key2;
            glay.vwstrarray2[13] = key2;
            glay.vwstrarray2[14] = key3;
            read_record();
            select_query();
            return View(glay);
        }
        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile, string subcheck, int tabcheck)
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
            if (subcheck == "RO")
            {
                glay.vwint7 = tabcheck;
                glay.vwint8 = 1;
                assign2();
                select_query();
                ModelState.Remove("subcheck");
                return View(glay);
            }
            if (glay.vwstrarray2[13] == null)
                glay.vwstrarray2[13] = "";
            update_file();
            if (err_flag)
            {
                if (glay.vwstrarray2[10] == "N" || glay.vwint9 !=1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "WorkTransDoc");
                }
            }
              
            glay.vwint8 = 1;
            assign2();
            select_query();
            return View(glay);
        }
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[WO_002_WKO] where work_order_id ='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            string swl = "delete from [dbo].[WO_002_WKODT] where work_order_id ='" + id + "'";
            int dt = db.Database.ExecuteSqlCommand(swl);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            WO_002_WKO = db.WO_002_WKO.Find(glay.vwstring0);
            if (WO_002_WKO != null)
            {
                db.WO_002_WKO.Remove(WO_002_WKO);
                db.SaveChanges();
            }
            string swl = "delete from [dbo].[WO_002_WKODT] where work_order_id ='" + glay.vwstring0 + "'";
            int dt = db.Database.ExecuteSqlCommand(swl);
        }
        private void select_query()
        {
            var bglist3 = from bh in db.IV_001_ITEM
                          where bh.active_status == "N"
                          select new { c1 = bh.item_code, c2 = bh.item_code + "-" + bh.item_name + " " + bh.specification };
            ViewBag.itemgroup = new SelectList(bglist3.ToList(), "c1", "c2");

            var bgmeasure = from bh in db.GB_999_MSG
                            where bh.type_msg == "cost"
                            select new { c1 = bh.code_msg, c2 = bh.name1_msg };
            ViewBag.timing = new SelectList(bgmeasure.ToList(), "c1", "c2");

            var bgcompete = from bg in db.TC_001_TCL
                            where bg.inactive_status == "N"
                            select new { d1 = bg.technical_competency_level_id, d2 = bg.description };
            ViewBag.competency = new SelectList(bgcompete.ToList(), "d1", "d2");

            var job = from bh in db.JB_001_JOB
                      select new { c1 = bh.job_id, c2 = bh.job_title };
            ViewBag.job1 = new SelectList(job.ToList(), "c1", "c2");


                if(ViewBag.emp == null)
            {
                var bgemp = from bg in db.GB_001_EMP
                            where bg.active_status == "N"
                            select new { c1 = bg.employee_code, c2 = bg.name };

                ViewBag.emp = new SelectList(bgemp.ToList(), "c1", "c2");
            }


            var chart = from bg in db.GL_001_CHART
                        select bg;
            ViewBag.chart1 = new SelectList(chart.ToList(), "account_code", "account_name", glay.vwstring7);

            var bgmaint = from bg in db.AG_001_AMG
                          where bg.inactive_status == "N" && bg.nature == "nat2"
                          select bg;
            ViewBag.maint = new SelectList(bgmaint.ToList(), "maintenance_group_type_id", "description", glay.vwstring3);
            var bgworkcent = from bg in db.WC_001_WKC
                             where bg.inactive_status == "N"
                             select bg;
            ViewBag.workcenter = new SelectList(bgworkcent.ToList(), "work_center_id", "description", glay.vwstring1);

            var subb = from bg in db.SC_001_SCM
                       join bh in db.AP_001_VENDR
                       on new { a1 = bg.vendor_id } equals new { a1 = bh.vendor_code }
                       into bh1
                       from bh2 in bh1.DefaultIfEmpty()
                       select new { c1 = bg.sub_contract_id, c2 = bg.sub_contract_id + "-" + bh2.vend_biz_name };
            ViewBag.subb = new SelectList(subb.ToList(), "c1", "c2");

            var taskk = from bh in db.TK_001_TAC
                        select new { c1 = bh.task_id, c2 = bh.task_description };
            ViewBag.taskid = new SelectList(taskk.ToList(), "c1", "c2");

            var bgteam = from bg in db.GB_001_EMP
                         where bg.active_status == "N"
                         select new { c1 = bg.employee_code, c2 = bg.name };

            ViewBag.team = new SelectList(bgteam.ToList(), "c1", "c2");


            if (glay.vwstrarray2[12] == "G")
            {
                var bglist1 = from bg in db.AG_001_ASG
                              where bg.inactive_status == "N"
                              select bg;


                ViewBag.description = new SelectList(bglist1.ToList(), "asset_grouping_id", "description", glay.vwstring9);

            }
            else if (glay.vwstrarray2[12] == "F" || glay.vwint6 == 2)
            {
                var bglist = from bg in db.FA_001_ASSET
                             where bg.active_status == "N"
                             orderby bg.reference_asset_code
                             select bg;

                ViewBag.description = new SelectList(bglist.ToList(), "fixed_asset_code", "description", glay.vwstring9);
            }

        }

        private void update_file()
        {
            err_flag = true;
            validation_routine();

            if (err_flag)
                update_record();

        }

        [HttpPost]
        public ActionResult workk(string id)
        {
            psess = (psess)Session["psess"];
            List<SelectListItem> ary2 = new List<SelectListItem>();

            //var bgassign3 = (from bg in db.JB_001_JOB
            //                 join bi in db.GB_001_EMP
            //                 on new { a1 = bg.job_id } equals new { a1 = bi.job_role }
            //                 into bi1
            //                 from bi2 in bi1.DefaultIfEmpty()
            //                 join bh in db.GB_999_MSG
            //                 on new { a1 = bg.costing_basis } equals new { a1 = bh.code_msg }
            //                 into bh1
            //                 from bh2 in bh1.DefaultIfEmpty()
            //                 where bg.inactive_status == "N" && bg.job_id == Convert.ToInt32(id) && bh2.type_msg == "cost"
            //                 select new { bi2, bg, bh2 }).FirstOrDefault();

            //if (bgassign3 != null)
            //{
            //    if (bgassign3.bi2 == null)
            //        ary2.Add(new SelectListItem { Value = "", Text = "" });
            //    else
            //        ary2.Add(new SelectListItem { Value = bgassign3.bi2.employee_code, Text = bgassign3.bi2.name });
            //    ary2.Add(new SelectListItem { Value = bgassign3.bg.job_id.ToString(), Text = bgassign3.bg.cost.ToString() });
            //    ary2.Add(new SelectListItem { Value = bgassign3.bh2.code_msg, Text = bgassign3.bh2.name1_msg });

            //}
           
            var bgassign4 = (from bg in db.GB_001_EMP
                             join bi in db.JB_001_JOB
                             on new { a1 = bg.job_role} equals new { a1 = bi.job_id}
                             into bi1
                             from bi2 in bi1.DefaultIfEmpty()
                             join bh in db.GB_999_MSG
                            on new { a1 = bi2.costing_basis, a2 = "cost" } equals new { a1 = bh.code_msg, a2 = bh.type_msg }
                            into bh1
                             from bh2 in bh1.DefaultIfEmpty()
                             where bg.employee_code == id
                             select new { bi2, bg, bh2 }).FirstOrDefault();

            if (bgassign4 != null)
            {
                    ary2.Add(new SelectListItem { Value = "emp", Text = bgassign4.bh2.name1_msg });

            }
            var bgassign2 = (from bg in db.IV_001_ITEM
                             join bi in db.GB_001_PCODE
                             on new { a1 = bg.sku_sequence } equals new { a1 = bi.parameter_code }
                             into bi1
                             from bi2 in bi1.DefaultIfEmpty()
                             where bg.item_code == id
                             select new { bg, bi2 }).FirstOrDefault();
            if (bgassign2 != null)
            {
                string price = bgassign2.bg.selling_price_class1.ToString();
                string uomm = bgassign2.bi2.parameter_name;
                ary2.Add(new SelectListItem { Value = "pricee", Text = price });
                ary2.Add(new SelectListItem { Value = "uom", Text = uomm });

            }

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(ary2.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");

        }

        public ActionResult asset(string id)
        {
            psess = (psess)Session["psess"];
            List<SelectListItem> ary2 = new List<SelectListItem>();

            var bgassign3 = (from bg in db.SC_001_SCM
                             join bi in db.AP_001_VENDR
                             on new { a1 = bg.vendor_id } equals new { a1 = bi.vendor_code }
                             into bi1
                             from bi2 in bi1.DefaultIfEmpty()
                             where bg.inactive_status == "N" && bg.sub_contract_id == id
                             select new { bg, bi2 }).FirstOrDefault();
            var bgassign4 = (from bg in db.AG_001_AMG
                             join bi in db.GB_999_MSG
                             on new { a1 = bg.nature } equals new { a1 = bi.code_msg }
                             into bi1
                             from bi2 in bi1.DefaultIfEmpty()
                             where bi2.type_msg == "nat" && bg.maintenance_group_type_id == id
                             select new { bg, bi2 }).FirstOrDefault();

            if (bgassign3 != null)
            {
                decimal subb = bgassign3.bg.total_cost;
                string subdesc = bgassign3.bg.description;
                string subvend = bgassign3.bi2.vend_biz_name;
                ary2.Add(new SelectListItem { Value = "subcon", Text = subb.ToString() });
                ary2.Add(new SelectListItem { Value = "subven", Text = subvend });
                ary2.Add(new SelectListItem { Value = "subdescc", Text = subdesc });

            }

            if (bgassign4 != null)
            {
                string naturee = bgassign4.bi2.name1_msg;
                ary2.Add(new SelectListItem { Value = "nature", Text = naturee });
                string glcode = bgassign4.bg.gl_account;
                ary2.Add(new SelectListItem { Value = "glaccc", Text = glcode });
            }
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(ary2.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");

        }
        public ActionResult asset2(string id)
        {
            psess = (psess)Session["psess"];
            List<SelectListItem> ary2 = new List<SelectListItem>();

            var bgassign3 = (from bg in db.FA_001_ASSET
                             where bg.active_status == "N"
                             select new { c1 = bg.fixed_asset_code, c2 = bg.description });
            var bgassign4 = (from bg in db.AG_001_ASG
                             where bg.inactive_status == "N"
                             select new { c1 = bg.asset_grouping_id, c2 = bg.description });
            if (id == "F")
            {
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(bgassign3.ToList(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
            }

            if (id == "G")
            {

                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(bgassign4.ToList(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);

            }
            return RedirectToAction("Index");

        }

        private void assign2()
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
            string temp11 = glay.vwstring11;
            string temp12 = glay.vwstring10;
            string[] sst = new string[glay.vwstrarray0.Length];
            glay.vwstrarray0.CopyTo(sst, 0);
            string temp10 = glay.vwstrarray2[0];
            st = new string[glay.vwstrarray14.Length];
            glay.vwstrarray14.CopyTo(st, 0);
            rt = new int[glay.vwitarray2.Length];
            int int1 = glay.vwint6;
            int calc = glay.vwint8;
            int tabc = glay.vwint7;
            int tabd = glay.vwint2;
            int datf = glay.vwint4;
            int datone = glay.vwint10;
            glay.vwitarray2.CopyTo(rt, 0);
            string[] secst = new string[glay.vwstrarray2.Length];
            glay.vwstrarray2.CopyTo(secst, 0);
            string[] emps = new string[glay.vwstrarray4.Length];
            glay.vwstrarray4.CopyTo(emps, 0);

            Decimal[] decc = new decimal[glay.vwdclarray4.Length];
            glay.vwdclarray4.CopyTo(decc, 0);
            DateTime[] date1 = new DateTime[glay.vwdtarray0.Length];
            glay.vwdtarray0.CopyTo(date1, 0);

            DateTime[] date2 = new DateTime[glay.vwdtarray1.Length];
            glay.vwdtarray1.CopyTo(date2, 0);


            //initial_rtn();
            for (int i = 0; i < 12; i++)
            {
                ModelState.Remove("vwstring" + i);
                ModelState.Remove("vwdclarray1[" + i + "]");
                ModelState.Remove("vwdtarray0[" + i + "]");
                ModelState.Remove("vwdtarray1[" + i + "]");
                ModelState.Remove("vwstrarray6[" + i + "]");
                ModelState.Remove("vwdclarray0[" + i + "]");
                ModelState.Remove("vwdclarray3[" + i + "]");
                ModelState.Remove("vwdclarray2[" + i + "]");
                ModelState.Remove("vwdclarray4[" + i + "]");
                ModelState.Remove("vwstrarray7[" + i + "]");
                ModelState.Remove("vwstrarray2[" + i + "]");
                ModelState.Remove("vwstrarray4[" + i + "]");
            }
            ModelState.Remove("vwstrarray0");
            ModelState.Remove("vwint10");
            ModelState.Remove("vwstrarray14");
            ModelState.Remove("vwitarray2");
            ModelState.Remove("vwint6");
            ModelState.Remove("vwint1");
            ModelState.Remove("vwint3");
            ModelState.Remove("vwint4");
            ModelState.Remove("vwint2");
            ModelState.Remove("vwint5");
            ModelState.Remove("vwdecimal0");
            ModelState.Remove("vwint8");
            ModelState.Remove("vwint7");


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
            glay.vwstring10 = temp12;
            glay.vwstring11 = temp11;
            glay.vwstrarray2[0] = temp10;
            secst.CopyTo(glay.vwstrarray2, 0);
            sst.CopyTo(glay.vwstrarray0, 0);
            st.CopyTo(glay.vwstrarray14, 0);
            decc.CopyTo(glay.vwdclarray4, 0);
            emps.CopyTo(glay.vwstrarray4, 0);
            date1.CopyTo(glay.vwdtarray0, 0);
            date2.CopyTo(glay.vwdtarray1, 0);
            glay.vwint8 = calc;
            glay.vwint6 = int1;
            glay.vwint7 = tabc;
            glay.vwint2 = tabd;
            glay.vwint4 = datf;
            glay.vwint10 = datone;
            string get_id = "";
            string get_taskid = "";



            //get items and jobs from item and pcode table respectively 
            var bgrunlist = (from bg in db.FA_001_ASSET
                             join bh in db.AG_001_AMG
                             on new { a1 = bg.group_type_id } equals new { a1 = bh.maintenance_group_type_id }
                             into bh1
                             from bh2 in bh1.DefaultIfEmpty()
                             join bk in db.GB_999_MSG
                             on new { a1 = bh2.nature } equals new { a1 = bk.code_msg }
                             into bk1
                             from bk2 in bk1.DefaultIfEmpty()
                             where bk2.type_msg == "nat" && bg.fixed_asset_code == temp9
                             select new { bg, bh2, bk2 }).FirstOrDefault();
            if (bgrunlist != null)
            {
                glay.vwstring6 = bgrunlist.bg.asset_location;
                glay.vwstring10 = bgrunlist.bg.asset_user;
                glay.vwstring3 = bgrunlist.bh2.description;
                glay.vwstring11 = bgrunlist.bk2.name1_msg;
                get_id = bgrunlist.bg.group_type_id;
                get_taskid = bgrunlist.bh2.task_descriptioin_id;
                glay.vwstring8 = bgrunlist.bg.description;
                glay.vwint1 = 1;
            }

            var bgrunlistt = (from bg in db.AG_001_ASG
                              join bh in db.AG_001_AMG
                              on new { a1 = bg.maintenance_type_id } equals new { a1 = bh.maintenance_group_type_id }
                              into bh1
                              from bh2 in bh1.DefaultIfEmpty()
                              join bk in db.GB_999_MSG
                              on new { a1 = bh2.nature } equals new { a1 = bk.code_msg }
                              into bk1
                              from bk2 in bk1.DefaultIfEmpty()
                              where bk2.type_msg == "nat" && bg.asset_grouping_id == temp9
                              select new { bg, bh2, bk2 }).FirstOrDefault();
            if (bgrunlistt != null)
            {
                glay.vwstring3 = bgrunlistt.bh2.description;
                glay.vwstring11 = bgrunlistt.bk2.name1_msg;
                get_id = bgrunlistt.bg.maintenance_type_id;
                get_taskid = bgrunlistt.bh2.task_descriptioin_id;
                glay.vwstring8 = bgrunlistt.bg.description;
            }

            string query = "select de.parameter_name vwstring8, pc.job_title vwstring0, ass.identity_code vwstring1, ass.sequence_no vwstring2,  ass.flagg vwstring3, ass.qtyreq vwint7,";
            query += " ass.total vwdecimal1, CONVERT(varchar(10), it.item_name) vwstring7, ass.head_count vwint1";
            query += " from AG_002_ASSET ass left outer join AG_001_AMG am";
            query += " on am.maintenance_group_type_id = ass.maintenance_group_type_id";
            query += " left outer join dbo.IV_001_ITEM it on ass.identity_code = it.item_code";
            query += " left outer join JB_001_JOB pc on ass.identity_code = 'pc.job_id'";
            query += " left outer join GB_001_PCODE de on it.sku_sequence = de.parameter_code";
            query += " where am.maintenance_group_type_id =" + util.sqlquote(get_id) + " order by flagg";
            var sqllite = db.Database.SqlQuery<vw_genlay>(query).ToList();


            if (glay.vwint8 == 1)
            {
                while (!(string.IsNullOrWhiteSpace(glay.vwstrarray1[counter])))
                {

                    me = glay.vwstrarray1[counter];
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
                        ViewData["extcostt" + counter] = glay.vwitarray2[counter] * glay.vwdclarray1[counter];
                        TempData["cmeasure" + counter] = bgassignc.bh2.parameter_name;
                        count.Add(glay.vwstrarray1[counter]);
                    }

                    deccalcc += glay.vwitarray2[counter] * glay.vwdclarray1[counter];
                    glay.vwdclarray3[0] = deccalcc;
                    counter++;


                }
                while (!(string.IsNullOrWhiteSpace(glay.vwstrarray5[counter1])))
                {

                    int jab = Convert.ToInt16(glay.vwstrarray5[counter1]);
                    me = glay.vwstrarray5[counter1];
                    var bgassignc = (from bg in db.JB_001_JOB
                                     join bh in db.GB_999_MSG
                                     on new { a1 = bg.costing_basis,  a2= "cost" } equals new { a1 = bh.code_msg, a2= bh.type_msg }
                                     into bh1
                                     from bh2 in bh1.DefaultIfEmpty()
                                     where  bg.job_id == jab
                                     select new { bg, bh2 }).FirstOrDefault();
                    if (bgassignc != null)
                    {
                        int empp = Convert.ToInt32(glay.vwstrarray5[counter1]);
                        var bgemp = from bg in db.GB_001_EMP
                                    where bg.active_status == "N" && bg.job_role == empp
                                    select new { c1 = bg.employee_code, c2 = bg.name };

                        ViewBag.emp = new SelectList(bgemp.ToList(), "c1", "c2");
                        ViewData["costphead" + counter1] = bgassignc.bg.cost;
                        glay.vwdclarray4[counter1] = glay.vwitarray0[counter1] * bgassignc.bg.cost;
                           deccalcc1 += glay.vwitarray0[counter1] * bgassignc.bg.cost;
                            glay.vwdclarray3[1] = deccalcc1;
                        if (bgassignc.bh2 != null)
                        glay.vwstrarray4[counter1] = bgassignc.bh2.name1_msg;

                        string emppval = glay.vwstrarray14[counter1];
                        var bgassign4 = (from bg in db.GB_001_EMP
                                         join bi in db.JB_001_JOB
                                         on new { a1 = bg.job_role } equals new { a1 = bi.job_id }
                                         into bi1
                                         from bi2 in bi1.DefaultIfEmpty()
                                         join bh in db.GB_999_MSG
                                        on new { a1 = bi2.costing_basis, a2 = "cost" } equals new { a1 = bh.code_msg, a2 = bh.type_msg }
                                        into bh1
                                         from bh2 in bh1.DefaultIfEmpty()
                                         where bg.employee_code == emppval
                                         select new { bi2, bg, bh2 }).FirstOrDefault();
                        if (bgassign4 != null)
                            glay.vwstrarray4[counter1] = bgassign4.bh2.name1_msg;
                        //if (glay.vwstrarray4[counter1] == "D")
                        //{
                        //    glay.vwdclarray4[counter1] = glay.vwitarray0[counter1] * bgassignc.bg.cost;
                        //    deccalcc1 += glay.vwitarray0[counter1] * bgassignc.bg.cost;
                        //    glay.vwdclarray3[1] = deccalcc1;
                        //}
                        //else if (glay.vwstrarray4[counter1] == "H")
                        //{
                        //    double hour = 0.041667;
                        //    glay.vwdclarray4[counter1] = Math.Round((glay.vwitarray0[counter1] * bgassignc.bg.cost) * Convert.ToDecimal(hour), 2);
                        //    deccalcc1 += glay.vwitarray0[counter1] * bgassignc.bg.cost * Convert.ToDecimal(hour);
                        //    glay.vwdclarray3[1] = Math.Round(deccalcc1, 2);
                        //}
                        //else if (glay.vwstrarray4[counter1] == "M")
                        //{
                        //    double hour = 0.016667;
                        //    glay.vwdclarray4[counter1] = Math.Round((glay.vwitarray0[counter1] * bgassignc.bg.cost) * Convert.ToDecimal(hour), 2);
                        //    deccalcc1 += glay.vwitarray0[counter1] * bgassignc.bg.cost * Convert.ToDecimal(hour);
                        //    glay.vwdclarray3[1] = Math.Round(deccalcc1, 2);
                        //}
                        count1.Add(glay.vwstrarray5[counter1]);

                    }
                    counter1++;

                }
                while (!(string.IsNullOrWhiteSpace(glay.vwstrarray15[counter3])))
                {

                    me = glay.vwstrarray15[counter3];
                    count3.Add(glay.vwstrarray15[counter3]);
                    var bgassignme = (from bg in db.SC_001_SCM
                                      join bh in db.AP_001_VENDR
                                      on new { a1 = bg.vendor_id } equals new { a1 = bh.vendor_code }
                                      into bh1
                                 where bg.sub_contract_id == me
                                             from bh2 in bh1.DefaultIfEmpty()
                                    select new { bg, bh2 }).FirstOrDefault();
                    if (bgassignme != null)
                    {
                        glay.vwdclarray0[counter3] = bgassignme.bg.total_cost;
                        glay.vwstrarray6[counter3] = bgassignme.bg.description;
                    }

                    deccalcc2 += glay.vwdclarray0[counter3];
                    glay.vwdclarray3[2] = deccalcc2;
                    counter3++;
                }
                int precount = 0;
                int countdate = 1;
                for (int x = 0; x < glay.vwitarray3.Length; x++)
                {
                        if (glay.vwitarray3[x] != 0)
                    {
                        precount =precount+1;
                        
                    }
                }

                 while (counter2 <= precount) 
                    {
                        //int index =  Array.IndexOf(glay.vwitarray1, counter2+1);
                    int[] index = glay.vwitarray1.Select((b, i) => b == counter2+1 ? i : -1).Where(i => i != -1).ToArray();
                    foreach(var item in index)
                    {
                        count2.Add(glay.vwstrarray7[counter2].ToString());
                        if (glay.vwdate0 != null)
                        {

                            if (glay.vwitarray1[item] == 1)
                            {
                                glay.vwdtarray1[item] = glay.vwdate0;
                                DateTime q_date = glay.vwdtarray1[item];
                                dt = q_date.AddDays(glay.vwitarray3[item]);
                                glay.vwdtarray0[item] = dt;
                            }
                            else
                            {
                                
                                int[] dateindex = glay.vwitarray1.Select((b, i) => b == item-countdate ? i : -1).Where(i => i != -1).ToArray();
                                List<DateTime> getdate = new List<DateTime>();
                                foreach(var dateitem in dateindex)
                                {
                                    getdate.Add(glay.vwdtarray0[dateitem]);
                                }
                                DateTime latest = getdate.Max(record => record.Date);
                                glay.vwdtarray1[item] = latest;
                                DateTime q_date = glay.vwdtarray1[item];
                                dt = q_date.AddDays(glay.vwitarray3[item]);
                                glay.vwdtarray0[item] = dt;
                                countdate++;
                            }
                        }
                  
                    }
                    DateTime datelate = glay.vwdtarray0.Max(record => record.Date);
                    DateTime dateearly = glay.vwdtarray1.Min(record => record.Date);
                    itcal = (datelate - dateearly).Days;
                    glay.vwint10 = itcal;
                    counter2++;
                }

               

                while (!(glay.vwdclarray2[counter4].Equals(0)))
                {
                    deccalcc4 += glay.vwdclarray2[counter4];
                    glay.vwdclarray3[4] = deccalcc4;
                    counter4++;
                }
            }

            else
            {

                    foreach (var item in sqllite)
                    {
                        count2.Add(item.vwstring1);
                        if (item.vwstring3 == "K")
                        {
                            //glay.vwstrarray7[counter2] = item.task_id;
                            //glay.vwitarray3[counter2] = tem.tk.estimated_no_of_hrs;
                            //glay.vwstrarray11[counter2] = tem.bk2.technical_competency_level_id;
                            //ViewData["task" + counter2] = tem.bk2.description;
                            //itcal += glay.vwitarray3[counter2];
                            //glay.vwint10 =itcal;
                            //DateTime q_date = glay.vwdtarray1[counter2];
                            //dt = q_date.AddDays(glay.vwitarray3[counter2]);
                            //me = dt.ToString("dd/MM/yyy");
                            //glay.vwdtarray0[counter2] = util.date_convert(me);
                            //counter2++;

                    }
                }

                foreach (var item in sqllite)
                {
                    if (item.vwstring3 == "I")
                    {
                        count.Add(item.vwstring1);
                        glay.vwitarray2[counter] = item.vwint7;
                        var bgassignc = (from bg in db.IV_001_ITEM
                                         join bh in db.GB_001_PCODE
                                         on new { a1 = bg.sku_sequence } equals new { a1 = bh.parameter_code }
                                         into bh1
                                         from bh2 in bh1.DefaultIfEmpty()
                                         where bg.item_code == item.vwstring1 && bh2.parameter_type == "10"
                                         select new { bg, bh2 }).FirstOrDefault();
                        glay.vwdclarray1[counter] = bgassignc.bg.selling_price_class1;
                        glay.vwstrarray1[counter] = item.vwstring1;
                        ViewData["extcostt" + counter] = item.vwdecimal1;
                        ViewData["measure" + counter] = item.vwstring8;
                        TempData["cmeasure" + counter] = bgassignc.bh2.parameter_name;
                        deccalcc += item.vwdecimal1;
                        glay.vwdclarray3[0] = deccalcc;
                        counter++;
                    }

                    else if (item.vwstring3 == "J")
                    {
                        count1.Add(item.vwstring1);
                        glay.vwstrarray5[counter1] = item.vwstring1;
                        glay.vwitarray0[counter1] = item.vwint7;
                        int jab = Convert.ToInt32(item.vwstring1);
                        var bgemp = from bg in db.GB_001_EMP
                                    where bg.active_status == "N" && bg.job_role == jab
                                    select new { c1 = bg.employee_code, c2 = bg.name };

                        ViewBag.emp = new SelectList(bgemp.ToList(), "c1", "c2");
                        var bgassignc = (from bg in db.JB_001_JOB
                                         where bg.job_id == jab
                                         select bg).FirstOrDefault();
                        ViewData["headcount" + counter1] = item.vwint1;
                        ViewData["costphead" + counter1] = bgassignc.cost;
                        glay.vwdclarray4[counter1] = item.vwint1 * bgassignc.cost;
                        deccalcc1 += item.vwint1 * bgassignc.cost;
                        glay.vwdclarray3[1] = deccalcc1;
                        counter1++;

                    }

                    else if (item.vwstring3 == "L")
                    {

                        count3.Add(item.vwstring1);
                        glay.vwstrarray15[counter3] = item.vwstring1;
                        string subval = glay.vwstrarray15[counter3];
                        var bgassignme = (from bg in db.SC_001_SCM
                                          join bh in db.AP_001_VENDR
                                          on new { a1 = bg.vendor_id } equals new { a1 = bh.vendor_code }
                                          into bh1
                                          from bh2 in bh1.DefaultIfEmpty()
                                          where bg.sub_contract_id == item.vwstring1
                                          select new { bg, bh2 }).FirstOrDefault();
                        glay.vwdclarray0[counter3] = bgassignme.bg.total_cost;
                        glay.vwstrarray6[counter3] = bgassignme.bg.description;
                        deccalcc2 += glay.vwdclarray0[counter3];
                        glay.vwdclarray3[2] = deccalcc2;
                        counter3++;
                    }



                }
            }
            if (glay.vwint6 != 2)
            {
                if (count.Count != 0)
                    glay.vwint1 = count.Count;
            }

            if (count1.Count != 0)
                glay.vwint5 = count1.Count;
            if (count2.Count != 0)
                glay.vwint3 = count2.Count;
            if (count3.Count != 0)
                glay.vwint4 = count3.Count;
            glay.vwdecimal0 = Math.Round(deccalcc + deccalcc1 + deccalcc2 + deccalcc3 + deccalcc4 + itcal, 2);

            if (glay.vwint7 == 1)
            {
                glay.vwstrarray2[5] = "tab-pane active";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[6] = "tab-pane";
                glay.vwstrarray2[7] = "tab-pane";
                glay.vwstrarray2[8] = "tab-pane";
                glay.vwstrarray2[9] = "tab-pane";
            }
            else if (glay.vwint7 == 2)
            {
                glay.vwstrarray2[6] = "tab-pane active";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[5] = "tab-pane";
                glay.vwstrarray2[7] = "tab-pane";
                glay.vwstrarray2[8] = "tab-pane";
                glay.vwstrarray2[9] = "tab-pane";
            }
            else if (glay.vwint7 == 3)
            {
                glay.vwstrarray2[7] = "tab-pane active";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[6] = "tab-pane";
                glay.vwstrarray2[5] = "tab-pane";
                glay.vwstrarray2[8] = "tab-pane";
                glay.vwstrarray2[9] = "tab-pane";

            }
            else if (glay.vwint7 == 4)
            {
                glay.vwstrarray2[8] = "tab-pane active";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[6] = "tab-pane";
                glay.vwstrarray2[5] = "tab-pane";
                glay.vwstrarray2[7] = "tab-pane";
                glay.vwstrarray2[9] = "tab-pane";
            }
            else if (glay.vwint7 == 5)
            {
                glay.vwstrarray2[9] = "tab-pane active";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[6] = "tab-pane";
                glay.vwstrarray2[5] = "tab-pane";
                glay.vwstrarray2[7] = "tab-pane";
                glay.vwstrarray2[8] = "tab-pane";

            }
        }
        private void validation_routine()
        {

            string error_msg = "";

            bool alert = false;
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;

            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please Specify WorkOrder  ID");
                err_flag = false;
            }



            if (string.IsNullOrWhiteSpace(glay.vwstrarray2[0]))
            {
                ModelState.AddModelError(String.Empty, "Please Specify Work Order Description");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring7))
            {
                ModelState.AddModelError(String.Empty, "Please Select GL Account");
                err_flag = false;
            }

            int find_itemcode = 0;
            for (int i = 0; i < glay.vwstrarray1.Length; i++)
            {
                string def = glay.vwstrarray1[i];

                for (int j = 0; j < glay.vwstrarray1.Length; j++)
                {
                    if (!(string.IsNullOrWhiteSpace(glay.vwstrarray1[j])))
                    {

                        if (def.Equals(glay.vwstrarray1[j]))
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
                WO_002_WKO asst = db.WO_002_WKO.Find(glay.vwstring0, glay.vwstring9);
                if (asst != null)
                {
                    ModelState.AddModelError(String.Empty, "Work Order ID Exists");
                    err_flag = false;
                }
                var bgassign = (from bg in db.WO_002_WKO
                                select bg.work_order_description).ToList();
                foreach (var item in bgassign)
                {
                    if (glay.vwstring9 == item)
                    {
                        ModelState.AddModelError(String.Empty, "Work Order Description Exists");
                        err_flag = false;
                        break;
                    }
                }
                var bgassign2 = (from bg in db.WO_002_WKODT
                                 where bg.flag == "I"
                                 select bg.identity_code).ToList();
                foreach (var tem in bgassign)
                {
                    for (int i = 0; i < glay.vwstrarray1.Length; i++)
                    {
                        if (glay.vwstrarray1[i] == tem)
                        {
                            ModelState.AddModelError(String.Empty, "Item Code already Exists");
                            err_flag = false;
                            break;
                        }
                    }

                }
                var bgassign3 = (from bg in db.WO_002_WKODT
                                 where bg.flag == "J"
                                 select bg.identity_code).ToList();
                foreach (var tem in bgassign)
                {

                    if (glay.vwstring9 == tem)
                    {
                        ModelState.AddModelError(String.Empty, "Job Title already Exists");
                        err_flag = false;
                        break;
                    }
                }
            }
        }

        private void read_record()
        {
            if (WO_002_WKO.maintenance_id != null)
            {
                glay.vwint6 = 1;
                var bgrunlistt = (from bg in db.AG_001_ASG
                                  join bh in db.AG_001_AMG
                                  on new { a1 = bg.maintenance_type_id } equals new { a1 = bh.maintenance_group_type_id }
                                  into bh1
                                  from bh2 in bh1.DefaultIfEmpty()
                                  join bk in db.GB_999_MSG
                                  on new { a1 = bh2.nature } equals new { a1 = bk.code_msg }
                                  into bk1
                                  from bk2 in bk1.DefaultIfEmpty()
                                  where bk2.type_msg == "nat" && bg.asset_grouping_id == glay.vwstring9
                                  select new { bg, bh2, bk2 }).FirstOrDefault();
                if (bgrunlistt != null)
                {
                    glay.vwstring3 = bgrunlistt.bh2.description;
                    glay.vwstring11 = bgrunlistt.bk2.name1_msg;
                    glay.vwstring8 = bgrunlistt.bg.description;
                }
                var repairs = (from bg in db.MN_002_MNT
                               join bh in db.GB_999_MSG
                               on new {a1  = bg.activation_type, a2= "act"} equals new {a1 = bh.code_msg, a2 = bh.type_msg}
                               into bh1 
                               from bh2 in bh1.DefaultIfEmpty()
                               where bg.fixed_asset_code == glay.vwstring9
                               select bh2).FirstOrDefault();
                if(repairs !=null)
                glay.vwstring4 = repairs.name1_msg;
                var bgrunlist = (from bg in db.FA_001_ASSET
                                 join bh in db.AG_001_AMG
                                 on new { a1 = bg.group_type_id } equals new { a1 = bh.maintenance_group_type_id }
                                 into bh1
                                 from bh2 in bh1.DefaultIfEmpty()
                                 join bk in db.GB_999_MSG
                                 on new { a1 = bh2.nature , a2 =  "nat" } equals new { a1 = bk.code_msg, a2= bk.type_msg }
                                 into bk1
                                 from bk2 in bk1.DefaultIfEmpty()
                                 where bg.fixed_asset_code == WO_002_WKO.asset_or_group
                                 select new { bg, bh2, bk2 }).FirstOrDefault();
                if (bgrunlist != null)
                {
                    glay.vwstring6 = bgrunlist.bg.asset_location;
                    glay.vwstring10 = bgrunlist.bg.asset_user;
                    if(bgrunlist.bh2!=null)
                    glay.vwstring3 = bgrunlist.bh2.description;
                    if (bgrunlist.bk2 != null)
                        glay.vwstring11 = bgrunlist.bk2.name1_msg;
                    glay.vwstring8 = bgrunlist.bg.description;
                    glay.vwint1 = 1;
                }

            }


            glay.vwstring0 = WO_002_WKO.work_order_id;
            glay.vwstrarray2[0] = WO_002_WKO.work_order_description;
            var bgstat = (from bg in db.WO_002_WKO
                          join bh in db.GB_999_MSG
                          on new { a1 = bg.status } equals new { a1 = bh.code_msg }
                          into bh1
                          from bh2 in bh1.DefaultIfEmpty()
                          where bh2.type_msg == "stat" && bg.work_order_id == glay.vwstring0
                          select new { bg, bh2 }).FirstOrDefault();
            if (bgstat != null)
            {
                glay.vwstrarray2[11] = bgstat.bh2.name1_msg;
                glay.vwstrarray2[14] = bgstat.bh2.code_msg;
            }
            glay.vwstrarray2[3] = WO_002_WKO.job_card_id;
            glay.vwdecimal0 = WO_002_WKO.estimated_total_cost;
            glay.vwstrarray2[1] = WO_002_WKO.estimated_start_date_time;
            glay.vwstrarray2[2] = WO_002_WKO.estimated_end_date_time;
            glay.vwstring9 = WO_002_WKO.asset_or_group;
            //glay.vwstring2 = WO_002_WKO.work_order_date;
            glay.vwstring1 = WO_002_WKO.work_center_id;
            glay.vwstring7 = WO_002_WKO.gl_account;
            glay.vwstring5 = WO_002_WKO.team_lead;
            glay.vwdclarray3[0] = WO_002_WKO.total_materials_cost;
            glay.vwdclarray3[1] = WO_002_WKO.total_hr_cost;
            glay.vwint10 = WO_002_WKO.total_duration;
            glay.vwdclarray3[2] = WO_002_WKO.total_contract_amount;
            glay.vwdclarray3[4] = WO_002_WKO.total_misc_amount;
            glay.vwstrarray2[10] = WO_002_WKO.cvt_to_wrk_ord;
            glay.vwstrarray2[15] = WO_002_WKO.flag;

            int counter = 0;
            string get_id = glay.vwstring0;
            var bgassign1 = (from bg in db.WO_002_WKODT
                             join bh in db.WO_002_WKO
                             on new { a1 = bg.work_order_id } equals new { a1 = bh.work_order_id }
                             into bh1
                             from bh2 in bh1.DefaultIfEmpty()
                             where bg.work_order_id == get_id
                             orderby bg.flag, bg.sequence_no
                             select new { bg, bh2 }).ToList();
            foreach (var item in bgassign1)
            {
                if (item.bg.flag == "I")
                {
                    count.Add(item.bg.sequence_no);
                    me = item.bg.identity_code;
                    glay.vwstrarray1[counter] = me;
                    ViewData["extcostt" + counter] = item.bg.total;
                    glay.vwitarray2[counter] = item.bg.qty;
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
                        TempData["cmeasure" + counter] = bgassignc.bh2.parameter_name;
                        ViewData["extcostt" + counter] = bgassignc.bg.selling_price_class1 * item.bg.qty;
                    }
                    counter++;
                }
                if (item.bg.flag == "J")
                {
                    count1.Add(item.bg.sequence_no);
                    int jab = Convert.ToInt32(item.bg.identity_code);
                    me = item.bg.identity_code;
                    glay.vwstrarray5[counter1] = me;
                    glay.vwstrarray14[counter1] = item.bg.description;
                    TempData["woest"+counter1] = item.bg.qty;
                    var bgassignc = (from bg in db.JB_001_JOB
                                     join bh in db.GB_999_MSG
                                     on new { a1 = bg.costing_basis } equals new { a1 = bh.code_msg }
                                     into bh1
                                     from bh2 in bh1.DefaultIfEmpty()
                                     join bj in db.GB_001_EMP
                                    on new { a1 = bg.job_id } equals new { a1 = bj.job_role }
                                    into bj1
                                     from bj2 in bj1.DefaultIfEmpty()
                                     where bh2.type_msg == "cost" && bg.job_id == jab
                                     select new { bg, bh2, bj2 }).FirstOrDefault();
                    if (bgassignc != null)
                    {
                        
                        int empp = Convert.ToInt32(item.bg.identity_code);
                        var bgemp = from bg in db.GB_001_EMP
                                    where bg.active_status == "N" && bg.job_role == empp
                                    select new { c1 = bg.employee_code, c2 = bg.name };

                        ViewBag.emp = new SelectList(bgemp.ToList(), "c1", "c2");
                        ViewData["costphead" + counter1] = bgassignc.bg.cost;
                        glay.vwdclarray4[counter1] = item.bg.total;
                    }
                    counter1++;

                }

                if (item.bg.flag == "K")
                {
                    count2.Add(item.bg.sequence_no);
                    glay.vwstrarray3[counter3] = item.bg.identity_code;
                    glay.vwitarray1[counter3] = Convert.ToInt32(item.bg.cost);
                    glay.vwitarray3[counter3] = item.bg.qty;
                    glay.vwstrarray7[counter3] = item.bg.description;
                    glay.vwstrarray11[counter3] = item.bg.time;
                    glay.vwdtarray1[counter3] = item.bg.start_date;
                    glay.vwdtarray0[counter3] = item.bg.end_date;
                    counter2++;
                }

                if (item.bg.flag == "L")
                {
                    me = item.bg.identity_code;
                    count3.Add(item.bg.sequence_no);
                    glay.vwstrarray15[counter3] = item.bg.identity_code;
                    glay.vwdclarray0[counter3] = item.bg.total;
                    var bgassignme = (from bg in db.SC_001_SCM
                                      join bh in db.AP_001_VENDR
                                      on new { a1 = bg.vendor_id } equals new { a1 = bh.vendor_code }
                                      into bh1
                                      from bh2 in bh1.DefaultIfEmpty()
                                      where bg.sub_contract_id == me
                                      select new { bg, bh2 }).FirstOrDefault();
                    if (bgassignme != null)
                    {
                        glay.vwstrarray6[counter3] = bgassignme.bg.description;
                    }
                    counter3++;
                }


                if (item.bg.flag == "M")
                {
                    count4.Add(item.bg.sequence_no);
                    glay.vwstrarray0[counter4] = item.bg.description;
                    glay.vwdclarray2[counter4] = item.bg.total;
                    counter4++;
                }
            }
            if(count.Count != 0)
            glay.vwint1 = count.Count;
            if (count1.Count != 0)
                glay.vwint5 = count1.Count;
            if (count2.Count != 0)
                glay.vwint3 = count2.Count;
            if (count3.Count != 0)
                glay.vwint4 = count3.Count;
        }
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwstrarray1 = new string[50];
            glay.vwstrarray2 = new string[50];
            glay.vwstrarray3 = new string[50];
            glay.vwstrarray4 = new string[50];
            glay.vwstrarray5 = new string[50];
            glay.vwstrarray6 = new string[50];
            glay.vwstrarray7 = new string[50];
            glay.vwstrarray8 = new string[50];
            glay.vwstrarray9 = new string[50];
            glay.vwstrarray10 = new string[50];
            glay.vwstrarray11 = new string[50];
            glay.vwstrarray12 = new string[50];
            glay.vwstrarray13 = new string[50];
            glay.vwstrarray14 = new string[50];
            glay.vwstrarray15 = new string[50];
            glay.vwstrarray16 = new string[50];
            glay.vwdclarray0 = new decimal[50];
            glay.vwdclarray1 = new decimal[50];
            glay.vwdclarray2 = new decimal[50];
            glay.vwdclarray3 = new decimal[50];
            glay.vwdclarray4 = new decimal[50];
            glay.vwblarray0 = new bool[10];
            glay.vwitarray0 = new int[50];
            glay.vwitarray1 = new int[50];
            glay.vwitarray2 = new int[50];
            glay.vwitarray3 = new int[50];
            glay.vwdtarray0 = new DateTime[50];
            glay.vwdtarray1 = new DateTime[50];
            glay.vwdate0 = DateTime.UtcNow;
            glay.vwstring2 = DateTime.UtcNow.ToString("dd/MM/yyyy");
            glay.vwint1 = 1;
            glay.vwint5 = 1;
            glay.vwint3 = 1;
            glay.vwint4 = 1;
            glay.vwint2 = 1;
            glay.vwstrarray2[0] = "";
            glay.vwstrarray2[1] = "";
            glay.vwstrarray2[2] = "";
            glay.vwstrarray2[3] = "";
            glay.vwstrarray2[4] = "tab-pane active";
            glay.vwstrarray2[5] = "tab-pane";
            glay.vwstrarray2[6] = "tab-pane";
            glay.vwstrarray2[7] = "tab-pane";
            glay.vwstrarray2[8] = "tab-pane";
            glay.vwstrarray2[9] = "tab-pane";
            glay.vwstrarray2[10] = "N";
            glay.vwstrarray2[11] = "";
            glay.vwstrarray2[13] = "";
            glay.vwstrarray2[12] = "F";
            glay.vwstrarray2[14] = "A";
            glay.vwstrarray2[15] = "";
            for (int i = 0; i < 50; i++)
            {
                glay.vwstrarray0[i] = "";
                glay.vwstrarray1 [i]=  "";
                //glay.vwstrarray2 [i]=  "";
                glay.vwstrarray3 [i]=  "";
                glay.vwstrarray4 [i]=  "";
                glay.vwstrarray5 [i]=  "";
                glay.vwstrarray6 [i]=  "";
                glay.vwstrarray7 [i]=  "";
                glay.vwstrarray8 [i]=  "";
                glay.vwstrarray9 [i]=  "";
                glay.vwstrarray10 [i]=  "";
                glay.vwstrarray11 [i]=  "";
                glay.vwstrarray12 [i]=  "";
                glay.vwitarray1[i] = 0;
                glay.vwstrarray13 [i]=  "";
                glay.vwstrarray14 [i]=  "";
                glay.vwstrarray15 [i]=  "";
                glay.vwstrarray16 [i]=  "";
                ViewData["extcostt" + i] = 0;
                glay.vwdclarray0[i] = 0;
                glay.vwdclarray1[i] = 0;
                glay.vwdclarray2[i] = 0;
                glay.vwstrarray4[i] = "";
                glay.vwdtarray0[i] = DateTime.UtcNow;
                glay.vwdtarray1[i] = DateTime.UtcNow;
                glay.vwitarray0[i] = 0;
                glay.vwitarray3[i] = 0;
                glay.vwitarray2[i] = 0;
                glay.vwdclarray3[i] = 0;

                ViewData["costphead" + i] = 0;
                ViewData["totalmaterialcost"] = 0;
                glay.vwdclarray4[i] = 0;
                ViewData["totalhrcost"] = 0;
                ViewData["totalcontractamount"] = 0;
                ViewData["totalmiscecost"] = 0;
                ViewData["totalduration"] = 0;
            }
            //glay.vwstring2 = "N";
            //glay.vwbool0 = true;
        }

        private void update_record()
        {
           if(action_flag == "Create")
            {
                WO_002_WKO = new WO_002_WKO();
                WO_002_WKO.created_by = pubsess.userid;
                WO_002_WKO.created_date = DateTime.UtcNow;
            }
            else
            {
                WO_002_WKO = db.WO_002_WKO.Find(glay.vwstring0, glay.vwstrarray2[15]);
            }
            WO_002_WKO.work_order_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            WO_002_WKO.work_order_description = string.IsNullOrWhiteSpace(glay.vwstrarray2[0]) ? "" : glay.vwstrarray2[0];
            WO_002_WKO.status = string.IsNullOrWhiteSpace(glay.vwstrarray2[14]) ? "" : glay.vwstrarray2[14];
            WO_002_WKO.job_card_id = "";
            WO_002_WKO.estimated_total_cost = glay.vwdecimal0;
            WO_002_WKO.estimated_start_date_time = string.IsNullOrWhiteSpace(glay.vwstrarray2[1]) ? "" : glay.vwstrarray2[1];
            WO_002_WKO.estimated_end_date_time = string.IsNullOrWhiteSpace(glay.vwstrarray2[2]) ? "" : glay.vwstrarray2[2];
            WO_002_WKO.maintenance_id = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            WO_002_WKO.activation_id = string.IsNullOrWhiteSpace(glay.vwstring4) ? "" : glay.vwstring4;
            WO_002_WKO.modified_date = DateTime.UtcNow;
            WO_002_WKO.work_center_id = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            WO_002_WKO.work_order_date = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
            WO_002_WKO.team_lead = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            WO_002_WKO.gl_account = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            WO_002_WKO.asset_or_group = string.IsNullOrWhiteSpace(glay.vwstring9) ? "" : glay.vwstring9;
            WO_002_WKO.modified_by = pubsess.userid;
            WO_002_WKO.total_materials_cost = glay.vwdclarray3[0];
            WO_002_WKO.total_hr_cost = glay.vwdclarray3[1];
            WO_002_WKO.total_duration = glay.vwint10;
            WO_002_WKO.total_contract_amount = glay.vwdclarray3[2];
            WO_002_WKO.total_misc_amount = glay.vwdclarray3[4];
            WO_002_WKO.approval_level =0;
            WO_002_WKO.approval_date = DateTime.UtcNow;
            WO_002_WKO.flag = string.IsNullOrWhiteSpace(glay.vwstrarray2[15]) ? "" : glay.vwstrarray2[15];
            WO_002_WKO.approval_by = "";
            WO_002_WKO.cvt_to_wrk_ord = string.IsNullOrWhiteSpace(glay.vwstrarray2[10]) ? "" : glay.vwstrarray2[10];
            WO_002_WKO.note = "";

            counter = 0;
            counter1 = 0;
            counter2 = 0;
            counter3 = 0;
            counter4 = 0;
            string sqlstr = "delete from WO_002_WKODT where work_order_id='" + glay.vwstring0 + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            for (int i = 0; i < glay.vwstrarray1.Length; i++)
            {

                if (!string.IsNullOrWhiteSpace(glay.vwstrarray1[i]))
                {
                    WO_002_WKODT WO_002_WKODT = new WO_002_WKODT();
                    WO_002_WKODT.work_order_id = glay.vwstring0;
                    WO_002_WKODT.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray1[i]) ? "" : glay.vwstrarray1[i];
                    WO_002_WKODT.qty = glay.vwitarray2[i];
                    WO_002_WKODT.cost = 0;
                    WO_002_WKODT.total = Convert.ToDecimal(ViewData["extcostt" + i]);
                    WO_002_WKODT.sequence_no = counter.ToString();
                    WO_002_WKODT.flag = "I";
                    WO_002_WKODT.time = "";
                    WO_002_WKODT.description = "";
                    WO_002_WKODT.start_date = DateTime.UtcNow;
                    WO_002_WKODT.end_date = DateTime.UtcNow;
                    db.Entry(WO_002_WKODT).State = EntityState.Added;
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
                    counter++;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstrarray1[i]))
                    break;
            }
            for (int i = 0; i < glay.vwstrarray5.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(glay.vwstrarray5[i]))
                {

                    WO_002_WKODT WO_002_WKODT = new WO_002_WKODT();

                    WO_002_WKODT.work_order_id = glay.vwstring0;
                    WO_002_WKODT.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray5[i]) ? "" : glay.vwstrarray5[i];
                    WO_002_WKODT.description = string.IsNullOrWhiteSpace(glay.vwstrarray14[i]) ? "" : glay.vwstrarray14[i];
                    WO_002_WKODT.qty = glay.vwitarray0[i];
                    WO_002_WKODT.time = string.IsNullOrWhiteSpace(glay.vwstrarray4[i]) ? "" : glay.vwstrarray4[i];
                    WO_002_WKODT.cost = 0;
                    WO_002_WKODT.total = glay.vwdclarray4[i];
                    WO_002_WKODT.sequence_no = counter1.ToString();
                    WO_002_WKODT.flag = "J";
                    WO_002_WKODT.start_date = DateTime.UtcNow;
                    WO_002_WKODT.end_date = DateTime.UtcNow;

                    db.Entry(WO_002_WKODT).State = EntityState.Added;
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
                    counter1++;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstrarray5[i]))
                    break;
            }
            for (int i = 0; i < glay.vwitarray3.Length; i++)
            {

                if (!(glay.vwitarray3[counter2].Equals(0)))
                {
                    WO_002_WKODT WO_002_WKODT = new WO_002_WKODT();
                    WO_002_WKODT.work_order_id = glay.vwstring0;
                    WO_002_WKODT.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray3[i]) ? "" : glay.vwstrarray3[i];
                    WO_002_WKODT.description = string.IsNullOrWhiteSpace(glay.vwstrarray7[i]) ? "" : glay.vwstrarray7[i];
                    WO_002_WKODT.qty = glay.vwitarray3[i];
                    WO_002_WKODT.start_date = glay.vwdtarray1[i];
                    WO_002_WKODT.end_date = glay.vwdtarray0[i];
                    //preceeding
                    WO_002_WKODT.cost = Convert.ToDecimal(glay.vwitarray1[i]);
                    WO_002_WKODT.total = 0;
                    WO_002_WKODT.time = string.IsNullOrWhiteSpace(glay.vwstrarray11[i]) ? "" : glay.vwstrarray11[i];
                    WO_002_WKODT.sequence_no = counter2.ToString();
                    WO_002_WKODT.flag = "K";

                    db.Entry(WO_002_WKODT).State = EntityState.Added;
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
                    counter2++;
                }
                if (glay.vwitarray3[counter2].Equals(0))
                    break;
            }
            for (int i = 0; i < glay.vwstrarray15.Length; i++)
            {
                WO_002_WKODT WO_002_WKODT = new WO_002_WKODT();
                WO_002_WKODT.work_order_id = glay.vwstring0;
                WO_002_WKODT.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray15[i]) ? "" : glay.vwstrarray15[i];
                WO_002_WKODT.qty = 0;
                WO_002_WKODT.description = "";
                WO_002_WKODT.start_date = DateTime.UtcNow;
                WO_002_WKODT.end_date = DateTime.UtcNow;
                WO_002_WKODT.cost = 0;
                WO_002_WKODT.total = glay.vwdclarray0[i];
                WO_002_WKODT.sequence_no = counter3.ToString();
                WO_002_WKODT.flag = "L";
                WO_002_WKODT.time = "";
                db.Entry(WO_002_WKODT).State = EntityState.Added;
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
                counter3++;
                if (string.IsNullOrWhiteSpace(glay.vwstrarray15[i]))
                    break;
            }
            for (int i = 0; i < glay.vwstrarray0.Length; i++)
            {

                if (!string.IsNullOrWhiteSpace(glay.vwstrarray0[i]))
                {
                    WO_002_WKODT WO_002_WKODT = new WO_002_WKODT();
                    WO_002_WKODT.work_order_id = glay.vwstring0;
                    WO_002_WKODT.identity_code = "";
                    WO_002_WKODT.qty = 0;
                    WO_002_WKODT.description = string.IsNullOrWhiteSpace(glay.vwstrarray0[i]) ? "" : glay.vwstrarray0[i];
                    WO_002_WKODT.start_date = DateTime.UtcNow;
                    WO_002_WKODT.end_date = DateTime.UtcNow;
                    WO_002_WKODT.cost = 0;
                    WO_002_WKODT.total = glay.vwdclarray2[i];
                    WO_002_WKODT.sequence_no = counter4.ToString();
                    WO_002_WKODT.flag = "M";
                    WO_002_WKODT.time = "";
                    db.Entry(WO_002_WKODT).State = EntityState.Added;
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
                    counter4++;
                }
                if (string.IsNullOrWhiteSpace(glay.vwstrarray0[i]))
                    break;

            }

           if(action_flag == "Create")
                db.Entry(WO_002_WKO).State = EntityState.Added;
            else
                db.Entry(WO_002_WKO).State = EntityState.Modified;
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
            var bgasj = (from bg in db.JC_002_JCD
                         where bg.Work_order_ID == glay.vwstring0
                         select bg.Work_order_ID).FirstOrDefault();
            if (glay.vwstrarray2[10] == "Y" && bgasj == null)
            {
                JC_002_JCD JC_002_JCD = new JC_002_JCD();
                glay.vwint9 = 1;
                var duplicate =
                from bg in db.JC_002_JCD
                select bg;
                var count = duplicate.Count();
                JC_002_JCD.Job_card_id = "jc" + count + 1;
                JC_002_JCD.Work_order_ID = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                JC_002_JCD.Job_card_Description = string.IsNullOrWhiteSpace(glay.vwstrarray2[0]) ? "" : glay.vwstrarray2[0];
                JC_002_JCD.estimated_total_cost = glay.vwdecimal0;
                JC_002_JCD.modified_date = DateTime.UtcNow;
                JC_002_JCD.Work_order_date = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
                JC_002_JCD.team_lead = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
                JC_002_JCD.gl_account = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
                JC_002_JCD.modified_by = pubsess.userid;
                JC_002_JCD.total_materials_cost = glay.vwdclarray3[0];
                JC_002_JCD.total_hr_cost = glay.vwdclarray3[1];
                JC_002_JCD.total_duration = glay.vwint10;
                JC_002_JCD.total_contract_amount = glay.vwdclarray3[2];
                JC_002_JCD.total_misc_amount = glay.vwdclarray3[4];
                JC_002_JCD.Asset_or_group_ID = string.IsNullOrWhiteSpace(glay.vwstring9) ? "" : glay.vwstring9;
                JC_002_JCD.created_by = pubsess.userid;
                JC_002_JCD.created_date = DateTime.UtcNow;
                JC_002_JCD.Work_order_completed = "";
                JC_002_JCD.work_center = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                JC_002_JCD.Work_order_completion_Date = "";
                MainContext db1 = new MainContext();
                var str = from a in db.WO_002_WKODT
                          where a.work_order_id == glay.vwstring0
                          select a;

                foreach (var val in str)
                {

                    JC_002_JDCDET ls = new JC_002_JDCDET();

                    ls.job_card_id = JC_002_JCD.Job_card_id;
                    ls.identity_code = val.identity_code;
                    ls.sequence_no = val.sequence_no;
                    ls.flag = val.flag;
                    ls.qty = val.qty;
                    ls.time = val.time;
                    ls.total = val.total;
                    ls.cost = val.cost;
                    ls.description = val.description;
                    ls.start_date = val.start_date;
                    ls.end_date = val.end_date;
                    ls.completed = "";

                    db1.Entry(ls).State = EntityState.Added;
                    try
                    {
                        db1.SaveChanges();
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

                db1.Entry(JC_002_JCD).State = EntityState.Added;
                try
                {
                    db1.SaveChanges();
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
}

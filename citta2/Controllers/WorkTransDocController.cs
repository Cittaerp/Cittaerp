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
    public class WorkTransDocController : Controller
    {
        JC_002_JCD JC_002_JCD = new JC_002_JCD();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
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
        bool err_flag = true;
        string action_flag = "";
        // GET: JobCard
        public ActionResult Index()
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            var bglist = from bh in db.JC_002_JCD
                         join bg in db.WO_002_WKO
                         on new { a1 = bh.Work_order_ID } equals new { a1 = bg.work_order_id }
                         into bh1
                         from bh2 in bh1.DefaultIfEmpty()
                         join bi in db.GB_999_MSG
                         on new { a1 = bh2.status, a2 = "stat" } equals new { a1 = bi.code_msg , a2 = bi.type_msg }
                         into bi1
                         from bi2 in bi1.DefaultIfEmpty()
                         join bj in db.GB_999_MSG
                         on new {a1 = bh.Work_order_completed, a2 = "com" } equals new {a1 = bj.code_msg, a2 = bj.type_msg}
                         into bj1
                         from bj2 in bj1.DefaultIfEmpty()
                         select new vw_genlay
                         {
                             vwstring0 = bh.Job_card_id,
                             vwstring1 = bh.Job_card_Description,
                             vwstring2 = bh2.work_order_description,
                             vwstring3 = bj2.name1_msg,
                             vwstring4 = bh2.work_order_id
                         };
            return View(bglist.ToList());
        }
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
        public ActionResult Create(vw_genlay glay_in, string subcheck)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;

            if (subcheck == "RO")
            {
                glay.vwint8 = 1;
                assign2();
                select_query();
                ModelState.Remove("subcheck");
                return View(glay);
            }
            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }
        public ActionResult Edit(string key1, string key2)
        {

            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            if (string.IsNullOrWhiteSpace(key1))
            {
                key1 = "";
            }
            JC_002_JCD = db.JC_002_JCD.Find(key1, key2);
            string sqlstr = "UPDATE [dbo].[WO_002_WKO] SET status = 'I', job_card_id = " + util.sqlquote(key1)+" WHERE work_order_id =" + util.sqlquote(key2);
             int delctr = db.Database.ExecuteSqlCommand(sqlstr);

            initial_rtn();
            if (JC_002_JCD != null)
                glay.vwstring10 = key2;
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
            update_file();
            if (err_flag)
                return RedirectToAction("Index");
            select_query();
            return View(glay);
        }
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[JC_002_JCD] where Job_card_id ='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            string swl = "delete from [dbo].[JC_002_JDCDET] where job_card_id ='" + id + "'";
            int dt = db.Database.ExecuteSqlCommand(swl);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            JC_002_JCD = db.JC_002_JCD.Find(glay.vwstring0);
            if (JC_002_JCD != null)
            {
                db.JC_002_JCD.Remove(JC_002_JCD);
                db.SaveChanges();
            }
            string swl = "delete from [dbo].[JC_002_JDCDET] where job_card_id ='" + glay.vwstring0 + "'";
            int dt = db.Database.ExecuteSqlCommand(swl);
        }

        private void read_record()
        {

            glay.vwstring0 = JC_002_JCD.Job_card_id;
            glay.vwstring1 = JC_002_JCD.Job_card_Description;
            var bgstat = (from bg in db.JC_002_JCD
                          join bh in db.WO_002_WKO
                          on new { a1 = bg.Work_order_ID } equals new { a1 = bh.work_order_id }
                          into bh1
                          from bh2 in bh1.DefaultIfEmpty()
                          join bi in db.FA_001_ASSET
                          on new {a2 = bh2.asset_or_group} equals new {a2 = bi.fixed_asset_code}
                          into bi1 
                          from bi2 in bi1.DefaultIfEmpty()
                          where bg.Job_card_id == glay.vwstring0 && bg.Work_order_ID == glay.vwstring10
                          select new { bg, bh2, bi2 }).FirstOrDefault();
            if (bgstat != null)
            {
                glay.vwstring2 = bgstat.bh2.work_order_description;
                glay.vwstring11 = bgstat.bh2.work_order_id;
                glay.vwstrarray2[7] = bgstat.bg.Asset_or_group_ID;
                glay.vwstrarray2[8] = bgstat.bi2.description;
            }
            var bgstat1 = (from bg in db.JC_002_JCD
                          join bh in db.WO_002_WKO
                          on new { a1 = bg.Work_order_ID } equals new { a1 = bh.work_order_id }
                          into bh1
                          from bh2 in bh1.DefaultIfEmpty()
                          join bi in db.AG_001_ASG
                          on new { a2 = bh2.asset_or_group } equals new { a2 = bi.asset_grouping_id }
                          into bi1
                          from bi2 in bi1.DefaultIfEmpty()
                          where bg.Job_card_id == glay.vwstring0 && bg.Work_order_ID == glay.vwstring10
                          select new { bg, bh2, bi2 }).FirstOrDefault();
            if (bgstat1 != null)
            {
                glay.vwstring2 = bgstat.bh2.work_order_description;
                glay.vwstring11 = bgstat.bh2.work_order_id;
                glay.vwstrarray2[7] = bgstat.bg.Asset_or_group_ID;
                glay.vwstrarray2[8] = bgstat.bi2.description;

            }
            string sqlstr = "UPDATE [dbo].[WO_002_WKO] SET status = 'I' WHERE work_order_id =" + util.sqlquote(glay.vwstring10);
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            var wcid = (from bg in db.WC_001_WKC
                         where bg.work_center_id == JC_002_JCD.work_center
                         select bg.description).FirstOrDefault();
            if(wcid !=null)
            glay.vwstring6 =wcid;
            glay.vwdecimal0 = JC_002_JCD.estimated_total_cost;
            glay.vwstring3 = JC_002_JCD.Work_order_date;
            glay.vwstring5 = JC_002_JCD.gl_account;
            
            var teaml = (from bg in db.GB_001_EMP
                        where bg.employee_code == JC_002_JCD.team_lead
                        select bg.name).FirstOrDefault();
            if(teaml !=null)
            glay.vwstring4 = teaml;
            glay.vwstring7 = JC_002_JCD.Work_order_completed;
            glay.vwstring8 = JC_002_JCD.Work_order_completion_Date;
            var bgfxed = (from bg in db.FA_001_ASSET
                          join bh in db.GB_999_MSG
                          on new {a1=bg.unit_of_reading, a2 = "cost"} equals new {a1 = bh.code_msg, a2 = bh.type_msg}
                          into bh1
                          from bh2 in bh1.DefaultIfEmpty()
                          where bg.fixed_asset_code == JC_002_JCD.Asset_or_group_ID
                         select new { bg, bh2 }).FirstOrDefault();
            if (bgfxed != null)
            {
               glay.vwint9=bgfxed.bg.cumulative_amount;
                glay.vwstrarray2[10] = bgfxed.bh2.name1_msg;
            }
            glay.vwdclarray3[0] = JC_002_JCD.total_materials_cost;
            glay.vwdclarray3[1] = JC_002_JCD.total_hr_cost;
            glay.vwdclarray3[2] = JC_002_JCD.total_duration;
            glay.vwdclarray3[3] = JC_002_JCD.total_contract_amount;
            glay.vwdclarray3[4] = JC_002_JCD.total_misc_amount;

            int counter = 0;
            string get_id = glay.vwstring0;
            var bgassign1 = (from bg in db.JC_002_JDCDET
                             join bh in db.JC_002_JCD
                             on new { a1 = bg.job_card_id } equals new { a1 = bh.Job_card_id }
                             into bh1
                             from bh2 in bh1.DefaultIfEmpty()
                             join bi in db.WO_002_WKODT
                             on new {a1 = bg.identity_code} equals new {a1 = bi.identity_code}
                             into bi1
                             from bi2 in bi1.DefaultIfEmpty()
                             where bg.job_card_id == get_id
                             orderby bg.flag, bg.sequence_no
                             select new { bg, bh2, bi2 }).ToList();
            foreach (var item in bgassign1)
            {
                if (item.bg.flag == "I")
                {
                    count.Add(item.bg.sequence_no);
                    me = item.bg.identity_code;
                    glay.vwstrarray1[counter] = me;
                    ViewData["extcostt" + counter] = item.bg.total;
                    glay.vwitarray2[counter] = item.bg.qty;
                    if(item.bi2 != null)
                    TempData["woqty"+ counter] = item.bi2.qty;
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
                    var bgitem = (from bg in db.WO_002_WKODT
                                  where bg.identity_code == me
                                  select bg).FirstOrDefault();
                    if(bgitem != null)
                    {
                        glay.vwblarray1[counter] = false;
                    }

                    else
                    {
                        glay.vwblarray1[counter] = true;
                        TempData["woqty" + counter] = 0;
                    }
                       
                    counter++;
                }
                if (item.bg.flag == "J")
                {

                    count1.Add(item.bg.sequence_no);
                    int jab = Convert.ToInt32(item.bg.identity_code);
                    glay.vwstrarray5[counter1] = item.bg.identity_code;
                    glay.vwstrarray14[counter1] = item.bg.description;
                    glay.vwitarray0[counter1] = item.bg.qty;
                    var bgassignc = (from bg in db.JB_001_JOB
                                     join bh in db.GB_999_MSG
                                     on new { a1 = bg.costing_basis, a2 =  "cost" } equals new { a1 = bh.code_msg, a2 = bh.type_msg }
                                     into bh1
                                     from bh2 in bh1.DefaultIfEmpty()
                                     join bj in db.GB_001_EMP
                                    on new { a1 = bg.job_id } equals new { a1 = bj.job_role }
                                    into bj1
                                     from bj2 in bj1.DefaultIfEmpty()
                                     join bj in db.WO_002_WKODT
                                    on new { a1 = item.bg.identity_code } equals new { a1 = bj.identity_code }
                                    into bk1
                                     from bk2 in bk1.DefaultIfEmpty()
                                     where bg.job_id ==jab
                                     select new { bg, bh2, bj2, bk2 }).FirstOrDefault();
                    if (bgassignc != null)
                    {
                        glay.vwstrarray4[counter1] = bgassignc.bh2.name1_msg;
                        int empp = Convert.ToInt32(item.bg.identity_code);
                        var bgemp = from bg in db.GB_001_EMP
                                    where bg.active_status == "N" && bg.job_role == empp
                                    select new { c1 = bg.employee_code, c2 = bg.name };

                        ViewData["emp" + counter1] = new SelectList(bgemp.ToList(), "c1", "c2");
                        ViewData["costphead" + counter1] = bgassignc.bg.cost;
                        ViewData["totalcostt" + counter1] = item.bg.total;
                        if (bgassignc.bk2 != null)
                            TempData["woest" + counter1] = bgassignc.bk2.qty;

                    }
                    var bgitem = (from bg in db.WO_002_WKODT
                                  where bg.identity_code == item.bg.identity_code
                                  select bg).FirstOrDefault();
                    if (bgitem != null)
                    {
                        glay.vwblarray2[counter1] = false;
                    }

                    else
                    {
                        glay.vwblarray2[counter1] = true;
                        TempData["woest" + counter1] = 0;
                    }
                    counter1++;

                }

                if (item.bg.flag == "K")
                {
                    count2.Add(item.bg.sequence_no);
                    glay.vwstrarray3[counter3] = item.bg.identity_code;
                    glay.vwitarray3[counter3] = item.bg.qty;
                    glay.vwitarray1[counter3] = Convert.ToInt32(item.bg.cost);
                    glay.vwstrarray9[counter3] = item.bg.description;
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
            if(count.Count !=0)
            glay.vwint1 = count.Count;
            if (count1.Count != 0)
                glay.vwint5 = count1.Count;
            if (count2.Count != 0)
                glay.vwint3 = count2.Count;
            if (count3.Count != 0)
                glay.vwint4 = count3.Count;
            if (count4.Count != 0)
                glay.vwint2 = count3.Count;
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


            if (ViewBag.emp == null)
            {
                var bgemp = from bg in db.GB_001_EMP
                            where bg.active_status == "N"
                            select new { c1 = bg.employee_code, c2 = bg.name };

                ViewBag.emp = new SelectList(bgemp.ToList(), "c1", "c2");
            }


            var chart = from bg in db.GL_001_CHART
                        select bg;
            ViewBag.chart1 = new SelectList(chart.ToList(), "account_code", "account_name", glay.vwstring5);

            var bgmainid = from bg in db.WO_002_WKO
                           select new { c1 = bg.work_order_id, c2 = bg.work_order_id + "-" + bg.work_order_description };
            ViewBag.workorder = new SelectList(bgmainid.ToList(), "c1", "c2", glay.vwstring1);

            var bgmaint = from bg in db.AG_001_AMG
                          where bg.inactive_status == "N" && bg.nature == "nat2"
                          select bg;
            ViewBag.maint = new SelectList(bgmaint.ToList(), "maintenance_group_type_id", "description", glay.vwstring3);
            var bgworkcent = from bg in db.WC_001_WKC
                             where bg.inactive_status == "N"
                             select bg;
            ViewBag.workcenter = new SelectList(bgworkcent.ToList(), "work_center_id", "description", glay.vwstring6);

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

            ViewBag.team = new SelectList(bgteam.ToList(), "c1", "c2", glay.vwstring4);

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
            glay.vwblarray1 = new bool[50];
            glay.vwblarray2 = new bool[50];
            glay.vwitarray0 = new int[50];
            glay.vwitarray1 = new int[50];
            glay.vwitarray2 = new int[50];
            glay.vwitarray3 = new int[50];
            glay.vwdtarray0 = new DateTime[50];
            glay.vwdtarray1 = new DateTime[50];
            glay.vwblarray0 = new Boolean[50];
            glay.vwint1 = 1;
            glay.vwint5 = 1;
            glay.vwint3 = 1;
            glay.vwint4 = 1;
            glay.vwint2 = 1;
            glay.vwstrarray2[0] = "tab-pane active";
            glay.vwstrarray2[1] = "tab-pane";
            glay.vwstrarray2[2] = "tab-pane";
            glay.vwstrarray2[3] = "tab-pane";
            glay.vwstrarray2[4] = "tab-pane";
            glay.vwstrarray2[5] = "tab-pane";
            glay.vwdate0 = DateTime.UtcNow;

            for (int i = 0; i < 50; i++)
            {
                glay.vwblarray1[i] = true;
                glay.vwblarray2[i] = true;
                glay.vwstrarray0[i] = "";
                glay.vwstrarray1 [i] = "";
                //glay.vwstrarray2 [i] = "";
                glay.vwstrarray3 [i] = "";
                glay.vwstrarray4 [i] = "";
                glay.vwstrarray5 [i] = "";
                glay.vwstrarray6 [i] = "";
                glay.vwstrarray7 [i] = "";
                glay.vwstrarray8 [i] = "";
                glay.vwstrarray9 [i] = "";
                glay.vwstrarray10 [i] = "";
                glay.vwitarray1[i] = 0;
                glay.vwstrarray11 [i] = "";
                glay.vwstrarray12 [i] = "";
                glay.vwstrarray13 [i] = "";
                glay.vwstrarray14 [i] = "";
                glay.vwstrarray15 [i] = "";
                glay.vwstrarray16 [i] = "";
                ViewData["extcostt" + i] = 0;
                glay.vwdclarray0[i] = 0;
                glay.vwdclarray1[i] = 0;
                glay.vwitarray1[i] = 0;
                glay.vwdclarray2[i] = 0;
                glay.vwstrarray4[i] = "D";
                glay.vwdtarray0[i] = DateTime.UtcNow;
                glay.vwdtarray1[i] = DateTime.UtcNow;
                glay.vwitarray0[i] = 0;
                glay.vwitarray3[i] = 0;
                glay.vwitarray2[i] = 0;
                glay.vwdclarray3[i] = 0;
                glay.vwdclarray4[i] = 0;
                ViewData["costphead" + i] = 0;
                ViewData["totalmaterialcost"] = 0;
                ViewData["totalcostt" + i] = 0;
                ViewData["totalhrcost"] = 0;
                ViewData["totalcontractamount"] = 0;
                ViewData["totalmiscecost"] = 0;
                ViewData["totalduration"] = 0;
            }
            //glay.vwstring2 = "N";
            //glay.vwbool0 = true;
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
                ModelState.AddModelError(String.Empty, "Please Specify Job Card  ID");
                err_flag = false;
            }



            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please Specify Job Card Description");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring5))
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
                JC_002_JCD asst = db.JC_002_JCD.Find(glay.vwstring0);
                if (asst != null)
                {
                    ModelState.AddModelError(String.Empty, "Job Card ID Exists");
                    err_flag = false;
                }
                var bgassign = (from bg in db.JC_002_JCD
                                select bg.Job_card_Description).ToList();
                foreach (var item in bgassign)
                {
                    if (glay.vwstring9 == item)
                    {
                        ModelState.AddModelError(String.Empty, "Job Card Description Exists");
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
                JC_002_JCD = new JC_002_JCD();
                JC_002_JCD.created_by = pubsess.userid;
                JC_002_JCD.created_date = DateTime.UtcNow;
            }
            else
            {
                JC_002_JCD = db.JC_002_JCD.Find(glay.vwstring0, glay.vwstring10);
            }
            JC_002_JCD.Job_card_id = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
            JC_002_JCD.Job_card_Description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            JC_002_JCD.estimated_total_cost = glay.vwdecimal0;
            JC_002_JCD.Work_order_ID = string.IsNullOrWhiteSpace(glay.vwstring10) ? "" : glay.vwstring10;
            JC_002_JCD.Work_order_date = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            JC_002_JCD.team_lead = "";
            JC_002_JCD.Asset_or_group_ID = string.IsNullOrWhiteSpace(glay.vwstrarray2[7]) ? "" : glay.vwstrarray2[7];
            JC_002_JCD.gl_account = string.IsNullOrWhiteSpace(glay.vwstring5) ? "" : glay.vwstring5;
            JC_002_JCD.work_center ="";
            JC_002_JCD.Work_order_completed = string.IsNullOrWhiteSpace(glay.vwstring7) ? "" : glay.vwstring7;
            JC_002_JCD.Work_order_completion_Date = string.IsNullOrWhiteSpace(glay.vwstring8) ? "" : glay.vwstring8;
            JC_002_JCD.modified_by = pubsess.userid;
            JC_002_JCD.total_materials_cost = glay.vwdclarray3[0];
            JC_002_JCD.total_hr_cost = glay.vwdclarray3[1];
            JC_002_JCD.total_duration = Convert.ToInt32(glay.vwdclarray3[2]);
            JC_002_JCD.total_contract_amount = glay.vwdclarray3[3];
            JC_002_JCD.total_misc_amount = glay.vwdclarray3[4];
            
            string sqlstr="";
            int delctr = 0; ;
                if (glay.vwstring7 == "Y")
            {
                sqlstr= "UPDATE [dbo].[WO_002_WKO] SET status = 'C' WHERE work_order_id =" + util.sqlquote(glay.vwstring10);
                 delctr = db.Database.ExecuteSqlCommand(sqlstr);
                sqlstr = "delete from [dbo].[WO_002_WKO] where work_order_id ="+util.sqlquote(glay.vwstring10);
                delctr = db.Database.ExecuteSqlCommand(sqlstr);
            }
            string query = "UPDATE [dbo].[WO_002_WKO] SET job_card_id = "+ util.sqlquote(glay.vwstring0)+" WHERE job_card_id =" + util.sqlquote(glay.vwstring0);
            int getdet = db.Database.ExecuteSqlCommand(query);
            string fixdq = "UPDATE [dbo].[FA_001_ASSET] SET last_maintenance_date = " + util.sqlquote(glay.vwstring8) + " WHERE fixed_asset_code =" + util.sqlquote(glay.vwstrarray2[7]);
            int getqry = db.Database.ExecuteSqlCommand(fixdq);
            string assetq = "UPDATE [dbo].[AG_001_ASG] SET last_maintenance_date = " + util.sqlquote(glay.vwstring8) + " WHERE asset_grouping_id =" + util.sqlquote(glay.vwstrarray2[7]);
            int getgrp = db.Database.ExecuteSqlCommand(assetq);
            counter = 0;
            counter1 = 0;
            counter2 = 0;
            counter3 = 0;
            counter4 = 0;
             sqlstr = "delete from JC_002_JDCDET where Job_card_id='" + glay.vwstring0 + "'";
             delctr = db.Database.ExecuteSqlCommand(sqlstr);
            for (int i = 0; i < glay.vwstrarray1.Length; i++)
            {

                if (!string.IsNullOrWhiteSpace(glay.vwstrarray1[i]))
                {
                    JC_002_JDCDET JC_002_JDCDET = new JC_002_JDCDET();
                    JC_002_JDCDET.job_card_id = glay.vwstring0;
                    JC_002_JDCDET.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray1[i]) ? "" : glay.vwstrarray1[i];
                    JC_002_JDCDET.qty = glay.vwitarray2[i];
                    JC_002_JDCDET.cost = 0;
                    JC_002_JDCDET.total = Convert.ToDecimal(ViewData["extcostt" + i]);
                    JC_002_JDCDET.sequence_no = counter.ToString();
                    JC_002_JDCDET.flag = "I";
                    JC_002_JDCDET.time = "";
                    JC_002_JDCDET.description = "";
                    JC_002_JDCDET.completed = "";
                    JC_002_JDCDET.start_date = DateTime.UtcNow;
                    JC_002_JDCDET.end_date = DateTime.UtcNow;
                    db.Entry(JC_002_JDCDET).State = EntityState.Added;
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

                    JC_002_JDCDET JC_002_JDCDET = new JC_002_JDCDET();

                    JC_002_JDCDET.job_card_id = glay.vwstring0;
                    JC_002_JDCDET.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray5[i]) ? "" : glay.vwstrarray5[i];
                    JC_002_JDCDET.description = string.IsNullOrWhiteSpace(glay.vwstrarray14[i]) ? "" : glay.vwstrarray14[i];
                    JC_002_JDCDET.qty = glay.vwitarray0[i];
                    JC_002_JDCDET.time = string.IsNullOrWhiteSpace(glay.vwstrarray4[i]) ? "" : glay.vwstrarray4[i];
                    JC_002_JDCDET.cost = 0;
                    JC_002_JDCDET.total = Convert.ToDecimal(ViewData["totalcostt" + i]);
                    JC_002_JDCDET.sequence_no = counter1.ToString();
                    JC_002_JDCDET.flag = "J";
                    JC_002_JDCDET.start_date = DateTime.UtcNow;
                    JC_002_JDCDET.completed = glay.vwblarray0[i] ? "Y" : "N"; 
                    JC_002_JDCDET.end_date = DateTime.UtcNow;

                    db.Entry(JC_002_JDCDET).State = EntityState.Added;
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
                    JC_002_JDCDET JC_002_JDCDET = new JC_002_JDCDET();
                    JC_002_JDCDET.job_card_id = glay.vwstring0;
                    JC_002_JDCDET.identity_code = "";
                    JC_002_JDCDET.description = string.IsNullOrWhiteSpace(glay.vwstrarray9[i]) ? "" : glay.vwstrarray9[i];
                    JC_002_JDCDET.qty = glay.vwitarray3[i];
                    JC_002_JDCDET.start_date = glay.vwdtarray1[i];
                    JC_002_JDCDET.end_date = glay.vwdtarray0[i];
                    JC_002_JDCDET.cost = 0;
                    JC_002_JDCDET.total = 0;
                    JC_002_JDCDET.completed = glay.vwblarray0[i] ? "Y" : "N"; 
                    JC_002_JDCDET.time = "";
                    JC_002_JDCDET.sequence_no = counter2.ToString();
                    JC_002_JDCDET.flag = "K";

                    db.Entry(JC_002_JDCDET).State = EntityState.Added;
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
                JC_002_JDCDET JC_002_JDCDET = new JC_002_JDCDET();
                JC_002_JDCDET.job_card_id = glay.vwstring0;
                JC_002_JDCDET.identity_code = string.IsNullOrWhiteSpace(glay.vwstrarray15[i]) ? "" : glay.vwstrarray15[i];
                JC_002_JDCDET.qty = 0;
                JC_002_JDCDET.description = "";
                JC_002_JDCDET.start_date = DateTime.UtcNow;
                JC_002_JDCDET.end_date = DateTime.UtcNow;
                JC_002_JDCDET.cost = 0;
                JC_002_JDCDET.total = glay.vwdclarray0[i];
                JC_002_JDCDET.completed = "";
                JC_002_JDCDET.sequence_no = counter3.ToString();
                JC_002_JDCDET.flag = "L";
                JC_002_JDCDET.time = "";
                db.Entry(JC_002_JDCDET).State = EntityState.Added;
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
                    JC_002_JDCDET JC_002_JDCDET = new JC_002_JDCDET();
                    JC_002_JDCDET.job_card_id = glay.vwstring0;
                    JC_002_JDCDET.identity_code = "";
                    JC_002_JDCDET.qty = 0;
                    JC_002_JDCDET.description = string.IsNullOrWhiteSpace(glay.vwstrarray0[i]) ? "" : glay.vwstrarray0[i];
                    JC_002_JDCDET.start_date = DateTime.UtcNow;
                    JC_002_JDCDET.end_date = DateTime.UtcNow;
                    JC_002_JDCDET.cost = 0;
                    JC_002_JDCDET.total = glay.vwdclarray2[i];
                    JC_002_JDCDET.sequence_no = counter4.ToString();
                    JC_002_JDCDET.completed = "";
                    JC_002_JDCDET.flag = "M";
                    JC_002_JDCDET.time = "";
                    db.Entry(JC_002_JDCDET).State = EntityState.Added;
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
                db.Entry(JC_002_JCD).State = EntityState.Added;
            else
                db.Entry(JC_002_JCD).State = EntityState.Modified;
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
            glay.vwitarray2.CopyTo(rt, 0);
            string[] secst = new string[glay.vwstrarray2.Length];
            glay.vwstrarray2.CopyTo(secst, 0);
            //Decimal[] decc = new decimal[glay.vwdclarray4.Length];
            //glay.vwdclarray4.CopyTo(decc, 0);
            //initial_rtn();
            Boolean[] bol = new Boolean[glay.vwblarray1.Length];
            glay.vwblarray1.CopyTo(bol, 0);
            
            DateTime[] date1 = new DateTime[glay.vwdtarray0.Length];
            glay.vwdtarray0.CopyTo(date1, 0);

            DateTime[] date2 = new DateTime[glay.vwdtarray1.Length];
            glay.vwdtarray1.CopyTo(date2, 0);
            for (int i = 0; i < 12; i++)
            {
                ModelState.Remove("vwstring" + i);
                ModelState.Remove("vwdclarray1[" + i + "]");
                ModelState.Remove("vwstrarray6[" + i + "]");
                ModelState.Remove("vwdtarray0[" + i + "]");
                ModelState.Remove("vwdtarray1[" + i + "]");
                ModelState.Remove("vwblarray1[" + i + "]");
                ModelState.Remove("vwdclarray0[" + i + "]");
                ModelState.Remove("vwdclarray3[" + i + "]");
                ModelState.Remove("vwdclarray2[" + i + "]");
                ModelState.Remove("vwdclarray4[" + i + "]");
                ModelState.Remove("vwstrarray7[" + i + "]");
                ModelState.Remove("vwstrarray2[" + i + "]");
            }
            ModelState.Remove("vwstrarray0");
            ModelState.Remove("vwstrarray14");
            ModelState.Remove("vwitarray2");
            ModelState.Remove("vwint6");
            ModelState.Remove("vwint1");
            ModelState.Remove("vwint3");
            ModelState.Remove("vwint4");
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
            bol.CopyTo(glay.vwblarray1, 0);
            date1.CopyTo(glay.vwdtarray0, 0);
            date2.CopyTo(glay.vwdtarray1, 0);
            //decc.CopyTo(glay.vwdclarray4, 0);
            glay.vwint8 = calc;
            glay.vwint6 = int1;
            glay.vwint7 = tabc;
            string get_id = "";
            string get_taskid = "";

            string query = "select de.parameter_name vwstring8, pc.job_title vwstring0, ass.identity_code vwstring1, ass.sequence_no vwstring2,  ass.flagg vwstring3, ass.qtyreq vwint7,";
            query += " ass.total vwdecimal1, CONVERT(varchar(10), it.item_name) vwstring7, ass.head_count vwint1";
            query += " from AG_002_ASSET ass left outer join AG_001_AMG am";
            query += " on am.maintenance_group_type_id = ass.maintenance_group_type_id";
            query += " left outer join dbo.IV_001_ITEM it on ass.identity_code = it.item_code";
            query += " left outer join JB_001_JOB pc on ass.identity_code = pc.job_id";
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
                        var bgitem = (from bg in db.WO_002_WKODT
                                      where bg.identity_code == me
                                      select bg).FirstOrDefault();
                        if (bgitem != null)
                        {

                            glay.vwblarray1[counter] = false;
                            if(bgitem.qty !=0)
                            TempData["woqty" + counter] = bgitem.qty;
                        }

                        else
                        {
                            glay.vwblarray1[counter] = true;
                            TempData["woqty" + counter] = 0;
                        }
                            
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
                                     on new { a1 = bg.costing_basis, a2 = "cost" } equals new { a1 = bh.code_msg, a2 = bh.type_msg }
                                     into bh1
                                     from bh2 in bh1.DefaultIfEmpty()
                                     where bg.job_id == jab
                                     select new { bg, bh2 }).FirstOrDefault();
                    if (bgassignc != null)
                    {
                        glay.vwstrarray4[counter1] = bgassignc.bh2.name1_msg;
                        me = glay.vwstrarray5[counter1];
                         jab = Convert.ToInt16(glay.vwstrarray5[counter1]);
                        var bgemp = from bg in db.GB_001_EMP
                                    where bg.active_status == "N" && bg.job_role == jab
                                    select new { c1 = bg.employee_code, c2 = bg.name };

                        ViewData["emp"+counter1] = new SelectList(bgemp.ToList(), "c1", "c2");

                        ViewData["costphead" + counter1] = bgassignc.bg.cost;
                        ViewData["totalcostt" + counter1] = glay.vwitarray0[counter1] * bgassignc.bg.cost;
                        deccalcc1 += glay.vwitarray0[counter1] * bgassignc.bg.cost;
                        glay.vwdclarray3[1] = deccalcc1;
                        //if (glay.vwstrarray4[counter1] == "D")
                        //{
                        //    ViewData["totalcostt" + counter1] = glay.vwitarray0[counter1] * bgassignc.bg.cost;
                        //    deccalcc1 += glay.vwitarray0[counter1] * bgassignc.bg.cost;
                        //    glay.vwdclarray3[1] = deccalcc1;
                        //}
                        //else if (glay.vwstrarray4[counter1] == "H")
                        //{
                        //    double hour = 0.041667;
                        //    ViewData["totalcostt" + counter1] = Math.Round((glay.vwitarray0[counter1] * bgassignc.bg.cost) * Convert.ToDecimal(hour), 2);
                        //    deccalcc1 += glay.vwitarray0[counter1] * bgassignc.bg.cost * Convert.ToDecimal(hour);
                        //    glay.vwdclarray3[1] = Math.Round(deccalcc1, 2);
                        //}
                        //else if (glay.vwstrarray4[counter1] == "M")
                        //{
                        //    double hour = 0.016667;
                        //    ViewData["totalcostt" + counter1] = Math.Round((glay.vwitarray0[counter1] * bgassignc.bg.cost) * Convert.ToDecimal(hour), 2);
                        //    deccalcc1 += glay.vwitarray0[counter1] * bgassignc.bg.cost * Convert.ToDecimal(hour);
                        //    glay.vwdclarray3[1] = Math.Round(deccalcc1, 2);
                        //}
                        count1.Add(glay.vwstrarray5[counter1]);

                    }
                    var bgitem = (from bg in db.WO_002_WKODT
                                  where bg.identity_code == me
                                  select bg).FirstOrDefault();
                    if (bgitem != null)
                    {
                        glay.vwblarray2[counter1] = false;
                        if (bgitem.qty != 0)
                        {
                            TempData["woest" + counter1] = bgitem.qty;
                        }
                            
                    }

                    else
                    {
                        glay.vwblarray2[counter1] = true;
                        TempData["woest" + counter1] = 0;
                    }
                    counter1++;

                }
                while (!(string.IsNullOrWhiteSpace(glay.vwstrarray15[counter3])))
                {

                    me = glay.vwstrarray15[counter3];
                    var bgassignme = (from bg in db.SC_001_SCM
                                      join bh in db.AP_001_VENDR
                                      on new { a1 = bg.vendor_id } equals new { a1 = bh.vendor_code }
                                      into bh1
                                      from bh2 in bh1.DefaultIfEmpty()
                                      where bg.sub_contract_id == me
                                      select new { bg, bh2 }).FirstOrDefault();
                    if (bgassignme != null)
                    {
                        count3.Add(glay.vwstrarray15[counter3]);
                        glay.vwdclarray0[counter3] = bgassignme.bg.total_cost;
                        glay.vwstrarray6[counter3] = bgassignme.bg.description;
                    }

                    deccalcc2 += glay.vwdclarray0[counter3];
                    glay.vwdclarray3[3] = deccalcc2;
                    counter3++;
                }


                int precount = 0;
                int countdate = 1;
                for (int x = 0; x < glay.vwitarray3.Length; x++)
                {
                    if (glay.vwitarray3[x] != 0)
                    {
                        precount = precount + 1;

                    }
                }

                while (counter2 <= precount)
                {
                    //int index =  Array.IndexOf(glay.vwitarray1, counter2+1);
                    int[] index = glay.vwitarray1.Select((b, i) => b == counter2 + 1 ? i : -1).Where(i => i != -1).ToArray();
                    foreach (var item in index)
                    {
                        count3.Add(glay.vwstrarray9[counter2].ToString());
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

                                int[] dateindex = glay.vwitarray1.Select((b, i) => b == item - countdate ? i : -1).Where(i => i != -1).ToArray();
                                List<DateTime> getdate = new List<DateTime>();
                                foreach (var dateitem in dateindex)
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
                    glay.vwdclarray3[2] = itcal;
                    counter2++;
                }

                //int precount = 0;

                //for (int x = 0; x < glay.vwitarray3.Length; x++)
                //{
                //    if (glay.vwitarray3[x] != 0)
                //    {
                //        precount = precount + 1;

                //    }
                //}

                //while (counter2 <= precount)
                //{
                //    int index = Array.IndexOf(glay.vwitarray1, counter2 + 1);
                //    if (index != -1)
                //    {
                //        count3.Add(glay.vwstrarray9[counter2].ToString());
                //        if (glay.vwdate0 != null)
                //        {

                //            if (glay.vwitarray1[index] == 1)
                //                glay.vwdtarray1[index] = glay.vwdate0;
                //            DateTime q_date = glay.vwdtarray1[index];
                //            dt = q_date.AddDays(glay.vwitarray3[index]);
                //            glay.vwdtarray0[index] = dt;
                //            glay.vwdtarray1[index + 1] = glay.vwdtarray0[index];
                //            itcal = (glay.vwdtarray1[index + 1] - glay.vwdate0).Days;
                //        }
                //    }
                //    glay.vwint10 = itcal;
                //    counter2++;
                //}
                while (!(glay.vwdclarray2[counter4].Equals(0)))
                {
                    deccalcc4 += glay.vwdclarray2[counter4];
                    glay.vwdclarray3[4] = deccalcc4;
                    counter4++;
                }
            }

            else
            {


                var bgtaskname = (from ti in db.TK_001_TAC
                                  where ti.task_id == get_taskid
                                  select ti).FirstOrDefault();
                var bgtask = (from tk in db.TK_001_TAC
                              join bk in db.TC_001_TCL
                              on new { a1 = tk.required_job_level_id } equals new { a1 = bk.technical_competency_level_id }
                              into bk1
                              from bk2 in bk1.DefaultIfEmpty()
                              where tk.task_id == get_taskid && tk.sequence != 0
                              select new { tk, bk2 }).ToList();

                if (bgtask != null)
                {
                    foreach (var tem in bgtask)
                    {
                        count2.Add(tem.tk.sequence.ToString());
                        if (tem.tk.sequence != 0)
                        {

                            glay.vwstrarray9[counter2] = tem.tk.task_id;
                            glay.vwitarray3[counter2] = tem.tk.estimated_no_of_hrs;
                            glay.vwstrarray11[counter2] = tem.bk2.technical_competency_level_id;
                            ViewData["task" + counter2] = tem.bk2.description;
                            itcal += glay.vwitarray3[counter2];
                            glay.vwdclarray3[2] = Convert.ToDecimal(itcal);
                            DateTime q_date = glay.vwdtarray1[counter2];
                            dt = q_date.AddDays(glay.vwitarray3[counter2]);
                            me = dt.ToString("dd/MM/yyy");
                            glay.vwdtarray0[counter2] = util.date_convert(me);
                            counter2++;

                        }
                    }
                }


                foreach (var item in sqllite)
                {
                    if (item.vwstring3 == "I")
                    {
                        count.Add(item.vwstring1);
                        glay.vwitarray2[counter] = item.vwint7;
                        var bgassignc = (from bg in db.IV_001_ITEM
                                         where bg.item_code == item.vwstring1
                                         select bg).FirstOrDefault();
                        glay.vwdclarray1[counter] = bgassignc.selling_price_class1;
                        glay.vwstrarray1[counter] = item.vwstring1;
                        ViewData["extcostt" + counter] = item.vwdecimal1;
                        ViewData["measure" + counter] = item.vwstring8;
                        deccalcc += item.vwdecimal1;
                        glay.vwdclarray3[0] = deccalcc;
                        counter++;
                    }

                    else if (item.vwstring3 == "J")
                    {
                        count1.Add(item.vwstring1);
                        glay.vwstrarray5[counter1] = item.vwstring1;
                        glay.vwitarray0[counter1] = item.vwint7;
                        var bgemp = from bg in db.GB_001_EMP
                                    where bg.active_status == "N" && bg.job_role == Convert.ToInt32(item.vwstring1)
                                    select new { c1 = bg.employee_code, c2 = bg.name };

                        ViewBag.emp = new SelectList(bgemp.ToList(), "c1", "c2");
                        var bgassignc = (from bg in db.JB_001_JOB
                                         where bg.job_id == Convert.ToInt32(item.vwstring1)
                                         select bg).FirstOrDefault();
                        ViewData["headcount" + counter1] = item.vwint1;
                        ViewData["costphead" + counter1] = bgassignc.cost;
                        ViewData["totalcostt" + counter1] = item.vwint1 * bgassignc.cost;
                        deccalcc1 += item.vwint1 * bgassignc.cost;
                        glay.vwdclarray3[1] = deccalcc1;
                        counter1++;

                    }

                    else if (item.vwstring3 == "S")
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
                        glay.vwdclarray3[3] = deccalcc2;
                        counter3++;
                    }



                }
            }
            if (glay.vwint6 != 2)
            {
                if (count.Count != 0)
                    glay.vwint1 = count.Count;
                if (count1.Count != 0)
                    glay.vwint5 = count1.Count;
                if (count3.Count != 0)
                    glay.vwint3 = count3.Count;
                if (count2.Count != 0)
                    glay.vwint4 = count2.Count;
            }
            glay.vwdecimal0 = Math.Round(deccalcc + deccalcc1 + deccalcc2 + deccalcc3 + deccalcc4 + itcal, 2);
            
            if (glay.vwint7 == 1)
            {
                glay.vwstrarray2[1] = "tab-pane active";
                glay.vwstrarray2[0] = "tab-pane";
                glay.vwstrarray2[2] = "tab-pane";
                glay.vwstrarray2[3] = "tab-pane";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[5] = "tab-pane";
            }
            else if (glay.vwint7 == 2)
            {
                glay.vwstrarray2[2] = "tab-pane active";
                glay.vwstrarray2[0] = "tab-pane";
                glay.vwstrarray2[1] = "tab-pane";
                glay.vwstrarray2[3] = "tab-pane";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[5] = "tab-pane";
            }
            else if (glay.vwint7 == 3)
            {
                glay.vwstrarray2[3] = "tab-pane active";
                glay.vwstrarray2[0] = "tab-pane";
                glay.vwstrarray2[2] = "tab-pane";
                glay.vwstrarray2[1] = "tab-pane";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[5] = "tab-pane";

            }
            else if (glay.vwint7 == 4)
            {
                glay.vwstrarray2[4] = "tab-pane active";
                glay.vwstrarray2[0] = "tab-pane";
                glay.vwstrarray2[2] = "tab-pane";
                glay.vwstrarray2[3] = "tab-pane";
                glay.vwstrarray2[1] = "tab-pane";
                glay.vwstrarray2[5] = "tab-pane";
            }
            else if (glay.vwint7 == 5)
            {
                glay.vwstrarray2[5] = "tab-pane active";
                glay.vwstrarray2[0] = "tab-pane";
                glay.vwstrarray2[2] = "tab-pane";
                glay.vwstrarray2[3] = "tab-pane";
                glay.vwstrarray2[4] = "tab-pane";
                glay.vwstrarray2[1] = "tab-pane";

            }
        }
    }
}
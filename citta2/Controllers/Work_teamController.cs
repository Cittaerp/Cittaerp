using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;

namespace CittaErp.Controllers
{
    public class Work_teamController : Controller
    {
        // GET: Work_team
        WT_001_WKT WT_001_WKT = new WT_001_WKT(); 

       MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        string[] container = new string[] { };

       
        bool err_flag;
        string action_flag = "";
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];

            var bglist = (from bh in db.WT_001_WKT
                          select new vw_genlay
                          {
                              vwstring0 = bh.work_team_id,
                              vwstring1 = bh.description,
                              vwstring2 = bh.staff_id,
                          }).Distinct();

            return View(bglist.ToList());
        }
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
        public ActionResult Create(vw_genlay glay_in, string[] vwstrarray0)
        {
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            update_file();
            if (err_flag)
                return RedirectToAction("Create");
            initial_rtn();
            select_query();
            return View(glay);
        }
        public ActionResult Edit(string key1, string key2)
        {
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            WT_001_WKT = db.WT_001_WKT.Find(key1, key2);
            if (WT_001_WKT != null)
            read_record(key1);
            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, string name)
        {
            pubsess = (pubsess)Session["pubsess"];
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt == "D")
            {
                delete_record();
                return RedirectToAction("Index");
            }
            ViewData["store"] = name;
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
            string sqlstr = "delete from [dbo].[WT_001_WKT] where work_team_id='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            WT_001_WKT = db.WT_001_WKT.Find(glay.vwstring0);
            if (WT_001_WKT != null)
            {
                db.WT_001_WKT.Remove(WT_001_WKT);
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
                WT_001_WKT = new WT_001_WKT();
                WT_001_WKT.created_by = pubsess.userid;
                WT_001_WKT.created_date = DateTime.UtcNow;
            }
            else
            {
                // WT_001_WKT = db.WT_001_WKT.Find(glay.vwstring0,glay.vwstrarray0[0]);
                string str1 = "delete from WT_001_WKT where work_team_id =" + util.sqlquote(glay.vwstring0);
                db.Database.ExecuteSqlCommand(str1);

            }

           
            for (int i = 0; i< glay.vwstrarray0.Length; i+=2) {
                WT_001_WKT WT_001_WKT = new WT_001_WKT();
                if (glay.vwstrarray0[i] != "") { 
            WT_001_WKT.work_team_id = glay.vwstring0;
            WT_001_WKT.description = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;

            WT_001_WKT.staff_id = string.IsNullOrWhiteSpace(glay.vwstrarray0[i]) ? "" : glay.vwstrarray0[i];
            WT_001_WKT.competence = string.IsNullOrWhiteSpace(glay.vwstrarray0[i+1]) ? "" : glay.vwstrarray0[i+1];

                WT_001_WKT.modified_date = DateTime.UtcNow;
            WT_001_WKT.modified_by = pubsess.userid;
            WT_001_WKT.inactive_status = glay.vwbool0 ? "Y" : "N";
            WT_001_WKT.note = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            WT_001_WKT.created_by = pubsess.userid;
            WT_001_WKT.created_date = DateTime.UtcNow;

                //if (Session["action_flag"].ToString() == "Create")
                db.Entry(WT_001_WKT).State = EntityState.Added;
            //else
            //    db.Entry(WT_001_WKT).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }

            catch (Exception err)
            {
                if (err.InnerException == null)
                    ModelState.AddModelError(String.Empty, err.Message);
                else
                    //ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);

                        if (err.InnerException.InnerException.Message != null)
                        {
                            ModelState.AddModelError(String.Empty, "Can not accept duplicates for Staff ID");
                            err_flag = false;
                        }

               
            }
                }
            }

        }
        private void validation_routine()
        {
            string error_msg = "";

            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                ModelState.AddModelError(String.Empty, "Please insert Work Team ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[0]))
            {
                ModelState.AddModelError(String.Empty, "Please specify Staff ID");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstrarray0[1]))
            {
                ModelState.AddModelError(String.Empty, "Please specify Competence Level");
                err_flag = false;
            }
            

           if(action_flag == "Create")
            {
                WT_001_WKT bnk = db.WT_001_WKT.Find(glay.vwstring0,glay.vwstrarray0[0]);
                if (bnk != null)
                    error_msg = "Work Team ID exists";

                var desc = (from bg in db.WT_001_WKT
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

            if (error_msg != "")
            {
                ModelState.AddModelError(String.Empty, error_msg);
                err_flag = false;
            }
        }
        private void select_query()
        {
            return_list();

            var stf = from bh in db.GB_001_EMP
                      select bh;
            ViewBag.stf1 = new SelectList(stf.ToList(), "employee_code", "name");

            var tcl = from bg in db.TC_001_TCL
                      select bg;
            ViewBag.tcl1 = new SelectList(tcl.ToList(), "technical_competency_level_id", "description");

        }
        public void return_list()
        {
            var query = container.Select((r) => new { Text = r, Value = r });
            ViewBag.selected_staff = new SelectList(query.ToList(), "Value", "Text");
        }
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
        }
        private void read_record(string id)
        {
            List<SelectListItem> new_list = new List<SelectListItem>();
           glay.vwstrarray0 = new string[50];
            glay.vwstring0 = WT_001_WKT.work_team_id;
            glay.vwstring1 = WT_001_WKT.description;

            int a = 0;
            int b = 1;

            var staff = (from bg in db.WT_001_WKT
                        where bg.work_team_id == id
                        select bg.staff_id).ToList();

            int num = staff.Count();
            ViewData["store"] = num;

          
            
                var staffing = from bh in db.WT_001_WKT
                               where bh.work_team_id == id
                               select new { bh.staff_id, bh.competence };

                foreach(var st1 in staffing)
                {
                    glay.vwstrarray0[a] = st1.staff_id;
                    glay.vwstrarray0[b] = st1.competence;

                    a += 2;
                    b += 2;
                }


           // glay.vwstring2 = WT_001_WKT.staff_id;

            if (WT_001_WKT.inactive_status == "Y")
            {
                glay.vwbool0 = true;
            }
            glay.vwstring3 = WT_001_WKT.note;






            //foreach (var p in pieces)
            //{
            //    var pin = (from bh in db.GB_001_EMP
            //               where bh.employee_code == p
            //               select bh.name).FirstOrDefault();

            //    var pin2 = (from bh in db.GB_001_EMP
            //                where bh.employee_code == p
            //                select bh.employee_code).FirstOrDefault();

            //  new_list.Add(new SelectListItem { Value = pin2, Text = pin });
            //}

            //ViewBag.edited_staff = new SelectList(new_list.ToList(), "Value", "Text");







        }
        private void error_message()
        {

        }

    }
}
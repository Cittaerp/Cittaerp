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

    public class OrganUnitsController : Controller
    {
        // GET: OrganUnits
        GB_001_ORG GB_001_ORG = new GB_001_ORG();
        cittautil util = new cittautil();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        comsess comsess;
        psess psess;
        bool err_flag = true;
        string pc1;
        string err_msg = "";
        string laction = "";
        string str1;
        string action_flag = "";
        public ActionResult Index(string pc = null)
        {
            util.init_values();

            comsess = (comsess)Session["comsess"];
            psess = (psess)Session["psess"];
            if (!(string.IsNullOrWhiteSpace(pc)))
            {
                psess.temp0 = pc;
            }
            pc = psess.temp0.ToString();

            if (pc == null)
            {
                return HttpNotFound();
            }

            pc = psess.temp0.ToString();
            nation_title(pc);
            
            var bglist = from bg in db.GB_001_ORG
                         join bh in db.GB_001_EMP
                        on new { a1 = bg.md_name } equals new { a1 = bh.employee_code }
                        into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         where (bg.flag == pc)
                         orderby bg2.employee_code
                         select new vw_genlay
                         {
                             vwstring0 = bg.org_id,
                             vwstring1 = bg.org_name,
                             vwstring2 = bg2.name
                         };
            Session["psess"] = psess;
            return View(bglist.ToList());
        }

        public ActionResult Create(string xy = null)
        {
            glay.vwstring10 = "C";
            //if (utils.check_option() == 1 || xy != "1")
            //    return RedirectToAction("Welcome", "Home");

            comsess = (comsess)Session["comsess"];
            psess = (psess)Session["psess"];
            pc1 = psess.temp0.ToString();
            write_temp("", "");
            initial_rtn();
            select_query();
            Session["psess"] = psess;
            return View("Edit", glay);
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, string[] sname, string[] sname2)
        {
            comsess = (comsess)Session["comsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;
            laction = "Create";
            pc1 = psess.temp0.ToString();
            select_write(sname);
            update_file();

            if (err_flag)
                return RedirectToAction("Create");
            select_query();
            initial_rtn();
            glay.vwstring10 = "C";
            Session["psess"] = psess;
            return View("Edit", glay);
        }

        private void update_file()
        {
            err_flag = true;
            validation_routine();

            if (err_flag)
                update_record();
        }


        public ActionResult Edit(string jy = null)
        {
            glay.vwstring10 = "E";
            //if (uti.check_option() == 1 || jy == null)
            //    return RedirectToAction("Welcome", "Home");

            comsess = (comsess)Session["comsess"];
            psess = (psess)Session["psess"];
            pc1 = psess.temp0.ToString();

            GB_001_ORG = db.GB_001_ORG.Find(jy, pc1);
            if (GB_001_ORG == null)
            {
                return RedirectToAction("Index"/*, null, new { anc = Ccheckg.convert_pass2("pc=") }*/);
            }

            read_record();
            write_temp("R", jy);
            initial_rtn();
            select_query();
            Session["psess"] = psess;
            return View(glay);
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string[] sname, string xhrt)
        {
            comsess = (comsess)Session["comsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;
            pc1 = psess.temp0.ToString();

            if (xhrt == "D")
            {
                delete_record();
                return RedirectToAction("Index"/*, null, new { anc = Ccheckg.convert_pass2("pc=") }*/);
            }

            select_write(sname);
            update_file();

            if (err_flag)
                return RedirectToAction("Index"/*, null, new { anc = Ccheckg.convert_pass2("pc=") }*/);

            select_query();
            initial_rtn();
            glay.vwstring10 = "E";
            Session["psess"] = psess;
            return View(glay);

        }

        private void delete_record()
        {
            GB_001_ORG = db.GB_001_ORG.Find(pc1, glay.vwstring0);
            write_log("Delete");
            db.GB_001_ORG.Remove(GB_001_ORG);
            db.SaveChanges();
            string str1 = "";

            if (pc1 == "02")
                str1 = "update GB_001_EMP set approval_category='' where approval_category =" + util.sqlquote(glay.vwstring0);
            else if (pc1 == "01")
                str1 = "update GB_001_EMP set approval_route='' where approval_route =" + util.sqlquote(glay.vwstring0);
            else if (pc1 == "03")
                str1 = "delete  from GB_001_EMP where para_code='H43' and array_code=" + util.sqlquote(glay.vwstring0);

            db.Database.ExecuteSqlCommand(str1);

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            psess = (psess)Session["psess"];
            pc1 = psess.temp0.ToString();
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_ORG] where org_id=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            if (pc1 == "02")
                str1 = "update GB_001_EMP set approval_category='' where approval_category =" + util.sqlquote(id);
            else if (pc1 == "01")
                str1 = "update GB_001_EMP set approval_route='' where approval_route =" + util.sqlquote(id);
            else if (pc1 == "03")
                str1 = "delete  from GB_001_EMP where para_code='H43' and array_code=" + util.sqlquote(id);

            return RedirectToAction("Index");
        }

        private void validation_routine()
        {
            if (string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                err_msg = "Description can not be spaces";
                ModelState.AddModelError(String.Empty, err_msg);
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                err_msg = "Indicate Code for Organisation Unit";
                ModelState.AddModelError(String.Empty, err_msg);
                err_flag = false;
            }

            if (laction == "Create")
            {
                GB_001_ORG cbank = db.GB_001_ORG.Find(pc1, glay.vwstring0);
                if (cbank != null)
                {
                    err_msg = "Record Already Created";
                    ModelState.AddModelError(String.Empty, err_msg);
                    err_flag = false;
                }

            }

        }


        private void read_record()
        {
            glay.vwstring0 = GB_001_ORG.org_id;
            glay.vwstring1 = GB_001_ORG.org_name;
            glay.vwstring2 = GB_001_ORG.rep_name;
            glay.vwstring3 = GB_001_ORG.md_name;
        }
        private void select_query()
        {
            var bgemp = from bg in db.GB_001_EMP
                        select bg;
            ViewBag.emp = new SelectList(bgemp.ToList(), "employee_code", "name", glay.vwstring2);

            if (glay.vwstring10 == "C")
                ViewBag.deed = new SelectList("", "");
            else
            {
                //var bgfix = (from bh in db.GB_001_EMP
                //             where bh.group_flag == glay.vwstring0
                //             select bh);
                //ViewBag.deed = new SelectList(bgfix.ToList(), "fixed_asset_code", "description");
            }
        }

        private void initial_rtn()
        {
            string str1 = "";
            //' read temp file
            str1 = "select report_code query0,name query1 from tab_temp_rep a, GB_001_EMP b ";
            str1 += " where report_line='DET' and user_code=" + util.sqlquote(comsess.userid) + " and report_code=employee_code order by 2 ";
            var str2 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.list2 = new SelectList(str2.ToList(), "query0", "query1");

            //' read outstanding if there
            str1 = "select employee_code query0, name query1 from GB_001_EMP  ";
            //close code means active members..Default =0
            str1 += " where close_code = '0' and not employee_code in (select report_code from tab_temp_rep b where ";
            str1 += " report_line='DET' and user_code=" + util.sqlquote(comsess.userid) + ") ";
            str1 += " order by 2";

            str2 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.list1 = new SelectList(str2.ToList(), "query0", "query1");
        }

        private void select_write(string[] sname)
        {
            str1 = "delete tab_temp_rep where user_code=" + util.sqlquote(comsess.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (sname != null)
            {
                foreach (var bh in sname)
                {
                    str1 = "insert into tab_temp_rep(report_line,user_code,op1,rep_count) values ('DET'," + util.sqlquote(comsess.userid) + ",";
                    str1 += util.sqlquote(bh) + ",1) ";
                    db.Database.ExecuteSqlCommand(str1);
                }
            }

        }


        private void update_record()
        {
            if(glay.vwstring10== "C")
            {
                GB_001_ORG = new GB_001_ORG();
                GB_001_ORG.created_by = comsess.userid;
                GB_001_ORG.created_date = DateTime.UtcNow;
                
            }
            else
            {
                GB_001_ORG = db.GB_001_ORG.Find( glay.vwstring0, pc1);
            }
            //string temp = glay.vwstring0;
            //ModelState.Remove("vwstring0");
            //glay.vwstring0 = temp;
            GB_001_ORG.org_id = glay.vwstring0;
            GB_001_ORG.org_name = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
            GB_001_ORG.md_name = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;
            GB_001_ORG.rep_name = "";
            GB_001_ORG.flag = pc1;
            GB_001_ORG.modified_date = DateTime.UtcNow;
            GB_001_ORG.modified_by = comsess.userid;
            if (glay.vwstring10== "C")
                db.Entry(GB_001_ORG).State = EntityState.Added;
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
                if (pc1 == "01")
                {
                    str1 = "update GB_001_EMP set approval_route= '' where approval_route=" + util.sqlquote(GB_001_ORG.org_id);
                    db.Database.ExecuteSqlCommand(str1);

                    str1 = "update GB_001_EMP set approval_route=" + util.sqlquote(GB_001_ORG.org_id) + " from GB_001_EMP a, tab_temp_rep b ";
                    str1 += " where employee_code=op1 and report_line='DET' and user_code=" + util.sqlquote(comsess.userid);
                    db.Database.ExecuteSqlCommand(str1);
                }
                else if (pc1 == "02")
                {
                    str1 = "update GB_001_EMP set approval_category= '' where approval_category=" + util.sqlquote(GB_001_ORG.org_id);
                    db.Database.ExecuteSqlCommand(str1);

                    str1 = "update GB_001_EMP set approval_category=" + util.sqlquote(GB_001_ORG.org_id) + " from GB_001_EMP a, tab_temp_rep b ";
                    str1 += " where employee_code=op1 and report_line='DET' and user_code=" + util.sqlquote(comsess.userid);
                    db.Database.ExecuteSqlCommand(str1);
                }
                else if (pc1 == "03")
                {
                    str1 = "delete from tab_array where para_code='H43' and array_code=" + util.sqlquote(GB_001_ORG.org_id);
                    db.Database.ExecuteSqlCommand(str1);

                    str1 = "insert into tab_array(para_code,array_code,count_array,operand) ";
                    str1 += " select 'H43'," + util.sqlquote(GB_001_ORG.org_id) + ",row_number() over(order by op1),op1 from tab_temp_rep  ";
                    str1 += " where report_line='DET' and user_code=" + util.sqlquote(comsess.userid);

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

                }

                write_temp("", "");
               //write_log(laction);

            }
        }

        private void nation_title(string pc)
        {
            if (pc == "01")
            {
                psess.temp1 = "Organisation Unit";
                psess.temp7 = "Organisation Unit";
                psess.temp2 = "Unit Head";
            }

            else if (pc == "02")
            {
                psess.temp1 = "Alternate Approval Grouping";
                psess.temp7 = "Alternate Approval";
            }

            else if (pc == "03")
            {
                psess.temp1 = "Team Grouping";
                psess.temp7 = "Team Group";
                psess.temp2 = "Team Document";
            }
            else if (pc == "04")
            {
                psess.temp1 = "Delegate Approval Definition";
                psess.temp7 = "Delegate Approval";
                psess.temp2 = "Team Document";
            }

        }

        private void write_temp(string action, string daily_code)
        {
            str1 = "Delete from tab_temp_rep where user_code=" + util.sqlquote(comsess.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (action == "R")
            {
                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,op1) ";
                str1 += " select 'DET'," + util.sqlquote(comsess.userid) + ",employee_code,1 ,employee_code from ";
                str1 += " GB_001_EMP where close_code='0' and ";
                if (pc1 == "01")
                    str1 += "approval_route= " + util.sqlquote(daily_code);
                else if (pc1 == "02")
                    str1 += "approval_category= " + util.sqlquote(daily_code);
                else if (pc1 == "03")
                {
                    str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,op1) ";
                    str1 += " select 'DET'," + util.sqlquote(comsess.userid) + ",operand,1 ,employee_code from ";
                    str1 += " tab_staff a, tab_array b where close_code='0' and para_code='H43' and operand=employee_code and array_code=" + util.sqlquote(daily_code);
                }
                db.Database.ExecuteSqlCommand(str1);
            }

        }

        private void write_log(string option)
        {
            string opt;
            if (option == "Create")
                opt = "N";
            else if (option == "Delete")
                opt = "D";
            else
                opt = "A";

            string select1 = " where flag=" + util.sqlquote(GB_001_ORG.flag) + " and org_id=" + util.sqlquote(GB_001_ORG.org_id);
            util.write_plog(GB_001_ORG.flag, select1, opt, "A", comsess.userid, "", GB_001_ORG.org_id);

        }
    }
}

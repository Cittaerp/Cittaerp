using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;

namespace CittaErp.Controllers
{

    public class Log_inController : Controller
    {
        //
        // GET: /Log_in/
        bool err_flag = true;
        GB_001_LOGIN GB_001_LOGIN = new GB_001_LOGIN();
        GB_001_COY GB_001_COY = new GB_001_COY();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        comsess comsess;
        cittautil util = new cittautil();
        psess psess;
        bool yes = false;
        bool yes1 = false;
        string action_flag = "";
        public ActionResult List()
        {
            util.init_values();
            comsess = (comsess)Session["comsess"];

            var bglist = from bh in db.GB_001_LOGIN
                         select new vw_genlay
                         {
                             vwstring0 = bh.username,
                             vwstring1 = bh.password,
                         };

            return View(bglist.ToList());
        }

        public ActionResult Home()  
        {
            util.init_values();
            comsess = (comsess)Session["comsess"];
            psess = new psess();
            psess.sarrayt0 = new string[20];
            psess.sarrayt1 = new string[20];
            Session["psess"] = psess;

            select_query();
            var str1 = "";
            char spc = '"';

            //string query = "select * vwstrarray0 from tab_soft where para_code = 'MAINSCRN' and rep_name2 = 'H'";
            //query += "order by case IsNumeric(report_name6) when 1 then Replicate('0', 100 - Len(report_name6)) +report_name6";
            //query += "else report_name6 end, report_name1";
            //var bglist = db.Database.SqlQuery<vw_genlay>(query);
            var bglist1 = (from bg in db.tab_soft
                          where bg.para_code == "MAINSCRN" && bg.rep_name2 == "H"
                          select bg).ToList();
            var bglist = bglist1.OrderBy(t => Convert.ToInt32(t.report_name6));
            foreach (var item in bglist)
            {

                str1 += "<li class=" + spc + "dropdown" + spc + ">\r\n";
                str1 += "<a href=" + "#" + " class=" + "dropdown-toggle" + " data-toggle=" + "dropdown" + "><b>" + item.report_name1 + "</b><span class=" + "caret" + "></span><span style = " + "font-size:12px;" + " class=" + "pull-right hidden-xs showopacity glyphicon glyphicon-home" + "></span></a>\r\n";
                str1 += "<ul class=" + "dropdown-menu" + " role=" + "menu" + ">\r\n";

                var bglist11 = (from bg in db.tab_soft
                               where bg.numeric_ind == item.report_code && bg.rep_name2 != "H"
                               select bg).ToList();
                var bglist2 = bglist11.OrderBy(t => Convert.ToInt32(t.report_name6));
                foreach (var sbitem in bglist2)
                {
                    str1 += "<li class=" + "dropdown-submenu" + ">\r\n";
                    str1 += "<a href = " + "#" + " class=" + "dropdown-toggle" + " data-toggle=" + "dropdown" + " id=" + "drop" + ">" + sbitem.report_name1 + "<span class=" + "caret" + "></span></a>\r\n";
                    str1 += "<ul class=" + "dropdown-menu a" + " role=" + "menu" + ">\r\n";
                    var bglist111 = (from bg in db.tab_soft
                                   where bg.para_code == "ITEM" && bg.rep_name2 == sbitem.rep_name1
                                   select new
                                   {
                                       c1 = bg.para_code,
                                       c2 = bg.report_code,
                                       c3 = bg.report_name1,
                                       c4 = bg.report_name2,
                                       c5 = bg.report_name3,
                                       c6 = bg.report_name4,
                                       c7 = bg.report_name5,
                                       c8 = bg.numeric_ind,
                                       c9 = bg.rep_name1,
                                       c10 = bg.rep_name2,
                                       c11 = bg.source_cat,
                                       c12 = bg.report_name6,
                                       c13 = bg.report_name7,
                                       c14 = bg.report_name8,
                                       c15 = bg.report_name9,
                                       c16 = bg.report_name10,
                                       a1 = "",
                                       a2 = "",
                                       a3 = "",
                                       a4 = "",
                                       a5 = "",
                                       a6 = "",
                                       a7 = "",
                                       a8 = "",
                                       a9 = "",
                                       a10 = "",
                                       a11 = "",
                                       a12 = "",
                                       a13 = "",
                                       a14 = "",
                                       a15 = "",
                                       a16 = "",
                                       a17 = "",
                                       a18 = "",
                                   }).ToList().Union(from bk in db.tab_calc
                                                     join bh in db.tab_soft
                                                     on new { a1 = bk.para_code, a2 = "UPER" } equals new { a1 = bh.report_code, a2 = bh.para_code }
                                                     into bk1
                                                     from bk2 in bk1.DefaultIfEmpty()
                                                     where bk.menu_option == sbitem.report_code
                                                     select new
                                                     {
                                                         c1 = bk.para_code,
                                                         c2 = bk.calc_code,
                                                         c3 = bk.name1,
                                                         c4 = bk2.report_name2,
                                                         c5 = bk2.report_name3,
                                                         c6 = bk2.report_name4,
                                                         c7 = bk2.report_name5,
                                                         c8 = bk2.numeric_ind,
                                                         c9 = bk2.rep_name1,
                                                         c10 = bk2.rep_name2,
                                                         c11 = bk2.source_cat,
                                                         c12 = bk2.report_name6,
                                                         c13 = bk2.report_name7,
                                                         c14 = bk2.report_name8,
                                                         c15 = bk2.report_name9,
                                                         c16 = bk2.report_name10,
                                                         a1 = "",
                                                         a2 = "",
                                                         a3 = "",
                                                         a4 = "",
                                                         a5 = "",
                                                         a6 = "",
                                                         a7 = "",
                                                         a8 = "",
                                                         a9 = "",
                                                         a10 = "",
                                                         a11 = "",
                                                         a12 = "",
                                                         a13 = "",
                                                         a14 = "",
                                                         a15 = "",
                                                         a16 = "",
                                                         a17 = "",
                                                         a18 = "",
                                                     }
                                            ).ToList();
                    var bglist3 = bglist111.OrderBy(t => Convert.ToInt32(t.c12));
                    foreach (var sbbitem in bglist3)
                    {
                        //controller and action method available
                        if (sbbitem.c4 != "" && sbbitem.c5 != "" && sbbitem.c6 == "" && sbbitem.c7 == "")
                        {
                            str1 += "<li><a href = " + spc + "/" + sbbitem.c5 + "/" + sbbitem.c4 + spc + ">" + sbbitem.c3 + "</a></li>\r\n";

                        }
                        //controller but no action method
                        else if (sbbitem.c4 == "" && sbbitem.c5 != "" && sbbitem.c6 == "" && sbbitem.c7 == "")
                        {
                            str1 += "<li><a href = " + spc + "/" + sbbitem.c5 + "/" + "Index" + spc + ">" + sbbitem.c3 + "</a></li>\r\n";
                        }
                        //controller, action method and parameters available
                        else if (sbbitem.c4 != "" && sbbitem.c5 != "" && sbbitem.c6 != "" && sbbitem.c7 == "")
                        {
                            if(sbbitem.c6!="MENU")
                            str1 += "<li><a href = " + spc + "/" + sbbitem.c5 + "/" + sbbitem.c4 + "/?" + sbbitem.c6 + spc + ">" + sbbitem.c3 + "</a></li>\r\n";
                            //For Dynamic Reports
                            else
                            str1 += "<li><a href = " + spc + "/" + sbbitem.c5 + "/" + sbbitem.c4 + "/?" + "pcode="+sbbitem.c1+"&pc="+sbbitem.c2 + spc + ">" + sbbitem.c3 + "</a></li>\r\n";
                            //else if (sbbitem.report_name2 != "" && sbbitem.report_name3 != "" && sbbitem.report_name4 != "" && sbbitem.report_name5 != "")
                            //    str1 += "<li><a href = " + spc + "/" + sbbitem.report_name3 + "/" + sbbitem.report_name2 + "/" + sbbitem.report_name4 + spc + ">" + sbbitem.report_name1 + "</a></li>\r\n";

                        }
                        else
                        {
                            var bglist1111 = (from bg in db.tab_soft
                                           where bg.para_code == "ITEMD" && bg.rep_name2 == sbbitem.c2
                                           orderby bg.report_name6
                                           select bg).ToList();
                            var bglist4 = bglist1111.OrderBy(t => Convert.ToInt32(t.report_name6));
                           str1 += "<li>\r\n";
                            //for non sub-sub menu
                            if (bglist4.Count() == 0)
                                str1 += "<li><a href=" + "#" + ">" + sbbitem.c3 + "</a></li>\r\n";
                            //for a sub-sub menu
                            else
                            {
                                str1 += "<a href=" + spc + "#" + spc + "class=" + spc + "dropdown-toggle" + spc + "data-toggle=" + spc + "dropdown" + spc + ">" + sbbitem.c3 + "<span class=" + spc + "caret" + spc + "></span><span></span></a>\r\n";
                                str1 += "<ul class=" + spc + "dropdown-menu a" + spc + " role=" + spc + "menu" + spc + ">\r\n";

                                foreach (var sbbbitem in bglist4)
                                {
                                    //both controller and action method available
                                    if (sbbbitem.report_name2 != "" && sbbbitem.report_name3 != "" && sbbbitem.report_name4 == "" && sbbbitem.report_name5 == "")
                                        str1 += "<li><a href = " + spc + "/" + sbbbitem.report_name3 + "/" + sbbbitem.report_name2 + spc + ">" + sbbbitem.report_name1 + "</a></li>\r\n";
                                    //only controller available without action method and parameter
                                    else if (sbbbitem.report_name2 == "" && sbbbitem.report_name3 != "" && sbbbitem.report_name4 == "" && sbbbitem.report_name5 == "")
                                        str1 += "<li><a href = " + spc + "/" + sbbbitem.report_name3 + "/" + "Index" + spc + ">" + sbbbitem.report_name1 + "</a></li>\r\n";
                                    //controller, action method and parameter available
                                    else if (sbbbitem.report_name2 != "" && sbbbitem.report_name3 != "" && sbbbitem.report_name4 != "" && sbbbitem.report_name5 == "")
                                        str1 += "<li><a href = " + spc + "/" + sbbbitem.report_name3 + "/" + sbbbitem.report_name2 + "/?" + sbbbitem.report_name4 + spc + ">" + sbbbitem.report_name1 + "</a></li>\r\n";
                                    //no controller, action method, and parameter
                                    else
                                        str1 += "<li><a href=" + "#" + ">" + sbbbitem.report_name1 + "</a></li>\r\n";
                                }
                                str1 += "</ul>\r\n";
                            }
                            str1 += "</li>\r\n";
                        }
                    }
                    str1 += "</ul>\r\n";
                    str1 += "</li>\r\n";
                    str1 += "<li class=" + spc + "divider" + spc + "></li>\r\n";
                }
                str1 += "</ul>\r\n";
                str1 += "</li>\r\n";

                //Gltran_enq / Create ? ptype1 = 001
            }
            psess.temp10 = str1;
            Session["psess"] = psess;
            //            return View(viewName:"~/Views/Shared/menu.cshtml");
            return View();
        }

        public ActionResult Show(int id)
        {

            var item = (from bg in db.GB_001_COY
                        where bg.id_code == "COYINFO"
                        select bg).FirstOrDefault();

            byte[] imagedata = item.company_logo;
            return File(imagedata, "png");
        }


        public void select_query()
        {
            //    var baglist1 = from bh in db.GB_001_COY
            //                   select bh;
            //    ViewBag.Company = baglist1.ToList();



        }
        public ActionResult Index()
        {
            Session.RemoveAll();
            return View();

        }

        [HttpPost]
        public ActionResult Index(vw_genlay glay_in)
        {
            glay = glay_in;

            var log = from bh in db.GB_001_LOGIN
                      where bh.username == glay.vwstring0
                      select bh.username;

            var log1 = from bh in db.GB_001_LOGIN
                       where bh.password == glay.vwstring1
                       select bh.password;

            foreach (var Pass in log)
            {
                if (glay.vwstring0.Equals(Pass))
                {
                    yes = true;
                }
            }


            foreach (var Pass1 in log1)
            {
                if (glay.vwstring1.Equals(Pass1))
                {
                    yes1 = true;
                }
            }

            if (yes && yes1)
            {
                //return RedirectToAction("Index", "Exchange_rate");
                return RedirectToAction("Home");
            }
            return View();
        }




        public ActionResult Edit(string key1)
        {
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            comsess = (comsess)Session["comsess"];
            GB_001_LOGIN = db.GB_001_LOGIN.Find(key1);
            if (GB_001_LOGIN != null)
                read_record();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            comsess = (comsess)Session["comsess"];
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
            return View(glay);

        }
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[GB_001_LOGIN] where username='" + id + "'";
            int delctr = db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }
        private void delete_record()
        {
            GB_001_LOGIN = db.GB_001_LOGIN.Find(glay.vwstring0);
            if (GB_001_LOGIN != null)
            {
                db.GB_001_LOGIN.Remove(GB_001_LOGIN);
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
            if (action_flag == "Create")
            {
                GB_001_LOGIN = new GB_001_LOGIN();
            }
            else
            {
                GB_001_LOGIN = db.GB_001_LOGIN.Find(glay.vwstring0);
            }

            GB_001_LOGIN.username = glay.vwstring0;
            GB_001_LOGIN.password = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;

            if (action_flag == "Create")
                db.Entry(GB_001_LOGIN).State = EntityState.Added;
            else
                db.Entry(GB_001_LOGIN).State = EntityState.Modified;

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
            //if (string.IsNullOrWhiteSpace(glay.vwstring0))
            //    error_msg = "Please enter Id";

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Please enter Name;

            if (action_flag == "Create")
            {
                GB_001_LOGIN bnk = db.GB_001_LOGIN.Find(glay.vwstring0);
                if (bnk != null)
                {
                    ModelState.AddModelError(String.Empty, "Can not accept duplicates");
                    err_flag = false;
                }
            }


        }

        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
        }
        private void read_record()
        {
            glay.vwstrarray0 = new string[50];

            glay.vwstring0 = GB_001_LOGIN.username;
            glay.vwstring1 = GB_001_LOGIN.password;
        }

        public ActionResult slide_show()
        {
            return View();
        }
    }
}
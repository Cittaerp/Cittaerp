using CittaErp.Models;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class HomeController : Controller
    {
        MainContext db = new MainContext();
        cittautil utils = new cittautil();
        psess psess = new psess();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ViewResult menuPage()
        {
            var str1 = "";
            char spc = '"';

            var bglist = (from bg in db.tab_soft
                          where bg.para_code == "MAINSCRN" && bg.rep_name2 =="H" orderby bg.report_name6
                          select bg).ToList();
            foreach (var item in bglist)
            {

                str1 += "<li class=" + spc + "dropdown" + spc + ">\r\n";
                str1 += "<a href=" + "#" + " class=" + "dropdown-toggle" + " data-toggle=" + "dropdown" + "><b>" + item.report_name1 + "</b><span class=" + "caret" + "></span><span style = " + "font-size:12px;" + " class=" + "pull-right hidden-xs showopacity glyphicon glyphicon-home" + "></span></a>\r\n";
                str1 += "<ul class=" + "dropdown-menu" + " role=" + "menu" + ">\r\n";

                var bglist2 = (from bg in db.tab_soft
                               where bg.numeric_ind == item.report_code && bg.rep_name2!="H"
                               select bg).ToList();
                foreach (var sbitem in bglist2)
                {
                    str1 += "<li class=" + "dropdown-submenu" + ">\r\n";
                    str1 += "<a href = " + "#" + " class=" + "dropdown-toggle" + " data-toggle=" + "dropdown" + " id=" + "drop" + ">" + sbitem.report_name1 + "<span class=" + "caret" + "></span></a>\r\n";
                    str1 += "<ul class=" + "dropdown-menu a" + " role=" + "menu" + ">\r\n";
                    var bglist3 = (from bg in db.tab_soft
                                   where bg.para_code == "ITEM" && bg.rep_name2 == sbitem.rep_name1
                                   select bg).ToList();
                    foreach (var sbbitem in bglist3) { 
                        //controller and action method available
                        if(sbbitem.report_name2!="" && sbbitem.report_name3!="" && sbbitem.report_name4 == "" && sbbitem.report_name5 == "")
                        str1 += "<li><a href = "+spc+"/"+sbbitem.report_name3+"/"+sbbitem.report_name2+spc+">"+sbbitem.report_name1+"</a></li>\r\n";
                        //controller but no action method
                        else if (sbbitem.report_name2 == "" && sbbitem.report_name3 != "" && sbbitem.report_name4 == "" && sbbitem.report_name5 == "")
                        str1 += "<li><a href = " + spc + "/" + sbbitem.report_name3 + "/" + "Index"+ spc + ">" + sbbitem.report_name1 + "</a></li>\r\n";
                        //controller, action method and parameters available
                        else if (sbbitem.report_name2 != "" && sbbitem.report_name3 != "" && sbbitem.report_name4 != "" && sbbitem.report_name5 == "")
                            str1 += "<li><a href = " + spc + "/" + sbbitem.report_name3 + "/"+sbbitem.report_name2+"/"+sbbitem.report_name4+ spc + ">" + sbbitem.report_name1 + "</a></li>\r\n";
                        //else if (sbbitem.report_name2 != "" && sbbitem.report_name3 != "" && sbbitem.report_name4 != "" && sbbitem.report_name5 != "")
                        //    str1 += "<li><a href = " + spc + "/" + sbbitem.report_name3 + "/" + sbbitem.report_name2 + "/" + sbbitem.report_name4 + spc + ">" + sbbitem.report_name1 + "</a></li>\r\n";
                        else
                            str1 += "<li><a href="+"#"+">"+sbbitem.report_name1+"</a></li>\r\n";
                    }
                    str1 += "</ul>\r\n";
                    str1 += "</li>\r\n";
                }
                str1 += "</ul>\r\n";
                str1 += "</li>\r\n";
                //Gltran_enq / Create ? ptype1 = 001
            }
            psess.temp10 = str1;
            Session["psess"] = psess;
            return View(viewName:"~/Views/Shared/menu.cshtml");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using anchor1.Models;
using anchor1.Filters;
using anchor1.utilities;
using System.Data.Entity;

namespace anchor1.Controllers
{
    public class RepDgnController : Controller
    {
        //
        // GET: /RepDgn/

        private anchor1Context db = new anchor1Context();
        Boolean err_flag = true;
        string laction = "";
        vw_collect mcollect = new vw_collect();
        Cutil utils = new Cutil();
        tab_calc tab_calc = new tab_calc();
        mysessvals pblock;
        string type_code = "";
        string err_message;
        worksess worksess;

        

        [EncryptionActionAttribute]
        public ActionResult Index(string pc = null)
        {
            if (utils.check_option() == 1||pc==null)
                return RedirectToAction("Welcome", "Home");


            worksess = (worksess)Session["worksess"];
            if (pc == "")
            {
                type_code = worksess.pc;
            }
            else
            {
                worksess.pc = pc;
                type_code = pc;
                formula_title(pc);
                worksess.idrep = type_code;
                worksess.jp = "1";
                Session["worksess"] = worksess;
            }


            var blist = from s in db.tab_calc
                        join sd in db.tab_soft
                        on new { a1 = "RMENU", a2 = s.menu_option } equals new { a1 = sd.para_code, a2 = sd.report_code }
                        into sd2
                        from sd3 in sd2.DefaultIfEmpty()
                        where (s.para_code == type_code) 
                        orderby s.name1
                        select new vw_collect { ws_string0 = s.calc_code, ws_string1 = s.name1, ws_string2 = sd3.report_name1 };

            return View(blist.ToList());
        }

        //
        // GET: /Bank/Create

        [EncryptionActionAttribute]
        public ActionResult Create(string xy = null)
        {
            mcollect.datmode = "C";
            if (utils.check_option() == 1 || xy != "1")
                return RedirectToAction("Welcome", "Home");

            pblock = (mysessvals)Session["mysessvals"];
            worksess = (worksess)Session["worksess"];
            type_code = worksess.pc;

            init_class();
            write_temp("");
            bank_group_select();
            mcollect.ws_decimal7 = 1;
            insert_default();
            read_names_operand();
            check_trans_type();
            mcollect.ws_string1 = "N";
            mcollect.ws_string5 = "N";

            return View("Edit", mcollect);
        }

        //
        // POST: /Bank/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_collect mcollect1)
        {
            pblock = (mysessvals)Session["mysessvals"];
            mcollect = mcollect1;
            worksess = (worksess)Session["worksess"];
            type_code = worksess.pc;

            laction = "Create";

            write_data();
            if (err_flag)
                return RedirectToAction("Create", null, new { anc = Ccheckg.convert_pass2("xy=1") });

            bank_group_select();
            read_names_operand();
            mcollect.datmode = "C";

            return View("Edit", mcollect);

        }

        //

        [EncryptionActionAttribute]
        public ActionResult Edit(string jy = null)
        {
            mcollect.datmode = "E";
            if (utils.check_option() == 1 || jy == null)
                return RedirectToAction("Welcome", "Home");

            pblock = (mysessvals)Session["mysessvals"];
            worksess = (worksess)Session["worksess"];
            type_code = worksess.pc;

            tab_calc = db.tab_calc.Find(type_code, jy);
            if (tab_calc == null)
            {
                return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            read_daily_record();
            bank_group_select();
            return View(mcollect);
        }

        //
        // POST: /Bank/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(vw_collect mcollect1,string xhrt)
        {
            mcollect = mcollect1;
            pblock = (mysessvals)Session["mysessvals"];
            worksess = (worksess)Session["worksess"];
            type_code = worksess.pc;

            if (xhrt == "D")
            {
                DeleteConfirmed();
                return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            write_data();
            if (err_flag)
                return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});

            bank_group_select();
            read_names_operand();
            mcollect.datmode = "E";

            return View(mcollect);

        }

        //
        // POST: /Bank/Delete/5

        //[HttpPost, ActionName("Delete")]
        private void DeleteConfirmed()
        {
            tab_calc = db.tab_calc.Find(type_code, mcollect.ws_code);
            write_log("Delete");
            db.tab_calc.Remove(tab_calc);
            db.SaveChanges();

            var str1 = "delete from tab_array where para_code=" + utils.pads(type_code) + " and array_code=" + utils.pads(mcollect.ws_code);
            db.Database.ExecuteSqlCommand(str1);
            str1 = "delete from tab_array where para_code='" + type_code + "S' and array_code=" + utils.pads(mcollect.ws_code);
            db.Database.ExecuteSqlCommand(str1);

            //return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void bank_group_select()
        {
            mcollect.ws_string10 = type_code;
            string str1;
            // foldercodes
            var bglist2 = (from bg in db.tab_soft
                           where (bg.para_code == "QUERY" && bg.report_code == type_code)
                           select bg).First();
            var report_name2 = bglist2.report_name2;
            worksess.temp4 = bglist2.report_name3;
            Session["worksess"] = worksess;
            report_name2 = report_name2.Replace("'", "");

            string[] opt_str;
            string[] sepa_str = new string[] { "," };

            opt_str = report_name2.Split(sepa_str, StringSplitOptions.None);

            var bglist3 = from bg2 in db.tab_soft
                          where (bg2.para_code == "QUERY" && opt_str.Contains(bg2.report_code))
                          orderby bg2.numeric_ind, bg2.report_name1
                          select bg2;

            ViewBag.folders = new SelectList(bglist3.ToList(), "report_code", "report_name1");

            var bgmenu = from bg2 in db.tab_soft
                         where bg2.para_code == "RMENU" && bg2.report_name5 == "E" 
                         orderby bg2.report_name4, bg2.report_name3
                         select bg2;

            //if (mcollect.ws_string3 == null)
            //    ViewBag.menu_group = new SelectList(bgmenu.ToList(), "report_code", "report_name1", worksess.temp3.ToString());
            //else
            //    ViewBag.menu_group = new SelectList(bgmenu.ToList(), "report_code", "report_name1", mcollect.ws_string3);

            var tmenu = from bg2 in db.tab_type
                         select new {c1="x" + bg2.trans_type,c2=bg2.name1};

            var exptype = from bg2 in db.tab_calc
                          where bg2.para_code == "H46"
                          select new { c1 = "q" + bg2.calc_code, c2 = bg2.name1 };

            var typecomb = tmenu.Concat(exptype).OrderBy(u => u.c2);
            ViewBag.trans_type = new SelectList(typecomb.ToList(), "c1", "c2", mcollect.ws_string6);

            var uname = (string)worksess.temp1;
            var bgsort1 = from bg2 in db.tab_soft
                          where bg2.para_code == "DSEL" && bg2.report_name5 == "Y"
                          select new { c1 = bg2.report_code, c2 = bg2.report_name1 };
            if (type_code == "HA07")
            {
                bgsort1 = from bg2 in db.tab_soft
                          where bg2.para_code == "DSEL" && bg2.report_name3 == "Y"
                          select new { c1 = bg2.report_code, c2 = bg2.report_name1 };

            }
            else if (type_code == "P04")
            {
                bgsort1 = from bg2 in db.tab_soft
                          where bg2.para_code == "DSEL" && bg2.report_name10 == "Y"
                          select new { c1 = bg2.report_code, c2 = bg2.report_name1 };
            }


            var bgsort2 = from bg2 in db.tab_analy
                          where bg2.para_code == "ANALY"
                          select new { c1 = bg2.analy_code, c2 = bg2.analy0 };

            //var bgsort11 = bgsort1.Concat(bgsort2).OrderBy(u => u.c2);
            //ViewBag.sort_query = new SelectList(bgsort11.ToList(), "c1", "c2");

            if (type_code == "H15")
            {
                str1 = "select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name5='Y' union all ";
                str1 += "select analy_code as c1, analy0 as c2, analy1 as c31 from tab_analy where para_code in ('ANALY') union all ";
                str1 += "select 'LINE' + ltrim(str(count_type)) as c1, rtrim(text_line) + '  T' as c2, '' from tab_type2, vw_hrcode where trans_type=" + utils.pads(worksess.temp5) + "  and text_code=vcode order by 2 ";
                //var bgsort12 = db.Database.SqlQuery<vw_query>(str1);
              //  ViewBag.sort_query = new SelectList(bgsort12.ToList(), "c1", "c2");
            }

            List<SelectListItem> sortad = new List<SelectListItem>();
            sortad.Add(new SelectListItem { Text = "Ascending", Value = "0", Selected=true });
            sortad.Add(new SelectListItem { Text = "Descending", Value = "1" });
            ViewBag.sort_order = sortad;

            List<SelectListItem> gratad = new List<SelectListItem>();
            gratad.Add(new SelectListItem { Text = "Gratuity", Value = "G", Selected = true });
            gratad.Add(new SelectListItem { Text = "Gratuity Provision", Value = "PG" });
            ViewBag.grat_type = gratad;

        }

        private void write_data()
        {
            err_flag = true;
            validate_fields();

            if (err_flag)
                write_rec();
        }

        private void write_rec()
        {
            DateTime odate = utils.logdatetime();
            update_seq();

            if (laction == "Create")
            {
                tab_calc = new tab_calc();
                tab_calc.created_by = pblock.userid;
                tab_calc.date_created = odate;
                tab_calc.internal_use = "N";
            }
            else
            {
                tab_calc = db.tab_calc.Find(type_code, mcollect.ws_code);
            }

            mcollect.ws_string1 = mcollect.ws_bool0 ? "Y" : "N";
            mcollect.ws_string5 = mcollect.ws_bool1 ? "Y" : "N";
            mcollect.ws_string2 = mcollect.ar_bool0[5] ? "Y" : "N";

            tab_calc.para_code = type_code;
            tab_calc.calc_code = mcollect.ws_code.Trim();
            tab_calc.name1 = mcollect.ws_string0;
            tab_calc.report_name = mcollect.ws_string6;
            tab_calc.amended_by = pblock.userid;
            tab_calc.date_amended = odate;
            tab_calc.column_no = Convert.ToInt16 (mcollect.ws_decimal1);
            tab_calc.line_spacing = mcollect.ws_decimal0 ;
            tab_calc.report_type = mcollect.ws_string4 ;
            tab_calc.menu_option = string.IsNullOrWhiteSpace(mcollect.ws_string3) ? worksess.temp3.ToString() : mcollect.ws_string3;
            tab_calc.wide_column = mcollect.ws_string5;
            tab_calc.suppress_zero = mcollect.ws_string2 ;
            tab_calc.transfer_code = mcollect.ws_string1 ;
            tab_calc.addnotes = "";
            tab_calc.comment_code = "";

            if (laction == "Create")
                db.tab_calc.Add(tab_calc);
            else
                db.Entry(tab_calc).State = EntityState.Modified;

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

            if (err_flag )
            {
                //delete the record in tab array and write from tab temp rep
                var str1 = "";
                fill_blanks();

                str1 = "delete from tab_array where para_code=" + utils.pads(type_code) + " and array_code=" + utils.pads(mcollect.ws_code);
                db.Database.ExecuteSqlCommand(str1);
                
// double sure of code and userid
                str1 = " update tab_temp_rep set report_code=" + utils.pads(mcollect.ws_code) + " where user_code=" + utils.pads(pblock.userid);
                db.Database.ExecuteSqlCommand(str1);

                str1 = "insert into tab_array (para_code,array_code,count_array,operand,source1,period,operator1,amended_by,date_amended) ";
                str1 += " select " + utils.pads(type_code) + ",report_code,rep_count,op1,src1,perd1,";
                str1 += " case sort1 when 1 then '+' when 2 then '-' when 3 then '*' when 4 then '/' else '' end, ";
                str1 += utils.pads(pblock.userid) + ", getutcdate() from tab_temp_rep ";
                str1 += " where user_code=" + utils.pads(pblock.userid);

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


// sort updates
                str1 = "delete from tab_array where para_code='" + type_code + "S' and array_code=" + utils.pads(mcollect.ws_code);
                db.Database.ExecuteSqlCommand(str1);

                for (int ctr1 = 0; ctr1 < 5; ctr1++)
                {
                    if (!string.IsNullOrWhiteSpace(mcollect.ar_string0[ctr1] ))
                    {
                        str1 = "insert into tab_array(para_code,array_code,count_array,operand,source1,period,sort1,select1,amended_by,date_amended) values ('";
                        str1 += type_code + "S'," + utils.pads(mcollect.ws_code) + "," + Convert.ToString(ctr1+1) + ",";
                        str1 += utils.pads(mcollect.ar_bool0[ctr1] == true ? "Y" : "N") + ", ";
                        str1 += utils.pads(mcollect.ar_bool1[ctr1] == true ? "Y" : "N") + ", ";
                        str1 += utils.pads(mcollect.ar_bool2[ctr1] == true ? "Y" : "N") + ", ";
                        str1 += utils.pads(mcollect.ar_string0[ctr1]) + "," + utils.pads(mcollect.ar_string2[ctr1]) + ",";
                        str1 += utils.pads(pblock.userid) + ", getutcdate() )";

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
                }


            }

            if (err_flag)
            {
                write_temp("");
                write_log(laction);
            }

        }

        private void read_daily_record()
        {
            init_class();
            mcollect.ws_code = tab_calc.calc_code;
            mcollect.ws_string0 = tab_calc.name1;
            mcollect.ws_string1 = tab_calc.transfer_code;
            mcollect.ws_decimal0 = tab_calc.line_spacing;
            mcollect.ws_string2 = tab_calc.suppress_zero;
            mcollect.ws_string3 = tab_calc.menu_option;
            mcollect.ws_string4 = tab_calc.report_type;
            mcollect.ws_string5 = tab_calc.wide_column;
            mcollect.ws_string6 = tab_calc.report_name;
            mcollect.ws_decimal1 = tab_calc.column_no;
            worksess.temp5 = mcollect.ws_string6;
            Session["worksess"] = worksess;

            if (mcollect.ws_string1 == "Y")
                mcollect.ws_bool0 = true;
            if (mcollect.ws_string5 == "Y")
                mcollect.ws_bool1 = true;
            if (mcollect.ws_string2 == "Y")
                mcollect.ar_bool0[5] = true;

            write_temp("R");
            read_names_operand();
            check_trans_type();
        }

        private void validate_fields()
        {
            err_message = "";
            if (string.IsNullOrWhiteSpace(mcollect.ws_code))
            {
                err_message = "Code can not be Spaces";
                ModelState.AddModelError(String.Empty, err_message);
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(mcollect.ws_string0))
            {
                err_message = "Name can not be Spaces";
                ModelState.AddModelError(String.Empty, err_message);
                err_flag = false;
            }

            mcollect.ws_string0 = mcollect.ws_string0 == null ? "" : mcollect.ws_string0;
            mcollect.ws_string1 = mcollect.ws_string1 == null ? "" : mcollect.ws_string1;
            mcollect.ws_string2 = mcollect.ws_string2 == null ? "" : mcollect.ws_string2;
            mcollect.ws_string3 = mcollect.ws_string3 == null ? "" : mcollect.ws_string3;
            mcollect.ws_string4 = mcollect.ws_string4 == null ? "" : mcollect.ws_string4;
            mcollect.ws_string5 = mcollect.ws_string5 == null ? "" : mcollect.ws_string5;
            mcollect.ws_string6 = mcollect.ws_string6 == null ? "" : mcollect.ws_string6;

            if (type_code == "P04")
                mcollect.ws_string4 = "G";

            if (laction == "Create")
            {
                tab_calc cbank = db.tab_calc.Find(type_code, mcollect.ws_code);
                if (cbank != null)
                {
                    err_message = "Record Already Created";
                    ModelState.AddModelError(String.Empty, err_message);
                    err_flag = false;
                }
            }

        }

        [HttpPost]
        public ActionResult DailyList(string idx)
        {
            List<SelectListItem> ary = new List<SelectListItem>();
            var str2 = utils.listdaily(idx);
            foreach (var item in str2.ToList())
            {
                ary.Add(new SelectListItem { Text = item.c2, Value = item.c1 });
            }
            ary.Add(new SelectListItem { Text = "operand", Value = "operand" });

            str2 = utils.listdaily(idx);
            foreach (var item in str2.ToList())
            {
                ary.Add(new SelectListItem { Text = item.c2, Value = item.c1 });
            }
            ary.Add(new SelectListItem { Text = "source", Value = "source" });

            str2 = utils.listdaily(idx);
            foreach (var item in str2.ToList())
            {
                ary.Add(new SelectListItem { Text = item.c2, Value = item.c1 });
            }
            ary.Add(new SelectListItem { Text = "period", Value = "period" });



            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        [HttpPost]
        public ActionResult Daily2List(string idx)
        {
            // only for hr calculation and appraisal and when selection is transaction type and performance pages
            var str2 = utils.listdaily2(idx);

                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    str2.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        [HttpPost]
        public ActionResult Daily3List(string idx)
        {
            type1a(idx);

            var str2 = utils.listdaily3(idx);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});

        }

        private void init_class()
        {
            mcollect.ws_code = "";
            mcollect.ar_decimal0 = new decimal[60];
            mcollect.ar_decimal1 = new decimal[60];
            mcollect.ar_decimal2 = new decimal[60];
            mcollect.ar_decimal3 = new decimal[60];
            mcollect.ar_string0 = new string[60];
            mcollect.ar_string1 = new string[60];
            mcollect.ar_string2 = new string[60];
            mcollect.ar_bool0 = new bool[60];
            mcollect.ar_bool1 = new bool[60];
            mcollect.ar_bool2 = new bool[60];
            mcollect.ws_string3 = worksess.temp3;    //(string) (worksess.temp3);

            if (type_code == "H20" || type_code == "HA20" || type_code == "H13" || type_code == "HA07" || type_code == "R04" || type_code == "F13" ||
                type_code == "H47")
                mcollect.ws_string8 = "A";
            else
                mcollect.ws_string8 = "";
        }

        private void write_temp(string action)
        {
            int ctr1;
            var str1 = "Delete from tab_temp_rep where user_code=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (action == "R")
            {
                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,op1,src1,perd1,advice_count) ";
                str1 = str1 + " select 'DET'," + utils.pads(pblock.userid) + ",array_code,count_array,operand,source1,period,count_array * 10 ";
                str1 = str1 + " from tab_array where para_code=" + utils.pads(type_code) + " and array_code=" + utils.pads(mcollect.ws_code);
                db.Database.ExecuteSqlCommand(str1);
                fill_blanks();

                string sortkey=type_code+"S";
                var bgsort = from bgl in db.tab_array
                             where bgl.para_code == sortkey && bgl.array_code == mcollect.ws_code
                             select bgl;

                foreach (var bgl in bgsort)
                {
                    ctr1 = Convert.ToInt16(bgl.count_array)-1;
                    mcollect.ar_string0[ctr1] = bgl.sort1;
                    mcollect.ar_string2[ctr1] = bgl.select1;
                    mcollect.ar_bool0[ctr1] = bgl.operand == "Y" ? true : false;
                    mcollect.ar_bool1[ctr1] = bgl.source1 == "Y" ? true : false;
                    mcollect.ar_bool2[ctr1] = bgl.period == "Y" ? true : false;
                }

            }

        }

        private void read_names_operand()
        {
            var rdailly = from rd in db.tab_temp_rep
                          where rd.user_code == pblock.userid
                          orderby rd.rep_count
                          select rd;
            int ctr = 0;
            mcollect.ar_string1 = new string[60];
            mcollect.ar_decimal0 = new decimal[60];

            string str1 = "select advice_count t1,dbo.validate_source1(op1,src1,perd1) c1 from tab_temp_rep where user_code=" + utils.pads(pblock.userid) + " order by rep_count ";
            var str2 = db.Database.SqlQuery<vw_query>(str1).ToList();
            
            foreach(var rd1 in str2)
            {
                mcollect.ar_string1[ctr] = rd1.c1;
                mcollect.ar_decimal0[ctr] = rd1.t1;

                ctr++;
            }

            ctr++;
            mcollect.ws_decimal7 = ctr;


        }


        private void formula_title(string pc)
        {
            if (pc == "13")
            {
                worksess.ptitle = "Staff Report Definition ";
                worksess.flag_type = "S";
                worksess.temp1 = " and report_name5='Y' ";
                worksess.temp2 = "";
                worksess.temp3 = "PYREP";
            }
            else if (pc == "14")
            {
                worksess.ptitle = "Transaction Report Definition ";
                worksess.flag_type = "T";
                worksess.temp1 = " and report_name5='Y' ";
                worksess.temp2 = "Y";
                worksess.temp3 = "PYREP";
            }
            else if (pc == "H13")
            {
                worksess.ptitle = "Staff Report Definition ";
                worksess.flag_type = "H";
                worksess.temp1 = " and report_name5='Y' ";
                worksess.temp2 = "Y";
                worksess.temp3 = "HRREP";
            }
            else if (pc == "HA07")
            {
                worksess.ptitle = "Performance Staff Report Definition ";
                worksess.flag_type = "T";
                worksess.temp1 = " and report_name3='Y' ";
                worksess.temp2 = "Y";
                worksess.temp3 = "PFREP";
            }
            else if (pc == "P04")
            {
                worksess.ptitle = "Gratuity Report Definition ";
                worksess.flag_type = "S";
                worksess.temp1 = " and report_name10='Y' ";
                worksess.temp2 = "";
                worksess.temp3 = "GTREP";
            }
            else if (pc == "H15")
            {
                worksess.ptitle = "HR Transaction Report  ";
                worksess.flag_type = "T";
                worksess.temp2 = "";
                worksess.temp3 = "HRREP";
            }
            else if (pc == "R04")
            {
                worksess.ptitle = "Applicant Report Definition ";
                worksess.flag_type = "";
                worksess.temp1 = " and report_name5='Y' ";
                worksess.temp2 = "Y";
                worksess.temp3 = "REREP";
            }
            else if (pc == "H47")
            {
                worksess.ptitle = "HR Expense Transaction ";
                worksess.flag_type = "T";
                worksess.temp2 = "Y";
                worksess.temp3 = "HRREP";
            }
            else if (pc == "CP04")
            {
                worksess.ptitle = "Coop Transaction ";
                worksess.flag_type = "T";
                worksess.temp2 = "Y";
                worksess.temp3 = "CPREP";
            }
            else if (pc == "F13")
            {
                worksess.ptitle = "Incidence Transaction ";
                worksess.flag_type = "S";
                worksess.temp2 = "Y";
                worksess.temp3 = "FLREP";
            }
            else if (pc == "F17")
            {
                worksess.ptitle = "Fleet Report Definition ";
                worksess.flag_type = "T";
                worksess.temp2 = "";
                worksess.temp3 = "FLREP";
            }
            else if (pc == "F18")
            {
                worksess.ptitle = "Maintenance Forecast Report Definition ";
                worksess.flag_type = "T";
                worksess.temp2 = "";
                worksess.temp3 = "FLREP";
            }

        }

        [HttpPost]
        public ActionResult update_view(string op,string src1,string perd1,string num1,string pos1,string max_pos,string ws_code,string operat)
        {
            //string ws_code; string src1; string perd1; string num1; string pos1; string max_pos; string op; ; string operat;
            int ctr0 = 0; int ctr1 = 0;
            //string opt = idx;
            pblock = (mysessvals)Session["mysessvals"];
            worksess = (worksess)Session["worksess"];
            vw_collect pcollect = new vw_collect();
            pcollect.ar_decimal0 = new decimal[60] ;
            type_code = worksess.pc;
            string h15type_code="";

            //ctr0 = opt.IndexOf("[]");
            //op = opt.Substring(0, ctr0);
            //ctr1 = opt.IndexOf("[]", ctr0 + 2);
            //src1 = opt.Substring(ctr0 + 2, ctr1 - ctr0 - 2);
            //ctr0 = opt.IndexOf("[]", ctr1 + 2);
            //perd1 = opt.Substring(ctr1 + 2, ctr0 - ctr1 - 2);
            //ctr1 = opt.IndexOf("[]", ctr0 + 2);
            //num1 = opt.Substring(ctr0 + 2, ctr1 - ctr0 - 2);
            //ctr0 = opt.IndexOf("[]", ctr1 + 2);
            //pos1 = opt.Substring(ctr1 + 2, ctr0 - ctr1 - 2);
            //ctr1 = opt.IndexOf("[]", ctr0 + 2);
            //max_pos = opt.Substring(ctr0 + 2, ctr1 - ctr0 - 2);
            //ctr0 = opt.IndexOf("[]", ctr1 + 2);
            //ws_code = opt.Substring(ctr1 + 2, ctr0 - ctr1 - 2);
            //ctr1 = opt.IndexOf("[]", ctr0 + 2);
            //operat = opt.Substring(ctr0 + 2) + " ";
            int len1 = (operat.Length + 1)/ 4;
            int temp_int = 0;
            for (int ct1 = 0; ct1 < len1; ct1++)
            {
                int.TryParse(operat.Substring(ct1 * 4, 4), out temp_int);
                pcollect.ar_decimal0[ct1] = temp_int;
            }

            var str1 = "";

            for (int cr0=0; cr0<len1; cr0++)
            {
                str1 = "update tab_temp_rep set advice_count=" + pcollect.ar_decimal0[cr0];
                str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid);
                str1 += " and rep_count = " + Convert.ToString(cr0 + 1);
                db.Database.ExecuteSqlCommand(str1);
            }

            if (type_code == "H15" )
            {
                h15type_code = worksess.temp5;
                if (h15type_code.Substring(0, 1) == "x" && op.Length > 4)
                {
                    if (op == "VDATE" || ((op.Substring(0, 4) == "LINE" || op.Substring(0, 4) == "SINE") && int.TryParse(op.Substring(4), out ctr1))||
                        (op == "WORKDURA" || op == "WORKRDATE" || op == "WORKRUSER" || op == "WORKLAUSER" || op == "WORKLADATE" || op == "WORKAGE"))
                    {
                        src1 = op;
                        op = h15type_code.Substring(1);
                    }
                }

                if (h15type_code.Substring(0, 1) == "q")
                {
                    int pt = op.IndexOf("_desc");
                    string op1 = op;
                    if (pt > 0)
                        op1 = op1.Substring(0, pt);
                    str1 = "select 1 t1 from tab_array where para_code='H46' and array_code=" + utils.pads(h15type_code.Substring(1)) + " and operand=" + utils.pads(op1);
                    var bgexp = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                    if (bgexp != null)
                    {
                        src1 = op;
                        op = h15type_code.Substring(1);
                    }

                }
            }

            if (type_code == "R04" && op.Length > 4)
            {
                if (op == "VDATE" || ((op.Substring(0, 4) == "LINE" || op.Substring(0, 4) == "SINE") && int.TryParse(op.Substring(4), out ctr1)))
                {
                    src1 = op;
                    op = h15type_code.Substring(1);
                }
            }

            
            string numstr = op;
            if (op == "NUM")
            {
                numstr = Convert.ToString(num1);
                src1 = "NUM";
            }

            // delete advice count 0
            str1 = "delete from tab_temp_rep where advice_count=0 and report_line='DET' and user_code=" + utils.pads(pblock.userid);
            int rec_count = db.Database.ExecuteSqlCommand(str1);

            bool max_flag = true;
            if (Convert.ToInt16(max_pos) > 50) max_flag = false;
            if (rec_count > 0) max_flag = true;
            
            if (numstr != "" && max_flag)
            {
                decimal decmax = pcollect.ar_decimal0.Max() + 10;
                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,op1,src1,perd1,advice_count) values (";
                str1 = str1 + " 'DET'," + utils.pads(pblock.userid) + "," + utils.pads(ws_code);
                str1 = str1 + "," + utils.pads(max_pos) + "," + utils.pads(numstr) + "," + utils.pads(src1);
                str1 = str1 + "," + utils.pads(perd1) + "," + Convert.ToString(decmax) + " )";
                db.Database.ExecuteSqlCommand(str1);
            }

            str1 = "update tab_temp_rep set src1='' from tab_temp_rep a, ( ";
            str1 += " select report_code, numeric_ind, report_name10 from tab_soft where para_code in ('CP', 'USERV')) b ";
            str1 += " where b.report_code = a.op1 and ";
            str1 += " ((report_name10 = 'X' and not(src1 = '' or src1 = 'CODE' or src1 = 'DESC')) or(numeric_ind <> 'T' and(src1 = 'CODE' or src1 = 'DESC')) ";
            str1 += " or (numeric_ind = 'T' and report_name10 <> 'X' and (src1 = 'CODE' or src1 = 'DESC'))) ";
            str1 += " and report_line='DET' and user_code=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

// renumbering
            if (rec_count > 0)
                fill_blanks();

            int ctr = 0;

            pcollect.ar_string1 = new string[60];
            pcollect.ar_decimal0 = new decimal[60] ;

            str1 = "select advice_count t1,dbo.validate_source1(op1,src1,perd1) c1 from tab_temp_rep where user_code=" + utils.pads(pblock.userid) + " order by rep_count ";
            var str2 = db.Database.SqlQuery<vw_query>(str1).ToList();

            foreach (var rd1 in str2)
            {
                pcollect.ar_string1[ctr] = rd1.c1;
                pcollect.ar_decimal0[ctr] = rd1.t1;

                ctr++;
            }

            ctr++;
            pcollect.ws_decimal7 = ctr;

            return PartialView("_RdsgnView", pcollect);
            //  ModelState.Clear();

        }

        private void insert_default()
        {
            string str1;
            str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,op1,src1,perd1,advice_count) ";
            if (type_code == "R04")
            {
                str1 += " select 'DET'," + utils.pads(pblock.userid) + ",'',1,'APREF','','',10 union all ";
                str1 += " select 'DET'," + utils.pads(pblock.userid) + ",'',2,'SNAME','','',20 ";
            }
            else if (type_code == "F13" || type_code == "F17" || type_code == "F18")
            {
                str1 += " select 'DET'," + utils.pads(pblock.userid) + ",'',1,'FLT','','',10 union all ";
                str1 += " select 'DET'," + utils.pads(pblock.userid) + ",'',2,'FLNAME','','',20 ";
            }
            else
            {
                str1 += " select 'DET'," + utils.pads(pblock.userid) + ",'',1,'NUMB','','',10 union all ";
                str1 += " select 'DET'," + utils.pads(pblock.userid) + ",'',2,'SNAME','','',20 ";
            }
            db.Database.ExecuteSqlCommand(str1);
        }

        private void check_trans_type()
        {
            if (type_code == "H20" || type_code == "HA20" || type_code == "H13" || type_code == "HA07" || type_code == "R04" || type_code == "F13" ||
                type_code == "H47" )
                mcollect.ws_string8 = "A";
            else
                mcollect.ws_string8 = "";
        }

        private void update_seq()
        {
            string str1 = "";
            for (int cr0 = 0; cr0 < mcollect.ws_decimal7-1; cr0++)
            {
                str1 = "update tab_temp_rep set advice_count=" + mcollect.ar_decimal0[cr0];
                str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid);
                str1 += " and rep_count = " + Convert.ToString(cr0 + 1);
                db.Database.ExecuteSqlCommand(str1);
            }

            // delete advice count 0
            str1 = "delete from tab_temp_rep where advice_count=0 and report_line='DET' and user_code=" + utils.pads(pblock.userid);
            var str21 = db.Database.SqlQuery<vw_query>(str1);

        }

        private void fill_blanks()
        {
            string str1 = "execute blank_rep @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);
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

            string select1 = " where para_code=" + utils.pads(tab_calc.para_code) + " and calc_code=" + utils.pads(tab_calc.calc_code);
            utils.write_plog(tab_calc.para_code, select1, opt, "B", pblock.userid, tab_calc.para_code, tab_calc.calc_code);

        }


        [HttpPost]
        public void type1a(string idx)
        {
            worksess = (worksess)Session["worksess"];
            worksess.temp5= idx;
            Session["worksess"] = worksess;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
//using CittaErp.Filters;
using CittaErp.utilities;
using System.Data.Entity;

namespace CittaErp.Controllers
{
    public class UserpController : Controller
    {
        //
        // GET: /RepDgn/

        private MainContext db = new MainContext();
        private MainContext db2 = new MainContext();
        Boolean err_flag = true;
        string laction = "";
        vw_collect mcollect = new vw_collect();
        cittautil utils = new cittautil();
        psess psess;
        tab_calc tab_calc = new tab_calc();
        comsess pblock;

        //
        // GET: /Allow/

        //[EncryptionActionAttribute]
        public ActionResult Index(string pc = null)
        {
            //if (utils.check_option() == 1||pc==null)
            //    return RedirectToAction("Welcome", "Home");
            utils.init_values();
            psess = (psess)Session["psess"];
            psess.temp5 = "21";
            psess.temp1 = "Group Permission";
            var blist = from s in db.tab_calc
                        where (s.para_code == "21")
                        orderby s.name1 
                        select s;
            Session["psess"] = psess;
            return View(blist.ToList());
        }

        //
        // GET: /Bank/Create

        //[EncryptionActionAttribute]
        public ActionResult Create(string xy = null)
        {
            mcollect.datmode = "C";
            //if (utils.check_option() == 1 || xy != "1")
            //    return RedirectToAction("Welcome", "Home");

            pblock = (comsess)Session["comsess"];
            init_class();
            write_temp("", "C");
            bank_group_select();
            mcollect.ws_string1 = "U";
            mcollect.ws_string2 = "V";
            return View("Edit", mcollect);
        }

        //
        // POST: /Bank/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_collect mcollect1, string[] snumber2)
        {
            pblock = (comsess)Session["comsess"];
            mcollect = mcollect1;
            laction = "Create";

            select_routine(snumber2);
            write_data();

            if (err_flag)
                return RedirectToAction("Create");//, null, new { anc = Ccheckg.convert_pass2("xy=1") });

            bank_group_select();
            mcollect.datmode = "C";

            return View("Edit", mcollect);

        }

        //

        //[EncryptionActionAttribute]
        public ActionResult Edit(string jy = null)
        {
            mcollect.datmode = "E";
            //if (utils.check_option() == 1 || jy == null)
            //    return RedirectToAction("Welcome", "Home");

            pblock = (comsess)Session["comsess"];
            tab_calc = db.tab_calc.Find("21", jy);
            if (tab_calc == null)
            {
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            read_daily_record();
            bank_group_select();
            return View(mcollect);
        }

        //
        // POST: /Bank/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(vw_collect mcollect1,string xhrt, string [] snumber2)
        {
            mcollect = mcollect1;
            pblock = (comsess)Session["comsess"];

            if (xhrt == "D")
            {
                DeleteConfirmed();
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            select_routine(snumber2);

            write_data();
            if (err_flag)
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});

            bank_group_select();
            mcollect.datmode = "E";

            return View(mcollect);

        }

        //
        // POST: /Bank/Delete/5

        //[HttpPost, ActionName("Delete")]
        private void DeleteConfirmed()
        {
            tab_calc = db.tab_calc.Find("21", mcollect.ws_code);
            write_log("Delete");
            db.tab_calc.Remove(tab_calc);
            db.SaveChanges();

            var str1 = "delete from tab_array where para_code like '21%' and array_code=" + utils.pads(mcollect.ws_code);
            db.Database.ExecuteSqlCommand(str1);

            //return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private  void bank_group_select()
        {
            string str1;
            // foldercodes
            var bglist2 = from bg in db.tab_soft
                           where bg.para_code == "MAINSCRN" && bg.rep_name2=="H"
                           orderby bg.report_name3 
                           select bg;

            str1 = "select report_code c1, report_name1 c2 from tab_soft where para_code='MAINSCRN' and rep_name2='H' and report_code in " + options_coy();
            str1 += " union all select report_code, report_name1 from tab_soft where para_code='MAINSCRN' and rep_name2='H' ";
            var bglist2w = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.folders = new SelectList(bglist2w.ToList(), "c1", "c2");

            str1 = "select header_sequence as c1, header_description as c2, ''  from GB_001_HANAL ";
            //str1 += "select analy_code as c1, analy0 as c2, analy1 as c3 from tab_analy where para_code in ('ANALY') order by 2 ";
            var bgsort12 = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.code_query = bgsort12.ToList();


            List<SelectListItem> sortad = new List<SelectListItem>();
            sortad.Add(new SelectListItem { Text = "User", Value = "U", Selected=true });
            sortad.Add(new SelectListItem { Text = "Administrator", Value = "S" });
            ViewBag.access_type = new SelectList(sortad.ToList(), "Value", "Text",mcollect.ws_string1 );

            List<SelectListItem> gratad = new List<SelectListItem>();
            gratad.Add(new SelectListItem { Text = "AND", Value = "AND"});
            gratad.Add(new SelectListItem { Text = "OR", Value = "OR" });
            ViewBag.logical_order = gratad.ToList();

            str1 = "select a.op1+a.src1 c1, b.report_name1 + ' - ([' + substring(c.report_code,1,2) + '] ' + c.report_name1 + ')' c2 from tab_temp_rep a, tab_soft b , tab_soft c ";
            str1 += " where b.para_code='item' and a.op1=b.report_code and c.report_name2=b.rep_name2 and c.para_code='MAINSCRN' ";
            str1 += " and a.report_line='DET' and a.user_code=" + utils.pads(pblock.userid);

            str1 = "select  a.op1+'[]'+a.src1 c1, b.report_name1 + ' - (' + c.report_name1 + ')' c2 from tab_temp_rep a, tab_soft b , tab_soft c ";
            str1 += " where b.para_code=a.src1 and a.op1=b.report_code and b.para_code='ITEM' and c.report_name2=b.rep_name2 and c.para_code='MAINSCRN' ";
            str1 += " and a.report_line='DET' and a.user_code=" + utils.pads(pblock.userid);
            str1 += " union all ";
            str1 += " select a.op1+'[]'+a.src1 c1, d.name1 + ' - ([' + b.report_name1 + ')' c2 from tab_temp_rep a, tab_soft b  , tab_calc d";
            str1 += " where b.para_code='UPER' and b.report_code=a.src1 and b.report_code <> 'TYPE' and d.para_code=a.src1 and d.calc_code=a.op1";
            str1 += " and a.report_line='DET' and a.user_code=" + utils.pads(pblock.userid);
            str1 += " union all";
            str1 += " select a.op1+'[]'+a.src1 c1, d.name1 + ' - ([' + b.report_name1 + ')' c2 from tab_temp_rep a, tab_soft b  , tab_type d ";
            str1 += " where b.para_code='UPER' and b.report_code=a.src1 and b.report_code = 'TYPE' and 'TYPE'=a.src1 and d.trans_type=a.op1 ";
            str1 += " and a.report_line='DET' and a.user_code=" + utils.pads(pblock.userid);

            var str2 = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.list2 = new SelectList(str2.ToList(), "c1", "c2");

            str1 = "select calc_code c1,name1 c2, isnull(operand,'') c3 from tab_calc a ";
            str1 += " left outer join tab_array b on (b.para_code='21K' and operand=calc_code and b.array_code= " + utils.pads(mcollect.ws_code);
            str1 += ") where a.para_code='UPDATE' order by a.line_spacing ";

            var bgsort2 = db.Database.SqlQuery<vw_query>(str1).ToList();
            ViewBag.paylist = bgsort2;


        }

        private void write_data()
        {
            pblock = (comsess)Session["comsess"];
            err_flag = true;
            validate_fields();

            if (err_flag)
                write_rec();
        }

        private void write_rec()
        {
            DateTime odate = utils.logdatetime();
            if (laction == "Create")
            {
                tab_calc = new tab_calc();
                tab_calc.created_by = pblock.userid;
                tab_calc.date_created = odate;
                tab_calc.internal_use = "N";
            }
            else
            {
                tab_calc = db.tab_calc.Find("21", mcollect.ws_code);
            }

            tab_calc.para_code = "21";
            tab_calc.calc_code = mcollect.ws_code;
            tab_calc.name1 = mcollect.ws_string0;
            tab_calc.report_name = "";
            tab_calc.amended_by = pblock.userid;
            tab_calc.date_amended = odate;
            tab_calc.column_no = 0;
            tab_calc.line_spacing = 0;
            tab_calc.report_type = mcollect.ws_string2;
            tab_calc.menu_option = "";
            tab_calc.wide_column = "";
            tab_calc.suppress_zero = "";
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
                update_seq();

                str1 = "delete from tab_array where para_code like '21' and array_code=" + utils.pads(mcollect.ws_code);
                db.Database.ExecuteSqlCommand(str1);

                str1 = "insert into tab_array (para_code,array_code,count_array,operand,source1,amended_by) ";
                str1 += " select '21'," + utils.pads(mcollect.ws_code) + ",ROW_NUMBER() over(order by rep_count),op1,src1,";
                str1 += utils.pads(pblock.userid) + " from tab_temp_rep ";
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


// Data restriction
                str1 = "delete from tab_array where para_code='21D' and array_code=" + utils.pads(mcollect.ws_code);
                db.Database.ExecuteSqlCommand(str1);

                for (int ctr1 = 0; ctr1 < 6; ctr1++)
                {
                    if (!string.IsNullOrWhiteSpace(mcollect.ar_string0[ctr1] ))
                    {
                        str1 = "insert into tab_array(para_code,array_code,count_array,operand,true_desc,false_desc,sort1,select1,amended_by) values ('21D',";
                        str1 += utils.pads(mcollect.ws_code) + "," + Convert.ToString(ctr1+1) + ",";
                        str1 += utils.pads(mcollect.ar_string0[ctr1]) + "," + utils.pads(mcollect.ar_string1[ctr1]) + ",";
                        str1 += utils.pads(mcollect.ar_string2[ctr1]) + "," ;
                        str1 += ctr1==5 ? "''":utils.pads(mcollect.ar_string3[ctr1]);
                        str1 += ",";
                        str1 += utils.pads(mcollect.ar_bool0[ctr1] == true ? "Y" : "N") + ", ";
                        str1 += utils.pads(pblock.userid) + " )";

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

                str1 = "delete from tab_array where para_code='21K' and array_code=" + utils.pads(mcollect.ws_code);
                db.Database.ExecuteSqlCommand(str1);

                if (mcollect.ar_string4 != null)
                {
                    for (int ctr1 = 0; ctr1 < mcollect.ar_string4.Length; ctr1++)
                    {
                        if (mcollect.ar_string5[ctr1] == "on")
                        {
                            str1 = "insert into tab_array(para_code,array_code,count_array,operand,amended_by) values ('21K',";
                            str1 += utils.pads(mcollect.ws_code) + "," + Convert.ToString(ctr1 + 1) + ",";
                            str1 += utils.pads(mcollect.ar_string4[ctr1]) + ",";
                            str1 += utils.pads(pblock.userid) + " )";

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
            }

            if(err_flag)
            {
                write_temp("", "");
                write_log(laction);
            }

        }

        private void read_daily_record()
        {
            pblock = (comsess)Session["comsess"];
            init_class();
            mcollect.ws_code = tab_calc.calc_code;
            mcollect.ws_string0 = tab_calc.name1;
            mcollect.ws_string1 = tab_calc.transfer_code;
            mcollect.ws_string2 = tab_calc.report_type;

            write_temp("R", tab_calc.calc_code);
        }

        private void validate_fields()
        {
            string err_msg = "";
            if (string.IsNullOrWhiteSpace(mcollect.ws_code))
            {
                err_msg = "Code Can not be Spaces";
                ModelState.AddModelError(String.Empty, err_msg);
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string0))
            {
                err_msg = "Description Can not be Spaces";
                ModelState.AddModelError(String.Empty, err_msg);
                err_flag = false;
            }

            mcollect.ws_string2 = string.IsNullOrWhiteSpace(mcollect.ws_string2) ? "I" : mcollect.ws_string2;

            if (laction == "Create")
            {
                tab_calc cbank = db.tab_calc.Find("21", mcollect.ws_code);
                if (cbank != null)
                {
                    ModelState.AddModelError(String.Empty, "Record Already Created");
                    err_flag = false;
                }
            }

        }

        [HttpPost]
        public ActionResult DailyList(string id)
        {
            string str14 = "";
            pblock = (comsess)Session["comsess"];
            string Code = id.Substring(3);
            string query_flag = id.Substring(0, 1);



            if (query_flag == "M")
                str14 = "select rep_name1 c1, report_name1 c2 from tab_soft where para_code='MAINSCRN' and numeric_ind=" + utils.pads(Code) + "and rep_name2 <> 'H' order by report_name3 ";
            else if (query_flag == "T")
            {
                Code = Code + "%";
                str14 = "select report_code + '[]ITEM' c1, report_name1 c2 from tab_soft where para_code='ITEM' and rep_name2 like " + utils.pads(Code);
                str14 += " and report_code + 'ITEM' not in (select op1 + src1  from tab_temp_rep where report_line='DET' and user_code=" + utils.pads(pblock.userid) + ") union all ";
                str14 += " select calc_code + '[]' + a.para_code c1, name1 c2 from tab_calc a, tab_soft b where b.para_code='UPER' and a.internal_use <> 'Y' and a.menu_option like " + utils.pads(Code);
                str14 += " and a.para_code=b.report_code and calc_code+a.para_code not in (select op1+src1 from tab_temp_rep where report_line='DET' and user_code=" + utils.pads(pblock.userid) + ")  union all ";
                str14 += " select trans_type + '[]TYPE' c1, name1 c2 from tab_type where internal_use <> 'Y' and selection_line like " + utils.pads(Code);
                str14 += " and trans_type + 'TYPE' not in (select op1+src1 from tab_temp_rep where report_line='DET' and user_code=" + utils.pads(pblock.userid) + ")  order by 2 ";
            }
            else if (query_flag == "U")
            {
                int pos1 = Code.IndexOf("[]]");
                string snumb1 = Code.Substring(0, pos1);
                string snumb2 = Code.Substring(pos1 + 3);
                string[] opt_str1; string[] opt_str2;
                string[] sepa_str = new string[] { "," };

                opt_str1 = snumb1.Split(sepa_str, StringSplitOptions.None);
                opt_str2 = snumb2.Split(sepa_str, StringSplitOptions.None);
                //select_routine(opt_str1, opt_str2);

                str14 = "select a.op1 + src1 c1, b.report_name1 + ' - ([' + substring(c.report_code,1,2) + '] ' + c.report_name1 + ')' c2 from tab_temp_rep a, tab_soft b , tab_soft c ";
                str14 += " where b.para_code='ITEM' and a.op1=b.report_code and c.report_name2=b.rep_name2 and c.para_code='MAINSCRN' ";
                str14 += " and a.report_line='DET' and src1='ITEM' and a.user_code=" + utils.pads(pblock.userid) + " union all ";
                str14 += " select a.op1 + src1 c1, b.name1 + ' - ([' + substring(c.report_code,1,2) + '] ' + c.report_name1 + ')' c2 from tab_temp_rep a, tab_calc b , tab_soft c ";
                str14 += " where b.para_code=a.src1 and a.op1=b.calc_code and c.report_name2=b.menu_option and c.para_code='MAINSCRN' ";
                str14 += " and a.report_line='DET' and src1 not in ('ITEM','TYPE') and a.user_code=" + utils.pads(pblock.userid) + " union all ";
                str14 += " select a.op1 + src1 c1, b.name1 + ' - ([' + substring(c.report_code,1,2) + '] ' + c.report_name1 + ')' c2 from tab_temp_rep a, tab_type b , tab_soft c ";
                str14 += " where a.op1=b.trans_type and c.report_name2=b.selection_line and c.para_code='MAINSCRN' ";
                str14 += " and a.report_line='DET' and src1 in ('TYPE') and a.user_code=" + utils.pads(pblock.userid) + " order by 2 ";
            }

            var str2 = db.Database.SqlQuery<vw_query>(str14);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        [HttpPost]
        public ActionResult populate_query(string id)
        {

            string str1 = "select report_code as c1, report_name1 as c2, ''  from tab_soft where para_code = 'DSEL'  and report_name4='Y' union all ";
            str1 += "select analy_code as c1, analy0 as c2, analy1 as c3 from tab_analy where para_code in ('ANALY') order by 2 ";
            var bgsort12 = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.code_query = bgsort12.ToList();
            var str2 = db.Database.SqlQuery<vw_query>(str1);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        private void init_class()
        {
            mcollect.ws_code = "";
            mcollect.ar_decimal0 = new decimal[50];
            mcollect.ar_decimal1 = new decimal[50];
            mcollect.ar_decimal2 = new decimal[50];
            mcollect.ar_decimal3 = new decimal[50];
            mcollect.ar_string0 = new string[150];
            mcollect.ar_string1 = new string[150];
            mcollect.ar_string2 = new string[150];
            mcollect.ar_string3 = new string[150];
            mcollect.ar_string4 = new string[150];
            mcollect.ar_string5 = new string[150];
            mcollect.ar_bool0 = new bool[50];
            mcollect.ar_bool1 = new bool[50];
            mcollect.ar_bool2 = new bool[50];
            for (int ctr = 0; ctr < 20; ctr++)
            {
                mcollect.ar_string5[ctr] = "off";
            }
        }

        private void write_temp(string action, string daily_code)
        {
            int ctr1;
            var str1 = "Delete from tab_temp_rep where user_code=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (action == "R")
            {
                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,op1,src1) ";
                str1 = str1 + " select 'DET'," + utils.pads(pblock.userid) + ",array_code,count_array,operand,source1 ";
                str1 = str1 + " from tab_array where para_code='21' and array_code=" + utils.pads(daily_code);
                db.Database.ExecuteSqlCommand(str1);

                var bgsort = from bgl in db.tab_array
                             where bgl.para_code == "21D" && bgl.array_code == daily_code
                             select bgl;

                foreach (var bgl in bgsort)
                {
                    ctr1 = Convert.ToInt16(bgl.count_array)-1;
                    mcollect.ar_string0[ctr1] = bgl.operand;
                    mcollect.ar_string1[ctr1] = bgl.true_desc;
                    mcollect.ar_string2[ctr1] = bgl.false_desc;
                    mcollect.ar_string3[ctr1] = bgl.sort1;
                    mcollect.ar_bool0[ctr1] = bgl.select1 == "Y" ? true : false;
                }

                var bgsort1 = from bgl in db.tab_array
                              join bg2 in db.tab_calc
                              on new { a1 = "UPDATE", a2 = bgl.operand } equals new { a1 = bg2.para_code, a2 = bg2.calc_code }
                              into bk3
                              from bg3 in bk3.DefaultIfEmpty()
                              where bgl.para_code == "21K" && bgl.array_code == daily_code
                              select new { bgl, bg3 };
                ctr1 = 0;
                foreach (var bgd in bgsort1)
                {
                    mcollect.ar_string4[ctr1] = bgd.bg3.calc_code;
                    if (!string.IsNullOrWhiteSpace(bgd.bgl.operand))
                        mcollect.ar_bool1[ctr1] = true ;
                    ctr1++;
                }

                str1 = "select calc_code c1,name1 c2, isnull(operand,'') c3 from tab_calc a ";
                str1 += " left outer join tab_array b on (b.para_code='21K' and operand=calc_code and b.array_code= " + utils.pads(daily_code);
                str1 += ") where a.para_code='UPDATE'  order by a.line_spacing ";

                var bgsort2 = db.Database.SqlQuery<vw_query>(str1).ToList();
                ViewBag.paylist = bgsort2;

            }

        }


        private void select_routine(string[] snumber1)
        {
            string str1 = "";
            int pos1 = 0;

            str1 = "delete tab_temp_rep where user_code=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (snumber1 != null)
            {
                foreach (var bh in snumber1)
                {
                    pos1 = bh.IndexOf("[]");
                    str1 = "insert into tab_temp_rep(report_line,user_code,report_code,op1,src1,rep_count) values ('DET'," + utils.pads(pblock.userid) + ",";
                    str1 += utils.pads(mcollect.ws_code) + ",";
                    str1 += utils.pads(bh.Substring(0,pos1)) + "," + utils.pads(bh.Substring(pos1+2)) + ",1) ";
                    db.Database.ExecuteSqlCommand(str1);
                }
            }

            //if (snumber2[0] != null)
            //{
            //    foreach (var bh in snumber2)
            //    {
            //        str1 = "delete tab_temp_rep where user_code=" + utils.pads(pblock.userid) + " and op1+src1=" + utils.pads(bh) + " and report_line='DET' ";
            //        db.Database.ExecuteSqlCommand(str1);
            //    }
            //}


        }

        private void update_seq()
        {
            string str1 = "";
            str1 = "select op1+src1 c1 from tab_temp_rep where user_code=" + utils.pads(pblock.userid) + " order by countID ";
            var str2 = db.Database.SqlQuery<vw_query>(str1);
            int ctr1 = 0;
            foreach(var item in str2.ToList())
            {
                str1 = "update tab_temp_rep set rep_count=" + ctr1.ToString();
                str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid);
                str1 += " and op1 +src1 = " + utils.pads(item.c1);
                db.Database.ExecuteSqlCommand(str1);
                ctr1++;
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

            string select1 = " where para_code=" + utils.pads(tab_calc.para_code) + " and calc_code=" + utils.pads(tab_calc.calc_code);
            utils.write_plog(tab_calc.para_code, select1, opt, "B", pblock.userid, tab_calc.para_code, tab_calc.calc_code);

        }

        private string options_coy()
        {
            int gh1=100;
            string menu_option = "";
            //bool gh0 = int.TryParse(pblock.mcode3, out gh1);

            if (gh1 >= 83)
            {
                menu_option += "'TA',";
                gh1 -= 83;
                // T&A
            }
            if (gh1 >= 41)
            {
                menu_option += "'RE',";
                gh1 -= 41;
                // erecruitment
            }
            if (gh1 >= 21)
            {
                menu_option += "'PF',";
                gh1 -= 21;
                // performance         
            }
            if (gh1 >= 11)
            {
                menu_option += "'GT',";
                gh1 -= 11;
                //grat        
            }
            if (gh1 >= 5)
            {
                menu_option += "'HR',";
                gh1 -= 5;
                //hr        
            }
            if (gh1 == 3)
            {
                menu_option += "'PY',";
                //payroll        
            }

            return  " (" + menu_option + "'')";
        }


    }
}

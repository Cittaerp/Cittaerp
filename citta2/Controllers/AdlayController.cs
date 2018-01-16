using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
//using CittaErp.Filters;
using CittaErp.utilities;
using System.Data.Entity;
//using System.Data.Linq;

namespace CittaErp.Controllers
{
    public class AdlayController : Controller
    {
        //
        // GET: /Adlay/

        private MainContext db = new MainContext();
        Boolean err_flag = true;
        string laction = "";
        vw_collect mcollect = new vw_collect();
        cittautil utils = new cittautil();
        string type_code = "";
        psess psess;
        comsess pblock;
        tab_calc tab_calc=new tab_calc();
        string str1;
        

       // [EncryptionActionAttribute]
        public ActionResult Index(string pc = null)
        {
            utils.init_values();
            //if (utils.check_option() == 1 || pc==null)
            //    return RedirectToAction("Welcome", "Home");


            if (pc == null) pc = "";
            if (pc == "")
            {
                type_code = (string)psess.temp0;
            }
            else
            {
                psess.temp0 = pc;
                type_code = pc;
            }
            psess.temp6= "990";
            psess.temp5 = type_code;
            psess.temp1 = "PaySlip Layout";

            pblock = (comsess)Session["comsess"];
            formula_title(type_code);
            var blist = from s in db.tab_calc
                        where (s.para_code == type_code)
                        orderby s.name1
                        select s;

            return View(blist.ToList());
        }

        //
        // GET: /Bank/Create

        //[EncryptionActionAttribute]
        public ActionResult Create(string xy=null)

        {
            mcollect.datmode="C";
            //if (utils.check_option() == 1 || xy != "1")
            //    return RedirectToAction("Welcome", "Home");

            pblock = (comsess)Session["comsess"];
            type_code = (string)psess.temp0;
            psess.temp4 = "1";
            init_class();
            write_temp("", "");
            bank_group_select();
            mcollect.ws_decimal7 = 1;
            mcollect.ws_decimal8 = 1;
            init_dec5();
            return View("Edit",mcollect);
        }

        //
        // POST: /Bank/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_collect mcollect1, string command)
        {
            mcollect.datmode="C";
            pblock = (comsess)Session["comsess"];
            type_code = (string)psess.temp0;
            mcollect = mcollect1;
            laction = "Create";

            if (command == "Insert")
            {
                cupdate_view();
                bank_group_select();
                return View("Edit",mcollect);
            }

            write_data();

            if (err_flag)
                return RedirectToAction("Create");//, null, new { anc = Ccheckg.convert_pass2("xy=1") });

            bank_group_select();
            read_names_operand();
            
            return View("Edit", mcollect);
        }

        //

        //[EncryptionActionAttribute]
        public ActionResult Edit(string jy = null)
        {
            mcollect.datmode="E";
            //if (utils.check_option() == 1 || jy == null)
            //    return RedirectToAction("Welcome", "Home");

            pblock = (comsess)Session["comsess"];
            type_code = (string)psess.temp0;

            tab_calc = db.tab_calc.Find(type_code, jy);
            if (tab_calc == null)
            {
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            pblock = (comsess)Session["comsess"];
            psess.temp4 = "1";
            init_dec5();
            read_daily_record();
            bank_group_select();
            return View(mcollect);
        }

        //
        // POST: /Bank/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(vw_collect mcollect1, string command,string xhrt)
        {
            mcollect.datmode="E";
            pblock = (comsess)Session["comsess"];
            type_code = (string)psess.temp0;
            mcollect = mcollect1;
            pblock = (comsess)Session["comsess"];

            if (xhrt == "D")
            {
                DeleteConfirmed();
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            if (command == "Insert")
            {
               cupdate_view();
                bank_group_select();
                return View(mcollect);
            }

            write_data();

            if (err_flag)
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});

            bank_group_select();
            read_names_operand();
            
            return View(mcollect);
        }

        //
        // POST: /Bank/Delete/5

        private void DeleteConfirmed()
        {
            pblock = (comsess)Session["comsess"];
            type_code = (string)psess.temp0;
            tab_calc = db.tab_calc.Find(type_code, mcollect.ws_code);
            write_log("Delete");
            db.tab_calc.Remove(tab_calc);
            db.SaveChanges();

            str1 = "delete from tab_array where para_code=" + utils.pads(type_code) + " and array_code=" + utils.pads(mcollect.ws_code);
            db.Database.ExecuteSqlCommand(str1);

        }

        public ActionResult delete_list(string id)
        {
            pblock = (comsess)Session["comsess"];
            type_code = (string)psess.temp0;
            tab_calc = db.tab_calc.Find(type_code, mcollect.ws_code);
            write_log("Delete");
            db.tab_calc.Remove(tab_calc);
            db.SaveChanges();

            str1 = "delete from tab_array where para_code=" + utils.pads(type_code) + " and array_code=" + utils.pads(mcollect.ws_code);
            db.Database.ExecuteSqlCommand(str1);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void bank_group_select()
        {
            // foldercodes
            var bglist2 = (from bg in db.tab_soft
                           where (bg.para_code == "QUERY" && bg.report_code == type_code)
                           select bg).First();
            var report_name2 = bglist2.report_name2;
            psess.temp3 = bglist2.report_name3;
            report_name2 = report_name2.Replace("'", "");

            string[] opt_str;
            string[] sepa_str = new string[] { "," };

            opt_str = report_name2.Split(sepa_str, StringSplitOptions.None);

            var bglist3 = from bg2 in db.tab_soft
                          where (bg2.para_code == "QUERY" && opt_str.Contains(bg2.report_code))
                          orderby bg2.numeric_ind
                          select bg2;

            ViewBag.folders = new SelectList(bglist3.ToList(), "report_code", "report_name1");

            List<SelectListItem> operator1 = new List<SelectListItem>();
            operator1.Add(new SelectListItem { Text = "+", Value = "1" });
            operator1.Add(new SelectListItem { Text = "-", Value = "2" });
            operator1.Add(new SelectListItem { Text = "*", Value = "3" });
            operator1.Add(new SelectListItem { Text = "/", Value = "4" });
            ViewBag.oper1 = operator1;

        }

        private void write_data()
        {
            err_flag = true;
               cupdate_view(1);
            validate_fields();
            //update_all_record();

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
            }
            else
            {
                tab_calc = db.tab_calc.Find(type_code, mcollect.ws_code);
            }

            tab_calc.para_code = type_code;
            tab_calc.name1 = mcollect.ws_string0;
            tab_calc.report_name = "";
            tab_calc.amended_by = pblock.userid;
            tab_calc.date_amended = odate;
            tab_calc.calc_code = mcollect.ws_code;
            tab_calc.column_no = Convert.ToInt16(mcollect.ws_decimal5);
            tab_calc.line_spacing = mcollect.ar_decimal6[2];
            tab_calc.report_type = "";
            tab_calc.menu_option = "";
            tab_calc.wide_column = mcollect.ar_decimal6[3].ToString();
            tab_calc.suppress_zero = mcollect.ar_decimal6[1].ToString();
            tab_calc.transfer_code = mcollect.ar_decimal6[0].ToString();
            tab_calc.internal_use = "N";
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

            if (err_flag)
            {
                //delete the record in tab array and write from tab temp rep
                str1 = "";

                str1 = "delete from tab_array where para_code like '" + type_code + "%' and array_code=" + utils.pads(mcollect.ws_code);
                db.Database.ExecuteSqlCommand(str1);

                str1 = "insert into tab_array (para_code,array_code,count_array,operand,source1,period,operator1,amount,percent1,true_desc,select1,sort1,false_desc,amended_by,fldr_code) ";
                str1 += " select row_col2,report_code,row_number() over (partition by report_code order by advice_count,rep_count),op1,src1,perd1,";
                str1 += " sort_order,sort1,sort2,row_desc, row_abs,row_row1,row_col, " + utils.pads(pblock.userid) + ",row_col3 from tab_temp_rep ";
                str1 += " where user_code=" + utils.pads(pblock.userid) + " and row_col2 = '" + type_code + "H' order by advice_count,rep_count ";

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
            

                str1 = "insert into tab_array (para_code,array_code,count_array,sort1,operand,source1,period,operator1,amended_by,date_amended,fldr_code) ";
                str1 += " select row_col2,report_code,rep_count,sort1,op1,src1,perd1,";
                str1 += " case sort1 when 1 then '+' when 2 then '-' when 3 then '*' when 4 then '/' else '' end, ";
                str1 += utils.pads(pblock.userid) + ", getutcdate(),row_col3 from tab_temp_rep ";
                str1 += " where user_code=" + utils.pads(pblock.userid) + " and row_col2 = '" + type_code + "C'";

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

                if (err_flag)
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
            mcollect.ws_decimal0 = tab_calc.line_spacing;
            mcollect.ws_decimal5 = tab_calc.column_no;
            mcollect.ar_decimal6[0] = Convert.ToDecimal(tab_calc.transfer_code);
            mcollect.ar_decimal6[1] = Convert.ToDecimal(tab_calc.suppress_zero);
           mcollect.ar_decimal6[2] = Convert.ToDecimal(tab_calc.line_spacing);
           mcollect.ar_decimal6[3] = Convert.ToDecimal(tab_calc.wide_column);
            write_temp("R", tab_calc.calc_code);
            read_names_operand();
            if ((string)(psess.temp0) == "H10" || (string)(psess.temp0) == "HA03")
                mcollect.ws_string8 = "A";
            else
                mcollect.ws_string8 = "";

        }

        private void validate_fields()
        {
            if (string.IsNullOrWhiteSpace(mcollect.ws_code))
            {
                ModelState.AddModelError(String.Empty, "Code can not be Spaces");
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(mcollect.ws_string0))
            {
                ModelState.AddModelError(String.Empty, "Name can not be Spaces");
                err_flag = false;
            }

            if (mcollect.ws_string1 == null)
                mcollect.ws_string1 = "";

            if (mcollect.ws_string2 == null)
                mcollect.ws_string2 = "";

            if (laction == "Create")
            {
                tab_calc cbank = db.tab_calc.Find(type_code, mcollect.ws_code);
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
            List<SelectListItem> ary = new List<SelectListItem>();
            var str2 = utils.listdaily(id);
            foreach (var item in str2.ToList())
            {
                ary.Add(new SelectListItem { Text = item.c2, Value = item.c1 });
            }
            ary.Add(new SelectListItem { Text = "operand", Value = "operand" });

            str2 = utils.listdaily(id);
            foreach (var item in str2.ToList())
            {
                ary.Add(new SelectListItem { Text = item.c2, Value = item.c1 });
            }
            ary.Add(new SelectListItem { Text = "source", Value = "source" });

            str2 = utils.listdaily(id);
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

            return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
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
            return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        private void init_class()
        {
            mcollect.ws_code = "";
            mcollect.ar_string0 = new string[500];
            mcollect.ar_string1 = new string[500];
            mcollect.ar_string2 = new string[500];
            mcollect.ar_string3 = new string[500];
            mcollect.ar_string4 = new string[500];
            mcollect.ar_string6 = new string[500];
            mcollect.ar_string7 = new string[500];
            mcollect.ar_decimal0 = new decimal[500];
            mcollect.ar_decimal1 = new decimal[500];
            mcollect.ar_decimal2 = new decimal[500];
            mcollect.ar_decimal3 = new decimal[500];
            mcollect.ar_decimal4 = new decimal[500];
            mcollect.ar_decimal5 = new decimal[500];
            mcollect.ar_decimal6 = new decimal[500];
            mcollect.ar_bool0 = new bool[500];
            mcollect.ar_bool1 = new bool[500];
            
            if ((string)(psess.temp0) == "H10" || (string)(psess.temp0) == "HA03")
                mcollect.ws_string8 = "A";
            else
                mcollect.ws_string8 = "";
        }

        private void write_temp(string action, string daily_code)
        {

            str1 = "Delete from tab_temp_rep where user_code=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (action == "R")
            {
                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,advice_count,op1,src1,perd1,sort_order,";
                str1 += " sort1,sort2,row_desc,row_abs,row_row1,row_col,row_col2,row_col3) ";
                str1 += " select 'DET'," + utils.pads(pblock.userid) + ",array_code,count_array bing,count_array * 10,operand,source1,period,";
                str1 += " operator1,cast(amount as int), cast(percent1 as int),true_desc,select1,sort1,false_desc,para_code, fldr_code ";
                str1 += "from tab_array where para_code='" + (type_code) + "H' and array_code=" + utils.pads(daily_code);
                db.Database.ExecuteSqlCommand(str1);
                fill_blanks(type_code+"H");

                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,row_col3,op1,src1,perd1,advice_count,row_col2) ";
                str1 = str1 + " select 'DET'," + utils.pads(pblock.userid) + ",array_code,count_array,fldr_code,operand,source1,period,count_array * 10 ,para_code";
                str1 = str1 + " from tab_array where para_code='" + (type_code) + "C' and array_code=" + utils.pads(daily_code);
                db.Database.ExecuteSqlCommand(str1);
                fill_blanks(type_code+"C");

            }

        }

        private void read_names_operand()
        {

// op1-operand
//src1-source1
//perd1-period
//sort order- operator1 - desc absolute
//sort1-amount - desc row
//sort2-percent1 - desc col
//row_desc -true_desc - description
//row_abs=select1 - amt absolute
//row_row1-sort1 - amt row
//row_col - false_desc - amt col

            mcollect.ar_string0 = new string[500];
            mcollect.ar_string1 = new string[500];
            mcollect.ar_string2 = new string[500];
            mcollect.ar_string3 = new string[500];
            mcollect.ar_string4 = new string[500];
            mcollect.ar_string6 = new string[500];
            mcollect.ar_string7 = new string[500];
            mcollect.ar_decimal0 = new decimal[500];
            mcollect.ar_decimal1 = new decimal[500];
            mcollect.ar_decimal2 = new decimal[500];
            mcollect.ar_decimal3 = new decimal[500];
            mcollect.ar_decimal4 = new decimal[500];
            mcollect.ar_decimal5 = new decimal[500];
            mcollect.ar_bool0 = new bool[500];
            mcollect.ar_bool1 = new bool[500];

            var rdailly = from rd in db.tab_temp_rep
                          where rd.user_code == pblock.userid 
                          orderby rd.rep_count
                          select rd;

            string str1 = "select dbo.validate_source1(row_col3,op1,src1,perd1) c1, row_desc c2, sort_order c3,row_abs c4, advice_count t1,sort1 c5,sort2 c6,";
            str1+=" row_row1 c7,row_col c8, op1 c9, src1 c10, perd1 c11,rep_count t2,row_col3 c12 from tab_temp_rep ";
            str1 += " where user_code=" + utils.pads(pblock.userid) + " and row_col2='" + type_code + "H'  order by advice_count, rep_count ";
            var str2 = db.Database.SqlQuery<vw_query>(str1).ToList();

            int ctr = 0;
            
            foreach (var rd1 in str2)
            {
                //mcollect.ar_string1[ctr] = utils.validate_source1(rd1.op1, rd1.src1, ws_op_code, ws_sr_code, rd1.perd1, " tab_type ", " tab_type2 ");
                mcollect.ar_string6[ctr] = rd1.c1;
                mcollect.ar_string2[ctr] = rd1.c9;
                mcollect.ar_string3[ctr] = rd1.c10;
                mcollect.ar_string4[ctr] = rd1.c11;
                mcollect.ar_string7[ctr] = rd1.c12;

                mcollect.ar_string0[ctr] = rd1.c2;
                mcollect.ar_bool0[ctr] = rd1.c3 == "1" ? true : false;
                mcollect.ar_bool1[ctr] = rd1.c4 == "1" ? true : false;
                mcollect.ar_decimal5[ctr] = (ctr+1) * 10;
                mcollect.ar_decimal1[ctr] = Convert.ToDecimal(rd1.c5);
                mcollect.ar_decimal2[ctr] = Convert.ToDecimal(rd1.c6);
                mcollect.ar_decimal3[ctr] = Convert.ToDecimal(rd1.c7);
                mcollect.ar_decimal4[ctr] = Convert.ToDecimal(rd1.c8);

                ctr++;
            }
            ctr++;
            mcollect.ws_decimal8 = ctr;

            ctr = 0;
            str1 = "select advice_count t1,dbo.validate_source1(row_col3,op1,src1,perd1) c1 from tab_temp_rep ";
            str1+=" where user_code=" + utils.pads(pblock.userid) + " and row_col2='" + type_code +"C' order by rep_count ";
            str2 = db.Database.SqlQuery<vw_query>(str1).ToList();

            foreach (var rd1 in str2)
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
            if (pc == "L13")
            {
                psess.temp1 = "Customer Statement Definition";
                psess.temp2 = "T";
                psess.temp8 = "";
            }
            else if (pc == "L14")
            {
                psess.temp1 = "Vendor Statement Definition";
                psess.temp2 = "T";
                psess.temp8 = "";
            }
            else if (pc == "L15")
            {
                psess.temp1 = "Bank Statement Definition";
                psess.temp2 = "T";
                psess.temp8 = "";
            }
            else if (pc == "L16")
            {
                psess.temp1 = "Property Quote Statement Definition";
                psess.temp2 = "T";
                psess.temp8 = "";
            }
            else if (pc == "L17")
            {
                psess.temp1 = "Property Contract Statement Definition";
                psess.temp2 = "T";
                psess.temp8 = "";
            }
            else if (pc == "161")
            {
                psess.temp1 = "Gratuity Slip Definition";
                psess.temp2 = "S";
            }
            else if (pc == "171")
            {
                psess.temp1 = "Coop Slip Definition";
                psess.temp2 = "T";
            }
        }
        [HttpPost]
        public ActionResult update_view(string op, string src1, string perd1, string num1, string pos1, string max_pos, string ws_code, string operat, string reportid)
        {
            //string ws_code; string src1; string perd1; string num1; string pos1; string max_pos; string op; ; string operat;
            int ctr0 = 0; int ctr1 = 0;
            //string opt = idx;
            pblock = (comsess)Session["comsess"];
            vw_collect pcollect = new vw_collect();
            pcollect.ar_decimal0 = new decimal[60];
            type_code = psess.temp0.ToString();
            string h15type_code = "";

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
            int len1 = (operat.Length + 1) / 4;
            int temp_int = 0;
            for (int ct1 = 0; ct1 < len1; ct1++)
            {
                int.TryParse(operat.Substring(ct1 * 4, 4), out temp_int);
                pcollect.ar_decimal0[ct1] = temp_int;
            }

            var str1 = "";

            for (int cr0 = 0; cr0 < len1; cr0++)
            {
                str1 = "update tab_temp_rep set advice_count=" + pcollect.ar_decimal0[cr0];
                str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid) + " and row_col2='" + type_code + "C'";
                str1 += " and rep_count = " + Convert.ToString(cr0 + 1);
                db.Database.ExecuteSqlCommand(str1);
            }

            if (type_code == "H15")
            {
                h15type_code = psess.temp6.ToString();
                if (h15type_code.Substring(0, 1) == "x" && op.Length > 4)
                {
                    if (op == "VDATE" || ((op.Substring(0, 4) == "LINE" || op.Substring(0, 4) == "SINE") && int.TryParse(op.Substring(4), out ctr1)) ||
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
            str1 = "delete from tab_temp_rep where advice_count=0 and report_line='DET' and user_code=" + utils.pads(pblock.userid) + " and row_col2='" + type_code + "C'";
            int rec_count = db.Database.ExecuteSqlCommand(str1);

            bool max_flag = true;
            if (Convert.ToInt16(max_pos) > 50) max_flag = false;
            if (rec_count > 0) max_flag = true;

            if (numstr != "" && max_flag)
            {
                decimal decmax = pcollect.ar_decimal0.Max() + 10;
                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,op1,src1,perd1,advice_count,row_col3,row_col2) values (";
                str1 = str1 + " 'DET'," + utils.pads(pblock.userid) + "," + utils.pads(ws_code);
                str1 = str1 + "," + utils.pads(max_pos) + "," + utils.pads(numstr) + "," + utils.pads(src1);
                str1 = str1 + "," + utils.pads(perd1) + "," + Convert.ToString(decmax) + "," + utils.pads(reportid)+ ",'" + type_code + "C'" +" )";
                db.Database.ExecuteSqlCommand(str1);
            }

            str1 = "update tab_temp_rep set src1='' from tab_temp_rep a, ( ";
            str1 += " select report_code, numeric_ind, report_name10 from tab_soft where para_code in ('CP', 'USERV','FC')) b ";
            str1 += " where b.report_code = a.op1 and ";
            str1 += " ((report_name10 = 'X' and not(src1 = '' or src1 = 'CODE' or src1 = 'DESC')) or(numeric_ind <> 'T' and(src1 = 'CODE' or src1 = 'DESC')) ";
            str1 += " or (numeric_ind = 'T' and report_name10 <> 'X' and (src1 = 'CODE' or src1 = 'DESC'))) ";
            str1 += " and report_line='DET' and user_code=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            // renumbering
            if (rec_count > 0)
                fill_blanks(type_code+"C");

            int ctr = 0;

            pcollect.ar_string1 = new string[60];
            pcollect.ar_decimal0 = new decimal[60];

            str1 = "select advice_count t1,dbo.validate_source1(row_col3,op1,src1,perd1) c1 from tab_temp_rep where user_code=" + utils.pads(pblock.userid) + " and row_col2='" + type_code + "C' order by rep_count ";
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

        private void cupdate_view(int up1 = 0)
        {
            // op1-operand
            //src1-source1
            //perd1-period
            //sort order- operator1 - desc absolute
            //sort1-amount - desc row
            //sort2-percent1 - desc col
            //row_desc -true_desc - description
            //row_abs=select1 - amt absolute
            //row_row1-sort1 - amt row
            //row_col - false_desc - amt col
            int ctr1 = 0;

            ctr1 = Convert.ToInt16(mcollect.ws_decimal8);
            string pos1 = mcollect.ws_string7;

            if (mcollect.ar_string5[0] != "")
            {
                if (!string.IsNullOrWhiteSpace(pos1))
                {
                    int pos2 = Convert.ToInt16(pos1);
                    mcollect.ar_string2[pos2] = mcollect.ar_string5[0];
                    mcollect.ar_string3[pos2] = mcollect.ar_string5[1];
                    mcollect.ar_string4[pos2] = mcollect.ar_string5[2];
                    mcollect.ar_string7[pos2] = mcollect.ar_string5[3];
                }
                else
                {
                    mcollect.ar_string2[ctr1 - 1] = mcollect.ar_string5[0];
                    mcollect.ar_string3[ctr1 - 1] = mcollect.ar_string5[1];
                    mcollect.ar_string4[ctr1 - 1] = mcollect.ar_string5[2];
                    mcollect.ar_string7[ctr1 - 1] = mcollect.ar_string5[3];
                }
            }

            str1 = "delete from tab_temp_rep where user_code=" + utils.pads(pblock.userid) + " and row_col2='" + type_code + "H'";
            db.Database.ExecuteSqlCommand(str1);

            for (int ctr11 = 0; ctr11 < ctr1; ctr11++)
            {
                if (mcollect.ar_decimal5[ctr11] != 0)
                {
                    str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,advice_count,op1,src1,perd1,sort_order,";
                    str1 += " sort1,sort2,row_abs,row_row1,row_col,row_desc,row_col2,row_col3) values ( ";
                    str1 += " 'DET'," + utils.pads(pblock.userid) + "," + utils.pads(mcollect.ws_code) + ",";
                    str1 += Convert.ToString(ctr11) + "," + mcollect.ar_decimal5[ctr11] + ",";
                    str1 += utils.pads(mcollect.ar_string2[ctr11]) + ", " + utils.pads(mcollect.ar_string3[ctr11]) + ", " + utils.pads(mcollect.ar_string4[ctr11]) + ", ";
                    str1 += mcollect.ar_bool0[ctr11] ? "1" : "0";
                    str1 += ", '" + mcollect.ar_decimal1[ctr11] + "', '" + mcollect.ar_decimal2[ctr11] + "', ";
                    str1 += mcollect.ar_bool1[ctr11] ? "1" : "0";
                    str1 += ", '" + mcollect.ar_decimal3[ctr11] + "', '" + mcollect.ar_decimal4[ctr11] + "', ";
                    str1 += utils.pads(mcollect.ar_string0[ctr11]) + " ,'" + type_code + "H',";
                    str1 += utils.pads(mcollect.ar_string7[ctr11]) +  ")";
                    db.Database.ExecuteSqlCommand(str1);
                }
            }

            if (up1 == 0)
            {
                read_names_operand();
                ModelState.Clear();
                mcollect.ws_string7 = "";
            }

        }

        private void check_up2(int ctr1)
        {
            str1 = "select count(0) t1 from tab_temp_rep where advice_count=" + ctr1.ToString();
            var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str11.t1 > 1)
            {
                str1 = "update tab_temp_rep set ";
                if (!string.IsNullOrWhiteSpace(mcollect.ar_string4[0] ))
                    str1+=",op1=" + utils.pads(mcollect.ar_string4[0]) ;
                if (!string.IsNullOrWhiteSpace(mcollect.ar_string4[1] ))
                    str1+=",src1=" + utils.pads(mcollect.ar_string4[1]) ;
                if (!string.IsNullOrWhiteSpace(mcollect.ar_string4[2]))
                    str1 += ",perd1=" + utils.pads(mcollect.ar_string4[2]);
                if (!string.IsNullOrWhiteSpace(mcollect.ar_string4[2]))
                    str1 += ",perd1=" + utils.pads(mcollect.ar_string4[2]);

                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,rep_count,advice_count,op1,src1,perd1,sort_order,";
                str1 += " sort1,sort2,row_abs,row_row1,row_col,row_desc) values ( ";
                str1 += " 'DET'," + utils.pads(pblock.userid) + "," + utils.pads(mcollect.ws_code) + ",";
                str1 += Convert.ToString(ctr1) + "," + mcollect.ar_decimal0[ctr1 - 1] + ",";
                str1 += utils.pads(mcollect.ar_string4[0]) + ", " + utils.pads(mcollect.ar_string4[1]) + ", " + utils.pads(mcollect.ar_string4[2]) + ", '";
                str1 += mcollect.ar_bool2[0] + "', '" + mcollect.ar_decimal5[0] + "', '" + mcollect.ar_decimal5[1] + "', '";
                str1 += mcollect.ar_bool2[1] + "', '" + mcollect.ar_decimal5[2] + "', '" + mcollect.ar_decimal5[3] + "', ";
                str1 += utils.pads(mcollect.ar_string5[0]) + ") ";
            }

        }

        private void update_all_record()
        {
            //update 
            int ctr1 = Convert.ToInt16(mcollect.ws_decimal7) ;

            for (int ctr11 = 0; ctr11 < ctr1; ctr11++)
            {
                str1 = "update tab_temp_rep set report_code=" + utils.pads(mcollect.ws_code) + ", ";
                str1 += " advice_count=" + mcollect.ar_decimal0[ctr11] + " ";
                str1 += " where user_code=" + utils.pads(pblock.userid) + " and rep_count-1 = " + ctr11;
                db.Database.ExecuteSqlCommand(str1);
            }


            // delete advice count 0
            str1 = "delete from tab_temp_rep where advice_count=0 and report_line='DET' and user_code=" + utils.pads(pblock.userid);
            int rec_count = db.Database.ExecuteSqlCommand(str1);
            if (rec_count > 0)
                fill_blanks(type_code+"H");

        }

        private void init_dec5()
        {
            mcollect.ar_decimal5 = new decimal[5];
            mcollect.ar_decimal5[0] = 0;
            mcollect.ar_decimal5[1] = 0;
            mcollect.ar_decimal5[2] = 0;
            mcollect.ar_decimal5[3] = 0;
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
            //utils.write_plog(tab_calc.para_code, select1, opt, "B", pblock.userid,tab_calc.para_code,tab_calc.calc_code);

        }

        private void fill_blanks(string rest_code)
        {
            string str1 = "execute blank_rep @p_userid=" + utils.pads(pblock.userid) + ",@restrict_code =" +utils.pads(rest_code);
            db.Database.ExecuteSqlCommand(str1);
        }



    }
}

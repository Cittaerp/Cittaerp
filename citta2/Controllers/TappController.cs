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
    public class TappController : Controller
    {
        //
        // GET: /Tapp/

        private MainContext db = new MainContext();
        private MainContext db2 = new MainContext();
        Boolean err_flag = true;
        string laction = "";
        vw_collect mcollect = new vw_collect();
        cittautil utils = new cittautil();
        tab_self_approval tab_self_approval = new tab_self_approval();
        comsess pblock;
        string type_code = "", pcode="";
        psess psess;
        string str1;
        int level_count = 60;
        int lctr = 0;

        //
        // GET: /Allow/

        //[EncryptionActionAttribute]
        public ActionResult Index(string pc = null)
        {
            //if (utils.check_option() == 1||pc==null)
            //    return RedirectToAction("Welcome", "Home");

            psess = (psess)Session["psess"];
            if (pc == ""   || pc == null)
                type_code = psess.temp0.ToString();
            else
            {
                psess.temp0 = pc;
                type_code = pc;
            }

            pblock = (comsess)Session["comsess"];
            formula_title(pc);
            psess.temp5 = type_code;
            if (psess.temp3!=null)
            pcode = psess.temp3.ToString();

            string str1 = "select distinct transaction_apv ws_string0, name1_msg ws_string4, category_apv ws_string1,approval_category ws_string2,approval_category_to ws_string3 ";
            str1 += " from tab_self_approval a  left outer join GB_999_MSG g on a.transaction_apv = g.code_msg and g.type_msg = 'HEAD' and name6_msg = 'P'";
                //str1=str1 + "e.c2 ws_string4,b.wname ws_string5, c.pname ws_string6,d.pname ws_string7 from tab_self_approval a ";
                //str1 = str1 + " left outer join vw_groupcodes b on (b.wcode=category_apv ) ";
                //str1 = str1 + " left outer join vw_user_parameter c on (c.pcode=approval_category and c.hcode=category_apv ) ";
                //str1 = str1 + " left outer join vw_user_parameter d on (d.pcode=approval_category_to and d.hcode=category_apv ) ";
                //str1 = str1 + " left outer join (select report_code as c1, report_name1 as c2 from tab_soft where para_code in ('PTRAN','HENQ') ";
                //str1 = str1 + " union all select trans_type, name1 from tab_type union all select 'ALL', 'All Transactions' ";
                //str1 = str1 + " union all select calc_code, name1 from tab_calc where para_code in ('H46','F12')) e on e.c1=transaction_apv ";
                str1 = str1 + " where a.para_code=" + utils.pads(pcode);
            var blist = db.Database.SqlQuery<vw_collect>(str1);
            Session["psess"] = psess;
            return View(blist.ToList());
            
        }

        //
        // GET: /Bank/Create

        //[EncryptionActionAttribute]
        public ActionResult Create(string xy = null)
        {
            psess = (psess)Session["psess"];
            mcollect.datmode = "C";
            //if (utils.check_option() == 1 || xy != "1")
            //    return RedirectToAction("Welcome", "Home");

            pblock = (comsess)Session["comsess"];
            type_code = psess.temp0.ToString();
            pcode = psess.temp3.ToString();
            init_class();
            write_temp("");
            bank_group_select();
            mcollect.ws_decimal7 = 1;
            read_names_operand("R");
            Session["psess"] = psess;
            return View("Edit", mcollect);
        }

        //
        // POST: /Bank/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_collect mcollect1)
        {
            pblock = (comsess)Session["comsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            type_code = psess.temp0.ToString();
            pcode = psess.temp3.ToString();
            laction = "Create";

            write_data();
            if (err_flag)
                return RedirectToAction("Create"); //, null, new { anc = Ccheckg.convert_pass2("xy=1") });

            bank_group_select();
            mcollect.datmode = "C";
            Session["psess"] = psess;
            return View("Edit", mcollect);

        }

        //

        //[EncryptionActionAttribute]
        public ActionResult Edit(string jy = null, string id2 = "", string id3 = "", string id4 = "")
        {
            mcollect.datmode = "E";
            //if (utils.check_option() == 1 || jy == null)
            //    return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            type_code = psess.temp0.ToString();
            pcode = psess.temp3.ToString();
            pblock = (comsess)Session["comsess"];

            tab_self_approval = db.tab_self_approval.Find(pcode, jy, id2, id3, id4,1);
            if (tab_self_approval == null)
            {
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            psess.temp4 = "1";
            read_daily_record();
            bank_group_select();
            Session["psess"] = psess;
            return View(mcollect);
        }

        //
        // POST: /Bank/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(vw_collect mcollect1,string xhrt)
        {
            pblock = (comsess)Session["comsess"];
            psess = (psess)Session["psess"];
            type_code = psess.temp0.ToString();
            pcode = psess.temp3.ToString();
            mcollect = mcollect1;

            if (xhrt == "D")
            {
                DeleteConfirmed();
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
            }

            write_data();
            if (err_flag)
                return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});

            bank_group_select();
            mcollect.datmode = "E";
            Session["psess"] = psess;
            return View(mcollect);

        }

        //
        // POST: /Bank/Delete/5

        //[HttpPost, ActionName("Delete")]
        private void DeleteConfirmed()
        {
            write_log("Delete");
            string  str1 = "delete from tab_self_approval where para_code=" + utils.pads(pcode) + " and transaction_apv=" + utils.pads(mcollect.ws_code);
            str1 += " and category_apv=" + utils.pads(mcollect.ws_string0) + " and approval_category=" + utils.pads(mcollect.ws_string1);
            str1 += " and approval_category_to=" + utils.pads(mcollect.ws_string2);
            db.Database.ExecuteSqlCommand(str1);

//            return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void bank_group_select(string oper1 = null)
        {

            var bglist1 = from bg in db.GB_999_MSG
                          where bg.type_msg == "HEAD" && bg.name6_msg == "P"
                          select bg;
            ViewBag.transaction = new SelectList(bglist1.ToList(), "code_msg", "name1_msg", mcollect.ws_code);
            //var bgsort2 = from bg in db.GB_999_MSG
            //              where bg.type_msg == "BASED"
            //              select bg;
            //ViewBag.based = new SelectList(bgsort2.ToList(), "code_msg", "name1_msg");


            if (pcode == "SELF")
            {
                str1 = "select array_code as query0, operand as query1, '' from tab_array where para_code='SELF' UNION ALL ";
                str1 = str1 + "select train_code, course_name, '' from tab_train where para_code='SELFT' union all ";
                //str1 = str1 + "select report_code, report_name1, '' from tab_soft where para_code='TRANSEXP' union all ";
                str1 = str1 + "select report_code, report_name1,'' from tab_soft where para_code = 'SELFT' order by 2 ";
            }
            else if (pcode == "ENT")
            {
                str1 = "select 'ALL' as c1, 'All Transactions' as c2,'0' c3 ";
                str1 = str1 + " union all select trans_type , name1, '1' from tab_type  ";
                str1 = str1 + " union all select report_code, report_name1,'1' from tab_soft where para_code = 'PTRAN' ";
                str1 = str1 + " order by 3,2 ";
            }
            else if (pcode == "GPAY")
                str1 = "select report_code c1,report_name1 c2,numeric_ind c3 from tab_soft where para_code='PTRAN' and report_name4 = 'G' order by 2 ";
            else if (pcode == "CASH")
                str1 = "select calc_code c1,name1 c2,'1' c3 from tab_calc where para_code='H46' order by 2 ";

            //           var bglist1 = db.Database.SqlQuery<querylay>(str1);
            //            ViewBag.transaction = new SelectList(bglist1.ToList(), "c1", "c2");

            str1 = "";
            if (pcode == "SELF" || pcode == "CASH")
            {
                //str1 = "select train_code as c1, course_name as c2,'1' from tab_train where para_code='H29' union all ";
                str1 = "select code_msg as query0, name1_msg as query1,'1' from GB_999_MSG where type_msg='H29' union all ";
                str1 = str1 + "select report_code, report_name1,'0' from tab_soft where para_code='APORG' union all ";
            }

            str1 = str1 + " select employee_code as query0, name as query1,'3' from GB_001_EMP where close_code='0' union all  ";
            str1 = str1 + "select report_code, report_name1,'0' from tab_soft where para_code='APORG' ";
            str1 = str1 + " union all select '', '' ,'0' order by 3, 2 ";
            var bglist2 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.staff = new SelectList(bglist2.ToList(), "query0", "query1");

            str1 = "select header_sequence query0,header_description query1 ,4 from GB_001_HANAL a, GB_001_Header b where b.sequence_no <> 99 and ";
            str1 += " b.header_code = a.header_sequence and header_type_code = " + utils.sqlquote(mcollect.ws_code) + " union all ";
            //str1 += " select report_code c1,report_name1 c2 ,4 from tab_soft where para_code='DSEL' and report_code='H32' union all ";
            str1 += " select report_code,report_name1,numeric_ind from tab_soft where para_code='PAYGRP' order by 2 ";
            var bgsort2 = db.Database.SqlQuery<querylay>(str1);

            ViewBag.based = new SelectList(bgsort2.ToList(), "query0", "query1");
            str1 = "select report_code as c1, report_name1 as c2, ''  from tab_soft where para_code = 'DSEL' and report_name5='Y'  union all ";
            str1 = str1 + "select analy_code as c1, analy0 as c2, analy1 as c3 from tab_analy where para_code in ('ANALY') union all select '','',''  order by 2 ";
            //            var bgsort22 = db.Database.SqlQuery<querylay>(str1);

            //            ViewBag.add_on = new SelectList(bgsort22.ToList(), "c1", "c2");

            List<SelectListItem> sortad = new List<SelectListItem>();
            sortad.Add(new SelectListItem { Text = "", Value = "0" });
            sortad.Add(new SelectListItem { Text = "OR", Value = "1" });
            sortad.Add(new SelectListItem { Text = "AND", Value = "2" });
            ViewBag.sort_order = sortad;

            str1 = "select gcode query0, gcode + ' : ' + gname query1 from vw_grouptrans where gpara=" + utils.sqlquote(mcollect.ws_code);
            var str2 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.select_from1 = new SelectList(str2.ToList(), "query0", "query1");
            ViewBag.select_to1 = ViewBag.select_from1;

            str1 = "select pcode c1, pcode + ' : ' + pname c2 from vw_user_parameter where hcode=" + utils.sqlquote(mcollect.ws_code);
            //            str2 = db.Database.SqlQuery<querylay>(str1);
            //            ViewBag.codef = new SelectList(str2.ToList(), "c1", "c2");
            //            ViewBag.codet = ViewBag.codef;

            var bglist6 = from bg in db.GB_001_EMP
                              //where bg.close_code == "0"
                          orderby bg.name
                          select new { c1 = bg.employee_code, c2 = bg.name };

            ViewBag.staffonly = bglist6.ToList();

        }

        private void write_data()
        {
            pcode = (string)psess.temp3;
            err_flag = true;
            validate_fields();

            if (err_flag)
                write_rec();
        }

        private void write_rec()
        {
            DateTime odate = utils.logdatetime();
            var str1 = "";
            str1 = "update tab_temp_rep set report_code=" + utils.pads(pcode) + " , sort1=" + utils.pads(mcollect.ws_code);
                str1 += ", op1=" + utils.pads(mcollect.ws_string0) + ", src1=" + utils.pads(mcollect.ws_string1);
                str1 += ", perd1=" + utils.pads(mcollect.ws_string2) + " where user_code=" + utils.pads((pblock.userid));
                db.Database.ExecuteSqlCommand(str1);

                if (!(mcollect.ar_string2 == null))
                {
                    for (int cr0 = 0; cr0 < mcollect.ar_string2.Length; cr0++)
                    {
                        str1 = "update tab_temp_rep set sort2=" + utils.pads(mcollect.ar_string2[cr0]);
                        str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid);
                        str1 += " and rep_count = " + Convert.ToString(cr0 + 1);
                        db.Database.ExecuteSqlCommand(str1);
                    }
                }

                if (!(mcollect.ar_decimal2 == null))
                {
                    for (int cr0 = 0; cr0 < mcollect.ar_decimal2.Length; cr0++)
                    {
                        str1 = "update tab_temp_rep set row_col=" + utils.pads(mcollect.ar_decimal2[cr0].ToString());
                        str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid);
                        str1 += " and rep_count = " + Convert.ToString(cr0 + 1);
                        db.Database.ExecuteSqlCommand(str1);
                    }
                }
                else
                {
                    str1 = "update tab_temp_rep set row_col='0.00' ";
                    str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid);
                    db.Database.ExecuteSqlCommand(str1);
                }

                str1 = "delete from tab_self_approval where para_code=" + utils.pads(pcode) + " and transaction_apv=" + utils.pads(mcollect.ws_code);
                str1 += " and category_apv=" + utils.pads(mcollect.ws_string0);
                str1 += " and approval_category=" + utils.pads(mcollect.ws_string1);
                str1 += " and approval_category_to=" + utils.pads(mcollect.ws_string2);
                db.Database.ExecuteSqlCommand(str1);

                str1 = "insert into tab_self_approval(para_code,transaction_apv,category_apv,approval_category,approval_category_to,level_count_apv,level1_apv,";
                str1 += "level11_apv,information_apv,level1_and_apv,min_amount,amended_by) ";
                str1 += " select report_code,sort1,op1,src1,perd1,row_number() over (order by rep_count),sort_order,row_abs,row_row1,";
                str1 += " case sort2 when 1 then 'OR' when 2 then 'AND' else '' end, row_col,";
                str1 += utils.pads(pblock.userid) + " from tab_temp_rep ";
                str1 += " where not (sort_order = '' and row_abs = '') and user_code=" + utils.pads((pblock.userid));

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


// for the additional
                if (mcollect.ws_string3 != "")
                {
                    str1 = "insert into tab_self_approval(para_code,transaction_apv,category_apv,approval_category,approval_category_to,level_count_apv,level1_apv,";
                    str1 += "level1_and_apv,level11_apv,amended_by) values ( " + utils.pads(pcode) + ", ";
                    str1 += utils.pads(mcollect.ws_code) + ", " + utils.pads(mcollect.ws_string0) + ", " + utils.pads(mcollect.ws_string1) + ", " + utils.pads(mcollect.ws_string2) + ", 99,";
                    str1 += utils.pads(mcollect.ws_string3) + "," + utils.pads(mcollect.ws_string4) + ", " + utils.pads(mcollect.ws_string5) + ", ";
                    str1 += utils.pads(pblock.userid) + ")";

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

// for the escalation
                level_count = 60;
                for (int ictr = 0; ictr < 10; ictr++)
                {
                    if (!string.IsNullOrWhiteSpace(mcollect.ar_string8[ictr]))
                    {
                        lctr = level_count + ictr;
                        str1 = "insert into tab_self_approval(para_code,transaction_apv,category_apv,approval_category,approval_category_to,level_count_apv,level1_apv,";
                        str1 += "level11_apv,amended_by) values ( " + utils.pads(pcode) + ", ";
                        str1 += utils.pads(mcollect.ws_code) + ", " + utils.pads(mcollect.ws_string0) + ", " + utils.pads(mcollect.ws_string1) + ", " + utils.pads(mcollect.ws_string2) + ", " + lctr.ToString() + ",";
                        str1 += utils.pads(mcollect.ar_string8[ictr]) + "," + utils.pads(mcollect.ar_decimal5[ictr].ToString()) + ", ";
                        str1 += utils.pads(pblock.userid) + ")";

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

// for information mail..
                if (!string.IsNullOrWhiteSpace(mcollect.ws_string6))
                {
                    str1 = "insert into tab_self_approval(para_code,transaction_apv,category_apv,approval_category,approval_category_to,level_count_apv,level1_apv,";
                    str1 += "amended_by) values ( " + utils.pads(pcode) + ", ";
                    str1 += utils.pads(mcollect.ws_code) + ", " + utils.pads(mcollect.ws_string0) + ", " + utils.pads(mcollect.ws_string1) + ", " + utils.pads(mcollect.ws_string2) + ", 71,";
                    str1 += utils.pads(mcollect.ws_string6) + "," + utils.pads(pblock.userid) + ")";

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
                if (!string.IsNullOrWhiteSpace(mcollect.ws_string7))
                {
                    str1 = "insert into tab_self_approval(para_code,transaction_apv,category_apv,approval_category,approval_category_to,level_count_apv,level1_apv,";
                    str1 += "amended_by) values ( " + utils.pads(pcode) + ", ";
                    str1 += utils.pads(mcollect.ws_code) + ", " + utils.pads(mcollect.ws_string0) + ", " + utils.pads(mcollect.ws_string1) + ", " + utils.pads(mcollect.ws_string2) + ", 72,";
                    str1 += utils.pads(mcollect.ws_string7) + "," + utils.pads(pblock.userid) + ")";

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
                    write_temp("");
                    write_log(laction);
                }

        }

        private void read_daily_record()
        {
            init_class();

            
            var bglist = from bg in db.GB_999_MSG
                         where bg.code_msg == tab_self_approval.transaction_apv && bg.type_msg == "HEAD"
                         select bg.name1_msg;
            mcollect.ws_code = bglist.FirstOrDefault();
            mcollect.ws_string0 = tab_self_approval.category_apv;
            mcollect.ws_string1 = tab_self_approval.approval_category;
            mcollect.ws_string2 = tab_self_approval.approval_category_to;
            transaction_name();

            write_temp("R", tab_self_approval);
            read_names_operand("R");
            if ((string)(psess.temp0) == "H20" || (string)(psess.temp0) == "HA20")
                mcollect.ws_string8 = "A";
            else
                mcollect.ws_string8 = "";

        }

        private void validate_fields()
        {
            string err_message = "";
            if (string.IsNullOrWhiteSpace(mcollect.ws_code)){
                ModelState.AddModelError(String.Empty, "Code can not be Spaces");
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(mcollect.ws_string0)){
                ModelState.AddModelError(String.Empty, "Group can not be Spaces");
                err_flag = false;
            }
            if (string.IsNullOrWhiteSpace(mcollect.ws_string1)){
                ModelState.AddModelError(String.Empty, "Range can not be Spaces");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string2))
                mcollect.ws_string2 = mcollect.ws_string1; 

            if (mcollect.ws_string3 == null)
                mcollect.ws_string3 = "";
            if (mcollect.ws_string4 == null)
                mcollect.ws_string4 = "";
            if (mcollect.ws_string5 == null)
                mcollect.ws_string5 = "";

            read_names_operand("R");
            if (psess.temp3.ToString() == "CASH")
            {
                mcollect.ar_string2 = new string[50];
                for (int ctr = 0; ctr < 50; ctr++)
                    mcollect.ar_string2[ctr] = "";
            }

            for (int ctr = 0; ctr < mcollect.ws_decimal7 - 1; ctr++)
            {
                if ((mcollect.ar_string2[ctr] != "0" && mcollect.ar_string2[ctr] != "") && (mcollect.ar_string3[ctr] == "" || mcollect.ar_string1[ctr] == ""))
                {
                    ModelState.AddModelError(String.Empty, "Logical Operator requires 2 ");
                    err_flag = false;
                }
            }

            if (laction == "Create" )
            {
                tab_self_approval cbank = db.tab_self_approval.Find(pcode, mcollect.ws_code, mcollect.ws_string0, mcollect.ws_string1, mcollect.ws_string2, 1);
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
            string Code = id;
            var ind = Code.Substring(0, 2);
            var rcode = Code.Substring(2);

            str1 = "select gcode query0, gcode + ' : ' + gname query1 from vw_grouptrans where gpara=" + utils.sqlquote(id);
            var str2 = db.Database.SqlQuery<querylay>(str1);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "query0",
                                "query1")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");//,null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        private void init_class()
        {
            utils.init_values();
            mcollect.ws_code = "";
            mcollect.ar_decimal0 = new decimal[50];
            mcollect.ar_decimal1 = new decimal[50];
            mcollect.ar_decimal2 = new decimal[50];
            mcollect.ar_decimal3 = new decimal[50];
            mcollect.ar_decimal5 = new decimal[50];
            mcollect.ar_string0 = new string[50];
            mcollect.ar_string1 = new string[50];
            mcollect.ar_string2 = new string[50];
            mcollect.ar_string3 = new string[50];
            mcollect.ar_string4 = new string[50];
            mcollect.ar_string8 = new string[50];
            mcollect.ar_string9 = new string[50];
            mcollect.ar_bool0 = new bool[50];
            mcollect.ar_bool1 = new bool[50];
            mcollect.ar_bool2 = new bool[50];
            
            if ((string)(psess.temp0) == "H20" || (string)(psess.temp0) == "HA20")
                mcollect.ws_string8 = "A";
            else
                mcollect.ws_string8 = "";
        }

        private void write_temp(string action, tab_self_approval tab_self_approval=null)
        {
            string str1;
            str1 = "Delete from tab_temp_rep where user_code=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (action == "R")
            {
                str1 = "insert into tab_temp_rep(report_line,user_code,report_code,sort1,op1,src1,perd1,rep_count,sort_order,sort2,row_abs,row_row1,row_col) ";
                str1 += " select 'DET'," + utils.pads(pblock.userid) + ",para_code,transaction_apv,category_apv,approval_category,approval_category_to,level_count_apv,";
                str1 += " level1_apv,";
                str1 += " case level1_and_apv when 'OR' then 1 when 'AND' then 2 else 0 end ,level11_apv,information_apv,min_amount ";
                str1 += " from tab_self_approval where para_code=" + utils.pads(pcode) + " and transaction_apv=" + utils.pads(tab_self_approval.transaction_apv);
                str1 += " and category_apv=" + utils.pads(tab_self_approval.category_apv);
                str1 += " and approval_category=" + utils.pads(tab_self_approval.approval_category);
                str1 += " and approval_category_to=" + utils.pads(tab_self_approval.approval_category_to);
                str1 += " and level_count_apv < 50 ";
                db.Database.ExecuteSqlCommand(str1);

                var bg99 = from bg9 in db2.tab_self_approval
                            where bg9.transaction_apv == tab_self_approval.transaction_apv && bg9.category_apv == tab_self_approval.category_apv &&
                                 bg9.approval_category == tab_self_approval.approval_category && bg9.level_count_apv > 50 && bg9.para_code==pcode &&
                                 bg9.approval_category_to == tab_self_approval.approval_category_to
                            select bg9;

                foreach(var item in bg99.ToList())
                {
                    if (item.level_count_apv > 59 && item.level_count_apv < 70)
                    {
                        mcollect.ar_string8[item.level_count_apv - 60] = item.level1_apv;
                        mcollect.ar_decimal5[item.level_count_apv - 60] = Convert.ToInt16(item.level11_apv);
                    }
                    else if (item.level_count_apv == 71)
                        mcollect.ws_string6 = item.level1_apv;
                    else if (item.level_count_apv == 72)
                        mcollect.ws_string7 = item.level1_apv;
                    else
                    mcollect.ws_string3 = item.level1_apv;
                    mcollect.ws_string4 = item.level1_and_apv;
                    mcollect.ws_string5 = item.level11_apv;
                }
            }

        }

        private void read_names_operand(string action)
        {
            //var rdailly = from rd in db.tab_temp_rep
            //              where rd.user_code == pblock.userid
            //              orderby rd.rep_count
            //              select rd;
            int ctr = 0;
            mcollect.ar_string1 = new string[50];
            mcollect.ar_string3 = new string[50];
            mcollect.ar_string4 = new string[50];

            if (action == "R")
            {
                mcollect.ar_string2 = new string[50];
                mcollect.ar_decimal2 = new decimal[50];
            }

            str1 = "select dbo.find_name(5,'APORG',sort_order) + dbo.find_name(0,'H29',sort_order) + dbo.find_name(4,sort_order, '') query0 ,";
            str1 += " dbo.find_name(5,'APORG',row_abs) + dbo.find_name(0,'H29',row_abs) + dbo.find_name(4,row_abs, '') query1 ,";
            str1 += " dbo.find_name(5,'APORG',row_row1) + dbo.find_name(0,'H29',row_row1) + dbo.find_name(4,row_row1,'') query2,sort2 query3, row_col query4 ";
            str1 += " from tab_temp_rep where user_code=" + utils.sqlquote(pblock.userid) + " order by rep_count ";

            var rdaily = db.Database.SqlQuery<querylay>(str1);

                        foreach (var rd1 in rdaily.ToList())
            {
                mcollect.ar_string1[ctr] = rd1.query0;
                mcollect.ar_string3[ctr] = rd1.query1;
                mcollect.ar_string4[ctr] = rd1.query2;

                if (action == "R")
                {
                    mcollect.ar_string2[ctr] = rd1.query3;
                    decimal.TryParse(rd1.query4, out mcollect.ar_decimal2[ctr]);
                }

                ctr++;
            }
            ctr++;
            mcollect.ws_decimal7 = ctr;


        }

        private string check_name(string sno)
        {

            string str1 = "select c1, c2, c3 from ( ";
            str1 += "select train_code as c1, course_name as c2,'1' c3 from tab_train where para_code='H29' union all ";
            str1 += " select report_code, report_name1,'0' from tab_soft where para_code='APORG' union all ";
	        str1 += " select staff_number, staff_name,'3' from tab_staff where close_code ='0') b ";
            str1 += " where c1=" + utils.pads(sno);
            var str2 = db2.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            if (str2 == null)
                return "";
            else
                return str2.c2;

        }


        private void formula_title(string pc)
        {
            if (pc == "01")
            {
                psess.temp1 = "Transaction Approval Definition ";
                psess.temp3 = "ENT";
                psess.temp7 = " and report_name5='Y' ";
                psess.temp8 = "";
                psess.temp9 = "PYREP";
            }
            else if (pc == "02")
            {
                psess.temp1 = "SelfService Approval Definition ";
                psess.temp3 = "SELF";
                psess.temp7 = " and report_name5='Y' ";
                psess.temp8 = "Y";
                psess.temp9 = "PYREP";
            }
            else if (pc == "03")
            {
                psess.temp1 = "Group Payroll Approval Definition ";
                psess.temp3 = "GPAY";
                psess.temp7 = " and report_name5='Y' ";
                psess.temp8 = "Y";
                psess.temp9 = "PYREP";
            }
            else if (pc == "CH")
            {
                psess.temp1 = "Expense Approval Definition ";
                psess.temp3 = "CASH";
                psess.temp7 = " and report_name5='Y' ";
                psess.temp8 = "Y";
                psess.temp9 = "PYREP";
            }
            else if (pc == "P04")
            {
                psess.temp1 = "Gratuity Report Definition ";
                psess.temp2 = "S";
                psess.temp7 = " and report_name10='Y' ";
                psess.temp8 = "";
                psess.temp9 = "GTREP";
            }
            else if (pc == "FL")
            {
                psess.temp1 = "Incidence Approval Definition ";
                psess.temp3 = "CRM";
                psess.temp7 = " and report_name5='Y' ";
                psess.temp8 = "Y";
                psess.temp9 = "FLREP";
            }

        }

        [HttpPost]
        public ActionResult update_view(string id)
        {
            pblock = (comsess)Session["comsess"];
            psess = (psess)Session["psess"];
            string pos1, max_pos, op, operat,amtvar;
            int ctr0 = 0, ctr1 = 0;
            string opt = id;
            vw_collect pcollect = new vw_collect();
            
            bool update_flag=false;
            string str1,insert_opt;
 
            insert_opt= opt.Substring(0, 1);
            ctr0 = opt.IndexOf("[]",3);
            op = opt.Substring(3, ctr0 - 3);
            ctr1 = opt.IndexOf("[]", ctr0 + 2);
            pos1= opt.Substring(ctr0 + 2, ctr1 - ctr0 - 2);
            ctr0 = opt.IndexOf("[]", ctr1 + 2);
            max_pos= opt.Substring(ctr1 + 2, ctr0 - ctr1 - 2);
            ctr1 = opt.IndexOf("[]", ctr0 + 2);
            operat= opt.Substring(ctr0 + 2, ctr1 - ctr0 - 2);
            amtvar = opt.Substring(ctr1 + 2) + " ";
            int len1 = operat.Length;

            if (Convert.ToInt16(max_pos) < 10)
            {
                if (pos1 == "")
                    update_flag = true;

                if (!update_flag)
                {

                    if (Convert.ToUInt16(max_pos) == Convert.ToUInt16(pos1))
                        update_flag = true;
                };

                if (update_flag)
                {
                    str1 = "insert into tab_temp_rep(report_line,user_code,rep_count,sort_order,row_abs,row_row1) values (";
                    str1 += " 'DET'," + utils.pads(pblock.userid) + "," + utils.pads(Convert.ToString(max_pos)) + ",";
                    if (insert_opt == "1") str1 += utils.pads(op) + ",'','')";
                    if (insert_opt == "2") str1 += "'', " + utils.pads(op) + ",'')";
                    if (insert_opt == "3") str1 += "'','', " + utils.pads(op) + ")";
                    db.Database.ExecuteSqlCommand(str1);

                }
                else
                {
                    int ctr2 = Convert.ToInt32(pos1) + 1;
                    string sctr2 = Convert.ToString(ctr2);
                    str1 = "update tab_temp_rep set ";
                    if (insert_opt == "1") str1 += " sort_order= " + utils.pads(op);
                    if (insert_opt == "2") str1 += " row_abs= " + utils.pads(op);
                    if (insert_opt == "3") str1 += " row_row1= " + utils.pads(op);
                    str1 += " where user_code=" + utils.pads(pblock.userid);
                    str1 += " and rep_count = " + sctr2;
                    db.Database.ExecuteSqlCommand(str1);
                }
            }

            for (int cr0 = 0; cr0 < len1; cr0++)
            {
                str1 = "update tab_temp_rep set sort2=" + utils.pads(operat.Substring(cr0, 1));
                str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid);
                str1 += " and rep_count = " + Convert.ToString(cr0 + 1);
                db.Database.ExecuteSqlCommand(str1);
            }

            string[] amtsepa = new string[] { "bb" };
            string[] amtval;
            amtval = amtvar.Split(amtsepa, StringSplitOptions.None);

            for (int cr0 = 0; cr0 < amtval.Length; cr0++)
            {
                str1 = "update tab_temp_rep set row_col=" + utils.pads(amtval[cr0]);
                str1 += " where report_line='DET' and user_code=" + utils.pads(pblock.userid);
                str1 += " and rep_count = " + Convert.ToString(cr0 + 1);
                db.Database.ExecuteSqlCommand(str1);
            }

            //var rdailly = from rd in db.tab_temp_rep
            //              where rd.user_code == pblock.userid
            //              orderby rd.rep_count
            //              select rd;
            int ctr = 0;
            pcollect.ar_string1 = new string[50];
            pcollect.ar_string2 = new string[50];
            pcollect.ar_string3 = new string[50];
            pcollect.ar_string4 = new string[50];
            pcollect.ar_decimal2 = new decimal[50];

            str1 = "select dbo.find_name(5,'APORG',sort_order) + dbo.find_name(0,'H29',sort_order) + dbo.find_name(4,sort_order, '') query0 ,";
            str1 += " dbo.find_name(5,'APORG',row_abs) + dbo.find_name(0,'H29',row_abs) + dbo.find_name(4,row_abs, '') query1 ,";
            str1 += " dbo.find_name(5,'APORG',row_row1) + dbo.find_name(0,'H29',row_row1) + dbo.find_name(4,row_row1,'') query2,sort2 query3, row_col query4 ";
            str1 += " from tab_temp_rep where user_code=" + utils.sqlquote(pblock.userid) + " order by rep_count ";

            var rdaily = db.Database.SqlQuery<querylay>(str1);

            foreach (var rd1 in rdaily.ToList())
            {
                pcollect.ar_string1[ctr] = rd1.query0;
                pcollect.ar_string3[ctr] = rd1.query1;
                pcollect.ar_string4[ctr] = rd1.query2;
                pcollect.ar_string2[ctr] = rd1.query4;
                decimal.TryParse(rd1.query5, out pcollect.ar_decimal2[ctr]);

                ctr++;
            }
            ctr++;
            pcollect.ws_decimal7 = ctr;

            List<SelectListItem> sortad = new List<SelectListItem>();
            sortad.Add(new SelectListItem { Text = "", Value = "0" });
            sortad.Add(new SelectListItem { Text = "OR", Value = "1" });
            sortad.Add(new SelectListItem { Text = "AND", Value = "2" });
            ViewBag.sort_order = sortad;
            string vname = "_TapprView";
           // if (psess.temp3.ToString() == "CASH")
                vname = "_CashAView";
            Session["psess"] = psess;
            return PartialView(vname, pcollect);

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

            string select1 = " where para_code=" + utils.pads(pcode) + " and transaction_apv=" + utils.pads(mcollect.ws_code);
            select1 += " and category_apv=" + utils.pads(mcollect.ws_string0) + " and approval_category=" + utils.pads(mcollect.ws_string1);
            select1 += " and approval_category_to=" + utils.pads(mcollect.ws_string2);
            utils.write_plog(type_code, select1, opt, "A", pblock.userid,"", mcollect.ws_code);

        }

        private void transaction_name()
        {
            mcollect.dsp_string = new string[10];
            str1 = "select 1 t1, codename c1,gname c2, gcode c3 from vw_grouptrans where gpara=" + utils.pads(mcollect.ws_string0) + " and gcode=" + utils.pads(mcollect.ws_string1);
            str1 += " union all select 2 t1, codename c1,gname c2, gcode c3 from vw_grouptrans where gpara=" + utils.pads(mcollect.ws_string0) + " and gcode=" + utils.pads(mcollect.ws_string2);
            var keyfs = db.Database.SqlQuery<vw_query>(str1);
            foreach (var item in keyfs.ToList())
            {
                mcollect.dsp_string[1] = item.c1;
                if (item.t1 == 1) mcollect.dsp_string[2] = item.c3 + " : " + item.c2;
                if (item.t1 == 2) mcollect.dsp_string[3] = item.c3 + " : " + item.c2;
            }


            if (pcode == "SELF")
            {
                str1 = "select source1 as c1, operand as c2, '' c3 from tab_array where para_code='SELFT' UNION ALL ";
                str1 = str1 + "select train_code, course_name, '' from tab_train where para_code='SELFT' union all ";
                str1 = str1 + "select report_code, report_name1,'' from tab_soft where para_code = 'SELFT' ";
            }
            else if (pcode == "ENT")
            {
                str1 = "select 'ALL' as c1, 'All Transactions' as c2,'0' c3 ";
                str1 = str1 + " union all select trans_type , name1, '1' from tab_type  ";
                str1 = str1 + " union all select report_code, report_name1,'1' from tab_soft where para_code = 'PTRAN' ";
            }
            else if (pcode == "GPAY")
                str1 = "select report_code c1,report_name1 c2,numeric_ind c3 from tab_soft where para_code='PTRAN' and report_name4 = 'G' ";
            else if (pcode == "CASH")
                str1 = "select calc_code c1,name1 c2,'1' c3 from tab_calc where para_code='H46' ";

            str1 = "select c2 from (" + str1 + ") b where c1=" + utils.pads(mcollect.ws_code);
            var tn1 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            mcollect.dsp_string[0] = tn1 == null ? "" : tn1.c2;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Globalization;
using anchor1.Models;
using System.Web.Mvc;
using System.Collections;
using System.Net;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using System.Drawing;

namespace anchor1.utilities
{
    public class Cutil
    {
        //string p_name_txt = "";
        //string p_src_name = "";
        string[] selfqflag = new string[30];

        string str30 = ""; string str31 = ""; string str41 = ""; string str51 = "";

        //DataClasses1DataContext context = new DataClasses1DataContext();
        private anchor1Context db = new anchor1Context();
        vw_query str2;

        public string pads(string oldstr)
        {
            if (oldstr == null)
                oldstr = "";
            string trimstr = oldstr.Trim();
            string newstr = "'" + trimstr.Replace("'", "''") + "'";
            return newstr;
        }

        public string padsnt(string oldstr)
        {
            if (oldstr == null)
                oldstr = "";
            string newstr = "'" + oldstr.Replace("'", "''") + "'";
            return newstr;
        }


        public string date_ddmmyyyy(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";
            string date2 = date1.Replace("/", "");
            if (date2.Length != 8)
                return "";
            else
                return date2.Substring(6, 2) + date2.Substring(4, 2) + date2.Substring(0, 4);
        }

        public string date_yyyymmdd(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";
            string date2 = date1.Replace("/", "");
            if (date2.Length != 8)
                return "";
            else
                return date2.Substring(4, 4) + date2.Substring(2, 2) + date2.Substring(0, 2);
        }


        public string date_convert(string date1)
        {
            if (string.IsNullOrWhiteSpace( date1 ))
                return "";
            string date2 = date1.Replace("/", "");
            if (date2.Length != 8)
                return "";
            else
                return date2.Substring(6, 2) + "/" + date2.Substring(4, 2) + "/" + date2.Substring(0, 4);
        }

        public string date_check(string date1)
        {
            string dd1 = ""; string mm1 = ""; string yy1 = "";
            if (string.IsNullOrWhiteSpace(date1))
                return "";
            string date2 = date1.Replace("/", "");
            if (date2 == "")
                return "";

            if (date2.Length == 4)
            {
                dd1 = date2.Substring(0, 1);
                mm1 = date2.Substring(1, 1);
                yy1 = date2.Substring(2, 2);
            }
            else if (date2.Length == 5)
            {
                dd1 = date2.Substring(0, 1);
                mm1 = date2.Substring(1, 2);
                yy1 = date2.Substring(3, 2);
            }
            else if (date2.Length == 6)
            {
                dd1 = date2.Substring(0, 2);
                mm1 = date2.Substring(2, 2);
                yy1 = date2.Substring(4, 2);
            }
            else if (date2.Length == 7)
            {
                dd1 = date2.Substring(0, 1);
                mm1 = date2.Substring(1, 2);
                yy1 = date2.Substring(3, 4);
            }
            else if (date2.Length == 8)
            {
                dd1 = date2.Substring(0, 2);
                mm1 = date2.Substring(2, 2);
                yy1 = date2.Substring(4, 4);
            }

            string date3 = dd1 + "/" + mm1 + "/" + yy1;
            DateTime date4;
            var formats = new[] { "dd/MM/yyyy", "dd/MM/yy" };
            if (DateTime.TryParseExact(date3, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date4))
            {
                dd1 = ("0" + date4.Day).Substring(date4.Day.ToString().Length - 1, 2);
                mm1 = ("0" + date4.Month).Substring(date4.Month.ToString().Length - 1, 2);
                yy1 = date4.Year.ToString();
                date3 = dd1 + "/" + mm1 + "/" + yy1;
            }
            else
                date3 = "";

            return date3;
        }

        public string period_mmyyyy(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";
            string date2 = date1.Replace("/", "");
            return date2.Substring(4, 2) + date2.Substring(0, 4);
        }

        public string period_yyyymm(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";
            string date2 = date1.Replace("/", "");
            return date2.Substring(2, 4) + date2.Substring(0, 2);
        }

        public string period_convert(string period2)
        {
            string dd1 = ""; string yy1 = "";
            string date2 = period2.Replace("/", "");
            if (date2 == "")
                return "";

            if (date2.Length == 3)
            {
                dd1 = date2.Substring(0, 1);
                yy1 = date2.Substring(1, 2);
            }
            else if (date2.Length == 4)
            {
                dd1 = date2.Substring(0, 2);
                yy1 = date2.Substring(2, 2);
            }
            else if (date2.Length == 5)
            {
                dd1 = date2.Substring(0, 1);
                yy1 = date2.Substring(1, 4);
            }
            else if (date2.Length == 6)
            {
                dd1 = date2.Substring(0, 2);
                yy1 = date2.Substring(2, 4);
            }

            if (yy1.Length == 2)
            {
                if (Convert.ToInt16(yy1) > 40)
                    yy1 = "19" + yy1;
                else
                    yy1 = "20" + yy1;
            }

            string date3 = dd1 + "/" + yy1;

            return date3;

        }

        public string period_convert2(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";
            
            if (date1.Length != 6)
                return "";
            else
                return date1.Substring(4, 2) + "/" + date1.Substring(0, 4);
        }

        public string data_ddmm(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";
            string date2 = date1.Replace("/", "");
            return date2.Substring(2, 2) + date2.Substring(0, 2);
        }

        public string data_convert(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";
            string date2 = date1.Replace("/", "");
            return date2.Substring(0, 2) + "/" + date2.Substring(2, 2);
        }

        public class cl_period
        {
            //public string processing_year { get; set; }
            //public string processing_month { get; set; }
            public string start_birth { get; set; }
            public string stop_birth { get; set; }
            public string starting_year { get; set; }
            public string stopping_year { get; set; }

        }

        public cl_period default_period()
        {
            //string str1 = "select processing_period c1 from tab_pay_default ";
            //str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            cl_period period_class = new cl_period();
            //if (str2.c1 != null)
            //{
            //    period_class.processing_year = str2.c1.Substring(0, 4);
            //    period_class.processing_month = str2.c1.Substring(4, 2);
            //}

            string str1 = "select field1 c1, field2 c2,field3 c5, field4 c6,field8 c7 from tab_train_default where default_code='DATES'";
            var str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            period_class.start_birth = str2.c1;
            period_class.stop_birth = str2.c2;
            period_class.starting_year = str2.c5;
            period_class.stopping_year = str2.c6;

            return period_class;
        }


        public class cl_report_sel
        {
            public string selection_code { get; set; }
            public string selection_range { get; set; }
            public string selection_operator { get; set; }
        }

        public cl_report_sel add_options(vw_collect mcollect)
        {
            string sel_code;
            string sel_from; string sel_to; string sel_op;
            cl_report_sel rep_sel = new cl_report_sel();
            string sp = new string(' ', 50);

            for (int ctr1 = 0; ctr1 < 5; ctr1++)
            {

                sel_code = mcollect.ar_string0[ctr1];
                sel_from = mcollect.ar_string1[ctr1];
                sel_to = mcollect.ar_string2[ctr1];
                if (sel_to == "")
                    sel_to = sel_from;
                if (string.IsNullOrWhiteSpace(sel_from))
                    sel_code = "";
                sel_op = mcollect.ar_string3[ctr1];
                if (sel_code.Length >= 5)
                {
                    if (sel_code.Substring(1, 4) == "DATE")
                    {
                        sel_from = date_yyyymmdd(sel_from);
                        sel_to = date_yyyymmdd(sel_to);
                    }
                }
                rep_sel.selection_code += (sel_code + sp).Substring(0, 10);
                rep_sel.selection_range += (sel_from + sp).Substring(0, 10);
                rep_sel.selection_range += (sel_to + sp).Substring(0, 10);
                rep_sel.selection_operator +=  (sel_op + sp).Substring(0, 3);
            }

            return rep_sel;
        }


        public bool validate_staff(string pnumber, int close_flag = 0)
        {
            bool err_flag = false;
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            string str1 = "select  staff_number c1,close_code c2 from tab_staff a where staff_number=" + pads(pnumber) + pblock.p2000;
            var str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str2 != null)
            {
                err_flag = true;
                if (close_flag == 0 && str2.c2 != "0")
                    err_flag = false;
            }
            return err_flag;

        }

        public decimal get_limit_balance(string limit_staff, string limit_code, string limit_year, int topt=0)
        {
            string str1; string str3;
            decimal limit_amt; decimal bal_amt;
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];

            //'check the year limit record id created
            str3 = "select limit_amount n1, limit_amount+carry_over-balance n2 from tab_balance where para_code='STAFF' ";
            str3 += " and staff_number=" + pads(limit_staff) + " and limit_code=" + pads(limit_code) + " and limit_year=" + pads(limit_year);
            var str2 = db.Database.SqlQuery<vw_query>(str3).FirstOrDefault();
            if (str2 == null)
            {
                str1 = "exec recompute_limits @limit_code=" + pads(limit_code) + ",@all_flag='S', @link_table=" + pads(limit_staff) + ", @p_userid=" + pads(pblock.userid);
                db.Database.ExecuteSqlCommand(str1);
                str3 = "select limit_amount n1, limit_amount+carry_over-balance n2 from tab_balance where para_code='STAFF' ";
                str3+=" and staff_number=" + pads(limit_staff) + " and limit_code=" + pads(limit_code) + " and limit_year=" + pads(limit_year);
                str2 = db.Database.SqlQuery<vw_query>(str3).FirstOrDefault();

            }

            if (str2 == null) return 0;
            limit_amt = str2.n1;
            bal_amt = str2.n2;
            //HttpContext.Current.Session["dyear_out"] = bal_amt.ToString("#,###");

            if (topt == 0)
                return bal_amt;
            else
                return limit_amt;
        }


        public string duration_calc(string sdate, string edate, int duration, string ctype="")
        {
            string str1;
            string sdate1; string edate1;
            DateTime sdate11;
            double days1;

            sdate1 = date_yyyymmdd(date_check(sdate));
            edate1 = date_yyyymmdd(date_check(edate));
            //if (sdate1 == "" || edate1 == "")
            //    return "";

            if ((sdate1 != "" && edate1 != "" && duration == 0) || (sdate1 != "" && edate1 != "" && duration != 0 && ctype == "E"))
            {
                // duration
                DateTime sdate2 = yyyymmdd_datetime(sdate1);
                DateTime sdate3 = yyyymmdd_datetime(edate1);

                days1 = sdate3.Subtract(sdate2).TotalDays;

                str1 = "select count(0) t1 from tab_holiday where date_hol between '" + sdate1 + "' and '" + edate1 + "' ";
                str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                days1 = days1 - str2.t1;
                return Convert.ToString(Convert.ToInt16(days1));
            }
            else if ((sdate1 != ""  && duration != 0 && edate1 == "") || 
                    (sdate1 != "" && duration != 0 && edate1 != "" && (ctype == "D"||ctype=="S")))
            {
                // end date
                DateTime sdate3 = yyyymmdd_datetime(sdate1);
                int dura1 = duration;
                int holdays = 0;

                while (true)
                {
                    sdate11 = sdate3.AddDays(dura1);
                    edate1 = datetime_yyyymmdd(sdate11);
                    str1 = "select count(0) t1 from tab_holiday where date_hol between " + pads(sdate1) + " and " + pads(edate1);
                    str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                    if (str2.t1 == holdays)
                        break;
                    dura1 = duration + str2.t1;
                    holdays = str2.t1;
                }

                return date_convert(edate1);
            }
            else if (sdate1 == "" && edate1 != "" && duration != 0)
            {
                //start date
                DateTime sdate3 = yyyymmdd_datetime(edate1);
                int dura1 = duration;
                int holdays = 0;

                while (true)
                {
                    sdate11 = sdate3.AddDays(0 - dura1);
                    sdate1 = datetime_yyyymmdd(sdate11);
                    str1 = "select count(0) t1 from tab_holiday where date_hol between " + pads(sdate1) + " and " + pads(edate1);
                    str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                    if (str2.t1 == holdays)
                        break;
                    dura1 = duration + str2.t1;
                    holdays = str2.t1;
                }

                return date_convert(sdate1);
            }


            return "";
        }

        public string datetime_yyyymmdd(DateTime sdate1)
        {
            int year1 = sdate1.Year;
            int month1 = sdate1.Month;
            int day1 = sdate1.Day;

            string month2 = "0" + Convert.ToString(month1);
            string day2 = "0" + Convert.ToString(day1);
            month2 = month2.Substring(month2.Length - 2, 2);
            day2 = day2.Substring(day2.Length - 2, 2);
            string year2 = Convert.ToString(year1);
            return year2 + month2 + day2;

        }

        public DateTime yyyymmdd_datetime(string sdate1)
        {
            if (string.IsNullOrWhiteSpace(sdate1))
                return new DateTime(1900, 01, 01);

            int year1 = Convert.ToInt16(sdate1.Substring(0, 4));
            int month1 = Convert.ToInt16(sdate1.Substring(4, 2));
            int day1 = Convert.ToInt16(sdate1.Substring(6, 2));

            DateTime d1 = new DateTime(year1, month1, day1);
            return d1;

        }
        public bool transaction_check(string ttype, string snumber, string vdate)
        {
            var dup1 = (from dp1 in db.tab_transaction
                        where dp1.staff_number == snumber && dp1.trans_type == ttype && dp1.value_date == vdate
                        select dp1).FirstOrDefault();

            bool staff_flag = false;
            if (dup1 != null)
                staff_flag = true;

            return staff_flag;

        }

        public int check_staff_approval(string trans_type, string mode_type, string snumber)
        {

            string str1; int err_flag;

            err_flag = 0;
            var slist = (from df1 in db.tab_type
                         where df1.parameter_close == "Y" && df1.trans_type == trans_type
                         select df1).FirstOrDefault();

            if (slist != null)
                err_flag = 1;


            if (mode_type == "S")
            {
                var slist1 = (from df2 in db.tab_array
                              where df2.para_code == "SECE" && df2.array_code == trans_type
                              select df2).FirstOrDefault();
                if (slist1 != null)
                    err_flag = 1;

                if (err_flag == 0)
                {
                    str1 = " select 1 from tab_staff e , tab_self_approval b where b.para_code in ('SELF','CASH') and e.staff_number=" + pads(snumber);
                    str1 = str1 + " and ((b.category_apv='STAFF' and e.staff_number between b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='01'  and e.category between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='47'  and e.cost_centre between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='JOBP'  and e.job_post between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN0'  and e.trans0 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN1'  and e.trans1 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN2'  and e.trans2 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN3'  and e.trans3 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN4'  and e.trans4 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN5'  and e.trans5 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN6'  and e.trans6 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN7'  and e.trans7 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN8'  and e.trans8 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='TRAN9'  and e.trans9 between  b.approval_category and b.approval_category_to) or ";
                    str1 = str1 + " (b.category_apv='COMPANY')) and transaction_apv=" + pads(trans_type);

                    str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                    if (str2 == null)
                        err_flag = 2;
                }
            }
            return err_flag;
        }

        public Boolean validate_grouptrans(string sn1, string grp1)
        {
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            string str1 = "select count(0) t1 from vw_grouptrans where gcode=" + pads(sn1) + " and gpara=" + pads(grp1);
            if (grp1 == "STAFF")
                str1 = "select count(0) t1 from tab_staff a where close_code='0'and staff_number=" + pads(sn1) + " " + pblock.p2000;

            var plist = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (plist.t1 == 0)
                return false;
            else
                return true;

        }

        public string check_payment(string lnumber, string ldate)
        {
            string str1; string vyear;

            str1 = "select field6 c0 from tab_train_default where default_code='LEAVE' ";
            var rec1 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            vyear = rec1.c0;

            str1 = "select line2 c0 from tab_transaction where trans_type='VAC' and line10='Y' and cast(line7 as numeric(15,5)) <> 0 and staff_number=" + pads(lnumber) + " and line2 <= " + pads(vyear);
            str1 = str1 + " and value_date <> '" + ldate + "' order by 1 ";
            rec1 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (rec1 == null)
                return "1000";
            else
                return rec1.c0;

        }

        public void update99_rtn(string stype, string snumber, string svdate, string userid)
        {
            string str1;
            
            str1 = " exec update_trans @stype=" + pads(stype) + ", @snumber=" + pads(snumber);
            str1 += ", @svdate=" + pads(svdate) + ", @p_userid=" + pads(userid);
            db.Database.ExecuteSqlCommand(str1);

            Cmail mailit = new Cmail();
            mailit.document_mail(userid);
            
        }


        public string getname(string opcode, string opvalue, int optype)
        {
            string str1 = "";

            if (optype == 1)
                str1 = "select c1 = analy0 from tab_analy where analy_code = " + pads(opcode);
            else if (optype == 2)
                str1 = "select c1 = report_name1 from tab_soft where para_code = " + pads(opcode) + " and report_code = " + pads(opvalue);
            else if (optype == 3)
                str1 = "select c1 = gname from vw_grouptrans where gpara = " + pads(opcode) + " and gcode = " + pads(opvalue);
            else if (optype == 4)
                str1 = "select c1 = name1 from tab_defined where type_code = 'PR' and para_code = " + pads(opcode) + " and trans_code = " + pads(opvalue);
            else if (optype == 5)
                str1 = "select c1 = staff_name from tab_staff where staff_number = " + pads(opvalue) + " and " + pads(opcode) + " in ('NS','STAFF') ";
            else
                str1 = "select c1 = pname from vw_user_parameter where hcode = " + pads(opcode) + " and pcode = " + pads(opvalue);
            var qlist = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (qlist == null)
                return "";
            else
                return qlist.c1;

        }

        //mark for delete
        public IEnumerable<tab_type2> read_type(string ttype, decimal app_flag)
        {

            string str1 = "select * from tab_type2 where trans_type=" + pads(ttype) + " and visible_line <=" + app_flag.ToString() + "  and sequence_no <> 0 ";
            str1 = str1 + " and data_line =" + app_flag.ToString() + " order by sequence_no ";
            var tlist1 = db.Database.SqlQuery<tab_type2>(str1);

            return tlist1;
        }

        public string check_valid_query(string query_code,string snumber="")
        {
            string str1;
            Boolean err_flag = false;
            string tempstr = "";

            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            if (query_code.Trim() != "")
            {
                str1 = "select 1 from tab_soft where para_code in ('HCODE','USERV') and report_code=" + pads(query_code);
                str1 = str1 + " union all select 1 from tab_train where para_code='HDEF' and train_code=" + pads(query_code);
                var str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                if (str2 == null)
                    err_flag = false;
                else
                    err_flag = true;
            }

            if (err_flag)
            {
                if (query_code == "HSDEP")
                    str1 = "select dependant c1, dependant c2 from tab_depend where staff_number=" + pads(snumber);
                else if (query_code == "ALLOW")
                    str1 = "select allow_code c1, c2=case '" + pblock.cquery + "' when 'Y' then allow_code + ' : ' + name1 else name1 end  from tab_allow where allowance_code='A'  ";
                else if (query_code == "DEDUCT")
                    str1 = "select allow_code c1, c2=case '" + pblock.cquery + "' when 'Y' then allow_code + ' : ' + name1 else name1 end  from tab_allow where allowance_code='D'  ";
                else if (query_code == "DAILY")
                    str1 = "select daily_code c1, c2=case '" + pblock.cquery + "' when 'Y' then daily_code + ' : ' + name1 else name1 end  from tab_daily where code_ind='A'  ";
                else if (query_code == "DDAILY")
                    str1 = "select daily_code c1, c2=case '" + pblock.cquery + "' when 'Y' then daily_code + ' : ' + name1 else name1 end  from tab_daily where code_ind='D'  ";
                else if (query_code == "TGRP")
                    str1 = "select train_group c1, group_name c2 from tab_train_group    ";
                else if (query_code == "NS")
                    str1 = "select staff_number c1, c2=case '" + pblock.cquery + "' when 'Y' then staff_number + ' : ' + staff_name else staff_name end from tab_staff where close_code='0' ";
                else if (query_code == "APPROVAL")
                    str1 = "select train_code c1,sname c2 from vw_approval ";
                else if (query_code == "HDEPTYPE")
                    str1 = "select report_code c1, report_name1 c2 from tab_soft where para_code='DEPTYPE'  ";
                else
                {
                    str1 = "select pcode c1, c2=case '" + pblock.cquery + "' when 'Y' then pcode + ' : ' + pname else pname end   from vw_user_parameter where hcode=" + pads(query_code);
                    str1 += " union all select report_code c1, report_name1 c2 from tab_soft where para_code=" + pads(query_code);
                }

                str1 = str1 + " order by 2 ";
                tempstr = str1;
            }
            return tempstr;
        }


        public string check_1a(string tcode, string tgrid="")
        {
            string str3 = ""; int cltr = 0;
            str30 = "";

            IQueryable<tab_type2> blist1;
            var blist = from bg in db.tab_type2
                        where bg.trans_type == tcode && bg.sequence_no != 0
                        orderby bg.sequence_no
                        select bg;

            if (tgrid == "Y")
                blist1 = blist.Where(m => m.grid_display == "Y");
            else
                blist1 = blist;

            foreach (var item in blist.ToList())
            {
                str3 += build_string(item.count_type, item.text_type, item.text_header, item.text_code, item.sequence_no);
                cltr++;
            }

            //HttpContext.Current.Session["h2"] = cltr;
            worksess worksess = (worksess) HttpContext.Current.Session["worksess"];
            worksess.intval0 = cltr;
            HttpContext.Current.Session["worksess"] = worksess;

            //' read transactions
            string str1 = "select a.staff_number as snumber, cast(processed as varchar(05)) as column24, dbo.date_out(value_date) as column27, value_date column25,";
            str1 += " b.staff_name column21, a.staff_number + ' - ' + b.staff_name as column26 ";
            str1 += str3 + " from tab_transaction a " + str30 + " , tab_staff b where a.staff_number=b.staff_number and trans_type =" + pads(tcode);

            return str1;
        }

        private string build_string(int countx, string typex, string headx, string tcode, int seq)
        {
            string str2; string w_prefix;string str21;
            string sln = countx.ToString().Trim();
            string seqt = seq.ToString().Trim();

            if (typex == "D" || typex == "C")
                str2 = ",dbo.date_out(line" + sln + ") as column" + seqt;
            else if (typex == "P")
                str2 = ",dbo.period_out(line" + sln + ") as column" + seqt;
            else if (typex == "N")
                str2 = ",line" + sln + " as column" + seqt;
            else if (typex == "A" || typex == "M")
            {
                str21 = "line" + sln ;
                str2 = ",case isnumeric(" + str21 + ") when 1 then convert(varchar(30),cast(" + str21 + " as money),1) else '0' end as column" + seqt;
            }
            else
                if (tcode != "" && (typex == "L" || typex == "K" || typex == "R" || typex == "G"))
                {
                    check_for_name(tcode);
                    if (!string.IsNullOrEmpty(str51))
                    {
                        w_prefix = "k" + sln;
                        str2 = ",isnull(" + str51.Replace("xx", w_prefix) + ",'') as column" + seqt;
                        str30 = str30 + str31.Replace("xx", w_prefix) + "=line" + sln + ") ";
                    }
                    else
                        str2 = ",line" + sln + " as column" + seqt;
                }
                else
                    str2 = ",line" + sln + " as column" + seqt;

            return str2;

        }

        private void check_for_name(string tcode)
        {
            if (tcode != "")
            {
                var blist = from d1 in db.tab_soft
                            where (d1.para_code == "HCODE" || d1.para_code == "HCODET") && d1.report_code == tcode
                            select new { a1 = d1.report_name2, a2 = d1.report_name3, a3 = d1.report_name4, a4 = d1.report_name5 };

                var blist2 = from d2 in db.tab_train
                             where d2.para_code == "HDEF" && d2.train_code == tcode
                             select new { a1 = "xx.type_code='DF' and xx.para_code='" + tcode + "' and trans_code", a2 = "tab_defined xx ", a3 = "xx.name1", a4 = "left outer join tab_defined xx on ( xx.type_code='DF' and xx.para_code='" + tcode + "' and xx.trans_code" };

                var blist3 = blist.Union(blist2).FirstOrDefault();
                if (blist3 != null)
                {
                    if (blist3.a1 != "") str31 = " and " + blist3.a1;
                    if (blist3.a2 != "") str41 = "," + blist3.a2;
                    if (blist3.a3 != "") str51 = blist3.a3;
                    if (blist3.a4 != "") str31 = blist3.a4;
                }

            }
        }

        public string write_file(string report_type, string transfer_code, string puserid)
        {
            int ws_count = 0;
            string ws_text = ""; string comma_flag = "";
            string ws_temp; string sp2 = "";
            string filename;

            //    ' read codes in layout into array
            var clist = from bg in db.tab_layout
                        where bg.para_code == report_type && bg.layout_code == transfer_code
                        orderby bg.sub_category
                        select bg;

            sp2.PadRight(50);
            foreach (var item in clist.ToList())
            {
                ws_count++;
                ws_temp = "left(column" + ws_count.ToString() + " + '" + sp2 + "'," + item.length_pos + ")";
                ws_text = ws_text + comma_flag + ws_temp;
                comma_flag = " + ";
            }


            string str2 = "select " + ws_text + " c1 from [" + puserid + "transf] ";
            var str3 = db.Database.SqlQuery<vw_query>(str2);

            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/uploads"));
            filename = dirInfo.FullName + @"\" + puserid + "transfer";
            if (System.IO.File.Exists(filename))
                System.IO.File.Delete(filename);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                foreach (var item in str3.ToList())
                {
                    file.WriteLine(item.c1);
                }
            }

            return filename;
        }

        public void write_plog(string table_code, string sel_str, string sel_action, string sel_seq, string puserid, string sel_pcode = "", string sel_keycode = "")
        {
            string hostname = GetWorkstation();
            string str1 = "execute write_plog @table_code=" + pads(table_code) + ", @sel_str=" + pads(sel_str) + ", @sel_action=" + pads(sel_action) + ", @sel_seq=" + pads(sel_seq);
            str1 += ",@hostname=" + pads(hostname) + ",@p_userid=" + pads(puserid) + ",@sel_pcode=" + pads(sel_pcode) + ",@sel_keycode=" + pads(sel_keycode);

            try
            {
                db.Database.ExecuteSqlCommand(str1);
            }

            catch (Exception err)
            {
                str1 = "insert into error_table(msg,progname) values (" + pads(err.Message) + ",'log '+" + pads(table_code) + ")";
                db.Database.ExecuteSqlCommand(str1);
            }


        }

        private string GetWorkstation()
        {
            string ip = HttpContext.Current.Request.UserHostName;
            IPAddress myIP = IPAddress.Parse(ip);
            try
            {
                IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
                List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
                return compName.First();
            }
            catch
            {
                return "";
            }
        }

        public void error_update(string emess)
        {
            string str1 = "insert into error_table(msg,progname) values(" + pads(emess.Substring(0, 5000)) + ", '')";
            db.Database.ExecuteSqlCommand(str1);

        }

        public int check_option()
        {
            string str1;
            int p1 = 0;
            string sp = new string(' ', 50);
            bool user_flag=true;

            if (HttpContext.Current.Session["mysessvals"] != null)
                user_flag = false;

            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            if (pblock.type2 != "S" && user_flag)
            {
                string valid_option = pblock.cquery2;
                string vreq = HttpContext.Current.Request.Url.PathAndQuery + sp;
                int ctr = vreq.IndexOf("/", 1);
                string rep8 = vreq.Substring(1, ctr - 1);

                str1 = " select count(0) t1 from tab_array fd, (";
                str1 += " select report_code c1, 'ITEM' c4, report_name8 c8 from tab_soft where para_code in ('ITEM','SELFT','SELFTQ') union all ";
                str1 += " select calc_code , a.para_code, calc_code from tab_calc a, tab_soft b where b.para_code='UPER' and b.report_code=a.para_code union all ";
                str1 += " select trans_type , 'TYPE', trans_type from tab_type a, tab_array b where a.internal_use <> 'Y' and a.trans_type = b.array_code and b.para_code in ('SELE')  ";
                str1 += ") xd  where fd.para_code='21' and fd.array_code=" + pads(pblock.ugroup) + " and fd.operand= xd.c1 and fd.source1=xd.c4 and xd.c8="+pads(rep8);
                var str21 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                if (((valid_option=="" || valid_option=="V") && str21.t1==0) || (valid_option=="I" && str21.t1>0))
//                if (str21 == null)
                    p1 = 1;

            }

            return p1;

        }

        public IEnumerable<vw_query> listdaily(string ID)
        {
            anchor1Context db2 = new anchor1Context();
            string qstr1 = "";
            string qstr2 = "";
            string qstr3 = "";
            string flag_switch = "";

            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            worksess worksess = (worksess)HttpContext.Current.Session["worksess"];
            string query_flag = worksess.jp;
            string str14 = "select 1 c1, 1 c2 from tab_soft where para_code='XXJJZZ' ";
            Boolean order_flag = false;
            string Code = ID;

            var str2 = db.Database.SqlQuery<vw_query>(str14);

            if (query_flag == "1")
            {
                var st1 = (from s in db2.tab_soft
                           where s.para_code == "QUERY" && s.report_code == Code
                           select s).FirstOrDefault();

                //HttpContext.Current.Session["list0"] = ID;
                worksess.temp0 = ID;
            
                qstr1 = st1.report_name2;
                qstr2 = st1.report_name3;
                qstr3 = st1.report_name4;
                if (st1.rep_name1 == "Y")
                {
                    //qstr2 += " " + HttpContext.Current.Session["report_name3"].ToString();
                    qstr2 += " " + worksess.temp4;
                    qstr2 += " order by report_name7 ";
                    order_flag = true;
                }
                else if (st1.rep_name1 == "X")
                {
                    qstr1 = st1.report_name2 + " and user_code=" + pads(pblock.userid) + ") order by 2 ";
                    order_flag = true;
                }

                if (worksess.flag_type == null)
                    flag_switch = "";
                else
                    flag_switch = worksess.flag_type;

                if (flag_switch == "S")
                    qstr3 = "";
                if (flag_switch == "D")
                {
                    qstr2 = "";
                    qstr3 = "";
                }

                if (Code == "HRTRANS")
                {
//                    flag_switch = HttpContext.Current.Session["ttype"].ToString();
                    flag_switch = worksess.temp5;
                    string flag1 = ""; string flag2 = "";
                    qstr1 = "select '' c1, ''c2, 0 ";
                    if (flag_switch.Length > 2)
                    {
                        flag1 = flag_switch.Substring(1);
                        flag2 = flag_switch.Substring(0, 1);

                        if (flag2 == "x")
                        {
                            qstr1 = st1.report_name2 + pads(flag1) + " union all ";
                            qstr1 += st1.report_name3 + pads(flag1) + " union all ";
                            qstr1 += st1.report_name4 + pads(flag1) + " union all ";
                            qstr1 += st1.report_name5;
                        }
                        else
                        {
                            var st2 = (from s in db2.tab_soft
                                       where s.para_code == "QUERY" && s.report_code == "HTRANSEXP"
                                       select s).FirstOrDefault();

                            qstr1 = st2.report_name2 + pads(flag1) + " union all ";
                            qstr1 += st2.report_name3 + pads(flag1) + " union all ";
                            qstr1 += st2.report_name4 + pads(flag1) + " union all ";
                            qstr1 += st2.rep_name1 + pads(flag1) + ") union all ";
                            qstr1 += st2.report_name5;

                        }
                        qstr1 += " order by 3,2 ";
                    }
                    qstr2 = "";
                    qstr3 = "";
                    order_flag = true;
                }

                if (flag_switch == "H")
                    qstr3 = st1.report_name5;

                worksess.temp2= qstr2;
                worksess.temp3= qstr3;

                query_flag = "2";
                if (!order_flag)
                    qstr1 += " order by 2 ";

                str2 = db.Database.SqlQuery<vw_query>(qstr1);
            }
            else if (query_flag == "2")
            {
                qstr2 = worksess.temp2;
                if (!string.IsNullOrWhiteSpace(qstr2))
                    str2 = db.Database.SqlQuery<vw_query>(qstr2);
                else
                    str2 = db.Database.SqlQuery<vw_query>(str14);
                query_flag = "3";
            }

            else if (query_flag == "3")
            {
                qstr3 = worksess.temp3;
                if (!string.IsNullOrWhiteSpace(qstr3))
                    str2 = db.Database.SqlQuery<vw_query>(qstr3);
                else
                    str2 = db.Database.SqlQuery<vw_query>(str14);

                query_flag = "1";
            }

            worksess.jp = query_flag;
            HttpContext.Current.Session["worksess"] = worksess;

            return str2;
        }

        public IEnumerable<vw_query> listdaily2(string ID)
        {
            // only for hr calculation and appraisal and when selection is transaction type and performance pages
            worksess worksess = (worksess)HttpContext.Current.Session["worksess"];
            string ws_op = worksess.temp4;
            string type_code2 = worksess.pc;
            string str1 = "";
            string Code = ID;

            if (type_code2 == "H20" || type_code2 == "HA20" || type_code2 == "H13" || type_code2 == "H10" || type_code2 == "HA03" || type_code2 == "H21" || type_code2 == "HA07" || type_code2 == "R01" || type_code2 == "R04" || type_code2 == "F13" || type_code2 == "H47")
            {
                if (ws_op == "TYPE" || ws_op == "TYPEC" || ws_op == "PFPAGES" || ws_op == "PFPAGECAL" || ws_op == "APPLYTYPE" || ws_op == "EXPEND" || ws_op == "INCID")
                {
                    if (ws_op == "TYPE")
                    {
                        var st1 = (from s in db.tab_soft
                                   where s.para_code == "QUERY" && s.report_code == "HRTRANS"
                                   select s).First();

                        str1 = st1.report_name2 + pads(Code) + " union all " + st1.report_name3 + pads(Code) + " union all ";
                        str1 += st1.report_name4 + pads(Code) + " union all " + st1.report_name5;
                        str1 += " order by 3 ";
                    }
                    else if (ws_op == "TYPEC")
                    {
                        var st1 = (from s in db.tab_soft
                                   where s.para_code == "QUERY" && s.report_code == "HRTRANSC"
                                   select s).First();

                        str1 = st1.report_name2 + pads(Code) + " union all " + st1.report_name3 + pads(Code) + " order by 3 ";
                    }
                    else if (ws_op == "APPLYTYPE")
                    {
                        var st1 = (from s in db.tab_soft
                                   where s.para_code == "QUERY" && s.report_code == "APPLYTRAN"
                                   select s).First();

                        str1 = st1.report_name2 + pads(Code) + " union all " + st1.report_name3 + pads(Code) + " order by 3 ";
                    }
                    else if (ws_op == "PFPAGES")
                    {
                        var st1 = (from s in db.tab_soft
                                   where s.para_code == "QUERY" && s.report_code == "PFDETAIL"
                                   select s).First();

                        if (Code == "RELAPAGE")
                            str1 = st1.rep_name2 + " order by 4, 1 ";
                        else
                        {
                            str1 = st1.report_name2 + pads(Code) + " union all " + st1.report_name3 + pads(Code) + " union all ";
                            str1 += st1.report_name4 + pads(Code) + " union all " + st1.report_name5 + pads(Code) + " union all ";
                            str1 += st1.rep_name1 + pads(Code) + " order by 4, 1 ";
                        }
                    }
                    else if (ws_op == "PFPAGECAL")
                    {
                        var st1 = (from s in db.tab_soft
                                   where s.para_code == "QUERY" && s.report_code == "PFDETNUM"
                                   select s).First();

                        if (Code == "RELAPAGE")
                            str1 = st1.rep_name2 + " order by 4, 1 ";
                        else
                        {
                            str1 = st1.report_name2 + pads(Code) + " union all " + st1.report_name3 + pads(Code) + " union all ";
                            str1 += st1.report_name4 + pads(Code) + " union all " + st1.report_name5 + pads(Code) + " union all ";
                            str1 += st1.rep_name1 + pads(Code) + " order by 4, 1 ";
                        }
                    }
                    else if (ws_op == "EXPEND")
                    {
                        var st1 = (from s in db.tab_soft
                                   where s.para_code == "QUERY" && s.report_code == "EXPENDET"
                                   select s).First();

                        str1 = st1.report_name2 + pads(Code) + " union all " + st1.report_name3 + pads(Code) + " union all ";
                        str1 += st1.report_name4 + pads(Code) + " union all " + st1.report_name5;
                        str1 += " order by 3,2 ";
                    }

                    else if (ws_op == "INCID")
                    {
                        var st1 = (from s in db.tab_soft
                                   where s.para_code == "QUERY" && s.report_code == "INCIDET"
                                   select s).First();

                        str1 = st1.report_name2 + pads(Code) + " union all " + st1.report_name3 + pads(Code) + " union all ";
                        str1 += st1.report_name4 + pads(Code) + " union all " + st1.report_name5;
                        str1 += " order by 3,2 ";
                    }

                }

                if (str1 == "")
                    str1 = worksess.temp2;

                if (str1 == "")
                    str1 = "select 1 c1 from tab_soft where para_code='xxyyaa'";

                var str2 = db.Database.SqlQuery<vw_query>(str1);
                return str2;
            }

            return db.Database.SqlQuery<vw_query>("select 1 c1 from tab_soft where para_code='xxyyaa'");
        }

        public IEnumerable<vw_query> listdaily3(string ID)
        {
            string id1 = ID.Substring(0, 1);
            string id2 = ID.Substring(1);
            string str1 = "select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name5='Y' union all ";
            str1 += "select analy_code as c1, analy0 as c2, analy1 as c31 from tab_analy where para_code in ('ANALY') union all ";
            str1 += "select '','','' union all ";
            if (id1 == "x")
                str1 += "select 'LINE' + ltrim(str(count_type)) as c1, rtrim(text_line) + '  T' as c2, '' from tab_type2, vw_hrcode where trans_type=" + pads(id2) + "  and text_code=vcode ";
            else
                str1 += "select operand,source1,'' from tab_array where para_code='H46' and array_code= " + pads(id2);

            str1 += "  order by 2";
            var str2 = db.Database.SqlQuery<vw_query>(str1);

            return str2;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}

        public DateTime logdatetime()
        {
            return logdatetime("0", DateTime.Now);
        }

        public DateTime logdatetime(string zoneflag,DateTime utdate)
        {
            string str1;
            string timezone;

            if (zoneflag == "0")
                return DateTime.UtcNow;

            //str1 = "select field12 c0 from tab_train_default where default_code='DATES'";
            //var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            timezone = zoneflag;

            if (!string.IsNullOrWhiteSpace(timezone))
            {
                //DateTime serverDateTime = DateTime.Now;
                //DateTime dbDateTime = serverDateTime.ToUniversalTime();

                //get date time offset for UTC date stored in the database
                //DateTimeOffset dbDateTimeOffset = new
                //                DateTimeOffset(dbDateTime, TimeSpan.Zero);

                //get user's time zone from profile stored in the database
                TimeZoneInfo userTimeZone =
                             TimeZoneInfo.FindSystemTimeZoneById(timezone);

                DateTime mydatetime = TimeZoneInfo.ConvertTimeFromUtc(utdate, userTimeZone);

                //convert  db offset to user offset
                //DateTimeOffset userDateTimeOffset =
                //           TimeZoneInfo.ConvertTime
                //          (dbDateTimeOffset, userTimeZone);


                return mydatetime;
            }
            else
                return utdate;

        }

        public string M200(string userid)
        {
            string str1 = "select dbo.P2000(" + pads(userid) + ") c1 ";
            var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            return str11.c1;
        }

        public void pub_update(string pvalue, string pname)
        {
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            string str1 = "delete from pub_table where userid=" + pads(pblock.userid) + " and name1=" + pads(pname);
            db.Database.ExecuteSqlCommand(str1);
            if (pvalue != "")
            {
                str1 = "insert into pub_table(userid,name1,cvalue) values(" + pads(pblock.userid) + "," + pads(pname) + "," + pads(pvalue) + ")";
                db.Database.ExecuteSqlCommand(str1);
            }
        }

        public string formatNumbers(string num, int decpl)
        {
            char a1='#';
            string fm = "#,###";
            if (decpl > 0)
            {
                fm += ".";
                string decstr=new string(a1,decpl);
                fm += decstr;
            }

            return Convert.ToDouble(num).ToString(fm);
        }

        public List<SelectListItem> year_list()
        {
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            List<SelectListItem> pyears = new List<SelectListItem>();
            SelectListItem pyvar;
            int cperiod = Convert.ToInt16(pblock.processing_period.Substring(0, 4));
            int start_year = cperiod - 10;   // Convert.ToInt16(utils.default_period().starting_year);
            int stop_year = cperiod + 10;    // Convert.ToInt16(utils.default_period().stopping_year);

            for (int ctr = start_year; ctr <= stop_year; ctr++)
            {
                pyvar = new SelectListItem()
                {
                    Value = Convert.ToString(ctr),
                    Text = Convert.ToString(ctr)
                };
                pyears.Add(pyvar);
            }

            return pyears;
        }

        public string staff_payment_cycle(string astaff)
        {
            string str1;

            if (string.IsNullOrWhiteSpace(astaff))
                str1 = "select transfer_code c0 from tab_calc where para_code='UPDATE' and calc_code=(select default_pay_cycle from tab_default)";
            else
                str1=" select payment_cycle c0 from tab_grade where para_code='01' and grade_code=(select category from tab_staff where staff_number=" + pads(astaff) + ")";

            var bgpay = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (bgpay == null)
            {
                str1 = "select transfer_code c0 from tab_calc where para_code='UPDATE' and calc_code=(select default_pay_cycle from tab_default)";
                bgpay = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            }

            return bgpay.c0;

        }

        public string zeroprefix(string value1, int lenoutput)
        {
            value1 = value1 == null ? "" : value1;
            string newv = "0000000000" + value1;
            string outv = newv.Substring(newv.Length - lenoutput);
            return outv;
        }


        public void pictupdate(HttpPostedFileBase[] upfiles,string hcode, string pcode, string comcode,string[] array1)
        {
            foreach (HttpPostedFileBase filep in upfiles)
            {
                if (filep != null && filep.ContentLength != 0)
                {
                    byte[] uploaded = new byte[filep.InputStream.Length];
                    filep.InputStream.Read(uploaded, 0, uploaded.Length);

                    tab_docpara tab_docpara = new tab_docpara();
                    tab_docpara.hcode = hcode;
                    tab_docpara.picture1 = uploaded;
                    tab_docpara.pcode = pcode;
                    tab_docpara.document_type = filep.ContentType;
                    tab_docpara.document_name = Path.GetFileName(filep.FileName);
                    tab_docpara.comment_code = comcode;

                    db.tab_docpara.Add(tab_docpara);

                    try
                    {
                        db.SaveChanges();
                    }

                    catch (Exception err)
                    {
                        save_message("Error on attachment files, but document processed : " + err.InnerException.InnerException.Message);
                        //ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);
                        //err_flag = false;
                    }

                }
            }

            if (array1 != null)
            {
                string arlist = string.Join(",", array1);
                arlist = arlist.Replace(",,", ",0,");
                string laststr = "0";
                if (arlist.Length > 1)
                    laststr = arlist.Substring(arlist.Length - 1);

                if (laststr == ",")
                    arlist += "0";

                if (arlist.Length > 0)
                {
                    string str1 = "delete from tab_docpara where sequence_no in (" + arlist + ")";
                    db.Database.ExecuteSqlCommand(str1);
                }
            }

        }

        public void picttrans(HttpPostedFileBase[] upfiles, string snumber, string transtype,string vdate, string comcode, string[] array1)
        {
            foreach (HttpPostedFileBase filep in upfiles)
            {
                if (filep != null && filep.ContentLength != 0)
                {
                    byte[] uploaded = new byte[filep.InputStream.Length];
                    filep.InputStream.Read(uploaded, 0, uploaded.Length);

                    tab_doctrans tab_doctrans = new tab_doctrans();
                    tab_doctrans.staff_number = snumber;
                    tab_doctrans.picture1 = uploaded;
                    tab_doctrans.trans_type = transtype;
                    tab_doctrans.document_type = filep.ContentType;
                    tab_doctrans.value_date = vdate;
                    tab_doctrans.document_name = Path.GetFileName(filep.FileName);
                    tab_doctrans.comment_code = comcode;

                    db.tab_doctrans.Add(tab_doctrans);

                    try
                    {
                        db.SaveChanges();
                    }

                    catch (Exception err)
                    {
                        save_message("Error on attachment files, but document processed : " + err.InnerException.InnerException.Message);
                        //ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);
                        //err_flag = false;
                    }

                }
            }

            if (array1 != null)
            {
                string arlist = string.Join(",", array1);
                arlist = arlist.Replace(",,", ",0,");
                string laststr = "0";
                if (arlist.Length > 1)
                    laststr = arlist.Substring(arlist.Length - 1);

                if (laststr == ",")
                    arlist += "0";

                if (arlist.Length > 0)
                {
                    string str1 = "delete from tab_doctrans where sequence_no in (" + arlist + ")";
                    db.Database.ExecuteSqlCommand(str1);
                }
            }

        }

        public IEnumerable<vw_picture> read_attachmentp(string hcode,string pcode, string ccode)
        {
            var bf = from bh in db.tab_docpara
                     where bh.hcode == hcode && bh.pcode == pcode && bh.comment_code == ccode
                     orderby bh.sequence_no
                     select bh;

            //return bf.ToList();

            string str1 = "select * from tab_docpara a left outer join tab_soft b on ( b.para_code = 'IMGDEF' and document_type like '%' + report_code) ";
            str1 += " where a.hcode=" + pads(hcode) + " and a.pcode=" + pads(pcode) + " and a.comment_code=" + pads(ccode);
            str1 += " order by 1 ";
            var bg2 = db.Database.SqlQuery<vw_picture>(str1);

            return bg2.ToList();

        }

        public void document_image_write(HttpPostedFileBase[] upfiles, string snumber, string [] document_code, string [] document_name, string[] array1)
        {
            DateTime odate = logdatetime();
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            int ptr = 0;

            foreach (HttpPostedFileBase filep in upfiles)
            {
                if (filep == null)
                    continue;

                tab_photo tab_photo = db.tab_photo.Find(snumber, "I", document_code[ptr]);
                bool ph_flag = false;

                if (tab_photo == null)
                {
                    tab_photo = new tab_photo();
                    tab_photo.date_approve = odate;
                    tab_photo.approval_user = pblock.userid;
                    tab_photo.internal_use = "N";
                    ph_flag = true;
                }


                byte[] uploaded = new byte[filep.InputStream.Length];
                filep.InputStream.Read(uploaded, 0, uploaded.Length);

                tab_photo.document_access = "00";
                tab_photo.document_code = document_code[ptr];
                tab_photo.document_name = document_name[ptr];
                tab_photo.document_type = filep.ContentType;
                tab_photo.down_count = 0;
                tab_photo.top_count = 0;
                tab_photo.input_date = odate;
                tab_photo.photo_type = "I";
                tab_photo.picture1 = uploaded;
                tab_photo.processed = 0;
                tab_photo.request_user = pblock.userid;
                tab_photo.staff_number = snumber;
                tab_photo.comment_code = "";

                if (ph_flag)
                    db.tab_photo.Add(tab_photo);
                else
                    db.Entry(tab_photo).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }

                catch (Exception err)
                {
                    save_message("Error on attachment files, but document processed : " + err.InnerException.InnerException.Message);
                }

                ptr++;
            }

            if (array1 != null)
            {
                string arlist = string.Join(",", array1);
                arlist = arlist.Replace(",,", ",0,");
                string laststr = "0";
                if (arlist.Length > 1)
                    laststr = arlist.Substring(arlist.Length - 1);

                if (laststr == ",")
                    arlist += "0";

                if (arlist.Length > 0)
                {
                    string str1 = "delete from tab_photo where photo_type='I' and staff_number=" + pads(snumber) + " and document_code in (" + arlist + ")";
                    db.Database.ExecuteSqlCommand(str1);
                }
            }

        }

        public IEnumerable<vw_picture> read_attachmentt(string snumber, string ttype, string vdate, string cur_code="")
        {
            var bf = from bj in db.tab_doctrans
                        where bj.staff_number == snumber && bj.trans_type == ttype && bj.value_date == vdate
                     
                     select bj;

            if (cur_code !="")
            {
                bf = bf.Where(m => m.comment_code == cur_code);
            }

            bf = bf.OrderBy(m => m.sequence_no);

            string str1 = "select * from tab_doctrans a left outer join tab_soft b on ( b.para_code = 'IMGDEF' and document_type like '%' + report_code) ";
            str1 += " where a.staff_number=" + pads(snumber) + " and a.trans_type=" + pads(ttype) + " and a.value_date=" + pads(vdate);
            if (cur_code != "") str1 += " and a.comment_code=" + pads(cur_code);
            str1 += " order by 1 ";
            var bg2 = db.Database.SqlQuery<vw_picture>(str1);
            
            return bg2.ToList();

        }


        public SelectList querylist(string qcode,string acode="", int ps=0)
        {
            string str1;
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            if (ps == 1)
            {
                str1 = "select c1 = report_code, c2=report_name1 from tab_soft where para_code=" + pads(qcode);
            }
            else if (ps==2)
            {
                str1 = "select c1 = pcode, c2=pcode + ' : ' + pname from vw_user_parameter where hcode=" + pads(qcode);
                if (qcode == "STAFF")
                    str1 = "select staff_number c1, c2=staff_number + ' : ' + dbo.capname(surname,first_name,other_name) from tab_staff where close_code='0' ";
                else if (qcode == "ALLOW")
                    str1 = "select c1 = allow_code, c2=allow_code + ' : ' + name1 from tab_allow where para_code='02' and allowance_code='A' ";
                else if (qcode == "DEDUCT")
                    str1 = "select c1 = allow_code, c2=allow_code + ' : ' + name1 from tab_allow where para_code='02' and allowance_code='D' ";
                else if (qcode == "DALLOW")
                    str1 = "select c1 = daily_code, c2=daily_code + ' : ' + name1 from tab_daily where para_code='03' and code_ind='A' ";
                else if (qcode == "DDEDUCT")
                    str1 = "select c1 = daily_code, c2=daily_code + ' : ' + name1 from tab_daily where para_code='03' and code_ind='D' ";
            }
            else
            {
                str1 = "select c1 = pcode, c2=case '" + pblock.cquery + "' when 'Y' then pcode + ' : ' + pname else pname end from vw_user_parameter where hcode=" + pads(qcode);
                if (qcode == "STAFF")
                    str1 = "select staff_number c1, c2=case '" + pblock.cquery + "' when 'Y' then staff_number + ' : ' + dbo.capname(surname,first_name,other_name) else dbo.capname(surname,first_name,other_name) end from tab_staff where close_code='0' ";
                else if (qcode == "ALLOW")
                    str1 = "select c1 = allow_code, c2=case '" + pblock.cquery + "' when 'Y' then allow_code + ' : ' + name1 else name1 end from tab_allow where para_code='02' and allowance_code='A' ";
                else if (qcode == "DEDUCT")
                    str1 = "select c1 = allow_code, c2=case '" + pblock.cquery + "' when 'Y' then allow_code + ' : ' + name1 else name1 end from tab_allow where para_code='02' and allowance_code='D' ";
            }

            str1 += " order by 2 ";

            var bglist1 = db.Database.SqlQuery<vw_query>(str1);
            return new SelectList(bglist1.ToList(), "c1", "c2", acode);

        }

        public void delete_attachmentp(string hcode, string pcode, string ccode)
        {
            string str1 = "delete from tab_docpara where hcode=" + pads(hcode) + " and pcode=" + pads(pcode) + "and comment_code=" + pads(ccode);
            db.Database.ExecuteSqlCommand(str1);

        }

        public SelectList staff_query_sel(string staff_no,string staff_sel)
        {

            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            SelectList selp;

            var bglist = from bg in db.tab_staff
                         orderby bg.staff_name
                         select new { c1 = bg.staff_number, c2 = bg.staff_name, c3 = bg.close_code };

            var bglista = bglist;
            if (staff_sel == "S")
            {
                bglista = bglist.Where(m => m.c1 == pblock.userid);
                staff_no = pblock.userid;
            }
            else if (staff_sel == "R")
                bglista = bglist.Where(m => m.c3 != "0");
            else
                bglista = bglist.Where(m => m.c3 == "0");

            selp = new SelectList(bglista.ToList(), "c1", "c2", staff_no);

            if (staff_sel == "G")
            {
                string strz = " select distinct a.staff_number c1,staff_name c2 from tab_staff a , vw_pen_basic b where a.staff_number=b.staff_number ";
                var bglista1 = db.Database.SqlQuery<vw_query>(strz);

                selp = new SelectList(bglista1.ToList(), "c1", "c2", staff_no);

            }

            if (staff_sel == "M")
            {
                string strz = " select distinct staff_number c1,staff_name c2 from tab_staff where close_code='0' and approval_route in (select train_code from tab_train where para_code='H29'and report_name=" + pads(pblock.userid) + ")";
                var bglista1 = db.Database.SqlQuery<vw_query>(strz);

                selp = new SelectList(bglista1.ToList(), "c1", "c2", staff_no);
            }

            return selp;
        }

        public string staff_query_check(string staff_no, string staff_sel)
        {
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];

            if (staff_sel == "S")
                return pblock.userid;

            string sno = "";
            string str1 = "select staff_number c1 from tab_staff a where close_code='0' ";
            if (staff_sel == "G")
                str1 = " select distinct a.staff_number c1 from tab_staff a , vw_pen_basic b where a.staff_number=b.staff_number ";
            else if (staff_sel == "M")
                str1 = " select distinct staff_number c1 from tab_staff a where close_code='0' and approval_route in (select train_code from tab_train where para_code='H29'and report_name=" + pads(pblock.userid) + ")";
            else if (staff_sel == "R")
                str1 = "select staff_number c1 from tab_staff a where close_code <> '0' ";

            str1 += " and a.staff_number=" + pads(staff_no);
            var selp = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (selp != null)
                sno = selp.c1;

            return sno;

        }

        public SelectList payment_run_list()
        {
            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];

            string str1 = "select distinct a.operand c1, b.name1 c2,b.line_spacing from ( ";
            str1 += " select operand from tab_array where para_code='21K' and array_code=" + pads(pblock.ugroup) + " and 'U'=";
            str1 += " (select transfer_code from tab_calc where para_code='21' and calc_code=" + pads(pblock.ugroup) + " ) union all ";
            str1 += " select calc_code from tab_calc where para_code='UPDATE' and 'S'=";
            str1 += " (select transfer_code from tab_calc where para_code='21' and calc_code=" + pads(pblock.ugroup) + " ) union all ";
            str1 += " select operand from tab_array where para_code = 'EEMP' and ''=" + pads(pblock.ugroup) + ") a, ";
            str1 += " tab_calc b where b.para_code='UPDATE' and b.calc_code=a.operand order by b.line_spacing";
            var bglist = db.Database.SqlQuery<vw_query>(str1);
            SelectList selp = new SelectList(bglist.ToList(), "c1", "c2");

            return selp;
        }

        public bool check_approval_trans()
        {
            string str1;

            str1 = "select field10 c0 from tab_train_default where default_code='RUN2' ";
            var str21 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str21.c0 == "Y")
            {
                save_message("No Transaction Entries Allowed");
                return true;
            }

            return false;

        }

        public void set_titles(string idrep,string ptitle,string apentry=null)
        {
            worksess worksess = (worksess)HttpContext.Current.Session["worksess"];
            worksess.ptitle = ptitle;
            worksess.idrep = idrep;
            worksess.apentry = apentry;
            HttpContext.Current.Session["worksess"] = worksess;

        }

        public void set_util3(string idx)
        {
            worksess worksess = (worksess)HttpContext.Current.Session["worksess"];
            worksess.temp5 = idx;
            HttpContext.Current.Session["worksess"] = worksess;

        }

        public void save_message(string errmsg)
        {
            worksess worksess = (worksess)HttpContext.Current.Session["worksess"];
            worksess.bye_mess = errmsg;
            HttpContext.Current.Session["worksess"] = worksess;
        }

        public string part_string(string mstr,int spos, int slen)
        {
            if (mstr.Length < spos + slen)
                return "";

            return mstr.Substring(spos, slen);

        }

        public string timetoage(string sdate1, string edate1)
        {
            if (string.IsNullOrWhiteSpace(sdate1)) sdate1 = edate1;

            int year1 = Convert.ToInt16(sdate1.Substring(0, 4));
            int month1 = Convert.ToInt16(sdate1.Substring(4, 2));
            int day1 = Convert.ToInt16(sdate1.Substring(6, 2));
            DateTime sdate2 = new DateTime(year1, month1, day1);

            year1 = Convert.ToInt16(edate1.Substring(0, 4));
            month1 = Convert.ToInt16(edate1.Substring(4, 2));
            day1 = Convert.ToInt16(edate1.Substring(6, 2));
            DateTime sdate3 = new DateTime(year1, month1, day1);

            int days = 0; int years = 0;
            int months = 12 * (sdate3.Year - sdate2.Year) + (sdate3.Month - sdate2.Month);
            if (sdate3.Day < sdate2.Day)
            {
                months--;
                days = DateTime.DaysInMonth(sdate2.Year, sdate2.Month) - sdate2.Day + sdate3.Day;
            }
            else
            {
                days = sdate3.Day - sdate2.Day;
            }
            // compute years & actual months
            years = months / 12;
            months -= years * 12;

            return Convert.ToString(years) + " year(s) " + Convert.ToString(months) + " month(s) " + Convert.ToString(days) + " day(s) ";

        }

    }
}
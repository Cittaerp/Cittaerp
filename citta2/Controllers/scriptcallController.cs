using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using anchor1.Models;
using anchor1v.Models;
using CittaErp.ReportA;
using anchor1.utilities;
using System.IO;

namespace anchor1.Controllers
{
    public class scriptcallController : Controller
    {
        //
        // GET: /scriptcall/

        private anchor1Context db = new anchor1Context();
        Boolean err_flag = true;
        vw_collect mcollect = new vw_collect();
        Cutil utils = new Cutil();
        mysessvals pblock;
        string type_code = "", pcode = "";
        string str1;
        worksess worksess;

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DailyListb(string idx)
        {
            string Code = idx;
            var ind = Code.Substring(0, 2);
            var rcode = Code.Substring(2);
            pblock = (mysessvals)Session["mysessvals"];
            worksess = (worksess)Session["worksess"];

            if (ind == "01")
            {
                // transaction and group based
                string pcode = worksess.temp0;
                if (pcode == "SELF")
                {
                    str1 = "select source1 as c1, operand as c2, '' c3 from tab_array where para_code='SELFT' UNION ALL ";
                    str1 = str1 + "select train_code, course_name, '' from tab_train where para_code='SELFT' union all ";
                    str1 = str1 + "select report_code, report_name1,'' from tab_soft where para_code = 'SELFT' and report_name2='S' order by 2 ";
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
                else if (pcode == "CRM")
                    str1 = "select calc_code c1,name1 c2,'1' c3 from tab_calc where para_code='F12' order by 2 ";

            }
            else if (ind == "02" || ind == "00")
            {
                // group list-00
                str1 = "select analy_code c1,analy0 c2 ,4 from tab_analy where para_code='ANALY' union all ";
                str1 += " select report_code,report_name1,numeric_ind from tab_soft where para_code='PAYGRP' ";

                if (ind == "02")
                {
                    //based for approval- alternate approval included
                    str1 += " union all select report_code c1,report_name1 c2 ,4 from tab_soft where para_code='DSEL' and report_code='H32' ";
                    str1 += " union all select report_code c1,report_name1 c2 ,4 from tab_soft where para_code='CRMOPT' and report_name2='Y' and exists( select 1 from crm_location where para_code='F01') ";
                }

                str1 += " order by 2 ";
            }
            else if (ind == "03" || ind == "10")
            {
                //free codes
            }
            else if (ind == "04")
            {
                // staff & query codes for queries, not for selection and reports
                if (rcode == "close")
                    str1 = "select staff_number c1, c2=dbo.capname(surname,first_name,other_name) from tab_staff a where close_code <> '0' " + pblock.p2000 + " order by 2 ";
                else if (rcode == "" || rcode == "Z" || rcode == "STAFF")
                    str1 = "select staff_number c1, c2=dbo.capname(surname,first_name,other_name) from tab_staff a where close_code='0' " + pblock.p2000 + " order by 2 ";
                else if (rcode == "S")
                    str1 = "select staff_number c1, c2=dbo.capname(surname,first_name,other_name) from tab_staff a where staff_number=" + utils.pads(pblock.userid) + " order by 2 ";
                else if (rcode == "R")
                    str1 = "select staff_number c1, c2=dbo.capname(surname,first_name,other_name) from tab_staff a where close_code <> '0' " + pblock.p2000 + " order by 2 ";
                else if (rcode == "M")
                    str1 = "select staff_number c1, c2=dbo.capname(surname,first_name,other_name) from tab_staff a where close_code ='0' " + pblock.p2000 + " and approval_route in (select train_code from tab_train where para_code='H29'and report_name=" + utils.pads(pblock.userid) + ") order by 2 ";
                else if (rcode == "G")
                    str1 = " select distinct a.staff_number c1,c2=dbo.capname(surname,first_name,other_name) from tab_staff a , vw_pen_basic b where a.staff_number=b.staff_number and b.payroll_type='P' " + pblock.p2000 + " order by 2 ";
                else if (rcode == "P")
                    str1 = "select user_code c1, c2=name1 from vw_user  order by 2 ";
                else if (rcode == "L")
                    str1 = "select user_code c1, name1 c2 from vw_user where type1 <> 'data' and locked_flag='Y'  order by 2 ";
                else
                    str1 = "select c1 = gcode, c2=case '" + pblock.cquery + "' when 'Y' then gcode + ' : ' + gname else gname end from vw_grouptrans where gpara=" + utils.pads(rcode) + " order by 2 ";
                //str1 = "select c1 = gcode, c2=gname from vw_grouptrans where gpara=" + utils.pads(rcode) + " order by 2 ";
            }
            else if (ind == "05")
            {
                //folder selection
                string pcode = worksess.temp0;
                if (pcode == "SELF" || pcode == "CASH" )
                {
                    str1 = "select train_code as c1, course_name as c2,'1' from tab_train where para_code='H29' union all ";
                    str1 += " select report_code, report_name1,'0' from tab_soft where para_code='APORG' union all ";
                    str1 += " select job_code, name1,'2' from tab_job_desc union all ";
                }

                //if (pcode == "CRM" )
                //{
                //    str1 = "select train_code as c1, course_name as c2,'1' from tab_train where para_code='H29' union all ";
                //    str1 += " select report_code, report_name1,'0' from tab_soft where para_code='APORG'  union all ";
                //}

                str1 +=  " select staff_number as c1, dbo.capname(surname,first_name,other_name) as c2,'3' from tab_staff where close_code='0' order by 3, 2 ";
            }
            else if (ind == "21")
            {
                // access group
                str1 = "select calc_code c1, c2=name1 from tab_calc where para_code='21' order by 2 ";

            }
            else if (ind == "06")
            {
                //dropdown list for all codes in range for selection and reports- base on group list selection
                str1 = "select pcode c1, pcode + '- ' + pname c2 from vw_user_parameter where close_code='N' and hcode= " + utils.pads(rcode);
                str1 += " union all select staff_number, staff_number + ' - ' + dbo.capname(surname,first_name,other_name) from tab_staff where close_code='0' ";
                str1 += " and " + utils.pads(rcode) + " in ('NUMB','STAFF') ";
                str1 += " union all select report_code, report_name1 from tab_soft where para_code= " + utils.pads(rcode);
                str1 += " union all select pcode, pcode + '- ' + pname from vw_user_parameter where close_code='N' and hcode in ";
                str1 += " (select report_name7 from tab_soft where para_code='APRDET' and report_code=" + utils.pads(rcode) + ")";
                str1 += " union all select pcode, pcode + '- ' + pname from vw_user_parameter where close_code='N' and hcode in (select field12 from tab_train_default where default_code='MEDIC') ";
                str1 += " and 'APDEPT'=" + utils.pads(rcode);
                str1 += " union all select pcode, pcode + '- ' + pname from vw_user_parameter where close_code='N' and hcode in (select field9 from tab_train_default where default_code='APPR') ";
                str1 += " and 'APLOC'=" + utils.pads(rcode);
                str1 += " union all select cast(count_array as varchar), operand from tab_array where para_code='APRTYPE' and 'APRCYCLE'=" + utils.pads(rcode);
                str1 += " union all select ''+cast(count_array as varchar), operand from tab_array where para_code='APPRN' and 'APRSTAT'=" + utils.pads(rcode);
                str1 += " union all select '-1','New' where 'APRSTAT'=" + utils.pads(rcode);
                str1 += " union all select '99', 'Completed' where 'APRSTAT'=" + utils.pads(rcode);
                str1 += " union all select staff_number, staff_number + ' - ' + dbo.capname(surname,first_name,other_name) from tab_staff where email_address <>'' and " + utils.pads(rcode) + " in ('APPRNAME1','APPRNAME2','APPRNAME3','APPRNAME4','APPRNAME5','APPRNAME6')";
                str1 += " union all select pcode, pcode + '- ' + pname from vw_user_parameter where close_code='N' and hcode = (select source1 from tab_array where para_code in ('EXPENSE2','INCIDENT') and array_code=" + utils.pads(rcode) + ") ";
                str1 += " order by 1";
            }

            else if (ind == "07")
            {
                // list1 for staff excluding selected
                    str1 = "select staff_number c1, dbo.capname(surname,first_name,other_name) c2 from tab_staff where close_code='0'and not staff_number in ";
                    str1 += " (select op1 from tab_temp_rep where report_line='DET' and user_code=" + utils.pads(pblock.userid) + " and src1 in ('S','')) order by 2 ";
            }
            else if (ind == "08")
            {
                // list2 for selected staff
                str1 = "select op1 c1, dbo.capname(surname,first_name,other_name) c2 from tab_temp_rep a, tab_staff where op1=staff_number and report_line='DET' and user_code=" + utils.pads(pblock.userid) + "  and src1 in ('S','') order by 2 ";
            }
            else if (ind == "09")
            {
                //Approval list 
                str1 = "select report_code c1, report_name1 c2 from tab_soft where para_code='APPADV' order by 2 ";
            }
            else if (ind == "11")
            {
                // payment run base on access
                str1 = "select distinct a.operand c1, b.name1 c2,b.line_spacing from ( ";
                str1 += " select operand from tab_array where para_code='21K' and array_code=" + utils.pads(pblock.ugroup) + " and 'U'=";
                str1+=" (select transfer_code from tab_calc where para_code='21' and calc_code=" + utils.pads(pblock.ugroup) + " ) union all ";
                str1 += " select calc_code from tab_calc where para_code='UPDATE' and 'S'=";
                str1+=" (select transfer_code from tab_calc where para_code='21' and calc_code=" + utils.pads(pblock.ugroup) + " ) union all ";
                str1 += " select operand from tab_array where para_code = 'EEMP' and ''=" + utils.pads(pblock.ugroup) + ") a, ";
                str1 += " tab_calc b where b.para_code='UPDATE' and b.calc_code=a.operand order by b.line_spacing";
            }
            else if (ind == "12")
            {
                string ptype = rcode.Substring(0, 1);
                rcode = rcode.Substring(1);
                if (ptype == "1")
                {
                    // process period period available depending on payment run selected and setting current to default
                    str1 = "select '0' c1,processing_period c2 from tab_pay_default , tab_calc where para_code='UPDATE' and ";
                    str1 += " transfer_code=payment_cycle and calc_code=" + utils.pads(rcode) + " union all ";
                    str1 += " select substring('00' + cast(count_seq as varchar),len('00' + cast(count_seq as varchar))-1,2) c1,period_name c2 from tab_calc , tab_calend where para_code='UPDATE' and ";
                    str1 += " transfer_code=payment_cycle and calc_code=" + utils.pads(rcode) + "  order by 1 ";
                }
                else if (ptype == "2")
                {
                    //Posting period depending on posting run selected
                    str1 = "select '0' c1,processing_period c2 from tab_pay_default , tab_calc where para_code='UPDATE' and ";
                    str1 += " transfer_code=payment_cycle and calc_code=(select report_name from tab_bank where para_code='46' and bank_code=" + utils.pads(rcode) + ") union all ";
                    str1 += " select substring('00' + cast(count_seq as varchar),len('00' + cast(count_seq as varchar))-1,2) c1,period_name c2 from tab_calc , tab_calend where para_code='UPDATE' and ";
                    str1 += " transfer_code=payment_cycle and calc_code=(select report_name from tab_bank where para_code='46' and bank_code=" + utils.pads(rcode) + ")  order by 1 ";
                }
                else if (ptype == "3")
                {
                    //posting period depending on staff
                    str1 = "select '0' c1,processing_period c2 from tab_pay_default , tab_calc where para_code='UPDATE' and ";
                    str1 += " transfer_code=payment_cycle and calc_code=(select default_pay_cycle from tab_default) union all ";
                    str1 += " select substring('00' + cast(count_seq as varchar),len('00' + cast(count_seq as varchar))-1,2) c1,period_name c2 from tab_calend where payment_cycle=";
                    str1 += " (select payment_cycle from tab_grade where para_code='01' and grade_code=(select category from tab_staff where staff_number=" + utils.pads(rcode);
                    str1 += " )) union all ";
                    str1 += " select substring('00' + cast(count_seq as varchar),len('00' + cast(count_seq as varchar))-1,2) c1,period_name c2  from tab_calc ,tab_calend ";
                    str1 += " where para_code='UPDATE' and calc_code=(select default_pay_cycle from tab_default) and payment_cycle=transfer_code and not exists ";
                    str1 += " (select payment_cycle from tab_grade where para_code='01' and grade_code=(select category from tab_staff where staff_number=" + utils.pads(rcode) + " )) ";
                    str1 += " order by 1 ";
                }
            }
            else if (ind == "13")
            {
                //menu list E=menu, s= emenu, t= all menu
                string menuopt = "report_name5=" + utils.pads(rcode);
                if (rcode == "T")
                    menuopt = " report_name5 in ('E','S') ";
                str1 = "select report_code c1, report_name1 c2 from tab_soft where para_code='RMENU' and " + menuopt + " order by report_name4 ";
            }
            else if (ind == "14")
            {
                //Posting period depending on posting run selected
                str1 = "select '0' c1,processing_period c2 from tab_pay_default , tab_calc where para_code='UPDATE' and ";
                str1 += " transfer_code=payment_cycle and calc_code=(select report_name from tab_bank where para_code='46' and bank_code=" + utils.pads(rcode) + ") union all ";
                str1 += " select substring('00' + cast(count_seq as varchar),len('00' + cast(count_seq as varchar))-1,2) c1,period_name c2 from tab_calc , tab_calend where para_code='UPDATE' and ";
                str1 += " transfer_code=payment_cycle and calc_code=(select report_name from tab_bank where para_code='46' and bank_code=" + utils.pads(rcode) + ")  order by 1 ";
            }
            else if (ind == "15")
            {
                //Performance who approves list 
                str1 = "select report_code c1, report_name1 c2,'0' from tab_soft where para_code='APORG' and report_name2='Y' union all ";
                str1 += " select staff_number, staff_name,'3' from tab_staff where close_code='0' order by 3, 2";
            }
            else if (ind == "16")
            {
                // additional fields for performance
                str1 = "select wcode c1, wname c2  from vw_groupcodes where wpara in ('ANALY','HDEF') union all ";
                str1 += "select report_code c1, report_name1 c2 from tab_soft where para_code='DSEL' and report_name3='Y' and report_name6 not in ('','D') order by 2 ";
            }
            else if (ind == "17")
            {
                //report design sort optons
                str1 = "select analy_code as c1, analy0 as c2, analy1 as c31 from tab_analy where para_code in ('ANALY') union all ";
                if (rcode == "HA07" || rcode == "HA08")
                    str1 += " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name3='Y' ";
                else if (rcode == "P04")
                    str1 += " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name10='Y' ";
                else
                    str1 += " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name5='Y' ";

                if (rcode == "H47")
                {
                    str1 += " union all select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'EXPLINES' and report_code='23' ";
                    str1 += " union all select array_code as c1, operand as c2, '' as c31 from tab_array where para_code = 'EXPENSE2' ";
                }

                if (rcode == "H15")
                {
                    str1 = "select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name5='Y' union all ";
                    str1 += "select analy_code as c1, analy0 as c2, analy1 as c31 from tab_analy where para_code in ('ANALY') union all ";
                    str1 += "select 'LINE' + ltrim(str(count_type)) as c1, rtrim(text_line) + '  T' as c2, '' from tab_type2, vw_hrcode where trans_type=" + utils.pads(worksess.temp5) + "  and text_code=vcode union all ";
                    str1 += "select operand as c1, source1 as c2, '' from tab_array where para_code='H46' and array_code=" + utils.pads(worksess.temp5);
                }

                if (rcode == "F13")
                {
                    str1 = " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'FDSEL' and report_name3='Y' ";
                    str1 += " union all select array_code, operand, ''  from tab_array where para_code = 'INCIDENT' ";
                }

                if (rcode == "F17")
                {
                    str1 = " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'FDSEL' and report_name2='Y' ";
                    str1 += " union all select array_code, operand, ''  from tab_array where para_code = 'INCIDENT' ";
                }
                if (rcode == "F18")
                {
                    str1 = " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'FDSEL' and report_name4='Y' ";
                    str1 += " union all select array_code, operand, ''  from tab_array where para_code = 'INCIDENT' ";
                }

                str1 += " order by 2 ";
            }
            else if (ind == "18")
            {
                // additional columns 
                str1 = "select report_code c1, report_name1 c2 from tab_soft where para_code='CP' and report_name5='Y' and report_name9 <> '' ";
                str1 += " union all select analy_code,analy0 from tab_analy where para_code in ('ANALY','PERS','AMT') order by 2 ";
            }
            else if (ind == "19")
            {
                // screen options for transfers
                
                if (rcode == "TT")
                    str1 = "select trans_type c1, b.name1 c2 from tab_calc a, tab_type b where a.para_code='TTD' and a.report_name=b.trans_type order by a.name1 ";
                else
                    str1 = "select report_code c1, report_name1 c2 from tab_soft where para_code='TR' and report_name9 like '" + rcode + "%' order by 2 ";
            }
            else if (ind == "20")
            {
                str1 = "select allow_code c1, name1 c2 from tab_allow where para_code='02' and allowance_code='A' union all ";
                str1 += " select 'zxpallow','zxpallow' union all ";
                str1 += " select allow_code c1, name1 from tab_allow where para_code='02' and allowance_code='D' union all ";
                str1 += " select 'zxpdeduct','zxpdeduct' union all ";
                str1 += " select daily_code c1, name1 from tab_daily where para_code='03' and code_ind='A' union all ";
                str1 += " select 'zxpdallow','zxpdallow' union all ";
                str1 += " select daily_code c1, name1 from tab_daily where para_code='03' and code_ind='D' union all ";
                str1 += " select 'zxpddeduct','zxpddeduct' union all ";
                str1 += " select loan_code c1, name1 from tab_loan where para_code='05'  union all ";
                str1 += " select 'zxploan','zxploan' ";
            }

            else if (ind == "22")
            {
                // period selected in words i.e. January 20xx
                str1 = "select period_name + ', ' + substring(processing_period,1,4) c1,'' c2 from tab_calc a, tab_pay_default b, tab_calend c ";
                str1 += "where para_code='UPDATE' and transfer_code=b.payment_cycle and calc_code=" + utils.pads(rcode);
                str1 += " and c.payment_cycle=b.payment_cycle and count_seq=substring(processing_period,5,2)";
            }
            else if (ind == "23")
            {
                //approval header definitions
                str1 = "select 'E' + trans_type c1, name1 c2,1 from tab_type union all ";
                str1 += "select 'V' + calc_code , name1,1 from tab_calc where para_code='H46' union all ";
                str1 += "select 'F' + calc_code , name1,1 from tab_calc where para_code='F12' union all ";
                str1 += " select 'P' + report_code,report_name1,1 from tab_soft where para_code='PTRAN' union all ";
                str1 += " select report_name2 + report_code, report_name1,0 from tab_soft where para_code='APPHEAD' ";
                str1 += " order by 3,2 ";
            }
            else if (ind == "24")
            {
                //transactions for approval header definition
                string arcode = (rcode + "  ").Substring(0, 1);
                string vcode = (rcode + "  ").Substring(1);
                str1 = "select report_name2 c4 from tab_soft where para_code='APPHEAD' and report_code=" + utils.pads(vcode);
                var dball = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                if (dball != null)
                { 
                    str1 = " select report_code c1, report_name1 c2, 1 from tab_soft where para_code = 'MESSCON' and report_name2 in ( ''," +  utils.pads(dball.c4) + ") and report_name3 = '' union all ";
                    str1 += " select array_code, operand, 1 from tab_array where para_code = 'EXPENSE2' and 'V' = " + utils.pads(dball.c4) + " union all ";
                    str1 += " select array_code, operand, 1 from tab_array where para_code = 'INCIDENT' and 'F' = " + utils.pads(dball.c4);
                }
                else if (arcode == "E")
                {
                    str1 = "select 'LINE' + cast(count_type as varchar) c1, text_line c2,sequence_no from tab_type2 where trans_type = " + utils.pads(vcode) + " union all ";
                    str1 += " select 'VDATE', value_date,0 from tab_type where trans_type =" + utils.pads(vcode) + " union all ";
                    str1 += " select report_code, report_name1,99 from tab_soft where para_code = 'MESSCON' and report_name2 = '' ";
                }
                else if (arcode == "V")
                {
                    str1 = "select operand c1,source1 c2,count_array from tab_array where para_code='H46' and array_code = " + utils.pads(vcode) + " union all ";
                    str1 += " select report_code,report_name1,1 from tab_soft where para_code='EXPLINES' and report_name2='' union all ";
                    str1 += " select report_code, report_name1,99 from tab_soft where para_code = 'MESSCON' and report_name2 = ''  ";
                }
                else if (arcode == "F")
                {
                    str1 = "select operand c1,source1 c2,count_array from tab_array where para_code='F12' and array_code = " + utils.pads(vcode) + " union all ";
                    str1 += " select report_code, report_name1,99 from tab_soft where para_code = 'MESSCON' and report_name2 = '' ";
                }
                else
                    str1 = " select '' c1, '' c2, 1 ";

                str1 += "   order by 2,3 ";
            }
            else if (ind == "25")
            {
                //users list
                str1 = "select user_code c1, name1 c2 from vw_user order by 2 ";
            }
            else if (ind == "26")
            {
                //parameter report list
                str1 = "select report_code c1, report_name1 c2 from tab_soft where para_code='LOG' and report_name9 like '" + rcode + "%' order by 2 ";
            }
            else if (ind == "27")
            {
                str1 = "select c1 = gcode, c2=case '" + pblock.cquery + "' when 'Y' then gcode + ' : ' + gname else gname end from vw_grouptrans where gpara=" + utils.pads(Code);
            }


            var str2 = db.Database.SqlQuery<vw_query>(str1);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index", null, new { anc = Ccheckg.convert_pass2("pc=") });
        }

        [HttpPost]
        public ActionResult defaultsn(string idx)
        {
            string str1 = "";
            string sno = "";
            string Code = idx;

            pblock = (mysessvals)Session["mysessvals"];
            if (string.IsNullOrWhiteSpace(idx))
                Code = "";

            if (Code == "close")
                str1 = "select staff_number c1, c2=case '" + pblock.cquery + "' when 'Y' then staff_number + ' : ' + dbo.capname(surname,first_name,other_name) else dbo.capname(surname,first_name,other_name) end  from tab_staff where close_code <> '0' " + pblock.p2000 + " order by 2 ";
            else if (Code == "" || Code == "Z" || Code == "STAFF")
                str1 = "select staff_number c1, c2=case '" + pblock.cquery + "' when 'Y' then staff_number + ' : ' + dbo.capname(surname,first_name,other_name) else dbo.capname(surname,first_name,other_name) end  from tab_staff where close_code='0' " + pblock.p2000 + " order by 2 ";
            else if (Code == "S")
                str1 = "select staff_number c1, c2=case '" + pblock.cquery + "' when 'Y' then staff_number + ' : ' + dbo.capname(surname,first_name,other_name) else dbo.capname(surname,first_name,other_name) end  from tab_staff where staff_number=" + utils.pads(pblock.userid) + " order by 2 ";
            else if (Code == "R")
                str1 = "select staff_number c1, c2=case '" + pblock.cquery + "' when 'Y' then staff_number + ' : ' + dbo.capname(surname,first_name,other_name) else dbo.capname(surname,first_name,other_name) end  from tab_staff where close_code <> '0' " + pblock.p2000 + " order by 2 ";
            else if (Code == "M")
                str1 = "select staff_number c1, c2=case '" + pblock.cquery + "' when 'Y' then staff_number + ' : ' + dbo.capname(surname,first_name,other_name) else dbo.capname(surname,first_name,other_name) end  from tab_staff where close_code ='0' " + pblock.p2000 + " and approval_route in (select train_code from tab_train where para_code='H29'and report_name=" + utils.pads(pblock.userid) + ") order by 2 ";
            else if (Code == "G")
                str1 = " select distinct a.staff_number c1,c2=case '" + pblock.cquery + "' when 'Y' then a.staff_number + ' : ' + dbo.capname(surname,first_name,other_name) else dbo.capname(surname,first_name,other_name) end  from tab_staff a , vw_pen_basic b where a.staff_number=b.staff_number and b.payroll_type='P' order by 2 ";
            else
                str1 = "select c1 = gcode, c2=case '" + pblock.cquery + "' when 'Y' then gcode + ' : ' + gname else gname end from vw_grouptrans where gpara=" + utils.pads(Code);

            var str2 = db.Database.SqlQuery<vw_query>(str1);
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index", null, new { anc = Ccheckg.convert_pass2("pc=") });
        }


        [HttpPost]
        public ActionResult defaultrep(string idx)
        {
            string str1 = "";
            string sno = "";
            string Code = idx;

            pblock = (mysessvals)Session["mysessvals"];
            if (string.IsNullOrWhiteSpace(idx))
                Code = "";

            if (Code == "close")
                str1 = "select staff_number c1, c2= staff_number + ' : ' + dbo.capname(surname,first_name,other_name) from tab_staff where close_code <> '0' " + pblock.p2000 + " order by 2 ";
            else if (Code == "" || Code == "Z" || Code == "STAFF")
                str1 = "select staff_number c1, c2= staff_number + ' : ' + dbo.capname(surname,first_name,other_name) from tab_staff where close_code='0' " + pblock.p2000 + " order by 2 ";
            else if (Code == "S")
                str1 = "select staff_number c1, c2= staff_number + ' : ' + dbo.capname(surname,first_name,other_name) from tab_staff where staff_number=" + utils.pads(pblock.userid) + " order by 2 ";
            else if (Code == "R")
                str1 = "select staff_number c1, c2= staff_number + ' : ' + dbo.capname(surname,first_name,other_name) from tab_staff where close_code <> '0' " + pblock.p2000 + " order by 2 ";
            else if (Code == "M")
                str1 = "select staff_number c1, c2= staff_number + ' : ' + dbo.capname(surname,first_name,other_name) from tab_staff where close_code ='0' " + pblock.p2000 + " and approval_route in (select train_code from tab_train where para_code='H29'and report_name=" + utils.pads(pblock.userid) + ") order by 2 ";
            else if (Code == "G")
                str1 = " select distinct a.staff_number c1,c2= a.staff_number + ' : ' + dbo.capname(surname,first_name,other_name) from tab_staff a , vw_pen_basic b where a.staff_number=b.staff_number and b.payroll_type='P' order by 2 ";
            else
                str1 = "select c1 = gcode, c2= gcode + ' : ' + gname from vw_grouptrans where gpara=" + utils.pads(Code);

            var str2 = db.Database.SqlQuery<vw_query>(str1);
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index", null, new { anc = Ccheckg.convert_pass2("pc=") });
        }

        private string app_select(string pcode)
        {
            if (pcode == "SELF")
            {
                str1 = "select array_code as c1, operand as c2, '' c3 from tab_array where para_code='SELF' UNION ALL ";
                str1 = str1 + "select train_code, course_name, '' from tab_train where para_code='SELFT' union all ";
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
            else if (pcode == "CRM")
                str1 = "select calc_code c1,name1 c2,'1' c3 from tab_calc where para_code='F12' order by 2 ";

            return str1;
        }


        [HttpPost]
        public void PrintList(string idx =null)
        {
            worksess = (worksess)Session["worksess"];
            string det_str;
            if (idx == null)
                det_str = worksess.det;
            else
                det_str = Ccheckg.convert_pass2(idx, 1);

            int ws_count = det_str.IndexOf(":");
            string type_code = det_str.Substring(0, ws_count);

            pblock = (mysessvals)Session["mysessvals"];
            worksess.temp4 = "5";
            worksess.ptitle = "Document Printing";

            if (type_code == "V")
                worksess.ptitle = "Expense Document Printing";
            else if (type_code == "A")
                worksess.ptitle = "Performance Document Printing";
            else if (type_code == "F")
                worksess.ptitle = "Incidence Document Printing";

            worksess.viewflag = "1";
            string str4 = "execute update_public_table @cvalue='1',@name1='displayv',@p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str4);

            string str1 = " execute docexp_rtn @det_str= " + utils.padsnt(det_str) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            ws_count = det_str.IndexOf(":");
            type_code = det_str.Substring(0, ws_count);
            int ws_count1 = det_str.IndexOf(":", ws_count + 1);
            string snumber = det_str.Substring(ws_count + 1, ws_count1 - ws_count - 1);
            ws_count = det_str.IndexOf(":", ws_count1 + 1);
            string strans_type = det_str.Substring(ws_count1 + 1, ws_count - ws_count1 - 1);
            ws_count1 = det_str.IndexOf(":", ws_count + 1);
            string sdate = det_str.Substring(ws_count + 1, ws_count1 - ws_count - 1);
            ws_count = det_str.IndexOf(":", ws_count1 + 1);

            string sindicator = det_str.Substring(ws_count1 + 1, ws_count - ws_count1 - 1);
            string sgroup = det_str.Substring(ws_count + 1);

            if (type_code == "V" || type_code == "Z")
                str1 = "select name1 c0 from tab_calc where para_code='H46' and calc_code=" + utils.pads(strans_type);
            else if (type_code == "F")
                str1 = "select name1 c0 from tab_calc where para_code='F12' and calc_code=" + utils.pads(strans_type);
            else if (type_code == "E")
                str1 = "select name1 c0 from tab_type where trans_type=" + utils.pads(strans_type);
            else
                str1 = "select report_name1 c0 from tab_soft where para_code='PTRAN' and report_code=" + utils.pads(strans_type);

            var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str11 != null)
                worksess.ptitle = str11.c0;

            worksess.temp0 = "select  * from [" + pblock.userid + "st01] ";
            Session["worksess"] = worksess;

            Sreport5 report = new Sreport5();
            report.Run();
            report.Document.Dispose();
            report.Dispose();
            report = null;

            worksess.filep = "print/zx" + pblock.userid + ".rdf";


        }

        private void selectstr(string rcode)
        {
            //report design sort optons
            str1 = "select analy_code as c1, analy0 as c2, analy1 as c31 from tab_analy where para_code in ('ANALY') union all ";
            if (rcode == "HA07" || rcode == "HA08")
                str1 += " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name3='Y' ";
            else if (rcode == "P04")
                str1 += " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name10='Y' ";
            else
                str1 += " select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name5='Y' ";

            if (rcode == "H47")
            {
                str1 += " union all select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'EXPLINES' and report_code='23' ";
                str1 += " union all select array_code as c1, operand as c2, '' as c31 from tab_array where para_code = 'EXPENSE2' ";
            }

            str1 += " order by 2 ";
            if (rcode == "H15")
            {
                str1 = "select report_code as c1, report_name1 as c2, '' as c31 from tab_soft where para_code = 'DSEL' and report_name5='Y' union all ";
                str1 += "select analy_code as c1, analy0 as c2, analy1 as c31 from tab_analy where para_code in ('ANALY') union all ";
                str1 += "select 'LINE' + ltrim(str(count_type)) as c1, rtrim(text_line) + '  T' as c2, '' from tab_type2, vw_hrcode where trans_type=" + utils.pads(worksess.temp5) + "  and text_code=vcode order by 2 ";
            }

        }



    }
}
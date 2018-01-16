using CittaErp.Models;
using CittaErp.utilities;
using anchor1.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class Gltran_enqController : Controller
    {
        AP_002_VTRAD AP_002_VTRAD = new AP_002_VTRAD();
        AP_002_VTRAN gdoc = new AP_002_VTRAN();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
      
        bool err_flag = true;
        string move_auto = "N";
        string ptype = "";

        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index(string ptype1 = "")
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            return View(glay);          
        }

        [EncryptionActionAttribute]
        public ActionResult Create(string ptype1 = "")
        {
            psess = (psess)Session["psess"];
            
            util.init_values();
            if (ptype1 != "")
            {
                psess.temp0 = ptype1;
            }
            Session["psess"] = psess;
            
            ptype = psess.temp0.ToString();

            header_rtn();
           
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            glay.vwstring0 = "";
           // header_rtn();
            read_recordh();
            select_query();

           // read_details();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, string subcheck)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();

            glay = glay_in;
            header_rtn();
            
            read_recordh();
            //read_details();
            select_query();
            
            return View(glay);
           
           
        }

        [EncryptionActionAttribute]
        public ActionResult Custenq(string ptype1 = "")
        {
            psess = (psess)Session["psess"];
            util.init_values();
            if (ptype1 != "")
            {
                psess.temp0 = ptype1;
            }
            Session["psess"] = psess;
           
            ptype = psess.temp0.ToString();
            
            custheader_rtn();

            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            glay.vwstring0 = "";
            // header_rtn();
            read_cust_record();
            select_query_cust();

            // read_details();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Custenq(vw_genlay glay_in, string subcheck)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();

            glay = glay_in;
            custheader_rtn();

            read_cust_record();
            //read_details();
            select_query_cust();

            return View(glay);


        }

        [EncryptionActionAttribute]
        public ActionResult ledgenq(string ptype1 = "")
        {
            util.init_values();
            psess = (psess)Session["psess"];
            
           
            ledgheader_rtn();

            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            glay.vwstring0 = "";
            // header_rtn();
            read_ledg_record();
            select_query_ledg();

            // read_details();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ledgenq(vw_genlay glay_in, string subcheck)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            glay = glay_in;
            ledgheader_rtn();

            read_ledg_record();
            //read_details();
            select_query_ledg();

            return View(glay);


        }
        
        private void header_rtn()
        {
            var hlabel = (from bh in db.GB_999_MSG
                          where bh.type_msg == "HEAD" && bh.code_msg == ptype && bh.name6_msg == "P"
                          select bh).FirstOrDefault();


            psess.temp1 = hlabel.name1_msg;
            psess.temp2 = hlabel.name2_msg;
            psess.temp3 = hlabel.name3_msg;
            glay.vwstrarray5 = new string[20];
            string[] aheader5 = new string[20];
            psess.sarrayt1 = aheader5;
            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == ptype && bg.sequence_no != 99
                         select bg;

            foreach (var item in bglist.ToList())
            {
                int count2 = item.sequence_no;
                //glay.vwstrarray4[count2] = item.header_code;
                var bglist2 = (from bg in db.GB_001_HANAL
                               where bg.header_sequence == item.header_code
                               select bg).FirstOrDefault();

                if (bglist2 != null)
                {
                    glay.vwstrarray5[count2] = bglist2.header_description;
                   
                }

            }

            psess.sarrayt1 = glay.vwstrarray5;
            Session["psess"] = psess;
            
            
        }

     
        private void select_query()
        {

            var emp = from pf in db.AP_002_VTRAN
                      where pf.module_account_type==ptype
                      select pf;
            ViewBag.jrnbr = new SelectList(emp.ToList(), "document_number", "document_number", glay.vwstring0);
        }
        private void read_recordh()
        {
            MainContext dbi = new MainContext();
         
            if (!string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                string str1 = "select col1, col2, col3, col4, col5, col6, col7,col29,col30,col8";
                str1 += "  from  temptab where col4 between " + util.date_yyyymmdd(glay.vwstring1) + " and " + util.date_yyyymmdd(glay.vwstring2) + "and coltype=" + ptype;
                var emp1 = db.Database.SqlQuery<temptab>(str1);
                ViewBag.x1 = emp1.ToList();
                foreach (var item in emp1)
                {
                    str1 = "select col3, col4, col5, col12, col10, col24, col25, col26, col27, col14, col15, col16, col2";
                    str1 += " from  temptab where col4 between " + util.date_yyyymmdd(glay.vwstring1) + " and " + util.date_yyyymmdd(glay.vwstring2) + "and coltype=" + ptype;
                    var emp2 = dbi.Database.SqlQuery<temptab>(str1);
                    ViewBag.x2 = emp2.ToList();
                }
            }
            else
            {
                string str1 = "select col1, col2, col3, col4, col5, col6, col7,col29,col30,col8";
                str1 += "  from  [" + pubsess.userid + "thead] where colid=" + util.sqlquote(glay.vwstring0) + " and coltype=" + ptype;
                var emp1 = db.Database.SqlQuery<temptab>(str1);
                ViewBag.x1 = emp1.ToList();

                str1 = "select col3, col4, col5, col12, col10, col24, col25, col26, col27, col14, col15, col16, col2";
                str1 += " from  [" + pubsess.userid + "tdet] where colid=" + util.sqlquote(glay.vwstring0) + " and coltype=" + ptype;
                emp1 = dbi.Database.SqlQuery<temptab>(str1);
                ViewBag.x2 = emp1.ToList();
            }
        }
        private void read_details()
        {
            string str1 = "select col1, col2, col3, col4, col6, col7, col8, col9, col10, col11, col12, col13, col14";
            str1 += " from  [" + pubsess.userid + "det] where colid=" + util.sqlquote(glay.vwstring0) + " and coltype=" + ptype;
            var emp1 = db.Database.SqlQuery<temptab>(str1);
            ViewBag.x2 = emp1.ToList();

        }
        private void read_record()
        {
            string str1 = "select col1 vwstring0, col2 vwstring1, col3 vwstring2, col4 vwstring3, col5 vwstring4, col6 vwstring5, col7 vwstring6,";
            str1 += " col8 vwstring7 from vw_curreny_display ";
            var emp1 = db.Database.SqlQuery<vw_genlay>(str1);

            var bglist = from bh in db.GL_002_JONAL
                         join bj in db.GB_999_MSG
                           on new { a1 = bh.period, a2 = "FYM" } equals new { a1 = bj.code_msg, a2 = bj.type_msg }
                           into bj1
                         from bj2 in bj1.DefaultIfEmpty()
                         join bi in db.GB_999_MSG
                           on new { a1 = bh.journal_type, a2 = "JT" } equals new { a1 = bi.code_msg, a2 = bi.type_msg }
                           into bi1
                         from bi2 in bi1.DefaultIfEmpty()
                         where bh.journal_number == 08
                         select new vw_genlay
                         {
                             vwint0 = bh.journal_number,
                             vwstring0 = bh.manual_reference_number,
                             vwdecimal0 = bh.total_debit,
                             vwdecimal1 = bh.total_credit,
                             vwstring5 = bi2.name1_msg,
                             vwstring1 = bh.batch_description,
                             vwstring2 = bh.control_flag == "" ? "No Entry" : bh.control == 0 ? "Yes" : "No",//bh.total_credit > bh.total_debit ? "No": bh.total_credit < bh.total_debit? "No" : "Yes",
                             vwstring3 = "Open",
                             vwstring4 = bj2.name1_msg != null ? bj2.name1_msg + " " + bh.year : bh.period + " " + bh.year,
                             vwdate0 = bh.approval_date,
                             vwstring6 = bh.created_by,
                             vwstring7 = bh.approval_by

                         };

            ViewBag.x1 = bglist.ToList();


        }

        private void custheader_rtn()
        {
            var hlabel = (from bh in db.GB_999_MSG
                          where bh.type_msg == "HEAD" && bh.code_msg == ptype && bh.name6_msg == "P"
                          select bh).FirstOrDefault();


            psess.temp1 = hlabel.name1_msg;
            psess.temp2 = hlabel.name2_msg;
            psess.temp3 = hlabel.name3_msg;

            glay.vwstrarray5 = new string[20];
            string[] aheader5 = new string[20];
            psess.sarrayt1 = aheader5;
            Session["psess"] = psess;
           

        }

        private void select_query_cust()
        {

            string str1 = "select bank_code query0, bank_name query1 from vw_paraquery where bank='" + ptype + "'";
            //if (ptype=="014")
            //    str1 = "select bank_code query0, bank_name query1 from vw_paraquery where bank='016'";

            var bg1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.custenq = new SelectList(bg1.ToList(), "query0", "query1", glay.vwstring0);
        }

        private void read_cust_record()
        {
            MainContext dbi = new MainContext();

            if (!string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                string str1 = " select cast(document_number as varchar) col1,sequence_number,case module_account_type when '" + ptype + "'";
                str1 += " then dbo.posting_account(account_type,account_code)else dbo.posting_account(module_account_type,transaction_code) end col5, ";
                str1 += " transaction_date col3, case module_account_type when '" + ptype + "' then case amount_type when 'C' then 'D' else 'C' end else amount_type end col7, ";
                str1 += " period,reference_number col2,cast(exchange_rate as varchar) col9, ";
                str1 += " case module_account_type when '" + ptype + "' then case amount_type when 'C' then cast(amount as varchar) else cast((0-amount) as varchar) end else ";
                str1 += " case amount_type when 'D' then cast(amount as varchar) else cast((0-amount) as varchar) end end col6, ";
                str1 += " dbo.find_name(2,'',currency), ";
                str1 += " case module_account_type when '" + ptype + "' then case amount_type when 'C' then cast(base_amount as varchar) else cast((0-base_amount) as varchar) end else  ";
                str1 += " case amount_type when 'D' then cast(base_amount as varchar) else cast((0-base_amount) as varchar) end end col8 ";
                str1 += " from AP_002_VTRAD where (module_account_type='" + ptype + "' and transaction_code=" + util.sqlquote(glay.vwstring0) + ") or (account_type='" + ptype + "' and account_code=" + util.sqlquote(glay.vwstring0) + ") ";
                var emp1 = dbi.Database.SqlQuery<temptab>(str1);
                ViewBag.x2 = emp1.ToList();
                if (ptype == "001")
                    str1 = "select customer_code col1,cust_biz_name col2, dbo.find_name(2,'',currency_code) col4,cast(current_balance as varchar) col3, '0' col5 from AR_001_CUSTM where customer_code= " + util.sqlquote(glay.vwstring0);
                else if (ptype == "014")
                {
                    str1 = " select contract_id col1, (select isnull(discount_name,' ') + ' ('+isnull(item_name,' ')+ ' ('+isnull(cust_biz_name,' ')+')' from IV_001_ITEM ax, AR_001_CUSTM bx, DC_001_DISC cx";
                    str1 += " where ax.item_code=a.item_code and bx.customer_code=a.customer_id and cx.discount_code = a.selected_promo and cx.discount_count=0) col2 , dbo.find_name(2,'',currency_code) col4,cast(a.current_balance as varchar) col3,";
                    str1 += " cast(a.net_sales_val as varchar) col5, cast ((a.net_sales_val + a.current_balance) as varchar) col6";
                    str1 += " from IV_001_PC a where contract_id= " + util.sqlquote(glay.vwstring0);
                }
                else if (ptype == "004")
                    str1 = "select bank_code col1,bank_name col2, dbo.find_name(2,'',currency_code) col4,cast(current_balance as varchar) col3, '0' col5 from BK_001_BANK where bank_code= " + util.sqlquote(glay.vwstring0);
                else if (ptype == "003")
                    str1 = "select account_code col1,account_name col2, dbo.find_name(2,'',currency_code) col4,cast(current_balance as varchar) col3, '0' col5 from GL_001_CHART where account_code= " + util.sqlquote(glay.vwstring0);

                emp1 = db.Database.SqlQuery<temptab>(str1);
                ViewBag.x1 = emp1.ToList();
            }

            if (!string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                string str1 = " select cast(document_number as varchar) col1,sequence_number,case module_account_type when '" + ptype + "'";
                str1 += " then dbo.posting_account(account_type,account_code)else dbo.posting_account(module_account_type,transaction_code) end col5, ";
                str1 += " transaction_date col3, case module_account_type when '" + ptype + "' then case amount_type when 'C' then 'D' else 'C' end else amount_type end col7, ";
                str1 += " period,reference_number col2,cast(exchange_rate as varchar) col9, ";
                str1 += " case module_account_type when '" + ptype + "' then case amount_type when 'C' then cast(amount as varchar) else cast((0-amount) as varchar) end else ";
                str1 += " case amount_type when 'D' then cast(amount as varchar) else cast((0-amount) as varchar) end end col6, ";
                str1 += " dbo.find_name(2,'',currency), ";
                str1 += " case module_account_type when '" + ptype + "' then case amount_type when 'C' then cast(base_amount as varchar) else cast((0-base_amount) as varchar) end else  ";
                str1 += " case amount_type when 'D' then cast(base_amount as varchar) else cast((0-base_amount) as varchar) end end col8 ";
                str1 += " from AP_002_VTRAD where (module_account_type='" + ptype + "' and transaction_date=" + util.date_yyyymmdd(glay.vwstring1) + ") ";
                var emp1 = dbi.Database.SqlQuery<temptab>(str1);
                ViewBag.x2 = emp1.ToList();
               
                foreach (var item in emp1)
                {

                    if (ptype == "001")
                        str1 = "select customer_code col1,cust_biz_name col2, dbo.find_name(2,'',currency_code) col4,cast(current_balance as varchar) col3, '0' col5 from AR_001_CUSTM ";
                    else if (ptype == "014")
                    {
                        str1 = " select contract_id col1, (select isnull(discount_name,' ') + ' ('+isnull(item_name,' ')+ ' ('+isnull(cust_biz_name,' ')+')' from IV_001_ITEM ax, AR_001_CUSTM bx, DC_001_DISC cx";
                        str1 += " where ax.item_code=a.item_code and bx.customer_code=a.customer_id and cx.discount_code = a.selected_promo and cx.discount_count=0) col2 , dbo.find_name(2,'',currency_code) col4,cast(a.current_balance as varchar) col3,";
                        str1 += " cast(a.net_sales_val as varchar) col5, cast ((a.net_sales_val + a.current_balance) as varchar) col6";
                        str1 += " from IV_001_PC a ";
                    }
                    else if (ptype == "004")
                        str1 = "select bank_code col1,bank_name col2, dbo.find_name(2,'',currency_code) col4,cast(current_balance as varchar) col3, '0' col5 from BK_001_BANK ";
                    else if (ptype == "003")
                        str1 = "select account_code col1,account_name col2, dbo.find_name(2,'',currency_code) col4,cast(current_balance as varchar) col3, '0' col5 from GL_001_CHART ";

                   var  emp2 = db.Database.SqlQuery<temptab>(str1);
                    ViewBag.x1 = emp2.ToList();
                }
            }
        }

        private void ledgheader_rtn()
        {
            //var hlabel = (from bh in db.GB_999_MSG
            //              where bh.type_msg == "TBAT" && bh.code_msg == ptype
            //              select bh).FirstOrDefault();


            //psess.temp1 = hlabel.name1_msg;
            //psess.temp2 = hlabel.name2_msg;
            //psess.temp3 = hlabel.name3_msg;

            //glay.vwstrarray5 = new string[20];
            //string[] aheader5 = new string[20];
            //psess.sarrayt1 = aheader5;


        }

        private void select_query_ledg()
        {

            string str1 = "select account_code query0, account_code + ' - ' + account_name query1 from GL_001_CHART ";
            
            var bg1 = db.Database.SqlQuery<querylay>(str1);
            ViewBag.custenq = new SelectList(bg1.ToList(), "query0", "query1", glay.vwstring0);
        }

        private void read_ledg_record()
        {
            MainContext dbi = new MainContext();
            if (!string.IsNullOrWhiteSpace(glay.vwstring0))
            {
                string str1 = "select journal_number col1,sequence_number,reference_number col2,description col4,dbo.find_name(3,'HEAD',source_identifier) col5,transaction_date col3,period,cast(exchange_rate as varchar) col9,";
                str1 += " cast(amount as varchar) col6,debit_credit_code col7,modified_by col10,convert(varchar, modified_date, 0) col11,cast(base_amount as varchar) col8 from AP_002_LEDEN where account_code=" + util.sqlquote(glay.vwstring0);

                var emp1 = dbi.Database.SqlQuery<temptab>(str1);
                ViewBag.x2 = emp1.ToList();

                str1 = "select account_code col1,account_name col2, dbo.find_name(2,'',currency_code) col4,cast(current_balance as varchar) col3 from GL_001_CHART where account_code= " + util.sqlquote(glay.vwstring0);

                emp1 = db.Database.SqlQuery<temptab>(str1);
                ViewBag.x1 = emp1.ToList();
            }
            if (!string.IsNullOrWhiteSpace(glay.vwstring1))
            {
                string str1 = "select journal_number col1,sequence_number,reference_number col2,description col4,dbo.find_name(3,'HEAD',source_identifier) col5,transaction_date col3,period,cast(exchange_rate as varchar) col9,";
                str1 += " cast(amount as varchar) col6,debit_credit_code col7,modified_by col10,convert(varchar, modified_date, 0) col11,cast(base_amount as varchar) col8 from AP_002_LEDEN where transaction_date=" + util.date_yyyymmdd(glay.vwstring1);

                var emp1 = dbi.Database.SqlQuery<temptab>(str1);
                ViewBag.x2 = emp1.ToList();
                foreach (var item in emp1)
                {
                    str1 = "select account_code col1,account_name col2, dbo.find_name(2,'',currency_code) col4,cast(current_balance as varchar) col3 from GL_001_CHART ";

                   var emp2 = db.Database.SqlQuery<temptab>(str1);
                    ViewBag.x1 = emp1.ToList();
                }
            }
        }
      
        private void initial_rtn()
        {
            glay.vwstrarray0 = new string[50];
            glay.vwdclarray0 = new decimal[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray7 = new string[20];
            glay.vwblarray0 = new bool[20];
            glay.vwstrarray2 = new string[50];
            glay.vwdclarray1 = new decimal[20];
            glay.vwstrarray9 = new string[100];
            glay.vwstrarray0[1] = "I";
            glay.vwstrarray0[5] = "N";
            glay.vwstrarray0[14] = "N";
            glay.vwstrarray0[16] = "N";
            glay.vwstrarray0[18] = "N";
            glay.vwstrarray0[13] = "N";

            glay.vwstrarray0[15] = "N";
            glay.vwstrarray0[17] = "N";

            glay.vwstrarray0[6] = "A";
            glay.vwblarray0[1] = true;
            glay.vwblarray0[2] = true;
           
            glay.vwstring1 = "F";
           for ( int ctr = 0; ctr < 100; ctr++)
           {

                glay.vwstrarray9[ctr] = "";
           }

           string[] head_dat = new string[20];
           glay.vwlist0 = new List<querylay>[20];

          
        }
   
        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[IV_001_ITEM] where item_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }
           
        private void header_ana()
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

            SelectList[] head_det = new SelectList[20];

           // Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;


            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "008" && bg.sequence_no != 99
                         select bg;

            foreach (var item in bglist.ToList())
            {
                int count2 = item.sequence_no;
                aheader7[count2] = item.mandatory_flag;
                glay.vwstrarray4[count2] = item.header_code;
                var bglist2 = (from bg in db.GB_001_HANAL
                               where bg.header_sequence == item.header_code
                               select bg).FirstOrDefault();

                if (bglist2 != null)
                {
                    glay.vwstrarray5[count2] = bglist2.header_description;
                    var bglist3 = from bg in db.GB_001_DANAL
                                  where bg.header_sequence == item.header_code
                                  select bg;
                    head_det[count2] = new SelectList(bglist3.ToList(), "analysis_code", "analysis_description", glay.vwstrarray6[count2]);

                }

            }

           // Session["head_det"] = head_det;
            //Session["aheader7"] = aheader7;
            psess.sarrayt1 = glay.vwstrarray5;
        }   
    
    }
}
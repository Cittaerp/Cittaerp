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
    public class Gldoc_enqController : Controller
    {
        GL_002_JONAD GL_002_JONAD = new GL_002_JONAD();
        GL_002_JONAL gdoc = new GL_002_JONAL();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
      
        bool err_flag = true;
        string move_auto = "N";
        //
        // GET: /Citta/
        [EncryptionActionAttribute]
        public ActionResult Index()
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            return View(glay);          
        }

        [EncryptionActionAttribute]
        public ActionResult Create()
        {
            util.init_values();

            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay.vwstring0 = "";
            header_rtn();
            read_recordh();
           // read_details();
            return View(glay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(vw_genlay glay_in, string subcheck)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            glay = glay_in;
            header_rtn();
            
            read_recordh();
            //read_details();
            return View(glay);
           
           
        }

        private void header_rtn()
        {
            glay.vwstrarray5 = new string[20];
            string[] aheader5 = new string[20];
            psess.sarrayt1 = aheader5;
            var bglist = from bg in db.GB_001_HEADER
                         where bg.header_type_code == "010" && bg.sequence_no != 99
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

            
        }
        private void select_query()
        {
            var emp = from pf in db.GL_002_JONAL
                      orderby pf.batch_description
                      select pf;
            ViewBag.jrnbr = new SelectList(emp.ToList(), "journal_number", "batch_description", glay.vwstring0);
        }
        private void read_recordh()
        {
            MainContext dbi = new MainContext();

            string str1 = "select col1, col2, col3, col4, col5, col6, col7,col29,col30";
            str1 += "  from  [" + pubsess.userid + "head] where colid="+ util.sqlquote(glay.vwstring0);
            var emp1 = db.Database.SqlQuery<temptab>(str1);
            ViewBag.x1 = emp1.ToList();

            str1 = "select col7, col8, col5, col12, col10, col24, col25, col26, col27, col14, col15, col16, col2";
            str1 += " from  [" + pubsess.userid + "det] where colid=" + util.sqlquote(glay.vwstring0);
             emp1 = dbi.Database.SqlQuery<temptab>(str1);
            ViewBag.x2 = emp1.ToList();
        }
        private void read_details()
        {
            string str1 = "select col1, col2, col3, col4, col6, col7, col8, col9, col10, col11, col12, col13, col14";
            str1+= " from  [" + pubsess.userid + "det] where colid=" + util.sqlquote(glay.vwstring0);
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

        private void read_detail()
        {
           // int jnum = 0;
            //int.TryParse(jrnbr, out jnum);

            string sqltr=" select sum(case amount_type when 'D' then (amount) else 0 end) dquery0, sum(case amount_type when 'C' then (amount) else 0 end) dquery1"; 
            sqltr += " sum (case amount_type when 'D' then (base_amount) else 0 end) dquery2, sum (case amount_type when 'C' then (base_amount) else 0 end) dquery3";
            sqltr += " from [dbo].[GL_002_JONAD] where journal_number = 4";
            var bgs = db.Database.SqlQuery<querylay>(sqltr).FirstOrDefault();

            string stri = "select case analysis_code1 != then  analysis_discription else  set delete_flag ='Y' from GB_001_HANAL a, BK_001_BANK b where header_sequence in (analysis_code1";
            stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
            stri += " and bank_code =" + util.sqlquote(glay.vwstring0);
            db.Database.ExecuteSqlCommand(stri);

            var bglist = from bh in db.GL_002_JONAD
                         join bg in db.MC_001_CUREN
                         on new { a1 = bh.currency } equals new { a1 = bg.currency_code }
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join bk in db.GL_001_CHART
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bk.account_code, a2 = "003" }
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         join bl in db.AR_001_CUSTM
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bl.customer_code, a2 = "001" }
                         into bl1
                         from bl2 in bl1.DefaultIfEmpty()
                         join bm in db.AP_001_VENDR
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bm.vendor_code, a2 = "002" }
                         into bm1
                         from bm2 in bm1.DefaultIfEmpty()
                         join bn in db.BK_001_BANK
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bn.bank_code, a2 = "004" }
                         into bn1
                         from bn2 in bn1.DefaultIfEmpty()
                         join bo in db.GB_001_RSONC
                         on new { a1 = bh.account_code_debit, a2 = bh.account_type_debit } equals new { a1 = bo.consignment_code, a2 = "012" }
                         into bo1
                         from bo2 in bo1.DefaultIfEmpty()
                         join bp in db.GB_001_DANAL
                         on new { a1 = bh.analysis_code1 } equals new { a1 = bp.analysis_code }
                         into bp1
                         from bp2 in bp1.DefaultIfEmpty()
                         join bq in db.GB_001_DANAL
                         on new { a1 = bh.analysis_code2 } equals new { a1 = bq.analysis_code }
                         into bq1
                         from bq2 in bq1.DefaultIfEmpty()
                         join br in db.GB_001_DANAL
                         on new { a1 = bh.analysis_code3 } equals new { a1 = br.analysis_code }
                         into br1
                         from br2 in br1.DefaultIfEmpty()
                         where bh.journal_number == 08
                         select new vw_genlay
                         {
                             vwint0 = bh.journal_number,
                             vwint1 = bh.sequence_number,
                             vwstring6 = bh.account_type_debit,
                             vwstring7 = bh.account_type_debit == "003" ? bk2.account_name : bh.account_type_debit == "001" ? bl2.cust_biz_name : bh.account_type_debit == "002" ? bm2.vend_biz_name : bh.account_type_debit == "004" ? bn2.bank_name : bo2.description,
                             vwstring2 = bh.description,
                             vwstring1 = bh.reference_number,
                             vwstring3 = bh.fixed_asset_code,
                             vwstring4 = bh.fa_transaction_type,
                             vwdecimal0 = bgs.dquery0,
                             vwdecimal1 = bgs.dquery1,
                             vwdecimal2 = bgs.dquery2,
                             vwdecimal3 = bgs.dquery3,
                             vwstring5 = bh.amount_type,
                             vwstring0 = bh.account_type_debit == "003" ? bk2.account_code : bh.account_type_debit == "001" ? bl2.customer_code : bh.account_type_debit == "002" ? bm2.vendor_code : bh.account_type_debit == "004" ? bn2.bank_code : bo2.consignment_code,
                             vwstring10 = bg2.currency_description,
                             vwdecimal4 = bh.exchange_rate,
                             vwstring8 = bp2.analysis_description,
                             vwstring9 = bq2.analysis_description,
                             vwstring11 = br2.analysis_description,
                         };
            ViewBag.x2 = bglist.ToList();
            // var check = bglist.ToList();   

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
           
        //private void header_ana()
        //{
        //    glay.vwstrarray4 = new string[20];
        //    glay.vwstrarray5 = new string[20];
        //    string[] aheader7 = new string[20];
        //    string[] aheader5 = new string[20];

        //    SelectList[] head_det = new SelectList[20];

        //   // Session["head_det"] = head_det;
        //    //Session["aheader7"] = aheader7;
        //    psess.sarrayt1 = aheader5;


        //    var bglist = from bg in db.GB_001_HEADER
        //                 where bg.header_type_code == "010" && bg.sequence_no != 99
        //                 select bg;

        //    foreach (var item in bglist.ToList())
        //    {
        //        int count2 = item.sequence_no;
        //        aheader7[count2] = item.mandatory_flag;
        //        glay.vwstrarray4[count2] = item.header_code;
        //        var bglist2 = (from bg in db.GB_001_HANAL
        //                       where bg.header_sequence == item.header_code
        //                       select bg).FirstOrDefault();

        //        if (bglist2 != null)
        //        {
        //            glay.vwstrarray5[count2] = bglist2.header_description;
        //            var bglist3 = from bg in db.GB_001_DANAL
        //                          where bg.header_sequence == item.header_code
        //                          select bg;
        //            head_det[count2] = new SelectList(bglist3.ToList(), "analysis_code", "analysis_description", glay.vwstrarray6[count2]);

        //        }

        //    }

        //   // Session["head_det"] = head_det;
        //    //Session["aheader7"] = aheader7;
        //    psess.sarrayt1 = glay.vwstrarray5;
        //}   
    
    }
}
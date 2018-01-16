using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
//using CittaErp.Filters;
using CittaErp.utilities;
using CittaErp.ReportA;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mime;
using System.Net.Mail;

namespace CittaErp.Controllers
{
    public class RepScrnController : Controller
    {
        //
        // GET: /RepScrn/
        private MainContext db = new MainContext();
        private MainContext db2 = new MainContext();
        vw_collect mcollect = new vw_collect();
        cittautil utils = new cittautil();
        pubsess pblock;
        string push1 = ""; string push2 = ""; string push3 = ""; string push4 = "";
        string sp = new string(' ', 100);
        string idgroup = "";
        string selectrows;
        psess psess;
        string idstr1 = "";
        string advice_ind = "N";
        decimal[] d1; int bctr;
        bool pay_flag = false;
        string proc_period;
        string viewflag = "1";
        string summarytop = "D";

        //string mail_mess; string mail_attach; string mail_subject;
        string periodok; string periodword; string sname;
        string qflag = "0";


//        [EncryptionActionAttribute]
        public ActionResult TransPRep(string pcode=null, string pc = null)
        {
            //if (utils.check_option() == 1 || pc == null)
            //    return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            period_select();
            //mcollect.ws_string0 = pblock.mcode2;
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string4 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string5 = pblock.processing_period.Substring(0, 4);
            psess.sarrayt0[3] = "TRANS";
            string id1 = (from pl in db.tab_calc where pl.para_code == pcode && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
            psess.temp4 = "Customers";
            if (pcode=="31")
                psess.temp4 = "Vendors";

            psess.temp3 = pcode;
            mcollect.tx_string2[0] = pcode;
            customer_query(pcode);
            Session["psess"] = psess;
            return View("TransPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransPRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;

            string pcode = psess.temp3.ToString();
            push1 = mcollect.ws_string6;
            push2 = mcollect.ws_string7;
            reset_dates();
            mcollect.ws_string6 = push1;mcollect.ws_string7 = push2;

            var rep_sel = utils.add_options(mcollect);
            push1 = (pcode + sp).Substring(0, 5) + (mcollect.ws_string1 + sp).Substring(0, 10);
            push1 += mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += mcollect.ws_string5 + utils.zeroprefix(mcollect.ws_string4, 2);
            push1 += mcollect.ws_string6 + mcollect.ws_string7;
            push1 += (mcollect.ws_string8 + sp).Substring(0, 10)+ (mcollect.ws_string9 + sp).Substring(0, 10);
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='TRANS', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            summarytop = mcollect.ws_string4;
            get_reportp(pcode, mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

        public ActionResult InfoPRep(string pcode = null, string pc = null)
        {
            //if (utils.check_option() == 1 || pc == null)
            //    return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            period_select();
            //mcollect.ws_string0 = pblock.mcode2;
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string4 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string5 = pblock.processing_period.Substring(0, 4);
            psess.sarrayt0[3] = "CUST";
            string id1 = (from pl in db.tab_calc where pl.para_code == pcode && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
            psess.temp4 = "Customers";
            if (pcode == "45")
                psess.temp4 = "Vendors";

            psess.temp3 = pcode;
            mcollect.tx_string2[0] = pcode;
            customer_query(pcode);
            Session["psess"] = psess;
            return View("TransPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InfoPRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;

            string pcode = psess.temp3.ToString();
            push1 = mcollect.ws_string6;
            push2 = mcollect.ws_string7;
            reset_dates();
            mcollect.ws_string6 = push1; mcollect.ws_string7 = push2;

            var rep_sel = utils.add_options(mcollect);
            push1 = (pcode + sp).Substring(0, 5) + (mcollect.ws_string1 + sp).Substring(0, 10);
            push1 += mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += mcollect.ws_string5 + utils.zeroprefix(mcollect.ws_string4, 2);
            push1 += mcollect.ws_string6 + mcollect.ws_string7;
            push1 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='CUST', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            summarytop = mcollect.ws_string4;
            get_reportp(pcode, mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

        //        [EncryptionActionAttribute]
        public ActionResult JnrlRep(string pc = null)
        {
            //if (utils.check_option() == 1 || pc == null)
            //    return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            period_select();
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string4 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string5 = pblock.processing_period.Substring(0, 4);

            psess.sarrayt0[3] = "TRANS";
            string id1 = (from pl in db.tab_calc where pl.para_code == "JL" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
            psess.temp4 = "Account Numbers";

            mcollect.tx_string2[0] = "JL";
            customer_query("JL");
            Session["psess"] = psess;
            return View("TransPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JnrlRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;

            push1 = mcollect.ws_string6;
            push2 = mcollect.ws_string7;
            reset_dates();
            mcollect.ws_string6 = push1; mcollect.ws_string7 = push2;

            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10);
            push1 += mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += mcollect.ws_string5 + utils.zeroprefix(mcollect.ws_string4, 2);
            push1 += mcollect.ws_string6 + mcollect.ws_string7;
            push1 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='JNRLREP', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            get_reportp("JL", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult ActlRep(string pcode = null, string pc = null)
        {
            //if (utils.check_option() == 1 || pc == null)
            //    return RedirectToAction("Welcome", "Home");
            
             init_class();
            psess = (psess)Session["psess"];
            period_select();
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            //mcollect.ws_string4 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            //mcollect.ws_string5 = pblock.processing_period.Substring(0, 4);

            psess.sarrayt0[3] = "ALIST";
            string id1 = (from pl in db.tab_calc where pl.para_code == "AL" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
            psess.temp4 = "Account Numbers";

            if (pcode == "AL")
                psess.temp3 = pcode;
            mcollect.tx_string2[0] = pcode;
            customer_query(pcode);
            Session["psess"] = psess;
            return View("StaffPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActlRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;


            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;

            string pcode = psess.temp3.ToString();
            //push1 = mcollect.ws_string6;
            //push2 = mcollect.ws_string7;
            reset_dates();
           // mcollect.ws_string6 = push1;
            //mcollect.ws_string7 = push2;

            var rep_sel = utils.add_options(mcollect);
            push1 = (pcode + sp).Substring(0, 5) + (mcollect.ws_string1 + sp).Substring(0, 10);
            push1 += mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            //push1 += mcollect.ws_string5 + utils.zeroprefix(mcollect.ws_string4, 2);
            //push1 += mcollect.ws_string6 + mcollect.ws_string7;
            push1 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            //var rep_sel = utils.add_options(mcollect);
            //push1 = (mcollect.ws_string1 + sp).Substring(0, 10);
            //push1 += mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            //push1 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);
            //push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            //viewstate();

            string str1 = " execute rep_controller @rep_ind='ACTLIST', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            get_reportp("AL", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult TBRep(string pc = null)
        {
            //if (utils.check_option() == 1 || pc == null)
            //    return RedirectToAction("Welcome", "Home");
            init_class();
            psess = (psess)Session["psess"];
            period_select();
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            //mcollect.ws_string4 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            //mcollect.ws_string5 = pblock.processing_period.Substring(0, 4);

            psess.sarrayt0[3] = "TB";
            psess.temp2 = "1";
            psess.temp1 = "Trial Balance Report";

            mcollect.tx_string2[0] = "16";
            Session["psess"] = psess;
            return View("StaffPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TBRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;

            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10);
            push1 += mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='TBalance', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            get_reportp("16", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult FStateRep(string pc = null)
        {
            //if (utils.check_option() == 1 || pc == null)
            //    return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            period_select();
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);

            psess.sarrayt0[3] = "FS";
            psess.temp2 = "1";
            string id1 = (from pl in db.tab_calc where pl.para_code == "33" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;

            mcollect.tx_string2[0] = "33";
            Session["psess"] = psess;
            return View("StaffPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FStateRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;

            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10);
            push1 += mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='FSTATE', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            get_reportp("33", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult GTransRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");

            init_class();
            psess = (psess)Session["psess"];
            period_select();
            var str2 = (from pl in db.tab_calc where pl.para_code == "P04" && pl.calc_code == pc select pl).FirstOrDefault();
            string id1 = str2.name1;
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string4 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string5 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string6 = "D";
            mcollect.ws_string8 = str2.report_type;
            mcollect.ws_string7 = "Y";
            psess.sarrayt0[3] = "GRAT";
            psess.temp1 = id1;
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "P04";
            Session["psess"] = psess;
            return View("HTransRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GTransRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += mcollect.ws_string5 + utils.zeroprefix(mcollect.ws_string4, 2);
            push1 += mcollect.ws_string6 + mcollect.ws_string7;
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='PTRANS', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            get_reportp("P04", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult JoinerRep(string pc = null, string id1 = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");

            psess = (psess)Session["psess"];
            psess.temp1 = id1;
            psess.temp1 = "Joiners Report";
            init_class();
            period_select();
            mcollect.ws_string1 = pc;
            psess.sarrayt0[3] = "JOIN";
            psess.temp2 = "1";
            Session["psess"] = psess;
            return View("HTransPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinerRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);

            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += mcollect.ws_string5 + utils.zeroprefix(mcollect.ws_string4, 2);
            push1 += (mcollect.ws_string0 + sp).Substring(0, 10);
            if (mcollect.ar_bool0[0])
                push1 += "J";
            else
                push1 += " ";
            if (mcollect.ar_bool0[1])
                push1 += "L";
            else
                push1 += " ";

            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='pay_joiner', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult PayTransRep(string pc = null, string id1 = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            qflag = "2";
            init_class();
            psess.temp1 = id1 == null ? "Transaction Report" : id1;

            period_select();
            //mcollect.ws_string0 = pblock.mcode2;
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);

            var plist = from pl in db.tab_soft
                        where pl.para_code == "DREP1"
                        orderby pl.report_name1
                        select pl;
            ViewBag.ttype = new SelectList(plist.ToList(), "report_name2", "report_name1");
            psess.sarrayt0[3] = "PAYT";
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "16";
            Session["psess"] = psess;
            return View("TransPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayTransRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += (mcollect.ws_string0 + sp).Substring(0, 10) + mcollect.ws_string4;
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='pay_det', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            get_reportp("16", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");
        }

//        [EncryptionActionAttribute]
        public ActionResult AllTransPRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            psess.temp1 = "All Transactions Report";

            period_select();
            //mcollect.ws_string0 = pblock.mcode2;
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string4 = "D";
            mcollect.ws_string5 = "T";
            mcollect.ws_string6 = "A";
            psess.sarrayt0[3] = "ATRAN";
            psess.temp2 = "2";
            mcollect.tx_string2[0] = "16";
            Session["psess"] = psess;
            return View("TransPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllTransPRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += (mcollect.ws_string0 + sp).Substring(0, 10) + mcollect.ws_string4;
            push1 += mcollect.ws_string5;
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='RALL', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            psess.temp2 = "2";
            pay_flag = true;
            get_reportp("16", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult StandPRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            //if (pc == "MSTAT")
            //{
            //    string ancs = Ccheckg.convert_pass2("pc=" + pc);
            //    return RedirectToAction("PayPartRep", "RepScrn", new { anc = ancs });
            //}


            init_class();
            period_select();
            //mcollect.ws_string0 = pblock.mcode2;
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);

            var plist = (from pl in db.tab_calc
                         where pl.para_code == "16" && pl.calc_code == mcollect.ws_string1
                         orderby pl.name1
                         select pl).FirstOrDefault();

            psess.sarrayt0[3] = plist.report_type;
            psess.temp5 = plist.suppress_zero;
            psess.temp1 = plist.name1;
            psess.temp2 = "1";
            if (plist.report_type == "ATRAN")
            {
                psess.sarrayt0[3] = "ATRAN";
                mcollect.ws_string4 = "D";
                mcollect.ws_string5 = "T";
                psess.temp2 = "2";
            }

            string view2 = "TransPRep";
            if (plist.report_type == "ACCT")
            {
                mcollect.ws_string6 = "A";
                mcollect.ws_string7 = "D";
                view2 = "PostRep";
            }
            mcollect.tx_string2[0] = "16";
            Session["psess"] = psess;
            return View(view2, mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StandPRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);

            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += (mcollect.ws_string0 + sp).Substring(0, 10);

            if (psess.sarrayt0[3].ToString() == "SUM")
                push2 = "pay_sum";
            else if (psess.sarrayt0[3].ToString() == "EXCEP")
                push2 = "pay_except";
            else if (psess.sarrayt0[3].ToString() == "VAR")
                push2 = "pay_var";
            else if (psess.sarrayt0[3].ToString() == "PDOC")
                push2 = "pay_doc";
            else if (psess.sarrayt0[3].ToString() == "WKORD")
                push2 = "pay_worker";
            else if (psess.sarrayt0[3].ToString() == "TRANS")
            {
                push2 = "pay_det";
                push1 += (psess.temp5.ToString().Substring(0, 1) + sp).Substring(0, 4);
            }
            else if (psess.sarrayt0[3].ToString() == "ATRAN")
            {
                push2 = "RALL";
                push1 += mcollect.ws_string4 + (mcollect.ws_string5 + sp).Substring(0, 3);
            }

            else if (psess.sarrayt0[3].ToString() == "ACCT")
            {
                push2 = "pay_post";
                push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string3;
                push1 += ("0" + mcollect.ws_string2).Substring(Convert.ToInt16(mcollect.ws_string2.Length) - 1);
                push1 += (mcollect.ws_string4 + sp).Substring(0, 20) + (mcollect.ws_string5 + sp).Substring(0, 20);
                push1 += (mcollect.ws_string0 + sp).Substring(0, 10) + mcollect.ws_string6 + mcollect.ws_string7;
            }

            if (psess.sarrayt0[3].ToString() == "SUM" || psess.sarrayt0[3].ToString() == "EXCEP" || psess.sarrayt0[3].ToString() == "VAR" || psess.sarrayt0[3].ToString() == "PDOC" || psess.sarrayt0[3].ToString() == "WKORD")
                push1 += sp.Substring(0, 4);

            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind=" + utils.pads(push2) + ", @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            get_reportp("16", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

        public ActionResult ErrorRep()
        {
            mcollect.ar_string9 = new string[] { "", "", "", "" };
            mcollect.ar_string9[0] = "0";
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            viewstate();

            string str1 = "execute set_tables 'error','',''," + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            psess.temp2 = "1";
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult PayPartRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            psess.temp1 = "Part Payment Report";
            psess.temp2 = "1";
            pblock = (pubsess)Session["pubsess"];
            init_class();
            part_select();
            mcollect.ws_string1 = pc;
            mcollect.tx_string2[0] = "16";
            Session["psess"] = psess;
            return View(mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayPartRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = mcollect.ws_string2;
            push2 = mcollect.ws_string3;
            if (string.IsNullOrWhiteSpace(push1))
                push1 = "";
            if (push2 == null || push2 == "")
                push2 = push1;

            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 12) + push2;
            push3 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='pay_active', @sel_string= " + utils.padsnt(push3) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            //pay_flag = true;
            get_reportp("16", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");
        }

//        [EncryptionActionAttribute]
        public ActionResult DataEntryRep(string pc = null, string id1 = null)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            psess.temp1 = "Data Entry Report";
            psess.temp2 = "1";
            init_class();
            period_select();
            //mcollect.ws_string0 = pblock.mcode2;
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = pblock.processing_period.Substring(4, 2);
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string4 = mcollect.ws_string2;
            mcollect.ws_string5 = mcollect.ws_string3;
            mcollect.ws_string7 = "A";

            var plist = from pl in db.tab_soft
                        where pl.para_code == "PTRAN1" && pl.report_name5 == "T"
                        orderby pl.report_name1
                        select pl;
            ViewBag.ttype = new SelectList(plist.ToList(), "report_code", "report_name1");
            mcollect.tx_string2[0] = "16";
            Session["psess"] = psess;
            return View(mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DataEntryRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            Session["psess"] = psess;
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += mcollect.ws_string5 + utils.zeroprefix(mcollect.ws_string4, 2);
            push1 += mcollect.ws_string7 + (mcollect.ws_string6 + sp).Substring(0, 7);

            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='pay_trans', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = true;
            get_reportp("16", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");
        }

//        [EncryptionActionAttribute]
        public ActionResult AdviceRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            pblock = (pubsess)Session["pubsess"];
            string id1 = (from pl in db.tab_calc where pl.para_code == "ADV" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
            init_class();

            period_cycle(pc);
            mcollect.ws_string1 = pc;
            mcollect.ws_string2 = Convert.ToInt16(proc_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = proc_period.Substring(0, 4);
            mcollect.tx_string2[0] = "151";
            Session["psess"] = psess;
            return View(mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdviceRep(vw_collect mcollect1)
        {
            psess.temp6= "ADV";
            pblock = (pubsess)Session["pubsess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);

            var plist = (from pl in db.tab_calc
                         where pl.calc_code == mcollect.ws_string1 && pl.para_code == "ADV"
                         select pl).FirstOrDefault();

            push1 += plist.transfer_code;
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='PAYADV', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            advice_ind = "Y";
            if (mcollect.ar_string9[0] == "2")
            {
                psess.temp2 = "3";
                psess.temp7 = ("0" + mcollect.ws_string2).Substring((("" + mcollect.ws_string2).Length) - 1) + "/" + mcollect.ws_string3;
            }

            else
                psess.temp2 = "4";

            get_reportp("151", mcollect.ws_string1);

            return RedirectToAction("coldisp");
        }

//        [EncryptionActionAttribute]
        public ActionResult PayAdvice(string pc = null, string id1 = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            psess.temp1 = id1;
            psess.temp2 = "1";
            init_class();
            mcollect.ws_string1 = pc;
            period_select();
            staff_list();
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            psess.temp1 = "Advice Selection";
            mcollect.tx_string2[0] = "151";
            Session["psess"] = psess;
            return View(mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayAdvice(vw_collect mcollect1, string snumber, string command)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            mcollect = mcollect1;

            string pperiod = mcollect1.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            string payrun = mcollect1.ws_string4;
            string staffno = ""; //utils.staff_query_check(snumber, mcollect.ws_string4);

            viewstate();

            psess.temp6= "ADV";
            if (staffno == "")
                return RedirectToAction("Welcome", "Home");


            push1 = ("XZPAYADV" + sp).Substring(0, 10) + pperiod + payrun;
            push1 = (push1 + sp).Substring(0, 100) + staffno;

            string selcode = "NUMB" + sp;
            string selop = "ANDANDANDANDANDAND";
            string str1 = " execute rep_controller @rep_ind='PAYADV', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.pads(selcode) + ", @sel_op=" + utils.pads(selop) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (mcollect.ar_string9[0] == "2")
            {
                psess.temp2 = "3";
                psess.temp7 = utils.period_convert(pperiod); //   period_convert2(pperiod); //  ("0" + mcollect.ws_string2).Substring((("" + mcollect.ws_string2).Length) - 1) + "/" + mcollect.ws_string3;
            }
            else
                psess.temp2 = "4";

            advice_ind = "Y";
            get_reportp("151", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");
        }

//        [EncryptionActionAttribute]
        public ActionResult ePayAdvice(string pc = null)
        {
            if (pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            pblock = (pubsess)Session["pubsess"];
            mcollect.ws_string1 = pblock.userid;
            payadvice_sel("S");
            Session["psess"] = psess;
            return View("PayAdvice", mcollect);
        }

//        [EncryptionActionAttribute]
        public ActionResult sPayAdvice(string pc = null)
        {
            if (pc == null)
                return RedirectToAction("Welcome", "Home");

            payadvice_sel("Z");
            return View("PayAdvice", mcollect);
        }

//        [EncryptionActionAttribute]
        public ActionResult rPayAdvice(string pc = null)
        {
            if (pc == null)
                return RedirectToAction("Welcome", "Home");

            payadvice_sel("R");
            return View("PayAdvice", mcollect);
        }

//        [EncryptionActionAttribute]
        public ActionResult mPayAdvice(string pc = null)
        {
            if (pc == null)
                return RedirectToAction("Welcome", "Home");

            payadvice_sel("M");
            return View("PayAdvice", mcollect);
        }

        private void payadvice_sel(string type1)
        {

            init_class();
            mcollect.ws_string5 = type1;
            period_select();
         //   ViewBag.update = utils.payment_run_list();
         //   ViewBag.staff_number = utils.staff_query_sel(mcollect.ws_string1, mcollect.ws_string5);
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            psess.temp1 = "Pay Advice";
        }

//        [EncryptionActionAttribute]
        public ActionResult CRFile(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            psess.temp1 = "Create Transfer File";
            init_class();
            period_select();
            crf_query();
            //mcollect.ws_string0 = pblock.mcode2;
            mcollect.ws_string1 = "";
            mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string2 = Convert.ToInt16(mcollect.ws_string2).ToString();
            mcollect.ws_string4 = "";
            mcollect.ws_string5 = "";
            Session["psess"] = psess;
            return View(mcollect);
        }

        [HttpPost]
        public ActionResult CRFile(vw_collect mcollect1)
        {
            psess = (psess)Session["psess"];
            
            pblock = (pubsess)Session["pubsess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 13) + mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
            push1 += (mcollect.ws_string0 + sp).Substring(0, 10);
            push1 += (mcollect.ws_string4 + sp).Substring(0, 10);
            push1 += mcollect.ws_string5;
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;

            string str1 = " execute rep_controller @rep_ind='TRFPROC', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            // and export the file

            string filename = ""; // utils.write_file("TRF", mcollect.ws_string4, pblock.userid);
            Session["psess"] = psess;
            return File(filename, "application/plain", mcollect.ws_string5);

        }


        private void period_select()
        {
            string str2 = "select count_seq t1,period_name c0 from tab_pay_default a, tab_default b, tab_calc c, tab_calend d ";
            str2 += " where b.default_pay_cycle=c.calc_code and c.para_code='UPDATE' and a.payment_cycle=c.transfer_code ";
            str2 += " and a.payment_cycle=d.payment_cycle order by d.count_seq ";
            str2 = "select cast(code_msg as int) t1, name1_msg c0 from GB_999_MSG where type_msg='FYM' ";
            var bglist1 = db.Database.SqlQuery<vw_query>(str2);

            string qmonth = (Convert.ToInt16(pblock.processing_period.Substring(4, 2))).ToString();
            ViewBag.pmonth = new SelectList(bglist1.ToList(), "t1", "c0", qmonth);

            List<SelectListItem> pyears = utils.year_list();
            ViewBag.pyear = new SelectList(pyears.ToList(), "Value", "Text", pblock.processing_period.Substring(0, 4));

        }

        private void period_cycle(string pc)
        {
            string str1 = "select count_seq t1,period_name c0  from tab_calend a , tab_calc b, tab_calc c where b.para_code='UPDATE' and b.transfer_code=a.payment_cycle ";
            str1 += " and b.calc_code=c.transfer_code and  c.para_code='ADV'and c.calc_code=" + utils.pads(pc);
            str1 += " order by a.sequence, a.count_seq";
            var bglist1 = db.Database.SqlQuery<vw_query>(str1);

            str1 = "select processing_period c0 from tab_pay_default a , tab_calc b, tab_calc c where b.para_code='UPDATE' and b.transfer_code=a.payment_cycle ";
            str1 += " and b.calc_code=c.transfer_code and  c.para_code='ADV'and c.calc_code=" + utils.pads(pc);
            var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            proc_period = str11.c0;

            int qmonth = Convert.ToInt16(proc_period.Substring(4, 2));
            ViewBag.pmonth = new SelectList(bglist1.ToList(), "t1", "c0", qmonth);

            List<SelectListItem> pyears = utils.year_list();
            ViewBag.pyear = new SelectList(pyears.ToList(), "Value", "Text", proc_period.Substring(0, 4));

        }

        private void part_select()
        {
            string str1 = "select para_code + allow_code c1,case allowance_code when 'A' then 'Allowance - ' else 'Deduction - ' end + name1 c2 from tab_allow union all ";
            str1 = str1 + " select para_code + daily_code,case code_ind when 'A' then 'Daily Allow - ' else 'Daily Deduct - ' end + name1 from tab_daily union all ";
            str1 = str1 + " select para_code + loan_code,'Loan - ' + name1 from tab_loan union all ";
            str1 = str1 + " select 'ST' + report_code,'Group - ' + report_name1 from tab_soft where para_code='STAT' ";
            var str2 = db.Database.SqlQuery<vw_query>(str1);

            ViewBag.ppart = new SelectList(str2.ToList(), "c1", "c2");

        }

        private void init_class()
        {

            utils.init_values();
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            psess.temp8 = "Based On";
            mcollect.ws_string0 = "";
            mcollect.ar_string0 = new string[10];
            mcollect.ar_string1 = new string[10];
            mcollect.ar_string2 = new string[10];
            mcollect.ar_string3 = new string[10];
            mcollect.ar_string4 = new string[10];
            mcollect.ar_string5 = new string[10];
            mcollect.ar_string9 = new string[10];
            mcollect.tx_string2 = new string[10];
            init_select();
        }

        private void init_select()
        {
            string str1;
            var plist = from pl in db.tab_calc
                        where pl.para_code == "UPDATE"
                        orderby pl.line_spacing
                        select pl;

            //if (pblock.type2 == "S")
            //    str1 = "select calc_code c1, name1 c2 from tab_calc where para_code='UPDATE' order by line_spacing ";
            //else
            //{
            //    str1 = "select calc_code c1, a.name1 c2 from tab_calc a, tab_array b where a.para_code='UPDATE' ";
            //    str1 += " and b.para_code='21K' and b.operand=a.calc_code and b.array_code=" + utils.pads(pblock.ugroup);
            //    str1 += " order by a.line_spacing ";
            //}

            //var str21 = db.Database.SqlQuery<vw_query>(str1);
            //ViewBag.update = new SelectList(str21.ToList(), "c1", "c2");

            if (qflag == "1")
            {
                str1 = " select report_code as c1, report_name1 as c2, '' from tab_soft where para_code ='ADSEL' union all ";
                str1 += " select analy_code,analy0, '' from tab_applicant_analy a where para_code ='ANALY' ";
            }
            else
            {
                str1 = " select report_code as c1, report_name1 as c2, '' from tab_soft where para_code ='DSEL' and report_name5='Y' union all ";
                str1 += " select analy_code,analy0, '' from tab_analy a where para_code ='ANALY' ";
            }

            if (qflag == "2")
                str1 += " union all select report_code, report_name1, '' from tab_soft where para_code='TRAREP' ";

            str1 += " order by 2 ";
            str1 = "select '' c1 , ''c2";
            var str2 = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.selpost = new SelectList(str2.ToList(), "c1", "c2");

            int pperiod = Convert.ToInt32(pblock.processing_period.Substring(4, 2));
            string pyear = pblock.processing_period.Substring(0, 4);

            var plist5 = (from pl5 in db.GB_999_MSG
                          where pl5.type_msg == "FYM"
                          select pl5).FirstOrDefault();

            ViewBag.period_name = plist5.name1_msg;
            ViewBag.period_year = pyear;

            List<SelectListItem> operator1 = new List<SelectListItem>();
            operator1.Add(new SelectListItem { Text = "AND", Value = "AND" });
            operator1.Add(new SelectListItem { Text = "OR", Value = "OR" });
            ViewBag.oper1 = operator1;

        }

        private void customer_query(string pc)
        {
            var plist1 = from pl5 in db.AR_001_CUSTM
                         select pl5;
            ViewBag.customer= new SelectList(plist1.ToList(), "customer_code", "cust_biz_name");
            if (pc=="JL")
            {
                var plist2 = from pl5 in db.GL_001_CHART
                             select pl5;
                ViewBag.customer = new SelectList(plist2.ToList(), "account_code", "account_name");

            }
            if (pc == "AL")
            {
                pblock.entry_mode = "Y";
                var query = from bg in db.GL_001_CHART
                            select new { c1 = bg.account_code, c2 = bg.account_code + "---" + bg.account_name };
                ViewBag.customer = new SelectList(query.ToList(), "c1", "c2");
            }
        }

        [HttpPost]
        public ActionResult DailyList(string idx)
        {
            string str1 = "";
            string sno = "";
            string Code = idx;
            sno = Code.Substring(0, 2);
            Code = Code.Substring(2);

            if (sno == "02")
            {
                str1 = "select pcode c1, pcode + '- ' + pname c2 from vw_user_parameter where close_code='N' and hcode= " + utils.pads(Code);
                str1 += " union all select staff_number, staff_number + ' - ' + staff_name from tab_staff where close_code='0' ";
                str1 += " and 'NUMB'= " + utils.pads(Code);
                str1 += " union all select report_code, report_name1 from tab_soft where para_code= " + utils.pads(Code);
                str1 += " union all select pcode, pcode + '- ' + pname from vw_user_parameter where close_code='N' and hcode in ";
                str1 += " (select report_name7 from tab_soft where para_code='APRDET' and report_code=" + utils.pads(Code) + ")";
                str1 += " union all select pcode, pcode + '- ' + pname from vw_user_parameter where close_code='N' and hcode in (select field12 from tab_train_default where default_code='MEDIC') ";
                str1 += " and 'APDEPT'=" + utils.pads(Code);
                str1 += " union all select pcode, pcode + '- ' + pname from vw_user_parameter where close_code='N' and hcode in (select field9 from tab_train_default where default_code='APPR') ";
                str1 += " and 'APLOC'=" + utils.pads(Code);
                str1 += " union all select cast(count_array as varchar), operand from tab_array where para_code='APRTYPE' and 'APRCYCLE'=" + utils.pads(Code);
                str1 += " union all select ''+cast(count_array as varchar), operand from tab_array where para_code='APPRN' and 'APRSTAT'=" + utils.pads(Code);
                str1 += " union all select '-1','New' where 'APRSTAT'=" + utils.pads(Code);
                str1 += " union all select '99', 'Completed' where 'APRSTAT'=" + utils.pads(Code);
                str1 += " union all select staff_number, staff_number + ' - ' + staff_name from tab_staff where email_address <>'' and " + utils.pads(Code) + " in ('APPRNAME1','APPRNAME2','APPRNAME3','APPRNAME4','APPRNAME5','APPRNAME6')";
                str1 += " union all select pcode, pcode + '- ' + pname from vw_user_parameter where close_code='N' and hcode = (select source1 from tab_array where para_code='EXPENSE2' and array_code=" + utils.pads(Code) + ") ";
                str1 += " order by 1";

                var str2 = db.Database.SqlQuery<vw_query>(str1);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    str2.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
            }

            if (sno == "03")
            {

                str1 = "select 0 t1,processing_period c2 from tab_pay_default , tab_calc where para_code='UPDATE' and ";
                str1 += " transfer_code=payment_cycle and calc_code=" + utils.pads(Code) + " union all ";
                str1 += " select count_seq t1,period_name c2 from tab_calc , tab_calend where para_code='UPDATE' and ";
                str1 += " transfer_code=payment_cycle and calc_code=" + utils.pads(Code) + "  order by 1 ";

                var str2 = db.Database.SqlQuery<vw_query>(str1);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    str2.ToArray(),
                                    "t1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
            }


            return RedirectToAction("Index"); //, null, new { anc = Ccheckg.convert_pass2("pc=") });
        }


        protected override void Dispose(bool disposing)
        {
            psess = (psess)Session["psess"];
            db.Dispose();
            base.Dispose(disposing);
            psess.temp8 = null;
        }

        private void staff_list()
        {
            //ViewBag.staff_number = utils.staff_query_sel("", mcollect.ws_string1);
            ////IQueryable<> bglista;
            //var bglist = from bg in db.tab_staff
            //             orderby bg.staff_name
            //             select new { c1 = bg.staff_number, c2 = bg.surname + ", " + bg.first_name + " " + bg.other_name, c3 = bg.close_code };

            //var bglista = bglist;
            //if (mcollect.ws_string1 == "S")
            //    bglista = bglist.Where(m => m.c1 == pblock.userid);
            //else if (mcollect.ws_string1 == "R")
            //    bglista = bglist.Where(m => m.c3 != "0");
            //else
            //    bglista = bglist.Where(m => m.c3 == "0");

            //ViewBag.staff_number = new SelectList(bglista.ToList(), "c1", "c2");

            //if (mcollect.ws_string1 == "G")
            //{
            //    string strz = " select distinct a.staff_number c1, surname + ', ' + first_name + ' ' + other_name c2 from tab_staff a , vw_pen_basic b where a.staff_number=b.staff_number ";
            //    var bglista1 = db.Database.SqlQuery<vw_query>(strz);
            //    ViewBag.staff_number = new SelectList(bglista1.ToList(), "c1", "c2");
            //}

            //if (mcollect.ws_string1 == "M")
            //{
            //    string strz = " select distinct staff_number c1,surname + ', ' + first_name + ' ' + other_name  c2 from tab_staff where approval_route in (select train_code from tab_train where para_code='H29'and report_name=" + utils.pads(pblock.userid) + ")";
            //    var bglista1 = db.Database.SqlQuery<vw_query>(strz);
            //    ViewBag.staff_number = new SelectList(bglista1.ToList(), "c1", "c2");
            //}

        }

        private string check_staff(string sno, string type1)
        {
            //return utils.staff_query_check(sno, type1);
            return "";

            //var sclose = (from dl in db.tab_staff
            //           where dl.staff_number == sno
            //           select dl.close_code).FirstOrDefault();

            //if (type1 == "R" && sclose == "0")
            //    sno = "";
            //else if (type1 == "S" && sno != pblock.userid)
            //    sno = pblock.userid;

            //if (type1 == "M")
            //{
            //    string strz = " select distinct staff_number c1,surname + ', ' + first_name + ' ' + other_name  c2 from tab_staff where approval_route in (select train_code from tab_train where para_code='H29'and report_name=" + utils.pads(pblock.userid) + ")";
            //    strz += " and staff_number=" + utils.pads(sno);
            //    var bglista1 = db.Database.SqlQuery<vw_query>(strz).FirstOrDefault();
            //    if (bglista1 == null)
            //        sno = "";

            //}

            //return sno;
        }

        private void crf_query()
        {
            var bhlist = from bh in db.tab_calc
                         where bh.transfer_code == "Y"
                         orderby bh.name1
                         select new { c1 = (bh.para_code + "   ").Substring(0, 3) + bh.calc_code, c2 = bh.name1 };

            ViewBag.repname = new SelectList(bhlist.ToList(), "c1", "c2");

            //var bhlist1 = from bh1 in db.tab_layout
            //              where bh1.para_code == "TRF"
            //              orderby bh1.layout_code
            //              select new { c1 = bh1.layout_code };

            //ViewBag.layout = new SelectList(bhlist1.ToList().Distinct(), "c1", "c1");

        }

//        [EncryptionActionAttribute]
        public ActionResult ATransRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            mcollect.ws_string1 = pc;
            qflag = "1";
            init_class();
            idgroup = select_areptype(pc);

            psess.temp1 = idgroup;
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "R04";
            Session["psess"] = psess;
            return View("HTransRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ATransRep(vw_collect mcollect1, Boolean[] bool1)
        {
            string str1;
            psess = (psess)Session["psess"];
            
            pblock = (pubsess)Session["pubsess"];
            mcollect = mcollect1;
            mcollect.ws_string2 = "" + mcollect.ws_string2;
            var rep_sel = utils.add_options(mcollect);
            string rep_type = psess.sarrayt0[3].ToString();

            push1 = mcollect.ws_string2;
            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 100);
            psess.temp9 = "APPL";
            viewstate();

            str1 = " execute rep_controller @rep_ind=" + utils.pads(psess.temp9.ToString()) + ", @sel_string= " + utils.padsnt(push3) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("R04", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");
        }

//        [EncryptionActionAttribute]
        public ActionResult HTransRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            pblock = (pubsess)Session["pubsess"];
            mcollect.ws_string1 = pc;
            init_class();
            mcollect.tx_string2[0] = "H16";
            idgroup = select_reptype(pc);
            mcollect.ws_string1 = pc;
            psess.temp1 = idgroup;
            psess.temp2 = "1";
            mcollect.ws_string7 = "Y";
            string viewname = "HTransRep";

            if (psess.sarrayt0[3].ToString() == "TURNOVER" && psess.sarrayt0[0].ToString() == "Y")
            {
                mcollect.ws_string2 = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
                mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            }

            if (pc == "TIMEREP")
            {
                viewname = "TSheetrep";
                mcollect.ws_string8 = "E";
            }
            Session["psess"] = psess;
            return View(viewname, mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HTransRep(vw_collect mcollect1, Boolean[] bool1)
        {
            string str1;
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            mcollect = mcollect1;
            mcollect.ws_string2 = "" + mcollect.ws_string2;
            var rep_sel = utils.add_options(mcollect);
            string rep_type = psess.sarrayt0[3].ToString();

            if (rep_type == "MANAGE" || rep_type == "SEPA")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 10);
                push3 += (push2 + sp).Substring(0, 10);
            }

            else if (rep_type == "VACP" || rep_type == "MEDIP")
            {
                string t1 = rep_type == "VACP" ? "LEAVE" : "MEDIC";
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string2 + t1 + sp).Substring(0, 20);

            }
            else if (rep_type == "SKILL" || rep_type == "CRVAC")
            {
                push3 = (mcollect.ws_string1 + sp).Substring(0, 30);
                //                push3 += rep_sel.selection_range;
            }
            else if (rep_type == "JOINER")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 8);
                push3 += (push2 + sp).Substring(0, 8);
                if (mcollect.ar_bool0[0])
                    push3 += "J";
                else
                    push3 += " ";
                if (mcollect.ar_bool0[1])
                    push3 += "L";
                else
                    push3 += " ";

                push3 += "  ";
            }
            else if (rep_type == "CONFM")
            {
                push1 = mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 20);
            }
            else if (rep_type == "ACRUA")
            {
                push1 = mcollect.ws_string2;
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 20);
            }
            else if (rep_type == "AWARD")
            {
                push1 = utils.zeroprefix(mcollect.ws_string2, 2) + mcollect.ws_string3;
                push1 += utils.zeroprefix(mcollect.ws_string2, 2) + mcollect.ws_string5;
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 20);
            }
            else if (rep_type == "EVENT")
            {
                push1 = mcollect.ws_decimal0.ToString();
                psess.temp9 = "hr_event";
            }
            else if (rep_type == "VACF" || rep_type == "ANNV")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + push1 + push2;
                push3 += sp.Substring(0, 4);
            }
            else if (rep_type == "VACBAL")
            {
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string2 + mcollect.ws_string6 + mcollect.ws_string7 + sp.Substring(0, 4);
                push3 += (mcollect.ws_string5 + sp).Substring(0, 10);
            }
            else if (rep_type == "MEDBAL")
            {
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string2 + mcollect.ws_string6 + mcollect.ws_string7 + sp.Substring(0, 4);
                push3 += ("MEDIC" + sp).Substring(0, 10);
            }
            else if (rep_type == "TRNTBAL")
            {
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string2 + mcollect.ws_string6 + mcollect.ws_string7 + sp.Substring(0, 4);
                push3 += ("TRAIN" + sp).Substring(0, 10);
            }

            else if (rep_type == "BALAN")
            {
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string2 + mcollect.ws_string6 + mcollect.ws_string7;
                push3 += (psess.sarrayt0[1].ToString() + sp).Substring(0, 10);
            }
            else if (rep_type == "VACTAKEN")
            {
                move_select_array(bool1);
                push1 = mcollect.ws_string4;
                push2 = mcollect.ws_string5;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string2 + mcollect.ws_string3;
                push3 += (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8) + mcollect.ws_string7;
                str1 = "update_public_table @name1='vactaken', @cvalue=" + utils.pads(selectrows.Substring(1)) + ", @p_userid=" + utils.pads(pblock.userid);
                db.Database.ExecuteSqlCommand(str1);

                psess.temp9 = "hr_vac";
            }
            else if (rep_type == "TRNTAKEN")
            {
                push1 = mcollect.ws_string4;
                push2 = mcollect.ws_string5;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string2 + mcollect.ws_string3;
                push3 += (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8) + mcollect.ws_string7;
                if (string.IsNullOrWhiteSpace(mcollect.ws_string8))
                {
                    mcollect.ws_string8 = ((char)32).ToString();
                    mcollect.ws_string9 = ((char)255).ToString();
                }
                push3 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);

                psess.temp9 = "hr_trnt";
            }
            else if (rep_type == "TRNOT")
            {
                //push4 = (from bg in db.tab_train where bg.para_code == "H01" && bg.train_code == mcollect.ws_string2 select bg.course_name).FirstOrDefault();
                psess.temp1 = "No Training for " + push4;

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string2 + sp).Substring(0, 20);

                psess.temp9 = "hr_trnot";
            }
            else if (rep_type == "TRQD")
            {
                //push4 = (from bg in db.tab_staff where bg.staff_number == mcollect.ws_string2 select bg.staff_name).FirstOrDefault();
                psess.temp1 = "Training Required for " + push4;

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string2 + sp).Substring(0, 20);

                psess.temp9 = "hr_trainrq";
            }
            else if (rep_type == "HMOENROLL")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 10);
                push3 += (push2 + sp).Substring(0, 10);
            }
            else if (rep_type == "HMOEXIT")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();
                if (push2 == "99991231")
                {
                    push1 = ""; push2 = "";
                }

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 10);
                push3 += (push2 + sp).Substring(0, 10);
            }
            else if (rep_type == "MEDBILL")
            {
                push1 = mcollect.ws_string4;
                push2 = mcollect.ws_string5;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string2 + mcollect.ws_string3;
                push3 += (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8) + mcollect.ws_string7 + (mcollect.ws_string8 + sp).Substring(0, 10);
                push3 += (mcollect.ws_string9 + sp).Substring(0, 10) + mcollect.ws_string6;
            }
            else if (rep_type == "MEMB" || rep_type == "DUES")
            {
                push1 = "";
                if (mcollect.ws_string3 != null)
                    push1 = mcollect.ws_string3 + utils.zeroprefix(mcollect.ws_string2, 2);
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 6);
                push3 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);

                if (rep_type == "DUES")
                    psess.temp1 = psess.temp1.ToString() + " as at " + utils.period_convert(utils.period_mmyyyy(push1));

            }
            else if (rep_type == "RMAN")
            {
                psess.temp9 = "hr_manrep";
                push3 = sp.Substring(30);
            }
            else if (rep_type == "TURNOVER")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                if (psess.sarrayt0[0].ToString() == "N")
                    reset_dates();
                else
                    push1 = utils.zeroprefix(push1, 2);


                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 10) + (push2 + sp).Substring(0, 10);
            }
            else if (rep_type == "NOTRANS")
            {
                push4 = (from bg in db.tab_type where bg.trans_type == mcollect.ws_string10 select bg.name1).FirstOrDefault();
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();
                psess.temp1 = "No Transactions for " + push4;

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string10 + sp).Substring(0, 10) + push1 + push2;
            }
            else if (rep_type == "MDEP")
            {
                psess.temp9 = "hr_hmdep";
                push1 = mcollect.ws_string8;
                push2 = mcollect.ws_string9;

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 10) + (push2 + sp).Substring(0, 10);
            }
            else if (rep_type == "TRANEXP" || rep_type == "TRANSBASE")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string10 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 8);
                push3 += (push2 + sp).Substring(0, 8);

                push1 = mcollect.ws_string4;
                push2 = mcollect.ws_string5;
                reset_dates();
                push3 += (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8) + (mcollect.ar_string5[0] + sp).Substring(0, 10) + mcollect.ws_string6 + mcollect.ws_string7;
            }
            else if (rep_type == "HREXPBAL")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8) + mcollect.ws_string6 + mcollect.ws_string7;
            }
            else if (rep_type == "AUDIT")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8);
                push3 += (mcollect.ws_string4 + sp).Substring(0, 10) + (mcollect.ws_string5 + sp).Substring(0, 10);
                push3 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);
            }
            else if (rep_type == "HREXDOCP")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 10) + (push2 + sp).Substring(0, 10) + mcollect.ws_string6 + mcollect.ws_string7;
                string repf1 = psess.temp1.ToString();
                if (mcollect.ws_string6 == "L")
                    repf1 += " - Listing";
                psess.temp1 = repf1;

            }
            else if (rep_type == "TIMEREP")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();

                if (string.IsNullOrWhiteSpace(mcollect.ws_string4))
                    mcollect.ws_string4 = "";
                if (string.IsNullOrWhiteSpace(mcollect.ws_string5))
                    mcollect.ws_string5 = mcollect.ws_string4;
                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (utils.date_yyyymmdd(mcollect.ws_string6) + sp).Substring(0, 8);
                push3 += (mcollect.ws_string4 + sp).Substring(0, 10);
                push3 += (mcollect.ws_string5 + sp).Substring(0, 10);
                push3 += (mcollect.ws_string7 + sp).Substring(0, 10);
                push3 += (push1 + sp).Substring(0, 8);
                push3 += (push2 + sp).Substring(0, 8) + mcollect.ws_string8;

                pay_flag = false;

            }

            push3 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;

            if (psess.sarrayt0[2] != null)
            {
                if (psess.sarrayt0[2].ToString() == "1")
                {
                    push3 += " and ( ";
                    push3 += " a.staff_number in (select staff_number from tab_staff where approval_route in (select train_code from tab_train where para_code='H29' and report_name=" + utils.pads(pblock.userid) + ")) ";
                    if (mcollect.ar_bool1[0])
                    {
                        push3 += " or a.staff_number in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name=" + utils.pads(pblock.userid) + ")))) ";
                    }

                    if (mcollect.ar_bool1[1])
                    {
                        push3 += " or a.staff_number in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name=" + utils.pads(pblock.userid) + ")))))) ";
                    }

                    push3 += ")";
                }
            }

            psess.sarrayt0[2] = null;

            if (rep_type == "AWARD")
            {
                str1 = " execute award_rtn @sel_string= " + utils.pads(push3) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
                db.Database.ExecuteSqlCommand(str1);
            }
            else if (rep_type == "EVENT")
            {
                str1 = " execute report_activity @lday=" + push1 + ", @p_userid=" + utils.pads(pblock.userid);
                db.Database.ExecuteSqlCommand(str1);
            }
            viewstate();

            str1 = " execute rep_controller @rep_ind=" + utils.pads(psess.temp9.ToString()) + ", @sel_string= " + utils.padsnt(push3) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("H16", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");
        }


//        [EncryptionActionAttribute]
        public ActionResult supreport(string pc)
        {
            psess = (psess)Session["psess"];
            
            psess.sarrayt0[2] = "1";
            Session["psess"] = psess;
            return RedirectToAction("HTransRep"); //, null, new { anc = Ccheckg.convert_pass2("pc=" + pc) });

        }

//        [EncryptionActionAttribute]
        public ActionResult mgrreport(string pc)
        {
            psess = (psess)Session["psess"];
            
            psess.sarrayt0[2] = "2";
            Session["psess"] = psess;
            return RedirectToAction("HTransRep"); //, null, new { anc = Ccheckg.convert_pass2("pc=" + pc) });
        }


        private void reset_dates()
        {
            if (string.IsNullOrWhiteSpace(push1) && string.IsNullOrWhiteSpace(push2))
            {
                push1 = "01011001";
                push2 = "31129999";
            }

            if (string.IsNullOrWhiteSpace(push1))
                push1 = "01010001";
            if (string.IsNullOrWhiteSpace(push2))
                push2 = push1;

            push1 = utils.date_yyyymmdd(push1);
            push2 = utils.date_yyyymmdd(push2);

        }

        private string select_reptype(string id)
        {
            string repname; string idbase = ""; string repdoc = "";
            period_select();
            psess.sarrayt0[3] = id;
            var glist = (from gl in db.tab_calc where gl.para_code == "H16" && gl.calc_code == id select gl).FirstOrDefault();
            if (glist != null)
            {
                idbase = glist.report_type;
                repname = glist.name1;
                repdoc = glist.suppress_zero;
            }
            else
            {
                repname = idstr1;
            }

            if (id == "MSUM")
            {
                psess.sarrayt0[3] = "MANAGE";
                psess.temp4 = "Start Date";
                psess.temp9 = "hr_orgsum";
            }
            else if (id == "SKILL")
            {
                psess.temp4 = "Effective Date";
                psess.temp9 = "hr_orgsum";
            }
            else if (id == "SEPA")
            {
                psess.temp4 = "Separated Date";
                psess.temp9 = "hr_sepa";
            }
            else if (id == "JOINER")
            {
                psess.temp4 = "Date Range";
                psess.temp9 = "hr_joiner";
            }
            else if (id == "CRVAC")
            {
                psess.temp4 = "Vacation Date";
                psess.temp9 = "hr_cleave";
            }
            else if (id == "MDEP")
            {
                // staff_query();
            }
            else if (id == "AWARD")
            {
                psess.temp4 = "Award Period";
                psess.temp9 = "hr_award";
                mcollect.ws_string2 = DateTime.Now.Month.ToString();
                mcollect.ws_string3 = DateTime.Now.Year.ToString();
                mcollect.ws_string4 = mcollect.ws_string2;
                mcollect.ws_string5 = mcollect.ws_string3;
            }
            else if (id == "CONFM")
            {
                psess.temp4 = "Confirmation Due in ";
                psess.temp9 = "hr_confm";
                mcollect.ws_string2 = DateTime.Now.Month.ToString();
                mcollect.ws_string3 = DateTime.Now.Year.ToString();
            }
            else if (id == "DUES")
            {
                psess.temp4 = "Membership Dues on";
                psess.temp9 = "hr_club";
                mcollect.ws_string2 = DateTime.Now.Month.ToString();
                mcollect.ws_string3 = DateTime.Now.Year.ToString();
                //ViewBag.hos = utils.querylist("H26", "", 2);
            }
            else if (id == "MEMB")
            {
                psess.temp4 = "Clubs/Associations";
                psess.temp9 = "hr_club";
                //ViewBag.hos = utils.querylist("H26", "", 2);
            }
            else if (id == "NOTRANS")
            {
                psess.temp4 = "Range of Date ";
                psess.temp9 = "hr_notrans";
                //ViewBag.type = utils.querylist("TYPE", "", 2);
            }
            else if (id == "ACRUA")
            {
                psess.temp4 = "Year ";
                psess.temp9 = "hr_acrua";
                mcollect.ws_string2 = DateTime.Now.Year.ToString();
            }
            else if (id == "VACP")
            {
                psess.temp4 = " Vacation Year ";
                psess.temp9 = "hr_comm";
                mcollect.ws_string2 = DateTime.Now.Year.ToString();
            }
            else if (id == "VACF")
            {
                psess.temp4 = " Effective Date ";
                psess.temp9 = "hr_vacdue";
            }
            else if (id == "MEDIP")
            {
                psess.temp4 = "Medical Year ";
                psess.temp9 = "hr_comm";
                mcollect.ws_string2 = DateTime.Now.Year.ToString();
            }
            else if (id == "HREXPBAL")
            {
                psess.temp4 = "Transaction Date";
                psess.temp9 = "hr_expbal";
            }
            else if (id == "HREXDOCP")
            {
                psess.temp4 = "Transaction Date";
                psess.temp9 = "hr_exdocp";
            }
            else if (id == "TIMEREP")
            {
                psess.temp4 = "Transaction Date";
                psess.temp9 = "hr_tsheet";
            }
            else if (idbase == "BALAN")
            {
                string str3 = "select train_code c1, report_name c2, md_name c3 from tab_train where para_code='H24' and train_code= " + utils.pads(repdoc);
                var str4 = db.Database.SqlQuery<vw_query>(str3).FirstOrDefault();
                psess.temp4 = str4.c2 + " Year ";
                psess.temp9 = "hr_vbal";
                mcollect.ws_string2 = DateTime.Now.Year.ToString();
                psess.sarrayt0[3] = idbase;
                mcollect.ws_string7 = "Y";
                psess.sarrayt0[1] = repdoc;
            }
            else if (idbase == "VACTAKEN")
            {
                mcollect.ws_string7 = "Y";
                psess.sarrayt0[3] = idbase;
                psess.temp4 = "Vacation Year ";
                psess.temp9 = "hr_vac";
                string str3 = "select train_code c1, course_name c2,1 t1 from tab_train where para_code='H02' order by default_cost,course_name ";
                var str4 = db.Database.SqlQuery<vw_query>(str3).ToList();
                ViewBag.VacType = str4;
                psess.sarrayt0[10] = str3;
            }
            else if (idbase == "TRNTAKEN")
            {
                mcollect.ws_string7 = "Y";
                psess.sarrayt0[3] = idbase;
                psess.temp4 = "Training Year ";
                psess.temp9 = "hr_trnt";
                string str3 = "select '' c1,'-----All-----' c2 union all select train_code c1, train_code + ' - ' + course_name c2 from tab_train where para_code='H01' order by 1 ";
                var str4 = db.Database.SqlQuery<vw_query>(str3).ToList();
                ViewBag.trnt_type = new SelectList(str4, "c1", "c2");
            }
            else if (id == "TRNOT")
            {
                psess.temp4 = "Training";
                mcollect.ws_string7 = "Y";
                psess.temp9 = "hr_trnot";
                string str3 = "select train_code c1, train_code + ' - ' + course_name c2 from tab_train where para_code='H01' order by 1 ";
                var str4 = db.Database.SqlQuery<vw_query>(str3).ToList();
                ViewBag.trnt_type = new SelectList(str4, "c1", "c2");
            }
            else if (id == "TRQD")
            {
                psess.temp4 = "Staff Name";
                psess.temp9 = "hr_trainrq";
                string str3 = "select staff_number c1, staff_name c2 from tab_staff where close_code='0' order by 2 ";
                var str4 = db.Database.SqlQuery<vw_query>(str3).ToList();
                ViewBag.trnt_type = new SelectList(str4, "c1", "c2");
            }
            else if (idbase == "HMOENROLL")
            {
                psess.temp4 = " Effective Date";
                psess.temp9 = "hr_hmorep";
                psess.sarrayt0[3] = idbase;
            }
            else if (idbase == "HMOEXIT")
            {
                psess.temp4 = " Separation Date";
                psess.temp9 = "hr_hmoexit";
                psess.sarrayt0[3] = idbase;
            }
            else if (idbase == "MEDBILL")
            {
                psess.temp4 = " Medical Year";
                psess.temp9 = "hr_mbill";
                psess.sarrayt0[3] = idbase;
                ViewBag.hos = utils.querylist("H03", "", 2);
                mcollect.ws_string7 = "Y";
            }
            else if (idbase == "TRANSBASE")
            {
                psess.temp4 = " Start Date";
                psess.temp9 = "hr_hrtran";
                psess.sarrayt0[3] = idbase;
                ViewBag.type = utils.querylist("TYPE", "", 2);
                mcollect.ws_string7 = "Y";
            }
            else if (idbase == "TURNOVER")
            {
                psess.temp4 = " Effective Date";
                psess.temp9 = "hr_turnrep";
                psess.sarrayt0[3] = idbase;
               // psess.sarrayt0[0] = (from bg in db.tab_train_default where bg.default_code == "MANN" select bg.field10).FirstOrDefault();
            }
            else if (idbase == "ANNV")
            {
                psess.temp4 = "Date Range ";
                psess.temp9 = "hr_bday";
                psess.sarrayt0[3] = idbase;
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                mcollect.ws_string2 = startDate.ToString("dd/MM/yyyy");
                mcollect.ws_string3 = endDate.ToString("dd/MM/yyyy");
            }
            else if (idbase == "TRANEXP")
            {
                psess.temp4 = "Date of Transaction";
                psess.temp9 = "hr_expense";
                psess.sarrayt0[3] = idbase;
                //ViewBag.type = utils.querylist("H46", "", 2);
                mcollect.ws_string7 = "Y";
                mcollect.tx_string2[0] = "H47";
            }
            else if (idbase == "AUDIT")
            {
                psess.sarrayt0[3] = idbase;
                if (repdoc == "HTRAN")
                {
                    mcollect.ws_int1 = 1;
                    psess.temp9 = "hr_audit";
                    psess.temp4 = " Value Date";
                    //ViewBag.type = utils.querylist("TYPE", "", 2);
                }
                if (repdoc == "HEXP")
                {
                    mcollect.ws_int1 = 0;
                    psess.temp9 = "hr_audexp";
                    psess.temp4 = "Value Date";
                    psess.sarrayt0[3] = "Document Number";
                 //   ViewBag.type = utils.querylist("H46", "", 2);
                }

            }

            if (!string.IsNullOrWhiteSpace(mcollect.ws_string2) && !(id == "BDATE" || idbase == "ANNV"))
                mcollect.ws_string2 = Convert.ToInt16(mcollect.ws_string2).ToString();

            return repname;
        }


        private void hospital_query(string pcode)
        {
            //var hos = from gl in db.tab_train
            //          where gl.para_code == pcode && gl.parameter_close == "N"
            //          select gl;
            string str1 = "select train_code c1, train_code + ' : ' + course_name c2 from tab_train where para_code=" + utils.pads(pcode) + " and parameter_close='N' order by 2 ";
            var hos = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.hos = new SelectList(hos.ToList(), "c1", "c2");
            //ViewBag.hos = new SelectList(hos.ToList(), "train_code", "course_name");

        }

        private void transaction_query()
        {
            var hos = from gl in db.tab_type
                      orderby gl.name1
                      select new { c1 = gl.trans_type, c2 = gl.name1 };
            ViewBag.type = new SelectList(hos.ToList(), "c1", "c2");
            ViewBag.type = utils.querylist("TYPE", "", 2);

        }

        private void expense_query()
        {
            var hos = from gl in db.tab_calc
                      where gl.para_code == "H46"
                      orderby gl.name1
                      select new { c1 = gl.calc_code, c2 = gl.name1 };
            ViewBag.type = new SelectList(hos.ToList(), "c1", "c2");

        }

        private void crmfleet_query()
        {
            var hos = from gl in db.tab_calc
                      where gl.para_code == "F12"
                      orderby gl.name1
                      select new { c1 = gl.calc_code, c2 = gl.name1 };
            ViewBag.type = new SelectList(hos.ToList(), "c1", "c2");

        }

        private void staff_query()
        {
            //var hos = from gl in db.tab_staff
            //          where gl.close_code=="0"
            //          orderby gl.staff_number
            //          select gl;

            //ViewBag.staff = new SelectList(hos.ToList(), "staff_number", "staff_name");
            string str1 = "select staff_number c1, staff_number + ' : ' + staff_name c2 from tab_staff where close_code='0' order by 2 ";
            var hos = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.staff = new SelectList(hos.ToList(), "c1", "c2");

        }

        private void calc_query(string pcode)
        {
            var hos = from gl in db.tab_calc
                      where gl.para_code == pcode
                      orderby gl.name1
                      select gl;

            ViewBag.report = new SelectList(hos.ToList(), "calc_code", "name1");

        }

        private string select_areptype(string id)
        {
            string repname = ""; string idbase = ""; string repdoc = "";

            psess.sarrayt0[3] = "R04";
            var glist = (from gl in db.tab_calc where gl.para_code == "R04" && gl.calc_code == id select gl).FirstOrDefault();
            if (glist != null)
            {
                idbase = glist.report_type;
                repname = glist.name1;
                repdoc = glist.suppress_zero;
            }

            string str3 = "select train_code c1, course_name c2,1 t1 from tab_train where para_code='R05' order by 2 ";
            var str4 = db.Database.SqlQuery<vw_query>(str3);
            ViewBag.basket = new SelectList(str4.ToList(), "c1", "c2");

            return repname;
        }


//        [EncryptionActionAttribute]
        public ActionResult HRStaffURep(string pc = null, string id1 = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            id1 = (from hg in db.tab_calc where hg.para_code == "H13" && hg.calc_code == pc select hg.name1).FirstOrDefault();
            psess.temp1 = id1;
            init_class();
            mcollect.ws_string1 = pc;
            psess.temp4 = " Transaction Date";
            psess.sarrayt0[3] = "HRSTAFF";
            mcollect.ws_string7 = "N";
            mcollect.ws_string6 = "D";
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "H13";
            Session["psess"] = psess;
            Session["psess"] = psess;
            return View("HTRansRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRStaffURep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = mcollect.ws_string2;
            push2 = mcollect.ws_string3;
            reset_dates();

            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + push1 + push2;
            push1 += mcollect.ws_string7 + mcollect.ws_string6;
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='HRSTAFF', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("H13", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult HRTransURep(string pc = null, string id1 = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            var bg = (from hg in db.tab_calc where hg.para_code == "H15" && hg.calc_code == pc select hg).FirstOrDefault();
            id1 = bg.name1;
            string repname = bg.report_name;
            string rep_type = repname.Substring(0, 1);
            repname = repname.Substring(1);
            psess.temp6 = repname;
            psess.temp1 = id1;
            psess.temp2 = "1";
            init_class();
            //staff_query();
            mcollect.ws_string1 = pc;
            string datelbl = "";
            if (rep_type == "x")
                datelbl = (from d1 in db.tab_type where d1.trans_type == repname select d1.value_date).FirstOrDefault();
            else
                datelbl = (from d1 in db.tab_array where d1.para_code == "H46" && d1.array_code == repname && d1.operand == "TVDATE" select d1.source1).FirstOrDefault();

            psess.temp4 = datelbl;
            psess.sarrayt0[3] = "HRTRANS";
            mcollect.ws_string7 = "Y";
            mcollect.tx_string2[0] = "H15";
            Session["psess"] = psess;
            return View("HTRansRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRTransURep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = mcollect.ws_string2;
            push2 = mcollect.ws_string3;
            reset_dates();

            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);
            push3 += push1 + push2 + mcollect.ws_string7;
            push1 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='HRTRANS', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("H15", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult HRManplRep(string pc = null, string id1 = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");

            init_class();
            id1 = (from hg in db.tab_calc where hg.para_code == "H30" && hg.calc_code == pc select hg.name1).FirstOrDefault();
            psess.temp1 = id1;
            
            mcollect.ws_string1 = pc;
            psess.temp4 = "";
            psess.sarrayt0[3] = "HRMANPL";
            mcollect.ws_string7 = "Y";
            psess.temp2 = "1";
            Session["psess"] = psess;
            return View("HTRansRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRManplRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10);
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='hr_stat', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult HRCreate(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            psess.temp1 = "Create Auto Transaction";
            //mcollect.ws_string1 = id;
            psess.temp4 = "";
            psess.sarrayt0[3] = "HRCREATE";
            //calc_query("H35");
            ViewBag.report = utils.querylist("H35", "");
            Session["psess"] = psess;
            return View("HTRansRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRCreate(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string2 + sp).Substring(0, 10);
            push1 += (mcollect.ws_string5 + sp).Substring(0, 20);
            push1 = push1 + rep_sel.selection_range;
            viewstate();

            string str1 = " execute create_hr_rtn @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);
            Session["psess"] = psess;
            return RedirectToAction("Welcome", "Home");

        }

        // HRDetail not use again
//        [EncryptionActionAttribute]
        public ActionResult HRDetail(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            psess.temp1 = "Transaction Details";
            
            //mcollect.ws_string1 = id;
            psess.temp4 = "";
            psess.sarrayt0[3] = "HRDETAIL";
            //calc_query("H13");
            ViewBag.type = utils.querylist("H13", "");
            //staff_query();
            mcollect.ws_string7 = "Y";
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "H13";
            Session["psess"] = psess;
            return View("HTRansRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRDetail(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            Session["psess"] = psess;
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = mcollect.ws_string2;
            push2 = mcollect.ws_string3;
            reset_dates();
            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string5 + sp).Substring(0, 10);
            push3 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);
            push3 += push1 + push2 + mcollect.ws_string7;
            push1 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='HRDETAIL', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("H13", mcollect.ws_string1);

            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult GAllTransRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            psess.temp1 = "All Gratuity Transactions Report";
            
            mcollect.ws_string1 = pc;
            psess.sarrayt0[3] = "GATRAN";
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "16";
            Session["psess"] = psess;
            return View("HTransRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GAllTransRep(vw_collect mcollect1)
        {
            psess = (psess)Session["psess"];
            
            pblock = (pubsess)Session["pubsess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 100);
            push1 = push1 + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='GRALL', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("16", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult GAdviceRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            string id1 = (from pl in db.tab_calc where pl.para_code == "PENS" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            
            mcollect.ws_string1 = pc;
            mcollect.ws_string7 = "PEN";
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "161";
            Session["psess"] = psess;
            return View("AdviceRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GAdviceRep(vw_collect mcollect1, string command)
        {
            psess.temp6= "PEN";
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + sp.Substring(0, 6) + "0";

            var plist = (from pl in db.tab_calc
                         where pl.calc_code == mcollect.ws_string1 && pl.para_code == "PEN"
                         select pl).FirstOrDefault();

            push1 += plist.transfer_code;
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='PENADV', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (viewflag == "2")
            {
                psess.temp2 = "3";
                psess.temp7 = ("0" + mcollect.ws_string2).Substring((("" + mcollect.ws_string2).Length) - 1) + "/" + mcollect.ws_string3;
            }

            advice_ind = "Y";
            get_reportp("161", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");
        }

        public ActionResult testgraph()
        {
            psess.temp2 = "7";
            psess.sarrayt0[4] = "1";
            return RedirectToAction("coldisp");
        }

        public ActionResult coldisp()
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            string repname = Url.Action("View1");

            if (psess.sarrayt0[4] == null) 
            psess.sarrayt0[4] = "0";
            psess.sarrayt0[5] = "print/zx" + pblock.userid + ".rdf";
            var me = psess.sarrayt0[4];
            Session["psess"] = psess;
            int vflag = Convert.ToInt16(psess.sarrayt0[4].ToString());
            if (vflag != 0)
            {
                if (psess.temp2.ToString() == "3")
                {
                    //report_mail();
                    return RedirectToAction("Welcome", "Home");
                }
                else if (psess.temp2.ToString() == "4")
                {
                    psess.sarrayt0[6] = "select  * from [" + pblock.userid + "st01] ";
                    psess.temp7 = "";
                    psess.sarrayt0[7] = "";
                    Sreport2 report = new Sreport2();
                    report.Run();
                    report.Document.Dispose();
                    report.Dispose();
                    report = null;
                }
                else if (psess.temp2.ToString() == "5")
                {
                    psess.sarrayt0[6] = "select  * from [" + pblock.userid + "st01] ";
                    Sreport5 report = new Sreport5();
                    report.Run();
                    report.Document.Dispose();
                    report.Dispose();
                    report = null;
                }
                else if (psess.temp2.ToString() == "6")
                {
                    psess.sarrayt0[6] = "select  * from [" + pblock.userid + "st01] ";
                    Sreport4 report = new Sreport4();
                    report.Run();
                    report.Document.Dispose();
                    report.Dispose();
                    report = null;
                }
                else if (psess.temp2.ToString() == "7")
                {
                    Sreport6 report = new Sreport6();
                    report.Run();
                    report.Document.Dispose();
                    report.Dispose();
                    report = null;
                }
                else
                {
                    Sreport1 report = new Sreport1();
                    report.Run();
                    report.Document.Dispose();
                    report.Dispose();
                    report = null;
                }
                psess.sarrayt0[8] = null;
                psess.sarrayt0[9] = repname;

                string str1 = " exec delete_temp_tables @p_userid=" + utils.pads(pblock.userid);
                db.Database.ExecuteSqlCommand(str1);

            }

            Session["psess"] = psess;
            return View();
        }

        public ActionResult View1()
        {
            return View();
        }

        private void report_mail()
        {
            string msubj; string mmessg; string mattach;
            string error_msg = "";
            utilities.Cmail cm = new Cmail();
            string filelocation = "";

            //var bgl = (from bg in db.tab_train_default where bg.default_code == "LOGOS" select bg).FirstOrDefault();
            string mail_mess = "";// bgl.field14;
            string mail_attach = "";// bgl.field15;
            string mail_subject = "";// bgl.field12;

            periodok = psess.temp7.ToString();

            string str1 = "select snumber c0, select2 c1,select4 c2 from [" + pblock.userid + "tns] where select2 <> '' ";
            var str21 = db.Database.SqlQuery<vw_query>(str1);

            foreach (var item in str21)
            {
                str1 = "select staff_name c0, a.period_name c1 from tab_calend a, tab_staff b, tab_grade c ";
                str1 += " where a.payment_cycle=c.payment_cycle and b.category=c.grade_code and c.para_code='01' and staff_number=" + utils.pads(item.c0);
                str1 += " and count_seq=" + periodok.Substring(0, 2);
                var bg2 = db2.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                periodword = bg2.c1 + ", " + periodok.Substring(3, 4);
                sname = bg2.c0;

                msubj = "";// report_mail_convert(mail_subject);
                mmessg = "";//  report_mail_convert(mail_mess);
                mattach = "";// report_mail_convert(mail_attach);

                if (!string.IsNullOrWhiteSpace(mattach))
                {
                    filelocation = Path.Combine(Server.MapPath("~/print/"), mattach + ".pdf");
                    if (System.IO.File.Exists(filelocation))
                        System.IO.File.Delete(filelocation);
                }

                psess.sarrayt0[6] = "select  * from [" + pblock.userid + "st01] where snumber='" + item.c0 + "' ";
                psess.temp7 = filelocation;
                psess.sarrayt0[7] = "rep";
                Sreport2 report = new Sreport2();
                report.Run();
                report.Document.Dispose();
                report.Dispose();
                report = null;


                cm.single_mail(item.c1, msubj, mmessg, filelocation);
                try
                {
                    System.IO.File.Delete(filelocation);
                }
                catch (Exception err)
                {
                    error_msg = err.Message;
                }

            }

        }

        private string report_mail_convert(string mconv)
        {
            string msubj = mconv;
            msubj = msubj.Replace("[PERIOD]", periodok);
            msubj = msubj.Replace("[PERIODW]", periodword);
            msubj = msubj.Replace("[SNAME]", sname);
            return msubj;
        }

        [HttpPost]
        public ActionResult DailyList1()
        {
            string str1 = "";
            pblock = (pubsess)Session["pubsess"];
            double all_ctr = 1;

            List<SelectListItem> alrow = new List<SelectListItem>();

            str1 = "<table class='display dataprintv ' ><thead class=' color-blue'>"; // + colhead;
            alrow.Add(new SelectListItem { Text = str1, Value = "0" });

            str1 = "select report_column c1, row_seq_id t1 from [" + pblock.userid + "sta01] order by row_seq_id ";
            var str3 = db.Database.SqlQuery<vw_query>(str1);
            var str4 = str3.ToList();
            foreach (var item in str4)
            {
                str1 = "<tr class='text-nowrap '>" + item.c1 + "</tr>";
                if (item.t1 == 0)
                    str1 += "</thead><tbody>";
                alrow.Add(new SelectListItem { Text = str1, Value = all_ctr.ToString() });
                all_ctr++;
            }

            alrow.Add(new SelectListItem { Text = "</tbody></table>", Value = "999999" });

            if (HttpContext.Request.IsAjaxRequest())
            {
                var jsonresult = Json(alrow
                               , JsonRequestBehavior.AllowGet);
                jsonresult.MaxJsonLength = int.MaxValue;
                return jsonresult;
            }

            return RedirectToAction("Index");   //, null, new { anc = Ccheckg.convert_pass2("pc=") });
        }

        [HttpPost]
        public ActionResult DailyList2(string idx)
        {
            string str1 = "";
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            List<SelectListItem> alrow = new List<SelectListItem>();

            str1 = "select doc_type c0 from tab_document where doc_code=" + utils.pads(idx);
            var bglist = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            string doc1 = "";
            if (bglist != null)
                doc1 = bglist.c0;

            alrow.Add(new SelectListItem { Text = doc1, Value = doc1 });
            str1 = "select " + utils.pads(doc1) + " c1," + utils.pads(doc1) + " c2, '0' c3 ";
            if (doc1 == "HT")
                str1 += " union all select 'H'+trans_type c1 , name1 c2, '1' from tab_type  ";
            else if (doc1 == "T1")
                str1 += " union all select 'P'+report_code, report_name1,'1' from tab_soft where para_code = 'PTRAN' ";
            else if (doc1 == "HE")
                str1 += " union all select 'X'+calc_code, name1,'1' from tab_calc where para_code = 'H46' ";
            else if (doc1 == "FT")
                str1 += " union all select 'I'+calc_code, name1,'1' from tab_calc where para_code = 'F12' ";

            str1 += "  union all select '','','0' order by 3,2 ";
            var str2 = db.Database.SqlQuery<vw_query>(str1);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                str2.ToArray(),
                                "c1",
                                "c2")
                           , JsonRequestBehavior.AllowGet);
            Session["psess"] = psess;
            return RedirectToAction("Index");  //, null, new { anc = Ccheckg.convert_pass2("pc=") });
        }

        private void move_select_array(Boolean[] bool1)
        {
            int bctr0 = 0;
            bctr = bool1.Length;
            d1 = new decimal[bctr];
            int bctr2 = 0;
            selectrows = "";
            string str3 = psess.sarrayt0[10].ToString();
            var str4 = db.Database.SqlQuery<vw_query>(str3);

            //            while (bctr0 < bctr)
            foreach (var item in str4.ToList())
            {
                if (bool1[bctr0])
                {
                    d1[bctr2] = 1;
                    bctr0 = bctr0 + 2;
                    selectrows += "," + utils.pads(item.c1);
                }
                else
                    bctr0++;

                bctr2++;
            }

            selectrows += " ";

        }

        //private void report_hrtranp(string rep_type)
        //{
        //    string str2 = "";
        //    push1 = mcollect.ws_string2;
        //    push2 = mcollect.ws_string3;
        //    reset_dates();
        //    if (push2 != "99991231")
        //        str2 += " and a.value_date between " + utils.pads(push1) + " and " + utils.pads(push2);
        //    push1 = mcollect.ws_string4;
        //    push2 = mcollect.ws_string5;
        //    reset_dates();
        //    if (push2 != "99991231")
        //        str2 += " and convert(varchar(08),a.date_approve,112) between " + utils.pads(push1) + " and " + utils.pads(push2);

        //    if (mcollect.ws_string7 == "N")
        //        str2 += " and b.close_code = '0' ";
        //    else if (mcollect.ws_string7 == "Z")
        //        str2 += " and b.close_code <> '0' ";

        //    if (rep_type == "HREP")
        //        str2 += " and a.processed =99 ";
        //    else
        //    {
        //        if (mcollect.ws_string6 == "U")
        //            str2 += " and a.processed <> 99 ";
        //        else if (mcollect.ws_string6 == "A")
        //            str2 += " and a.processed =99 ";
        //    }

        //    if (!string.IsNullOrWhiteSpace(mcollect.ws_string8))
        //    {
        //        if (string.IsNullOrWhiteSpace(mcollect.ws_string9))
        //            mcollect.ws_string9 = mcollect.ws_string8;

        //        str2 += " and a.staff_number between " + utils.pads(mcollect.ws_string8) + " and " + utils.pads(mcollect.ws_string9);
        //    }


        //    if (mcollect.ws_string10 == "REIMBURSE" || mcollect.ws_string10 == "ADVANCE" || mcollect.ws_string10 == "DOCPOST" || mcollect.ws_string10 == "SETTLE")
        //    {
        //        string str1 = "execute hrdoc_rep @rep_type= " + utils.pads(rep_type) + ", @doctype=" + utils.pads(mcollect.ws_string10) + ",@Lselection=" + utils.pads(str2);
        //        str1 += ", @p_userid=" + utils.pads(pblock.userid);
        //        db.Database.ExecuteSqlCommand(str1);

        //        psess.temp1 = "Document Report";
        //    }
        //    else
        //    {
        //        push1 = pblock.userid + "st01";
        //        string str1 = "execute col_table_create " + utils.pads(push1);
        //        db.Database.ExecuteSqlCommand(str1);

        //        string gtr = utils.check_1a(mcollect.ws_string10);
        //        str1 = " , '' column22, a.request_user column23, a.input_date column28, a.approval_user column29, a.date_approve column30 from ";
        //        gtr = gtr.Replace("from", str1);

        //        gtr += str2;

        //        var bl = from bg in db.tab_type2
        //                 where bg.trans_type == mcollect.ws_string10
        //                 orderby bg.sequence_no
        //                 select bg;

        //        str1 = "select max(sequence_no) t1 from tab_type2 where trans_type=" + utils.pads(mcollect.ws_string10);
        //        var str21 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

        //        str2 = "";
        //        string str3 = "";
        //        int maxcount = str21.t1;
        //        for (int ctr = 1; ctr <= maxcount; ctr++)
        //        {
        //            str2 += ",column" + ctr.ToString();
        //            str3 += ",column" + ((ctr + 3).ToString());
        //        }

        //        if (rep_type != "HREP")
        //        {
        //            str2 += ", column23, column28, column29, column30";
        //            str3 += ", column" + (maxcount + 4).ToString() + ",column" + (maxcount + 5).ToString() + ",column" + (maxcount + 6).ToString() + ",column" + (maxcount + 7).ToString();
        //        }

        //        str2 = "insert into [" + pblock.userid + "st01] (snumber,column1,column2,column3" + str3 + ") select snumber,snumber xy,column21,column27" + str2;
        //        str2 += " from (" + gtr + ") bv  order by column26,column25";
        //        db.Database.ExecuteSqlCommand(str2);

        //        var ghlist = (from gh in db.tab_type where gh.trans_type == mcollect.ws_string10 select gh.name1).FirstOrDefault();
        //        psess.temp1 = ghlist;
        //    }
        //}


//        [EncryptionActionAttribute]
        public ActionResult PFStaffRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
                string id1 = (from pl in db.tab_calc where pl.para_code == "HA07" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.sarrayt0[3] = "PFSTAFF";
            pfquery();
            mcollect.ws_string1 = pc;
            mcollect.ws_string7 = "N";
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "HA07";
            Session["psess"] = psess;
            return View("StaffPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PFStaffRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string2 + sp).Substring(0, 6);
            push1 += mcollect.ws_string7;
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='PFSTAFF', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("HA07", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult PFStandRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            var bglist = (from pl in db.tab_calc where pl.para_code == "HA08" && pl.calc_code == pc select pl).FirstOrDefault();

            psess.temp1 = bglist.name1;
            psess.sarrayt0[3] = "PFSTAND";
            psess.temp2 = "1";
            psess.sarrayt0[11] = bglist.report_type;
            pfquery();
            mcollect.ws_string1 = pc;
            mcollect.ws_string7 = "N";
            mcollect.tx_string2[0] = "HA08";
            Session["psess"] = psess;
            return View("StaffPRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PFStandRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
           
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string1 + sp).Substring(0, 10);
            if (string.IsNullOrWhiteSpace(mcollect.ws_string3))
                mcollect.ws_string3 = mcollect.ws_string2;
            push1 += (mcollect.ws_string2 + sp).Substring(0, 6);
            push1 += (mcollect.ws_string3 + sp).Substring(0, 14);
            utils.pub_update("", "reporttype");
            if (psess.sarrayt0[11].ToString() == "ACTY")
            {
                push1 = (mcollect.ws_string1 + sp).Substring(0, 10);
                push1 += (mcollect.ws_string2 + sp).Substring(0, 4);
                push1 += (mcollect.ws_string3 + sp).Substring(0, 4);
                push1 += (mcollect.ws_string4 + sp).Substring(0, 10);
                push1 += (mcollect.ws_string5 + sp).Substring(0, 10);

                psess.temp2 = "6";
                utils.pub_update("acty", "reporttype");
            }

            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            if (psess.sarrayt0[11].ToString() == "DOC")
                psess.temp2 = "5";

            string str1 = " execute rep_controller @rep_ind='apprstat', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("HA08", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

        private void pfquery()
        {
            string str1 = "select distinct cast(current_period_definition as varchar) + cast(current_cycle_definition as varchar) c1, current_period_definition + ' - ' + isnull(b.operand,count_array) c2 from tab_app_definition a ";
            str1 += " left outer join tab_array b on (a.current_cycle_definition=b.count_array and para_code='APRTYPE')  order by 1 ";

            if (psess.sarrayt0[3].ToString() != "PFSTAND")
                str1 += " desc ";

            var str2 = db.Database.SqlQuery<vw_query>(str1).ToList();
            ViewBag.pflist = str2;

            str1 = " select report_code as c1, report_name1 as c2, '' from tab_soft where para_code ='DSEL' and report_name5='Y' union all ";
            str1 += " select analy_code,analy0, '' from tab_analy a where para_code ='ANALY' ";
            str1 += " union all select report_code as c1, report_name1 as c2, '' from tab_soft where para_code ='APRDET' and report_name3='Y'  ";
            str1 += " union all select array_code + cast(count_array as varchar(01)),operand, '' from tab_array a where para_code ='APPRN' and operand<>''  ";

            var str21 = db.Database.SqlQuery<vw_query>(str1).OrderBy(j => j.c2);
            ViewBag.selpost = new SelectList(str21.ToList(), "c1", "c2");

            str1 = "select train_code c1, train_code + ' - ' + course_name c2 from tab_train where para_code='HA09' order by 1 ";
            var str22 = db.Database.SqlQuery<vw_query>(str1).ToList();
            ViewBag.acty = str22;

        }

        private void get_reportp(string pcode, string rcode)
        {
            if (viewflag == "0")
                return;

            int wcount = 0;

            // total 1-5
            // rcount 1-5
            // payroll period
            // report title
            // page 1-5
            // line spacing
            string str_page = "";
            vw_collect mycol = new vw_collect();
            mycol.ar_string0 = new string[] { "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "", "", "N", "N", "N", "N", "N", "1" };
            string str1 = "select count_array n1, operand c1, source1 c2, period c3 from tab_array where array_code=" + utils.pads(rcode) + " and para_code='" + pcode + "S' order by count_array ";
            var str21 = db.Database.SqlQuery<vw_query>(str1);
            foreach (var item in str21)
            {
                if (summarytop == "D" || (summarytop == "S" && wcount > 0))
                {
                    int ctr = Convert.ToInt16(item.n1) - 1;
                    mycol.ar_string0[ctr] = item.c1;
                    mycol.ar_string0[ctr + 5] = item.c3;
                    mycol.ar_string0[ctr + 12] = item.c2;
                }
                wcount++;
            }

            if (pay_flag)
                mycol.ar_string0[10] = ("0" + mcollect.ws_string2).Substring((("" + mcollect.ws_string2).Length) - 1) + "/" + mcollect.ws_string3;

            if (advice_ind == "Y")
            {
                mycol.ar_string0[11] = mcollect.ws_string1;
                mycol.ar_string0[12] = "Y";
                //str_page = " level1p = 'Y' ,";
                str1 = "update [" + pblock.userid + "st01] set level5_name=level4_name, level4_name=level3_name,level3_name=level2_name,level2_name=level1_name,level1_name=snumber ";
                db.Database.ExecuteSqlCommand(str1);
            }

            decimal linep = (from bg in db.tab_calc where bg.para_code == pcode && bg.calc_code == rcode select bg.line_spacing).FirstOrDefault();
            mycol.ar_string0[17] = linep.ToString();

            psess.sarrayt1 = mycol.ar_string0;
            Session["psess"] = psess;
            if (str_page != "")
            {
                str1 = "update [" + pblock.userid + "st01] set " + str_page.Substring(0, str_page.Length - 1);
                // db.Database.ExecuteSqlCommand(str1);
            }
        }


        private void viewstate()
        {
            viewflag = mcollect.ar_string9[0];
            psess.sarrayt0[4] = viewflag;
            string str4 = "execute update_public_table @cvalue=" + utils.pads(viewflag) + ",@name1='displayv',@p_userid=" + utils.pads(pblock.userid);

            db.Database.ExecuteSqlCommand(str4);
        }

//        [EncryptionActionAttribute]
        public ActionResult MailReport(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            
            init_class();
            mcollect.ws_string1 = pc;
            mail_query();

            mcollect.ar_string4[2] = Convert.ToInt16(pblock.processing_period.Substring(4, 2)).ToString();
            mcollect.ar_string4[3] = pblock.processing_period.Substring(0, 4);
            mcollect.ar_string4[4] = mcollect.ar_string4[2];
            mcollect.ar_string4[5] = mcollect.ar_string4[3];
            mcollect.ws_string4 = "N";
            psess.temp1 = "Generation of Mail Merge Documents";
            psess.temp2 = "1";
            Session["psess"] = psess;
            return View(mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MailReport(vw_collect mcollect1)
        {
            string str1;
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;

            push1 = mcollect.ar_string4[0];
            push2 = mcollect.ar_string4[1];
            reset_dates();
            mcollect.ar_string4[0] = push1;
            mcollect.ar_string4[1] = push2;

            var rep_sel = utils.add_options(mcollect);
            push1 = (mcollect.ws_string2 + sp).Substring(0, 10) + (mcollect.ws_string3 + sp).Substring(0, 11);
            push1 += mcollect.ws_string4 + mcollect.ar_string4[0] + mcollect.ar_string4[1] + mcollect.ar_string4[3] + utils.zeroprefix(mcollect.ar_string4[2], 2);
            push1 += mcollect.ar_string4[5] + utils.zeroprefix(mcollect.ar_string4[4], 2);
            push1 = (push1 + sp).Substring(0, 100) + rep_sel.selection_range;

            str1 = " execute rep_controller @rep_ind='MAILR', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "update [" + pblock.userid + "st01doc] set column3=" + utils.pads(mcollect.ar_string4[6]) + ", column4=" + utils.pads(mcollect.ar_string4[7]);
            db.Database.ExecuteSqlCommand(str1);

            document_mail(mcollect.ws_string9);

            return RedirectToAction("Welcome", "Home");
        }

        private void mail_query()
        {

            var blist1 = from bg in db.tab_document
                         orderby bg.name1
                         select new { c1 = bg.doc_code, c2 = bg.name1 };

            ViewBag.document = new SelectList(blist1.ToList(), "c1", "c2", mcollect.ws_string2);


            string str3 = "select 'H'+trans_type c1 , name1 c2, '1' from tab_type  ";
            str3 += "union all select 'P'+report_code, report_name1,'1' from tab_soft where para_code = 'PTRAN' order by 3,2 ";
            var str4 = db.Database.SqlQuery<vw_query>(str3);
            ViewBag.basket = new SelectList(str4.ToList(), "c1", "c2");
            period_select();

        }

        private void document_mail(string docopt)
        {
            if (docopt == "2")
            {
                Cmail mailt = new Cmail();
                mailt.document_mail(pblock.userid);
            }
            else
            {
                string str1 = "select * from [" + pblock.userid + "st01doc] where column1 <> ''";
                var str2 = db.Database.SqlQuery<rep_column>(str1);
                Document doc = new Document(PageSize.A4, 25, 25, 30, 30);

                foreach (var item in str2.ToList())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                        doc.AddTitle(item.column3);
                        doc.AddSubject("Automated Document");
                        doc.AddCreator("AnchorSystems");
                        doc.AddAuthor("AnchorSystems");
                        doc.Open();

                        string str_text = item.column2;
                        using (var srHtml = new StringReader(str_text))
                        {

                            //Parse the HTML
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        }

                        doc.Close();
                        byte[] memdoc = ms.ToArray();
                        ms.Close();

                        string filename = Path.Combine(Server.MapPath("~/print/"), item.column3 + item.snumber + ".pdf");
                        System.IO.File.WriteAllBytes(filename, memdoc);

                    }
                }
            }
        }

        public ActionResult view_lastreport()
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            string repname = "";

            psess.sarrayt0[5] = "print/zx" + pblock.userid + ".rdf";

            repname = Url.Action("View1");
            psess.sarrayt0[9] = repname;
            Session["psess"] = psess;
            return View("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult TexpRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];

            init_class();
            psess.sarrayt0[3] = "HREXPN";
            string id1 = (from pl in db.tab_calc where pl.para_code == "H47" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
           
            mcollect.ws_string1 = pc;
            ViewBag.type = utils.querylist("H46", "", 2);
            psess.temp4 = "Start Date";
            mcollect.tx_string2[0] = "H47";
            Session["psess"] = psess;
            return View("HTransRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TexpRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = mcollect.ws_string2;
            push2 = mcollect.ws_string3;
            reset_dates();

            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8);

            push1 = mcollect.ws_string4;
            push2 = mcollect.ws_string5;
            reset_dates();
            push3 += (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8);
            push3 += (mcollect.ws_string8 + sp).Substring(0, 10) + (mcollect.ws_string9 + sp).Substring(0, 10);
            push3 += (mcollect.ar_string5[0] + sp).Substring(0, 10);
            push3 += mcollect.ws_string6 + mcollect.ws_string7;

            push1 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='HREXPN', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            pay_flag = false;
            get_reportp("H47", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult ChartRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            string id1 = (from hg in db.tab_calc where hg.para_code == "H48" && hg.calc_code == pc select hg.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "7";
            mcollect.ws_string1 = pc;
            string datelbl = (from d1 in db.tab_calc
                              join d2 in db.tab_type
                              on new { c1 = d1.report_name } equals new { c1 = d2.trans_type }
                              into d31
                              from d3 in d31.DefaultIfEmpty()
                              where d1.para_code == "H48" && d1.calc_code == pc
                              select d3.value_date).FirstOrDefault();

            psess.temp4 = string.IsNullOrWhiteSpace(datelbl) ? "Transaction Date" : datelbl;
            psess.sarrayt0[3] = "CHTR";
            mcollect.ws_string7 = "Y";
            mcollect.tx_string2[0] = "H48";
            Session["psess"] = psess;
            return View("HTRansRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChartRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);
            push1 = mcollect.ws_string2;
            push2 = mcollect.ws_string3;
            reset_dates();

            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + push1 + push2 + mcollect.ws_string7;
            push1 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='ChartRep', @sel_string= " + utils.padsnt(push1) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("H48", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult FLTReport(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            mcollect.ws_string1 = pc;
            mcollect.tx_string2[0] = "F14";
            idgroup = select_freptype(pc);
            mcollect.ws_string1 = pc;
            psess.temp1 = idgroup;
            psess.temp2 = "1";
            Session["psess"] = psess;
            return View("HTransRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FLTReport(vw_collect mcollect1, Boolean[] bool1)
        {
            string str1;
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            mcollect.ws_string2 = "" + mcollect.ws_string2;
            var rep_sel = utils.add_options(mcollect);
            string rep_type = psess.sarrayt0[3].ToString();

            if (rep_type == "CRMBASE")
            {
                push1 = mcollect.ws_string2;
                push2 = mcollect.ws_string3;
                reset_dates();

                push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string10 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 8);
                push3 += (push2 + sp).Substring(0, 8);

                push1 = mcollect.ws_string4;
                push2 = mcollect.ws_string5;
                reset_dates();
                push3 += (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8) + (mcollect.ar_string5[0] + sp).Substring(0, 10) + mcollect.ws_string6 + mcollect.ws_string7;
            }


            push3 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;

            if (psess.sarrayt0[2] != null)
            {
                if (psess.sarrayt0[2].ToString() == "1")
                {
                    push3 += " and ( ";
                    push3 += " a.staff_number in (select staff_number from tab_staff where approval_route in (select train_code from tab_train where para_code='H29' and report_name=" + utils.pads(pblock.userid) + ")) ";
                    if (mcollect.ar_bool1[0])
                    {
                        push3 += " or a.staff_number in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name=" + utils.pads(pblock.userid) + ")))) ";
                    }

                    if (mcollect.ar_bool1[1])
                    {
                        push3 += " or a.staff_number in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name in ";
                        push3 += " (select staff_number from tab_staff where approval_route in ";
                        push3 += " (select train_code from tab_train where para_code='H29' and report_name=" + utils.pads(pblock.userid) + ")))))) ";
                    }

                    push3 += ")";
                }
            }

            psess.sarrayt0[2] = null;

            viewstate();

            str1 = " execute rep_controller @rep_ind='crm_fleet', @sel_string= " + utils.padsnt(push3) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            get_reportp("F14", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");
        }

        private string select_freptype(string id)
        {
            string repname = ""; string idbase = ""; string repdoc = "";

            psess.sarrayt0[3] = id;
            var glist = (from gl in db.tab_calc where gl.para_code == "F14" && gl.calc_code == id select gl).FirstOrDefault();
            if (glist != null)
            {
                idbase = glist.report_type;
                repname = glist.name1;
                repdoc = glist.suppress_zero;
            }

            if (idbase == "CRMBASE")
            {
                psess.temp4 = "Date of Incidence";
                psess.temp9 = "crm_fleet";
                psess.sarrayt0[3] = idbase;
                ViewBag.type = utils.querylist("F12", "", 2);
                mcollect.ws_string7 = "Y";
                mcollect.tx_string2[0] = "F14";
            }


            return repname;
        }

//        [EncryptionActionAttribute]
        public ActionResult FLTransRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            init_class();
            mcollect.ws_string1 = pc;
            mcollect.ws_string7 = "Y";
            psess.sarrayt0[3] = "FLTRANS";
            string id1 = (from pl in db.tab_calc where pl.para_code == "F13" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
            psess.temp4 = "Incidence Date";
            mcollect.tx_string2[0] = "F13";
            Session["psess"] = psess;
            return View("HTransRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FLTransRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);

            push1 = mcollect.ws_string2;
            push2 = mcollect.ws_string3;
            reset_dates();

            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8) + mcollect.ws_string7;

            push3 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='FLTRANS', @sel_string= " + utils.padsnt(push3) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            //pay_flag = true;
            get_reportp("F13", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult FLMastRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];
            

            init_class();
            mcollect.ws_string1 = pc;
            mcollect.ws_string7 = "Y";
            psess.sarrayt0[3] = "FLMAST";
            string id1 = (from pl in db.tab_calc where pl.para_code == "F17" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "F17";
            Session["psess"] = psess;
            return View("HTransRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FLMastRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);

            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + mcollect.ws_string7;

            push3 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='FLMAST', @sel_string= " + utils.padsnt(push3) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            //pay_flag = true;
            get_reportp("F17", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

//        [EncryptionActionAttribute]
        public ActionResult FLManRep(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");
            psess = (psess)Session["psess"];

            init_class();
            mcollect.ws_string1 = pc;
            mcollect.ws_string6 = "D";
            psess.sarrayt0[3] = "FLMAN";
//            mcollect.ws_string2 = utils.date_convert(pblock.link_date);
//            mcollect.ws_string3 = utils.date_convert(pblock.link_date);
            string id1 = (from pl in db.tab_calc where pl.para_code == "F18" && pl.calc_code == pc select pl.name1).FirstOrDefault();
            psess.temp1 = id1;
            psess.temp2 = "1";
            mcollect.tx_string2[0] = "F18";
            Session["psess"] = psess;
            return View("HTransRep", mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FLManRep(vw_collect mcollect1)
        {
            pblock = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            mcollect = mcollect1;
            var rep_sel = utils.add_options(mcollect);

            push1 = mcollect.ws_string2;
            push2 = mcollect.ws_string3;
            reset_dates();

            push3 = (mcollect.ws_string1 + sp).Substring(0, 10) + (push1 + sp).Substring(0, 8) + (push2 + sp).Substring(0, 8) + mcollect.ws_string6;

            push3 = (push3 + sp).Substring(0, 100) + rep_sel.selection_range;
            viewstate();

            string str1 = " execute rep_controller @rep_ind='FLMANT', @sel_string= " + utils.padsnt(push3) + ", @sel_select= " + utils.padsnt(rep_sel.selection_code) + ", @sel_op=" + utils.padsnt(rep_sel.selection_operator) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            //pay_flag = true;
            get_reportp("F18", mcollect.ws_string1);
            Session["psess"] = psess;
            return RedirectToAction("coldisp");

        }

       public ActionResult Test()
        {
           //// Sreport7 report = new Sreport7();
           // report.Run();
           // report.Document.Dispose();
           // report.Dispose();
           // report = null;
            return View(viewName: "~/Views/RepScrn/View1");
        }
    }
}

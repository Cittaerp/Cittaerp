using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;


namespace CittaErp.Controllers
{

    public class RequistnController : Controller
    {
        //
        // GET: /Employee/

        AP_002_RQUSN AP_002_RQUSN = new AP_002_RQUSN();
        AP_002_RSQNH gdoc = new AP_002_RSQNH();
        MainContext db = new MainContext();
        MainContext db1 = new MainContext();
        vw_genlay glay = new vw_genlay();
        queryhead qheader = new queryhead();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        discountfsp fsp = new discountfsp();
        HttpPostedFileBase[] photo1;

        string preseq = "";
        string ptype = "";
        decimal f = 0;
        decimal y = 0;
        bool err_flag = true;
        decimal cur_bal = 0; decimal re_level = 0; decimal lpc = 0; int l_time = 0; string lpv = ""; string lpd = ""; string cust_code;
        decimal actual_dis = 0; decimal disc; decimal disc_per1 = 0; string dis_error = ""; decimal item_dis = 0; DateTime expdate;
        string[,] recarray; string sku_name = ""; decimal updated_price = 0;
        string action_flag = "";
        bool submit_flag = false;
        int line_cr = 0;
        public ActionResult Index()
        {

            util.init_values();

            //  Session["curren_name"] = "";

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];

            var bglist = from bh in db.AP_002_RSQNH
                         join bg in db.GB_999_MSG
                         on new { a1 = bh.purchase_transaction_type, a2 = "RTY" } equals new { a1= bg.code_msg, a2=bg.type_msg}
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join gf in db.IV_001_WAREH
                         on new { a1 = bh.warehouse } equals new { a1=gf.warehouse_code}
                         into gf1
                         from gf2 in gf1.DefaultIfEmpty()
                         where bh.approval_level == 0 && bh.created_by== pubsess.userid
                         select new vw_genlay
                         {
                             vwint0 = bh.purchase_sequence_number,
                             vwstring0 = bh.purchase_transaction_type,
                             vwstring2=bg2.name1_msg,
                             vwstring3 = bh.transaction_date,
                             vwstring4 = gf2.warehouse_name,
                             vwstring5=bh.requisition_reference
                         };

            return View(bglist.ToList());


        }
        public ActionResult CreateHeader()
        {
            ViewBag.action_flag = "CreateHeader";
            action_flag = "Createheader";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();

            header_ana("D");
            select_query_head("H");
            glay.vwstring8 = "H";
            glay.vwstrarray0[2] = pubsess.base_currency_code;
            glay.vwbool1 = true;
            //Session["curren_name"] = pubsess.base_currency_description;
            glay.vwstrarray0[6] = pubsess.base_currency_description;
            glay.vwstring4 = pubsess.exchange_editable;

            return View(glay);
        }


        [HttpPost]
        public ActionResult CreateHeader(vw_genlay glay_in, HttpPostedFileBase[] photofile, string subcheck, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            glay = glay_in;
            photo1 = photofile;
            if (subcheck == "RO")
            {
                get_price();
                header_ana("D");
                select_query_head("H");
                ModelState.Remove("subcheck");
                return View(glay);
            }
            if (headtype == "send_app")
            {
                submit_flag = true;

            }
            update_file();

            if (err_flag)
            {
                return RedirectToAction("CreateHeader");
            }

            header_ana("D");
            select_query_head("H");
            glay.vwlist0 = new List<querylay>[20];
           
            glay.vwstring8 = "H";
            //cur_read();
            return View(glay);

        }

        public ActionResult EditHeader(int key1)
        {
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            //psess.temp4 = "";
            //Session["shw_baseamt"] = "N";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            //qheader = (queryhead)Session["qheader"];
            initial_rtn();
            glay.vwstring8 = "H";
            glay.vwstring4 = pubsess.exchange_editable;

            gdoc = db.AP_002_RSQNH.Find(key1);
            if (gdoc != null)
            {
                set_qheader();
                read_header();
                get_price1(key1);
                
            }
            header_ana("D");
            select_query_head("H");

            
            //gdoc = db.AP_002_RSQNH.Find(qheader.intquery0);
            //if (gdoc != null)
            //    read_header();

            //select_query_head("H");

            return View(glay);
        }

        [HttpPost]
        public ActionResult EditHeader(vw_genlay glay_in, HttpPostedFileBase[] photofile, string subcheck,string id_xhrt,string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            glay = glay_in;
            glay.vwstring9 = qheader.intquery0.ToString();

            photo1 = photofile;
            if (subcheck == "RO")
            {
                get_price();
                header_ana("D");
                select_query_head("H");
                ModelState.Remove("subcheck");
                return View(glay);
            }
            if (headtype == "send_app")
            {
                submit_flag = true;

            }
            if (id_xhrt == "D")
            {
                delete_record();
                return RedirectToAction("Index");
            }
            update_file();

            if (err_flag)
            {
                return RedirectToAction("CreateHeader");
            }

            header_ana("D");
            select_query_head("H");
            glay.vwlist0 = new List<querylay>[20];
            
            glay.vwstring8 = "H";
            //cur_read();
            return View(glay);

        }

        public ActionResult CreateDetails(string headtype = "D")
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];


            initial_rtn();
            header_ana("D");
            select_query_head("D");
            glay.vwstring8 = headtype;
            glay.vwstring9 = qheader.intquery0.ToString();
            detail_intial();

            show_screen_info();

            return View(glay);
        }

        [HttpPost]
        public ActionResult CreateDetails(vw_genlay glay_in, string hid_price, string headtype = "D")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";

            glay = glay_in;
            glay.vwstring9 = qheader.intquery0.ToString();
            glay.vwstring7 = qheader.query3;
            preseq = qheader.query3;

            if (headtype == "send_app")
            {
                //validation_routine(headtype);
                if (err_flag)
                {
                    string str = "update AP_002_RSQNH set approval_level=99  where purchase_sequence_number=" + qheader.intquery0;
                    db.Database.ExecuteSqlCommand(str);
                    return RedirectToAction("CreateHeader");
                }

            }
            if (err_flag)
            {
                update_file();

                if (err_flag)
                {
                    return RedirectToAction("CreateDetails");
                }
            }

            header_ana("D");
            detail_intial();
            show_screen_info();
            select_query_head("D");


            return View(glay);

        }

        public ActionResult Edit(int key1)
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];

            gdoc = db.AP_002_RSQNH.Find(key1);
            if (gdoc != null)
            {
                set_qheader();
                return RedirectToAction("CreateDetails", new { headtype = "D" });
            }


            return View(glay);

        }

        public ActionResult EditDetails(int key1, int key2)
        {
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];

            AP_002_RQUSN = db.AP_002_RQUSN.Find(qheader.intquery0, key2, 0);
            if (AP_002_RQUSN == null)
                return View(glay);

            initial_rtn();
            move_detail();
            header_ana("D");
            select_query_head("D");
            glay.vwstring8 = "D";
            detail_intial();

            show_screen_info();

            return View(glay);

        }

        [HttpPost]
        public ActionResult EditDetails(vw_genlay glay_in, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";

            glay = glay_in;
            glay.vwstring9 = qheader.intquery0.ToString();
            glay.vwdecimal4 = Convert.ToDecimal(Session["line_num"]);

            if (headtype == "send_app")
            {
                //validation_routine(headtype);
                if (err_flag)
                {
                    string str = "update AP_002_RSQNH set approval_level=99  where purchase_sequence_number=" + qheader.intquery0;
                    db.Database.ExecuteSqlCommand(str);
                    return RedirectToAction("CreateHeader");

                }

            }
            update_file();

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head("D");

            glay.vwstring8 = "H";
            detail_intial();
            show_screen_info();
            header_ana();

            return View(glay);

        }
        private void delete_record()
        {
            gdoc = db.AP_002_RSQNH.Find(glay.vwint0);
            if (gdoc != null)
            {
                db.AP_002_RSQNH.Remove(gdoc);
                db.SaveChanges();
            }
            string swl = "delete from [dbo].[AP_002_RQUSN] where purchase_sequence_number =" + glay.vwint0 + "";
            int dt = db.Database.ExecuteSqlCommand(swl);
        }
        private void update_file()
        {

            err_flag = true;
            validation_routine();

            if (err_flag)
                update_record();

        }

        private void update_record()
        {
            string cr_flag = action_flag;

            if (cr_flag.IndexOf("Header") > 0)
            {
                if (cr_flag == "CreateHeader")
                {
                    gdoc = new AP_002_RSQNH();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;
                    gdoc.approval_level = 0;
                    gdoc.approval_date = DateTime.UtcNow;

                    var headlist = (from bh in db.AP_001_PUROT
                                    where bh.parameter_code == "PURSEQ" && (bh.order_type == "01" || bh.order_type == "single")
                                    select bh).FirstOrDefault();
                    string pref = headlist.order_prefix;
                    int seq = headlist.order_sequence;
                    int num = headlist.numeric_size;
                    string sqlstr = " update AP_001_PUROT set order_sequence = order_sequence+1 where parameter_code = 'PURSEQ' and (order_type = '01' or order_type = 'single')";
                    db.Database.ExecuteSqlCommand(sqlstr);
                    if (num == 0)
                    {
                        preseq = pref + seq.ToString();
                    }
                    else
                    {
                        string seq1 = ("000000000" + seq.ToString()).Substring(9 - num + seq.ToString().Length, num);
                        preseq = pref + seq1;
                    }
                    glay.vwstring7 = preseq;
                    gdoc.requisition_reference = preseq;
                }
                 else
                {
                    gdoc = db.AP_002_RSQNH.Find(qheader.intquery0);
                }
                //gdoc.status = 01;
                gdoc.purchase_transaction_type = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : (glay.vwstring0);
                gdoc.transaction_date = util.date_yyyymmdd(glay.vwstring2);
                gdoc.warehouse = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : (glay.vwstring1);
                gdoc.note = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : (glay.vwstring3);
           
                gdoc.analysis_code1 = "";
                gdoc.analysis_code2 = "";
                gdoc.analysis_code3 = "";
                gdoc.analysis_code4 = "";
                gdoc.analysis_code5 = "";
                gdoc.analysis_code6 = "";
                gdoc.analysis_code7 = "";
                gdoc.analysis_code8 = "";
                gdoc.analysis_code9 = "";
                gdoc.analysis_code10 = "";
                gdoc.modified_date = DateTime.UtcNow;
                gdoc.modified_by = pubsess.userid;

                if (glay.vwstrarray6 != null)
                {
                    int arrlen = glay.vwstrarray6.Length;
                    if (arrlen > 0)
                        gdoc.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                    if (arrlen > 1)
                        gdoc.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                    if (arrlen > 2)
                        gdoc.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                    if (arrlen > 3)
                        gdoc.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                    if (arrlen > 4)
                        gdoc.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                    if (arrlen > 5)
                        gdoc.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                    if (arrlen > 6)
                        gdoc.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                    if (arrlen > 7)
                        gdoc.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                    if (arrlen > 8)
                        gdoc.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                    if (arrlen > 9)
                        gdoc.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                    psess.intemp0 = arrlen;
                    Session["psess"] = psess;
                }
                if (submit_flag)
                {
                    gdoc.approval_level = 0;
                    util.update_entry("REQ", gdoc.purchase_sequence_number.ToString(), pubsess.userid);
                }

                if (cr_flag == "CreateHeader")
                    db.Entry(gdoc).State = EntityState.Added;
                else
                    db.Entry(gdoc).State = EntityState.Modified;
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

                        set_qheader();
                        util.write_document("PURQSTN", gdoc.purchase_sequence_number.ToString(), photo1, glay.vwstrarray9);
            
                string sqdeb = "delete from AP_002_RQUSN where purchase_sequence_number=" + qheader.intquery0 + "";
                int delctr = db.Database.ExecuteSqlCommand(sqdeb);
                
                for (int i = 0; i < glay.vwstrarray0.Length; i++)
                {
                    glay.vwint2 = get_next_line_sequence();

                    if (!string.IsNullOrWhiteSpace(glay.vwstrarray0[i]))
                    {
                        AP_002_RQUSN AP_002_RQUSN = new AP_002_RQUSN();
                       AP_002_RQUSN.purchase_sequence_number = qheader.intquery0;
                       AP_002_RQUSN.line_sequence = glay.vwint2;
                       AP_002_RQUSN.sub_line_sequence = 0;
                       AP_002_RQUSN.purchase_requisition_sequence = qheader.query3;
                       AP_002_RQUSN.item_code = string.IsNullOrWhiteSpace(glay.vwstrarray0[i]) ? "" : glay.vwstrarray0[i];
                       AP_002_RQUSN.warehouse = "";
                       AP_002_RQUSN.note = "";
                       AP_002_RQUSN.purchase_requition_quantity = glay.vwitarray0[i];
                       AP_002_RQUSN.requisition_date = "";
                       AP_002_RQUSN.created_by = pubsess.userid;
                       AP_002_RQUSN.modified_date = DateTime.UtcNow;
                       AP_002_RQUSN.modified_by = pubsess.userid;
                       AP_002_RQUSN.created_date = DateTime.UtcNow;
                       db.Entry(AP_002_RQUSN).State = EntityState.Added;

                    }
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
                }


                if (err_flag && cr_flag.IndexOf("Header") < 0)
                {
                    //string str = " update AP_002_RSQNH set exchange_rate =" + qheader.dquery0 + " where purchase_transaction_type =" + qheader.intquery0;
                    //db.Database.ExecuteSqlCommand(str);
                    //// call_discount();
                    //qheader.bquery0 = false;
                    //Session["qheader"] = qheader;
                }

              
            }
        }

        private int get_next_line_sequence()
        {
            string sqlstr = "select isnull(max(line_sequence),0) vwint1 from AP_002_RQUSN where line_sequence <> 999 and purchase_requisition_sequence=" + util.sqlquote(qheader.query3);
            var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
            return sql1.vwint1 + 1;

        }

        private void validation_routine()
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];



            DateTime date_chk = Convert.ToDateTime(psess.temp4);

            aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;


            string cr_flag = action_flag;
            DateTime date_chki = DateTime.Now;
            DateTime invaliddate = new DateTime(1000, 01, 01);
  
                if (!util.date_validate(glay.vwstring2))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
                    err_flag = false;
                }

             

                var headlist = (from bh in db.AP_001_PUROT
                                where bh.parameter_code == "PURSEQ" && (bh.order_type == "01" || bh.order_type == "single")
                                select bh).FirstOrDefault();
                if (headlist == null)
                {
                    ModelState.AddModelError(String.Empty, "requisition sequencing number not created");
                    err_flag = false;
                }



                for (int count1 = 0; count1 < 10; count1++)
                {
                    if (aheader7[count1] == "Y" && string.IsNullOrWhiteSpace(glay.vwstrarray6[count1]))
                    {
                        error_msg = aheader5[count1] + " is mandatory. ";
                        ModelState.AddModelError(String.Empty, error_msg);
                        err_flag = false;
                    }
                }


            }

        private void select_query_head(string type1)
        {
            if (type1 == "H")
            {
                year_cal();
                ViewBag.cust = util.para_selectquery("002", glay.vwstrarray0[1]);

                var empz = from pf in db.GB_999_MSG
                           where pf.type_msg == "RTY"
                           orderby pf.code_msg
                           select pf;
                ViewBag.salestt = new SelectList(empz.Distinct().ToList(), "code_msg", "name1_msg", glay.vwstrarray0[0]);

                var del = from jk in db.AR_001_DADRS
                          where jk.address_type == "CU" && jk.active_status == "N"
                          orderby jk.location_alias
                          select jk;
                ViewBag.deladd = new SelectList(del.ToList(), "address_code", "location_alias", glay.vwstrarray0[1]);

                var empe = from pf in db.IV_001_ITEM
                           where pf.active_status == "N" && pf.sales == "Y"
                           orderby pf.item_code, pf.item_name
                           select new { c1 = pf.item_code, c2 = pf.item_code + "--- " + pf.item_name };
                ViewBag.item = new SelectList(empe.ToList(), "c1", "c2", glay.vwstring0);

                string str1 = "select warehouse_code query0, c2 query1 from vw_warehouse_site ";
                var emp1 = db.Database.SqlQuery<querylay>(str1);
                ViewBag.itemw = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring1);

                var emp = from pf in db.FA_001_ASSET
                          where pf.active_status == "N"
                          select pf;
                ViewBag.fix = new SelectList(emp.ToList(), "fixed_asset_code", "reference_asset_code", glay.vwstring2);

            }


            if (type1 == "D")
            {
                // ViewBag.item = util.para_selectquery("008", glay.vwstring0);
                var empe = from pf in db.IV_001_ITEM
                           where pf.active_status == "N" && pf.sales == "Y"
                           orderby pf.item_code, pf.item_name
                           select new { c1 = pf.item_code, c2 = pf.item_code + "--- " + pf.item_name };
                ViewBag.item = new SelectList(empe.ToList(), "c1", "c2", glay.vwstring0);

                string str1 = "select warehouse_code query0, c2 query1 from vw_warehouse_site ";
                var emp1 = db.Database.SqlQuery<querylay>(str1);
                ViewBag.itemw = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring1);

                var emp = from pf in db.FA_001_ASSET
                          where pf.active_status == "N"
                          select pf;
                ViewBag.fix = new SelectList(emp.ToList(), "fixed_asset_code", "reference_asset_code", glay.vwstring2);


            }
        }

        private void select_queryhide(string type1 = "D")
        {
            if (type1 == "D")
            {
                var empe = from pf in db.IV_001_ITEM
                           where pf.active_status == "N"
                           select pf;
                ViewBag.item = new SelectList(empe.ToList(), "item_code", "item_name", glay.vwstring0);

                var emp = from pf in db.FA_001_ASSET
                          where pf.active_status == "N"
                          select pf;
                ViewBag.fix = new SelectList(emp.ToList(), "fixed_asset_code", "reference_asset_code", glay.vwstring1);
            }

        }

        private void initial_rtn()
        {
            pubsess = (pubsess)Session["pubsess"];
            glay.vwdclarray0 = new decimal[20];
            glay.vwdclarray1 = new decimal[20];
            glay.vwdclarray2 = new decimal[20];
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
            glay.vwstrarray2 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray3 = new string[20];
            glay.vwstrarray4 = new string[20];
            glay.vwint1 = 1;
            glay.vwitarray0 = new int[50];
            glay.vwitarray1 = new int[50];

            for (int wctr = 0; wctr < 20; wctr++)
            {
                glay.vwstrarray1[wctr] = "";
                glay.vwstrarray2[wctr] = "";
                glay.vwstrarray3[wctr] = "";
                glay.vwstrarray4[wctr] = "";
                glay.vwstrarray5[wctr] = "";
                glay.vwstrarray0[wctr] = "";
            }
            glay.vwstring2 = DateTime.Now.ToString("dd/MM/yyyy");
            //glay.vwstrarray0[4] = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray7 = new string[20];
            glay.vwstring1 = "N";
            psess.temp1 = pubsess.manual_discount;
            glay.vwlist0 = new List<querylay>[20];
            Session["psess"] = psess;

        }


        private void cur_read()
        {
            string curcode = "";
            curcode = glay.vwstrarray0[2];
            string read_cur = (from bg in db.MC_001_CUREN
                               where bg.currency_code == curcode
                               select bg.currency_description).FirstOrDefault();

            //  Session["curren_name"] = read_cur;
            glay.vwstrarray0[6] = read_cur;
        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AP_002_RSQNH] where cast (purchase_sequence_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "delete from [dbo].[AP_002_RQUSN] where cast(purchase_sequence_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        public ActionResult delete_detail(string id)
        {
            // write your query statement
            qheader = (queryhead)Session["qheader"];
            string sqlstr = "delete from [dbo].[AP_002_RQUSN] where cast(purchase_sequence_number as varchar) +'[]'+ cast(line_sequence as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "select count(0) intquery0 from AP_002_RQUSN where dis_flag='N' and purchase_sequence_number=" + qheader.intquery0.ToString();
            var delctr = db.Database.SqlQuery<querylay>(sqlstr).FirstOrDefault();
            if (delctr.intquery0 == 0)
            {
                sqlstr = "delete from AP_002_RQUSN where purchase_sequence_number=" + qheader.intquery0.ToString();
                db.Database.ExecuteSqlCommand(sqlstr);
                sqlstr = "update AP_002_RSQNH set quote_total_qty=0,quote_total_ext_price=0,quote_total_discount=0, quote_total_inv_tax=0,quote_total_tax=0,quote_total_amount=0 ";
                sqlstr += " where purchase_sequence_number=" + qheader.intquery0.ToString();
                db.Database.ExecuteSqlCommand(sqlstr);

            }
            else
                // call_discount();

               // display_header();
            select_queryhide("D");
            if (HttpContext.Request.IsAjaxRequest())
                return Json(
                            JsonRequestBehavior.AllowGet);
            return RedirectToAction("CreateDetails");

        }

        private void read_details()
        {
            int hbug = qheader.intquery0;
            decimal exc_rt = qheader.dquery1;

            var bglist = from bh in db.AP_002_RQUSN
                         join bg in db.IV_001_ITEM
                         on new { a1 = bh.item_code } equals new { a1 = bg.item_code }
                         into bg1
                         from bg2 in bg1.DefaultIfEmpty()
                         join bf in db.GB_001_PCODE
                         on new { a1 = bg2.sku_sequence, a2 = "10" } equals new { a1 = bf.parameter_code, a2 = bf.parameter_type }
                         into bf1
                         from bf2 in bf1.DefaultIfEmpty()
                         where bh.purchase_sequence_number == hbug
                         orderby bh.line_sequence, bh.sub_line_sequence
                         select new vw_genlay
                         {
                             vwint0 = bh.purchase_sequence_number,
                             vwint1 = bh.line_sequence,
                             vwint2 = bh.sub_line_sequence,
                             vwstring1 = bh.purchase_requisition_sequence,
                             vwstring2 = bg2.item_name,
                             vwstring0 = bh.item_code,
                             vwstring3 = bf2.parameter_name,
                             vwdecimal1 = bh.purchase_requition_quantity,
                         };

            var bgl = bglist.ToList();
            ViewBag.x1 = bgl;
            if (bgl.Count > 0)
            {
                qheader.bquery0 = false;
                Session["qheader"] = qheader;
                glay.vwbool1 = false;
            }


        }

        private void show_screen_info()
        {
           // display_header();
            read_details();

        }

        private void detail_intial()
        {
            glay.vwstring7 = qheader.query3;
            glay.vwstring2 = qheader.query5;
            glay.vwstring3 = qheader.dquery0.ToString();
            preseq = qheader.query3;
            if (pubsess.price_editable == "Y")
                glay.vwbool0 = true;
            glay.vwbool1 = qheader.bquery0;
            glay.vwstring4 = pubsess.price_editable;

        }
        private void read_header()
        {
            string amt_check = "";
            glay.vwstrarray0 = new string[20];
            int hbug = qheader.intquery0;

            var bg2list = (from bh in db.AP_002_RSQNH
                           where bh.purchase_sequence_number == hbug
                           select new { bh }).FirstOrDefault();

            if (bg2list != null)
            {
                glay.vwint0 = bg2list.bh.purchase_sequence_number;
                glay.vwstring2 = util.date_slash(bg2list.bh.transaction_date);
                glay.vwstring0 = bg2list.bh.purchase_transaction_type;
                glay.vwstring1 = bg2list.bh.warehouse;
                string docode = gdoc.purchase_transaction_type.ToString();
                var bglist = from bg in db.GB_001_DOC
                             where bg.screen_code == "RQN" && bg.document_code == docode
                             orderby bg.document_sequence
                             select bg;

                ViewBag.anapict = bglist.ToList();
               
            }
        }

        private void exp_cal()
        {

        }
        //private void display_header()
        //{

        //    //ModelState.Clear();
        //    ModelState.Remove("vwstrarray1[3]");
        //    ModelState.Remove("vwstrarray1[4]");
        //    ModelState.Remove("vwstrarray1[5]");
        //    ModelState.Remove("vwstrarray1[6]");
        //    ModelState.Remove("vwstrarray1[7]");
        //    ModelState.Remove("exchrate");
        //    expdate = DateTime.Now;
        //    int qdate = qheader.intquery0;
        //    var chkdate = (from bg in db.AP_002_RSQNH
        //                   where bg.purchase_sequence_number == qdate
        //                   select new
        //                   {
        //                       c0 = bg.requisition_date,
        //                       c1 = bg.number_of_day
        //                   }).FirstOrDefault();
        //    int numday = chkdate.c1;

        //    string dh1 = chkdate.c0;
        //    DateTime quotedate = new DateTime(Convert.ToInt16(dh1.Substring(0, 4)), Convert.ToInt16(dh1.Substring(4, 2)), Convert.ToInt16(dh1.Substring(6, 2)));
        //    expdate = quotedate.AddDays(numday);

        //    decimal exc_rt = qheader.dquery1;

        //    int hbug = qheader.intquery0;
        //    var bg2list = (from bh in db.AP_002_RSQNH
        //                   join bj in db.AP_001_PTRAN
        //                   on new { a1 = bh.purchase_transaction_type } equals new { a1 = bj.purchase_order_code, }
        //                   into bj1
        //                   from bj2 in bj1.DefaultIfEmpty()
        //                   where bh.purchase_sequence_number == hbug
        //                   select new { bh, bj2 }).FirstOrDefault();


        //    vw_genlay glayhead = new vw_genlay();
        //    if (bg2list != null)
        //    {
        //        glayhead.vwstrarray1 = new string[20];
        //        //glayhead.vwstrarray1[0] = bg2list.bh1.cust_biz_name;
        //        glayhead.vwstrarray1[0] = expdate.ToString("dd/MM/yyyy");
        //        glayhead.vwstrarray1[1] = qheader.query3;
        //        glayhead.vwstrarray1[2] = util.date_slash(bg2list.bh.requisition_date);
        //        if (bg2list.bj2 != null)
        //            glayhead.vwstrarray1[3] = bg2list.bj2.purchase_order_name;
        //        else
        //            glayhead.vwstrarray1[3] = "";
        //        glayhead.vwstrarray1[4] = (bg2list.bh.quote_total_discount * exc_rt).ToString();
        //        glayhead.vwstrarray1[5] = (bg2list.bh.quote_total_tax * exc_rt).ToString();
        //        glayhead.vwstrarray1[6] = (bg2list.bh.quote_total_amount * exc_rt).ToString();
        //        glayhead.vwstrarray1[7] = util.date_slash(qheader.query2);
        //        glayhead.vwstrarray1[8] = qheader.dquery0.ToString();
        //        glayhead.vwstrarray1[9] = qheader.query6;

        //    }
        //    ViewBag.x2 = glayhead;
        //}

        private void init_header()
        {
            gdoc.purchase_transaction_type = "";
            gdoc.warehouse = "";
            gdoc.transaction_date = "";
            gdoc.requisition_reference = "";
            gdoc.note = "";
            gdoc.approval_level = 0;
            gdoc.created_by = "";
            gdoc.approval_by = "";
            gdoc.modified_by = "";
            gdoc.analysis_code1 = "";
            gdoc.analysis_code10 = "";
            gdoc.analysis_code2 = "";
            gdoc.analysis_code3 = "";
            gdoc.analysis_code4 = "";
            gdoc.analysis_code5 = "";
            gdoc.analysis_code6 = "";
            gdoc.analysis_code7 = "";
            gdoc.analysis_code8 = "";
            gdoc.analysis_code9 = "";
            gdoc.created_by = "";
            
        }

        [HttpPost]
        public ActionResult get_currency(string cust_code, string tdate)
        {
            // write your query statement

            tdate = util.date_yyyymmdd(tdate);
            decimal rat_code = 0;

            var curlist = (from bg in db.MC_001_CUREN
                           join bh in db.AR_001_CUSTM
                           on new { c1 = bg.currency_code } equals new { c1 = bh.currency_code }
                           where bh.customer_code == cust_code
                           select new
                           {
                               c1 = bg.currency_description,
                               c0 = bg.currency_code
                           }).FirstOrDefault();



            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = curlist.c0, Text = curlist.c1 });

            pubsess pubsess = (pubsess)Session["pubsess"];
            if (pubsess.base_currency_code == curlist.c0)
                ary.Add(new SelectListItem { Value = curlist.c0, Text = "1" });
            else
            {
                string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(curlist.c0) + " and '" + tdate + "' between date_from and date_to";
                var dbexch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
                if (dbexch != null)
                    rat_code = dbexch.dquery0;

                ary.Add(new SelectListItem { Value = curlist.c0, Text = rat_code.ToString() });
            }


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            //}
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult assign(string item_code)
        {

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "sku", Text = sku_name });
            ary.Add(new SelectListItem { Value = "cur", Text = cur_bal.ToString() });
            ary.Add(new SelectListItem { Value = "reodr", Text = re_level.ToString() });
            ary.Add(new SelectListItem { Value = "lpc", Text = lpc.ToString() });
            ary.Add(new SelectListItem { Value = "lpd", Text = lpd });
            ary.Add(new SelectListItem { Value = "lpv", Text = lpv });
            ary.Add(new SelectListItem { Value = "leed", Text = l_time.ToString() });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }

        private void 
            get_price()
        {
         
            for (int i = 0; i < 12; i++)
            {
                ModelState.Remove("vwstrarray1[" + i + "]");
                ModelState.Remove("vwdclarray0[" + i + "]");
                ModelState.Remove("vwdclarray1[" + i + "]");
                ModelState.Remove("vwstrarray2[" + i + "]");
                ModelState.Remove("vwdclarray2[" + i + "]");
                ModelState.Remove("vwstrarray3[" + i + "]");
                ModelState.Remove("vwitarray1[" + i + "]");
            }
            ModelState.Remove("vwint1");
                int counter = 0;
            string me = "";
            glay.vwlist0 = new List<querylay>[20];
            
            List<string> count1 = new List<string>();
                 while (!(string.IsNullOrWhiteSpace(glay.vwstrarray0[counter])))
                {

                    me = glay.vwstrarray0[counter];
                    count1.Add(me);
                    var bgassign = (from bg in db.IV_001_ITEM
                            join bn in db.GB_001_PCODE
                            on new { a1 = bg.sku_sequence, a2 = "10" } equals new { a1 = bn.parameter_code, a2 = bn.parameter_type }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            where bg.item_code == me
                            select new { bg, bn2}).FirstOrDefault();
                    if (bgassign != null)
            {
                
                glay.vwstrarray1[counter] = bgassign.bn2.parameter_name;
                glay.vwstrarray3[counter]  = util.date_slash(bgassign.bg.last_purchase_date);
                glay.vwstrarray2[counter]  = bgassign.bg.last_purchase_vendor;
                glay.vwdclarray2[counter] = bgassign.bg.last_purchase_cost;
                glay.vwdclarray1[counter] = bgassign.bg.reorder_level;
                glay.vwdclarray0[counter] = bgassign.bg.current_balance;
                glay.vwitarray1[counter] = bgassign.bg.standard_lead_time;

            }

                    counter++;


                }
                 if (count1.Count != 0)
                     glay.vwint1 = count1.Count;

        }
        private void get_price1(int key1)
        {

            for (int i = 0; i < 12; i++)
            {
                ModelState.Remove("vwstrarray1[" + i + "]");
                ModelState.Remove("vwdclarray0[" + i + "]");
                ModelState.Remove("vwdclarray1[" + i + "]");
                ModelState.Remove("vwstrarray2[" + i + "]");
                ModelState.Remove("vwdclarray2[" + i + "]");
                ModelState.Remove("vwstrarray3[" + i + "]");
                ModelState.Remove("vwitarray1[" + i + "]");
            }
            ModelState.Remove("vwint1");
            int counter = 0;
            List<string> count = new List<string>();

            var bgassign1 = (from bg in db.AP_002_RQUSN
                             where bg.purchase_sequence_number == key1
                             select bg).ToList();
            foreach (var item in bgassign1)
            {
                var bgassign = (from bg in db.IV_001_ITEM
                                join bn in db.GB_001_PCODE
                                on new { a1 = bg.sku_sequence, a2 = "10" } equals new { a1 = bn.parameter_code, a2 = bn.parameter_type }
                                into bn1
                                from bn2 in bn1.DefaultIfEmpty()
                                where bg.item_code == item.item_code
                                select new { bg, bn2 }).FirstOrDefault();
                if (bgassign != null)
                {
                    count.Add(item.item_code);
                    glay.vwstrarray0[counter]  = item.item_code;
                    glay.vwitarray0[counter] = Convert.ToInt32(item.purchase_requition_quantity);
                    glay.vwstrarray1[counter] = bgassign.bn2.parameter_name;
                    glay.vwstrarray3[counter] = bgassign.bg.last_purchase_date;
                    glay.vwstrarray2[counter] = bgassign.bg.last_purchase_vendor;
                    glay.vwdclarray2[counter] = bgassign.bg.last_purchase_cost;
                    glay.vwdclarray1[counter] = bgassign.bg.reorder_level;
                    glay.vwdclarray0[counter] = bgassign.bg.current_balance;
                    glay.vwitarray1[counter] = bgassign.bg.standard_lead_time;

                }

                counter++;


            }
            if (count.Count != 0)
                glay.vwint1 = count.Count;

                        }

        private void move_detail()
        {
            ModelState.Remove("vwdclarray1[0]");
            ModelState.Remove("vwdclarray1[1]");
            ModelState.Remove("vwdclarray1[2]");
            ModelState.Remove("vwdclarray1[3]");
            ModelState.Remove("vwdclarray1[4]");
            glay.vwstrarray6 = new string[20];
            glay.vwdclarray1 = new decimal[20];

            decimal exc_rt = qheader.dquery0;

            var bgdtl = (from bh in db.IV_001_ITEM
                         join bf in db.GB_001_PCODE
                         on new { a1 = bh.sku_sequence, a2 = "10" } equals new { a1 = bf.parameter_code, a2 = bf.parameter_type }
                         into bf1
                         from bf2 in bf1.DefaultIfEmpty()
                         where bh.item_code == AP_002_RQUSN.item_code
                         select new { bh, bf2 }).FirstOrDefault();


            glay.vwint2 = AP_002_RQUSN.line_sequence;
            glay.vwstring7 = AP_002_RQUSN.purchase_requisition_sequence;
            glay.vwstring0 = AP_002_RQUSN.item_code;
            glay.vwdecimal0 = AP_002_RQUSN.purchase_requition_quantity;
            glay.vwstring2 = AP_002_RQUSN.item_code;
            glay.vwstring1 = AP_002_RQUSN.warehouse;
            glay.vwstring3 = AP_002_RQUSN.note;
            glay.vwstring4 = util.date_slash(AP_002_RQUSN.requisition_date);
            psess.temp2 = bgdtl.bf2.parameter_name;
            Session["psess"] = psess;

        }

        private void header_ana(string commandn = "")
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            SelectList[] head_det = new SelectList[20];

            psess.sarrayt1 = aheader5;

            if (commandn == "D")
            {
                var bglist = from bg in db.GB_001_HEADER
                             where bg.header_type_code == "002" && bg.sequence_no != 99
                             select bg;

                foreach (var item in bglist.ToList())
                {
                    int count2 = item.sequence_no - 1;
                    aheader7[count2] = item.mandatory_flag;
                    glay.vwstrarray4[count2] = item.header_code;
                    var bglist2 = (from bg in db.GB_001_HANAL
                                   where bg.header_sequence == item.header_code
                                   select bg).FirstOrDefault();

                    if (bglist2 != null)
                    {
                        glay.vwstrarray5[count2] = bglist2.header_description;
                        string str = " select analysis_code query0, analysis_description query1 from GB_001_DANAL where header_sequence = ";
                        str += util.sqlquote(item.header_code);
                        var str1 = db.Database.SqlQuery<querylay>(str);
                        glay.vwlist0[count2] = str1.ToList();

                    }

                }

                psess.sarrayt0 = aheader7;
                psess.sarrayt1 = glay.vwstrarray5;
            }

        }

        private void quote_update()
        {
            string sqlstr = " update AP_002_RSQNH set quote_total_amount = a001, quote_total_tax = a002, quote_total_discount = a003, quote_total_ext_price = a004, quote_total_qty = a005 from AP_002_RSQNH ax, ";
            sqlstr += " (select sum(quote_amount) a001, sum(tax_amount) a002, sum (actual_discount) a003, sum(ext_price) a004, sum(quote_qty) as a005 from AP_002_RQUSN where  quote_reference=" + util.sqlquote(qheader.query3);
            sqlstr += " ) bxz where ax.quote_reference=" + util.sqlquote(qheader.query3);
            //sqlstr += " where quote_reference=" + util.sqlquote(preseq);
            db.Database.ExecuteSqlCommand(sqlstr);

       
        }
        private void year_cal()
        {
            int thisyear = DateTime.Now.Year;
            int startyear = thisyear - 10;
            int endyear = thisyear + 11;
            List<SelectListItem> ary = new List<SelectListItem>();
            for (int yctr = startyear; yctr < endyear; yctr++)
            {
                ary.Add(new SelectListItem { Value = yctr.ToString(), Text = yctr.ToString() });
            }

            ViewBag.year = ary;

        }

        private void set_qheader()
        {
            qheader.query0 = gdoc.purchase_transaction_type;
            qheader.intquery0 = gdoc.purchase_sequence_number;
            qheader.query2 = gdoc.transaction_date;
            qheader.query3 = gdoc.requisition_reference;
            qheader.bquery0 = true;
            var bgassign = (from bg in db.AR_001_STRAN
                            where bg.order_code == gdoc.purchase_transaction_type && bg.sequence_no == 0
                            select bg).FirstOrDefault();

            qheader.query4 = "Y";
            if (bgassign != null)
            {
                if (string.IsNullOrWhiteSpace(bgassign.tax_invoice1) && string.IsNullOrWhiteSpace(bgassign.tax_invoice2))
                    qheader.query4 = "N";
            }
            qheader.query5 = "Y";
            //if (pubsess.base_currency_code == gdoc.currency_code)
            //    qheader.query5 = "N";

            //if (pubsess.exchange_editable != "Y")
            //{
            //    string exstr = "select exchange_rate dquery0 from MC_001_EXCRT where currency_code=" + util.sqlquote(gdoc.currency_code) + " and '" + gdoc.transaction_date + "' between date_from and date_to";
            //    var exch = db.Database.SqlQuery<querylay>(exstr).FirstOrDefault();
            //    if (exch != null)
            //        qheader.dquery0 = exch.dquery0;
            //}

            //string read_cur = (from bg in db.MC_001_CUREN
            //                   where bg.currency_code == gdoc.currency_code
            //                   select bg.currency_description).FirstOrDefault();
            //if (read_cur != null)
            //{
            //    qheader.query6 = read_cur;
            //    qheader.dquery1 = qheader.dquery0;
            //    if (pubsess.exchange_rate_mode == "F")
            //    {
            //        qheader.dquery1 = 1 / qheader.dquery0;
            //    }
            //}
            Session["qheader"] = qheader;

        }
        public ActionResult show_doc(int id)
        {
            var bglist = (from bg in db.GB_001_DOC
                          where bg.document_sequence == id
                          select bg).FirstOrDefault();


            byte[] imagedata = bglist.document_image;
            return File(imagedata, "png");
        }

    }
}

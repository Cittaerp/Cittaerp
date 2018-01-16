using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;
using anchor1.Filters;

namespace CittaErp.Controllers
{

    public class InvenController : Controller
    {
        
         //Get: /Employee/

        IV_002_IVTYD IV_002_IVTYD = new IV_002_IVTYD();
        IV_002_INVTY gdoc = new IV_002_INVTY();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;
        queryhead qheader = new queryhead();
        
        string preseq = "";
        string ptype = "";
        decimal f = 0;
        decimal y = 0;
        bool err_flag = true;
        string set_price = "n";
        decimal qty = 0; decimal pricep = 0; decimal tax_amt = 0; decimal net_amt = 0; decimal quote_amt = 0; decimal ext_price = 0;
        decimal net_amt1 = 0; decimal quote_amt1 = 0; decimal qtx = 0;
        decimal disc = 0; decimal disc_per1 = 0;
        string action_flag = "";
        public ActionResult Index()
        {
            
            util.init_values();
            //Session["ivnt"] = "";


            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
           
            var bglist = from bh in db.IV_002_INVTY
                         
                         select new vw_genlay
                         {
                             vwint0 = bh.inventory_sequence_number,
                             vwstring1 = bh.manual_reference,
                             //vwstring2 = bn.name,
                             vwstring4 =bh.transaction_date,
                             vwstring3 = bh.description
                         };

            return View(bglist.ToList());


        }
        public ActionResult CreateHeader()
        {
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            select_query_head("H");
            glay.vwstring8 = "H";

            //show_screen_info();

            return View(glay);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHeader(vw_genlay glay_in, HttpPostedFileBase[] photofile, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "CreateHeader";
            action_flag = "CreateHeader";
            glay = glay_in;
            photo1 = photofile;
            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head("H");
            glay.vwstring8 = "H";
            
            return View(glay);

        }

        [EncryptionActionAttribute]
        public ActionResult EditHeader()
        {
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            initial_rtn();


            glay.vwstring8 = "H";
            glay.vwstring9 = qheader.intquery0.ToString();

            gdoc = db.IV_002_INVTY.Find(qheader.intquery0);
            if (gdoc != null)
                read_header();

            select_query_head("H");

            return View(glay);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHeader(vw_genlay glay_in, HttpPostedFileBase[] photofile, string headtype)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "EditHeader";
            action_flag = "EditHeader";
            glay = glay_in;
            glay.vwstring9 = qheader.intquery0.ToString();

            photo1 = photofile;
            update_file(headtype);

            if (err_flag)
            {
                return RedirectToAction("CreateDetails");
            }

            select_query_head("H");

            glay.vwstring8 = "H";
            
            return View(glay);

        }

        [EncryptionActionAttribute]
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
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetails(vw_genlay glay_in, string headtype = "D")
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            glay = glay_in;

            glay.vwstring9 = qheader.intquery0.ToString();

            if (headtype == "send_app")
            {
                string str = "update IV_002_INVTY set approval_level=99 where inventory_sequence_number=" + qheader.intquery0;
                db.Database.ExecuteSqlCommand(str);
                util.update_entry("013", qheader.intquery0.ToString(), pubsess.userid);
               
            }

            if (err_flag)
            {
                update_file(headtype);

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

        [EncryptionActionAttribute]
        public ActionResult Edit(int key1)
        {
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
           
            gdoc = db.IV_002_INVTY.Find(key1);
            if (gdoc != null)
            {
                set_qheader();
                return RedirectToAction("CreateDetails", new { headtype = "D" });
            }


            return View(glay);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(vw_genlay glay_in, string headtype, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "CreateDetails";
            action_flag = "CreateDetails";
            glay = glay_in;

            if (id_xhrt == "D")
            {
                delete_record();
                return RedirectToAction("index");
            }
            update_file(headtype);
            if (err_flag)
                return RedirectToAction("index");

            select_query_head("H");
            return View(glay);
        }

        [EncryptionActionAttribute]
        public ActionResult EditDetails(int key1, int key2 )
        {
            ViewBag.action_flag = "EditDetails";
            action_flag = "EditDetails";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            qheader = (queryhead)Session["qheader"];


            IV_002_IVTYD = db.IV_002_IVTYD.Find(qheader.intquery0, key2);
            if (IV_002_IVTYD == null)
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
        [ValidateAntiForgeryToken]
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
                string str = "update IV_002_INVTY set approval_level=99 where inventory_sequence_number=" + qheader.intquery0;
                db.Database.ExecuteSqlCommand(str);
                util.update_entry("013", qheader.intquery0.ToString(), pubsess.userid);
                
            }
            update_file(headtype);

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
            IV_002_IVTYD = db.IV_002_IVTYD.Find(glay.vwint0);
            if (IV_002_IVTYD != null)
            {
                db.IV_002_IVTYD.Remove(IV_002_IVTYD);
                db.SaveChanges();
            }
        }
        private void update_file(string headtype)
        {
            err_flag = true;
            validation_routine(headtype);

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
                    gdoc = new IV_002_INVTY();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.approval_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;
                    //gdoc.inventory_sequence_number = glay.vwint0;
                    //Session["ivnt"] = gdoc.inventory_sequence_number;
                }
                 else
                {
                    gdoc = db.IV_002_INVTY.Find(qheader.intquery0);
                }
                gdoc.transaction_type = "";
                gdoc.manual_reference = glay.vwstrarray0[1];
                gdoc.description = glay.vwstrarray0[3];
                gdoc.transaction_date = util.date_yyyymmdd(glay.vwstrarray0[2]);
                gdoc.period = "";
                gdoc.year = "";
                gdoc.modified_by = pubsess.userid;
                gdoc.modified_date = DateTime.UtcNow;

                if (cr_flag == "CreateHeader")
                    db.Entry(gdoc).State = EntityState.Added;
                else
                    db.Entry(gdoc).State = EntityState.Modified;
            }
            else
            {
                if (cr_flag == "CreateDetails")
                {
    
                    IV_002_IVTYD = new IV_002_IVTYD();
                    IV_002_IVTYD.created_by = pubsess.userid;
                    IV_002_IVTYD.created_date = DateTime.UtcNow;
                    //string sqlstr = "select isnull(max(sequence_number),0) vwint1 from IV_002_IVTYD where reference_number=" + util.sqlquote(glay.vwstring10);
                    //var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
                    //glay.vwdecimal4 = sql1.vwint1 + 1;
                    glay.vwint3 = get_next_line_sequence();
                
                    
                }
            else
            {
                IV_002_IVTYD = db.IV_002_IVTYD.Find(qheader.intquery0, glay.vwint3);
            }

                // the detailsub button has been clicked
                IV_002_IVTYD.inventory_sequence_number = qheader.intquery0;
                IV_002_IVTYD.sequence_number = glay.vwint3;
                IV_002_IVTYD.item_code = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                IV_002_IVTYD.description = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;
                IV_002_IVTYD.quantity = glay.vwdecimal2;
                IV_002_IVTYD.unit_cost = glay.vwdecimal1;
                IV_002_IVTYD.extended_cost = glay.vwdecimal3;
                IV_002_IVTYD.reason_code = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
                IV_002_IVTYD.warehouse_from = string.IsNullOrWhiteSpace(glay.vwstring4)? "" : glay.vwstring4;
                IV_002_IVTYD.bin_location_from = "";
                IV_002_IVTYD.transaction_type = string.IsNullOrWhiteSpace(glay.vwstring5)?"":glay.vwstring5;
                IV_002_IVTYD.bin_location_to = "";
                IV_002_IVTYD.reference_number = "";
                IV_002_IVTYD.modified_date = DateTime.UtcNow;
                IV_002_IVTYD.modified_by = pubsess.userid;

                IV_002_IVTYD.analysis_code1 = "";
                IV_002_IVTYD.analysis_code2 = "";
                IV_002_IVTYD.analysis_code3 = "";
                IV_002_IVTYD.analysis_code4 = "";
                IV_002_IVTYD.analysis_code5 = "";
                IV_002_IVTYD.analysis_code6 = "";
                IV_002_IVTYD.analysis_code7 = "";
                IV_002_IVTYD.analysis_code8 = "";
                IV_002_IVTYD.analysis_code9 = "";
                IV_002_IVTYD.analysis_code10 = "";

                if (glay.vwstrarray6 != null)
                {
                    int arrlen = glay.vwstrarray6.Length;
                    if (arrlen > 0)
                        IV_002_IVTYD.analysis_code1 = string.IsNullOrWhiteSpace(glay.vwstrarray6[0]) ? "" : glay.vwstrarray6[0];
                    if (arrlen > 1)
                        IV_002_IVTYD.analysis_code2 = string.IsNullOrWhiteSpace(glay.vwstrarray6[1]) ? "" : glay.vwstrarray6[1];
                    if (arrlen > 2)
                        IV_002_IVTYD.analysis_code3 = string.IsNullOrWhiteSpace(glay.vwstrarray6[2]) ? "" : glay.vwstrarray6[2];
                    if (arrlen > 3)
                        IV_002_IVTYD.analysis_code4 = string.IsNullOrWhiteSpace(glay.vwstrarray6[3]) ? "" : glay.vwstrarray6[3];
                    if (arrlen > 4)
                        IV_002_IVTYD.analysis_code5 = string.IsNullOrWhiteSpace(glay.vwstrarray6[4]) ? "" : glay.vwstrarray6[4];
                    if (arrlen > 5)
                        IV_002_IVTYD.analysis_code6 = string.IsNullOrWhiteSpace(glay.vwstrarray6[5]) ? "" : glay.vwstrarray6[5];
                    if (arrlen > 6)
                        IV_002_IVTYD.analysis_code7 = string.IsNullOrWhiteSpace(glay.vwstrarray6[6]) ? "" : glay.vwstrarray6[6];
                    if (arrlen > 7)
                        IV_002_IVTYD.analysis_code8 = string.IsNullOrWhiteSpace(glay.vwstrarray6[7]) ? "" : glay.vwstrarray6[7];
                    if (arrlen > 8)
                        IV_002_IVTYD.analysis_code9 = string.IsNullOrWhiteSpace(glay.vwstrarray6[8]) ? "" : glay.vwstrarray6[8];
                    if (arrlen > 9)
                        IV_002_IVTYD.analysis_code10 = string.IsNullOrWhiteSpace(glay.vwstrarray6[9]) ? "" : glay.vwstrarray6[9];
                    psess.intemp0 = arrlen;
                    Session["psess"] = psess;
                }
                if (cr_flag == "CreateDetails")

                    db.Entry(IV_002_IVTYD).State = EntityState.Added;
                else
                    db.Entry(IV_002_IVTYD).State = EntityState.Modified;
            }    

            try
            {
                db.SaveChanges();
            }

            catch (Exception err)
            {
                if (err.InnerException == null)
                    ModelState.AddModelError(string.Empty, err.Message);
                else
                    ModelState.AddModelError(string.Empty, err.InnerException.InnerException.Message);

                err_flag = false;
            }
            if (err_flag && cr_flag.IndexOf("Header") < 0)
            {
                //string stri = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, IV_002_IVTYD b where header_sequence in (analysis_code1";
                //stri += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                //stri += " and inventory_sequence_number =" + glay.vwint0;
                ////db.Database.ExecuteSqlCommand(stri);
                qheader.intquery1 = IV_002_IVTYD.sequence_number;
                Session["qheader"] = qheader;
            

            }
           
            if (err_flag && cr_flag.IndexOf("Header") > 0)
            {
                //if (Session["action_flag"].ToString() == "Create")
                {
                    set_qheader();
                    util.write_document("INVENTORY", gdoc.inventory_sequence_number.ToString(), photo1, glay.vwstrarray9);
                }

            }

        }

        private int get_next_line_sequence()
        {
            string sqlstr = "select isnull(max(sequence_number),0) vwint1 from IV_002_IVTYD where inventory_sequence_number=" + qheader.intquery0;
            var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
            return sql1.vwint1 + 1;
          }
        private void validation_routine(string commandn)
        {
            string error_msg = "";
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];

          aheader7 = psess.sarrayt0;
            aheader5 = psess.sarrayt1;
           
            if (error_msg != "")
            {
                ModelState.AddModelError(string.Empty, error_msg);
                err_flag = false;
            }
            DateTime date_chk = DateTime.Now;
            DateTime invaliddate = new DateTime(1000, 01, 01);
            string cr_flag = action_flag;
            if (cr_flag.IndexOf("Header") > 0)
            {

                if (!util.date_validate(glay.vwstrarray0[2]))
                {
                    ModelState.AddModelError(String.Empty, "Please insert a valid transaction date");
                    err_flag = false;
                }
            }
            if (cr_flag.IndexOf("Details") > 0)
            {
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

           
        }
        private void select_query_head(string type1)
        {

            //var emp = from pf in db.GB_999_MSG
            //          where pf.type_msg == "TRT"
            //          orderby pf.name1_msg
            //          select pf;
            //ViewBag.type = new SelectList(emp.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[0]);

            //ViewBag.emp = util.para_selectquery("007", glay.vwstrarray0[1]);
           //ViewBag.emp = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[1]);

            //var empi = from pf in db.GB_001_EMP
            //           where pf.active_status == "N"
            //           orderby pf.name
            //           select pf;
            //ViewBag.emp = new SelectList(empi.ToList(), "manual_reference", "name", glay.vwstrarray0[1]);

            //var empu = from pf in db.GB_999_MSG
            //           where pf.type_msg == "FYM"
            //           orderby pf.name1_msg
            //           select pf;
            //ViewBag.month = new SelectList(empu.ToList(), "code_msg", "name1_msg", glay.vwstrarray0[4]);

            if (type1 == "D")
            {

                var empt = from pf in db.GB_999_MSG
                           where pf.type_msg == "TRT" && pf.code_msg != "01" && pf.code_msg != "05"
                          orderby pf.name1_msg
                          select pf;
                ViewBag.type = new SelectList(empt.ToList(), "code_msg", "name1_msg", glay.vwstring5);

                ViewBag.lemp = util.para_selectquery("62", glay.vwstrarray0[6]);
               // ViewBag.lemp = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstrarray0[6]);

                ViewBag.item = util.para_selectquery("60", glay.vwstring1);
                //ViewBag.item = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring1);
                ViewBag.rcode = util.para_selectquery("03", glay.vwstring0);
                //var pcode = from bg in db.GB_001_PCODE
                //            where bg.parameter_type == "03" && bg.active_status == "N"
                //            orderby bg.parameter_name
                //            select bg;
                //ViewBag.rcode = new SelectList(pcode.ToList(), "parameter_code", "parameter_name", glay.vwstring0);

                ViewBag.ware = util.para_selectquery("57", glay.vwstring4);
               // ViewBag.ware = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring4);

             }
        }

        private void initial_rtn()
        {
            //string hcount = Session["ivnt"].ToString();
            //int hbug = 0;
            //int.TryParse(hcount, out hbug);
            glay.vwdclarray0 = new decimal[50];
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            glay.vwstrarray6 = new string[20];
            glay.vwstrarray1 = new string[20];

           // string ref_def = (from bg in db.IV_002_INVTY
              //                where bg.inventory_sequence_number == hbug
              //                select bg.).FirstOrDefault();
            //glay.vwstring1 = ref_def;
            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwstrarray0[wctr] = "";
           // glay.vwstrarray0[3] = DateTime.Now.ToString("dd/mm/yyyy");
            glay.vwlist0 = new List<querylay>[20];


        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
             //write your query statement
            string sqlstr = "delete from [dbo].[IV_002_INVTY] where cast(inventory_sequence_number as varchar)=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "delete from [dbo].[IV_002_IVTYD] where cast(inventory_sequence_number as varchar)=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        public ActionResult delete_detail(string id)
        {
             //write your query statement
            //glay.vwstring9 = Session["ivnt"].ToString();
            string sqlstr = "delete from [dbo].[IV_002_IVTYD] where cast(inventory_sequence_number as varchar) +'[]'+ cast(sequence_number as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            //quote_update();
            return RedirectToAction("CreateDetails");
            
        }

        private void read_details()
        {
           // string sorder = Session["ivnt"].ToString();
            int hbug = qheader.intquery0; 
            //int.TryParse(sorder, out hbug);

            var bglist = from bh in db.IV_002_IVTYD
                         join bg1 in db.IV_001_ITEM
                         on new { a1 = bh.item_code } equals new { a1 = bg1.item_code}
                         into bg2
                         from bg3 in bg2.DefaultIfEmpty()
                         join bh4 in db.GB_001_PCODE
                         on new{ a1 = bh.reason_code, a2 = "03"} equals new {a1 = bh4.parameter_code, a2 = bh4.parameter_type}
                         into bh5
                         from bh6 in bh5.DefaultIfEmpty()
                         join bk in db.IV_001_WAREH
                         on new { a1 = bh.warehouse_from } equals new { a1=bk.warehouse_code}
                         into bk1
                         from bk2 in bk1.DefaultIfEmpty()
                         join pf in db.GB_999_MSG
                         on new { a1 = bh.transaction_type, a2= "TRT" } equals new { a1= pf.code_msg, a2 = pf.type_msg}
                         into pf1
                         from pf2 in pf1.DefaultIfEmpty()
                         where bh.inventory_sequence_number == hbug

                         select new vw_genlay
                         {
                             vwint0 = bh.inventory_sequence_number,
                             vwint3 = bh.sequence_number,
                             vwstring1 = bh.item_code,
                             vwstring10 = bh.reference_number,
                             vwstring2 = bh.description,
                             vwdecimal2 = bh.quantity,
                             vwdecimal1 = bh.unit_cost,
                             vwdecimal3 = bh.extended_cost,
                             vwstring3 = bh6.parameter_name,
                             vwstring4 = bh.warehouse_from,
                             vwstring6 = pf2.name1_msg,
                             vwstring5 = bg3.item_name,
                         };
            ViewBag.x1 = bglist.ToList();
        }

        [HttpPost]
        public ActionResult assign(string item_code)
        {
            int bal_qty = 0; string sku_name = ""; string item_des = "";
            var bgassign = (from bg in db.IV_001_ITEM
                            join bn in db.IV_001_ITMST
                            on new { a1 = bg.item_code } equals new { a1 = bn.item_code }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            join bf in db.GB_001_PCODE
                            on new { a2 = bg.sku_sequence, a3 = "10" } equals new { a2 = bf.parameter_code, a3 = bf.parameter_type }
                            into bf1
                            from bf2 in bf1.DefaultIfEmpty()
                            where bg.item_code == item_code
                            select new { bg, bn2, bf2 }).FirstOrDefault();
            if (bgassign != null)
            {
                if (bgassign.bn2!=null)
                    bal_qty = bgassign.bn2.bal_qty;
                else 
                    bal_qty = 0;

                item_des = bgassign.bg.description;

                if (bgassign.bf2 != null)
                    sku_name = bgassign.bf2.parameter_name;
                else
                    sku_name = "";
            }

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "sku", Text = sku_name });
            ary.Add(new SelectListItem { Value = "qty", Text = bal_qty.ToString() });
            ary.Add(new SelectListItem { Value = "des", Text = item_des });


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
        public ActionResult price_cal(string unit_cost, string item_qty)
        {

            decimal.TryParse(item_qty, out qty);
            decimal.TryParse(unit_cost, out qtx);


            ext_price = qty * qtx;


            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = ext_price.ToString("#,##0.00") });

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }

        private void show_screen_info()
        {
            display_header();
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
            //string sorder = Session["ivnt"].ToString();
            int hbug = qheader.intquery0;

            var bg2list = (from bh in db.IV_002_INVTY
                          join bg in db.GB_001_EMP
                          on new { a1 = bh.manual_reference } equals new { a1 = bg.employee_code}
                          into bg1
                          from bg2 in bg1.DefaultIfEmpty()
                          join bf in db.GB_999_MSG
                          on new { a1 = bh.transaction_type, a2 = "TRT" } equals new { a1 = bf.code_msg, a2 = bf.type_msg}
                          into bf1
                          from bf2 in bf1.DefaultIfEmpty()
                           where bh.inventory_sequence_number == hbug
                          select new {bh,bg2,bf2}).FirstOrDefault();

            
            if (bg2list != null)
            {
                //glayhead.vwstrarray1 = new string[20];
                //glayhead.vwint0 = bg2list.bh.inventory_sequence_number;
                //glayhead.vwstrarray1[0] = bg2list.bh.transaction_id;
                //glayhead.vwstrarray1[1] = bg2list.bh.manual_reference;
                //glayhead.vwstrarray1[3] = bg2list.bh.description;
                ////glayhead.vwstrarray1[0] = bg2list.bf2.name1_msg;
                //glayhead.vwstrarray1[2] = util.date_slash(bg2list.bh.transaction_date);
                ////glayhead.vwstrarray1[4] = bg2list.bh.period;

                glay.vwstrarray0[1] = bg2list.bh.manual_reference;
                glay.vwstrarray0[3] = bg2list.bh.description;
                glay.vwstrarray0[2] = util.date_slash(bg2list.bh.transaction_date);
                //glay.vwstrarray0[4] = bg2list.bh.period;
               // glay.vwstrarray0[5] = bg2list.bh.year;
                glay.vwint2 = bg2list.bh.approval_level;
               // glay.vwstrarray0[6] = bg2list.bh.approval_by;

                string docode = gdoc.inventory_sequence_number.ToString();
                var bglist = from bg in db.GB_001_DOC
                             where bg.screen_code == "INVENTORY" && bg.document_code == docode
                             orderby bg.document_sequence
                             select bg;

                ViewBag.anapict = bglist.ToList();
            }
           

        }
        private void display_header()
        {

            //ModelState.Clear();
            ModelState.Remove("vwstrarray1[3]");
            ModelState.Remove("vwstrarray1[4]");
            ModelState.Remove("vwstrarray1[5]");
            ModelState.Remove("vwstrarray1[6]");
            ModelState.Remove("vwstrarray1[7]");
            ModelState.Remove("exchrate");
            

            int hbug = qheader.intquery0;
            var bg2list = (from bh in db.IV_002_INVTY
                           join bg in db.GB_001_EMP
                           on new { a1 = bh.manual_reference } equals new { a1 = bg.employee_code }
                           into bg1
                           from bg2 in bg1.DefaultIfEmpty()
                           join bf in db.GB_999_MSG
                           on new { a1 = bh.transaction_type, a2 = "TRT" } equals new { a1 = bf.code_msg, a2 = bf.type_msg }
                           into bf1
                           from bf2 in bf1.DefaultIfEmpty()
                           where bh.inventory_sequence_number == hbug
                           select new { bh, bg2, bf2 }).FirstOrDefault();


            vw_genlay glayhead = new vw_genlay();
            
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwint0 = bg2list.bh.inventory_sequence_number;
                glayhead.vwstrarray1[1] = bg2list.bh.manual_reference;
                glayhead.vwstrarray1[3] = bg2list.bh.description;
                //glayhead.vwstrarray1[0] = bg2list.bf2.name1_msg;
                glayhead.vwstrarray1[2] = util.date_slash(bg2list.bh.transaction_date);
                //glayhead.vwstrarray1[4] = bg2list.bh.period;

                //psess.temp3 = bg2list.bh.customer_code;


            }
            ViewBag.x2 = glayhead;
        }

     

        private void init_header()
        {
            gdoc.created_by = "";
            gdoc.transaction_type = "";
            gdoc.manual_reference = "";
            gdoc.description = "";
            gdoc.period = "";
            gdoc.approval_level = 0;
            gdoc.approval_by = "";
            gdoc.modified_by = "";
            gdoc.note = "";
            gdoc.attach_document = "";
            gdoc.transaction_date = "";
            gdoc.year = "";
            }

        private void move_detail()
        {
            //int key1 = qheader.intquery0;
            //int key2 = qheader.intquery1;
             
            //var bgdtl = (from bg in db.IV_002_IVTYD
            //                where bg.inventory_sequence_number == key1 && bg.sequence_number == key2
            //                select bg).FirstOrDefault();

               glay.vwint3 = IV_002_IVTYD.sequence_number;
               glay.vwstring1 = IV_002_IVTYD.item_code;
               glay.vwstring2 = IV_002_IVTYD.description;
               glay.vwdecimal2 = IV_002_IVTYD.quantity;
               glay.vwdecimal1 = IV_002_IVTYD.unit_cost;
               glay.vwdecimal3 = IV_002_IVTYD.extended_cost;
               glay.vwstring0 = IV_002_IVTYD.reason_code;
               glay.vwstring4 = IV_002_IVTYD.warehouse_from;
               glay.vwstring5 = IV_002_IVTYD.transaction_type;
               glay.vwstring10 = IV_002_IVTYD.reference_number;
               glay.vwstrarray6[0] = IV_002_IVTYD.analysis_code1;
               glay.vwstrarray6[1] = IV_002_IVTYD.analysis_code2;
               glay.vwstrarray6[2] = IV_002_IVTYD.analysis_code3;
               glay.vwstrarray6[3] = IV_002_IVTYD.analysis_code4;
               glay.vwstrarray6[4] = IV_002_IVTYD.analysis_code5;
               glay.vwstrarray6[5] = IV_002_IVTYD.analysis_code6;
               glay.vwstrarray6[6] = IV_002_IVTYD.analysis_code7;
               glay.vwstrarray6[7] = IV_002_IVTYD.analysis_code8;
               glay.vwstrarray6[8] = IV_002_IVTYD.analysis_code9;
               glay.vwstrarray6[9] = IV_002_IVTYD.analysis_code10;       

        }

        private void header_ana(string commandn = "")
        {
            glay.vwstrarray4 = new string[20];
            glay.vwstrarray5 = new string[20];
            string[] aheader7 = new string[20];
            string[] aheader5 = new string[20];
            SelectList[] head_det = new SelectList[20];

           //// Session["head_det"] = head_det;
           // //Session["aheader7"] = aheader7;
            psess.sarrayt1 = aheader5;

            if (commandn == "D")
            {
                var bglist = from bg in db.GB_001_HEADER
                             where bg.header_type_code == "013" && bg.sequence_no != 99
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

                // // Session["head_det"] = head_det;
                // //Session["aheader7"] = aheader7;
                // psess.sarrayt1 = glay.vwstrarray5;
                psess.sarrayt0 = aheader7;
                psess.sarrayt1 = glay.vwstrarray5;
            }
        }


        //private void quote_update()
        //{
        //    string sqlstr = " update IV_002_INVTY set quote_total_amount = a001, quote_total_tax = a002, quote_total_discount = a003, quote_total_ext_price = a004, quote_total_qty = a005";
        //    sqlstr += " from IV_002_INVTY a, (select sum(quote_amount) a001, sum(tax_amount) a002, sum (discount_amount) a003, sum(ext_price) a004, sum(quote_qty)a005 from IV_002_IVTYD where quote_reference=" + util.sqlquote(glay.vwstring9) + ") bx";
        //    sqlstr += " where quote_reference=" + util.sqlquote(glay.vwstring9);
        //    db.database.executesqlcommand(sqlstr);

        //}

        private void set_qheader()
        {
            //qheader.query0 = gdoc.sales_transaction_type;
            qheader.intquery0 = gdoc.inventory_sequence_number;
            qheader.query1 = gdoc.manual_reference;
           // qheader.dquery0 = gdoc.exchange_rate;
            qheader.query2 = gdoc.transaction_date;
           
                      
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

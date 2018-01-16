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

    public class StockController : Controller
    {
        //
        // GET: /Employee/

        IV_002_CONTD IV_002_CONTD = new IV_002_CONTD();
        IV_002_COUNT gdoc = new IV_002_COUNT();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();

        string preseq = "";
        string ptype = "";
        decimal f = 0;
        decimal y = 0;
        bool err_flag = true;
        string set_price = "N";
        int qty = 0; decimal pricep = 0; int phy = 0; decimal net_amt = 0; decimal quote_amt = 0; decimal diff = 0;
        decimal net_amt1 = 0; decimal quote_amt1 = 0; decimal qtx = 0;
        decimal disc = 0; decimal disc_per1 = 0;
        string action_flag = "";
        
        public ActionResult Index()
        {
            
            util.init_values();
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            psess.temp0 = "";

           Session["psess"] = psess;
            
            var bglist = from bh in db.IV_002_COUNT
                         select new vw_genlay
                         {
                             vwstring0 = bh.stock_sheet_number,
                             vwstring1 = bh.period,
                             vwstring2 = bh.warehouse_code,
                             vwstring3 = bh.approval_by,
                             vwstring4 = bh.stock_count_date,
                             vwint1 = bh.approval_level
                         };

            return View(bglist.ToList());


        }
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            select_query_head();
            glay.vwstring8 = "H";
               
            read_header();
            read_details();
            
            return View(glay);
        }
      
        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, string commandn)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;

            update_file(commandn);

            if (err_flag)
            {
                
                    
                    return RedirectToAction("Details");
            }

            select_query_head();
            glay.vwstring8 = "H";
            read_header();
            read_details();
            return View(glay);

        }

        public ActionResult Details(string headtype="D")
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            //if (headtype=="D")
            initial_rtn();
           
            glay.vwstring8 = headtype;
            glay.vwstring9 = psess.temp0.ToString();

            read_header();
            read_details();
            select_query_head();
            if (headtype == "DE")
            {
                move_detail();
                glay.vwstring8 = "D";
            }

            return View(glay);
        }


        [HttpPost]
        public ActionResult Details(vw_genlay glay_in, string commandn)
        {
            pubsess = (pubsess)Session["pubsess"];
            glay = glay_in;

            if(commandn == "headedit")
            {
                ViewBag.action_flag = "Edit";
           
                glay.vwstring8 = "H";

                return RedirectToAction("Details", new { headtype = "H"});
            }
            glay.vwstring9 = psess.temp0.ToString();

            update_file(commandn);

            if (err_flag)
            {
                if (commandn == "headsub")
                {
                   
                    return RedirectToAction("Details");
                }

                if (commandn == "detailsub")
                {
                    return RedirectToAction("Details");
                }
            }
            glay.vwstrarray1 = new string[20];
           
            read_header();
            read_details();
            select_query_head();

            return View(glay);

        }


        public ActionResult Edit(string key1)
        {
            ViewBag.action_flag = "Create";
            
            pubsess = (pubsess)Session["pubsess"];
            

            gdoc = db.IV_002_COUNT.Find(key1);
            if (gdoc != null)
            {
                psess.temp0 = gdoc.stock_sheet_number;
                Session["psess"] = psess;
            
                return RedirectToAction("Details",  new {headtype="D"});
            }

            util.init_values();
            initial_rtn();
            select_query_head();
            glay.vwstring8 = "D";

            read_header();
            read_details();

            return View(glay);

        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string commandn, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            glay = glay_in;

            if (id_xhrt == "D")
            {
                delete_record();
                return RedirectToAction("Index");
            }
            update_file(commandn);
            if (err_flag)
                return RedirectToAction("Index");

            select_query_head();
            return View(glay);
        }

        public ActionResult EditDetails(int key1, string key2 )
        {
            ViewBag.action_flag = "Edit";

            pubsess = (pubsess)Session["pubsess"];


            IV_002_CONTD = db.IV_002_CONTD.Find(key1, key2);
            if (IV_002_CONTD != null)
            {
                psess.temp0 = gdoc.stock_sheet_number;
                Session["psess"] = psess;
            
                return RedirectToAction("Details", new { headtype = "DE" });
            }

            util.init_values();
            initial_rtn();
            select_query_head();
            glay.vwstring8 = "D";

            read_header();
            read_details();

            return View(glay);

        }

        private void delete_record()
        {
            IV_002_CONTD = db.IV_002_CONTD.Find(glay.vwstring0);
            if (IV_002_CONTD != null)
            {
                db.IV_002_CONTD.Remove(IV_002_CONTD);
                db.SaveChanges();
            }
        }
        private void update_file(string commandn)
        {
            err_flag = true;
            validation_routine(commandn);

            if (err_flag)
                update_record(commandn);

        }

        private void update_record(string commandn)
        {
           if(action_flag == "Create")
            {
                if (commandn == "headsub")
                {
                    gdoc = new IV_002_COUNT();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;
                    gdoc.stock_sheet_number = glay.vwint0.ToString();
                    psess.temp0 = gdoc.stock_sheet_number;
                    Session["psess"] = psess;
            
                }

                if (commandn == "detailsub")
                {
    
                    IV_002_CONTD = new IV_002_CONTD();
                    IV_002_CONTD.created_by = pubsess.userid;
                    IV_002_CONTD.created_date = DateTime.UtcNow;

                    
                }
            }
            else
            {
                if (commandn == "headsub")
                {
                    gdoc = db.IV_002_COUNT.Find(glay.vwstring9);
                }
                else
                {
                    IV_002_CONTD = db.IV_002_CONTD.Find(glay.vwstring9, glay.vwstring6);
                }
            }


            if (commandn == "headsub")
            {
                // the headsub button has been clicked

               
                gdoc.stock_count_date = util.date_yyyymmdd(glay.vwstring1);
                gdoc.period = glay.vwstring2;
                gdoc.warehouse_code = glay.vwstring3;
                gdoc.approval_level = glay.vwint5;
                gdoc.approval_by = glay.vwstring5;
                gdoc.modified_by = pubsess.userid;
                gdoc.modified_date = DateTime.UtcNow;

            }
            else
            {
                // the detailsub button has been clicked
                diff = glay.vwint2 - glay.vwint3;

                IV_002_CONTD.stock_sheet_number = glay.vwstring9;
                IV_002_CONTD.item_code = glay.vwstring6;
                IV_002_CONTD.quantity = glay.vwint2;
                IV_002_CONTD.physical_count = glay.vwint3;
                IV_002_CONTD.difference = Convert.ToInt16(diff);
                IV_002_CONTD.modified_date = DateTime.UtcNow;
                IV_002_CONTD.modified_by = pubsess.userid;
            }    

           if(action_flag == "Create")
            {
                if (commandn == "detailsub")
                {
                    db.Entry(IV_002_CONTD).State = EntityState.Added;
                }
                else
                {
                    db.Entry(gdoc).State = EntityState.Added;
                }
            }
            else
            {
                if (commandn == "detailsub")
                {
                    db.Entry(IV_002_CONTD).State = EntityState.Modified;
                }
                else
                { 
                    db.Entry(gdoc).State = EntityState.Modified; 
                }
            
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
        private void validation_routine(string commandn)
        {
            //string error_msg = "";
            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Please enter Name;

            //if (Session["action_flag"].ToString() == "Index")
            //{
            //    IV_002_CONTD bnk = db.IV_002_CONTD.Find(glay.vwint0);
            //    if (bnk != null)
            //        error_msg = "Can not accept duplicates";
            //}

            DateTime date_chk = DateTime.Now;
            DateTime invaliddate = new DateTime(1000, 01, 01);
            
            if (!util.date_validate(glay.vwstring1))
            {
                ModelState.AddModelError(String.Empty, "Please insert a stock count  date");
                err_flag = false;
            }
           

            //if (error_msg != "")
            //{
            //    ModelState.AddModelError(String.Empty, error_msg);
            //    err_flag = false;
            //}
        }
        private void select_query_head()
        {
            ViewBag.ware = util.para_selectquery("57", glay.vwstring3);
            //ViewBag.ware = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring3);

            //var empi = from pf in db.IV_001_WAREH
            //           where pf.active_status == "N"
            //           orderby pf.warehouse_name
            //          select pf;
            //ViewBag.ware = new SelectList(empi.ToList(), "warehouse_code", "warehouse_name", glay.vwstring3);

            ViewBag.item = util.para_selectquery("60", glay.vwstring6);
            //ViewBag.item = new SelectList(bg2.ToList(), "query0", "query1", glay.vwstring6);

            //var empe = from pf in db.IV_001_ITEM
            //           where pf.active_status == "N"
            //           orderby pf.item_name
            //           select pf;
            //ViewBag.item = new SelectList(empe.ToList(), "item_code", "item_name", glay.vwstring6);
        }

        private void initial_rtn()
        {

            glay.vwstrarray1 = new string[20];


        }

        private void read_record(string commandn)
        {
            if (commandn == "detailsub")
            {
                glay.vwstring0 = IV_002_CONTD.stock_sheet_number;
                glay.vwstring6 = IV_002_CONTD.item_code;
                glay.vwint2 = IV_002_CONTD.quantity;
                glay.vwint3 = IV_002_CONTD.physical_count;
                glay.vwint4 = IV_002_CONTD.difference;

            }
            else
            {
                var bglist = from bg in db.IV_002_COUNT
                            where bg.stock_sheet_number == glay.vwstring0
                             orderby bg.stock_sheet_number
                             select bg;

                ViewBag.anapict = bglist.ToList();
            }
        }
        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[IV_002_COUNT] where stock_sheet_number=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            sqlstr = "delete from [dbo].[IV_002_CONTD] where stock_sheet_number=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        public ActionResult delete_detail(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[IV_002_CONTD] where cast(stock_sheet_number as varchar) +'[]'+ item_code =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Details");
            
        }

        private void read_details()
        {
            string hcount = psess.temp0.ToString();
            var bglist = from bh in db.IV_002_CONTD
                         join bg1 in db.IV_001_ITEM
                         on new { a1 = bh.item_code } equals new { a1 = bg1.item_code}
                         into bg2
                         from bg3 in bg2.DefaultIfEmpty()
                         where bh.stock_sheet_number == hcount

                         select new vw_genlay
                         {
                             vwstring0 = bh.stock_sheet_number,
                             vwstring6 = bg3.item_name,
                             vwint2 = bh.quantity,
                             vwint3 = bh.physical_count,
                             vwint4 = bh.difference,
                         };
            ViewBag.x1 = bglist.ToList();

        }
        [HttpPost]
        public ActionResult assign(string item_code)
        {
            
            var bgassign = (from bg in db.IV_001_ITEM
                            join bn in db.IV_001_ITMST
                            on new { a1 = bg.item_code } equals new { a1 = bn.item_code }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            join bf in db.GB_001_PCODE
                            on new { a2 = bg.sku_sequence } equals new { a2 = bf.parameter_code}
                            into bf1
                            from bf2 in bf1.DefaultIfEmpty()
                            where bg.item_code == item_code
                            select new { bg, bn2, bf2 }).FirstOrDefault();

            int bal_qty = bgassign.bn2.bal_qty;
            string bin_loc = bgassign.bn2.bin_location;
            string item_des = bgassign.bg.description;
            string sku_name = bgassign.bf2.parameter_name;


                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "sku", Text = sku_name });
                ary.Add(new SelectListItem { Value = "qty", Text = bal_qty.ToString()});
                ary.Add(new SelectListItem { Value = "loc", Text = bin_loc });
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
        public ActionResult diff_cal(string phy_count, string item_qty)
        {
           
            int.TryParse(item_qty, out qty);
            int.TryParse(phy_count, out phy);
            

             diff = qty - phy;


            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = diff.ToString("#,##0.00") });

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");

        }

        private void read_header()
        {
            string hcount = psess.temp0.ToString();
            var bg2list = (from bh in db.IV_002_COUNT
                           join bg in db.IV_001_WAREH
                           on new { a1 = bh.warehouse_code } equals new { a1= bg.warehouse_code}
                           where bh.stock_sheet_number== hcount
                           select new { bh, bg }).FirstOrDefault();

            
            vw_genlay glayhead=new vw_genlay();
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwstrarray1[0] = bg2list.bh.stock_sheet_number.ToString();
                glayhead.vwstrarray1[1] = util.date_slash(bg2list.bh.stock_count_date);
                glayhead.vwstrarray1[2] = bg2list.bh.period;
                glayhead.vwstrarray1[3] = bg2list.bg.warehouse_name;
                glayhead.vwstrarray1[4] = bg2list.bh.approval_level.ToString();
                glayhead.vwstrarray1[5] = bg2list.bh.approval_by;

                glay.vwstring0 = bg2list.bh.stock_sheet_number;
                glay.vwstring1 = util.date_slash(bg2list.bh.stock_count_date);
                glay.vwstring2 = bg2list.bh.period;
                glay.vwstring3 = bg2list.bh.warehouse_code;
                glay.vwstring4 = bg2list.bh.approval_level.ToString();
                glay.vwstring5 = bg2list.bh.approval_by;
                string s_sheet = glay.vwstring0;
                glay.vwint0 = Convert.ToInt16(s_sheet);
                string id_approv = glay.vwstring4;
                glay.vwint5 = Convert.ToInt16(id_approv);
                
            }
                              ViewBag.x2 = glayhead;
           

        }

        private void init_header()
        {
            gdoc.stock_sheet_number = "";
            gdoc.stock_count_date = "";
            gdoc.period = "";
            gdoc.warehouse_code = "";
            gdoc.approval_level= 0;
            gdoc.approval_by = "";
            gdoc.modified_by = "";
            gdoc.note = "";
            gdoc.attach_document = "";
            
            }

        private void move_detail()
        {
            string key1 = psess.temp0.ToString();
            string key2 = glay.vwstring1;
            var bgdtl = (from bg in db.IV_002_CONTD
                         where bg.stock_sheet_number == key1 && bg.item_code == key2
                         select bg).FirstOrDefault();
            glay.vwstring6 = bgdtl.item_code;
            glay.vwint2 = bgdtl.quantity;
            glay.vwint3 = bgdtl.physical_count;
            glay.vwint4 = bgdtl.difference;
        }


        //private void qty_update()
        //{
        //    string sqlstr = " update IV_002_COUNT set quote_total_amount = a001, quote_total_tax = a002, quote_total_discount = a003, quote_total_ext_price = a004, quote_total_qty = a005";
        //    sqlstr += " from IV_002_COUNT a, (select sum(quote_amount) a001, sum(tax_amount) a002, sum (discount_amount) a003, sum(ext_price) a004 , sum(quote_qty) from IV_002_CONTD where quote_reference=" + util.sqlquote(glay.vwstring9) + ") bx";
        //    sqlstr += " where quote_reference=" + util.sqlquote(glay.vwstring9);
        //    db.Database.ExecuteSqlCommand(sqlstr);

        //}



    }
}

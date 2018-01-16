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

    public class SalestController : Controller
    {
        //
        // GET: /Employee/

        AR_002_QUOTE AR_002_QUOTE = new AR_002_QUOTE();
        AR_002_SALES gdoc = new AR_002_SALES();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();

        string preseq = "";
        string ptype = "";
        decimal f = 0;
        decimal y = 0;
        bool err_flag = true;
        string set_price = "N";
        decimal qty = 0; decimal pricep = 0; decimal tax_amt = 0; decimal net_amt = 0; decimal quote_amt = 0; decimal ext_price = 0;
        decimal net_amt1 = 0; decimal quote_amt1 = 0; decimal qtx = 0;
        decimal disc = 0; decimal disc_per1 = 0;
        string action_flag = "";
        
        public ActionResult Index()
        {
            
            util.init_values();
            Session["horder"] = "";


            pubsess = (pubsess)Session["pubsess"];

            var bglist = from bh in db.AR_002_SALES
                         join bg1 in db.AR_001_CUSTM
                         on new { a2 = bh.customer_code } equals new { a2 = bg1.customer_code}
                         join bn in db.MC_001_CUREN
                         on new { b3 = bg1.currency_code } equals new { b3 = bn.currency_code}
                        
                         where bh.status == 01
                         select new vw_genlay
                         {
                             vwint0 = bh.sale_sequence_number,
                             vwstring0 = bh.quote_reference,
                             vwstring1 = bg1.cust_biz_name,
                             vwdecimal0 = bh.quote_total_amount,
                             vwstring3 = bh.quote_date,
                             vwstring4 = bh.quote_expiration_date,
                             vwstring2 = bn.currency_description
                         };

            return View(bglist.ToList());


        }
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
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
            glay.vwstring9 = Session["horder"].ToString();
            glay.vwdecimal5 = Convert.ToDecimal(Session["hseq"]);


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

            glay.vwstring9 = Session["horder"].ToString();
            glay.vwdecimal5 = Convert.ToDecimal(Session["hseq"]);

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
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
           
            read_header();
            read_details();
            select_query_head();

            return View(glay);

        }


        public ActionResult Edit(int key1)
        {
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            
            pubsess = (pubsess)Session["pubsess"];
            

            gdoc = db.AR_002_SALES.Find(key1);
            if (gdoc != null)
            {
                Session["horder"] = gdoc.quote_reference;
                Session["hseq"] = gdoc.sale_sequence_number;
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

        public ActionResult EditDetails(int key1, string key2, int key3  )
        {
            ViewBag.action_flag = "Edit";
            action_flag = "Edit";
            
            pubsess = (pubsess)Session["pubsess"];


            AR_002_QUOTE = db.AR_002_QUOTE.Find(key1, key2,key3);
            if (AR_002_QUOTE != null)
            {
                Session["horder"] = AR_002_QUOTE.quote_reference;
                Session["hseq"] = AR_002_QUOTE.sale_sequence_number;
                Session["line_num"] = AR_002_QUOTE.line_sequence;
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
            AR_002_QUOTE = db.AR_002_QUOTE.Find(glay.vwint0);
            if (AR_002_QUOTE != null)
            {
                db.AR_002_QUOTE.Remove(AR_002_QUOTE);
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
                    gdoc = new AR_002_SALES();
                    init_header();
                    gdoc.created_date = DateTime.UtcNow;
                    gdoc.created_by = pubsess.userid;
                    var headlist = (from bh in db.AP_001_PUROT
                                    where bh.parameter_code == "SLSEQ" && (bh.order_type == "01" || bh.order_type == "single")
                                    select bh).FirstOrDefault();
                    string pref = headlist.order_prefix;
                    int seq = headlist.order_sequence;
                    int num = headlist.numeric_size;
                    string sqlstr = " update AP_001_PUROT set order_sequence = order_sequence+1 where parameter_code = 'SLSEQ' and (order_type = '01' or order_type = 'single')";
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
                    glay.vwstring9 = preseq;
                    gdoc.sale_sequence_number = glay.vwint0;
                    gdoc.quote_reference = preseq;
                    Session["horder"] = preseq;
                }

                if (commandn == "detailsub")
                {
    
                    AR_002_QUOTE = new AR_002_QUOTE();
                    AR_002_QUOTE.created_by = pubsess.userid;
                    AR_002_QUOTE.created_date = DateTime.UtcNow;
                    string sqlstr = "select isnull(max(line_sequence),0) vwint0 from AR_002_QUOTE where quote_reference=" + util.sqlquote(glay.vwstring9);
                    var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
                    glay.vwdecimal4=sql1.vwint0+1;

                    
                }
            }
            else
            {
                if (commandn == "headsub")
                {
                    gdoc = db.AR_002_SALES.Find(Convert.ToInt16(glay.vwdecimal5));
                }
                else
                {
                    glay.vwdecimal4 = Convert.ToInt16(Session["line_num"]);
                    AR_002_QUOTE = db.AR_002_QUOTE.Find(Convert.ToInt16(glay.vwdecimal5), glay.vwstring9, Convert.ToInt16(glay.vwdecimal4));
                }
            }


            if (commandn == "headsub")
            {
                // the headsub button has been clicked



                gdoc.status = 01;

                gdoc.customer_code = glay.vwstrarray0[0];
                gdoc.exchange_rate = 0;
                gdoc.transaction_date = util.date_yyyymmdd(glay.vwstrarray0[1]);
                gdoc.quote_date = util.date_yyyymmdd(glay.vwstrarray0[2]);
                gdoc.quote_expiration_date = util.date_yyyymmdd(glay.vwstrarray0[3]);
                //gdoc.payment_term_code = glay.vwstrarray0[4];
                gdoc.quote_total_ext_price = 0;
                gdoc.modified_by = pubsess.userid;
                gdoc.modified_date = DateTime.UtcNow;

            }
            else
            {
                // the detailsub button has been clicked

                ext_price = glay.vwdclarray0[1] * glay.vwdclarray0[0];
                disc = glay.vwdclarray0[3];
                disc_per1 = glay.vwdclarray0[4];

                re_cal(glay.vwstring1);
                AR_002_QUOTE.sale_sequence_number = Convert.ToInt16(glay.vwdecimal5);
                AR_002_QUOTE.quote_reference = glay.vwstring9;
                AR_002_QUOTE.line_sequence = Convert.ToInt16(glay.vwdecimal4);
                AR_002_QUOTE.item_code = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
                AR_002_QUOTE.quote_qty = glay.vwdclarray0[1];
                AR_002_QUOTE.price = glay.vwdclarray0[0];
                AR_002_QUOTE.tax_amount = tax_amt;
                AR_002_QUOTE.base_amount = 0;
                AR_002_QUOTE.ext_price = ext_price;
                AR_002_QUOTE.discount_percent = glay.vwdclarray0[4];
                AR_002_QUOTE.discount_amount = glay.vwdclarray0[3];
                AR_002_QUOTE.net_amount = net_amt;
                AR_002_QUOTE.quote_amount = quote_amt;

                AR_002_QUOTE.modified_date = DateTime.UtcNow;
                AR_002_QUOTE.modified_by = pubsess.userid;
            }    

           if(action_flag == "Create")
            {
                if (commandn == "detailsub")
                {
                    db.Entry(AR_002_QUOTE).State = EntityState.Added;
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
                    db.Entry(AR_002_QUOTE).State = EntityState.Modified;
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

            if (commandn == "headsub")
                Session["hseq"] = gdoc.sale_sequence_number;

            if (err_flag && commandn == "detailsub")
            {
                quote_update();
            }
          

        }
        private void validation_routine(string commandn)
        {
            string error_msg = "";
            if (commandn == "detailsub")
            {
                if (glay.vwdclarray0[3] != 0 && glay.vwdclarray0[4] != 0)
                    error_msg = "You can either enter Discount Amount or Discount Percent";
            }
            

            //if (string.IsNullOrWhiteSpace(glay.vwstring1))
            //    error_msg = "Please enter Name;

            //if (Session["action_flag"].ToString() == "Index")
            //{
            //    AR_002_QUOTE bnk = db.AR_002_QUOTE.Find(glay.vwint0);
            //    if (bnk != null)
            //        error_msg = "Can not accept duplicates";
            //}

            var headlist = (from bh in db.AP_001_PUROT
                            where bh.parameter_code == "SLSEQ" && (bh.order_type == "01" || bh.order_type == "single")
                            select bh).FirstOrDefault();
            if (headlist == null)
            {
                error_msg = "Quote sequencing number not created";
            }

            if (error_msg != "")
            {
                ModelState.AddModelError(String.Empty, error_msg);
                err_flag = false;
            }
        }
        private void select_query_head()
        {

            var emp = from pf in db.AR_001_CUSTM
                      orderby pf.cust_biz_name
                      select pf;
            ViewBag.cus = new SelectList(emp.ToList(), "customer_code", "cust_biz_name", glay.vwstrarray0[0]);

            var empi = from pf in db.AP_001_PTERM
                       orderby pf.description
                      select pf;
            ViewBag.pay = new SelectList(empi.ToList(), "payment_term_code", "description", glay.vwstrarray0[4]);

           
            var empe = from pf in db.IV_001_ITEM
                       orderby pf.item_name
                       select pf;
            ViewBag.item = new SelectList(empe.ToList(), "item_code", "item_name", glay.vwstring1);
        }

        private void initial_rtn()
        {

            glay.vwdclarray0 = new decimal[50];
            glay.vwstrarray0 = new string[20];
            glay.vwstrarray1 = new string[20];
            for (int wctr = 0; wctr < 20; wctr++)
                glay.vwstrarray0[wctr] = "";
            glay.vwstrarray0[1] = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwstrarray0[2] = DateTime.Now.ToString("dd/MM/yyyy");
            glay.vwstrarray0[3] = DateTime.Now.ToString("dd/MM/yyyy");


        }

        private void read_record(string commandn)
        {
            if (commandn == "detailsub")
            {
                glay.vwdecimal5 = Convert.ToDecimal(AR_002_QUOTE.sale_sequence_number);
                glay.vwdecimal4 = Convert.ToDecimal(AR_002_QUOTE.line_sequence);
                glay.vwstring9 = AR_002_QUOTE.quote_reference;
                glay.vwstring1 = AR_002_QUOTE.item_code;
                glay.vwdclarray0[1] = AR_002_QUOTE.quote_qty;
                glay.vwdclarray0[0] = AR_002_QUOTE.price;
                tax_amt = AR_002_QUOTE.tax_amount;
                ext_price = AR_002_QUOTE.ext_price;
                glay.vwdclarray0[4] = AR_002_QUOTE.discount_percent;
                glay.vwdclarray0[3] = AR_002_QUOTE.discount_amount;
                //glay.vwdclarray0[6] = AR_002_QUOTE.base_amount;
                net_amt = AR_002_QUOTE.net_amount;
                quote_amt = AR_002_QUOTE.quote_amount;

            }
            else
            {
                var bglist = from bg in db.AR_002_SALES
                            where bg.sale_sequence_number == glay.vwint0
                             orderby bg.sale_sequence_number
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
            string sqlstr = "delete from [dbo].[AR_002_SALES] where sale_sequence_number=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
             sqlstr = "delete from [dbo].[AR_002_QUOTE] where sale_sequence_number=" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            return RedirectToAction("Index");
        }

        public ActionResult delete_detail(string id)
        {
            // write your query statement
            glay.vwstring9 = Session["horder"].ToString();
            string sqlstr = "delete from [dbo].[AR_002_QUOTE] where cast(sale_sequence_number as varchar) +'[]'+ quote_reference +'[]'+ cast(line_sequence as varchar) =" + util.sqlquote(id);
            db.Database.ExecuteSqlCommand(sqlstr);
            quote_update();
            return RedirectToAction("Details");
            
        }

        private void read_details()
        {
            string sorder = Session["horder"].ToString();
            var bglist = from bh in db.AR_002_QUOTE
                         join bg1 in db.IV_001_ITEM
                         on new { a1 = bh.item_code } equals new { a1 = bg1.item_code}
                         into bg2
                         from bg3 in bg2.DefaultIfEmpty()
                         join bh4 in db.GB_001_PCODE
                         on new{ a1 = bg3.sku_sequence, a2 = "10"} equals new {a1 = bh4.parameter_code, a2 = bh4.parameter_type}
                         into bh5
                         from bh6 in bh5.DefaultIfEmpty()
                         where bh.quote_reference == sorder

                         select new vw_genlay
                         {
                             vwint0 = bh.sale_sequence_number,
                             vwint1 = bh.line_sequence,
                             vwstring1 = bh.quote_reference,
                             vwstring2 = bg3.item_name,
                             vwstring3 = bh6.parameter_name,
                             vwdecimal0 = bh.price,
                             vwdecimal1 = bh.quote_qty,
                             vwdecimal2 = bh.ext_price,
                             vwdecimal3 = bh.tax_amount,
                             vwdecimal4 = bh.discount_amount,
                             vwdecimal5 = bh.discount_percent,
                             vwdecimal6 = bh.quote_amount
                         };
            ViewBag.x1 = bglist.ToList();

        }
        [HttpPost]
        public ActionResult assign(string item_code)
        {
            
            var bgassign = (from bg in db.IV_001_ITEM
                            join bn in db.GB_001_PCODE
                            on new { a1 = bg.sku_sequence } equals new { a1 = bn.parameter_code }
                            into bn1
                            from bn2 in bn1.DefaultIfEmpty()
                            where bg.item_code == item_code
                            select new { bg, bn2 }).FirstOrDefault();

            string sku_name = bgassign.bn2.parameter_name;
            string price_dis = bgassign.bg.item_type;

            var edit_chk = (from cy in db.GB_001_COY where cy.id_code=="COYPRICE"
                            select cy).FirstOrDefault();

            string price_edt = edit_chk.field12;
            if (price_edt == "N" && price_dis == "I")
            {
                set_price = "Y";
            }
           
            var bgfind = (from bh in db.AR_001_CUSTM
                              select bh).FirstOrDefault();
           
                string cust_class = bgfind.price_class;

                if (cust_class == "2")
                    f = bgassign.bg.selling_price_class2;
                else if (cust_class == "3")
                    f = bgassign.bg.selling_price_class3;

                else if (cust_class == "4")
                    f = bgassign.bg.selling_price_class4;
                else if (cust_class == "5")
                    f = bgassign.bg.selling_price_class5;
                else if (cust_class == "6")
                    f = bgassign.bg.selling_price_class6;
                else  
                    f = bgassign.bg.selling_price_class1;


                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "sku", Text = sku_name });
                ary.Add(new SelectListItem { Value = "price", Text = f.ToString() });
                ary.Add(new SelectListItem { Value = "price_flag", Text = set_price });
                

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
        public ActionResult price_cal(string item_price, string item_qty, string item_code, string disc_amt, string disc_per)
        {
           
            //qty = Convert.ToDecimal(item_qty);
           // price = Convert.ToDecimal(item_price);
            decimal.TryParse(item_qty, out qty);
            decimal.TryParse(item_price, out pricep);
            decimal.TryParse(disc_amt, out disc);
            decimal.TryParse(disc_per, out disc_per1);

             ext_price = qty * pricep;

            re_cal(item_code);

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = ext_price.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "2", Text = tax_amt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "3", Text = net_amt.ToString("#,##0.00") });
            ary.Add(new SelectListItem { Value = "4", Text = quote_amt.ToString("#,##0.00") });

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
        public ActionResult dis_cal(string item_dis, string item_extprice)
        {

             decimal quote_amt1 = 0;
            disc = Convert.ToDecimal(item_dis);
            net_amt1 = Convert.ToDecimal(item_extprice);
            
            quote_amt1 = net_amt1 - disc;


            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = quote_amt1.ToString("#,##0.00") });
            

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
        public ActionResult dis_cal1(string item_dis, string item_extprice)
        {

            decimal disc = 0; decimal quote_amt1 = 0; decimal qtx = 0;
            disc = Convert.ToDecimal(item_dis);
            net_amt1 = Convert.ToDecimal(item_extprice);
            qtx = disc / 100 * net_amt1;
            quote_amt1 = net_amt1 - qtx;

            List<SelectListItem> ary = new List<SelectListItem>();
            ary.Add(new SelectListItem { Value = "1", Text = quote_amt1.ToString("#,##0.00") });


            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);
            //}
            return RedirectToAction("Index");


        }
        private void re_cal(string item_code)
        {
            net_amt = ext_price - disc-(disc_per1*ext_price/100);

            var bgassign = (from bg in db.IV_001_ITEM
                            where bg.item_code == item_code
                            select bg).FirstOrDefault();
            if (bgassign != null)
            {
                decimal tax1 = tax_calculation(bgassign.tax_code1, net_amt);
                decimal tax2 = tax_calculation(bgassign.tax_code2, net_amt);
                decimal tax3 = tax_calculation(bgassign.tax_code3, net_amt);
                decimal tax4 = tax_calculation(bgassign.tax_code4, net_amt);
                decimal tax5 = tax_calculation(bgassign.tax_code5, net_amt);

                tax_amt = tax1 + tax2 + tax3 + tax4 + tax5;
            }
            quote_amt = net_amt - tax_amt;

        }
        private void read_header()
        {
           
            string sorder = Session["horder"].ToString();
            var bg2list = (from bh in db.AR_002_SALES
                          join bh1 in db.AR_001_CUSTM
                          on new { a1 = bh.customer_code } equals new { a1=bh1.customer_code}
                          join bg in db.AP_001_PTERM
                          on new { a1 = bh.payment_term_code } equals new { a1 = bg.payment_term_code}
                          into bg1
                          from bg2 in bg1.DefaultIfEmpty()
                          where bh.quote_reference == sorder && bh.status == 01
                          select new {bh,bh1,bg2}).FirstOrDefault();

            
            vw_genlay glayhead=new vw_genlay();
            if (bg2list != null)
            {
                glayhead.vwstrarray1 = new string[20];
                glayhead.vwint0 = bg2list.bh.sale_sequence_number;
                glayhead.vwstrarray1[2] = bg2list.bh.quote_reference;
                glayhead.vwstrarray1[0] = bg2list.bh1.cust_biz_name;
                glayhead.vwstring2 = util.date_slash(bg2list.bh.transaction_date);
                glayhead.vwstrarray1[3] = util.date_slash(bg2list.bh.quote_date);
                glayhead.vwstrarray1[4] = util.date_slash(bg2list.bh.quote_expiration_date);
                glayhead.vwstrarray1[1] = bg2list.bg2.description;
                glayhead.vwstrarray1[9] = bg2list.bh.quote_total_amount.ToString("#,##0.00");
                glayhead.vwstrarray1[5] = bg2list.bh.quote_total_qty.ToString("#,##0");
                glayhead.vwstrarray1[6] = bg2list.bh.quote_total_ext_price.ToString("#,##0.00");
                glayhead.vwstrarray1[7] = bg2list.bh.quote_total_discount.ToString("#,##0.00");
                glayhead.vwstrarray1[8] = bg2list.bh.quote_total_tax.ToString("#,##0.00");

                glay.vwstrarray0[0] = bg2list.bh1.customer_code;
                glay.vwstrarray0[1] = util.date_slash(bg2list.bh.transaction_date);
                glay.vwstrarray0[2] = util.date_slash(bg2list.bh.quote_date);
                glay.vwstrarray0[3] = util.date_slash(bg2list.bh.quote_expiration_date);
                glay.vwstrarray0[4] = bg2list.bg2.description;
                
            }
                              ViewBag.x2 = glayhead;
           

        }

        private void init_header()
        {
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
            gdoc.currency_code = "";
            gdoc.customer_code = "";
            gdoc.delivery_address_code = "";
            gdoc.exchange_rate = 0;
            gdoc.expected_delivery_date = "";
            gdoc.item_warehouse_code = "";
            gdoc.approval_by = "";
            gdoc.modified_by = "";
            gdoc.note = "";
            gdoc.order_date = "";
            gdoc.order_expiration_date = "";
            gdoc.payment_term_code = "";
            gdoc.project_code = "";
            gdoc.quote_date = "";
            gdoc.quote_expiration_date = "";
            gdoc.sales_transaction_type = "";
            gdoc.transaction_date = "";
            gdoc.quote_reference = "";
            gdoc.invoice_reference = "";
            gdoc.order_reference = "";
            
            }

        private decimal tax_calculation(string icode, decimal amount)
        {
            var bgtax = (from bg in db.GB_001_TAX
                         where bg.tax_code == icode
                         select bg).FirstOrDefault();
            if (bgtax == null)
                return 0;
            if (bgtax.module_basis == "P")
                return 0;
            if (bgtax.computation_basis == "T")
                return 0;
            decimal tax = 0;
            tax = amount * bgtax.tax_rate / 100;
            if (bgtax.tax_impact == "S")
                tax = 0 - tax;
            return tax;

            
        }
        private void move_detail()
        {
               int key1 = Convert.ToInt16(Session["hseq"]);
               string key2 = Session["horder"].ToString();
               int key3 = Convert.ToInt16(Session["line_num"]);
            var bgdtl = (from bg in db.AR_002_QUOTE
                         where bg.sale_sequence_number == key1 && bg.quote_reference == key2 && bg.line_sequence == key3
                         select bg).FirstOrDefault();
            glay.vwstring1 = bgdtl.item_code;
            glay.vwdclarray0[0] = bgdtl.price;
            glay.vwdclarray0[1] = bgdtl.quote_qty;
            glay.vwdecimal1 = bgdtl.ext_price;
            glay.vwdclarray0[3] = bgdtl.discount_amount;
            glay.vwdclarray0[4] = bgdtl.discount_percent;
            glay.vwdclarray0[2] = bgdtl.net_amount;
            glay.vwdecimal0 = bgdtl.tax_amount;
            glay.vwdclarray0[5] = bgdtl.quote_amount;
            glay.vwdecimal4 = bgdtl.line_sequence;
        }

        private void quote_update()
        {
            string sqlstr = " update AR_002_SALES set quote_total_amount = a001, quote_total_tax = a002, quote_total_discount = a003, quote_total_ext_price = a004, quote_total_qty = a005";
            sqlstr += " from AR_002_SALES a, (select sum(quote_amount) a001, sum(tax_amount) a002, sum (discount_amount) a003, sum(ext_price) a004, sum(quote_qty)a005 from AR_002_QUOTE where quote_reference=" + util.sqlquote(glay.vwstring9) + ") bx";
            sqlstr += " where quote_reference=" + util.sqlquote(glay.vwstring9);
            db.Database.ExecuteSqlCommand(sqlstr);

        }

        //private void qty_update()
        //{
        //    string sqlstr = " update AR_002_SALES set quote_total_amount = a001, quote_total_tax = a002, quote_total_discount = a003, quote_total_ext_price = a004, quote_total_qty = a005";
        //    sqlstr += " from AR_002_SALES a, (select sum(quote_amount) a001, sum(tax_amount) a002, sum (discount_amount) a003, sum(ext_price) a004 , sum(quote_qty) from AR_002_QUOTE where quote_reference=" + util.sqlquote(glay.vwstring9) + ") bx";
        //    sqlstr += " where quote_reference=" + util.sqlquote(glay.vwstring9);
        //    db.Database.ExecuteSqlCommand(sqlstr);

        //}



    }
}

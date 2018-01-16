using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using CittaErp.utilities;



namespace CittaErp.utilities
{
    public class discountfsp
    {
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        cittautil util = new cittautil();
        queryhead qlay = new queryhead();
        pubsess com = new pubsess();

        public decimal[] quote_tax_rtn(string item_code, decimal ext_price)
        {
            decimal tax1 = 0; decimal tax2 = 0; decimal tax3 = 0; decimal tax4 = 0; decimal tax5 = 0;
            decimal man_disc = 0;
            decimal[] decarray = new decimal[20];

            queryhead qheader = (queryhead)HttpContext.Current.Session["qheader"];
            string cust_code = qheader.query1;
            int hbug = qheader.intquery0;

            decimal net_amt = ext_price;

            //man_disc = disc == 0 ? (disc_per1 * ext_price / 100) : disc;
            //net_amt = net_amt - man_disc;

            if (qheader.query4 == "N")
            {
                var bgassign = (from bg in db.IV_001_ITEM
                                where bg.item_code == item_code
                                select bg).FirstOrDefault();

                if (bgassign != null)
                {
                    tax1 = tax_calculation(bgassign.tax_code1, net_amt, "S", "L");
                    tax2 = tax_calculation(bgassign.tax_code2, net_amt, "S", "L");
                    tax3 = tax_calculation(bgassign.tax_code3, net_amt, "S", "L");
                    tax4 = tax_calculation(bgassign.tax_code4, net_amt, "S", "L");
                    tax5 = tax_calculation(bgassign.tax_code5, net_amt, "S", "L");
                }
            }

            decimal quote_amt = net_amt + tax1 + tax2 + tax3 + tax4 + tax5;

            decarray[ 0] = net_amt;
            decarray[ 1] = 0;
            decarray[ 2] = tax1;
            decarray[ 3] = tax2;
            decarray[ 4] = tax3;
            decarray[ 5] = tax4;
            decarray[ 6] = tax5;
            decarray[7] = quote_amt;
            decarray[8] = 0;

            return decarray;

        }

        public string[,] itemdis_calculation(string hbug,int tablecode=1)
        {
            string str1 = "";
            string[,] retarray = new string[30, 20];
            string[] recdummy= new string[30];
            int rowctr = 0;
            decimal promo_amt = 0; decimal promoq_amt = 0; decimal chck = 0; decimal promo_qty = 0; decimal promo_disc = 0;
            decimal discount_amt = 0; decimal tot_ext_price = 0;
            decimal tax1 = 0; decimal tax2 = 0; decimal tax3 = 0; decimal tax4 = 0; decimal tax5 = 0;
            string table_name="";
            if (tablecode == 1)
                table_name = " AR_002_QUOTE ";
            else if (tablecode == 2)
                table_name = " AR_002_SODT ";


            str1 = "delete from " + table_name + " where dis_flag <> 'N' and sale_sequence_number=" + hbug;
            db.Database.ExecuteSqlCommand(str1);

            queryhead qheader = (queryhead)HttpContext.Current.Session["qheader"];
            string cust_code = qheader.query1;


            com = (pubsess)HttpContext.Current.Session["pubsess"];

            string str2z = "select line_sequence from " + table_name + " where (discount_amount<> 0 or discount_percent<>0) and 'N'=" + util.sqlquote(com.manual_others);
            str2z += " and sale_sequence_number=" + hbug;

// manu discount
            str1 = "select item_code as vwstring0,ext_price as vwdecimal0, discount_percent as vwdecimal1, discount_amount vwdecimal2, line_sequence vwint0  ";
            str1 += " from " + table_name + " where sale_sequence_number=" + hbug.ToString();
            str1 += " and (discount_percent<>0 or discount_amount <> 0) ";
            var bgla = db.Database.SqlQuery<vw_genlay>(str1);

            foreach (var item in bgla.ToList())
            {
                decimal man_disc = item.vwdecimal2 == 0 ? (item.vwdecimal1 * item.vwdecimal0 / 100) : item.vwdecimal2;
                man_disc = 0 - man_disc;

                if (man_disc != 0)
                {
                    if (qheader.query4 == "N")
                    {
                        var bgassign = (from bg in db.IV_001_ITEM
                                        where bg.item_code == item.vwstring0
                                        select bg).FirstOrDefault();

                        if (bgassign != null)
                        {
                            tax1 = tax_calculation(bgassign.tax_code1, man_disc, "S", "L");
                            tax2 = tax_calculation(bgassign.tax_code2, man_disc, "S", "L");
                            tax3 = tax_calculation(bgassign.tax_code3, man_disc, "S", "L");
                            tax4 = tax_calculation(bgassign.tax_code4, man_disc, "S", "L");
                            tax5 = tax_calculation(bgassign.tax_code5, man_disc, "S", "L");
                        }
                    }

                    decimal quote_amt = man_disc + tax1 + tax2 + tax3 + tax4 + tax5;
                    retarray[rowctr, 0] = item.vwstring0;
                    retarray[rowctr, 1] = (item.vwdecimal0 + man_disc).ToString();
                    retarray[rowctr, 2] = man_disc.ToString();
                    retarray[rowctr, 3] = tax1.ToString();
                    retarray[rowctr, 4] = tax2.ToString();
                    retarray[rowctr, 5] = tax3.ToString();
                    retarray[rowctr, 6] = tax4.ToString();
                    retarray[rowctr, 7] = tax5.ToString();
                    retarray[rowctr, 8] = "0";
                    retarray[rowctr, 9] = "0";
                    retarray[rowctr, 10] = "M";
                    retarray[rowctr, 11] = "0";
                    retarray[rowctr, 12] = "0";
                    retarray[rowctr, 13] = item.vwint0.ToString();
                    retarray[rowctr, 14] = quote_amt.ToString();
                    rowctr++;
                }
            }
            

            // flat discount
            tax1 = 0; tax2 = 0; tax3 = 0; tax4 = 0; tax5 = 0; 
            str1 = "select ax.item_code as vwstring0,isnull(sum(ax.ext_price),0) as vwdecimal0, isnull(sum(ax.quote_qty),0) as vwdecimal1, max(line_sequence) vwint0 ";
            str1 += " from " + table_name + " ax where sale_sequence_number=" + hbug.ToString() + " and line_sequence not in ( " + str2z + ")";
            str1 += " and item_code in (select a.item_code from IV_001_ITEM a, DC_001_DISC b where a.discount_code=b.discount_code and b.discount_count=0 and b.stepped_discount_active='F' ) ";
            str1 += " group by item_code ";
            var bgl = db.Database.SqlQuery<vw_genlay>(str1);

            foreach (var item in bgl.ToList())
            {

                var bgassigni = (from bg in db.IV_001_ITEM
                                 join bm in db.DC_001_DISC
                                 on new { a1 = bg.discount_code } equals new { a1 = bm.discount_code }
                                 into bm1
                                 from bm2 in bm1.DefaultIfEmpty()
                                 where bg.item_code == item.vwstring0 && bm2.discount_count == 0 && bm2.stepped_discount_active == "F"
                                 select new { bg, bm2 }).FirstOrDefault();

                if (bgassigni != null)
                {
                    decimal fdisper = bgassigni.bm2.discount_percent;
                    if (fdisper == 0)
                        discount_amt = bgassigni.bm2.discount_amount;
                    else
                        discount_amt = item.vwdecimal0 * bgassigni.bm2.discount_percent / 100;

                    discount_amt = 0 - discount_amt;
                    if (discount_amt != 0)
                    {
                        if (qheader.query4 == "N")
                        {
                            tax1 = tax_calculation(bgassigni.bg.tax_code1, discount_amt, "S", "L");
                            tax2 = tax_calculation(bgassigni.bg.tax_code2, discount_amt, "S", "L");
                            tax3 = tax_calculation(bgassigni.bg.tax_code3, discount_amt, "S", "L");
                            tax4 = tax_calculation(bgassigni.bg.tax_code4, discount_amt, "S", "L");
                            tax5 = tax_calculation(bgassigni.bg.tax_code5, discount_amt, "S", "L");
                        }

                        decimal quote_amt = discount_amt + tax1 + tax2 + tax3 + tax4 + tax5;
                        retarray[rowctr, 0] = item.vwstring0;
                        retarray[rowctr, 1] = (item.vwdecimal0 + discount_amt).ToString();
                        retarray[rowctr, 2] = discount_amt.ToString();
                        retarray[rowctr, 3] = tax1.ToString();
                        retarray[rowctr, 4] = tax2.ToString();
                        retarray[rowctr, 5] = tax3.ToString();
                        retarray[rowctr, 6] = tax4.ToString();
                        retarray[rowctr, 7] = tax5.ToString();
                        retarray[rowctr, 8] = "0";
                        retarray[rowctr, 9] = item.vwdecimal1.ToString();
                        retarray[rowctr, 10] = "F";
                        retarray[rowctr, 11] = "0"; //(item.vwdecimal0 / item.vwdecimal1).ToString();
                        retarray[rowctr, 12] = "0";
                        retarray[rowctr, 13] = item.vwint0.ToString();
                        retarray[rowctr, 14] = quote_amt.ToString();
                        rowctr++;
                    }
                }
            }



            //promotion calculation
            tax1 = 0; tax2 = 0; tax3 = 0; tax4 = 0; tax5 = 0;
            str1 = "select ax.item_code as vwstring0,sum(ax.ext_price) as vwdecimal0, sum(ax.quote_qty) as vwdecimal1, max(line_sequence) vwint0, ";
            str1 += "  promo_criteria as vwstring4,IV_001_ITEM.discount_code as vwstring3, min(DC_001_DISC.discount_percent) vwdecimal2, min(DC_001_DISC.discount_amount) vwdecimal3, min(qualified_quantity) vwdecimal4 ";
            str1 += " from " + table_name + " ax, DC_001_DISC, IV_001_ITEM where sale_sequence_number=" + hbug + " and IV_001_ITEM.item_code=ax.item_code and";
            str1 += " IV_001_ITEM.discount_code=DC_001_DISC.discount_code and stepped_discount_active in ('P') and ax.dis_flag ='N'";
            str1 += " and line_sequence not in ( " + str2z + ")";
            str1 += " group by ax.item_code, promo_criteria,IV_001_ITEM.discount_code ";

            var str2 = db.Database.SqlQuery<vw_genlay>(str1).ToList();

            foreach (var item in str2)
            {
                promo_amt = 0; promoq_amt = 0; chck = 0; promo_qty = 0; promo_disc = 0;

                var bgassigni = (from bg in db.IV_001_ITEM
                                 join bm in db.DC_001_DISC
                                 on new { a1 = bg.discount_code } equals new { a1 = bm.discount_code }
                                 into bm1
                                 from bm2 in bm1.DefaultIfEmpty()
                                 where bg.item_code == item.vwstring0 && bm2.discount_count == 0
                                 select new { bg, bm2 }).FirstOrDefault();


                if (item.vwstring4 == "E")
                {
                    decimal qty_ratio = Convert.ToInt32(item.vwdecimal1) / Convert.ToInt32(item.vwdecimal4);
                    promo_qty = qty_ratio * item.vwdecimal3;
                }
                else
                {
                    if (item.vwdecimal1 >= item.vwdecimal4)
                        promo_qty = item.vwdecimal3;

                }
                promo_amt = (item.vwdecimal0 / item.vwdecimal1) * promo_qty;
                promo_disc = promo_amt * item.vwdecimal2 / 100;
                promoq_amt = promo_amt - promo_disc;

                promo_disc = 0 - promo_disc;
                promoq_amt = 0 - promoq_amt;

                if (promo_disc != 0)
                {
                    if (qheader.query4 == "N")
                    {
                        tax1 = tax_calculation(bgassigni.bg.tax_code1, promo_disc, "S", "L");
                        tax2 = tax_calculation(bgassigni.bg.tax_code2, promo_disc, "S", "L");
                        tax3 = tax_calculation(bgassigni.bg.tax_code3, promo_disc, "S", "L");
                        tax4 = tax_calculation(bgassigni.bg.tax_code4, promo_disc, "S", "L");
                        tax5 = tax_calculation(bgassigni.bg.tax_code5, promo_disc, "S", "L");
                    }

                    decimal quote_amt = promo_disc + tax1 + tax2 + tax3 + tax4 + tax5;
                    retarray[rowctr, 0] = item.vwstring0;
                    retarray[rowctr, 1] = (promoq_amt).ToString();
                    retarray[rowctr, 2] = (promo_disc).ToString();
                    retarray[rowctr, 3] = tax1.ToString();
                    retarray[rowctr, 4] = tax2.ToString();
                    retarray[rowctr, 5] = tax3.ToString();
                    retarray[rowctr, 6] = tax4.ToString();
                    retarray[rowctr, 7] = tax5.ToString();
                    retarray[rowctr, 8] = "0";
                    retarray[rowctr, 9] = promo_qty.ToString();
                    retarray[rowctr, 10] = "Y";
                    retarray[rowctr, 11] = (item.vwdecimal0 / item.vwdecimal1).ToString();
                    retarray[rowctr, 12] = "0";
                    retarray[rowctr, 13] = item.vwint0.ToString();
                    retarray[rowctr, 14] = quote_amt.ToString();
                    rowctr++;
                }
            }


            //stepped discount

            tax1 = 0; tax2 = 0; tax3 = 0; tax4 = 0; tax5 = 0;
            str1 = "select ax.item_code as vwstring0,sum(ax.ext_price) as vwdecimal0, sum(ax.quote_qty) as vwdecimal1, ";
            str1 += " stepped_criteria as vwstring1, DC_001_DISC.discount_code vwstring3, max(line_sequence) vwint0 ";
            str1 += " from " + table_name + " ax, DC_001_DISC,IV_001_ITEM where sale_sequence_number=" + hbug + " and IV_001_ITEM.item_code=ax.item_code and ";
            str1 += " IV_001_ITEM.discount_code=DC_001_DISC.discount_code and stepped_discount_active in ('S') and ax.dis_flag ='N' and discount_count = 0 ";
            str1 += " and line_sequence not in ( " + str2z + ")";
            str1 += " group by ax.item_code, stepped_criteria, DC_001_DISC.discount_code ";

            decimal step_amt = 0; decimal step_qty = 0; decimal step_disc = 0;

            str2 = db.Database.SqlQuery<vw_genlay>(str1).ToList();
            foreach (var item in str2.ToList())
            {
                var bgassigni = (from bg in db.IV_001_ITEM
                                 join bm in db.DC_001_DISC
                                 on new { a1 = bg.discount_code } equals new { a1 = bm.discount_code }
                                 into bm1
                                 from bm2 in bm1.DefaultIfEmpty()
                                 where bg.item_code == item.vwstring0 && bm2.discount_count == 0
                                 select new { bg, bm2 }).FirstOrDefault();

                promo_amt = 0; promoq_amt = 0; chck = 0; promo_qty = 0; promo_disc = 0;
                if (item.vwstring1 == "E")
                {

                    string stx = "select discount_percent as dquery0, discount_amount as dquery1, lower_limit as intquery0, upper_limit as intquery1 from DC_001_DISC";
                    stx += " where discount_code =" + util.sqlquote(item.vwstring3) + "and discount_count != 0";
                    var str3 = db.Database.SqlQuery<querylay>(stx).ToList();
                    decimal actqty = item.vwdecimal1;
                    foreach (var ikem in str3.ToList())
                    {
                        if (actqty >= ikem.intquery1)
                            step_qty = ikem.intquery1;
                        else
                            step_qty = actqty;

                        step_amt = (item.vwdecimal0 / item.vwdecimal1) * step_qty;
                        step_disc = ikem.dquery0 == 0 ? ikem.dquery1 : step_amt * ikem.dquery0 / 100;
                        promo_qty += step_qty;
                        promo_amt += step_amt;
                        promo_disc += step_disc;
                        actqty = actqty - step_qty;
                        if (actqty == 0)
                            break;
                    }
                }
                else
                {
                    string stx = "select discount_percent as dquery0, discount_amount as dquery1, lower_limit as intquery0, upper_limit as intquery1 from DC_001_DISC";
                    stx += " where discount_code =" + util.sqlquote(item.vwstring3) + "and discount_count != 0";
                    stx += " and " + item.vwdecimal1.ToString() + " between lower_limit and upper_limit ";
                    var str31 = db.Database.SqlQuery<querylay>(stx).FirstOrDefault();
                    if (str31 != null)
                    {
                        promo_qty = item.vwdecimal1;
                        promo_amt = (item.vwdecimal0 / item.vwdecimal1) * promo_qty;
                        promo_disc = str31.dquery0 == 0 ? str31.dquery1 : str31.dquery0 * promo_amt / 100;
                    }
                }

                promoq_amt = promo_amt - promo_disc;
                promo_disc = 0 - promo_disc;
                promoq_amt = 0 - promoq_amt;

                if (promo_disc != 0)
                {
                    if (qheader.query4 == "N")
                    {
                        tax1 = tax_calculation(bgassigni.bg.tax_code1, promo_disc, "S", "L");
                        tax2 = tax_calculation(bgassigni.bg.tax_code2, promo_disc, "S", "L");
                        tax3 = tax_calculation(bgassigni.bg.tax_code3, promo_disc, "S", "L");
                        tax4 = tax_calculation(bgassigni.bg.tax_code4, promo_disc, "S", "L");
                        tax5 = tax_calculation(bgassigni.bg.tax_code5, promo_disc, "S", "L");
                    }

                    decimal quote_amt = promo_disc + tax1 + tax2 + tax3 + tax4 + tax5;
                    retarray[rowctr, 0] = item.vwstring0;
                    retarray[rowctr, 1] = (promoq_amt).ToString();
                    retarray[rowctr, 2] = (promo_disc).ToString();
                    retarray[rowctr, 3] = tax1.ToString();
                    retarray[rowctr, 4] = tax2.ToString();
                    retarray[rowctr, 5] = tax3.ToString();
                    retarray[rowctr, 6] = tax4.ToString();
                    retarray[rowctr, 7] = tax5.ToString();
                    retarray[rowctr, 8] = "0";
                    retarray[rowctr, 9] = promo_qty.ToString();
                    retarray[rowctr, 10] = "S";
                    retarray[rowctr, 11] = (item.vwdecimal0 / item.vwdecimal1).ToString();
                    retarray[rowctr, 12] = "0";
                    retarray[rowctr, 13] = item.vwint0.ToString();
                    retarray[rowctr, 14] = quote_amt.ToString();
                    rowctr++;
                }
            }

            // special discount
            var spl_dis = (from sp in db.AR_001_CUSTM
                           where sp.customer_code == cust_code
                           select sp).FirstOrDefault();

            string spl_com = spl_dis.special_discount;
            decimal spl_disper = spl_dis.cust_discount_percent;
            tax1 = 0; tax2 = 0; tax3 = 0; tax4 = 0; tax5 = 0;
            discount_amt = 0;
            if (spl_com == "Y")
            {
                str1 = "select ax.item_code as vwstring0,isnull(sum(ax.ext_price),0) as vwdecimal0, isnull(sum(ax.quote_qty),0) as vwdecimal1 ";
                str1 += " from " + table_name + " ax where sale_sequence_number=" + hbug.ToString() + " and dis_flag='N' group by item_code ";
                var bgl2 = db.Database.SqlQuery<vw_genlay>(str1);

                foreach (var item in bgl2.ToList())
                {
                    var bgassigni = (from bg in db.IV_001_ITEM
                                     where bg.item_code == item.vwstring0
                                     select bg).FirstOrDefault();

                    promo_disc = spl_disper / 100 * item.vwdecimal0;
                    promo_disc = 0 - promo_disc;

                    if (qheader.query4 == "N" && promo_disc != 0)
                    {
                        tax1 += tax_calculation(bgassigni.tax_code1, promo_disc, "S", "L");
                        tax2 += tax_calculation(bgassigni.tax_code2, promo_disc, "S", "L");
                        tax3 += tax_calculation(bgassigni.tax_code3, promo_disc, "S", "L");
                        tax4 += tax_calculation(bgassigni.tax_code4, promo_disc, "S", "L");
                        tax5 += tax_calculation(bgassigni.tax_code5, promo_disc, "S", "L");
                    }

                    discount_amt += promo_disc;
                    tot_ext_price += item.vwdecimal0;
                }


                if (promo_disc != 0)
                {
                    decimal quote_amt = discount_amt + tax1 + tax2 + tax3 + tax4 + tax5;
                    retarray[rowctr, 0] = "";
                    retarray[rowctr, 1] = (tot_ext_price + discount_amt).ToString();
                    retarray[rowctr, 2] = discount_amt.ToString();
                    retarray[rowctr, 3] = tax1.ToString();
                    retarray[rowctr, 4] = tax2.ToString();
                    retarray[rowctr, 5] = tax3.ToString();
                    retarray[rowctr, 6] = tax4.ToString();
                    retarray[rowctr, 7] = tax5.ToString();
                    retarray[rowctr, 8] = "0";
                    retarray[rowctr, 9] = "0";
                    retarray[rowctr, 10] = "L";
                    retarray[rowctr, 11] = "0"; // (tot_ext_price / discount_amt).ToString();
                    retarray[rowctr, 12] = "0";
                    retarray[rowctr, 13] = "997";
                    retarray[rowctr, 14] = quote_amt.ToString();
                    rowctr++;
                }
            }



            return retarray;

        }

        private decimal tax_calculation(string icode, decimal amount, string module_basic, string comp_basis)
        {
            if (string.IsNullOrWhiteSpace(icode))
                return 0;

            var bgtax = (from bg in db.GB_001_TAX
                         where bg.tax_code == icode
                         select bg).FirstOrDefault();
            if (bgtax == null)
                return 0;

            if (bgtax.module_basis != module_basic)
                return 0;

            if (bgtax.computation_basis != comp_basis)
                return 0;

            decimal tax = 0;
            tax = amount * bgtax.tax_rate / 100;
            if (bgtax.tax_impact == "S")
                tax = 0 - tax;

            return tax;

        }

        public string[,] totalinv_calculation(string hbug,int tablecode=1)
        {
            // transaction type tax - invoice tax
            string[,] retarray = new string[30, 20];
            decimal net_amt = 0; decimal tax1 = 0; decimal tax2 = 0; int rowctr = 0;
            int key1 = Convert.ToInt16(hbug);
            string table_name = "";
            if (tablecode == 1)
                table_name = " AR_002_QUOTE ";
            else if (tablecode == 2)
                table_name = " AR_002_SODT ";


            string str1 = "select isnull(sum(quote_amount),0) dquery0 from " + table_name + " where sale_sequence_number=" + hbug;
            var str4 = db.Database.SqlQuery<querylay>(str1).FirstOrDefault();
            if (str4 != null)
                net_amt = str4.dquery0;


            var bgassign = (from bg in db.AR_001_STRAN
                            join bg1 in db.AR_002_SALES
                            on new { a1 = bg.order_code } equals new { a1 = bg1.sales_transaction_type }
                            where bg1.sale_sequence_number == key1
                            select bg).FirstOrDefault();
            if (bgassign != null)
            {
                tax1 = tax_calculation(bgassign.tax_invoice1, net_amt, "S", "T");
                tax2 = tax_calculation(bgassign.tax_invoice2, net_amt, "S", "T");

            }
            decimal tax_amt = tax1 + tax2;

            if (tax_amt != 0 )
            {
                decimal quote_amt = net_amt + tax1 + tax2;
                retarray[rowctr, 0] = "";
                retarray[rowctr, 1] = net_amt.ToString();
                retarray[rowctr, 2] = "0";
                retarray[rowctr, 3] = tax1.ToString();
                retarray[rowctr, 4] = tax2.ToString();
                retarray[rowctr, 5] = "0";
                retarray[rowctr, 6] = "0";
                retarray[rowctr, 7] = "0";
                retarray[rowctr, 8] = "0";
                retarray[rowctr, 9] = "0";
                retarray[rowctr, 10] = "I";
                retarray[rowctr, 11] = "0";
                retarray[rowctr, 12] = "0";
                retarray[rowctr, 13] = "998";
                retarray[rowctr, 14] = tax_amt.ToString();
                rowctr++;
            }
            return retarray;
        }




        public decimal [] get_taxrate(string item_code,decimal price,decimal qty)
        {
            decimal[] decarray = new decimal[20];
            var taxinc = (from bg in db.IV_001_ITEM
                          where bg.item_code == item_code
                          select bg).FirstOrDefault();

            if (taxinc.tax_inclusive == "N")
                return decarray;

            decimal tax1 = 0; decimal tax2 = 0; decimal tax3 = 0; decimal tax4 = 0; decimal tax5 = 0;

            var taxr= (from bg in db.GB_001_TAX where bg.tax_code == taxinc.tax_code1 select bg).FirstOrDefault();
            if (taxr!=null)
                tax1 = taxr.tax_impact == "A" ? taxr.tax_rate : 0 - taxr.tax_rate;

            taxr= (from bg in db.GB_001_TAX where bg.tax_code == taxinc.tax_code2 select bg).FirstOrDefault();
            if (taxr!=null)
                tax2 = taxr.tax_impact == "A" ? taxr.tax_rate : 0 - taxr.tax_rate;

            taxr= (from bg in db.GB_001_TAX where bg.tax_code == taxinc.tax_code3 select bg).FirstOrDefault();
            if (taxr!=null)
                tax3 = taxr.tax_impact == "A" ? taxr.tax_rate : 0 - taxr.tax_rate;

            taxr= (from bg in db.GB_001_TAX where bg.tax_code == taxinc.tax_code4 select bg).FirstOrDefault();
            if (taxr!=null)
                tax4 = taxr.tax_impact == "A" ? taxr.tax_rate : 0 - taxr.tax_rate;

            taxr= (from bg in db.GB_001_TAX where bg.tax_code == taxinc.tax_code5 select bg).FirstOrDefault();
            if (taxr!=null)
                tax5 = taxr.tax_impact == "A" ? taxr.tax_rate : 0 - taxr.tax_rate;


            decimal quote_amt = price*qty;
            decimal trate = 100 + tax1 + tax2 + tax3 + tax4 + tax5;
            decimal price1 = price / trate * 100;
            price1 = Math.Round(price1, 2,MidpointRounding.AwayFromZero);
            decimal ext_price2 = quote_amt / trate * 100;

            tax1 = tax_calculation(taxinc.tax_code1, ext_price2, "S", "L");
            tax2 = tax_calculation(taxinc.tax_code2, ext_price2, "S", "L");
            tax3 = tax_calculation(taxinc.tax_code3, ext_price2, "S", "L");
            tax4 = tax_calculation(taxinc.tax_code4, ext_price2, "S", "L");
            tax5 = tax_calculation(taxinc.tax_code5, ext_price2, "S", "L");
            tax1 = Math.Round(tax1, 2, MidpointRounding.AwayFromZero);
            tax2 = Math.Round(tax2, 2, MidpointRounding.AwayFromZero);
            tax3 = Math.Round(tax3, 2, MidpointRounding.AwayFromZero);
            tax4 = Math.Round(tax4, 2, MidpointRounding.AwayFromZero);
            tax5 = Math.Round(tax5, 2, MidpointRounding.AwayFromZero);
            decimal diff = quote_amt - ext_price2 - tax1 - tax2 - tax3 - tax4 - tax5;
            ext_price2 += diff;

            decarray[0] = ext_price2;
            decarray[1] = 0;
            decarray[2] = tax1;
            decarray[3] = tax2;
            decarray[4] = tax3;
            decarray[5] = tax4;
            decarray[6] = tax5;
            decarray[7] = quote_amt;
            decarray[8] = price1;

            return decarray;


        }
    }
}

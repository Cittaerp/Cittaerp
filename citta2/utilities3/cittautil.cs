using CittaErp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using anchor1.Models;
using System.Web.Mvc;
using System.IO;

namespace CittaErp.utilities
{
    public class cittautil
    {
        MainContext db = new MainContext();
        private MainContext db2 = new MainContext();
        psess psess;

        public void init_values()
        {
            pubsess pubsess = new pubsess();
            pubsess.userid = "a01";
            pubsess.processing_period = "201709";
            var bg = (from fg in db.GB_001_COY
                      join fh in db.MC_001_CUREN
                      on new { a1 = fg.field7 } equals new { a1 = fh.currency_code }
                      where fg.id_code == "COYINFO"
                      select new { d1 = fg.field7, d2 = fh.currency_description, d3 = fg.field6 }).FirstOrDefault();

                pubsess.base_currency_code = bg==null?"NA":bg.d1;
                pubsess.base_currency_description = bg == null ? "B" : bg.d2;
                pubsess.country_operation = bg == null ? "NIG" : bg.d3;

            var bgset = (from bg2 in db.GB_001_COY
                         where bg2.id_code == "COYSET"
                         select bg2).FirstOrDefault();


            pubsess.multi_currency = bgset == null ? "" : bgset.field1;
            pubsess.entry_mode = bgset == null ? "" : bgset.field8;
            pubsess.exchange_editable = bgset == null ? "" : bgset.field9;
            pubsess.exchange_rate_mode = bgset == null ? "B" : bgset.field6;

            var bgt = (from bg2 in db.GB_001_COY
                         where bg2.id_code == "COYPRD"
                         select bg2).FirstOrDefault();


            string curdate=DateTime.Now.ToString("yyyyMMdd");

            string datefrom; string dateto; string wdatefrom; string wdateto;

            if (pubsess.period_closing == "P")
            {
                datefrom = bgt.field4;
                dateto = datefrom;
                wdatefrom = datefrom;
                wdateto = dateto;
                if (bgt.field8 == "Y" && !string.IsNullOrWhiteSpace(bgt.field7))
                {
                    if (Convert.ToInt16(bgt.field7) > Convert.ToInt16(curdate))
                    {
                        if (!string.IsNullOrWhiteSpace(bgt.field5))
                            wdatefrom = bgt.field5;
                        if (!string.IsNullOrWhiteSpace(bgt.field6))
                            wdateto = bgt.field6;
                    }
                }
            }
            else
            {
                DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                datefrom = date1.ToString("yyyyMMdd");
                date1 = date1.AddMonths(1).AddDays(-1);
                dateto = date1.ToString("yyyyMMdd");
                wdatefrom = datefrom;
                wdateto = dateto;
                if (bgt.field8=="Y" && !string.IsNullOrWhiteSpace(bgt.field7))
                {
                    if (Convert.ToInt32(bgt.field7) > Convert.ToInt32(curdate))
                    {
                        if (!string.IsNullOrWhiteSpace(bgt.field5))
                            wdatefrom=bgt.field5;
                        if (!string.IsNullOrWhiteSpace(bgt.field6))
                            wdateto=bgt.field6;
                    }
                }
            }

            pubsess.curent_datefrm = datefrom;
            pubsess.curent_dateto = dateto;
            pubsess.valid_datefrm = wdatefrom;
            pubsess.valid_dateto = wdateto;
            pubsess.period_closing = bgt == null ? "C" : bgt.field1;
            pubsess.processing_period = string.IsNullOrWhiteSpace(bgt.field4) ? "201709" : bgt.field4;

          string str1 = "select field5 vwstring0, field13 vwstring1, field12 vwstring2 from GB_001_COY where id_code='COYPRICE' ";
            var bg8 = db.Database.SqlQuery<vw_genlay>(str1).FirstOrDefault();

            if (bg8 != null)
            {
                pubsess.manual_discount = bg8.vwstring0;
                pubsess.manual_others = bg8.vwstring1;
                pubsess.price_editable = bg8.vwstring2;
            }
            HttpContext.Current.Session["pubsess"] = pubsess;
        }

        public DateTime date_convert(string date1)
        {
            DateTime invaliddate = new DateTime(1000, 01, 01);
            if (date1.Length == 10)
            {
                //int dd = Convert.ToInt16(date1.Substring(0, 2));
                //int mm = Convert.ToInt16(date1.Substring(3, 2));
                //int yy = Convert.ToInt16(date1.Substring(6, 4));

                DateTime d1;
                string format = "dd/MM/yyyy";
                if (DateTime.TryParseExact(date1, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out d1))
                    return d1;
                else
                    return invaliddate;
            }
            else return invaliddate;
        }

        public string sqlquote(string sqlstr1)
        {
            if (sqlstr1 == null)
                sqlstr1 = "";

            string sqlstr2 = sqlstr1.Replace("'", "''");
            sqlstr2 = "'" + sqlstr2 + "'";
            return sqlstr2;
        }

        public querypass basecur_description(string cur_code)
        {
            querypass pass1 = new querypass();
            var curlist = (from bg in db.MC_001_CUREN
                           join bh in db.GB_001_COY
                           on new { c1 = "COYINFO" } equals new { c1 = bh.id_code }
                           where bg.currency_code == cur_code
                           select new
                           {
                               c0 = bg.currency_description,
                               c1 = bh.field7
                           }).FirstOrDefault();

            if (curlist != null)
            {
                pass1.query0 = curlist.c1;
                pass1.query1 = curlist.c0;
            }

            return pass1;

        }

        public SelectList read_ledger(string ledcode, string defvalue = "")
        {
            pubsess pubsess = (pubsess)HttpContext.Current.Session["pubsess"];
            string sqlstr = "select account_code query0, account_name query1,account_code + ' - ' + account_name as query2 from GL_001_CHART a,";
            sqlstr += " (select gl_default_id, acct_type1 acttype from GL_001_GLDS where acct_type1 <> '' union all ";
            sqlstr += " select gl_default_id, acct_type2 from GL_001_GLDS where acct_type2 <> '' union all ";
            sqlstr += " select gl_default_id, acct_type3 from GL_001_GLDS where acct_type3 <> '' union all ";
            sqlstr += " select gl_default_id, acct_type4 from GL_001_GLDS where acct_type4 <> '' union all ";
            sqlstr += " select gl_default_id, acct_type5 from GL_001_GLDS where acct_type5 <> '' ) b ";
            sqlstr += " where b.gl_default_id=" + sqlquote(ledcode) + " and account_type_code =b.acttype ";

            var bg2 = db.Database.SqlQuery<querylay>(sqlstr).ToList();
            return new SelectList(bg2.ToList(), "query0", "query2", defvalue);
        }

        public DateTime logdatetime()
        {
            return logdatetime("0", DateTime.Now);
        }

        public DateTime logdatetime(string zoneflag, DateTime utdate)
        {
            string str1;
            string timezone;

            if (zoneflag == "0")
                return DateTime.UtcNow;

            //str1 = "select field12 c0 from tab_train_default where default_code='DATES'";
            //var str11 = db.Database.SqlQuery<querylay>(str1).FirstOrDefault();

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
        public void write_plog(string table_code, string sel_str, string sel_action, string sel_seq, string puserid, string sel_pcode = "", string sel_keycode = "")
        {
            string hostname = GetWorkstation();
            string str1 = "execute write_plog @table_code=" + sqlquote(table_code) + ", @sel_str=" + sqlquote(sel_str) + ", @sel_action=" + sqlquote(sel_action) + ", @sel_seq=" + sqlquote(sel_seq);
            str1 += ",@hostname=" + sqlquote(hostname) + ",@p_userid=" + sqlquote(puserid) + ",@sel_pcode=" + sqlquote(sel_pcode) + ",@sel_keycode=" + sqlquote(sel_keycode);

            try
            {
                db.Database.ExecuteSqlCommand(str1);
            }

            catch (Exception err)
            {
                str1 = "insert into error_table(msg,progname) values (" + sqlquote(err.Message) + ",'log '+" + sqlquote(table_code) + ")";
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

        public void parameter_deleteflag(string datatype, string givenval)
        {
            string table_name = "";
            string acctname = "";
            string custcode = "";

            if (datatype == "001")
            {
                table_name = " AR_001_CUSTM ";
                acctname = " gl_account_code ";
                custcode = " customer_code ";
            }

            if (datatype == "002")
            {
                table_name = " AP_001_VENDR ";
                acctname = " gl_account_code ";
                custcode = " vendor_code ";
            }

            if (datatype == "004")
            {
                table_name = " BK_001_BANK ";
                acctname = " gl_account_code ";
                custcode = " bank_code ";
            }

            if (datatype == "005")
            {
                table_name = " FA_001_ASSET ";
                acctname = " gl_asset_acc_code,gl_accum_depn_code,gl_depn_expense_code,asset_class,gl_disposal_code ";
                custcode = " fixed_asset_code ";
            }

            if (datatype == "006")
            {
                table_name = " IV_001_ITEM ";
                acctname = " gl_cos_code,gl_income_code,gl_inv_code,gl_price_var_code,gl_stockcount_variance_code ";
                custcode = " item_code ";
            }
            if (datatype == "007")
            {
                table_name = " MC_001_CUREN ";
                acctname = " gl_acc_for_real,gl_acc_for_unreal ";
                custcode = " currency_code ";
            }
            if (datatype == "008")
            {
                table_name = " DC_001_DISC ";
                acctname = " discount_gl_acc ";
                custcode = " discount_code ";
            }
            if (datatype == "009")
            {
                table_name = " IC_001_INCOY ";
                acctname = " gl_account_code ";
                custcode = " intercoy_code ";
            }
            if (datatype == "010")
            {
                table_name = " GB_001_PCODE ";
                acctname = " gl_account_code ";
                custcode = " parameter_code ";
            }
            if (datatype == "011")
            {
                table_name = " GB_001_TAX ";
                acctname = " gl_tax_acc_code ";
                custcode = " tax_code ";
            }
            if (datatype == "012")
            {
                table_name = " GB_001_RSONC ";
                acctname = " gl_account ";
                custcode = " consignment_code ";
            }

            string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a," + table_name + " b where a.account_code in (b.";
            str += acctname + ") and b." + custcode + "=" + sqlquote(givenval);
            db.Database.ExecuteSqlCommand(str);
            if (datatype == "001" | datatype == "002" | datatype == "004" | datatype == "005" | datatype == "006")
            {
                str = "update GB_001_HANAL set delete_flag ='Y' from GB_001_HANAL a, " + table_name + " b where a.header_sequence in (analysis_code1";
                str += ",analysis_code2,analysis_code3,analysis_code4,analysis_code5,analysis_code6,analysis_code7,analysis_code8,analysis_code9,analysis_code10)";
                str += " and b." + custcode + "=" + sqlquote(givenval);
                db.Database.ExecuteSqlCommand(str);
            }
        }

        public SelectList para_selectquery(string datatype, string defvalue="", string queryt ="")
        {
            pubsess pubsess = (pubsess)HttpContext.Current.Session["pubsess"];
            if (queryt != "")
            {
                string str1 = "select bank_code query0, bank_name query1 from vw_paraquery  where bank=" + sqlquote(datatype) + " order by 2 ";
                var bg2 = db.Database.SqlQuery<querylay>(str1).ToList();
                return new SelectList(bg2.ToList(), "query0", "query1", defvalue);
            }
            else
            {
                string str1 = "select bank_code query0, query1 =case " + sqlquote(pubsess.entry_mode) + " when 'Y' then c2 else bank_name end from vw_paraquery  where bank=" + sqlquote(datatype) + " order by 2 ";
                var bg2 = db.Database.SqlQuery<querylay>(str1).ToList();
                return new SelectList(bg2.ToList(), "query0", "query1", defvalue);
            }
        }

        public Boolean delete_check(string ptype, string key1, string key2 = "", string key3 = "", string key4 = "")
        {
            if (ptype == "CHART")
            {
                string dely = (from bg in db.GL_001_CHART where bg.account_code == key1 select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "ANALY")
            {
                //chart, vendor, bank, 
                string dely = (from bg in db.GB_001_HEADER where bg.header_code == key1 && bg.delete_flag == "Y" select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "ATYPE")
            {
                string dely = (from bg in db.GL_001_ATYPE where bg.acct_type_code == key1 select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "HANAL")
            {
                string dely = (from bg in db.GB_001_HANAL where bg.header_sequence == key1 select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "CTERM")
            {
                string dely = (from bg in db.AR_001_CTERM where bg.credit_term_code == key1 select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "CUSTM")
            {
                string dely = (from bg in db.AR_001_CUSTM where bg.customer_code == key1 select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "HEAD")
            {
                string dely = (from bg in db.GB_001_HEADER where bg.header_code == key1 select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "MCAT")
            {
                string dely = (from bg in db.GL_001_CATEG where bg.acct_cat_sequence == key1 select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "VENDR")
            {
                string dely = (from bg in db.AP_001_VENDR where bg.vendor_code == key1 select bg.delete_flag).FirstOrDefault();
                string del_flag = dely == null ? "Y" : dely;
                if (dely == "Y")
                    return false;
                return true;
            }
            else if (ptype == "MAP")
            {
                string dely = (from bg in db.GB_001_MAP where bg.trans_type == key1 select bg.tran_seq).FirstOrDefault();
                //string del_flag = dely == null ? "Y" : dely;
                //if (dely == "Y")
                //    return false;
                return true;
            }
            return true;
        }

        public void write_document(string screen_code, string document_code, HttpPostedFileBase[] photo1, string[] aryopt)
        {
            pubsess comess = (pubsess)HttpContext.Current.Session["pubsess"];

            foreach (HttpPostedFileBase filep in photo1)
            {
                if (filep != null && filep.ContentLength != 0)
                {
                    byte[] uploaded = new byte[filep.InputStream.Length];
                    filep.InputStream.Read(uploaded, 0, uploaded.Length);

                    GB_001_DOC doc = new GB_001_DOC();
                    doc.screen_code = screen_code;
                    doc.document_code = document_code;
                    doc.description = "";
                    doc.document_image = uploaded;
                    doc.document_type = filep.ContentType;
                    doc.document_name = Path.GetFileName(filep.FileName);
                    
                    db.GB_001_DOC.Add(doc);

                    try
                    {
                        db.SaveChanges();
                    }

                    catch (Exception err)
                    {
                        //save_message("Error on attachment files, but document processed : " + err.InnerException.InnerException.Message);
                        //ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);
                        //err_flag = false;
                    }

                }
            }

            if (aryopt != null)
            {
                for (int pctr = 0; pctr < aryopt.Length; pctr++)
                {
                    if (!(string.IsNullOrWhiteSpace(aryopt[pctr])))
                    {
                        int seqno = Convert.ToInt16(aryopt[pctr]);
                        GB_001_DOC gdoc1 = db.GB_001_DOC.Find(seqno);

                        db.GB_001_DOC.Remove(gdoc1);
                        db.SaveChanges();
                    }
                }
            }
        }

        public string date_yyyymmdd(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";

            string date2=date1.Replace("/","");
          string date3 = date2.Substring(4, 4) + date2.Substring(2, 2) + date2.Substring(0, 2);

            return date3;

        }

        public string date_ddmmyyyyy(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return "";

            string date2 = date1.Replace("/", "");
           string date3 = date2.Substring(6, 2) + date2.Substring(4, 2) + date2.Substring(0, 4);

            return date3;

        }

        public string date_slash(string date1)
        {
            if (date1 != "")
            {
                string date2 = date_ddmmyyyyy(date1);
                date2 = date2.Substring(0, 2) + "/" + date2.Substring(2, 2) + "/" + date2.Substring(4);
                return date2;
            }
            else return "";
        }

        public Boolean date_validate(string date1)
        {
            if (string.IsNullOrWhiteSpace(date1))
                return false;

            DateTime invaliddate = new DateTime(1000, 01, 01);
            if (date1.Length == 10)
            {
                //int dd = Convert.ToInt16(date1.Substring(0, 2));
                //int mm = Convert.ToInt16(date1.Substring(3, 2));
                //int yy = Convert.ToInt16(date1.Substring(6, 4));

                DateTime d1;
                string format = "dd/MM/yyyy";
                if (DateTime.TryParseExact(date1, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out d1))
                    return true;
                else
                    return false;
            }
            else return false;
        }

        public string autogen_num(string typecode)
        {
            string gen_num ="";
            int new_num = 0;
            int readflag = 0;

            var coyinfo = (from bg in db.GB_001_COY
                           where bg.id_code == "COYAUTO"
                           select bg).FirstOrDefault();
            var coyinfo1 = (from bg in db.GB_001_COY
                           where bg.id_code == "COYAUTO1"
                           select bg).FirstOrDefault();

            int seqlen = Convert.ToInt16(coyinfo.field1);

            if (typecode == "CUS")
            {
                readflag = 1;
                 new_num = Convert.ToInt16(coyinfo.field3);
                new_num++;
                gen_num = coyinfo.field2 + zero_prefix(new_num, seqlen);
            }
           else if (typecode == "VEN")
            {
                readflag = 2;
                 new_num = Convert.ToInt16(coyinfo.field6);
                new_num++;
                gen_num = coyinfo.field5 + zero_prefix(new_num, seqlen);
            }
          else  if (typecode == "FA")
            {
                readflag = 3;
                 new_num = Convert.ToInt16(coyinfo.field9);
                new_num++;
                gen_num = coyinfo.field8 + zero_prefix(new_num, seqlen);
            }
           else if (typecode == "ITM")
            {
                readflag = 4;
                 new_num = Convert.ToInt16(coyinfo.field12);
                new_num++;
                gen_num = coyinfo.field11 + zero_prefix(new_num, seqlen);
            }
          else if (typecode == "CHA")
            {
                readflag = 5;
                 new_num = Convert.ToInt16(coyinfo.field15);
                new_num++;
                gen_num = coyinfo.field14 + zero_prefix(new_num, seqlen);
            }
            else if (typecode == "EMP")
            {
                readflag = 6;
                new_num = Convert.ToInt16(coyinfo1.field2);
                new_num++;
                gen_num = coyinfo1.field1 + zero_prefix(new_num, seqlen);
            }
            if (typecode == "PC")
            {
                readflag = 7;
                new_num = Convert.ToInt16(coyinfo1.field11);
                new_num++;
                gen_num = coyinfo1.field10 + zero_prefix(new_num, seqlen);
            }

            if (readflag == 1)
            {
                string str = " update GB_001_COY set field3 =" + new_num.ToString() + " where id_code ='COYAUTO'";
                db.Database.ExecuteSqlCommand(str);
            }
           else if (readflag == 2)
            {
                string str = " update GB_001_COY set field6 =" + new_num.ToString() + " where id_code ='COYAUTO'";
                db.Database.ExecuteSqlCommand(str);
            }
           else if (readflag == 3)
            {
                string str = " update GB_001_COY set field9 =" + new_num.ToString() + " where id_code ='COYAUTO'";
                db.Database.ExecuteSqlCommand(str);
            }
            else if (readflag == 4)
            {
                string str = " update GB_001_COY set field12 =" + new_num.ToString() + " where id_code ='COYAUTO'";
                db.Database.ExecuteSqlCommand(str);
            }
           else if (readflag == 5)
            {
                string str = " update GB_001_COY set field15 =" + new_num.ToString() + " where id_code ='COYAUTO'";
                db.Database.ExecuteSqlCommand(str);
            }
            else if (readflag == 6)
            {
                string str = " update GB_001_COY set field2 =" + new_num.ToString() + " where id_code ='COYAUTO1'";
                db.Database.ExecuteSqlCommand(str);
            }
            else if (readflag == 7)
            {
                string str = " update GB_001_COY set field11 =" + new_num.ToString() + " where id_code ='COYAUTO1'";
                db.Database.ExecuteSqlCommand(str);
            }
            return gen_num;
        }


        public string zero_prefix(int org_value, int dlength)
        {
            int pnum1;

            pnum1 = org_value == null ? 0 : org_value;
         string pnum = "0000000000" + pnum1.ToString();
            int slen = pnum.Length - dlength;
            pnum = pnum.Substring(slen, dlength);

            return pnum;

        }

        public List<SelectListItem> cal_gl(string gl_id)
        {
            List<SelectListItem> ary = new List<SelectListItem>();
            var hdet = (from bg in db.GL_001_GLDS
                        join bg1 in db.GL_001_ATYPE
                        on new { a1 = bg.acct_type1 } equals new { a1 = bg1.acct_type_code }
                        into bf1
                        from bf2 in bf1.DefaultIfEmpty()
                        join bg2 in db.GL_001_ATYPE
                        on new { a1 = bg.acct_type2 } equals new { a1 = bg2.acct_type_code }
                        into bf3
                        from bf4 in bf1.DefaultIfEmpty()
                        join bg3 in db.GL_001_ATYPE
                        on new { a1 = bg.acct_type3 } equals new { a1 = bg3.acct_type_code }
                        into bf5
                        from bf6 in bf1.DefaultIfEmpty()
                        join bg4 in db.GL_001_ATYPE
                        on new { a1 = bg.acct_type4 } equals new { a1 = bg4.acct_type_code }
                        into bf7
                        from bf8 in bf1.DefaultIfEmpty()
                        join bg5 in db.GL_001_ATYPE
                        on new { a1 = bg.acct_type5 } equals new { a1 = bg5.acct_type_code }
                        into bf9
                        from bf10 in bf1.DefaultIfEmpty()
                        where bg.gl_default_id == gl_id
                        select new { bg, bf2, bf4, bf6, bf8, bf10 }).Distinct().FirstOrDefault();
            if (hdet != null)
            {
                string pcl1 = hdet.bf2.acct_type_desc;
                string pcl2 = hdet.bf4.acct_type_desc;
                string pcl3 = hdet.bf6.acct_type_desc;
                string pcl4 = hdet.bf8.acct_type_desc;
                string pcl5 = hdet.bf10.acct_type_desc;

                ary.Add(new SelectListItem { Value = "1", Text = pcl1 });
                ary.Add(new SelectListItem { Value = "2", Text = pcl2 });
                ary.Add(new SelectListItem { Value = "3", Text = pcl3 });
                ary.Add(new SelectListItem { Value = "4", Text = pcl4 });
                ary.Add(new SelectListItem { Value = "5", Text = pcl5 });

            }
            return ary;
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
                rep_sel.selection_operator += (sel_op + sp).Substring(0, 3);
            }

            return rep_sel;
        }


        public void pub_update(string pvalue, string pname)
        {
            pubsess pblock = (pubsess)HttpContext.Current.Session["pubsess"];
            string str1 = "delete from pub_table where userid=" + sqlquote(pblock.userid) + " and name1=" + sqlquote(pname);
            db.Database.ExecuteSqlCommand(str1);
            if (pvalue != "")
            {
                str1 = "insert into pub_table(userid,name1,cvalue) values(" + sqlquote(pblock.userid) + "," + sqlquote(pname) + "," + sqlquote(pvalue) + ")";
                db.Database.ExecuteSqlCommand(str1);
            }
        }

        public int check_option()
        {
            string str1;
            int p1 = 0;
            string sp = new string(' ', 50);
            bool user_flag = true;

            //if (HttpContext.Current.Session["pubsess"] != null)
            //    user_flag = false;

            //pubsess pblock = (pubsess)HttpContext.Current.Session["pubsess"];
            //if (pblock.type2 != "S" && user_flag)
            //{
            //    string valid_option = pblock.cquery2;
            //    string vreq = HttpContext.Current.Request.Url.PathAndQuery + sp;
            //    int ctr = vreq.IndexOf("/", 1);
            //    string rep8 = vreq.Substring(1, ctr - 1);

            //    str1 = " select count(0) t1 from tab_array fd, (";
            //    str1 += " select report_code c1, 'ITEM' c4, report_name8 c8 from tab_soft where para_code in ('ITEM','SELFT','SELFTQ') union all ";
            //    str1 += " select calc_code , a.para_code, calc_code from tab_calc a, tab_soft b where b.para_code='UPER' and b.report_code=a.para_code union all ";
            //    str1 += " select trans_type , 'TYPE', trans_type from tab_type a, tab_array b where a.internal_use <> 'Y' and a.trans_type = b.array_code and b.para_code in ('SELE')  ";
            //    str1 += ") xd  where fd.para_code='21' and fd.array_code=" + pads(pblock.ugroup) + " and fd.operand= xd.c1 and fd.source1=xd.c4 and xd.c8=" + pads(rep8);
            //    var str21 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            //    if (((valid_option == "" || valid_option == "V") && str21.t1 == 0) || (valid_option == "I" && str21.t1 > 0))
            //        //                if (str21 == null)
            //        p1 = 1;

            //}

            ////return p1;
            return 0;

        }

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

        public string zeroprefix(string value1, int lenoutput)
        {
            value1 = value1 == null ? "" : value1;
            string newv = "0000000000" + value1;
            string outv = newv.Substring(newv.Length - lenoutput);
            return outv;
        }

        public List<SelectListItem> year_list()
        {
            //cl_period pyclass = default_period();
            string str1 = "select field9 c1, field10 c2  from GB_001_COY where id_code='COYPRD'";
            var str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            List<SelectListItem> pyears = new List<SelectListItem>();
            SelectListItem pyvar;
            int start_year = Convert.ToInt16(str2.c1);
            int stop_year = Convert.ToInt16(str2.c2);

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

        public SelectList querylist(string qcode, string acode = "", int ps = 0)
        {
            string str1="select '' c1, '' c2";
            //pubsess pblock = (pubsess)HttpContext.Current.Session["pubsess"];
            //if (ps == 1)
            //{
            //    str1 = "select c1 = report_code, c2=report_name1 from tab_soft where para_code=" + pads(qcode);
            //}
            //else if (ps == 2)
            //{
            //    str1 = "select c1 = pcode, c2=pcode + ' : ' + pname from vw_user_parameter where hcode=" + pads(qcode);
            //    if (qcode == "STAFF")
            //        str1 = "select staff_number c1, c2=staff_number + ' : ' + dbo.capname(surname,first_name,other_name) from tab_staff where close_code='0' ";
            //    else if (qcode == "ALLOW")
            //        str1 = "select c1 = allow_code, c2=allow_code + ' : ' + name1 from tab_allow where para_code='02' and allowance_code='A' ";
            //    else if (qcode == "DEDUCT")
            //        str1 = "select c1 = allow_code, c2=allow_code + ' : ' + name1 from tab_allow where para_code='02' and allowance_code='D' ";
            //    else if (qcode == "DALLOW")
            //        str1 = "select c1 = daily_code, c2=daily_code + ' : ' + name1 from tab_daily where para_code='03' and code_ind='A' ";
            //    else if (qcode == "DDEDUCT")
            //        str1 = "select c1 = daily_code, c2=daily_code + ' : ' + name1 from tab_daily where para_code='03' and code_ind='D' ";
            //}
            //else
            //{
            //    str1 = "select c1 = pcode, c2=case '" + pblock.cquery + "' when 'Y' then pcode + ' : ' + pname else pname end from vw_user_parameter where hcode=" + pads(qcode);
            //    if (qcode == "STAFF")
            //        str1 = "select staff_number c1, c2=case '" + pblock.cquery + "' when 'Y' then staff_number + ' : ' + dbo.capname(surname,first_name,other_name) else dbo.capname(surname,first_name,other_name) end from tab_staff where close_code='0' ";
            //    else if (qcode == "ALLOW")
            //        str1 = "select c1 = allow_code, c2=case '" + pblock.cquery + "' when 'Y' then allow_code + ' : ' + name1 else name1 end from tab_allow where para_code='02' and allowance_code='A' ";
            //    else if (qcode == "DEDUCT")
            //        str1 = "select c1 = allow_code, c2=case '" + pblock.cquery + "' when 'Y' then allow_code + ' : ' + name1 else name1 end from tab_allow where para_code='02' and allowance_code='D' ";
            //}

            //str1 += " order by 2 ";

            var bglist1 = db.Database.SqlQuery<vw_query>(str1);
            return new SelectList(bglist1.ToList(), "c1", "c2", acode);

        }


        public void update_entry(string pvalue, string pname, string puser)
        {
            pubsess pblock = (pubsess)HttpContext.Current.Session["pubsess"];
            string str1 = "execute update_entry @posting_type=" + sqlquote(pvalue) + ", @reference=" + sqlquote(pname)+", @puserid="+sqlquote(puser);
            db.Database.ExecuteSqlCommand(str1);
            // mail and any others
        }

        public IEnumerable<vw_picture> read_attachmentp(string scrncode, string docode)
        {
            //string docode = bg2list.bh.document_number.ToString();
            //string scrncode = psess.temp0.ToString();
            //var bglist = from bg in db.GB_001_DOC
            //             where bg.screen_code == scrncode && bg.document_code == docode
            //             orderby bg.document_sequence
            //             select bg;

            //ViewBag.anapict = bglist.ToList();


            string str1 = "select * from GB_001_DOC a left outer join tab_soft b on ( b.para_code = 'IMGDEF' and document_type like '%' + report_code) ";
            str1 += " where a.screen_code=" + pads(scrncode) + " and a.document_code=" + pads(docode);
            str1 += " order by document_sequence ";
            var bg2 = db.Database.SqlQuery<vw_picture>(str1);

            return bg2.ToList();

        }



    }
}

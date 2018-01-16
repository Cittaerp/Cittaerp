using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using anchor1.Models;
using anchor1.utilities;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net.Mime;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace anchor1.utilities
{
    public class Cmail
    {

        ///      static DataClasses1DataContext context = new DataClasses1DataContext();
        private anchor1Context db = new anchor1Context();

        //        Ccompany  constr = new Ccompany();
        Cutil utils = new Cutil();
        string[] toadd = new string[5];
        string[] copyadd = new string[5];
        string[] bcopyadd = new string[5];
        string subj; string mail_msg; string fromadd;
        string[] attdoc = new string[5];
        string type_code; string snumber; string strans_type;
        string sdate; string sindicator; string sgroup;
        string rmess1; string rmess2; string rmess3; string rmess4; string rmess5;
        string usernamel = "";
        string first_flag = "";
        string categ; string tran_name;
        string curapp; string nextapp; string next2app;
        string err_msg;
        string doc_message = "";

        private Boolean send_mailp(string fromadd, string[] toadd, string[] tocc, string[] tobcc, string msubj, string messg, string[] attachdoc)
        {
            var smail = (from sm in db.tab_default
                         where sm.default_code == "00000"
                         select sm).FirstOrDefault();

            bool err_flag = true;

            try
            {
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();

                for (int ctr1 = 0; ctr1 < toadd.Length; ctr1++)
                {
                    if (!string.IsNullOrWhiteSpace(toadd[ctr1]))
                        m.To.Add(toadd[ctr1]);
                }

                for (int ctr1 = 0; ctr1 < tocc.Length; ctr1++)
                {
                    if (!string.IsNullOrWhiteSpace(tocc[ctr1]))
                        m.CC.Add(tocc[ctr1]);
                }

                for (int ctr1 = 0; ctr1 < tobcc.Length; ctr1++)
                {
                    if (!string.IsNullOrWhiteSpace(tobcc[ctr1]))
                        m.Bcc.Add(tobcc[ctr1]);
                }

                for (int ctr1 = 0; ctr1 < attachdoc.Length; ctr1++)
                {
                    if (!string.IsNullOrWhiteSpace(attachdoc[ctr1]))
                        m.Attachments.Add(new System.Net.Mail.Attachment(attachdoc[ctr1]));
                }

                m.From = new MailAddress(fromadd);

                m.Subject = msubj;
                m.Body = messg;

                sc.Host = smail.smtp_host;
                sc.Port = smail.smtp_port;

                sc.Credentials = new System.Net.NetworkCredential(smail.mail_user, smail.mail_password);
                //                if (smail.field5 == "Y")
                //                    sc.EnableSsl = true;

                sc.Send(m);
            }
            catch (Exception err)
            {
                err_flag = false;
                err_msg = "Error on Mail delivery. Message To " + toadd[0] + " \n Message subject " + msubj + " \n------ error msg " + err.Message;
                utils.save_message(err_msg);
                string str1 = "insert into error_table(msg,progname) values(" + utils.pads(err_msg) + ",'mail error')";
                db.Database.ExecuteSqlCommand(str1);

            }

            return err_flag;

        }

        public Boolean send_mail(string det_str, string userid)
        {
            breakup_det(det_str);
            get_message_template(strans_type);

            usernamel = (from bh in db.vw_user where bh.user_code == userid select bh.name1).FirstOrDefault();

            // col1=staff number of transaction
            // col2=staff name
            // col3=mail address
            // col4=processed
            // col5=sourceid
            // col6=transaction name
            // col7=
            // col10=current approval name
            // col11=current approval email
            // col12=next approval name
            // col13=next approval email
            // col14=alternate approval name
            // col15=alternate approval email
            // col16=info approval name
            // col17=info approval email
            // col18=final approval 1 mail
            // col19=transaction type
            // col20=final approval 2 mail
            // col21=next approval number
            // col22=alternate approval number
            // col23=info approval number

            string str1 = "select a.snumber column1, b.staff_name column2, isnull(b.email_address,'') column3, a.column20 column4,a.column26 column5, a.column12 column6, ";
            str1 += " a.column9 column10, isnull(c.email_address,'') column11, a.column33 column12, isnull(d.email_address,'') column13, ";
            str1 += " a.column34 column14, isnull(e.email_address,'') column15, f.staff_name column16, isnull(f.email_address,'') column17, ";
            str1 += " isnull(h.email_address,'') column18, column5 column19, isnull(k.email_address,'') column20,  ";
            str1 += " isnull(m.mail_notification,'N') column21, isnull(n.mail_notification,'N') column22, isnull(p.mail_notification,'N') column23 from [" + userid + "tapp] a ";
            str1 += " left outer join tab_staff b on (b.staff_number=a.snumber) ";
            str1 += " left outer join tab_staff c on (c.staff_number=a.column8) ";
            str1 += " left outer join tab_staff d on (d.staff_number=a.column31) ";
            str1 += " left outer join tab_staff e on (e.staff_number=a.column32) ";
            str1 += " left outer join tab_staff f on (f.staff_number=a.column29) ";
            str1 += " left outer join tab_staff h on (h.staff_number=a.column37) ";
            str1 += " left outer join tab_staff k on (k.staff_number=a.column38) ";
            str1 += " left outer join tab_staff_person m on (m.staff_number=a.column31) ";
            str1 += " left outer join tab_staff_person n on (n.staff_number=a.column32) ";
            str1 += " left outer join tab_staff_person p on (p.staff_number=a.column29) ";
            str1 += " where a.snumber=" + utils.pads(snumber) + " and column1=" + utils.pads(strans_type) + " and column2=" + utils.pads(sdate);
            str1 += " and column3=" + utils.pads(sindicator) + " and column4=" + utils.pads(sgroup);
            var query1 = db.Database.SqlQuery<rep_column>(str1).FirstOrDefault();

            categ = query1.column19;
            curapp = query1.column10;
            nextapp = query1.column12;
            next2app = query1.column14;
            tran_name = query1.column6;
            message_converter(userid);

            string reqmsg = rmess1;
            string statmsg = rmess2;
            string apprmsg = rmess3;
            string infomsg = rmess4;
            string rejmsg=rmess5;

            if (query1.column4 != "99")
            {
                fromadd = query1.column11;
                toadd[0] = query1.column13;
                toadd[1] = query1.column15;
                copyadd[0] = query1.column17;

                if (query1.column21 == "Y") toadd[0] = "";
                if (query1.column22 == "Y") toadd[1] = "";
                if (query1.column23 == "Y") copyadd[0] = "";

                subj = "Request: Approval on " + query1.column6 + " for " + query1.column2;
                mail_msg = reqmsg;
                if (!(toadd[0] == "" && toadd[1] == "" && copyadd[0]==""))
                    send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

                subj = "Request status on " + query1.column6;
                mail_msg = statmsg;
                fromadd = query1.column11;
                toadd[0] = query1.column3;
                toadd[1] = "";
                copyadd[0] = "";
                if (toadd[0] != "" && query1.column5 != "P")
                    send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

            }
            else
            {
                subj = "Approved: Request on " + query1.column6 + " for " + query1.column2;
                mail_msg = apprmsg;
                fromadd = query1.column11;
                toadd[0] = query1.column3;
                if (toadd[0] != "")
                    send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

                subj = "Approved: Information on " + query1.column6 + " for " + query1.column2;
                mail_msg = infomsg;
                fromadd = query1.column11;
                toadd[0] = query1.column18;
                toadd[1] = query1.column20;
                if (!(toadd[0] == "" && toadd[1] == ""))
                    send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

            }

            if (first_flag == "1")
            {
                string doublename = query1.column12;
                if (!string.IsNullOrWhiteSpace(query1.column14))
                    doublename += ", " + query1.column14;
                string bmsg = "Your request awaits approval from " + doublename;
                bmsg += ", <br> " + doc_message;
                save_message(bmsg);


            }


            return true;
        }


        public Boolean reject_mail(string det_str, string userid, string comment1)
        {

            breakup_det(det_str);
            get_message_template(strans_type);
            message_converter(userid);

            string rejmsg = rmess5;


            string str1 = "select b.email_address column24, c.email_address column25, * from [" + userid + "tapp] a ";
            str1+=" left outer join tab_staff b on (b.staff_number=a.snumber) ";
            str1+=" left outer join tab_staff c on (c.staff_number=a.column8) ";
            str1+=" where a.snumber=" + utils.pads(snumber) + " and column1=" + utils.pads(strans_type);
            str1 += " and column2=" + utils.pads(sdate) + " and column3=" + utils.pads(sindicator) + " and column4=" + utils.pads(sgroup);
            var q1 = db.Database.SqlQuery<rep_column>(str1).FirstOrDefault();

            subj = "Attn: Request reject on " + q1.column12;
            mail_msg = rejmsg;
            fromadd = q1.column25;
            toadd[0] = q1.column24;
            copyadd[0] = "";
            bcopyadd[0] = "";
            if (toadd[0] != "")
                send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

            return true;

        }

        public Boolean reject_batch_mail(string table_close, string userid)
        {

            string str1 = "select b.email_address column24, c.email_address column25, a.* from [" + userid + "tapp] a ";
            str1 += " left outer join tab_staff b on (b.staff_number=a.snumber) ";
            str1 += " left outer join tab_staff c on (c.staff_number=a.column8) ";
            str1 += " inner join [" + table_close + "] d on (d.column5 <> 'P' )";
            str1 += " where a.snumber=d.snumber and a.column1=d.column1 and a.column2=d.column2 and a.column3=d.column3 and a.column4=d.column4 ";
            var q1 = db.Database.SqlQuery<rep_column>(str1);

            foreach (var item in q1.ToList())
            {
                snumber = item.snumber;
                strans_type = item.column1;
                sdate = item.column2;
                sindicator = item.column3;
                sgroup = item.column4;
                get_message_template(strans_type);
                message_converter(userid);

                string rejmsg = rmess5;

                subj = "Attn: Request reject on " + item.column12;
                mail_msg = rejmsg;
                fromadd = item.column25;
                toadd[0] = item.column24;
                copyadd[0] = "";
                bcopyadd[0] = "";
                if (toadd[0] != "")
                    send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);
            }

            return true;

        }

        private void breakup_det(string det_str)
        {
            int ws_count = det_str.IndexOf(":");
            type_code = det_str.Substring(0, ws_count);
            int ws_count1 = det_str.IndexOf(":", ws_count + 1);
            snumber = det_str.Substring(ws_count + 1, ws_count1 - ws_count - 1);
            ws_count = det_str.IndexOf(":", ws_count1 + 1);
            strans_type = det_str.Substring(ws_count1 + 1, ws_count - ws_count1 - 1);
            ws_count1 = det_str.IndexOf(":", ws_count + 1);
            sdate = det_str.Substring(ws_count + 1, ws_count1 - ws_count - 1);
            ws_count = det_str.IndexOf(":", ws_count1 + 1);
            sindicator = det_str.Substring(ws_count1 + 1, ws_count - ws_count1 - 1);
            ws_count1 = det_str.IndexOf(":", ws_count + 1);
            sgroup = det_str.Substring(ws_count + 1);

        }

        public Boolean appraisal_mail(string det_str, string userid, decimal app_type)
        {

            //string[] appmail = new string[10];

            breakup_det(det_str);

            tab_bank q1 = new tab_bank();
            if (app_type > (decimal)0.9)
            {
                q1 = (from b1 in db.tab_bank
                      where b1.para_code == "ML" && b1.bank_code == "XZAPPRA"
                      select b1).FirstOrDefault();
            }
            else
            {
                q1 = (from b1 in db.tab_bank
                      where b1.para_code == "ML" && b1.bank_code == "XZECOPY"
                      select b1).FirstOrDefault();
            }

            if (q1 == null)
            {
                q1 = (from b1 in db.tab_bank
                      where b1.para_code == "ML" && b1.bank_code == "DEFXX"
                      select b1).FirstOrDefault();
            }

            if (q1 == null)
                return false;

            usernamel = (from bh in db.vw_user where bh.user_code == userid select bh.name1).FirstOrDefault();
            rmess1 = q1.address3;
            rmess2 = q1.email;
            rmess3 = q1.sort_code;

            string str1 = "select a.snumber column1, b.staff_name column2, isnull(b.email_address,'') column3, a.column20 column4,a.column26 column5, a.column12 column6, ";
            str1 += " a.column9 column10, isnull(c.email_address,'') column11, a.column33 column12, isnull(d.email_address,'') column13, ";
            str1 += " a.column34 column14, isnull(e.email_address,'') column15, f.staff_name column16, isnull(f.email_address,'') column17, ";
            str1 += " isnull(h.email_address,'') column18, column5 column19, isnull(k.email_address,'') column20 from [" + userid + "tapp] a ";
            str1 += " left outer join tab_staff b on (b.staff_number=a.snumber) ";
            str1 += " left outer join tab_staff c on (c.staff_number=a.column8) ";
            str1 += " left outer join tab_staff d on (d.staff_number=a.column31) ";
            str1 += " left outer join tab_staff e on (e.staff_number=a.column32) ";
            str1 += " left outer join tab_staff f on (f.staff_number=a.column29) ";
            str1 += " left outer join tab_staff h on (h.staff_number=a.column37) ";
            str1 += " left outer join tab_staff k on (k.staff_number=a.column38) ";
            str1 += " where a.snumber=" + utils.pads(snumber) + " and column2=" + utils.pads(strans_type) ;
            var query1 = db.Database.SqlQuery<rep_column>(str1).FirstOrDefault();

            categ = query1.column19;
            curapp = query1.column10;
            nextapp = query1.column12;
            next2app = query1.column14;
            tran_name = query1.column6;
            message_converter( userid);

            string reqmsg = rmess1;
            string statmsg = rmess2;
            string apprmsg = rmess3;

            if ((app_type > (decimal)0.9) || (app_type == (decimal)0.0))
            {
                if (app_type.ToString() != "99")
                {
                    fromadd = query1.column11;
                    toadd[0] = query1.column13;
                    toadd[1] = query1.column15;
                    copyadd[0] = query1.column17;

                    subj = "Request: Approval on " + query1.column6 + " for " + query1.column2;
                    mail_msg = reqmsg;
                    if (!(toadd[0] == "" && toadd[1] == ""))
                        send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

                    subj = "Request status on " + query1.column6;
                    mail_msg = statmsg;
                    fromadd = query1.column11;
                    toadd[0] = query1.column3;
                    toadd[1] = "";
                    copyadd[0] = "";
                    if (toadd[0] != "" && query1.column5 != "E")
                        send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

                }
                else
                {
                    subj = "Approved: Request on " + query1.column6 + " for " + query1.column2;
                    mail_msg = apprmsg;
                    fromadd = query1.column11;
                    toadd[0] = query1.column3;
                    if (toadd[0] != "")
                        send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

                    subj = "Approved: Information on " + query1.column6 + " for " + query1.column2;
                    mail_msg = rmess4;
                    fromadd = query1.column11;
                    toadd[0] = query1.column18;
                    toadd[1] = query1.column20;
                    if (!(toadd[0] == "" && toadd[1] == ""))
                        send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

                }
            }
            else
            {
                //reqmsg - 0.2 submit for appraisee comment
                //statmsg - 0.4 submit for manager
                //aprmsg - -1 return toadd appraisee

                fromadd = query1.column11;
                toadd[0] = query1.column13;
                toadd[1] = query1.column15;
                copyadd[0] = query1.column17;

                if (app_type.ToString() == "0.2")
                {
                    subj = "Request: Your Comment on Performance ";
                    mail_msg = reqmsg;
                    if (!(toadd[0] == ""))
                        send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);
                }

                if (app_type.ToString() == "0.4")
                {
                    subj = "Request: Approval on Comment (Performance)";
                    mail_msg = statmsg;
                    if (toadd[0] != "")
                        send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

                }

                if (app_type.ToString() == "-1")
                {
                    subj = "Request: Return to Appraisee " + query1.column3;
                    mail_msg = apprmsg;
                    fromadd = userid;
                    toadd[0] = query1.column3;
                    if (toadd[0] != "")
                        send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);
                }
            }

            return true;
        }

        public Boolean init_userp(string user_code)
        {
            db = new anchor1Context();
            string str2; string password_msg;
            string passq; string str1;
            Boolean err_flag = true;
            string def_user = "";

            mysessvals pblock = (mysessvals)HttpContext.Current.Session["mysessvals"];
            if (pblock == null)
            {
                str1 = "select sender_mail c0 from tab_default ";
                var str12 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                def_user = str12.c0;
            }
            else
            {
                str1 = "select email_address c0 from tab_staff where staff_number=" + utils.pads(pblock.userid);
                var str12 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                if (str12 == null)
                {
                    str1 = "select sender_mail c0 from tab_default ";
                    var str13 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                    def_user = str13.c0;
                }
                else
                    def_user = str12.c0;
            }

            var plist = (from bg in db.vw_user
                         where bg.user_code == user_code
                         select bg).FirstOrDefault();

            if (plist == null)
            {
                err_msg = "Staff Number not valid";
                return false;
            }

            string password_type = (from bh in db.tab_train_default where bh.default_code == "PASS" select bh.field4).FirstOrDefault();

            if (plist.type1 == "staff")
            {
                var slist = (from bg in db.tab_staff
                             where bg.staff_number == user_code & bg.close_code == "0"
                             select bg).FirstOrDefault();

                if (password_type == "SP")
                {
                    str2 = "";
                    password_msg = " Blank";
                }
                else if (password_type == "DB")
                {
                    str2 = slist.date_of_birth;
                    password_msg = " Date of Birth (YYYYMMDD)";
                }
                else if (password_type == "DE")
                {
                    str2 = slist.date_of_employment;
                    password_msg = " Date of Employment (YYYYMMDD)";
                }
                else
                {
                    Random rand02 = new Random();
                    int rand01 = rand02.Next();
                    str2 = Convert.ToString(rand01);
                    if (str2.Length > 10)
                        str2 = str2.Substring(0, 10);

                    password_msg = str2;
                }

                passq = GetMD5Hash(str2 + user_code);

                str1 = "update tab_staff set password6=password5,password5=password4,password4=password3,change_flag='Y',";
                str1 += " password3=password2,password2=password1,password1=password_user,count_user=0,password_user=" + utils.pads(passq) + " where staff_number=" + utils.pads(user_code);
                db.Database.ExecuteSqlCommand(str1);

                //	' send mail to user involved
                string initname; string inituser = "";
                if (pblock == null)
                {
                    initname = " the password Forgotten option ";
                    inituser = def_user;
                }
                else
                {
                    initname = pblock.pname;
                    inituser = pblock.userid;
                }

                str1 = " Your password has been initialised by " + initname + " on " + DateTime.Now + "\r\n Your new password is " + password_msg + " \r\n  You can logon using your new details at ";
                if (slist.email_address != "")
                {
                    toadd[0] = slist.email_address;
                    err_flag = send_mailp(def_user, toadd, copyadd, bcopyadd, "Initialisation of Password", str1, attdoc);
                    if (err_flag)
                        utils.save_message("The New Password has been mailed to the Staff.");
                }
                else
                {
                    string myemail = (from bk in db.tab_staff where bk.staff_number == inituser select bk.email_address).FirstOrDefault();
                    if (myemail != null)
                    {
                        toadd[0] = myemail;
                        err_flag = send_mailp(def_user, toadd, copyadd, bcopyadd, "Initialisation of Password", str1, attdoc);
                        if (err_flag)
                            utils.save_message("The New Password has been mailed to you");
                    }
                    else
                        utils.save_message("Your New Password is " + password_msg);
                }
            }
            else
                if (plist.type1 == "local")
                {
                    if (password_type == "SP")
                    {
                        str2 = "";
                        password_msg = " Blank";
                    }
                    else
                    {
                        Random rand02 = new Random();
                        int rand01 = rand02.Next();
                        str2 = Convert.ToString(rand01);
                        if (str2.Length > 10)
                            str2 = str2.Substring(0, 10);

                        password_msg = str2;
                    }
                    //	' check if staff has enterprise or not
                    passq = GetMD5Hash(str2 + user_code);
                    str1 = "update tab_local_user set password6=password5,password5=password4,password4=password3,change_flag='Y',";
                    str1 += " password3=password2,password2=password1,password1=password_user,count_user=0,password_user=" + utils.pads(passq) + " where user_code=" + utils.pads(user_code);
                    db.Database.ExecuteSqlCommand(str1);

                utils.save_message("Your New Password is " + password_msg);
                }

            return true;

        }

        public Boolean mail_level0(string ttype, string userid, string det_str)
        {
            breakup_det(det_str);
            string table_name = userid + "tapp";
            if (type_code == "V")
                doc_message = "Document number is " + sdate;
            else if (type_code == "F")
                doc_message = "Ticket number is " + sdate;

            string mail0_rec = snumber + strans_type + sdate + sindicator + sgroup;
            utils.pub_update(mail0_rec, "mailrec0");

            string str1 = " exec get_sapproval_transaction @table_name=" + utils.pads(table_name) + ", @qnumber=" + utils.pads(snumber);
            str1 += ", @link_user=" + utils.pads(userid) + ",@qtype="+utils.pads(strans_type)+" ,@request_flag=1 ";
            db.Database.ExecuteSqlCommand(str1);

            str1 = "select count(0) t1 from [" + table_name + "]";
            var str_ctr = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str_ctr.t1 > 0)
            {
                first_flag = "1";
                return send_mail(det_str, userid);
            }
            else
            {
                err_msg = "Transaction submitted but no approver(s), Pls check with your administrator";
                utils.save_message(err_msg);
                return false;
            }

        }

        public void appraisal_messg(string[] keyinit, int pt, string userid)
        {
            int keyctr = keyinit.Length;
            string keyinit_str;
            string snumber; string scode; string str1;
            string enumber;
            int ws_count;

            var q1 = (from b1 in db.tab_bank
                      where b1.bank_code == "ML" && b1.bank_code == "XZAPMAIL"
                      select b1).FirstOrDefault();
            if (q1 != null)
            {
                usernamel = (from bh in db.vw_user where bh.user_code == userid select bh.name1).FirstOrDefault();
                string bmess1 = q1.address3;
                string bmess2 = q1.email;
                string bmess3 = q1.sort_code;
                categ = "A";

                for (int ctr = 0; ctr < keyctr; ctr++)
                {
                    keyinit_str = keyinit[ctr];
                    ws_count = keyinit_str.IndexOf(":");
                    snumber = keyinit_str.Substring(0, ws_count);
                    scode = keyinit_str.Substring(ws_count + 1);
                    str1 = "select appraiser1 c1,appraiser2 c2,appraiser3 c3,appraiser4 c4,appraiser5 c5,appraiser6 c6,processed n1 from tab_appraisal ";
                    str1 += " where processed <> 99 and staff_number=" + utils.pads(snumber) + " and code_definition=" + utils.pads(scode);
                    var str12 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

                    string det_str = "A:" + snumber + ":" + scode + ":::";
                    rmess1 = bmess1;
                    rmess2 = bmess2;
                    rmess3 = bmess3;
                    message_converter( userid);
                    string reqmsg = rmess1;
                    string statmsg = rmess2;
                    string apprmsg = rmess3;

                    if (pt == 1)
                    {
                        if (str12.n1 == 0 || str12.n1 == (decimal)0.4)
                            enumber = str12.c1;
                        else if (str12.n1 == 1)
                            enumber = str12.c2;
                        else if (str12.n1 == 2)
                            enumber = str12.c3;
                        else if (str12.n1 == 3)
                            enumber = str12.c4;
                        else if (str12.n1 == 4)
                            enumber = str12.c5;
                        else if (str12.n1 == 5)
                            enumber = str12.c6;
                        else
                            enumber = snumber;

                        fromadd = userid;
                        toadd[0] = get_mailAddress(enumber);
                        toadd[1] = "";
                        copyadd[0] = "";

                        subj = "Re: Status on Performance for " + get_mailAddress(snumber, 1);
                        mail_msg = reqmsg;
                        if (!(toadd[0] == "" && toadd[1] == ""))
                            send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);
                    }
                    else
                    {
                        str1 = " update tab_appraisal set processed= case processed when 0.4 then 0.2 when 0.2 then 0 else processed - 1 end from tab_appraisal ";
                        str1 += " where staff_number=" + utils.pads(snumber) + " and code_definition=" + utils.pads(scode);
                        db.Database.ExecuteSqlCommand(str1);

                        fromadd = userid;
                        toadd[0] = get_mailAddress(snumber);
                        toadd[1] = "";
                        copyadd[0] = "";

                        subj = "Return Performance for " + get_mailAddress(snumber, 1);
                        mail_msg = statmsg;
                        if (!(toadd[0] == "" && toadd[1] == ""))
                            send_mailp(fromadd, toadd, copyadd, bcopyadd, subj, mail_msg, attdoc);

                    }
                }
            }
        }

        private string get_mailAddress(string snumber, int type = 0)
        {
            string str1 = "select email_address c1, staff_name c2 from tab_staff where staff_number=" + utils.pads(snumber);
            var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str11 == null)
                return "";
            else if (type == 0)
                return str11.c1;
            else
                return str11.c2;
        }

        public void single_mail(string toadd1, string tsubj, string tmessg, string tfile)
        {

            string str1 = "select sender_mail c0 from tab_default ";
            var str12 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str12 != null)
            {
                toadd[0] = toadd1;
                attdoc[0] = tfile;
                send_mailp(str12.c0, toadd, copyadd, bcopyadd, tsubj, tmessg, attdoc);
            }

        }

        private String GetMD5Hash(String TextToHash)
        {
            //Check wether data was passed
            if ((TextToHash == null) || (TextToHash.Length == 0))
            {
                return String.Empty;
            }

            //Calculate MD5 hash. This requires that the string is splitted into a byte[].
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(TextToHash);
            byte[] result = md5.ComputeHash(textToHash);

            //Convert result back to string.
            string sresult = System.BitConverter.ToString(result);
            sresult = sresult.Replace("-", "");
            return sresult;
        }

        private void message_converter(string userid)
        {
            string tablename = userid + "msg";
            string str1 = "execute table_text_create @tablename=" + utils.pads(tablename);
            db.Database.ExecuteSqlCommand(str1);
            str1 = "insert into [" + tablename + "](column1,column2,column3,column4,column5) values(" + utils.pads(rmess1) + "," + utils.pads(rmess2) + "," + utils.pads(rmess3) + "," + utils.pads(rmess4) + "," + utils.pads(rmess5) + ")";
            db.Database.ExecuteSqlCommand(str1);
            str1 = "execute message_converter @lnumber=" + utils.pads(snumber) + ", @lttype=" + utils.pads(strans_type) + ", @ldate=" + utils.pads(sdate);
            str1 += ", @lgroup=" + utils.pads(sgroup) + ", @lind=" + utils.pads(sindicator) + ", @lcateg=" + utils.pads(categ) + ", @p_userid=" + utils.pads(userid);
            db.Database.ExecuteSqlCommand(str1);

            str1 = "select column1 c0, column2 c1,column3 c2,column4 c3,column5 c4 from [" + tablename + "]";
            var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            rmess1 = str11.c0;
            rmess2 = str11.c1;
            rmess3 = str11.c2;
            rmess4 = str11.c3;
            rmess5 = str11.c4;

        }

        public void sender_mail(string fromadd, string[] toadd1, string[] ccadd, string tsubj, string tmessg, string tfile1, string tfile2)
        {
            attdoc[0] = tfile1;
            attdoc[1] = tfile1;

            send_mailp(fromadd, toadd1, ccadd, bcopyadd, tsubj, tmessg, attdoc);

        }

        public void document_mail(string puserid)
        {
            string str1 = "select * from [" + puserid + "st01doc] where column1 <> ''";
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

                    // Create attachment
                    ContentType contentType = new ContentType();
                    contentType.MediaType = MediaTypeNames.Application.Pdf;
                    contentType.Name = item.column3 + "(" + item.snumber + ")";
                    Attachment attach1 = new Attachment(new MemoryStream(memdoc), contentType);

                    var smail = (from sm in db.tab_default
                                 where sm.default_code == "00000"
                                 select sm).FirstOrDefault();

                    try
                    {
                        MailMessage m = new MailMessage();
                        SmtpClient sc = new SmtpClient();
                        m.To.Add(item.column1);
                        m.Attachments.Add(attach1);

                        m.From = new MailAddress(smail.sender_mail);

                        m.Subject = item.column3;
                        m.Body = item.column4;

                        sc.Host = smail.smtp_host;
                        sc.Port = smail.smtp_port;

                        sc.Credentials = new System.Net.NetworkCredential(smail.mail_user, smail.mail_password);

                        sc.Send(m);
                    }
                    catch (Exception err)
                    {
                        string err_msg = "Error on Mail delivery. Message To " + item.column1 + " \n Message subject " + item.column3 + " \n------ error msg " + err.Message;
                        utils.save_message(err_msg);
                        str1 = "insert into error_table(msg,progname) values(" + utils.pads(err_msg) + ",'mail error')";
                        db.Database.ExecuteSqlCommand(str1);

                    }

                }
            }
        }


        private void get_message_template(string strans_type1)
        {

            var q1 = (from b1 in db.tab_bank
                      where b1.para_code == "ML" && b1.bank_code == strans_type1
                      select b1).FirstOrDefault();

            if (first_flag == "1")
            {
                q1 = (from b1 in db.tab_bank
                      where b1.para_code == "ML" && b1.bank_code == "DEFZF"
                      select b1).FirstOrDefault();

            }

            if (q1 == null)
            {
                q1 = (from b1 in db.tab_bank
                      where b1.para_code == "ML" && b1.bank_code == "DEFXX"
                      select b1).FirstOrDefault();
            }

            if (q1 == null)
            {
                err_msg = "No mail Template";
                utils.save_message(err_msg);
                return;
            }

            rmess1 = q1.address3;       //request mail
            rmess2 = q1.email;          // status mail
            rmess3 = q1.sort_code;      // approval mail
            rmess4 = q1.address2;       // information mail
            rmess5 = q1.address1;       // reject mail

        }

        
        private void save_message(string errmsg)
        {
            worksess worksess = (worksess)HttpContext.Current.Session["worksess"];
            worksess.bye_mess = errmsg;
            HttpContext.Current.Session["worksess"] = worksess;
        }

    }

}



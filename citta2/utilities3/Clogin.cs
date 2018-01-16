using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using anchor1.Models;
using anchor1v.Models;
using System.Data.SqlClient;
using System.Data;

namespace anchor1.utilities
{


    public class Clogin
    {

        string err_msg;
        string comp_v1;
        string comp_v2;
        string comp_v3;


        public int check_company()
        {
            int ret_val = 0;
            err_msg = "";
            //dataconnect dat1 = (dataconnect)HttpContext.Current.Session["dat1"];
            //worksess worksess =  (worksess)HttpContext.Current.Session["worksess"];
            dataconnect dat1 = new dataconnect();
            worksess worksess = new worksess();
            worksess.err_msg = "";

            if (dat1 == null)
                dat1 = new Models.dataconnect();

            string company_code = dat1.serialno;
            if (company_code == null)
                company_code = read_comp();

            loginContext context = new loginContext();

            string str1 = "select name1 c0, user_date c1, data_code1 c2, data_code3 c3, pass_code c4,data_code c5,user_name c6,user_password c7 from tab_database where database_code='" + company_code + "'";
            str1 += " and sequence_no=0 ";
            //if (HttpContext.Current.Session["dbase"] != null)
            //    str1 += " and data_code1='" + HttpContext.Current.Session["dbase"].ToString() + "'";

            var rec1 = context.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            if (rec1 != null)
            {
                string staffno = Ccheckg.convert_pass1(rec1.c4, 1);
                string data_serial = rec1.c5;
                string q3 = data_serial.Substring(7, 1);
                string q1 = data_serial.Substring(17, 1);
                string q2 = data_serial.Substring(31, 1);
                string p1 = data_serial.Substring(19, 1);
                string p2 = data_serial.Substring(2, 1);
                string p3 = data_serial.Substring(23, 1);
                string duration = q1 + q2 + q3;
                DateTime dt = new DateTime(Convert.ToInt16(rec1.c1.Substring(0, 4)), Convert.ToInt16(rec1.c1.Substring(4, 2)), Convert.ToInt16(rec1.c1.Substring(6, 2)));
                double dura = 0;
                double.TryParse(duration, out dura);
                dt = dt.AddDays(dura);
                string enddate = dt.ToString("yyyyMMdd");

                data_serial = data_serial.Remove(31, 1);
                data_serial = data_serial.Remove(23, 1);
                data_serial = data_serial.Remove(19, 1);
                data_serial = data_serial.Remove(17, 1);
                data_serial = data_serial.Remove(7, 1);
                data_serial = data_serial.Remove(2, 1);

                comp_v1 = rec1.c6;
                comp_v2 = Ccheckg.convert_pass1(rec1.c7, 1);
                comp_v3 = rec1.c2;
                //HttpContext.Current.Session["s1"] = rec1.c3;

                //                string cheCcheckg = Ccheckg.GetMD5Hash(company_code + rec1.c0 + rec1.c1 + enddate + rec1.c2 + rec1.c3 + staffno + 12);
                string cheCcheckg = Ccheckg.GetMD5Hash(company_code + rec1.c0 + rec1.c1 + enddate + staffno + 12);
                //HttpContext.Current.Session["err_msg"] = "";

                if (cheCcheckg != data_serial)
                {
                    err_msg = "System Needs Serialization Code";
                    //HttpContext.Current.Session["err_msg"] = err_msg;
                    worksess.err_msg = err_msg;

                    //return 2;
                    ret_val = 2;
                }
                else
                {
                    // check for expiry date....
                    TimeSpan tdiff = dt - DateTime.Now;
                    if (tdiff.TotalDays <= 0)
                    {
                        err_msg = "The current Serialization Code had expired on " + enddate.Substring(6, 2) + "/" + enddate.Substring(4, 2) + "/" + enddate.Substring(0, 4);
                        //HttpContext.Current.Session["err_msg"] = err_msg;
                        //return 2;
                        worksess.err_msg = err_msg;
                        ret_val = 2;
                    }
                    else if (tdiff.TotalDays < 45)
                    {
                        err_msg = "The current Serialization Code will expire on " + enddate.Substring(6, 2) + "/" + enddate.Substring(4, 2) + "/" + enddate.Substring(0, 4);
                        err_msg += "\n \n You currently have " + ((int)tdiff.TotalDays).ToString() + " days left ";
                        //HttpContext.Current.Session["err_msg"] = err_msg;
                        worksess.err_msg = err_msg;
                        ret_val = 1;
                    }


                    //HttpContext.Current.Session["serialno"] = company_code;
                    //HttpContext.Current.Session["compname"] = rec1.c0;
                    //HttpContext.Current.Session["p1"] = p1 + p2 + p3;
                    ////HttpContext.Current.Session["q1"] = rec1.c4;
                    dat1.serialno = company_code;
                    dat1.company_name = rec1.c0;
                    dat1.p1 = p1 + p2 + p3;

                    HttpContext.Current.Session["dat1"] = dat1;

//                    return ret_val;
                }

            }
            else
            {
                worksess.err_msg = "Company not Setup!!!!!";
                //HttpContext.Current.Session["err_msg"] = err_msg;
                //HttpContext.Current.Session["serialno"] = "";
                //HttpContext.Current.Session["compname"] = "";
                //HttpContext.Current.Session["p1"] = "";

                //return 2;
                ret_val = 2;
            }

            HttpContext.Current.Session["worksess"] = worksess;
            return ret_val;

        }

        private string read_comp()
        {
            string line1;
            var sfile = System.Web.HttpContext.Current.Server.MapPath("~/content/soft.text");            

            StreamReader sr = new StreamReader(sfile);
            line1 = sr.ReadLine();
            return line1;

        }


        public SqlConnectionStringBuilder baseconnect()
        {
            dataconnect dat1 = (dataconnect)HttpContext.Current.Session["dat1"];
            if (dat1 == null)
                dat1 = new Models.dataconnect();

            if (dat1.dconnectstring == null)
            {
                //string spath = Directory.GetCurrentDirectory();
                string line1;
                var sfile = System.Web.HttpContext.Current.Server.MapPath("~/content/soft.text");
                int wctr = 0; string[] ara = new string[10];

                StreamReader sr = new StreamReader(sfile);
                line1 = sr.ReadLine();

                while (line1 != null)
                {
                    ara[wctr] = line1;
                    line1 = sr.ReadLine();
                    wctr++;
                }

                sr.Close();

                dat1.dconnectstring = ara[1];
                dat1.duser = ara[2];
                dat1.dpassword = ara[3];
                dat1.dcatalog = ara[4];
                dat1.dtimeout = Convert.ToInt16(ara[5]);
                dat1.serialno = ara[0];

            }

            string connectstring = "";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectstring);

            builder.ConnectionString = dat1.dconnectstring;
            builder.UserID = dat1.duser;
            builder.Password =  Ccheckg.convert_pass1(dat1.dpassword, 1);
            builder.InitialCatalog = dat1.dcatalog;
            builder.ConnectTimeout = dat1.dtimeout;

            HttpContext.Current.Session["dat1"] = dat1;

            return builder;
            
        }


        public SqlConnectionStringBuilder mydataconnect()
        {
            string connectstring = "";

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectstring);
            dataconnect dat1 = (dataconnect)HttpContext.Current.Session["dat1"];

            if (dat1 == null)
                return baseconnect();

            if (dat1.duser1 == null)
            {
                loginContext context = new loginContext();

                //if (HttpContext.Current.Session["serialno"] != null)
                if (dat1.serialno != null)
                {
                    string str1 = "select user_name c6,user_password c7,sequence_no t1,name1 c1,company_name c2 from tab_database where database_code='" + dat1.serialno + "'";
                    str1 += " and data_code1='" + dat1.dbase + "'";

                    var rec1 = context.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

                    if (rec1 != null)
                    {
                        dat1.duser1 = rec1.c6;
                        //dat1.dpassword1 = Ccheckg.convert_pass1(rec1.c7, 1);
                        dat1.dpassword1 = rec1.c7;
                        //HttpContext.Current.Session["jk1"] = rec1.c1;
                        //if (rec1.t1 != 0)
                        //    HttpContext.Current.Session["jk1"] = rec1.c1 + " : " + rec1.c2;
                    }
                    else
                        return baseconnect();
                }

            }

            builder.ConnectionString = dat1.dconnectstring;
            builder.UserID = dat1.duser1;
            builder.Password = Ccheckg.convert_pass1(dat1.dpassword1, 1);
            builder.InitialCatalog = dat1.dbase;
            builder.ConnectTimeout = dat1.dtimeout;

            HttpContext.Current.Session["dat1"] = dat1;

            //string test1 = dat1.basec.ConnectionString;
            //    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(test1);
            //    builder.UserID = comp_v1;
            //    builder.Password = comp_v2;
            //    string dbase = "";
            //    if (HttpContext.Current.Session["dbase"] != null)
            //        dbase = HttpContext.Current.Session["dbase"].ToString();

            //    builder.InitialCatalog = dbase;

            //    dat1.datac = builder;
            //    dat1.dbase = dbase;
            //    HttpContext.Current.Session["dat1"] = dat1;

            return builder;

        }

    
    }

}
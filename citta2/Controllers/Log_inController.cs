using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using anchor1.Models;
using anchor1v.Models;
using anchor1.utilities;
//using WebMatrix.WebData;
using System.IO;
using System.Data.SqlClient;
using anchor1.Filters;
using CittaErp.Models;

namespace CittaErp.Controllers
{
    public class log_inController : Controller
    {
        //

        Boolean err_flag = true;
        vw_collect mcollect = new vw_collect();
        Cutil utils;
        Cmail cmail ;
        Clogin clink = new Clogin();

        Boolean lock_flag = true;
        string w_changed;

        sideview sidedata = new sideview();
        mysessvals profiledata = new mysessvals();
        private  loginContext logdb ;
        loginContext context = new loginContext();
        dataconnect dat1;
        string sno = "";
        worksess worksess;
//        psess psess;

        string err_msg;
        int days_pass = 0; string 
            pass_change = ""; string aging = "N"; 
        string str1;


        [OverrideActionFilters]
        public ActionResult Index()
        {
            //init_sess(0);
            ViewBag.err_msg = "";
            mcollect.ws_int0 = 6;
            mcollect.ws_int0 = clink.check_company();
            
            dat1 = (dataconnect)Session["dat1"];
            mcollect.ws_string5= dat1.serialno;
            mcollect.ws_string6 = dat1.company_name;
            
            sno = mcollect.ws_string5;
            database_query();
            show_company();

            return View(mcollect);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(vw_collect mcollect1)
        {
            mcollect = mcollect1;
            dat1 = (dataconnect)Session["dat1"];
            worksess = (worksess)Session["worksess"];
            dat1.duser1 = null;
            dat1.dbase = mcollect.ws_string1;
            dat1.dbase = "Cittaaccount";
            Session["dat1"] = dat1;

//            if (clink.check_company() < 2)
            if (mcollect.ws_int0 < 2)
            {
                if (check_menu())
                {
                    Random rnd = new Random();
                    int rnd0 = rnd.Next();
                    worksess.xyr = rnd0;
                    Session["worksess"] = worksess;
                    if (w_changed == "Y")
                        return RedirectToAction("LChangePassword", null, new { anc = Ccheckg.convert_pass2("pc=") });
                    else
                    {
                        //if (check_approval())
                        //    return RedirectToAction("approval", "PEnquiry", new { anc = Ccheckg.convert_pass2("pc=1") });
                        //else
                            return RedirectToAction("Welcome");
                    }
                }
            }

            //init_sess();

            sno = mcollect.ws_string5;
            database_query();
            return View(mcollect);
        }

        [EncryptionAction]
        public ActionResult Welcome()
        {
            string datepatt=@"dddd, MMMM dd,   yyyy  hh:mm:ssss tt";
            pubsess pblock = (pubsess)Session["pubsess"];
            mcollect.ws_code  = pblock.userid;
            mcollect.ws_string0  = pblock.pname;
            mcollect.ws_string1 = pblock.lld.ToString(datepatt);
            return View(mcollect);
        }

        [EncryptionActionAttribute]
        public ActionResult LChangePassword(string pc=null)
        {
            if (pc == null)
                return RedirectToAction("Signout");

            anchor1Context db = new anchor1Context();
            LocalPasswordModel lpass = new LocalPasswordModel();
            var slist = (from bg in db.tab_train_default
                         where bg.default_code == "PASS"
                         select bg).FirstOrDefault();

            lpass.pass1 = slist.field5;
            lpass.pass2 = slist.field1;
            pubsess pblock = (pubsess)Session["pubsess"];
            lpass.pass3 = pblock.userid;
            question_query(db, pblock.userid);

            return View("_ChangePasswordPartial",lpass );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LChangePassword(LocalPasswordModel lpass)
        {
            anchor1Context db = new anchor1Context();
            utils = new Cutil();
            pubsess pblock = (pubsess)Session["pubsess"];
            Boolean val_flag = validate_change_password(lpass,db);
            if (val_flag)
                return RedirectToAction("Index");

            question_query(db, pblock.userid);

            return View("_ChangePasswordPartial",lpass );

        }

        public ActionResult LostPassword(string pc=null)
        {

            ViewBag.err_msg = "";
            mcollect.ws_int0 = 6;
            mcollect.ws_int0 = clink.check_company();

            dat1 = (dataconnect)Session["dat1"];
            mcollect.ws_string5 = dat1.serialno;
            mcollect.ws_string6 = dat1.company_name;

            sno = mcollect.ws_string5;
            database_query();

            return View(mcollect);

            //loginContext context = new loginContext();
            //dat1 = (dataconnect)Session["dat1"];
            //sno = dat1.serialno;
            //init_sess();
            //database_query();
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(vw_collect mcollect)
        {
            pubsess pblock = (pubsess)Session["pubsess"];
            dat1 = (dataconnect)Session["dat1"];
            worksess = (worksess)Session["worksess"];
            dat1.duser1 = null;
            dat1.dbase = mcollect.ws_string1;
            Session["dat1"] = dat1;

            Boolean val_flag = validate_lost_password(mcollect);
            if (val_flag)
                return RedirectToAction("Index");

            error_rtn(err_msg);
            sno = dat1.serialno;
            database_query();
            init_sess();

            return View(mcollect);
            

        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }


        private int down_carousel(anchor1Context db)
        {
            int ctr = 0;
            bool pic_flag;
            string coyname;
            string filelocation;
            string str1 = "select * from tab_docpara where hcode='UPLOAD' and substring(pcode,1,1)='C' ";
            var car1 = db.Database.SqlQuery<tab_docpara>(str1).ToList();
            foreach (var item in car1)
            {
                ctr++;
                coyname = "pic" + ctr.ToString() + ".png";
                filelocation = Path.Combine(Server.MapPath("~/uploads/"), coyname);
                pic_flag = true;

                if (System.IO.File.Exists(filelocation))
                {
                    DateTime dc = System.IO.File.GetCreationTime(filelocation);
                    TimeSpan d3 = DateTime.Now.Subtract(dc);
                    double timediff = d3.TotalMinutes;

                    if (timediff > 20)
                        System.IO.File.Delete(filelocation);
                    else
                        pic_flag = false;
                }

                if (pic_flag)
                {
                    byte[] imagedata = item.picture1;
                    System.IO.File.WriteAllBytes(filelocation, imagedata);
                }

            }

            return ctr;
        }

        private Boolean check_menu()
        {            
            err_msg = "";
            err_flag = false;
            int lock_timeout = 0;

//' check max of 10 mins
            int s2 = Convert.ToInt16(mcollect.ws_string3.Substring(0, 2));
            int s3 = Convert.ToInt16(mcollect.ws_string3.Substring(2, 2));
            int s1 = Convert.ToInt16(mcollect.ws_string3.Substring(4, 2));

            DateTime d1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, s1, s2, s3);
            TimeSpan d3 = DateTime.Now.Subtract(d1);
            double timediff = d3.TotalMinutes;

            if (timediff > 10)
                return error_rtn("Time out in validating your password, Pls login again.");

            // Session["dbase"] = mcollect.ws_string1;

            anchor1Context db = new anchor1Context();

            if (check_staff(db))
                return false;

            utils = new Cutil();

// valid login and staff
            var bgstaff = (from bg in db.vw_user where bg.user_code == mcollect.ws_code select bg).ToList();
            if (bgstaff.Count != 1)
                return error_rtn("Invalid Staff number and Password combination.");

            var bgstaff1 = bgstaff.First();
            if (mcollect.ws_string2 != Ccheckg.GetMD5Hash(bgstaff1.password_user + mcollect.ws_string3))
            {
                lock_user_login(mcollect.ws_code, bgstaff1.type1,db);
                return error_rtn("Invalid Staff number and Password combination.");
            }


            //'check if password has exceeded
            var bglist = from bg in db.tab_train_default
                         where bg.default_code == "pass"
                         select bg;

            var bglist1 = bglist.FirstOrDefault();
            if (bglist1 == null)
            {
                str1 = "insert into tab_train_default(default_code,field2,field3,field6,field7)values ('pass','c','n','0','0')";
                db.Database.ExecuteSqlCommand(str1);
                bglist1 = (from bg in db.tab_train_default where bg.default_code == "pass" select bg).FirstOrDefault();
            }

            days_pass = Convert.ToInt16(bglist1.field6);
            pass_change = bglist1.field2;
            aging = bglist1.field3;
            lock_timeout = Convert.ToInt16(bglist1.field7);

            DateTime todayu = utils.logdatetime();
            double pass_dur = todayu.Subtract(bgstaff1.password_date).TotalDays;
            if (((bgstaff1.user_group == "" & aging == "Y") | (bgstaff1.user_group != "")) & pass_dur > days_pass & days_pass > 0)
            {
                if (bgstaff1.type1 == "staff")
                {
                    if (pass_change == "C")
                        str1 = "update tab_staff set change_flag='Y' where staff_number =" + utils.pads(mcollect.ws_code);
                    else
                        str1 = "update tab_staff set locked_flag='Y',locked_date=getutcdate() where staff_number =" + utils.pads(mcollect.ws_code);
                }
                else
                {
                    if (pass_change == "C")
                        str1 = "update tab_local_user set change_flag='Y' where user_code =" + utils.pads(mcollect.ws_code);
                    else
                        str1 = "update tab_local_user set locked_flag='Y',locked_date=getutcdate() where user_code =" + utils.pads(mcollect.ws_code);
                }

                db.Database.ExecuteSqlCommand(str1);
            }

// check locked staff
            //str1 = "select locked_flag c0, locked_date d1, change_flag c1 from vw_user where user_code=" + utils.pads(mcollect.ws_code);
            //var locklist = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            if (bgstaff1.locked_flag == "Y")
            {
                if (lock_timeout == 0)
                    return error_rtn("Staff Locked out, See Administrator");
                else if (DateTime.UtcNow.Subtract(bgstaff1.locked_date).TotalMinutes < lock_timeout)
                    return error_rtn("Minimum Minutes for Unlocking : " + lock_timeout.ToString());
                else
                {
                    if (profiledata.type1 == "local")
                        str1 = "update tab_local_user set locked_flag='N', count_user=0 where user_code=" + utils.pads(mcollect.ws_code);
                    else
                        str1 = "update tab_staff set locked_flag='N', count_user=0 where staff_number=" + utils.pads(mcollect.ws_code);

                    db.Database.ExecuteSqlCommand(str1);
                }
            }


            err_flag=true;
            // valid user
            w_changed = bgstaff1.change_flag;

            str1 = "select a.field12 c0 , b.payroll_approval c1,c.field6 c2 ,d.field5 c3, e.field15 c9,b.item_row t1,b.code_query c10,a.field6 c11, ";
            str1 += " c.field10 c12 from tab_train_default a,tab_default b, tab_train_default c, tab_train_default d, tab_train_default e ";
            str1 += " where a.default_code='DATES'  and c.default_code='00000' and d.default_code='ENQUIRY'  and e.default_code='REMIND' ";
            var str17 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            profiledata.tzone = str17.c0;
            int k1 = 0;
            int.TryParse(str17.c11, out k1);
            profiledata.no_records = k1;
            profiledata.item_row = str17.t1;
            profiledata.cquery = str17.c10;
            profiledata.chapp = str17.c1;
            profiledata.hrapp = str17.c2;
            int.TryParse(str17.c12, out k1);
            profiledata.drop_lines = k1;

            int ctrq=down_carousel(db);
            worksess = (worksess)Session["worksess"];
            worksess.portalname = str17.c3;

            string[] pcar = new string[20];
            for (int jl = 0; jl < 20; jl++) pcar[jl] = "";

            if (ctrq > 0)
            {
                for (int jk = 0; jk < ctrq; jk++)
                {
                    pcar[jk] = "pic" + (jk+1).ToString() + ".png";
                }
            }
            //pcar[0] = str17.c4;
            //pcar[1] = str17.c5;
            //pcar[2] = str17.c6;
            //pcar[3] = str17.c7;
            //pcar[4] = str17.c8;
            worksess.pcar = pcar;
            //Session["psv"] = str17.c9;


            profiledata.userid = bgstaff1.user_code;
            profiledata.ugroup = bgstaff1.user_group;
            profiledata.lld = utils.logdatetime(str17.c0,bgstaff1.last_Login);
            profiledata.ugroup_type = bgstaff1.self_access;
            profiledata.datacode = bgstaff1.database_code;
            profiledata.link_date = DateTime.Now.ToString("yyyyMMdd");
            profiledata.type1 = bgstaff1.type1;
            profiledata.pname = bgstaff1.name1;
            profiledata.mcode3 = dat1.p1;
            profiledata.printer_code = bgstaff1.printer_user;

            var grprec = (from bg in db.tab_calc where bg.para_code == "21" && bg.calc_code == bgstaff1.user_group select new vw_collect { ws_code=bg.transfer_code,ws_string0=bg.report_type} ).FirstOrDefault();
            
            profiledata.type2 = grprec == null ? "U" : grprec.ws_code;
            profiledata.cquery2 = grprec == null ? "" : grprec.ws_string0 == "I" ? " Not " : "";
            string str2 = "select processing_period c1, calc_code c2 from tab_pay_default a, tab_calc b where b.para_code='UPDATE'and payment_cycle=transfer_code ";
            str2 += " and calc_code=(select default_pay_cycle from tab_default) ";

            var bgdefault = db.Database.SqlQuery<vw_query>(str2).FirstOrDefault();
            profiledata.processing_period = bgdefault == null ? "999912" : bgdefault.c1;
            profiledata.mcode2 = bgdefault == null ? "P" : bgdefault.c2;
            str1 = "select dbo.P2000(" + utils.pads(bgstaff1.user_code) + ") c1 ";
            var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            profiledata.p2000 = str11.c1;

            str1 = " select font_sizep t1,b.report_name1 c2,c.report_name1 c3 from tab_staff_person a ";
            str1 += " left outer join tab_soft b on (b.para_code='PFONT' and a.font_family1=b.report_code) ";
            str1 += " left outer join tab_soft c on (c.para_code='PFONT' and a.font_family2=c.report_code) ";
            str1 += " where a.staff_number=" + utils.pads(mcollect.ws_code);
            str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            fontclass sfont = new fontclass();
            if (str11 == null)
            {
                sfont.font_sizep = "";
                sfont.font_family1 = "";
            }
            else
            {
                sfont.font_sizep = str11.t1.ToString() + "px";
                sfont.font_family1 = "\'" + str11.c2 + "\',\'" + str11.c3 + "\'";
            }

            Session["sfont"] = sfont;
            Session["mysessval"] = profiledata;
            Session["worksess"] = worksess;
            // move to erp values
            init_values();
            menuRead();
            //Session.Remove("p1");

//side data
            string datepatt = @"dddd, MMMM dd,   yyyy  hh:mm:ssss tt";
            sidedata.ws_code = profiledata.userid;
            sidedata.ws_string0 = profiledata.pname;
            sidedata.ws_string1 = profiledata.lld.ToString(datepatt);
            sidedata.ws_string2 = mcollect.ws_string2;

            Session["sidedata"] = sidedata;

            if (profiledata.type1 == "local")
                str1 = "update tab_local_user set last_login=getutcdate(), count_user=0 where user_code=" + utils.pads(mcollect.ws_code);
            else
                str1 = "update tab_staff set last_login=getutcdate(), count_user=0 where staff_number=" + utils.pads(mcollect.ws_code);

            db.Database.ExecuteSqlCommand(str1);

            str1 = " exec initial_tables @p_userid=" + utils.pads(mcollect.ws_code);
            //db.Database.ExecuteSqlCommand(str1);

            return err_flag;

        }



        private Boolean error_rtn(string err_message)
        {
            
            ModelState.AddModelError("", err_message);
            //Session["err_msg"] = err_message;
            ViewBag.err_msg = err_message;
            //if (psess != null)
            //{
            //    //psess.err_msg = err_message; will check later
            //    Session["psess"] = psess;
            //}
            return false;

        }

        private void lock_user_login(string luser, string datacode, anchor1Context db)
        {
            //string dtime = utils.logdatetime().ToString("o").Substring(0, 23);
            if (datacode == "local")
            {
                str1 = "update tab_local_user set count_user=count_user + 1,locked_flag=case count_user+1 when 3 then 'Y' else 'N' end,";
                str1 += " locked_date= case count_user+1 when 3 then getutcdate() else locked_date end where user_code=" + utils.pads(luser);
            }
            else
            {
                str1 = "update tab_staff set count_user=count_user + 1,locked_flag=case count_user+1 when 3 then 'Y' else 'N' end,";
                str1 += " locked_date= case count_user+1 when 3 then getutcdate() else locked_date end where staff_number=" + utils.pads(luser);
            }

            db.Database.ExecuteSqlCommand(str1);
        }

        [OverrideActionFilters]
        public ActionResult Signout()
        {
            string bye_pname = " ";
            pubsess pblock = (pubsess)Session["pubsess"];
            if (pblock != null)
            {
                anchor1Context db = new anchor1Context();
                bye_pname = (from bg in db.vw_user where bg.user_code == pblock.userid select bg.name1).FirstOrDefault();
                str1 = " exec initial_tables @p_userid='" + pblock.userid + "'";
                db.Database.ExecuteSqlCommand(str1);
                init_sess(1);
            }

            ViewBag.byename = bye_pname;

            Session.Abandon();
            Session.Timeout = 1;

            return View();

        }

        [HttpPost]
        public ActionResult KeepAlive()
        {
            List<SelectListItem> loan1 = new List<SelectListItem>();
            SelectListItem ln2;
            ln2 = new SelectListItem() { Value = "", Text = ""};
            loan1.Add(ln2);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                loan1.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            return View();
        }

        private Boolean validate_change_password(LocalPasswordModel lpass, anchor1Context db)
        {
            err_flag = true;
            string dtime = utils.logdatetime().ToString("o").Substring(0, 23);

            var plist = (from bg in db.vw_user
                         where bg.user_code == lpass.pass3
                         select bg).FirstOrDefault();

            if (plist.password_user != lpass.OldPassword)
                error_rtn( "Old password not Same!!!");

            if (err_flag)
            {
                string c_pass = lpass.ConfirmPassword;
                if (c_pass == plist.password1 || c_pass == plist.password2 || c_pass == plist.password_user ||
                     c_pass == plist.password3 || c_pass == plist.password4 || c_pass == plist.password5 || c_pass == plist.password6)
                    error_rtn( "Invalid Password.... Password already used");

                string str1;
                if (err_flag)
                {
                    if (plist.type1 == "staff")
                    {
                        str1 = "update tab_staff set password6=password5, password5=password4, password4=password3, password3=password2, password2=password1, password1=password_user, password_user=";
                        str1 += utils.pads(lpass.ConfirmPassword) + ", password_date=convert(datetime,'" + dtime + "',126), change_flag='N' where staff_number=" + utils.pads(lpass.pass3);
                        db.Database.ExecuteSqlCommand(str1);
                        if (lpass.pass5 != "")
                        {
                            str1 = "update tab_staff set password_question=" + utils.pads(lpass.pass4) + ", password_answer=" + utils.pads(lpass.pass5) + " where staff_number=" + utils.pads(lpass.pass3);
                            db.Database.ExecuteSqlCommand(str1);
                        }
                    }
                    else
                    {
                        str1 = "update tab_local_user set password6=password5, password5=password4, password4=password3, password3=password2, password2=password1, password1=password_user, password_user=";
                        str1 += utils.pads(lpass.ConfirmPassword) + ", password_date=convert(datetime,'" + dtime + "',126), change_flag='N' where user_code=" + utils.pads(lpass.pass3);
                        db.Database.ExecuteSqlCommand(str1);
                        if (lpass.pass5 != "")
                        {
                            str1 = "update tab_local_user set password_question=" + utils.pads(lpass.pass4) + ", password_answer=" + utils.pads(lpass.pass5) + " where user_code=" + utils.pads(lpass.pass3);
                            db.Database.ExecuteSqlCommand(str1);
                        }
                    }
                }
            }

            return err_flag;
        }

        private Boolean validate_lost_password(vw_collect mcollect)
        {
            // do connection for database
            worksess = (worksess)Session["worksess"];
            Random rnd = new Random();
            int rnd0 = rnd.Next();
            worksess.xyr = rnd0;
            Session["worksess"] = worksess;

            err_flag = false;
            Boolean quest_flag = true;

            if (mcollect.ws_int0 < 2)
            {
                anchor1Context db = new anchor1Context();
                cmail = new Cmail();
                utils = new Cutil();

                err_flag = true;
                str1 = "select field8 c0 from tab_train_default where default_code='PASS' ";
                var slist = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                string quest_check = slist.c0;

                str1 = "select a.type1 c0,a.user_code c1, a.password_answer c2, isnull(b.date_of_birth,'99991231') c3 from vw_user a left outer join tab_staff b on (a.user_code=b.staff_number) where user_code =" + utils.pads(mcollect.ws_code);
                slist = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
                if (slist != null)
                {
                    if (quest_check == "Y")
                    {
                        if (slist.c2 != mcollect.ws_string2 && slist.c2 !="")
                            quest_flag = false;
                    }

                    if (quest_flag && ((slist.c0 == "staff" && utils.date_yyyymmdd(mcollect.ws_string0) == slist.c3) || (slist.c0 == "local" && "99991231" == slist.c3)))
                        err_flag = true;
                }
            }

            if (err_flag==false)
            {
                err_msg = "Details not Valid...";
                err_flag = false;
            }
            if (err_flag)
                cmail.init_userp(mcollect.ws_code);

            return err_flag;
        }

        private Boolean check_staff(anchor1Context db)
        {

            // logdb = new loginContext();

            if (!CheckConnection(db))
            {
                worksess.err_msg = "Database Connection not successful";
                err_flag = false;
                Session["worksess"] = worksess;
                return true;
            }

            err_flag = true;
            lock_flag = false;
            int staff1 = 0;

            str1 = "select count(0) t1 from tab_staff where close_code='0'";
            var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            int staff_count = str11.t1;

            str1 = "select pass_code c1 from tab_database where database_code='" + dat1.serialno + "' and sequence_no=0 ";
            var rec1 = context.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            string staff_buy = "0";
            if (rec1 !=null)
                staff_buy=Ccheckg.convert_pass1(rec1.c1, 1);

            int.TryParse(staff_buy, out staff1);

            if (staff_count > (staff1 * 0.9))
            {
                ViewBag.err_msg = "You have exceeded 90% of your staff count. \n \n Current Staff Count: " + staff_count.ToString() + " Purchased Staff Count : " + staff1.ToString();
                err_flag = false;
            }

            if (staff_count > (staff1 *1.10))
            {
                ViewBag.err_msg = "You have exceeded 100% of your staff count. \n \n Current Staff Count: " + staff_count.ToString() + " Purchased Staff Count : " + staff1.ToString();
                err_flag = false;
                lock_flag = true;
            }

            //Session.Remove("q1");
            return lock_flag;
        }

        [HttpPost]
        public ActionResult DailyList(string idx)
        {
            string str1 = "";
            string quest1 = "";
            dat1 = (dataconnect)Session["dat1"];
            int ws_ctr = idx.IndexOf("[]");
            dat1.dbase = idx.Substring(ws_ctr + 2);
            Session["dat1"] = dat1;
            string sno = idx.Substring(0, ws_ctr);
            anchor1Context db = new anchor1Context();

            str1 = "select field15 c1 from tab_train_default where default_code='REMIND' ";
            var str12 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str12.c1 == "Y")
            {
                str1 = "select b.name1 c1 from vw_user a, tab_bank b where a.user_code='" + sno + "' and b.para_code='22' and b.bank_code=a.password_question ";
                var str11 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

                if (str11 != null)
                    quest1 = str11.c1;
            }

            List<SelectListItem> loan1 = new List<SelectListItem>();
            SelectListItem ln2;
            ln2 = new SelectListItem() { Value = quest1, Text = quest1 };
            loan1.Add(ln2);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                loan1.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
        }


        private Boolean check_approval()
        {
            anchor1Context db = new anchor1Context();
            string table_name = profiledata.userid + "tapp";

            string str1 = " exec get_sapproval_transaction @table_name=" + utils.pads(table_name) + ", @qnumber=" + utils.pads(profiledata.userid);
            str1 += ", @link_user=" + utils.pads(profiledata.userid) + ",@qtype='',@request_flag=0 ";
            db.Database.ExecuteSqlCommand(str1);

//check for approval
            str1 = "select count(0) t1 from [" + table_name + "] where column6 in ('','A','D','L')";
            var str2=db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str2.t1 > 0)
                return true;

            str1 = " exec initial_tables @p_userid=" + utils.pads(mcollect.ws_code);
            db.Database.ExecuteSqlCommand(str1);

            return false;
        }

        private void database_query()
        {

            //loginContext context = new loginContext();

            //string sno="";
            //if (Session["serialno"] != null)
            //    sno = Session["serialno"].ToString();

            string str1 = "select data_code1 c0, company_name c1 from tab_database where database_code='" + sno +"'";
            var str2 = context.Database.SqlQuery<vw_query>(str1);
            ViewBag.ws_string1 = new SelectList(str2.ToList(), "c0", "c1");
        }

        private void init_sess(int ind=0)
        {
            Session.Remove("dat1");
            if (ind == 1)
            {
                Session.Clear();
                Session.Abandon();
            }

        }

        public static bool CheckConnection(anchor1Context mycontext)
        {
            try
            {
                mycontext.Database.Connection.Open();
                mycontext.Database.Connection.Close();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }

        private void question_query(anchor1Context db,string userid)
        {
            string psv = (from bh in db.tab_train_default where bh.default_code == "PASS" select bh.field8).FirstOrDefault();
            ViewBag.psv = psv;
            if (psv == "Y")
            {
                string qask = "select password_question c1 from vw_user where user_code ='" + (userid) + "'";
                var qaskan = db.Database.SqlQuery<vw_query>(qask).FirstOrDefault();
                var qlist = from bg in db.tab_bank
                            where bg.para_code == "22"
                            orderby bg.name1
                            select new { c1 = bg.bank_code, c2 = bg.name1 };

                ViewBag.qtime = new SelectList(qlist.ToList(), "c1", "c2", qaskan.c1);
            }

        }

        public void show_company()
        {
            string coyname = "company.png";
            string filelocation = Path.Combine(Server.MapPath("~/uploads/"), coyname);
            if (System.IO.File.Exists(filelocation))
            {
                DateTime dc = System.IO.File.GetCreationTime(filelocation);
                TimeSpan d3 = DateTime.Now.Subtract(dc);
                double timediff = d3.TotalMinutes;

                if (timediff > 20)
                    System.IO.File.Delete(filelocation);
                else
                    return;
            }

            logdb = new loginContext();
            string str1 = "select * from tab_photo_coy where staff_number='COMPANY'";
            var tab_photo = logdb.Database.SqlQuery<tab_photo_coy>(str1).FirstOrDefault();

            if (tab_photo != null)
            {
                byte[] imagedata = tab_photo.picture1;
                System.IO.File.WriteAllBytes(filelocation, imagedata);

            }
        }


        public void init_values()
        {
            MainContext db = new MainContext();
            pubsess pubsess = new pubsess();
            psess psess = new psess();

            pubsess.userid = profiledata.userid;
            pubsess.processing_period = profiledata.processing_period;
            pubsess.printer_code = profiledata.printer_code;
            pubsess.pname = profiledata.pname;

            var bg = (from fg in db.GB_001_COY
                      join fh in db.MC_001_CUREN
                      on new { a1 = fg.field7 } equals new { a1 = fh.currency_code }
                      where fg.id_code == "COYINFO"
                      select new { d1 = fg.field7, d2 = fh.currency_description, d3 = fg.field6 }).FirstOrDefault();

            pubsess.base_currency_code = bg == null ? "NA" : bg.d1;
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


            string curdate = DateTime.Now.ToString("yyyyMMdd");

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
                if (bgt.field8 == "Y" && !string.IsNullOrWhiteSpace(bgt.field7))
                {
                    if (Convert.ToInt32(bgt.field7) > Convert.ToInt32(curdate))
                    {
                        if (!string.IsNullOrWhiteSpace(bgt.field5))
                            wdatefrom = bgt.field5;
                        if (!string.IsNullOrWhiteSpace(bgt.field6))
                            wdateto = bgt.field6;
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
            Session["pubsess"] = pubsess;

            psess.pcar = worksess.pcar;
            psess.portalname = worksess.portalname;
            Session["psess"] = psess;

        }

        private void menuRead()
        {
           psess psess = (psess)Session["psess"];
            anchor1Context db = new anchor1Context();
            var str1 = "";
            char spc = '"';

            //string query = "select * vwstrarray0 from tab_soft where para_code = 'MAINSCRN' and rep_name2 = 'H'";
            //query += "order by case IsNumeric(report_name6) when 1 then Replicate('0', 100 - Len(report_name6)) +report_name6";
            //query += "else report_name6 end, report_name1";
            //var bglist = db.Database.SqlQuery<vw_genlay>(query);
            var bglist1 = (from bg in db.tab_soft
                           where bg.para_code == "MAINSCRN" && bg.rep_name2 == "H"
                           select bg).ToList();
            var bglist = bglist1.OrderBy(t => Convert.ToInt32(t.report_name6));
            foreach (var item in bglist)
            {

                str1 += "<li class=" + spc + "dropdown" + spc + ">\r\n";
                str1 += "<a href=" + "#" + " class=" + "dropdown-toggle" + " data-toggle=" + "dropdown" + "><b>" + item.report_name1 + "</b><span class=" + "caret" + "></span><span style = " + "font-size:12px;" + " class=" + "pull-right hidden-xs showopacity glyphicon glyphicon-home" + "></span></a>\r\n";
                str1 += "<ul class=" + "dropdown-menu" + " role=" + "menu" + ">\r\n";

                var bglist11 = (from bg in db.tab_soft
                                where bg.numeric_ind == item.report_code && bg.rep_name2 != "H"
                                select bg).ToList();
                var bglist2 = bglist11.OrderBy(t => Convert.ToInt32(t.report_name6));
                foreach (var sbitem in bglist2)
                {
                    str1 += "<li class=" + "dropdown-submenu" + ">\r\n";
                    str1 += "<a href = " + "#" + " class=" + "dropdown-toggle" + " data-toggle=" + "dropdown" + " id=" + "drop" + ">" + sbitem.report_name1 + "<span class=" + "caret" + "></span></a>\r\n";
                    str1 += "<ul class=" + "dropdown-menu a" + " role=" + "menu" + ">\r\n";
                    var bglist3 = (from bg in db.tab_soft
                                   where bg.para_code == "ITEM" && bg.rep_name2 == sbitem.rep_name1
                                   select bg).ToList();
                    foreach (var item3 in bglist3)
                    {
                        if (!string.IsNullOrWhiteSpace(item3.report_name5))
                        {
                            str1 += "<li class=" + "dropdown-submenu" + ">\r\n";
                            str1 += "<a href = " + "#" + " class=" + "dropdown-toggle" + " data-toggle=" + "dropdown" + " id=" + "drop" + ">" + item3.report_name1 + "<span class=" + "caret" + "></span></a>\r\n";
                            str1 += "<ul class=" + "dropdown-menu a" + " role=" + "menu" + ">\r\n";
                            var bglist40 = (from bg in db.tab_soft
                                           where bg.para_code == "ITEM" && bg.rep_name2 == item3.report_name5
                                           select bg).ToList();
                            var bglist4 = bglist40.OrderBy(t => Convert.ToInt32(t.report_name6));
                            foreach (var item4 in bglist4)
                            {
                                if (!string.IsNullOrWhiteSpace(item4.report_name5))
                                {
                                    menu_head1(item4.report_name1, item4.report_name5, db);
                                }
                                else
                                    str1 += "<li><a href = '../menu/itemrun/" + item4.report_code + "[]" + item4.para_code + "[]" + "" + "'> " + item4.report_name1 + "</a></li>\r\n";

                            }
                            str1 += "</ul>\r\n";
                            str1 += "</li>\r\n";
                            str1 += "<li class='divider'></li>\r\n";

                        }
                        else
                            str1 += "<li><a href = '../menu/itemrun/" + item3.report_code + "[]" + item3.para_code + "[]" + "" + "'> " + item3.report_name1 + "</a></li>\r\n";

                    }
                    str1 += "</ul>\r\n";
                    str1 += "</li>\r\n";
                    str1 += "<li class='divider'></li>\r\n";


                }
                str1 += "</ul>\r\n";
                str1 += "</li>\r\n";

                //Gltran_enq / Create ? ptype1 = 001
            }
            //}
            psess.temp10 = str1;
            Session["psess"] = psess;
            //            return View(viewName:"~/Views/Shared/menu.cshtml");
            
        }

        private void menu_head1(string menu_name, string menu_code, anchor1Context db)
        {
            str1 += "<li class=" + "dropdown-submenu" + ">\r\n";
            str1 += "<a href = " + "#" + " class=" + "dropdown-toggle" + " data-toggle=" + "dropdown" + " id=" + "drop" + ">" + menu_name + "<span class=" + "caret" + "></span></a>\r\n";
            str1 += "<ul class=" + "dropdown-menu a" + " role=" + "menu" + ">\r\n";
            var bglist411 = (from bg in db.tab_soft
                           where bg.para_code == "ITEM" && bg.rep_name2 == menu_code
                           select bg).ToList();
            var bglist41 = bglist411.OrderBy(t => Convert.ToInt32(t.report_name6));
            foreach (var item41 in bglist41)
            {
                if (!string.IsNullOrWhiteSpace(item41.report_name5))
                {
                    menu_head1(item41.report_name1, item41.report_name5, db);
                }
                else
                {
                    str1 += "<li><a href = '../menu/itemrun/" + item41.report_code + "[]" + item41.para_code + "[]" + "" + "'> " + item41.report_name1 + "</a></li>\r\n";

                }
            }
            str1 += "</ul>\r\n";
            str1 += "</li>\r\n";
            str1 += "<li class='divider'></li>\r\n";

        }

    }
}
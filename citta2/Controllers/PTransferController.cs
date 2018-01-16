using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using anchor1.Models;
using anchor1.utilities;
using anchor1.Filters;
using System.Data.Entity;
using System.IO;
using System.Web.Hosting;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.Data.SqlClient;
using ExcelDataReader;

namespace anchor1.Controllers
{
    public class PTransferController : Controller
    {
        private anchor1Context db = new anchor1Context();
        private anchor1Context dbread = new anchor1Context();
        Cutil utils = new Cutil();
//        Ctransfer  ctransfer = new Ctransfer();
        mysessvals pblock;
        vw_collect mcollect = new vw_collect();
        string push1; 
        string sp = new string(' ', 50);
        string opt;
        string act_type;
        int maxcol = 0;
        bool err_flag = true;
        HttpPostedFileBase upload;

        //
        // GET: /PTransfer/

        [EncryptionActionAttribute]
        public ActionResult Index(string pc=null)
        {
            if (utils.check_option() == 1||pc==null)
                return RedirectToAction("Welcome", "Home");

            pblock = (mysessvals)Session["mysessvals"];
            opt = pc;
//            Session["opt"] = opt;
            mcollect.ws_string10 = opt;
            bank_group_select();
            return View(mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase tfile1, vw_collect mcollect1)
        {
            pblock = (mysessvals)Session["mysessvals"];
            string filelocation = "";
            mcollect = mcollect1;
            err_flag = true;
            upload = tfile1;

            if (string.IsNullOrWhiteSpace(mcollect.ws_string0))
            {
                ModelState.AddModelError(String.Empty, "Select the Screen");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string1))
            {
                ModelState.AddModelError(String.Empty, "Select the mode");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string2))
            {
                ModelState.AddModelError(String.Empty, "Select the Transfer Layout");
                err_flag = false;
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string3))
            {
                ModelState.AddModelError(String.Empty, "Select the File Type");
                err_flag = false;
            }

            if (mcollect.ws_string1 == "I" && mcollect.ws_string3 == "E" && string.IsNullOrWhiteSpace(mcollect.ws_string4))
            {
                ModelState.AddModelError(String.Empty, "Pls specify the excel sheet");
                err_flag = false;
            }

            if (mcollect.ws_string1 == "I" && (tfile1 == null || tfile1 != null && tfile1.ContentLength == 0))
            {
                ModelState.AddModelError(String.Empty, "Pls specify the excel workbook");
                err_flag = false;
            }

            if (!err_flag)
            {
                bank_group_select();
                return View(mcollect);
            }

            push1 = mcollect.ws_string1 + (mcollect.ws_string2 + sp).Substring(0, 10);
            push1 += mcollect.ws_string3 + (mcollect.ws_string5 + sp).Substring(0, 10);
            push1 += (mcollect.ws_string6 + sp).Substring(0, 10) + (mcollect.ws_string7 + sp).Substring(0, 10);
            push1 += (mcollect.ws_string8 + sp).Substring(0, 10);
            push1 += mcollect.ws_bool0 ? "Y " : "N ";
            //push1 += mcollect.ws_string4;
            
            string str1 = "select count(0) t1 from tab_array where para_code='TT' and array_code=" + utils.pads(mcollect.ws_string2);
            var str21 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            mcollect.ws_decimal8 = str21.t1;
            maxcol=str21.t1;

            err_flag = true;
            if (mcollect.ws_string1 == "I")
            {
                ptransfer_excel(filelocation, mcollect.ws_string4);

                if (!err_flag)
                {
                    ModelState.AddModelError(String.Empty, "Excel sheet name not valid");
                    bank_group_select();
                    return View(mcollect);
                }
            }

            push1 += mcollect.ws_string10 + maxcol.ToString();
            str1 = "execute para1_out @sel_str=" + utils.pads(push1) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

            if (mcollect.ws_string1 == "O")
            {
                string str2 = "select rcol c1 from [" + pblock.userid + "transf1] ";
                var str3 = db.Database.SqlQuery<vw_query>(str2);

                string filename = Path.Combine(Server.MapPath("~/uploads/"), pblock.userid + "transfer");
                if (System.IO.File.Exists(filename))
                    System.IO.File.Delete(filename);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
                {
                    foreach (var item in str3.ToList())
                    {
                        file.WriteLine(item.c1);
                    }
                }


               return  DownloadFile(mcollect.ws_string9);

            }
            utils.set_titles("", "Transfer upload report ");
            return RedirectToAction( "ErrorRep","RepScrn");
        }


        private void bank_group_select()
        {
            //opt = Session["opt"].ToString();
            opt = mcollect.ws_string10;
            var bglist = from hh in db.tab_soft
                         where hh.report_name9.StartsWith(opt) && hh.para_code == "TR"
                         orderby hh.report_name1 
                         select new { c1 = hh.report_code, c2 = hh.report_name1 };

            ViewBag.screen = new SelectList(bglist.ToList(), "c1", "c2");
 
            if (opt == "TT")
            {
                var bglist1 = from bg1 in db.tab_calc
                              join bg2 in db.tab_type
                              on new { a1 = bg1.report_name } equals new { a1 = bg2.trans_type }
                              into bg3
                              from bg4 in bg3
                              where bg1.para_code == "TTD"
                              select new { c1 = bg4.trans_type, c2 = bg4.name1 };

                ViewBag.screen = new SelectList(bglist1.ToList(), "c1", "c2");
            }

            
            string str1 = "";
            if (opt == "TT")
            {
                str1 = "select calc_code as c1, name1 as c2, '' from tab_calc where para_code = 'TTD' and report_name = " + utils.pads(mcollect.ws_string0) + " order by name1 ";
            }
            else
            {
                str1 = "select calc_code as c1, name1 as c2, '' from tab_calc a, tab_soft b where a.para_code = 'TT' and b.para_code='TR' and a.report_name=b.report_code ";
                str1 += " and a.report_name = " + utils.pads(mcollect.ws_string0) + " order by name1 ";
            }

            var str2 = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.layout = new SelectList(str2.ToList(), "c1", "c2", mcollect.ws_string2);

        }

        [EncryptionActionAttribute]
        public ActionResult create_file(string pc = null)
        {
            if (utils.check_option() == 1||pc==null)
                return RedirectToAction("Welcome", "Home");

            pblock = (mysessvals)Session["mysessvals"];
            utils.set_titles("", "Accounting File");
            act_type = pc;
            mcollect.ws_string3 = pblock.processing_period.Substring(0, 4);
            mcollect.ws_string2 = pblock.processing_period.Substring(4, 2);
            mcollect.ws_string9 = act_type;
            query_select();
            return View(mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create_file(vw_collect mcollect1)
        {
            pblock = (mysessvals)Session["mysessvals"];
            mcollect = mcollect1;
            push1 = mcollect.ws_string3;
            push1 += ("0" + mcollect.ws_string2).Substring(Convert.ToInt16(mcollect.ws_string2.Length) - 1);
            push1 += (mcollect.ws_string0 + sp).Substring(0, 10) + (mcollect.ws_string4 + sp).Substring(0, 10) + mcollect.ws_string7;
            push1 += ("0" + mcollect.ws_string6).Substring(Convert.ToInt16(mcollect.ws_string6.Length) - 1);
            push1 += (mcollect1.ws_string8 +sp).Substring(0,20);
            string str1 = " execute get_post1 @select_str= " + utils.pads(push1) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);
            // get the entries into a file

            string filename = utils.write_file(mcollect.ws_string9, mcollect.ws_string4,pblock.userid);
            
            return File(filename, "application/plain", mcollect.ws_string8);
                
        }


        private void query_select()
        {
            List<SelectListItem> pyears = new List<SelectListItem>();
            SelectListItem pyvar;
            int cperiod = Convert.ToInt16(pblock.processing_period.Substring(0, 4));
            int start1 = cperiod - 10;   // Convert.ToInt16(utils.default_period().starting_year);
            int stop1 = cperiod + 10;    // Convert.ToInt16(utils.default_period().stopping_year);

            for (int ctr = start1; ctr <= stop1; ctr++)
            {
                pyvar = new SelectListItem()
                {
                    Value = Convert.ToString(ctr),
                    Text = Convert.ToString(ctr)
                };
                pyears.Add(pyvar);
            }
            ViewBag.pyear = new SelectList(pyears.ToList(), "Value", "Text", mcollect.ws_string1 );
            ViewBag.pyear2 = ViewBag.pyear;
            List<SelectListItem> pyears2 = new List<SelectListItem>();

            for (int ctr = 1; ctr <= 52; ctr++)
            {
                pyvar = new SelectListItem()
                {
                    Value = Convert.ToString(ctr),
                    Text = Convert.ToString(ctr)
                };
                pyears2.Add(pyvar);
            }
            ViewBag.pmonth2 = new SelectList(pyears2.ToList(), "Value", "Text",Convert.ToUInt16(mcollect.ws_string0)) ;


            var bglist1 = from bg in db.tab_calend
                          orderby bg.sequence, bg.count_seq
                          select bg;

            ViewBag.pmonth = new SelectList(bglist1.ToList(), "count_seq", "period_name", Convert.ToUInt16(mcollect.ws_string0));

            var plist = from pl in db.tab_calc
                        where pl.para_code == "UPDATE"
                        orderby pl.line_spacing
                        select pl;
            ViewBag.update = new SelectList(plist.ToList(), "calc_code", "name1");

            string str1 = "select distinct layout_code as c1, ' ' as c2 from tab_layout where para_code=" + utils.pads(act_type) + " order by layout_code";
            var bglist2 = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.layout = new SelectList(bglist2.ToList(), "c1", "c1");

        }


        [HttpPost]
        public ActionResult DailyList(string idx)
        {
            string str1 = "";
            int ctr1 = idx.IndexOf("[]");
            string sno = idx.Substring(0, ctr1);
            int ctr2 = idx.IndexOf("[]", ctr1 + 2);
            string opt = idx.Substring(ctr1 + 2, ctr2 - ctr1 - 2);
            string Code = idx.Substring(ctr2 + 2);

            if (sno == "01")
            {
                if (opt == "TT")
                {
                    str1 = "select calc_code as c1, name1 as c2, '' from tab_calc where para_code = 'TTD' and report_name = " + utils.pads(Code) + " order by name1 ";
                }
                else
                {
                    str1 = "select calc_code as c1, name1 as c2, '' from tab_calc a, tab_soft b where a.para_code = 'TT' and b.para_code='TR' and a.report_name=b.report_code ";
                    str1 += " and a.report_name = " + utils.pads(Code) + " order by name1 ";
                }

                var str2 = db.Database.SqlQuery<vw_query>(str1).OrderBy(u => u.c2);
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    str2.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
            }
            else if (sno == "02")
            {
                List<SelectListItem> loan1 = new List<SelectListItem>();
                SelectListItem ln2;

                if (opt == "TT")
                {
                    ln2 = new SelectListItem() { Value = "staff", Text = "Staff Number" };
                    loan1.Add(ln2);
                }
                else
                {
                    var slist = (from s1 in db.tab_soft
                                 where s1.para_code == "TR" && s1.report_code == Code
                                 select s1).FirstOrDefault();
                    if (slist != null)
                    {
                        ln2 = new SelectListItem() { Value = "staff", Text = slist.rep_name1.Trim() };
                        loan1.Add(ln2);
                        ln2 = new SelectListItem() { Value = "code", Text = slist.rep_name2.Trim() };
                        loan1.Add(ln2);
                    }
                }

                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    loan1.ToArray(),
                                    "Value",
                                    "Text")
                               , JsonRequestBehavior.AllowGet);
            }

            
            return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});
        }

        [EncryptionActionAttribute]
        public ActionResult batch(string pc = null)
        {
            if (utils.check_option() == 1||pc==null)
                return RedirectToAction("Welcome", "Home");

            utils.set_titles("", "Batch Image Transfer");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Batch(HttpPostedFileBase tfile1)
        {

            string Line1;
            int count1; int count2;
            string snumber; string photo_type; string doc_code; string doc_type; string doc_name; string doc_location;
            pblock = (mysessvals)Session["mysessvals"];
            string puserid = pblock.userid;
            string  path1;

            if (tfile1 != null && tfile1.ContentLength > 0)
            {
                var fileName = Path.GetFileName(tfile1.FileName);
                path1 = Path.Combine(Server.MapPath("~/uploads/"), fileName);
                tfile1.SaveAs(path1);
            }
            else
                return View();


            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(path1);
            while ((Line1 = file.ReadLine()) != null)
            {
                //' breakup into number,type,docname and description

                count1 = Line1.IndexOf("}");
                snumber = Line1.Substring(0, count1);
                count2 = Line1.IndexOf("}", count1 + 1);
                photo_type = Line1.Substring(count1 + 1, count2 - count1 - 1);
                count1 = Line1.IndexOf("}", count2 + 1);
                doc_code = Line1.Substring(count2 + 1, count1 - count2 - 1);
                count2 = Line1.IndexOf("}", count1 + 1);
                doc_type = Line1.Substring(count1 + 1, count2 - count1 - 1);
                count1 = Line1.IndexOf("}", count2 + 1);
                doc_name = Line1.Substring(count2 + 1, count1 - count2 - 1);
                count2 = Line1.IndexOf("}", count1 + 1);
                doc_location = Line1.Substring(count1 + 1);

                DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/uploads"));
                string doc_loc1 = Path.Combine(dirInfo.FullName, doc_location);

                // Load file meta data with FileInfo
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(doc_loc1);

                // The byte[] to save the data in
                byte[] data = new byte[fileInfo.Length];

                // Load a filestream and put its content into the byte[]
                using (System.IO.FileStream fs = fileInfo.OpenRead())
                {
                    fs.Read(data, 0, data.Length);
                }

                // Delete the temporary file
                fileInfo.Delete();
                DateTime odate = utils.logdatetime();

                tab_photo tab_photo = db.tab_photo.Find(snumber, photo_type, doc_code);
                if (tab_photo == null)
                {
                    tab_photo.approval_user = puserid;
                    tab_photo.date_approve = odate;
                    tab_photo.document_access = "00";
                    tab_photo.document_code = doc_code;
                    tab_photo.document_name = doc_name;
                    tab_photo.document_type = doc_type;
                    tab_photo.down_count = 0;
                    tab_photo.top_count = 0;
                    tab_photo.input_date = odate;
                    tab_photo.internal_use = "N";
                    tab_photo.photo_type = photo_type;
                    tab_photo.picture1 = data;
                    tab_photo.processed = 0;
                    tab_photo.request_user = puserid;
                    tab_photo.staff_number = snumber;

                    db.tab_photo.Add(tab_photo);

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

                    }

                }
            }

            file.Close();

            return RedirectToAction("Welcome", "Home");

        }


        [EncryptionActionAttribute]
        public ActionResult preport(string pc = null)
        {
            //codename=string1
            //othername=string2
            //dname=string3
            pblock = (mysessvals)Session["mysessvals"];
            
            string str2 = "";
            string str1 = "select report_name1 c1, report_name2 c2, report_name3 c3,report_name6 c6, report_name7 c7 from tab_soft where para_code='LOG' and report_code=" + utils.pads(pc);
	        var rec3=db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (rec3 != null)
            {
                mcollect.ws_string1 = rec3.c2;
                mcollect.ws_string2 = rec3.c3;
                utils.set_titles("", rec3.c1 + " Report");
                str1 = (rec3.c6).Trim();
                str2 = (rec3.c7).Trim();
                //                If dcode = "H23" Then get_location
            }
            else
            {
                mcollect.ws_string1 = "Staff Number ";
                if (pc == "SKILL")
                    utils.set_titles("", "Staff Skill Transaction Report");

                str1 = "select staff_number, staff_name from tab_staff ";
            }

            if (str1=="")
                str1 = "select report_name1 c1, report_name2 c2, report_name3 c3,report_name6 c6, report_name7 c7 from tab_soft where para_code='LOG' and report_code=" + utils.pads(pc);

            var str4 = db.Database.SqlQuery<vw_query>(str1);
            ViewBag.codequery = new SelectList(str4.ToList(), "c1", "c2");
            if (str2 != "")
            {
                var str5 = db.Database.SqlQuery<vw_query>(str2);
                ViewBag.numquery = new SelectList(str5.ToList(), "c1", "c2");
            }

            mcollect.ws_string0 = pc;
            return View(mcollect);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult preport(vw_collect mcollect1)
        {
            pblock = (mysessvals)Session["mysessvals"];

            mcollect = mcollect1;
            if (string.IsNullOrWhiteSpace(mcollect.ws_string4))
            {
                mcollect.ws_string4 = "";
                mcollect.ws_string5 = "";
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string6))
            {
                mcollect.ws_string6 = "";
                mcollect.ws_string7 = "";
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string5))
                mcollect.ws_string5 = mcollect.ws_string4;
            if (string.IsNullOrWhiteSpace(mcollect.ws_string7))
                mcollect.ws_string7 = mcollect.ws_string6;

            push1 = (mcollect.ws_string0 + sp).Substring(0, 10) + (mcollect.ws_string4 + sp).Substring(0, 10) + (mcollect.ws_string5 + sp).Substring(0, 10);
            push1 += (mcollect.ws_string6 + sp).Substring(0, 10) + (mcollect.ws_string7 + sp).Substring(0, 10);

            string str1 = "execute preport @select_str=" + utils.pads(push1) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

// to convert date to zone date
//            zonedate_update(mcollect.ws_string0);

            //string tr1 = Session["id"].ToString();
            var str2 = (from bg in db.tab_soft
                       where bg.para_code == "LOGREP" && bg.report_code == mcollect.ws_string0
                       select bg).FirstOrDefault();
            utils.set_titles("", str2.report_name1);

            return RedirectToAction("coldisp","RepScrn");

        }

        [EncryptionActionAttribute]
        public ActionResult Selpreport(string pc = null)
        {
            if (utils.check_option() == 1||pc==null)
                return RedirectToAction("Welcome", "Home");

            var sl = from bg in db.tab_soft
                     where bg.para_code == "LOG" && bg.report_name9.StartsWith(pc)
                     orderby bg.report_name1 
                     select bg;

            //ViewBag.selreport = new SelectList(sl.ToList(), "report_code", "report_name1");
            mcollect.ws_string5 = pc;
            return View(mcollect);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Selpreport(vw_collect mcollect1)
        {
            return RedirectToAction("preport", null, new { anc = Ccheckg.convert_pass2("pc=" + mcollect1.ws_string0) });
        }


        [EncryptionActionAttribute]
        public ActionResult Logpreport(string pc = null)
        {
            if (utils.check_option() == 1||pc==null)
                return RedirectToAction("Welcome", "Home");

            var sl = from bg in db.tab_soft
                     where bg.para_code == "LOG" && bg.report_name9.StartsWith(pc)
                     orderby bg.report_name1
                     select bg;

            //ViewBag.selreport = new SelectList(sl.ToList(), "report_code", "report_name1");
            mcollect.ws_string5 = pc;

            return View(mcollect);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logpreport(vw_collect mcollect1)
        {
            pblock = (mysessvals)Session["mysessvals"];

            mcollect = mcollect1;
            if (string.IsNullOrWhiteSpace(mcollect.ws_string1))
            {
                mcollect.ws_string1 = "";
                mcollect.ws_string2 = "";
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string3))
            {
                mcollect.ws_string3 = "";
                mcollect.ws_string4 = "";
            }

            if (string.IsNullOrWhiteSpace(mcollect.ws_string2))
                mcollect.ws_string2 = mcollect.ws_string1;
            if (string.IsNullOrWhiteSpace(mcollect.ws_string4))
                mcollect.ws_string4 = mcollect.ws_string3;

            string tempv="";
            if (mcollect.ar_bool0[0])
                tempv += "N";
            if (mcollect.ar_bool0[1])
                tempv += "A";
            if (mcollect.ar_bool0[2])
                tempv += "D";
            tempv = (tempv + sp).Substring(0, 3);

            string push1 = mcollect.ws_string0;
            push1 = (push1 + sp).Substring(0, 10) + tempv + (mcollect.ws_string1 + sp).Substring(0, 10) + (mcollect.ws_string2 + sp).Substring(0, 10);
            push1 += (utils.date_yyyymmdd(mcollect.ws_string3) + sp).Substring(0, 10) + (utils.date_yyyymmdd(mcollect.ws_string4) + sp).Substring(0, 10);

            string str1 = "execute log_preport @select_str=" + utils.pads(push1) + ", @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);

           // zonedate_update(mcollect.ws_string0);

            string tr1 = mcollect.ws_string0;
            var str2 = (from bg in db.tab_soft
                        where bg.para_code == "LOGREP" && bg.report_code == tr1
                        select bg).FirstOrDefault();
            utils.set_titles("", str2.report_name2);

            return RedirectToAction("coldisp", "RepScrn");

        }

        private void ptransfer_excel(string filename, string sheetname)
        {
            Stream stream = upload.InputStream;

            IExcelDataReader reader = null;

            if (upload.FileName.EndsWith(".xls"))
            {

                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (upload.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            else
            {
                ModelState.AddModelError("File", "This file format is not supported");
                err_flag = false;
                return;
            }

            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (data) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
            });

            var excel_table1 = result.Tables;
            var result1 = excel_table1[sheetname];

            reader.Close();

            //DataSet ds = new DataSet();
            //string fileExtension = System.IO.Path.GetExtension(filename);

            //if (fileExtension == ".xls" || fileExtension == ".xlsx")
            //{
            //    string excelConnectionString = string.Empty;
            //    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            //    filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            //    //connection String for xls file format.
            //    if (fileExtension == ".xls")
            //    {
            //        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
            //        filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            //    }
            //    //connection String for xlsx file format.
            //    else if (fileExtension == ".xlsx")
            //    {
            //        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            //        filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            //    }

            //    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

            //    string query = "Select * from [" + sheetname + "$]";
            //    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
            //    {
            //        try
            //        {
            //            dataAdapter.Fill(ds);
            //        }
            //        catch (Exception err)
            //        {
            //            err_flag = false;
            //            return;
            //        }
            //    }
            //}

            //if (fileExtension.ToString().ToLower().Equals(".xml"))
            //{
            //    XmlTextReader xmlreader = new XmlTextReader(filename);
            //    // DataSet ds = new DataSet();
            //    ds.ReadXml(xmlreader);
            //    xmlreader.Close();
            //}

// get the max columns from the code use
            string sline1="";
            string sline2="";
            string sline3="";
            string table1=pblock.userid + "transf1";

// check max column
            maxcol = Convert.ToInt16(mcollect.ws_decimal8);

            sline1="execute col_table_create @tablename="+ utils.pads(table1);
            db.Database.ExecuteSqlCommand(sline1);

            maxcol = result1.Columns.Count;
            if (maxcol > 40)
            {
                for (int ctr = 41; ctr <= maxcol; ctr++)
                {
                    sline2 += ", column" + ctr.ToString() + " varchar(500) ";
                }
                if (sline2 != "")
                {
                    sline2 = "alter table [" + pblock.userid + "transf1] add " + sline2.Substring(1, sline2.Length - 1);
                    db.Database.ExecuteSqlCommand(sline2);
                }
            }

            string colopt = "";
            if (maxcol != 0)
            {
                for (int i = 0; i < result1.Rows.Count; i++)
                {
                    colopt = result1.Rows[i][0].ToString();
                    if (colopt == "N" || colopt == "D" || colopt == "U")
                    {
                        sline3 = "Insert into [" + table1 + "](";
                        sline1 = "snumber";
                        sline2 = utils.pads(result1.Rows[i][0].ToString());
                        for (int ctr = 1; ctr < maxcol; ctr++)
                        {
                            sline1 += ",column" + ctr.ToString();
                            sline2 += "," + utils.pads(result1.Rows[i][ctr].ToString());
                        }

                        sline3 = sline3 + sline1 + ") Values(" + sline2 + ")";
                        db.Database.ExecuteSqlCommand(sline3);
                    }
                }
            }


        }

        [EncryptionActionAttribute]
        public ActionResult DownloadFile(string name1)
        {
            string filename = Path.Combine(Server.MapPath("~/uploads/"), pblock.userid + "transfer");
            string locdirect = Server.MapPath("~/uploads/");

            if (!filename.StartsWith(locdirect))
            {
                // Ensure that we are serving file only inside the App_Data folder
                // and block requests outside like "../web.config"
                throw new HttpException(403, "Forbidden");
            }

            if (!System.IO.File.Exists(filename))
            {
                ModelState.AddModelError(String.Empty, "No file(s) created.");
            }


            return File(filename, System.Net.Mime.MediaTypeNames.Application.Octet, name1);
        }


        private void zonedate_update(string scode)
        {
            // to convert date to zone date
            string str1 = "select 'column'+report_name4 c0 from tab_soft where para_code='AREPT' and report_code like (select rep_name1 from tab_soft where para_code='LOGREP' and report_code=" + utils.pads(scode);
            str1 += " ) and numeric_ind='DL' ";
            var str1q = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str1q != null)
            {
                string colname = str1q.c0;
                str1 = "select " + colname + " c0, rectr t1 from [" + pblock.userid + "st01] ";
                var str2q = db.Database.SqlQuery<vw_query>(str1);
                foreach (var item in str2q.ToList())
                {
                    str1 = "update [" + pblock.userid + "st01] set " + colname + " = " + utils.pads((utils.logdatetime(pblock.tzone, Convert.ToDateTime(item.c0))).ToString("F"));
                    str1 += "  where rectr=" + item.t1.ToString();
                    dbread.Database.ExecuteSqlCommand(str1);
                }
            }
            str1 = "alter table [" + pblock.userid + "st01] drop column rectr";
            db.Database.ExecuteSqlCommand(str1);
            str1 = "execute insert_table_headers @p_userid=" + utils.pads(pblock.userid);
            db.Database.ExecuteSqlCommand(str1);


        }


        [EncryptionActionAttribute]
        public ActionResult Sap02(string pc = null)
        {
            if (utils.check_option() == 1 || pc == null)
                return RedirectToAction("Welcome", "Home");

            pblock = (mysessvals)Session["mysessvals"];
            utils.set_titles("", "SAP Upload ");

            return View(mcollect);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sap02(HttpPostedFileBase tfile, vw_collect mcollect1)
        {
            pblock = (mysessvals)Session["mysessvals"];
            string str1 = "";
            mcollect = mcollect1;
            err_flag = true;

            string vstr; string dstr;
            int colcount = 0;

            if ((tfile == null || tfile != null && tfile.ContentLength == 0))
            {
                ModelState.AddModelError(String.Empty, "Pls specify the filename");
                err_flag = false;
            }

            if (!err_flag)
            {
                return View(mcollect);
            }

            string delete_flag = "N";
            if (mcollect.ws_bool0) delete_flag = "Y";

            string idname = pblock.userid + "sap01";
            str1 = " execute col_table_create @tablename=" + utils.pads(idname);
            db.Database.ExecuteSqlCommand(str1);
            int maxrow = 40;

            string path = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = path + Path.GetFileName(tfile.FileName);
            tfile.SaveAs(filePath);

            //Read the contents of CSV file.
            string csvData = System.IO.File.ReadAllText(filePath);
            maxcol = 0;
            //Execute a loop over the rows.
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    string[] ws_alpha = row.Split(';');
                    colcount = ws_alpha.Length;

                    if (colcount > maxrow)
                    {
                        adjust_table(idname,maxrow,colcount);
                        maxrow = colcount;
                    }

                      vstr = "";  dstr = "";
                    for (int ws_count1 = 0; ws_count1 < colcount; ws_count1++)
                    {
                        vstr = vstr + " column" + (ws_count1 + 1).ToString() + ", ";
                        dstr = dstr + utils.pads(ws_alpha[ws_count1]) + ", ";
                    }

                    str1 = "insert into [" + idname + "] (" + vstr + "snumber) values (" + dstr + "'') ";
                    db.Database.ExecuteSqlCommand(str1);

                }
            }

            str1 = " execute interface_rtn @p_userid=" + utils.pads(pblock.userid) + " , @delete_flag=" + utils.pads(delete_flag);
            db.Database.ExecuteSqlCommand(str1);

            utils.set_titles("", "SAP upload report ");
            return RedirectToAction("ErrorRep", "RepScrn");
        }

        private void adjust_table(string idname, int old_max, int new_max)
        {
            string str1 = "";
            
            for (int ws_count1 = old_max+1; ws_count1 <= new_max; ws_count1++)
                str1 = str1 + ",column" + ws_count1.ToString() + " varchar(max) not null default '' ";

            str1 = "alter table [" + idname + "] add " + str1.Substring(2);
            db.Database.ExecuteSqlCommand(str1);
        }


    }
}

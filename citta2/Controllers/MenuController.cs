using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using anchor1.Models;
using anchor1.Filters;
using anchor1.utilities;
using System.Data.SqlClient;


namespace anchor1.Controllers
{
    public class MenuController : Controller
    {
        private anchor1Context db = new anchor1Context();
        //Connectstr constr = new Connectstr();
       // static DataClasses1DataContext context = new DataClasses1DataContext();
//        DataContext context;
        mysessvals pblock;
        Cutil utils = new Cutil();
        List<SelectListItem> alrow = new List<SelectListItem>();
        int all_ctr = 1; string str1; string tr1;
        worksess worksess;

        //
        // GET: /Nation/

        [EncryptionActionAttribute]
        public ActionResult Index(string pc = null)
        {
            if (pc==null)
                return RedirectToAction("Welcome", "Home");

            pblock = (mysessvals)Session["mysessvals"];
            string menuname = (from fg in db.tab_soft where fg.para_code == "MAINSCRN" && fg.report_code == pc && fg.rep_name2 == "H" select fg.report_name1).FirstOrDefault();
            ViewBag.menu1 = menuname;

            str1 = "select ws_code = report_code, ws_string0 = report_name1, ws_string1 = report_name2, ws_string2 = report_name4, ws_string3 = report_name5, ws_string4 = report_name3 ";
            str1 += " from tab_soft where para_code='MAINSCRN' and report_name2 in (";
            str1 += " select distinct c8 from (" + item_string(pc + "%") + ") xz )";
            str1 += " order by report_name3 ";
            var blist1 = db.Database.SqlQuery<vw_collect>(str1);

            init_session_variables();

            return View(blist1.ToList());
        }

        public ActionResult self()
        {
            //SqlConnectionStringBuilder db1 = constr.company_database("99999");
            //context = new DataContext(db1.ConnectionString);

            pblock = (mysessvals)Session["mysessvals"];

            string pc = "SF";
            string menuname = (from fg in db.tab_soft where fg.para_code == "SELFMENU" && fg.report_code == pc && fg.rep_name2 == "H" select fg.report_name1).FirstOrDefault();
            ViewBag.menu1 = menuname;

            str1 = "select ws_code = report_code, ws_string0 = report_name1, ws_string1 = report_name2, ws_string2 = report_name4, ws_string3 = report_name5, ws_string4 = report_name3 ";
            str1 += " from tab_soft where para_code='SELFMENU' and cast(numeric_ind as int) <= " + pblock.ugroup_type + " and report_name2 in (";
            str1 += " select distinct c8 from (" + item_string(pc + "%") + ") xz )";
            str1 += " order by report_name3 ";
            var blist1 = db.Database.SqlQuery<vw_collect>(str1);

            init_session_variables();

            return View("index", blist1.ToList());
        }

        //
        // GET: /Nation/Create



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void init_session_variables()
        {
            worksess = (worksess)Session["worksess"];
            worksess.temp0 = "";
            worksess.temp1 = "";
            worksess.temp2 = "";
            worksess.temp3 = "";
            worksess.temp4 = "";
            worksess.temp5 = "";
            worksess.temp6 = "";
            worksess.vdate = "";
            worksess.viewflag = "";
            worksess.apentry = "";
            worksess.det = "";
            worksess.err_msg = "";
            worksess.filep = "";
            worksess.flag_type = "";
            worksess.idrep = "";
            worksess.intval0 = 0;
            worksess.jp = "";
            worksess.pc = "";
            worksess.ptitle = "";
            worksess.tarray = new String[50];
            Session["worksess"] = worksess;
            
            //Session["bye_mess"] = "";
            //Session.Remove("apvac");
            //Session.Remove("apcash");
            //Session.Remove("apentry");
            //Session.Remove("temp0");
            //Session.Remove("deltemp");

        }

        private List<SelectListItem> menuread(string opt2)
        {
//            Session["suprep"] = null;
            int item_row = pblock.item_row;

            alrow.Add(new SelectListItem { Text = "<table class='menutable' width='100%'  ><tbody>", Value = "0" });
            menu_write(opt2, item_row);

            string opt3 = opt2 + "S";
            str1 = "select '1' c1 from tab_soft where para_code in ('ITEM') and rep_name2 in (" + utils.pads(opt3) + ") ";
            var str3 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str3 != null)
            {
                tr1 = "<tr><td colspan=10><hr class='hr-line'></td><tr>";
                all_ctr++;
                alrow.Add(new SelectListItem { Text = tr1, Value = Convert.ToString(all_ctr) });
                menu_write(opt3, item_row);
            }


            alrow.Add(new SelectListItem { Text = "</tbody></table>", Value = "99" });
            return alrow;
        }

        private void menu_write(string opt2, int item_row = 0)
        {
            int row_ctr = 0;
            int wdt;
            try
            {
                wdt = 100 / item_row;
            }
            catch
            {
                wdt = 100;
            }


            string swdt = wdt.ToString() + "%";
            tr1 = "<tr>";
            str1 = "";

            str1 = item_string(opt2);
            str1 += " order by 10,5,2 ";
            var str2 = db.Database.SqlQuery<vw_query>(str1);
            foreach (var item in str2)
            {
                tr1 += "<td width='" + swdt + ")'><a onClick='lgshow()'  href='../itemrun/" + item.c1 + "[]" + item.c4 + "[]" + item.c9 + "'  >";
                tr1 += "<img align=middle src='../../images/";
                if (item.c6 == "Y")
                {
                    //if (item.c7 == "XXX")
                    //    tr1 += "postit.gif";
                    //else
                        tr1 += "report.png";
                }
                else
                    tr1 += item.c3;

                tr1 += "'  border=0></img><br>";
                tr1 += "<span class='hmenu'>" + item.c2 + "</span></a></td> ";

                row_ctr++;
                if (row_ctr == item_row)
                {
                    tr1 += "</tr><tr>";
                    alrow.Add(new SelectListItem { Text = tr1, Value = Convert.ToString(all_ctr) });
                    all_ctr += 1;
                    tr1 = "<tr>";
                    row_ctr = 0;
                }
            }

            if (opt2 == "SFCOY" || opt2 == "SFCOMP")
            {
                string key1 = "";
                string imgtype = "";
                string str1 = "select * from tab_docpara a left outer join tab_soft b on ( b.para_code = 'IMGDEF' and document_type like '%' + report_code) ";
                str1 += " where a.hcode='UPLOAD' and substring(pcode,1,1)<>'C' ";
                str1 += " order by 1 ";
                var bg2 = db.Database.SqlQuery<vw_picture>(str1);

                foreach (var item in bg2.ToList())
                {
                    if (item.report_name2 == null)
                    {
                        imgtype = "../../Images/handbook.gif";
                    }
                    else
                    {
                        imgtype = "../../Images/" + item.report_name2;
                    }

                    key1 = "PP:" + item.sequence_no.ToString() + "::";
                    key1 = "" + key1 + "::";

                    tr1 += "<td width='" + swdt + "'><a onClick='showdocument(\"" + key1 + "\")'  href='javascript:;'  >";
                    tr1 += "<img align=middle src=" + utils.pads(imgtype);
                    tr1 += " border=0></img><br />";
                    tr1 += "<span class='hmenu'>" + item.document_name + "</span></a></td> ";

                    row_ctr++;
                    if (row_ctr == item_row)
                    {
                        tr1 += "</tr><tr>";
                        alrow.Add(new SelectListItem { Text = tr1, Value = Convert.ToString(all_ctr) });
                        all_ctr += 1;
                        tr1 = "<tr>";
                        row_ctr = 0;
                    }

                }
            }

            tr1 += "</tr>";
            alrow.Add(new SelectListItem { Text = tr1, Value = Convert.ToString(all_ctr) });

        }

        private string item_string(string opt2)
        {
            str1="";

            str1 += "select report_code c1, report_name1 c2, report_name4 c3, 'ITEM' c4, report_name7 c5,'N' c6, ''c7, rep_name2 c8,'' c9,'0' c10 from tab_soft where para_code in ('ITEM','SELFT','SELFTQ') and rep_name2 like " + utils.pads(opt2) + " union all ";

            str1 += " select calc_code , a.name1, b.report_name2, a.para_code, '999',internal_use,report_type, a.menu_option,'',a.para_code from tab_calc a, tab_soft b where b.para_code='UPER' and b.report_code=a.para_code  ";
            str1 += "  and b.report_name4<>'Y' and a.menu_option like " + utils.pads(opt2) + " union all ";

            str1 += " select trans_type , name1, c.report_name2, c.report_code, '999','N','',a.selection_line,'SELE','tty' from tab_type a, tab_array b, tab_soft c where a.internal_use <> 'Y' and a.selection_line like " + utils.pads(opt2);
            str1 += " and a.trans_type = b.source1 and b.para_code in ('SELE') and c.para_code='UPER' and c.report_code='TYPE' and b.array_code=c.report_code union all ";
            str1 += " select trans_type , name1, c.report_name2, c.report_code, '999','N','',a.eemployee_menu,'SELF','tty' from tab_type a, tab_array b, tab_soft c where a.internal_use <> 'Y' and a.eemployee_menu like " + utils.pads(opt2);
            str1 += " and a.trans_type = b.source1 and b.para_code in ('SELFT') and c.para_code='UPER' and c.report_code='TYPE' and b.array_code=c.report_code union all ";

            str1 += " select code_definition , name1, c.report_name2, c.report_code , '999','N','',a.menu_option,'SELE','appr' from tab_app_definition a, tab_array b, tab_soft c where a.menu_option like " + utils.pads(opt2);
            str1 += " and a.code_definition = b.array_code and b.para_code in ('SELE') and c.para_code='UPER' and c.report_code='APPR' and b.source1=c.report_code union all ";
            str1 += " select code_definition , name1, c.report_name2, c.report_code, '999','N','',a.eemployee_option,'SELF','appr' from tab_app_definition a, tab_array b, tab_soft c where a.eemployee_option like " + utils.pads(opt2);
            str1 += " and a.code_definition = b.array_code and b.para_code in ('SELFT') and c.para_code='UPER' and c.report_code='APPR' and b.source1=c.report_code union all ";

            str1 += " select calc_code , name1, c.report_name2, c.report_code, '999','N','MM',a.menu_option,'SELE',a.para_code from tab_calc a, tab_array b, tab_soft c where a.para_code=b.array_code and a.menu_option like " + utils.pads(opt2);
            str1 += " and a.calc_code = b.source1 and b.para_code in ('SELE') and c.para_code='UPER' and b.array_code=c.report_code and c.report_name4='Y' union all ";
            str1 += " select calc_code , name1, c.report_name2, c.report_code, '999','N','MM',a.report_name,'SELF',a.para_code from tab_calc a, tab_array b, tab_soft c where a.para_code=b.array_code and a.report_name like " + utils.pads(opt2);
            str1 += " and a.calc_code = b.source1 and b.para_code in ('SELFT') and c.para_code='UPER' and b.array_code=c.report_code and c.report_name4='Y' union all ";

            str1 += " select a.operand,isnull(c.operand,report_name1) , isnull(report_name4,'opscreen.gif'), isnull(c.source1,'SFDEL'), cast(a.count_array as varchar) ,'N', ''c7, 'SFDELREQ' c8,'','0' from tab_array a ";
            str1 += " left outer join tab_soft b on (b.para_code='SELFT' and a.operand=report_code) ";
            str1 += " left outer join tab_array c on (c.para_code='SELF' and a.operand=c.array_code) ";
            str1 += " where a.para_code='H45' and a.source1='T'and a.period=" + utils.pads(pblock.userid) + " and 'SFDELREQ' like " + utils.pads(opt2);


            if (pblock.type2 == "U" & opt2.Substring(0, 2) != "SF")
            {
                str1 = " select c1,c2,c3,c4,c5,c6,c7,c8,c9,c10 from  (" + str1 ;
                str1 += ") xd where xd.c1+xd.c4 " + pblock.cquery2;
                str1 += " in (select operand+source1 from tab_array where para_code='21' and array_code=" + utils.pads(pblock.ugroup) +")";
            }

            return str1;
        }

        [HttpPost]
        public ActionResult DailyList(string IDX)
        {
            string Code = IDX;
            pblock = (mysessvals)Session["mysessvals"];
            List<SelectListItem> alrow = new List<SelectListItem>();

            alrow = menuread(IDX);


            if (HttpContext.Request.IsAjaxRequest())
                return Json(alrow
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index",null,new{anc=Ccheckg.convert_pass2("pc=")});

        }


        public ActionResult itemrun(string id)
        {
            // check he has accesss
            string str1;
            int pos1 = id.IndexOf("[]");
            string code1 = id.Substring(0, pos1);
            int pos2 = id.IndexOf("[]", pos1 + 2);
            string type1 = id.Substring(pos1 + 2,pos2-pos1-2);
            string optiony = id.Substring(pos2 + 2);

            pblock = (mysessvals)Session["mysessvals"];

            // check for self services
            if (type1 == "ITEM")
                str1 = "select report_name8 c1, report_name9 c2, report_name10 c3 from tab_soft where para_code in ('ITEM','SELFT','SELFTQ') and report_code=" + utils.pads(code1);
            else if (type1 == "SFDEL")
                str1 = "select report_name8 c1, report_name9 c2, report_name10 c3 from tab_soft where para_code in ('UPER2') and report_code in (" + utils.pads(code1) + "," + utils.pads(type1) + ") order by numeric_ind ";
            else if (optiony=="SELF")
                str1 = "select report_name8 c1, report_name9 c2, report_name10 c3 from tab_soft where para_code in ('UPER3') and report_code=" + utils.pads(type1);
            else 
                str1 = "select report_name8 c1, report_name9 c2, report_name10 c3 from tab_soft where para_code in ('UPER') and report_code=" + utils.pads(type1);

            var str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();

            if (str2 == null)
                return RedirectToAction("Welcome", "home");

            if (string.IsNullOrWhiteSpace(str2.c1))
                return RedirectToAction("Welcome", "home");

            if (!string.IsNullOrWhiteSpace(str2.c3))
            {
                string ancs = Ccheckg.convert_pass2("pc=" + str2.c3);
                return RedirectToAction(str2.c2.Trim(), str2.c1.Trim(), new { anc = ancs });
                //                return RedirectToAction(str2.c2.Trim(), str2.c1.Trim(), new { pc = str2.c3 });
            }
            else
                if (type1 == "ITEM")
                {
                    string ancs = Ccheckg.convert_pass2("pc=");
                    return RedirectToAction(str2.c2.Trim(), str2.c1.Trim(), new { anc = ancs });
                }
                else
                {
                    string ancs = Ccheckg.convert_pass2("pc=" + code1);
                    return RedirectToAction(str2.c2.Trim(), str2.c1.Trim(), new { anc = ancs });
                    //                return RedirectToAction(str2.c2.Trim(), str2.c1.Trim(), new { pc = code1 });
                }
        }


    }
}
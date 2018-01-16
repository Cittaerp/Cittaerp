using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using CittaErp.Models;
using anchor1.Models;
using System.Data.SqlClient;
using System.IO;

namespace CittaErp.ReportA
{
    /// <summary>
    /// Summary description for SectionReport1.
    /// </summary>
    public partial class Sreport3 : GrapeCity.ActiveReports.SectionReport
    {

        GrapeCity.ActiveReports.SectionReportModel.Label label_obj;
        GrapeCity.ActiveReports.SectionReportModel.TextBox text_tobj;
        GrapeCity.ActiveReports.SectionReportModel.TextBox text_fobj;
        //GrapeCity.ActiveReports.SectionReportModel.Picture tpict;
        GrapeCity.ActiveReports.SectionReportModel.Shape tshape;

        string pfont = "tahoma";
        int psize = 10;
        float x1 = 0;
        float fwidth = 0;
        vw_collect rpt_value = new vw_collect();
        SqlConnection con;
        SqlDataReader reader;
        psess psess;
        MainContext db ;
        pubsess pblock;
        string query;
        float const_y = 1.062F;
        float const_height = 0.2F;
        float const_x = 0.125F;
        string head_color; string total_color;
        string negative_type = "S"; string audit_type = "C"; decimal p_charsize = 12; float p_spacing = 0.125F;
        float leve11_size = 0; float leve12_size = 0; float leve13_size = 0; float leve14_size = 0; float leve15_size = 0;
        float ctr_lbl; float ctr_field;
        float lineh = 0.2f;
        string sv_backup = "";
        string[] ap_header = new string[20];
        string[,] ap_header1 = new string[20, 20]; string[,] ap_header2 = new string[20, 20];
        int apr_count = 0; int counter1 = 0; int max_apr_count=0;
        float y2 = 0;

        public Sreport3()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }


        private void Sreport3_ReportStart(object sender, EventArgs e)
        {

            db = new MainContext();
            pblock = (pubsess)HttpContext.Current.Session["pubsess"];

            string conString = db.Database.Connection.ConnectionString;

            con = new SqlConnection(conString);
            //string str4 = "select cvalue c1 from pub_table where name1='orderby' and userid='" + pblock.userid + "'";
            //var str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
            //string orderbyt = "";
            //if (str5 != null)
            //    orderbyt = str5.c1;

            //string query = HttpContext.Current.psess.sarrayt0[6].ToString();
            string query = "select * from [" + pblock.userid + "rdoc] order by snumber,cast(column24 as int) ,cast(colseq as int) ";
            //query += " order by " + orderbyt + " snumber";
            //query += ", cast(column2 as int)";

            SqlCommand ad = new SqlCommand(query, con);
            con.Open();
            reader = ad.ExecuteReader();
            this.DataSource = reader;

            main01();

        }

        private void Sreport3_ReportEnd(object sender, EventArgs e)
        {
            string view_name = HttpContext.Current.Server.MapPath("~/print/zx" + pblock.userid + ".rdf");

            this.Document.Save(view_name);

            reader.Close();
            con.Close();

        }
        
        private void main01()
        {
            decimal page_top=0; decimal page_bottom=0; decimal page_right=0; decimal page_left=0;
            decimal page_width = 0; decimal page_height = 0; 
            
            string str4 = "select field1 c1,field2 c2,field9 c3, field10 c4,field5 c5, field6 c6,field11 c7 from tab_train_default where default_code='REPORT'";
            var str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();

            if (str5 != null)
            {
                negative_type = str5.c1;
                audit_type = str5.c2;
                p_charsize = Convert.ToDecimal(str5.c3);
                p_spacing = CmToInch((float)Convert.ToDouble(str5.c4));
                head_color = str5.c5;
                total_color = str5.c6;
                lineh = CmToInch((float)Convert.ToDouble(str5.c7));
            }

            // need values from layout
            // total 1-5
            // rcount 1-5
            // payroll period
            // report title
            // page 1-5

            if (psess.sarrayt1 == null)
                rpt_value.ar_string0 = new string[] { "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "", "", "N", "N", "N", "N", "N","1" };
            //else
            //    rpt_value = (vw_collect)HttpContext.Current.psess.sarrayt1;

            string lineg = rpt_value.ar_string0[17];
            if (Convert.ToInt16(lineg) > 1)
                lineh = lineh * Convert.ToInt16(lineg);

            this.detail.Height = lineh;

            string pr_codet = pblock.printer_code;

// read printer to get font and size
            if (rpt_value.ar_string0[11] != "")
            {
                str4 = "select report_name c0 from tab_calc where calc_code='" + rpt_value.ar_string0[11] + "' and para_code='" + HttpContext.Current.Session["adv1"].ToString() + "' ";
                str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
                if (str5 != null)
                    pr_codet = str5.c0;
            }

            str4 = "select suppress_zero c0, line_spacing n1,report_name c1, transfer_code c2,wide_column c3,report_type c4 from tab_calc where para_code='19' and calc_code='" + pr_codet + "'";
            str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();

            pfont = "tahoma";
            psize = 10;
            string paper_type = "A4";
            string paper_orientation = "P";
            if (str5 != null)
            {
                pfont = str5.c0;
                psize = Convert.ToInt16(str5.n1);
                paper_type = str5.c1;
                paper_orientation = str5.c2;
                page_width = Convert.ToDecimal(str5.c3);
                page_height = Convert.ToDecimal(str5.c4);
            }

            str4 = "select operand c0, source1 c1, amount n1, percent1 n2 from tab_array where para_code='19' and array_code='" + pr_codet + "'";
            str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
            if (str5 != null)
            {
                page_top = Convert.ToDecimal(str5.c0);
                page_bottom = Convert.ToDecimal(str5.c1);
                page_right = Convert.ToDecimal(str5.n1);
                page_left = Convert.ToDecimal(str5.n2);
            }

            //str4 = "select cast(max(len(level1_name)) as varchar) c0,cast(max(len(level2_name)) as varchar) c1,cast(max(len(level3_name)) as varchar) c2, ";
            //str4+= " cast(max(len(level4_name)) as varchar) c3, cast(max(len(level5_name)) as varchar) c4 from [" + pblock.userid + "st01] ";
            //str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
            //leve11_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c0) ) / 1440);
            //leve12_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c1) ) / 1440);
            //leve13_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c2) ) / 1440);
            //leve14_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c3) ) / 1440);
            //leve15_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c4) ) / 1440); 


            // logo picture
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Uploads/company.png");
            if (!System.IO.File.Exists(filePath))
                filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Images/nologo.png");

            this.picture1.Image = Image.FromFile(filePath);
            //dataconnect dat1 = (dataconnect)HttpContext.Current.Session["dat1"];
            this.company_name.Text = "abc company";//dat1.company_name;
            this.company_name.Font = new System.Drawing.Font(pfont, psize+2, FontStyle.Bold);
            this.report_name.Text = psess.temp1.ToString();
            this.report_name.Font = new System.Drawing.Font(pfont, psize+2, FontStyle.Bold);
            this.userid.Text = "Staff Number : " + pblock.userid;
            this.userid.Font = new System.Drawing.Font(pfont, psize+2, FontStyle.Bold);
            this.pperiod.Visible = false;
            if (rpt_value.ar_string0[10] != "")
            {
                this.pperiod.Text = "Payroll Period : " + rpt_value.ar_string0[10];
                this.pperiod.Visible = true;
                this.pperiod.Font = new System.Drawing.Font(pfont, psize+2, FontStyle.Bold);
            }

            float coyx = 8f - this.company_name.Width - 0.2f;
            this.company_name.Location = new PointF(coyx, this.company_name.Location.Y);
            this.userid.Location = new PointF(coyx, this.userid.Location.Y);
            this.pperiod.Location = new PointF(coyx, this.pperiod.Location.Y);


//label for sno
// start position x = 0.125, y = 1.062, height =0.2
            ctr_field = (float)((p_charsize * psize * 6) / 1440);
            ctr_lbl = (float)((p_charsize * psize * 17) / 1440);

            fwidth = (float)((p_charsize * psize * 6) / 1440);
            int widthint = 0;

            this.det1.Width = fwidth;
            this.det1.Location = new PointF(const_x, 0f);
            this.det1.DataField = "colseq";
            this.det1.Font = new System.Drawing.Font(pfont, psize);
            this.det1.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
            this.det1.Size = new SizeF(fwidth, const_height);
            
            int col_ctr=0;
            x1 = const_x;
            apr_count=0;counter1=0;
            float maxsize = 0;
            apr_count = -1;

            query = "select column1 c0, column2 c1, column3 c2, column11 c3,column5 c4,column4 c5,snumber c6 from [" + pblock.userid + "ophead] order by cast(column13 as int),snumber,cast(column1 as int) ";
            var str12 = db.Database.SqlQuery<vw_query>(query);
            foreach (var item in str12)
            {

                if (sv_backup != item.c6)
                {
                    if (x1 > maxsize) maxsize = x1;
                    apr_count++;
                    sv_backup = item.c6;
                    ap_header[apr_count] = item.c6;
                    x1 = const_x;
                    col_ctr = 0;
                    fwidth = (float)((p_charsize * psize * 6) / 1440);
                }

                col_ctr++;
                x1 += fwidth + p_spacing;

                widthint = Convert.ToInt16(item.c3);
                fwidth = (float)((p_charsize * psize * widthint) / 1440);

                label_obj = new GrapeCity.ActiveReports.SectionReportModel.Label();
                label_obj.Location = new PointF(x1, 0);
                label_obj.Text = item.c5;
                label_obj.Size = new SizeF(fwidth, const_height);
                label_obj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                label_obj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
                label_obj.Name = item.c6 + "hcol" + item.c0;
                if (item.c4 == "M" || item.c4 == "A" || item.c4 == "NT")
                    label_obj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;

                ap_header1[apr_count, Convert.ToInt16(item.c0)] = label_obj.Name;
                label_obj.Visible = false;
                this.Sections["groupHeader1"].Controls.Add(label_obj);


// detail line
                text_tobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
                text_tobj.Location = new PointF(x1, 0);
                text_tobj.Text = "column" + col_ctr.ToString();
                text_tobj.Size = new SizeF(fwidth, const_height);
                text_tobj.Font = new System.Drawing.Font(pfont, psize);
                text_tobj.DataField = text_tobj.Text;
                text_tobj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
                text_tobj.Name = item.c6 + "dcol" + item.c0;
                ap_header2[apr_count, Convert.ToInt16(item.c0)] = text_tobj.Name;
                text_tobj.Visible = false;
//numeric figures
                if (item.c4 == "N" || item.c4 == "NT")
                {
                    text_tobj.OutputFormat = "#,##0";
                    text_tobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left;
                }

                if (item.c4 == "R")
                {
                    text_tobj.OutputFormat = "#,##0.0000";
                    text_tobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left;
                }

                if (item.c4 == "M" || item.c4 == "A")
                {
                    text_tobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;
                    text_tobj.OutputFormat = "#,##0.00 ";

                    if (negative_type == "B")
                        text_tobj.OutputFormat = "#,##0.00 ;(#,##0.00)";
                }

                this.Sections["detail"].Controls.Add(text_tobj);


            }


            x1 = maxsize+fwidth + p_spacing;
            //this.headbox.Width = x1;
            //this.headbox.Visible = false;
            max_apr_count=apr_count;
            apr_count = 0;
            float x2 = x1;

// staff name
            query = "select * from [" + pblock.userid + "rtotal1]";
            var str13 = db.Database.SqlQuery<rep_column>(query);
            foreach (var item in str13)
            {
                fwidth = (float)((p_charsize * psize * 6) / 1440);

                    x1 = 0;
                col_ctr++;
                y2 = 0;

                set_name_label("8");
                label_obj.Text = "Name :";
                set_name_textbox(item.column1);
                text_fobj.DataField = "column11";

                set_name_label("21");
                label_obj.Text = "Performance Period :";
                set_name_textbox(item.column2);
                text_fobj.DataField = "column12";

                set_name_label("21");
                label_obj.Text = "Grade During Perf. :";
                set_name_textbox(item.column3);
                text_fobj.DataField = "column13";

                set_name_label("12");
                label_obj.Text = "Location :";
                set_name_textbox(item.column4);
                text_fobj.DataField = "column14";

                set_name_label("12");
                label_obj.Text = "Job Post :";
                set_name_textbox(item.column5);
                text_fobj.DataField = "column15";

                y2 = const_height;
                fwidth = (float)((p_charsize * psize * 6) / 1440);
                x1 = 0;
                set_name_label("17");
                label_obj.Text = "Current Grade :";
                set_name_textbox(item.column6);
                text_fobj.DataField = "column16";

                set_name_label("22");
                label_obj.Text = "Duration in Months :";
                set_name_textbox(item.column7);
                text_fobj.DataField = "column17";

                set_name_label(item.column8);
                label_obj.DataField = "column18";
                set_name_textbox(item.column9);
                text_fobj.DataField = "column19";

                set_name_label(item.column10);
                label_obj.DataField = "column20";
                set_name_textbox(item.column11);
                text_fobj.DataField = "column21";

                set_name_label(item.column12);
                label_obj.DataField = "column25";
                set_name_textbox(item.column13);
                text_fobj.DataField = "column26";

            }

            if (x2 > x1) x1 = x2;
            if (x1 > 8.27f)
            {
                this.PrintWidth = x1 + p_spacing;
                this.page_line.X2 = x1;
            }
            else
            {
                this.PrintWidth = 8.27f;
                this.page_line.X2 = 8.27f;
            }

            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait;
            if (paper_orientation == "L")
                this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;

            this.PageSettings.Margins.Bottom = (float)page_bottom;
            this.PageSettings.Margins.Top = (float)page_top;
            this.PageSettings.Margins.Right = (float)page_right;
            this.PageSettings.Margins.Left = (float)page_left;
            this.PageSettings.PaperWidth = (float)page_width;
            this.PageSettings.PaperHeight = (float)page_height;

            //header_checker();

        }



        private void detail_Format(object sender, EventArgs e)
        {

        }

        private void groupHeader1_Format(object sender, EventArgs e)
        {
            string ws_name = "";
            int apr_count_back = apr_count;
                //' set header, get the appr count
                sv_backup = Fields["column22"].Value.ToString();

                for (int ctr = 0; ctr <= max_apr_count; ctr++)
                {
                    if (ap_header[ctr] == sv_backup)
                        apr_count= ctr;
                }

                for (int ctr = 0; ctr < 10; ctr++)
                {
                    ws_name = ap_header1[apr_count, ctr];
                    if (!string.IsNullOrWhiteSpace(ws_name))
                    {
                        this.Sections["groupHeader1"].Controls[ws_name].Visible = true;
                        ws_name = ap_header2[apr_count, ctr];
                        this.Sections["detail"].Controls[ws_name].Visible = true;
                    }

                    if (apr_count != apr_count_back)
                    {
                        ws_name = ap_header1[apr_count_back, ctr];
                        if (!string.IsNullOrWhiteSpace(ws_name))
                        {
                            this.Sections["groupHeader1"].Controls[ws_name].Visible = false;
                            ws_name = ap_header2[apr_count_back, ctr];
                            this.Sections["detail"].Controls[ws_name].Visible = false;
                        }
                    }
                }

        }


        private void set_name_label (string sizet)
        {
            x1 += fwidth + p_spacing;
            int widthint = Convert.ToInt16(sizet);
            fwidth = (float)((p_charsize * psize * widthint) / 1440);

            label_obj = new GrapeCity.ActiveReports.SectionReportModel.Label();
            label_obj.Location = new PointF(x1, y2);
            label_obj.Size = new SizeF(fwidth, const_height);
            label_obj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Regular);
            label_obj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
            this.Sections["groupHeader2"].Controls.Add(label_obj);
        }

        private void set_name_textbox(string sizet)
        {
            x1 += fwidth + p_spacing;
            int widthint = Convert.ToInt16(sizet);
            fwidth = (float)((p_charsize * psize * widthint) / 1440);

            text_fobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            text_fobj.Location = new PointF(x1, y2);
            text_fobj.Size = new SizeF(fwidth, const_height);
            text_fobj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
            text_fobj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
            this.Sections["groupHeader2"].Controls.Add(text_fobj);
        }

        private void groupHeader2_Format(object sender, EventArgs e)
        {

        }

        private void pageHeader_Format(object sender, EventArgs e)
        {

        }


    }
}

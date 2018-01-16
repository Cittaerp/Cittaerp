﻿using System;
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
    public partial class Sreport4 : GrapeCity.ActiveReports.SectionReport
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
        MainContext db;
        pubsess pblock;
        string query;
        float const_y = 1.062F;
        float const_height = 0.2F;
        float const_x = 0.125F;
        psess psess;
        string head_color; string total_color;
        string negative_type = "S"; string audit_type = "C"; decimal p_charsize = 12; float p_spacing = 0.125F;
        float leve11_size = 0; float leve12_size = 0; float leve13_size = 0; float leve14_size = 0; float leve15_size = 0;
        float leve1_code_size = 0; int code_max; float head_start = 0;
        float ctr_lbl; float ctr_field;
        float lineh = 0.2f;
        float y2 = 0;
        string divname = "";
        string srep_type = "";
        float x3 = 0;

        public Sreport4()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }


        private void Sreport4_ReportStart(object sender, EventArgs e)
        {

            db = new MainContext();
            pblock = (pubsess)HttpContext.Current.Session["pubsess"];

            string conString = db.Database.Connection.ConnectionString;

            con = new SqlConnection(conString);
            string str4 = "select cvalue c1 from pub_table where name1='orderby' and userid='" + pblock.userid + "'";
            var str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
            string orderbyt = "";
            if (str5 != null)
                orderbyt = str5.c1;

            str4 = "select cvalue c1 from pub_table where name1='reporttype' and userid='" + pblock.userid + "'";
            str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
            if (str5 != null)
                srep_type = str5.c1;


            string query = "select  * from [" + pblock.userid + "st01] order by " + orderbyt + " key_column,record_counter";
            //if (rpt_value.ar_string0[11] != "")
            //    query += ", cast(column2 as int)";
            SqlCommand ad = new SqlCommand(query, con);
            con.Open();
            reader = ad.ExecuteReader();
            this.DataSource = reader;

            main01();
        }

        private void Sreport4_ReportEnd(object sender, EventArgs e)
        {
            string view_name = HttpContext.Current.Server.MapPath("~/print/zx" + pblock.userid + ".rdf");

            this.Document.Save(view_name);

            reader.Close();
            con.Close();

        }

        private void main01()
        {
            decimal page_top = 0; decimal page_bottom = 0; decimal page_right = 0; decimal page_left = 0;
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
                rpt_value.ar_string0 = new string[] { "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "", "", "N", "N", "N", "N", "N", "1" };
            //else
            //    rpt_value = (vw_collect)HttpContext.psess.sarrayt1;

            if (srep_type == "acty")
            {
                for (int wctr = 4; wctr < 0; wctr--)
                    rpt_value.ar_string0[wctr] = rpt_value.ar_string0[wctr - 1];
                rpt_value.ar_string0[0] = "Y";
            }

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

            str4 = "select cast(max(len(level1_name)) as varchar) c0,cast(max(len(level2_name)) as varchar) c1,cast(max(len(level3_name)) as varchar) c2, ";
            str4 += " cast(max(len(level4_name)) as varchar) c3, cast(max(len(level5_name)) as varchar) c4, ";
            str4 += " cast(max(len(level1_code)) as varchar) c5, cast(max(len(level2_code)) as varchar) c6, cast(max(len(level3_code)) as varchar) c7, ";
            str4 += " cast(max(len(level4_code)) as varchar) c8, cast(max(len(level5_code)) as varchar) c9 ";
            str4 += " from [" + pblock.userid + "st01] ";
            str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
            leve11_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c0)) / 1440);
            leve12_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c1)) / 1440);
            leve13_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c2)) / 1440);
            leve14_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c3)) / 1440);
            leve15_size = (float)((p_charsize * psize * Convert.ToDecimal(str5.c4)) / 1440);

            int[] arrint = { Convert.ToInt16(str5.c5), Convert.ToInt16(str5.c6), Convert.ToInt16(str5.c7), Convert.ToInt16(str5.c8), Convert.ToInt16(str5.c9) };
            code_max = arrint.Max() + 4;
            leve1_code_size = (float)((p_charsize * psize * code_max) / 1440);
            head_start = leve1_code_size + 0.5f;

            // logo picture
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Uploads/company.png");
            if (!System.IO.File.Exists(filePath))
                filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Images/nologo.png");

            this.picture1.Image = Image.FromFile(filePath);
            //dataconnect dat1 = (dataconnect)HttpContext.Current.Session["dat1"];
            this.company_name.Text = "abc company";//dat1.company_name;
            this.company_name.Font = new System.Drawing.Font(pfont, psize + 2, FontStyle.Bold);
            this.report_name.Text = psess.temp1.ToString();
            this.report_name.Font = new System.Drawing.Font(pfont, psize + 2, FontStyle.Bold);
            this.userid.Text = "Staff Number : " + pblock.userid;
            this.userid.Font = new System.Drawing.Font(pfont, psize + 2, FontStyle.Bold);
            this.pperiod.Visible = false;
            if (rpt_value.ar_string0[10] != "")
            {
                this.pperiod.Text = "Payroll Period : " + rpt_value.ar_string0[10];
                this.pperiod.Visible = true;
                this.pperiod.Font = new System.Drawing.Font(pfont, psize + 2, FontStyle.Bold);
            }

            float coyx = 8f - this.company_name.Width - 0.2f;
            this.company_name.Location = new PointF(coyx, this.company_name.Location.Y);
            this.userid.Location = new PointF(coyx, this.userid.Location.Y);
            this.pperiod.Location = new PointF(coyx, this.pperiod.Location.Y);

            if (head_color.Trim() != "")
                headbox.BackColor = Color.FromName(head_color);

            //label for sno
            // start position x = 0.125, y = 1.062, height =0.2
            ctr_field = (float)((p_charsize * psize * 6) / 1440);
            ctr_lbl = (float)((p_charsize * psize * 17) / 1440);

            fwidth = (float)((p_charsize * psize * 6) / 1440);
            int widthint = 0;
            this.seqno1.Text = "S/No";
            this.seqno1.Size = new SizeF(fwidth, const_height);
            this.seqno1.Location = new PointF(const_x, const_y + const_height + const_height);
            this.seqno1.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
            this.seqno1.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;

            this.det1.Width = fwidth;
            this.det1.Location = new PointF(const_x, 0f);
            this.det1.DataField = "record_counter";
            this.det1.Font = new System.Drawing.Font(pfont, psize);
            this.det1.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
            this.det1.Size = new SizeF(fwidth, const_height);

            int col_ctr = 0;
            x1 = const_x;

            query = "select column1 c0, column2 c1, column3 c2, column11 c3,column5 c4,column4 c5 from [" + pblock.userid + "ophead] order by cast(column1 as int)";
            var str12 = db.Database.SqlQuery<vw_query>(query);
            foreach (var item in str12)
            {
                col_ctr++;
                x1 += fwidth + p_spacing;

                widthint = Convert.ToInt16(item.c3);
                fwidth = (float)((p_charsize * psize * widthint) / 1440);

                label_obj = new GrapeCity.ActiveReports.SectionReportModel.Label();
                label_obj.Location = new PointF(x1, const_y);
                label_obj.Text = item.c1;
                label_obj.Size = new SizeF(fwidth, const_height);
                label_obj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                label_obj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
                if (item.c4 == "M" || item.c4 == "A" || item.c4 == "NT")
                    label_obj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;
                this.Sections["pageHeader"].Controls.Add(label_obj);

                label_obj = new GrapeCity.ActiveReports.SectionReportModel.Label();
                label_obj.Location = new PointF(x1, const_y + const_height);
                label_obj.Text = item.c2;
                label_obj.Size = new SizeF(fwidth, const_height);
                label_obj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                label_obj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
                if (item.c4 == "M" || item.c4 == "A" || item.c4 == "NT")
                    label_obj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;
                this.Sections["pageHeader"].Controls.Add(label_obj);

                label_obj = new GrapeCity.ActiveReports.SectionReportModel.Label();
                label_obj.Location = new PointF(x1, const_y + const_height + const_height);
                label_obj.Text = item.c5;
                label_obj.Size = new SizeF(fwidth, const_height);
                label_obj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                label_obj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
                if (item.c4 == "M" || item.c4 == "A" || item.c4 == "NT")
                    label_obj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;
                this.Sections["pageHeader"].Controls.Add(label_obj);

                // detail line
                text_tobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
                text_tobj.Location = new PointF(x1, 0);
                text_tobj.Text = "column" + col_ctr.ToString();
                text_tobj.Size = new SizeF(fwidth, const_height);
                text_tobj.Font = new System.Drawing.Font(pfont, psize);
                text_tobj.DataField = text_tobj.Text;
                text_tobj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
                text_tobj.Name = text_tobj.Text;
                //numeric figures
                if (item.c4 == "N")
                {
                    text_tobj.OutputFormat = "#,##0";
                    text_tobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left;
                }

                if (item.c4 == "R")
                {
                    text_tobj.OutputFormat = "#,##0.0000";
                    text_tobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left;
                }

                if (item.c4 == "M" || item.c4 == "A" || item.c4 == "NT")
                {
                    text_tobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;
                    text_tobj.OutputFormat = "#,##0.00 ";

                    if (negative_type == "B")
                        text_tobj.OutputFormat = "#,##0.00 ;(#,##0.00)";
                }

                this.Sections["detail"].Controls.Add(text_tobj);


                if (item.c4 == "M" || item.c4 == "A" || item.c4 == "NT")
                {
                    if (rpt_value.ar_string0[0] == "Y")
                    {
                        text_fobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
                        default_footer();
                        if (item.c4 == "NT") text_fobj.OutputFormat = "#,##0 ";
                        text_fobj.DataField = text_tobj.DataField;
                        text_fobj.SummaryGroup = "groupHeader1";
                        this.Sections["groupFooter1"].Controls.Add(text_fobj);
                    }

                    if (rpt_value.ar_string0[1] == "Y")
                    {
                        text_fobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
                        default_footer();
                        if (item.c4 == "NT") text_fobj.OutputFormat = "#,##0 ";
                        text_fobj.DataField = text_tobj.DataField;
                        text_fobj.SummaryGroup = "groupHeader2";
                        this.Sections["groupFooter2"].Controls.Add(text_fobj);
                    }

                    if (rpt_value.ar_string0[2] == "Y")
                    {
                        text_fobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
                        default_footer();
                        if (item.c4 == "NT") text_fobj.OutputFormat = "#,##0 ";
                        text_fobj.DataField = text_tobj.DataField;
                        text_fobj.SummaryGroup = "groupHeader3";
                        this.Sections["groupFooter3"].Controls.Add(text_fobj);

                    }

                    if (rpt_value.ar_string0[3] == "Y")
                    {
                        text_fobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
                        default_footer();
                        if (item.c4 == "NT") text_fobj.OutputFormat = "#,##0 ";
                        text_fobj.DataField = text_tobj.DataField;
                        text_fobj.SummaryGroup = "groupHeader4";
                        this.Sections["groupFooter4"].Controls.Add(text_fobj);
                    }

                    if (rpt_value.ar_string0[4] == "Y")
                    {
                        text_fobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
                        default_footer();
                        if (item.c4 == "NT") text_fobj.OutputFormat = "#,##0 ";
                        text_fobj.DataField = text_tobj.DataField;
                        text_fobj.SummaryGroup = "groupHeader5";
                        this.Sections["groupFooter5"].Controls.Add(text_fobj);
                    }

                    if (audit_type != "N")
                    {
                        text_fobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
                        default_footer();
                        if (item.c4 == "NT") text_fobj.OutputFormat = "#,##0 ";
                        text_fobj.DataField = text_tobj.DataField;
                        text_fobj.SummaryGroup = "";
                        text_fobj.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All;
                        text_fobj.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
                        text_fobj.Location = new PointF(x1, 0.312F);
                        this.Sections["reportFooter1"].Controls.Add(text_fobj);
                    }

                }

            }

            x1 += fwidth + p_spacing;
            this.headbox.Width = x1;

            float x2 = x1;
            activity_header();
            if (x2 > x1)
                x1 = x2;

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

// read header record for header6 - no - name  KRA :- business 2016 Date :   Target   Score

            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait;
            if (paper_orientation == "L")
                this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;

            this.PageSettings.Margins.Bottom = (float)page_bottom;
            this.PageSettings.Margins.Top = (float)page_top;
            this.PageSettings.Margins.Right = (float)page_right;
            this.PageSettings.Margins.Left = (float)page_left;
            this.PageSettings.PaperWidth = (float)page_width;
            this.PageSettings.PaperHeight = (float)page_height;

            header_checker();

        }


        private void default_footer()
        {
            float defx = x1 + 0.07F;
            text_fobj.Location = new PointF(defx, 0.07F);
            text_fobj.Size = new SizeF(fwidth, const_height);
            text_fobj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold | FontStyle.Underline);
            text_fobj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
            text_fobj.OutputFormat = "#,##0.00 ";
            if (negative_type == "B")
                text_fobj.OutputFormat = "#,##0.00 ;(#,##0.00)";
            text_fobj.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Sum;
            text_fobj.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.Group;
            text_fobj.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.SubTotal;
            text_fobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;
        }

        private void header_checker()
        {
            float recpos = const_height + 0.07f + 0.0667f;
            float curpos = this.lcount1.Location.X;
            if (rpt_value.ar_string0[0] == "Y" || rpt_value.ar_string0[5] == "Y" || rpt_value.ar_string0[12] == "Y")
            {
                recpos = 0.07f;
                this.groupHeader1.Visible = true;
                this.lcode1.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname1.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname1.Size = new SizeF(leve11_size, const_height);
                this.lcode1.Size = new SizeF(leve1_code_size, const_height);
                this.lname1.Location = new PointF(head_start, 0);

                this.groupHeader1.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
                if (rpt_value.ar_string0[12] == "Y")
                    this.groupHeader1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;

                if (rpt_value.ar_string0[0] == "Y" || rpt_value.ar_string0[5] == "Y")
                {
                    this.groupFooter1.Visible = true;

                    if (rpt_value.ar_string0[0] == "Y")
                        recpos += const_height + 0.0667f;

                    if (rpt_value.ar_string0[5] == "Y")
                    {
                        this.lcount1.Visible = true;
                        this.fcount1.Visible = true;
                        this.lcount1.Location = new PointF(4f, recpos);
                        this.lcount1.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.fcount1.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.lcount1.Size = new SizeF(ctr_lbl, const_height);
                        this.fcount1.Size = new SizeF(ctr_field, const_height);
                        curpos = 4f + lcount1.Width;
                        this.fcount1.Location = new PointF(curpos, recpos);

                        recpos += const_height + 0.0667f;
                    }

                    if (rpt_value.ar_string0[0] == "Y")
                    {
                        footer_color();
                        tshape.Height = recpos - 0.036F;
                        this.Sections["groupFooter1"].Controls.Add(tshape);
                        tshape.SendToBack();
                    }
                }
            }

            if (rpt_value.ar_string0[1] == "Y" || rpt_value.ar_string0[6] == "Y" || rpt_value.ar_string0[13] == "Y")
            {
                recpos = 0.07f;
                this.groupHeader2.Visible = true;
                this.lcode2.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname2.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname2.Size = new SizeF(leve12_size, const_height);
                this.lcode2.Size = new SizeF(leve1_code_size, const_height);
                this.lname2.Location = new PointF(head_start, 0);

                this.groupHeader2.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
                if (rpt_value.ar_string0[13] == "Y")
                    this.groupHeader2.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;

                if (rpt_value.ar_string0[1] == "Y" || rpt_value.ar_string0[6] == "Y")
                {
                    this.groupFooter2.Visible = true;

                    if (rpt_value.ar_string0[1] == "Y")
                        recpos += const_height + 0.0667f;

                    if (rpt_value.ar_string0[6] == "Y")
                    {
                        this.lcount2.Visible = true;
                        this.fcount2.Visible = true;
                        this.lcount2.Location = new PointF(4f, recpos);
                        this.lcount2.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.fcount2.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.lcount2.Size = new SizeF(ctr_lbl, const_height);
                        this.fcount2.Size = new SizeF(ctr_field, const_height);
                        curpos = 4f + lcount2.Width;
                        this.fcount2.Location = new PointF(curpos, recpos);
                        recpos += const_height + 0.0667f;
                    }

                    if (rpt_value.ar_string0[1] == "Y")
                    {
                        footer_color();
                        tshape.Height = recpos - 0.036F;
                        this.Sections["groupFooter2"].Controls.Add(tshape);
                        tshape.SendToBack();
                    }
                }
            }

            if (rpt_value.ar_string0[2] == "Y" || rpt_value.ar_string0[7] == "Y" || rpt_value.ar_string0[14] == "Y")
            {
                recpos = 0.07f;
                this.groupHeader3.Visible = true;
                this.lcode3.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname3.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname3.Size = new SizeF(leve13_size, const_height);
                this.lcode3.Size = new SizeF(leve1_code_size, const_height);
                this.lname3.Location = new PointF(head_start, 0);

                this.groupHeader3.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
                if (rpt_value.ar_string0[14] == "Y")
                    this.groupHeader3.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;

                if (rpt_value.ar_string0[2] == "Y" || rpt_value.ar_string0[7] == "Y")
                {
                    this.groupFooter3.Visible = true;

                    if (rpt_value.ar_string0[2] == "Y")
                        recpos += const_height + 0.0667f;

                    if (rpt_value.ar_string0[7] == "Y")
                    {
                        this.lcount3.Visible = true;
                        this.fcount3.Visible = true;
                        this.lcount3.Location = new PointF(4f, recpos);
                        this.lcount3.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.fcount3.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.lcount3.Size = new SizeF(ctr_lbl, const_height);
                        this.fcount3.Size = new SizeF(ctr_field, const_height);
                        curpos = 4f + lcount3.Width;
                        this.fcount3.Location = new PointF(curpos, recpos);
                        recpos += const_height + 0.0667f;
                    }

                    if (rpt_value.ar_string0[2] == "Y")
                    {
                        footer_color();
                        tshape.Height = recpos - 0.036F;
                        this.Sections["groupFooter3"].Controls.Add(tshape);
                        tshape.SendToBack();
                    }
                }
            }

            if (rpt_value.ar_string0[3] == "Y" || rpt_value.ar_string0[8] == "Y" || rpt_value.ar_string0[15] == "Y")
            {
                recpos = 0.07f;
                this.lcode4.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname4.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname4.Size = new SizeF(leve14_size, const_height);
                this.lcode4.Size = new SizeF(leve1_code_size, const_height);
                this.lname4.Location = new PointF(head_start, 0);

                this.groupHeader4.Visible = true;
                this.groupHeader4.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
                if (rpt_value.ar_string0[15] == "Y")
                    this.groupHeader4.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;

                if (rpt_value.ar_string0[3] == "Y" || rpt_value.ar_string0[8] == "Y")
                {
                    this.groupFooter4.Visible = true;

                    if (rpt_value.ar_string0[3] == "Y")
                        recpos += const_height + 0.0667f;

                    if (rpt_value.ar_string0[8] == "Y")
                    {
                        this.lcount4.Visible = true;
                        this.fcount4.Visible = true;
                        this.lcount4.Location = new PointF(4f, recpos);
                        this.lcount4.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.fcount4.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.lcount4.Size = new SizeF(ctr_lbl, const_height);
                        this.fcount4.Size = new SizeF(ctr_field, const_height);
                        curpos = 4f + lcount4.Width;
                        this.fcount4.Location = new PointF(curpos, recpos);
                        recpos += const_height + 0.0667f;
                    }

                    if (rpt_value.ar_string0[3] == "Y")
                    {
                        footer_color();
                        tshape.Height = recpos - 0.036F;
                        this.Sections["groupFooter4"].Controls.Add(tshape);
                        tshape.SendToBack();
                    }
                }
            }

            if (rpt_value.ar_string0[4] == "Y" || rpt_value.ar_string0[9] == "Y" || rpt_value.ar_string0[16] == "Y")
            {
                recpos = 0.07f;
                this.groupHeader5.Visible = true;
                this.lcode5.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname5.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lname5.Size = new SizeF(leve15_size, const_height);
                this.lcode5.Size = new SizeF(leve1_code_size, const_height);
                this.lname5.Location = new PointF(head_start, 0);

                this.groupHeader5.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
                if (rpt_value.ar_string0[16] == "Y")
                    this.groupHeader5.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;

                if (rpt_value.ar_string0[4] == "Y" || rpt_value.ar_string0[9] == "Y")
                {
                    this.groupFooter5.Visible = true;

                    if (rpt_value.ar_string0[4] == "Y")
                        recpos += const_height + 0.0667f;

                    if (rpt_value.ar_string0[9] == "Y")
                    {
                        this.lcount5.Visible = true;
                        this.fcount5.Visible = true;
                        this.lcount5.Location = new PointF(4f, recpos);
                        this.lcount5.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.fcount5.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                        this.lcount5.Size = new SizeF(ctr_lbl, const_height);
                        this.fcount5.Size = new SizeF(ctr_field, const_height);
                        curpos = 4f + lcount5.Width;
                        this.fcount5.Location = new PointF(curpos, recpos);
                        recpos += const_height + 0.0667f;
                    }

                    if (rpt_value.ar_string0[4] == "Y")
                    {
                        footer_color();
                        tshape.Height = recpos - 0.036F;
                        this.Sections["groupFooter5"].Controls.Add(tshape);
                        tshape.SendToBack();
                    }
                }
            }

            // grand footer
            if (audit_type != "N")
            {
                if (audit_type == "P")
                    this.reportFooter1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;

                this.lcountr.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.fcountr.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
                this.lcountr.Size = new SizeF(ctr_lbl, const_height);
                this.fcountr.Size = new SizeF(ctr_field, const_height);
                curpos = 4f + lcountr.Width;
                this.fcountr.Location = new PointF(curpos, this.fcountr.Location.Y);

                recpos = (2 * const_height) + 0.14F;
                footer_color();
                tshape.Location = new PointF(headbox.Location.X, 0.30f);
                tshape.Height = recpos;
                this.Sections["reportFooter1"].Controls.Add(tshape);
                tshape.SendToBack();
            }

        }


        private void footer_color()
        {
            tshape = new GrapeCity.ActiveReports.SectionReportModel.Shape();
            tshape.Location = new PointF(headbox.Location.X, 0.04f);
            tshape.Size = headbox.Size;
            tshape.Style = GrapeCity.ActiveReports.SectionReportModel.ShapeType.RoundRect;
            tshape.RoundingRadius = headbox.RoundingRadius;
            tshape.BackColor = System.Drawing.Color.GhostWhite;
            if (total_color.Trim() != "")
                tshape.BackColor = Color.FromName(total_color);

        }

        private void activity_header()
        {
            int linepos=1;
            divname = "groupHeader6";

            x3 = 0;
            x1=0;
            y2=0;
            fwidth = (float)((p_charsize * psize * 6) / 1440);

            query = "select * from [" + pblock.userid + "rtotal1] order by cast(column5 as int), cast(column7 as int) ";
            var str13 = db.Database.SqlQuery<rep_column>(query);
            foreach(var item in str13)
            {
                if (linepos != Convert.ToInt16(item.column5))
                {
                    x1 += fwidth + p_spacing;
                    linepos = Convert.ToInt16(item.column5);
                    y2=(linepos-1)* const_height;
                    if (x1 > x3)
                        x3 = x1;
                    x1=0;
                    fwidth = (float)((p_charsize * psize * 6) / 1440);
                }
                set_name_label(item.column3, item.column4);
                label_obj.Text = item.column1;
                text_fobj.DataField = item.column2;

                if (item.column6 == "M" || item.column6 == "A" || item.column6 == "NT")
                {
                    text_tobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;
                    text_tobj.OutputFormat = "#,##0.00 ";
                    if (negative_type == "B")
                        text_tobj.OutputFormat = "#,##0.00 ;(#,##0.00)";
                }

            }

// footer values
            divname = "groupFooter6";

            x1 = 0;
            y2 = 0.18F;
            linepos = 1;
            fwidth = (float)((p_charsize * psize * 6) / 1440);

            query = "select * from [" + pblock.userid + "rtotal2] order by cast(column5 as int), cast(column7 as int) ";
            str13 = db.Database.SqlQuery<rep_column>(query);
            foreach (var item in str13)
            {
                if (linepos != Convert.ToInt16(item.column5))
                {
                    x1 += fwidth + p_spacing;
                    linepos = Convert.ToInt16(item.column5);
                    y2 = (linepos - 1) * const_height + 0.18F; 
                    if (x1 > x3)
                        x3 = x1;
                    x1 = 0;
                    fwidth = (float)((p_charsize * psize * 6) / 1440);
                }
                set_name_label(item.column3, item.column4);
                label_obj.Text = item.column1;
                text_fobj.DataField = item.column2;

                if (item.column6 == "M" || item.column6 == "A" || item.column6 == "NT")
                {
                    text_tobj.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right;
                    text_tobj.OutputFormat = "#,##0.00 ";
                    if (negative_type == "B")
                        text_tobj.OutputFormat = "#,##0.00 ;(#,##0.00)";
                }

            }

            x1 = x3 > x1 ? x3 : x1;

            //    foreach (var item in str13)
        //    {
        //        fwidth = (float)((p_charsize * psize * 6) / 1440);

        //        x1 = 0;
        //        //col_ctr++;
        //        y2 = 0;

        //        set_name_label("10", item.column1);
        //        label_obj.Text = "KRA :";
        //        text_fobj.DataField = "column7";

        //        set_name_label("6", item.column2);
        //        label_obj.Text = "Year:";
        //        text_fobj.DataField = "column6";

        //        set_name_label("16", item.column3);
        //        label_obj.Text = "Date Duration :";
        //        text_fobj.DataField = "column8";

        //        set_name_label("15", item.column4);
        //        label_obj.Text = "Target Score :";
        //        text_fobj.DataField = "column9";

        //        divname = "groupFooter6";
        //        x1 = 0;
        //        y2 = 0.18F;

        //        set_name_label("18", "85");
        //        label_obj.Text = "Summary Comment :";
        //        text_fobj.DataField = "column12";

        //        set_name_label("16", item.column8);
        //        label_obj.Text = "Summary Score :";
        //        text_fobj.DataField = "column13";

        //        y2 = const_height;
        //        divname = "groupHeader6";
        //        fwidth = (float)((p_charsize * psize * 6) / 1440);
        //        x1 = 0;
        //        set_name_label("10", "90");
        //        label_obj.Text = "Target :";
        //        text_fobj.DataField = "column10";

        //        set_name_label("10", item.column6);
        //        label_obj.Text = "Status :";
        //        text_fobj.DataField = "column11";

        //    }
        }

        private void set_name_label(string sizet,string sizev)
        {
            x1 += fwidth + p_spacing;
            int widthint = Convert.ToInt16(sizet);
            fwidth = (float)((p_charsize * psize * widthint) / 1440);

            label_obj = new GrapeCity.ActiveReports.SectionReportModel.Label();
            label_obj.Location = new PointF(x1, y2);
            label_obj.Size = new SizeF(fwidth, const_height);
            label_obj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Regular);
            label_obj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
            this.Sections[divname].Controls.Add(label_obj);

            x1 += fwidth + p_spacing;
            widthint = Convert.ToInt16(sizev);
            fwidth = (float)((p_charsize * psize * widthint) / 1440);

            text_fobj = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            text_fobj.Location = new PointF(x1, y2);
            text_fobj.Size = new SizeF(fwidth, const_height);
            text_fobj.Font = new System.Drawing.Font(pfont, psize, FontStyle.Bold);
            text_fobj.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
            this.Sections[divname].Controls.Add(text_fobj);
        }

    }
}

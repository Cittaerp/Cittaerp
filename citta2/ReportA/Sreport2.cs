using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CittaErp.Models;
using anchor1.Models;
using System.Web;
using System.Data.SqlClient;
using System.IO;

namespace CittaErp.ReportA
{
    /// <summary>
    /// Summary description for SectionReport1.
    /// </summary>
    public partial class Sreport2 : GrapeCity.ActiveReports.SectionReport
    {

        //GrapeCity.ActiveReports.SectionReportModel.Label label_obj;
        //GrapeCity.ActiveReports.SectionReportModel.TextBox text_tobj;
        //GrapeCity.ActiveReports.SectionReportModel.TextBox text_fobj;
        //GrapeCity.ActiveReports.SectionReportModel.Picture tpict;
        //GrapeCity.ActiveReports.SectionReportModel.Shape tshape;

        string pfont = "tahoma";
        int psize = 10;
        float x1 = 0;
        float fwidth = 0;
        vw_collect rpt_value = new vw_collect();
        SqlConnection con;
        SqlDataReader reader;
        MainContext db;
        pubsess pblock;
        psess psess;
        string query;
        //float const_y = 1.062F;
        float const_height = 0.2F;
        float const_x = 0.125F;
        string head_color; string total_color;
        string negative_type = "S"; string audit_type = "C"; decimal p_charsize = 12; float p_spacing = 0.125F;
        string filepathpdf; string pic_status;

        public Sreport2()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private void Sreport2_ReportStart(object sender, EventArgs e)
        {

            db = new MainContext();
            pblock = (pubsess)HttpContext.Current.Session["pubsess"];
            filepathpdf = psess.temp7.ToString();
            pic_status= psess.sarrayt0[7].ToString();

            string conString = db.Database.Connection.ConnectionString;

            con = new SqlConnection(conString);
            string str4 = "select cvalue c1 from pub_table where name1='orderby' and userid='" + pblock.userid + "'";
            var str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
            string orderbyt = "";
            if (str5 != null)
                orderbyt = str5.c1;

            string query = psess.sarrayt0[6].ToString();
            query += " order by " + orderbyt + " snumber";
            query += ", cast(column2 as int)";

            SqlCommand ad = new SqlCommand(query, con);
            con.Open();
            reader = ad.ExecuteReader();
            this.DataSource = reader;

            main01();
        }

        private void Sreport2_ReportEnd(object sender, EventArgs e)
        {
            string view_name = HttpContext.Current.Server.MapPath("~/print/zx" + pblock.userid + ".rdf");

            if (filepathpdf == "")
                this.Document.Save(view_name);
            else
                export_rtn();

            reader.Close();
            con.Close();

        }

        private void main01()
        {
            decimal page_top = 0; decimal page_bottom = 0; decimal page_right = 0; decimal page_left = 0;
            decimal page_width = 0; decimal page_height = 0;

            string str4 = "select field1 c1,field2 c2,field9 c3, field10 c4,field5 c5, field6 c6 from tab_train_default where default_code='REPORT'";
            var str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();

            if (str5 != null)
            {
                negative_type = str5.c1;
                audit_type = str5.c2;
                p_charsize = Convert.ToDecimal(str5.c3);
                p_spacing = (float)Convert.ToDouble(str5.c4);
                head_color = str5.c5;
                total_color = str5.c6;
            }

            // need values from layout
            // total 1-5
            // rcount 1-5
            // payroll period
            // report title
            // page 1-5

            if (psess.sarrayt1 == null)
                rpt_value.ar_string0 = new string[] { "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "", "", "N", "N", "N", "N", "N" };
            //else
            //    rpt_value = (vw_collect)HttpContext.Current.psess.sarrayt1;

            string pr_codet = pblock.printer_code;
            var dblogo = (from bgl in db.GB_001_COY where bgl.id_code == "COYINFO" select bgl).FirstOrDefault();

            // read printer to get font and size
            if (rpt_value.ar_string0[11] != "")
            {
                str4 = "select report_name c0 from tab_calc where calc_code='" + rpt_value.ar_string0[11] + "' and para_code='" + HttpContext.Current.Session["adv1"].ToString() + "' ";
                str5 = db.Database.SqlQuery<vw_query>(str4).FirstOrDefault();
                if (str5 != null)
                    pr_codet = str5.c0;

                if (str5 == null)
                {
                    pr_codet = dblogo.field5;
                }
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




            // logo picture
            string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Uploads/company.png");
            if (!System.IO.File.Exists(filePath))
                filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Images/nologo.png");

            this.picture1.Image = Image.FromFile(filePath);
            if (pic_status == "doc")
                this.picture1.Visible = false;

// if mailing hide report footer message
            if (filepathpdf != "")
                this.reportFooter1.Visible = false;
            
            //label for sno
            // start position x = 0.125, y = 1.062, height =0.2
    
            query = "select column11 c0 from [" + pblock.userid + "ophead] where column1 ='1'";
            var str12 = db.Database.SqlQuery<vw_query>(query).FirstOrDefault();
            fwidth = (float)((p_charsize * psize * Convert.ToDecimal(str12.c0)) / 1440);

            this.det1.Location = new PointF(const_x, 0f);
            this.det1.DataField = "column1";
            this.det1.Font = new System.Drawing.Font(pfont, psize);
            this.det1.VerticalAlignment = GrapeCity.ActiveReports.Document.Section.VerticalTextAlignment.Middle;
            this.det1.Size = new SizeF(fwidth, const_height);

            x1 = const_x;

            x1 += fwidth + p_spacing;
            this.PrintWidth = x1 + p_spacing;

            this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait;
            if (paper_orientation == "L")
                this.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;

            this.PageSettings.Margins.Bottom = (float)page_bottom;
            this.PageSettings.Margins.Top = (float)page_top;
            this.PageSettings.Margins.Right = (float)page_right;
            this.PageSettings.Margins.Left = (float)page_left;
            this.PageSettings.PaperWidth = (float)page_width;
            this.PageSettings.PaperHeight = (float)page_height;

            if (rpt_value.ar_string0[12] == "Y")
            {
                this.groupHeader1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
                this.groupHeader1.Visible = true;
            }

            if (rpt_value.ar_string0[13] == "Y")
            {
                this.groupHeader2.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
                this.groupHeader2.Visible = true;
            }

            if (rpt_value.ar_string0[14] == "Y")
            {
                this.groupHeader3.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
                this.groupHeader3.Visible = true;
            }

            if (rpt_value.ar_string0[15] == "Y")
            {
                this.groupHeader4.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
                this.groupHeader4.Visible = true;
            }

            if (rpt_value.ar_string0[16] == "Y")
            {
                this.groupHeader5.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
                this.groupHeader5.Visible = true;
            }

            this.reportFooter1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;

        }

        private void export_rtn()
        {
            GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport pdffile = new GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport();
            pdffile.Export(this.Document, filepathpdf);

        }

    }
}

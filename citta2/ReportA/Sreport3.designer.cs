namespace CittaErp.ReportA
{
    /// <summary>
    /// Summary description for SectionReport1.
    /// </summary>
    partial class Sreport3
    {
        private GrapeCity.ActiveReports.SectionReportModel.PageHeader pageHeader;
        private GrapeCity.ActiveReports.SectionReportModel.Detail detail;
        private GrapeCity.ActiveReports.SectionReportModel.PageFooter pageFooter;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        #region ActiveReport Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sreport3));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.company_name = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.report_name = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.userid = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.picture1 = new GrapeCity.ActiveReports.SectionReportModel.Picture();
            this.pperiod = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.header_line = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.det1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
            this.pagetctr = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.reportInfo1 = new GrapeCity.ActiveReports.SectionReportModel.ReportInfo();
            this.page_line = new GrapeCity.ActiveReports.SectionReportModel.Line();
            this.groupHeader1 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter1 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.groupHeader2 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter2 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.groupHeader3 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter3 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.groupHeader4 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter4 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.groupHeader5 = new GrapeCity.ActiveReports.SectionReportModel.GroupHeader();
            this.groupFooter5 = new GrapeCity.ActiveReports.SectionReportModel.GroupFooter();
            this.reportHeader1 = new GrapeCity.ActiveReports.SectionReportModel.ReportHeader();
            this.reportFooter1 = new GrapeCity.ActiveReports.SectionReportModel.ReportFooter();
            this.lcountr = new GrapeCity.ActiveReports.SectionReportModel.Label();
            this.fcountr = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.company_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.report_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pperiod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.det1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pagetctr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcountr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fcountr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.company_name,
            this.report_name,
            this.userid,
            this.picture1,
            this.pperiod,
            this.header_line});
            this.pageHeader.Height = 1.2115F;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.Format += new System.EventHandler(this.pageHeader_Format);
            // 
            // company_name
            // 
            this.company_name.Height = 0.2F;
            this.company_name.HyperLink = null;
            this.company_name.Left = 5F;
            this.company_name.Name = "company_name";
            this.company_name.Style = "font-size: 12pt; vertical-align: middle; ddo-char-set: 0; ddo-shrink-to-fit: none" +
    "";
            this.company_name.Text = "Company Name";
            this.company_name.Top = 0.012F;
            this.company_name.Width = 3.5F;
            // 
            // report_name
            // 
            this.report_name.Height = 0.2F;
            this.report_name.HyperLink = null;
            this.report_name.Left = 0.06200005F;
            this.report_name.Name = "report_name";
            this.report_name.Style = "font-size: 14.25pt; vertical-align: middle; ddo-char-set: 0";
            this.report_name.Text = "Report Name";
            this.report_name.Top = 0.837F;
            this.report_name.Width = 4.5F;
            // 
            // userid
            // 
            this.userid.Height = 0.2F;
            this.userid.HyperLink = null;
            this.userid.Left = 4.99F;
            this.userid.MultiLine = false;
            this.userid.Name = "userid";
            this.userid.Style = "font-size: 12pt; vertical-align: middle; ddo-char-set: 0; ddo-shrink-to-fit: none" +
    "";
            this.userid.Text = "User Id";
            this.userid.Top = 0.25F;
            this.userid.Width = 3F;
            // 
            // picture1
            // 
            this.picture1.Height = 0.812F;
            this.picture1.ImageData = ((System.IO.Stream)(resources.GetObject("picture1.ImageData")));
            this.picture1.Left = 0.078F;
            this.picture1.Name = "picture1";
            this.picture1.PictureAlignment = GrapeCity.ActiveReports.SectionReportModel.PictureAlignment.TopLeft;
            this.picture1.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Zoom;
            this.picture1.Top = 0F;
            this.picture1.Width = 2.922F;
            // 
            // pperiod
            // 
            this.pperiod.Height = 0.2F;
            this.pperiod.HyperLink = null;
            this.pperiod.Left = 5F;
            this.pperiod.MultiLine = false;
            this.pperiod.Name = "pperiod";
            this.pperiod.Style = "font-size: 12pt; vertical-align: middle; ddo-char-set: 0; ddo-shrink-to-fit: none" +
    "";
            this.pperiod.Text = "Payroll Period";
            this.pperiod.Top = 0.5F;
            this.pperiod.Width = 3F;
            // 
            // header_line
            // 
            this.header_line.Height = 0F;
            this.header_line.Left = 0F;
            this.header_line.LineWeight = 1F;
            this.header_line.Name = "header_line";
            this.header_line.Top = 1.062F;
            this.header_line.Width = 6.937F;
            this.header_line.X1 = 0F;
            this.header_line.X2 = 6.937F;
            this.header_line.Y1 = 1.062F;
            this.header_line.Y2 = 1.062F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.det1});
            this.detail.Height = 0.333F;
            this.detail.Name = "detail";
            this.detail.Format += new System.EventHandler(this.detail_Format);
            // 
            // det1
            // 
            this.det1.DataField = "colseq";
            this.det1.Height = 0.2F;
            this.det1.Left = 0.06200004F;
            this.det1.Name = "det1";
            this.det1.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count;
            this.det1.SummaryGroup = "groupHeader1";
            this.det1.Text = "9999";
            this.det1.Top = 0F;
            this.det1.Width = 0.375F;
            // 
            // pageFooter
            // 
            this.pageFooter.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.pagetctr,
            this.reportInfo1,
            this.page_line});
            this.pageFooter.Height = 0.3541666F;
            this.pageFooter.Name = "pageFooter";
            // 
            // pagetctr
            // 
            this.pagetctr.FormatString = "Page {PageNumber} of {PageCount}";
            this.pagetctr.Height = 0.2F;
            this.pagetctr.Left = 5.292F;
            this.pagetctr.Name = "pagetctr";
            this.pagetctr.Style = "vertical-align: middle";
            this.pagetctr.Top = 0.062F;
            this.pagetctr.Width = 2.375F;
            // 
            // reportInfo1
            // 
            this.reportInfo1.CanShrink = true;
            this.reportInfo1.FormatString = "{RunDateTime:dddd, MMMM d yyyy  hh:mm tt} ";
            this.reportInfo1.Height = 0.2F;
            this.reportInfo1.Left = 0.062F;
            this.reportInfo1.Name = "reportInfo1";
            this.reportInfo1.Style = "vertical-align: middle";
            this.reportInfo1.Top = 0.062F;
            this.reportInfo1.Width = 3.438F;
            // 
            // page_line
            // 
            this.page_line.Height = 0F;
            this.page_line.Left = 0.187F;
            this.page_line.LineWeight = 1F;
            this.page_line.Name = "page_line";
            this.page_line.Top = 0F;
            this.page_line.Width = 4.137F;
            this.page_line.X1 = 0.187F;
            this.page_line.X2 = 4.324F;
            this.page_line.Y1 = 0F;
            this.page_line.Y2 = 0F;
            // 
            // groupHeader1
            // 
            this.groupHeader1.DataField = "column22";
            this.groupHeader1.Height = 0.3225833F;
            this.groupHeader1.Name = "groupHeader1";
            this.groupHeader1.RepeatStyle = GrapeCity.ActiveReports.SectionReportModel.RepeatStyle.OnPageIncludeNoDetail;
            this.groupHeader1.Format += new System.EventHandler(this.groupHeader1_Format);
            // 
            // groupFooter1
            // 
            this.groupFooter1.Height = 0.208F;
            this.groupFooter1.Name = "groupFooter1";
            // 
            // groupHeader2
            // 
            this.groupHeader2.DataField = "snumber";
            this.groupHeader2.Height = 0.6871666F;
            this.groupHeader2.Name = "groupHeader2";
            this.groupHeader2.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            this.groupHeader2.Format += new System.EventHandler(this.groupHeader2_Format);
            // 
            // groupFooter2
            // 
            this.groupFooter2.Height = 0.208F;
            this.groupFooter2.Name = "groupFooter2";
            this.groupFooter2.Visible = false;
            // 
            // groupHeader3
            // 
            this.groupHeader3.DataField = "level3_name";
            this.groupHeader3.Height = 0.208F;
            this.groupHeader3.Name = "groupHeader3";
            this.groupHeader3.Visible = false;
            // 
            // groupFooter3
            // 
            this.groupFooter3.Height = 0.208F;
            this.groupFooter3.Name = "groupFooter3";
            this.groupFooter3.Visible = false;
            // 
            // groupHeader4
            // 
            this.groupHeader4.DataField = "level4_name";
            this.groupHeader4.Height = 0.208F;
            this.groupHeader4.Name = "groupHeader4";
            this.groupHeader4.Visible = false;
            // 
            // groupFooter4
            // 
            this.groupFooter4.Height = 0.208F;
            this.groupFooter4.Name = "groupFooter4";
            this.groupFooter4.Visible = false;
            // 
            // groupHeader5
            // 
            this.groupHeader5.DataField = "level5_name";
            this.groupHeader5.Height = 0.2083333F;
            this.groupHeader5.Name = "groupHeader5";
            this.groupHeader5.Visible = false;
            // 
            // groupFooter5
            // 
            this.groupFooter5.Height = 0.208F;
            this.groupFooter5.Name = "groupFooter5";
            this.groupFooter5.Visible = false;
            // 
            // reportHeader1
            // 
            this.reportHeader1.Height = 0F;
            this.reportHeader1.Name = "reportHeader1";
            // 
            // reportFooter1
            // 
            this.reportFooter1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lcountr,
            this.fcountr});
            this.reportFooter1.Height = 1.125F;
            this.reportFooter1.Name = "reportFooter1";
            this.reportFooter1.Visible = false;
            // 
            // lcountr
            // 
            this.lcountr.Height = 0.1979167F;
            this.lcountr.HyperLink = null;
            this.lcountr.Left = 4F;
            this.lcountr.Name = "lcountr";
            this.lcountr.Style = "vertical-align: middle";
            this.lcountr.Text = "Record Count is ";
            this.lcountr.Top = 0.625F;
            this.lcountr.Width = 1.312F;
            // 
            // fcountr
            // 
            this.fcountr.DataField = "record_counter";
            this.fcountr.Height = 0.1979167F;
            this.fcountr.Left = 5.501F;
            this.fcountr.Name = "fcountr";
            this.fcountr.OutputFormat = resources.GetString("fcountr.OutputFormat");
            this.fcountr.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count;
            this.fcountr.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All;
            this.fcountr.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.fcountr.Text = "99999";
            this.fcountr.Top = 0.625F;
            this.fcountr.Width = 0.4370003F;
            // 
            // Sreport3
            // 
            this.MasterReport = false;
            this.PageSettings.DefaultPaperSize = false;
            this.PageSettings.Margins.Bottom = 0.5F;
            this.PageSettings.Margins.Left = 0.5F;
            this.PageSettings.Margins.Right = 0.5F;
            this.PageSettings.Margins.Top = 0.5F;
            this.PageSettings.PaperHeight = 11.69291F;
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.PageSettings.PaperWidth = 8.267716F;
            this.PrintWidth = 9.708333F;
            this.Sections.Add(this.reportHeader1);
            this.Sections.Add(this.pageHeader);
            this.Sections.Add(this.groupHeader5);
            this.Sections.Add(this.groupHeader4);
            this.Sections.Add(this.groupHeader3);
            this.Sections.Add(this.groupHeader2);
            this.Sections.Add(this.groupHeader1);
            this.Sections.Add(this.detail);
            this.Sections.Add(this.groupFooter1);
            this.Sections.Add(this.groupFooter2);
            this.Sections.Add(this.groupFooter3);
            this.Sections.Add(this.groupFooter4);
            this.Sections.Add(this.groupFooter5);
            this.Sections.Add(this.pageFooter);
            this.Sections.Add(this.reportFooter1);
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" +
            "l; font-size: 10pt; color: Black; ddo-char-set: 186; vertical-align: middle", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ddo-char-set: 186", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
            "lic; ddo-char-set: 186", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 186", "Heading3", "Normal"));
            this.ReportStart += new System.EventHandler(this.Sreport3_ReportStart);
            this.ReportEnd += new System.EventHandler(this.Sreport3_ReportEnd);
            ((System.ComponentModel.ISupportInitialize)(this.company_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.report_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pperiod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.det1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pagetctr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcountr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fcountr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Label company_name;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader2;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter2;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader3;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter3;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader4;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter4;
        private GrapeCity.ActiveReports.SectionReportModel.GroupHeader groupHeader5;
        private GrapeCity.ActiveReports.SectionReportModel.GroupFooter groupFooter5;
        private GrapeCity.ActiveReports.SectionReportModel.Label report_name;
        private GrapeCity.ActiveReports.SectionReportModel.Label userid;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox det1;
        private GrapeCity.ActiveReports.SectionReportModel.ReportHeader reportHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.ReportFooter reportFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lcountr;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox fcountr;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo pagetctr;
        private GrapeCity.ActiveReports.SectionReportModel.ReportInfo reportInfo1;
        private GrapeCity.ActiveReports.SectionReportModel.Picture picture1;
        private GrapeCity.ActiveReports.SectionReportModel.Line page_line;
        private GrapeCity.ActiveReports.SectionReportModel.Label pperiod;
        private GrapeCity.ActiveReports.SectionReportModel.Line header_line;
    }
}

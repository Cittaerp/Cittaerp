namespace CittaErp.ReportA
{
    /// <summary>
    /// Summary description for SectionReport1.
    /// </summary>
    partial class Sreport2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sreport2));
            this.pageHeader = new GrapeCity.ActiveReports.SectionReportModel.PageHeader();
            this.picture1 = new GrapeCity.ActiveReports.SectionReportModel.Picture();
            this.detail = new GrapeCity.ActiveReports.SectionReportModel.Detail();
            this.det1 = new GrapeCity.ActiveReports.SectionReportModel.TextBox();
            this.pageFooter = new GrapeCity.ActiveReports.SectionReportModel.PageFooter();
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
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.det1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcountr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fcountr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeader
            // 
            this.pageHeader.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.picture1});
            this.pageHeader.Height = 0.9370001F;
            this.pageHeader.Name = "pageHeader";
            // 
            // picture1
            // 
            this.picture1.Height = 0.812F;
            this.picture1.HyperLink = null;
            this.picture1.ImageData = ((System.IO.Stream)(resources.GetObject("picture1.ImageData")));
            this.picture1.Left = 0.05200005F;
            this.picture1.Name = "picture1";
            this.picture1.PictureAlignment = GrapeCity.ActiveReports.SectionReportModel.PictureAlignment.TopLeft;
            this.picture1.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Zoom;
            this.picture1.Top = 0F;
            this.picture1.Width = 3.5F;
            // 
            // detail
            // 
            this.detail.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.det1});
            this.detail.Height = 0.2500001F;
            this.detail.Name = "detail";
            // 
            // det1
            // 
            this.det1.Height = 0.2F;
            this.det1.Left = 0.0625F;
            this.det1.Name = "det1";
            this.det1.Style = "white-space: nowrap; ddo-wrap-mode: nowrap";
            this.det1.Text = "det1";
            this.det1.Top = 0F;
            this.det1.Width = 1F;
            // 
            // pageFooter
            // 
            this.pageFooter.Name = "pageFooter";
            this.pageFooter.Visible = false;
            // 
            // groupHeader1
            // 
            this.groupHeader1.CanShrink = true;
            this.groupHeader1.DataField = "level1_name";
            this.groupHeader1.Name = "groupHeader1";
            this.groupHeader1.Visible = false;
            // 
            // groupFooter1
            // 
            this.groupFooter1.Name = "groupFooter1";
            this.groupFooter1.Visible = false;
            // 
            // groupHeader2
            // 
            this.groupHeader2.DataField = "level2_name";
            this.groupHeader2.Name = "groupHeader2";
            this.groupHeader2.Visible = false;
            // 
            // groupFooter2
            // 
            this.groupFooter2.Name = "groupFooter2";
            this.groupFooter2.Visible = false;
            // 
            // groupHeader3
            // 
            this.groupHeader3.DataField = "level3_name";
            this.groupHeader3.Name = "groupHeader3";
            this.groupHeader3.Visible = false;
            // 
            // groupFooter3
            // 
            this.groupFooter3.Name = "groupFooter3";
            this.groupFooter3.Visible = false;
            // 
            // groupHeader4
            // 
            this.groupHeader4.DataField = "level4_name";
            this.groupHeader4.Name = "groupHeader4";
            this.groupHeader4.Visible = false;
            // 
            // groupFooter4
            // 
            this.groupFooter4.Name = "groupFooter4";
            this.groupFooter4.Visible = false;
            // 
            // groupHeader5
            // 
            this.groupHeader5.DataField = "level5_name";
            this.groupHeader5.Name = "groupHeader5";
            this.groupHeader5.Visible = false;
            // 
            // groupFooter5
            // 
            this.groupFooter5.Name = "groupFooter5";
            this.groupFooter5.Visible = false;
            // 
            // reportHeader1
            // 
            this.reportHeader1.Height = 0.0625F;
            this.reportHeader1.Name = "reportHeader1";
            this.reportHeader1.Visible = false;
            // 
            // reportFooter1
            // 
            this.reportFooter1.Controls.AddRange(new GrapeCity.ActiveReports.SectionReportModel.ARControl[] {
            this.lcountr,
            this.fcountr});
            this.reportFooter1.Height = 0.53125F;
            this.reportFooter1.Name = "reportFooter1";
            this.reportFooter1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before;
            // 
            // lcountr
            // 
            this.lcountr.Height = 0.1979167F;
            this.lcountr.HyperLink = null;
            this.lcountr.Left = 2.4165F;
            this.lcountr.Name = "lcountr";
            this.lcountr.Style = "vertical-align: middle";
            this.lcountr.Text = "Record Count is ";
            this.lcountr.Top = 0.1666666F;
            this.lcountr.Width = 1.125F;
            // 
            // fcountr
            // 
            this.fcountr.DistinctField = "snumber";
            this.fcountr.Height = 0.1979167F;
            this.fcountr.Left = 3.6465F;
            this.fcountr.Name = "fcountr";
            this.fcountr.OutputFormat = resources.GetString("fcountr.OutputFormat");
            this.fcountr.SummaryFunc = GrapeCity.ActiveReports.SectionReportModel.SummaryFunc.Count;
            this.fcountr.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.All;
            this.fcountr.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.GrandTotal;
            this.fcountr.Text = "99999";
            this.fcountr.Top = 0.1666666F;
            this.fcountr.Width = 0.4370004F;
            // 
            // Sreport2
            // 
            this.MasterReport = false;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
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
            "l; font-size: 10pt; color: Black", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" +
            "lic", "Heading2", "Normal"));
            this.StyleSheet.Add(new DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
            this.ReportStart += new System.EventHandler(this.Sreport2_ReportStart);
            this.ReportEnd += new System.EventHandler(this.Sreport2_ReportEnd);
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.det1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcountr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fcountr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private GrapeCity.ActiveReports.SectionReportModel.Picture picture1;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox det1;
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
        private GrapeCity.ActiveReports.SectionReportModel.ReportHeader reportHeader1;
        private GrapeCity.ActiveReports.SectionReportModel.ReportFooter reportFooter1;
        private GrapeCity.ActiveReports.SectionReportModel.Label lcountr;
        private GrapeCity.ActiveReports.SectionReportModel.TextBox fcountr;
    }
}

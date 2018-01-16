using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using CittaErp.Models;
using anchor1.Models;
using System.Data.SqlClient;
using System.Web;
using System.Data.OleDb;
using System.Data;

namespace CittaErp.ReportA
{
    /// <summary>
    /// Summary description for Sreport6.
    /// </summary>
    public partial class Sreport6 : GrapeCity.ActiveReports.SectionReport
    {
        vw_collect rpt_value = new vw_collect();
        SqlConnection con;
        SqlDataReader reader;
        MainContext db;
        pubsess pblock;
        GrapeCity.ActiveReports.Chart.Series series;
        GrapeCity.ActiveReports.Chart.ChartType ctype;
        Boolean err;
        psess psess;
        DataSet ds = new DataSet();
        string  minDate;string maxDate;
        string tablename; string datatypeq = ""; int chtcounter = 2;
        string colynames = "";
        string ygrid = "0"; string xgrid = "0"; int cstep = 0;string chtgrid="9001";


        public Sreport6()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private void Sreport6_ReportStart(object sender, EventArgs e)
        {
            db = new MainContext();
            pblock = (pubsess)HttpContext.Current.Session["pubsess"];

            string conString = db.Database.Connection.ConnectionString;

            con = new SqlConnection(conString);

            
            tablename = "[" + pblock.userid + "st01]";
            string str1 = "select min(column20) c0, max(column21) c1, min(column22) c2 from " + tablename;
            var str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            minDate = str2.c0;
            maxDate = str2.c1;
            datatypeq = str2.c2;

            str1 = "select cvalue c0 from pub_table where name1='chtctr' and userid='" + pblock.userid + "'";
            str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str2 !=null)
                chtcounter = Convert.ToInt16(str2.c0);

            str1 = "select cvalue c0 from pub_table where name1='chtgrid' and userid='" + pblock.userid + "'";
            str2 = db.Database.SqlQuery<vw_query>(str1).FirstOrDefault();
            if (str2 != null)
                chtgrid = str2.c0;
            xgrid = chtgrid.Substring(1, 1);
            ygrid = chtgrid.Substring(2, 1);
            cstep = Convert.ToInt16(chtgrid.Substring(3));
            if (cstep == 0)
                cstep = 1;

            this.chartControl1.DataSource = ds;

            main01();

        }

        private void main01()
        {

            string query = "Select * from " + tablename;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
            dataAdapter.Fill(ds, "Orders");
            int wctr1=0;
            string colname;
            int series_no = Convert.ToInt16(chtcounter / 2);
            for (int wctr = 1; wctr <= series_no; wctr++)
            {
                series = new GrapeCity.ActiveReports.Chart.Series();
                series.Type = GrapeCity.ActiveReports.Chart.ChartType.Gantt;
                this.chartControl1.Series.Add(series);
                colname = "seriest" + wctr.ToString();
                this.chartControl1.Series[wctr - 1].Name = colname;
                this.chartControl1.Series[colname].ValueMemberX = "snumber";
                wctr1 = (wctr * 2) - 1;
                colynames = "column" + wctr1.ToString() + ",column" + (wctr1 + 1).ToString();
                this.chartControl1.Series[colname].ValueMembersY = colynames;
            }
            //series = new GrapeCity.ActiveReports.Chart.Series();
            //series.Type = GrapeCity.ActiveReports.Chart.ChartType.Gantt;
            //this.chartControl1.Series.Add(series);
            //this.chartControl1.Series[0].Name = "seriest1";
            //colynames += ",column" + wctr.ToString();

            // Chart Titles
            this.chartControl1.Titles["header"].Text = psess.temp1.ToString(); ;
            this.chartControl1.Titles.Remove(this.chartControl1.Titles["footer"]); 
            // ValueMember
            //this.chartControl1.Series["seriest1"].ValueMemberX = "snumber";
            //this.chartControl1.Series["seriest2"].ValueMemberX = "snumber";
            //colynames = colynames.Substring(1);
            //this.chartControl1.Series["seriest1"].ValueMembersY = "column1,column2";
            //this.chartControl1.Series["seriest2"].ValueMembersY = "column3,column4";
            // AxisX properties
            this.chartControl1.ChartAreas[0].Axes["AxisX"].Title = "";
            this.chartControl1.ChartAreas[0].Axes["AxisX"].TitleFont.Angle = -90;
            this.chartControl1.ChartAreas[0].Axes["AxisX"].MajorTick.Visible = true;
            this.chartControl1.ChartAreas[0].Axes["AxisX"].LabelsVisible = true;
            if (xgrid == "1")
                this.chartControl1.ChartAreas[0].Axes["AxisX"].MajorTick.GridLine = new GrapeCity.ActiveReports.Chart.Graphics.Line(Color.Blue, 1, GrapeCity.ActiveReports.Chart.Graphics.LineStyle.Solid);
            // AxisY properties
            GrapeCity.ActiveReports.Chart.AxisBase axisY = this.chartControl1.ChartAreas[0].Axes["AxisY"];

            int labelCount = 0;
            if (datatypeq == "D")
            {
                // Use the min and max dates to add labels to the y-axis
                DateTime mdate = new DateTime(Convert.ToInt16(minDate.Substring(0, 4)), Convert.ToInt16(minDate.Substring(4, 2)), Convert.ToInt16(minDate.Substring(6, 2)));
                DateTime mxdate = new DateTime(Convert.ToInt16(maxDate.Substring(0, 4)), Convert.ToInt16(maxDate.Substring(4, 2)), Convert.ToInt16(maxDate.Substring(6, 2)));

                for (DateTime counter = mdate; counter <= mxdate; counter = counter.AddDays(cstep))
                {
                    labelCount++;
                    axisY.Labels.Add(counter.ToShortDateString());
                }
            }

            axisY.Min = 0;
            labelCount--;
            axisY.Max = labelCount;
            axisY.MajorTick.Step = cstep;
            axisY.MajorTick.Visible = true;
            axisY.LabelsVisible = true;
            if (ygrid=="1")
                axisY.MajorTick.GridLine = new GrapeCity.ActiveReports.Chart.Graphics.Line(Color.Gray, 1, GrapeCity.ActiveReports.Chart.Graphics.LineStyle.Solid);
            axisY.LabelFont.Angle = -45;
            axisY.TitleFont.Angle = 0;
            axisY.SmartLabels = false;
            axisY.Title = "";
            // Set the Chartcontrol Data Source
        }

        private void Sreport6_ReportEnd(object sender, EventArgs e)
        {
            string view_name = HttpContext.Current.Server.MapPath("~/print/zx" + pblock.userid + ".rdf");

            this.Document.Save(view_name);

            //reader.Close();
            con.Close();

        }

    }
}

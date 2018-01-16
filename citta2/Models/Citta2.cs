using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Models
{
    public class vw_genlay
    {
        public string vwstring0 {get;set;}
        public string vwstring1 {get;set;}
        public string vwstring2 {get;set;}
        public string vwstring3 {get;set;}
        public string vwstring4 {get;set;}
        public string vwstring5 {get;set;}
        public string vwstring6 {get;set;}
        public string vwstring7 {get;set;}
        public string vwstring8 {get;set;}
        public string vwstring9 {get;set;}
        public string vwstring10 { get; set; }
        public string vwstring11 { get; set; }
        public decimal vwdecimal0 { get; set; }
        public decimal vwdecimal1 {get;set;}
        public decimal vwdecimal2 {get;set;}
        public decimal vwdecimal3 {get;set;}
        public decimal vwdecimal4 {get;set;}
        public decimal vwdecimal5 { get; set; }
        public decimal vwdecimal6 { get; set; }
        public decimal vwdecimal7 { get; set; }
        public decimal vwdecimal8 { get; set; }
        public decimal vwfloat1 { get; set; }
        public int vwint0 {get;set;}
        public int vwint1 {get;set;}
        public int vwint2 {get;set;}
        public int vwint3 {get;set;}
        public int vwint4 {get;set;}
        public int vwint5 { get; set; }
        public int vwint6 { get; set; }
        public int vwint7 { get; set; }
        public int vwint8 { get; set; }
        public int vwint9 { get; set; }
        public int vwint10 { get; set; }
        public int[] vwitarray0 { get; set; }
        public int[] vwitarray1 { get; set; }
        public int[] vwitarray2 { get; set; }

        public int[] vwitarray3 { get; set; }
        public int[] vwitarray4 { get; set; }
        public bool vwbool0 { get; set; }
        public bool vwbool1 { get; set; }
        public bool vwbool2 { get; set; }
        public bool[] vwblarray0 { get; set; }
        public bool[] vwblarray1 { get; set; }
        public bool[] vwblarray2 { get; set; }
        public bool[] vwblarray3 { get; set; }
        public bool[] vwblarray4 { get; set; }
        public bool[] vwblarray5 { get; set; }

        public DateTime vwdate0 {get;set;}
        public DateTime vwdate1 { get; set; }
        public DateTime vwdate2 {get;set;}
        public DateTime[] vwdtarray0 { get; set; }
        public DateTime[] vwdtarray1 { get; set; }

        public string[] vwstrarray0 { get; set; }
        public string[] vwstrarray1 { get; set; }
        public string[] vwstrarray2 { get; set; }
        public string[] vwstrarray3 { get; set; }
        public string[] vwstrarray4 { get; set; }
        public string[] vwstrarray5 { get; set; }
        public string [] vwstrarray6 { get; set; }
        public string[] vwstrarray7 { get; set; }
        public string[] vwstrarray8 { get; set; }
        public string[] vwstrarray9 { get; set; }
        public string[] vwstrarray10 { get; set; }
        public string[] vwstrarray11 { get; set; }
        public string[] vwstrarray12 { get; set; }
        public string[] vwstrarray13 { get; set; }
        public string[] vwstrarray14 { get; set; }
        public string[] vwstrarray15 { get; set; }
        public string[] vwstrarray16 { get; set; }
        public string[] vwstrarray17 { get; set; }
        public string[] vwstrarray18 { get; set; }
        public decimal[] vwdclarray0 { get; set; }
        public decimal[] vwdclarray1 { get; set; }
        public decimal[] vwdclarray2 { get; set; }
        public decimal[] vwdclarray3 { get; set; }
        public decimal[] vwdclarray4 { get; set; }
        public decimal[] vwdclarray5 { get; set; }
        public decimal[] vwdclarray6 { get; set; }
        public byte[] picture12 { get; set; }
        public byte vwbyte1 { get; set; }
        public List<querylay>[] vwlist0 { get; set; }

        public string imagecat { get; set; }
        public string imagedesc { get; set; }
        public string datmode { get; set; }
        public string vwcode { get; set; }
        public string[] dsp_string { get; set; }


    }

    public class querylay
    {
        public string query0 { get; set; }
        public string query1 { get; set; }
        public string query2 { get; set; }
        public string query3 { get; set; }
        public string query4 { get; set; }
        public string query5 { get; set; }
        public string query6 { get; set; }
        public int intquery0 { get; set; }
        public int intquery1 { get; set; }
        public int intquery2 { get; set; }
        public decimal dquery0 { get; set; }
        public decimal dquery1 { get; set; }
        public decimal dquery2 { get; set; }
        public decimal dquery3 { get; set; }
        public bool bquery0 { get; set; }
        public decimal[] darray0 { get; set; }
        public decimal[] darray1 { get; set; }
        public decimal[] darray2 { get; set; }
        public decimal[] darray3 { get; set; }

    }
    
    [Serializable]
    public class psess
    {
        public string temp0 { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }
        public string temp6 { get; set; }
        public string temp7 { get; set; }
        public string temp8 { get; set; }
        public string temp9 { get; set; }
        public string temp10 { get; set; }
        public int intemp0 { get; set; }
        public int intemp1 { get; set; }
        public int intemp2 { get; set; }
        public decimal dtemp0 { get; set; }
        public decimal dtemp1 { get; set; }
        public decimal dtemp2 { get; set; }
        public decimal dtemp4 { get; set; }
        public bool btemp0 { get; set; }
        public decimal[] darrayt0 { get; set; }
        public decimal[] darrayt1 { get; set; }
        public decimal[] darrayt2 { get; set; }
        public decimal[] darrayt3 { get; set; }
        public string[] sarrayt0 { get; set; }
        public string[] sarrayt1 { get; set; }
        public string[] sarrayt2 { get; set; }
        public string[] sarrayt3 { get; set; }
        public string portalname { get; set; }
        public string[] pcar { get; set; }
        public string ptitle { get; set; }

    }

    [Serializable]
    public class pubsess
    {
        public string userid { get; set; }
        public string username { get; set; }
        public string usergroup { get; set; }
        public DateTime datein { get; set; }
        public string base_currency_code { get; set; }
        public string base_currency_description { get; set; }
        public string period_closing { get; set; }
        public string multi_currency { get; set; }
        public string country_operation { get; set; }
        public string curent_datefrm { get; set; }
        public string curent_dateto { get; set; }
        public string valid_datefrm { get; set; }
        public string valid_dateto { get; set; }
        public string entry_mode { get; set; }
        public string manual_discount { get; set; }
        public string manual_others { get; set; }
        public string price_editable { get; set; }
        public string exchange_editable { get; set; }
        public string exchange_rate_mode { get; set; }

        public string processing_period { get; set; }
        public DateTime lld { get; set; }       //last login date
        public string pname { get; set; }       // staff name
        public string printer_code { get; set; }    // default printer
    }
    public class addr
    {
        public string CU { get; set; }
        public string VR { get; set; }
        public string CY { get; set; }
        public decimal crdt { get; set; }
        public decimal dbt { get; set; }
        public decimal cntrl { get; set; }
    }

    public class querypass
    {
        public string query0 { get; set; }
        public string query1 { get; set; }
        public string query2 { get; set; }
        public string query3 { get; set; }

    }
    public class temptab
    {
        public string col1 { get; set; }
        public string col2 { get; set; }
        public string col3 { get; set; }
        public string col4 { get; set; }
        public string col5 { get; set; }
        public string col6 { get; set; }
        public string col7 { get; set; }
        public string col8 { get; set; }
        public string col9 { get; set; }
        public string col10 { get; set; }
        public string col11 { get; set; }
        public string col12 { get; set; }
        public string col13 { get; set; }
        public string col14 { get; set; }
        public string col15 { get; set; }
        public string col16 { get; set; }
        public string col17 { get; set; }
        public string col18 { get; set; }
        public string col19 { get; set; }
        public string col20 { get; set; }
        public string col21 { get; set; }
        public string col22 { get; set; }
        public string col23 { get; set; }
        public string col24 { get; set; }
        public string col25 { get; set; }
        public string col26 { get; set; }
        public string col27 { get; set; }
        public string col28 { get; set; }
        public string col29 { get; set; }
        public string col30 { get; set; }
       }

    [Serializable]
    public class queryhead
    {
        public string query0 { get; set; }
        public string query1 { get; set; }
        public string query2 { get; set; }
        public string query3 { get; set; }
        public string query4 { get; set; }
        public string query5 { get; set; }
        public string query6 { get; set; }
        public int intquery0 { get; set; }
        public int intquery1 { get; set; }
        public int intquery2 { get; set; }
        public decimal dquery0 { get; set; }
        public decimal dquery1 { get; set; }
        public bool bquery0 { get; set; }
    }

}
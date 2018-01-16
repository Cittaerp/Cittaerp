using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using anchor1.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;

namespace anchor1.Models
{

    public class vw_groupcodes
    {
        [Key]
        public Int64 rowno { get; set; }
        public string wpara { get; set; }
        public string wcode { get; set; }
        public string wname { get; set; }
        public int order1 { get; set; }
    }

    public class vw_grouptrans
    {
        [Key]
        public Int64 rowno { get; set; }
        public string gpara { get; set; }
        public string gcode { get; set; }
        public string gname { get; set; }
        public string codename { get; set; }
    }

    public class vw_grouptranst
    {
        [Key]
        public Int64 rowno { get; set; }
        public string gpara { get; set; }
        public string gcode { get; set; }
        public string gname { get; set; }
        public string codename { get; set; }
    }

    public class vw_stat_name
    {
        [Key]
        public Int64 rowno { get; set; }
        public string indcode { get; set; }
        public string stcode { get; set; }
        public string name1 { get; set; }
        public string type1 { get; set; }
        public string para_code { get; set; }
    }

    public class vw_step
    {
        [Key, Column(Order = 0)]
        [StringLength(10)]
        public string array_code { get; set; }
        [Key, Column(Order = 1)]
        public decimal count_array { get; set; }
        public string amount { get; set; }
    }

    public class vw_collect
    {
        [Display(Name = "Code")]
        public string ws_code { get; set; }
        public string ws_string0 { get; set; }
        public string ws_string1 { get; set; }
        public string ws_string2 { get; set; }
        public string ws_string3 { get; set; }
        public string ws_string4 { get; set; }
        public string ws_string5 { get; set; }
        public string ws_string6 { get; set; }
        public string ws_string7 { get; set; }
        public string ws_string8 { get; set; }
        public string ws_string9 { get; set; }
        public string ws_string10 { get; set; }

        public decimal ws_decimal0 { get; set; }
        public decimal ws_decimal1 { get; set; }
        public decimal ws_decimal2 { get; set; }
        public decimal ws_decimal3 { get; set; }
        public decimal ws_decimal4 { get; set; }
        public decimal ws_decimal5 { get; set; }
        public decimal ws_decimal6 { get; set; }
        public decimal ws_decimal7 { get; set; }
        public decimal ws_decimal8 { get; set; }
        public decimal ws_decimal9 { get; set; }
        public int  ws_int0 { get; set; }
        public int ws_int1 { get; set; }
        public bool ws_bool0 { get; set; }
        public bool ws_bool1 { get; set; }
        public string datmode { get; set; }
        public string imagecat { get; set; }
        public string imagedesc { get; set; }
        public string[] ar_string0 { get; set; }
        public string[] ar_string1 { get; set; }
        public string[] ar_string2 { get; set; }
        public string[] ar_string3 { get; set; }
        public string[] ar_string4 { get; set; }
        public string[] ar_string5 { get; set; }
        public string[] ar_string6 { get; set; }
        public string[] ar_string7 { get; set; }
        public string[] ar_string8 { get; set; }
        public string[] ar_string9 { get; set; }
        public string[] ar_string10 { get; set; }

        public decimal[] ar_decimal0 { get; set; }
        public decimal[] ar_decimal1 { get; set; }
        public decimal[] ar_decimal2 { get; set; }
        public decimal[] ar_decimal3 { get; set; }
        public decimal[] ar_decimal4 { get; set; }
        public decimal[] ar_decimal5 { get; set; }
        public decimal[] ar_decimal6 { get; set; }
        public decimal[] ar_decimal7 { get; set; }
        public decimal[] ar_decimal8 { get; set; }
        public decimal[] ar_decimal9 { get; set; }

        public bool[] ar_bool0 { get; set; }
        public bool[] ar_bool1 { get; set; }
        public bool[] ar_bool2 { get; set; }
        public bool[] ar_bool3 { get; set; }
        public bool[] ar_bool4 { get; set; }
        public bool[,] ar_bool5 { get; set; }
        public DateTime[] ar_date1 { get; set; }
        public string[][] mr_string7 { get; set; }
        public string[][] mr_string8 { get; set; }
        [AllowHtml]
        public string[] tx_string0 { get; set; }
        [AllowHtml]
        public string[] tx_string1 { get; set; }
        [AllowHtml]
        public string[] tx_string2 { get; set; }
        [AllowHtml]
        public string[] tx_string3 { get; set; }
        public int[] ar_int0 { get; set; }
        public int[] ar_int1 { get; set; }
        public int[] ar_int2 { get; set; }
        public int[] ar_int3 { get; set; }
        public byte[] imagedata1 { get; set; }
        public string[,] mt_string0 { get; set; }
        public string[,,,] mt_string1 { get; set; }
        public string[] dsp_string { get; set;}
        public List<vw_query>[] qlist { get; set; }
        public List<SelectListItem> pyears { get; set; }
    }

    public class vw_query
    {
        public string c0 { get; set; }
        public string c1 { get; set; }
        public string c2 { get; set; }
        public string c3 { get; set; }
        public string c4 { get; set; }
        public string c5 { get; set; }
        public string c6 { get; set; }
        public string c7 { get; set; }
        public string c8 { get; set; }
        public string c9 { get; set; }
        public string c10 { get; set; }
        public string c11 { get; set; }
        public string c12 { get; set; }
        public string c13 { get; set; }
        public string c14 { get; set; }
        public string c15 { get; set; }
        public decimal n1 { get; set; }
        public decimal n2 { get; set; }
        public decimal n3 { get; set; }
        public decimal n4 { get; set; }
        public decimal n5 { get; set; }
        public Int32 t1 { get; set; }
        public Int32 t2 { get; set; }
        public DateTime d1 { get; set; }
        public DateTime d2 { get; set; }
    }

    public class rep_column
    {
        public string snumber { get; set; }
        public string column1 { get; set; }
        public string column2 { get; set; }
        public string column3 { get; set; }
        public string column4 { get; set; }
        public string column5 { get; set; }
        public string column6 { get; set; }
        public string column7 { get; set; }
        public string column8 { get; set; }
        public string column9 { get; set; }
        public string column10 { get; set; }
        public string column11 { get; set; }
        public string column12 { get; set; }
        public string column13 { get; set; }
        public string column14 { get; set; }
        public string column15 { get; set; }
        public string column16 { get; set; }
        public string column17 { get; set; }
        public string column18 { get; set; }
        public string column19 { get; set; }
        public string column20 { get; set; }
        public string column21 { get; set; }
        public string column22 { get; set; }
        public string column23 { get; set; }
        public string column24 { get; set; }
        public string column25 { get; set; }
        public string column26 { get; set; }
        public string column27 { get; set; }
        public string column28 { get; set; }
        public string column29 { get; set; }
        public string column30 { get; set; }
        public string column31 { get; set; }
        public string column32 { get; set; }
        public string column33 { get; set; }
        public string column34 { get; set; }
        public string column35 { get; set; }
        public string column36 { get; set; }
        public string column37 { get; set; }
        public string column38 { get; set; }
        public string column39 { get; set; }
        public string column40 { get; set; }
        public int colseq { get; set; }
        public DateTime coldate1 { get; set; }
        public DateTime coldate2 { get; set; }

    }

    public class vw_mcollect
    {
        [Display(Name = "Code")]
        public string ws_code { get; set; }
        public string ws_string0 { get; set; }
        public string ws_string1 { get; set; }
        public string ws_string2 { get; set; }
        public string ws_string3 { get; set; }
        public string ws_string4 { get; set; }
        public string ws_string5 { get; set; }
        public string ws_string6 { get; set; }
        public string ws_string7 { get; set; }
        public string ws_string8 { get; set; }
        public string ws_string9 { get; set; }
        public string ws_string10 { get; set; }
        public decimal ws_decimal0 { get; set; }
        public decimal ws_decimal1 { get; set; }
        public decimal ws_decimal2 { get; set; }
        public decimal ws_decimal3 { get; set; }
        public decimal ws_decimal4 { get; set; }
        public decimal ws_decimal5 { get; set; }
        public decimal ws_decimal6 { get; set; }
        public decimal ws_decimal7 { get; set; }
        public decimal ws_decimal8 { get; set; }
        public decimal ws_decimal9 { get; set; }
        public int ws_int0 { get; set; }
        public bool ws_bool0 { get; set; }
        public string[,] ar_string0 { get; set; }
        public string[,] ar_string1 { get; set; }
        public string[,] ar_string2 { get; set; }
        public string[,] ar_string3 { get; set; }
        public string[,] ar_string4 { get; set; }
        public string[,] ar_string5 { get; set; }
        public string[,] ar_string6 { get; set; }
        public string[,] ar_string7 { get; set; }
        public string[,] ar_string8 { get; set; }
        public string[,] ar_string9 { get; set; }
        public decimal[,] ar_decimal0 { get; set; }
        public decimal[,] ar_decimal1 { get; set; }
        public decimal[,] ar_decimal2 { get; set; }
        public decimal[,] ar_decimal3 { get; set; }
        public decimal[,] ar_decimal4 { get; set; }
        public decimal[,] ar_decimal5 { get; set; }
        public decimal[,] ar_decimal6 { get; set; }
        public bool[,] ar_bool0 { get; set; }
        public bool[,] ar_bool1 { get; set; }
        public bool[,] ar_bool2 { get; set; }
        public string[,] ar_checkbox { get; set; }

    }

    [Serializable]
    public class mysessvals
    {
        public string userid { get; set; }
        public string processing_period { get; set; }
        public string ugroup { get; set; }
        public string ugroup_type { get; set; }
        public DateTime lld { get; set; }       //last login date
        public string datacode { get; set; }    // database code
        public string link_date { get; set; }   //today
        public string type1 { get; set; }       // user type staff or local
        public string type2 { get; set; }       // type of group of access
        public int item_row { get; set; }       // no of items per row
        public string pname { get; set; }       // staff name
        public string sess_con { get; set; }    // no usage
        public string tzone { get; set; }       // client timezone
        public string mcode2 { get; set; }      // default pay cycle
        public string mcode3 { get; set; }      // modules acquire
        public string printer_code { get; set; }    // default printer
        public string p2000 { get; set; }       // user restriction
        public string cquery { get; set; }      // show code in query list
        public string cquery2 { get; set; }     // access invalid or valid
        public string chapp { get; set; }       // payroll approval
        public string hrapp { get; set; }       // hr approval
        public int no_records { get; set; }     // no of max record
        public int drop_lines { get; set; }
        
        //public string prcode { get; set; }

        //public string rt { get; set; }


    }

    public class tab_soft
    {

        [Key, Column(Order = 0)]
        [StringLength(10)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(10)]
        public string report_code { get; set; }

        public string report_name1 { get; set; }
        public string report_name2 { get; set; }
        public string report_name3 { get; set; }
        public string report_name4 { get; set; }
        public string report_name5 { get; set; }
        public string numeric_ind { get; set; }
        public string rep_name1 { get; set; }
        public string rep_name2 { get; set; }
        public string report_name6 { get; set; }
        public string source_cat { get; set; }
        public string report_name7 { get; set; }
        public string report_name8 { get; set; }
        public string report_name9 { get; set; }
        public string report_name10 { get; set; }

    }

    public class vw_user
    {
        [Key, Column(Order = 0)]
        public string type1 { get; set; }
        [Key, Column(Order = 1)]
        public string database_code { get; set; }

        [Key, Column(Order = 2)]
        public string user_code { get; set; }
        public string name1 { get; set; }
        public string user_group { get; set; }
        public string printer_user { get; set; }
        public string color_user { get; set; }
        public string password_user{ get; set; }
        public string password1 { get; set; }
        public string password2 { get; set; }
        public string password3 { get; set; }
        public string password4 { get; set; }
        public string password5 { get; set; }
        public string password6 { get; set; }
        public DateTime password_date { get; set; }
        public string password_question { get; set; }
        public string password_answer { get; set; }
        public string in_flag { get; set; }
        public string locked_flag { get; set; }
        public int count_user { get; set; }
        public string change_flag { get; set; }
        public DateTime last_Login { get; set; }
        public DateTime locked_date { get; set; }
        public string created_by{ get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended{ get; set; }
        public string self_access { get; set; }

    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
//        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string pass1 { get; set; }
        public string pass2 { get; set; }
        public string pass3 { get; set; }
        public string pass4 { get; set; }
        public string pass5 { get; set; }

    }

    public class localpassage
    {
        public string ws_code { get; set; }
        public string ws_string0 { get; set; }
        public string[] ar_string0 { get; set; }
        public decimal[] ar_decimal0 { get; set; }
        public bool[] ar_bool0 { get; set; }
    }

    public class mailmessage
    {
        public string ws_code { get; set; }
        public string ws_string0 { get; set; }
        [AllowHtml]
        public string ws_string1 { get; set; }
        [AllowHtml]
        public string ws_string2 { get; set; }
        [AllowHtml]
        public string ws_string3 { get; set; }

        public string ws_string4 { get; set; }
        public string ws_string5 { get; set; }
    }

    [Serializable]
    public class sideview
    {
        public string ws_code { get; set; }
        public string ws_string0 { get; set; }
        public string ws_string1 { get; set; }
        public string ws_string2 { get; set; }        
    }

    [Serializable]
    public class dataconnect
    {
        public string serialno { get; set; }
        public string company_name { get; set; }
        public string dbase { get; set; }
        public string p1 { get; set; }
        public string dconnectstring { get; set; }
        public string duser { get; set; }
        public string dpassword { get; set; }
        public string dcatalog { get; set; }
        public int dtimeout { get; set; }
        public string duser1 { get; set; }
        public string dpassword1 { get; set; }
    }

    [Serializable]
    public class worksess
    {
        public string err_msg { get; set; }
        public string ptitle { get; set; }
        public string idrep { get; set; }
        public string apentry { get; set; }
        public int xyr { get; set; }
        public string portalname { get; set; }
        public string[] pcar { get; set; }
        public string pc { get; set; }
        public string jp { get; set; }
        public string det { get; set; }
        public string flag_type { get; set; }
        public string vdate { get; set; }
        public string temp0 { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }
        public string temp6 { get; set; }
        public int intval0 { get; set; }
        public string bye_mess { get; set; }
        public string viewflag { get; set; }
        public string filep { get; set; }
        public string[] tarray { get; set; }
        public string atfile { get; set; }

    }

    [Serializable]
    public class fontclass
    {
        public string font_sizep { get; set; }
        public string font_family1 { get; set; }
    }

    public class DataTableData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<vw_collect> data { get; set; }
    }

    public class vw_picture
    {
        public int sequence_no { get; set; }
        public string hcode { get; set; }
        public string pcode { get; set; }
        public string document_type { get; set; }
        public string report_name2 { get; set; }
        public string document_name { get; set; }
        public string comment_code { get; set; }
        public string staff_number { get; set; }
        public string document_code { get; set; }
    }

}
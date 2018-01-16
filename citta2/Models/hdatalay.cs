using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace anchor1v.Models
{

    public class tab_soft
    {

        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
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

    public class tab_database
    {
        [Key, Column(Order = 0)]
        public string database_code { get; set; }

        [Key, Column(Order = 1)]
        public int sequence_no { get; set; }
        
        public string name1 { get; set; }
        public string active { get; set; }
        public string pass_code { get; set; }
        public string default_data { get; set; }
        public string company_name { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }
        public string user_date { get; set; }
        public string data_code { get; set; }
        public string data_code1 { get; set; }
        public string data_code2 { get; set; }
        public string data_code3 { get; set; }
    }

    public class tab_photo_coy
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        public string document_name { get; set; }
        public byte[] picture1 { get; set; }
        public string image_type { get; set; }
        public string internal_use { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
    }

}
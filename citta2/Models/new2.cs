using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp1.Models
{
    public class GB_001_ORG
    {
        [Key, Column(Order = 0)]
        public string org_id { get; set; }
        [Key, Column(Order = 1)]
        public string flag { get; set; }
        public string org_name { get; set; }
        public string rep_name { get; set; }
        public string md_name { get; set; }

        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
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

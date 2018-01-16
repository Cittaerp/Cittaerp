using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CittaErp.Models
{
    public class GB_001_PCT
    {
        [Key, Column(Order = 0)] 
        public string period_number { get; set;}
        public string prd_description { get; set; }
        public string date_from { get; set;}	
        public string date_to { get; set;}
        public DateTime created_date { get; set;}
        public string created_by { get; set;}
        public DateTime modified_date { get; set;}
        public string modified_by { get; set;}
        public string note { get; set;}
    }
    public class GL_001_GLDS
    {
        [Key, Column(Order = 0)]
        public string gl_default_id { get; set; }
        public string acct_type1 { get; set; }
        public string acct_type2 { get; set; }
        public string acct_type3 { get; set; }
        public string acct_type4 { get; set; }
        public string acct_type5 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
    }
    public class AP_001_PUROT
    {
        [Key, Column(Order = 0)]
        public string parameter_code { get; set; }
         [Key, Column(Order = 1)]
        public string sequence_type { get; set; }
        [Key, Column(Order = 2)]
         public string order_type { get; set; }
        public int numeric_size { get; set; }
        public string order_prefix { get; set; }
        public int order_sequence { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
    }
    public class DC_001_DISTS
    {
        [Key, Column(Order = 0)]
        public string discount_selection_basis { get; set; }
        [Key, Column(Order = 1)]
        public string selection_code { get; set; }
        [Key, Column(Order = 2)]
        public string discount_code { get; set; }
        public string discount_flag { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }

    }
    public class MC_001_EXCRT
    {
        [Key, Column(Order = 0)]
        public string currency_code { get; set; }
        [Key, Column(Order = 1)]
        public string date_from { get; set; }
        public string date_to { get; set; }
        public decimal exchange_rate { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }


    }
    public class WF_001_APRDG
    {
        [Key, Column(Order = 0)]
        public int approval_delegation_sequence { get; set; }
        public string delegation_transaction { get; set; }
        public string delegator_employee { get; set; }
        public string delegated_employee { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class GB_999_MSG
    {
        [Key, Column(Order = 0)]
        public string type_msg { get; set; }
        [Key, Column(Order = 1)]
        public string code_msg { get; set; }
        public string name1_msg { get; set; }
        public string name2_msg { get; set; }
        public string name3_msg { get; set; }
        public string name4_msg { get; set; }
        public string name5_msg { get; set; }
        public string name6_msg { get; set; }
    
    }
    public class AP_001_PTERM
    {
        [Key, Column(Order = 0)]
        public string payment_term_code { get; set; }
        public string description { get; set;}
        public int num_of_days { get; set; }
        public DateTime created_date { get; set;}
        public string created_by { get; set;}
        public DateTime modified_date { get; set;}
        public string modified_by { get; set;}
        public string active_status { get; set;}
        public string note { get; set; }
    }
    public class AR_001_CTERM
    {
        [Key, Column(Order = 0)]
        public string credit_term_code { get; set; }
        public string description { get; set; }
        public string delete_flag { get; set; }
        public int num_of_days { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
    }
    public class GL_001_CATEG
    {
        [Key, Column(Order = 0)]
        public string acct_cat_sequence { get; set; }
        public string acct_cat_desc { get; set; }
        public string acct_cat_rpt_group { get; set; }
        public int acct_cat_rpt_sequence { get; set; }
        public string delete_flag { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class GL_001_JONT
    {
        [Key, Column(Order = 0)]
        public string journal_code { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_no { get; set; }
        public string name { get; set; }
        public string journal_type { get; set; }
        public string analysis_code { get; set; }
        public string visibility { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
       
    }
    public class WF_001_WKFL
    {
        [Key, Column(Order = 0)]
        public string approval_group_code { get; set; }
        public string description  { get; set; }
        public string group_member  { get; set; }
        public decimal transaction_minimum_amount  { get; set; }
        public DateTime created_date  { get; set; }
        public string created_by  { get; set; }
        public DateTime modified_date  { get; set; }
        public string modified_by  { get; set; }
        public string active_status  { get; set; }
        public string note  { get; set; }
        public string attach_document { get; set; }

    }
    public class GL_001_CHART
    {
        [Key, Column(Order = 0)]
        public string account_code { get; set; }
        public string account_name { get; set; }
        public string category_name { get; set; }
        public string account_type_code { get; set; }
        public string archiving { get; set; }
        public string delete_flag { get; set; }
        public string currency_code { get; set; }
        public string analysis_code1  { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class GL_001_ATYPE
    {
        [Key, Column(Order = 0)]
        public string acct_type_code { get; set; }
        public string acct_type_desc { get; set; }
        public string acct_cat_sequence { get; set; }
        public int acct_type_rpt_sequence { get; set; }
        public string delete_flag { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class AR_001_DADRS
    {
        [Key, Column(Order = 0)]
        public string customer_code { get; set; }
        [Key, Column(Order = 1)]
        public string address_type {get; set; }
        [Key, Column(Order = 2)]
        public int address_code{ get; set; }
        public string location_alias { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string contact_name { get; set; }
        public string contact_dept { get; set; }
        public string contact_job_title { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class MC_001_CUREN
    {
        [Key, Column(Order = 0)]
        public string currency_code { get; set; }
        public string currency_description { get; set; }
        public string currency_sym { get; set; }
        public string symbol_display { get; set; }
        public string gl_acc_for_unreal { get; set; }
        public string gl_acc_for_real { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set;  }

    }
    public class GB_001_HANAL
    {
        [Key, Column(Order = 0)]
        public string header_sequence { get; set; }
        public string header_description { get; set;  }
        public string delete_flag { get; set; }
        public string items_to_capture { get; set; }

        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class GB_001_RSONC
    {
        [Key, Column(Order = 0)]
        public string consignment_code { get; set; }
        public string description { get; set; }
        public string os_purchase_order { get; set; }
        public string gl_account { get; set; }
        public string currency_code { get; set; }
        public string allocation_basis { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }
    }
    public class GB_001_DANAL
    {
        [Key, Column(Order = 0)]
        public string header_sequence { get; set;  }
        [Key, Column(Order = 1)]
        public string analysis_code { get; set;  }
        public string analysis_description { get; set;  }
        public string date_range_limit { get; set;  }
        public string date_from { get; set; }
        public string date_to { get; set; }
        public DateTime created_date { get; set;  }
        public string created_by { get; set;  }
        public DateTime modified_date { get; set;  }
        public string modified_by { get; set;  }
        public string active_status { get; set;  }
        public string note { get; set;  }
        public string attach_document { get; set; }

    }
    public class AR_001_CUSTM
    {
        [Key, Column(Order = 0)]
        public string customer_code { get; set; }
        public string cust_biz_name { get; set; }
        public string cust_address1 { get; set; }
        public string cust_address2 { get; set; }
        public decimal current_balance { get; set; }
        public string delete_flag { get; set; }
        public string del_same_loc { get; set; }
        public string bil_same_loc { get; set; }
        public string special_discount { get; set; }
        public string business_type { get; set; }
        public string cust_city { get; set; }
        public string cust_postal_code { get; set; }
        public string cust_state { get; set; }
        public string cust_country { get; set; }
        public string cust_email { get; set; }
        public string cust_website { get; set; }
        public string cust_phone { get; set; }
        public string tax_reg_number { get; set; }
        public string biz_reg_number { get; set; }
        public string billing_address1 { get; set; }
        public string billing_address2 { get; set; }
        public string billing_city { get; set; }
        public string billing_postal_code { get; set; }
        public string billing_state { get; set; }
        public string billing_country { get; set; }
        public string billing_email { get; set; }
        public string billing_phone { get; set; }
        public string contact_name { get; set; }
        public string contact_dept { get; set; }
        public string contact_job_title { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        //public string contact_country { get; set; }
        public string currency_code { get; set; }
        public string payment_term_code { get; set; }
        public string payment_matching { get; set; }
        public string credit_facilities { get; set; }
        public decimal credit_limit_amt { get; set; }
        public string payment_method { get; set; }
        public string card_details_number { get; set; }
        public string name_on_card { get; set; }
        public string card_details_maker { get; set; }
        public string card_security_code { get; set; }
        public string card_expiry_date { get; set; }
        public string sales_rep { get; set; }
        public string price_class { get; set; }
        public decimal cust_discount_percent { get; set; }
        public decimal poutstand_instalment_amt { get; set; }
        public int num_pinstalment_outstand { get; set; }
        public decimal pinstalment_paid { get; set; }
        public string gl_account_code { get; set; }
        public string statement_email { get; set; }
        public string other_name { get; set; }
        public string title { get; set; }
        public string sex { get; set; }
        public string marital_status { get; set; }
        public string dob { get; set; }
        public string nationality { get; set; }
        public string emp_address { get; set; }
        public string kin_name { get; set; }
        public string kin_address { get; set; }
        public string kin_phone { get; set; }
        public string purpose { get; set; }
        public string employer { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class AP_001_VENDR
    {
        [Key, Column(Order = 0)]
        public string vendor_code { get; set; }
        public string vend_biz_name { get; set; }
        public string vend_address1 { get; set; }
        public string vend_address2 { get; set; }
        public string business_type { get; set; }
        public string relationship_mgr { get; set; }
        public string vend_city { get; set; }
        public string delete_flag { get; set; }
        public string vend_postal_code { get; set; }
        public string vend_state { get; set; }
        public string vend_country { get; set; }
        public string vend_email { get; set; }
        public string vend_website { get; set; }
        public string vend_phone { get; set; }
        public string tax_reg_number { get; set; }
        public string biz_reg_number { get; set; }
        public string contact_name { get; set; }
        public string contact_dept { get; set; }
        public string contact_job_title { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        public string branch_same_loc { get; set; }
       // public string contact_country { get; set; }
        public string currency_code { get; set; }
        public string payment_term_code { get; set; }
        public string credit_facilities { get; set; }
        public decimal credit_limit_amt { get; set; }
        public decimal current_balance { get; set; }
        public string gl_account_code { get; set; }
        public string statement { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }
     }
    public class GB_001_INTP
    {
        public string parameter_type { get; set; }
        [Key, Column(Order = 0)]
        public int parameter_sequence { get; set; }
        public string description { get; set; }
        public string description2 { get; set; }
        public string description3 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class BK_001_BANK
    {
        [Key, Column(Order = 0)]
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string branch { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string delete_flag { get; set; }
        public decimal current_balance { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string phone_number { get; set; }
        public string bank_acc_number { get; set; }
        public string default_payment_option { get; set; }
        public string relationship_name { get; set; }
        public string relationship_job_title { get; set; }
        public string relationship_email { get; set; }
        public string relationship_phone { get; set; }
        public string currency_code { get; set; }
        public string gl_account_code { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class AR_001_MTRIX
    {
        [Key, Column(Order = 0)]
        public string item_code { get; set; }
        [Key, Column(Order = 1)]
        public string header_sequence { get; set; }
        [Key, Column(Order = 2)]
        public string analysis_code { get; set; }
        public string tax_inclusive { get; set; }
        public decimal selling_price_class1 { get; set; }
        public decimal selling_price_class2 { get; set; }
        public decimal selling_price_class3 { get; set; }
        public decimal selling_price_class4 { get; set; }
        public decimal selling_price_class5 { get; set; }
        public decimal selling_price_class6 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class AR_001_PMTRX
    {
        [Key, Column(Order = 0)]
        public string item_code { get; set; }
        [Key, Column(Order = 1)]
        public int tenor { get; set; }
        [Key, Column(Order = 2)]
        public string currency { get; set; }
        [Key, Column(Order = 3)]
        public string selected_promo { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int auto_id { get; set; }
        public string tax_inclusive { get; set; }
        public decimal selling_price_class1 { get; set; }
        public decimal month_price { get; set; }
        public decimal num_installment { get; set; }
        public decimal installment_amt { get; set; }
        public decimal installment_interval { get; set; }
        public string installment_discrip { get; set; }
        public decimal deposit_flat { get; set; }
        public string property_acquisition { get; set; }
        public decimal deposit_percent { get; set; }
        public decimal interest_rate_pen { get; set; }
        public decimal interest_rate { get; set; }
        public decimal penalty_amt { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
     
    }
    public class FA_001_ASSET
    {
        [Key, Column(Order = 0)]
        public string fixed_asset_code { get; set; }
        public string reference_asset_code { get; set; }
        public string description { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string asset_specs { get; set; }
        public string asset_requires_maintenace { get; set; }
        public string supplier { get; set; }
        public decimal current_balance { get; set; }
        public string asset_user { get; set; }
        public string asset_class { get; set; }
        public string asset_detail { get; set; }
        public byte[] asset_picture { get; set; }
        public string asset_location { get; set; }
        public string department { get; set; }
        public string purchase_date { get; set; }
        public string comm_date { get; set; }
        public string depreciation_type { get; set; }
        public decimal asset_cost { get; set; }
        public decimal revalued_cost { get; set; }
        public int revalued_useful_life { get; set; }
        public string revalued_start_date { get; set; }
        public decimal depreciation_cost { get; set; }
        public decimal net_book_value { get; set; }
        public decimal depreciation_to_date { get; set; }
        public int intial_useful_life { get; set; }
        public string initial_start_date { get; set; }
        public string internal_start_date { get; set; }
        public string internal_end_date { get; set; }
        public string insurance_policy { get; set; }
        public string asset_tag { get; set; }
        public string asset_manufacturers_num { get; set; }
        public decimal residual_value { get; set; }
        public string parent_asset_code { get; set; }
        public string gl_asset_acc_code { get; set; }
        public string gl_accum_depn_code { get; set; }
        public string gl_depn_expense_code { get; set; }
        public string gl_disposal_code { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public string additional_cost { get; set; }
        public string revalue { get; set; }
        public string clearing_account { get; set; }
        public string reserve_account { get; set; }
        public int cumulative_amount { get; set; }
        public string last_maintenance_date { get; set; }
        public string group_type_id { get; set; }
        public string unit_of_reading { get; set; }
        public int required_maintenance_val { get; set; }
        public string group_flag { get; set; }
        public string asset_nature { get; set; }
        public string dispose_asset { get; set; }
        public String disposal_date { get; set; }
        public string disposed { get; set; }
        public decimal depreciation_rate { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class GB_001_EMP
    {
        [Key, Column(Order=0)]
        public string employee_code { get; set; }
        public string name { get; set; }
        [Column(TypeName = "image")]
        public byte[] emp_photo { get; set; }
        public string department { get; set; }
        public int job_role { get; set; }
        public string email { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public decimal current_balance { get; set; }
        public string address_home { get; set; }
        public string city { get; set; }
        public string bank_acc { get; set; }
        public string close_code { get; set; }
        public string bank_code { get; set; }
        public string country { get; set; }
        public string commission_policy { get; set; }
        public string gl_commission_pay { get; set; }
        public string gl_iou_adv { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public String created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class FA_001_ASSETM
    {
        [Key, Column(Order = 0)]
        public string fixed_asset_code { get; set; }
        public string reference_asset_code { get; set; }
        public string description { get; set; }
    }
    public class AR_001_STRAN
    {
        [Key, Column(Order = 0)]
        public string order_code { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_no { get; set; }
        
        public string order_name { get; set; }
        public string quote { get; set; }
        public int quote_sequence { get; set; }
        public string order { get; set; }
        public int order_sequence { get; set; }
        public string confirm_order { get; set; }
        public string reserve_stock { get; set; }
        public string waybill { get; set; }
        public int waybill_sequence { get; set; }
        public string invoice { get; set; }
        public int invoice_sequence { get; set; }
        public string tax_invoice1 { get; set; }
        public string tax_invoice2 { get; set; }
        public string header_code { get; set; }
        public string mandatory_flag { get; set; }
        //public string analysis_code3 { get; set; }
        //public string analysis_code4 { get; set; }
        //public string analysis_code5 { get; set; }
        //public string analysis_code6 { get; set; }
        //public string analysis_code7 { get; set; }
        //public string analysis_code8 { get; set; }
        //public string analysis_code9 { get; set; }
        //public string analysis_code10 { get; set; }
        public  DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class GB_001_DOC
    {
        public string screen_code { get; set; }
        public string document_code { get; set; }
        [Key, Column(Order = 0)]
        public int document_sequence { get; set; }
        [Column(TypeName = "image")]
        public byte[] document_image { get; set; }
        public string description { get; set; }
        public string document_type { get; set; }
        public string document_name { get; set; }

    }
    public class GB_001_PCODE
    {
        [Key, Column(Order = 0)]
        public string parameter_type { get; set; }
        [Key, Column(Order = 1)]
        public string parameter_code { get; set; }
        public string parameter_name { get; set; }
        public string gl_account_code { get; set; }
        public string con_state_link { get; set; }
        public string delete_flag { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class IV_001_WAREH
    {
        [Key, Column(Order = 0)]
        public string warehouse_code { get; set; }
        public string warehouse_name { get; set; }
        public string site_code { get; set; }
        public string branch_address_code { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string default_warehouse { get; set; }
        public decimal current_balance { get; set; }
        public string contact_phone { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class GB_001_HEADER
    {
        [Key, Column(Order = 0)]
        public string header_type_code { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_no { get; set; }
        public string header_code { get; set; }
        public string mandatory_flag { get; set; }
        public string delete_flag { get; set; }
        public string note { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
    }
    public class DC_001_DISC
    {
        [Key, Column(Order = 0)]
        public string discount_code { get; set; }
         [Key, Column(Order = 1)]
        public int discount_count { get; set; }
        public string discount_name { get; set; }
        public decimal discount_percent { get; set; }
        public decimal discount_amount { get; set; }
        public int upper_limit { get; set; }
        public int lower_limit { get; set; }
        public decimal qualified_quantity { get; set; }
        public decimal qualified_amount { get; set; }
        public string time_bound { get; set; }
        public string stepped_criteria { get; set; }
        public string promo_criteria { get; set; }
        public string stepped_discount_active { get; set; }
        public string discount_date_from { get; set; }
        public string discount_date_to { get; set; }
        public string discount_gl_acc { get; set; }
        public string gift_code { get; set; }
        public decimal gift_qty { get; set; }
        public string gl_gift { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class AP_001_PTRAN
    {
        [Key, Column(Order = 0)]
        public string purchase_order_code { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_no { get; set; }
        public string purchase_order_name { get; set; }
        public string purchase_order_class { get; set; }
        public string purchase_requisition { get; set; }
        public int purchase_requisition_sequence { get; set; }
        public string request_for_quote { get; set; }
        public int request_for_quote_sequence { get; set; }
        public string purchase_order { get; set; }
        public int purchase_order_sequence { get; set; }
        public string quarantine_receipt { get; set; }
        public int quarantine_receipt_sequence { get; set; }
        public string inspection_certification { get; set; }
        public int inspection_certification_sequence { get; set; }
        public string grn { get; set; }
        public int grn_sequence { get; set; }
        public string fixed_asset_register { get; set; }
        public int fixed_asset_register_sequence { get; set; }
        public string vendor_invoice_booking { get; set; }
        public int vendor_invoice_booking_sequence { get; set; }
        public string tax_invoice1 { get; set; }
        public string tax_invoice2 { get; set; }
        public string header_code { get; set; }
        public string mandatory_flag { get; set; }
        //public string analysis_code3 { get; set; }
        //public string analysis_code4 { get; set; }
        //public string analysis_code5 { get; set; }
        //public string analysis_code6 { get; set; }
        //public string analysis_code7 { get; set; }
        //public string analysis_code8 { get; set; }
        //public string analysis_code9 { get; set; }
        //public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class GB_001_COY1
    {
        [Key, Column(Order = 0)]
        public string company_code { get; set; }
        public string parent_company_code { get; set; }
        public string company_name { get; set; }
        public int logo_size { get; set; }
        public byte [] company_logo { get; set; }
        public string business_category { get; set; }
        public string business_reg_number { get; set; }
        public string contact_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string country { get; set; }
        public string country_of_operation { get; set; }
        public string state { get; set; }
        public string website { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string multi_currency { get; set; }
        public string base_currency_code { get; set; }
        public string exchange_rate_ratio { get; set; }
        public string we_sell { get; set; }
        public string financial_year_mth_end { get; set; }
        public string period_closing_basis { get; set; }
        public string period_calender_code { get; set; }
        public decimal maximum_number_periods { get; set; }
        public string open_period_from { get; set; }
        public string open_period_to { get; set; }
        public string standard_chart { get; set; }
        public string credit_grant { get; set; }
        public string credit_policy { get; set; }
        public string credit_control_basis { get; set; }
        public string allow_neg_inventory_bal { get; set; }
        public string allow_manual_discount { get; set; }
        public string price_class1 { get; set; }
        public string price_class2 { get; set; }
        public string price_class3 { get; set; }
        public string price_class4 { get; set; }
        public string price_class5 { get; set; }
        public string price_class6 { get; set; }
        public string allow_price_edit { get; set; }
        public DateTime expiry_date { get; set; }
        //public string quote_prefix { get; set; }
        //public decimal sales_order_sequence { get; set; }
        //public string sales_order_prefix { get; set; }
        //public decimal invoice_sequence { get; set; }
        //public string invoice_prefix { get; set; }
        //public decimal waybill_sequence { get; set; }
        //public string waybill_prefix { get; set; }
        //public decimal payment_sequence { get; set; }
        //public string payment_prefix { get; set; }
        //public decimal item_return_sequence { get; set; }
        //public string item_return_prefix { get; set; }
        //public string journal_prefix { get; set; }
        //public decimal journal_number_sequence { get; set; }
        public string workflow_notification { get; set; }
        public string workflow_time_limit { get; set; }
        public string workflow_escalation { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class GB_001_COY
    {
        [Key, Column(Order = 0)]
        public string id_code { get; set; }
        public string field1{ get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string field10 { get; set; }
        public string field11 { get; set; }
        public string field12 { get; set; }
        public string field13 { get; set; }
        public string field14{ get; set; }
        public string field15 { get; set; }
        public string field16 { get; set; }
        public byte[] company_logo { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        
    }
    public class IV_001_ITEM
    {
        [Key, Column(Order = 0)]
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_type { get; set; }
        public string description { get; set; }
         [Column(TypeName = "image")]
        public byte[] item_picture { get; set; }
        public string item_group { get; set; }
        public string tax_inclusive { get; set; }
        public string tax { get; set; }
        public decimal current_balance { get; set; }
        public decimal selling_price_class1 { get; set; }
        public decimal selling_price_class2 { get; set; }
        public decimal selling_price_class3 { get; set; }
        public decimal selling_price_class4 { get; set; }
        public decimal selling_price_class5 { get; set; }
        public decimal selling_price_class6 { get; set; }
        public string price_matrix { get; set; }
        public string header_sequence { get; set; }
        public string sku_sequence { get; set; }
        public string specification { get; set; }
        public decimal weight_per_sku { get; set; }
        public decimal reorder_level { get; set; }
        public decimal minimum_stock { get; set; }
        public decimal maximum_stock { get; set; }
        public decimal reorder_quantity { get; set; }
        public decimal exchange_rate { get; set; }
        public int standard_lead_time { get; set; }
        public string item_group_maintenance { get; set; }
        public string currency { get; set; }
        public string item_costing_method { get; set; }
        public string issurance_method { get; set; }
        public decimal average_cost { get; set; }
        public decimal standard_cost { get; set; }
        public decimal last_purchase_cost { get; set; }
        public string last_purchase_date { get; set; }
        public string last_purchase_vendor { get; set; }
        public string serial_number { get; set; }
        public string discount_code { get; set; }
        public string gl_price_var_code { get; set; }
        public string gl_inv_code { get; set; }
        public string gl_income_code { get; set; }
        public string gl_cos_code { get; set; }
        public string gl_stockcount_variance_code { get; set; }
        public string gl_property { get; set; }
        public string gl_comm_exp { get; set; }
        public string gl_def_com_exp { get; set; }
        public string title_ref_des { get; set; }
        public string preferred_vendor { get; set; }
        public string tax_code1 { get; set; }
        public string tax_code2 { get; set; }
        public string tax_code3 { get; set; }
        public string tax_code4 { get; set; }
        public string tax_code5 { get; set; }
        public string maintanance { get; set; }
        public string sales { get; set; }
        public string purchases { get; set; }
        public string production { get; set; }
        public string consumables { get; set; }
        public string reusable { get; set; }
        public string marketing { get; set; }
        public string title_ref_num { get; set; }
        public string location_address { get; set; }
        public string property_type { get; set; }
        public string product_manager { get; set; }
        public decimal agency_comm_flat { get; set; }
        public decimal agency_comm_per { get; set; }
        public decimal Contact_sales_value { get; set; }
        public decimal num_installment { get; set; }
        public decimal installment_amt { get; set; }
        public decimal installment_interval { get; set; }
        public string installment_discrip { get; set; }
        public decimal deposit_flat { get; set; }
        public string property_acquisition { get; set; }
        public decimal owner_transfer { get; set; }
        public decimal deposit_percent { get; set; }
        public decimal interest_rate_pen { get; set; }
        public decimal interest_rate { get; set; }
        public decimal penalty_amt { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }
     }
    public class IV_001_ITMST
    {
        [Key, Column(Order = 0)]
        public string item_code { get; set; }
        [Key, Column(Order = 1)]
        public string warehouse { get; set; }
        public decimal reorder_level { get; set; }
        public int bal_qty { get; set; }
        public string bin_location { get; set; }
        public decimal minimum_stock { get; set; }
        public decimal maximum_stock { get; set; }
        public decimal reorder_quantity { get; set; }
        public int standard_lead_time { get; set; }
        public decimal average_cost { get; set; }
        public decimal standard_cost { get; set; }
        public decimal last_purchase_cost { get; set; }
        public DateTime last_purchase_date { get; set; }
        public string last_purchase_vendor { get; set; }
        public string serial_number { get; set; }
        public string discount_code { get; set; }
        public string gl_price_var_code { get; set; }
        public string gl_inv_code { get; set; }
        public string gl_income_code { get; set; }
        public string gl_cos_code { get; set; }
        public string gl_stockcount_variance_code { get; set; }
        public string preferred_vendor { get; set; }
        public string tax_code1 { get; set; }
        public string tax_code2 { get; set; }
        public string tax_code3 { get; set; }
        public string tax_code4 { get; set; }
        public string tax_code5 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
    }
    public class GB_001_ITMG
    {
        [Key, Column(Order = 0)]
        public string item_group_id { get; set; }
        public string item_type { get; set; }
        public string item_group { get; set; }
        public string item_group_maintenance { get; set; }
        public string tax { get; set; }
        public string sku_sequence { get; set; }
        public string item_costing_method { get; set; }
        //public string issurance_method { get; set; }
        //public decimal average_cost { get; set; }
        //public decimal standard_cost { get; set; }
        public string gl_price_var_code { get; set; }
        public string gl_inv_code { get; set; }
        public string gl_income_code { get; set; }
        public string gl_cos_code { get; set; }
        public string gl_stockcount_variance_code { get; set; }
        public string preferred_vendor { get; set; }
        public string tax_code1 { get; set; }
        public string tax_code2 { get; set; }
        public string tax_code3 { get; set; }
        public string tax_code4 { get; set; }
        public string tax_code5 { get; set; }
        public string maintanance { get; set; }
        public string sales { get; set; }
        public string purchases { get; set; }
        public string production { get; set; }
        public string consumables { get; set; }
        public string reusable { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
    }
    public class AR_002_QUOTE
    {
        [Key, Column(Order = 0)]
        public int sale_sequence_number { get; set; }
        [Key, Column(Order = 1)]
        public int line_sequence { get; set; }
        [Key, Column(Order = 2)]
        public int sub_line_sequence { get; set; }
        public string quote_reference { get; set; }
        public string dis_flag { get; set; }
        public string customer_code { get; set; }
        public string item_code { get; set; }
        public decimal quote_qty { get; set; }
        public decimal price { get; set; }
        public decimal price_rate { get; set; }
        public decimal ext_price { get; set; }
        public decimal discount_percent { get; set; }
        public decimal discount_amount { get; set; }
        public decimal actual_discount { get; set; }
        public decimal net_amount { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax_amount1 { get; set; }
        public decimal tax_amount2 { get; set; }
        public decimal tax_amount3 { get; set; }
        public decimal tax_amount4 { get; set; }
        public decimal tax_amount5 { get; set; }
        public decimal tax_invoiceamt1 { get; set; }
        public decimal tax_invoiceamt2 { get; set; }
        public decimal quote_amount { get; set; }
        public decimal base_amount { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
       
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class AR_002_SALES
    {
        [Key, Column(Order = 0)]
        public int sale_sequence_number { get; set; }
        public string sales_transaction_type { get; set; }
        public string customer_code { get; set; }
        public string quote_reference { get; set; }
        public string order_reference { get; set; }
        public string invoice_reference { get; set; }
        public int status { get; set; }
        public int number_of_day { get; set; }
        public string currency_code { get; set; }
        public decimal exchange_rate { get; set; }
        public string exchange_rate_mode { get; set; }
        public string transaction_date { get; set; }
        public string quote_date { get; set; }
        public string quote_expiration_date { get; set; }
        public string order_date { get; set; }
        public decimal order_total_inv_tax { get; set; }
        public string order_expiration_date { get; set; }
        public string cust_order_number { get; set; }
        public string expected_delivery_date { get; set; }
        public string delivery_address_code { get; set; }
        public string item_warehouse_code { get; set; }
       // public decimal item_discount { get; set; }
        public string payment_term_code { get; set; }
        public decimal quote_total_qty { get; set; }
        public decimal quote_total_ext_price { get; set; }
        public decimal quote_total_discount { get; set; }
        public decimal quote_total_tax { get; set; }
        public decimal quote_total_amount { get; set; }
        public decimal order_total_ext_price { get; set; }
        public decimal order_total_discount { get; set; }
        public decimal quote_total_inv_tax { get; set; }
        public decimal order_total_tax { get; set; }
        public decimal order_total_qty { get; set; }
        public decimal order_total_amount { get; set; }
        public decimal invoice_total_ext_price { get; set; }
        public decimal invoice_total_discount { get; set; }
        public decimal invoice_total_tax { get; set; }
        public decimal invoice_total_amount { get; set; }
        public string project_code { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class GB_001_TAX
    {
        [Key, Column(Order = 0)]
        public string tax_code { get; set; }
        public string tax_name { get; set; }
        public decimal tax_rate { get; set; }
        public string tax_reg_id { get; set; }
        public string tax_agency { get; set; }
        public string tax_impact { get; set; }
        public string module_basis { get; set; }
        public string computation_basis { get; set; }
        public string reclaimable { get; set; }
        public string salestax_payable_recognition { get; set; }
        public string salestax_paytime { get; set; }
        public string purchasetax_payable_recognition { get; set; }

        public string gl_tax_acc_code { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; } 

    }
    public class IC_001_INCOY
    {
        [Key, Column(Order = 0)]
        public string intercoy_code { get; set; }
        public string intercoy_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string phone_number { get; set; }
        public string relationship_name { get; set; }
        public string job_title { get; set; }
        public string relationship_email { get; set; }
        public string phone { get; set; }
        public string currency_code { get; set; }
        public string gl_account_code { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class AP_002_VTRAN
    {
        [Key, Column(Order = 0)]
        public int document_number { get; set; }
        public string module_account_type { get; set; }
        public string transaction_code { get; set; }
        public string reference_number { get; set; }
        public string transaction_type { get; set; }
        public string batch_information { get; set; }
        public string transaction_date { get; set; }
        public string period { get; set; }
        public decimal total_amount { get; set; }
        public decimal processed { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class AP_002_VTRAD
    {
        [Key, Column(Order = 0)]
        public int document_number { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_number { get; set; }
        public string module_account_type { get; set; }
        public string transaction_code { get; set; }
        public string transaction_date { get; set; }
        public string transaction_type { get; set; }
        public string description { get; set; }
        public string account_type { get; set; }
        public string account_code { get; set; }
        public string period { get; set; }
        public string reference_number { get; set; }
        public decimal amount { get; set; }
        public decimal exchange_rate { get; set; }
        public string amount_type { get; set; }
        public string currency { get; set; }
        public decimal base_amount { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class AP_002_PTRN
    {
        [Key, Column(Order = 0)]
        public int document_number { get; set; }
        public string module_account_type { get; set; }
        public string transaction_code { get; set; }
        public string reference_number { get; set; }
        public string transaction_type { get; set; }
        public string batch_information { get; set; }
        public string transaction_date { get; set; }
        public string period { get; set; }
        public decimal exchange_rate { get; set; }
        public decimal total_amount { get; set; }
        public string project_code { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public string attach_document { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class AP_002_PTRND
    {
        [Key, Column(Order = 0)]
        public int document_number { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_number { get; set; }
        public string module_account_type { get; set; }
        public string transaction_code { get; set; }
        public string transaction_date { get; set; }
        public string transaction_type { get; set; }
        public string description { get; set; }
        public string account_type { get; set; }
        public string account_code { get; set; }
        public string contract_id { get; set; }
        public string reference_number { get; set; }
        public decimal amount { get; set; }
        public decimal exchange_rate { get; set; }
        public string amount_type { get; set; }
        public string currency { get; set; }
        public decimal base_amount { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class GL_002_JONAL
    {
        [Key, Column(Order = 0)]
        public int journal_number { get; set; }
        public string manual_reference_number { get; set; }
        public string journal_type { get; set; }
        public string batch_description { get; set; }
        public decimal exchange_rate { get; set; }
        public int number_of_cycle { get; set; }
        public string control_flag { get; set; }
        public int cycle_interval { get; set; }
        public string reversing_date { get; set; }
        public string period { get; set; }
        public string year { get; set; }
        public string approval_mode { get; set; }
        public string rev_approval_mode { get; set; }
        public decimal total_debit { get; set; }
        public decimal total_credit { get; set; }
        public decimal control { get; set; }
        public string project_code { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public string attach_document { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class GL_002_JONAD
    {
        [Key, Column(Order=0)]
        public int journal_number { get; set; }
         [Key, Column(Order = 1)]
        public int sequence_number { get; set; }
        public string account_type_debit { get; set; }
        public string account_code_debit { get; set; }
        public string description { get; set; }
        public string reference_number { get; set; }
        public decimal amount { get; set; }
        public string amount_type { get; set; }
        public string transaction_date { get; set; }
        public string asset_class { get; set; }
        public string fixed_asset_code { get; set; }
        public string fa_transaction_type { get; set; }
        public string currency { get; set; }
        public decimal exchange_rate { get; set; }
        public decimal base_amount { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class GL_002_JONAR
    {
        [Key, Column(Order = 0)]
        public int journal_number { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_number { get; set; }
        public string account_type_debit { get; set; }
        public string account_code_debit { get; set; }
        public string description { get; set; }
        public string batch_description { get; set; }
        public int number_of_cycle { get; set; }
        public int cycle_interval { get; set; }
        public string reversing_date { get; set; }
        public string period { get; set; }
        public string year { get; set; }
        public string reference_number { get; set; }
        public decimal amount { get; set; }
        public string amount_type { get; set; }
        public string transaction_date { get; set; }
        public string asset_class { get; set; }
        public string fixed_asset_code { get; set; }
        public string fa_transaction_type { get; set; }
        public string currency { get; set; }
        public decimal exchange_rate { get; set; }
        public decimal base_amount { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class AP_002_ADOEN
    {
         [Key, Column(Order=0)]
        public int sequence_number { get; set; }
        public int journal_number { get; set; }
        public string account_code { get; set; }
        public string intercoy_code { get; set; }
        public string description { get; set; }
        public string source_identifier { get; set; }
        public string transaction_date { get; set; }
        public string period { get; set; }
        public string year { get; set; }
        public decimal exchange_rate { get; set; }
        public string currency_code { get; set; }
        public string reference_number_detail { get; set; }
        public string reference_number { get; set; }
        public decimal amount { get; set; }
        public decimal base_amount { get; set; }
        public string debit_credit_code { get; set; }
        public string project_code { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class BK_002_BANKR
    {
        [Key, Column(Order=0)]
        public string bank_code { get; set; }
        public string transaction_date { get; set; }
        public int reference_number { get; set; }
        public string description { get; set; }
        public decimal debit_amount { get; set; }
        public decimal credit_amount { get; set; }
        public string value_date { get; set; }
        public string post_checkbox { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class BU_002_BUGTH
    {
        [Key, Column(Order=0)]
        public int budget_journal_number { get; set; }
        public string budget_reference_number { get; set; }
        public string batch_description { get; set; }
        public string period { get; set; }
        public string transaction_date { get; set; }
        public string control_flag { get; set; }
        public decimal total_debit { get; set; }
        public decimal total_credit { get; set; }
        public decimal control { get; set; }
        public string project_code { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public string attach_document { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class BU_002_BUGTD
    { 
        [Key, Column(Order=0)]
        public int budget_journal_number { get; set; }
        [Key, Column(Order=1)]
        public int sequence_number { get; set; }
        public string account_type_debit { get; set; }
        public string account_code_debit { get; set; }
        public string description { get; set; }
        public decimal exchange_rate { get; set; }
        //public string account_code_credit { get; set; }
        public string reference_number { get; set; }
        public decimal amount { get; set; }
        public string currency_code { get; set; }
        public string amount_type { get; set; }
        //public decimal net_amount { get; set; }
        public decimal base_amount { get; set; }
       // public string analysis_code { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class BU_002_BUGTR
    {
        [Key, Column(Order = 0)]
        public int budget_journal_number { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_number { get; set; }
        public string account_type_debit { get; set; }
        public string account_code_debit { get; set; }
        public string description { get; set; }
        public decimal exchange_rate { get; set; }
        public string period { get; set; }
        public string year { get; set; }
        public string reference_number { get; set; }
        public decimal amount { get; set; }
        public string currency_code { get; set; }
        public string amount_type { get; set; }
        public string batch_description { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public decimal base_amount { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }  
    public class IV_002_INVTY
    {
        [Key, Column(Order=0)]
        public int inventory_sequence_number { get; set; }
        public string transaction_type { get; set; }
        public string manual_reference { get; set; }
        public string description { get; set; }
        public string transaction_date { get; set; }
        public string period { get; set; }
        public string year { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class IV_002_IVTYD
    {
        [Key, Column(Order=0)]
        public int inventory_sequence_number { get; set; }
        [Key, Column(Order=1)]
        public int sequence_number { get; set; }
        public string reference_number { get; set; }
        public string item_code { get; set; }
        public string description { get; set; }
        public decimal quantity { get; set; }
        public decimal unit_cost { get; set; }
        public decimal extended_cost { get; set; }
        public string reason_code { get; set; }
        public string warehouse_from { get; set; }
        public string bin_location_from { get; set; }
        public string transaction_type { get; set; }
        //public string warehouse_to { get; set; }
        public string bin_location_to { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class IV_002_COUNT
    {
        [Key, Column(Order=0)]
        public string stock_sheet_number { get; set; }
        public string stock_count_date { get; set; }
        public string period { get; set; }
        public string warehouse_code { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class IV_002_CONTD
    {
        [Key, Column(Order = 0)]
        public string stock_sheet_number { get; set; }
        [Key, Column(Order = 1)]
        public string item_code { get; set; }
        public int quantity { get; set; }
        public int physical_count { get; set; }
        public int difference { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class MC_002_REVAL
    {
        [Key, Column(Order=0)]
        public string  reference_number { get; set; }
        public string currency_code { get; set; }
        public string posting_date { get; set; }
        public string account_code { get; set; }
        public decimal base_balance { get; set; }
        public decimal current_balance { get; set; }
        public decimal closing_exchange_rate { get; set; }
        public decimal base_revalued_balance { get; set; }
        public decimal unrealised_gain_loss { get; set; }
        public string auto_reversal_date { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string attach_document { get; set; }

    }
    public class AR_002_SODT
    {
        [Key, Column(Order = 0)]
        public int sale_sequence_number { get; set; }
        [Key, Column(Order = 1)]
        public int line_sequence { get; set; }
        [Key, Column(Order = 2)]
        public int sub_line_sequence { get; set; }
        public string sales_order_sequence { get; set; }
        public string customer_code { get; set; }
        public string item_code { get; set; }
        public decimal quote_qty { get; set; }
        public string item_warehouse_code { get; set; }
        public decimal pick_quantity { get; set; }
        public decimal price_rate { get; set; }
        public decimal price { get; set; }
        public decimal tax_invoice { get; set; }
        public string dis_flag { get; set; }
        public decimal ext_price { get; set; }
        public decimal discount_percent { get; set; }
        public decimal discount_amount { get; set; }
        public decimal actual_discount { get; set; }
        public decimal net_amount { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax_amount1 { get; set; }
        public decimal tax_amount2 { get; set; }
        public decimal tax_amount3 { get; set; }
        public decimal tax_amount4 { get; set; }
        public decimal tax_amount5 { get; set; }
        public decimal tax_invoiceamt1 { get; set; }
        public decimal tax_invoiceamt2 { get; set; }
        public decimal quote_amount { get; set; }
        public decimal base_amount { get; set; }
        //public DateTime order_date { get; set; }
        public string include_tax { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
    }
    public class AP_002_POD
    {
        [Key, Column(Order = 0)]
        public int purchase_sequence_number { get; set; }
        [Key, Column(Order = 1)]
        public int line_sequence { get; set; }
        [Key, Column(Order = 2)]
        public int sub_line_sequence { get; set; }
        public string purchase_order_sequence { get; set; }
        public string vendor_code { get; set; }
        public string item_code { get; set; }
        public decimal order_quantity { get; set; }
        public string item_warehouse_code { get; set; }
        public string additional_description { get; set; }
        public decimal delivery_quantity { get; set; }
        public decimal price_rate { get; set; }
        public decimal price { get; set; }
        //public decimal tax_invoice { get; set; }
        //public string dis_flag { get; set; }
        public decimal extended_price { get; set; }
        //public decimal discount_percent { get; set; }
        //public decimal discount_amount { get; set; }
       // public decimal actual_discount { get; set; }
        public decimal net_amount { get; set; }
        public decimal wht { get; set; }
        public decimal net_payable_amount { get; set; }
       // public string order_date { get; set; }
        //public decimal tax_amount3 { get; set; }
        //public decimal tax_amount4 { get; set; }
        //public decimal tax_amount5 { get; set; }
        //public decimal tax_invoiceamt1 { get; set; }
        //public decimal tax_invoiceamt2 { get; set; }
        public decimal order_amount { get; set; }
        public decimal base_amount { get; set; }
        //public DateTime order_date { get; set; }
        public string include_tax { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
    }
    public class AR_002_INVCE
    {
        [Key, Column(Order=0)] 
        public int sale_sequence_number { get; set; }
        [Key, Column(Order = 1)]
        public int line_sequence { get; set; }
        public string invoice_sequence { get; set; }
        public string item_code { get; set; }
        public string waybill_sequence { get; set; }
        public string customer_code { get; set; }
        //public DateTime invoice_date { get; set; }
        //public DateTime waybill_date { get; set; }
        public decimal invoice_quantity { get; set; }
        public decimal order_quantity { get; set; }
        public string item_warehouse_code { get; set; }
        public decimal price { get; set; }
        public decimal ext_price { get; set; }
        public decimal discount_percent { get; set; }
        public decimal discount_amount { get; set; }
        public decimal actual_discount { get; set; }
        public decimal net_amount { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax_amount1 { get; set; }
        public decimal tax_amount2 { get; set; }
        public decimal tax_amount3 { get; set; }
        public decimal tax_amount4 { get; set; }
        public decimal tax_amount5 { get; set; }
        public decimal invoice_amount { get; set; }
        public decimal base_amount { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class AR_002_CUSIR
    {
        [Key, Column(Order = 0)]
        public int return_sequence { get; set; }
        [Key, Column(Order = 1)]
        public string reference_number { get; set; }
        public string item_code { get; set; }
        public string waybill_sequence { get; set; }
        public DateTime invoice_date { get; set; }
        public DateTime waybill_date { get; set; }
        public decimal invoice_quantity { get; set; }
        public string item_warehouse_code { get; set; }
        public decimal price { get; set; }
        public decimal ext_price { get; set; }
        public decimal discount_percent { get; set; }
        public decimal discount_amount { get; set; }
        public decimal net_amount { get; set; }
        public decimal tax_amount { get; set; }
        public decimal invoice_amount { get; set; }
        public decimal base_amount { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class GB_001_PST
    {
        [Key, Column(Order = 0)]
        public string period_sel_code { get; set; }
        public decimal maximum_number_periods { get; set; }
        public string open_period_from { get; set; }
        public string open_period_to { get; set; }
        public string financial_year_mth_end { get; set; }
        public string period_closing_basis { get; set; }
        public string current_period { get; set; }
        public string posting_waiver { get; set; }
        public string expiring_date { get; set; }
        public string delete_flag { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class GB_001_LOGIN
    {
        [Key, Column(Order = 0)]
        public string username { get; set; }
        public string password { get; set; }
    }
    public class GB_001_TAGY
    {
        [Key, Column(Order = 0)]
        public string tagency_id { get; set; }
        public string agency { get; set; }
        public string item_group { get; set; }
        public decimal rate { get; set; }
    }
    public class FA_001_INSUR
    {
        [Key, Column(Order = 0)]
        public string insurance_policy_id { get; set; }
        public string description { get; set; }
        public string underwriter { get; set; }
        public string insurance_broker { get; set; }
        public int policy_cover_num { get; set; }
        public string effective_date { get; set; }
        public string expiry_date { get; set; }
        public decimal sum_insured { get; set; }
        public string premuim { get; set; }
        public decimal premuim_rate { get; set; }
        public string renewal_reminder_date { get; set; }
        public string insured_asset_comment { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
  
    }
    public class GB_001_ITMUS
    {
        [Key, Column(Order = 0)]
        public string item_usage_id { get; set; }
        public string item_usage_name { get; set; }
        public string spare_part { get; set; }
        public string sales { get; set; }
        public string purchases { get; set; }
        public string production { get; set; }
        public string consumables { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
  
    }
    public class IV_002_PCNT
    {
        [Key, Column(Order = 0)]
        public string property_id { get; set; }
        [Key, Column(Order = 1)]
        public string contract_id { get; set; }
        public string contract_description { get; set; }
        public string contract_type { get; set; }
        public string transaction_date { get; set; }
        public string sales_rep { get; set; }
        public string customer_id { get; set; }
        public string product { get; set; }
        public decimal dep_amt_paid { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }
        public decimal sales_com { get; set; }
        public decimal sales_val { get; set; }
        public decimal monthly_amt { get; set; }
        public decimal num_payment { get; set; }
        public string effect_period { get; set; }
        public string effect_year { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class IV_001_PC
    {
        public string property_id { get; set; }
        [Key, Column(Order = 1)]
        public string contract_id { get; set; }
        public string other_name { get; set; }
        [Column(TypeName = "image")]
        public byte[] cus_photo { get; set; }
        public string transaction_date { get; set; }
        public string sales_rep { get; set; }
        public string customer_id { get; set; }
        public string item_code { get; set; }
        public decimal current_balance { get; set; }
        public decimal gift_min_amount { get; set; }
        public decimal commission_balance { get; set; }
        public decimal commission_paid { get; set; }
        public string leeds_name { get; set; }
        public string selected_promo { get; set; }
        public string gift_flag { get; set; }
        public int tenor { get; set; }
        public decimal price { get; set; }
        public decimal exp_deposit { get; set; }
        public string termination_charge { get; set; }
        public decimal quantity { get; set; }
        public decimal sales_com { get; set; }
        public decimal sales_val { get; set; }
        public decimal net_sales_val { get; set; }
        public decimal monthly_amt { get; set; }
        public decimal exchange_rate { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public string title { get; set; }
        public string sex { get; set; }
        public string business_type { get; set; }
        public string marital_status { get; set; }
        public string dob { get; set; }
        public string nationality { get; set; }
        public string address { get; set; }
        public string cust_email { get; set; }
        public string purpose { get; set; }
        public string kin_phone { get; set; }
        public string emp_address { get; set; }
        public string kin_name { get; set; }
        public string kin_address { get; set; }
        public string full_payment { get; set; }
        public string gift1 { get; set; }
        public string gift2 { get; set; }
        public string gift3 { get; set; }
        public string gift4 { get; set; }
        public string gift5 { get; set; }
        public decimal gift_qty1 { get; set; }
        public decimal gift_qty2 { get; set; }
        public decimal gift_qty3 { get; set; }
        public decimal gift_qty4 { get; set; }
        public decimal gift_qty5 { get; set; }
        public string contact_job_title { get; set; }
        public string cust_phone { get; set; }
        public string currency_code { get; set; }
        public string specification { get; set; }
        public string employer { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string active_status { get; set; }
        public string note { get; set; }

    }
    public class IV_002_PAY
    {
        [Key, Column(Order = 1)]
        public string contract_id { get; set; }
        public int ref_number { get; set; }
        public string teller_num { get; set; }
        public string property_id { get; set; }
        public string transaction_date { get; set; }
        public string pay_method { get; set; }
        public string bank_account { get; set; }
        public string item_code { get; set; }
        public decimal price { get; set; }
        public decimal payment_amt { get; set; }
        public decimal sales_com { get; set; }
        public decimal sales_val { get; set; }
        public decimal monthly_amt { get; set; }
        public decimal tenor { get; set; }
        public string currency { get; set; }
        public decimal exchange_rate { get; set; }
        public decimal base_amount { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class IV_002_OST
    {
        [Key, Column(Order = 0)]
        public string contract_id { get; set; }
        public string property { get; set; }
        public string transaction_date { get; set; }
        public decimal payment_amt { get; set; }
        public decimal sales_com { get; set; }
        public decimal sales_val { get; set; }
        public decimal p_status { get; set; }
        public decimal vat { get; set; }
        public decimal gift { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }

    }
    public class RA_002_RSA
    {
        [Key, Column(Order = 0)]
        public int reset_id { get; set; }
        public string fixed_asset_code { get; set; }
        public string transaction_date { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public string reset { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string attach_document { get; set; }
        public string inactive_status { get; set; }
        public string note { get; set; }
    }
    public class JB_001_JOB
    {
        [Key, Column(Order = 0)]
        public int job_id { get; set; }
        public string job_title { get; set; }
        public string costing_basis { get; set; }
        public decimal cost { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string attach_document { get; set; }
        public string inactive_status { get; set; }
        public string note { get; set; }
    }
    public class JC_002_JCD
    {
        [Key, Column(Order = 0)]
        public string Job_card_id { get; set; }
        [Key, Column(Order = 1)]
        public string Work_order_ID { get; set; }
        public string Job_card_Description { get; set; }
        public string Work_order_date { get; set; }
        public string team_lead { get; set; }
        public string Asset_or_group_ID { get; set; }
        public string work_center { get; set; }
        public string Work_order_completed { get; set; }
        public decimal estimated_total_cost { get; set; }
        public decimal total_materials_cost { get; set; }
        public decimal total_hr_cost { get; set; }
        public int total_duration { get; set; }
        public decimal total_contract_amount { get; set; }
        public decimal total_misc_amount { get; set; }
        public string gl_account { get; set; }
        public string Work_order_completion_Date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
    }
    public class JC_002_JDCDET
    {
        [Key, Column(Order = 0)]
        public string job_card_id { get; set; }
        [Key, Column(Order = 1)]
        public string identity_code { get; set; }
        [Key, Column(Order = 2)]
        public string sequence_no { get; set; }
        [Key, Column(Order = 3)]
        public string flag { get; set; }
        public int qty { get; set; }
        public decimal total { get; set; }
        public decimal cost { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string description { get; set; }
        public string time { get; set; }
        public string completed { get; set; }
    }
    public class AA_002_AAM
    {

        [Key, Column(Order = 0)]
        public string fixed_assest_id { get; set; }
        public string maintenance_type { get; set; }
        public string Tran_date { get; set; }
        public string cumulative_activity_level { get; set; }
        public int cumulative_activity_level_figure { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string attach_document { get; set; }
        // public string inactive_status { get; set; }
        public string note { get; set; }
    }
    public class WO_002_WKO
    {
        [Key, Column(Order = 0)]
        public string work_order_id { get; set; }

        [Key, Column(Order = 1)]
        public string flag { get; set; }
        public string asset_or_group { get; set; }
        public string work_order_date { get; set; }
        public string maintenance_id { get; set; }
        public string activation_id { get; set; }
        public string work_center_id { get; set; }
        public string gl_account { get; set; }
        public string work_order_description { get; set; }
        public decimal total_contract_amount { get; set; }
        public string team_lead { get; set; }

        public decimal total_materials_cost { get; set; }
        public decimal total_hr_cost { get; set; }
        public int total_duration { get; set; }
        public decimal total_misc_amount { get; set; }
        public string cvt_to_wrk_ord { get; set; }
        public decimal estimated_total_cost { get; set; }
        public string estimated_start_date_time { get; set; }
        public string estimated_end_date_time { get; set; }
        public string job_card_id { get; set; }
        public string status { get; set; }

        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        
    }
    public class WO_002_WKODT
    {
        [Key, Column(Order = 0)]
        public string work_order_id { get; set; }
        [Key, Column(Order = 1)]
        public string identity_code { get; set; }
        [Key, Column(Order = 2)]
        public string sequence_no { get; set; }
        [Key, Column(Order = 3)]
        public string flag { get; set; }
        public int qty { get; set; }
        public decimal total { get; set; }
        public decimal cost { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string description { get; set; }
        public string time { get; set; }

    }
    public class MN_002_MNT
    {
        [Key, Column(Order = 0)]
        public int activation_id { get; set; }
        public string activation_type { get; set; }
        public string trans_date { get; set; }
        public string fixed_asset_code { get; set; }
        public string explain_work { get; set; }
        public string reported_by { get; set; }
        public string convert_to_work_order { get; set; }
        public string attach_document { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
    }
    public class AG_002_ASSET
    {
        [Key, Column(Order = 0)]
        public string maintenance_group_type_id { get; set; }
        [Key, Column(Order = 1)]
        public string identity_code { get; set; }
        [Key, Column(Order = 2)]
        public string sequence_no { get; set; }
        [Key, Column(Order = 3)]
        public string flagg { get; set; }
        public int qtyreq { get; set; }
        public decimal total { get; set; }
        public int head_count { get; set; }
        public string time { get; set; }
    }
    public class TC_001_TCL
    {
        [Key, Column(Order = 0)]
        public string technical_competency_level_id { get; set; }
        public string description { get; set; }
        public string required_competence { get; set; }
        public string comments { get; set; }

        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string inactive_status { get; set; }
    }
    public class AG_001_AMG
    {
        [Key, Column(Order = 0)]
        public string maintenance_group_type_id { get; set; }
        public string description { get; set; }
        public string nature { get; set; }
        public string required_maintenance_basis { get; set; }
        public int require_asset_running { get; set; }
        public string item_group_code { get; set; }
        public string qty { get; set; }
        public string gl_account { get; set; }
        public string staff_id { get; set; }
        public string Estimated_hour { get; set; }
        public string sub_contracts_id { get; set; }
        public string task_descriptioin_id { get; set; }
        public string Completed { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string inactive_status { get; set; }
        public Decimal estimated_total { get; set; }
        public Decimal hr_total { get; set; }
        public Decimal material_total { get; set; }
        public Decimal subcontract_total { get; set; }
    }
    public class SC_001_SCM
    {
        [Key, Column(Order = 0)]
        public string sub_contract_id { get; set; }
        public string description { get; set; }
        public string vendor_id { get; set; }
        public string service_order_id { get; set; }
        public decimal total_cost { get; set; }
        public string contract_ref_no { get; set; }
        public string contract_date { get; set; }
        public string duration { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string inactive_status { get; set; }
    }
    public class WT_001_WKT
    {
        [Key, Column(Order = 0)]
        public string work_team_id { get; set; }
        public string description { get; set; }
        [Key, Column(Order = 1)]
        public string staff_id { get; set; }
        public string competence { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string inactive_status { get; set; }
    }
    public class AG_001_ASG
    {
        [Key, Column(Order = 0)]
        public string asset_grouping_id { get; set; }
        public string description { get; set; }
        public int cumulative_amount { get; set; }
        public string maintenance_type_id { get; set; }
        public string asset_requires_maintenace { get; set; }
        public string last_maintenance_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string inactive_status { get; set; }
        public int required_maintenance_val { get; set; }
        public string uor { get; set; }
        public string group_flag { get; set; }
    }
    public class AM_001_AMS
    {

        [Key, Column(Order = 0)]
        public string asset_maintenance_id { get; set; }
        public string asset_maintenance_description { get; set; }
        public string asset_group_id { get; set; }
        public string maintenance_group_type_id { get; set; }
        public string required_running_days { get; set; }
        public string next_maintenance_date { get; set; }
        public string task_id { get; set; }
        public string completed { get; set; }

        public string analysis_code1 { get; set; }

        public string analysis_code2 { get; set; }

        public string analysis_code3 { get; set; }

        public string analysis_code4 { get; set; }

        public string analysis_code5 { get; set; }

        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public string gl_account_code { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string inactive_status { get; set; }
    }
    public class WC_001_WKC
    {
        [Key, Column(Order = 0)]
        public string work_center_id { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string employee_code { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
        public string inactive_status { get; set; }

    }
    public class TK_001_TAC
    {
        [Key, Column(Order = 0)]
        public string task_id { get; set; }
        [Key, Column(Order = 1)]
        public int sequence { get; set; }
        [Key, Column(Order = 2)]
        public string flag { get; set; }
        public string name { get; set; }
        public int estimated_no_of_hrs { get; set; }
        public string task_description { get; set; }
        public string required_job_level_id { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }

        public string modified_by { get; set; }
        public string note { get; set; }
        public string inactive_status { get; set; }

    }
    public class AP_002_PURHEAD
    {
        [Key, Column(Order = 0)]
        public int purchase_sequence_number { get; set; }
        public string purchase_transaction_type { get; set; }
        public string vendor_code { get; set; }
        public string vend_order_number { get; set; }
        public int status { get; set; }
        public int number_of_day { get; set; }
        public string currency_code { get; set; }
        public decimal exchange_rate { get; set; }
        public string transaction_date { get; set; }
        public string warehouse { get; set; }
        public string requisition_date { get; set; }
        public string requisition_reference { get; set; }
        public string purchase_quote_date { get; set; }
        public string purchase_quote_expiration_date { get; set; }
        public string purchase_order_date { get; set; }
        public string purchase_order_expiration_date { get; set; }
        public string expected_delivery_date { get; set; }
        public string delivery_address_code { get; set; }
        public string item_warehouse_code { get; set; }
        public string payment_term_code { get; set; }
        public decimal quote_total_extended_price { get; set; }
        public decimal quote_total_discount { get; set; }
        public decimal quote_total_tax { get; set; }
        public decimal quote_total_amount { get; set; }
        public decimal quote_total_wht_amount { get; set; }
        public decimal quote_total_net_payable_amount { get; set; }
        public decimal order_total_extended_price { get; set; }
        public decimal order_total_discount { get; set; }
        public decimal order_total_tax { get; set; }
        public decimal order_total_amount { get; set; }
        public decimal order_total_wht_amount { get; set; }
        public decimal order_total_net_payable_amount { get; set; }
        public decimal invoice_total_ext_price { get; set; }
        public decimal invoice_total_discount { get; set; }
        public decimal invoice_total_tax { get; set; }
        public decimal invoice_total_amount { get; set; }
        public decimal invoice_total_wht_amount { get; set; }
        public decimal invoice_total_net_payable_amount { get; set; }
        public string order_reference { get; set; }
        public string quote_reference { get; set; }
        public string grn_reference { get; set; }
        public string order_expiration_date { get; set; }
        public string order_date { get; set; }
        public string exchange_rate_mode { get; set; }
        public string project_code { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public string attach_document { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public string note { get; set; }
    }
    public class GB_001_PTYS
    {
        [Key, Column(Order = 0)]
        public string contract_id { get; set; }
        [Key, Column(Order = 1)]
        public string flag { get; set; }
        public decimal penalty { get; set; }
        public string transfer_to { get; set; }
        public string gl_account_code { get; set; }
        public decimal current_value { get; set; }
        public string note { get; set; }
        public int approval_level { get; set; }
        public DateTime approval_date { get; set; }
        public string approval_by { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        
    }
    public class GB_001_PHEAD
    {
        [Key, Column(Order = 0)]
        public string header_type_code { get; set; }
        [Key, Column(Order = 1)]
        public int sequence_no { get; set; }
        public string fee_id { get; set; }
        public string mandatory_flag { get; set; }
        public string delete_flag { get; set; }
        public decimal amount { get; set; }
        public string note { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
    }
    public class GB_001_MAP
    {
        [Key, Column(Order = 0)]
        public string main_head { get; set; }
        [Key, Column(Order = 1)]
        public string trans_type { get; set; }
        [Key, Column(Order = 2)]
        public int seq_no { get; set; }
        public string jon_seq { get; set; }
        public string tran_seq { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
    }
    public class AP_002_GRN
    {
        [Key, Column(Order = 0)]
        public int purchase_sequence_number { get; set; }
        [Key, Column(Order = 1)]
        public int line_sequence { get; set; }
        [Key, Column(Order = 2)]
        public int sub_line_sequence { get; set; }
        public string purchase_grn_sequence { get; set; }
        public string vendor_code { get; set; }
        public string item_code { get; set; }
        public decimal grn_quantity { get; set; }
        public decimal po_quantity { get; set; }
        public decimal outstanding_quantity { get; set; }
        public decimal price { get; set; }
        public decimal wht { get; set; }
        public decimal base_amount { get; set; }
        public decimal extended_price { get; set; }
        public string item_warehouse_code { get; set; }
        public string additional_description { get; set; }
        public string bin_location_code { get; set; }
        public string grn_date { get; set; }
        //public decimal delivery_quantity { get; set; }
       //public decimal price_rate { get; set; }
       //public decimal price { get; set; }
       //public decimal extended_price { get; set; }
         //public decimal net_amount { get; set; }
        //public decimal wht { get; set; }
       //public decimal net_payable_amount { get; set; }
       //public decimal order_amount { get; set; }
        //public decimal base_amount { get; set; }
        public string transaction_type { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
    }
    public class AP_002_IBKNG
    {
        [Key, Column(Order = 0)]
        public int purchase_sequence_number { get; set; }
        [Key, Column(Order = 1)]
        public int line_sequence { get; set; }
        [Key, Column(Order = 2)]
        public int sub_line_sequence { get; set; }
        public string purchase_order_sequence { get; set; }
        public string vendor_code { get; set; }
        public string item_code { get; set; }
        public decimal order_quantity { get; set; }
        public string item_warehouse_code { get; set; }
        public string additional_description { get; set; }
        public decimal delivery_quantity { get; set; }
        public decimal price_rate { get; set; }
        public decimal price { get; set; }
        //public decimal tax_invoice { get; set; }
        //public string dis_flag { get; set; }
        public decimal extended_price { get; set; }
        //public decimal discount_percent { get; set; }
        //public decimal discount_amount { get; set; }
        // public decimal actual_discount { get; set; }
        public decimal net_amount { get; set; }
        public decimal wht { get; set; }
        public decimal net_payable_amount { get; set; }
        // public string order_date { get; set; }
        //public decimal tax_amount3 { get; set; }
        //public decimal tax_amount4 { get; set; }
        //public decimal tax_amount5 { get; set; }
        //public decimal tax_invoiceamt1 { get; set; }
        //public decimal tax_invoiceamt2 { get; set; }
        public decimal order_amount { get; set; }
        public decimal base_amount { get; set; }
        //public DateTime order_date { get; set; }
        public string include_tax { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
    }
    public class AP_002_RQUSN
    {
        [Key, Column(Order = 0)]
        public int purchase_sequence_number { get; set; }
        [Key, Column(Order = 1)]
        public int line_sequence { get; set; }
        public int sub_line_sequence { get; set; }
        public string purchase_requisition_sequence { get; set; }
        public string item_code { get; set; }
        public string warehouse { get; set; }
        public decimal purchase_requition_quantity { get; set; }
        public string requisition_date { get; set; }
        public string note { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
    }
    public class AP_002_QUOTE
    {
        [Key, Column(Order = 0)]
        public int purchase_sequence_number { get; set; }
        [Key, Column(Order = 1)]
        public int line_sequence { get; set; }
        [Key, Column(Order = 2)]
        public int sub_line_sequence { get; set; }
        public string purchase_quote_sequence { get; set; }
        public string dis_flag { get; set; }
        public string vendor_code { get; set; }
        public string item_code { get; set; }
        public string additional_description { get; set; }
        public decimal purchase_quote_qty { get; set; }
        public decimal price { get; set; }
        public decimal price_rate { get; set; }
        public decimal ext_price { get; set; }
        public decimal discount_percent { get; set; }
        public decimal discount_amount { get; set; }
        public decimal actual_discount { get; set; }
        public decimal net_amount { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax_amount1 { get; set; }
        public decimal tax_amount2 { get; set; }
        public decimal tax_amount3 { get; set; }
        public decimal tax_amount4 { get; set; }
        public decimal tax_amount5 { get; set; }
        public decimal tax_invoiceamt1 { get; set; }
        public decimal tax_invoiceamt2 { get; set; }
        public decimal quote_amount { get; set; }
        public decimal base_amount { get; set; }
        public decimal wht { get; set; }
        public decimal net_payable_amount { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }

        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

    }
    public class AP_002_RSQNH
    {
        [Key, Column(Order = 0)]
        public int purchase_sequence_number { get; set; }
        public string warehouse { get; set; }
        public string purchase_transaction_type { get; set; }
        public string requisition_reference { get; set; }
        public string transaction_date { get; set; }
        public string note { get; set; }
        public string analysis_code1 { get; set; }
        public string analysis_code2 { get; set; }
        public string analysis_code3 { get; set; }
        public string analysis_code4 { get; set; }
        public string analysis_code5 { get; set; }
        public string analysis_code6 { get; set; }
        public string analysis_code7 { get; set; }
        public string analysis_code8 { get; set; }
        public string analysis_code9 { get; set; }
        public string analysis_code10 { get; set; }
        public int approval_level { get; set; }
        public string approval_by { get; set; }
        public DateTime approval_date { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
      
    }
    
   
}


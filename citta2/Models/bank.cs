using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anchor1.Models
{
    public class tab_bank
    {

        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string bank_code { get; set; }

        public string name1 { get; set; }
        public string report_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string bank_group { get; set; }
        public string sort_code { get; set; }
        public string comment_code { get; set; }
        public string addnotes { get; set; }
        public string email { get; set; }
        public string parameter_close { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_analy
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }


        [Key, Column(Order = 1)]
        public string analy_code { get; set; }

        public string analy0 { get; set; }
        public string analy1 { get; set; }
        public string analy2 { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }


    }

    public class tab_allow
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string allow_code { get; set; }

        public string name1 { get; set; }
        public string allowance_code { get; set; }
        public string frequency { get; set; }
        public string payroll_type { get; set; }
        public string calculation_code { get; set; }
        public string employee_type { get; set; }
        public int start_period { get; set; }
        public int interval_period { get; set; }
        public string report_name { get; set; }
        public int maximum_payment { get; set; }
        public int eoyperiod { get; set; }
        public string loan_payment { get; set; }
        public string reversal_code_type { get; set; }
        public string reversal_when { get; set; }
        public string reversal_code { get; set; }
        public string gratuity_enable { get; set; }
        public string internal_use { get; set; }
        public string parameter_close { get; set; }

        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_calc
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string calc_code { get; set; }

        public string name1 { get; set; }
        public string report_name { get; set; }
        public string transfer_code { get; set; }
        public string suppress_zero { get; set; }
        public decimal line_spacing { get; set; }
        public string wide_column { get; set; }
        public int column_no { get; set; }
        public string report_type { get; set; }
        public string menu_option { get; set; }
        public string internal_use { get; set; }
        public string comment_code { get; set; }
        public string addnotes { get; set; }

        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }


    }

    public class tab_loan
    {

        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string loan_code { get; set; }

        public string name1 { get; set; }
        public string report_name { get; set; }
        public decimal interest_rate { get; set; }
        public string calculation_method { get; set; }
        public string payroll_type { get; set; }
        public string deduction_mode { get; set; }
        public string loan_payrun { get; set; }
        public string frequency { get; set; }
        public int start_loan { get; set; }
        public int interval_loan { get; set; }
        public int start_interest { get; set; }
        public int interval_interest { get; set; }
        public string interest_frequency { get; set; }
        public string flexible_rate { get; set; }
        public string self_code { get; set; }
        public string condition1 { get; set; }
        public string condition2 { get; set; }
        public string condition3 { get; set; }
        public string condition4 { get; set; }

        public string parameter_close { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_analy_code
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string trans_code { get; set; }

        public string name1 { get; set; }
        public string report_name { get; set; }
        public string parameter_close { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }


    }

    public class tab_grade
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string grade_code { get; set; }

        public string name1 { get; set; }
        public string report_name { get; set; }
        public string overtime_code { get; set; }
        public string taxincome_code { get; set; }
        public string taxtable_code { get; set; }
        public string relief_code { get; set; }
        public string payment_cycle { get; set; }
        public string payment_mode { get; set; }
        public string net_gross { get; set; }
        public string parameter_close { get; set; }

        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }


    }

    public class tab_over
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string over_code { get; set; }

        public string name1 { get; set; }
        public decimal weekday_rate { get; set; }
        public decimal weekend_rate { get; set; }
        public decimal public_rate { get; set; }
        public decimal max_weekday { get; set; }
        public decimal max_weekend { get; set; }
        public decimal max_public { get; set; }
        public decimal max_amount { get; set; }
        public decimal max_percentage { get; set; }
        public string report_name { get; set; }
        public string payroll_type { get; set; }
        public string parameter_close { get; set; }

        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_tax
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string tax_code { get; set; }

        public string name1 { get; set; }
        public decimal default_amount { get; set; }
        public decimal default_percentage { get; set; }
        public decimal min_amount_tax { get; set; }
        public decimal min_amount { get; set; }
        public decimal min_percentage { get; set; }
        public string report_name { get; set; }

        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }


    }

    public class tab_category_advice
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string advice_type { get; set; }
        
        [Key, Column(Order = 2)]
        public string category_from_adv { get; set; }
        
        [Key, Column(Order = 3)]
        public string category_to_adv { get; set; }
        
        [Key, Column(Order = 4)]
        public string payroll_type { get; set; }

        public string advice_document { get; set; }
        public string advice_document_type { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }


    }

    public class tab_array
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string array_code { get; set; }

        [Key, Column(Order = 2)]
        public decimal count_array { get; set; }

        public string operand { get; set; }
        public string source1 { get; set; }
        public string period { get; set; }
        public string operator1 { get; set; }
        public decimal amount { get; set; }
        public decimal percent1 { get; set; }
        public string select1 { get; set; }
        public string sort1 { get; set; }
        public string true_desc { get; set; }
        public string false_desc { get; set; }
        public string internal_use { get; set; }

        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

        public static string fldr_code { get; set; }

    }

    public class tab_calend
    {
        [Key, Column(Order = 0)]
        public string payment_cycle { get; set; }

        [Key, Column(Order = 1)]
        public int sequence { get; set; }

        [Key, Column(Order = 2)]
        public int count_seq { get; set; }

        public string period_name { get; set; }
        public int days_in_period { get; set; }
        public int working_days { get; set; }
        public int working_hours { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public string internal_use { get; set; }

        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }


    }

    public class tab_train
    {

        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string train_code { get; set; }

        public string course_name { get; set; }
        public string trainer_company { get; set; }
        public string address1 { get; set; }
        public string md_name { get; set; }
        public string skill_attached { get; set; }
        public string proficiency_level { get; set; }
        public decimal default_cost { get; set; }
        public string report_name { get; set; }

        public string internal_use { get; set; }
        public string parameter_close { get; set; }
        public string comment_code { get; set; }
        public string addnotes { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_daily
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string daily_code { get; set; }

        public string name1 { get; set; }
        public string code_ind { get; set; }
        public string days_input { get; set; }
        public decimal rate_amount { get; set; }
        public decimal max_amount { get; set; }
        public decimal max_percentage { get; set; }
        public decimal max_days { get; set; }
        public string report_name { get; set; }
        public string payroll_type { get; set; }
        public int eoyperiod { get; set; }
        public string parameter_close { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_temp_rep
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int countID { get; set; }
        public string user_code { get; set; }
        public string report_code { get; set; }
        public string sort1 { get; set; }
        public string op1 { get; set; }
        public string src1 { get; set; }
        public string perd1 { get; set; }
        public string sort_order { get; set; }
        public string report_line { get; set; }
        public Int32 rep_count { get; set; }
        public string sort2 { get; set; }
        public string row_desc { get; set; }
        public string row_abs { get; set; }
        public string row_row1 { get; set; }
        public string row_col { get; set; }
        public Int32 advice_count { get; set; }
        public string text1 { get; set; }
        public string text2 { get; set; }
        public string text3 { get; set; }
        public string text4 { get; set; }
    }

    public class tab_default
    {
        [Key]
        public string default_code { get; set; }

        public string processing_period1 { get; set; }
        public int no_of_periods1 { get; set; }
        public int item_row { get; set; }
        public string loan_repay { get; set; }
        public int decimal_places { get; set; }
        public string staff_service { get; set; }
        public string negative_code { get; set; }
        public string tax_mode { get; set; }
        public string daily_mode { get; set; }
        public string separate_code { get; set; }
        public decimal loan_percentage { get; set; }
        public int maximum_hours { get; set; }
        public string payroll_approval { get; set; }
        public int finance_eop { get; set; }
        public string close_query { get; set; }
        public string code_query { get; set; }
        public string update_switch { get; set; }
        public int smtp_port { get; set; }
        public string smtp_host { get; set; }
        public string sender_name { get; set; }
        public string sender_mail { get; set; }
        public string mail_user { get; set; }
        public string mail_password { get; set; }
        public int  mail_reminder { get; set; }
        public string transfer_start_char { get; set; }
        public string separate_mark { get; set; }
        public string default_pay_cycle { get; set; }
        public decimal basic_percentage { get; set; }
        public string basic_analysis { get; set; }
        public int screen_timeout { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_pay_default
    {
        [Key]
        public string payment_cycle { get; set; }

        public string name1 { get; set; }
        public string processing_period { get; set; }
        public int no_of_periods { get; set; }
        public int finance_eop { get; set; }
        public int maximum_hours { get; set; }
        public string relief_gross { get; set; }
        public string netpay_gross { get; set; }
        public string allow_gross { get; set; }
        public string parameter_close { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_staff
    {
        [Key]
        [StringLength(10)]
        public string staff_number { get; set; }

        public string surname { get; set; }
        public string first_name { get; set; }
        public string other_name { get; set; }
        public string date_of_birth { get; set; }
        public string nationality { get; set; }
        public string state { get; set; }
        public string state_lga { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string telephone { get; set; }
        public string salutation { get; set; }
        public string sex { get; set; }
        public string marital_status { get; set; }
        public int children { get; set; }
        public int dependant { get; set; }
        public string category { get; set; }
        public int step { get; set; }
        public decimal annual_salary { get; set; }
        public string approval_category { get; set; }
        public string date_of_employment { get; set; }
        public string payment_mode { get; set; }
        public string bank_code { get; set; }
        public string account_number { get; set; }
        public string spouse { get; set; }
        public string spouse_dob { get; set; }
        public string maiden_name { get; set; }
        public string email_address { get; set; }
        public string telephone_ext { get; set; }
        public string job_post { get; set; }
        public string mobile_phone { get; set; }
        public string religion { get; set; }
        public string home_place { get; set; }
        public string pfa_number { get; set; }
        public string cost_centre { get; set; }
        public string pension_number { get; set; }
        public string nhf_number { get; set; }
        public string tax_number { get; set; }
        public string kin_name { get; set; }
        public string kin_relationship { get; set; }
        public string kin_address1 { get; set; }
        public string kin_address2 { get; set; }
        public string kin_address3 { get; set; }
        public string kin_telephone { get; set; }
        public string kin_email { get; set; }
        public string approval_route { get; set; }
        public string trans0 { get; set; }
        public string trans1 { get; set; }
        public string trans2 { get; set; }
        public string trans3 { get; set; }
        public string trans4 { get; set; }
        public string trans5 { get; set; }
        public string trans6 { get; set; }
        public string trans7 { get; set; }
        public string trans8 { get; set; }
        public string trans9 { get; set; }
        public string person0 { get; set; }
        public string person1 { get; set; }
        public string person2 { get; set; }
        public string person3 { get; set; }
        public string person4 { get; set; }
        public string person5 { get; set; }
        public string person6 { get; set; }
        public string person7 { get; set; }
        public string person8 { get; set; }
        public string person9 { get; set; }
        public decimal amount0 { get; set; }
        public decimal amount1 { get; set; }
        public decimal amount2 { get; set; }
        public decimal amount3 { get; set; }
        public decimal amount4 { get; set; }
        public decimal amount5 { get; set; }
        public decimal amount6 { get; set; }
        public decimal amount7 { get; set; }
        public decimal amount8 { get; set; }
        public decimal amount9 { get; set; }
        public decimal staff_approval { get; set; }
        public string close_code { get; set; }
        public string close_date { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

        public string staff_name { get; set; }
    }

    public class vw_staff_mast
    {

        [Key]
        public string staff_number { get; set; }

        public string surname { get; set; }
        public string first_name { get; set; }
        public string other_name { get; set; }
        public Decimal basic_ytd { get; set; }
        public Decimal gross_ytd { get; set; }
        public Decimal payable_ytd { get; set; }
        public Decimal allowance_ytd { get; set; }
        public Decimal deduction_ytd { get; set; }
        public Decimal taxable_ytd { get; set; }
        public Decimal freepay_ytd { get; set; }
        public Decimal payroll_allowance_ytd { get; set; }
        public Decimal tax_paid_ytd { get; set; }
        public Decimal tax_month { get; set; }
        public Decimal weekdays_ytd { get; set; }
        public Decimal weekend_ytd { get; set; }
        public Decimal public_ytd { get; set; }
        public Decimal overtime_ytd { get; set; }
        public Decimal bik_tax { get; set; }
        public string staff_name { get; set; }

    }

    public class tab_pen_pfa
    {
        [Key]
        public string pfa_number { get; set; }

        public string pfa_name { get; set; }
        public string pfa_address1 { get; set; }
        public string pfa_address2 { get; set; }
        public string pfa_address3 { get; set; }
        public string pfa_contact { get; set; }
        public string pfa_mail { get; set; }
        public string pfa_telephones { get; set; }
        public string reg_pfa_number { get; set; }
        public string pension_custodian { get; set; }
        public string parameter_close { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_job_desc
    {
        [Key]
        public string job_code { get; set; }
       
        public string name1 { get; set; }
	    public string initial_category { get; set; }
	    public int lower_age { get; set; }
	    public int upper_age { get; set; }
	    public string qualification_group1 { get; set;}
	    public string course_group1 { get; set;}
	    public string class_group1 { get; set;}
	    public string qualification_group2 { get; set;}
	    public string course_group2 { get; set;}
	    public string class_group2 { get; set;}
	    public string qualification_group3 { get; set;}
	    public string course_group3 { get; set;}
	    public string class_group3 { get; set;}
	    public decimal experience {get;set;}
	    public string reporting_to { get; set;}
	    public int manning {get;set;}
	    public int rank  {get;set;}
	    public string department_code { get; set;}
	    public string user_code1 { get; set;}
	    public string user_code2 { get; set;}
	    public string user_code3 { get; set;}
	    public string user_code4 { get; set;}
	    public string parameter_close {get;set;}
        public string addnotes { get; set; }
        public string comment_code { get; set; }
        public string created_by { get; set;}
	    public DateTime date_created {get;set;}
	    public string amended_by { get; set;}
	    public DateTime date_amended {get;set;}

    }

    public class tab_trans_input
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string date_dal { get; set; }
        
        public string payroll_period { get; set; }        
        public decimal hours_dal { get; set; }
        public decimal processed { get; set; }
        public decimal approval { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_trans_over
    {
        [Key]
        public string staff_number { get; set; }

        public string payroll_period { get; set; }
        public decimal days_worked { get; set; }
        public decimal weekdays_hrs { get; set; }
        public decimal weekend_hrs { get; set; }
        public decimal public_hrs { get; set; }
        public decimal calculated_days { get; set; }
        public decimal approval { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_trans_loan
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }
  
        [Key, Column(Order = 1)]
        public string loan_code { get; set; }
        
        [Key, Column(Order = 2)]
        public string indicatora { get; set; }

        public string payroll_period { get; set; }
        public decimal amount { get; set; }
        public int tenor { get; set; }
        public decimal monthly_amount { get; set; }
        public string effective_period { get; set; }
        public string pay_loan { get; set; }
        public decimal interest_rate_trans { get; set; }
        public decimal approval { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_mast_loan
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }
        
        [Key, Column(Order = 1)]
        public string loan_code { get; set; }

        public decimal loan_amount { get; set; }
        public decimal loan_balance { get; set; }
        public decimal deduction_amount_td { get; set; }
        public decimal interest_amount_td { get; set; }
        public decimal interest_balance { get; set; }
        public decimal interest_rate { get; set; }
        public string effective_period { get; set; }
        public string effective_date { get; set; }
        public decimal amort_amount { get; set; }
        public int tenor { get; set; }
        public int no_of_payment { get; set; }
        public string deduction_method { get; set; }
        public string calc_method { get; set; }
        public string last_interest_period { get; set; }
        public string payroll_period { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_trans_allow
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }
   
        [Key, Column(Order = 1)]
        public string codea { get; set; }
        
        [Key, Column(Order = 2)]
        public string indicatora { get; set; }
        
        [Key, Column(Order = 3)]
        public string group_code { get; set; }

        public string payroll_period { get; set; }
        public decimal allow_amount { get; set; }
        public decimal allow_percentage { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax_percentage { get; set; }
        public string date_ind { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public decimal approval { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_trans_daily
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }
        
        [Key, Column(Order = 1)]
        public string daily_code { get; set; }
        
        [Key, Column(Order = 2)]
        public string group_code { get; set; }

        public string payroll_period { get; set; }
        public decimal no_of_days { get; set; }
        public decimal approval { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }
    
    public class tab_trans_prom
    {
        [Key, Column(Order = 0)]
        public string trans_code { get; set; }
        
        [Key, Column(Order = 1)]
        public string staff_number { get; set; }
        
        [Key, Column(Order = 2)]
        public string group_code { get; set; }

        public string payroll_period { get; set; }
        public string category { get; set; }
        public int step { get; set; }
        public decimal annual_salary { get; set; }
        public string effective_date { get; set; }
        public string new_staff { get; set; }
        public decimal approval { get; set; }
        public decimal basic_percentage { get; set; }
        public decimal gross_amount { get; set; }
        public string effective_period { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }
    
    public class tab_trans_stat
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }
       
        [Key, Column(Order = 1)]
        public string codea { get; set; }
        
        [Key, Column(Order = 2)]
        public string indicatora { get; set; }
        
        [Key, Column(Order = 3)]
        public string group_code { get; set; }

        public string payroll_period { get; set; }
        public decimal percentage { get; set; }
        public string date_range { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public string start_payment { get; set; }
        public string stop_payment { get; set; }
        public decimal approval { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_mast_status
    {
        [Key, Column(Order = 0)]
        public string group_code { get; set; }
       
        [Key, Column(Order = 1)]
        public string staff_number { get; set; }
        
        [Key, Column(Order = 2)]
        public string codea { get; set; }
        
        [Key, Column(Order = 3)]
        public string indicatora { get; set; }

        public decimal percentage { get; set; }
        public string date_range { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public string payroll_period { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_document
    {
        [Key, Column(Order = 0)]
        [StringLength(10)]
        public string doc_code { get; set; }

        public string doc_type { get; set; }
        public string name1 { get; set; }
        public decimal right_margin { get; set; }
        public string doc_text { get; set; }
        public decimal numeric_size { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_advice
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string advice_code { get; set; }
        
        public string name1 { get; set; }
        public string payment_run { get; set; }
        public string slip_document { get; set; }
        public string slip_document_type { get; set; }
        public string slip_category { get; set; }
        public string printer_code { get; set; }
        public string current_staff { get; set; }
        public string select1 { get; set; }
        public string select2 { get; set; }
        public string select3 { get; set; }
        public string select4 { get; set; }
        public string select5 { get; set; }
        public string sort1 { get; set; }
        public string sort2 { get; set; }
        public string sort3 { get; set; }
        public string sort4 { get; set; }
        public string sort5 { get; set; }
        public string menu_group { get; set; }
 
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_attach
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string attach_category { get; set; }

        [Key, Column(Order = 2)]
        public string category_from { get; set; }

        [Key, Column(Order = 3)]
        public string category_to { get; set; }

        [Key, Column(Order = 4)]
        public string attach_code { get; set; }
        
        public decimal allow_amount { get; set; }
        public decimal allow_percentage { get; set; }
        public string allow_select { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax_percentage { get; set; }
        public string tax_select { get; set; }
        public string gross_code { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_mast_allow
    {
        [Key, Column(Order = 0)]
        public string record_type { get; set; }

        [Key, Column(Order = 1)]
        public string group_code { get; set; }

        [Key, Column(Order = 2)]
        public string staff_number { get; set; }

        [Key, Column(Order = 3)]
        public string allow_code { get; set; }

        public decimal allowance_ytd { get; set; }
        public decimal taxable_ytd { get; set; }
        public decimal allowance_gtd { get; set; }
        public decimal taxable_gtd { get; set; }
        public decimal allowance_amount { get; set; }
        public decimal allowance_percentage { get; set; }
        public decimal taxable_amount { get; set; }
        public decimal taxable_percentage { get; set; }
        public string reference { get; set; }
        public string start_period { get; set; }
        public string stop_period { get; set; }
        public decimal counter1 { get; set; }
        public decimal allowance_lytd { get; set; }
        public decimal taxable_lytd { get; set; }
        public string payroll_period { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_self_approval
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string transaction_apv { get; set; }

        [Key, Column(Order = 2)]
        public string category_apv { get; set; }

        [Key, Column(Order = 3)]
        public string approval_category { get; set; }

        [Key, Column(Order = 4)]
        public string approval_category_to { get; set; }

        [Key, Column(Order = 5)]
        public int level_count_apv { get; set; }
        
        public string level1_apv { get; set; }
        public string level1_and_apv { get; set; }
        public string level11_apv { get; set; }
        public string group_to_apv { get; set; }
        public string information_apv { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_type
    {
        [Key, Column(Order = 0)]
        public string trans_type { get; set; }

        public string name1 { get; set; }
        public string report_name { get; set; }
        public string mapping_code1 { get; set; }
        public string mapping_code2 { get; set; }
        public string mapping_code3 { get; set; }
        public string value_date { get; set; }
        public string selection_line { get; set; }
        public string self_name { get; set; }
        public string self_option { get; set; }
        public string eemployee_menu { get; set; }
        public string tabular_screen { get; set; }
	    public int mail_reminder { get; set; }
	    public string limit_code { get; set; }
        public string restrict_access { get; set; }
        public string internal_use { get; set; }
        public string parameter_close { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_type2
    {
        [Key, Column(Order = 0)]
        public string trans_type { get; set; }

        [Key, Column(Order = 1)]
        public int count_type { get; set; }

        public string text_line { get; set; }
        public int text_row { get; set; }
        public int text_len { get; set; }
        public string text_code { get; set; }
        public string text_type { get; set; }
        public string text_mandatory { get; set; }
        public string text_header { get; set; }
        public string data_line { get; set; }
        public string visible_line { get; set; }
        public int sequence_no { get; set; }
        public string alert_type { get; set; }
        public int line_input { get; set; }
        public int line_result { get; set; }
        public string grid_display { get; set; }
        public string internal_use { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_defined
    {
        [Key, Column(Order = 0)]
        public string type_code { get; set; }

        [Key, Column(Order = 1)]
        public string para_code { get; set; }

        [Key, Column(Order = 2)]
        public string trans_code { get; set; }

        public string name1 { get; set; }
        public string rate_name { get; set; }
        public string report_name { get; set; }
        public int weight_rate { get; set; }
        public decimal amount1 { get; set; }
        public string internal_use { get; set; }
        public string parameter_close { get; set; }
        public string addnotes { get; set; }
        public string comment_code { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_layout
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string layout_code { get; set; }

        [Key, Column(Order = 2)]
        public string sub_category { get; set; }

        public string name1 { get; set; }
        public int start_pos { get; set; }
        public int length_pos { get; set; }
        public string cons_lay { get; set; }
        public int decimal_lay { get; set; }
        public string decimal_point_lay { get; set; }
        public string indicator_lay { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_account
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }

        [Key, Column(Order = 1)]
        public string post_code { get; set; }

        [Key, Column(Order = 2)]
        public string operand1 { get; set; }

        [Key, Column(Order = 3)]
        public string source1 { get; set; }
        
        public string name1 { get; set; }
        public string debit_code { get; set; }
        public string debit_post1 { get; set; }
        public string debit_post2 { get; set; }
        public string debit_post3 { get; set; }
        public string debit_post4 { get; set; }
        public string debit_post5 { get; set; }
        public string debit_post6 { get; set; }
        public string debit_post7 { get; set; }
        public string credit_code { get; set; }
        public string credit_post1 { get; set; }
        public string credit_post2 { get; set; }
        public string credit_post3 { get; set; }
        public string credit_post4 { get; set; }
        public string credit_post5 { get; set; }
        public string credit_post6 { get; set; }
        public string credit_post7 { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_train_default
    {
        [Key, Column(Order = 0)]
        public string default_code { get; set; }

        public string train_budget { get; set; }
        public string field1 { get; set; }
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
        public int field13 { get; set; }
        public string field14 { get; set; }
        public string field15 { get; set; }
        public decimal field16 { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_photo
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string photo_type { get; set; }

        [Key, Column(Order = 2)]
        public string document_code { get; set; }

        public string document_type { get; set; }
        public string document_name { get; set; }
        public string document_access { get; set; }
        public byte[] picture1 { get; set; }
        public decimal processed { get; set; }
        public string internal_use { get; set; }
        public int top_count { get; set; }
        public int down_count { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
        public string comment_code { get; set; }
    }

    public class tab_holiday
    {
        [Key, Column(Order = 0)]
        public int sequence { get; set; }
       
        [Key, Column(Order = 1)]
        public int count_hol { get; set; }
        
        public string date_hol { get; set; }
        public string date_type { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_delegate
    {
        [Key, Column(Order = 0)]
        public string trans_type { get; set; }

        [Key, Column(Order = 1)]
        public string delegator { get; set; }

        public string delegatee { get; set; }
        public string start_date { get; set; }
        public string admin_use { get; set; }
        public string end_date { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_transaction
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string trans_type { get; set; }
        
        [Key, Column(Order = 2)]
        public string value_date { get; set; }
        
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        public string line4 { get; set; }
        public string line5 { get; set; }
        public string line6 { get; set; }
        public string line7 { get; set; }
        public string line8 { get; set; }
        public string line9 { get; set; }
        public string line10 { get; set; }
        public string line11 { get; set; }
        public string line12 { get; set; }
        public string line13 { get; set; }
        public string line14 { get; set; }
        public string line15 { get; set; }
        public string line16 { get; set; }
        public string line17 { get; set; }
        public string line18 { get; set; }
        public string line19 { get; set; }
        public string line20 { get; set; }
        public string internal_value1 { get; set; }
        public string internal_value2 { get; set; }
        public decimal balance_amount { get; set; }
        public decimal processed { get; set; }
        public string in_flag { get; set; }
        public int top_count { get; set; }
        public int down_count { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
        public string sourceid { get; set; }
        public string comment_code { get; set; }
    }

    public class tab_depend
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string dependant { get; set; }

        public string birth_date { get; set; }
        public string relationship { get; set; }
        public string depend_initial { get; set; }
        public int depend_count { get; set; }
    }

    public class tab_train_group
    {
        [Key, Column(Order = 0)]
        public string train_group { get; set; }

        public string group_name { get; set; }
        public string description { get; set; }
        public string skills_attached { get; set; }
        public string prof_level { get; set; }
        public decimal average_cost { get; set; }
        public string parameter_close { get; set; }
    }

    public class tab_train_plan
    {
        [Key, Column(Order = 0)]
        public int year_plan { get; set; }
        
        [Key, Column(Order = 1)]
        public string train_code { get; set; }

        [Key, Column(Order = 2)]
        public string start_date { get; set; }
        
        public string end_date { get; set; }
        public decimal cost { get; set; }
        public string venue1 { get; set; }
        public string venue2 { get; set; }
        public string venue3 { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_limit
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }
        
        [Key, Column(Order = 1)]
        public string staff_number { get; set; }

        [Key, Column(Order = 2)]
        public string code_to{ get; set; }

        [Key, Column(Order = 3)]
        public string limit_code { get; set; }

        [Key, Column(Order = 4)]
        public int length_service_from { get; set; }

        [Key, Column(Order = 5)]
        public int length_service_to { get; set; }

        [Key, Column(Order = 6)]
        public string limit_year { get; set; }

        public string limit_year_to { get; set; }
        public decimal limit_amount { get; set; }
        public string report_name { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_manning
    {
        [Key, Column(Order = 0)]
        public string para_code { get; set; }
        
        [Key, Column(Order = 1)]
        public string code_mann { get; set; }
        
        [Key, Column(Order = 2)]
        public string job_desc { get; set; }
   
        public decimal amount { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_train_attach
    {
        [Key, Column(Order = 0)]
        public string type_code { get; set; }
        
        [Key, Column(Order = 1)]
        public string para_code { get; set; }
        
        [Key, Column(Order = 2)]
        public string staff_number{ get; set; }
        
        [Key, Column(Order = 3)]
        public string train_code { get; set; }
        
        public string created_by { get; set; }
        public DateTime date_created { get; set; }

    }

    //public class tab_map
    //{
    //    [Key, Column(Order = 0)]
    //    public string para_code { get; set; }

    //    [Key, Column(Order = 1)]
    //    public string map_code { get; set; }

    //    [Key, Column(Order = 2)]
    //    public int count_map { get; set; }

    //    public string name1 { get; set; }
    //    public string value_date { get; set; }
    //    public string trans_type { get; set; }
    //    public string map_type { get; set; }
    //    public string line1 { get; set; }
    //    public string line2 { get; set; }
    //    public string line3 { get; set; }
    //    public string line4 { get; set; }
    //    public string line5 { get; set; }
    //    public string line6 { get; set; }
    //    public string line7 { get; set; }
    //    public string line8 { get; set; }
    //    public string line9 { get; set; }
    //    public string line10 { get; set; }
    //    public string line11 { get; set; }
    //    public string line12 { get; set; }
    //    public string line13 { get; set; }
    //    public string line14 { get; set; }
    //    public string line15 { get; set; }
    //    public string line16 { get; set; }
    //    public string line17 { get; set; }
    //    public string line18 { get; set; }
    //    public string line19 { get; set; }
    //    public string line20 { get; set; }
    //    public string map_code_type { get; set; }
    //    public string map_code_ind { get; set; }
    //    public string internal_use { get; set; }
    //    public string created_by { get; set; }
    //    public DateTime date_created { get; set; }
    //    public string amended_by { get; set; }
    //    public DateTime date_amended { get; set; }
    //}

    public class tab_pen_allow
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }
       
        [Key, Column(Order = 1)]
        public string codea { get; set; }
        
        [Key, Column(Order = 2)]
        public string indicatora { get; set; }
        
        [Key, Column(Order = 3)]
        public string group_code { get; set; }
        
        public string payroll_period { get; set; }
        public decimal allow_amount { get; set; }
        public decimal allow_percentage { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax_percentage { get; set; }
        public string date_ind { get; set; }
        public string start_date { get; set; }
        public string stop_date { get; set; }
        public decimal approval { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_pen_daily
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }
        
        [Key, Column(Order = 1)]
        public string daily_code { get; set; }
        
        [Key, Column(Order = 2)]
        public string group_code { get; set; }
        
        public string payroll_period { get; set; }
        public string allow_code { get; set; }
        public decimal no_of_days { get; set; }
        public decimal approval { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
    }

    public class tab_escalation
    {
        [Key, Column(Order = 0)]
        public string record_type { get; set; }

        [Key, Column(Order = 1)]
        public string trans_type { get; set; }

        [Key, Column(Order = 2)]
        public int approval_level { get; set; }

        public string escalated_user { get; set; }
        public int escalated_days { get; set; }
    }

    public class tab_app_category
    {
        [Key, Column(Order = 0)]
        public string form_cat { get; set; }
      
        [Key, Column(Order = 1)]
        public string appraisal_category { get; set; }
        
        [Key, Column(Order = 2)]
        public string appraisal_code { get; set; }
        
        [Key, Column(Order = 3)]
        public decimal sequence_cat { get; set; }

        public string description_cat { get; set; }
        public string  header_cat { get; set; }
        public string line_cat { get; set; }
        public string comment_cat { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }
    }

    public class tab_app_definition
    {
        [Key, Column(Order = 0)]
        public string code_definition { get; set; }

        public string name1 { get; set; }
        public string performance_form { get; set; }
        public string form_based { get; set; }
        public string current_period_definition { get; set; }
        public int  current_cycle_definition { get; set; }
        public string next_period_definition { get; set; }
        public int next_cycle_definition { get; set; }
        public string load_period_definition { get; set; }
        public int load_cycle_definition { get; set; }
        public int appraiser_no { get; set; }
        public string kbi_description { get; set; }
        public string kbi_manager { get; set; }
        public string grade_description { get; set; }
        public string field1_definition { get; set; }
        public string field2_definition { get; set; }
        public string menu_option { get; set; }
        public string eemployee_option { get; set; }
        public string parameter_close { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_app_goals
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string period_goal { get; set; }
        
        [Key, Column(Order = 2)]
        public int performance_cycle { get; set; }
        
        [Key, Column(Order = 3)]
        public int sequence_goal { get; set; }
        
        [Key, Column(Order = 4)]
        public int column_no { get; set; }

        [Column(TypeName = "text")]
        public string appraisal_text { get; set; }
        public decimal processed { get; set; }
        public string kra_goal { get; set; }
        public string input_user { get; set; }
        public DateTime input_date { get; set; }

    }

    public class tab_appraisal
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string code_definition { get; set; }

        [Key, Column(Order = 2)]
        public int team_review { get; set; }

        public int performance_cycle { get; set; }
        public string appraisal_year { get; set; }
        public string next_period { get; set; }
        public string performance_form { get; set; }
        public string form_based { get; set; }
        public string performance_code { get; set; }
        public string appraisal_grade { get; set; }
        public int duration_on_grade { get; set; }
        public string field1_addition { get; set; }
        public string field2_addition { get; set; }
        public string field3_addition { get; set; }
        public string field4_addition { get; set; }
        public string field5_addition { get; set; }
        public string appraiser1 { get; set; }
        public string appraiser2 { get; set; }
        public string appraiser3 { get; set; }
        public string appraiser4 { get; set; }
        public string appraiser5 { get; set; }
        public string appraiser6 { get; set; }
        [Column(TypeName = "text")]
        public string appraisee_comment { get; set; }
        [Column(TypeName = "text")]
        public string level1_comment1 { get; set; }
        public DateTime level1_date { get; set; }
        [Column(TypeName = "text")]
        public string level2_comment1 { get; set; }
        public DateTime level2_date { get; set; }
        [Column(TypeName = "text")]
        public string level3_comment1 { get; set; }
        public DateTime level3_date { get; set; }
        [Column(TypeName = "text")]
        public string level4_comment1 { get; set; }
        public DateTime level4_date { get; set; }
        [Column(TypeName = "text")]
        public string level5_comment1 { get; set; }
        public DateTime level5_date { get; set; }
        [Column(TypeName = "text")]
        public string level6_comment1 { get; set; }
        public DateTime level6_date { get; set; }
        public decimal processed { get; set; }
        public int top_count { get; set; }
        public int down_count { get; set; }
     
        public string input_user { get; set; }
        public DateTime input_date { get; set; }
        public string sourceid { get; set; }
        public DateTime last_date { get; set; }
        public string comment_code { get; set; }

    }

    public class tab_appraisal_default
    {
        [Key, Column(Order = 0)]
        public string category { get; set; }

        [Key, Column(Order = 1)]
        public string code_from { get; set; }
        
        [Key, Column(Order = 2)]
        public string appraisal_code { get; set; }
        
        [Key, Column(Order = 3)]
        public int sequence_no { get; set; }
        
        [Key, Column(Order = 4)]
        public int column_no { get; set; }

        [Column(TypeName = "text")]
        public string appraisal_text { get; set; }
        public int readonly_ind { get; set; }
        public string input_user { get; set; }
        public DateTime input_date { get; set; }

    }

    public class tab_staff_person
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        public int font_sizep { get; set; }
        public string font_family1 { get; set; }
        public string font_family2 { get; set; }
        public string mail_notification { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
        public string amended_by { get; set; }
        public DateTime date_amended { get; set; }

    }

    public class tab_transexpense
    {
        [Key, Column(Order = 0)]
        public string document_number { get; set; }

        public string trans_type { get; set; }
        public string staff_number { get; set; }
        public string document_type { get; set; }
        public string advance_number { get; set; }
        public string value_date { get; set; }
        public string cost_centre { get; set; }
        public string summary_description { get; set; }
        public decimal total_amount { get; set; }
        public decimal total_amount1 { get; set; }
        public decimal tax_amount { get; set; }
        public string advance_date { get; set; }
        public string expense_type1 { get; set; }
        public decimal balance_amount { get; set; }
        public decimal pending_amount { get; set; }
        public string user1_value { get; set; }
        public string user2_value { get; set; }
        public string user3_value { get; set; }
        public string user4_value { get; set; }
        public string user5_value { get; set; }
        public string user6_value { get; set; }
        public string user7_value { get; set; }
        public string user8_value { get; set; }
        public string user9_value { get; set; }
        public string user10_value { get; set; }
        public string customer_staff { get; set; }
        public decimal processed { get; set; }
        public string in_flag { get; set; }
        public int top_count { get; set; }
        public int down_count { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
        public string sourceid { get; set; }
        public string comment_code { get; set; }
    }

    public class tab_doctrans
    {
        [Key, Column(Order = 0)]
        public int sequence_no { get; set; }

        public string staff_number { get; set; }
        public string trans_type { get; set; }
        public string value_date { get; set; }
        public string document_type { get; set; }
        public byte[] picture1 { get; set; }
        public string document_name { get; set; }
        public string comment_code { get; set; }
    }

    public class tab_docpara
    {
        [Key, Column(Order = 0)]
        public int sequence_no { get; set; }

        public string hcode { get; set; }
        public string pcode { get; set; }
        public string document_type { get; set; }
        public byte[] picture1 { get; set; }
        public string document_name { get; set; }
        public string comment_code { get; set; }
    }

    public class tab_docpost
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string document_number { get; set; }

        [Key, Column(Order = 2)]
        public int sequence_no { get; set; }

        public string trans_type { get; set; }
        public string document_type { get; set; }
        public decimal total_amount { get; set; }
        public string value_date { get; set; }
        public string general_account { get; set; }
        public string analytical_account { get; set; }
        public string work_order { get; set; }
        public string description { get; set; }
        public decimal dr_amount { get; set; }
        public decimal cr_amount { get; set; }
        public decimal processed { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
        public string sourceid { get; set; }
        public string comment_code { get; set; }
    }

    public class tab_staff_skill
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string skill_code { get; set; }

        public string proficiency_level { get; set; }
        public string date_acquired { get; set; }
        public string created_by { get; set; }
        public DateTime date_created { get; set; }
    }

    public class tab_app_actbud
    {
        [Key, Column(Order = 0)]
        public int budget_sequence { get; set; }

        public string staff_number { get; set; }
        public string kra_code { get; set; }
        public string budget_year { get; set; }
        public string budget_date { get; set; }
        public string budget_date_to { get; set; }
        public string budget_target { get; set; }
        public decimal budget_score { get; set; }
        public string manager_summary { get; set; }
        public decimal manager_score { get; set; }
        public string budget_close { get; set; }
        public DateTime last_date { get; set; }
    }

    public class tab_app_actdet
    {
        [Key, Column(Order = 0)]
        public int activity_sequence { get; set; }


        public int budget_sequence { get; set; }

        public string staff_number { get; set; }
        public string kra_code { get; set; }
        public string budget_year { get; set; }
        public string activity_date { get; set; }
        public string staff_activity { get; set; }
        public string manager_comment { get; set; }
        public string activity_close { get; set; }
        public DateTime last_date { get; set; }
    }

    public class tab_staff_bal
    {
        [Key, Column(Order = 0)]
        public string staff_number { get; set; }

        [Key, Column(Order = 1)]
        public string payment_run { get; set; }

        public decimal freepay_ytd { get; set; }
        public decimal overtime_ytd { get; set; }
        public decimal tax_paid_ytd { get; set; }
        public decimal taxable_ytd { get; set; }
        public decimal payable_ytd { get; set; }
        public decimal gross_ytd { get; set; }
        public decimal basic_ytd { get; set; }
        public decimal weekdays_ytd { get; set; }
        public decimal weekend_ytd { get; set; }
        public decimal public_ytd { get; set; }
        public decimal payroll_allowance_ytd { get; set; }
        public decimal allowance_ytd { get; set; }
        public decimal deduction_ytd { get; set; }
        public decimal loan_payment_ytd { get; set; }
        public decimal taxable_allowance_ytd { get; set; }
        public decimal freepay { get; set; }
        public decimal bik_tax { get; set; }
        public int tax_month { get; set; }

    }

    public class tab_transexpense_details
    {
        [Key, Column(Order = 0)]
        public string document_number { get; set; }

        [Key, Column(Order = 1)]
        public int sequence_no { get; set; }

        public string staff_number { get; set; }
        public string date1 { get; set; }
        public string work_order1 { get; set; }
        public string line1 { get; set; }
        public decimal amount1 { get; set; }
        public string sourceid { get; set; }
    }

    public class tab_hreason
    {
        [Key, Column(Order = 0)]
        public int sequence_noh { get; set; }

        public string staff_number { get; set; }
        public string trans_type { get; set; }
        public string value_date { get; set; }
        public string reason1 { get; set; }
        public string staff_reject { get; set; }
        public DateTime date_reject { get; set; }

        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        public string line4 { get; set; }
        public string line5 { get; set; }
        public string line6 { get; set; }
        public string line7 { get; set; }
        public string line8 { get; set; }
        public string line9 { get; set; }
        public string line10 { get; set; }
        public string line11 { get; set; }
        public string line12 { get; set; }
        public string line13 { get; set; }
        public string line14 { get; set; }
        public string line15 { get; set; }
        public string line16 { get; set; }
        public string line17 { get; set; }
        public string line18 { get; set; }
        public string line19 { get; set; }
        public string line20 { get; set; }
        public string internal_value1 { get; set; }
        public string internal_value2 { get; set; }
        public decimal balance_amount { get; set; }
        public decimal processed { get; set; }
        public string request_user { get; set; }
        public DateTime input_date { get; set; }
        public string approval_user { get; set; }
        public DateTime date_approve { get; set; }
        public string sourceid { get; set; }
        public string transaction_type { get; set; }
        public string comment_code { get; set; }
        public decimal tax_amount { get; set; }
        public decimal total_amount1 { get; set; }
    }

}

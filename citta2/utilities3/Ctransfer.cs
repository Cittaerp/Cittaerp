using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using anchor1.Models;


namespace anchor1.utilities
{
    public class Ctransfer
    {

///      static DataClasses1DataContext context = new DataClasses1DataContext();
        private anchor1Context db = new anchor1Context();
        Cutil utils = new Cutil();

        string[] ws_field_code = new string[60];
        string[] ws_field_name = new string[60];
        string[] ws_field_valid = new string[60];
        string[] ws_field_text = new string[60];
        string[] ws_field_desc = new string[60];
        string[] ws_field_ind = new string[60];
        string[] ws_field_header = new string[60];
        string[] ws_field_length = new string[60];
        string[] ws_keyfield = new string[60];
        string[] ws_alpha = new string[60];
        int count_valid = 0; int count_invalid = 0;
        int w_count_new = 0; int w_count_delete = 0; int w_count_update = 0;
        Boolean yes_all_flag = false;
        string ws_select_line;
        string ws_select_field;
        string ws_screencode;
        string select2_field;
        string tablename;
        string para_str = "";
        string str_select; string selection_line;
        int count_where = 0;
        string p_userid;

        public Boolean allow_in(string filename, string sel_str,  DataSet ds, string puserid)
        {

            string str1;
            p_userid = puserid;

            string type1 = sel_str.Substring(0, 1);
            string ws_transfer_code = sel_str.Substring(1, 10);
            string sc_file_type = sel_str.Substring(11, 1);
            string sc_from_number = sel_str.Substring(12, 10);
            string sc_to_number = sel_str.Substring(22, 10);
            string sc_from_code = sel_str.Substring(32, 10);
            string sc_to_code = sel_str.Substring(42, 10);
            if (sel_str.Substring(52, 1) == "Y")
                yes_all_flag = true;

            string sheet_name = sel_str.Substring(53);
            //    kp_code = pass3_code
            if (sc_from_number == "")
                sc_from_number = "ALL";
            if (sc_to_number == "")
                sc_to_number = sc_from_number;
            if (sc_from_code == "")
                sc_from_code = "ALL";
            if (sc_to_code == "")
                sc_to_code = sc_from_code;

            //' read for type of transfer type from tab_array
            string ws_transfer_type = (from bg in db.tab_calc where bg.para_code == "TT" & bg.calc_code == ws_transfer_code select bg.report_name).FirstOrDefault();

            //'read the selection line and selection field from the 'TR' in report_name3 and report_name4
            var bglist = (from bg in db.tab_soft
                          where bg.para_code == "TR" & bg.report_code == ws_transfer_type
                          select bg).FirstOrDefault();

            ws_select_line = bglist.report_name3;
            ws_select_field = bglist.report_name4;
            ws_screencode = bglist.report_code;
            select2_field = bglist.report_name6;
            tablename = bglist.report_name10;

            Boolean where_flag = false;
            selection_line = "";
            str1 = "";
            if (!(sc_from_number == "ALL" || sc_from_number == ""))
            {
                str1 = ws_select_field + " between " + utils.pads(sc_from_number) + " and " + utils.pads(sc_to_number);
                where_flag = true;
            }

            if (!(sc_from_code == "ALL" || sc_from_code == ""))
            {
                if (where_flag)
                    str1 += " and ";

                str1 += select2_field + " between " + utils.pads(sc_from_code) + " and " + utils.pads(sc_to_code);
            }

            selection_line = str1;

            var bglist2 = from bg in db.tab_array
                          join bg1 in db.tab_soft
                          on new { a1 = bg.operand, a2 = "TR" + bg.source1 } equals new { a1 = bg1.report_code, a2 = bg1.para_code }
                          into bg2
                          from bg3 in bg2.DefaultIfEmpty()
                          where bg.array_code == ws_transfer_code
                          select new { bg, bg3 };

            foreach (var bt in bglist2.ToList())
            {
                int ctr = Convert.ToInt16(bt.bg.count_array);
                ws_field_code[ctr] = bt.bg.operand;
                ws_field_name[ctr] = bt.bg3.report_name2;
                ws_field_text[ctr] = bt.bg3.report_name3;
                ws_field_valid[ctr] = bt.bg3.report_name4;
                ws_field_desc[ctr] = bt.bg3.report_name1;
                ws_field_ind[ctr] = bt.bg3.numeric_ind;
                ws_field_length[ctr] = bt.bg3.rep_name1;
                ws_keyfield[ctr] = bt.bg3.source_cat;
            }

            if (type1 == "O")
            {
                allow_out_trans(filename);
                return true;
            }

            str_select = ws_select_line;
            count_where = ws_select_line.IndexOf("where");
            if (count_where > 0)
                str_select = ws_select_line.Substring(0, count_where);

            //' read the layout into array

            if (sc_file_type == "E")
            {

                for (int rec_ctr = 0; rec_ctr < ds.Tables[sheet_name].Rows.Count; rec_ctr++)
                {
                    for (int ws_count = 0; ws_count < 40; ws_count++)
                    {
                        if ((ds.Tables[sheet_name].Rows[rec_ctr][ws_count].ToString() == null))
                            ws_alpha[ws_count] = "";
                        else
                        {
                            ws_alpha[ws_count] = ds.Tables[sheet_name].Rows[rec_ctr][ws_count].ToString();

                            //            If out_rec.Fields(ws_count).Type = adDate Then ws_alpha(ws_count) = Right("0" + Trim(CStr(Day(out_rec.Fields(ws_count)))), 2) + "/" + Right("0" + Trim(CStr(Month(out_rec.Fields(ws_count)))), 2) + "/" + Trim(CStr(Year(out_rec.Fields(ws_count))))
                        }
                    }
                    if (ws_alpha[1] == "")
                        break;


                    in_allow_transfer();

                    count_valid++;
                }
            }
            else
            {
                string line;
                StreamReader sr = new StreamReader(filename);

                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    break_transfer(line, "|", "|");
                    line = sr.ReadLine();

                    in_allow_transfer();

                    count_valid++;
                }

                //close the file
                sr.Close();
            }

            utils.error_update("No of Record(s) Created = " + w_count_new.ToString());
            utils.error_update("No of Record(s) Updated = " + w_count_update.ToString());
            utils.error_update("No of Record(s) Deleted = " + w_count_delete.ToString());
            utils.error_update("No of Record(s) Rejected = " + count_invalid.ToString());
            utils.error_update("No of Record(s) = " + count_valid.ToString());

            return true;

        }

        private Boolean in_allow_transfer()
        {
            Boolean err_flag = true;
            //and_flag = False
            //   ' update_line ws_alpha
            string ws_mode = ws_alpha[0];
            string wh_string = get_where();

            if (ws_mode == "D")
            {
                delete_allow_rec(wh_string);
                return true;
            }
            string key_code = ws_alpha[1];


            Boolean write_flag = true;
            //' validate all inputs
            for (int ws_count = 1; ws_count < 60; ws_count++)
            {
                if (ws_field_code[ws_count] == "")
                    break;

                if (ws_field_valid[ws_count] == "NS")
                    err_flag = check_space(ws_alpha[ws_count], ws_field_desc[ws_count]);
                else if (ws_field_valid[ws_count] == "D")
                {
                    err_flag = check_transfer_date(ws_alpha[ws_count], ws_field_desc[ws_count]);
                    if (err_flag)
                        ws_alpha[ws_count] = utils.date_yyyymmdd(utils.date_check(ws_alpha[ws_count]));
                }
                else if (ws_field_valid[ws_count] == "NZ")
                    err_flag = check_zero(ws_alpha[ws_count], ws_field_desc[ws_count]);
                else if (ws_field_valid[ws_count] == "F")
                    err_flag = check_file_code(ws_alpha[ws_count], ws_field_desc[ws_count], ws_field_text[ws_count]);
                else if (ws_field_valid[ws_count] == "FZ")
                    err_flag = check_file_code(ws_alpha[ws_count], ws_field_desc[ws_count], ws_field_text[ws_count], true);
                else if (ws_field_valid[ws_count] == "O")
                    err_flag = check_option(ws_alpha[ws_count], ws_field_desc[ws_count], ws_field_text[ws_count]);
                else if (ws_field_valid[ws_count] == "DZ")
                {
                    err_flag = check_transfer_date(ws_alpha[ws_count], ws_field_desc[ws_count], true);
                    if (err_flag)
                        ws_alpha[ws_count] = utils.date_yyyymmdd(utils.date_check(ws_alpha[ws_count]));
                }

                if (!err_flag)
                    write_flag = false;
            }


            if (write_flag)
            {
                if (ws_mode == "U")
                    update_record_rtn(wh_string);
                else
                    new_record_rtn(wh_string);
            }
            else
                count_invalid++;

            return true;

        }


        private void new_record_rtn(string wh_stringp)
        {
            string str1 = str_select + wh_stringp;
            var bglist4 = db.Database.SqlQuery<vw_query>(str1);

            if (bglist4 != null)
                utils.error_update("Duplicate record for " + ws_alpha[1]);

            if (yes_all_flag)
                delete_allow_rec(wh_stringp);

            if (bglist4 == null || yes_all_flag)
            {
                //' breakup select line and create the insertion line
                string str0 = ") values (";
                str1 = str_select.Replace("select * from", "insert into");
                str1 += "(" + para_str + " amended_by";
                if (para_str.Length > 0)
                    str0 += utils.pads(ws_screencode) + ",";

                str0 += utils.pads(p_userid);

                //' check for tab_array which does not have created by
                int ws_pos = ws_select_line.IndexOf("tab_array");
                if (ws_pos == 0)
                {
                    str1 += ",created_by";
                    str0 += "," + utils.pads(p_userid);
                }

                for (int ws_count = 1; ws_count < 60; ws_count++)
                {
                    if (ws_field_name[ws_count] == "")
                        break;

                    str1 += "," + ws_field_name[ws_count];

                    if (ws_field_ind[ws_count] == "P")
                        str0 += "," + utils.pads(utils.period_yyyymm(utils.period_convert(ws_alpha[ws_count])));
                    else if (ws_field_ind[ws_count] == "N" || ws_field_ind[ws_count] == "A")
                        str0 += "," + (ws_alpha[ws_count]);
                    else
                        str0 += "," + utils.pads(ws_alpha[ws_count].Substring(0, Convert.ToInt16(ws_field_length[ws_count])));

                }

                str1 += str0 + ")";
                int norecords = db.Database.ExecuteSqlCommand(str1);

                if (norecords == 0)
                    utils.error_update(ws_alpha[1] + " : " + "Record not inserted");
                else
                {
                    write_log("N");
                    w_count_new++;
                }
            }

        }

        private void update_record_rtn(string wh_stringp)
        {
            Boolean where_flag;
            where_flag = false;

            string str1 = str_select.Replace("select * from", "update ");
            str1 += " set amended_by=" + utils.pads(p_userid) + ", date_amended=getutcdate(),";

            for (int ws_count = 1; ws_count < 60; ws_count++)
            {
                if (ws_field_name[ws_count] == "")
                    break;

                if (ws_keyfield[ws_count] != "K")
                {
                    if (where_flag)
                        str1 += ", ";

                    if (ws_field_ind[ws_count] == "P")
                    {
                        ws_alpha[ws_count] = utils.period_yyyymm(utils.period_convert(ws_alpha[ws_count]));
                        str1 += ws_field_name[ws_count] + " = " + ws_alpha[ws_count];
                    }
                    else if (ws_field_ind[ws_count] == "N" || ws_field_ind[ws_count] == "A")
                        str1 += ws_field_name[ws_count] + " = " + ws_alpha[ws_count];
                    else
                        str1 += ws_field_name[ws_count] + " = " + utils.pads(ws_alpha[ws_count].Substring(0, Convert.ToInt16(ws_field_length[ws_count])));

                    where_flag = true;
                }
            }

            //' get the where clause from the selection line
            str1 += wh_stringp;

            int norecords = db.Database.ExecuteSqlCommand(str1);
            if (norecords == 0)
                utils.error_update(ws_alpha[1] + " : Record not Updated");
            else
            {
                write_log("A");
                w_count_update++;
            }

        }

        private void allow_out_trans(string filename)
        {

            string st_string = "|";
            string str1 = "";
            if (count_where > 0)
                str1 = " and " + selection_line;
            else
                str1 = " where " + selection_line;

            str1 = ws_select_line + str1;

            StreamWriter sw = new StreamWriter(filename);

            //Write a line of text
            sw.WriteLine(write_header());

            var bglist2 = db.Database.SqlQuery<vw_query>(str1);

            foreach (var item in bglist2.ToList())
            {
                str1 = "U";
                for (int ws_count = 1; ws_count < 60; ws_count++)
                {
                    if (ws_field_code[ws_count] == "")
                        break;

                    string vtext = Convert.ToString(item.GetType().GetProperty(ws_field_name[ws_count]).GetValue(item));

                    if (ws_field_ind[ws_count] == "D")
                        str1 += st_string + utils.date_ddmmyyyy(vtext);
                    else if (ws_field_ind[ws_count] == "P")
                        str1 += st_string + utils.period_mmyyyy(vtext);
                    else
                        str1 += st_string + vtext;

                }

                sw.WriteLine(str1);

            }

            //Close the file
            sw.Close();

        }

        private string write_header()
        {

            string str1 = "Transfer Type";
            for (int ws_count = 1; ws_count < 60; ws_count++)
            {
                if (ws_field_code[ws_count] == "")
                    break;
                str1 += "|" + ws_field_header[ws_count];
            }

            return str1;

        }

        private void delete_allow_rec(string wh_stringp)
        {
            //write_log ("D")
            string str1 = str_select.Replace("Select * ", "Delete ");
            str1 += wh_stringp;
            int norecords = db.Database.ExecuteSqlCommand(str1);


            if (norecords == 0)
                count_invalid++;
            else
                w_count_delete++;


        }

        private void init_values()
        {
            for (int ws_count = 0; ws_count < 60; ws_count++)
            {
                ws_field_code[ws_count] = "";
                ws_field_name[ws_count] = "";
                ws_field_text[ws_count] = "";
                ws_field_desc[ws_count] = "";
                ws_field_ind[ws_count] = "";
            }

        }

        private void write_log(String opcode)
        {
            //Dim str1 As String, str2 As String
            //Dim ctr1 As Integer, ctr2 As Integer

            //ctr1 = InStr(1, ws_select_line, "tab")
            //ctr2 = InStr(1, ws_select_line, "where")
            //If ctr2 > 0 Then str2 = Mid(ws_select_line, ctr1 - 1, (ctr2 - ctr1 + 1))
            //If ctr2 = 0 Then str2 = Mid(ws_select_line, ctr1 - 1)


            //    utils.write_plog3 str2, opcode, wh_string, ws_screencode, ws_screencode, ws_alpha(1), "A"

        }

        private string get_where()
        {
            Boolean and_flag;
            string where_string;
            int ws_count1;
            string wh_string;

            and_flag = false;
            wh_string = " ";
            for (int ws_count = 1; ws_count < 60; ws_count++)
            {
                if (ws_field_name[ws_count] == "")
                    break;
                if (ws_keyfield[ws_count] == "K")
                {
                    if (and_flag)
                        wh_string += " and ";
                    and_flag = true;
                    wh_string += ws_field_name[ws_count] + " = ";
                    if (ws_field_ind[ws_count] == "D")
                        wh_string += utils.pads(utils.date_yyyymmdd(ws_alpha[ws_count]));
                    else if (ws_field_ind[ws_count] == "P")
                        wh_string += utils.pads(utils.period_yyyymm(utils.period_convert(ws_alpha[ws_count])));
                    else if (ws_field_ind[ws_count] == "N" || ws_field_ind[ws_count] == "A")
                        wh_string += ws_alpha[ws_count];
                    else
                        wh_string += utils.pads(ws_alpha[ws_count].Substring(0, Convert.ToInt16(ws_field_length[ws_count])));

                }

            }

            //'check if where already in select statement and create a comprehensive where statement
            string str1 = "";

            where_string = " where ";
            int count_where = ws_select_line.IndexOf("where");
            if (count_where > 0)
            {
                str1 = ws_select_line.Substring(0, count_where);
                where_string = " and ";
                //' isolate the field name after the where
                ws_count1 = ws_select_line.IndexOf(" ", count_where);
                int ws_count = ws_select_line.IndexOf("=", ws_count1);
                para_str = ws_select_line.Substring(ws_count1, ws_count - ws_count1) + ", ";
            }

            wh_string = " " + str1 + where_string + wh_string;

            return wh_string;
        }


        private Boolean check_space(string c_field, string c_message)
        {
            Boolean err_flag = true;
            if (c_field == "")
            {
                utils.error_update(ws_alpha[1] + " : " + c_message + " must not be spaces");
                err_flag = false;
            }
            return err_flag;

        }

        private Boolean check_zero(string c_field, string c_message)
        {
            Boolean err_flag = true;
            if (Convert.ToInt16(c_field) == 0)
            {
                utils.error_update(ws_alpha[1] + " : " + c_message + " must not be zero");
                err_flag = false;
            }
            return err_flag;

        }

        private Boolean check_transfer_date(string c_value, string c_message, Boolean blank_flag = false)
        {
            Boolean err_flag = true;

            if (blank_flag && Convert.ToInt16(c_value) == 0)
                return err_flag;

            string ws_temp = utils.date_check(c_value);
            if (ws_temp == "")
            {
                utils.error_update(ws_alpha[1] + " : " + c_message + " not valid Date");
                err_flag = false;
            }
            return err_flag;

        }

        private Boolean check_option(string c_value, string c_message, string c_option)
        {
            string[] w_opt = new string[10];
            string w_char; string w_join_char = "";
            int w_count; Boolean err_flag;
            w_count = 0;

            for (int ws_count = 1; ws_count <= c_option.Length; ws_count++)
            {
                w_char = c_option.Substring(ws_count, 1);
                if (w_char == ",")
                {
                    w_opt[w_count] = w_join_char;
                    w_join_char = "";
                    w_count++;
                }
                else
                    w_join_char += w_char;

            }
            w_opt[w_count] = w_join_char;

            for (int ws_count = 0; ws_count <= w_count; ws_count++)
            {
                if (c_value == w_opt[ws_count])
                    return true;
            }

            utils.error_update(ws_alpha[1] + " : " + c_message + " Can only be " + c_option);
            return false;

        }

        private Boolean check_file_code(string c_value, string c_message, string c_select, Boolean blank_flag = false)
        {

            if (blank_flag && c_value == "")
                return true;

            string str1 = c_select + utils.pads(c_value);

            int recno = db.Database.ExecuteSqlCommand(str1);

            if (recno == 0)
            {
                utils.error_update(ws_alpha[1] + " : " + c_message + " not Defined");
                return false;
            }

            return true;

        }

        private Boolean check_step(string c_value, string c_message, string c_cat)
        {

            if (c_value == "")
                return true;
            else
            {
                if (Convert.ToInt16(c_value) == 0)
                    return true;
                else
                {
                    string str1 = "select '1'c1 from tab_array where para_code ='11' and array_code=" + utils.pads(c_cat) + " and count_array=" + c_value;
                    var str2 = db.Database.SqlQuery<vw_query>(str1);
                    if (str2 == null)
                    {
                        utils.error_update(ws_alpha[1] + " : " + c_message + " not Defined for category");
                        return false;
                    }
                }
            }
            return true;
        }

        private Boolean break_transfer(string t_line, string st_string, string sp_string)
        {

            int st_pos;
            int ws_count = 0;
            int t_start = 0;

            do
            {
                st_pos = t_line.IndexOf(st_string, t_start);
                if (st_pos == 0)
                    st_pos = t_line.Length + 1;
                if (st_pos - t_start != 0)
                    ws_alpha[ws_count] = t_line.Substring(t_start, st_pos - t_start);
                else
                    ws_alpha[ws_count] = "";

                ws_count++;
                t_start = st_pos + 1;
                if (t_start > t_line.Length)
                    break;
            } while (true);

            return true;

        }



    }
}
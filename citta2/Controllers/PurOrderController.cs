using CittaErp.Models;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class PurOrderController : Controller
    {
        AP_001_PUROT AP_001_PUROT = new AP_001_PUROT();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        

        string ptype = "";
        bool err_flag = true;

        //
        // GET: /Citta/
        public ActionResult Index(string ptype1 = "")
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            if (ptype1 != "")
            {
                psess.temp0 = ptype1;
            }
            Session["psess"] = psess;
            
            ptype = psess.temp0.ToString();
               
           return RedirectToAction("Edit");
            

        }
   
        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            initial_rtn();
            readmsg();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            glay = glay_in;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            readmsg();
            return View(glay);
        }

        public ActionResult Edit()
        {

            ViewBag.action_flag = "Edit";

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            initial_rtn();
            ptype = psess.temp0.ToString();
            readmsg();
            read_record();

            return View(glay);

        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt)
        {
            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            glay = glay_in;

            update_file();
            if (err_flag)
                return RedirectToAction("Index");

            initial_rtn();
            readmsg();
            return View(glay);
        }
    
        private void update_file()
        {
            err_flag = true;
            validation_routine();

            if (err_flag)
                update_record();

        }

        private void update_record()
        {
            string sqlstr = "delete from AP_001_PUROT where parameter_code =" + util.sqlquote(ptype);
            db.Database.ExecuteSqlCommand(sqlstr);
            if (glay.vwstring0 == "Y")
            {
                if (glay.vwstring1 != "")
                {
                    
                    sqlstr = "  insert into AP_001_PUROT(parameter_code,sequence_type,order_type,numeric_size,order_prefix,order_sequence,created_by) Values ( ";
                    sqlstr += util.sqlquote(ptype) + "," + util.sqlquote(glay.vwstring0) + ", 'single' ,";
                    sqlstr += glay.vwint0.ToString() + "," + util.sqlquote(glay.vwstring1) + ",";
                    sqlstr += glay.vwint1.ToString() + "," + util.sqlquote(pubsess.userid) + ")";
                    db.Database.ExecuteSqlCommand(sqlstr);
                }
            }
            else
                if (glay.vwstring0 == "N")
                {

                    for (int dtr = 0; dtr < glay.vwstrarray2.Length; dtr++)
                    {
                        if (glay.vwstrarray2[dtr] != "")
                        {
                            sqlstr = "  insert into AP_001_PUROT(parameter_code,sequence_type,order_type,numeric_size,order_prefix,order_sequence,created_by) Values ( ";
                            sqlstr += util.sqlquote(ptype) + "," + util.sqlquote(glay.vwstring0) + "," + util.sqlquote(glay.vwstrarray3[dtr]) + ",";
                            sqlstr += glay.vwint0.ToString() + "," + util.sqlquote(glay.vwstrarray2[dtr]) + ",";
                            sqlstr += glay.vwitarray0[dtr].ToString() + "," + util.sqlquote(pubsess.userid) + ")";
                            db.Database.ExecuteSqlCommand(sqlstr);
                        }
                    }

                }

        }

        private void validation_routine()
        {
           
        }

        private void read_record()
        {
            AP_001_PUROT = (from bk1 in db.AP_001_PUROT
                            where bk1.parameter_code == ptype
                            select bk1).FirstOrDefault();

            header_rtn();
            if (AP_001_PUROT != null)
            {
                glay.vwstring0 = AP_001_PUROT.sequence_type;
                glay.vwint0 = AP_001_PUROT.numeric_size;
                if (glay.vwstring0 == "Y")
                {

                    glay.vwstring1 = AP_001_PUROT.order_prefix;
                    glay.vwint1 = AP_001_PUROT.order_sequence;
                }
                else
                    if (glay.vwstring0 == "N")
                    {
                        var preseq = from bg1 in db.GB_999_MSG
                                     join bg in db.AP_001_PUROT
                                      on new { a1 = bg1.code_msg } equals new { a1 = bg.order_type }
                                      into bg2
                                     from bg3 in bg2.DefaultIfEmpty()
                                     where bg1.type_msg == ptype && bg3.parameter_code == ptype
                                     orderby bg1.code_msg
                                     select new { bg1, bg3 };
                        int wtr = 0;
                        foreach (var item in preseq.ToList())
                        {
                            glay.vwstrarray3[wtr] = item.bg1.code_msg;
                            glay.vwstrarray1[wtr] = item.bg1.name1_msg;
                            if (item.bg3 != null)
                            {
                                glay.vwstrarray2[wtr] = item.bg3.order_prefix;
                                glay.vwitarray0[wtr] = item.bg3.order_sequence;
                            }
                            wtr++;
                        }
                    }
            }
        }

        private void initial_rtn()
        {
            glay.vwdclarray0 = new decimal[50];
            glay.vwstrarray1 = new string[20];
            glay.vwstrarray2 = new string[20];
            glay.vwstrarray3 = new string[20];
            glay.vwitarray0 = new int[20];
            glay.vwstring0 = "N";
            glay.vwlist0 = new List<querylay>[20];
        }
        private void select_query()
        {
         
        }

        private void error_message()
        {

        }

        [HttpPost]
        public ActionResult delete_list(string id)
        {
            // write your query statement
            string sqlstr = "delete from [dbo].[AP_001_PUROT] where discount_code=" + util.sqlquote(id);
             db.Database.ExecuteSqlCommand(sqlstr);


            return RedirectToAction("Index");
        }


        public void readmsg ()
        {
            var preseq = from bg in db.GB_999_MSG
                         where bg.type_msg == ptype
                         orderby bg.code_msg
                         select bg;
            int wtr = 0;
            foreach (var item in preseq.ToList())
            {
                glay.vwstrarray3[wtr] = item.code_msg;
                glay.vwstrarray1[ wtr] = item.name1_msg;
                wtr++;
            }
        }

        private void header_rtn()
        {

            if (ptype == "PURSEQ")
            {
                psess.temp1 = "Purchase Transaction Sequencing Maintainance";

            }
            if (ptype == "SLSEQ")
            {
                psess.temp1 = "Sales Transaction Sequencing Maintainance";

            }
            Session["psess"] = psess;
            
        }

	}
}
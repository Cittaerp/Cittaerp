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
    public class Term_transController : Controller
    {
        GB_001_PTYS GB_001_PTYS = new GB_001_PTYS();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess;
        psess psess;
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        string ptype = "";
        bool err_flag = true;
        string delmsg = "";
        string action_flag = "";
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

            header_rtn();
            if (ptype == "")
                error_message();

            var type = from bg in db.AR_001_CUSTM
                       select bg;
            ViewBag.ctype = new SelectList(type.ToList(), "customer_code", "cust_biz_name", glay.vwstring0);


            string str = "select contract_id query0, leeds_name+ '-' + contract_id query1 from [dbo].[IV_001_PC] where leeds_name <> '' union all";
            str += " select contract_id query0, cust_biz_name+ '-' + contract_id query1 from [dbo].[IV_001_PC], [dbo].[AR_001_CUSTM] where customer_id = customer_code";
            var emp1 = db.Database.SqlQuery<querylay>(str);
            ViewBag.cont = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring0);
            return View(glay);

        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(vw_genlay glay_in)
        {
            delete_record();
            db.SaveChanges();
            return View("index");
        }
        public ActionResult Create1()
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            header_rtn();
            select_query();
            return View(glay);
        }
        [HttpPost]
        public ActionResult Create1(vw_genlay glay_in, HttpPostedFileBase[] photofile)
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            photo1 = photofile;
            update_file();

            if (err_flag)
                return RedirectToAction("Create");

            select_query();
            return View(glay);
        }
        public ActionResult Create(string ptype1 = "")
        {
            util.init_values();

            pubsess = (pubsess)Session["pubsess"];
            psess = (psess)Session["psess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            if (ptype1 != "")
            {
                psess.temp0 = ptype1;
            }

            ptype = psess.temp0.ToString();

            header_rtn();
            if (ptype == "")
                error_message();

            select_query();
            return View(glay);

        }
        [HttpPost]
        public ActionResult Create(vw_genlay glay_in, string headtype = "D")
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            ViewBag.action_flag = "Create";
            action_flag = "Create";
            glay = glay_in;
            if (headtype != "send_app")
            {
                loaddetails();

                if (err_flag)
                {
                    header_rtn();
                    select_query();
                    return View(glay);
                }
            }
            if (headtype == "send_app")
            {
                update_file();
                if (err_flag)
                    return RedirectToAction("Create");
            }
            header_rtn();
            select_query();
            return View(glay);
        }

        public ActionResult Edit(string key1)
        {
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
            ViewBag.plabel = "Site";

            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            GB_001_PTYS = db.GB_001_PTYS.Find(ptype, key1);
            if (GB_001_PTYS != null)
                read_record();

            select_query();
            return View(glay);
        }

        [HttpPost]
        public ActionResult Edit(vw_genlay glay_in, string id_xhrt, HttpPostedFileBase[] photofile)
        {
            psess = (psess)Session["psess"];
            ptype = psess.temp0.ToString();
            pubsess = (pubsess)Session["pubsess"];
            action_flag = "Edit";
            ViewBag.action_flag = "Edit";
            glay = glay_in;

            if (id_xhrt == "D")
            { //delete record
                delete_record();
                return RedirectToAction("Index");
            }

            photo1 = photofile;
            update_file();
            if (err_flag)
                return RedirectToAction("Index");

            select_query();
            return View(glay);
        }

        private void loaddetails()
        {
            string cust = glay.vwstring1;
            string contract = glay.vwstring0;
            
            ModelState.Clear();
            var curlist = (from bg in db.IV_001_PC
                           join bg1 in db.IV_001_ITEM
                           on new { a1 = bg.item_code } equals new { a1 = bg1.item_code }
                           into bg2
                           from bg3 in bg2.DefaultIfEmpty()
                           join gf in db.AR_001_CUSTM
                           on new {a1=bg.customer_id} equals new {a1=gf.customer_code}
                           into gf1
                           from gf2 in gf1.DefaultIfEmpty()
                           join he in db.AR_001_PMTRX
                           on new{a1= bg3.item_code} equals new {a1= he.item_code}
                           into he1
                           from he2 in he1.DefaultIfEmpty()
                           where bg.contract_id == contract && he2.tenor == bg.tenor
                           select new {bg, bg3, gf2, he2}).FirstOrDefault();
            glay.vwstring0 = contract.ToString();
            glay.vwstring1 = cust;
            glay.vwstring2 = glay.vwstring0 + "-" + curlist.gf2.cust_biz_name;
            glay.vwstring3 = curlist.bg3.item_code;
            glay.vwdecimal0 = curlist.bg.sales_val-curlist.bg.sales_com;
            glay.vwdecimal1 = curlist.bg.tenor;
            glay.vwdecimal2 = glay.vwdecimal0 - curlist.bg.current_balance;
            glay.vwdecimal3 = curlist.bg.current_balance;

            var penalty = (from bg in db.GB_001_COY
                           where bg.id_code == "COYPPRTY"
                           select bg).FirstOrDefault();

            decimal penalty_amt = 0; decimal flat_amt = 0; decimal percent = 0;
            if (ptype == "01")
            {
             decimal.TryParse(penalty.field11, out flat_amt);  
                decimal.TryParse(penalty.field12, out percent);
                penalty_amt = flat_amt ==0 ?(percent / 100) * curlist.he2.selling_price_class1 : flat_amt;
                glay.vwstring6 = penalty.field13;
            }
            else if (ptype == "02")
            {
                decimal.TryParse(penalty.field15, out flat_amt);
                decimal.TryParse(penalty.field16, out percent);
                penalty_amt = flat_amt == 0 ? (percent / 100) * glay.vwdecimal5 : flat_amt;
                glay.vwstring6 = penalty.field14;
            }
            glay.vwdecimal4 = penalty_amt;
          
        }
        private void delete_record()
        {
            ptype = psess.temp0.ToString();
            if (util.delete_check(ptype, glay.vwstring0))
            {
                GB_001_PTYS = db.GB_001_PTYS.Find(ptype, glay.vwstring0);
                db.GB_001_PTYS.Remove(GB_001_PTYS);
                db.SaveChanges();
            }
            else
            {
                string kname = psess.temp2.ToString();
                delmsg = kname +"in Use";
                ModelState.AddModelError(String.Empty, delmsg);
                err_flag = false;

            }
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
           if(action_flag == "Create")
            {
                GB_001_PTYS = new GB_001_PTYS();
                GB_001_PTYS.created_by = pubsess.userid;
                GB_001_PTYS.approval_by = pubsess.userid;
                GB_001_PTYS.approval_level = 0;
                GB_001_PTYS.created_date = DateTime.UtcNow;
                GB_001_PTYS.approval_date = DateTime.UtcNow;
                GB_001_PTYS.contract_id = glay.vwstring0;
                //string sqlstr = "select isnull(max(parameter_code),0) vwint1 from GB_001_PTYS where parameter_type=" + ptype;
                //var sql1 = db.Database.SqlQuery<vw_genlay>(sqlstr).FirstOrDefault();
                //glay.vwdecimal4 = sql1.vwint1 + 1;
               // GB_001_PTYS.delete_flag = "N";
                
            }
            else
            {
                GB_001_PTYS = db.GB_001_PTYS.Find(glay.vwstring0, ptype);
            }
            GB_001_PTYS.flag = ptype;
            GB_001_PTYS.note = glay.vwstring5;
            GB_001_PTYS.transfer_to = glay.vwstring4;
            GB_001_PTYS.penalty = glay.vwdecimal4;
            GB_001_PTYS.current_value = glay.vwdecimal5;
            GB_001_PTYS.gl_account_code = "";
            GB_001_PTYS.note = "";
            if (ptype == "01")
                GB_001_PTYS.transfer_to = "";
           if(action_flag == "Create")
                db.Entry(GB_001_PTYS).State = EntityState.Added;
            else
                db.Entry(GB_001_PTYS).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }

            catch (Exception err)
            {
                if (err.InnerException == null)
                    ModelState.AddModelError(String.Empty, err.Message);
                else
                    ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);

                err_flag = false;
            }
            if (err_flag)
            {
                if (ptype == "01")
                {
                    util.update_entry("TERM", GB_001_PTYS.contract_id, pubsess.userid);
                    //if (glay.vwbool1)
                    //{
                    //    string str = " update GB_001_PTYS set gl_account_code = 'N' where parameter_type = '01' and parameter_code !=" + Convert.ToInt16(glay.vwdecimal4);
                    //    db.Database.ExecuteSqlCommand(str);
                    //}
                }
                if (ptype == "02")
                {
                    util.update_entry("TRAN", GB_001_PTYS.contract_id, pubsess.userid);
                }
                if (ptype == "03")
                {
                    string dflag = glay.vwint0.ToString();
                    util.parameter_deleteflag("010", dflag);
                    //string str = "update GL_001_CHART set delete_flag ='Y' from GL_001_CHART a, GB_001_PTYS b where a.account_code = b.gl_account_code";
                    //str += " and parameter_code =" + util.sqlquote(glay.vwstring0);
                    //db.Database.ExecuteSqlCommand(str);

                }
                {
                    //string pcod = GB_001_PTYS.parameter_code.ToString();
                   // util.write_document(ptype, pcod, photo1, glay.vwstrarray9);

                    //GB_001_DOC gdoc = new GB_001_DOC();
                    //gdoc.screen_code = ptype;
                    //gdoc.document_code = pcod;
                    //gdoc.description = "";
                    //gdoc.created_by = pubsess.userid;
                    //gdoc.modified_by = pubsess.userid;
                    //gdoc.created_date = DateTime.UtcNow;
                    //gdoc.modified_date = DateTime.UtcNow;
                    //gdoc.active_status = "Y";
                    //gdoc.note = "";
                    //foreach (var bg in photo1)
                    //{
                    //    if (bg != null)
                    //    {
                    //        byte[] uploaded = new byte[bg.InputStream.Length];
                    //        bg.InputStream.Read(uploaded, 0, uploaded.Length);
                    //        gdoc.document_image = uploaded;
                    //        db.Entry(gdoc).State = EntityState.Added;
                    //        db.SaveChanges();
                    //    }
                    //}
                    //if (glay.vwstrarray9 != null)
                    //{
                    //    for (int pctr = 0; pctr < glay.vwstrarray9.Length; pctr++)
                    //    {
                    //        if (!(string.IsNullOrWhiteSpace(glay.vwstrarray9[pctr])))
                    //        {
                    //            int seqno = Convert.ToInt16(glay.vwstrarray9[pctr]);
                    //            GB_001_DOC gdoc1 = db.GB_001_DOC.Find(seqno);

                    //            db.GB_001_DOC.Remove(gdoc1);
                    //            db.SaveChanges();
                    //        }
                    //    }
                    //}

                }

            }

        }

        private void validation_routine()
        {
  

        }


        private void select_query()
        {
            var type = from bg in db.AR_001_CUSTM
                       select bg;
            ViewBag.cus = new SelectList(type.ToList(), "customer_code", "cust_biz_name");


            //string str = "select contract_id query0, contract_id + '-' + leeds_name query1 from [dbo].[IV_001_PC] where leeds_name <> '' and current_balance >= 0  union all";
            //str += " select contract_id query0, contract_id + '-' + cust_biz_name query1 from [dbo].[IV_001_PC], [dbo].[AR_001_CUSTM] where customer_id = customer_code";

            string str = "select isnull(contract_id, 0) query0, isnull(contract_id + '-'+ cust_biz_name, 'no name') query1";
            str += " from [dbo].[IV_001_PC] a ";
            str += " LEFT outer JOIN [dbo].[AR_001_CUSTM] b  on (customer_id = customer_code )";
            str += " where  a.current_balance >= 0";
            var emp1 = db.Database.SqlQuery<querylay>(str);
            ViewBag.cont = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring0);
        }

        private void read_record()
        {
            glay.vwbool0 = false;

            glay.vwstring5 = GB_001_PTYS.note;
            glay.vwstring4 = GB_001_PTYS.transfer_to;
            glay.vwdecimal4 = GB_001_PTYS.penalty;
            glay.vwdecimal5 = GB_001_PTYS.current_value;
            //glay.vwstring1 = GB_001_PTYS.parameter_name;
            //glay.vwstring5 = GB_001_PTYS.parameter_name;
            //glay.vwstring2 = GB_001_PTYS.gl_account_code;
            //glay.vwstring3 = GB_001_PTYS.note;
            //glay.vwstring4 = GB_001_PTYS.con_state_link;
            //if (ptype == "14")m
            //    glay.vwstring2 = GB_001_PTYS.con_state_link;
            //if (GB_001_PTYS.active_status == "Y")
            //    glay.vwbool0 = true;
            //string pcod = GB_001_PTYS.parameter_code.ToString();
            //var bglist = from bg in db.GB_001_DOC
            //             where bg.screen_code == ptype && bg.document_code == pcod
            //             orderby bg.document_sequence
            //             select bg;

            //ViewBag.anapict = bglist.ToList();

        }

        [HttpPost]
        public ActionResult loadcontract(string id)
        {
            
               var hdet = from bg in db.IV_001_PC
                          join bf in db.AR_001_CUSTM
                          on new { a1 = bg.customer_id } equals new { a1 = bf.customer_code}
                          into bf1
                          from bf2 in bf1.DefaultIfEmpty()
                          where bg.customer_id==id && bg.current_balance >= 0
                       select new
                       {
                           c1 = bg.contract_id,
                           c2 = bg.contract_id + "&nbsp; &nbsp;" + bf2.cust_biz_name
                       };



                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    hdet.ToArray(),
                                    "c1",
                                    "c2")
                               , JsonRequestBehavior.AllowGet);
                //}
                return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult delete_list(string id)
        {
            err_flag = true;
            glay.vwstring0 = id;
            delete_record();

            if (!err_flag)
            {
                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "1", Text = delmsg });
                if (HttpContext.Request.IsAjaxRequest())
                    return Json(new SelectList(
                                    ary.ToArray(),
                                    "Value",
                                    "Text")
                                   , JsonRequestBehavior.AllowGet);


            }
            return RedirectToAction("Index");
        }

        private void header_rtn()
        {

            if (ptype == "01")
            {
                psess.temp1 = "Termination Screen";
                psess.temp3 = "Termination Listing";

            }
            if (ptype == "02")
            {
                psess.temp1 = "3rd Party Transfer Screen";
                psess.temp3 = "3rd Party Transfer Listing";

            }
            Session["psess"] = psess;
            
        }

        private void get_contract()
        {

            string str = "select contract_id query0, leeds_name+ '-' + contract_id query1 from [dbo].[IV_001_PC] where leeds_name <> '' union all";
            str += " select contract_id query0, cust_biz_name+ '-' + contract_id query1 from [dbo].[IV_001_PC], [dbo].[AR_001_CUSTM] where customer_id = customer_code";
            var emp1 = db.Database.SqlQuery<querylay>(str);
            ViewBag.cid = new SelectList(emp1.ToList(), "query0", "query1", glay.vwstring0);

          }
     
        private void error_message()
        {

        }


        public ActionResult show_doc(int id)
        {
            var bglist = (from bg in db.GB_001_DOC
                          where bg.document_sequence == id
                          select bg).FirstOrDefault();


            byte[] imagedata = bglist.document_image;
            return File(imagedata, "png");
        }


    }
}


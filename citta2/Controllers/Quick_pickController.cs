using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;
using CittaErp.Models;
using System.Data.Entity;
using System.IO;
using CittaErp.utilities;
using System.Drawing;


namespace CittaErp.Controllers
{
    public class Quick_pickController : Controller
    {
        //
        // GET: /Quick_pick/
        IV_001_ITEM IV_001_ITEM = new IV_001_ITEM();
        //QP_001_QUPD QP_001_QUPD = new QP_001_QUPD();


        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        HttpPostedFileBase[] photo1;

        bool err_flag = true;
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult show(string id)
        //{
        //    var item = from bg in db.IV_001_ITEM
        //              select new vw_genlay
        //                           {
        //                               picture12 = bg.item_picture,
        //                               //vwstring0 = bg.item_url
        //                               vwstring1 = bg.item_code
        //                           };

        //    return View(item.ToList());
        //}

        public ActionResult show(string id)
        {

            var item = (from bg in db.IV_001_ITEM
                        where bg.item_code == id
                        select bg).FirstOrDefault();

            byte[] imagedata = item.item_picture;
            return File(imagedata, "image/png");
        }

        public ActionResult View_pick ()
        {
            var item = from bg in db.IV_001_ITEM
                       select bg;

           ViewBag.item_pic = item.ToList();
            return View();
        }
         
        [HttpPost]
        public ActionResult item_details(string id)
        {
           string b1 = "";
           decimal b2 = 0;
            var item = (from bg in db.IV_001_ITEM
                        where bg.item_code == id
                        select bg).FirstOrDefault();
            if (item !=null)
            {
                            b1 = item.item_name;
                            b2 = item.selling_price_class1;
                        };


            List<SelectListItem> ary = new List<SelectListItem>();
           ary.Add(new SelectListItem { Value = "1", Text = b1 });
           ary.Add(new SelectListItem { Value = "2", Text = b2.ToString() });
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                ary.ToArray(),
                                "Value",
                                "Text")
                           , JsonRequestBehavior.AllowGet);


            return RedirectToAction("View_pick");
        }



        public void read_image()
        {
            List<byte[]> mylist = new List<byte[]>();

            byte[] load = new byte [] { };
            var bglist = from bg in db.IV_001_ITEM
                         select bg.item_picture;
            mylist = bglist.ToList();
           // load =  new byte [mylist.ToArray().Length];
            ViewBag.anapict = mylist;
        }

        //public ActionResult Create()
        //{      
        //    ViewBag.action_flag = "Create";
        //    pubsess = (pubsess)Session["pubsess"];
        //    // initial_rtn();
        //    // select_query();
        //    int[] ball = new int[] { };
        //    ArrayList log = new ArrayList();
        //    int a = 0;
        //    int b = 0;
        //    var number = (from bh in db.QP_001_QUPD
        //                  select bh.quick_layout_id).ToList();

        //    ball = (int[])log.ToArray(typeof(int));

        //    if (ball.Length==0)
        //    {
        //        b = a + 1;
        //    }

        //    foreach (var num in number)
        //    {
        //        int mun = num;
        //        log.Add(mun);
        //        int[] link = (int[])log.ToArray(typeof(int));
        //        a = link.Last();
        //        b = a + 1;
        //    }

        //    glay.vwint0 = b;
        //    return View(glay);
        //}
        
        //[HttpPost]
        //public ActionResult Create(vw_genlay glay_in)
        //{
        //    util.init_values();
        //    pubsess = (pubsess)Session["pubsess"];
        //    glay = glay_in;
        //   // update_file();
        //    if (err_flag)
        //        return RedirectToAction("Create");
        //    //initial_rtn();
        //    //select_query();
        //    return View(glay);
        //}

        //private void update_file()
        //{
        //    err_flag = true;
        //    validation_routine();

        //    if (err_flag)
        //        update_record();

        //}

        //private void update_record()
        //{
        //   if(action_flag == "Create")
        //    {
        //        QP_001_QUPD = new QP_001_QUPD();
        //        QP_001_QUPD.created_by = pubsess.userid;
        //        QP_001_QUPD.created_date = DateTime.UtcNow;
        //    }
        //    else
        //    {
        //        QP_001_QUPD = db.QP_001_QUPD.Find(glay.vwint0);
        //    }
        //    QP_001_QUPD.note = "";
        //    QP_001_QUPD.quick_layout_id = glay.vwint0;
        //    QP_001_QUPD.layout_name = string.IsNullOrWhiteSpace(glay.vwstring0) ? "" : glay.vwstring0;
        //    QP_001_QUPD.line_nbr = glay.vwint1;
        //    QP_001_QUPD.item_id = string.IsNullOrWhiteSpace(glay.vwstring1) ? "" : glay.vwstring1;
        //    QP_001_QUPD.variant_id = string.IsNullOrWhiteSpace(glay.vwstring2) ? "" : glay.vwstring2;

        //    QP_001_QUPD.modified_date = DateTime.UtcNow;
        //    QP_001_QUPD.modified_by = pubsess.userid;
        //    QP_001_QUPD.inactive_status = glay.vwbool0 ? "Inactive" : "Active";
        //    QP_001_QUPD.note = string.IsNullOrWhiteSpace(glay.vwstring3) ? "" : glay.vwstring3;

        //   if(action_flag == "Create")
        //        db.Entry(QP_001_QUPD).State = EntityState.Added;
        //    else
        //        db.Entry(QP_001_QUPD).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }

        //    catch (Exception err)
        //    {
        //        if (err.InnerException == null)
        //            ModelState.AddModelError(String.Empty, err.Message);
        //        else
        //            ModelState.AddModelError(String.Empty, err.InnerException.InnerException.Message);
        //        err_flag = false;
        //    }
        //}

        //private void validation_routine()
        //{
        //    string error_msg = "";
        //    //if (string.IsNullOrWhiteSpace(glay.vwstring0))
        //    //    error_msg = "Please enter Id";

        //    //if (string.IsNullOrWhiteSpace(glay.vwstring1))
        //    //    error_msg = "Please enter Name;

        //   if(action_flag == "Create")
        //    {
        //        QP_001_QUPD bnk = db.QP_001_QUPD.Find(glay.vwint0);
        //        if (bnk != null)
        //            error_msg = "Can not accept duplicates";
        //    }

        //    if (error_msg != "")
        //    {
        //        ModelState.AddModelError(String.Empty, error_msg);
        //        err_flag = false;
        //    }
        //}

        private void select_query()
        {

        }

        private void initial_rtn()
        {

            glay.vwstrarray0 = new string[50];
            glay.vwstrarray0[4] = "P";
            glay.vwstrarray0[6] = "N";
            glay.vwstrarray0[12] = "Y";
        }


	}
}
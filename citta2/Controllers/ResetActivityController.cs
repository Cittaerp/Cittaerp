using CittaErp.Models;
using CittaErp.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CittaErp.Controllers
{
    public class ResetActivityController : Controller
    {
        RA_002_RSA RA_002_RSA = new RA_002_RSA();
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        pubsess pubsess = new pubsess();
        cittautil util = new cittautil();
        // GET: ResetActivity
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.action_flag = "Create";
            pubsess = (pubsess)Session["pubsess"];
            initial_rtn();
            select_query();
            return View(glay);
        }

        private void select_query()
        {
            var fxd = from bg in db.FA_001_ASSET
                      where bg.active_status == "N"
                      select bg;
            ViewBag.fxd = new SelectList(fxd.ToList(), "fixed_asset_code", "description");
             }

        private void initial_rtn()
        {
        }

        [HttpPost]
        public ActionResult Create(vw_genlay glay_in)
        {
            pubsess = (pubsess)Session["pubsess"];
            glay = glay_in;

            update_file();
            select_query();
            return View(glay);
        }

        private void update_file()
        {
        }
    }

}
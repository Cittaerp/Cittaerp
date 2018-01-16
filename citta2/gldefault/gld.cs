using RealApp.Models;
using RealApp.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace citta2.gldefault
{
    public class gld
    {
        MainContext db = new MainContext();
        vw_genlay glay = new vw_genlay();
        cittautil util = new cittautil();
        public void cal_gl()
        {
           // HttpContext.Current.Session["retrngl"] = "";
           // ModelState.Clear();
            string gl_id = HttpContext.Current.Session["gl_id"].ToString();
            var hdet = (from bg in db.GL_001_GLDS
                       join bg1 in db.GL_001_ATYPE
                       on new { a1 = bg.acct_type1 } equals new { a1 = bg1.acct_type_code }
                       into bf1
                       from bf2 in bf1.DefaultIfEmpty()
                       join bg2 in db.GL_001_ATYPE
                       on new { a1 = bg.acct_type2 } equals new { a1 = bg2.acct_type_code }
                       into bf3
                        from bf4 in bf1.DefaultIfEmpty()
                       join bg3 in db.GL_001_ATYPE
                       on new { a1 = bg.acct_type3 } equals new { a1 = bg3.acct_type_code }
                       into bf5
                        from bf6 in bf1.DefaultIfEmpty()
                       join bg4 in db.GL_001_ATYPE
                       on new { a1 = bg.acct_type4 } equals new { a1 = bg4.acct_type_code }
                       into bf7
                        from bf8 in bf1.DefaultIfEmpty()
                       join bg5 in db.GL_001_ATYPE
                       on new { a1 = bg.acct_type5 } equals new { a1 = bg5.acct_type_code }
                       into bf9
                        from bf10 in bf1.DefaultIfEmpty()
                       where bg.gl_default_id == gl_id
                       select new { bg, bf2, bf4, bf6, bf8, bf10 }).Distinct().FirstOrDefault();
            if (hdet != null)
            {
                string pcl1 = hdet.bf2.acct_type_desc;
                string pcl2 = hdet.bf4.acct_type_desc;
                string pcl3 = hdet.bf6.acct_type_desc;
                string pcl4 = hdet.bf8.acct_type_desc;
                string pcl5 = hdet.bf10.acct_type_desc;

                List<SelectListItem> ary = new List<SelectListItem>();
                ary.Add(new SelectListItem { Value = "1", Text = pcl1 });
                ary.Add(new SelectListItem { Value = "2", Text = pcl2 });
                ary.Add(new SelectListItem { Value = "3", Text = pcl3 });
                ary.Add(new SelectListItem { Value = "4", Text = pcl4 });
                ary.Add(new SelectListItem { Value = "5", Text = pcl5 });

                HttpContext.Current.Session["retrngl"] = new SelectList(ary.ToArray(), "Value", "Text");
            }
        }
    }
}
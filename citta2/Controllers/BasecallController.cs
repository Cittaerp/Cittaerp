using anchor1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using anchor1v.Models;

namespace anchor1.Controllers
{
    public class BasecallController : Controller
    {
        // GET: Basecall
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult showcoy(string idx)
        {
            //bool pic_flag = false;
            //string coyname = idx + ".png";
            //string filelocation = Path.Combine(Server.MapPath("~/uploads/"), coyname);
            loginContext logdb = new loginContext();
            tab_photo_coy tab_photo_coy = logdb.tab_photo_coy.Find(idx);

            if (tab_photo_coy == null)
            {
                if (idx == "COMPANY3")
                    return File(Server.MapPath("~/images/company_information.gif"), "gif");
                else
                    return File(Server.MapPath("~/images/nologo.png"), "png");
            }

            return File(tab_photo_coy.picture1, "image/png");
        }


        public ActionResult show(string idx, string id2 = "PHOTO", string id3 = "P")
        {
            anchor1Context db = new anchor1Context();
            tab_photo tab_photo = db.tab_photo.Find(idx, id3, id2);

            if (tab_photo == null)
                return File(Server.MapPath("~/images/nopict.jpg"), "jpg");

            return File(tab_photo.picture1, tab_photo.document_type);

        }

        public ActionResult showpara(int seqno)
        {
            anchor1Context db = new anchor1Context();
            tab_docpara tab_docph = db.tab_docpara.Find(seqno);

            if (tab_docph == null)
                return File(Server.MapPath("~/images/upload.png"), "png");

            return File(tab_docph.picture1, tab_docph.document_type);

        }

        public ActionResult showdoc(string det_str)
        {
            anchor1Context db = new anchor1Context();

            int ws_count = det_str.IndexOf(":");
            string type_code = det_str.Substring(0, ws_count);
            int ws_count1 = det_str.IndexOf(":", ws_count + 1);
            string snumber = det_str.Substring(ws_count + 1, ws_count1 - ws_count - 1);
            ws_count = det_str.IndexOf(":", ws_count1 + 1);
            string strans_type = det_str.Substring(ws_count1 + 1, ws_count - ws_count1 - 1);
            ws_count1 = det_str.IndexOf(":", ws_count + 1);
            string sdate = det_str.Substring(ws_count + 1, ws_count1 - ws_count - 1);
            ws_count = det_str.IndexOf(":", ws_count1 + 1);
            string sindicator = det_str.Substring(ws_count1 + 1, ws_count - ws_count1 - 1);
            string sgroup = det_str.Substring(ws_count + 1);


            int seqno = 0;
            int.TryParse(snumber, out seqno);

            if (type_code == "VC")
            {
                //tab_applicant_doctrans tab_docph = db.tab_applicant_doctrans.Find(snumber, seqno);

                //if (tab_docph == null)
                //    return File(Server.MapPath("~/images/nopict.jpg"), "jpg");

                return File("", "pdf");
            }
            else if (type_code == "PP")
            {
                return showpara(seqno);
            }
            else if (type_code == "IM")
            {
                return show(snumber, strans_type, "I");
            }


            else
            {
                tab_doctrans tab_docph = db.tab_doctrans.Find(seqno);

                if (tab_docph == null)
                    return File(Server.MapPath("~/images/nopict.jpg"), "jpg");

                return File(tab_docph.picture1, tab_docph.document_type);
            }

        }

    }
}
using pg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace pg.Controllers
{
    public class HomeController : Controller
    {
        PLACEMENTGURUEntities p = new PLACEMENTGURUEntities();
        [HttpGet]
        public ActionResult uregister()
        {
            return View();
        }


        [HttpPost]
        public ActionResult uregister(TBL_USER svw, HttpPostedFileBase imgfile)
        {
            TBL_USER s = new TBL_USER();
            try 
            {
                s.U_NAME = svw.U_NAME;
                s.U_PASSWORD = svw.U_PASSWORD;
                s.U_IMAGE = uploadimage(imgfile);
                p.TBL_USER.Add(s);
                p.SaveChanges();
                return RedirectToAction("ulogin");
            }
            catch(Exception)
            {
                ViewBag.msg = "Data can't be uploaded";
            }
            return View();

             
        }

        public String uploadimage(HttpPostedFileBase imgfile)
        {
            string path = "-1";
            try
            {
                if (imgfile != null && imgfile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(imgfile.FileName);
                    if (extension.ToLower().Equals("jpg")|| extension.ToLower().Equals("png")|| extension.ToLower().Equals("jpeg")) 
                    {
                        Random r = new Random();
                       path= Path.Combine(Server.MapPath("~/Content/img"),r+Path.GetFileName(imgfile.FileName));
                        imgfile.SaveAs(path);
                        path = "~/Content/img" + r + Path.GetFileName(imgfile.FileName);
                    }

                }
            }
            catch (Exception) 
            {
            
            }
            return path;
         }
        public ActionResult logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            if (Session["ad_id"]!=null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public ActionResult iquestion()
        {
            return View();
        }

        public ActionResult tquestion()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["ad_id"] == null) {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Addcategory()
        {
            //Session["ad_id"] = 2;//we wil remove it later 
            if (Session["ad_id"] == null)
            {
                return RedirectToAction("Index");
            }
            var cid = Convert.ToInt32(Session["ad_id"].ToString());
            List<CATEGORY> li = p.CATEGORies.Where(x=>x.cat_fk_adid == cid).OrderByDescending(x => x.cat_id).ToList();
            ViewData["list"] = li;
            return View();
        }
        [HttpPost]
        public ActionResult Addcategory(CATEGORY cat)
        {
            List<CATEGORY> li = p.CATEGORies.OrderByDescending(x => x.cat_id).ToList();
            ViewData["list"] = li;
            Random r = new Random();
            CATEGORY c = new CATEGORY();
            c.cat_name = cat.cat_name;
            c.cat_encrypt = crypto.Encrypt(cat.cat_name.Trim() + r.Next().ToString(), true);
            c.cat_fk_adid = Convert.ToInt32(Session["ad_id"].ToString());
            p.CATEGORies.Add(c);
            p.SaveChanges();
            return RedirectToAction("Addcategory");
        }

        [HttpGet]
        public ActionResult Addquestion()
        {
            int sid = Convert.ToInt32(Session["ad_id"]);
            List<CATEGORY> li = p.CATEGORies.Where(x => x.cat_fk_adid == sid).ToList();
            ViewBag.list = new SelectList(li, "cat_id", "cat_name");
            return View();
        }

        [HttpPost]
        public ActionResult Addquestion(QUESTION q)
        {
                        
                int sid = Convert.ToInt32(Session["ad_id"]);
                List<CATEGORY> li = p.CATEGORies.Where(x => x.cat_fk_adid == sid).ToList();
                ViewBag.list = new SelectList(li, "cat_id", "cat_name");
                QUESTION qa = new QUESTION();
                qa.Q_TEXT = q.Q_TEXT;
                qa.OPA = q.OPA;
                qa.OPB = q.OPB;
                qa.OPC = q.OPC;
                qa.OPD = q.OPD;
                qa.COP = q.COP;
                qa.q_fk_catid = q.q_fk_catid;
                p.QUESTIONS.Add(qa);
                p.SaveChanges();
                TempData["msg"] = "Question Added Successfully";
                TempData.Keep();
                return RedirectToAction("Addquestion");
            
            }

        [HttpGet]
        public ActionResult alogin()
        {
         
            return View();
        }
        [HttpPost]
        public ActionResult alogin(TBL_ADMIN a)
        {
            TBL_ADMIN ad = p.TBL_ADMIN.Where(x => x.AD_NAME == a.AD_NAME && x.AD_PASSWORD == a.AD_PASSWORD).SingleOrDefault();
            if (ad != null)
            {
                Session["ad_id"] = ad.AD_ID;
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.msg = "Invalid username or password";
            }
            return View();
        }

        public ActionResult ulogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ulogin(TBL_USER u)
        {
            TBL_USER us = p.TBL_USER.Where(x => x.U_NAME == u.U_NAME && x.U_PASSWORD == u.U_PASSWORD).SingleOrDefault();
            if (us == null)
            {
                ViewBag.msg = "Invalid username or password";
            }

            else {
                Session["us_id"] = us.U_ID;
                return RedirectToAction("uexam");
            }
            return View();
        }
        public ActionResult uexam()
        {
            if (Session["us_id"] == null)
            {
                return RedirectToAction("ulogin");
            }

            return View();
        }
        [HttpPost]
        public ActionResult uexam(string room)
        {
            List<CATEGORY> list = p.CATEGORies.ToList();
            TempData["score"] = 5;
            foreach (var item in list)
            {
                if (item.cat_encrypt == room)
                {

                    List<QUESTION> li = p.QUESTIONS.Where(x => x.q_fk_catid == item.cat_id).ToList();
                    Queue<QUESTION> queue = new Queue<QUESTION>();
                    foreach (QUESTION a in li)
                    {
                        queue.Enqueue(a);


                    }
                    TempData["examid"] = item.cat_id;
                    TempData["questions"] = queue;
                    
                    TempData.Keep();
                    return RedirectToAction("examstart");

                }
                else
                {
                    ViewBag.error = "No Room Found......";
                }
            }
                return View();
            }

            
            public ActionResult examstart()
        {
            if (Session["us_id"] == null)
            {
                return RedirectToAction("ulogin");
            }
            if (TempData["questions"] == null)
            {
                return RedirectToAction("uexam");
            }
            TempData.Keep();
            QUESTION Q = null;
            if (TempData["questions"] != null)
            {
                Queue<QUESTION> qlist = (Queue<QUESTION>)TempData["questions"];
                if (qlist.Count > 0)
                {
                    Q = qlist.Peek();
                    qlist.Dequeue();
                    TempData["questions"] = qlist;
                    return View(Q);
                }
                else {
                    return RedirectToAction("endexam");   
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult examstart(QUESTION q)
        {
            string correctans = null;
            if (q.OPA != null)
            {
                correctans = "A";
            }
            else if (q.OPB != null)
            {
                correctans += "B";
            }
            else if (q.OPC != null)
            {
                correctans += "C";
            }
            else if (q.OPD != null) 
            {
                correctans += "D";
            }
            
            if (correctans.Equals(q.COP)) 
            {
               
                TempData["score"] = Convert.ToInt32(TempData["score"]) + 1;
            }
            TempData.Keep();
            return RedirectToAction("examstart");
        }
        public ActionResult endexam()
        {
            if (Session["us_id"] == null)
            {
                return RedirectToAction("Index");
            }
            EXAM ex = new EXAM();
            
            try
            {
                ex.EXAM_ID = Convert.ToInt32(TempData["examid"]);
                ex.EXAM_FK_USER = Convert.ToInt32(Session["us_id"]);
                ex.EXAM_DATE = System.DateTime.Now;
                int total = Convert.ToInt32(TempData["examid"]);
                ex.EXAM_USER_SCORE = (int)Convert.ToInt32(TempData["score"].ToString());
                p.EXAMs.Add(ex);
                p.SaveChanges();
            }
            catch (Exception)
            {

            }
            Session.RemoveAll();
            TempData["score"] = ex.EXAM_USER_SCORE;

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
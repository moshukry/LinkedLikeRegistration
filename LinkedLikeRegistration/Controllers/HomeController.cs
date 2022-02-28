using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinkedLikeRegistration.Models;
using PagedList;
namespace LinkedLikeRegistration.Controllers
{
    public class HomeController : Controller
    {
        LinkedUserContext db = new LinkedUserContext();
        public ActionResult Index(int? pageno)
        {
            if (Session["user_id"] == null) return RedirectToAction("login", "Registration");
            ViewBag.pageno = pageno;
            if(pageno != null)
            {
                return RedirectToAction("news",(int) pageno);
            }
            return View(db.Users.Find(int.Parse(Session["user_id"].ToString())));
        }
        public ActionResult catlinks()
        {
            ViewBag.cats = db.Catalogs.ToList();
            return PartialView();
        }

        public ActionResult news(int? cat_id, string search, string sort,int? pageno)
        {
            List<News> Usernews = new List<News>();
            int userId = int.Parse(Session["user_id"].ToString());
            Usernews = db.News.Where(n => n.user_id == userId).ToList();
            //ViewBag.search = search;
            if (pageno != null)
            {
                if (cat_id != null)
                {
                    ViewBag.news = Usernews.Where(n => n.cat_id == cat_id).OrderBy(n => n.datetime).ToPagedList((int)pageno, 3);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    ViewBag.news = Usernews.Where(n => n.title.Contains(search)).OrderBy(n => n.datetime).ToPagedList((int)pageno, 3);
                }
                else if (!string.IsNullOrEmpty(sort))
                {
                    if (sort == "title")
                        ViewBag.news = Usernews.OrderBy(n => n.title).ToPagedList((int)pageno, 3);
                    else if (sort == "bref")
                        ViewBag.news = Usernews.OrderBy(n => n.bref).ToPagedList((int)pageno, 3);
                }
                else
                {
                    ViewBag.news = Usernews.OrderBy(n => n.datetime).ToPagedList((int)pageno, 3);
                }
            }
            else
            {
                if (cat_id != null)
                {
                    ViewBag.news = Usernews.Where(n => n.cat_id == cat_id).OrderBy(n => n.datetime).ToPagedList(1, 3);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    ViewBag.news = Usernews.Where(n => n.title.Contains(search)).OrderBy(n => n.datetime).ToPagedList(1, 3);
                }
                else if (!string.IsNullOrEmpty(sort))
                {
                    if (sort == "title")
                        ViewBag.news = Usernews.OrderBy(n => n.title).ToPagedList(1, 3);
                    else if (sort == "bref")
                        ViewBag.news = Usernews.OrderBy(n => n.bref).ToPagedList(1, 3);
                }
                else
                {
                    ViewBag.news = Usernews.OrderBy(n => n.datetime).ToPagedList(1, 3);
                }
            }
            return PartialView();
        }
        public ActionResult allnews(int? pageno, int? cat_id)
        {
            if (Session["user_id"] == null) return RedirectToAction("login", "Registration");
            if (cat_id != null)
            {
                if(pageno != null)
                {
                    ViewBag.news = db.News.Where(n => n.cat_id == cat_id).OrderBy(n => n.datetime).ToPagedList((int)pageno, 3);
                }
                else
                {
                    ViewBag.news = db.News.Where(n => n.cat_id == cat_id).OrderBy(n => n.datetime).ToPagedList(1, 3);
                }
            }
            else
            {
                if (pageno != null)
                {
                    ViewBag.news = db.News.OrderBy(n => n.datetime).ToPagedList((int)pageno, 3);
                }
                else
                {
                    ViewBag.news = db.News.OrderBy(n => n.datetime).ToPagedList(1, 3);
                }
            }
            
            ViewBag.cats = db.Catalogs.ToList();
            return View();
        }


        public ActionResult addnews()
        {
            int id = int.Parse(Session["user_id"].ToString());
            ViewBag.user_id = id;
            SelectList cats = new SelectList(db.Catalogs.ToList(),"cat_id","cat_name");
            ViewBag.cats = cats;
            return View();
        }
        [HttpPost]
        public ActionResult addnews(News n,HttpPostedFileBase image)
        {
            image.SaveAs(Server.MapPath($"~/Attach/{image.FileName}"));
            n.image = image.FileName;
            n.user_id = int.Parse(Session["user_id"].ToString());
            db.News.Add(n);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult edit(int id)
        {
            SelectList cats = new SelectList(db.Catalogs.ToList(), "cat_id", "cat_name");
            ViewBag.cats = cats;
            News ns = db.News.Where(n=>n.news_id == id).FirstOrDefault();
            return View(ns);
        }
        [HttpPost]
        public ActionResult edit(News s, HttpPostedFileBase image)
        {
            News us = db.News.Find(s.news_id);
            if (image != null)
            {
                image.SaveAs(Server.MapPath($"~/attach/{image.FileName}"));
                us.image = image.FileName;

            }
            us.title = s.title;
            us.bref = s.bref;
            us.describtion = s.describtion;
            us.cat_id = s.cat_id;
            us.datetime = s.datetime;

            db.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult delete(int id)
        {
            News ns = db.News.Where(n => n.news_id == id).FirstOrDefault();
            db.News.Remove(ns);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        
        public ActionResult details(int id)
        {
            News ns = db.News.Where(n=> n.news_id==id).FirstOrDefault();
            return PartialView(ns);
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
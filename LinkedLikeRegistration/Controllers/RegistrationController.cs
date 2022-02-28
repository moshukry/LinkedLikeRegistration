using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LinkedLikeRegistration.Models;
using LinkedLikeRegistration.ViewModels;


namespace LinkedLikeRegistration.Controllers
{
    public class RegistrationController : Controller
    {
        LinkedUserContext db = new LinkedUserContext();
        public ActionResult profile()
        {
            int id = int.Parse(Session["user_id"].ToString());
            User u = db.Users.Find(id);
            if (u.photo == null)
                u.photo = "dummy.jpg";
            return View(u);
        }

        public ActionResult Register()
        {
            User_Skills viewmodel = new User_Skills() { user =null, skills = db.Skills.ToList() };
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult Register(User_Skills us,HttpPostedFileBase photo,HttpPostedFileBase resume)
        {
            if (ModelState.IsValid) 
            {
                if (photo != null)
                {
                    photo.SaveAs(Server.MapPath($"~/Attach/{photo.FileName}"));
                    us.user.photo = photo.FileName;
                }
                if (resume != null)
                {
                    resume.SaveAs(Server.MapPath($"~/Attach/{resume.FileName}"));
                    us.user.resume = resume.FileName;
                }
                List<Skill> SelectedSkills = new List<Skill>();
                foreach (var item in us.skills)
                {
                    if(item.isChecked)
                        SelectedSkills.Add(item);
                }
                User user2 = new User();
                user2 = us.user;
                user2.Skills = SelectedSkills;
                db.Users.Add(user2);
                foreach (var item in SelectedSkills)
                {
                    db.Skills.Attach(item);
                }
                db.SaveChanges();
                return View("profile");
            }
            else
            {
                ViewBag.sk = db.Skills.ToList();
                return View();
            }
        }

        public ActionResult checkemail(string email)
        {
            User u = db.Users.Where(n => n.email == email).FirstOrDefault();
            if(u== null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult login()
        {
            if (Request.Cookies["users"] != null)
            {
                Session.Add("user_id", Request.Cookies["users"].Values["user_id"]);
                return RedirectToAction("profile");
            }
            return View();
        }
        [HttpPost]
        public ActionResult login(User u, bool doremember)
        {
            User s = db.Users.Where(n => n.email == u.email && n.password == u.password).FirstOrDefault();
            if (s != null) // matched
            {
                if (doremember)
                {
                    // create a cookie
                    HttpCookie cookie = new HttpCookie("users");
                    cookie.Values.Add("user_id", s.user_id.ToString());
                    cookie.Values.Add("name", s.username.ToString());
                    cookie.Expires = DateTime.Now.AddMonths(3);
                    Response.Cookies.Add(cookie);
                }
                // create a session
                Session.Add("user_id", s.user_id);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.invalid = "incorrect username or password!";
                return View();
            }
        }   
        public ActionResult logout()
        {
            Session.Remove("user_id");
            // or? Session["userid"] = null;
            HttpCookie cookie = new HttpCookie("users");
            cookie.Expires = DateTime.Now.AddMonths(-1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("login");
        }

        public ActionResult editprofile()
        {
            ViewBag.sk = db.Skills.ToList();
            int id = int.Parse(Session["user_id"].ToString());
            User u = db.Users.Find(id);

            return View(u);
        }
        [HttpPost]
        public ActionResult editprofile(User u, HttpPostedFileBase photo, HttpPostedFileBase resume)
        {
            User us = db.Users.Find(u.user_id);
            if (photo != null)
            {
                photo.SaveAs(Server.MapPath($"~/attach/{photo.FileName}"));
                us.photo = photo.FileName;
            }
            if (resume != null)
            {
                resume.SaveAs(Server.MapPath($"~/attach/{resume.FileName}"));
                us.photo = resume.FileName;
            }
            us.username = u.username;
            us.email = u.email;
            us.age = u.age;
            us.address = u.address;

            db.SaveChanges();
            return RedirectToAction("profile");
        }
    
        public ActionResult changepassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult changepassword(string oldpass, string newpass, string confirmpass)
        {
            User u = db.Users.Find(int.Parse(Session["user_id"].ToString()));
                if (u.password == oldpass)
                {
                    if (newpass != confirmpass)
                    {
                        ViewBag.notmatch = "pass didn't match";
                        return View();
                    }
                    else
                    {
                        u.password = newpass;
                        u.confirm_password = newpass;
                        db.SaveChanges();
                        return RedirectToAction("profile");
                    }
                }
            ViewBag.wrongpass = "Wrong password";
            return View();
        }
    }
}
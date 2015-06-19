using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class HomeController : Controller
    {
        private PersonManager _personManager = new PersonManager();
        private LoginManager _logManager = new LoginManager();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string name = form["Name"];
            string password = form["Password"];
            bool okauth = true;

            if (!_personManager.NameExists(name))
            {
                okauth = false;
                ViewBag.NameError = "Name doesn't exists.";
            }
            else if (_personManager.GetByName(name).Password != password)
            {
                ViewBag.PassError = "Wrong password.";
                okauth = false;
            }
            if (okauth)
            {
                string cookie = Guid.NewGuid().ToString();
                CookieManager.SetCookie("UserCookie", cookie, new TimeSpan(2, 30, 30));
                _logManager.AddLogin(name, cookie);
                return RedirectToAction("Index", "Projects");

            }

            return View();
        }

        public ActionResult Career()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            _logManager.DeleteLogin(CookieManager.GetCookie("UserCookie"));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Career(FormCollection form)
        {
            string name = form["Name"];
            if (_personManager.NameExists(name))
            {
                ViewBag.NotValidName = "Name is taken, try another one.";
                return View();
            }
            else if (name == "")
            {
                ViewBag.NotValidName = "Name can not be empty.";
                return View();
            }
            string email = form["Email"];
            if (!email.Contains("@"))
            {
                ViewBag.NotValidEmail = "Doesn't look like email format.";
                return View();
            }
            string password = form["Password"];
            if (password == "")
            {
                ViewBag.NotMatch = "Passwords can not be empty.";
                return View();
            }
            string confirm = form["ConPassword"];
            if (password != confirm)
            {
                ViewBag.NotMatch = "Passwords don't match.";
                return View();

            }
            Employee newEmp = new Employee();
            newEmp.Name = name;
            newEmp.Password = password;
            newEmp.Email = email;

            _personManager.Add(newEmp);
            string cookie = Guid.NewGuid().ToString();
            CookieManager.SetCookie("UserCookie", cookie, new TimeSpan(2, 30, 30));
            _logManager.AddLogin(name, cookie);
            return RedirectToAction("Index", "Projects");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            string name = form["Name"];
            if (_personManager.NameExists(name))
            {
                ViewBag.NotValidName = "Name is taken, try another one.";
                return View();
            }
            else if (name == "")
            {
                ViewBag.NotValidName = "Name can not be empty.";
                return View();
            }
            string email = form["Email"];
            if (!email.Contains("@"))
            {
                ViewBag.NotValidEmail = "Doesn't look like email format.";
                return View();
            }
            string password = form["Password"];
            if (password == "")
            {
                ViewBag.NotMatch = "Passwords can not be empty.";
                return View();
            }
            string confirm = form["ConPassword"];
            if (password != confirm)
            {
                ViewBag.NotMatch = "Passwords don't match.";
                return View();

            }
            Client newClient = new Client();
            newClient.Name = name;
            newClient.Password = password;
            newClient.Email = email;

            _personManager.Add(newClient);

            string cookie = Guid.NewGuid().ToString();
            CookieManager.SetCookie("UserCookie", cookie, new TimeSpan(2, 30, 30));
            _logManager.AddLogin(name, cookie);

            return RedirectToAction("Index", "Projects");
        }

    }
}
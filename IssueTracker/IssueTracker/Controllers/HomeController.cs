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
        public ActionResult Index()
        {
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
            PersonManager manager = new PersonManager();
            Employee pepa = new Employee();
            pepa.Name = "pepaN";
            pepa.Password = "<juchuuJsemChytrej>";
            pepa.Email = "jen@jauuuu";


            ViewBag.UpdateExisting = manager.GetByName("pepaN").Password;
            Person pes = manager.GetByName("pes");
            if (pes != null)
            {
                ViewBag.UpdateNotExisting = pes.Email;
            }
            else
            {
                ViewBag.UpdateNotExisting = "Osoba nenalezena";
            }

            ViewBag.SearchEmplo = manager.GetAllEmplo();
            ViewBag.SearchClient = manager.GetAllCusto();
            ViewBag.SearchSub = manager.GetAllWithSubscribtion();
            ViewBag.SearchAll = manager.GetAll();
            return View();
        }
    }
}